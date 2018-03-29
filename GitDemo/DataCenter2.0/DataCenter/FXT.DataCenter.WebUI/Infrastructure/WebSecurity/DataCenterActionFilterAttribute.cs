using System;
using System.Linq;
using System.Web.Mvc;
using FXT.DataCenter.Infrastructure.Data.ServicesImpl;

namespace FXT.DataCenter.WebUI.Infrastructure.WebSecurity
{
    /// <summary>
    /// Attribute for power Authorize
    /// </summary>
    public class DataCenterActionFilterAttribute : ActionFilterAttribute
    {
        private readonly int _menuCode;
        private readonly int _funcCode;

        public DataCenterActionFilterAttribute(int menuCode, int funcCode)
        {
            this._menuCode = menuCode;
            this._funcCode = funcCode;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!this.Authorize(filterContext, _menuCode, _funcCode))
                filterContext.Result = new ContentResult
                {
                    Content = "<script>alert('对不起，您当前没有操作权限！！');if(typeof(top.tb_remove)!='function'){history.go(-1);}else{top.tb_remove();} </script>"
                };
        }

        protected virtual bool Authorize(ActionExecutingContext filterContext, int menuCode, int funcCode)
        {
            if (filterContext.HttpContext == null)
                throw new ArgumentNullException("httpContext");

            if (!filterContext.HttpContext.User.Identity.IsAuthenticated) return false;
            var menu = new Menu();
            var roles = Passport.Current.Roles;
            var menuObj = menu.GetMenuByTypeCode(_menuCode).FirstOrDefault();

            return roles.Select(item => menuObj != null ? menu.GetFunctionsByParams(menuObj.ID, item.ID) : null).Any(funcs => funcs.Any(m => m.FunctionCode == funcCode));
        }
    }
}