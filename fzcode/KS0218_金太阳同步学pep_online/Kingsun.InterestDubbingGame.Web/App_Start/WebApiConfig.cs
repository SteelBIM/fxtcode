using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Kingsun.InterestDubbingGame.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //webapi【 “ObjectContent`1”类型未能序列化内容类型“application/xml; charset=utf-8”的响应正文。】错误解决办法
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //config.MessageHandlers.Add(new AuthenticationHandler());
        }
    }
}
