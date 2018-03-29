using System;
using System.Linq;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using System.Data;
using Dapper;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class CityAvgPrice : ICityAvgPrice
    {
        public IQueryable<DAT_AvgPrice_Month> GetCityAvgPrices(DateTime caseDateFrom, DateTime caseDateTo, int areaId, int cityId, int pageSize, int pageIndex, out int totalCount)
        {
            var dateFrom = caseDateFrom.ToString("yyyy-MM-dd");
            var dateTo = caseDateTo.ToString("yyyy-MM-dd");

            var sql = @"SELECT Convert(VARCHAR(4), year(AvgPriceDate), 120) + (
		                CASE len(month(AvgPriceDate))
			                WHEN 1
				                THEN '-0' + convert(VARCHAR(2), month(AvgPriceDate), 120)
			                ELSE '-' + convert(VARCHAR(2), month(AvgPriceDate), 120)
			                END
		                ) AS AvgPriceDate
	                ,AvgPrice
	                ,id
	                ,areaid
                FROM FXTProject.dbo.DAT_AvgPrice_Month WITH (NOLOCK)
                WHERE CityId = @cityId
	                AND projectid = 0
	                AND subareaid = 0
                    and areaId = @areaId
	                and AvgPriceDate between @dateFrom and @dateTo ";


            //分页SQL
            var pagenatedSql = @"select top " + pageSize + @" tt.*
                                from (
	                                select row_number() over (
			                                order by t.AvgPriceDate desc
			                                ) rownumber
		                                ,t.*
	                                from (" + sql + @") t ) tt
                                where tt.rownumber > (" + pageIndex + @" - 1) * " + pageSize;

            //总条数SQL
            var totalCountSql = "select count(1) from (" + sql + ") as t1";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                totalCount = conn.Query<int>(totalCountSql, new { areaId, cityId, dateFrom, dateTo }).FirstOrDefault();
                return conn.Query<DAT_AvgPrice_Month>(pagenatedSql, new { areaId, cityId, dateFrom, dateTo }).AsQueryable();
            }

        }

        public DAT_AvgPrice_Month GetCityAvgPrice(int id)
        {
            var sql = @"SELECT Convert(VARCHAR(4), year(AvgPriceDate), 120) + (
		                CASE len(month(AvgPriceDate))
			                WHEN 1
				                THEN '-0' + convert(VARCHAR(2), month(AvgPriceDate), 120)
			                ELSE '-' + convert(VARCHAR(2), month(AvgPriceDate), 120)
			                END
		                ) AS AvgPriceDate
	                ,AvgPrice
	                ,id
	                ,areaid
                FROM FXTProject.dbo.DAT_AvgPrice_Month WITH (NOLOCK)
                WHERE id = @id";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<DAT_AvgPrice_Month>(sql, new { id }).AsQueryable().FirstOrDefault();
            }
        }

        public DAT_AvgPrice_Month IsExistCityAvgPrice(int cityId, int areaId, DateTime AvgPriceDate)
        {
            var sql = @"select * from fxtproject.dbo.DAT_AvgPrice_Month where CityId = @cityId and AreaId = @areaId and AvgPriceDate = @AvgPriceDate order by Id desc";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<DAT_AvgPrice_Month>(sql, new { cityId, areaId, AvgPriceDate }).AsQueryable().FirstOrDefault();
            }
        }

        public int AddCityAvgPrice(DAT_AvgPrice_Month avgPrice)
        {
            const string sql = @"INSERT INTO FXTProject.dbo.DAT_AvgPrice_Month([CityId],[AreaId],[SubAreaId],[ProjectId],[AvgPriceDate],[AvgPrice])VALUES (@CityId,@AreaId,@SubAreaId,@ProjectId,@AvgPriceDate,@AvgPrice)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(sql, avgPrice);
            }
        }

        public int UpdateAvgPrice(DAT_AvgPrice_Month avgPrice)
        {
            const string sql = "Update FXTProject.dbo.DAT_AvgPrice_Month set AvgPrice = @AvgPrice where id=@id";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(sql, avgPrice);
            }
        }

        public int DeleteAvgPrice(int id)
        {
            const string sql = "delete from FXTProject.dbo.DAT_AvgPrice_Month  where id=@id";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(sql, new { id });
            }
        }


        public DAT_AvgPrice_Month GetAvgPrice(DAT_AvgPrice_Month avgPrice)
        {
            var access = Access(avgPrice.CityId, avgPrice.FxtCompanyId);
            var projectTable = access.Item1;
            var caseTable = access.Item2;
            var showcomId = access.Item3;
            var casecomId = access.Item4;

            //var sql = "SELECT  sum(c.UnitPrice*c.BuildingArea) /sum(c.BuildingArea) as AvgPrice from " + caseTable + " C," + projectTable + " p where C.Valid = 1 and c.ProjectId=p.ProjectId and casedate between @dateFrom and @dateTo and c.CaseTypeCode in(3001001,3001002,3001003) and c.PurposeCode in(1002001,1002002,1002003,1002004,1002011,1002012,1002013) and p.ProjectId not in(select ProjectId from DAT_AvgPrice_Day cc where cc.CityId=p.CityId and cc.ProjectId=c.ProjectId and cc.BuildingAreaType=0) ";
            var sql = @"
SELECT sum(c.UnitPrice * c.BuildingArea) / sum(c.BuildingArea) AS AvgPrice
FROM (
	select ProjectId,AreaID from " + projectTable + @" p with(nolock)
	where not exists(
		select ProjectId from " + projectTable + @"_sub ps with(nolock)
		where ps.ProjectId = p.ProjectId
		and ps.CityID = @cityid 
		and ps.Fxt_CompanyId = @fxtcompanyid
	)
	and Valid = 1
	and CityID = @cityid
	and FxtCompanyId in (" + showcomId + @")
	union
	select ProjectId,AreaID from " + projectTable + @"_sub p with(nolock)
	where Valid = 1
	and CityID = @cityid
	and Fxt_CompanyId = @fxtcompanyid
)T 
inner join (
	select * from " + caseTable + @" with(nolock)
	where Valid = 1
	and CityID = @cityid
	and FXTCompanyId in (" + casecomId + @")
	and BuildingArea > 0
	and UnitPrice > 0
	and casedate BETWEEN @dateFrom AND @dateTo
	and CaseTypeCode IN (3001001,3001002,3001003)
	and PurposeCode IN (1002001,1002002,1002003,1002004,1002011,1002012,1002013)
)C
on C.ProjectId = T.ProjectId
WHERE 1 = 1
	AND T.ProjectId NOT IN (
		SELECT ProjectId
		FROM DAT_AvgPrice_Day cc
		WHERE cc.CityId = @cityid 
			AND cc.ProjectId = c.ProjectId
			AND cc.BuildingAreaType = 0
		)";
            if (avgPrice.AreaId > 0) sql += " and T.AreaID = @areaid group by T.AreaID";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<DAT_AvgPrice_Month>(sql, avgPrice).AsQueryable().FirstOrDefault();
            }

        }

        #region 公共

        public Tuple<string, string, string, string> Access(int cityid, int fxtcompanyid)
        {
            const string sql = @"SELECT [ProjectTable],[BuildingTable],[HouseTable],[CaseTable],[QueryInfoTable],[ReportTable],[PrintTable],[HistoryTable],[QueryTaxTable],s.ShowCompanyId,s.CaseCompanyId FROM FxtDataCenter.dbo.[SYS_City_Table] c with(nolock),[Privi_Company_ShowData] s with(nolock) where c.CityId=@cityid  and c.CityId=s.CityId and s.FxtCompanyId=@fxtcompanyid and typecode= 1003002";

            AccessedTable accessedTable;

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                accessedTable = conn.Query<AccessedTable>(sql, new { cityid, fxtcompanyid }).AsQueryable().FirstOrDefault();
            }

            return accessedTable == null ? Tuple.Create("", "", "", "") : Tuple.Create(accessedTable.ProjectTable, accessedTable.CaseTable, accessedTable.ShowCompanyId, accessedTable.CaseCompanyId);
        }

        private class AccessedTable
        {
            public string ProjectTable { get; set; }
            public string CaseTable { get; set; }
            public string ShowCompanyId { get; set; }
            public string CaseCompanyId { get; set; }
        }

        #endregion
    }
}
