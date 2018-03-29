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
    public class buildinglist : HttpHandlerBase
    {
        public override void ProcessRequest(HttpContext context)
        {
            if (!CheckMustRequest(new string[] { "cityid", "projectid", "type", "fxtcompanyid" })) return;
            string result = "";
            string key = HttpUtility.UrlDecode(GetRequest("key"));
            int fxtCompanyId = StringHelper.TryGetInt(GetRequest("fxtcompanyid"));
            int cityid = StringHelper.TryGetInt(GetRequest("cityid"));
            int projectId = StringHelper.TryGetInt(GetRequest("projectid"));
            string type = GetRequest("type");

            switch (type)
            {
                case "list":
                    List<DATBuilding> list = DatBuildingBL.GetDATBuildingList(search, projectId, key);
                    result = GetJson(list, "");
                    break;
                case "dropdown":
                    int avgprice = StringHelper.TryGetInt(context.Request["avgprice"]);
                    DataSet ds = DatBuildingBL.GetBuildingBaseInfoList(search, projectId, avgprice);
                    result = DataTableToJSON(ds.Tables[0]);
                    break;
                case "autobuildinglist":
                    var buildingList = DatBuildingBL.GetAutoBuildingInfoList(search, projectId,key).Select(o => new {
                        buildingid = o.buildingid,
                        buildingname=o.buildingname,
                        isevalue = o.isevalue,
                        weight = o.weight,
                        recordcount = o.recordcount,
                        totalfloor= o.totalfloor
                    });
                    result = GetJson(buildingList);
                    break;
            }
            context.Response.Write(result);
        }
    }
}