using System.Linq;
using System.Data;
using Dapper;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using System.Data.SqlClient;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class OfficeTenant : IOfficeTenant
    {
        #region access
        private static void Access(int cityid, int fxtcompanyid, out string ptable, out string ctable, out string btable, out string comId)
        {
            var sql = @"SELECT [ProjectTable],[BuildingTable],[HouseTable],[CaseTable],[QueryInfoTable],[ReportTable],[PrintTable],[HistoryTable],[QueryTaxTable],s.OfficeCompanyId FROM FxtDataCenter.dbo.[SYS_City_Table] c with(nolock),[Privi_Company_ShowData] s with(nolock) where c.CityId=@cityid  and c.CityId=s.CityId and s.FxtCompanyId=@fxtcompanyid and typecode= 1003002";

            SqlParameter[] parameter = { 
                                           new SqlParameter("@cityid",SqlDbType.Int),
                                           new SqlParameter("@fxtcompanyid",SqlDbType.Int)
                                       };
            parameter[0].Value = cityid;
            parameter[1].Value = fxtcompanyid;

            DBHelperSql.ConnectionString = ConfigurationHelper.FxtDataCenter;
            var dt = DBHelperSql.ExecuteDataTable(sql, parameter);
            if (dt.Rows.Count == 0)
            {
                ptable = "";
                ctable = "";
                btable = "";
                comId = "";
            }
            else
            {
                ptable = dt.Rows[0]["ProjectTable"].ToString();
                ctable = dt.Rows[0]["CaseTable"].ToString();
                btable = dt.Rows[0]["BuildingTable"].ToString();
                comId = dt.Rows[0]["OfficeCompanyId"].ToString();
            }

        }

        #endregion

        public IQueryable<DatTenantOffice> GetTenants(DatTenantOffice officeTenant, int pageIndex, int pageSize, out int totalCount, bool self = true)
        {
            string ptable, ctable, btable, comId;
            Access(officeTenant.CityId, officeTenant.FxtCompanyId, out ptable, out ctable, out btable, out comId);
            if (string.IsNullOrEmpty(comId))
            {
                comId = officeTenant.FxtCompanyId.ToString();
            }
            if (self)
            {
                comId = officeTenant.FxtCompanyId.ToString();
            }

            string tenantWhere = " and t.SurveyDate >= @SurveyDateFrom and t.SurveyDate - 1 < @SurveyDateTo";
            string projectWhere = string.Empty;
            if (officeTenant.ProjectName != null && !string.IsNullOrEmpty(officeTenant.ProjectName.Trim()))
            {
                projectWhere += " and p.ProjectName like '%" + officeTenant.ProjectName.Trim() + "%'";
            }
            if (officeTenant.ProjectOtherName != null && !string.IsNullOrEmpty(officeTenant.ProjectOtherName.Trim()))
            {
                projectWhere += " and p.OtherName like '%" + officeTenant.ProjectOtherName.Trim() + "%'";
            }
            string buildingWhere = string.Empty;
            if (officeTenant.BuildingName != null && !string.IsNullOrEmpty(officeTenant.BuildingName.Trim()))
            {
                buildingWhere += " and b.BuildingName like '%" + officeTenant.BuildingName.Trim() + "%'";
            }
            if (officeTenant.BuildingOtherName != null && !string.IsNullOrEmpty(officeTenant.BuildingOtherName.Trim()))
            {
                buildingWhere += " and b.OtherName like '%" + officeTenant.BuildingOtherName.Trim() + "%'";
            }

            var strSql = @"
SELECT tenantTable.*
,buildingTable.BuildingName
,buildingTable.buildingOtherName
,projectTable.ProjectName
,projectTable.projectOtherName
,projectTable.AreaId
,projectTable.AreaName
,projectTable.SubAreaId
,projectTable.SubAreaName
FROM (
	SELECT t.*
	,case when IsTypical = 1 then '是' else '否' end as IsTypical1
	,case when IsVacant = 1 then '是' else '否' end as IsVacant1
	,c.ChineseName as TenantName
	,c1.CodeName as TypeCodeName
	,c2.CodeName as SubTypeCodeName
	FROM FxtData_Office.dbo.Dat_Tenant_Office t WITH (NOLOCK)
	left join fxtdatacenter.dbo.DAT_Company c WITH (NOLOCK) on t.TenantID = c.CompanyId
	left join FxtDataCenter.dbo.SYS_Code c1 WITH (NOLOCK) on t.TypeCode = c1.Code
	left join FxtDataCenter.dbo.SYS_Code c2 WITH (NOLOCK) on t.SubTypeCode = c2.Code
	WHERE NOT EXISTS (
			SELECT HouseTenantId
			FROM FxtData_Office.dbo.Dat_Tenant_Office_sub t1 WITH (NOLOCK)
			WHERE t1.CityId = t.CityId
				AND t1.FxtCompanyId = @fxtcompanyid
				AND t1.HouseTenantId = t.HouseTenantId
				AND t1.BuildingId = t.BuildingId
				AND t1.ProjectId = t.ProjectId
			)
		AND t.Valid = 1
		AND t.CityId = @cityid
		AND t.FxtCompanyId IN (" + comId + @")"
        + tenantWhere + @"
    UNION	
	SELECT t.*
	,case when IsTypical = 1 then '是' else '否' end as IsTypical1
	,case when IsVacant = 1 then '是' else '否' end as IsVacant1
	,c.ChineseName as TenantName
	,c1.CodeName as TypeCodeName
	,c2.CodeName as SubTypeCodeName
	FROM FxtData_Office.dbo.Dat_Tenant_Office_sub t WITH (NOLOCK)
	left join fxtdatacenter.dbo.DAT_Company c WITH (NOLOCK) on t.TenantID = c.CompanyId
	left join FxtDataCenter.dbo.SYS_Code c1 WITH (NOLOCK) on t.TypeCode = c1.Code
	left join FxtDataCenter.dbo.SYS_Code c2 WITH (NOLOCK) on t.SubTypeCode = c2.Code
	WHERE t.Valid = 1
		AND t.CityId = @cityid
		AND t.FxtCompanyId = @fxtcompanyid"
        + tenantWhere + @"
	) tenantTable
INNER JOIN (
	SELECT b.BuildingId
		,b.ProjectId
		,b.CityId
		,b.BuildingName
		,b.OtherName AS buildingOtherName
		,b.Valid
		,b.FxtCompanyId
	FROM FxtData_Office.dbo.Dat_Building_Office b WITH (NOLOCK)
	WHERE NOT EXISTS (
			SELECT BuildingId
			FROM FxtData_Office.dbo.Dat_Building_Office_sub b1 WITH (NOLOCK)
			WHERE b1.cityId = b.cityId
				AND b1.fxtCompanyId = @fxtCompanyId
				AND b1.BuildingId = b.BuildingId
				AND b1.ProjectId = b.ProjectId
			)
		AND b.valid = 1
		AND b.CityId = @CityId
		AND b.FxtCompanyId IN (" + comId + @")" + buildingWhere + @"
	UNION
	SELECT b.BuildingId
		,b.ProjectId
		,b.CityId
		,b.BuildingName
		,b.OtherName AS buildingOtherName
		,b.Valid
		,b.FxtCompanyId
	FROM FxtData_Office.dbo.Dat_Building_Office_sub b WITH (NOLOCK)
	WHERE b.valid = 1
		AND b.CityId = @CityId
		AND b.FxtCompanyId = @fxtCompanyId" + buildingWhere + @"
	) buildingTable ON tenantTable.BuildingId = buildingTable.BuildingId and tenantTable.ProjectId = buildingTable.ProjectId
INNER JOIN (
	SELECT p.ProjectId
		,p.CityId
		,p.AreaId
		,a.AreaName
		,p.SubAreaId
		,sb.SubAreaName
		,p.ProjectName
		,p.OtherName AS projectOtherName
		,p.Valid
		,p.FxtCompanyId
	FROM FxtData_Office.dbo.Dat_Project_Office p WITH (NOLOCK)
	LEFT JOIN FxtDataCenter.dbo.SYS_SubArea_Office sb WITH (NOLOCK) ON p.SubAreaId = sb.SubAreaId
	LEFT JOIN FxtDataCenter.dbo.SYS_Area a WITH (NOLOCK) ON p.AreaId = a.AreaId
	WHERE NOT EXISTS (
			SELECT ProjectId
			FROM FxtData_Office.dbo.Dat_Project_Office_sub p1 WITH (NOLOCK)
			WHERE p1.areaId = p.areaId
				AND p1.cityId = p.cityId
				AND p1.fxtCompanyId = @fxtCompanyId
				AND p1.projectId = p.projectId
			)
		AND p.valid = 1
		AND p.CityId = @CityId
		AND p.FxtCompanyId IN (" + comId + @")
		and (p.AreaId = @AreaId or @AreaId = -1)
		and (p.SubAreaId = @SubAreaId or @SubAreaId = -1)" + projectWhere + @"
	UNION	
	SELECT p.ProjectId
		,p.CityId
		,p.AreaId
		,a.AreaName
		,p.SubAreaId
		,sb.SubAreaName
		,p.ProjectName
		,p.OtherName AS projectOtherName
		,p.Valid AS projectValid
		,p.FxtCompanyId AS projectFxtCompanyId
	FROM FxtData_Office.dbo.Dat_Project_Office_sub p WITH (NOLOCK)
	LEFT JOIN FxtDataCenter.dbo.SYS_SubArea_Office sb WITH (NOLOCK) ON p.SubAreaId = sb.SubAreaId
	LEFT JOIN FxtDataCenter.dbo.SYS_Area a WITH (NOLOCK) ON p.AreaId = a.AreaId
	WHERE p.valid = 1
		AND p.CityId = @CityId
		AND p.FxtCompanyId = @fxtCompanyId
		and (p.AreaId = @AreaId or @AreaId = -1)
		and (p.SubAreaId = @SubAreaId or @SubAreaId = -1)" + projectWhere + @"
	) projectTable ON buildingTable.ProjectId = projectTable.ProjectId";

            //分页SQL
            var pagenatedSql = @"select top " + pageSize + @" tt.*
                                from (
	                                select row_number() over (
			                                order by t.HouseTenantId desc
			                                ) rownumber
		                                ,t.*
	                                from (" + strSql + @") t ) tt
                                where tt.rownumber > (" + pageIndex + @" - 1) * " + pageSize;

            //总条数SQL
            var totalCountSql = "select count(1) from (" + strSql + ") as t1";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                totalCount = conn.Query<int>(totalCountSql, new { fxtCompanyId = officeTenant.FxtCompanyId, CityId = officeTenant.CityId, AreaId = officeTenant.AreaId, SubAreaId = officeTenant.SubAreaId, SurveyDateFrom = officeTenant.SurveyDateFrom, SurveyDateTo = officeTenant.SurveyDateTo }).FirstOrDefault();
                return conn.Query<DatTenantOffice>(pagenatedSql, new { fxtCompanyId = officeTenant.FxtCompanyId, CityId = officeTenant.CityId, AreaId = officeTenant.AreaId, SubAreaId = officeTenant.SubAreaId, SurveyDateFrom = officeTenant.SurveyDateFrom, SurveyDateTo = officeTenant.SurveyDateTo }).AsQueryable();
            }
        }

        public DatTenantOffice GetTenantNameById(int houseTenantId, int fxtCompanyId)
        {
            string sql = @"
select 
	tenantTable.*
	,projectTable.ProjectName
	,projectTable.AreaName
	,projectTable.SubAreaName
	,projectTable.OtherName as projectOtherName
	,buildingTable.BuildingName
	,buildingTable.OtherName as buildingOtherName
from (
	SELECT t.*
	,case when t.IsVacant = 1 then '是' else '否' end as IsVacant1 
	,case when t.IsTypical = 1 then '是' else '否' end as IsTypical1 
	,c.ChineseName as TenantName
	,c1.CodeName as TypeCodeName
	,c2.CodeName as SubTypeCodeName
	FROM FxtData_Office.dbo.Dat_Tenant_Office t WITH (NOLOCK)
	left join FxtDataCenter.dbo.DAT_Company c with(nolock) on t.TenantID = c.CompanyId
	left join FxtDataCenter.dbo.SYS_Code c1 with(nolock) on t.TypeCode = c1.Code
	left join FxtDataCenter.dbo.SYS_Code c2 with(nolock) on t.SubTypeCode = c2.Code
	WHERE NOT EXISTS (
			SELECT HouseTenantId
			FROM Dat_Tenant_Office_sub ts WITH (NOLOCK)
			WHERE ts.HouseTenantId = t.HouseTenantId
				AND ts.ProjectId = t.ProjectId
				AND ts.BuildingId = t.BuildingId
				AND ts.FxtCompanyId = @fxtCompanyId
				AND ts.CityId = t.CityId
			)
		AND t.Valid = 1
		AND t.HouseTenantId = @HouseTenantId
	UNION
	SELECT t.*
	,case when t.IsVacant = 1 then '是' else '否' end as IsVacant1 
	,case when t.IsTypical = 1 then '是' else '否' end as IsTypical1 
	,c.ChineseName as TenantName
	,c1.CodeName as TypeCodeName
	,c2.CodeName as SubTypeCodeName
	FROM Dat_Tenant_Office_sub t WITH (NOLOCK)
	left join FxtDataCenter.dbo.DAT_Company c with(nolock) on t.TenantID = c.CompanyId
	left join FxtDataCenter.dbo.SYS_Code c1 with(nolock) on t.TypeCode = c1.Code
	left join FxtDataCenter.dbo.SYS_Code c2 with(nolock) on t.SubTypeCode = c2.Code
	WHERE t.Valid = 1
		AND t.HouseTenantId = @HouseTenantId
		AND t.FxtCompanyId = @fxtCompanyId
)tenantTable
inner join (
	select p.*
	,a.AreaName 
	,sa.SubAreaName
	from FxtData_Office.dbo.Dat_Project_Office p with(nolock)
	left join FxtDataCenter.dbo.SYS_Area a with(nolock) on p.AreaId = a.AreaId
	left join fxtdatacenter.dbo.SYS_SubArea_Office sa with(nolock) on p.SubAreaId = sa.SubAreaId
	where not exists (
	select * from FxtData_Office.dbo.Dat_Project_Office_sub ps with(nolock)
	where ps.ProjectId = p.ProjectId
	and ps.FxtCompanyId = @fxtCompanyId
	and ps.CityId = p.CityId
	and ps.AreaId = p.AreaId
	)
	and p.Valid = 1
	union
	select  p.*
	,a.AreaName 
	,sa.SubAreaName
	from FxtData_Office.dbo.Dat_Project_Office_sub p with(nolock)
	left join FxtDataCenter.dbo.SYS_Area a with(nolock) on p.AreaId = a.AreaId
	left join fxtdatacenter.dbo.SYS_SubArea_Office sa with(nolock) on p.SubAreaId = sa.SubAreaId
	where p.Valid = 1
	and p.FxtCompanyId = @fxtCompanyId
)projectTable on tenantTable.ProjectId = projectTable.ProjectId
inner join (
	select *
	from FxtData_Office.dbo.Dat_Building_Office b with(nolock)
	where not exists (
	select * from FxtData_Office.dbo.Dat_Building_Office_sub bs with(nolock)
	where bs.ProjectId = b.ProjectId
	and bs.BuildingId = b.BuildingId
	and bs.FxtCompanyId = @fxtCompanyId
	and bs.CityId = b.CityId
	)
	and b.Valid = 1
	union
	select *
	from FxtData_Office.dbo.Dat_Building_Office_sub b with(nolock)
	where b.Valid = 1
	and b.FxtCompanyId = @fxtCompanyId
)buildingTable on tenantTable.BuildingId = buildingTable.BuildingId";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                return conn.Query<DatTenantOffice>(sql, new { houseTenantId = houseTenantId, fxtCompanyId = fxtCompanyId }).FirstOrDefault();
            }
        }

        public int UpdateTenantOffice(DatTenantOffice officeTenant, int currentCompanyId)
        {
            var strSqlMainUpdate = @"
UPDATE FxtData_Office.dbo.Dat_Tenant_Office WITH (ROWLOCK)
SET IsVacant = @IsVacant,BuildingArea = @BuildingArea,Rent = @Rent,TenantID = @TenantID,TypeCode = @TypeCode,SubTypeCode = @SubTypeCode,JoinDate = @JoinDate,SurveyDate = @SurveyDate,SurveyUser = @SurveyUser,Remarks = @Remarks,IsTypical = @IsTypical,FloorNum = @FloorNum,HouseName = @HouseName,SaveUser = @SaveUser,SaveDateTime = @SaveDateTime
WHERE HouseTenantId = @HouseTenantId and (FxtCompanyId = @FxtCompanyId or @fxtcompanyid = " + ConfigurationHelper.FxtCompanyId + ")";

            var strSqlSubAdd = @"
insert into FxtData_Office.dbo.Dat_Tenant_Office_sub(HouseTenantId,CityId,ProjectId,BuildingId,HouseId,IsVacant,BuildingArea,Rent,TenantID,TypeCode,SubTypeCode,JoinDate,SurveyDate,SurveyUser,FxtCompanyId,Creator,CreateTime,SaveDateTime,SaveUser,Remarks,IsTypical,FloorNum,HouseName)
values(@HouseTenantId,@CityId,@ProjectId,@BuildingId,@HouseId,@IsVacant,@BuildingArea,@Rent,@TenantID,@TypeCode,@SubTypeCode,@JoinDate,@SurveyDate,@SurveyUser,@FxtCompanyId,@Creator,@CreateTime,@SaveDateTime,@SaveUser,@Remarks,@IsTypical,@FloorNum,@HouseName)";

            var strSqlSubUpdate = @"
UPDATE FxtData_Office.dbo.Dat_Tenant_Office_sub WITH (ROWLOCK)
SET IsVacant = @IsVacant,BuildingArea = @BuildingArea,Rent = @Rent,TenantID = @TenantID,TypeCode = @TypeCode,SubTypeCode = @SubTypeCode,JoinDate = @JoinDate,SurveyDate = @SurveyDate,SurveyUser = @SurveyUser,Remarks = @Remarks,IsTypical = @IsTypical,FloorNum = @FloorNum,HouseName = @HouseName,SaveUser = @SaveUser,SaveDateTime = @SaveDateTime
WHERE HouseTenantId = @HouseTenantId and fxtCompanyId = @fxtCompanyId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                officeTenant.FxtCompanyId = currentCompanyId;
                var mainreturn = conn.Execute(strSqlMainUpdate, officeTenant);

                if (mainreturn == 0)
                {
                    if (conn.Execute(strSqlSubUpdate, officeTenant) == 0)
                    {
                        return conn.Execute(strSqlSubAdd, officeTenant);
                    }
                    return 1;
                }
                return 1;
            }
        }

        public int AddTenantOffice(DatTenantOffice officeTenant)
        {
            var strSql = @"
INSERT INTO FxtData_Office.dbo.Dat_Tenant_Office(CityId,ProjectId,BuildingId,HouseId,IsVacant,BuildingArea,Rent,TenantID,TypeCode,SubTypeCode,JoinDate,SurveyDate,SurveyUser,FxtCompanyId,Creator,CreateTime,Remarks,IsTypical,FloorNum,HouseName)
VALUES (@CityId,@ProjectId,@BuildingId,@HouseId,@IsVacant,@BuildingArea,@Rent,@TenantID,@TypeCode,@SubTypeCode,@JoinDate,@SurveyDate,@SurveyUser,@FxtCompanyId,@Creator,@CreateTime,@Remarks,@IsTypical,@FloorNum,@HouseName)";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                return conn.Execute(strSql, officeTenant);
            }
        }

        public int DeleteTenantOffice(DatTenantOffice officeTenant, int currentCompanyId)
        {
            var strSqlMainDelete = @"
UPDATE FxtData_Office.dbo.Dat_Tenant_Office WITH (ROWLOCK)
SET Valid = 0,SaveUser = @SaveUser,SaveDateTime = @SaveDateTime
WHERE HouseTenantId = @HouseTenantId and (FxtCompanyId = @FxtCompanyId or @fxtcompanyid = " + ConfigurationHelper.FxtCompanyId + ")";

            var strSqlSubAdd = @"
insert into FxtData_Office.dbo.Dat_Tenant_Office_sub(HouseTenantId,CityId,ProjectId,BuildingId,HouseId,IsVacant,BuildingArea,Rent,TenantID,TypeCode,SubTypeCode,JoinDate,SurveyDate,SurveyUser,FxtCompanyId,Creator,CreateTime,SaveDateTime,SaveUser,Remarks,IsTypical,FloorNum,HouseName,Valid)
values(@HouseTenantId,@CityId,@ProjectId,@BuildingId,@HouseId,@IsVacant,@BuildingArea,@Rent,@TenantID,@TypeCode,@SubTypeCode,@JoinDate,@SurveyDate,@SurveyUser,@FxtCompanyId,@Creator,@CreateTime,@SaveDateTime,@SaveUser,@Remarks,@IsTypical,@FloorNum,@HouseName,@Valid)";

            var strSqlSubDelete = @"
UPDATE FxtData_Office.dbo.Dat_Tenant_Office_sub WITH (ROWLOCK)
SET Valid = 0,SaveUser = @SaveUser,SaveDateTime = @SaveDateTime
WHERE HouseTenantId = @HouseTenantId and fxtCompanyId = @fxtCompanyId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                officeTenant.FxtCompanyId = currentCompanyId;
                var mainreturn = conn.Execute(strSqlMainDelete, officeTenant);

                if (mainreturn == 0)
                {
                    if (conn.Execute(strSqlSubDelete, officeTenant) == 0)
                    {
                        officeTenant.Valid = 0;
                        return conn.Execute(strSqlSubAdd, officeTenant);
                    }
                    return 1;
                }
                return 1;
            }
        }
    }
}
