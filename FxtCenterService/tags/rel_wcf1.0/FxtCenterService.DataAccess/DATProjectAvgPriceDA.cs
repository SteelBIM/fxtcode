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
    public class DATProjectAvgPriceDA : Base
    {
        /// <summary>
        /// 建筑类型及面积段分类均价表
        /// </summary>
        /// <param name="fxtcompanyid"></param>
        /// <param name="cityid"></param>
        /// <param name="projectid">楼盘ID</param>
        /// <param name="purposetype">楼盘类型（普通住宅：1002001）</param>
        /// <param name="startdate">案例日期</param>
        /// <param name="enddate">案例日期</param>
        /// <param name="onlyhasprice">是否只获取均价大于0的数据</param>
        /// <param name="daterange">计算范围</param>
        /// <returns></returns>
        public static List<DATProjectAvgPrice> GetProjectAvgPriceList(int fxtcompanyid, int cityid, int projectid, string purposetype, DateTime startdate, DateTime enddate, bool onlyhasprice,int daterange)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            string sql = SQL.Project.ProjectAvgPrice;

            parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", fxtcompanyid, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@cityid", cityid, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@projectid", projectid, SqlDbType.Int));
            //均价表日期格式为年月字符        
            parameters.Add(SqlHelper.GetSqlParameter("@startdate", startdate.ToString("yyyy-MM"), SqlDbType.VarChar));
            parameters.Add(SqlHelper.GetSqlParameter("@enddate", enddate.ToString("yyyyMM"), SqlDbType.VarChar));
            string where = "";
            if (!string.IsNullOrEmpty(purposetype))
            {
                where += "  and purposetype in (" + purposetype + ")";
            }
            if (onlyhasprice)
            {
                where += " and avgprice > 0";
            }
            //计算范围
            sql += " and daterange=@daterange";
            parameters.Add(SqlHelper.GetSqlParameter("@daterange", daterange, SqlDbType.Int));

            sql = sql.Replace("#where#", where);
            return ExecuteToEntityList<DATProjectAvgPrice>(sql, CommandType.Text, parameters);
        }

        /// <summary>
        /// 均价走势图 caoq 2014-1-20
        /// </summary>
        /// <param name="fxtcompanyid"></param>
        /// <param name="cityid">城市ID</param>
        /// <param name="areaid">行政区ID</param>
        /// <param name="purposetype">楼盘类型（普通住宅：1002001）</param>
        /// <param name="projectid">楼盘ID</param>
        /// <param name="buildingareatype">面积段CODE</param>
        /// <param name="buildingtypecode">建筑类型Code</param>
        /// <param name="startdate">起始日期</param>
        /// <param name="enddate">结束日期</param>
        /// <param name="daterange">计算范围</param>
        /// <returns>avgtype=1 楼盘,avgtype=2 行政区,avgtype=3 城市</returns>
        public static DataSet GetAvgPriceTrend(int fxtcompanyid, int cityid, int areaid, int projectid, int purposetype, int buildingareatype, int buildingtypecode, DateTime startdate, DateTime enddate, int daterange)
        {
            string sql = SQL.Project.AvgPriceTrend;

            SqlCommand command = new SqlCommand();
            command.CommandText = sql;
            command.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", fxtcompanyid, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", cityid, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@purposetype", purposetype, SqlDbType.Int));

            command.Parameters.Add(SqlHelper.GetSqlParameter("@areaid", areaid, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@projectid", projectid, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@buildingareatype", buildingareatype, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@buildingtypecode", buildingtypecode, SqlDbType.Int));

            //command.Parameters.Add(SqlHelper.GetSqlParameter("@startdate", startdate.ToString("yyyy-MM-dd"), SqlDbType.DateTime));
            //command.Parameters.Add(SqlHelper.GetSqlParameter("@enddate", enddate.ToString("yyyy-MM-dd"), SqlDbType.DateTime));
            //均价表日期格式为年月字符  
            command.Parameters.Add(SqlHelper.GetSqlParameter("@startdate", startdate.ToString("yyyyMM"), SqlDbType.VarChar));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@enddate", enddate.ToString("yyyyMM"), SqlDbType.VarChar));
            //价格计算范围
            command.Parameters.Add(SqlHelper.GetSqlParameter("@daterange", daterange, SqlDbType.Int));

            return ExecuteDataSet(command);
        }


        /// <summary>
        /// 周边楼盘价格、环比涨跌幅
        /// </summary>
        /// <param name="cityid"></param>
        /// <param name="fxtcompanyid"></param>
        /// <param name="projectid"></param>
        /// <param name="projectname"></param>
        /// <param name="surveyx"></param>
        /// <param name="surveyy"></param>
        /// <param name="purposetype"></param>
        /// <param name="avgdate">均价日期</param>
        /// <param name="daterange">计算范围</param>
        /// <returns>dataset(column:projectid,projectname,avgprice,preavgprice,changepercent,projectx,projecty)</returns>
        public static DataSet GetMapPrice(int cityid, int fxtcompanyid, int projectid, string projectname, double surveyx, double surveyy, int purposetype, DateTime avgdate, int daterange)
        {
            SqlCommand cmd = new SqlCommand();

            string sql = SQL.Project.GetMapPrice;
            CityTable city = CityTableDA.GetCityTable(cityid);
            sql = sql.Replace("@projecttable", city.ProjectTable);
            sql = sql.Replace("@projectsubtable", city.ProjectTable + "_sub");

            sql = sql.Replace("@@x", surveyx.ToString());
            sql = sql.Replace("@@y", surveyy.ToString());

            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", cityid, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", fxtcompanyid, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@projectid", projectid, SqlDbType.Int));

            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@preavgdate", avgdate.AddMonths(-1).ToString("yyyyMM"), SqlDbType.VarChar));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@avgdate", avgdate.ToString("yyyyMM"), SqlDbType.VarChar));

            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@purposetype", purposetype, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@projectname", projectname, SqlDbType.VarChar));
            //价格计算范围
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@daterange", daterange, SqlDbType.Int));

            cmd.CommandText = sql;
            return ExecuteDataSet(cmd);
        }


    }
}
