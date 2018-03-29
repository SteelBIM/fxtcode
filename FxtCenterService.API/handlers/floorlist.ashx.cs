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
    public class floorlist : HttpHandlerBase
    {
        public override void ProcessRequest(HttpContext context)
        {
            if (!CheckMustRequest(new string[] { "cityid", "buildingid", "type", "fxtcompanyid" })) return;
            string result = "";
            string key = HttpUtility.UrlDecode(GetRequest("key"));
            int fxtCompanyId = StringHelper.TryGetInt(GetRequest("fxtcompanyid"));
            int buildingId = StringHelper.TryGetInt(context.Request["buildingid"]);
            string type = GetRequest("type");
            DataSet ds = null;
            try
            {
                switch (type)
                {
                    case "dropdown":
                        ds = DatHouseBL.GetHouseFileListWithSub(search.CityId, buildingId, "floorno", fxtCompanyId);
                        result = DataTableToJSON(ds.Tables[0]);
                        break;
                    case "unit":
                        ds = DatHouseBL.GetHouseFileListWithSub(search.CityId, buildingId, "unitno", fxtCompanyId);
                        result = DataTableToJSON(ds.Tables[0]);
                        break;
                    case "autofloorlist":
                        ds = DatHouseBL.GetAutoFloorNoList(search, buildingId,key);
                        result = DataTableToJSON(ds.Tables[0]);
                        break;
                    default:
                        break;
                }
                
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                result = GetJson(-1, "异常");
            }
            context.Response.Write(result);
        }
    }
}