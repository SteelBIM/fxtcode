using System.Web.Mvc;

namespace CBSS.Web.Admin.Areas.Cfgmanager
{
    public class CfgmanagerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Cfgmanager";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Cfgmanager_default",
                "Cfgmanager/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }

      
    }
}
