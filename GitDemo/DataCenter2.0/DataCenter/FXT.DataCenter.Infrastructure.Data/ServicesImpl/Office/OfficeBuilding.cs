using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using System.Data.SqlClient;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class OfficeBuilding : IOfficeBuilding
    {

        public IQueryable<DatBuildingOffice> GetOfficeBuildings(DatBuildingOffice datBuildingOffice, bool self, int pageIndex, int pageSize, out int totalCount)
        {

            var access = Access(datBuildingOffice.CityId, datBuildingOffice.FxtCompanyId);
            var companyIds = string.IsNullOrEmpty(access.Item4) || self
                ? datBuildingOffice.FxtCompanyId.ToString()
                : access.Item4;

            //查询条件
            var sb = new StringBuilder();
            if (datBuildingOffice.OfficeType > 0)
                sb.Append("  and b.OfficeType = @OfficeType");
            if (datBuildingOffice.PurposeCode > 0)
                sb.Append(" and b.PurposeCode = @PurposeCode");
            if (datBuildingOffice.StructureCode > 0)
                sb.Append("  and b.StructureCode = @StructureCode");
            if (datBuildingOffice.BuildingTypeCode > 0)
                sb.Append(" and b.BuildingTypeCode = @BuildingTypeCode");
            if (!string.IsNullOrEmpty(datBuildingOffice.BuildingName))
                sb.Append(" and b.BuildingName like @BuildingName");
            if (datBuildingOffice.RentSaleType > 0)
                sb.Append(" and b.RentSaleType like @RentSaleType");

            //查询语句
            var strSql = @"
SELECT b.*
	,a.AreaName
	,sa.SubAreaName
	,p.ProjectName
	,c.CodeName AS OfficeTypeName
	,c1.CodeName AS PurposeName
	,c2.CodeName AS StructureName
	,c3.CodeName AS BuildingTypeName
	,c4.CodeName AS RentSaleTypeName
	,c5.CodeName AS LobbyFitmentName
	,c6.CodeName AS LiftFitmentName
	,c7.CodeName AS PublicFitmentName
	,c8.CodeName AS WallFitmentName
FROM FxtData_Office.dbo.Dat_Building_Office b WITH (NOLOCK)
left join FxtData_Office.dbo.Dat_Project_Office p with(nolock) on b.ProjectId = p.ProjectId
left join FxtDataCenter.dbo.SYS_Area a with(nolock) on p.AreaId = a.AreaId
left join FxtDataCenter.dbo.SYS_SubArea_Office sa with(nolock) on p.SubAreaId = sa.SubAreaId
LEFT JOIN FxtDataCenter.dbo.SYS_Code c WITH (NOLOCK) ON b.OfficeType = c.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c1 WITH (NOLOCK) ON b.PurposeCode = c1.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c2 WITH (NOLOCK) ON b.StructureCode = c2.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c3 WITH (NOLOCK) ON b.BuildingTypeCode = c3.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c4 WITH (NOLOCK) ON b.RentSaleType = c4.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c5 WITH (NOLOCK) ON b.LobbyFitment = c5.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c6 WITH (NOLOCK) ON b.LiftFitment = c6.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c7 WITH (NOLOCK) ON b.PublicFitment = c7.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c8 WITH (NOLOCK) ON b.WallFitment = c8.Code
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
	AND b.ProjectId = @ProjectId
	AND b.FxtCompanyId IN (" + companyIds + @") " + sb + @"
UNION
SELECT b.*
	,a.AreaName
	,sa.SubAreaName
	,p.ProjectName
	,c.CodeName AS OfficeTypeName
	,c1.CodeName AS PurposeName
	,c2.CodeName AS StructureName
	,c3.CodeName AS BuildingTypeName
	,c4.CodeName AS RentSaleTypeName
	,c5.CodeName AS LobbyFitmentName
	,c6.CodeName AS LiftFitmentName
	,c7.CodeName AS PublicFitmentName
	,c8.CodeName AS WallFitmentName
FROM FxtData_Office.dbo.Dat_Building_Office_sub b WITH (NOLOCK)
left join FxtData_Office.dbo.Dat_Project_Office p with(nolock) on b.ProjectId = p.ProjectId
left join FxtDataCenter.dbo.SYS_Area a with(nolock) on p.AreaId = a.AreaId
left join FxtDataCenter.dbo.SYS_SubArea_Office sa with(nolock) on p.SubAreaId = sa.SubAreaId
LEFT JOIN FxtDataCenter.dbo.SYS_Code c WITH (NOLOCK) ON b.OfficeType = c.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c1 WITH (NOLOCK) ON b.PurposeCode = c1.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c2 WITH (NOLOCK) ON b.StructureCode = c2.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c3 WITH (NOLOCK) ON b.BuildingTypeCode = c3.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c4 WITH (NOLOCK) ON b.RentSaleType = c4.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c5 WITH (NOLOCK) ON b.LobbyFitment = c5.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c6 WITH (NOLOCK) ON b.LiftFitment = c6.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c7 WITH (NOLOCK) ON b.PublicFitment = c7.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c8 WITH (NOLOCK) ON b.WallFitment = c8.Code
WHERE b.valid = 1
	AND b.CityId = @CityId
	AND b.ProjectId = @ProjectId
	AND b.FxtCompanyId = @fxtCompanyId" + sb;

            //分页SQL
            var pagenatedSql = @"select top " + pageSize + @" tt.*
                                from (
	                                select row_number() over (
			                                order by t.BuildingId desc
			                                ) rownumber
		                                ,t.*
	                                from (" + strSql + @") t ) tt
                                where tt.rownumber > (" + pageIndex + @" - 1) * " + pageSize;

            //总条数SQL
            var totalCountSql = "select count(1) from (" + strSql + ") as t1";

            //参数
            var param = new
            {
                datBuildingOffice.CityId,
                datBuildingOffice.FxtCompanyId,
                datBuildingOffice.ProjectId,
                datBuildingOffice.OfficeType,
                datBuildingOffice.PurposeCode,
                datBuildingOffice.StructureCode,
                datBuildingOffice.BuildingTypeCode,
                BuildingName = "%" + datBuildingOffice.BuildingName + "%",
                datBuildingOffice.RentSaleType,
            };

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                totalCount = conn.Query<int>(totalCountSql, param).FirstOrDefault();
                return conn.Query<DatBuildingOffice>(pagenatedSql, param).AsQueryable();
            }
        }

        public IQueryable<DatBuildingOffice> GetOfficeBuildings(long projectId, int fxtCompanyId)
        {
            const string strSql = @"
SELECT b.buildingId,b.buildingName
FROM FxtData_Office.dbo.Dat_Building_Office b WITH (NOLOCK)
WHERE NOT EXISTS (
		SELECT BuildingId
		FROM FxtData_Office.dbo.Dat_Building_Office_sub b1 WITH (NOLOCK)
		WHERE b1.cityId = b.cityId
			AND b1.fxtCompanyId = @fxtCompanyId
			AND b1.BuildingId = b.BuildingId
		)
	AND b.valid = 1
	AND b.projectId = @projectId
UNION
SELECT b.buildingId,b.buildingName
FROM FxtData_Office.dbo.Dat_Building_Office_sub b WITH (NOLOCK)
WHERE b.valid = 1
	AND b.projectId = @projectId
	AND b.fxtCompanyId = @fxtCompanyId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                return conn.Query<DatBuildingOffice>(strSql, new { projectId, fxtCompanyId }).AsQueryable();
            }
        }

        public IQueryable<DatBuildingOffice> GetOfficeBuildings(int cityId, int fxtCompanyId, int areaId, int subAreaId, int projectId = -1, int buildingId = -1, bool self = true)
        {
            var access = Access(cityId, fxtCompanyId);
            var companyIds = string.IsNullOrEmpty(access.Item4) || self
                ? fxtCompanyId.ToString()
                : access.Item4;

            //查询条件
            var projectWhere = new StringBuilder();
            var buildingWhere = new StringBuilder();
            if (areaId > 0)
                projectWhere.Append("  and p.AreaId = @AreaId");
            if (subAreaId > 0)
                projectWhere.Append("  and p.SubAreaId = @SubAreaId");
            if (projectId > 0)
            {
                projectWhere.Append("  and p.ProjectId = @projectId");
                buildingWhere.Append(" and b.ProjectId = @projectId");
            }
            if (buildingId > 0)
                buildingWhere.Append(" and b.BuildingId = @BuildingId");

            //查询语句
            var strSql = @"
select buildingTable.* from (
	SELECT b.*
	FROM FxtData_Office.dbo.Dat_Building_Office b WITH (NOLOCK)
	WHERE NOT EXISTS (
			SELECT BuildingId
			FROM FxtData_Office.dbo.Dat_Building_Office_sub b1 WITH (NOLOCK)
			WHERE b1.cityId = b.cityId
				AND b1.fxtCompanyId = @fxtCompanyId
				AND b1.BuildingId = b.BuildingId
				AND b1.ProjectId = b.ProjectId)
		AND b.valid = 1
		AND b.CityId = @CityId
		AND b.FxtCompanyId IN (" + companyIds + @")" + buildingWhere + @"
	UNION
	SELECT b.*
	FROM FxtData_Office.dbo.Dat_Building_Office_sub b WITH (NOLOCK)
	WHERE b.valid = 1
		AND b.CityId = @CityId
		AND b.FxtCompanyId = @fxtCompanyId" + buildingWhere + @"
)buildingTable
inner join (	
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
		AND p.FxtCompanyId IN (" + companyIds + @")" + projectWhere + @"
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
		AND p.FxtCompanyId = @fxtCompanyId" + projectWhere + @"
)projectTable on buildingTable.ProjectId = projectTable.ProjectId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                return conn.Query<DatBuildingOffice>(strSql, new { CityId = cityId, fxtCompanyId = fxtCompanyId, AreaId = areaId, SubAreaId = subAreaId, ProjectId = projectId, BuildingId = buildingId }).AsQueryable();
            }
        }

        public DatBuildingOffice GetOfficeBuilding(int buildingId, int fxtCompanyId)
        {
            const string strSql = @"
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
	AND b.buildingId = @buildingId
UNION
SELECT b.*
FROM FxtData_Office.dbo.Dat_Building_Office_sub b WITH (NOLOCK)
WHERE b.valid = 1
	AND b.buildingId = @buildingId
	AND b.fxtCompanyId = @fxtCompanyId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                return conn.Query<DatBuildingOffice>(strSql, new { buildingId, fxtCompanyId }).AsQueryable().FirstOrDefault();
            }
        }

        public long GetOfficeBuildingId(long projectId, long buildingId, string buildingName, int cityId, int companyId)
        {
            var access = Access(cityId, companyId);
            var companyIds = string.IsNullOrEmpty(access.Item4) ? companyId.ToString() : access.Item4;

            var strWhere = buildingId <= 0 ? "" : " and b.BuildingId != @BuildingId";

            //查询语句
            var strSql = @"
SELECT b.BuildingId
FROM FxtData_Office.dbo.Dat_Building_Office b WITH (NOLOCK)
WHERE NOT EXISTS (
		SELECT BuildingId
		FROM FxtData_Office.dbo.Dat_Building_Office_sub b1 WITH (NOLOCK)
		WHERE b1.cityId = b.cityId
			AND b1.fxtCompanyId = @CompanyId
			AND b1.BuildingId = b.BuildingId
		)
	AND b.valid = 1
	AND b.CityId = @CityId
	AND b.ProjectId = @projectId
	AND b.BuildingName = @BuildingName
	AND b.FxtCompanyId IN (" + companyIds + @")" + strWhere + @"
UNION
SELECT b.BuildingId
FROM FxtData_Office.dbo.Dat_Building_Office_sub b WITH (NOLOCK)
WHERE b.valid = 1
	AND b.CityId = @CityId
	AND b.FxtCompanyId = @CompanyId
	AND b.ProjectId = @projectId
	AND b.BuildingName = @BuildingName" + strWhere;

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                var query = conn.Query<long>(strSql, new { projectId, buildingId, buildingName, cityId, companyId }).AsQueryable();
                return query.FirstOrDefault();
            }

        }

        public int CopyBuilding(int cityId, int companyId, string buildingName, string destBuildingName, int buildingId, int projectId)
        {
            var access = Access(cityId, companyId);
            var companyIds = string.IsNullOrEmpty(access.Item4) ? companyId.ToString() : access.Item4;

            var strSql = @"SELECT b.BuildingId
                            FROM FxtData_Office.dbo.Dat_Building_Office b WITH (NOLOCK)
                            WHERE NOT EXISTS (
		                            SELECT BuildingId
		                            FROM FxtData_Office.dbo.Dat_Building_Office_sub b1 WITH (NOLOCK)
		                            WHERE b1.cityId = b.cityId
			                            AND b1.fxtCompanyId = @CompanyId
			                            AND b1.BuildingId = b.BuildingId
		                            )
	                            AND b.valid = 1
	                            AND b.CityId = @CityId
	                            AND b.ProjectId = @projectId
	                            AND b.BuildingName = @BuildingName
	                            AND b.FxtCompanyId IN (" + companyIds + @")
                            UNION
                            SELECT b.BuildingId
                            FROM FxtData_Office.dbo.Dat_Building_Office_sub b WITH (NOLOCK)
                            WHERE b.valid = 1
	                            AND b.CityId = @CityId
	                            AND b.FxtCompanyId = @CompanyId
	                            AND b.ProjectId = @projectId
	                            AND b.BuildingName = @BuildingName";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                var destBuildingId = conn.Query<long>(strSql, new { cityId, companyId, projectId, BuildingName = destBuildingName, }).FirstOrDefault();

                var ret = 1;
                var officeHouse = new OfficeHouse();
                var houses = officeHouse.GetOfficeHouseList(buildingId, cityId, companyId, false);

                if (destBuildingId > 0)//目标楼栋已存在
                {
                    var destHouses = officeHouse.GetOfficeHouseList(Convert.ToInt32(destBuildingId), cityId, companyId, false);
                    if (destHouses.Any()) //目标楼栋下已存在房号
                    {
                        return -1;
                    }
                    foreach (var item in houses)
                    {
                        item.BuildingId = destBuildingId;
                        item.CityId = cityId;
                        item.FxtCompanyId = companyId;
                        ret += officeHouse.AddOfficeHouse(item);
                    }
                }
                else
                {
                    var building = GetOfficeBuilding(buildingId, companyId);
                    building.BuildingName = destBuildingName;
                    building.CityId = cityId;
                    building.FxtCompanyId = companyId;

                    var newBuildingId = AddOfficeBuilding(building);
                    if (newBuildingId <= 0) return -2; //楼栋复制失败
                    if (newBuildingId > 0)
                    {
                        foreach (var item in houses)
                        {
                            item.BuildingId = newBuildingId;
                            item.CityId = cityId;
                            item.FxtCompanyId = companyId;
                            ret += officeHouse.AddOfficeHouse(item);
                        }
                    }
                }

                return ret;
            }

        }

        public int AddOfficeBuilding(DatBuildingOffice datBuildingOffice)
        {
            const string strSql = @"insert into FxtData_Office.dbo.Dat_Building_Office (projectid,cityid,buildingname,othername,officetype,purposecode,structurecode,buildingtypecode,totalfloor,totalhigh,buildingarea,enddate,saledate,rentsaletype,officearea,officefloor,podiumbuildingnum,basementnum,functional,lobbyarea,lobbyhigh,lobbyfitment,liftnum,liftfitment,liftbrand,toiletbrand,publicfitment,wallfitment,floorhigh,fxtcompanyid,x,y,creator,createtime,savedatetime,saveuser,remarks,AveragePrice,PriceDetail,Weight) 
values(@projectid,@cityid,@buildingname,@othername,@officetype,@purposecode,@structurecode,@buildingtypecode,@totalfloor,@totalhigh,@buildingarea,@enddate,@saledate,@rentsaletype,@officearea,@officefloor,@podiumbuildingnum,@basementnum,@functional,@lobbyarea,@lobbyhigh,@lobbyfitment,@liftnum,@liftfitment,@liftbrand,@toiletbrand,@publicfitment,@wallfitment,@floorhigh,@fxtcompanyid,@x,@y,@creator,@createtime,@savedatetime,@saveuser,@remarks,@AveragePrice,@PriceDetail,@Weight);
select SCOPE_IDENTITY() as Id";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                //var ret = conn.Execute(strSql, datBuildingOffice);
                //if (ret == 0) return ret;
                //dynamic identity = conn.Query("SELECT @@IDENTITY AS Id").Single();
                //return Convert.ToInt32(identity.Id);
                dynamic identity = conn.Query(strSql, datBuildingOffice).Single();
                return Convert.ToInt32(identity.Id);
            }
        }

        public int UpdateOfficeBuilding(DatBuildingOffice datBuildingOffice, int currentCompanyId)
        {
            var strSqlMainUpdate = @"update FxtData_Office.dbo.Dat_Building_Office with(rowlock) set projectid = @projectid,buildingname = @buildingname,othername = @othername,officetype = @officetype,purposecode = @purposecode,structurecode = @structurecode,buildingtypecode = @buildingtypecode,totalfloor = @totalfloor,totalhigh = @totalhigh,buildingarea = @buildingarea,enddate = @enddate,saledate = @saledate,rentsaletype = @rentsaletype,officearea = @officearea,officefloor = @officefloor,podiumbuildingnum = @podiumbuildingnum,basementnum = @basementnum,functional = @functional,lobbyarea = @lobbyarea,lobbyhigh = @lobbyhigh,lobbyfitment = @lobbyfitment,liftnum = @liftnum,liftfitment = @liftfitment,liftbrand = @liftbrand,toiletbrand = @toiletbrand,publicfitment = @publicfitment,wallfitment = @wallfitment,floorhigh = @floorhigh,x = @x,y = @y,savedatetime = @savedatetime,saveuser = @saveuser,remarks = @remarks,AveragePrice = @AveragePrice,PriceDetail = @PriceDetail,Weight = @Weight
where buildingid = @buildingid and (FxtCompanyId = @FxtCompanyId or @fxtcompanyid = " + ConfigurationHelper.FxtCompanyId + ")";

            var strSqlSubUpdate = @"update FxtData_Office.dbo.Dat_Building_Office_Sub with(rowlock)  set projectid = @projectid,buildingname = @buildingname,othername = @othername,officetype = @officetype,purposecode = @purposecode,structurecode = @structurecode,buildingtypecode = @buildingtypecode,totalfloor = @totalfloor,totalhigh = @totalhigh,buildingarea = @buildingarea,enddate = @enddate,saledate = @saledate,rentsaletype = @rentsaletype,officearea = @officearea,officefloor = @officefloor,podiumbuildingnum = @podiumbuildingnum,basementnum = @basementnum,functional = @functional,lobbyarea = @lobbyarea,lobbyhigh = @lobbyhigh,lobbyfitment = @lobbyfitment,liftnum = @liftnum,liftfitment = @liftfitment,liftbrand = @liftbrand,toiletbrand = @toiletbrand,publicfitment = @publicfitment,wallfitment = @wallfitment,floorhigh = @floorhigh,x = @x,y = @y,savedatetime = @savedatetime,saveuser = @saveuser,remarks = @remarks,AveragePrice = @AveragePrice,PriceDetail = @PriceDetail,Weight = @Weight
where buildingid = @buildingid and FxtCompanyId = @FxtCompanyId";

            var strSqlSubAdd = @"insert into FxtData_Office.dbo.Dat_Building_Office (buildingid,projectid,cityid,buildingname,othername,officetype,purposecode,structurecode,buildingtypecode,totalfloor,totalhigh,buildingarea,enddate,saledate,rentsaletype,officearea,officefloor,podiumbuildingnum,basementnum,functional,lobbyarea,lobbyhigh,lobbyfitment,liftnum,liftfitment,liftbrand,toiletbrand,publicfitment,wallfitment,floorhigh,fxtcompanyid,x,y,creator,createtime,savedatetime,saveuser,remarks,AveragePrice,PriceDetail,Weight) 
values(@buildingid,@projectid,@cityid,@buildingname,@othername,@officetype,@purposecode,@structurecode,@buildingtypecode,@totalfloor,@totalhigh,@buildingarea,@enddate,@saledate,@rentsaletype,@officearea,@officefloor,@podiumbuildingnum,@basementnum,@functional,@lobbyarea,@lobbyhigh,@lobbyfitment,@liftnum,@liftfitment,@liftbrand,@toiletbrand,@publicfitment,@wallfitment,@floorhigh,@fxtcompanyid,@x,@y,@creator,@createtime,@savedatetime,@saveuser,@remarks,@AveragePrice,@PriceDetail,@Weight)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                datBuildingOffice.FxtCompanyId = currentCompanyId;
                var mainreturn = conn.Execute(strSqlMainUpdate, datBuildingOffice);

                if (mainreturn == 0)
                {
                    if (conn.Execute(strSqlSubUpdate, datBuildingOffice) == 0)
                    {
                        return conn.Execute(strSqlSubAdd, datBuildingOffice);
                    }
                    return 1;
                }
                return 1;
            }
        }

        public int DeleteOfficeBuilding(DatBuildingOffice datBuildingOffice, int currentCompanyId)
        {
            var strSqlMainDelete = @"Update FxtData_Office.dbo.Dat_Building_Office  with(rowlock) set valid = 0,savedatetime = @savedatetime,saveuser = @saveuser where buildingId = @buildingId and (FxtCompanyId = @FxtCompanyId or @fxtcompanyid = " + ConfigurationHelper.FxtCompanyId + ")";
            var strSqlSubDelete = @"Update FxtData_Office.dbo.Dat_Building_Office  with(rowlock) set valid = 0,savedatetime = @savedatetime,saveuser = @saveuser where buildingId = @buildingId and FxtCompanyId = @FxtCompanyId";
            var strSqlSubAdd = @"insert into FxtData_Office.dbo.Dat_Building_Office (buildingid,projectid,cityid,buildingname,othername,officetype,purposecode,structurecode,buildingtypecode,totalfloor,totalhigh,buildingarea,enddate,saledate,rentsaletype,officearea,officefloor,podiumbuildingnum,basementnum,functional,lobbyarea,lobbyhigh,lobbyfitment,liftnum,liftfitment,liftbrand,toiletbrand,publicfitment,wallfitment,floorhigh,fxtcompanyid,x,y,creator,createtime,savedatetime,saveuser,remarks,AveragePrice,PriceDetail,Valid,Weight) 
values(@buildingid,@projectid,@cityid,@buildingname,@othername,@officetype,@purposecode,@structurecode,@buildingtypecode,@totalfloor,@totalhigh,@buildingarea,@enddate,@saledate,@rentsaletype,@officearea,@officefloor,@podiumbuildingnum,@basementnum,@functional,@lobbyarea,@lobbyhigh,@lobbyfitment,@liftnum,@liftfitment,@liftbrand,@toiletbrand,@publicfitment,@wallfitment,@floorhigh,@fxtcompanyid,@x,@y,@creator,@createtime,@savedatetime,@saveuser,@remarks,@AveragePrice,@PriceDetail,@Valid,@Weight)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                datBuildingOffice.FxtCompanyId = currentCompanyId;
                var mainreturn = conn.Execute(strSqlMainDelete, datBuildingOffice);

                if (mainreturn == 0)
                {
                    if (conn.Execute(strSqlSubDelete, datBuildingOffice) == 0)
                    {
                        datBuildingOffice.Valid = 0;
                        return conn.Execute(strSqlSubAdd, datBuildingOffice);
                    }
                    return 1;
                }
                return 1;
            }
        }

        public IQueryable<LnkBPhoto> GetOfficeBuildingPhotoes(LnkBPhoto lnkPPhoto, bool self = true)
        {
            var access = Access(lnkPPhoto.CityId, lnkPPhoto.FxtCompanyId);
            var companyIds = string.IsNullOrEmpty(access.Item4) || self
              ? lnkPPhoto.FxtCompanyId.ToString()
              : access.Item4;

            var strSql = @"
SELECT p.*,c.CodeName AS PhotoTypeName
FROM FxtData_Office.dbo.LNK_B_Photo p WITH (NOLOCK)
LEFT JOIN FxtDataCenter.dbo.SYS_Code c WITH (NOLOCK) ON p.PhotoTypeCode = c.Code
WHERE NOT EXISTS (
		SELECT id
		FROM FxtData_Office.dbo.LNK_B_Photo_Sub p1 WITH (NOLOCK)
		WHERE p1.buildingId = p.buildingId
			AND p1.cityId = p.cityId
			AND p1.fxtCompanyId = @fxtCompanyId
		)
	AND p.valid = 1
	AND p.CityId = @CityId
	AND p.FxtCompanyId IN (" + companyIds + @")
	AND p.buildingId = @buildingId
UNION
SELECT p.*
	,c.CodeName AS PhotoTypeName
FROM FxtData_Office.dbo.LNK_B_Photo_Sub p WITH (NOLOCK)
LEFT JOIN FxtDataCenter.dbo.SYS_Code c WITH (NOLOCK) ON p.PhotoTypeCode = c.Code
WHERE p.valid = 1
	AND p.CityId = @CityId
	AND p.FxtCompanyId = @fxtCompanyId
	AND p.buildingId = @buildingId
ORDER BY 1 DESC";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                return conn.Query<LnkBPhoto>(strSql, lnkPPhoto).AsQueryable();
            }
        }

        public LnkBPhoto GetOfficeBuildingPhoto(int id, int fxtCompanyId)
        {
            var strSql = @"
SELECT p.*,c.CodeName AS PhotoTypeName
FROM FxtData_Office.dbo.LNK_B_Photo p WITH (NOLOCK)
LEFT JOIN FxtDataCenter.dbo.SYS_Code c WITH (NOLOCK) ON p.PhotoTypeCode = c.Code
WHERE NOT EXISTS (
		SELECT id
		FROM FxtData_Office.dbo.LNK_B_Photo_Sub p1 WITH (NOLOCK)
		WHERE p1.buildingId = p.buildingId
			AND p1.cityId = p.cityId
			AND p1.fxtCompanyId = @fxtCompanyId
		)
	AND p.valid = 1
	AND p.id = @id
UNION
SELECT p.*,c.CodeName AS PhotoTypeName
FROM FxtData_Office.dbo.LNK_B_Photo_Sub p WITH (NOLOCK)
LEFT JOIN FxtDataCenter.dbo.SYS_Code c WITH (NOLOCK) ON p.PhotoTypeCode = c.Code
WHERE p.valid = 1
	AND p.id = @id
ORDER BY 1 DESC";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                return conn.Query<LnkBPhoto>(strSql, new { id, fxtCompanyId }).FirstOrDefault();
            }
        }

        public int AddOfficeBuildingPhoto(LnkBPhoto lnkPPhoto)
        {
            var strSql = @"insert into FxtData_Office.dbo.LNK_B_Photo  (buildingId,phototypecode,path,photodate,photoname,cityid,fxtcompanyid,saveuser,savedate) 
values(@buildingId,@phototypecode,@path,@photodate,@photoname,@cityid,@fxtcompanyid,@saveuser,@savedate)";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                return conn.Execute(strSql, lnkPPhoto);
            }
        }

        public int UpdateOfficeBuildingPhoto(LnkBPhoto lnkPPhoto, int currentCompanyId)
        {
            var strSqlMainUpdate = @"update FxtData_Office.dbo.LNK_B_Photo  set phototypecode = @phototypecode,path = @path,photoname = @photoname,saveuser = @saveuser,savedate = @savedate
where id = @id and (FxtCompanyId = @FxtCompanyId or @fxtcompanyid = " + ConfigurationHelper.FxtCompanyId + ")";
            var strSqlSubUpdate = @"update FxtData_Office.dbo.LNK_B_Photo_Sub set phototypecode = @phototypecode,path = @path,photoname = @photoname,saveuser = @saveuser,savedate = @savedate
where id = @id and FxtCompanyId = @FxtCompanyId";
            var strSqlSubAdd = @"insert into FxtData_Office.dbo.LNK_B_Photo_Sub (id,buildingId,phototypecode,path,photodate,photoname,cityid,fxtcompanyid,saveuser,savedate) 
values(@id,@buildingId,@phototypecode,@path,@photodate,@photoname,@cityid,@fxtcompanyid,@saveuser,@savedate)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                lnkPPhoto.FxtCompanyId = currentCompanyId;
                var mainreturn = conn.Execute(strSqlMainUpdate, lnkPPhoto);

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

        public int DeleteOfficeBuildingPhoto(LnkBPhoto lnkPPhoto, int currentCompanyId)
        {
            var strSqlMainDelete = @"Update FxtData_Office.dbo.LNK_B_Photo  with(rowlock) set valid = 0,saveuser = @saveuser,savedate = @savedate where id = @id and (FxtCompanyId = @FxtCompanyId or @fxtcompanyid = " + ConfigurationHelper.FxtCompanyId + ")";
            var strSqlSubDelete = @"Update FxtData_Office.dbo.LNK_B_Photo_Sub  with(rowlock) set valid = 0,saveuser = @saveuser,savedate = @savedate where id = @id and FxtCompanyId = @FxtCompanyId";
            var strSqlSubAdd = @"insert into FxtData_Office.dbo.LNK_B_Photo_Sub  (id,buildingId,phototypecode,path,photodate,photoname,cityid,fxtcompanyid,saveuser,savedate) 
values(@id,@buildingId,@phototypecode,@path,@photodate,@photoname,@cityid,@fxtcompanyid,@saveuser,@savedate)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                lnkPPhoto.FxtCompanyId = currentCompanyId;
                var mainreturn = conn.Execute(strSqlMainDelete, lnkPPhoto);

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

        public int GetHouseCounts(int buildingId, int cityId, int fxtCompanyId, bool self)
        {
            var access = Access(cityId, fxtCompanyId);
            var companyIds = string.IsNullOrEmpty(access.Item4) || self
                ? fxtCompanyId.ToString()
                : access.Item4;

            string str = @"
SELECT COUNT(*) FROM (
	SELECT * FROM FxtData_Office.dbo.Dat_House_Office h WITH (NOLOCK)
	WHERE NOT EXISTS (
			SELECT HouseId
			FROM FxtData_Office.dbo.Dat_House_Office_sub hs WITH (NOLOCK)
			WHERE hs.CityId = h.CityId
				AND hs.FxtCompanyId = @fxtcompanyId
				AND hs.ProjectId = h.ProjectId
				AND hs.BuildingId = h.BuildingId
				AND hs.HouseId = h.HouseId
			)
		AND h.Valid = 1
		AND h.CityId = @cityId
		AND h.BuildingId = @buildingId
		AND h.FxtCompanyId IN (" + companyIds + @")
	UNION
	SELECT * FROM fxtdata_office.dbo.Dat_House_Office_sub h WITH (NOLOCK)
	WHERE h.Valid = 1
		AND h.CityId = @cityId
		AND h.FxtCompanyId = @fxtcompanyId
		AND h.BuildingId = @buildingId
)T";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                return conn.Query<int>(str, new { buildingId, cityId, fxtCompanyId }).FirstOrDefault();
            }
        }

        public DataTable BuildingSelfDefineExport(DatBuildingOffice buildingOffice, List<string> buildingAttr, int CityId, int FxtCompanyId, bool self = true)
        {
            try
            {
                List<SqlParameter> paramet = new List<SqlParameter>();
                var access = Access(CityId, FxtCompanyId);
                var companyIds = string.IsNullOrEmpty(access.Item4) || self
                    ? FxtCompanyId.ToString()
                    : access.Item4;

                //查询条件
                var where = " where 1 = 1";
                if (!string.IsNullOrEmpty(buildingOffice.ProjectName))
                {
                    where += " and ProjectName like @ProjectName";
                    paramet.Add(new SqlParameter("@ProjectName", buildingOffice.ProjectName));
                }
                if (!string.IsNullOrEmpty(buildingOffice.BuildingName))
                {
                    where += " and BuildingName like @BuildingName";
                    paramet.Add(new SqlParameter("@BuildingName", buildingOffice.BuildingName));
                }
                if (!string.IsNullOrEmpty(buildingOffice.OtherName))
                {
                    where += " and OtherName like @OtherName";
                    paramet.Add(new SqlParameter("@OtherName", buildingOffice.OtherName));
                }
                if (buildingOffice.OfficeType > 0)
                {
                    where += "  and OfficeType = @OfficeType";
                    paramet.Add(new SqlParameter("@OfficeType", buildingOffice.OfficeType));
                }
                if (buildingOffice.PurposeCode > 0)
                {
                    where += " and PurposeCode = @PurposeCode";
                    paramet.Add(new SqlParameter("@PurposeCode", buildingOffice.PurposeCode));
                }
                if (buildingOffice.StructureCode > 0)
                {
                    where += "  and StructureCode = @StructureCode";
                    paramet.Add(new SqlParameter("@StructureCode", buildingOffice.StructureCode));
                }
                if (buildingOffice.BuildingTypeCode > 0)
                {
                    where += " and BuildingTypeCode = @BuildingTypeCode";
                    paramet.Add(new SqlParameter("@BuildingTypeCode", buildingOffice.BuildingTypeCode));
                }
                if (buildingOffice.RentSaleType > 0)
                {
                    where += " and RentSaleType = @RentSaleType";
                    paramet.Add(new SqlParameter("@RentSaleType", buildingOffice.RentSaleType));
                }

                //查询语句
                var strSql = @"
select 
	buildingTable.*
	,projectTable.AreaName
	,projectTable.ProjectName
	,c.CodeName as OfficeTypeName
	,c1.CodeName as PurposeCodeName
	,c2.CodeName as StructureCodeName
	,c3.CodeName as BuildingTypeCodeName
	,c4.CodeName as RentSaleTypeName
	,c5.CodeName as LobbyFitmentName
	,c6.CodeName as LiftFitmentName
	,c7.CodeName as PublicFitmentName
	,c8.CodeName as WallFitmentName
from (
	SELECT b.*
	FROM FxtData_Office.dbo.Dat_Building_Office b WITH (NOLOCK)
	WHERE NOT EXISTS (
			SELECT BuildingId
			FROM FxtData_Office.dbo.Dat_Building_Office_sub b1 WITH (NOLOCK)
			WHERE b1.cityId = b.cityId
				AND b1.fxtCompanyId = @fxtCompanyId
				AND b1.BuildingId = b.BuildingId
				AND b1.ProjectId = b.ProjectId)
		AND b.valid = 1
		AND b.CityId = @CityId
		AND b.FxtCompanyId IN (" + companyIds + @")
	UNION
	SELECT b.*
	FROM FxtData_Office.dbo.Dat_Building_Office_sub b WITH (NOLOCK)
	WHERE b.valid = 1
		AND b.CityId = @CityId
		AND b.FxtCompanyId = @fxtCompanyId
)buildingTable
inner join (	
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
		AND p.FxtCompanyId IN (" + companyIds + @")
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
)projectTable on buildingTable.ProjectId = projectTable.ProjectId
left join FxtDataCenter.dbo.SYS_Code c on buildingTable.OfficeType = c.Code
left join FxtDataCenter.dbo.SYS_Code c1 on buildingTable.PurposeCode = c1.Code
left join FxtDataCenter.dbo.SYS_Code c2 on buildingTable.StructureCode = c2.Code
left join FxtDataCenter.dbo.SYS_Code c3 on buildingTable.BuildingTypeCode = c3.Code
left join FxtDataCenter.dbo.SYS_Code c4 on buildingTable.RentSaleType = c4.Code
left join FxtDataCenter.dbo.SYS_Code c5 on buildingTable.LobbyFitment = c5.Code
left join FxtDataCenter.dbo.SYS_Code c6 on buildingTable.LiftFitment = c6.Code
left join FxtDataCenter.dbo.SYS_Code c7 on buildingTable.PublicFitment = c7.Code
left join FxtDataCenter.dbo.SYS_Code c8 on buildingTable.WallFitment = c8.Code" + where;

                paramet.Add(new SqlParameter("@CityId", CityId));
                paramet.Add(new SqlParameter("@FxtCompanyId", FxtCompanyId));
                string paramList = string.Empty;
                for (int i = 0; i < buildingAttr.Count; i++)
                {
                    paramList += buildingAttr[i].Replace("&", " as ") + ",";
                }
                string sql = "select " + paramList.TrimEnd(',') + " from (" + strSql + ")T";

                SqlParameter[] param = paramet.ToArray();
                DBHelperSql.ConnectionString = ConfigurationHelper.FxtDataOffice;
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
