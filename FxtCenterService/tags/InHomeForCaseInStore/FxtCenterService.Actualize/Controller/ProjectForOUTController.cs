using CAS.Common;
using CAS.Entity;
using FxtCenterService.Logic;
using Newtonsoft.Json.Linq;
using OpenPlatform.Framework.FlowMonitor;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace FxtCenterService.Actualize
{
    public partial class DataController
    {
        /// <summary>
        /// 获取楼层列表forOUT
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Building)]
        public static string GetHouseNoList_MCAS_ForOUT(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.CityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            search.SysTypeCode = company.parentproducttypecode;//1003036;
            int buildingId = EncryptHelper.ProjectIdDecrypt(funinfo.Value<string>("buildingid"));
            string key = funinfo.Value<string>("key");
            key = HttpUtility.UrlDecode(key);
            DataSet ds = DatHouseBL.GetAutoFloorNoList_OUT(search, buildingId, key);
            return ds == null || ds.Tables.Count <= 0 ? "" : ds.Tables[0].ToJson();
        }
    }
}
