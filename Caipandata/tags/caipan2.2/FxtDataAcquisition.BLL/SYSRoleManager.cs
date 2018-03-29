using FxtDataAcquisition.Common;
using FxtDataAcquisition.Data;
using FxtDataAcquisition.DTODomain.NHibernate;
using FxtDataAcquisition.NHibernate.Entities;
using log4net;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace FxtDataAcquisition.BLL
{
    public static class SYSRoleManager
    {
        //
        public static readonly ILog log = LogManager.GetLogger(typeof(SYSRoleManager));
        /// <summary>
        /// 管理员角色ID
        /// </summary>
        public static readonly int RoleId_Admin = GetSYSRoleAdminId();

        /// <summary>
        /// 获取机构下的用户列表对应的角色信息
        /// </summary>
        /// <param name="cityId">当前城市</param>
        /// <param name="companyId">当前企业</param>
        /// <param name="userNames">当前用户数组</param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static IList<SYSRole> GetSYSRoleByCityIdAndUserNames(int cityId, int companyId, string[] userNames, DataBase _db = null)
        {
            if (userNames == null || userNames.Length < 1)
            {
                return new List<SYSRole>();
            }
            DataBase db = new DataBase(_db);
            try
            {
                StringBuilder userNameSb = new StringBuilder();
                foreach (string str in userNames)
                {
                    userNameSb.Append("'").Append(str).Append("',");
                }
                string sql = string.Format(" {0} Valid=1 and FxtCompanyId in (0,{1}) and CityId in (0,{2})  and Id in (select RoleId from {3} with(nolock) where FxtCompanyId={1} and CityID={2} and UserName in ({4}))",
                NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_SYSRole),
                companyId, cityId, NHibernateUtility.TableName_SYSRoleUser, userNameSb.ToString().TrimEnd(','));
                IList<SYSRole> list = db.DB.GetCustomSQLQueryList<SYSRole>(sql, null);
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
        /// 获取结构下的角色列表
        /// </summary>
        /// <param name="cityId">哪个城市</param>
        /// <param name="companyId">当前机构ID</param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static IList<SYSRole> GetSYSRoleByCompanyId(int cityId, int companyId, DataBase _db = null)
        {
            DataBase db = new DataBase(_db);
            try
            {
                string sql = "{0} CityID in (0,:cityId) and FxtCompanyID in (0,:companyId) and Valid=1";
                sql = string.Format(sql, NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_SYSRole));
                List<NHParameter> parameters = new List<NHParameter>();
                parameters.Add(new NHParameter("cityId", cityId, NHibernateUtil.Int32));
                parameters.Add(new NHParameter("companyId", companyId, NHibernateUtil.Int32));
                IList<SYSRole> list = db.DB.GetCustomSQLQueryList<SYSRole>(sql, parameters).ToList();
                //IList<SYSRole> list = db.DB.GetListCustom<SYSRole>(
                //    (Expression<Func<SYSRole, bool>>)(tbl => (tbl.CityID == 0 || tbl.CityID == cityId) && (tbl.FxtCompanyID == 0 || tbl.FxtCompanyID == companyId) && tbl.Valid == 1)
                //    ).ToList();
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
        /// 根据多个ID获取角色信息
        /// </summary>
        /// <param name="roleIds"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static IList<SYSRole> GetSYSRoleByRoleIds(int[] roleIds, DataBase _db = null)
        {
            if (roleIds == null || roleIds.Length < 1)
            {
                return new List<SYSRole>();
            }
            DataBase db = new DataBase(_db);
            try
            {
                string sql = string.Format("{0} ID in ({1})", NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_SYSRole), roleIds.ConvertToString());
                IList<SYSRole> list = db.DB.GetCustomSQLQueryList<SYSRole>(sql, null).ToList<SYSRole>();
                // db.DB.GetListCustom<SYSRole>("ID", roleIds).ToList();
                db.Close();
                return list;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }
        public static int GetSYSRoleAdminId(DataBase _db = null)
        {
            DataBase db = new DataBase(_db);
            try
            {
                string sql = string.Format("{0} CityID=0 and FxtCompanyID=0 and Valid=1 and RoleName='管理员' ", NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_SYSRole));
                SYSRole obj = db.DB.GetCustomSQLQueryEntity<SYSRole>(sql, null);
                // db.DB.GetListCustom<SYSRole>("ID", roleIds).ToList();
                db.Close();
                if (obj == null)
                {
                    return 0;
                }
                return obj.ID;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }
    }
}
