using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity;
using System.Data.SqlClient;
using CAS.DataAccess.BaseDAModels;
using System.Data;
using CAS.Entity.DBEntity;
using CAS.Common;

namespace CAS.DataAccess.DA.User
{
    /// <summary>
    /// 用户登录退出相关DA
    /// </summary>
    public class LoginDA:Base
    {
        /// <summary>
        /// 云查勘手机登录
        /// </summary>
        /// <param name="search"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static PriviUser LoginBySurveyMobile(SearchBase search, string password)
        {
            try
            {
                string sql = SQL.User.LoginBySurveyMobile.ToLower();
                SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@systypecode", search.SysTypeCode, SqlDbType.Int));
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@userid", search.UserId, SqlDbType.NVarChar));
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@password", password, SqlDbType.NVarChar));
                cmd.CommandType = CommandType.Text;                
                return ExecuteToEntity<PriviUser>(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
