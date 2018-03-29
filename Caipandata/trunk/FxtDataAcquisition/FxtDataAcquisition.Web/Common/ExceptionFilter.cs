namespace FxtDataAcquisition.Web.Common
{
    using System;
    using System.Net;
    using System.Linq;
    using System.Web.Mvc;

    using CAS.Common.MVC4;

    /// <summary>
    /// 拦截所有异常，并做友好的提示
    /// </summary>
    public class ExceptionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception == null)
                base.OnActionExecuted(filterContext);
            else
            {
                LogHelper.CASLog.Error(filterContext.Exception);
                OnException(filterContext);
            }
        }

        public void OnException(ActionExecutedContext filterContext)
        {
            string errCode = "302";
            string message = filterContext.Exception.Message;
            if (filterContext.Exception is WebException)
                errCode = "302";
            else
                errCode = "301";

            var resultAttrs = filterContext.ActionDescriptor.GetCustomAttributes(false);
            if (resultAttrs != null && resultAttrs.Count() > 0)
            {
                var af = resultAttrs[0] as AuthorizeFilterAttribute;
                if (af != null)
                {
                    if (af.NowRequestType == RequestType.AJAX)
                    {
                        filterContext.ExceptionHandled = true;
                        filterContext.Result = new NJsonResult(new AjaxResult
                        {
                            Code = errCode,
                            Message = message,
                            Result = false
                        });
                    }
                    else
                    {
                        filterContext.ExceptionHandled = true;
                        var viewResult = new ViewResult
                        {
                            ViewName = "Error-500",
                        };
                        var doneInfo = new DoneInfo
                        {
                            LazyTime = 5,
                            Message = message,
                            Title = ExceptionTitle(filterContext.Exception)
                            //,
                            //RefreshUrl = filterContext.RequestContext.HttpContext.Request.UrlReferrer.OriginalString,
                        };
                        doneInfo.Links.Add("返回", "javascript:location.go(-1)");
                        viewResult.ViewData.Model = doneInfo;
                        filterContext.Result = viewResult;
                    }
                }

            }
            else
            {
                filterContext.ExceptionHandled = true;
                var viewResult = new ViewResult
                {
                    ViewName = "Error-500",
                };
                var doneInfo = new DoneInfo
                {
                    LazyTime = 5,
                    Message = message,
                    Title = ExceptionTitle(filterContext.Exception)
                    //,
                    //RefreshUrl = filterContext.RequestContext.HttpContext.Request.UrlReferrer.OriginalString,
                };
                doneInfo.Links.Add("返回", "javascript:location.go(-1)");
                viewResult.ViewData.Model = doneInfo;
                filterContext.Result = viewResult;
            }

            if (!filterContext.ExceptionHandled)
            {
                filterContext.Exception = new Exception(message);
            }
        }

        private string ExceptionTitle(Exception ex)
        {
            string title = string.Empty;
            //if (ex is ExcelException)
            //{
            //    title = "Excel表格操作有误！";
            //}
            //else if (ex is ValidException)
            //{
            //    title = "输入的信息有误！";
            //}
            //else
            //{
            //    title = "系统出错啦，非常抱歉……";
            //}
            return title;
        }
    }

}
