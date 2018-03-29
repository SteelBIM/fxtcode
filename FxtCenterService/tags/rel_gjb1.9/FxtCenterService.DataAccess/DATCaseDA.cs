using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Common;
using System.Data.SqlClient;
using CAS.DataAccess.BaseDAModels;
using System.Data;
using CAS.Entity.GJBEntity;

namespace FxtCenterService.DataAccess
{
    public class DATCaseDA : Base
    {
        public static int Add(DATCase model)
        {
            return InsertFromEntity<DATCase>(model);
        }
        public static int Update(DATCase model)
        {
            return UpdateFromEntity<DATCase>(model);
        }
        public static int Delete(int id)
        {
            return DeleteByPrimaryKey<DATCase>(id);
        }
        public static DATCase GetDATCaseByPK(int id)
        {
            return ExecuteToEntityByPrimaryKey<DATCase>(id);
        }
        /// <summary>
        /// 查询测算时使用的案例数据列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="projectname"></param>
        /// <returns></returns>
        public static List<Dat_Case> GetDATCaseListByCalculate(SearchBase search, string projectname, int minBuildingArea, int maxBuildingArea, int minFloorNumber, int maxFloorNumber, string address)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.Project.GetDatCaseListByCalculate;
            CityTable city = CityTableDA.GetCityTable(search.CityId);
            if (null == city)
                return new List<Dat_Case>();
            sql = sql.Replace("@projecttable", city.ProjectTable);
            sql = sql.Replace("@projectsubtable", city.ProjectTable + "_sub");
            sql = sql.Replace("@casetable", city.casetable);

