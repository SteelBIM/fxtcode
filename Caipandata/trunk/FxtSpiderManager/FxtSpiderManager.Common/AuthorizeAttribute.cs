using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

/***
 * 作者:  李晓东
 * 时间:  2013.12.4
 * 摘要:  创建 AuthorizeAttribute 验证类
 * **/
namespace FxtSpiderManager.Common
{
    /// <summary>
    /// 身份验证
    /// </summary>
    public class AuthorizeAttribute : ActionFilterAttribute
    {
        //在执行操作方法之前由 MVC 框架调用
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //是否通过身份验证
            if (!filterContext.RequestContext.HttpContext.Request.IsAuthenticated)
            { }
            //if (session["user"] == null)
            //{
            //    filterContext.Result = new RedirectResult("~/Login/index");
            //}
            base.OnActionExecuting(filterContext);
        }
    }
}
