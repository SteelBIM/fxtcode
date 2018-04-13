using System.Web.Mvc;

namespace CBSS.Web.Admin.Areas.Account
{
    public class AccountAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Account";
            }
        }
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Account_default",
                "Account/{controller}/{action}/{id}",
                new { action = "Index",  id = UrlParameter.Optional }
            );
        }
    }

    //public class CustomRazorViewEngine : RazorViewEngine
    //{
    //    public CustomRazorViewEngine()
    //    {
    //        ViewLocationFormats = new string[] {

    //            "~/Areas/Account/Views/{1}/{0}.cshtml",
    //             "~/Areas/Account/Views/Shared/{0}.cshtml",
    //              "~/Areas/Account/Views/Shared_PartialView/{0}.cshtml"//指定查找某个文件的路径
    //        };
    //        PartialViewLocationFormats = new string[] {
    //            "~/Areas/Tbx/Views/{1}/{0}.cshtml",
    //             "~/Areas/Tbx/View",
    //              "~/Areas/Tbx/Views/Shared_PartialView/{0}.cshtml"////指定查找某个文件的路径
    //        };
    //    }
    //}


}