            string topSql = search.Top > 0 ? " top " + search.Top.ToString() : "";
            sql = sql.Replace("@top", topSql);
            search.Where += " and a.UnitPrice > 0";      //由于数据中心可能存在为零的数据，故添加此条件
            if (0 < minBuildingArea)
            {
                search.Where += "and a.BuildingArea >= " + minBuildingArea; 
            }
            else
            {
                search.Where += "and a.BuildingArea > 0";   //由于数据中心可能存在为零的数据，故添加此条件
            }
            if (0 < maxBuildingArea)
            {
                search.Where += "and a.BuildingArea <= " + maxBuildingArea; 
            }
            if (0 < minFloorNumber)
            {
                search.Where += " and a.FloorNumber >= " + minFloorNumber;
            }
            if (0 < maxFloorNumber)
            {
                search.Where += " and a.FloorNumber <= " + maxFloorNumber;
            }
            if (!string.IsNullOrEmpty(address))
            {
                search.Where += " and b.Address like @Address";
                parameters.Add(SqlHelper.GetSqlParameter("@Address", "%" + SQLFilterHelper.EscapeLikeString(address, "$") + "%", SqlDbType.NVarChar));
            }
            //search.Where += " and b.projectname like @projectname escape '$'";            
            search.Where += " and b.projectname like @projectname";
            parameters.Add(SqlHelper.GetSqlParameter("@projectname", "%" + SQLFilterHelper.EscapeLikeString(projectname, "$") + "%", SqlDbType.NVarChar));
            parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));
            
            search.OrderBy = " a.casedate desc";
            sql = HandleSQL(search, sql);
            return ExecuteToEntityList<Dat_Case>(sql, System.Data.CommandType.Text, parameters);
        }

        #region 特殊客户

        /// <summary>
        /// 查询测算时使用的案例数据列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="projectname"></param>
        /// <returns></returns>
        public static List<Dat_Case_Dhhy> GetDATCaseListByCalculateForSpecial(SearchBase search, string projectname, int minBuildingArea, int maxBuildingArea, int minFloorNumber, int maxFloorNumber, string address)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.Project.GetDatCaseListByCalculateForSpecial;
            CityTable city = CityTableDA.GetCityTable(search.CityId);
            if (null == city)
                return new List<Dat_Case_Dhhy>();
            sql = sql.Replace("@projecttable", city.ProjectTable);
            sql = sql.Replace("@projectsubtable", city.ProjectTable + "_sub");
            sql = sql.Replace("@casetable", city.casetable);

            string topSql = search.Top > 0 ? " top " + search.Top.ToString() : "";
            sql = sql.Replace("@top", topSql);
            search.Where += " and a.UnitPrice > 0";      //由于数据中心可能存在为零的数据，故添加此条件
            if (0 < minBuildingArea)
            {
                search.Where += "and a.BuildingArea >= " + minBuildingArea;
            }
            else
            {
                search.Where += "and a.BuildingArea > 0";   //由于数据中心可能存在为零的数据，故添加此条件
            }
            if (0 < maxBuildingArea)
            {
                search.Where += "and a.BuildingArea <= " + maxBuildingArea;
            }
            if (0 < minFloorNumber)
            {
                search.Where += " and a.FloorNumber >= " + minFloorNumber;
            }
            if (0 < maxFloorNumber)
            {
                search.Where += " and a.FloorNumber <= " + maxFloorNumber;
            }
            if (!string.IsNullOrEmpty(address))
            {
                search.Where += " and b.Address like @Address";
                parameters.Add(SqlHelper.GetSqlParameter("@Address", "%" + SQLFilterHelper.EscapeLikeString(address, "$") + "%", SqlDbType.NVarChar));
            }
            //search.Where += " and b.projectname like @projectname escape '$'";            
            search.Where += " and b.projectname like @projectname";
            parameters.Add(SqlHelper.GetSqlParameter("@projectname", "%" + SQLFilterHelper.EscapeLikeString(projectname, "$") + "%", SqlDbType.NVarChar));
            parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));
            search.OrderBy = " a.casedate desc";
            sql = HandleSQL(search, sql);
            return ExecuteToEntityList<Dat_Case_Dhhy>(sql, System.Data.CommandType.Text, parameters);
        }

        public static List<Dat_Case_Dhhy> GetDATCaseListForSpecial(SearchBase search, string key, string projectname, int[] caseIds)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.Project.GetDatCaseListForSpecial;
            CityTable city = CityTableDA.GetCityTable(search.CityId);
            if (null == city)
                return new List<Dat_Case_Dhhy>();
            sql = sql.Replace("@projecttable", city.ProjectTable);
            sql = sql.Replace("@projectsubtable", city.ProjectTable + "_sub");
            sql = sql.Replace("@casetable", city.casetable);

            string topSql = search.Top > 0 ? " top " + search.Top.ToString() : "";
            sql = sql.Replace("@top", topSql);

            if (!string.IsNullOrEmpty(key))
            {
                //search.Where += " and b.projectname like @key escape '$'";
                search.Where += " and b.projectname like @key";
                parameters.Add(SqlHelper.GetSqlParameter("@key", "%" + SQLFilterHelper.EscapeLikeString(key, "$") + "%", SqlDbType.NVarChar, 100));
            }
            if (!string.IsNullOrEmpty(projectname))
            {
                //search.Where += " and b.projectname like @projectname escape '$'";
                search.Where += " and b.projectname like @projectname";
                parameters.Add(SqlHelper.GetSqlParameter("@projectname", "%" + SQLFilterHelper.EscapeLikeString(projectname, "$") + "%", SqlDbType.NVarChar));
            }
            if (null != caseIds && 0 < caseIds.Length)
            {
                search.Where += " and a.caseid in (" + string.Join(",", caseIds) + ")";
            }
            search.Where += " and a.UnitPrice > 0 and a.BuildingArea > 0";      //由于数据中心可能存在为零的数据，故添加此条件
            parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));
            search.OrderBy = " a.casedate desc";
            sql = HandleSQL(search, sql);
            return ExecuteToEntityList<Dat_Case_Dhhy>(sql, System.Data.CommandType.Text, parameters);
        }

        #endregion

        public static List<Dat_Case> GetDATCaseList(SearchBase search, string key, string projectname, int[] caseIds)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.Project.GetDatCaseList;
            CityTable city = CityTableDA.GetCityTable(search.CityId);
            if (null == city)
                return new List<Dat_Case>();
            sql = sql.Replace("@projecttable", city.ProjectTable);
            sql = sql.Replace("@projectsubtable", city.ProjectTable + "_sub");
            sql = sql.Replace("@casetable", city.casetable);

            string topSql = search.Top > 0 ? " top " + search.Top.ToString() : "";
            sql = sql.Replace("@top", topSql);

            if (!string.IsNullOrEmpty(key))
            {
                //search.Where += " and b.projectname like @key escape '$'";
                search.Where += " and b.projectname like @key";
                parameters.Add(SqlHelper.GetSqlParameter("@key", "%" + SQLFilterHelper.EscapeLikeString(key, "$") + "%", SqlDbType.NVarChar, 100));
            }
            if (!string.IsNullOrEmpty(projectname))
            {
                //search.Where += " and b.projectname like @projectname escape '$'";
                search.Where += " and b.projectname like @projectname";
                parameters.Add(SqlHelper.GetSqlParameter("@projectname", "%" + SQLFilterHelper.EscapeLikeString(projectname, "$") + "%", SqlDbType.NVarChar));
            }
            if (null != caseIds && 0 < caseIds.Length)
            {
                search.Where += " and a.caseid in (" + string.Join(",", caseIds) + ")";
            }
            search.Where += " and a.UnitPrice > 0 and a.BuildingArea > 0";      //由于数据中心可能存在为零的数据，故添加此条件
            parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));
            search.OrderBy = " a.casedate desc";
            sql = HandleSQL(search, sql);
            return ExecuteToEntityList<Dat_Case>(sql, System.Data.CommandType.Text, parameters);
        }

        /// <summary>
        /// 获取行政区内指定类型案例  caoq 2014-1-20
        /// </summary>
        /// <param name="fxtcompanyid"></param>
        /// <param name="cityid"></param>
        /// <param name="areaid"></param>
        /// <param name="purposecode"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public static List<CAS.Entity.DBEntity.DATCase> GetAreaCase(int fxtcompanyid, int cityid, int areaid, int purposecode, DateTime startdate, DateTime enddate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.Project.GetBaseCaseList;
            CityTable city = CityTableDA.GetCityTable(cityid);
            sql = sql.Replace("@casetable", city.casetable);

            parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", fxtcompanyid, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@cityid", cityid, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@startdate", startdate.ToString("yyyy-MM-01"), SqlDbType.VarChar));//取月份第一天
            parameters.Add(SqlHelper.GetSqlParameter("@enddate", enddate.ToString("yyyy-MM-dd 23:59:59"), SqlDbType.VarChar));//最晚时间

            if (areaid > 0)
            {
                sql += " and areaid=" + areaid;
            }
            if (purposecode > 0)
            {
                sql += " and purposecode=" + purposecode;
            }
            return ExecuteToEntityList<CAS.Entity.DBEntity.DATCase>(sql, System.Data.CommandType.Text, parameters);
        }

        /// <summary>
        /// 获取楼盘周边案例  caoq 2014-1-20
        /// </summary>
        /// <param name="fxtcompanyid">公司ID</param>
        /// <param name="cityid">城市ID</param>
        /// <param name="projectid">楼盘ID</param>
        /// <param name="buildingareatype">面积段CODE</param>
        /// <param name="buildingtypecode">建筑类型Code</param>
        /// <param name="buildingenddate">竣工时间</param>
        /// <param name="areaid">行政区</param>
        /// <param name="surveyx">查勘X坐标</param>
        /// <param name="surveyy">查勘Y坐标</param>
        /// <param name="startdate">起始日期</param>
        /// <param name="enddate">结束日期</param>
        /// <returns></returns>
        public static List<Dat_AroundCase> GetProjectAroundCase(int fxtcompanyid, int cityid, int projectid, int buildingareatype, int buildingtypecode, DateTime buildingenddate, int areaid, double surveyx, double surveyy, DateTime startdate, DateTime enddate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<Dat_AroundCase> list = null;

            string sql = SQL.Project.GetProjectAroundCase;
            CityTable city = CityTableDA.GetCityTable(cityid);
            sql = sql.Replace("@projecttable", city.ProjectTable);
            sql = sql.Replace("@projectsubtable", city.ProjectTable + "_sub");
            sql = sql.Replace("@casetable", city.casetable);

            sql = sql.Replace("@@cityid", cityid.ToString());
            sql = sql.Replace("@@fxtcompanyid", fxtcompanyid.ToString());
            sql = sql.Replace("@@projectid", projectid.ToString());
            sql = sql.Replace("@@buildingareatype", buildingareatype.ToString());
            sql = sql.Replace("@@buildingtypecode", buildingtypecode.ToString());
            sql = sql.Replace("@@areaid", areaid.ToString());

            sql = sql.Replace("@@x", surveyx.ToString());
            sql = sql.Replace("@@y", surveyy.ToString());
            SqlCommand command = new SqlCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            command.Parameters.Add(SqlHelper.GetSqlParameter("@startdate", startdate, SqlDbType.DateTime));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@enddate", enddate, SqlDbType.DateTime));
            //竣工时间
            command.Parameters.Add(SqlHelper.GetSqlParameter("@buildingstartdate", buildingenddate.AddYears(-3).ToString("yyyy"), SqlDbType.VarChar));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@buildingenddate", buildingenddate.AddYears(3).ToString("yyyy"), SqlDbType.VarChar));

            DataSet ds = ExecuteDataSet(command);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                list = new List<Dat_AroundCase>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    list.Add(new Dat_AroundCase()
                    {
                        datatype = StringHelper.TryGetInt(row["datatype"].ToString()),
                        unitprice = StringHelper.TryGetDecimal(row["unitprice"].ToString()),
                        buildingareatype = StringHelper.TryGetInt(row["buildingareatype"].ToString()),
                        buildingarea = StringHelper.TryGetDecimal(row["buildingarea"].ToString()),
                        buildingtypecode = StringHelper.TryGetInt(row["buildingtypecode"].ToString()),
                    });
                }
            }

            return list;
        }

        /// <summary>
        /// 周边同质楼盘价格计算  caoq 2014-1-20
        /// </summary>
        /// <param name="fxtcompanyid">公司ID</param>
        /// <param name="cityid">城市ID</param>
        /// <param name="projectid">楼盘ID</param>
        /// <param name="buildingtypecode">建筑类型Code</param>
        /// <param name="buildingenddate">竣工时间</param>
        /// <param name="areaid">行政区ID</param>
        /// <param name="surveyx">查勘坐标X</param>
        /// <param name="surveyy">查勘坐标Y</param>
        /// <param name="startdate">起始日期</param>
        /// <param name="enddate">结束日期</param>
        /// <returns>dataset(column:projectid,projectname,avgprice,preavgprice,changepercent,projectx,projecty)</returns>
        public static DataSet GetSameProjectCasePrice(int fxtcompanyid, int cityid, int projectid, int buildingtypecode, DateTime buildingenddate, int areaid, double surveyx, double surveyy, DateTime startdate, DateTime enddate)
        {
            string sql = SQL.Project.SameProjectCasePrice;

            CityTable city = CityTableDA.GetCityTable(cityid);
            sql = sql.Replace("@projecttable", city.ProjectTable);
            sql = sql.Replace("@projectsubtable", city.ProjectTable + "_sub");
            sql = sql.Replace("@casetable", city.casetable);

            sql = sql.Replace("@@x", surveyx.ToString());
            sql = sql.Replace("@@y", surveyy.ToString());

            SqlCommand command = new SqlCommand();
            command.CommandText = sql;
            command.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", cityid, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", fxtcompanyid, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@projectid", projectid, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@buildingtypecode", buildingtypecode, SqlDbType.Int));

            command.Parameters.Add(SqlHelper.GetSqlParameter("@buildingstartdate", buildingenddate.AddYears(-3).ToString("yyyy"), SqlDbType.VarChar));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@buildingenddate", buildingenddate.AddYears(3).ToString("yyyy"), SqlDbType.VarChar));

            command.Parameters.Add(SqlHelper.GetSqlParameter("@areaid", areaid, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@startdate", startdate.ToString("yyyy-MM-dd"), SqlDbType.DateTime));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@enddate", enddate.ToString("yyyy-MM-dd"), SqlDbType.DateTime));

            return ExecuteDataSet(command);
        }

        /// <summary>
        /// 不同渠道楼盘均价获取  caoq 2014-1-20
        /// </summary>
        /// <param name="fxtcompanyid">公司ID</param>
        /// <param name="cityid">城市ID</param>        
        /// <param name="projectid">楼盘ID</param>
        /// <param name="purposecode">物业类型</param>
        /// <param name="buildingareatype">面积段CODE</param>
        /// <param name="buildingtypecode">建筑类型Code</param>
        /// <param name="startdate">起始日期</param>
        /// <param name="enddate">结束日期</param>
        /// <param name="daterange">计算范围</param>
        /// <returns>dataset(column:avgprice,projectid,sourcename)</returns>
        public static DataSet GetOtherChannelCasePrice(int fxtcompanyid, int cityid, int projectid, int purposecode, int buildingareatype, int buildingtypecode, DateTime startdate, DateTime enddate, int daterange)
        {
            string sql = SQL.Project.OtherChannelCasePrice;

            CityTable city = CityTableDA.GetCityTable(cityid);
            sql = sql.Replace("@casetable", city.casetable);

            SqlCommand command = new SqlCommand();
            command.CommandText = sql;

            command.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", fxtcompanyid, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", cityid, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@projectid", projectid, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@purposecode", purposecode, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@buildingareatype", buildingareatype, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@buildingtypecode", buildingtypecode, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@startdate", startdate.ToString("yyyy-MM-dd"), SqlDbType.DateTime));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@enddate", enddate.ToString("yyyy-MM-dd"), SqlDbType.DateTime));
            //均价表日期格式为年月字符          
            command.Parameters.Add(SqlHelper.GetSqlParameter("@avgstartdate", startdate.ToString("yyyyMM"), SqlDbType.VarChar));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@avgenddate", enddate.ToString("yyyyMM"), SqlDbType.VarChar));
            //计算范围
            command.Parameters.Add(SqlHelper.GetSqlParameter("@daterange", daterange, SqlDbType.Int));
            return ExecuteDataSet(command);
        }

        /// <summary>
        /// 获取楼盘案例总数 caoq 2014-3-28
        /// </summary>
        /// <param name="fxtcompanyid">公司ID</param>
        /// <param name="cityid">城市ID</param>        
        /// <param name="projectid">楼盘ID</param>
        /// <param name="purposecode">物业类型</param>
        /// <param name="buildingareatype">面积段CODE</param>
        /// <param name="buildingtypecode">建筑类型Code</param>
        /// <param name="startdate">起始日期</param>
        /// <param name="enddate">结束日期</param>
        /// <returns></returns>
        public static int GetProjectCaseCount(int fxtcompanyid, int cityid, int projectid, int purposecode, int buildingareatype, int buildingtypecode, DateTime startdate, DateTime enddate)
        {
            string sql = SQL.Project.ProjectCaseCount;

            CityTable city = CityTableDA.GetCityTable(cityid);
            sql = sql.Replace("@casetable", city.casetable);

            SqlCommand command = new SqlCommand();
            command.CommandText = sql;

            command.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", fxtcompanyid, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", cityid, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@projectid", projectid, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@purposecode", purposecode, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@startdate", startdate.ToString("yyyy-MM-dd"), SqlDbType.DateTime));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@enddate", enddate.ToString("yyyy-MM-dd"), SqlDbType.DateTime));

            return StringHelper.TryGetInt(ExecuteScalar(command).ToString());
        }
    }
}
