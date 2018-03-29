using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using FxtCenterService.DataAccess;
using System.Data;
using System.Collections;
using CAS.Common;
using CAS.Entity;

namespace FxtCenterService.Logic
{
    public class DatHouseBL
    {
        /// <summary>
        /// 获取单元或楼层列表(过滤删除的)
        /// </summary>
        /// <param name="buildingid">楼栋ID</param>
        /// <param name="files">字段 FloorNo/UnitNo </param>
        /// <returns></returns>
        public static DataSet GetHouseFileListWithSub(int cityId, int buildingId, string files, int fxtcompanyid)
        {
            return DatHouseDA.GetHouseFileListWithSub(cityId, buildingId, files, "asc",fxtcompanyid);
        }
        /// <summary>
        /// 获取房号下拉列表
        /// </summary>
        /// <returns></returns>
        public static DataSet GetHouseDropDownList(SearchBase search, int buildingId, int floorno)
        {
            return DatHouseDA.GetHouseDropDownList(search, buildingId, floorno);
        }

        /// <summary>
        /// 获取楼栋楼层列表以及每层所含房号总数
        /// </summary>
        public static DataSet GetAutoFloorNoList(SearchBase search, int buildingid,string key)
        {
            return DatHouseDA.GetAutoFloorNoList(search, buildingid,key);

        }

        /// <summary>
        /// 获取房号列表
        /// </summary>
        public static List<DATHouse> GetAutoHouseListList(SearchBase search, int buildingid, int floorno,string key) 
        {
            return DatHouseDA.GetAutoHouseListList(search, buildingid, floorno,key);
        }

        /// <summary>
        /// 得到云估价数据 重写
        /// </summary>
        /// <param name="CityId"></param>
        /// <param name="ProjectId"></param>
        /// <param name="BuildingId">可为0</param>
        /// <param name="HouseId">可为0</param>
        /// <param name="FXTCompanyId">评估机构ID</param>
        /// <param name="Type">0：正常询价，1：只要均价</param>
        /// <param name="CompanyId">客户单位Id</param>
        /// <param name="UserId">账号</param>
        /// <param name="BuildingArea">建筑面积</param>
        /// <param name="StartDate">可为空</param>
        /// <param name="EndDate">可为空</param>
        /// <param name="QueryTypeCode">询价目的[1]</param>
        /// <returns>Tables[0]:询价结果，Tables[1]:案例统计，Tables[2]:案例明细，Tables[3]:按天统计案例均价</returns>
        public static AutoPrice GetEValueByProjectId(int CityId, int ProjectId, int BuildingId, int HouseId, int FXTCompanyId,
            int CompanyId, string UserId, double BuildingArea, string StartDate, string EndDate)
        {
            UserId = string.IsNullOrEmpty(UserId) ? "" : UserId;
            DataSet set= DatHouseDA.GetEValueByProjectId(CityId, ProjectId, BuildingId, HouseId, FXTCompanyId, 0,
             CompanyId, UserId, BuildingArea, StartDate, EndDate, 1004001, 0, 1003001, 0, 0, 0, 0);
            AutoPrice autoprice = new AutoPrice();
            if (set.Tables.Count>0)
            {
                autoprice.avgprice = StringHelper.TryGetDecimal(set.Tables[0].Rows[0]["avgPrice"].ToString());
                autoprice.beprice =StringHelper.TryGetDecimal( set.Tables[0].Rows[0]["BEPrice"].ToString());
                autoprice.casecount = StringHelper.TryGetInt(set.Tables[1].Rows[0]["casecount"].ToString());
                autoprice.casemax = StringHelper.TryGetInt(set.Tables[1].Rows[0]["casemax"].ToString());
                autoprice.casemin = StringHelper.TryGetInt(set.Tables[1].Rows[0]["casemin"].ToString());
                autoprice.heprice = StringHelper.TryGetDecimal(set.Tables[0].Rows[0]["HEPrice"].ToString());
                autoprice.unitprice = StringHelper.TryGetDecimal(set.Tables[0].Rows[0]["UnitPrice"].ToString());
                autoprice.eprice = StringHelper.TryGetDecimal(set.Tables[0].Rows[0]["EPrice"].ToString());
            }

            return autoprice;
        }

         /// <summary>
        /// 得到云估价数据
        /// </summary>
        /// <param name="CityId"></param>
        /// <param name="ProjectId"></param>
        /// <param name="BuildingId">可为0</param>
        /// <param name="HouseId">可为0</param>
        /// <param name="FXTCompanyId">评估机构ID</param>
        /// <param name="Type">0：正常询价，1：只要均价</param>
        /// <param name="CompanyId">客户单位Id</param>
        /// <param name="UserId">账号</param>
        /// <param name="BuildingArea">建筑面积</param>
        /// <param name="StartDate">可为空</param>
        /// <param name="EndDate">可为空</param>
        /// <param name="QueryTypeCode">询价目的[1]</param>
        /// <returns>Tables[0]:询价结果，Tables[1]:案例统计，Tables[2]:案例明细，Tables[3]:按天统计案例均价</returns>
        public static DataSet GetEValueByProjectId(int CityId, int ProjectId, int BuildingId, int HouseId, int FXTCompanyId, int Type,
            int CompanyId, string UserId, double BuildingArea, string StartDate, string EndDate, int QueryTypeCode, int qid, int sysTypeCode, int subhousetype, double subhousearea, double subhouseavgprice, double subhousetotalprice)
        {
            return DatHouseDA.GetEValueByProjectId( CityId,  ProjectId,  BuildingId,  HouseId,  FXTCompanyId,  Type,
             CompanyId,  UserId,  BuildingArea,  StartDate,  EndDate,  QueryTypeCode,  qid,  sysTypeCode,  subhousetype,  subhousearea,  subhouseavgprice,  subhousetotalprice);
        }

    }

   
}
