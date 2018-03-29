using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using CAS.Common;
using CAS.Entity;
using CAS.DataAccess.BaseDAModels;
using CAS.Entity.DBEntity;


namespace FxtUserCenterService.DataAccess
{
    public class DatPhoneMessageDA : Base
    {
        public static int Add(DatPhoneMessage model)
        {
            return InsertFromEntity<DatPhoneMessage>(model);
        }
        public static int Update(DatPhoneMessage model)
        {
            return UpdateFromEntity<DatPhoneMessage>(model);
        }
        public static int Delete(int id)
        {
            return DeleteByPrimaryKey<DatPhoneMessage>(id);
        }
        public static DatPhoneMessage GetDatPhoneMessageByPK(int id)
        {
            return ExecuteToEntityByPrimaryKey<DatPhoneMessage>(id);
        }
        public static List<DatPhoneMessage> GetDatPhoneMessageList(SearchBase search, string key)
		{
			List<SqlParameter> parameters = new List<SqlParameter>();
			if (!string.IsNullOrEmpty(key))
			{
				search.Where += " and <search field> like @key escape '$'";
				parameters.Add(SqlHelper.GetSqlParameter("@key", "%" + SQLFilterHelper.EscapeLikeString(key, "$") + "%", SqlDbType.NVarChar));
			}
            string sql = "";//SQL.DatPhoneMessage.DatPhoneMessageList;
			sql = HandleSQL(search, sql);
			return ExecuteToEntityList<DatPhoneMessage>(sql, System.Data.CommandType.Text, parameters);
		}
    }
}
