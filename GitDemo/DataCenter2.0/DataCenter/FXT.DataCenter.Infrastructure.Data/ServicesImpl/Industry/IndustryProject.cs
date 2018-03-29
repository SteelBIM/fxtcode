using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Dapper;
using System.Data.SqlClient;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Common.DBHelper;


namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class IndustryProject : IIndustryProject
    {
        private static void Access(int cityid, int fxtcompanyid, out string ptable, out string ctable, out string btable, out string comId)
        {
            var sql = @"
SELECT [ProjectTable]
	,[BuildingTable]
	,[HouseTable]
	,[CaseTable]
	,[QueryInfoTable]
	,[ReportTable]
	,[PrintTable]
	,[HistoryTable]
	,[QueryTaxTable]
	,s.IndustryCompanyId
FROM FxtDataCenter.dbo.[SYS_City_Table] c WITH (NOLOCK)
	,FxtDataCenter.dbo.Privi_Company_ShowData s WITH (NOLOCK)
WHERE c.CityId = @cityid
	AND c.CityId = s.CityId
	AND s.FxtCompanyId = @fxtcompanyid and typecode= 1003002";

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
                comId = dt.Rows[0]["IndustryCompanyId"].ToString();
            }
        }

        public IQueryable<DatProjectIndustry> GetProjectIndustrys(DatProjectIndustry projectIndustry, int pageIndex, int pageSize, out int totalCount, bool self = true)
        {
            string ptable, ctable, btable, comId;
            Access(projectIndustry.CityId, projectIndustry.FxtCompanyId, out ptable, out ctable, out btable, out comId);
            if (string.IsNullOrEmpty(comId)) comId = projectIndustry.FxtCompanyId.ToString();
            if (self) comId = projectIndustry.FxtCompanyId.ToString();

            var where = string.Empty;
            if (!(new[] { 0, -1 }).Contains(projectIndustry.AreaId)) where += " and p.AreaId = @AreaId";
            if (!(new[] { 0, -1 }).Contains(projectIndustry.SubAreaId)) where += "  and p.SubAreaId = @SubAreaId";
            if (!string.IsNullOrEmpty(projectIndustry.ProjectName)) where += " and p.ProjectName like @ProjectName";
            if (!string.IsNullOrEmpty(projectIndustry.OtherName)) where += "  and p.OtherName like @OtherName";
            if (!(new[] { 0, -1 }).Contains(projectIndustry.IndustryType ?? -1)) where += "  and p.IndustryType = @IndustryType";
            if (!(new[] { 0, -1 }).Contains(projectIndustry.RentSaleType ?? -1)) where += "  and p.RentSaleType = @RentSaleType";

            //查询全部字段时，select * 与 select a,b...效率差不多
            var strSql = @"
SELECT p.*
	,c.CodeName AS CorrelationTypeName
	,c1.CodeName AS PurposeCodeName
	,c2.CodeName as BuildingTypeName
	,c3.CodeName as IndustryTypeName
	,c4.CodeName AS TrafficTypeName
	,c5.CodeName AS ParkingLevelName
	,c6.CodeName as ParkingTypeName
	,c7.CodeName as RentSaleTypeName
	,c8.CodeName AS AirConditionTypeName
	,a.AreaName
	,sb.SubAreaName
FROM FxtData_Industry.dbo.Dat_Project_Industry p WITH (NOLOCK)
LEFT JOIN FxtDataCenter.dbo.SYS_SubArea_Industry sb WITH (NOLOCK) ON p.SubAreaId = sb.SubAreaId
LEFT JOIN FxtDataCenter.dbo.SYS_Area a WITH (NOLOCK) ON p.AreaId = a.AreaId
LEFT JOIN FxtDataCenter.dbo.SYS_Code c WITH (NOLOCK) ON p.CorrelationType = c.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c1 WITH (NOLOCK) ON p.PurposeCode = c1.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c2 WITH (NOLOCK) ON p.BuildingType = c2.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c3 WITH (NOLOCK) ON p.IndustryType = c3.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c4 WITH (NOLOCK) ON p.TrafficType = c4.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c5 WITH (NOLOCK) ON p.ParkingLevel = c5.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c6 WITH (NOLOCK) ON p.ParkingType = c6.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c7 WITH (NOLOCK) ON p.RentSaleType = c7.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c8 WITH (NOLOCK) ON p.AirConditionType = c8.Code
WHERE not exists (
		SELECT ProjectId
		FROM FxtData_Industry.dbo.Dat_Project_Industry_sub p1 WITH (NOLOCK)
		WHERE p1.areaId = p.areaId
			AND p1.cityId = p.cityId
			AND p1.fxtCompanyId =  @fxtCompanyId
			AND p1.projectId = p.projectId)
	AND p.valid = 1
	AND p.CityId = @CityId
	AND p.FxtCompanyId IN (" + comId + @")" + where + @"                            
UNION 
SELECT p.*
	,c.CodeName AS CorrelationTypeName
	,c1.CodeName AS PurposeCodeName
	,c2.CodeName as BuildingTypeName
	,c3.CodeName as IndustryTypeName
	,c4.CodeName AS TrafficTypeName
	,c5.CodeName AS ParkingLevelName
	,c6.CodeName as ParkingTypeName
	,c7.CodeName as RentSaleTypeName
	,c8.CodeName AS AirConditionTypeName
	,a.AreaName
	,sb.SubAreaName
FROM FxtData_Industry.dbo.Dat_Project_Industry_sub p WITH (NOLOCK)
LEFT JOIN FxtDataCenter.dbo.SYS_SubArea_Industry sb WITH (NOLOCK) ON p.SubAreaId = sb.SubAreaId
LEFT JOIN FxtDataCenter.dbo.SYS_Area a WITH (NOLOCK) ON p.AreaId = a.AreaId
LEFT JOIN FxtDataCenter.dbo.SYS_Code c WITH (NOLOCK) ON p.CorrelationType = c.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c1 WITH (NOLOCK) ON p.PurposeCode = c1.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c2 WITH (NOLOCK) ON p.BuildingType = c2.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c3 WITH (NOLOCK) ON p.IndustryType = c3.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c4 WITH (NOLOCK) ON p.TrafficType = c4.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c5 WITH (NOLOCK) ON p.ParkingLevel = c5.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c6 WITH (NOLOCK) ON p.ParkingType = c6.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c7 WITH (NOLOCK) ON p.RentSaleType = c7.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c8 WITH (NOLOCK) ON p.AirConditionType = c8.Code
WHERE p.valid = 1
	AND p.CityId = @CityId
	AND p.FxtCompanyId = @fxtCompanyId" + where;

            //分页SQL
            var pagenatedSql = @"select top " + pageSize + @" tt.*
                                from (
	                                select row_number() over (
			                                order by t.projectId desc
			                                ) rownumber
		                                ,t.*
	                                from (" + strSql + @") t ) tt
                                where tt.rownumber > (" + pageIndex + @" - 1) * " + pageSize;

            //总条数SQL
            var totalCountSql = "select count(1) from (" + strSql + ") as t1";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                totalCount = conn.Query<int>(totalCountSql, new { projectIndustry.FxtCompanyId, projectIndustry.SubAreaId, projectIndustry.CityId, projectIndustry.AreaId, ProjectName = "%" + projectIndustry.ProjectName + "%", OtherName = "%" + projectIndustry.OtherName + "%", projectIndustry.IndustryType, projectIndustry.RentSaleType }).FirstOrDefault();
                return conn.Query<DatProjectIndustry>(pagenatedSql, new { projectIndustry.FxtCompanyId, projectIndustry.SubAreaId, projectIndustry.CityId, projectIndustry.AreaId, ProjectName = "%" + projectIndustry.ProjectName + "%", OtherName = "%" + projectIndustry.OtherName + "%", projectIndustry.IndustryType, projectIndustry.RentSaleType }).AsQueryable();
            }
        }

        public IQueryable<DatProjectIndustry> GetProjectIndustrys(int cityId, int fxtCompanyId, int areaId = -1, int subAreaId = -1, int projectId = -1, bool self = true)
        {
            string ptable, ctable, btable, comId;
            Access(cityId, fxtCompanyId, out ptable, out ctable, out btable, out comId);
            if (string.IsNullOrEmpty(comId)) comId = fxtCompanyId.ToString();
            if (self) comId = fxtCompanyId.ToString();

            var where = string.Empty;
            if (areaId > 0) where += "  and p.AreaId = @AreaId";
            if (subAreaId > 0) where += "  and p.SubAreaId = @SubAreaId";
            if (projectId > 0) where += "  and p.ProjectId = @ProjectId";

            var strSql = @"
SELECT p.projectId,p.projectName,p.OtherName,a.areaName
FROM FxtData_Industry.dbo.Dat_Project_Industry p WITH (NOLOCK)
LEFT JOIN FxtDataCenter.dbo.SYS_Area a WITH (NOLOCK) ON p.AreaId = a.AreaId        
WHERE NOT EXISTS (
		SELECT ProjectId
		FROM FxtData_Industry.dbo.Dat_Project_Industry_sub ps WITH (NOLOCK)
		WHERE p.CityId = ps.CityId
			AND ps.FxtCompanyId = @fxtCompanyId
			AND ps.ProjectId = p.ProjectId
		)
	AND p.valid = 1
	and p.cityId=@cityId
    and p.fxtcompanyId in (" + comId + @")" + where + @"
UNION
SELECT p.projectId,p.projectName,p.OtherName,a.areaName
FROM FxtData_Industry.dbo.Dat_Project_Industry_Sub p WITH (NOLOCK)
LEFT JOIN FxtDataCenter.dbo.SYS_Area a WITH (NOLOCK) ON p.AreaId = a.AreaId 
WHERE p.valid = 1
    and p.cityId=@cityId
    and p.fxtcompanyId = @fxtCompanyId" + where;

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Query<DatProjectIndustry>(strSql, new { cityId = cityId, AreaId = areaId, SubAreaId = subAreaId, ProjectId = projectId, fxtCompanyId = fxtCompanyId }).AsQueryable();
            }
        }

        public DatProjectIndustry GetProjectNameById(long id, int fxtCompanyId)
        {
            var strSql = @"
SELECT p.*
	,sa.SubAreaName
	,c.CodeName AS CorrelationTypeName
	,c1.CodeName AS TrafficTypeName
FROM FxtData_Industry.dbo.Dat_Project_Industry p WITH (NOLOCK)
LEFT JOIN FxtDataCenter.dbo.SYS_SubArea_Industry sa WITH (NOLOCK) ON p.SubAreaId = sa.SubAreaId
LEFT JOIN FxtDataCenter.dbo.SYS_Code c WITH (NOLOCK) ON p.CorrelationType = c.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c1 WITH (NOLOCK) ON p.TrafficType = c1.Code
WHERE NOT EXISTS (
		SELECT ProjectId
		FROM FxtData_Industry.dbo.Dat_Project_Industry_sub ps WITH (NOLOCK)
		WHERE p.CityId = ps.CityId
			AND @FxtCompanyId = ps.FxtCompanyId
			AND p.ProjectId = ps.ProjectId
		)
	AND p.valid = 1
	AND p.ProjectId = @ProjectId
UNION
SELECT p.*
	,sa.SubAreaName
	,c.CodeName AS CorrelationTypeName
	,c1.CodeName AS TrafficTypeName
FROM FxtData_Industry.dbo.Dat_Project_Industry_sub p WITH (NOLOCK)
LEFT JOIN FxtDataCenter.dbo.SYS_SubArea_Industry sa WITH (NOLOCK) ON p.SubAreaId = sa.SubAreaId
LEFT JOIN FxtDataCenter.dbo.SYS_Code c WITH (NOLOCK) ON p.CorrelationType = c.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c1 WITH (NOLOCK) ON p.TrafficType = c1.Code
WHERE p.valid = 1
	AND p.ProjectId = @ProjectId
	AND p.FxtCompanyId = @fxtCompanyId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Query<DatProjectIndustry>(strSql, new { ProjectId = id, FxtCompanyId = fxtCompanyId }).FirstOrDefault();
            }
        }

        public bool IsExistProjectIndustry(int areaId, long projectId, string projectName, int cityId, int fxtCompanyId)
        {
            string ptable, ctable, btable, comId;
            Access(cityId, fxtCompanyId, out ptable, out ctable, out btable, out comId);
            if (string.IsNullOrEmpty(comId)) comId = fxtCompanyId.ToString();

            var strWhere = projectId == -1 ? "" : " and p.ProjectId != @projectId";

            var strSql = @"
SELECT projectId FROM FxtData_Industry.dbo.Dat_Project_Industry p WITH (NOLOCK)
WHERE NOT EXISTS (
		SELECT ProjectId
		FROM FxtData_Industry.dbo.Dat_Project_Industry_sub p1 WITH (NOLOCK)
		WHERE p1.areaId = p.areaId
			AND p1.cityId = p.cityId
			AND p1.fxtCompanyId = @fxtCompanyId
			AND p1.projectId = p.projectId)
	AND p.valid = 1
	AND p.ProjectName = @ProjectName
	AND p.AreaId = @AreaId
	AND p.CityId = @CityId
	AND p.FxtCompanyId IN (" + comId + @")" + strWhere + @"
union
select projectId
from FxtData_Industry.dbo.Dat_Project_Industry_sub p with (nolock)
where p.valid = 1
	and p.ProjectName=@ProjectName
    and p.AreaId= @AreaId	
	and p.CityId = @CityId
	and p.FxtCompanyId = @fxtCompanyId" + strWhere;

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Query<long>(strSql, new { areaId, projectId, projectName, cityId, comId, fxtCompanyId }).Any();
            }
        }

        public int UpdateProjectIndustry(DatProjectIndustry projectIndustry, int currentCompanyId)
        {
            var strSqlMainUpdate = @"UPDATE FxtData_Industry.dbo.Dat_Project_Industry WITH (ROWLOCK)
                                    SET AreaId = @AreaId,SubAreaId = @SubAreaId,CorrelationType = @CorrelationType,PurposeCode = @PurposeCode,ProjectName = @ProjectName,OtherName = @OtherName,Address = @Address,FieldNo = @FieldNo,LandArea = @LandArea,StartDate = @StartDate,StartEndDate=@StartEndDate,UsableYear = @UsableYear,BuildingArea = @BuildingArea,CubageRate = @CubageRate,GreenRate = @GreenRate,BuildingNum = @BuildingNum,BuildingType = @BuildingType,IndustryType = @IndustryType,EndDate = @EndDate,SaleDate = @SaleDate,OfficeArea = @OfficeArea,BizArea = @BizArea,IndustryArea = @IndustryArea,ManagerPrice = @ManagerPrice,ManagerTel = @ManagerTel,TrafficType = @TrafficType,TrafficDetails = @TrafficDetails,ParkingLevel = @ParkingLevel,ParkingType = @ParkingType,ParkingPrice = @ParkingPrice,RentSaleType = @RentSaleType,AirConditionType = @AirConditionType,Details = @Details,ZZProjectId = @ZZProjectId,East = @East,south = @south,west = @west,north = @north,SaveDateTime = @SaveDateTime,SaveUser = @SaveUser,PinYin = @PinYin,PinYinAll = @PinYinAll,X = @X,Y = @Y,Remarks = @Remarks,AveragePrice = @AveragePrice
                                    WHERE ProjectId = @ProjectId and (FxtCompanyId = @FxtCompanyId or @fxtcompanyid = " + ConfigurationHelper.FxtCompanyId + ")";

            var strSqlSubAdd = @"INSERT INTO FxtData_Industry.dbo.Dat_Project_Industry_Sub (ProjectId,CityId,AreaId,SubAreaId,CorrelationType,PurposeCode,ProjectName,OtherName,Address,FieldNo,LandArea,StartDate,StartEndDate,UsableYear,BuildingArea,CubageRate,GreenRate,BuildingNum,BuildingType,IndustryType,EndDate,SaleDate,OfficeArea,BizArea,IndustryArea,ManagerPrice,ManagerTel,TrafficType,TrafficDetails,ParkingLevel,ParkingType,ParkingPrice,RentSaleType,AirConditionType,Details,ZZProjectId,East,south,west,north,Creator,CreateTime,SaveDateTime,SaveUser,PinYin,PinYinAll,FxtCompanyId,X,Y,Remarks,AveragePrice)
                                VALUES (@ProjectId,@CityId,@AreaId,@SubAreaId,@CorrelationType,@PurposeCode,@ProjectName,@OtherName,@Address,@FieldNo,@LandArea,@StartDate,@StartEndDate,@UsableYear,@BuildingArea,@CubageRate,@GreenRate,@BuildingNum,@BuildingType,@IndustryType,@EndDate,@SaleDate,@OfficeArea,@BizArea,@IndustryArea,@ManagerPrice,@ManagerTel,@TrafficType,@TrafficDetails,@ParkingLevel,@ParkingType,@ParkingPrice,@RentSaleType,@AirConditionType,@Details,@ZZProjectId,@East,@south,@west,@north,@Creator,@CreateTime,@SaveDateTime,@SaveUser,@PinYin,@PinYinAll,@FxtCompanyId,@X,@Y,@Remarks,@AveragePrice)";

            var strSqlSubUpdate = @"UPDATE FxtData_Industry.dbo.Dat_Project_Industry_Sub WITH (ROWLOCK)
                                    SET AreaId = @AreaId,SubAreaId = @SubAreaId,CorrelationType = @CorrelationType,PurposeCode = @PurposeCode,ProjectName = @ProjectName,OtherName = @OtherName,Address = @Address,FieldNo = @FieldNo,LandArea = @LandArea,StartDate = @StartDate,StartEndDate=@StartEndDate,UsableYear = @UsableYear,BuildingArea = @BuildingArea,CubageRate = @CubageRate,GreenRate = @GreenRate,BuildingNum = @BuildingNum,BuildingType = @BuildingType,IndustryType = @IndustryType,EndDate = @EndDate,SaleDate = @SaleDate,OfficeArea = @OfficeArea,BizArea = @BizArea,IndustryArea = @IndustryArea,ManagerPrice = @ManagerPrice,ManagerTel = @ManagerTel,TrafficType = @TrafficType,TrafficDetails = @TrafficDetails,ParkingLevel = @ParkingLevel,ParkingType = @ParkingType,ParkingPrice = @ParkingPrice,RentSaleType = @RentSaleType,AirConditionType = @AirConditionType,Details = @Details,ZZProjectId = @ZZProjectId,East = @East,south = @south,west = @west,north = @north,SaveDateTime = @SaveDateTime,SaveUser = @SaveUser,PinYin = @PinYin,PinYinAll = @PinYinAll,X = @X,Y = @Y,Remarks = @Remarks,AveragePrice = @AveragePrice
                                    WHERE projectid = @projectid and fxtCompanyId = @fxtCompanyId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                projectIndustry.FxtCompanyId = currentCompanyId;
                var mainreturn = conn.Execute(strSqlMainUpdate, projectIndustry);

                if (mainreturn == 0)
                {
                    if (conn.Execute(strSqlSubUpdate, projectIndustry) == 0)
                    {
                        return conn.Execute(strSqlSubAdd, projectIndustry);
                    }
                    return 1;
                }
                return 1;
            }
        }

        public int AddProjectIndustry(DatProjectIndustry projectIndustry)
        {
            var strSql = @"INSERT INTO FxtData_Industry.dbo.Dat_Project_Industry (CityId,AreaId,SubAreaId,CorrelationType,PurposeCode,ProjectName,OtherName,Address,FieldNo,LandArea,StartDate,StartEndDate,UsableYear,BuildingArea,CubageRate,GreenRate,BuildingNum,BuildingType,IndustryType,EndDate,SaleDate,OfficeArea,BizArea,IndustryArea,ManagerPrice,ManagerTel,TrafficType,TrafficDetails,ParkingLevel,ParkingType,ParkingPrice,RentSaleType,AirConditionType,Details,ZZProjectId,East,south,west,north,Creator,CreateTime,SaveDateTime,SaveUser,PinYin,PinYinAll,FxtCompanyId,X,Y,Remarks,AveragePrice)
                            VALUES (@CityId,@AreaId,@SubAreaId,@CorrelationType,@PurposeCode,@ProjectName,@OtherName,@Address,@FieldNo,@LandArea,@StartDate,@StartEndDate,@UsableYear,@BuildingArea,@CubageRate,@GreenRate,@BuildingNum,@BuildingType,@IndustryType,@EndDate,@SaleDate,@OfficeArea,@BizArea,@IndustryArea,@ManagerPrice,@ManagerTel,@TrafficType,@TrafficDetails,@ParkingLevel,@ParkingType,@ParkingPrice,@RentSaleType,@AirConditionType,@Details,@ZZProjectId,@East,@south,@west,@north,@Creator,@CreateTime,@SaveDateTime,@SaveUser,@PinYin,@PinYinAll,@FxtCompanyId,@X,@Y,@Remarks,@AveragePrice)";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Execute(strSql, projectIndustry);
            }
        }

        public int DeleteProjectIndustry(DatProjectIndustry projectIndustry, int currentCompanyId)
        {
            var strSqlMainDelete = @"UPDATE FxtData_Industry.dbo.Dat_Project_Industry WITH (ROWLOCK) SET valid = 0,SaveDateTime = @SaveDateTime,SaveUser = @SaveUser
                                    WHERE projectid = @projectid and (FxtCompanyId = @fxtcompanyid or @fxtcompanyid = @MainCompanyId)";
            var strSqlSubDelete = @"UPDATE FxtData_Industry.dbo.Dat_Project_Industry_Sub WITH (ROWLOCK) SET valid = 0,SaveDateTime = @SaveDateTime,SaveUser = @SaveUser
                                    WHERE projectid = @projectid and fxtCompanyId = @fxtCompanyId";
            var strSqlSubAdd = @"INSERT INTO FxtData_Industry.dbo.Dat_Project_Industry_Sub (ProjectId,CityId,AreaId,SubAreaId,CorrelationType,PurposeCode,ProjectName,OtherName,Address,FieldNo,LandArea,StartDate,StartEndDate,UsableYear,BuildingArea,CubageRate,GreenRate,BuildingNum,BuildingType,IndustryType,EndDate,SaleDate,OfficeArea,BizArea,IndustryArea,ManagerPrice,ManagerTel,TrafficType,TrafficDetails,ParkingLevel,ParkingType,ParkingPrice,RentSaleType,AirConditionType,Details,ZZProjectId,East,south,west,north,Creator,CreateTime,SaveDateTime,SaveUser,PinYin,PinYinAll,FxtCompanyId,X,Y,Remarks,AveragePrice,Valid)
                                 VALUES (@ProjectId,@CityId,@AreaId,@SubAreaId,@CorrelationType,@PurposeCode,@ProjectName,@OtherName,@Address,@FieldNo,@LandArea,@StartDate,@StartEndDate,@UsableYear,@BuildingArea,@CubageRate,@GreenRate,@BuildingNum,@BuildingType,@IndustryType,@EndDate,@SaleDate,@OfficeArea,@BizArea,@IndustryArea,@ManagerPrice,@ManagerTel,@TrafficType,@TrafficDetails,@ParkingLevel,@ParkingType,@ParkingPrice,@RentSaleType,@AirConditionType,@Details,@ZZProjectId,@East,@south,@west,@north,@Creator,@CreateTime,@SaveDateTime,@SaveUser,@PinYin,@PinYinAll,@FxtCompanyId,@X,@Y,@Remarks,@AveragePrice,@Valid)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                projectIndustry.FxtCompanyId = currentCompanyId;
                var mainreturn = conn.Execute(strSqlMainDelete, new { projectid = projectIndustry.ProjectId, fxtcompanyid = projectIndustry.FxtCompanyId, SaveDateTime = projectIndustry.SaveDateTime, SaveUser = projectIndustry.SaveUser, MainCompanyId = ConfigurationHelper.FxtCompanyId });

                if (mainreturn == 0)
                {
                    if (conn.Execute(strSqlSubDelete, projectIndustry) == 0)
                    {
                        projectIndustry.Valid = 0;
                        return conn.Execute(strSqlSubAdd, projectIndustry);
                    }
                    return 1;
                }
                return 1;
            }
        }

        public IQueryable<LNKPPhoto> GetIndustryProjectPhotoes(LNKPPhoto lnkPPhoto, bool self = true)
        {
            string ptable, ctable, btable, comId;
            Access(lnkPPhoto.CityId, lnkPPhoto.FxtCompanyId, out ptable, out ctable, out btable, out comId);
            if (string.IsNullOrEmpty(comId)) comId = lnkPPhoto.FxtCompanyId.ToString();
            if (self) comId = lnkPPhoto.FxtCompanyId.ToString();

            var strSql = @"
SELECT p.*,c.CodeName AS PhotoTypeName
FROM FxtData_Industry.dbo.LNK_P_Photo p WITH (NOLOCK)
LEFT JOIN FxtDataCenter.dbo.SYS_Code c WITH (NOLOCK) ON p.PhotoTypeCode = c.Code
WHERE not exists (
		SELECT id
		FROM FxtData_Industry.dbo.LNK_P_Photo_Sub p1 WITH (NOLOCK)
		WHERE p1.projectId = p.projectId
			AND p1.fxtCompanyId = @fxtCompanyId
			AND p1.cityId = p.cityId
			AND p1.fxtCompanyId = p.fxtCompanyId)
	AND p.valid = 1
	AND p.CityId = @CityId
	AND p.FxtCompanyId IN (" + comId + @")
	AND p.ProjectId = @projectId
UNION
SELECT p.*,c.CodeName AS PhotoTypeName
FROM FxtData_Industry.dbo.LNK_P_Photo_Sub p WITH (NOLOCK)
LEFT JOIN FxtDataCenter.dbo.SYS_Code c WITH (NOLOCK) ON p.PhotoTypeCode = c.Code
WHERE p.valid = 1
	AND p.CityId = @CityId
	AND p.FxtCompanyId = @fxtCompanyId
	AND p.ProjectId = @projectId
ORDER BY 1 DESC";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Query<LNKPPhoto>(strSql, lnkPPhoto).AsQueryable();
            }
        }

        public LNKPPhoto GetIndustryProjectPhoto(int id, int fxtCompanyId)
        {
            var strSql = @"
SELECT p.*
	,c.CodeName AS PhotoTypeName
FROM FxtData_Industry.dbo.LNK_P_Photo p WITH (NOLOCK)
LEFT JOIN FxtDataCenter.dbo.SYS_Code c WITH (NOLOCK) ON p.PhotoTypeCode = c.Code
WHERE not exists (
		SELECT id
		FROM FxtData_Industry.dbo.LNK_P_Photo_Sub p1 WITH (NOLOCK)
		WHERE p1.projectId = p.projectId
			AND p1.cityId = p.cityId
			AND p1.fxtCompanyId = @fxtCompanyId
		)
	AND p.valid = 1
	AND p.id = @id
UNION
SELECT p.*
	,c.CodeName AS PhotoTypeName
FROM FxtData_Industry.dbo.LNK_P_Photo_Sub p WITH (NOLOCK)
LEFT JOIN FxtDataCenter.dbo.SYS_Code c WITH (NOLOCK) ON p.PhotoTypeCode = c.Code
WHERE p.valid = 1
	AND p.id = @id
ORDER BY 1 DESC";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Query<LNKPPhoto>(strSql, new { id = id, fxtCompanyId = fxtCompanyId }).FirstOrDefault();
            }
        }

        public int AddIndustryProjectPhoto(LNKPPhoto lnkPPhoto)
        {
            var strSql = @"INSERT INTO FxtData_Industry.dbo.LNK_P_Photo (projectid,phototypecode,path,photodate,photoname,cityid,fxtcompanyid,saveuser,savedate)
                           VALUES (@projectid,@phototypecode,@path,@photodate,@photoname,@cityid,@fxtcompanyid,@saveuser,@savedate)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Execute(strSql, lnkPPhoto);
            }
        }

        public int UpdateIndustryProjectPhoto(LNKPPhoto lnkPPhoto, int currentCompanyId)
        {
            var strSqlMainUpdate = @"UPDATE FxtData_Industry.dbo.LNK_P_Photo
                                    SET PhotoTypeCode = @PhotoTypeCode,Path = @Path,PhotoName = @PhotoName,SaveUser = @SaveUser,SaveDate = @SaveDate
                                    WHERE Id = @Id and (FxtCompanyId = @FxtCompanyId or @fxtcompanyid = @MainCompanyId)";

            var strSqlSubUpdate = @"UPDATE FxtData_Industry.dbo.LNK_P_Photo_Sub
                                    SET phototypecode = @phototypecode,path = @path,photoname = @photoname,saveuser = @saveuser,savedate = @savedate
                                    WHERE id = @id and fxtCompanyId = @fxtCompanyId";

            var strSqlSubAdd = @"INSERT INTO FxtData_Industry.dbo.LNK_P_Photo_Sub (id,projectid,phototypecode,path,photodate,photoname,cityid,fxtcompanyid,saveuser,savedate)
                                VALUES (@id,@projectid,@phototypecode,@path,@photodate,@photoname,@cityid,@fxtcompanyid,@saveuser,@savedate)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                lnkPPhoto.FxtCompanyId = currentCompanyId;
                var mainreturn = conn.Execute(strSqlMainUpdate, new
                {
                    PhotoTypeCode = lnkPPhoto.PhotoTypeCode,
                    Path = lnkPPhoto.Path,
                    PhotoName = lnkPPhoto.PhotoName,
                    SaveUser = lnkPPhoto.SaveUser,
                    SaveDate = lnkPPhoto.SaveDate,
                    Id = lnkPPhoto.Id,
                    FxtCompanyId = lnkPPhoto.FxtCompanyId,
                    MainCompanyId = ConfigurationHelper.FxtCompanyId
                });

                if (mainreturn == 0)
                {
                    if (conn.Execute(strSqlSubUpdate, lnkPPhoto) == 0)
                    {
                        return conn.Execute(strSqlSubAdd, lnkPPhoto);
                    }
                    return 1;
                }
                return 1;
            }
        }

        public int DeleteIndustryProjectPhoto(LNKPPhoto lnkPPhoto, int currentCompanyId)
        {
            var strSqlMainDelete = @"UPDATE FxtData_Industry.dbo.LNK_P_Photo WITH (ROWLOCK) SET valid = 0,SaveUser = @SaveUser,SaveDate = @SaveDate
                                    WHERE id = @id and (FxtCompanyId = @fxtcompanyid or @fxtcompanyid = @MainCompanyId)";

            var strSqlSubDelete = @"UPDATE FxtData_Industry.dbo.LNK_P_Photo_Sub WITH (ROWLOCK) SET valid = 0,SaveUser = @SaveUser,SaveDate = @SaveDate
                                    WHERE id = @id and fxtCompanyId = @fxtCompanyId";

            var strSqlSubAdd = @"INSERT INTO FxtData_Industry.dbo.LNK_P_Photo_Sub (projectid,phototypecode,path,photodate,photoname,cityid,fxtcompanyid,saveuser,savedate,Valid)
                                VALUES (@projectid,@phototypecode,@path,@photodate,@photoname,@cityid,@fxtcompanyid,@saveuser,@savedate,Valid)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                lnkPPhoto.FxtCompanyId = currentCompanyId;
                var mainreturn = conn.Execute(strSqlMainDelete, new { id = lnkPPhoto.Id, fxtcompanyid = lnkPPhoto.FxtCompanyId, SaveUser = lnkPPhoto.SaveUser, SaveDate = lnkPPhoto.SaveDate, MainCompanyId = ConfigurationHelper.FxtCompanyId });

                if (mainreturn == 0)
                {
                    if (conn.Execute(strSqlSubDelete, lnkPPhoto) == 0)
                    {
                        lnkPPhoto.Valid = 0;
                        return conn.Execute(strSqlSubAdd, lnkPPhoto);
                    }
                    return 1;
                }
                return 1;
            }
        }

        public IQueryable<long> GetProjectIdByName(string projectName, int areaId, int cityId, int companyId)
        {
            string ptable, ctable, btable, comId;
            Access(cityId, companyId, out ptable, out ctable, out btable, out comId);
            comId = string.IsNullOrEmpty(comId) ? companyId.ToString() : comId;

            var strWhere = string.Empty;
            if (areaId > 0) strWhere = "AND p.AreaId = @AreaId";

            //查询语句
            var strSql = @"
SELECT p.ProjectId
FROM FxtData_Industry.dbo.Dat_Project_Industry p WITH (NOLOCK)
WHERE NOT EXISTS (
		SELECT ProjectId
		FROM FxtData_Industry.dbo.Dat_Project_Industry_sub ps WITH (NOLOCK)
		WHERE ps.cityId = p.cityId
			AND ps.fxtCompanyId = @CompanyId
			AND ps.AreaId = p.AreaId
			AND ps.ProjectId = p.ProjectId
		)
	AND p.valid = 1
	AND p.CityId = @CityId
	" + strWhere + @"
	AND p.ProjectName = @ProjectName
	AND p.FxtCompanyId IN (" + comId + @")
UNION
SELECT p.ProjectId
FROM FxtData_Industry.dbo.Dat_Project_Industry_sub p WITH (NOLOCK)
WHERE p.valid = 1
	AND p.CityId = @CityId
	" + strWhere + @"
	AND p.ProjectName = @ProjectName
	AND p.FxtCompanyId = @CompanyId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                var query = conn.Query<long>(strSql, new { ProjectName = projectName, AreaId = areaId, CityId = cityId, CompanyId = companyId }).AsQueryable();
                return query;
            }
        }

        public int GetBuildingCounts(int projectId, int cityId, int fxtCompanyId, bool self)
        {
            string ptable, ctable, btable, comId;
            Access(cityId, fxtCompanyId, out ptable, out ctable, out btable, out comId);
            if (string.IsNullOrEmpty(comId)) comId = fxtCompanyId.ToString();
            if (self) comId = fxtCompanyId.ToString();

            string strSql = @"
SELECT COUNT(*) FROM (
    SELECT *
    FROM FxtData_Industry.dbo.Dat_Building_Industry b WITH (NOLOCK)
    WHERE NOT EXISTS (
		    SELECT BuildingId
		    FROM FxtData_Industry.dbo.Dat_Building_Industry_sub bs WITH (NOLOCK)
		    WHERE bs.CityId = b.CityId
			    AND bs.FxtCompanyId = @fxtcompanyId
			    AND bs.ProjectId = b.ProjectId
			    AND bs.BuildingId = b.BuildingId
		    )
	    AND b.Valid = 1
	    AND b.CityId = @cityId
	    AND b.ProjectId = @projectId
	    AND b.FxtCompanyId IN (" + comId + @")
    UNION
    SELECT *
    FROM FxtData_Industry.dbo.Dat_Building_Industry_sub b WITH (NOLOCK)
    WHERE b.Valid = 1
	    AND b.CityId = @cityId
	    AND b.FxtCompanyId = @fxtcompanyId
	    AND b.ProjectId = @projectId
)T";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Query<int>(strSql, new { projectId, cityId, fxtCompanyId }).FirstOrDefault();
            }
        }

        public int GetProjectCounts(int subAreaId, int cityId, int fxtCompanyId, bool self)
        {
            string ptable, ctable, btable, comId;
            Access(cityId, fxtCompanyId, out ptable, out ctable, out btable, out comId);
            if (string.IsNullOrEmpty(comId)) comId = fxtCompanyId.ToString();
            if (self) comId = fxtCompanyId.ToString();

            string strSql = @"
SELECT COUNT(*) FROM (
	SELECT * FROM FxtData_Industry.dbo.Dat_Project_Industry p WITH (NOLOCK)
	WHERE NOT EXISTS (
			SELECT ProjectId
			FROM FxtData_Industry.dbo.Dat_Project_Industry_sub p1 WITH (NOLOCK)
			WHERE p1.areaId = p.areaId
				AND p1.cityId = p.cityId
				AND p1.fxtCompanyId = @fxtCompanyId
				AND p1.projectId = p.projectId
			)
		AND p.valid = 1
		AND p.CityId = @CityId
		AND p.FxtCompanyId IN (" + comId + @")
		and p.SubAreaId = @subAreaId
	UNION
	SELECT * FROM FxtData_Industry.dbo.Dat_Project_Industry_sub p WITH (NOLOCK)
	WHERE p.valid = 1
		AND p.CityId = @CityId
		AND p.FxtCompanyId = @fxtCompanyId
		and p.SubAreaId = @subAreaId
)T";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Query<int>(strSql, new { subAreaId, cityId, fxtCompanyId }).FirstOrDefault();
            }
        }

        public DataTable ProjectSelfDefineExport(DatProjectIndustry projectIndustry, List<string> projectAttr, int CityId, int FxtCompanyId, bool self = true)
        {
            try
            {
                List<SqlParameter> paramet = new List<SqlParameter>();
                string ptable, ctable, btable, comId;
                Access(projectIndustry.CityId, projectIndustry.FxtCompanyId, out ptable, out ctable, out btable, out comId);
                if (string.IsNullOrEmpty(comId)) comId = projectIndustry.FxtCompanyId.ToString();
                if (self) comId = projectIndustry.FxtCompanyId.ToString();

                string where = string.Empty;
                if (!(new[] { 0, -1 }).Contains(projectIndustry.AreaId))
                {
                    where += " and p.AreaId = @AreaId";
                    paramet.Add(new SqlParameter("@AreaId", projectIndustry.AreaId));
                }
                if (!(new[] { 0, -1 }).Contains(projectIndustry.SubAreaId))
                {
                    where += "  and p.SubAreaId = @SubAreaId";
                    paramet.Add(new SqlParameter("@SubAreaId", projectIndustry.SubAreaId));
                }
                if (!string.IsNullOrEmpty(projectIndustry.ProjectName))
                {
                    where += " and p.ProjectName like @ProjectName";
                    paramet.Add(new SqlParameter("@ProjectName", projectIndustry.ProjectName));
                }
                if (!string.IsNullOrEmpty(projectIndustry.OtherName))
                {
                    where += "  and p.OtherName like @OtherName";
                    paramet.Add(new SqlParameter("@OtherName", projectIndustry.OtherName));
                }
                if (!(new[] { 0, -1 }).Contains(projectIndustry.IndustryType ?? -1))
                {
                    where += "  and p.IndustryType = @IndustryType";
                    paramet.Add(new SqlParameter("@IndustryType", projectIndustry.IndustryType));
                }
                if (!(new[] { 0, -1 }).Contains(projectIndustry.RentSaleType ?? -1))
                {
                    where += "  and p.RentSaleType = @RentSaleType";
                    paramet.Add(new SqlParameter("@RentSaleType", projectIndustry.RentSaleType));
                }

                var strSql = @"
SELECT p.ProjectName
	,p.OtherName
	,p.PinYin
	,p.PinYinAll
	,p.FieldNo
	,p.Address
	,p.East
	,p.south
	,p.west
	,p.north
	,convert(nvarchar(50),p.X) as X
	,convert(nvarchar(50),p.Y) as Y
	,convert(nvarchar(10),p.StartDate,121) as StartDate
    ,convert(nvarchar(10),p.StartEndDate,121) as StartEndDate
	,convert(nvarchar(10),p.UsableYear,121) as UsableYear
	,convert(nvarchar(20),p.LandArea) as LandArea
	,convert(nvarchar(20),p.BuildingArea) as BuildingArea
	,convert(nvarchar(9),p.CubageRate) as CubageRate
	,convert(nvarchar(9),p.GreenRate) as GreenRate
	,convert(nvarchar(5),p.BuildingNum) as BuildingNum
	,convert(nvarchar(10),p.EndDate,121) as EndDate
	,convert(nvarchar(10),p.SaleDate,121) as SaleDate
	,convert(nvarchar(20),p.OfficeArea) as OfficeArea
	,convert(nvarchar(20),p.BizArea) as BizArea
	,convert(nvarchar(20),p.IndustryArea) as IndustryArea
	,p.ManagerPrice
	,p.ManagerTel
	,p.TrafficDetails
	,p.ParkingPrice
	,p.Details
	,p.Remarks
	,convert(nvarchar(20),p.AveragePrice) as AveragePrice
	,c.CodeName AS CorrelationTypeName
	,c1.CodeName AS TrafficTypeName
	,c2.CodeName as IndustryTypeName
	,c3.CodeName as RentSaleTypeName
	,c4.CodeName as PurposeCodeName
	,c5.CodeName as BuildingTypeName
	,c6.CodeName as ParkingLevelName
	,c7.CodeName as ParkingTypeName
	,c8.CodeName as AirConditionTypeName
	,a.AreaName
	,sb.SubAreaName
FROM FxtData_Industry.dbo.Dat_Project_Industry p WITH (NOLOCK)
LEFT JOIN FxtDataCenter.dbo.SYS_SubArea_Industry sb WITH (NOLOCK) ON p.SubAreaId = sb.SubAreaId
LEFT JOIN FxtDataCenter.dbo.SYS_Area a WITH (NOLOCK) ON p.AreaId = a.AreaId
LEFT JOIN FxtDataCenter.dbo.SYS_Code c WITH (NOLOCK) ON p.CorrelationType = c.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c1 WITH (NOLOCK) ON p.TrafficType = c1.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c2 WITH (NOLOCK) ON p.IndustryType = c2.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c3 WITH (NOLOCK) ON p.RentSaleType = c3.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c4 WITH (NOLOCK) ON p.PurposeCode = c4.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c5 WITH (NOLOCK) ON p.BuildingType = c5.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c6 WITH (NOLOCK) ON p.ParkingLevel = c6.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c7 WITH (NOLOCK) ON p.ParkingType = c7.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c8 WITH (NOLOCK) ON p.AirConditionType = c8.Code
WHERE not exists (
		SELECT ProjectId
		FROM FxtData_Industry.dbo.Dat_Project_Industry_sub p1 WITH (NOLOCK)
		WHERE p1.areaId = p.areaId
			AND p1.cityId = p.cityId
			AND p1.fxtCompanyId =  @fxtCompanyId
			AND p1.projectId = p.projectId)
	AND p.valid = 1
	AND p.CityId = @CityId
	AND p.FxtCompanyId IN (" + comId + @")" + where + @"                            
UNION 
SELECT  
	p.ProjectName
	,p.OtherName
	,p.PinYin
	,p.PinYinAll
	,p.FieldNo
	,p.Address
	,p.East
	,p.south
	,p.west
	,p.north
	,convert(nvarchar(50),p.X) as X
	,convert(nvarchar(50),p.Y) as Y
	,convert(nvarchar(10),p.StartDate,121) as StartDate
    ,convert(nvarchar(10),p.StartEndDate,121) as StartEndDate
	,convert(nvarchar(10),p.UsableYear,121) as UsableYear
	,convert(nvarchar(20),p.LandArea) as LandArea
	,convert(nvarchar(20),p.BuildingArea) as BuildingArea
	,convert(nvarchar(9),p.CubageRate) as CubageRate
	,convert(nvarchar(9),p.GreenRate) as GreenRate
	,convert(nvarchar(5),p.BuildingNum) as BuildingNum
	,convert(nvarchar(10),p.EndDate,121) as EndDate
	,convert(nvarchar(10),p.SaleDate,121) as SaleDate
	,convert(nvarchar(20),p.OfficeArea) as OfficeArea
	,convert(nvarchar(20),p.BizArea) as BizArea
	,convert(nvarchar(20),p.IndustryArea) as IndustryArea
	,p.ManagerPrice
	,p.ManagerTel
	,p.TrafficDetails
	,p.ParkingPrice
	,p.Details
	,p.Remarks
	,convert(nvarchar(20),p.AveragePrice) as AveragePrice
	,c.CodeName AS CorrelationTypeName
	,c1.CodeName AS TrafficTypeName
	,c2.CodeName as IndustryTypeName
	,c3.CodeName as RentSaleTypeName
	,c4.CodeName as PurposeCodeName
	,c5.CodeName as BuildingTypeName
	,c6.CodeName as ParkingLevelName
	,c7.CodeName as ParkingTypeName
	,c8.CodeName as AirConditionTypeName
	,a.AreaName
	,sb.SubAreaName
FROM FxtData_Industry.dbo.Dat_Project_Industry_sub p WITH (NOLOCK)
LEFT JOIN FxtDataCenter.dbo.SYS_SubArea_Industry sb WITH (NOLOCK) ON p.SubAreaId = sb.SubAreaId
LEFT JOIN FxtDataCenter.dbo.SYS_Area a WITH (NOLOCK) ON p.AreaId = a.AreaId
LEFT JOIN FxtDataCenter.dbo.SYS_Code c WITH (NOLOCK) ON p.CorrelationType = c.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c1 WITH (NOLOCK) ON p.TrafficType = c1.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c2 WITH (NOLOCK) ON p.IndustryType = c2.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c3 WITH (NOLOCK) ON p.RentSaleType = c3.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c4 WITH (NOLOCK) ON p.PurposeCode = c4.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c5 WITH (NOLOCK) ON p.BuildingType = c5.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c6 WITH (NOLOCK) ON p.ParkingLevel = c6.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c7 WITH (NOLOCK) ON p.ParkingType = c7.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c8 WITH (NOLOCK) ON p.AirConditionType = c8.Code
WHERE p.valid = 1
	AND p.CityId = @CityId
	AND p.FxtCompanyId = @fxtCompanyId" + where;

                paramet.Add(new SqlParameter("@CityId", CityId));
                paramet.Add(new SqlParameter("@FxtCompanyId", FxtCompanyId));
                string paramList = string.Empty;
                for (int i = 0; i < projectAttr.Count; i++)
                {
                    paramList += projectAttr[i].Replace("&", " as ") + ",";
                }
                string sql = "select " + paramList.TrimEnd(',') + " from (" + strSql + ")T";

                SqlParameter[] param = paramet.ToArray();
                DBHelperSql.ConnectionString = ConfigurationHelper.FxtDataIndustry;
                DataTable dtable = DBHelperSql.ExecuteDataTable(sql, param);
                return dtable;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
    }
}
