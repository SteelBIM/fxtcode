using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

/***
 * 作者:  李晓东
 * 时间:  2013.12.4
 * 摘要:  创建 BaseController 控制器基类
 * **/
namespace FxtSpiderManager.Common
{
    [AuthorizeAttribute]
    public class BaseController:Controller
    {
        /// <summary>
        /// 获得绝对地址
        /// </summary>
        /// <param name="address">相对地址,例如:api/xx/</param>
        /// <returns></returns>
        public string GetUrl(string address)
        {
            return string.Format("http://localhost:4430/{0}", address);
        }
        //重写异常处理
        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                JsonResult result = new JsonResult();
                result.Data = new { type = "error", data = "请求异常" };
                filterContext.Result = result;
            }
            else
            {
                filterContext.Result = new RedirectResult("~/Error/index");
            }
            filterContext.ExceptionHandled = true;
            base.OnException(filterContext);
        }
    }
}
