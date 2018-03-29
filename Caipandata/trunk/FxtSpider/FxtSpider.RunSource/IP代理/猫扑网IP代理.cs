using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.Bll.SpiderCommon.Models;
using FxtSpider.Bll.SpiderCommon;
using FxtSpider.Common;
using System.Net;
using FxtSpider.Bll;
using FxtSpider.DAL.LinqToSql;
using log4net;
using FxtSpider.Bll.ProxyIpCommom;
using System.IO;

namespace FxtSpider.RunSource.IP代理
{
    public class 猫扑网IP代理
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(猫扑网IP代理));
        public void Run()
        {
            //ip列表页ip信息
            RegexInfo regex_ipInfo = new RegexInfo("(\\&nbsp\\;[^<>]+<br />)", "$1");
            //ip列表页ip
            RegexInfo regex_ip = new RegexInfo("\\&nbsp\\;([^<>\\s]+) ([^<>\\s]+) [^<>]+<br />", "$1:$2");
            //ip列表页ip区域
            RegexInfo regex_area = new RegexInfo("\\&nbsp\\;[^<>\\s]+ [^<>\\s]+ ([^<>\\s]+) [^<>]+<br />", "$1");
            Dictionary<string, RegexInfo> regexDic1 = new Dictionary<string, RegexInfo>();
            regexDic1.Add("regex_ipInfo", regex_ipInfo);
            Dictionary<string, RegexInfo> regexDic2 = new Dictionary<string, RegexInfo>();
            regexDic2.Add("regex_ip", regex_ip);
            regexDic2.Add("regex_area", regex_area);
            //从文本文件中获取爬取ip分类页面列表
            List<string> urlList = new List<string>();
            string configPath = AppDomain.CurrentDomain.BaseDirectory + "IP代理/Config/猫扑网IP代理ProxyIpConfig.txt";
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
            //但从当前页面中获取爬取ip分类列表
            RegexInfo regex_urllist = new RegexInfo("<DT><a href=\"([^\"]+)\" target=\"_blank\">[^<>]+</a></DT>", "$1");
            RegexInfo regex_urllist_pagecount = new RegexInfo("<a href=\"http://www.itmop.com/proxy/catalog.asp\\?page=(\\d+)\">\\&raquo\\;</a></DIV>", "$1");
            Dictionary<string, RegexInfo> regexDic3 = new Dictionary<string, RegexInfo>();
            regexDic3.Add("regex_urllist", regex_urllist);
            regexDic3.Add("regex_urllist_pagecount", regex_urllist_pagecount);
            int urllist_index = 1;
            int urllist_max_index = 1;
            string urllist_nextPage_para = "http://www.itmop.com/proxy/catalog.asp?page={0}";//下一页
            string urllist_nextPage = string.Format(urllist_nextPage_para, urllist_index);//下一页

            try
            {
            begin_nextpage:
                //获取ip列表页url
                Dictionary<string, List<string>> dicValueList3 = SpiderHelp.GetHtmlByRegexNotProxyIp(urllist_nextPage, "utf-8", regexDic3);
                List<string> urlList2 = dicValueList3["regex_urllist"];//所有ip列表集合
                urlList.AddRange(urlList2);
                //获取ip列表列表页的最大页数
                urllist_max_index = dicValueList3["regex_urllist_pagecount"].Count < 1 ? 0 : Convert.ToInt32(dicValueList3["regex_urllist_pagecount"][0].TrimBlank());
                if (urllist_index < urllist_max_index)
                {
                    urllist_index = urllist_index + 1;
                    urllist_nextPage = string.Format(urllist_nextPage_para, urllist_index);
                }
                else
                {
                    urllist_nextPage = "";
                }
                //开始爬取当前页列表ip页面
                SysData_ProxyIp existsObj = new SysData_ProxyIp();
                string message = "";
                for (int i = 0; i < urlList.Count(); i++)
                {
                    string urlInfo = urlList[i];
                    urlInfo = urlInfo.Replace("&amp;", "&");
                    string url = urlInfo.Split('$')[0];
                    string urlHost = urlInfo.Split('$').Length < 2 ? "http://www.itmop.com" : urlInfo.Split('$')[1];
                requestpage:
                    //根据ip列表页url爬取ip信息
                    if (!url.ToLower().Contains("http://"))
                    {
                        url = urlHost + url;
                    }
                    Dictionary<string, List<string>> dicValueList = SpiderHelp.GetHtmlByRegexNotProxyIp(url, "utf-8", regexDic1);
                    List<string> ipInfoList = dicValueList["regex_ipInfo"];//所有ip集合
                    string nextPage = "";
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
                        ipArea = ipArea.RemoveHeml();
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

                    }
                    if (!string.IsNullOrEmpty(nextPage))
                    {
                        url = urlHost + nextPage;
                        goto requestpage;
                    }
                }
                if (!string.IsNullOrEmpty(urllist_nextPage))
                {
                    urlList = new List<string>();
                    goto begin_nextpage;
                }
            }
            catch (Exception ex)
            {
                log.Error("系统异常", ex);
            }
        }
    }
}
