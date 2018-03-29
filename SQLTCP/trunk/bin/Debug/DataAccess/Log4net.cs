using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using CAS.Common;
using CAS.Entity.DBEntity;
using CAS.DataAccess.BaseDAModels;

namespace CAS.DataAccess
{
	public class Log4netDA : Base 
	{
		public static int Add(Log4net model)
		{
			return InsertFromEntity<Log4net>(model);
		}
		public static int Update(Log4net model)
		{
			return UpdateFromEntity<Log4net>(model);
		}
		//批量更新
		public static int UpdateMul(Log4net model,int[] ids)
		{
			return UpdateFromIds<Log4net>(model,ids);
		}
		public static int Delete(int id)
		{
			return DeleteByPrimaryKey<Log4net>(id);
		}
		public static Log4net GetLog4netByPK(int id)
		{
			return ExecuteToEntityByPrimaryKey<Log4net>(id);
		}
		public static List<Log4net> GetLog4netList(SearchBase search, string key)
		{
			List<SqlParameter> parameters = new List<SqlParameter>();
			if (!string.IsNullOrEmpty(key))
			{
				search.Where += " and <search field> like @key escape '$'";
				parameters.Add(SqlHelper.GetSqlParameter("@key", "%" + SQLFilterHelper.EscapeLikeString(key, "$") + "%", SqlDbType.NVarChar, <search field length>));
			}
			string sql = SQL.Log4net.Log4netList;
			sql = HandleSQL(search, sql);
			return ExecuteToEntityList<Log4net>(sql, System.Data.CommandType.Text, parameters);
		}
	}
}
