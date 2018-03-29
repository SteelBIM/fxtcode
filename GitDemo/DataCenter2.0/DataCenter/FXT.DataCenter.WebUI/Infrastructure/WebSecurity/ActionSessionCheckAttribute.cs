using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FXT.DataCenter.WebUI.Infrastructure.WebSecurity
{
    public class ActionSessionCheckAttribute : ActionFilterAttribute
    {
        private readonly string[] _keys;

        public ActionSessionCheckAttribute(params string[] keys)
        {
            this._keys = keys;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!this.Authorize(filterContext))
            {
                var path = filterContext.HttpContext.Request.Path;
                var strUrl = "/Login/Index";

                filterContext.HttpContext.Response.Redirect(string.Format(strUrl, HttpUtility.UrlEncode(path)), true);
            }

        }

        protected virtual bool Authorize(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext == null)
                throw new ArgumentNullException("httpContext");

            return _keys.All(key => HttpContext.Current.Session[key] != null);
        }
    }
}
