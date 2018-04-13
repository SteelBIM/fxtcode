using System.Web.Http;
using System.Web.Mvc;

namespace CBSS.Web.API.Areas.HelpPage
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“HelpPageAreaRegistration”的 XML 注释
    public class HelpPageAreaRegistration : AreaRegistration
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“HelpPageAreaRegistration”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“HelpPageAreaRegistration.AreaName”的 XML 注释
        public override string AreaName
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“HelpPageAreaRegistration.AreaName”的 XML 注释
        {
            get
            {
                return "HelpPage";
            }
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“HelpPageAreaRegistration.RegisterArea(AreaRegistrationContext)”的 XML 注释
        public override void RegisterArea(AreaRegistrationContext context)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“HelpPageAreaRegistration.RegisterArea(AreaRegistrationContext)”的 XML 注释
        {
            context.MapRoute(
                "HelpPage_Default",
                "Help/{action}/{apiId}",
                new { controller = "Help", action = "Index", apiId = UrlParameter.Optional });

            HelpPageConfig.Register(GlobalConfiguration.Configuration);
        }
    }
}