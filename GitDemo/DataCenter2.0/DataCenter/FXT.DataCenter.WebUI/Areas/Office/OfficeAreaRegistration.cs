using System.Web.Mvc;

namespace FXT.DataCenter.WebUI.Areas.Office
{
    public class OfficeAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Office";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Office_default",
                "Office/{controller}/{action}/{id}",
                new {id = UrlParameter.Optional }
            );
        }
    }
}
