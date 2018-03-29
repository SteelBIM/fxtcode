using FxtDataAcquisition.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FxtDataAcquisition.Web.Common
{
    public class AuthorizeAttribute : ActionFilterAttribute
    {
        //在执行操作方法之前由 MVC 框架调用
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            int errorType = 0;
            string message = "";
            RequestType rType = NowRequestType;
            bool checkLogin = IsCheckLogin;
            if (checkLogin)
            {
                //是否通过身份验证
                if (!WebUserHelp.CheckUser(filterContext,AndNowFunctionCodes,OrNowFunctionCodes,NowFunctionPageUrl, out errorType, out message))
                {
                    //登陆超时
                    if (errorType == WebUserHelp.NotLogin)
                    {
                        if (rType == RequestType.ACTION)//同步页面请求
                        {
                            filterContext.Result = WebUserHelp.GetActionLoginPage();
                        }
                        else if (rType == RequestType.OPEN)//弹出窗口请求
                        {
                            filterContext.Result = WebUserHelp.GetActionLoginPageOpen();
                        }
                        else//ajax页面请求
                        {
                            filterContext.Result = new Ajax_JsonFormatResult_NotLogin();
                        }
                    }
                    else if (errorType == WebUserHelp.NotRight)//无权限
                    {
                        if (rType == RequestType.ACTION)//同步页面请求
                        {
                            filterContext.Result = WebUserHelp.GetActionNotRightPage();
                        }
                        else//ajax页面请求
                        {
                            filterContext.Result = new Ajax_JsonFormatResult_NotRight();
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
        /// 当前操作的页面URL(用于权限),可为null
        /// </summary>
        public string NowFunctionPageUrl
        {
            get;
            set;
        }
        private int[] andNowFunctionCodes = null;
        /// <summary>
        /// 当前验证通过必须包含的操作项CODE(用于权限),可为null
        /// </summary>
        public int[] AndNowFunctionCodes
        {
            get { return andNowFunctionCodes; }
            set { andNowFunctionCodes = value; }
        }
        private int[] orNowFunctionCodes = null;
        /// <summary>
        /// 当前验证通过可选包含的操作项CODE(用于权限),可为null
        /// </summary>
        public int[] OrNowFunctionCodes
        {
            get { return orNowFunctionCodes; }
            set { orNowFunctionCodes = value; }
        }
        private bool isCheckLogin = true;
        /// <summary>
        /// 是否验证登录
        /// </summary>
        public bool IsCheckLogin
        {
            get { return isCheckLogin; }
            set { isCheckLogin = value; }
        }
        private RequestType nowRequestType = RequestType.ACTION;
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
        /// <summary>
        /// 弹出窗口
        /// </summary>
        OPEN
    }
    /// <summary>
    /// 用于ajax操作,登录超时时
    /// </summary>
    public class Ajax_JsonFormatResult_NotLogin : JsonResult
    {
        public Ajax_JsonFormatResult_NotLogin()
        {

        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Write("null".MvcResponseJson(result: 0, errorType: WebUserHelp.NotLogin, message: "登录超时"));
            context.HttpContext.Response.End();
        }
    }
    /// <summary>
    /// 用于ajax操作,无权限时
    /// </summary>
    public class Ajax_JsonFormatResult_NotRight : JsonResult
    {
        public Ajax_JsonFormatResult_NotRight()
        {

        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Write("null".MvcResponseJson(result: 0, errorType: WebUserHelp.NotRight, message: "无此操作权限"));
            context.HttpContext.Response.End();
        }
    }
}