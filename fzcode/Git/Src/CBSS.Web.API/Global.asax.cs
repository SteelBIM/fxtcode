using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Routing;
using System.ServiceModel.Activation;
using System.Web.Http;
using System.Threading.Tasks;
using CourseActivate.Web.API.Filter;

namespace CBSS.Web.API
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            RouteTable.Routes.Add(new ServiceRoute("dc", new WebServiceHostFactory(), typeof(CBSSService)));//总线接口
            RouteTable.Routes.Add(new ServiceRoute("uc", new WebServiceHostFactory(), typeof(UserRecordService)));//分线接口，会进行接口分发

            WebApiConfig.Register(GlobalConfiguration.Configuration);

            // GlobalConfiguration.Configuration.MessageHandlers.Add(new GzipFilter());
        }
    }
}