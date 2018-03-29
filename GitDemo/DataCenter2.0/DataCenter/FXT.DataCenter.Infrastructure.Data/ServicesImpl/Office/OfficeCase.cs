using System;
using System.Data;
using System.Linq;
using Dapper;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class OfficeCase : IOfficeCase
    {
        public IQueryable<DatCaseOffice> GetOfficeCases(DatCaseOffice datCaseOffice, int pageIndex, int pageSize, out int totalCount, bool self = true)
        {
            var access = Access(datCaseOffice.CityId, datCaseOffice.FxtCompanyId);
            var companyIds = string.IsNullOrEmpty(access.Item4) || self
                ? datCaseOffice.FxtCompanyId.ToString()
                : access.Item4;

            var where = string.Empty;
            if (!string.IsNullOrEmpty(datCaseOffice.ProjectName)) where += " and co.ProjectName like '%'+@ProjectName+'%' ";
            if (datCaseOffice.CaseDateStart != default(DateTime)) where += " and co.CaseDate >= @CaseDateStart";
            if (datCaseOffice.CaseDateEnd != default(DateTime)) where += " and co.CaseDate-1 < @CaseDateEnd";
            if (datCaseOffice.CaseType == 0) where += " and co.CaseTypeCode in (3001001,3001002,3001003,3001004,3001005)";
            if (datCaseOffice.CaseType == 1) where += " and co.CaseTypeCode in (3001006,3001007,3001008,3001009)";
            if (!new[] { 0, -1 }.Contains(datCaseOffice.CaseTypeCode)) where += " and co.CaseTypeCode = @CaseTypeCode";
            if (datCaseOffice.UnitPriceFrom != default(decimal)) where += " and co.UnitPrice >=@UnitPriceFrom";
            if (datCaseOffice.UnitPriceTo != default(decimal)) where += " and co.UnitPrice <= @UnitPriceTo";
            if (datCaseOffice.BuildingAreaFrom != default(decimal)) where += " and co.BuildingArea >=@BuildingAreaFrom";
            if (datCaseOffice.BuildingAreaTo != default(decimal)) where += " and co.BuildingArea <= @BuildingAreaTo";
            
            var strSql = @"
SELECT co.*
	,c.CodeName AS CaseTypeName
	,c1.CodeName AS OfficeTypeName
	,c2.CodeName AS FitmentName
	,a.AreaName as AreaName
FROM FxtData_Office.dbo.Dat_Case_Office co WITH (NOLOCK)
inner join (
	select ProjectId,AreaId from FxtData_Office.dbo.Dat_Project_Office p with(nolock)
	where not exists(
		select ProjectId from FxtData_Office.dbo.Dat_Project_Office_sub ps with (nolock)
		where ps.ProjectId = p.ProjectId
		and ps.FxtCompanyId = @fxtcompanyid
		and ps.CityId = @CityId
	)
	and p.Valid = 1
	and p.CityId = @CityId
	and p.FxtCompanyId in (" + companyIds + @")
	union 
	select ProjectId,AreaId from FxtData_Office.dbo.Dat_Project_Office_sub p with(nolock)
	where p.Valid = 1
	and p.FxtCompanyId = @fxtcompanyid
	and p.CityId = @CityId
)p on co.ProjectId = p.ProjectId
left join FxtDataCenter.dbo.SYS_Area a with(nolock) on p.AreaId = a.AreaId
left join FxtDataCenter.dbo.SYS_Code c with(nolock) on co.CaseTypeCode = c.Code
left join FxtDataCenter.dbo.SYS_Code c1 with(nolock) on co.OfficeType = c1.Code
left join FxtDataCenter.dbo.SYS_Code c2 with(nolock) on co.Fitment = c2.Code
where co.valid = 1 and  co.CityId = @CityId and co.FxtCompanyId in (" + companyIds + ")" + where;

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
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                totalCount = conn.Query<int>(totalCountSql, datCaseOffice).FirstOrDefault();
                return conn.Query<DatCaseOffice>(pagenatedSql, datCaseOffice).AsQueryable();
            }
        }

        public DatCaseOffice GetOfficeCase(int id)
        {
            var strSql = @"select co.*,c.CodeName as CaseTypeName
                        from  FxtData_Office.dbo.Dat_Case_Office co with(nolock)
                        left join FxtDataCenter.dbo.SYS_Code c with(nolock) on c.Code = co.CaseTypeCode
                        where co.id = @id";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                return conn.Query<DatCaseOffice>(strSql, new { id }).FirstOrDefault();
            }
        }

        public int AddOfficeCase(DatCaseOffice datCaseOffice)
        {
            var strSql = @"insert into FxtData_Office.dbo.Dat_Case_Office(cityid,projectid,buildingid,houseid,projectname,buildingname,housename,BuildingArea,UnitPrice,totalprice,casetypecode,casedate,rentrate,sourcename,sourcelink,sourcephone,fxtcompanyid,creator,createtime,FloorNo,TotalFloor,Fitment,ManagerPrice,OfficeType,AgencyCompany,Agent,AgencyTel) 
values(@cityid,@projectid,@buildingid,@houseid,@projectname,@buildingname,@housename,@BuildingArea,@UnitPrice,@totalprice,@casetypecode,@casedate,@rentrate,@sourcename,@sourcelink,@sourcephone,@fxtcompanyid,@creator,@createtime,@FloorNo,@TotalFloor,@Fitment,@ManagerPrice,@OfficeType,@AgencyCompany,@Agent,@AgencyTel)";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                return conn.Execute(strSql, datCaseOffice);
            }
        }

        public int UpdateOfficeCase(DatCaseOffice datCaseOffice)
        {
            var strSql = @"update FxtData_Office.dbo.Dat_Case_Office set projectid = @projectid,buildingid = @buildingid,houseid = @houseid,projectname = @projectname,buildingname = @buildingname,housename = @housename,BuildingArea = @BuildingArea,UnitPrice = @UnitPrice,totalprice = @totalprice,casetypecode = @casetypecode,casedate = @casedate,rentrate = @rentrate,sourcename = @sourcename,sourcelink = @sourcelink,sourcephone = @sourcephone,savedatetime = @savedatetime,saveuser = @saveuser,FloorNo = @FloorNo,TotalFloor = @TotalFloor,Fitment = @Fitment,ManagerPrice = @ManagerPrice,OfficeType = @OfficeType,AgencyCompany = @AgencyCompany,Agent = @Agent,AgencyTel = @AgencyTel
where id = @id";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                return conn.Execute(strSql, datCaseOffice);
            }
        }

        public int DeleteOfficeCase(DatCaseOffice datCaseOffice)
        {
            var strSql = @"update FxtData_Office.dbo.Dat_Case_Office with(rowlock)  set valid = 0,SaveDateTime = @SaveDateTime,SaveUser=@SaveUser where id = @id";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                return conn.Execute(strSql, datCaseOffice);
            }
        }

        public int DeleteSameOfficeCase(int fxtCompanyId, int cityId, DateTime caseDateFrom, DateTime caseDateTo, string saveUser)
        {
            var dateFrom = caseDateFrom.ToString("yyyy-MM-dd") + " 00:00:00";
            var dateTo = caseDateTo.ToString("yyyy-MM-dd") + " 23:59:59";

            var strSql = @"
update FxtData_Office.dbo.Dat_Case_Office set Valid = 0,SaveDateTime = GETDATE(),SaveUser = '" + saveUser + @"'
where Id in (
select Id from (
	select
		ROW_NUMBER() over(partition by ProjectId,BuildingArea,UnitPrice,CaseTypeCode order by CaseDate desc,createTime desc) as rank1
		,*
	from FxtData_Office.dbo.Dat_Case_Office
	where CityId = " + cityId + @"
	and FxtCompanyId = " + fxtCompanyId + @"
	and CaseDate between '" + dateFrom + @"' and '" + dateTo + @"'
	and Valid = 1
	)T
where rank1 > 1
)";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                return conn.Execute(strSql, commandTimeout: 300);
            }
        }

        #region 公共

        public Tuple<string, string, string, string> Access(int cityid, int fxtcompanyid)
        {
            const string sql = @"SELECT [ProjectTable],[BuildingTable],[HouseTable],[CaseTable],[QueryInfoTable],[ReportTable],[PrintTable],[HistoryTable],[QueryTaxTable],s.OfficeCaseCompanyId FROM FxtDataCenter.dbo.[SYS_City_Table] c with(nolock),[Privi_Company_ShowData] s with(nolock) where c.CityId=@cityid  and c.CityId=s.CityId and s.FxtCompanyId=@fxtcompanyid and typecode= 1003002";

            AccessedTable accessedTable;

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                accessedTable = conn.Query<AccessedTable>(sql, new { cityid, fxtcompanyid }).AsQueryable().FirstOrDefault();
            }

            return accessedTable == null ? Tuple.Create("", "", "", "") : Tuple.Create(accessedTable.ProjectTable, accessedTable.CaseTable, accessedTable.BuildingTable, accessedTable.OfficeCaseCompanyId);
        }

        private class AccessedTable
        {
            public string ProjectTable { get; set; }
            public string CaseTable { get; set; }
            public string BuildingTable { get; set; }
            public string OfficeCaseCompanyId { get; set; }
        }

        #endregion
    }
}
