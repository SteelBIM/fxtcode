using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http.Filters;

/***
 * 作者:  李晓东
 * 时间:  2013.12.5
 * 摘要:  创建 ApiException 异常类
 * **/
namespace FxtSpider.Manager.Common
{
    public class ApiException:ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnException(actionExecutedContext);
        }
    }
}
