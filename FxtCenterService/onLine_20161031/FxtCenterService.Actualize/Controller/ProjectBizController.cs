using System;
using System.Collections.Generic;
using System.Linq;
using CAS.Common;
using FxtCenterService.Logic;
using CAS.Entity;
using Newtonsoft.Json.Linq;
using CAS.Entity.DBEntity;
using System.Data;
using CAS.Entity.GJBEntity;
using System.Web;
using System.IO;
using System.ServiceModel.Web;
using System.Text.RegularExpressions;
using OpenPlatform.Framework.FlowMonitor;
using Newtonsoft.Json;
using FxtCommon.Openplatform.Data;
using FxtCommon.Openplatform.GrpcService;
using FxtOpenClient.ClientService;
using CAS.Entity.FxtProject;
using FxtCenterService.Common;
using System.Diagnostics;
using System.Data.Common;
using System.Data.SqlClient;
using FxtCenterService.DataAccess;
using System.Threading.Tasks;

namespace FxtCenterService.Actualize
{
    public partial class DataController
    {
        /// <summary>/// <summary>
        /// 获取商业街列表
        /// </summary>
        /// <param name="funinfo">功能参数</param>
        /// <param name="company">公司</param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Project)]
        public static string GetListBiz(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            string key = funinfo.Value<string>("key");
            key = HttpUtility.UrlDecode(key);
            int items = StringHelper.TryGetInt(funinfo.Value<string>("items"));
            if (items == 0) items = 15;
            List<Dictionary<string, object>> list = DatProjectBizBL.GetListBiz(search, key, items);
            return list.ToJson();
        }

        /// <summary>/// <summary>
        /// 获取商业楼栋列表
        /// </summary>
        /// <param name="funinfo">功能参数</param>
        /// <param name="company">公司</param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Project)]
        public static string GetBuildingListBiz(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            int projectId = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            string key = funinfo.Value<string>("key");
            key = HttpUtility.UrlDecode(key);
            int items = StringHelper.TryGetInt(funinfo.Value<string>("items"));
            if (items == 0) items = 15;
            List<Dictionary<string, object>> list = DatProjectBizBL.GetBuildingListBiz(search, projectId, key, items);
            return list.ToJson();
        }

        /// <summary>/// <summary>
        /// 获取商业楼层列表
        /// </summary>
        /// <param name="funinfo">功能参数</param>
        /// <param name="company">公司</param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Project)]
        public static string GetFloorListBiz(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            int buildingid = StringHelper.TryGetInt(funinfo.Value<string>("buildingid"));
            string key = funinfo.Value<string>("key");
            key = HttpUtility.UrlDecode(key);
            int items = 100;
            List<Dictionary<string, object>> list = DatProjectBizBL.GetFloorListBiz(search,buildingid, key, items);
            return list.ToJson();
        }

        /// <summary>/// <summary>
        /// 获取商业房号列表
        /// </summary>
        /// <param name="funinfo">功能参数</param>
        /// <param name="company">公司</param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Project)]
        public static string GetHouseListBiz(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            int floorId = StringHelper.TryGetInt(funinfo.Value<string>("floorId"));
            string key = funinfo.Value<string>("key");
            key = HttpUtility.UrlDecode(key);
            int items = 100;
            List<Dictionary<string, object>> list = DatProjectBizBL.GetHouseListBiz(search, floorId, key, items);
            return list.ToJson();
        }

    }
}
