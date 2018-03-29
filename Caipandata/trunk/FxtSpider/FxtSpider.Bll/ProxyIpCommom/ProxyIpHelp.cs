using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using FxtSpider.DAL.LinqToSql;

namespace FxtSpider.Bll.ProxyIpCommom
{
    public static class ProxyIpHelp
    {
        /// <summary>
        /// 导入IP
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="ipArea"></param>
        /// <param name="existsObj"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static int ImportProxyIp(string ip, string ipArea, out SysData_ProxyIp existsObj, out string message)
        {
            existsObj = null;
            message = "";
            if (string.IsNullOrEmpty(ip))
            {
                message = "ip不能为空";
                return 0;
            }
        work:
            if (!WorkItemManager.CheckPassSpider())//****检查数据库是否有维护程序在执行******//
            {
                System.Threading.Thread.Sleep(60000);
                goto work;
            }
            existsObj = ProxyIpManager.GetProxyIpByIp(ip);
            if (existsObj != null)
            {
                message = "ip已存在";
                return 0;
            }
            if (!CheckProxyIp(ip))
            {
                message = "ip不可用";
                return 0;
            }
            int result = ProxyIpManager.InsertProxyIp(ip, ipArea, out existsObj, out message,checkExists:false);
            if (result != 1)
            {
                return 0;
            }
            else
            {
                message = "ip插入成功";
            }
            return 1;
        }
        /// <summary>
        /// 检查代理ip是否可用
        /// </summary>
        /// <param name="proxyIp"></param>
        /// <returns></returns>
        static bool CheckProxyIp(string proxyIp)
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
