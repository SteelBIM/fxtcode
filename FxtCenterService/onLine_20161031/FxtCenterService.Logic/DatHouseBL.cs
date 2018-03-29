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
using System.Diagnostics;

namespace FxtCenterService.Logic
{
    public class DatHouseBL
    {
        /// <summary>
        /// 新增房号信息到主表
        /// 创建人:曾智磊, 日期:2014-06-27
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tableName">根据城市查询出来的表名</param>
        /// <returns></returns>
        public static int Add(DATHouse model, string tableName)
        {
            if (model == null || string.IsNullOrEmpty(tableName))
            {
                return 0;
            }
            string _tableName = tableName;
            DATHouse.SetTableName<DATHouse>(_tableName);
            return DatHouseDA.Add(model);

        }
        /// <summary>
        /// 新增房号信息到子表
        /// 创建人:曾智磊, 日期:2014-06-27
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int AddSub(DATHouse model, string tableName)
        {
            if (model == null || string.IsNullOrEmpty(tableName))
            {
                return 0;
            }
            string _tableName = tableName;
            _tableName = tableName + "_sub";
            DATHouse.SetTableName<DATHouse>(_tableName);
            return DatHouseDA.Add(model);
        }
        /// <summary>
        /// 修改房号信息到主表
        /// 创建人:曾智磊, 日期:2014-06-27
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tableName">根据城市查询出来的表名</param>
        /// <returns></returns>
        public static int Update(DATHouse model, string tableName)
        {
            if (model == null || string.IsNullOrEmpty(tableName))
            {
                return 0;
            }
            string _tableName = tableName;
            DATHouse.SetTableName<DATHouse>(_tableName);
            return DatHouseDA.Update(model);

        }
        /// <summary>
        /// 修改房号信息到子表
        /// 创建人:曾智磊, 日期:2014-06-27
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tableName">根据城市查询出来的表名</param>
        /// <returns></returns>
        public static int UpdateSub(DATHouse model, string tableName)
        {
            if (model == null || string.IsNullOrEmpty(tableName))
            {
                return 0;
            }
            string _tableName = tableName + "_sub";
            DATHouse.SetTableName<DATHouse>(_tableName);
            return DatHouseDA.Update(model);

        }
        /// <summary>
        /// 获取单元或楼层列表(过滤删除的)
        /// </summary>
        /// <param name="buildingid">楼栋ID</param>
        /// <param name="files">字段 FloorNo/UnitNo </param>
        /// <returns></returns>
        public static DataSet GetHouseFileListWithSub(int cityId, int buildingId, string files, int fxtcompanyid, int typecode)
        {
            return DatHouseDA.GetHouseFileListWithSub(cityId, buildingId, files, "asc", fxtcompanyid, typecode);
        }
        /// <summary>
        /// 获取房号下拉列表
        /// </summary>
        /// <returns></returns>
        public static List<DATHouseOrderBy> GetHouseDropDownList(SearchBase search, int buildingId, int floorno)
        {
            return DatHouseDA.GetHouseDropDownList(search, buildingId, floorno);
        }
        /// <summary>
        /// 获取房号下拉列表forMCAS
        /// </summary>
        /// <returns></returns>
        public static List<DATHouseOrderBy> GetHouseDropDownList_MCAS(SearchBase search, int buildingId, int floorno, string key, string serialno,int producttypecode, int companyid)
        {
            List<DATHouseOrderBy> listResult = null;
            //if (producttypecode == 1003038 && companyid == 6)
            //{
            //    listResult = DatHouseDA.GetHouseDropDownList_MCAS_Mariadb(search, buildingId, floorno, key + "%", "%" + key + "%", serialno);
            //}
            //else
            //{
            //    listResult = DatHouseDA.GetHouseDropDownList_MCAS(search, buildingId, floorno, key + "%", "%" + key + "%", serialno);
            //}

            //string condition = "";
            //if (!string.IsNullOrEmpty(key))
            //{
            //    key = "" + SQLFilterHelper.EscapeLikeString(key, "$") + "%";
            //condition = " and HouseName like @key";
            //}
            //var listResult = DatHouseDA.GetHouseDropDownList_MCAS(search, buildingId, floorno, key, "", condition);
            //string param = "";
            //if (!string.IsNullOrEmpty(key))
            //{
            //    param = "%" + SQLFilterHelper.EscapeLikeString(key, "$") + "%";
            //    condition = " and HouseName not like @key and HouseName like @param";
            //    var listResult2 = DatHouseDA.GetHouseDropDownList_MCAS(search, buildingId, floorno, key, param, condition);
            //    listResult = listResult.Concat(listResult2).ToList();
            //}

            listResult = DatHouseDA.GetHouseDropDownList_MCAS(search, buildingId, floorno, key + "%", "%" + key + "%", serialno);
            return listResult;
        }

