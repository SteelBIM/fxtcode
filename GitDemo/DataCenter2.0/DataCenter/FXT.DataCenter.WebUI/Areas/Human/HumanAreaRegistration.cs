using System.Web.Mvc;

namespace FXT.DataCenter.WebUI.Areas.Human
{
    public class HumanAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Human";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Human_default",
                "Human/{controller}/{action}/{id}",
                new { id = UrlParameter.Optional }
            );
        }
    }
}
