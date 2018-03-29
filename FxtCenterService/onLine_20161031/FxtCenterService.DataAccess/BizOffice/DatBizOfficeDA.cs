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
using CAS.Entity.FxtDataCenter;

namespace FxtCenterService.DataAccess
{
    public class DatBizOfficeDA : Base
    {
        /// <summary>
        /// 获取商业案例列表
        /// </summary>
        public static List<DatCaseBiz> GetCaseListBiz(SearchBase search, string projectname, decimal minBuildingArea, decimal maxBuildingArea, decimal minUnitPrice, decimal maxUnitPrice, int caseTypeCode, int areaid, int subareaid, DateTime startCaseDate, DateTime endCaseDate, int structurecode, int iselevator, string structurecodename)
        {
            string sql = SQL.BizOffice.CaseBiz;
            List<SqlParameter> parameters = new List<SqlParameter>();

            CAS.Entity.DBEntity.CityTable city = CityTableDA.GetCityTable(search.CityId);
            sql = sql.Replace("@buildingtable", city.BuildingTable);

            if (0 < minBuildingArea)
            {
                search.Where += " and cb.BuildingArea >= " + minBuildingArea;
            }
            if (0 < maxBuildingArea)
            {
                search.Where += " and cb.BuildingArea <= " + maxBuildingArea;
            }
            if (0 < minUnitPrice)
            {
                search.Where += " and cb.UnitPrice >= " + minUnitPrice;
            }
            if (0 < maxUnitPrice)
            {
                search.Where += " and cb.UnitPrice <= " + maxUnitPrice;
            }
            //if (!string.IsNullOrEmpty(address))
            //{
            //    search.Where += " and cb.Address like @Address";
            //    parameters.Add(SqlHelper.GetSqlParameter("@Address", "%" + SQLFilterHelper.EscapeLikeString(address, "$") + "%", SqlDbType.NVarChar));
            //}
            if (caseTypeCode > 0)
            {
                search.Where += " and cb.CaseTypeCode = @caseTypeCode";
                parameters.Add(SqlHelper.GetSqlParameter("@caseTypeCode", caseTypeCode, SqlDbType.Int));
            }
            if (0 < areaid)
            {
                search.Where += " and cb.AreaId = " + areaid;
            }
            if (0 < subareaid)
            {
                search.Where += " and cb.SubAreaId = " + subareaid;
            }

            if (default(DateTime) != startCaseDate)
            {
                search.Where += " and cb.casedate >= '" + startCaseDate + "'";
            }
            if (default(DateTime) != endCaseDate)
            {
                search.Where += " and cb.casedate <= '" + endCaseDate + "'";
            }

            //建筑结构
            if (structurecode > 0)
            {
                search.Where += " and b.StructureCode = '" + structurecode + "'";
            }
            else if (!string.IsNullOrEmpty(structurecodename))
            {
                var code = SYSCodeDA.GetCode(2010, structurecodename);
                if (code != null)
                {
                    search.Where += " and b.StructureCode = '" + code.code + "'";
                }
                else {
                    return new List<DatCaseBiz>();
                }
            }

            //有电梯
            if (iselevator == 1)
            {
                search.Where += " and b.IsElevator = " + iselevator + " ";
            }

            //无电梯
            if (iselevator == 2)
            {
                search.Where += " and b.IsElevator = " + 0 + " ";
            }

            search.Where += " and cb.ProjectName like @projectname";
            parameters.Add(SqlHelper.GetSqlParameter("@projectname", "%" + SQLFilterHelper.EscapeLikeString(projectname, "$") + "%", SqlDbType.NVarChar));
            parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));

