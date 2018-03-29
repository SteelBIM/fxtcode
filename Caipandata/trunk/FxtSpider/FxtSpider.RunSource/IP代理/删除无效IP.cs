using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using FxtSpider.Bll;

namespace FxtSpider.RunSource.IP代理
{
    public class 删除无效IP
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(删除无效IP));
        public void Run()
        {
            log.Debug("所有ip正在删除中...........");
        work:
            if (!WorkItemManager.CheckPassSpider())//****检查数据库是否有维护程序在执行******//
            {
                System.Threading.Thread.Sleep(60000);
                goto work;
            }
            ProxyIpManager.DeleteNotEffectiveProxyIp();
            log.Debug("所有ip删除完成...........");
        }
    }
}
