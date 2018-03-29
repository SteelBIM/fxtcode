using FxtDataAcquisition.Data;
using FxtDataAcquisition.DTODomain.NHibernate;
using FxtDataAcquisition.NHibernate.Entities;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace FxtDataAcquisition.BLL
{
    public static class PriviDepartmentUserManager
    {
        /// <summary>
        /// 获取当前用户的小组信息
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="companyId"></param>
        /// <param name="userName"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static PriviDepartmentUser GetDepartmentUserByUserName(int cityId, int companyId, string userName, DataBase _db = null)
        {
            DataBase db = new DataBase(_db);
            try
            {
                string sql = "{0} FxtCompanyID=:companyId and CityId=:cityId and UserName=:userName and exists  (select * from {1} as tb2 with(nolock) where tb2.DepartmentID=_tb.DepartmentID and DValid=1)";
                sql = string.Format(sql, NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_PriviDepartmentUser), NHibernateUtility.TableName_PriviDepartment);
                List<NHParameter> parameters = new List<NHParameter>();
                parameters.Add(new NHParameter("companyId", companyId, NHibernateUtil.Int32));
                parameters.Add(new NHParameter("cityId", cityId, NHibernateUtil.Int32));
                parameters.Add(new NHParameter("userName", userName, NHibernateUtil.String));
                PriviDepartmentUser pdu = db.DB.GetCustomSQLQueryEntity<PriviDepartmentUser>(sql, parameters);
                //PriviDepartmentUser pdu = db.DB.GetCustom<PriviDepartmentUser>(
                //     (Expression<Func<PriviDepartmentUser, bool>>)(tbl => tbl.CityID == cityId && tbl.FxtCompanyID == companyId && tbl.UserName == userName)
                //    );
                db.Close();
                return pdu;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }
        /// <summary>
        /// 删除当前用户的小组信息
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
                string sql = string.Format("delete {0} where CityID={1} and FxtCompanyID={2} and UserName='{3}'", NHibernateUtility.TableName_PriviDepartmentUser, cityId, companyId, userName);
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
    }
}
