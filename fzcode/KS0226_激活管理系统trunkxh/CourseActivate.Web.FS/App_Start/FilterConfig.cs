using System.Web;
using System.Web.Mvc;

namespace CourseActivate.Web.FS
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}