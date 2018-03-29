using FxtDataAcquisition.Data;
using FxtDataAcquisition.NHibernate.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Criterion;
using System.Linq.Expressions;
using FxtDataAcquisition.DTODomain.NHibernate;
using NHibernate;
using FxtDataAcquisition.Common;

namespace FxtDataAcquisition.BLL
{
    public static class LNKPAppendageManager
    {
        /// <summary>
        /// 根据多个楼盘ID+城市ID获取配套信息
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="projectIds"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static IList<LNKPAppendage> GetLNKPAppendageByProjectIds(int cityId, int[] projectIds, DataBase _db = null)
        {
            if (projectIds == null || projectIds.Length < 1)
            {
                return new List<LNKPAppendage>();
            }
            DataBase db = new DataBase(_db);

            try
            {
                string sql = "{0} CityId=:cityId and  ProjectId in ({1}) ";
                sql = string.Format(sql, NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_LNKPAppendage), projectIds.ConvertToString());

                List<NHParameter> parameters = new List<NHParameter>();
                parameters.Add(new NHParameter("cityId", cityId, NHibernateUtil.Int32));
                IList<LNKPAppendage> list = db.DB.GetCustomSQLQueryList<LNKPAppendage>(sql, parameters);
                //IList<LNKPAppendage> list = db.DB.CreateCriteria(typeof(LNKPAppendage)).Add(
                //    Restrictions.And(
                //    Restrictions.In("ProjectId", projectIds),
                //    Restrictions.Eq("CityId", cityId))
                //    ).List<LNKPAppendage>();
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
        /// 根据楼盘ID+城市ID获取配套信息
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="projectId"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static IList<LNKPAppendage> GetLNKPAppendageByProjectId(int cityId, int projectId, DataBase _db = null)
        {
            DataBase db = new DataBase(_db);
            try
            {
                string sql = "{0} ProjectId=:projectId and CityId=:cityId";
                sql = string.Format(sql, NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_LNKPAppendage));
                List<NHParameter> parameters = new List<NHParameter>();
                parameters.Add(new NHParameter("projectId", projectId, NHibernateUtil.Int32));
                parameters.Add(new NHParameter("cityId", cityId, NHibernateUtil.Int32));
                IList<LNKPAppendage> list = db.DB.GetCustomSQLQueryList<LNKPAppendage>(sql, parameters).ToList();
                //.GetListCustom<LNKPAppendage>
                //    ((Expression<Func<LNKPAppendage, bool>>)
                //    (tbl => tbl.ProjectId == projectId && tbl.CityId == cityId)).ToList<LNKPAppendage>();
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
        /// 根据多个配套code获取楼盘配套信息
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="projectId"></param>
        /// <param name="appendageCodes"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static IList<LNKPAppendage> GetLNKPAppendageByProjectIdAndAppendageCodes(int cityId, int projectId, int[] appendageCodes, DataBase _db = null)
        {
            if (appendageCodes == null || appendageCodes.Length < 1)
            {
                return new List<LNKPAppendage>();
            }
            DataBase db = new DataBase(_db);
            try
            {
                string sql = "{0} CityId=:cityId and ProjectId=:projectId and  AppendageCode in ({1}) ";
                sql = string.Format(sql, NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_LNKPAppendage), appendageCodes.ConvertToString());

                List<NHParameter> parameters = new List<NHParameter>();
                parameters.Add(new NHParameter("cityId", cityId, NHibernateUtil.Int32));
                parameters.Add(new NHParameter("projectId", projectId, NHibernateUtil.Int32));
                IList<LNKPAppendage> list = db.DB.GetCustomSQLQueryList<LNKPAppendage>(sql, parameters);
                //IList<LNKPAppendage> list = db.DB.CreateCriteria(typeof(LNKPAppendage)).Add(
                //    Restrictions.And(
                //    Restrictions.Eq("ProjectId", projectId),
                //    Restrictions.Eq("CityId", cityId))
                //    ).Add(Restrictions.In("AppendageCode", appendageCodes))
                //    .List<LNKPAppendage>();
                db.Close();
                return list;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }

        }


    }
}
