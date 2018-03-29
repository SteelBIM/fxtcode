using FxtDataAcquisition.Common;
using FxtDataAcquisition.Data;
using FxtDataAcquisition.DTODomain.NHibernate;
using FxtDataAcquisition.NHibernate.Entities;
using log4net;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace FxtDataAcquisition.BLL
{
    public static class SYSRoleUserManager
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(SYSRoleUserManager));
        /// <summary>
        /// 获取用户所属的角色信息
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="companyId"></param>
        /// <param name="userName"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static IList<SYSRoleUser> GetSYSRoleUserByUserName(int cityId, int companyId, string userName, DataBase _db = null)
        {
            DataBase db = new DataBase(_db);
            try
            {
                string sql = "{0} CityID in(0,:cityId) and FxtCompanyID in(0,:companyId) and UserName=:userName";
                sql = string.Format(sql, NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_SYSRoleUser));
                List<NHParameter> parameters = new List<NHParameter>();
                parameters.Add(new NHParameter("companyId", companyId, NHibernateUtil.Int32));
                parameters.Add(new NHParameter("cityId", cityId, NHibernateUtil.Int32));
                parameters.Add(new NHParameter("userName", userName, NHibernateUtil.String));
                IList<SYSRoleUser> list = db.DB.GetCustomSQLQueryList<SYSRoleUser>(sql, parameters).ToList();
                //.GetListCustom<SYSRoleUser>(
                //(Expression<Func<SYSRoleUser, bool>>)(tbl => tbl.CityID == cityId && tbl.FxtCompanyID == companyId && tbl.UserName == userName)
                //).ToList();
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
        /// 删除用户的所有角色
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="companyId"></param>
        /// <param name="userName"></param>
        /// <param name="_db"></param>
        /// <param name="tx"></param>
        /// <returns></returns>
        public static bool DeleteByUserName(int cityId, int companyId, string userName, DataBase _db = null, ITransaction tx = null)
        {
            DataBase db = new DataBase(_db);
            try
            {
                string sql = string.Format("delete {0} where CityID in(0,{1}) and FxtCompanyID in(0,{2}) and UserName='{3}'", NHibernateUtility.TableName_SYSRoleUser, cityId, companyId, userName);
                bool result = db.DB.DeleteBySQL(sql, tx);
                db.Close();
                return result;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }
        /// <summary>
        /// 删除用户指定角色
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="companyId"></param>
        /// <param name="userName"></param>
        /// <param name="roleIds">要删除的角色ID,多个角色ID组成的数据</param>
        /// <param name="_db"></param>
        /// <param name="tx"></param>
        /// <returns></returns>
        public static bool DeleteByUserName(int cityId, int companyId, string userName, int[] roleIds, DataBase _db = null, ITransaction tx = null)
        {
            if (roleIds == null || roleIds.Length < 1)
            {
                return true;
            }
            DataBase db = new DataBase(_db);
            try
            {
                string sql = string.Format("delete {0} where CityID in(0,{1}) and FxtCompanyID in(0,{2}) and RoleID in ({3}) and UserName='{4}'", NHibernateUtility.TableName_SYSRoleUser, cityId, companyId, roleIds.ConvertToString(), userName);
                bool result = db.DB.DeleteBySQL(sql, tx);
                db.Close();
                return result;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }
        /// <summary>
        /// 给用户设置角色
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="companyId"></param>
        /// <param name="userName"></param>
        /// <param name="message"></param>
        /// <param name="roleIds"></param>
        /// <param name="_db"></param>
        /// <param name="tx"></param>
        /// <returns></returns>
        public static bool SetRoleUser(int cityId, int companyId, string userName, string truename, out string message, int[] roleIds, DataBase _db = null, ITransaction tx = null)
        {
            message = "";
            bool result = true;
            DataBase db = new DataBase(_db);
            TransactionHelper th = new TransactionHelper(db.DB, tx);
            try
            {
                IList<SYSRoleUser> roleUserlists = SYSRoleUserManager.GetSYSRoleUserByUserName(cityId, companyId, userName, db);
                //如果为删除所有
                if (roleIds == null || roleIds.Length < 1)
                {
                    if (!SYSRoleUserManager.DeleteByUserName(cityId, companyId, userName, db, tx))
                    {
                        message = "删除所有角色异常";
                        result = false;
                        th.Rollback();
                        goto end;
                    }
                    goto resultend;
                }
                IList<SYSRoleUser> roleUserList = SYSRoleUserManager.GetSYSRoleUserByUserName(cityId, companyId, userName, db);
                IList<SYSRoleUser> addUserList = new List<SYSRoleUser>();//存储要新增的role
                List<int> delList = new List<int>();//存储要删除的role
                //获取要删除的role
                IList<SYSRoleUser> delUserList = roleUserList.Where(obj => !roleIds.Contains(obj.RoleID)).ToList();
                foreach (SYSRoleUser roleUser in delUserList)
                {
                    delList.Add(roleUser.RoleID);
                }
                //获取要新增的role
                foreach (int roleId in roleIds)
                {
                    int existsCount = roleUserList.Where(obj => obj.RoleID == roleId).Count();
                    int existsCount2 = addUserList.Where(obj => obj.RoleID == roleId).Count();
                    if (existsCount < 1 && existsCount2 < 1)
                    {
                        var r = roleUserlists.Where(m => m.RoleID == roleId && m.UserName == userName).FirstOrDefault();
                        if (r != null)
                        {
                            SYSRoleUser roleUser = new SYSRoleUser { CityID = r.CityID, FxtCompanyID = r.FxtCompanyID, RoleID = roleId, UserName = userName, TrueName = truename };
                            addUserList.Add(roleUser);
                        }
                        else
                        {
                            SYSRoleUser roleUser = new SYSRoleUser { CityID = cityId, FxtCompanyID = companyId, RoleID = roleId, UserName = userName, TrueName = truename };
                            //SYSRoleUser roleUser = new SYSRoleUser { CityID = cityId, FxtCompanyID = companyId, RoleID = roleId, UserName = userName, TrueName = truename };
                            addUserList.Add(roleUser);
                        }
                    }
                }
                if (!SYSRoleUserManager.DeleteByUserName(cityId, companyId, userName, delList.ToArray(), db, tx))
                {
                    message = "删除指定角色异常";
                    result = false;
                    th.Rollback();
                    goto end;
                }
                if (!db.DB.Create<SYSRoleUser>(addUserList, tx))
                {
                    message = "新增角色异常";
                    result = false;
                    th.Rollback();
                    goto end;
                }

            resultend:
                th.Commit();
            }
            catch (Exception ex)
            {

                th.Rollback();
                message = "系统异常";
                log.Error(message, ex);
                result = false;
            }
        end:
            db.Close();
            return result;
        }
    }
}
