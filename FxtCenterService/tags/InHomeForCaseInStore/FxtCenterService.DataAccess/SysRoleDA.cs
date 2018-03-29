using CAS.DataAccess.BaseDAModels;
using CAS.Entity.FxtUserCenter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace FxtCenterService.DataAccess
{
    public class SysRoleDA : Base
    {
        public static List<SysRoleUser> GetSysRoleUserIds(string userName, int fxtCompanyId, int sysTypeCode)
        {
            string sql = SQL.Sys.GetRoleUser;
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlHelper.GetSqlParameter("@UserName", userName, SqlDbType.NVarChar));
            parameters.Add(SqlHelper.GetSqlParameter("@FxtCompanyId", fxtCompanyId, SqlDbType.Int));

            return ExecuteToEntityList<SysRoleUser>(sql, CommandType.Text, parameters);
        }
    }
}
