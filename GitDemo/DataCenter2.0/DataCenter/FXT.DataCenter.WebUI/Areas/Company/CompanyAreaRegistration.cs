using System.Web.Mvc;

namespace FXT.DataCenter.WebUI.Areas.Company
{
    public class CompanyAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Company";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Company_default",
                "Company/{controller}/{action}/{id}",
                new {id = UrlParameter.Optional }
            );
        }
    }
}
