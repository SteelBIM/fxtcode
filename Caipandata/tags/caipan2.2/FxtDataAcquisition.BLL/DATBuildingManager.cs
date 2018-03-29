using FxtDataAcquisition.Common;
using FxtDataAcquisition.Data;
using FxtDataAcquisition.DTODomain.NHibernate;
using FxtDataAcquisition.NHibernate.Entities;
using log4net;
using Newtonsoft.Json.Linq;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace FxtDataAcquisition.BLL
{
    public static class DATBuildingManager
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(DATBuildingManager));
        #region (查询)
        /// <summary>
        /// 根据楼盘ID获取楼栋列表
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static IList<DATBuilding> GetBuildingByProjectId(int projectId, int cityId, DataBase _db = null)
        {
            DataBase db = new DataBase(_db);
            try
            {
                string sql = "{0} ProjectId = :projectId and CityID = :cityId and Valid=1";
                sql = string.Format(sql, NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_DATBuilding));
                List<NHParameter> parameters = new List<NHParameter>();
                parameters.Add(new NHParameter("projectId", projectId, NHibernateUtil.Int32));
                parameters.Add(new NHParameter("cityId", cityId, NHibernateUtil.Int32));
                IList<DATBuilding> buildingList = db.DB.GetCustomSQLQueryList<DATBuilding>(sql, parameters).ToList();
                //  GetListCustom<DATBuilding>(
                //(Expression<Func<DATBuilding, bool>>)
                //(tbl =>
                //    tbl.ProjectId == projectId && tbl.CityID == cityId&&tbl.Valid==1
                //)).ToList<DATBuilding>();
                db.Close();
                return buildingList;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }
        /// <summary>
        /// 根据楼栋ID获取楼栋信息
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="cityId"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static DATBuilding GetBuildingByBuildingId(int buildingId, int cityId, DataBase _db = null)
        {
            DataBase db = new DataBase(_db);
            try
            {
                string sql = "{0} BuildingId=:buildingId and CityID=:cityId and Valid=1";
                sql = string.Format(sql, NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_DATBuilding));
                List<NHParameter> parameters = new List<NHParameter>();
                parameters.Add(new NHParameter("buildingId", buildingId, NHibernateUtil.Int32));
                parameters.Add(new NHParameter("cityId", cityId, NHibernateUtil.Int32));
                DATBuilding building = db.DB.GetCustomSQLQueryEntity<DATBuilding>(sql, parameters);
                //.GetCustom<DATBuilding>(
                //  (Expression<Func<DATBuilding, bool>>)
                //  (tbl =>
                //      tbl.BuildingId == buildingId && tbl.CityID == cityId && tbl.Valid == 1
                //  ));
                db.Close();
                return building;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }

        #endregion

        #region (更新)

        /// <summary>
        /// 批量删除楼栋
        /// </summary>
        /// <param name="list"></param>
        /// <param name="cityId"></param>
        /// <param name="_db"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static bool DeleteByBuildingIds(IList<DATBuilding> list, int cityId, DataBase _db = null, ITransaction transaction = null)
        {
            if (list == null)
            {
                return true;
            }
            DataBase db = new DataBase(_db);
            try
            {
                StringBuilder sb = new StringBuilder();
                foreach (DATBuilding building in list)
                {
                    sb.Append(building.BuildingId).Append(",");
                }
                string sql = string.Format("update {0} set  Valid=0 where BuildingId in ({1}) and CityId = {2} ",
                    NHibernateUtility.TableName_DATBuilding, sb.ToString().TrimEnd(','), cityId);
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
        /// 批量删除楼栋-删除非指定ID的楼栋
        /// </summary>
        /// <param name="buildingIds"></param>
        /// <param name="cityId"></param>
        /// <param name="_db"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static bool DeleteByNotBuildingIds(int[] buildingIds, int projectId, int cityId, DataBase _db = null, ITransaction transaction = null)
        {
            if (buildingIds == null || buildingIds.Length < 1)
            {
                return true;
            }
            DataBase db = new DataBase(_db);
            try
            {
                StringBuilder sb = new StringBuilder();
                foreach (int building in buildingIds)
                {
                    sb.Append(building).Append(",");
                }
                string sql = string.Format("update {0} set  Valid=0 where  CityId = {1} and ProjectId={2} and BuildingId not in ({3})  ",
                  NHibernateUtility.TableName_DATBuilding, cityId, projectId, sb.ToString().TrimEnd(','));
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
        /// 修改楼栋信息
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="cityId"></param>
        /// <param name="userName"></param>
        /// <param name="buildingObj"></param>
        /// <param name="message"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static int UpdateBuildingInfo(int buildingId, int cityId, string userName, JObject buildingObj, out string message, DataBase _db = null)
        {

            DateTime nowTime = DateTime.Now;
            DataBase db = new DataBase(_db);
            message = "";
            //更新Building值
            DATBuilding building = DATBuildingManager.GetBuildingByBuildingId(buildingId, cityId, _db: db);
            if (building == null)
            {
                db.Close();
                message = "楼栋不存在或已被删除";
                return 0;
            }
            try
            {
                foreach (var _prop in buildingObj)
                {
                    string key = _prop.Key;
                    var property = building.GetType().GetProperties()
                             .Where(pInfo => pInfo.Name.ToLower().Equals(key.ToLower())).FirstOrDefault();
                    if (property == null)
                    {
                        continue;
                    }
                    object _value = CommonUtility.valueType(property.PropertyType, _prop.Value.Value<JValue>().Value, true);
                    property.SetValue(building, _value, null);
                }
                building.SaveDateTime = nowTime;//最后修改时间
                building.SaveUser = userName;//修改人
                db.DB.Update(building);
            }
            catch (Exception ex)
            {
                db.Close();
                message = "系统异常";
                log.Error("修改楼栋时失败,buildingId:" + buildingId, ex);
                return -1;
            }
            db.Close();
            return 1;
        }

        #endregion
    }
}
