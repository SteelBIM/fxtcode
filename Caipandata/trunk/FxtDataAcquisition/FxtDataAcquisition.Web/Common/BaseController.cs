namespace FxtDataAcquisition.Web.Common
{
    using System;
    using System.Web;
    using System.Web.Mvc;

    using Newtonsoft.Json;
    using FxtDataAcquisition.Common;
    using FxtDataAcquisition.Application.Interfaces;

    [ExceptionFilter]
    //[AuthorizeAttribute]
    public class BaseController : Controller
    {
        internal readonly IAdminService _unitOfWork;

        public BaseController(IAdminService unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 自定义返回Json格式
        /// </summary>
        /// <param name="result"></param>
        /// <param name="nceLoopHandling">是否循环序列表</param>
        /// <returns></returns>
        protected internal JsonResult AjaxJson(AjaxResult result, bool nceLoopHandling = true)
        {
            return new NJsonResult(result, nceLoopHandling ? ReferenceLoopHandling.Serialize : ReferenceLoopHandling.Ignore);
        }

        /// <summary>
        /// 自定义返回Json格式
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        protected internal JsonResult AjaxJson(object result)
        {
            return new NJsonResult(result);
        }

        ////重写异常处理
        //protected override void OnException(ExceptionContext filterContext)
        //{
        //    if (filterContext.HttpContext.Request.IsAjaxRequest())
        //    {
        //        JsonResult result = new JsonResult();
        //        result.Data = new { type = "error", data = "请求异常" };
        //        filterContext.Result = result;
        //    }
        //    else 
        //    {
        //        filterContext.Result = new RedirectResult("~/Error/index");
        //    }
        //    filterContext.ExceptionHandled = true;
        //    base.OnException(filterContext);
        //}
    }
}