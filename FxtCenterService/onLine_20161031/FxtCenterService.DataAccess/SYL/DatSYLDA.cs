using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using CAS.DataAccess.BaseDAModels;
using CAS.Entity.FxtDataCenter;

namespace FxtCenterService.DataAccess
{
    public class DatSYLDA : Base
    {
        public static List<SYLDat> GetSYLList(int cityId)
        {
            string sql = SQL.SYL.GetSYLDat;
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlHelper.GetSqlParameter("@cityid", cityId, SqlDbType.Int));
            return ExecuteToEntityList<SYLDat>(sql, System.Data.CommandType.Text, parameters);
        }
    }
}
