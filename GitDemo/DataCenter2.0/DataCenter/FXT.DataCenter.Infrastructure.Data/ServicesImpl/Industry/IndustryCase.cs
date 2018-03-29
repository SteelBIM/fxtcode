using System;
using System.Data;
using System.Linq;
using Dapper;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Common.DBHelper;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class IndustryCase : IIndustryCase
    {
        public Tuple<string, string, string, string> Access(int cityid, int fxtcompanyid)
        {
            const string sql = @"
SELECT [ProjectTable]
	,[BuildingTable]
	,[HouseTable]
	,[CaseTable]
	,[QueryInfoTable]
	,[ReportTable]
	,[PrintTable]
	,[HistoryTable]
	,[QueryTaxTable]
	,s.IndustryCaseCompanyId as ShowCompanyId
FROM FxtDataCenter.dbo.[SYS_City_Table] c WITH (NOLOCK)
	,FxtDataCenter.dbo.Privi_Company_ShowData s WITH (NOLOCK)
WHERE c.CityId = @cityid
	AND c.CityId = s.CityId
	AND s.FxtCompanyId = @fxtcompanyid and typecode= 1003002";

            AccessedTable accessedTable;
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                accessedTable = conn.Query<AccessedTable>(sql, new { cityid, fxtcompanyid }).AsQueryable().FirstOrDefault();
            }
            return accessedTable == null ? Tuple.Create("", "", "", "") : Tuple.Create(accessedTable.ProjectTable, accessedTable.CaseTable, accessedTable.BuildingTable, accessedTable.ShowCompanyId);
        }

        private class AccessedTable
        {
            public string ProjectTable { get; set; }
            public string CaseTable { get; set; }
            public string BuildingTable { get; set; }
            public string ShowCompanyId { get; set; }
        }

        public IQueryable<DatCaseIndustry> GetIndustryCases(DatCaseIndustry datCase, int pageIndex, int pageSize, out int totalCount, bool self = true)
        {
            var access = Access(datCase.CityId, datCase.FxtCompanyId);
            var companyIds = string.IsNullOrEmpty(access.Item4) || self
                ? datCase.FxtCompanyId.ToString()
                : access.Item4;

            var where = string.Empty;
            if (!string.IsNullOrEmpty(datCase.ProjectName)) where += " and ca.ProjectName = @ProjectName";
            if (datCase.CaseDateStart != default(DateTime)) where += " and ca.CaseDate >= @CaseDateStart";
            if (datCase.CaseDateEnd != default(DateTime)) where += " and ca.CaseDate-1 < @CaseDateEnd";
            if (datCase.CaseType == 0) where += " and ca.CaseTypeCode in (3001001,3001002,3001003,3001004,3001005)";
            if (datCase.CaseType == 1) where += " and ca.CaseTypeCode in (3001006,3001007,3001008,3001009)";
            if (!new[] { 0, -1 }.Contains(datCase.CaseTypeCode)) where += " and ca.CaseTypeCode = @CaseTypeCode";
            if (datCase.UnitPriceFrom != default(decimal)) where += " and ca.UnitPrice >=@UnitPriceFrom";
            if (datCase.UnitPriceTo != default(decimal)) where += " and ca.UnitPrice <= @UnitPriceTo";
            if (datCase.BuildingAreaFrom != default(decimal)) where += " and ca.BuildingArea >=@BuildingAreaFrom";
            if (datCase.BuildingAreaTo != default(decimal)) where += " and ca.BuildingArea <= @BuildingAreaTo";

            var strSql = @"
SELECT ca.*
	,c.CodeName AS CaseTypeCodeName
	,a.AreaName AS AreaName
	,sa.SubAreaName SubAreaName 
FROM FxtData_Industry.dbo.Dat_Case_Industry ca WITH (NOLOCK)
LEFT JOIN FxtDataCenter.dbo.SYS_Area a WITH (NOLOCK) ON ca.AreaId = a.AreaId
LEFT JOIN FxtDataCenter.dbo.SYS_SubArea_Industry sa WITH (NOLOCK) ON ca.SubAreaId = sa.SubAreaId
LEFT JOIN FxtDataCenter.dbo.SYS_Code c WITH (NOLOCK) ON ca.CaseTypeCode = c.Code
WHERE ca.valid = 1 AND ca.CityId = @CityId AND ca.FxtCompanyId IN (" + companyIds + ")" + where;

            //分页SQL
            var pagenatedSql = @"select top " + pageSize + @" tt.*
                                from (
	                                select row_number() over (
			                                order by t.id desc
			                                ) rownumber
		                                ,t.*
	                                from (" + strSql + @") t ) tt
                                where tt.rownumber > (" + pageIndex + @" - 1) * " + pageSize;

            //总条数SQL
            var totalCountSql = "select count(1) from (" + strSql + ") as t1";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                totalCount = conn.Query<int>(totalCountSql, datCase).FirstOrDefault();
                return conn.Query<DatCaseIndustry>(pagenatedSql, datCase).AsQueryable();
            }
        }

        public DatCaseIndustry GetIndustryCase(int id)
        {
            var strSql = @"
SELECT ca.*
	,c.CodeName AS CaseTypeName
	,a.AreaName AS AreaName
	,sa.SubAreaName SubAreaName 
FROM FxtData_Industry.dbo.Dat_Case_Industry ca WITH (NOLOCK)
LEFT JOIN FxtDataCenter.dbo.SYS_Area a WITH (NOLOCK) ON ca.AreaId = a.AreaId
LEFT JOIN FxtDataCenter.dbo.SYS_SubArea_Industry sa WITH (NOLOCK) ON ca.SubAreaId = sa.SubAreaId
LEFT JOIN FxtDataCenter.dbo.SYS_Code c WITH (NOLOCK) ON c.Code = ca.CaseTypeCode
WHERE ca.id = @id";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Query<DatCaseIndustry>(strSql, new { id }).FirstOrDefault();
            }
        }

        public int AddIndustryCase(DatCaseIndustry datCase)
        {
            var strSql = @"insert into FxtData_Industry.dbo.Dat_Case_Industry (cityid,areaid,subareaid,address,projectid,buildingid,houseid,projectname,buildingname,housename,BuildingArea,UnitPrice,totalprice,casetypecode,casedate,FloorNo,TotalFloor,ManagerPrice,rentrate,AgencyCompany,Agent,AgencyTel,sourcename,sourcelink,sourcephone,fxtcompanyid,creator) 
    values(@cityid,@areaid,@subareaid,@address,@projectid,@buildingid,@houseid,@projectname,@buildingname,@housename,@buildingarea,@unitprice,@totalprice,@casetypecode,@casedate,@FloorNo,@TotalFloor,@ManagerPrice,@rentrate,@AgencyCompany,@Agent,@AgencyTel,@sourcename,@sourcelink,@sourcephone,@fxtcompanyid,@creator)";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Execute(strSql, datCase);
            }
        }

        public int UpdateIndustryCase(DatCaseIndustry datCase)
        {
            var strSql = @"update FxtData_Industry.dbo.Dat_Case_Industry with(rowlock) set areaid = @areaid,subareaid = @subareaid,address = @address,projectid = @projectid,buildingid = @buildingid,houseid = @houseid,projectname = @projectname,buildingname = @buildingname,housename = @housename,BuildingArea = @BuildingArea,UnitPrice = @UnitPrice,totalprice = @totalprice,casetypecode = @casetypecode,casedate = @casedate,FloorNo = @FloorNo,TotalFloor = @TotalFloor,ManagerPrice = @ManagerPrice,rentrate = @rentrate,AgencyCompany = @AgencyCompany,Agent = @Agent,AgencyTel = @AgencyTel,sourcename = @sourcename,sourcelink = @sourcelink,sourcephone = @sourcephone,savedatetime = @savedatetime,saveuser = @saveuser
    where id = @id";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Execute(strSql, datCase);
            }
        }

        public int DeleteIndustryCase(DatCaseIndustry datCase)
        {
            var strSql = @"update FxtData_Industry.dbo.Dat_Case_Industry with(rowlock) set valid = 0,SaveDateTime = @SaveDateTime,SaveUser=@SaveUser where id = @id";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Execute(strSql, datCase);
            }
        }

        public int DeleteSameIndustryCase(int fxtCompanyId, int cityId, DateTime caseDateFrom, DateTime caseDateTo, string saveUser)
        {
            var dateFrom = caseDateFrom.ToString("yyyy-MM-dd") + " 00:00:00";
            var dateTo = caseDateTo.ToString("yyyy-MM-dd") + " 23:59:59";

            var strSql = @"
update FxtData_Industry.dbo.Dat_Case_Industry set Valid = 0,SaveDateTime = GETDATE(),SaveUser = '" + saveUser + @"'
where Id in (
select Id from (
	select
		ROW_NUMBER() over(partition by ProjectId,BuildingArea,UnitPrice,CaseTypeCode order by CaseDate desc,createTime desc) as rank1
		,*
	from FxtData_Industry.dbo.Dat_Case_Industry
	where CityId = " + cityId + @"
	and FxtCompanyId = " + fxtCompanyId + @"
	and CaseDate between '" + dateFrom + @"' and '" + dateTo + @"'
	and Valid = 1
	)T
where rank1 > 1
)";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Execute(strSql, commandTimeout: 300);
            }
        }

    }
}
