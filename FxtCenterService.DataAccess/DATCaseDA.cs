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
        public static List<Dat_Case> GetDATCaseListByCalculate(SearchBase search, string projectname, int minBuildingArea, int maxBuildingArea,
            int minFloorNumber, int maxFloorNumber, decimal minUnitPrice, decimal maxUnitPrice, string address, int caseTypeCode, int areaid, int subareaid,
            DateTime startCaseDate, DateTime endCaseDate, int structurecode, int iselevator, string structurecodename)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.Project.GetDatCaseListByCalculate;
            CityTable city = CityTableDA.GetCityTable(search.CityId);
            if (null == city)
                return new List<Dat_Case>();
            sql = sql.Replace("@projecttable", city.ProjectTable);
            sql = sql.Replace("@projectsubtable", city.ProjectTable + "_sub");
            sql = sql.Replace("@buildingtable", city.BuildingTable);
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
            if (0 < minUnitPrice)
            {
                search.Where += " and a.UnitPrice >= " + minUnitPrice;
            }
            if (0 < maxUnitPrice)
            {
                search.Where += " and a.UnitPrice <= " + maxUnitPrice;
            }
            if (!string.IsNullOrEmpty(address))
            {
                search.Where += " and b.Address like @Address";
                parameters.Add(SqlHelper.GetSqlParameter("@Address", "%" + SQLFilterHelper.EscapeLikeString(address, "$") + "%", SqlDbType.NVarChar));
            }

            //search.Where += " and (@caseTypeCode = 0 or (@caseTypeCode > 0 and a.CaseTypeCode = @caseTypeCode))";

            if (caseTypeCode > 0)
            {
                search.Where += " and a.CaseTypeCode = @caseTypeCode";
                parameters.Add(SqlHelper.GetSqlParameter("@caseTypeCode", caseTypeCode, SqlDbType.Int));
            }
            if (0 < areaid)
            {
                search.Where += " and b.AreaID = " + areaid;
            }
            if (0 < subareaid)
            {
                search.Where += " and b.SubAreaId = " + subareaid;
            }

            if (default(DateTime) != startCaseDate)
            {
                search.Where += " and a.casedate >= '" + startCaseDate + "'";
            }
            if (default(DateTime) != endCaseDate)
            {
                search.Where += " and a.casedate <= '" + endCaseDate + "'";
            }

            //建筑结构
            if (structurecode > 0)
            {
                search.Where += " and c.StructureCode = '" + structurecode + "'";
            }
            else if (!string.IsNullOrEmpty(structurecodename))
            {
                var code =  SYSCodeDA.GetCode(2010, structurecodename);
                if (code != null)
                {
                    search.Where += " and c.StructureCode = '" + code.code + "'";
                }
                else {
                    return new List<Dat_Case>();
                }
            }

            //有电梯
            if (iselevator == 1)
            {
                search.Where += " and c.IsElevator = " + iselevator + " ";
            }

            //无电梯
            if (iselevator == 2)
            {
                search.Where += " and c.IsElevator = " + 0 + " ";
            }

            //search.Where += " and b.projectname like @projectname escape '$'";            
            search.Where += " and b.projectname like @projectname";
            parameters.Add(SqlHelper.GetSqlParameter("@projectname", "%" + SQLFilterHelper.EscapeLikeString(projectname, "$") + "%", SqlDbType.NVarChar));
            parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
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
        public static List<Dat_Case_Dhhy> GetDATCaseListByCalculateForSpecial(SearchBase search, string projectname, int minBuildingArea, int maxBuildingArea, int minFloorNumber, int maxFloorNumber, decimal minUnitPrice, decimal maxUnitPrice, string address, int caseTypeCode, int areaid, int subareaid, DateTime startCaseDate, DateTime endCaseDate, int structurecode, int iselevator)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.Project.GetDatCaseListByCalculateForSpecial;
            CityTable city = CityTableDA.GetCityTable(search.CityId);
            if (null == city)
                return new List<Dat_Case_Dhhy>();
            sql = sql.Replace("@projecttable", city.ProjectTable);
            sql = sql.Replace("@projectsubtable", city.ProjectTable + "_sub");
            sql = sql.Replace("@buildingtable", city.BuildingTable);
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
            if (0 < minUnitPrice)
            {
                search.Where += " and a.minUnitPrice >= " + minUnitPrice;
            }
            if (0 < maxUnitPrice)
            {
                search.Where += " and a.maxUnitPrice <= " + maxUnitPrice;
            }
            if (!string.IsNullOrEmpty(address))
            {
                search.Where += " and b.Address like @Address";
                parameters.Add(SqlHelper.GetSqlParameter("@Address", "%" + SQLFilterHelper.EscapeLikeString(address, "$") + "%", SqlDbType.NVarChar));
            }

            //search.Where += " and (@caseTypeCode = 0 or (@caseTypeCode > 0 and a.CaseTypeCode = @caseTypeCode))";

            if (caseTypeCode > 0)
            {
                search.Where += " and a.CaseTypeCode = @caseTypeCode";
                parameters.Add(SqlHelper.GetSqlParameter("@caseTypeCode", caseTypeCode, SqlDbType.Int));
            }
            if (0 < areaid)
            {
                search.Where += " and b.AreaID = " + areaid;
            }
            if (0 < subareaid)
            {
                search.Where += " and b.SubAreaId = " + subareaid;
            }

            if (default(DateTime) != startCaseDate)
            {
                search.Where += " and a.casedate >= '" + startCaseDate + "'";
            }
            if (default(DateTime) != endCaseDate)
            {
                search.Where += " and a.casedate <= '" + endCaseDate + "'";
            }

            //建筑结构
            if (structurecode > 0)
            {
                search.Where += " and c.StructureCode = '" + structurecode + "'";
            }

            //有电梯
            if (iselevator == 1)
            {
                search.Where += " and c.IsElevator = " + iselevator + " ";
            }

            //无电梯
            if (iselevator == 2)
            {
                search.Where += " and c.IsElevator = " + 0 + " ";
            }

            //search.Where += " and b.projectname like @projectname escape '$'";            
            search.Where += " and b.projectname like @projectname";
            parameters.Add(SqlHelper.GetSqlParameter("@projectname", "%" + SQLFilterHelper.EscapeLikeString(projectname, "$") + "%", SqlDbType.NVarChar));
            parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
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
            parameters.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
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
            parameters.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
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
        public static List<CAS.Entity.DBEntity.DATCase> GetAreaCase(int fxtcompanyid, int cityid, int areaid, int purposecode, DateTime startdate, DateTime enddate, int typecode)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.Project.GetBaseCaseList;
            CityTable city = CityTableDA.GetCityTable(cityid);
            sql = sql.Replace("@casetable", city.casetable);

            parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", fxtcompanyid, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@typecode", typecode, SqlDbType.Int));
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
        /// <param name="fivemiles">坐标定位偏差(默认5公里偏差（0.5）)</param>
        /// <returns></returns>
        public static List<Dat_AroundCase> GetProjectAroundCase(int fxtcompanyid, int cityid, int projectid, int buildingareatype, int buildingtypecode, DateTime buildingenddate, int areaid, double surveyx, double surveyy, DateTime startdate, DateTime enddate, int typecode, double fivemiles = 0.5)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<Dat_AroundCase> list = null;

            string sql = SQL.Project.GetProjectAroundCase;
            CityTable city = CityTableDA.GetCityTable(cityid);
            sql = sql.Replace("@projecttable", city.ProjectTable);
            sql = sql.Replace("@projectsubtable", city.ProjectTable + "_sub");
            sql = sql.Replace("@casetable", city.casetable);

            sql = sql.Replace("@@fivemiles", fivemiles.ToString());
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
            command.Parameters.Add(SqlHelper.GetSqlParameter("@typecode", typecode, SqlDbType.Int));
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
        public static DataSet GetSameProjectCasePrice(int fxtcompanyid, int cityid, int projectid, int buildingtypecode, DateTime buildingenddate, int areaid, double surveyx, double surveyy, DateTime startdate, DateTime enddate, int typecode)
        {
            string sql = SQL.Project.SameProjectCasePrice;

            CityTable city = CityTableDA.GetCityTable(cityid);
            sql = sql.Replace("@projecttable", city.ProjectTable);
            sql = sql.Replace("@projectavgtable", city.projectavgtable);
            sql = sql.Replace("@weightproject", city.weightproject);
            //sql = sql.Replace("@projectsubtable", city.ProjectTable + "_sub");
            //sql = sql.Replace("@casetable", city.casetable);
            //sql = sql.Replace("@@x", surveyx.ToString());
            //sql = sql.Replace("@@y", surveyy.ToString());

            SqlCommand command = new SqlCommand();
            command.CommandText = sql;
            command.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", cityid, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", fxtcompanyid, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@typecode", typecode, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@projectid", projectid, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@buildingtypecode", buildingtypecode, SqlDbType.Int));

            //command.Parameters.Add(SqlHelper.GetSqlParameter("@buildingstartdate", buildingenddate.AddYears(-3).ToString("yyyy"), SqlDbType.VarChar));
            //command.Parameters.Add(SqlHelper.GetSqlParameter("@buildingenddate", buildingenddate.AddYears(3).ToString("yyyy"), SqlDbType.VarChar));
            //command.Parameters.Add(SqlHelper.GetSqlParameter("@areaid", areaid, SqlDbType.Int));

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
        public static DataSet GetOtherChannelCasePrice(int fxtcompanyid, int cityid, int projectid, int purposecode, int buildingareatype, int buildingtypecode, DateTime startdate, DateTime enddate, int daterange, int typecode)
        {
            string sql = SQL.Project.OtherChannelCasePrice;

            CityTable city = CityTableDA.GetCityTable(cityid);
            sql = sql.Replace("@casetable", city.casetable);

            SqlCommand command = new SqlCommand();
            command.CommandText = sql;

            command.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", fxtcompanyid, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@typecode", typecode, SqlDbType.Int));
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
        public static int GetProjectCaseCount(int fxtcompanyid, int cityid, int projectid, int purposecode, int buildingareatype, int buildingtypecode, DateTime startdate, DateTime enddate, int typecode)
        {
            string sql = SQL.Project.ProjectCaseCount;

            CityTable city = CityTableDA.GetCityTable(cityid);
            sql = sql.Replace("@casetable", city.casetable);

            SqlCommand command = new SqlCommand();
            command.CommandText = sql;

            command.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", fxtcompanyid, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@typecode", typecode, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", cityid, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@projectid", projectid, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@purposecode", purposecode, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@startdate", startdate.ToString("yyyy-MM-dd"), SqlDbType.DateTime));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@enddate", enddate.ToString("yyyy-MM-dd"), SqlDbType.DateTime));

            return StringHelper.TryGetInt(ExecuteScalar(command).ToString());
        }

        /// <summary>
        /// 获取单个楼盘近n个月案例总数 库晶晶20150210
        /// </summary>
        public static int GetCaseCountByProjectId_MCAS(int fxtcompanyid, int cityid, int projectid, int months, int typecode)
        {
            string sql = SQL.Project.GetCaseCountByProjectId_MCAS;
            CityTable city = CityTableDA.GetCityTable(cityid);
            if (null == city)
            {
                return 0;
            }
            sql = sql.Replace("@projecttable", city.ProjectTable);
            sql = sql.Replace("@projectsubtable", city.ProjectTable + "_sub");
            sql = sql.Replace("@casetable", city.casetable);

            SqlCommand command = new SqlCommand();
            command.CommandText = sql;
            command.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", fxtcompanyid, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@typecode", typecode, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", cityid, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@projectid", projectid, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@months", months, SqlDbType.Int));

            return StringHelper.TryGetInt(ExecuteScalar(command).ToString());
        }

        /// <summary>
        /// 获取多个楼盘近n个月案例总数 tanql20150922
        /// </summary>
        public static DataSet GetCaseCountByProjectIds_MCAS(int fxtcompanyid, int cityid, string projectids, int months, int typecode)
        {
            string sql = SQL.Project.GetCaseCountByProjectIds_MCAS;
            CityTable city = CityTableDA.GetCityTable(cityid);
            sql = sql.Replace("@projecttable", city.ProjectTable);
            sql = sql.Replace("@projectsubtable", city.ProjectTable + "_sub");
            sql = sql.Replace("@casetable", city.casetable);
            sql = sql.Replace("@projectids", projectids);

            SqlCommand command = new SqlCommand();
            command.CommandText = sql;
            command.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", fxtcompanyid, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@typecode", typecode, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", cityid, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@months", months, SqlDbType.Int));

            return ExecuteDataSet(command);
        }

        /// <summary>
        /// 获取单个楼盘坐标及照片总数 库晶晶20150210
        /// </summary>
        public static DataSet GetProjectListInfo_MCAS(int fxtcompanyid, int projectid, int cityid, int typecode)
        {
            string sql = SQL.Project.GetProjectListInfo_MCAS;
            CityTable city = CityTableDA.GetCityTable(cityid);
            if (null == city)
            {
                return null;
            }
            sql = sql.Replace("@projecttable", city.ProjectTable);
            sql = sql.Replace("@projectsubtable", city.ProjectTable + "_sub");

            SqlCommand command = new SqlCommand();
            command.CommandText = sql;
            command.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", fxtcompanyid, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@typecode", typecode, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", cityid, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@projectid", projectid, SqlDbType.Int));

            return ExecuteDataSet(command);
        }

        /// <summary>
        /// 获取住宅案例列表 库晶晶20150415
        /// </summary>
        public static DataSet GetCaseListNew(SearchBase search, int buildingtypecode, int purposecode, int casetypecode, DateTime casedatefrom, DateTime casedateto, decimal? buildingareafrom, decimal? buildingareato, string projectname, int isSource)
        {
            string sql = SQL.Project.GetCaseListNew;
            CityTable city = CityTableDA.GetCityTable(search.CityId);
            if (null == city)
            {
                return null;
            }
            sql = sql.Replace("@projecttable", city.ProjectTable);
            sql = sql.Replace("@projectsubtable", city.ProjectTable + "_sub");
            sql = sql.Replace("@casetable", city.casetable);

            SqlCommand command = new SqlCommand();
            command.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@casedatefrom", casedatefrom, SqlDbType.DateTime));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@casedateto", casedateto, SqlDbType.DateTime));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@pageindex", search.PageIndex, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@pagerecord", search.PageRecords, SqlDbType.Int));
            if (search.AreaId > 0)
            {
                search.Where += " and T.AreaID = @areaid";
                command.Parameters.Add(SqlHelper.GetSqlParameter("@areaid", search.AreaId, SqlDbType.Int));
            }
            if (buildingtypecode > 0)
            {
                search.Where += " and T.BuildingTypeCode = @buildingtypecode";
                command.Parameters.Add(SqlHelper.GetSqlParameter("@buildingtypecode", buildingtypecode, SqlDbType.Int));
            }
            if (purposecode > 0)
            {
                search.Where += " and T.PurposeCode = @purposecode";
                command.Parameters.Add(SqlHelper.GetSqlParameter("@purposecode", purposecode, SqlDbType.Int));
            }
            if (casetypecode > 0)
            {
                search.Where += " and T.CaseTypeCode = @casetypecode";
                command.Parameters.Add(SqlHelper.GetSqlParameter("@casetypecode", casetypecode, SqlDbType.Int));
            }
            if (!string.IsNullOrWhiteSpace(projectname))
            {
                search.Where += " and T.ProjectName like @projectname";
                command.Parameters.Add(SqlHelper.GetSqlParameter("@projectname", "%" + projectname.Trim() + "%", SqlDbType.NVarChar, 100));
            }
            if (buildingareafrom != null && buildingareafrom > 0)
            {
                search.Where += " and T.BuildingArea >= @buildingareafrom";
                command.Parameters.Add(SqlHelper.GetSqlParameter("@buildingareafrom", buildingareafrom, SqlDbType.Decimal));
            }
            if (buildingareato != null && buildingareato > 0)
            {
                search.Where += " and T.BuildingArea <= @buildingareato";
                command.Parameters.Add(SqlHelper.GetSqlParameter("@buildingareato", buildingareato, SqlDbType.Decimal));
            }

            if (isSource == 0)//全部
            {
            }
            else if (isSource == 1)//有案例来源
            {
                search.Where += " and (T.SourceName is not null and T.SourceName <> '') ";
            }
            else if (isSource == 2)//无案例来源
            {
                search.Where += " and (T.SourceName is null or T.SourceName = '') ";
            }

            //search.OrderBy = !string.IsNullOrEmpty(search.OrderBy) ? search.OrderBy : " casedate desc";
            //sql = HandleSQL(search, sql);
            if (string.IsNullOrEmpty(search.Where))
            {
                sql = sql.Replace("$where","");
            }
            else
            {
                sql = sql.Replace("$where", search.Where);
            }
            command.CommandText = sql;
            return ExecuteDataSet(command);
        }

        /// <summary>
        /// 获取住宅案例最高单价、最低单价、平均单价 库晶晶20150416
        /// </summary>
        public static DataSet GetCasePrice(SearchBase search, int buildingtypecode, int purposecode, int casetypecode, DateTime casedatefrom, DateTime casedateto, decimal? buildingareafrom, decimal? buildingareato, string projectname, int isSource)
        {
            string sql = SQL.Project.GetCaseListNew;
            CityTable city = CityTableDA.GetCityTable(search.CityId);
            if (null == city)
            {
                return null;
            }
            sql = sql.Replace("@projecttable", city.ProjectTable);
            sql = sql.Replace("@projectsubtable", city.ProjectTable + "_sub");
            sql = sql.Replace("@casetable", city.casetable);

            string where = string.Empty;
            string where1 = " and CaseTypeCode in (3001001,3001002,3001003,3001004,3001005)";
            SqlCommand command = new SqlCommand();
            command.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@casedatefrom", casedatefrom, SqlDbType.DateTime));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@casedateto", casedateto, SqlDbType.DateTime));
            if (search.AreaId > 0)
            {
                where += " and T.AreaID = @areaid";
                command.Parameters.Add(SqlHelper.GetSqlParameter("@areaid", search.AreaId, SqlDbType.Int));
            }
            if (buildingtypecode > 0)
            {
                where += " and T.BuildingTypeCode = @buildingtypecode";
                command.Parameters.Add(SqlHelper.GetSqlParameter("@buildingtypecode", buildingtypecode, SqlDbType.Int));
            }
            if (purposecode > 0)
            {
                where += " and T.PurposeCode = @purposecode";
                command.Parameters.Add(SqlHelper.GetSqlParameter("@purposecode", purposecode, SqlDbType.Int));
            }
            if (casetypecode > 0)
            {
                where += " and T.CaseTypeCode = @casetypecode";
                where1 = " and CaseTypeCode = @casetypecode";
                command.Parameters.Add(SqlHelper.GetSqlParameter("@casetypecode", casetypecode, SqlDbType.Int));
            }
            if (!string.IsNullOrEmpty(projectname.Trim()))
            {
                where += " and T.ProjectName like @projectname";
                command.Parameters.Add(SqlHelper.GetSqlParameter("@projectname", "%" + projectname.Trim() + "%", SqlDbType.NVarChar, 100));
            }
            if (buildingareafrom != null && buildingareafrom > 0)
            {
                where += " and T.BuildingArea >= @buildingareafrom";
                command.Parameters.Add(SqlHelper.GetSqlParameter("@buildingareafrom", buildingareafrom, SqlDbType.Decimal));
            }
            if (buildingareato != null && buildingareato > 0)
            {
                where += " and T.BuildingArea <= @buildingareato";
                command.Parameters.Add(SqlHelper.GetSqlParameter("@buildingareato", buildingareato, SqlDbType.Decimal));
            }

            if (isSource == 0)//全部
            {
            }
            else if (isSource == 1)//有案例来源
            {
                where += " and (T.SourceName is not null and T.SourceName <> '') ";
            }
            else if (isSource == 2)//无案例来源
            {
                where += " and (T.SourceName is null or T.SourceName = '') ";
            }

            sql += where;

            sql = @"select 
	maxunitprice
	,minunitprice
	,case when totalbuildingare > 0 then CONVERT(numeric(18,0),totalprice / totalbuildingare) else 0 end as avgprice
