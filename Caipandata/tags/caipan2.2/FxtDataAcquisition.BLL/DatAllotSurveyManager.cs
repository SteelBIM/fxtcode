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

namespace FxtDataAcquisition.BLL
{
    public static class DatAllotSurveyManager
    {
        #region (查询)
        /// <summary>
        /// 根据状态ID和多个任务ID获取任务状态记录信息
        /// </summary>
        /// <param name="cityId">城市</param>
        /// <param name="allotIds">逗号分隔的多个任务ID</param>
        /// <param name="stateCode">状态</param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static IList<DatAllotSurvey> GetDatAllotSurveyByAllotIdsAndStateCode(int cityId, long[] allotIds, int stateCode, DataBase _db = null)
        {
            if (allotIds == null || allotIds.Length < 1)
            {
                return new List<DatAllotSurvey>();
            }
            DataBase db = new DataBase(_db);
            try
            {

                string sql = "{0} CityId=:cityId and  StateCode=:stateCode and AllotId in ({1}) ";
                sql = string.Format(sql, NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_DatAllotSurvey), allotIds.ConvertToString());

                List<NHParameter> parameters = new List<NHParameter>();
                parameters.Add(new NHParameter("cityId", cityId, NHibernateUtil.Int32));
                parameters.Add(new NHParameter("stateCode", stateCode, NHibernateUtil.Int32));

                IList<DatAllotSurvey> list = db.DB.GetCustomSQLQueryList<DatAllotSurvey>(sql, parameters);

                //IList<DatAllotSurvey> list = db.DB.CreateCriteria(typeof(DatAllotSurvey)).Add(
                //    Restrictions.And(
                //    Restrictions.Eq("CityId", cityId),
                //    Restrictions.Eq("StateCode", stateCode))
                //    ).Add(
                //    Restrictions.In("AllotId", allotIds)
                //    ).List<DatAllotSurvey>();
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
        /// 根据状态ID和任务ID获取任务状态记录信息
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="allotId"></param>
        /// <param name="stateCode"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static IList<DatAllotSurvey> GetDatAllotSurveyByAllotIdAndStateCode(int cityId, long allotId, int stateCode, DataBase _db = null)
        {
            DataBase db = new DataBase(_db);
            try
            {
                string sql = "{0} AllotId=:allotId and CityId=:cityId and StateCode=:stateCode";
                sql = string.Format(sql, NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_DatAllotSurvey));
                List<NHParameter> parameters = new List<NHParameter>();
                parameters.Add(new NHParameter("allotId", allotId, NHibernateUtil.Int64));
                parameters.Add(new NHParameter("cityId", cityId, NHibernateUtil.Int32));
                parameters.Add(new NHParameter("stateCode", stateCode, NHibernateUtil.Int32));
                IList<DatAllotSurvey> list = db.DB.GetCustomSQLQueryList<DatAllotSurvey>(sql, parameters).ToList();

                //IList<DatAllotSurvey> list = db.DB.GetListCustom<DatAllotSurvey>
                //    ((Expression<Func<DatAllotSurvey, bool>>)
                //    (tbl => tbl.AllotId == allotId && tbl.CityId == cityId && tbl.StateCode == stateCode)).ToList<DatAllotSurvey>();
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
        /// 根据状态ID和任务ID获取最新一条任务状态记录信息
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="allotId"></param>
        /// <param name="stateCode"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static DatAllotSurvey GetDatAllotSurveyLastByAllotIdAndStateCode(int cityId, long allotId, int stateCode, DataBase _db = null)
        {
            DataBase db = new DataBase(_db);
            try
            {
                string sql = "{0} AllotId=:allotId and CityId=:cityId and StateCode=:stateCode Order By Id desc";
                sql = string.Format(sql, NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_DatAllotSurvey, keyword: "top 1"));
                List<NHParameter> parameters = new List<NHParameter>();
                parameters.Add(new NHParameter("allotId", allotId, NHibernateUtil.Int64));
                parameters.Add(new NHParameter("cityId", cityId, NHibernateUtil.Int32));
                parameters.Add(new NHParameter("stateCode", stateCode, NHibernateUtil.Int32));
                IList<DatAllotSurvey> list = db.DB.GetCustomSQLQueryList<DatAllotSurvey>(sql, parameters).ToList();
                db.Close();
                if (list == null || list.Count < 1)
                {
                    return null;
                }
                return list[0];
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }
        /// <summary>
        /// 根据状态ID和多个任务ID获取每个任务最新一条任务状态记录信息
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="allotIds"></param>
        /// <param name="stateCode"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static IList<DatAllotSurvey> GetDatAllotSurveyLastByAllotIdAndStateCode(int cityId, long[] allotIds, int stateCode, DataBase _db = null)
        {
            if (allotIds == null || allotIds.Length < 1)
            {
                return new List<DatAllotSurvey>();
            }
            DataBase db = new DataBase(_db);
            try
            {
                string sql = string.Format("{0} AllotId in ({1}) and CityId=:cityId and StateCode=:stateCode  and ID=(select max(ID) from {2} with(nolock) where allotId=_tb.allotId and statecode=_tb.statecode and cityId=_tb.cityId)",
                    NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_DatAllotSurvey),
                    allotIds.ConvertToString(), NHibernateUtility.TableName_DatAllotSurvey);
                List<NHParameter> parameters = new List<NHParameter>();
                parameters.Add(new NHParameter("cityId", cityId, NHibernateUtil.Int32));
                parameters.Add(new NHParameter("stateCode", stateCode, NHibernateUtil.Int32));
                IList<DatAllotSurvey> list = db.DB.GetCustomSQLQueryList<DatAllotSurvey>(sql, parameters).ToList();
                db.Close();
                return list;
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
        /// 任务状态记录表插入信息
        /// </summary>
        /// <param name="allotId">任务ID</param>
        /// <param name="cityId">城市ID</param>
        /// <param name="fxtcompanyId">当前机构ID</param>
        /// <param name="userName">当前用户名</param>
        /// <param name="stateCode">当前状态ID</param>
        /// <param name="stateDate">当前状态更改时间</param>
        /// <param name="_db"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static bool InsertAllotSurvey(long allotId, int cityId, int fxtcompanyId, string userName, int stateCode, DateTime stateDate, DataBase _db = null, ITransaction tran = null)
        {
            DataBase db = new DataBase(_db);
            try
            {
                DatAllotSurvey allotSurvey = GetDatAllotSurveyInsertEntitie(allotId, cityId, fxtcompanyId, userName, stateCode, stateDate);
                db.DB.Create(allotSurvey, tran);
                db.Close();
                return true;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }
        #endregion

        #region common
        /// <summary>
        /// 获取插入状态的实体
        /// </summary>
        /// <param name="allotId"></param>
        /// <param name="cityId"></param>
        /// <param name="fxtcompanyId"></param>
        /// <param name="userName"></param>
        /// <param name="stateCode"></param>
        /// <param name="stateDate"></param>
        /// <param name="remark">说明</param>
        /// <returns></returns>
        public static DatAllotSurvey GetDatAllotSurveyInsertEntitie(long allotId, int cityId, int fxtcompanyId, string userName, int stateCode, DateTime stateDate, string remark = null)
        {
            DatAllotSurvey allotSurvey = new DatAllotSurvey
            {
                AllotId = allotId,
                CityId = cityId,
                FxtCompanyId = fxtcompanyId,
                UserName = userName,
                CreateDate = DateTime.Now,
                StateCode = stateCode,
                StateDate = stateDate,
                Remark = remark
            };
            return allotSurvey;
        }
        #endregion
    }
}
