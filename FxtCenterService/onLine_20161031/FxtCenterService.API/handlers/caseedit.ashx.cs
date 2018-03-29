using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CAS.Common;
using FxtCenterService.Logic;
using CAS.Entity.DBEntity;

namespace FxtCenterService.API.handlers
{
    /// <summary>
    /// projectlist楼盘列表
    /// </summary>
    public class caseedit : HttpHandlerBase
    {
        public override void ProcessRequest(HttpContext context)
        {
            if (!CheckMustRequest(new string[] { "cityid", "fxtcompanyid", "type" })) return;
            string result = "";
            string type = GetRequest("type");
            switch (type)
            {
                case "add":
                    DATCase model = WebCommon.InitModel<DATCase>(context.Request);
                    DATCaseBL.Add(model);
                    break;
                case "edit":
                    break;
            }
            context.Response.Write(result);
            context.Response.End();
        }
    }
}