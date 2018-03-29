using FxtDataAcquisition.Common;
using FxtDataAcquisition.Data;
using FxtDataAcquisition.DTODomain.NHibernate;
using FxtDataAcquisition.NHibernate.Entities;
using log4net;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.BLL
{
    public static class SYSRoleMenuFunctionManager
    {
        //
        public static readonly ILog log = LogManager.GetLogger(typeof(SYSRoleMenuFunctionManager));

        /// <summary>
        /// 根据城市+企业ID+用户+页面url,获取其具有的操作项
        /// </summary>
        /// <param name="cityId">城市</param>
        /// <param name="companyId">企业ID</param>
        /// <param name="userName">用户</param>
        /// <param name="pageUrl">页面url</param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static IList<SYSRoleMenuFunction> GetSYSRoleMenuFunctionByUserNameAndCompanyIdAndCityIdAndUrl(int cityId, int companyId, string userName, string pageUrl, DataBase _db = null)
        {
            if (string.IsNullOrEmpty(pageUrl) || string.IsNullOrEmpty(userName))
            {
                return new List<SYSRoleMenuFunction>();
            }
            DataBase db = new DataBase(_db);
            try
            {
                string sql = new StringBuilder("{0} cityId in (:cityId,0) ")
                               .Append(" and fxtCompanyId in(:companyId,0) ")
                               .Append(" and Valid=1 and RoleMenuId in (")
                                .Append("   select Id from {1}  where ")
                               .Append(" cityId in (:cityId,0) and fxtCompanyId in (:companyId,0) ")
                                .Append("   and ")
                                .Append(" RoleId in ( ")
                               .Append(" select RoleId from {2} where userName=:userName and cityId in (:cityId,0) and fxtCompanyId in (:companyId,0) ")
                                  .Append(" ) ")
                                  .Append(" and menuId in (select Id from {3} where url=:pageUrl) ")
                                .Append(")").ToString();
                sql = string.Format(sql, NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_SYSRoleMenuFunction),
                    NHibernateUtility.TableName_SYSRoleMenu, NHibernateUtility.TableName_SYSRoleUser, NHibernateUtility.TableName_SYSMenu);
                List<NHParameter> parameters = new List<NHParameter>();
                parameters.Add(new NHParameter("cityId", cityId, NHibernateUtil.Int32));
                parameters.Add(new NHParameter("companyId", companyId, NHibernateUtil.Int32));
                parameters.Add(new NHParameter("userName", userName, NHibernateUtil.String));
                parameters.Add(new NHParameter("pageUrl", pageUrl.ToLower(), NHibernateUtil.String));

                IList<SYSRoleMenuFunction> list = db.DB.GetCustomSQLQueryList<SYSRoleMenuFunction>(sql, parameters);
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
        /// 根据城市+企业ID+用户+页面url+指定的操作项,获取其具有的操作项
        /// </summary>
        /// <param name="cityId">城市</param>
        /// <param name="companyId">企业ID</param>
        /// <param name="userName">用户</param>
        /// <param name="pageUrl">页面url</param>
        /// <param name="functionCodes">指定的操作项codes</param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static IList<SYSRoleMenuFunction> GetSYSRoleMenuFunctionByUserNameAndCompanyIdAndCityIdAndUrlAndFunctionCodes(int cityId, int companyId, string userName, string pageUrl, int[] functionCodes, DataBase _db = null)
        {
            if (string.IsNullOrEmpty(pageUrl) || string.IsNullOrEmpty(userName) || functionCodes == null || functionCodes.Length < 1)
            {
                return new List<SYSRoleMenuFunction>();
            }
            DataBase db = new DataBase(_db);
            try
            {
                string sql = new StringBuilder("{0} cityId in (:cityId,0) ")
                               .Append(" and fxtCompanyId in(:companyId,0) ")
                               .Append(" and FunctionCode in ({1}) ")
                               .Append(" and Valid=1 and RoleMenuId in ( ")
                                .Append(" select Id from {2} with(nolock)  where ")
                                 .Append(" cityId in (:cityId,0) and fxtCompanyId in (:companyId,0) ")
                                  .Append(" and ")
                                 .Append(" RoleId in ( ")
                                    .Append(" select RoleId from {3} with(nolock) where userName=:userName and cityId in (:cityId,0) and fxtCompanyId in (:companyId,0) ")
                                  .Append(" ) ")
                                  .Append(" and menuId in (select Id from {4} with(nolock) where url=:pageUrl) ")
                               .Append(" )").ToString();
                sql = string.Format(sql, NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_SYSRoleMenuFunction),
                 functionCodes.ConvertToString(), NHibernateUtility.TableName_SYSRoleMenu, NHibernateUtility.TableName_SYSRoleUser, NHibernateUtility.TableName_SYSMenu);
                List<NHParameter> parameters = new List<NHParameter>();
                parameters.Add(new NHParameter("cityId", cityId, NHibernateUtil.Int32));
                parameters.Add(new NHParameter("companyId", companyId, NHibernateUtil.Int32));
                parameters.Add(new NHParameter("userName", userName, NHibernateUtil.String));
                parameters.Add(new NHParameter("pageUrl", pageUrl.ToLower(), NHibernateUtil.String));

                IList<SYSRoleMenuFunction> list = db.DB.GetCustomSQLQueryList<SYSRoleMenuFunction>(sql, parameters);
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
