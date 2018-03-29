using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CAS.Common;
using FxtCenterService.Logic;
using CAS.Entity.DBEntity;
using System.Data;

namespace FxtCenterService.API.handlers
{
    /// <summary>
    /// projectlist楼盘列表
    /// </summary>
    public class houselist : HttpHandlerBase
    {
        public override void ProcessRequest(HttpContext context)
        {
            if (!CheckMustRequest(new string[] { "cityid", "buildingid", "type", "fxtcompanyid" })) return;
            string result = "";
            string key = HttpUtility.UrlDecode(GetRequest("key"));
            int floorNo = StringHelper.TryGetInt(context.Request["floorno"]);
            int buildingId = StringHelper.TryGetInt(context.Request["buildingid"]);
            string type = GetRequest("type");
            DataSet ds = null;
            switch (type)
            {
                case "dropdown":
                    ds = DatHouseBL.GetHouseDropDownList(search, buildingId, floorNo);
                    result = DataTableToJSON(ds.Tables[0]);
                    break;
                case "autohouselist":
                    var houselist = DatHouseBL.GetAutoHouseListList(search, buildingId, floorNo,key).Select(o => new
                    {
                        houseid = o.houseid,
                        housename = o.housename,
                        buildarea=o.buildarea,
                        isevalue=o.isevalue,
                        weight = o.weight,
                        recordcount = o.recordcount
                    });
                    result = GetJson(houselist,1,"success");
                    break;
            }
            context.Response.Write(result);
        }
    }
}