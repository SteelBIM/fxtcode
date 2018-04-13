using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using FluentScheduler;
using KSWF.Web.Admin.Models;

namespace KSWF.Web.Admin
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
              //定时任务
            TimedTask timedtask = new TimedTask();
            JobManager.Initialize(timedtask.Start());
        }
        protected void Application_End(object sender, EventArgs e)
        {
            try
            {
                string strUrl = "http://mkt.kingsun.cn/Login/Index";// System.Configuration.ConfigurationManager.AppSettings["FS_SelfAddress"];//本程序部署地址
                System.Net.HttpWebRequest httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(strUrl);
                System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)httpWebRequest.GetResponse();
                System.IO.Stream stream = httpWebResponse.GetResponseStream();//得到回写的字节流
                httpWebResponse.Close();
            }
            catch (Exception ex)
            {
                TestLog4Net.LogHelper.Info("唤醒服务异常：" + ex.Message);
            }
        }
    }
}