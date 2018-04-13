using FluentScheduler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kingsun.SynchronousStudy.FS
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
            Log4Net.LogHelper.Info("定时任务");
            TimedTask timedtask = new TimedTask();
            JobManager.Initialize(timedtask.Start());
        }
        protected void Application_End(object sender, EventArgs e)
        {
            try
            {
                Log4Net.LogHelper.Info("进程即将被IIS回收");
                Log4Net.LogHelper.Info("重新访问一个页面，以唤醒服务");
                string strUrl = System.Configuration.ConfigurationManager.AppSettings["FS_SelfAddress"];//本程序部署地址
                Log4Net.LogHelper.Info("重新访问一个页面,地址：" + strUrl);
                System.Net.HttpWebRequest httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(strUrl);
                Log4Net.LogHelper.Info("重新访问一个页面,httpWebRequest");
                System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)httpWebRequest.GetResponse();
                Log4Net.LogHelper.Info("重新访问一个页面,httpWebResponse");
                System.IO.Stream stream = httpWebResponse.GetResponseStream();//得到回写的字节流
                Log4Net.LogHelper.Info("重新访问一个页面,得到回写的字节流stream");
                httpWebResponse.Close();
                Log4Net.LogHelper.Info("重新访问一个页面，唤醒服务成功,地址：" + strUrl);
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "唤醒服务失败");
            }
        }
    }
}