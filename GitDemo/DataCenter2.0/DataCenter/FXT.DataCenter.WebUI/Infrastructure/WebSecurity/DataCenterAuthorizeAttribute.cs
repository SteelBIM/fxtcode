using System;
using System.Linq;
using System.Web.Mvc;
using System.Web;

namespace FXT.DataCenter.WebUI.Infrastructure.WebSecurity
{
    public class DataCenterAuthorizeAttribute : AuthorizeAttribute
    {
        private int _code { get; set; }

        public DataCenterAuthorizeAttribute(int code)
        {
            this._code = code;
        }

        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            if (Passport.Current.IsAuthenticated)
            {
                if (Passport.Current.Menus.Count > 0)
                {
                   return Passport.Current.Menus.Select(m => m.ModuleCode).Contains(_code);
                }

            }
            return false;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (!AuthorizeCore(filterContext.HttpContext))
            {
                HandleUnauthorizedRequest(filterContext);
            }
            
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            else
            {
                string path = filterContext.HttpContext.Request.Path;
                string strUrl = "/Login/Unanthroize";

                filterContext.HttpContext.Response.Redirect(string.Format(strUrl, HttpUtility.UrlEncode(path)), true);

            }
        }

    }
}
