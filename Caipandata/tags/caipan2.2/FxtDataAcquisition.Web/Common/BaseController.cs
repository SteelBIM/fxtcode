using FxtDataAcquisition.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using FxtDataAcquisition.Common;

namespace FxtDataAcquisition.Web.Common
{
    [ExceptionFilter]
    //[AuthorizeAttribute]
    public class BaseController : Controller
    {
        internal readonly IAdminService _unitOfWork;

        public BaseController(IAdminService unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        protected internal JsonResult AjaxJson(AjaxResult result)
        {
            return new NJsonResult(result);
        }

        protected internal JsonResult AjaxJson(object result)
        {
            return new NJsonResult(result);
        }

        /// <summary>
        /// 获得绝对地址
        /// </summary>
        /// <param name="address">相对地址,例如:api/xx/</param>
        /// <returns></returns>
        public string GetUrl(string address)
        {
            return string.Format("http://localhost:4430/{0}", address);
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