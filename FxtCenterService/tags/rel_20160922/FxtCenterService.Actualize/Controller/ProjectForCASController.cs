using CAS.Common;
using CAS.Entity;
using CAS.Entity.DBEntity;
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
        /// 获取楼盘列表ForCAS_OUT
        /// </summary>
        /// <param name="funinfo">功能参数</param>
        /// <param name="company">公司</param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Project)]
        public static string GetProjectListByKey_ForCAS_OUT(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            if (search.PageIndex == 0)
            {
                search.PageIndex = 1;
                search.PageRecords = 15;
                search.Page = true;
            }
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            string key = funinfo.Value<string>("key");

            var list = DatProjectForCASBL.GetProjecttListByKey(search, company.parentshowdatacompanyid, search.CityId, key);

            return list.ToJson();
        }

        /// <summary>
        /// 获取楼栋下拉列表ForCAS_OUT
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Building)]
        public static string GetBuildingList_ForCAS_OUT(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            int projectId = EncryptHelper.ProjectIdDecrypt(funinfo.Value<string>("projectid"));
            string key = funinfo.Value<string>("key");
            if (search.PageIndex == 0)
            {
                search.PageIndex = 1;
                search.PageRecords = 15;
                search.Page = true;
            }
            List<DATBuilding> list = DatBuildingBL.GetAutoBuildingInfoList(search, projectId, key);
            var buildingList = list.Select(o => new
            {
                buildingid = EncryptHelper.ProjectIdEncrypt(o.buildingid.ToString()),
                buildingname = o.buildingname,
                isevalue = o.isevalue,
                weight = o.weight,
                recordcount = o.recordcount,
                totalfloor = o.totalfloor,
                unitsnumber = o.unitsnumber,
                totalnumber = o.totalnumber,
                projectid = EncryptHelper.ProjectIdEncrypt(o.projectid.ToString())
            });
            return buildingList.ToJson();
        }

        /// <summary>
        /// 获取楼层列表ForCAS_OUT
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Building)]
        public static string GetHouseNoList_ForCAS_OUT(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            int buildingId = EncryptHelper.ProjectIdDecrypt(funinfo.Value<string>("buildingid"));
            string key = funinfo.Value<string>("key");
            key = HttpUtility.UrlDecode(key);
            DataSet ds = DatHouseBL.GetAutoFloorNoList(search, buildingId, key);
            return ds == null || ds.Tables.Count <= 0 ? "" : ds.Tables[0].ToJson();
        }

        /// <summary>
        /// 获取房号列表ForCAS_OUT
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.House)]
        public static string GetHouseList_ForCAS_OUT(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            int buildingId = EncryptHelper.ProjectIdDecrypt(funinfo.Value<string>("buildingid"));
            //int floorNo = StringHelper.TryGetInt(funinfo.Value<string>("floorno"));
            string fn = funinfo.Value<string>("floorno");
            int? floorNo = null;
            if (!string.IsNullOrEmpty(fn))
            {
                floorNo = StringHelper.TryGetInt(fn);
            }
            string key = funinfo.Value<string>("key");
            List<DATHouse> list = DatHouseBL.GetAutoHouseListList(search, buildingId, floorNo, key);
            var houselist = list.Select(o => new
            {
                houseid = EncryptHelper.ProjectIdEncrypt(o.houseid.ToString()),
                housename = o.housename,
                buildarea = o.buildarea,
                isevalue = o.isevalue,
                weight = o.weight,
                builddate = o.builddate,
                purposecode = o.purposecode,
                purposecodename = o.purposecodename,
                housetypecode = o.housetypecode,
                housetypecodename = o.housetypecodename,
                recordcount = o.recordcount,
                unitno = o.unitno
            });
            return houselist.ToJson();
        }

        /// <summary>
        /// 自动估价，不往数据中心插入自动估价记录ForCAS_OUT
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string GetCASEValueByPId_ForCAS_OUT(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            int projectId = 0;
            int buildingId = 0;
            int houseId = 0;
            string pid = funinfo.Value<string>("projectid");
            if (!string.IsNullOrEmpty(pid))
            {
                projectId = EncryptHelper.ProjectIdDecrypt(pid);
            }
            string bid = funinfo.Value<string>("buildingid");
            if (!string.IsNullOrEmpty(bid))
            {
                buildingId = EncryptHelper.ProjectIdDecrypt(bid);
            }
            string hid = funinfo.Value<string>("houseid");
            if (!string.IsNullOrEmpty(bid))
            {
                houseId = EncryptHelper.ProjectIdDecrypt(hid);
            }
            //0，返回自动估价使用的案例；1，不返回自动估价使用的案例
            int type = StringHelper.TryGetInt(funinfo.Value<string>("type")) == 0 ? 0 : StringHelper.TryGetInt(funinfo.Value<string>("type"));
            //自动估价来源产品：默认为1003001（CAS）
            //int systypecode = StringHelper.TryGetInt(funinfo.Value<string>("systypecode")) == 0 ? 1003001 : StringHelper.TryGetInt(funinfo.Value<string>("systypecode"));
            int systypecode = company.parentproducttypecode;
            //账号
            string userid = string.IsNullOrEmpty(funinfo.Value<string>("userid")) ? "" : funinfo.Value<string>("userid");
            //客户公司id
            int companyid = company.companyid;// StringHelper.TryGetInt(funinfo.Value<string>("companyid")) == 0 ? company.companyid : StringHelper.TryGetInt(funinfo.Value<string>("companyid"));
            //自动估价目的；默认为1004001
            int queryTypeCode = StringHelper.TryGetInt(funinfo.Value<string>("queryTypeCode")) == 0 ? 1004001 : StringHelper.TryGetInt(funinfo.Value<string>("queryTypeCode"));
            //评估机构ID
            search.FxtCompanyId = company.parentshowdatacompanyid;
            int qid = StringHelper.TryGetInt(funinfo.Value<string>("qid"));
            int subhousetype = StringHelper.TryGetInt(funinfo.Value<string>("subhousetype"));
            double subhousearea = StringHelper.TryGetDouble(funinfo.Value<string>("subhousearea"));
            double subhouseavgprice = StringHelper.TryGetDouble(funinfo.Value<string>("subhouseavgprice"));
            double subhousetotalprice = StringHelper.TryGetDouble(funinfo.Value<string>("subhousetotalprice"));
            double buildingArea = StringHelper.TryGetDouble(funinfo.Value<string>("buildingarea"));//物业面积
            string StartDate = string.Empty;
            string EndDate = string.Empty;
            AutoPrice autoPrice = null;//自动估价结果
            autoPrice = DatHouseBL.GetCASEValueByPId(search.CityId, projectId, buildingId, houseId, search.FxtCompanyId, type,
             companyid, company.username, buildingArea, StartDate, EndDate, queryTypeCode, qid, systypecode, subhousetype, subhousearea, subhouseavgprice, subhousetotalprice);
            return autoPrice.ToJson();
        }
    }
}
