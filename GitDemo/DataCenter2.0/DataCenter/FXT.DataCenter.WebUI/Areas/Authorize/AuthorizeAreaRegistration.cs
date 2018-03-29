using System.Web.Mvc;

namespace FXT.DataCenter.WebUI.Areas.Authorize
{
    public class AuthorizeAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Authorize";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Authorize_default",
                "Authorize/{controller}/{action}/{id}",
                new {id = UrlParameter.Optional }
            );
        }
    }
}
