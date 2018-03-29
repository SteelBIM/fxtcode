using System.Web.Mvc;

namespace FXT.DataCenter.WebUI.Areas.Industry
{
    public class IndustryAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Industry";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Industry_default",
                "Industry/{controller}/{action}/{id}",
                new { id = UrlParameter.Optional }
            );
        }
    }
}
