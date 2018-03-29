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

namespace FxtCenterService.DataAccess
{
    public class SYSCityDA : Base
    {
        /// <summary>
        /// 获取cityid设置获取案例月份数
        /// </summary>
        /// <param name="cityid"></param>
        /// <returns></returns>
        public static int GetCityCaseMonth(int cityid)
        {
            SqlCommand command = new SqlCommand();
            string sql = SQL.CityArea.CityCaseMonth;
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            command.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", cityid, SqlDbType.Int));
            return StringHelper.TryGetInt(ExecuteScalar(command).ToString());
        }
        /// <summary>
        /// 获取城市列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="provinceid">省份ID</param>
        /// <returns></returns>
        public static List<SYSCity> GetSYSCityList(SearchBase search, int provinceid)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.CityArea.CityList;
            if (provinceid > 0)
            {
                sql += " and provinceid=" + provinceid;
            }
            sql = HandleSQL(search, sql);
            return ExecuteToEntityList<SYSCity>(sql, System.Data.CommandType.Text, parameters);
        }
    }
}
