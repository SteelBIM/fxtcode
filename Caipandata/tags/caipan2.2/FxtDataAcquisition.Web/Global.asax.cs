using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Combres;
using log4net;
using FxtDataAcquisition.Web.Common;
using FxtDataAcquisition.Domain.Models;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

namespace FxtDataAcquisition.Web
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        ILog logger = LogManager.GetLogger(typeof(MvcApplication));

        protected void Session_Start(object sender, EventArgs e)
        {
            //MVC中第一次创建Session后要，读一次才能将SessionId传到客户端。原因未知。
            string sessionID = HttpContext.Current.Session.SessionID;
        }
        protected void Application_Start()
        {
            RouteTable.Routes.AddCombresRoute("Combres");
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            //NInject 注册 
            ControllerBuilder.Current.SetControllerFactory(new
                         NinjectControllerFactory());
            //绑定Mapper
            MapperBinder.Binder();
            //绑定登陆用户信息
            var binder = new LoginUserInfoBinder();
            ModelBinders.Binders.Add(typeof(Domain.DTO.FxtUserCenterDTO.UserCenter_LoginUserInfo), binder);
            //绑定实体信息
            var modelBinder = new ModelBinder();
            var types = typeof(AllotFlow).Assembly.GetTypes();
            foreach (var type in types)
            {
                if (type.Namespace == "FxtDataAcquisition.Domain.Models")
                {
                    ModelBinders.Binders.Add(type, modelBinder);
                }
            }
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //logger.Error(e.ExceptionObject);
            //Environment.Exit(-1);
        }

        protected void Application_Error(object sender, EventArgs e)
        {

            //Exception prev_ex = null;
            //try
            //{
            //    prev_ex = Server.GetLastError();
            //    if (prev_ex != null)
            //        logger.Error(prev_ex);
            //    Server.ClearError();
            //}
            //catch (Exception ex)
            //{
            //    logger.Error(ex);
            //}

        }

    }
}