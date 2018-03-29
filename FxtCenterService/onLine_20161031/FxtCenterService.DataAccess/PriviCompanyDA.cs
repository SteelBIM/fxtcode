using CAS.Entity.FxtProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using CAS.DataAccess.BaseDAModels;
using System.Data;

namespace FxtCenterService.DataAccess
{
    public class PriviCompanyDA : Base
    {
        public static Privi_Company GetPriviCompanyForVQ(string name, string companycode)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.PriviCompany.GetPriviCompanyByName;
            parameters.Add(SqlHelper.GetSqlParameter("@name", name, SqlDbType.VarChar));
            parameters.Add(SqlHelper.GetSqlParameter("@companycode", companycode, SqlDbType.VarChar));
            return ExecuteToEntity<Privi_Company>(sql, System.Data.CommandType.Text, parameters);
        }
        public static int AddPriviCompanyForVQ(Privi_Company model)
        {
            return InsertFromEntity<Privi_Company>(model);
        }
    }
}
