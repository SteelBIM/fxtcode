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
using CAS.Entity.FxtProject;
using System.Diagnostics;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace FxtCenterService.DataAccess
{
    public class DatProjectBizDA : Base
    {
        /// <summary>
        /// 获取商业街列表
        /// </summary>
        public static DataTable GetListBiz(SearchBase search, string key)
        {
            string sql = SQL.ProjectBiz.ProjectBizList;           
            string topSql = search.Top > 0 ? " top " + search.Top.ToString() : "";
            sql = sql.Replace("@top", topSql);
            SqlCommand cmd = new SqlCommand();
            if (!string.IsNullOrEmpty(key))
            {
                key = "%" + key + "%";
                sql = sql.Replace("$keylimit","and (ProjectName like @key or OtherName like @key or Address like @key or PinYin like @key or PinYinAll like @key)");
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@key", key, SqlDbType.NVarChar, 81));
            }
            else
            {
                sql = sql.Replace("$keylimit", "");
            }
            sql = HandleSQL(search, sql);
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
            cmd.CommandText = sql;
            return ExecuteDataSet(cmd).Tables[0];
        }

        /// <summary>
        /// 获取商业楼栋列表
        /// </summary>
        public static DataTable GetBuildingListBiz(SearchBase search, int projectId, string key)
        {
            string sql = SQL.ProjectBiz.BuildingBizList;
            string topSql = search.Top > 0 ? " top " + search.Top.ToString() : "";
            sql = sql.Replace("@top", topSql);
            SqlCommand cmd = new SqlCommand();
            if (!string.IsNullOrEmpty(key))
            {
                key = "%" + key + "%";
                sql = sql.Replace("$keylimit", "and (BuildingName like @key or Address like @key or PinYin like @key or PinYinAll like @key)");
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@key", key, SqlDbType.NVarChar, 81));
            }
            else
            {
                sql = sql.Replace("$keylimit", "");
            }
            sql = HandleSQL(search, sql);
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@projectid", projectId, SqlDbType.Int));
            cmd.CommandText = sql;
            return ExecuteDataSet(cmd).Tables[0];
        }

        /// <summary>
        /// 获取商业楼栋列表
        /// </summary>
        public static DataTable GetFloorListBiz(SearchBase search, int buildingid, string key)
        {
            string sql = SQL.ProjectBiz.FloorBizList;
            SqlCommand cmd = new SqlCommand();
            if (!string.IsNullOrEmpty(key))
            {
                key = "%" + key + "%";
                sql = sql.Replace("$keylimit", "and FloorNo like @key ");
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@key", key, SqlDbType.NVarChar, 81));
            }
            else
            {
                sql = sql.Replace("$keylimit", "");
            }
            sql = HandleSQL(search, sql);
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@buildingid", buildingid, SqlDbType.Int));
            cmd.CommandText = sql;
            return ExecuteDataSet(cmd).Tables[0];
        }

        /// <summary>
        /// 获取商业楼栋列表
        /// </summary>
        public static DataTable GetHouseListBiz(SearchBase search,int floorId, string key)
        {
            string sql = SQL.ProjectBiz.HouseBizList;
            SqlCommand cmd = new SqlCommand();
            if (!string.IsNullOrEmpty(key))
            {
                key = "%" + key + "%";
                sql = sql.Replace("$keylimit", "and HouseName like @key ");
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@key", key, SqlDbType.NVarChar, 81));
            }
            else
            {
                sql = sql.Replace("$keylimit", "");
            }
            sql = HandleSQL(search, sql);
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@floorId", floorId, SqlDbType.Int));
            cmd.CommandText = sql;
            return ExecuteDataSet(cmd).Tables[0];
        }
    }
}