        /// <summary>
        /// 获取楼栋楼层列表以及每层所含房号总数
        /// </summary>
        public static DataSet GetAutoFloorNoList(SearchBase search, int buildingid, string key)
        {
            return DatHouseDA.GetAutoFloorNoList(search, buildingid, key);

        }

        /// <summary>
        /// 获取楼栋楼层列表以及每层所含房号总数forMCAS
        /// </summary>
        public static DataSet GetAutoFloorNoList_MCAS(SearchBase search, int buildingid, string key, int producttypecode, int companyid,string serialno = "")
        {
            DataSet listResult = new DataSet();
            //if (!string.IsNullOrEmpty(key))
            //{
            //    key = "" + SQLFilterHelper.EscapeLikeString(key, "$") + "%";
            //    condition = " and floorno like @floorno ";
            //}
            //if (producttypecode == 1003038 && companyid == 6)
            //{
            //    listResult = DatHouseDA.GetAutoFloorNoList_MCAS_Mariadb(search, buildingid, key + "%", "%" + key + "%", serialno);
            //}
            //else
            //{
            //    listResult = DatHouseDA.GetAutoFloorNoList_MCAS(search, buildingid, key + "%", "%" + key + "%", serialno);
            //}
            //if (!string.IsNullOrEmpty(key))
            //{
            //    param = "%" + SQLFilterHelper.EscapeLikeString(param, "$") + "%";
            //    condition = " and floorno not like @floorno and floorno like @param";
            //    var listResult2 = DatHouseDA.GetAutoFloorNoList_MCAS(search, buildingid, key, param, condition);
            //    for (int i = 0; i < listResult2.Tables[0].Rows.Count; i++)
            //    {
            //        DataRow dr = listResult.Tables[0].NewRow();
            //        dr["floorno"] = listResult2.Tables[0].Rows[i]["floorno"].ToString();
            //        dr["housecnt"] = int.Parse(listResult2.Tables[0].Rows[i]["housecnt"].ToString());
            //        listResult.Tables[0].Rows.Add(dr);
            //    }
            //}
            listResult = DatHouseDA.GetAutoFloorNoList_MCAS(search, buildingid, key + "%", "%" + key + "%", serialno);
            return listResult;
        }

        /// <summary>
        /// 获取楼栋楼层列表以及每层所含房号总数forMCAS
        /// </summary>
        public static DataSet GetAutoFloorNoList_OUT(SearchBase search, int buildingid, string key)
        {
            //string condition = "";
            //string param = key;
            //if (!string.IsNullOrEmpty(key))
            //{
            //    key = "" + SQLFilterHelper.EscapeLikeString(key, "$") + "%";
            //    condition = " and floorno like @floorno ";
            //}
            var listResult = DatHouseDA.GetAutoFloorNoList_OUT(search, buildingid, key + "%", "%" + key + "%");
            //if (!string.IsNullOrEmpty(key))
            //{
            //    param = "%" + SQLFilterHelper.EscapeLikeString(param, "$") + "%";
            //    condition = " and floorno not like @floorno and floorno like @param";
            //    var listResult2 = DatHouseDA.GetAutoFloorNoList_OUT(search, buildingid, key, param, condition);
            //    for (int i = 0; i < listResult2.Tables[0].Rows.Count; i++)
            //    {
            //        DataRow dr = listResult.Tables[0].NewRow();
            //        dr["floorno"] = listResult2.Tables[0].Rows[i]["floorno"].ToString();
            //        dr["housecnt"] = int.Parse(listResult2.Tables[0].Rows[i]["housecnt"].ToString());
            //        listResult.Tables[0].Rows.Add(dr);
            //    }
            //}
            return listResult;
        }

