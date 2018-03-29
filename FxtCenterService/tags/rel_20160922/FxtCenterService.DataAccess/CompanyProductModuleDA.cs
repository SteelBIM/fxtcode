using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.DataAccess.DA;
using CAS.Common;
using System.Data.SqlClient;
using System.Data;
using CAS.DataAccess.BaseDAModels;
using CAS.Entity;
using CAS.Entity.DBEntity;
using CAS.Entity.FxtUserCenter;

namespace FxtCenterService.DataAccess
{
    public class CompanyProductModuleDA : Base
    {
        //public static int IsAllowCompanyProductModule(SearchBase search, int module)
        //{
        //    SqlCommand comm = new SqlCommand();

        //    string sql = SQL.CompanyProductModule.GetCompanyProductModule;
        //    comm.CommandText = sql;
        //    comm.Parameters.Add(SqlHelper.GetSqlParameter("@companyid", search.FxtCompanyId, SqlDbType.Int));
        //    comm.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));
        //    comm.Parameters.Add(SqlHelper.GetSqlParameter("@producttypecode", search.SysTypeCode, SqlDbType.Int));
        //    comm.Parameters.Add(SqlHelper.GetSqlParameter("@parentmodulecode", 0, SqlDbType.Int));
        //    comm.Parameters.Add(SqlHelper.GetSqlParameter("@modulecode", module, SqlDbType.Int));
        //    return StringHelper.TryGetInt(ExecuteScalar(comm).ToString());
        //}

        //public static CompanyProduct IsCompanyProductCity(int conpanyId, int systypecode, int cityId)
        //{
        //    string sql = SQL.CompanyProductModule.GetCompanyProductCity;
        //    List<SqlParameter> pas = new List<SqlParameter>();
        //    pas.Add(SqlHelper.GetSqlParameter("@companyid", conpanyId, SqlDbType.Int));
        //    pas.Add(SqlHelper.GetSqlParameter("@cityid", cityId, SqlDbType.Int));
        //    pas.Add(SqlHelper.GetSqlParameter("@producttypecode", systypecode, SqlDbType.Int));

        //    var list = ExecuteToEntityList<CompanyProduct>(sql, CommandType.Text, pas);
        //    return list.FirstOrDefault();
        //}
    }
}
