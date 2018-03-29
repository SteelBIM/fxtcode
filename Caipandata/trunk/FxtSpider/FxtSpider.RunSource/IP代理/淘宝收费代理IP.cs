using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.Bll.SpiderCommon;
using FxtSpider.Bll;
using FxtSpider.DAL.LinqToSql;
using log4net;
using FxtSpider.Bll.ProxyIpCommom;
using System.IO;

namespace FxtSpider.RunSource.IP代理
{
    public  class 淘宝收费代理IP
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(淘宝收费代理IP));
        public void Run()
        {
            List<string> orderList = new List<string>();
            string configPath = AppDomain.CurrentDomain.BaseDirectory + "IP代理/Config/TaobaoProxyIpConfig.txt";
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
                    orderList.Add(str);
                }
            }
            sr.Close();
            sr.Dispose();
            string url = "http://121.199.38.28/ip/?tid={0}&num=1&ports=80,808,3128&filter=on";
            string error = "ERROR|订单剩余数量不足";
            SysData_ProxyIp existsObj = new SysData_ProxyIp();
            string message = "";
            foreach (string orderStr in orderList)
            {
                string requestUrl = string.Format(url, orderStr);
                while (true)
                {
                    string ipHtml = "";
                begin:
                    try
                    {
                        ipHtml = SpiderHelp.GetHtml(requestUrl, "utf-8");
                    }
                    catch (Exception ex)
                    {
                        System.Threading.Thread.Sleep(3000);
                        goto begin;
                    }
                    if (ipHtml.Contains(error))
                    {
                        break;
                    }
                    int result = ProxyIpHelp.ImportProxyIp(ipHtml, "匿名", out existsObj, out message);
                    if (result != 1)
                    {
                        log.Debug(string.Format("{0},订单:{1},url:{2},ip:{3}", message, orderStr,url, ipHtml == null ? "null" : ipHtml));
                        continue;
                    }
                    else
                    {
                        log.Debug(string.Format("ip插入成功,订单:{0},url:{1},ip:{2}",orderStr, url, ipHtml == null ? "null" : ipHtml));
                    }
                }
                log.Debug(string.Format("订单:{0}导入完成,url:{1}", orderStr, url));
            }
            log.Debug("所有订单:导入完成,url:{1}");
        }
    }
}
