using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kingsun.SynchronousStudy.BLL;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.SynchronousStudy.Web.Handler
{
    /// <summary>
    /// GetModuleList 的摘要说明
    /// </summary>
    public class GetModuleList : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            ModularManageBLL modularManageBll = new ModularManageBLL();
            List<TB_ModularManage> data = modularManageBll.GetModularList();
            context.Response.Write(JsonHelper.EncodeJson(data));
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