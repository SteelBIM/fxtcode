using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using FxtSpider.Bll.SpiderCommon.Models;
using FxtSpider.Bll.SpiderCommon;
using FxtSpider.DAL.LinqToSql;
using FxtSpider.Common;
using FxtSpider.Bll;
using System.Net;
using FxtSpider.Bll.ProxyIpCommom;
using System.IO;
using System.Threading;

namespace FxtSpider.RunSource.IP代理
{
    public class 站大爷代理IP网
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(有代理IP网));
        private RegexInfo regex_ipInfo = new RegexInfo("(<tr>(?:(?!</tr>).)*</tr>)", "$1");
        private RegexInfo regex_ip = new RegexInfo("<tr><td>([^<>]+)</td><td>([^<>]*)</td>(?:(?!</tr>).)*</tr>", "$1:$2");
        private Dictionary<string, RegexInfo> regexDic = new Dictionary<string, RegexInfo>();
        public void Run()
        {
            regexDic.Add("regex_ipInfo", regex_ipInfo);
            regexDic.Add("regex_ip", regex_ip);
            //从文本文件中获取爬取页面列表
            List<string> urlList = new List<string>();
            string configPath = AppDomain.CurrentDomain.BaseDirectory + "IP代理/Config/站大爷ProxyIpConfig.txt";
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
            foreach (string str in urlList)
            {
                Thread m_thread = new Thread(new ParameterizedThreadStart(RunPageList));
                m_thread.Start(str);
            }
        }
        public void RunPageList(object url)
        {
            string _url = Convert.ToString(url);
            string pageUrlPara = url + "&pageid={0}";
            try
            {
                SysData_ProxyIp existsObj = new SysData_ProxyIp();
                bool notStop = true;
                string message = "";
                int pageIndex = 1;
                while (notStop)
                {
                    string paegUrl = string.Format(pageUrlPara, pageIndex);
                    int reqCount = 1;
                reqBegin:
                    Dictionary<string, List<string>> dicValueList = SpiderHelp.GetHtmlByRegexNotProxyIp(paegUrl, "gb2312", regexDic);
                    List<string> ipInfoList = dicValueList["regex_ipInfo"];//所有ip集合
                    if (ipInfoList == null || ipInfoList.Count < 1)
                    {
                        if (reqCount < 3)
                        {
                            reqCount = reqCount + 1;
                            goto reqBegin;
                        }
                        else
                        {
                            pageIndex = 1;
                            log.Debug(string.Format("未获取到IP列表,url:{0}", url));
                            continue;
                        }
                    }
                    foreach (string ipInfo in ipInfoList)
                    {
                        Dictionary<string, List<string>> infoListDic = SpiderHelp.GetStrByRegex(ipInfo, regexDic);
                        string ip = infoListDic["regex_ip"].Count < 1 ? "" : infoListDic["regex_ip"][0];
                        ip = ip.TrimBlank();
                        int result = ProxyIpHelp.ImportProxyIp(ip, "匿名", out existsObj, out message);
                        if (result != 1)
                        {
                            log.Debug(string.Format("{0},url:{1},ip:{2}", message, url, ip == null ? "null" : ip));
                            continue;
                        }
                        else
                        {
                            log.Debug(string.Format("ip插入成功,url:{0},ip:{1}", url, ip == null ? "null" : ip));
                        }
                    }
                    pageIndex = pageIndex + 1;
                }
            }
            catch (Exception ex)
            {
                log.Error("系统异常", ex);
            }
        }
    }
}
