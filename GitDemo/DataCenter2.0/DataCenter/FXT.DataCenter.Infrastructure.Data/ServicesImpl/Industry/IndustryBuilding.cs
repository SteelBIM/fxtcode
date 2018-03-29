using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using System.Data.SqlClient;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Common.DBHelper;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class IndustryBuilding : IIndustryBuilding
    {
        public IQueryable<DatBuildingIndustry> GetIndustryBuildings(DatBuildingIndustry datBuildingIndustry, bool self, int pageIndex, int pageSize, out int totalCount)
        {
            var access = Access(datBuildingIndustry.CityId, datBuildingIndustry.FxtCompanyId);
            var companyIds = string.IsNullOrEmpty(access.Item4) || self
                ? datBuildingIndustry.FxtCompanyId.ToString()
                : access.Item4;

            //查询条件
            var sb = new StringBuilder();
            if (datBuildingIndustry.IndustryType > 0)
            {
                sb.Append("  and buildingTable.IndustryType = @IndustryType");
            }
            if (datBuildingIndustry.PurposeCode > 0)
            {
                sb.Append(" and buildingTable.PurposeCode = @PurposeCode");
            }
            if (datBuildingIndustry.StructureCode > 0)
            {
                sb.Append("  and buildingTable.StructureCode = @StructureCode");
            }
            if (datBuildingIndustry.BuildingTypeCode > 0)
            {
                sb.Append(" and buildingTable.BuildingTypeCode = @BuildingTypeCode");
            }
            if (!string.IsNullOrEmpty(datBuildingIndustry.BuildingName))
            {
                sb.Append(" and buildingTable.BuildingName like @BuildingName");
            }
            if (datBuildingIndustry.RentSaleType > 0)
            {
                sb.Append(" and buildingTable.RentSaleType like @RentSaleType");
            }

            //查询语句
            var strSql = @"
select buildingTable.*
	,projectTable.AreaId
	,projectTable.SubAreaId
	,projectTable.ProjectName
	,a.AreaName
	,sa.SubAreaName
	,c.CodeName AS IndustryTypeName
	,c1.CodeName AS PurposeCodeName
	,c2.CodeName AS StructureCodeName
	,c3.CodeName AS BuildingTypeCodeName
	,c4.CodeName AS RentSaleTypeName
	,c5.CodeName AS LobbyFitmentName
	,c6.CodeName AS LiftFitmentName
	,c7.CodeName AS PublicFitmentName
	,c8.CodeName AS WallFitmentName
from (
	SELECT b.*
	FROM FxtData_Industry.dbo.Dat_Building_Industry b WITH (NOLOCK)
	WHERE NOT EXISTS (
			SELECT BuildingId
			FROM FxtData_Industry.dbo.Dat_Building_Industry_sub b1 WITH (NOLOCK)
			WHERE b1.cityId = b.cityId
				AND b1.fxtCompanyId = @fxtCompanyId
				AND b1.BuildingId = b.BuildingId
				AND b1.ProjectId = b.ProjectId
			)
		AND b.valid = 1
		AND b.CityId = @CityId
		AND b.ProjectId = @ProjectId
		AND b.FxtCompanyId IN (" + companyIds + @")
	UNION
	SELECT b.*
	FROM FxtData_Industry.dbo.Dat_Building_Industry_sub b WITH (NOLOCK)
	WHERE b.valid = 1
		AND b.CityId = @CityId
		AND b.ProjectId = @ProjectId
		AND b.FxtCompanyId = @fxtCompanyId
)buildingTable
inner join (
	select * from FxtData_Industry.dbo.Dat_Project_Industry p with (nolock)
	where not exists(
	select ProjectId from FxtData_Industry.dbo.Dat_Project_Industry_sub ps with(nolock)
	where ps.CityId = p.CityId
	and ps.FxtCompanyId = @fxtCompanyId
	and ps.ProjectId = p.ProjectId
	)
	and p.Valid = 1
	and p.CityId = @CityId
	and p.FxtCompanyId in (" + companyIds + @")
	AND p.ProjectId = @ProjectId
	union 
	select * from FxtData_Industry.dbo.Dat_Project_Industry_sub p with(nolock)
	where p.Valid = 1
	and p.CityId = @CityId
	and p.FxtCompanyId = @fxtCompanyId
	and p.ProjectId = @ProjectId
)projectTable on buildingTable.ProjectId = projectTable.ProjectId
left join FxtDataCenter.dbo.SYS_Area a with(nolock) on projectTable.AreaId = a.AreaId
left join FxtDataCenter.dbo.SYS_SubArea_Industry sa with(nolock) on projectTable.SubAreaId = sa.SubAreaId
LEFT JOIN FxtDataCenter.dbo.SYS_Code c WITH (NOLOCK) ON buildingTable.IndustryType = c.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c1 WITH (NOLOCK) ON buildingTable.PurposeCode = c1.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c2 WITH (NOLOCK) ON buildingTable.StructureCode = c2.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c3 WITH (NOLOCK) ON buildingTable.BuildingTypeCode = c3.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c4 WITH (NOLOCK) ON buildingTable.RentSaleType = c4.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c5 WITH (NOLOCK) ON buildingTable.LobbyFitment = c5.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c6 WITH (NOLOCK) ON buildingTable.LiftFitment = c6.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c7 WITH (NOLOCK) ON buildingTable.PublicFitment = c7.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c8 WITH (NOLOCK) ON buildingTable.WallFitment = c8.Code
where 1 = 1" + sb;

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
                datBuildingIndustry.CityId,
                datBuildingIndustry.FxtCompanyId,
                datBuildingIndustry.ProjectId,
                datBuildingIndustry.IndustryType,
                datBuildingIndustry.PurposeCode,
                datBuildingIndustry.StructureCode,
                datBuildingIndustry.BuildingTypeCode,
                BuildingName = "%" + datBuildingIndustry.BuildingName + "%",
                datBuildingIndustry.RentSaleType,
            };

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                totalCount = conn.Query<int>(totalCountSql, param).FirstOrDefault();
                return conn.Query<DatBuildingIndustry>(pagenatedSql, param).AsQueryable();
            }
        }

        public IQueryable<DatBuildingIndustry> GetIndustryBuildings(int cityId, int fxtCompanyId, int areaId, int subAreaId, int projectId = -1, int buildingId = -1, bool self = true)
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
	FROM FxtData_Industry.dbo.Dat_Building_Industry b WITH (NOLOCK)
	WHERE NOT EXISTS (
			SELECT BuildingId
			FROM FxtData_Industry.dbo.Dat_Building_Industry_sub b1 WITH (NOLOCK)
			WHERE b1.cityId = b.cityId
				AND b1.fxtCompanyId = @fxtCompanyId
				AND b1.BuildingId = b.BuildingId
				AND b1.ProjectId = b.ProjectId)
		AND b.valid = 1
		AND b.CityId = @CityId
		AND b.FxtCompanyId IN (" + companyIds + @")" + buildingWhere + @"
	UNION
	SELECT b.*
	FROM FxtData_Industry.dbo.Dat_Building_Industry_sub b WITH (NOLOCK)
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
	FROM FxtData_Industry.dbo.Dat_Project_Industry p WITH (NOLOCK)
	LEFT JOIN FxtDataCenter.dbo.SYS_SubArea_Industry sb WITH (NOLOCK) ON p.SubAreaId = sb.SubAreaId
	LEFT JOIN FxtDataCenter.dbo.SYS_Area a WITH (NOLOCK) ON p.AreaId = a.AreaId
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
	FROM FxtData_Industry.dbo.Dat_Project_Industry_sub p WITH (NOLOCK)
	LEFT JOIN FxtDataCenter.dbo.SYS_SubArea_Industry sb WITH (NOLOCK) ON p.SubAreaId = sb.SubAreaId
	LEFT JOIN FxtDataCenter.dbo.SYS_Area a WITH (NOLOCK) ON p.AreaId = a.AreaId
	WHERE p.valid = 1
		AND p.CityId = @CityId
		AND p.FxtCompanyId = @fxtCompanyId" + projectWhere + @"
)projectTable on buildingTable.ProjectId = projectTable.ProjectId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Query<DatBuildingIndustry>(strSql, new { CityId = cityId, fxtCompanyId = fxtCompanyId, AreaId = areaId, SubAreaId = subAreaId, ProjectId = projectId, BuildingId = buildingId }).AsQueryable();
            }
        }

        public IQueryable<DatBuildingIndustry> GetIndustryBuildings(long projectId, int fxtCompanyId)
        {
            const string strSql = @"
SELECT b.buildingId,b.buildingName
FROM FxtData_Industry.dbo.Dat_Building_Industry b WITH (NOLOCK)
WHERE NOT EXISTS (
		SELECT BuildingId
		FROM FxtData_Industry.dbo.Dat_Building_Industry_sub b1 WITH (NOLOCK)
		WHERE b1.cityId = b.cityId
			AND b1.fxtCompanyId = @fxtCompanyId
			AND b1.BuildingId = b.BuildingId
		)
	AND b.valid = 1
	AND b.projectId = @projectId
UNION
SELECT b.buildingId,b.buildingName
FROM FxtData_Industry.dbo.Dat_Building_Industry_sub b WITH (NOLOCK)
WHERE b.valid = 1
	AND b.projectId = @projectId
	AND b.fxtCompanyId = @fxtCompanyId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Query<DatBuildingIndustry>(strSql, new { projectId, fxtCompanyId }).AsQueryable();
            }
        }

        public DatBuildingIndustry GetIndustryBuilding(int buildingId, int fxtCompanyId)
        {
            const string strSql = @"
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
	AND b.buildingId = @buildingId
UNION
SELECT b.*
FROM FxtData_Industry.dbo.Dat_Building_Industry_sub b WITH (NOLOCK)
WHERE b.valid = 1
	AND b.buildingId = @buildingId
	AND b.fxtCompanyId = @fxtCompanyId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Query<DatBuildingIndustry>(strSql, new { buildingId, fxtCompanyId }).AsQueryable().FirstOrDefault();
            }
        }

        public long GetIndustryBuildingId(long projectId, long buildingId, string buildingName, int cityId, int companyId)
        {
            var access = Access(cityId, companyId);
            var companyIds = string.IsNullOrEmpty(access.Item4) ? companyId.ToString() : access.Item4;

            var strWhere = buildingId <= 0 ? "" : " and b.BuildingId != @BuildingId";

            //查询语句
            var strSql = @"
SELECT b.BuildingId
FROM FxtData_Industry.dbo.Dat_Building_Industry b WITH (NOLOCK)
WHERE NOT EXISTS (
		SELECT BuildingId
		FROM FxtData_Industry.dbo.Dat_Building_Industry_sub b1 WITH (NOLOCK)
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
FROM FxtData_Industry.dbo.Dat_Building_Industry_sub b WITH (NOLOCK)
WHERE b.valid = 1
	AND b.CityId = @CityId
	AND b.FxtCompanyId = @CompanyId
	AND b.ProjectId = @projectId
	AND b.BuildingName = @BuildingName" + strWhere;

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                var query = conn.Query<long>(strSql, new { projectId, buildingId, buildingName, cityId, companyId }).AsQueryable();
                return query.FirstOrDefault();
            }
        }

        public int AddIndustryBuilding(DatBuildingIndustry datBuildingIndustry)
        {
            const string strSql = @"insert into FxtData_Industry.dbo.Dat_Building_Industry (projectid,cityid,buildingname,othername,industrytype,purposecode,structurecode,buildingtypecode,totalfloor,totalhigh,buildingarea,enddate,saledate,rentsaletype,industryarea,industryfloor,podiumbuildingnum,basementnum,functional,lobbyarea,lobbyhigh,lobbyfitment,liftnum,liftfitment,liftbrand,toiletbrand,publicfitment,wallfitment,floorhigh,fxtcompanyid,x,y,creator,createtime,savedatetime,saveuser,remarks,AveragePrice,PriceDetail) 
values(@projectid,@cityid,@buildingname,@othername,@industrytype,@purposecode,@structurecode,@buildingtypecode,@totalfloor,@totalhigh,@buildingarea,@enddate,@saledate,@rentsaletype,@industryarea,@industryfloor,@podiumbuildingnum,@basementnum,@functional,@lobbyarea,@lobbyhigh,@lobbyfitment,@liftnum,@liftfitment,@liftbrand,@toiletbrand,@publicfitment,@wallfitment,@floorhigh,@fxtcompanyid,@x,@y,@creator,@createtime,@savedatetime,@saveuser,@remarks,@AveragePrice,@PriceDetail);
SELECT SCOPE_IDENTITY() AS Id;";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                //var ret = conn.Execute(strSql, datBuildingIndustry);
                //if (ret == 0) return ret;
                //dynamic identity = conn.Query("SELECT @@IDENTITY AS Id").Single();
                //return Convert.ToInt32(identity.Id);
                dynamic identity = conn.Query(strSql, datBuildingIndustry).Single();
                return Convert.ToInt32(identity.Id);
            }
        }

        public int UpdateIndustryBuilding(DatBuildingIndustry datBuildingIndustry, int currentCompanyId)
        {
            var strSqlMainUpdate = @"update FxtData_Industry.dbo.Dat_Building_Industry with(rowlock) set projectid = @projectid,buildingname = @buildingname,othername = @othername,industrytype = @industrytype,purposecode = @purposecode,structurecode = @structurecode,buildingtypecode = @buildingtypecode,totalfloor = @totalfloor,totalhigh = @totalhigh,buildingarea = @buildingarea,enddate = @enddate,saledate = @saledate,rentsaletype = @rentsaletype,industryarea = @industryarea,industryfloor = @industryfloor,podiumbuildingnum = @podiumbuildingnum,basementnum = @basementnum,functional = @functional,lobbyarea = @lobbyarea,lobbyhigh = @lobbyhigh,lobbyfitment = @lobbyfitment,liftnum = @liftnum,liftfitment = @liftfitment,liftbrand = @liftbrand,toiletbrand = @toiletbrand,publicfitment = @publicfitment,wallfitment = @wallfitment,floorhigh = @floorhigh,x = @x,y = @y,savedatetime = @savedatetime,saveuser = @saveuser,remarks = @remarks,AveragePrice = @AveragePrice,PriceDetail = @PriceDetail
where buildingid = @buildingid and (FxtCompanyId = @FxtCompanyId or @fxtcompanyid = " + ConfigurationHelper.FxtCompanyId + ")";

            var strSqlSubUpdate = @"update FxtData_Industry.dbo.Dat_Building_Industry_Sub with(rowlock)  set projectid = @projectid,buildingname = @buildingname,othername = @othername,industrytype = @industrytype,purposecode = @purposecode,structurecode = @structurecode,buildingtypecode = @buildingtypecode,totalfloor = @totalfloor,totalhigh = @totalhigh,buildingarea = @buildingarea,enddate = @enddate,saledate = @saledate,rentsaletype = @rentsaletype,industryarea = @industryarea,industryfloor = @industryfloor,podiumbuildingnum = @podiumbuildingnum,basementnum = @basementnum,functional = @functional,lobbyarea = @lobbyarea,lobbyhigh = @lobbyhigh,lobbyfitment = @lobbyfitment,liftnum = @liftnum,liftfitment = @liftfitment,liftbrand = @liftbrand,toiletbrand = @toiletbrand,publicfitment = @publicfitment,wallfitment = @wallfitment,floorhigh = @floorhigh,x = @x,y = @y,savedatetime = @savedatetime,saveuser = @saveuser,remarks = @remarks,AveragePrice = @AveragePrice,PriceDetail = @PriceDetail
where buildingid = @buildingid and FxtCompanyId = @FxtCompanyId";

            var strSqlSubAdd = @"insert into FxtData_Industry.dbo.Dat_Building_Industry_Sub (buildingid,projectid,cityid,buildingname,othername,industrytype,purposecode,structurecode,buildingtypecode,totalfloor,totalhigh,buildingarea,enddate,saledate,rentsaletype,industryarea,industryfloor,podiumbuildingnum,basementnum,functional,lobbyarea,lobbyhigh,lobbyfitment,liftnum,liftfitment,liftbrand,toiletbrand,publicfitment,wallfitment,floorhigh,fxtcompanyid,x,y,creator,createtime,savedatetime,saveuser,remarks,AveragePrice,PriceDetail) 
values(@buildingid,@projectid,@cityid,@buildingname,@othername,@industrytype,@purposecode,@structurecode,@buildingtypecode,@totalfloor,@totalhigh,@buildingarea,@enddate,@saledate,@rentsaletype,@industryarea,@industryfloor,@podiumbuildingnum,@basementnum,@functional,@lobbyarea,@lobbyhigh,@lobbyfitment,@liftnum,@liftfitment,@liftbrand,@toiletbrand,@publicfitment,@wallfitment,@floorhigh,@fxtcompanyid,@x,@y,@creator,@createtime,@savedatetime,@saveuser,@remarks,@AveragePrice,@PriceDetail)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                datBuildingIndustry.FxtCompanyId = currentCompanyId;
                var mainreturn = conn.Execute(strSqlMainUpdate, datBuildingIndustry);

                if (mainreturn == 0)
                {
                    if (conn.Execute(strSqlSubUpdate, datBuildingIndustry) == 0)
                    {
                        return conn.Execute(strSqlSubAdd, datBuildingIndustry);
                    }
                    return 1;
                }
                return 1;
            }
        }

        public int DeleteIndustryBuilding(DatBuildingIndustry datBuildingIndustry, int currentCompanyId)
        {
            var strSqlMainDelete = @"Update FxtData_Industry.dbo.Dat_Building_Industry  with(rowlock) set valid = 0,savedatetime = @savedatetime,saveuser = @saveuser where buildingId = @buildingId and (FxtCompanyId = @FxtCompanyId or @fxtcompanyid = " + ConfigurationHelper.FxtCompanyId + ")";
            var strSqlSubDelete = @"Update FxtData_Industry.dbo.Dat_Building_Industry_Sub  with(rowlock) set valid = 0,savedatetime = @savedatetime,saveuser = @saveuser where buildingId = @buildingId and FxtCompanyId = @FxtCompanyId";
            var strSqlSubAdd = @"insert into FxtData_Industry.dbo.Dat_Building_Industry_Sub (buildingid,projectid,cityid,buildingname,othername,industrytype,purposecode,structurecode,buildingtypecode,totalfloor,totalhigh,buildingarea,enddate,saledate,rentsaletype,industryarea,industryfloor,podiumbuildingnum,basementnum,functional,lobbyarea,lobbyhigh,lobbyfitment,liftnum,liftfitment,liftbrand,toiletbrand,publicfitment,wallfitment,floorhigh,fxtcompanyid,x,y,creator,createtime,savedatetime,saveuser,remarks,AveragePrice,PriceDetail,Valid) 
values(@buildingid,@projectid,@cityid,@buildingname,@othername,@industrytype,@purposecode,@structurecode,@buildingtypecode,@totalfloor,@totalhigh,@buildingarea,@enddate,@saledate,@rentsaletype,@industryarea,@industryfloor,@podiumbuildingnum,@basementnum,@functional,@lobbyarea,@lobbyhigh,@lobbyfitment,@liftnum,@liftfitment,@liftbrand,@toiletbrand,@publicfitment,@wallfitment,@floorhigh,@fxtcompanyid,@x,@y,@creator,@createtime,@savedatetime,@saveuser,@remarks,@AveragePrice,@PriceDetail,@Valid)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                datBuildingIndustry.FxtCompanyId = currentCompanyId;
                var mainreturn = conn.Execute(strSqlMainDelete, datBuildingIndustry);

                if (mainreturn == 0)
                {
                    if (conn.Execute(strSqlSubDelete, datBuildingIndustry) == 0)
                    {
                        datBuildingIndustry.Valid = 0;
                        return conn.Execute(strSqlSubAdd, datBuildingIndustry);
                    }
                    return 1;
                }
                return 1;
            }
        }

        public int CopyBuilding(int cityId, int companyId, string buildingName, string destBuildingName, int buildingId, int projectId)
        {
            var access = Access(cityId, companyId);
            var companyIds = string.IsNullOrEmpty(access.Item4) ? companyId.ToString() : access.Item4;

            var strSql = @"
SELECT b.BuildingId
FROM FxtData_Industry.dbo.Dat_Building_Industry b WITH (NOLOCK)
WHERE NOT EXISTS (
		SELECT BuildingId
		FROM FxtData_Industry.dbo.Dat_Building_Industry_sub b1 WITH (NOLOCK)
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
FROM FxtData_Industry.dbo.Dat_Building_Industry_sub b WITH (NOLOCK)
WHERE b.valid = 1
	AND b.CityId = @CityId
	AND b.FxtCompanyId = @CompanyId
	AND b.ProjectId = @projectId
	AND b.BuildingName = @BuildingName";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                var destBuildingId = conn.Query<long>(strSql, new { cityId, companyId, projectId, BuildingName = destBuildingName, }).FirstOrDefault();

                var ret = 1;
                var industryHouse = new IndustryHouse();
                var houses = industryHouse.GetIndustryHouseList(buildingId, cityId, companyId, false);

                if (destBuildingId > 0)//目标楼栋已存在
                {
                    var destHouses = industryHouse.GetIndustryHouseList(Convert.ToInt32(destBuildingId), cityId, companyId, false);
                    if (destHouses.Any()) //目标楼栋下已存在房号
                    {
                        return -1;
                    }

                    foreach (var item in houses)
                    {
                        item.BuildingId = destBuildingId;
                        item.CityId = cityId;
                        item.FxtCompanyId = companyId;
                        ret += industryHouse.AddIndustryHouse(item);
                    }
                }
                else
                {
                    var building = GetIndustryBuilding(buildingId, companyId);
                    building.BuildingName = destBuildingName;
                    building.CityId = cityId;
                    building.FxtCompanyId = companyId;

                    var newBuildingId = AddIndustryBuilding(building);
                    if (newBuildingId <= 0) return -2; //楼栋复制失败
                    if (newBuildingId > 0)
                    {
                        foreach (var item in houses)
                        {
                            item.BuildingId = newBuildingId;
                            item.CityId = cityId;
                            item.FxtCompanyId = companyId;
                            ret += industryHouse.AddIndustryHouse(item);
                        }
                    }
                }
                return ret;
            }
        }

        public DataTable BuildingSelfDefineExport(DatBuildingIndustry datBuildingIndustry, List<string> buildingAttr, int CityId, int FxtCompanyId, bool self = true)
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
                if (!string.IsNullOrEmpty(datBuildingIndustry.ProjectName))
                {
                    where += " and ProjectName like @ProjectName";
                    paramet.Add(new SqlParameter("@ProjectName", datBuildingIndustry.ProjectName));
                }
                if (!string.IsNullOrEmpty(datBuildingIndustry.BuildingName))
                {
                    where += " and BuildingName like @BuildingName";
                    paramet.Add(new SqlParameter("@BuildingName", datBuildingIndustry.BuildingName));
                }
                if (!string.IsNullOrEmpty(datBuildingIndustry.OtherName))
                {
                    where += " and OtherName like @OtherName";
                    paramet.Add(new SqlParameter("@OtherName", datBuildingIndustry.OtherName));
                }
                if (datBuildingIndustry.IndustryType > 0)
                {
                    where += "  and IndustryType = @IndustryType";
                    paramet.Add(new SqlParameter("@IndustryType", datBuildingIndustry.IndustryType));
                }
                if (datBuildingIndustry.PurposeCode > 0)
                {
                    where += " and PurposeCode = @PurposeCode";
                    paramet.Add(new SqlParameter("@PurposeCode", datBuildingIndustry.PurposeCode));
                }
                if (datBuildingIndustry.StructureCode > 0)
                {
                    where += "  and StructureCode = @StructureCode";
                    paramet.Add(new SqlParameter("@StructureCode", datBuildingIndustry.StructureCode));
                }
                if (datBuildingIndustry.BuildingTypeCode > 0)
                {
                    where += " and BuildingTypeCode = @BuildingTypeCode";
                    paramet.Add(new SqlParameter("@BuildingTypeCode", datBuildingIndustry.BuildingTypeCode));
                }
                if (datBuildingIndustry.RentSaleType > 0)
                {
                    where += " and RentSaleType = @RentSaleType";
                    paramet.Add(new SqlParameter("@RentSaleType", datBuildingIndustry.RentSaleType));
                }

                //查询语句
                var strSql = @"
