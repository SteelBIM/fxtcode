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
    public static class PriviDepartmentManager
    {
        /// <summary>
        /// 获取当前机构的分组信息列表
        /// </summary>
        /// <param name="cityId">哪个城市</param>
        /// <param name="companyId">哪个结构ID</param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static IList<PriviDepartment> GetDepartmentByCompanyId(int cityId, int companyId, DataBase _db = null)
        {
            DataBase db = new DataBase(_db);
            try
            {
                string sql = string.Format("{0} FK_CityId in (0,:cityId) and Fk_CompanyId in (0,:companyId) and DValid=1",
                    NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_PriviDepartment));
                List<NHParameter> parameters = new List<NHParameter>();
                parameters.Add(new NHParameter("cityId", cityId, NHibernateUtil.Int32));
                parameters.Add(new NHParameter("companyId", companyId, NHibernateUtil.Int32));
                IList<PriviDepartment> list = db.DB.GetCustomSQLQueryList<PriviDepartment>(sql, parameters).ToList<PriviDepartment>();
                //IList<PriviDepartment> list = db.DB.GetListCustom<PriviDepartment>(
                //    (Expression<Func<PriviDepartment, bool>>)(tbl => (tbl.FK_CityId == 0 || tbl.FK_CityId == cityId) && (tbl.Fk_CompanyId == 0 || tbl.Fk_CompanyId == companyId) && tbl.DValid == 1)
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
        /// 获取小组列表
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="companyId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <param name="isGetCount"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static IList<PriviDepartment> GetDepartmentByCompanyId(int cityId, int companyId, string keyword, int pageIndex, int pageSize, out int count, bool isGetCount = true, DataBase _db = null)
        {

            DataBase db = new DataBase(_db);
            try
            {
                count = 0;
                IList<PriviDepartment> list = new List<PriviDepartment>();
                string sql = "{0} FK_CityId in (0,:cityId) and Fk_CompanyId in (0,:companyId) and DValid=1";
                sql = string.Format(sql, NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_PriviDepartment));
                List<NHParameter> parameters = new List<NHParameter>();
                parameters.Add(new NHParameter("cityId", cityId, NHibernateUtil.Int32));
                parameters.Add(new NHParameter("companyId", companyId, NHibernateUtil.Int32));
                if (!string.IsNullOrEmpty(keyword))
                {
                    sql = sql + " and DepartmentName like :keyword";
                    parameters.Add(new NHParameter("keyword", "%" + keyword + "%", NHibernateUtil.String));
                }
                UtilityPager pageInfo = new UtilityPager(pageSize: pageSize, pageIndex: pageIndex, isGetCount: isGetCount);
                list = db.DB.PagerList<PriviDepartment>(pageInfo, sql, parameters, " DepartmentId", "Desc");
                count = pageInfo.Count;
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
        /// 根据ID获取分组信息
        /// </summary>
        /// <param name="departmentId"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static PriviDepartment GetDepartmentById(int departmentId, DataBase _db = null)
        {
            DataBase db = new DataBase(_db);
            try
            {
                //db.DB.SessionLock("PriviDepartment", LockMode.None);
                string sql = string.Format("{0} DepartmentId={1}", NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_PriviDepartment), departmentId);
                PriviDepartment pd = db.DB.GetCustomSQLQueryEntity<PriviDepartment>(sql, null);
                //.GetCustom<PriviDepartment>(
                //    (Expression<Func<PriviDepartment, bool>>)(tbl => tbl.DepartmentId == departmentId)
                //    );
                db.Close();
                return pd;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }
        /// <summary>
        /// 根据多个ID获取小组信息
        /// </summary>
        /// <param name="departmentIds"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static IList<PriviDepartment> GetDepartmentByIds(int[] departmentIds, DataBase _db = null)
        {
            if (departmentIds == null || departmentIds.Length < 1)
            {
                return new List<PriviDepartment>();
            }

            DataBase db = new DataBase(_db);
            try
            {
                string sql = string.Format("{0} DepartmentId in ({1})", NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_PriviDepartment), departmentIds.ConvertToString());
                IList<PriviDepartment> list = db.DB.GetCustomSQLQueryList<PriviDepartment>(sql, null).ToList();// db.DB.GetListCustom<PriviDepartment>("DepartmentId", departmentIds).ToList();
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
        /// 获取当前用户属于的组信息
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="companyId"></param>
        /// <param name="userName"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static PriviDepartment GetDepartmentByUserName(int cityId, int companyId, string userName, DataBase _db = null)
        {
            DataBase db = new DataBase(_db);
            try
            {
                string sql = "{0} Fk_CompanyId=:companyId and FK_CityId=:cityId and DValid=1 and  exists  (select * from {1} as tb2 with(nolock) where tb2.DepartmentID=_tb.DepartmentID and UserName=:userName)";
                sql = string.Format(sql, NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_PriviDepartment), NHibernateUtility.TableName_PriviDepartmentUser);
                List<NHParameter> parameters = new List<NHParameter>();
                parameters.Add(new NHParameter("companyId", companyId, NHibernateUtil.Int32));
                parameters.Add(new NHParameter("cityId", cityId, NHibernateUtil.Int32));
                parameters.Add(new NHParameter("userName", userName, NHibernateUtil.String));
                PriviDepartment obj = db.DB.GetCustomSQLQueryEntity<PriviDepartment>(sql, parameters);
                db.Close();
                return obj;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }

        public static PriviDepartment GetDepartmentByDepartmentName(int cityId, int companyId, string departmentName, DataBase _db = null)
        {
            DataBase db = new DataBase(_db);
            try
            {
                string sql = "{0} FK_CityId in (0,:cityId) and Fk_CompanyId in (0,:companyId) and DValid=1 and DepartmentName=:departmentName";
                sql = string.Format(sql, NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_PriviDepartment));
                List<NHParameter> parameters = new List<NHParameter>();
                parameters.Add(new NHParameter("cityId", cityId, NHibernateUtil.Int32));
                parameters.Add(new NHParameter("companyId", companyId, NHibernateUtil.Int32));
                parameters.Add(new NHParameter("departmentName", departmentName, NHibernateUtil.String));
                PriviDepartment obj = db.DB.GetCustomSQLQueryEntity<PriviDepartment>(sql, parameters);
                //db.DB.GetCustom<PriviDepartment>(
                //(Expression<Func<PriviDepartment, bool>>)(tbl => (tbl.FK_CityId == 0 || tbl.FK_CityId == cityId) && (tbl.Fk_CompanyId == 0 || tbl.Fk_CompanyId == companyId) && tbl.DValid == 1 && tbl.DepartmentName == departmentName)
                //);
                db.Close();
                return obj;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }

        #region (更新)
        /// <summary>
        /// 新增小组
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="companyId"></param>
        /// <param name="departmentName"></param>
        /// <param name="message"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static PriviDepartment InsertDepartment(int cityId, int companyId, string departmentName, out string message, DataBase _db = null)
        {
            message = "";
            departmentName = departmentName.TrimBlank();
            if (string.IsNullOrEmpty(departmentName))
            {
                message = "请输入小组名";
                return null;
            }
            PriviDepartment existsPD = null;
            DataBase db = new DataBase(_db);
            try
            {
                existsPD = GetDepartmentByDepartmentName(cityId, companyId, departmentName, db);
                if (existsPD != null)
                {
                    message = "小组名称已存在";
                    db.Close();
                    return null;
                }
                existsPD = new PriviDepartment { DepartmentName = departmentName, Fk_CompanyId = companyId, FxtCompanyId = companyId, FK_CityId = cityId, DValid = 1, FK_DepTypeCode = 5005003 };
                if (!db.DB.Create(existsPD))
                {
                    existsPD = null;
                    message = "插入失败:系统异常";
                }
            }
            catch (Exception ex)
            {
                existsPD = null;
                message = "系统异常";
            }
            db.Close();
            return existsPD;
        }
        /// <summary>
        /// 修改小组
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="companyId"></param>
        /// <param name="companyId"></param>
        /// <param name="departmentId"></param>
        /// <param name="message"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static bool UpdateDepartment(int departmentId, string departmentName, out string message, DataBase _db = null)
        {
            message = "";
            departmentName = departmentName.TrimBlank();
            if (string.IsNullOrEmpty(departmentName))
            {
                message = "请输入小组名";
                return false;
            }
            bool result = true;
            DataBase db = new DataBase(_db);
            try
            {
                PriviDepartment nowPD = GetDepartmentById(departmentId, db);
                if (nowPD == null || nowPD.DValid == 0)
                {
                    message = "你所修改的小组不存在或已被删除";
                    db.Close();
                    return false;
                }
                PriviDepartment existsPD = GetDepartmentByDepartmentName(nowPD.FK_CityId, nowPD.Fk_CompanyId, departmentName, db);
                if (existsPD != null && existsPD.DepartmentId != departmentId)
                {
                    message = "小组名称已存在";
                    db.Close();
                    return false;
                }
                nowPD.DepartmentName = departmentName;
                if (!db.DB.Update(nowPD))
                {
                    result = false;
                    message = "修改失败:系统异常";
                }
            }
            catch (Exception ex)
            {
                result = false;
                message = "系统异常";
            }
            db.Close();
            return result;
        }
        /// <summary>
        /// 删除小组
        /// </summary>
        /// <param name="departmentIds"></param>
        /// <param name="message"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static bool DeleteDepartment(int[] departmentIds, out string message, DataBase _db = null)
        {
            message = "";
            bool result = true;
            if (departmentIds == null || departmentIds.Length < 1)
            {
                return true;
            }
            DataBase db = new DataBase(_db);
            IList<PriviDepartment> list = GetDepartmentByIds(departmentIds, db);
            if (list == null || list.Count < 1)
            {
                db.Close();
                return true;
            }
            for (int i = 0; i < list.Count; i++)
            {
                list[i].DValid = 0;
            }
            using (ITransaction tx = db.DB.BeginTransaction())
            {
                try
                {
                    if (!db.DB.Update<PriviDepartment>(list, tx))
                    {
                        result = false;
                        message = "删除失败:系统异常";
                        tx.Rollback();
                    }
                    else
                    {
                        tx.Commit();
                    }
                }
                catch (Exception ex)
                {
                    result = false;
                    message = "系统异常";
                    tx.Rollback();
                }
            }
            db.Close();
            return result;
        }
        #endregion

    }
}