        /// <summary>
        /// 获取房号列表
        /// </summary>
        public static List<DATHouse> GetAutoHouseListList(SearchBase search, int buildingid, int? floorno, string key)
        {

            //List<DATHouse> result = new List<DATHouse>();
            //var list = DatHouseDA.GetAutoHouseListList(search, buildingid, floorno, key);
            //var group = list.GroupBy(m => m.housename);
            //foreach (var item in group)
            //{
            //    foreach (var item2 in item)
            //    {
            //        if (item.Count() > 1)
            //        {
            //            item2.housename = item2.unitno + item2.floorno.ToString() + item2.housename;
            //        }
            //        result.Add(item2);
            //    }
            //}
            //return result;

            return DatHouseDA.GetAutoHouseListList(search, buildingid, floorno, key);
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
        public static AutoPrice GetEValueByProjectId(int CityId, int ProjectId, int BuildingId, int HouseId, int FXTCompanyId, int type,
            int CompanyId, string UserId, double BuildingArea, string StartDate, string EndDate, int systypecode)
        {
            UserId = string.IsNullOrEmpty(UserId) ? "" : UserId;
            DataSet ds = DatHouseDA.GetEValueByProjectId(CityId, ProjectId, BuildingId, HouseId, FXTCompanyId, type,
             CompanyId, UserId, BuildingArea, StartDate, EndDate, 1004001, 0, systypecode, 0, 0, 0, 0);
            AutoPrice autoPrice = new AutoPrice();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                autoPrice = new AutoPrice();
                autoPrice.avgprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["avgPrice"].ToString());
                autoPrice.beprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["BEPrice"].ToString());
                if (ds.Tables.Count > 1)
                {
                    autoPrice.casecount = StringHelper.TryGetInt(ds.Tables[1].Rows[0]["casecount"].ToString());
                    autoPrice.casemax = StringHelper.TryGetInt(ds.Tables[1].Rows[0]["casemax"].ToString());
                    autoPrice.casemin = StringHelper.TryGetInt(ds.Tables[1].Rows[0]["casemin"].ToString());
                }
                autoPrice.heprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["HEPrice"].ToString());
                autoPrice.unitprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["UnitPrice"].ToString());
                autoPrice.eprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["EPrice"].ToString());
            }


            return autoPrice;
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
            return DatHouseDA.GetEValueByProjectId(CityId, ProjectId, BuildingId, HouseId, FXTCompanyId, Type,
             CompanyId, UserId, BuildingArea, StartDate, EndDate, QueryTypeCode, qid, sysTypeCode, subhousetype, subhousearea, subhouseavgprice, subhousetotalprice);
        }


        /// <summary>
        /// 获取房号信息
        /// 创建人:曾智磊,日期:2014-06-30
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="buildingId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <param name="houseName"></param>
        /// <returns></returns>
        public static DATHouse GetHouseByName(int cityId, int buildingId, int fxtCompanyId, string houseName, int typecode)
        {
            return DatHouseDA.GetHouseByName(cityId, buildingId, fxtCompanyId, houseName, typecode);
        }
        /// <summary>
        /// 根据获取房号信息
        /// 创建人:曾智磊,日期:2014-07-03
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="buildingId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <param name="houseId"></param>
        /// <returns></returns>
        public static DATHouse GetHouseById(int cityId, int buildingId, int fxtCompanyId, int houseId)
        {
            return DatHouseDA.GetHouseById(cityId, buildingId, fxtCompanyId, houseId);
        }
        /// <summary>
        /// 自动估价，这个方法将不往数据中心插入自动估价结果，所有的结果都会返回来
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
        public static AutoPrice GetCASEValueByPId(int CityId, int ProjectId, int BuildingId, int HouseId, int FXTCompanyId, int Type,
            int CompanyId, string UserId, double BuildingArea, string StartDate, string EndDate, int QueryTypeCode, int qid, int sysTypeCode, int subhousetype, double subhousearea, double subhouseavgprice, double subhousetotalprice)
        {
            AutoPrice autoPrice = new AutoPrice();

            DataSet ds = DatHouseDA.GetCASEValueByPId(CityId, ProjectId, BuildingId, HouseId, FXTCompanyId, Type,
             CompanyId, UserId, BuildingArea, StartDate, EndDate, QueryTypeCode, qid, sysTypeCode, subhousetype, subhousearea, subhouseavgprice, subhousetotalprice);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                autoPrice = new AutoPrice();
                autoPrice.avgprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["avgPrice"].ToString());
                autoPrice.beprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["BEPrice"].ToString());
                if (ds.Tables.Count > 1)
                {
                    autoPrice.casecount = StringHelper.TryGetInt(ds.Tables[1].Rows[0]["casecount"].ToString());
                    autoPrice.casemax = StringHelper.TryGetInt(ds.Tables[1].Rows[0]["casemax"].ToString());
                    autoPrice.casemin = StringHelper.TryGetInt(ds.Tables[1].Rows[0]["casemin"].ToString());
                    autoPrice.caseavg = StringHelper.TryGetInt(ds.Tables[1].Rows[0]["caseavg"].ToString());
                    autoPrice.startdate = Convert.ToString(ds.Tables[1].Rows[0]["startdate"]);
                    autoPrice.enddate = Convert.ToString(ds.Tables[1].Rows[0]["enddate"]);
                }
                autoPrice.heprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["HEPrice"].ToString());
                autoPrice.unitprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["UnitPrice"].ToString());
                autoPrice.eprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["eprice"].ToString());
                autoPrice.purposecode = StringHelper.TryGetInt(ds.Tables[0].Rows[0]["purposecode"].ToString());
                autoPrice.buildingtypecode = StringHelper.TryGetInt(ds.Tables[0].Rows[0]["buildingtypecode"].ToString());
                autoPrice.totalfloor = StringHelper.TryGetInt(ds.Tables[0].Rows[0]["totalfloor"].ToString());
                autoPrice.builddate = Convert.ToString(ds.Tables[0].Rows[0]["builddate"]);
                autoPrice.nominalfloor = Convert.ToString(ds.Tables[0].Rows[0]["nominalfloor"]);
                autoPrice.buildingname = Convert.ToString(ds.Tables[0].Rows[0]["buildingname"]);

                autoPrice.housetypecode = StringHelper.TryGetInt(ds.Tables[0].Rows[0]["housetypecode"].ToString());

                autoPrice.housename = Convert.ToString(ds.Tables[0].Rows[0]["housename"]);
                autoPrice.structurecode = StringHelper.TryGetInt(ds.Tables[0].Rows[0]["structurecode"].ToString());



                autoPrice.address = Convert.ToString(ds.Tables[0].Rows[0]["address"]);
                autoPrice.projectname = Convert.ToString(ds.Tables[0].Rows[0]["projectname"]);

            }

            return autoPrice;
        }


        public static DataSet GetHouseDetailInfo(int houseid, SearchBase search)
        {
            return DatHouseDA.GetHouseDetailInfo(houseid, search);
        }
    }


}
