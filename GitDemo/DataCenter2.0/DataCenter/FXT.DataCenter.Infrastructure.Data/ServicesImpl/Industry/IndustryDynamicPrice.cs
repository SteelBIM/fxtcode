using System;
using System.Data;
using System.Linq;
using Dapper;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Common.DBHelper;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class IndustryDynamicPrice : IIndustryDynamicPrice
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
	,s.IndustryCompanyId as ShowCompanyId
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

        public IQueryable<DatPBPriceIndustry> GetDynamicPriceSurveys(DatPBPriceIndustry dynamicPriceSurvey, int pageIndex, int pageSize, out int totalCount, bool self = true)
        {
            var access = Access(dynamicPriceSurvey.CityId, dynamicPriceSurvey.FxtCompanyId);
            var companyIds = string.IsNullOrEmpty(access.Item4) || self
                ? dynamicPriceSurvey.FxtCompanyId.ToString()
                : access.Item4;

            string where = string.Empty;
            if (!string.IsNullOrWhiteSpace(dynamicPriceSurvey.ProjectName))
            {
                where = "  and T.ProjectName like '%'+@ProjectName+'%'";
            }

            var strSql = @"
select T.*,b.BuildingName from (
	SELECT pb.*
		,p.AreaId
		,p.AreaName
		,p.ProjectName
		,s1.CodeName AS SurveyTypeCodeName
	FROM FxtData_Industry.dbo.Dat_P_B_Price_Industry pb WITH (NOLOCK)
	inner JOIN (
		SELECT p.projectId
			,p.projectName
			,a.AreaName
			,a.AreaId
		FROM FxtData_Industry.dbo.Dat_Project_Industry p WITH (NOLOCK)
		left join FxtDataCenter.dbo.SYS_Area a with(nolock) on p.AreaId = a.AreaId
		WHERE NOT EXISTS (
				SELECT ProjectId
				FROM FxtData_Industry.dbo.Dat_Project_Industry_sub ps WITH (NOLOCK)
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
		FROM FxtData_Industry.dbo.Dat_Project_Industry_Sub p WITH (NOLOCK)
		left join FxtDataCenter.dbo.SYS_Area a with(nolock) on p.AreaId = a.AreaId
		WHERE p.valid = 1
			AND p.cityId = @cityId
			AND p.fxtcompanyId = @fxtCompanyId
		) p ON p.ProjectId = pb.ProjectId
	LEFT JOIN FxtDataCenter.dbo.sys_code s1 WITH (NOLOCK) ON s1.code = pb.SurveyTypeCode
	WHERE pb.Valid = 1
		AND pb.CityId = @cityId
		AND pb.FxtCompanyId IN (" + companyIds + @")
)T
INNER JOIN (
	SELECT b.*
	FROM FxtData_Industry.dbo.Dat_Building_Industry b WITH (NOLOCK)
	WHERE NOT EXISTS (
			SELECT BuildingId
			FROM FxtData_Industry.dbo.Dat_Building_Industry_sub b1 WITH (NOLOCK)
			WHERE b1.cityId = b.cityId
				AND b1.fxtCompanyId = @fxtCompanyId
				AND b1.BuildingId = b.BuildingId
			)
		AND b.valid = 1
		AND b.fxtcompanyId IN (" + companyIds + @")	
	UNION	
	SELECT b.*
	FROM FxtData_Industry.dbo.Dat_Building_Industry_sub b WITH (NOLOCK)
	WHERE b.valid = 1
		AND b.fxtCompanyId = @fxtCompanyId
	) b on b.BuildingId = T.BuildingId
	where 1 = 1" + where + @"
UNION
select T.*,'' as buildingName from (
	SELECT pb.*
		,p.AreaId
		,p.AreaName
		,p.ProjectName
		,s1.CodeName AS SurveyTypeName
	FROM FxtData_Industry.dbo.Dat_P_B_Price_Industry pb WITH (NOLOCK)
	INNER JOIN (
		SELECT p.projectId
			,p.projectName
			,a.AreaName
			,a.AreaId
		FROM FxtData_Industry.dbo.Dat_Project_Industry p WITH (NOLOCK)
		left join FxtDataCenter.dbo.SYS_Area a with(nolock) on p.AreaId = a.AreaId
		WHERE NOT EXISTS (
				SELECT ProjectId
				FROM FxtData_Industry.dbo.Dat_Project_Industry_sub ps WITH (NOLOCK)
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
		FROM FxtData_Industry.dbo.Dat_Project_Industry_sub p WITH (NOLOCK)
		left join FxtDataCenter.dbo.SYS_Area a with(nolock) on p.AreaId = a.AreaId
		WHERE p.valid = 1
			AND p.cityId = @cityId
			AND p.fxtcompanyId = @fxtCompanyId
		) p ON p.ProjectId = pb.ProjectId
	LEFT JOIN FxtDataCenter.dbo.sys_code s1 WITH (NOLOCK) ON s1.code = pb.SurveyTypeCode
	WHERE pb.Valid = 1
		AND pb.CityId = @cityId
		AND pb.FxtCompanyId IN (" + companyIds + @")
)T
where T.BuildingId = 0" + where;

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

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                totalCount = conn.Query<int>(totalCountSql, dynamicPriceSurvey).FirstOrDefault();
                return conn.Query<DatPBPriceIndustry>(pagenatedSql, dynamicPriceSurvey).AsQueryable();
            }
        }

        public DatPBPriceIndustry GetDynamicPriceSurveyById(int id)
        {
            var strSql = @"select pb.* from FxtData_Industry.dbo.Dat_P_B_Price_Industry pb with(nolock) where Id= @id";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Query<DatPBPriceIndustry>(strSql, new { id }).FirstOrDefault();
            }
        }

        public int UpdateDynamicPriceSurvey(DatPBPriceIndustry dynamicPriceSurvey)
        {
            var strSql = @"update FxtData_Industry.dbo.Dat_P_B_Price_Industry set projectid = @projectid,buildingid = @buildingid,avgrent = @avgrent,avgsaleprice = @avgsaleprice,rent1 = @rent1,rent2 = @rent2,saleprice1 = @saleprice1,saleprice2 = @saleprice2,tenantarea = @tenantarea,vacantarea = @vacantarea,vacantrate = @vacantrate,rentsalerate = @rentsalerate,managerprice = @managerprice,surveydate = @surveydate,surveyuser = @surveyuser,surveytypecode = @surveytypecode,savedatetime = @savedatetime,saveuser = @saveuser
where id = @id";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Execute(strSql, dynamicPriceSurvey);
            }
        }

        public int AddDynamicPriceSurvey(DatPBPriceIndustry dynamicPriceSurvey)
        {
            var strSql = @"insert into FxtData_Industry.dbo.Dat_P_B_Price_Industry(cityid,projectid,buildingid,avgrent,avgsaleprice,rent1,rent2,saleprice1,saleprice2,tenantarea,vacantarea,vacantrate,rentsalerate,managerprice,surveydate,surveyuser,surveytypecode,fxtcompanyid,creator,CreateTime) 
values(@cityid,@projectid,@buildingid,@avgrent,@avgsaleprice,@rent1,@rent2,@saleprice1,@saleprice2,@tenantarea,@vacantarea,@vacantrate,@rentsalerate,@managerprice,@surveydate,@surveyuser,@surveytypecode,@fxtcompanyid,@creator,@CreateTime)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Execute(strSql, dynamicPriceSurvey);
            }
        }

        public int DeleteDynamicPriceSurvey(DatPBPriceIndustry dynamicPriceSurvey)
        {
            var strSql = @"update FxtData_Industry.dbo.Dat_P_B_Price_Industry set valid = 0,SaveDateTime = @SaveDateTime,SaveUser = @SaveUser where id = @id";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Execute(strSql, dynamicPriceSurvey);
            }
        }
    }
}
