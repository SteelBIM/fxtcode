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
	public class DATHouseDA : Base 
	{
		public static int Add(DATHouse model)
		{
			return InsertFromEntity<DATHouse>(model);
		}
		public static int Update(DATHouse model)
		{
			return UpdateFromEntity<DATHouse>(model);
		}
		//批量更新
		public static int UpdateMul(DATHouse model,int[] ids)
		{
			return UpdateFromIds<DATHouse>(model,ids);
		}
		public static int Delete(int id)
		{
			return DeleteByPrimaryKey<DATHouse>(id);
		}
		public static DATHouse GetDATHouseByPK(int id)
		{
			return ExecuteToEntityByPrimaryKey<DATHouse>(id);
		}
		public static List<DATHouse> GetDATHouseList(SearchBase search, string key)
		{
			List<SqlParameter> parameters = new List<SqlParameter>();
			if (!string.IsNullOrEmpty(key))
			{
				search.Where += " and <search field> like @key escape '$'";
				parameters.Add(SqlHelper.GetSqlParameter("@key", "%" + SQLFilterHelper.EscapeLikeString(key, "$") + "%", SqlDbType.NVarChar, <search field length>));
			}
			string sql = SQL.DATHouse.DATHouseList;
			sql = HandleSQL(search, sql);
			return ExecuteToEntityList<DATHouse>(sql, System.Data.CommandType.Text, parameters);
		}
	}
}
