using System.Web.Mvc;

namespace FXT.DataCenter.WebUI.Areas.VQPrice
{
    public class VQPriceAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "VQPrice";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "VQPrice_default",
                "VQPrice/{controller}/{action}/{id}",
                new { id = UrlParameter.Optional }
            );
        }
    }
}
