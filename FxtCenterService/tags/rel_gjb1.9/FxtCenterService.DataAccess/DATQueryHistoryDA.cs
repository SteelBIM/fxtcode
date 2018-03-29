using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using CAS.Common;
using CAS.Entity.DBEntity;
using CAS.DataAccess.BaseDAModels;
using CAS.Entity.FxtProject;

namespace FxtCenterService.DataAccess
{
    public class DATQueryHistoryDA : Base
    {
        public static int Add(DATQueryHistory model)
        {
            return InsertFromEntity<DATQueryHistory>(model);
        }
        public static int Update(DATQueryHistory model)
        {
            return UpdateFromEntity<DATQueryHistory>(model);
        }
        //批量更新
        public static int UpdateMul(DATQueryHistory model, int[] ids)
        {
            return UpdateFromIds<DATQueryHistory>(model, ids);
        }
        public static int Delete(int id)
        {
            return DeleteByPrimaryKey<DATQueryHistory>(id);
        }
        public static DATQueryHistory GetDATQueryHistoryByPK(int id)
        {
            return ExecuteToEntityByPrimaryKey<DATQueryHistory>(id);
        }
        /// <summary>
        /// 获取自动估价记录
        /// </summary>
        /// <param name="search"></param>
        /// <param name="username">账号</param>
        /// <returns></returns>
        public static List<DATQueryHistory> GetDATQueryHistoryList(SearchBase search, string username, string wxopenid)
		{
			List<SqlParameter> parameters = new List<SqlParameter>();
			string sql = SQL.Project.GetQueryHistoryList;
            if (string.IsNullOrEmpty(wxopenid)) { wxopenid = ""; }
            parameters.Add(SqlHelper.GetSqlParameter("@wxopenid", wxopenid, SqlDbType.NVarChar));
            parameters.Add(SqlHelper.GetSqlParameter("@username", username, SqlDbType.NVarChar));
            search.OrderBy = " QueryDate desc";
			sql = HandleSQL(search, sql);
			return ExecuteToEntityList<DATQueryHistory>(sql, System.Data.CommandType.Text, parameters);
		}


        /// <summary>
        /// 标准化楼盘楼栋房号匹配
        /// </summary>
        /// <param name="cityid">城市Id</param>
        /// <param name="projectname">楼盘名称</param>
        /// <param name="addresss">地址</param>
        /// <param name="buildingname">楼栋名称</param>
        /// <param name="housename">房号名称</param>
        /// <returns></returns>
        public static DataSet GetMatchingData(int cityid, string projectname, string addresss,
            string buildingname, string housename, int p_projectid, int p_buildingid, int fxtcompanyid)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlCommand cmd = new SqlCommand();
            string sql = SQL.Project.GetMatchingData;
            CityTable cityTb = CityTableDA.GetCityTable(cityid);
            sql = sql.Replace("@table_project", cityTb.ProjectTable);
            sql = sql.Replace("@table_building", cityTb.BuildingTable);
            sql = sql.Replace("@table_house", cityTb.HouseTable);
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@projectid", p_projectid, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@buildingid", p_buildingid, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", cityid, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", fxtcompanyid, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@projectname", projectname, SqlDbType.NVarChar, 200));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@address", addresss, SqlDbType.NVarChar, 200));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@buildingname", buildingname, SqlDbType.NVarChar, 200));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@housename", housename, SqlDbType.NVarChar, 200));
            return ExecuteDataSet(cmd);
        }


    }

}