from (
	select 
		MAX(UnitPrice) as maxunitprice
		,MIN(UnitPrice) as minunitprice
		,SUM(case when BuildingArea > 0 and UnitPrice > 0 then BuildingArea * UnitPrice else 0 end) as totalprice
		,SUM(case when BuildingArea > 0 and UnitPrice > 0 then BuildingArea else 0 end) as totalbuildingare
	from (	" + sql + @"
	)T
	where 1 = 1" + where1 + @"
)T";
            command.CommandText = sql;
            return ExecuteDataSet(command);
        }

        /// <summary>
        /// 获取住宅案例最高单价、最低单价、平均单价
        /// </summary>
        public static DataSet GetCasePriceModify(SearchBase search, int buildingtypecode, int purposecode, int casetypecode, DateTime casedatefrom, DateTime casedateto, decimal? buildingareafrom, decimal? buildingareato, string projectname, int isSource)
        {
            string sql = SQL.Project.GetCasePrice;
            CityTable city = CityTableDA.GetCityTable(search.CityId);
            if (null == city)
            {
                return null;
            }
            sql = sql.Replace("@projecttable", city.ProjectTable);
            sql = sql.Replace("@projectsubtable", city.ProjectTable + "_sub");
            sql = sql.Replace("@casetable", city.casetable);
            SqlCommand command = new SqlCommand();
            command.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@casedatefrom", casedatefrom, SqlDbType.DateTime));
            command.Parameters.Add(SqlHelper.GetSqlParameter("@casedateto", casedateto, SqlDbType.DateTime));
            string projectlimitStr = string.Empty;
            string caselimitStr = string.Empty;
            if (search.AreaId > 0)
            {
                projectlimitStr += " and p.AreaID = @areaid";
                command.Parameters.Add(SqlHelper.GetSqlParameter("@areaid", search.AreaId, SqlDbType.Int));
            }
            if (buildingtypecode > 0)
            {
                projectlimitStr += " and p.BuildingTypeCode = @buildingtypecode";
                command.Parameters.Add(SqlHelper.GetSqlParameter("@buildingtypecode", buildingtypecode, SqlDbType.Int));
            }
            if (purposecode > 0)
            {
                projectlimitStr += " and p.PurposeCode = @purposecode";
                command.Parameters.Add(SqlHelper.GetSqlParameter("@purposecode", purposecode, SqlDbType.Int));
            }
            if (!string.IsNullOrEmpty(projectname.Trim()))
            {
                projectlimitStr += " and p.ProjectName like @projectname";
                command.Parameters.Add(SqlHelper.GetSqlParameter("@projectname", "%" + projectname.Trim() + "%", SqlDbType.NVarChar, 100));
            }
            if (isSource == 1)//有案例来源
            {
                projectlimitStr += " and (p.SourceName is not null and p.SourceName <> '')";
            }
            else if (isSource == 2)//无案例来源
            {
                projectlimitStr += " and (p.SourceName is null or p.SourceName = '')";
            }
            if (casetypecode > 0)
            {
                caselimitStr = "and c.CaseTypeCode = @casetypecode";
                command.Parameters.Add(SqlHelper.GetSqlParameter("@casetypecode", casetypecode, SqlDbType.Int));
            }
            else
            {
                caselimitStr = "and c.CaseTypeCode in (3001001,3001002,3001003,3001004,3001005)";
            }
            if (buildingareafrom != null && buildingareafrom > 0)
            {
                caselimitStr += " and c.BuildingArea >= @buildingareafrom";
                command.Parameters.Add(SqlHelper.GetSqlParameter("@buildingareafrom", buildingareafrom, SqlDbType.Decimal));
            }
            if (buildingareato != null && buildingareato > 0)
            {
                caselimitStr += " and c.BuildingArea <= @buildingareato";
                command.Parameters.Add(SqlHelper.GetSqlParameter("@buildingareato", buildingareato, SqlDbType.Decimal));
            }
            sql = sql.Replace("$projectlimit", projectlimitStr).Replace("$caselimit", caselimitStr);
            command.CommandText = sql;
            return ExecuteDataSet(command);
        }
    }
}
