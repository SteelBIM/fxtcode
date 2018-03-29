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
    public class DATAvgPriceMonthDA : Base
    {
        /// <summary>
        /// 价格走势
        /// </summary>
        /// <param name="search"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static DataSet GetDATAvgPriceMonthList(SearchBase search, int projectid)
		{
            SqlCommand cmd = new SqlCommand();
            string sql = SQL.Project.ProjectTrend;
            sql += " and Projectid = @projectid and AvgPriceDate between @avgDateBegin and @avgDateEnd And cityid=@cityid order by AvgPriceDate asc";
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@avgDateBegin", search.DateBegin, SqlDbType.NVarChar, 30));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@avgDateEnd", search.DateEnd, SqlDbType.NVarChar, 30));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@projectid", projectid, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));

            cmd.CommandText = sql;
            return ExecuteDataSet(cmd);
		}

        /// <summary>
        /// 价格走势
        /// </summary>
        /// <param name="topcnt"></param>
        /// <param name="cityid"></param>
        /// <param name="areaid"></param>
        /// <param name="projectid"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public static DataSet GetDATAvgPriceMonthList(int topcnt,int cityid, int areaid, int projectid,DateTime startdate,DateTime enddate)
        {
            SqlCommand cmd = new SqlCommand();
            string sql = SQL.Project.ProjectTrend;
            sql += " and Projectid = @projectid and AvgPriceDate between @avgDateBegin and @avgDateEnd order by AvgPriceDate asc";
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@avgDateBegin", startdate, SqlDbType.DateTime));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@avgDateEnd", enddate, SqlDbType.DateTime));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@projectid", projectid, SqlDbType.Int));

            cmd.CommandText = sql;
            return ExecuteDataSet(cmd);
        }

        /// <summary>
        /// 获取城市，行政区均价走势（不区分类型）
        /// </summary>
        /// <param name="topcnt"></param>
        /// <param name="cityid"></param>
        /// <param name="areaid"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public static DataSet GetCityAreaAvgPriceTrend(int topcnt, int cityid, int areaid, DateTime startdate, DateTime enddate)
        {
            SqlCommand cmd = new SqlCommand();
            string sql = SQL.Project.CityAreaAvgPriceTrend;
            sql = sql.Replace("@topcnt", topcnt.ToString());
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", cityid, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@areaid", areaid, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@startdate", startdate, SqlDbType.DateTime));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@enddate", enddate, SqlDbType.DateTime));

            cmd.CommandText = sql;
            return ExecuteDataSet(cmd);
        }
        /// <summary>
        /// 行政区均价（不区分类型）
        /// 创建人:曾智磊，2014-08-05
        /// </summary>
        /// <param name="topcnt">最多取几条</param>
        /// <param name="cityid"></param>
        /// <param name="areaid"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public static DataSet GetAreaAvgPriceTrend(int topcnt, int cityid, int areaid, DateTime startdate, DateTime enddate)
        {
            SqlCommand cmd = new SqlCommand();
            string sql = SQL.Project.AreaAvgPriceTrend;
            sql = sql.Replace("@topcnt", topcnt.ToString());
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", cityid, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@areaid", areaid, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@startdate", startdate, SqlDbType.DateTime));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@enddate", enddate, SqlDbType.DateTime));

            cmd.CommandText = sql;
            return ExecuteDataSet(cmd);
        }


        
    }
}