            search.OrderBy = " CaseDate desc";
            sql = HandleSQL(search, sql);
            return ExecuteToEntityList<DatCaseBiz>(sql, System.Data.CommandType.Text, parameters);
        }
        /// <summary>
        /// 获取商业案例列表forMCAS
        /// </summary>
        public static List<DatCaseBiz> GetCaseListBiz_MCAS(SearchBase search, int areaid, int subareaid, int casetype, DateTime casestatime, DateTime caseendtime, string projectname, decimal? buildstaarea, decimal? buildendarea, decimal? pricesta, decimal? priceend)
        {
            string sql = SQL.BizOffice.CaseBiz_MCAS;

            search.Where += " and (@casetype = 0 and cb.CaseTypeCode in (3001001,3001002,3001003,3001004,3001005) or (@casetype = 1 and cb.CaseTypeCode in (3001006,3001007,3001008,3001009)))";
            search.Where += " and cb.CaseDate between @casestatime and @caseendtime + 1";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@casetype", casetype, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@casestatime", casestatime, SqlDbType.DateTime));
            parameters.Add(SqlHelper.GetSqlParameter("@caseendtime", caseendtime, SqlDbType.DateTime));
            if (areaid > 0)
            {
                search.Where += " and cb.AreaId = @areaid";
                parameters.Add(SqlHelper.GetSqlParameter("@areaid", areaid, SqlDbType.Int));
            }
            if (subareaid > 0)
            {
                search.Where += " and cb.SubAreaId = @subareaid";
                parameters.Add(SqlHelper.GetSqlParameter("@subareaid", subareaid, SqlDbType.Int));
            }
            if (!string.IsNullOrEmpty(projectname.Trim()))
            {
                search.Where += " and cb.ProjectName like @projectname";
                parameters.Add(SqlHelper.GetSqlParameter("@projectname", "%" + projectname.Trim() + "%", SqlDbType.NVarChar, 100));
            }
            if (buildstaarea != null && buildstaarea > 0)
            {
                search.Where += " and cb.BuildingArea >= @buildstaarea";
                parameters.Add(SqlHelper.GetSqlParameter("@buildstaarea", buildstaarea, SqlDbType.Decimal));
            }
            if (buildendarea != null && buildendarea > 0)
            {
                search.Where += " and cb.BuildingArea <= @buildendarea";
                parameters.Add(SqlHelper.GetSqlParameter("@buildendarea", buildendarea, SqlDbType.Decimal));
            }
            if (pricesta != null && pricesta > 0)
            {
                search.Where += " and cb.UnitPrice >= @pricesta";
                parameters.Add(SqlHelper.GetSqlParameter("@pricesta", pricesta, SqlDbType.Decimal));
            }
            if (priceend != null && priceend > 0)
            {
                search.Where += " and cb.UnitPrice <= @priceend";
                parameters.Add(SqlHelper.GetSqlParameter("@priceend", priceend, SqlDbType.Decimal));
            }
            search.OrderBy = !string.IsNullOrEmpty(search.OrderBy) ? search.OrderBy : " casedate desc";
            sql = HandleSQL(search, sql);
            return ExecuteToEntityList<DatCaseBiz>(sql, System.Data.CommandType.Text, parameters);
        }
        /// <summary>
        /// 获取商业案例详情
        /// </summary>
        public static DataTable GetCaseInfoBiz(SearchBase search, int[] ids)
        {
            SqlCommand comm = new SqlCommand();
            string sql = SQL.BizOffice.CaseBizInfo;
            CAS.Entity.DBEntity.CityTable city = CityTableDA.GetCityTable(search.CityId);
            sql = sql.Replace("@buildingtable", city.BuildingTable);

            string strwhere = "";
            comm.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
            comm.Parameters.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
            comm.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));

            if (null != ids && 0 < ids.Length)
            {
                search.Where += " and cb.Id in (" + string.Join(",", ids) + ")";
            }
            sql += strwhere;
            sql = HandleSQL(search, sql);
            comm.CommandText = sql;
            return ExecuteDataSet(comm).Tables[0];
        }
        /// <summary>
        /// 获取商业案例详情forMCAS
        /// </summary>
        public static List<DatCaseBiz> GetCaseInfoBiz_MCAS(SearchBase search, int id)
        {
            string sql = SQL.BizOffice.CaseBiz_MCAS;
            string strwhere = "";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));
            if (id > 0)
            {
                strwhere += " and cb.Id = @id";
                parameters.Add(SqlHelper.GetSqlParameter("@id", id, SqlDbType.Int));
            }
            sql += strwhere;
            sql = HandleSQL(search, sql);
            return ExecuteToEntityList<DatCaseBiz>(sql, System.Data.CommandType.Text, parameters);
        }
        /// <summary>
        /// 获取办公案例列表
        /// </summary>
        public static List<DatCaseOff> GetCaseListOffice(SearchBase search, string projectname, decimal minBuildingArea, decimal maxBuildingArea, decimal minUnitPrice, decimal maxUnitPrice, int officeType, int purposeCode, int caseTypeCode, int areaid, int subareaid, DateTime startCaseDate, DateTime endCaseDate, int structurecode, int iselevator,string structurecodename)
        {
            string sql = SQL.BizOffice.CaseOfficeInfo;
            List<SqlParameter> parameters = new List<SqlParameter>();

            CAS.Entity.DBEntity.CityTable city = CityTableDA.GetCityTable(search.CityId);
            sql = sql.Replace("@buildingtable", city.BuildingTable);

            if (0 < minBuildingArea)
            {
                search.Where += " and co.BuildingArea >= " + minBuildingArea;
            }
            if (0 < maxBuildingArea)
            {
                search.Where += " and co.BuildingArea <= " + maxBuildingArea;
            }
            if (0 < minUnitPrice)
            {
                search.Where += " and co.UnitPrice >= " + minUnitPrice;
            }
            if (0 < maxUnitPrice)
            {
                search.Where += " and co.UnitPrice <= " + maxUnitPrice;
            }
            //if (!string.IsNullOrEmpty(address))
            //{
            //    search.Where += " and p.Address like @Address";
            //    parameters.Add(SqlHelper.GetSqlParameter("@Address", "%" + SQLFilterHelper.EscapeLikeString(address, "$") + "%", SqlDbType.NVarChar));
            //}
            if (officeType > 0)
            {
                search.Where += " and co.OfficeType = @officeType";
                parameters.Add(SqlHelper.GetSqlParameter("@officeType", officeType, SqlDbType.Int));
            }
            if (purposeCode > 0)
            {
                search.Where += " and p.PurposeCode = @purposeCode";
                parameters.Add(SqlHelper.GetSqlParameter("@purposeCode", purposeCode, SqlDbType.Int));
            }
            if (caseTypeCode > 0)
            {
                search.Where += " and co.CaseTypeCode = @caseTypeCode";
                parameters.Add(SqlHelper.GetSqlParameter("@caseTypeCode", caseTypeCode, SqlDbType.Int));
            }
            if (0 < areaid)
            {
                search.Where += " and p.AreaId = " + areaid;
            }
            if (0 < subareaid)
            {
                search.Where += " and p.SubAreaId = " + subareaid;
            }

            if (default(DateTime) != startCaseDate)
            {
                search.Where += " and co.casedate >= '" + startCaseDate + "'";
            }
            if (default(DateTime) != endCaseDate)
            {
                search.Where += " and co.casedate <= '" + endCaseDate + "'";
            }

            //建筑结构
            if (structurecode > 0)
            {
                search.Where += " and b.StructureCode = '" + structurecode + "'";
            }
            else if (!string.IsNullOrEmpty(structurecodename))
            {
                var code = SYSCodeDA.GetCode(2010, structurecodename);
                if (code != null)
                {
                    search.Where += " and b.StructureCode = '" + code.code + "'";
                }
                else {
                    return new List<DatCaseOff>();
                }
            }

            //有电梯
            if (iselevator == 1)
            {
                search.Where += " and b.IsElevator = " + iselevator + " ";
            }

            //无电梯
            if (iselevator == 2)
            {
                search.Where += " and b.IsElevator = " + 0 + " ";
            }

            search.Where += " and co.ProjectName like @projectname";
            parameters.Add(SqlHelper.GetSqlParameter("@projectname", "%" + SQLFilterHelper.EscapeLikeString(projectname, "$") + "%", SqlDbType.NVarChar));
            parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));

            search.OrderBy = " CaseDate desc";
            sql = HandleSQL(search, sql);
            return ExecuteToEntityList<DatCaseOff>(sql, System.Data.CommandType.Text, parameters);
        }

        /// <summary>
        /// 获取办公案例列表forMCAS
        /// </summary>
        public static List<DatCaseOff> GetCaseListOffice_MCAS(SearchBase search, int areaid, int subareaid, int casetype, DateTime casestatime, DateTime caseendtime, string projectname, decimal? buildstaarea, decimal? buildendarea, decimal? pricesta, decimal? priceend)
        {
            string sql = SQL.BizOffice.CaseOffice_MCAS;

            search.Where += " and (@casetype = 0 and co.CaseTypeCode in (3001001,3001002,3001003,3001004,3001005) or (@casetype = 1 and co.CaseTypeCode in (3001006,3001007,3001008,3001009)))";
            search.Where += " and co.CaseDate between @casestatime and @caseendtime + 1";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@casetype", casetype, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@casestatime", casestatime, SqlDbType.DateTime));
            parameters.Add(SqlHelper.GetSqlParameter("@caseendtime", caseendtime, SqlDbType.DateTime));
            if (areaid > 0)
            {
                search.Where += " and p.AreaId = @areaid";
                parameters.Add(SqlHelper.GetSqlParameter("@areaid", areaid, SqlDbType.Int));
            }
            if (subareaid > 0)
            {
                search.Where += " and p.SubAreaId = @subareaid";
                parameters.Add(SqlHelper.GetSqlParameter("@subareaid", subareaid, SqlDbType.Int));
            }
            if (!string.IsNullOrEmpty(projectname.Trim()))
            {
                search.Where += " and co.ProjectName like @projectname";
                parameters.Add(SqlHelper.GetSqlParameter("@projectname", "%" + projectname.Trim() + "%", SqlDbType.NVarChar, 100));
            }
            if (buildstaarea != null && buildstaarea > 0)
            {
                search.Where += " and co.BuildingArea >= @buildstaarea";
                parameters.Add(SqlHelper.GetSqlParameter("@buildstaarea", buildstaarea, SqlDbType.Decimal));
            }
            if (buildendarea != null && buildendarea > 0)
            {
                search.Where += " and co.BuildingArea <= @buildendarea";
                parameters.Add(SqlHelper.GetSqlParameter("@buildendarea", buildendarea, SqlDbType.Decimal));
            }
            if (pricesta != null && pricesta > 0)
            {
                search.Where += " and co.UnitPrice >= @pricesta";
                parameters.Add(SqlHelper.GetSqlParameter("@pricesta", pricesta, SqlDbType.Decimal));
            }
            if (priceend != null && priceend > 0)
            {
                search.Where += " and co.UnitPrice <= @priceend";
                parameters.Add(SqlHelper.GetSqlParameter("@priceend", priceend, SqlDbType.Decimal));
            }
            search.OrderBy = !string.IsNullOrEmpty(search.OrderBy) ? search.OrderBy : " casedate desc";
            sql = HandleSQL(search, sql);
            return ExecuteToEntityList<DatCaseOff>(sql, System.Data.CommandType.Text, parameters);
        }

        /// <summary>
        /// 获取办公案例详情
        /// </summary>
        public static DataTable GetCaseInfoOffice(SearchBase search, int[] ids)
        {
            SqlCommand comm = new SqlCommand();
            string sql = SQL.BizOffice.CaseOfficeInfo;

            CAS.Entity.DBEntity.CityTable city = CityTableDA.GetCityTable(search.CityId);
            sql = sql.Replace("@buildingtable", city.BuildingTable);

            string strwhere = "";
            comm.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
            comm.Parameters.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
            comm.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));

            if (null != ids && 0 < ids.Length)
            {
                search.Where += " and co.Id in (" + string.Join(",", ids) + ")";
            }
            sql += strwhere;
            sql = HandleSQL(search, sql);
            comm.CommandText = sql;
            return ExecuteDataSet(comm).Tables[0];
        }

        /// <summary>
        /// 获取土地案例列表
        /// </summary>
        public static List<DatCaseLand> GetCaseListLand(SearchBase search, string landno,string landclassname,string landclasscode, decimal minBuildingArea, decimal maxBuildingArea, decimal minUnitPrice, decimal maxUnitPrice, string landPurposeDesc, int areaid, int subareaid, DateTime startCaseDate, DateTime endCaseDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.BizOffice.CaseLandInfo;

            if (0 < minBuildingArea)
            {
                search.Where += " and cl.LandArea >= " + minBuildingArea;
            }
            if (0 < maxBuildingArea)
            {
                search.Where += " and cl.LandArea <= " + maxBuildingArea;
            }
            if (0 < minUnitPrice)
            {
                search.Where += " and cl.LandUnitPrice >= " + minUnitPrice;
            }
            if (0 < maxUnitPrice)
            {
                search.Where += " and cl.LandUnitPrice <= " + maxUnitPrice;
            }
            if (!string.IsNullOrWhiteSpace(landPurposeDesc))
            {
                search.Where += " and cl.LandPurposeDesc like @landPurposeDesc";
                parameters.Add(SqlHelper.GetSqlParameter("@landPurposeDesc", "%" + SQLFilterHelper.EscapeLikeString(landPurposeDesc, "$") + "%", SqlDbType.NVarChar));
            }
            if (!string.IsNullOrWhiteSpace(landclassname))
            {
                search.Where += " and c3.CodeType = '土地等级' and c3.CodeName = @landclassname";
                parameters.Add(SqlHelper.GetSqlParameter("@landclassname", landclassname, SqlDbType.NVarChar));
            }
            if (!string.IsNullOrWhiteSpace(landclasscode))
            {
                search.Where += " and cl.LandClass = @landclasscode";
                parameters.Add(SqlHelper.GetSqlParameter("@landclasscode", landclasscode, SqlDbType.NVarChar));
            }
            if (0 < areaid)
            {
                search.Where += " and cl.AreaID = " + areaid;
            }
            if (0 < subareaid)
            {
                search.Where += " and cl.SubAreaId = " + subareaid;
            }

            if (default(DateTime) != startCaseDate)
            {
                search.Where += " and cl.casedate >= '" + startCaseDate + "'";
            }
            if (default(DateTime) != endCaseDate)
            {
                search.Where += " and cl.casedate <= '" + endCaseDate + "'";
            }

            search.Where += " and cl.landno like @landno";
            parameters.Add(SqlHelper.GetSqlParameter("@landno", "%" + SQLFilterHelper.EscapeLikeString(landno, "$") + "%", SqlDbType.NVarChar));
            parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));
            search.OrderBy = " CaseDate desc";
            sql = HandleSQL(search, sql);
            return ExecuteToEntityList<DatCaseLand>(sql, System.Data.CommandType.Text, parameters);
        }

        /// <summary>
        /// 获取土地案例详情
        /// </summary>
        public static DataTable GetCaseInfoLand(SearchBase search, int[] ids)
        {
            SqlCommand comm = new SqlCommand();
            string sql = SQL.BizOffice.CaseLandInfo;
            comm.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
            comm.Parameters.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
            comm.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));

            if (null != ids && 0 < ids.Length)
            {
                search.Where += " and cl.CaseID in (" + string.Join(",", ids) + ")";
            }
            sql = HandleSQL(search, sql);
            comm.CommandText = sql;
            return ExecuteDataSet(comm).Tables[0];
        }

        /// <summary>
        /// 获取住宅案例详情
        /// </summary>
        public static DataTable GetCaseInfo(SearchBase search, int[] ids)
        {
            SqlCommand comm = new SqlCommand();
            string sql = SQL.BizOffice.CaseInfo;
            CAS.Entity.DBEntity.CityTable city = CityTableDA.GetCityTable(search.CityId);
            if (null == city)
                return new DataTable();
            sql = sql.Replace("@projecttable", city.ProjectTable);
            sql = sql.Replace("@projectsubtable", city.ProjectTable + "_sub");
            sql = sql.Replace("@casetable", city.casetable);
            sql = sql.Replace("@buildingtable", city.BuildingTable);

            comm.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
            comm.Parameters.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
            comm.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));

            if (null != ids && 0 < ids.Length)
            {
                search.Where += " and c.CaseID in (" + string.Join(",", ids) + ")";
            }
            sql = HandleSQL(search, sql);
            comm.CommandText = sql;
            return ExecuteDataSet(comm).Tables[0];
        }

        /// <summary>
        /// 获取工业案例列表
        /// </summary>
        public static List<DatCaseIndustry> GetCaseListIndustry(SearchBase search, string projectName, decimal minBuildingArea, decimal maxBuildingArea, decimal minUnitPrice, decimal maxUnitPrice, int purposeCode, int caseTypeCode, int areaid, int subareaid, DateTime startCaseDate, DateTime endCaseDate, int structurecode, int iselevator, string structurecodename)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.BizOffice.CaseIndustryInfo;

            CAS.Entity.DBEntity.CityTable city = CityTableDA.GetCityTable(search.CityId);
            sql = sql.Replace("@buildingtable", city.BuildingTable);

            if (0 < minBuildingArea)
            {
                search.Where += " and ci.BuildingArea >= " + minBuildingArea;
            }
            if (0 < maxBuildingArea)
            {
                search.Where += " and ci.BuildingArea <= " + maxBuildingArea;
            }
            if (0 < minUnitPrice)
            {
                search.Where += " and ci.UnitPrice >= " + minUnitPrice;
            }
            if (0 < maxUnitPrice)
            {
                search.Where += " and ci.UnitPrice <= " + maxUnitPrice;
            }
            if (purposeCode > 0)
            {
                search.Where += " and p.PurposeCode = @purposeCode";
                parameters.Add(SqlHelper.GetSqlParameter("@purposeCode", purposeCode, SqlDbType.Int));
            }
            if (caseTypeCode > 0)
            {
                search.Where += " and ci.CaseTypeCode = @caseTypeCode";
                parameters.Add(SqlHelper.GetSqlParameter("@caseTypeCode", caseTypeCode, SqlDbType.Int));
            }
            if (0 < areaid)
            {
                search.Where += " and ci.AreaId = " + areaid;
            }
            if (0 < subareaid)
            {
                search.Where += " and ci.SubAreaId = " + subareaid;
            }

            if (default(DateTime) != startCaseDate)
            {
                search.Where += " and ci.casedate >= '" + startCaseDate + "'";
            }
            if (default(DateTime) != endCaseDate)
            {
                search.Where += " and ci.casedate <= '" + endCaseDate + "'";
            }

            //建筑结构
            if (structurecode > 0)
            {
                search.Where += " and b.StructureCode = '" + structurecode + "'";
            }
            else if (!string.IsNullOrEmpty(structurecodename))
            {
                var code = SYSCodeDA.GetCode(2010, structurecodename);
                if (code != null)
                {
                    search.Where += " and b.StructureCode = '" + code.code + "'";
                }
                else {
                    return new List<DatCaseIndustry>();
                }
            }

            //有电梯
            if (iselevator == 1)
            {
                search.Where += " and b.IsElevator = " + iselevator + " ";
            }

            //无电梯
            if (iselevator == 2)
            {
                search.Where += " and b.IsElevator = " + 0 + " ";
            }

            search.Where += " and ci.ProjectName like @projectName";
            parameters.Add(SqlHelper.GetSqlParameter("@projectName", "%" + SQLFilterHelper.EscapeLikeString(projectName, "$") + "%", SqlDbType.NVarChar));
            parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));
            search.OrderBy = " CaseDate desc";
            sql = HandleSQL(search, sql);
            return ExecuteToEntityList<DatCaseIndustry>(sql, System.Data.CommandType.Text, parameters);
        }

        /// <summary>
        /// 获取工业案例详情
        /// </summary>
        public static DataTable GetCaseInfoIndustry(SearchBase search, int[] ids)
        {
            SqlCommand comm = new SqlCommand();
            string sql = SQL.BizOffice.CaseIndustryInfo;
            CAS.Entity.DBEntity.CityTable city = CityTableDA.GetCityTable(search.CityId);
            sql = sql.Replace("@buildingtable", city.BuildingTable);

            comm.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
            comm.Parameters.Add(SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int));
            comm.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));

            if (null != ids && 0 < ids.Length)
            {
                search.Where += " and ci.Id in (" + string.Join(",", ids) + ")";
            }
            sql = HandleSQL(search, sql);
            comm.CommandText = sql;
            return ExecuteDataSet(comm).Tables[0];
        }
    }
}
