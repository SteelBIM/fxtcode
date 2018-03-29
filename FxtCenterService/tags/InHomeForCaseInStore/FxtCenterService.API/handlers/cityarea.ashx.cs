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
    /// cityarea 楼盘列表
    /// </summary>
    public class cityarea : HttpHandlerBase
    {
        public override void ProcessRequest(HttpContext context)
        {
            if (!CheckMustRequest(new string[] { "type" })) return;
            string type = GetRequest("type");
            int[] areaids = GetRequest("areaid").Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(StringHelper.TryGetInt).ToArray();
            int provinceid = StringHelper.TryGetInt(GetRequest("provinceid"));

            string result = "";
            switch (type)
            {
                case "provincelist"://省份列表
                    List<SYSProvince> prolist = SYSProvinceBL.GetSYSProvinceList(search);
                    result = GetJson(prolist);
                    break;
                case "citylist"://城市列表
                    List<SYSCity> citylist = SYSCityBL.GetSYSCityList(search, provinceid);
                    result = GetJson(citylist);
                    break;
                case "arealist"://区域列表
                    List<SYSArea> arealist = SYSAreaBL.GetSYSAreaList(search, areaids);
                    result = GetJson(arealist);
                    break;
            }
            context.Response.Write(result);
            context.Response.End();
        }
    }
}