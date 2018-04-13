using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CBSS.Web.API
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“RouteConfig”的 XML 注释
    public class RouteConfig
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“RouteConfig”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“RouteConfig.RegisterRoutes(RouteCollection)”的 XML 注释
        public static void RegisterRoutes(RouteCollection routes)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“RouteConfig.RegisterRoutes(RouteCollection)”的 XML 注释
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}