select 
	buildingTable.*
	,projectTable.AreaName
	,projectTable.ProjectName
	,c.CodeName as IndustryTypeName
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
	FROM FxtData_Industry.dbo.Dat_Building_Industry b WITH (NOLOCK)
	WHERE NOT EXISTS (
			SELECT BuildingId
			FROM FxtData_Industry.dbo.Dat_Building_Industry_sub b1 WITH (NOLOCK)
			WHERE b1.cityId = b.cityId
				AND b1.fxtCompanyId = @fxtCompanyId
				AND b1.BuildingId = b.BuildingId
				AND b1.ProjectId = b.ProjectId)
		AND b.valid = 1
		AND b.CityId = @CityId
		AND b.FxtCompanyId IN (" + companyIds + @")
	UNION
	SELECT b.*
	FROM FxtData_Industry.dbo.Dat_Building_Industry_sub b WITH (NOLOCK)
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
	FROM FxtData_Industry.dbo.Dat_Project_Industry p WITH (NOLOCK)
	LEFT JOIN FxtDataCenter.dbo.SYS_SubArea_Industry sb WITH (NOLOCK) ON p.SubAreaId = sb.SubAreaId
	LEFT JOIN FxtDataCenter.dbo.SYS_Area a WITH (NOLOCK) ON p.AreaId = a.AreaId
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
	FROM FxtData_Industry.dbo.Dat_Project_Industry_sub p WITH (NOLOCK)
	LEFT JOIN FxtDataCenter.dbo.SYS_SubArea_Industry sb WITH (NOLOCK) ON p.SubAreaId = sb.SubAreaId
	LEFT JOIN FxtDataCenter.dbo.SYS_Area a WITH (NOLOCK) ON p.AreaId = a.AreaId
	WHERE p.valid = 1
		AND p.CityId = @CityId
		AND p.FxtCompanyId = @fxtCompanyId
)projectTable on buildingTable.ProjectId = projectTable.ProjectId
left join FxtDataCenter.dbo.SYS_Code c on buildingTable.IndustryType = c.Code
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
                DBHelperSql.ConnectionString = ConfigurationHelper.FxtDataIndustry;
                DataTable dtable = DBHelperSql.ExecuteDataTable(sql, param);
                return dtable;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IQueryable<LNKBPhoto> GetIndustryBuildingPhotoes(LNKBPhoto lnkPPhoto, bool self = true)
        {
            var access = Access(lnkPPhoto.CityId, lnkPPhoto.FxtCompanyId);
            var companyIds = string.IsNullOrEmpty(access.Item4) || self
              ? lnkPPhoto.FxtCompanyId.ToString()
              : access.Item4;

            var strSql = @"
SELECT p.*,c.CodeName AS PhotoTypeCodeName
FROM FxtData_Industry.dbo.LNK_B_Photo p WITH (NOLOCK)
LEFT JOIN FxtDataCenter.dbo.SYS_Code c WITH (NOLOCK) ON p.PhotoTypeCode = c.Code
WHERE NOT EXISTS (
		SELECT id
		FROM FxtData_Industry.dbo.LNK_B_Photo_Sub p1 WITH (NOLOCK)
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
	,c.CodeName AS PhotoTypeCodeName
FROM FxtData_Industry.dbo.LNK_B_Photo_Sub p WITH (NOLOCK)
LEFT JOIN FxtDataCenter.dbo.SYS_Code c WITH (NOLOCK) ON p.PhotoTypeCode = c.Code
WHERE p.valid = 1
	AND p.CityId = @CityId
	AND p.FxtCompanyId = @fxtCompanyId
	AND p.buildingId = @buildingId
ORDER BY 1 DESC";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Query<LNKBPhoto>(strSql, lnkPPhoto).AsQueryable();
            }
        }

        public LNKBPhoto GetIndustryBuildingPhoto(int id, int fxtCompanyId)
        {
            var strSql = @"
SELECT p.*,c.CodeName AS PhotoTypeName
FROM FxtData_Industry.dbo.LNK_B_Photo p WITH (NOLOCK)
LEFT JOIN FxtDataCenter.dbo.SYS_Code c WITH (NOLOCK) ON p.PhotoTypeCode = c.Code
WHERE NOT EXISTS (
		SELECT id
		FROM FxtData_Industry.dbo.LNK_B_Photo_Sub p1 WITH (NOLOCK)
		WHERE p1.buildingId = p.buildingId
			AND p1.cityId = p.cityId
			AND p1.fxtCompanyId = @fxtCompanyId
		)
	AND p.valid = 1
	AND p.id = @id
UNION
SELECT p.*,c.CodeName AS PhotoTypeName
FROM FxtData_Industry.dbo.LNK_B_Photo_Sub p WITH (NOLOCK)
LEFT JOIN FxtDataCenter.dbo.SYS_Code c WITH (NOLOCK) ON p.PhotoTypeCode = c.Code
WHERE p.valid = 1
	AND p.id = @id
ORDER BY 1 DESC";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Query<LNKBPhoto>(strSql, new { id, fxtCompanyId }).FirstOrDefault();
            }
        }

        public int AddIndustryBuildingPhoto(LNKBPhoto lnkPPhoto)
        {
            var strSql = @"insert into FxtData_Industry.dbo.LNK_B_Photo  (buildingId,phototypecode,path,photodate,photoname,cityid,fxtcompanyid,saveuser,savedate) 
values(@buildingId,@phototypecode,@path,@photodate,@photoname,@cityid,@fxtcompanyid,@saveuser,@savedate)";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Execute(strSql, lnkPPhoto);
            }
        }

        public int UpdateIndustryBuildingPhoto(LNKBPhoto lnkPPhoto, int currentCompanyId)
        {
            var strSqlMainUpdate = @"update FxtData_Industry.dbo.LNK_B_Photo  set phototypecode = @phototypecode,path = @path,photoname = @photoname,saveuser = @saveuser,savedate = @savedate
where id = @id and (FxtCompanyId = @FxtCompanyId or @fxtcompanyid = " + ConfigurationHelper.FxtCompanyId + ")";
            var strSqlSubUpdate = @"update FxtData_Industry.dbo.LNK_B_Photo_Sub set phototypecode = @phototypecode,path = @path,photoname = @photoname,saveuser = @saveuser,savedate = @savedate
where id = @id and FxtCompanyId = @FxtCompanyId";
            var strSqlSubAdd = @"insert into FxtData_Industry.dbo.LNK_B_Photo_Sub (id,buildingId,phototypecode,path,photodate,photoname,cityid,fxtcompanyid,saveuser,savedate) 
values(@id,@buildingId,@phototypecode,@path,@photodate,@photoname,@cityid,@fxtcompanyid,@saveuser,@savedate)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
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

        public int DeleteIndustryBuildingPhoto(LNKBPhoto lnkPPhoto, int currentCompanyId)
        {
            var strSqlMainDelete = @"Update FxtData_Industry.dbo.LNK_B_Photo  with(rowlock) set valid = 0,saveuser = @saveuser,savedate = @savedate where id = @id and (FxtCompanyId = @FxtCompanyId or @fxtcompanyid = " + ConfigurationHelper.FxtCompanyId + ")";
            var strSqlSubDelete = @"Update FxtData_Industry.dbo.LNK_B_Photo_Sub  with(rowlock) set valid = 0,saveuser = @saveuser,savedate = @savedate where id = @id and FxtCompanyId = @FxtCompanyId";
            var strSqlSubAdd = @"insert into FxtData_Industry.dbo.LNK_B_Photo_Sub  (id,buildingId,phototypecode,path,photodate,photoname,cityid,fxtcompanyid,saveuser,savedate) 
values(@id,@buildingId,@phototypecode,@path,@photodate,@photoname,@cityid,@fxtcompanyid,@saveuser,@savedate)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
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

        public int GetHouseCounts(int buildingId, int cityId, int fxtCompanyId, bool self)
        {
            var access = Access(cityId, fxtCompanyId);
            var companyIds = string.IsNullOrEmpty(access.Item4) || self
                ? fxtCompanyId.ToString()
                : access.Item4;

            string str = @"
SELECT COUNT(*) FROM (
	SELECT * FROM FxtData_Industry.dbo.Dat_House_Industry h WITH (NOLOCK)
	WHERE NOT EXISTS (
			SELECT HouseId
			FROM FxtData_Industry.dbo.Dat_House_Industry_sub hs WITH (NOLOCK)
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
	SELECT * FROM FxtData_Industry.dbo.Dat_House_Industry_sub h WITH (NOLOCK)
	WHERE h.Valid = 1
		AND h.CityId = @cityId
		AND h.FxtCompanyId = @fxtcompanyId
		AND h.BuildingId = @buildingId
)T";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Query<int>(str, new { buildingId, cityId, fxtCompanyId }).FirstOrDefault();
            }
        }

        #region 公共
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
        #endregion
    }
}
