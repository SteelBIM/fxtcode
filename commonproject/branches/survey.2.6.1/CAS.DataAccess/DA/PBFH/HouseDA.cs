using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using CAS.DataAccess.BaseDAModels;
using CAS.Entity;
using CAS.Common;

// Project - Building - Floor - House 联动功能 kevin 2013-3-21
namespace CAS.DataAccess.DA.PBFH
{
    public class HouseDA : Base
    {

        /// <summary>
        /// 获取房号下拉列表
        /// </summary>
        /// <returns></returns>
        public static DataSet GetHouseDropDownList(SearchBase search, int buildingId, int floorno)
        {
            string sql = SQL.PBFH.HouseDropDownList;
            string houseTable = TableByCity.housetable;
            SqlCommand cmd = new SqlCommand();
            try
            {
                sql = sql.Replace("@table_house", houseTable);
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@buildingid", buildingId, SqlDbType.Int));
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@floorno", floorno, SqlDbType.Int));
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", WebCommon.LoginInfo.fxtcompanyid, SqlDbType.Int));
                sql = HandleSQL(search, sql);
                cmd.CommandText = sql;
                return ExecuteDataSet(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获得房号详细信息
        /// </summary>
        /// <returns></returns>
        public static DataSet GetHouseBaseInfo(SearchBase search, int houseId)
        {
            string sql = SQL.PBFH.HouseBaseInfo;
            string houseTable = TableByCity.housetable;
            SqlCommand cmd = new SqlCommand();
            try
            {
                sql = sql.Replace("@table_house", houseTable);
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", WebCommon.LoginInfo.fxtcompanyid, SqlDbType.Int));
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@houseid", houseId, SqlDbType.Int));
                sql = HandleSQL(search, sql);
                cmd.CommandText = sql;
                return ExecuteDataSet(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
