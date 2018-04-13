using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Routing;
using System.ServiceModel.Activation;
using CourseActivate.Activate.BLL;
using System.Web.Http;
using System.Threading.Tasks;

namespace CourseActivate.Web.API
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            RouteTable.Routes.Add(new ServiceRoute("dc", new WebServiceHostFactory(), typeof(CourseActivateService)));

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            #region 初始化redis数据库 
            //CourseActivateRedis redis = new CourseActivateRedis();
            //Task.Run(() => redis.InitRedisData());
            //redis.InitRedisData();
            #endregion
        }
    }
}