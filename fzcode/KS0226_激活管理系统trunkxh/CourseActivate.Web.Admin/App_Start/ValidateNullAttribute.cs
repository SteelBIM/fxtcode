using CourseActivate.Core.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseActivate.Web.Admin
{
    public class ValidateNullAttribute :ActionFilterAttribute 
    {
         
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            foreach(var par in filterContext.ActionParameters)
            {
                if (par.Value==null)
                {
                    JsonResult result = new JsonResult(){
                       Data= KingResponse.GetErrorResponse(string.Format("{0}参数不能为空",par.Key))
                    };
                    filterContext.Result = result;
                }
            }
        }
    }
}