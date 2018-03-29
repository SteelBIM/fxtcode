using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using CAS.Entity;
using CAS.DataAccess.BaseDAModels;
using CAS.Entity.DBEntity;
using CAS.Common;

// Project - Building - Floor - House 联动功能 kevin 2013-3-21
namespace CAS.DataAccess.DA.PBFH
{
    public class BuildingDA : Base
    {
        /// <summary>
        /// 楼栋-获取楼栋下拉列表
        /// </summary>
        /// <returns></returns>
        public static List<DATBuilding> GetBuildingDropDownList(SearchBase search, int projectId)
        {

            string sql = SQL.PBFH.BuildingDropDownList;
            SqlCommand cmd = new SqlCommand();
            try
            {
                string buildingTable = TableByCity.buildingtable;
                sql = sql.Replace("@buildingtable", buildingTable);
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@projectid", projectId, SqlDbType.Int));
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
                sql = HandleSQL(search, sql);
                cmd.CommandText = sql;
                return ExecuteToEntityList<DATBuilding>(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 楼栋-获取楼栋基本信息
        /// </summary>
        /// <returns></returns>
        public static DataSet GetBuildingBaseInfo(SearchBase search, int buildingId)
        {
            string buildingTable = TableByCity.buildingtable;
            string sql = SQL.PBFH.BuildingBaseInfo;
            SqlCommand cmd = new SqlCommand();
            try
            {
                sql = sql.Replace("@table_building", buildingTable);
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@buildingid", buildingId, SqlDbType.Int));
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyId", search.FxtCompanyId, SqlDbType.Int));
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
