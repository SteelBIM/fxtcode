using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using CAS.Entity;
using CAS.Common;
using FxtUseCenterService.Web.Library;

namespace FxtUseCenterService.Web
{
    /// <summary>
    /// ucapi 的摘要说明
    /// </summary>
    public class ucapi : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string token = context.Request["token"],
                   np = context.Request["np"];
            string content = string.Empty;
            WCFJsonData jsonData = new WCFJsonData();
            jsonData = Public.GetAPIPwdCheckResult( np, token, context);
            content = JSONHelper.ObjectToJSON(jsonData);
            context.Response.Write(content);
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}