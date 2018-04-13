using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Kingsun.SynchronousStudy.App.Common;

namespace Kingsun.SynchronousStudy.App
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
           
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Server.MapPath("Log4Net.config")));  

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            GlobalConfiguration.Configuration.MessageHandlers.Add(new AuthenticationHandler());  
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (Context.Request.FilePath == "/") Context.RewritePath("index.html");
        }
    }
}