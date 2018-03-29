using System;
using System.Data;
using System.Linq;
using Dapper;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class OfficeDynamicPrice : IOfficeDynamicPrice
    {
        public IQueryable<DatPbPriceOffice> GetDynamicPriceSurveys(DatPbPriceOffice dynamicPriceSurvey, int pageIndex, int pageSize, out int totalCount, bool self = true)
        {
            var access = Access(dynamicPriceSurvey.CityId, dynamicPriceSurvey.FxtCompanyId);
            var companyIds = string.IsNullOrEmpty(access.Item4) || self
                ? dynamicPriceSurvey.FxtCompanyId.ToString()
                : access.Item4;

            var strSql = @"
SELECT pb.*
	,p.AreaId
	,p.AreaName
	,p.ProjectName
	,b.BuildingName
	,s1.CodeName AS SurveyTypeName
FROM FxtData_Office.dbo.Dat_P_B_Price_Office pb WITH (NOLOCK)
LEFT JOIN (
	SELECT p.projectId
		,p.projectName
		,a.AreaName
		,a.AreaId
	FROM FxtData_Office.dbo.Dat_Project_Office p WITH (NOLOCK)
	left join FxtDataCenter.dbo.SYS_Area a with(nolock) on p.AreaId = a.AreaId
	WHERE NOT EXISTS (
			SELECT ProjectId
			FROM FxtData_Office.dbo.Dat_Project_Office_sub ps WITH (NOLOCK)
			WHERE p.CityId = ps.CityId
				AND ps.FxtCompanyId = @fxtCompanyId
				AND ps.projectId = p.projectId
			)
		AND p.valid = 1
		AND p.cityId = @cityId
		AND p.fxtcompanyId IN (" + companyIds + @")	
	UNION	
	SELECT p.projectId
		,p.projectName
		,a.AreaName
		,a.AreaId
	FROM FxtData_Office.dbo.Dat_Project_Office_Sub p WITH (NOLOCK)
	left join FxtDataCenter.dbo.SYS_Area a with(nolock) on p.AreaId = a.AreaId
	WHERE p.valid = 1
		AND p.cityId = @cityId
		AND p.fxtcompanyId = @fxtCompanyId
	) p ON p.ProjectId = pb.ProjectId
LEFT JOIN (
	SELECT b.*
	FROM FxtData_Office.dbo.Dat_Building_Office b WITH (NOLOCK)
	WHERE NOT EXISTS (
			SELECT BuildingId
			FROM FxtData_Office.dbo.Dat_Building_Office_sub b1 WITH (NOLOCK)
			WHERE b1.cityId = b.cityId
				AND b1.fxtCompanyId = @fxtCompanyId
				AND b1.BuildingId = b.BuildingId
			)
		AND b.valid = 1
		AND b.fxtcompanyId IN (" + companyIds + @")	
	UNION	
	SELECT b.*
	FROM FxtData_Office.dbo.Dat_Building_Office_sub b WITH (NOLOCK)
	WHERE b.valid = 1
		AND b.fxtCompanyId = @fxtCompanyId
	) b ON b.BuildingId = pb.BuildingId
LEFT JOIN FxtDataCenter.dbo.sys_code s1 WITH (NOLOCK) ON s1.code = pb.SurveyTypeCode
WHERE pb.Valid = 1
	AND pb.CityId = @cityId
	AND pb.FxtCompanyId IN (" + companyIds + @")";

            if (!string.IsNullOrWhiteSpace(dynamicPriceSurvey.ProjectName)) strSql += "  and p.ProjectName like '%'+@ProjectName+'%' ";

            //分页SQL
            var pagenatedSql = @"select top " + pageSize + @" tt.*
                                from (
	                                select row_number() over (
			                                order by t.Id desc
			                                ) rownumber
		                                ,t.*
	                                from (" + strSql + @") t ) tt
                                where tt.rownumber > (" + pageIndex + @" - 1) * " + pageSize;

            //总条数SQL
            var totalCountSql = "select count(1) from (" + strSql + ") as t1";


            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                totalCount = conn.Query<int>(totalCountSql, dynamicPriceSurvey).FirstOrDefault();
                return conn.Query<DatPbPriceOffice>(pagenatedSql, dynamicPriceSurvey).AsQueryable();
            }

        }

        public DatPbPriceOffice GetDynamicPriceSurveyById(int id)
        {
            var strSql = @"select pb.* from FxtData_Office.dbo.Dat_P_B_Price_Office pb with(nolock)
where Id= @id";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                return conn.Query<DatPbPriceOffice>(strSql, new { id }).FirstOrDefault();
            }
        }

        public long GetDynamicPriceSurveyId(long projectId, long buildingId, int cityId, int fxtCompanyId)
        {
            var strSql = @"select pb.id from  FxtData_Office.dbo.Dat_P_B_Price_Office pb with(nolock)
where ProjectId=@projectId and BuildingId= @buildingId and CityId=@cityId and FxtCompanyId = @fxtCompanyId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                return conn.Query<long>(strSql, new { projectId, buildingId, cityId, fxtCompanyId }).FirstOrDefault();
            }
        }

        public int AddDynamicPriceSurvey(DatPbPriceOffice dynamicPriceSurvey)
        {
            var strSql = @"insert into FxtData_Office.dbo.Dat_P_B_Price_Office (cityid,projectid,buildingid,avgrent,avgsaleprice,rent1,rent2,saleprice1,saleprice2,tenantarea,vacantarea,vacantrate,rentsalerate,managerprice,surveydate,surveyuser,surveytypecode,fxtcompanyid,creator,CreateTime) 
values(@cityid,@projectid,@buildingid,@avgrent,@avgsaleprice,@rent1,@rent2,@saleprice1,@saleprice2,@tenantarea,@vacantarea,@vacantrate,@rentsalerate,@managerprice,@surveydate,@surveyuser,@surveytypecode,@fxtcompanyid,@creator,@CreateTime)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                return conn.Execute(strSql, dynamicPriceSurvey);
            }
        }

        public int UpdateDynamicPriceSurvey(DatPbPriceOffice dynamicPriceSurvey)
        {
            var strSql = @"update FxtData_Office.dbo.Dat_P_B_Price_Office set projectid = @projectid,buildingid = @buildingid,avgrent = @avgrent,avgsaleprice = @avgsaleprice,rent1 = @rent1,rent2 = @rent2,saleprice1 = @saleprice1,saleprice2 = @saleprice2,tenantarea = @tenantarea,vacantarea = @vacantarea,vacantrate = @vacantrate,rentsalerate = @rentsalerate,managerprice = @managerprice,surveydate = @surveydate,surveyuser = @surveyuser,surveytypecode = @surveytypecode,savedatetime = @savedatetime,saveuser = @saveuser
where id = @id";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                return conn.Execute(strSql, dynamicPriceSurvey);
            }
        }

        public int DeleteDynamicPriceSurvey(DatPbPriceOffice dynamicPriceSurvey)
        {
            var strSql = @"update FxtData_Office.dbo.Dat_P_B_Price_Office  set valid = 0,SaveDateTime = @SaveDateTime,SaveUser = @SaveUser where id = @id";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                return conn.Execute(strSql, dynamicPriceSurvey);
            }
        }

        #region 公共

        public Tuple<string, string, string, string> Access(int cityid, int fxtcompanyid)
        {
            const string sql = @"SELECT [ProjectTable],[BuildingTable],[HouseTable],[CaseTable],[QueryInfoTable],[ReportTable],[PrintTable],[HistoryTable],[QueryTaxTable],s.OfficeCompanyId FROM FxtDataCenter.dbo.[SYS_City_Table] c with(nolock),[Privi_Company_ShowData] s with(nolock) where c.CityId=@cityid  and c.CityId=s.CityId and s.FxtCompanyId=@fxtcompanyid and typecode= 1003002";

            AccessedTable accessedTable;

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                accessedTable = conn.Query<AccessedTable>(sql, new { cityid, fxtcompanyid }).AsQueryable().FirstOrDefault();
            }

            return accessedTable == null ? Tuple.Create("", "", "", "") : Tuple.Create(accessedTable.ProjectTable, accessedTable.CaseTable, accessedTable.BuildingTable, accessedTable.OfficeCompanyId);
        }

        private class AccessedTable
        {
            public string ProjectTable { get; set; }
            public string CaseTable { get; set; }
            public string BuildingTable { get; set; }
            public string OfficeCompanyId { get; set; }
        }

        #endregion
    }
}
