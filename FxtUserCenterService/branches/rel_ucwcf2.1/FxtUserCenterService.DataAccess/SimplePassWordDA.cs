using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using CAS.DataAccess.BaseDAModels;
using System.Data;

namespace FxtUserCenterService.DataAccess
{
    public class SimplePassWordDA : Base
    {
        /// <summary>
        /// 修改用户密码 hody 2014-07-25
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static int CheckIsSimplePassWord(string simplePassWord)
        {
            SqlCommand cmd = new SqlCommand();
            string sql = SQLName.SimplePassWord.CheckIsSimplePassWord;
            cmd.CommandText = sql;
            List<SqlParameter> parameters = new List<SqlParameter>();
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@simplepassword", simplePassWord, SqlDbType.VarChar));
            return (int)ExecuteScalar(cmd);
        }
    }

}
