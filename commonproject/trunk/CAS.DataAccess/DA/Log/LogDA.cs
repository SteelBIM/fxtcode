using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.DataAccess.BaseDAModels;
using CAS.Entity;
using System.Data.SqlClient;
using CAS.Common;
using System.Data;

namespace CAS.DataAccess.DA.Log
{
    public class LogDA:Base
    {
        static LogDA()
        {
            SetConnectionName<SYSLog>(System.Configuration.ConfigurationManager.ConnectionStrings["FxtLogConnectionName"].ConnectionString);
        }
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static long Add(SYSLog model)
        {   
            return InsertFromEntityAndReturnLongId<SYSLog>(model);
        }
        public static int Update(SYSLog model)
        {
            
            return UpdateFromEntity<SYSLog>(model);
        }
        public static int Delete(long id)
        {
            
            return DeleteByPrimaryKey<SYSLog>(id);
        }
        public static SYSLog GetSYSLogByPK(long id)
        {
            
            return ExecuteToEntityByPrimaryKey<SYSLog>(id);
        }
        public static List<SYSLog> GetSYSLogList(SearchBase search, int eventtype, int logtype, string key)
		{
			List<SqlParameter> parameters = new List<SqlParameter>();
			if (!string.IsNullOrEmpty(key))
			{
                search.Where += " and (a.userid like @key escape '$' or a.username like @key escape '$' or a.objectname like @key escape '$' or c.companyname like @key escape '$')";
				parameters.Add(SqlHelper.GetSqlParameter("@key", "%" + SQLFilterHelper.EscapeLikeString(key, "$") + "%", SqlDbType.NVarChar, 300));
			}
            if (search.CityId > 0)
            {
                search.Where += " and a.cityid = @cityid";
                parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));
            }
            if (search.CompanyId > 0)
            {
                search.Where += " and c.CompanyId = @CompanyId";
                parameters.Add(SqlHelper.GetSqlParameter("@CompanyId", search.CompanyId, SqlDbType.Int));
            }
            if (search.FxtCompanyId != 0)
            {
                search.Where += " and a.FxtCompanyId = @FxtCompanyId";
                parameters.Add(SqlHelper.GetSqlParameter("@FxtCompanyId", search.FxtCompanyId, SqlDbType.Int));
            }
            if (eventtype > 0) {
                search.Where += " and eventtype = @eventtype";
                parameters.Add(SqlHelper.GetSqlParameter("@eventtype", eventtype, SqlDbType.Int));
            }
            if (logtype > 0)
            {
                search.Where += " and logtype = @logtype";
                parameters.Add(SqlHelper.GetSqlParameter("@logtype", logtype, SqlDbType.Int));
            }
            
			string sql = SQL.Log.LogList;
			sql = HandleSQL(search, sql);            
			return ExecuteToEntityList<SYSLog>(sql, System.Data.CommandType.Text, parameters);
		}
    }
}
