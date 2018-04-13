using System.Web;
using System.Web.Mvc;

namespace CourseActivate.Web.Admin
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthenticationAttribute() { IsCheck = true });
            filters.Add(new ValidateNullAttribute());
        }
    }
}