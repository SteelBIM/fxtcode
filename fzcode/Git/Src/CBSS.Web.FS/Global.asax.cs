using CBSS.Core.Config;
using CBSS.Core.Log;
using CourseActivate.Web.FS.Controllers;
using FluentScheduler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace CourseActivate.Web.FS
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
            Log4NetHelper.Info(LoggerType.FsExceptionLog, "定时任务开始启动");
            TimedTask timedtask = new TimedTask();
            JobManager.Initialize(timedtask.Start());
        }

        protected void Application_End(object sender, EventArgs e)
        {
            try
            {
                Log4NetHelper.Info(LoggerType.FsExceptionLog, "进程即将被IIS回收");
                Log4NetHelper.Info(LoggerType.FsExceptionLog, "重新访问一个页面，以唤醒服务");
                string strUrl = CachedConfigContext.Current.SystemConfig.FS_SelfAddress;//本程序部署地址
                System.Net.HttpWebRequest httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(strUrl);
                System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)httpWebRequest.GetResponse();
                System.IO.Stream stream = httpWebResponse.GetResponseStream();//得到回写的字节流
                httpWebResponse.Close();
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(LoggerType.FsExceptionLog, "唤醒服务异常", ex);
            }
        }
    }
}