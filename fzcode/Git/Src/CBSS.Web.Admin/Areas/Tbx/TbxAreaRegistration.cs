using System.Web.Mvc;

namespace CBSS.Web.Admin.Areas.Tbx
{
    public class TbxAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Tbx";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Tbx_default",
                "Tbx/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
