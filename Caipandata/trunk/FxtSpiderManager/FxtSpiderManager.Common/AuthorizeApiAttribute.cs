using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Net;
using System.Net.Http;


/***
 * 作者:  李晓东
 * 时间:  2013.12.5
 * 摘要:  创建 AuthorizeApiAttribute WebApi验证类
 * **/
namespace FxtSpiderManager.Common
{
    /// <summary>
    /// WebApi身份验证
    /// </summary>
    public class AuthorizeApiAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            //actionContext.Response = new HttpResponseMessage(HttpStatusCode.NotFound);
            
            base.OnActionExecuting(actionContext);
        }
    }
}
