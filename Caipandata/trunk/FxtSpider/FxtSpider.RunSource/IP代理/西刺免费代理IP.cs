using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using FxtSpider.Bll.SpiderCommon.Models;
using FxtSpider.DAL.LinqToSql;
using FxtSpider.Bll.SpiderCommon;
using FxtSpider.Common;
using FxtSpider.Bll;
using System.Net;
using FxtSpider.Bll.ProxyIpCommom;
using System.IO;

namespace FxtSpider.RunSource.IP代理
{
    public class 西刺免费代理IP
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(有代理IP网));
        public void Run()
        {
            RegexInfo regex_ipInfo = new RegexInfo("(<tr class=\"[^\"]*\">(?:(?!</tr>).)*</tr>)", "$1");
            RegexInfo regex_nextPage = new RegexInfo("<a class=\"next_page\" rel=\"next\" href=\"([^\"]+)\">下一页[^<>]*</a>", "$1");
            RegexInfo regex_ip = new RegexInfo("<tr class=\"[^\"]*\"><td>(?:(?!</td>).)*</td><td>([^<>]+)</td><td>([^<>]+)</td><td><a href=\"[^\"]*\">[^<>]+</a></td>"+
            "<td>[^<>]*</td><td>[^<>]*</td><td>(?:(?!</td>).)*</td><td>(?:(?!</td>).)*</td><td>[^<>]+</td>", "$1:$2");
            RegexInfo regex_area = new RegexInfo("<tr class=\"[^\"]*\"><td>(?:(?!</td>).)*</td><td>[^<>]+</td><td>[^<>]+</td><td><a href=\"[^\"]*\">([^<>]+)</a></td>" +
            "<td>[^<>]*</td><td>[^<>]*</td><td>(?:(?!</td>).)*</td><td>(?:(?!</td>).)*</td><td>[^<>]+</td>", "$1");
            RegexInfo regex_date = new RegexInfo("<tr class=\"[^\"]*\"><td>(?:(?!</td>).)*</td><td>[^<>]+</td><td>[^<>]+</td><td><a href=\"[^\"]*\">[^<>]+</a></td>" +
            "<td>[^<>]*</td><td>[^<>]*</td><td>(?:(?!</td>).)*</td><td>(?:(?!</td>).)*</td><td>([^<>]+)</td>", "$1");
            Dictionary<string, RegexInfo> regexDic1 = new Dictionary<string, RegexInfo>();
            regexDic1.Add("regex_ipInfo", regex_ipInfo);
            regexDic1.Add("regex_nextPage", regex_nextPage);
            Dictionary<string, RegexInfo> regexDic2 = new Dictionary<string, RegexInfo>();
            regexDic2.Add("regex_ip", regex_ip);
            regexDic2.Add("regex_area", regex_area);
            regexDic2.Add("regex_date", regex_date);
            //从文本文件中获取爬取配置
            List<string> urlList = new List<string>();
            string configPath = AppDomain.CurrentDomain.BaseDirectory + "IP代理/Config/西刺ProxyIpConfig.txt";
            StreamReader sr = new StreamReader(configPath);
            while (true)
            {
                string str = sr.ReadLine();
                if (str == null)
                {
                    break;
                }
                else
                {
                    urlList.Add(str);
                }
            }
            sr.Close();
            sr.Dispose();
            //urlList.Add("http://www.xici.net.co/nt/$http://www.xici.net.co/$2014-5-13");
            try
            {
                SysData_ProxyIp existsObj = new SysData_ProxyIp();
                string message = "";
                foreach (string urlInfo in urlList)
                {
                    string url = urlInfo.Split('$')[0];
                    string urlHost = urlInfo.Split('$')[1];
                    DateTime maxDate = Convert.ToDateTime(urlInfo.Split('$')[2]);
                requestpage:
                    Dictionary<string, List<string>> dicValueList = SpiderHelp.GetHtmlByRegexNotProxyIp(url, "utf-8", regexDic1);
                    List<string> ipInfoList = dicValueList["regex_ipInfo"];//所有ip集合
                    string nextPage = dicValueList["regex_nextPage"].Count < 1 ? "" : dicValueList["regex_nextPage"][0];//下一页链接
                    if (ipInfoList == null || ipInfoList.Count < 1)
                    {
                        log.Debug(string.Format("未获取到IP列表,url:{0}", url));
                        continue;
                    }
                    foreach (string ipInfo in ipInfoList)
                    {
                        Dictionary<string, List<string>> infoListDic = SpiderHelp.GetStrByRegex(ipInfo, regexDic2);
                        string ip = infoListDic["regex_ip"].Count < 1 ? "" : infoListDic["regex_ip"][0];
                        string ipArea = infoListDic["regex_area"].Count < 1 ? "" : infoListDic["regex_area"][0];
                        DateTime ipDate = infoListDic["regex_date"].Count < 1 ? DateTime.Now : Convert.ToDateTime(infoListDic["regex_date"][0]);
                       
                        ip = ip.TrimBlank();

                        int result = ProxyIpHelp.ImportProxyIp(ip, ipArea, out existsObj, out message);
                        if (result != 1)
                        {
                            log.Debug(string.Format("{0},url:{1},ip:{2}", message, url, ip == null ? "null" : ip));
                            continue;
                        }
                        else
                        {
                            log.Debug(string.Format("ip插入成功,url:{0},ip:{1}", url, ip == null ? "null" : ip));
                        }
                        //如果自定日期
                        if (ipDate < maxDate)
                        {
                            nextPage = null;
                            break;
                        }
                    }
                    if (!string.IsNullOrEmpty(nextPage))
                    {
                        url = urlHost + nextPage;
                        goto requestpage;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("系统异常", ex);
            }
        }
        /// <summary>
        /// 检查代理ip是否可用
        /// </summary>
        /// <param name="proxyIp"></param>
        /// <returns></returns>
        bool CheckProxyIp(string proxyIp)
        {
            if (string.IsNullOrEmpty(proxyIp))
            {
                return false;
            }
            proxyIp = proxyIp.ToLower().Replace("http://", "");
            if (string.IsNullOrEmpty(proxyIp))
            {
                return false;
            }
            proxyIp = "http://" + proxyIp;
            int 网络异常重试次数 = 0;
        begin:
            try
            {
                WebProxy wp = new WebProxy(new Uri(proxyIp));
                HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(new Uri("http://www.baidu.com", false));
                request.Proxy = wp;
                request.Method = "get";
                request.Timeout = 3000;
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
                System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                //网络异常重试次数++;
                //if (网络异常重试次数 < 3)
                //{
                //    System.Threading.Thread.Sleep(1600);
                //    goto begin;
                //}
                return false;
            }
            return true;

        }
    }
}
