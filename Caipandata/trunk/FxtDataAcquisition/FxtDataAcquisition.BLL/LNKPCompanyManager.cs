using FxtDataAcquisition.Data;
using FxtDataAcquisition.NHibernate.Entities;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using FxtDataAcquisition.DTODomain.NHibernate;
using NHibernate;
using FxtDataAcquisition.Common;

namespace FxtDataAcquisition.BLL
{
    public static class LNKPCompanyManager
    {
        public static IList<LNKPCompany> GetLNKPCompanyByProjectIds(int cityId, int[] projectIds, DataBase _db = null)
        {
            if (projectIds == null || projectIds.Length < 1)
            {
                return new List<LNKPCompany>();
            }
            DataBase db = new DataBase(_db);
            try
            {
                string sql = "{0} CityId=:cityId and ProjectId in ({1}) ";
                sql = string.Format(sql, NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_LNKPCompany), projectIds.ConvertToString());

                List<NHParameter> parameters = new List<NHParameter>();
                parameters.Add(new NHParameter("cityId", cityId, NHibernateUtil.Int32));
                IList<LNKPCompany> list = db.DB.GetCustomSQLQueryList<LNKPCompany>(sql, parameters);

                //IList<LNKPCompany> list = db.DB.CreateCriteria(typeof(LNKPCompany)).Add(
                //    Restrictions.And(
                //    Restrictions.In("LNKPCompanyPX.ProjectId", projectIds),
                //    Restrictions.Eq("LNKPCompanyPX.CityId", cityId))
                //    ).List<LNKPCompany>();
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
        /// 根据楼盘ID获取管理公司
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="projectId"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static IList<LNKPCompany> GetLNKPCompanyByProjectId(int cityId, int projectId, DataBase _db = null)
        {

            DataBase db = new DataBase(_db);
            try
            {
                string sql = "{0} ProjectId=:projectId and CityId=:cityId";
                sql = string.Format(sql, NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_LNKPCompany));
                List<NHParameter> parameters = new List<NHParameter>();
                parameters.Add(new NHParameter("projectId", projectId, NHibernateUtil.Int32));
                parameters.Add(new NHParameter("cityId", cityId, NHibernateUtil.Int32));
                IList<LNKPCompany> list = db.DB.GetCustomSQLQueryList<LNKPCompany>(sql, parameters).ToList();
                //GetListCustom<LNKPCompany>
                //    ((Expression<Func<LNKPCompany, bool>>)
                //    (tbl => tbl.LNKPCompanyPX.ProjectId == projectId && tbl.LNKPCompanyPX.CityId == cityId)).ToList<LNKPCompany>();
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
        /// 根据多个机构类型code获取楼盘关联公司
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="projectId"></param>
        /// <param name="companyTypes"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static IList<LNKPCompany> GetLNKPCompanyByCompanyTypes(int cityId, int projectId, int[] companyTypes, DataBase _db = null)
        {
            if (companyTypes == null || companyTypes.Length < 1)
            {
                return new List<LNKPCompany>();
            }
            DataBase db = new DataBase(_db);
            try
            {
                string sql = "{0} CityId=:cityId and ProjectId=:projectId and CompanyType in ({1}) ";
                sql = string.Format(sql, NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_LNKPCompany), companyTypes.ConvertToString());

                List<NHParameter> parameters = new List<NHParameter>();
                parameters.Add(new NHParameter("cityId", cityId, NHibernateUtil.Int32));
                parameters.Add(new NHParameter("projectId", projectId, NHibernateUtil.Int32));
                IList<LNKPCompany> list = db.DB.GetCustomSQLQueryList<LNKPCompany>(sql, parameters);

                //IList<LNKPCompany> list = db.DB.CreateCriteria(typeof(LNKPCompany)).Add(
                //    Restrictions.And(
                //    Restrictions.Eq("LNKPCompanyPX.ProjectId", projectId),
                //    Restrictions.Eq("LNKPCompanyPX.CityId", cityId))
                //    ).Add(Restrictions.In("LNKPCompanyPX.CompanyType", companyTypes)).List<LNKPCompany>();
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

