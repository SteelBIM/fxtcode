using System.Web;
using System.Web.Mvc;

namespace CBSS.Web.API
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“FilterConfig”的 XML 注释
    public class FilterConfig
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“FilterConfig”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“FilterConfig.RegisterGlobalFilters(GlobalFilterCollection)”的 XML 注释
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“FilterConfig.RegisterGlobalFilters(GlobalFilterCollection)”的 XML 注释
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}