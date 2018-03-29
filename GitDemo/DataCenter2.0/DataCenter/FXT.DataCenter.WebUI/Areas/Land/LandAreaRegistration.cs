using System.Web.Mvc;

namespace FXT.DataCenter.WebUI.Areas.Land
{
    public class LandAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Land";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Land_default",
                "Land/{controller}/{action}/{id}",
                new { id = UrlParameter.Optional }
            );
        }
    }
}
