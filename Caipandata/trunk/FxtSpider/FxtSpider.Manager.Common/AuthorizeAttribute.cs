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
namespace FxtSpider.Manager.Common
{
   
    /// <summary>
    /// 身份验证
    /// </summary>
    public class AuthorizeAttribute : ActionFilterAttribute
    {
        //在执行操作方法之前由 MVC 框架调用
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            int errorType = 0;
            string message = "";
            int operationType = NowOperationType;
            RequestType rType = NowRequestType;
            bool checkLogin = IsCheckLogin;
            if (checkLogin)
            {
                //是否通过身份验证
                if (!WebUserHelp.CheckUser(operationType, out errorType, out message))
                {
                    if (errorType == WebUserHelp.NotLogin)
                    {
                        if (rType == RequestType.ACTION)
                        {
                            filterContext.Result = new RedirectResult("~/Login/index");
                        }
                        else
                        {
                            filterContext.Result = new JsonFormatResult();
                        }
                    }
                }
                ////是否通过身份验证
                //if (!filterContext.RequestContext.HttpContext.Request.IsAuthenticated)
                //{ }
                //if (session["user"] == null)
                //{
                //    filterContext.Result = new RedirectResult("~/Login/index");
                //}
            }
            base.OnActionExecuting(filterContext);
        }
        /// <summary>
        /// 当前操作类型
        /// </summary>
        public int NowOperationType
        {
            get;
            set;
        }
        private bool isCheckLogin = false;
        /// <summary>
        /// 是否验证登录
        /// </summary>
        public bool IsCheckLogin
        {
            get{return isCheckLogin; }
            set { isCheckLogin = value; }
        }
        private RequestType nowRequestType=RequestType.ACTION;
        /// <summary>
        /// 标记当前请求为同步还是异步
        /// </summary>
        public RequestType NowRequestType
        {
            get { return nowRequestType; }
            set { nowRequestType = value; }
        }

    }
    /// <summary>
    /// 比较的方向，如下：
    /// ACTION：同步请求(默认)
    /// AJAX：异步请求
    /// </summary>
    public enum RequestType
    {
        /// <summary>
        /// 同步请求(默认)
        /// </summary>
        ACTION,
        /// <summary>
        /// 异步请求
        /// </summary>
        AJAX,
    }

    public class JsonFormatResult : JsonResult
    {
        public JsonFormatResult()
        {

        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Write("null".MvcResponseJson(result: 0, errorType: WebUserHelp.NotLogin.ToString(), message: "登录超时"));
            context.HttpContext.Response.End();
        }
    }

}
