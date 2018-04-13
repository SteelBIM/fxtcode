using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using TestLog4Net;

namespace KSWF.Web.Admin.App_Start
{
    public class MyHandleExceptionAttribute :HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            var log  = new LogHelper();
            TestLog4Net.LogHelper.WriteLog(MethodBase.GetCurrentMethod().DeclaringType,  filterContext.Exception);
        }
    }
}