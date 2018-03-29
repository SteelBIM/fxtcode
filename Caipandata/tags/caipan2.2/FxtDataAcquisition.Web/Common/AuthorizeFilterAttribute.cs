using FxtDataAcquisition.Application.Interfaces;
using FxtDataAcquisition.Framework.Ioc;
using Ninject;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using System.Web;
using log4net;

namespace FxtDataAcquisition.Web.Common
{
    public class AuthorizeFilterAttribute : AuthorizeAttribute
    {
        private readonly ISysRoleMenuFunctionService _functionService;
        private static readonly ILog log = LogManager.GetLogger(typeof(AuthorizeFilterAttribute));
        public AuthorizeFilterAttribute()
        {
            this._functionService = new StandardKernel(new SysRoleMenuFunctionBinder()).Get<ISysRoleMenuFunctionService>();
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            int errorType = 0;
            RequestType rType = NowRequestType;
            bool checkLogin = IsCheckLogin;
            if (checkLogin)
            {
                //验证登录
                var user = WebUserHelp.GetNowLoginUser();
                if (user != null)
                {
                    //验证权限
                    if ((
                        (AndNowFunctionCodes != null && AndNowFunctionCodes.Length > 0) || (OrNowFunctionCodes != null && OrNowFunctionCodes.Length > 0)
                        )
                        && !string.IsNullOrEmpty(NowFunctionPageUrl))
                    {
                        //if (WebUserHelp.CheckNowPageFunctionCode(NowFunctionPageUrl, AndNowFunctionCodes, OrNowFunctionCodes))
                        //{
                        user.NowCityId = WebUserHelp.GetNowCityId();
                        List<int> intList = new List<int>();
                        var list = _functionService.GetAllBy(user.UserName, user.FxtCompanyId, user.NowCityId, NowFunctionPageUrl).ToList();
                        if (AndNowFunctionCodes != null && list.Where(obj => AndNowFunctionCodes.Contains(obj.FunctionCode)).Count() < AndNowFunctionCodes.Length)
                        {
                            errorType = WebUserHelp.NotRight;
                        }

                        if (OrNowFunctionCodes != null && list.Where(obj => OrNowFunctionCodes.Contains(obj.FunctionCode)).Count() < 1)
                        {
                            errorType = WebUserHelp.NotRight;
                        }
                        //}
                    }
                }
                else
                {
                    log.Info("登陆失败");
                    errorType = WebUserHelp.NotLogin;
                }

                //是否通过身份验证
                if (errorType != 0)
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
                            filterContext.Result = new NJsonResult(new AjaxResult()
                            {
                                Result = true,
                                Code = "301",
                                Message = "登陆超时"
                            });
                        }
                    }
                    else if (errorType == WebUserHelp.NotRight)//无权限
                    {
                        if (rType == RequestType.ACTION)//同步页面请求
                        {
                            filterContext.Result = WebUserHelp.GetActionNotRightPage();
                        }
                        else if (rType == RequestType.OPEN)
                        {
                            filterContext.Result = new ContentResult() { Content = "无权限" };
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
            //base.OnActionExecuting(filterContext);
        }

        //在执行操作方法之前由 MVC 框架调用
        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{

        //    int errorType = 0;
        //    RequestType rType = NowRequestType;
        //    bool checkLogin = IsCheckLogin;
        //    if (checkLogin)
        //    {
        //        //验证登录
        //        if (WebUserHelp.GetNowLoginUser() != null)
        //        {
        //            //验证权限
        //            if ((
        //                (AndNowFunctionCodes != null && AndNowFunctionCodes.Length > 0) || (OrNowFunctionCodes != null && OrNowFunctionCodes.Length > 0)
        //                )
        //                && !string.IsNullOrEmpty(NowFunctionPageUrl))
        //            {
        //                //if (WebUserHelp.CheckNowPageFunctionCode(NowFunctionPageUrl, AndNowFunctionCodes, OrNowFunctionCodes))
        //                //{
        //                    var user = WebUserHelp.GetNowLoginUser();
        //                    List<int> intList = new List<int>();
        //                    var list = _functionService.GetAllBy(user.UserName, user.FxtCompanyId, user.NowCityId, NowFunctionPageUrl).ToList();
        //                    if (AndNowFunctionCodes != null && list.Where(obj => AndNowFunctionCodes.Contains(obj.FunctionCode)).Count() < AndNowFunctionCodes.Length)
        //                    {
        //                        errorType = WebUserHelp.NotRight;
        //                    }

        //                    if (OrNowFunctionCodes != null && list.Where(obj => OrNowFunctionCodes.Contains(obj.FunctionCode)).Count() < 1)
        //                    {
        //                        errorType = WebUserHelp.NotRight;
        //                    }
        //                //}
        //            }
        //        }
        //        else
        //        {
        //            errorType = WebUserHelp.NotLogin;
        //        }

        //        //是否通过身份验证
        //        if (errorType != 0)
        //        {
        //            //登陆超时
        //            if (errorType == WebUserHelp.NotLogin)
        //            {
        //                if (rType == RequestType.ACTION)//同步页面请求
        //                {
        //                    filterContext.Result = WebUserHelp.GetActionLoginPage();
        //                }
        //                else if (rType == RequestType.OPEN)//弹出窗口请求
        //                {
        //                    filterContext.Result = WebUserHelp.GetActionLoginPageOpen();
        //                }
        //                else//ajax页面请求
        //                {
        //                    filterContext.Result = new Ajax_JsonFormatResult_NotLogin();
        //                }
        //            }
        //            else if (errorType == WebUserHelp.NotRight)//无权限
        //            {
        //                if (rType == RequestType.ACTION)//同步页面请求
        //                {
        //                    filterContext.Result = WebUserHelp.GetActionNotRightPage();
        //                }
        //                else//ajax页面请求
        //                {
        //                    filterContext.Result = new Ajax_JsonFormatResult_NotRight();
        //                }
        //            }
        //        }
        //        ////是否通过身份验证
        //        //if (!filterContext.RequestContext.HttpContext.Request.IsAuthenticated)
        //        //{ }
        //        //if (session["user"] == null)
        //        //{
        //        //    filterContext.Result = new RedirectResult("~/Login/index");
        //        //}
        //    }
        //    base.OnActionExecuting(filterContext);
        //}
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
            context.HttpContext.Response.ContentType = "application/json";
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
            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.Write("null".MvcResponseJson(result: 0, errorType: WebUserHelp.NotRight, message: "无此操作权限"));
            context.HttpContext.Response.End();
        }
    }
}