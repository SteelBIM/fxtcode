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
using CAS.Entity.FxtDataCenter;

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
        public static List<DATProjectAvgPrice> GetProjectAvgPriceList(int fxtcompanyid, int cityid, int projectid, string purposetype, DateTime startdate, DateTime enddate, bool onlyhasprice, int daterange, int typecode)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            string sql = SQL.Project.ProjectAvgPrice;

            parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", fxtcompanyid, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@typecode", typecode, SqlDbType.Int));
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
        public static DataSet GetAvgPriceTrend(int fxtcompanyid, int cityid, int areaid, int projectid, int purposetype, int buildingareatype, int buildingtypecode, DateTime startdate, DateTime enddate, int daterange, int typecode)
        {
            string sql = SQL.Project.AvgPriceTrend;

            SqlCommand command = new SqlCommand();
            command.CommandText = sql;
            command.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", fxtcompanyid, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@typecode", typecode, SqlDbType.Int));
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
        public static DataSet GetMapPrice(int cityid, int fxtcompanyid, int projectid, string projectname, double surveyx, double surveyy, int purposetype, DateTime avgdate, int daterange, int typecode)
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
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@typecode", typecode, SqlDbType.Int));
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
        /// <summary>
        /// MCAS自动估价:楼栋，房号
        /// </summary>
        /// <param name="CityId">城市ID</param>
        /// <param name="ProjectId">楼盘ID</param>
        /// <param name="BuildingId">楼栋ID</param>
        /// <param name="HouseId">房号ID</param>
        /// <param name="FXTCompanyId">公司ID</param>
        /// <param name="totalFloor">总楼层</param>
        /// <param name="FloorNumber">所在楼层</param>
        /// <param name="Frontcode">朝向</param>
        /// <param name="projectprice">楼盘建筑类型均价</param>
        /// <param name="buildarea">面积</param>
        /// <returns></returns>
        public static DataSet GetMCASBHAutoPrice(int cityId, int projectId, int buildingId, int houseId, int FXTCompanyId, int totalFloor, int floorNumber
            , int frontCode, double projectprice, double buildarea)
        {

            try
            {
                string strsql = "[dbo].[procGetMCASBHAutoPrice]";
                CityTable city = CityTableDA.GetCityTable(cityId);
                if (null == city)
                    return null;
                SqlCommand cmd = new SqlCommand(strsql);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@weightBuildingTable", city.weightbuilding));
                cmd.Parameters.Add(new SqlParameter("@weightHouseTable", city.weighthouse));
                cmd.Parameters.Add(new SqlParameter("@CityId", cityId));
                cmd.Parameters.Add(new SqlParameter("@projectid", projectId));
                cmd.Parameters.Add(new SqlParameter("@BuildingId", buildingId));
                cmd.Parameters.Add(new SqlParameter("@HouseId", houseId));
                cmd.Parameters.Add(new SqlParameter("@FXTCompanyId", FXTCompanyId));//默认25
                cmd.Parameters.Add(new SqlParameter("@totalFloor", totalFloor));//总楼层
                cmd.Parameters.Add(new SqlParameter("@floorNumber", floorNumber));//所在楼
                cmd.Parameters.Add(new SqlParameter("@frontCode", frontCode));//朝向,默认值0,没有朝向
                cmd.Parameters.Add(new SqlParameter("@projectprice", projectprice));
                cmd.Parameters.Add(new SqlParameter("@buildarea", buildarea));//
                DataSet ds = ExecuteDataSet(cmd);
                return ds;
            }
            catch (SqlException ex)
            {
                //throw new Exception(ex.Message);
                return null;
            }
        }
        /// <summary>
        ///  MCAS自动估价:楼盘
        /// </summary>
        /// <param name="CityId"></param>
        /// <param name="ProjectId"></param>
        /// <param name="BuildingId">可为0</param>
        /// <param name="HouseId">可为0</param>
        /// <param name="FXTCompanyId">评估机构ID</param>
        /// <param name="CompanyId">客户单位Id</param>
        /// <param name="UserId">账号</param>
        /// <param name="BuildingArea">建筑面积</param>
        /// <param name="StartDate">可为空</param>
        /// <param name="EndDate">可为空</param>
        /// <param name="sysTypeCode">系统code</param>
        /// <returns>Tables[0]:询价结果</returns>
        public static DataSet GetMCASProjectAutoPrice(int CityId, int ProjectId, int FxtCompanyId, string UseMonth)
        {
            #region 旧的
            //try
            //{
            //    int eType = 0;
            //    string strsql = "[dbo].[procGetMCASValueByPId]";
            //    SqlCommand cmd = new SqlCommand(strsql);
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.Add(new SqlParameter("@CityId", CityId));
            //    cmd.Parameters.Add(new SqlParameter("@PorjectId", ProjectId));
            //    cmd.Parameters.Add(new SqlParameter("@BuildingId", BuildingId));
            //    cmd.Parameters.Add(new SqlParameter("@HouseId", HouseId));
            //    cmd.Parameters.Add(new SqlParameter("@FXTCompanyId", FXTCompanyId));
            //    cmd.Parameters.Add(new SqlParameter("@EType", eType));//返回数据类型，0：正常询价，1：只要均价
            //    cmd.Parameters.Add(new SqlParameter("@CompanyId", CompanyId));//25,客户公司ID
            //    cmd.Parameters.Add(new SqlParameter("@UserId", username));
            //    cmd.Parameters.Add(new SqlParameter("@BuildingArea", BuildingArea));
            //    cmd.Parameters.Add(new SqlParameter("@sysTypeCode", sysTypeCode));//默认1003001
            //    if (!string.IsNullOrEmpty(StartDate))
            //        cmd.Parameters.Add(new SqlParameter("@DateStart", StartDate));//案例选取起始时间
            //    if (!string.IsNullOrEmpty(EndDate))
            //        cmd.Parameters.Add(new SqlParameter("@DateEnd", EndDate));//案例选取截止时间
            //    DataSet ds = ExecuteDataSet(cmd);
            //    return ds;
            //}
            //catch (SqlException ex)
            //{
            //    //throw new Exception(ex.Message);
            //    return null;
            //}
            #endregion

            #region 新的
            //string casid = null;
            //int Type = 0;
            //string UserId = "";
            //int QueryTypeCode = 1004001;
            try
            {
                string strsql = "[dbo].[procGetProjectPrice]";
                SqlCommand cmd = new SqlCommand(strsql);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CityId", CityId));
                cmd.Parameters.Add(new SqlParameter("@ProjectId", ProjectId));
                //cmd.Parameters.Add(new SqlParameter("@FxtCompanyId", FxtCompanyId));
                //cmd.Parameters.Add(new SqlParameter("@UseMonth", UseMonth));
                DataSet ds = ExecuteDataSet(cmd);

                return ds;
            }
            catch (SqlException ex)
            {
                //LogHelper.Error(ex);
                //throw new Exception(ex.Message);
                //return null;
                throw ex;
            }
            #endregion
        }

        /// <summary>
        /// 楼盘自动估价（历史案例均价）
        /// </summary>
        /// <param name="CityId"></param>
        /// <param name="ProjectId"></param>
        /// <param name="UseMonth"></param>
        /// <returns></returns>
        public static DataSet GetMCASProjectHistoryAutoPrice(int CityId, int ProjectId, string UseMonth)
        {
            #region 新的
            try
            {
                string strsql = "[dbo].[procGetProjectHistoryPrice]";
                SqlCommand cmd = new SqlCommand(strsql);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CityId", CityId));
                cmd.Parameters.Add(new SqlParameter("@ProjectId", ProjectId));
                cmd.Parameters.Add(new SqlParameter("@UseMonth", UseMonth));
                DataSet ds = ExecuteDataSet(cmd);
                return ds;
            }
            catch (SqlException ex)
            {
                //LogHelper.Error(ex);
                //return null;
                throw ex;
            }
            #endregion
        }

        /// <summary>
        ///  MCAS自动估价:房号
        /// </summary>
        /// <returns>Tables[0]:询价结果</returns>
        public static DataSet GetMCASHouseAutoPrice(SearchBase search, int ProjectId, int BuildingId, int HouseId,
            int floorcount, int floorno, int frontcode, decimal buildingarea, int projectprice, string begindate, string enddate, int weighttype)
        {
            try
            {

                CityTable city = CityTableDA.GetCityTable(search.CityId);

                string strsql = "[dbo].[procGetVQBHAutoPrice]";
                SqlCommand cmd = new SqlCommand(strsql);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@weightBuildingTable", city.weightbuilding));
                cmd.Parameters.Add(new SqlParameter("@weightHouseTable", city.weighthouse));
                cmd.Parameters.Add(new SqlParameter("@HouseTable", city.HouseTable));
                cmd.Parameters.Add(new SqlParameter("@fxtcompanyid", search.FxtCompanyId));
                cmd.Parameters.Add(new SqlParameter("@cityid", search.CityId));
                //cmd.Parameters.Add(new SqlParameter("@sysTypeCode", search.SysTypeCode));
                cmd.Parameters.Add(new SqlParameter("@projectid", ProjectId));
                cmd.Parameters.Add(new SqlParameter("@buildingid", BuildingId));
                cmd.Parameters.Add(new SqlParameter("@houseid", HouseId));
                cmd.Parameters.Add(new SqlParameter("@totalfloor", floorcount));
                cmd.Parameters.Add(new SqlParameter("@floornumber", floorno));
                cmd.Parameters.Add(new SqlParameter("@frontcode", frontcode));
                cmd.Parameters.Add(new SqlParameter("@buildarea", buildingarea));
                cmd.Parameters.Add(new SqlParameter("@projectprice", projectprice));
                cmd.Parameters.Add(new SqlParameter("@weighttype", weighttype));
                //cmd.Parameters.Add(new SqlParameter("@begindate", begindate));
                //cmd.Parameters.Add(new SqlParameter("@enddate", enddate));

                //LogHelper.Info("weightBuildingTable:" + city.weightbuilding + ",weightHouseTable:" + city.weighthouse + ",HouseTable:" + city.HouseTable);
                //LogHelper.Info("cityid:" + search.CityId + "。projectid:" + ProjectId + "。buildingid:" + BuildingId + "。houseid:" + HouseId + "。totalfloor:"
                //    + floorcount + "。floornumber:" + floorno + "。frontcode:" + frontcode + "。buildarea:" + buildingarea + "。projectprice:" + projectprice + "。weighttype:" + weighttype);

                DataSet ds = ExecuteDataSet(cmd);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
                //throw new Exception(ex.Message);
                //return null;
            }
        }

        //获取行政区价格监测 kujj 20150421
        public static DataSet GetProcAreaAvgList(int fxtCompanyId, int cityId, string avgPriceDate)
        {
            try
            {
                string strsql = "[dbo].[procAreaAvgList]";
                SqlCommand cmd = new SqlCommand(strsql);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@avgPriceMonthTable", "dbo.DAT_SampleAvgPrice_month"));
                cmd.Parameters.Add(new SqlParameter("@fxtCompanyId", fxtCompanyId));
                cmd.Parameters.Add(new SqlParameter("@cityId", cityId));
                cmd.Parameters.Add(new SqlParameter("@avgPriceDate", avgPriceDate));
                DataSet ds = ExecuteDataSet(cmd);
                return ds;
            }
            catch (SqlException ex)
            {
                return null;
            }
        }
        //获取片区价格监测 kujj 20150422
        public static DataSet GetProcSubAreaAvgList(int fxtCompanyId, int cityId, int areaId, string avgPriceDate)
        {
            try
            {
                string strsql = "[dbo].[procSubAreaAvgList]";
                SqlCommand cmd = new SqlCommand(strsql);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@avgPriceMonthTable", "dbo.DAT_SampleAvgPrice_month"));
                cmd.Parameters.Add(new SqlParameter("@fxtCompanyId", fxtCompanyId));
                cmd.Parameters.Add(new SqlParameter("@cityId", cityId));
                cmd.Parameters.Add(new SqlParameter("@areaId", areaId));
                cmd.Parameters.Add(new SqlParameter("@avgPriceDate", avgPriceDate));
                DataSet ds = ExecuteDataSet(cmd);
                return ds;
            }
            catch (SqlException ex)
            {
                return null;
            }
        }
        //获取楼盘价格监测 kujj 20150422
        public static DataSet GetProcProjectAvgList(int fxtCompanyId, int cityId, int areaId, int subAreaId, string avgPriceDate)
        {
            try
            {
                string strsql = "[dbo].[procProjectAvgList]";
                CityTable city = CityTableDA.GetCityTable(cityId);
                if (null == city)
                    return new DataSet();

                SqlCommand cmd = new SqlCommand(strsql);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@avgPriceMonthTable", "dbo.DAT_SampleAvgPrice_month"));
                cmd.Parameters.Add(new SqlParameter("@projectTable", city.ProjectTable));
                cmd.Parameters.Add(new SqlParameter("@fxtCompanyId", fxtCompanyId));
                cmd.Parameters.Add(new SqlParameter("@cityId", cityId));
                cmd.Parameters.Add(new SqlParameter("@areaId", areaId));
                cmd.Parameters.Add(new SqlParameter("@subAreaId", subAreaId));
                cmd.Parameters.Add(new SqlParameter("@avgPriceDate", avgPriceDate));
                DataSet ds = ExecuteDataSet(cmd);
                return ds;
            }
            catch (SqlException ex)
            {
                return null;
            }
        }
        //获取行政区、片区近一年均价 kujj 20150507
        public static DataSet GetAreaYearAvgList(int fxtCompanyId, int cityId, int areaId, int subAreaId)
        {
            string strSql = SQL.Project.AreaYearAvgList;
            strSql = strSql.Replace("@table", "fxtproject.dbo.DAT_SampleAvgPrice_month");

            SqlCommand cmd = new SqlCommand(strSql);
            cmd.Parameters.Add(new SqlParameter("@cityid", cityId));
            cmd.Parameters.Add(new SqlParameter("@fxtcompanyid", fxtCompanyId));
            cmd.Parameters.Add(new SqlParameter("@areaid", areaId));
            cmd.Parameters.Add(new SqlParameter("@subareaid", subAreaId));
            cmd.CommandText = strSql;
            DataSet ds = ExecuteDataSet(cmd);
            return ds;
        }
        //获取行政区、片区、楼盘近一年走势 kujj 20150507
        public static DataSet GetDiffTypeAvgList(int type, int fxtCompanyId, int cityId, int areaid, int subareaid, int projectid, int buildingareatype, int buildingdatetype, int buildingtypecode, int housetype, int purposetype, int e_housetype, string begin, string end)
        {
            string sql = string.Empty;
            switch (type)
            {
                case 0: sql = "dbo.procMonthPriceMonitoring"; break;//列出以月为单位的均价列表
                case 1: sql = "dbo.procWeekPriceMonitoring"; break;//列出以周为单位的均价列表
                case 2: sql = "dbo.procQuarterPriceMonitoring"; break;//列出以季为单位的均价列表
                default: sql = "dbo.procYearPriceMonitoring"; break;//列出以年为单位的均价列表
            }
            SqlCommand cmd = new SqlCommand(sql);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@avgPriceMonthTable", "dbo.DAT_SampleAvgPrice_Month"));
            cmd.Parameters.Add(new SqlParameter("@begin", begin));
            cmd.Parameters.Add(new SqlParameter("@end", end));
            cmd.Parameters.Add(new SqlParameter("@fxtCompanyId", fxtCompanyId));
            cmd.Parameters.Add(new SqlParameter("@CityId", cityId));
            cmd.Parameters.Add(new SqlParameter("@AreaId", areaid));
            cmd.Parameters.Add(new SqlParameter("@SubAreaId", subareaid));
            cmd.Parameters.Add(new SqlParameter("@ProjectId", projectid));
            cmd.Parameters.Add(new SqlParameter("@BuildingAreaType", buildingareatype));
            cmd.Parameters.Add(new SqlParameter("@BuildingDateType", buildingdatetype));
            cmd.Parameters.Add(new SqlParameter("@BuildingTypeCode", buildingtypecode));
            cmd.Parameters.Add(new SqlParameter("@HouseType", housetype));
            cmd.Parameters.Add(new SqlParameter("@PurposeType", purposetype));
            cmd.Parameters.Add(new SqlParameter("@e_HouseType", e_housetype));
            DataSet ds = ExecuteDataSet(cmd);
            return ds;
        }

        //获取楼盘建筑类型均价 kujj 20150612
        public static DataSet GetMCASWeightProjectPrice(int cityid, int fxtcompanyid, int projectid, DateTime begindate, DateTime enddate)
        {
            string strSql = SQL.AutoPrice.WeightProjectPrice;
            CityTable city = CityTableDA.GetCityTable(cityid);
            if (null == city)
                return null;
            strSql = strSql.Replace("@table", "fxtproject." + city.weightproject);

            SqlCommand cmd = new SqlCommand(strSql);
            cmd.Parameters.Add(new SqlParameter("@cityid", cityid));
            cmd.Parameters.Add(new SqlParameter("@fxtcompanyid", fxtcompanyid));
            cmd.Parameters.Add(new SqlParameter("@projectid", projectid));
            cmd.Parameters.Add(new SqlParameter("@begindate", begindate.ToString("yyyy-MM-dd HH:mm:ss")));
            cmd.Parameters.Add(new SqlParameter("@enddate", enddate.ToString("yyyy-MM-dd HH:mm:ss")));
            cmd.CommandText = strSql;
            DataSet ds = ExecuteDataSet(cmd);
            return ds;
        }

        /// <summary>
        /// 询价单
        /// </summary>
        /// <param name="search"></param>
        /// <param name="projectid"></param>
        /// <param name="buildingid"></param>
        /// <param name="houseid"></param>
        /// <returns></returns>
        public static DataSet GetMCASInquiry_ForVQ(SearchBase search, int projectid, int buildingid, int houseid)
        {
            string strSql = string.Empty;
            if (projectid != 0)
            {
                if (buildingid != 0 & houseid != 0)
                {
                    //查全部
                    strSql = SQL.Project.HouseBuildingProjectByHouseID;
                }
                else if (buildingid != 0 & houseid == 0)
                {
                    //查楼盘与楼栋
                    strSql = SQL.Project.BuildingProjectByBuildingID;
                }
            }

            CityTable city = CityTableDA.GetCityTable(search.CityId);
            strSql = strSql.Replace("@housetable", "fxtproject." + city.HouseTable);
            strSql = strSql.Replace("@buildingtable", "fxtproject." + city.BuildingTable);
            strSql = strSql.Replace("@projecttable", "fxtproject." + city.ProjectTable);

            SqlCommand cmd = new SqlCommand(strSql);
            cmd.Parameters.Add(new SqlParameter("@CityId", search.CityId));
            cmd.Parameters.Add(new SqlParameter("@ProjectId", projectid));
            cmd.Parameters.Add(new SqlParameter("@BuildingId", buildingid));
            cmd.Parameters.Add(new SqlParameter("@FxtCompanyId", search.FxtCompanyId));
            cmd.Parameters.Add(new SqlParameter("@TypeCode", search.SysTypeCode));
            cmd.Parameters.Add(new SqlParameter("@HouseId", houseid));
            cmd.CommandText = strSql;
            DataSet ds = ExecuteDataSet(cmd);
            return ds;
        }
    }
}
