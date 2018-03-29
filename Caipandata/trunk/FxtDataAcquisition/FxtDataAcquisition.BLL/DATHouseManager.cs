using FxtDataAcquisition.Common;
using FxtDataAcquisition.Data;
using FxtDataAcquisition.DTODomain.NHibernate;
using FxtDataAcquisition.NHibernate.Entities;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Newtonsoft.Json.Linq;
using log4net;

namespace FxtDataAcquisition.BLL
{
    public static class DATHouseManager
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(DATHouseManager));
        /// <summary>
        /// 根据楼栋ID+楼层获取房号列表
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="cityId"></param>
        /// <param name="floorNo"楼层></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static IList<DATHouse> GetHouseByBuildingIdAndFloorNo(int buildingId, int cityId, int floorNo, DataBase _db = null)
        {
            DataBase db = new DataBase(_db);
            try
            {
                string sql = "{0} BuildingId=:buildingId and CityID =:cityId  and Valid=1";
                if (floorNo > 0)
                {
                    sql = sql + " and FloorNo=" + floorNo;
                }
                sql = string.Format(sql, NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_DATHouse));
                List<NHParameter> parameters = new List<NHParameter>();
                parameters.Add(new NHParameter("buildingId", buildingId, NHibernateUtil.Int32));
                parameters.Add(new NHParameter("cityId", cityId, NHibernateUtil.Int32));
                //parameters.Add(new NHParameter("floorNo", floorNo, NHibernateUtil.Int32));
                IList<DATHouse> houseList = db.DB.GetCustomSQLQueryList<DATHouse>(sql, parameters).ToList();
                // .GetListCustom<DATHouse>(
                //(Expression<Func<DATHouse, bool>>)
                //(tbl =>
                //    tbl.BuildingId == buildingId && tbl.CityID == cityId&&tbl.FloorNo==floorNo&&tbl.Valid==1
                //)).ToList<DATHouse>();
                db.Close();
                return houseList;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }
        /// <summary>
        /// 根据楼栋ID获取房号列表
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="cityId"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static IList<DATHouse> GetHouseByBuildingId(int buildingId, int cityId, DataBase _db = null)
        {
            DataBase db = new DataBase(_db);
            try
            {
                string sql = "{0} BuildingId=:buildingId and CityID =:cityId and Valid=1";
                sql = string.Format(sql, NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_DATHouse));
                List<NHParameter> parameters = new List<NHParameter>();
                parameters.Add(new NHParameter("buildingId", buildingId, NHibernateUtil.Int32));
                parameters.Add(new NHParameter("cityId", cityId, NHibernateUtil.Int32));
                IList<DATHouse> houseList = db.DB.GetCustomSQLQueryList<DATHouse>(sql, parameters).ToList();
                // .GetListCustom<DATHouse>(
                //(Expression<Func<DATHouse, bool>>)
                //(tbl =>
                //    tbl.BuildingId == buildingId && tbl.CityID == cityId&&tbl.FloorNo==floorNo&&tbl.Valid==1
                //)).ToList<DATHouse>();
                db.Close();
                return houseList;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }

        /// <summary>
        /// 根据房号ID获取房号信息
        /// </summary>
        /// <param name="houseId"></param>
        /// <param name="cityId"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static DATHouse GetHouseByHouseId(int houseId, int cityId, DataBase _db = null)
        {
            DataBase db = new DataBase(_db);
            try
            {
                string sql = "{0} HouseId=:houseId and CityID=:cityId and Valid=1";
                sql = string.Format(sql, NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_DATHouse));
                List<NHParameter> parameters = new List<NHParameter>();
                parameters.Add(new NHParameter("houseId", houseId, NHibernateUtil.Int32));
                parameters.Add(new NHParameter("cityId", cityId, NHibernateUtil.Int32));
                DATHouse house = db.DB.GetCustomSQLQueryEntity<DATHouse>(sql, parameters);

                //DATHouse house = db.DB.GetCustom<DATHouse>(
                //   (Expression<Func<DATHouse, bool>>)
                //   (tbl =>
                //       tbl.HouseId == houseId && tbl.CityID == cityId&&tbl.Valid==1
                //   ));
                db.Close();
                return house;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }
        public static IList<DATHouse> GetHouseByHouseIds(int[] houseIds, int cityId, DataBase _db = null)
        {
            if (houseIds == null || houseIds.Length < 1)
            {
                return new List<DATHouse>();
            }
            DataBase db = new DataBase(_db);
            try
            {
                string sql = "{0} CityID=:cityId and  HouseId in ({1}) ";
                sql = string.Format(sql, NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_DATHouse), houseIds.ConvertToString());

                List<NHParameter> parameters = new List<NHParameter>();
                parameters.Add(new NHParameter("cityId", cityId, NHibernateUtil.Int32));
                IList<DATHouse> list = db.DB.GetCustomSQLQueryList<DATHouse>(sql, parameters);

                //IList<DATHouse> list = db.DB.CreateCriteria(typeof(DATHouse)).Add(
                //    Restrictions.And(
                //    Restrictions.Eq("CityID", cityId),
                //    Restrictions.In("HouseId", houseIds))
                //    ).List<DATHouse>();
                db.Close();
                return list;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }
        /// <summary>
        /// 根据楼盘获取所有房号信息
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static IList<DATHouse> GetHouseByProjectId(int projectId, int cityId, DataBase _db = null)
        {
            DataBase db = new DataBase(_db);
            try
            {
                string sql = "{0} cityId=:cityId and buildingId in (select buildingId from {1} with(nolock) where projectid=:projectId and cityId=:cityId)";
                sql = string.Format(sql, NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_DATHouse), NHibernateUtility.TableName_DATBuilding);

                List<NHParameter> parameters = new List<NHParameter>();
                parameters.Add(new NHParameter("cityId", cityId, NHibernateUtil.Int32));
                parameters.Add(new NHParameter("projectId", projectId, NHibernateUtil.Int32));
                IList<DATHouse> list = db.DB.GetCustomSQLQueryList<DATHouse>(sql, parameters);
                db.Close();
                return list;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }

        #region 更新
        /// <summary>
        /// 批量删除房号
        /// </summary>
        /// <param name="list"></param>
        /// <param name="cityId"></param>
        /// <param name="_db"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static bool DeleteByHouseIds(IList<DATHouse> list, int cityId, DataBase _db = null, ITransaction transaction = null)
        {
            if (list == null)
            {
                return true;
            }
            DataBase db = new DataBase(_db);
            try
            {
                StringBuilder sb = new StringBuilder();
                foreach (DATHouse house in list)
                {
                    sb.Append(house.HouseId).Append(",");
                }
                string sql = string.Format("update {0} set  Valid=0 where HouseId in ({1}) and CityId = {2} ",
                    NHibernateUtility.TableName_DATHouse, sb.ToString().TrimEnd(','), cityId);
                db.DB.Update(sql, transaction);
                db.Close();
                return true;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }
        /// <summary>
        /// 批量删除房号-删除非指定ID的房号
        /// </summary>
        /// <param name="list"></param>
        /// <param name="cityId"></param>
        /// <param name="_db"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static bool DeleteByNotHouseIds(IList<DATHouse> list, int buildingId, int cityId, DataBase _db = null, ITransaction transaction = null)
        {
            if (list == null)
            {
                return true;
            }
            DataBase db = new DataBase(_db);
            try
            {
                StringBuilder sb = new StringBuilder();
                foreach (DATHouse house in list)
                {
                    sb.Append(house.HouseId).Append(",");
                }
                string sql = string.Format("update {0} set  Valid=0 where  CityId = {1} and BuildingId={2} and HouseId not in ({3}) ",
                    NHibernateUtility.TableName_DATHouse, cityId, buildingId, sb.ToString().TrimEnd(','));
                db.DB.Update(sql, transaction);
                db.Close();
                return true;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }

        /// <summary>
        /// 修改房号信息
        /// </summary>
        /// <param name="houseId"></param>
        /// <param name="cityId"></param>
        /// <param name="userName"></param>
        /// <param name="houseObj"></param>
        /// <param name="message"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static int UpdateHouseInfo(int houseId, int cityId, string userName, JObject houseObj, out string message, DataBase _db = null)
        {

            DateTime nowTime = DateTime.Now;
            DataBase db = new DataBase(_db);
            message = "";
            //更新house值
            DATHouse house = DATHouseManager.GetHouseByHouseId(houseId, cityId, _db: db);

            if (house == null)
            {
                db.Close();
                message = "房号不存在或已被删除";
                return 0;
            }
            try
            {
                foreach (var _prop in houseObj)
                {
                    string key = _prop.Key;
                    var property = house.GetType().GetProperties()
                             .Where(pInfo => pInfo.Name.ToLower().Equals(key.ToLower())).FirstOrDefault();
                    if (property == null)
                    {
                        continue;
                    }
                    object _value = CommonUtility.valueType(property.PropertyType, _prop.Value.Value<JValue>().Value, true);
                    property.SetValue(house, _value, null);
                }
                house.SaveDateTime = nowTime;//最后修改时间
                house.SaveUser = userName;//修改人

                DATBuilding building = DATBuildingManager.GetBuildingByBuildingId(house.BuildingId, cityId, db);
                if (building.TotalFloor.HasValue && house.EndFloorNo.HasValue && house.EndFloorNo > building.TotalFloor)
                {
                    db.Close();
                    message = "房号终止楼层不能大于楼栋总层数";
                    return 0;
                }
                else if (house.EndFloorNo.HasValue && house.EndFloorNo < house.FloorNo)
                {
                    db.Close();
                    message = "起始楼层不能大于终止楼层";
                    return 0;
                }else if(house.EndFloorNo.HasValue && (house.EndFloorNo < -5 || house.FloorNo < -5))
                {
                    db.Close();
                    message = "房号的起始楼层和终止楼层不能小于-5";
                    return 0;
                }
                else
                {
                    db.DB.Update(house);
                }
            }
            catch (Exception ex)
            {
                db.Close();
                message = "系统异常";
                log.Error("修改房号时失败,houseId:" + houseId, ex);
                return -1;
            }
            db.Close();
            return 1;
        }


        #endregion

        #region commom
        /// <summary>
        /// 根据单元字段分隔单元号和室号
        /// </summary>
        /// <param name="unitNoStr"></param>
        /// <param name="unitno"></param>
        /// <param name="houseno"></param>
        public static void GetHouseUnitNoAndHouseNo(string unitNoStr, out string unitno, out string houseno)
        {
            unitno = "";
            houseno = "";
            if (unitNoStr != null && unitNoStr.Contains("$"))
            {
                unitno = unitNoStr.Split('$')[0];
                houseno = unitNoStr.Split('$')[1];
            }
            else
            {
                unitno = unitNoStr;
            }
        }
        /// <summary>
        /// 获取单元号
        /// </summary>
        /// <param name="unitNoStr"></param>
        /// <returns></returns>
        public static string GetUnitNoByUnitNoStr(string unitNoStr)
        {
            string unitno = "";
            string houseno = "";
            GetHouseUnitNoAndHouseNo(unitNoStr, out unitno, out houseno);
            return unitno;
        }
        /// <summary>
        /// 获取室号
        /// </summary>
        /// <param name="unitNoStr"></param>
        /// <returns></returns>
        public static string GetHouseNoByUnitNoStr(string unitNoStr)
        {
            string unitno = "";
            string houseno = "";
            GetHouseUnitNoAndHouseNo(unitNoStr, out unitno, out houseno);
            return houseno;
        }
        /// <summary>
        /// 单元号和室号组成单元字段
        /// </summary>
        /// <param name="unitno"></param>
        /// <param name="houseno"></param>
        /// <returns></returns>
        public static string SetHouseUnitNoAndHouseNo(string unitno, string houseno)
        {
            string unitNoStr = "{0}${1}";
            return string.Format(unitNoStr, unitno, houseno);

        }
        #endregion
    }
}
