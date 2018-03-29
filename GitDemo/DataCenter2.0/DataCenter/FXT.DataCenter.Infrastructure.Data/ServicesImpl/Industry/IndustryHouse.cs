using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using System.Data.SqlClient;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Common.DBHelper;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class IndustryHouse : IIndustryHouse
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

        public DataTable GetIndustryHouses(int projectId, int buildingId, int cityId, int fxtCompanyId, bool self)
        {
            var access = Access(cityId, fxtCompanyId);
            var companyIds = string.IsNullOrEmpty(access.Item4) || self
                ? fxtCompanyId.ToString()
                : access.Item4;

            var strSql = @"
SELECT houseTable.*
	,c.CodeName AS PurposeName
	,c1.CodeName AS SJPurposeName
	,c2.CodeName AS FrontName
	,c3.CodeName AS SightName
	,projectTable.AveragePrice AS PAvePrice
	,buildingTable.AveragePrice AS BAvePrice
	,buildingTable.PriceDetail
FROM (
	SELECT h.[HouseId]
		,h.[BuildingId]
		,h.[ProjectId]
		,h.[FloorNo]
		,h.[FloorNum]
		,ISNULL(h.[UnitNo],'') as UnitNo
		,h.[HouseNo]
		,h.[HouseName]
		,h.[PurposeCode]
		,h.[SJPurposeCode]
		,h.[BuildingArea]
		,h.[InnerBuildingArea]
		,h.[FrontCode]
		,h.[SightCode]
		,h.[UnitPrice]
		,h.[Weight]
		,(
			CASE h.IsEValue
				WHEN '1'
					THEN '是'
				WHEN '0'
					THEN '否'
				ELSE '否'
				END
			) IsEValue
		,h.[FxtCompanyId]
	FROM FxtData_Industry.dbo.Dat_House_Industry h WITH (NOLOCK)
	WHERE NOT EXISTS (
			SELECT houseId
			FROM FxtData_Industry.dbo.Dat_House_Industry_sub hs WITH (NOLOCK)
			WHERE hs.cityId = h.cityId
				AND hs.houseId = h.houseId
				AND hs.fxtcompanyId = " + fxtCompanyId + @"
			)
		AND h.valid = 1
		AND h.CityId = " + cityId + @"
		AND h.FxtCompanyId IN (" + companyIds + @")
		AND h.BuildingId = " + buildingId + @"
		AND h.ProjectId = " + projectId + @"
	UNION	
	SELECT h.[HouseId]
		,h.[BuildingId]
		,h.[ProjectId]
		,h.[FloorNo]
		,h.[FloorNum]
		,ISNULL(h.[UnitNo],'') as UnitNo
		,h.[HouseNo]
		,h.[HouseName]
		,h.[PurposeCode]
		,h.[SJPurposeCode]
		,h.[BuildingArea]
		,h.[InnerBuildingArea]
		,h.[FrontCode]
		,h.[SightCode]
		,h.[UnitPrice]
		,h.[Weight]
		,(
			CASE h.IsEValue
				WHEN '1'
					THEN '是'
				WHEN '0'
					THEN '否'
				ELSE '否'
				END
			) IsEValue
		,h.[FxtCompanyId]
	FROM FxtData_Industry.dbo.Dat_House_Industry_sub h WITH (NOLOCK)
	WHERE h.valid = 1
		AND h.CityId = " + cityId + @"
		AND h.FxtCompanyId = " + fxtCompanyId + @"
		AND h.BuildingId = " + buildingId + @"
		AND h.ProjectId = " + projectId + @"
	) houseTable
INNER JOIN (
	SELECT p.ProjectId
		,p.AveragePrice
	FROM FxtData_Industry.dbo.Dat_Project_Industry p WITH (NOLOCK)
	WHERE NOT EXISTS (
			SELECT ProjectId
			FROM FxtData_Industry.dbo.Dat_Project_Industry_sub ps WITH (NOLOCK)
			WHERE ps.ProjectId = p.ProjectId
				AND ps.CityId = p.CityId
				AND ps.FxtCompanyId = " + fxtCompanyId + @"
			)
		AND p.Valid = 1
		AND p.CityId = " + cityId + @"
		AND p.FxtCompanyId IN (" + companyIds + @")
		AND p.ProjectId = " + projectId + @"	
	UNION	
	SELECT p.ProjectId
		,p.AveragePrice
	FROM FxtData_Industry.dbo.Dat_Project_Industry_sub p WITH (NOLOCK)
	WHERE p.Valid = 1
		AND p.CityId = " + cityId + @"
		AND p.FxtCompanyId = " + fxtCompanyId + @"
		AND p.ProjectId = " + projectId + @"
	) projectTable ON houseTable.ProjectId = projectTable.ProjectId
INNER JOIN (
	SELECT b.BuildingId
		,b.AveragePrice
		,b.PriceDetail
	FROM FxtData_Industry.dbo.Dat_Building_Industry b WITH (NOLOCK)
	WHERE NOT EXISTS (
			SELECT BuildingId
			FROM FxtData_Industry.dbo.Dat_Building_Industry_sub bs WITH (NOLOCK)
			WHERE bs.BuildingId = b.BuildingId
				AND bs.CityId = b.CityId
				AND bs.FxtCompanyId = " + fxtCompanyId + @"
			)
		AND b.Valid = 1
		AND b.CityId = " + cityId + @"
		AND b.FxtCompanyId IN (" + companyIds + @")
		AND b.BuildingId = " + buildingId + @"	
	UNION	
	SELECT b.BuildingId
		,b.AveragePrice
		,b.PriceDetail
	FROM FxtData_Industry.dbo.Dat_Building_Industry_sub b WITH (NOLOCK)
	WHERE b.Valid = 1
		AND b.CityId = " + cityId + @"
		AND b.FxtCompanyId = " + fxtCompanyId + @"
		AND b.BuildingId = " + buildingId + @"
	) buildingTable ON houseTable.BuildingId = buildingTable.BuildingId
LEFT JOIN FxtDataCenter.dbo.SYS_Code c WITH (NOLOCK) ON houseTable.PurposeCode = c.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c1 WITH (NOLOCK) ON houseTable.SJPurposeCode = c1.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c2 WITH (NOLOCK) ON houseTable.FrontCode = c2.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code c3 WITH (NOLOCK) ON houseTable.SightCode = c3.Code
ORDER BY FloorNo ,UnitNo ,HouseNo";

            DBHelperSql.ConnectionString = ConfigurationHelper.FxtDataIndustry;
            return DBHelperSql.ExecuteDataTable(strSql);
        }

        public IQueryable<DatHouseIndustry> GetIndustryHouseList(int buildingId, int cityId, int fxtCompanyId, bool self)
        {
            var access = Access(cityId, fxtCompanyId);
            var companyIds = string.IsNullOrEmpty(access.Item4) || self
                ? fxtCompanyId.ToString()
                : access.Item4;

            var strSql = @"
SELECT *
FROM FxtData_Industry.dbo.Dat_House_Industry h WITH (NOLOCK)
WHERE NOT EXISTS (
		SELECT houseId
		FROM FxtData_Industry.dbo.Dat_House_Industry_sub hs WITH (NOLOCK)
		WHERE hs.cityId = h.cityId
			AND hs.houseId = h.houseId
			AND hs.fxtcompanyId = @fxtCompanyId
		)
	AND h.valid = 1
	AND h.CityId = @cityId
	AND h.FxtCompanyId IN (" + companyIds + @")
	AND h.buildingId = @buildingId
UNION
SELECT *
FROM FxtData_Industry.dbo.Dat_House_Industry_sub h WITH (NOLOCK)
WHERE h.valid = 1
	AND h.CityId = @cityId
	AND h.BuildingId = @buildingId
	AND h.FxtCompanyId = @fxtCompanyId
ORDER BY FloorNo,UnitNo";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Query<DatHouseIndustry>(strSql, new { buildingId, cityId, fxtCompanyId }).AsQueryable();
            }
        }

        public IQueryable<DatHouseIndustry> GetIndustryHouses_UnitNo(int buildingId, int cityId, int fxtCompanyId, bool self)
        {
            var access = Access(cityId, fxtCompanyId);
            var companyIds = string.IsNullOrEmpty(access.Item4) || self
                ? fxtCompanyId.ToString()
                : access.Item4;

            var strSql = @"
SELECT ISNULL(h.UnitNo,'') as UnitNo,h.HouseNo
FROM FxtData_Industry.dbo.Dat_House_Industry h WITH (NOLOCK)
WHERE NOT EXISTS (
		SELECT HouseId
		FROM FxtData_Industry.dbo.Dat_House_Industry_sub hs WITH (NOLOCK)
		WHERE hs.cityId = h.cityId
			AND hs.houseId = h.houseId
			AND hs.fxtcompanyId = @fxtCompanyId
		)
	AND h.valid = 1
	AND h.CityId = @CityId
	AND h.BuildingId = @BuildingId
	AND h.FxtCompanyId IN (" + companyIds + @")
UNION
SELECT ISNULL(h.UnitNo,'') as UnitNo,h.HouseNo
FROM FxtData_Industry.dbo.Dat_House_Industry_sub h WITH (NOLOCK)
WHERE h.valid = 1
	AND h.CityId = @CityId
	AND h.BuildingId = @buildingId
	AND h.FxtCompanyId = @fxtCompanyId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Query<DatHouseIndustry>(strSql, new { buildingId, cityId, fxtCompanyId }).AsQueryable();
            }
        }

        public IQueryable<int> GetIndustryHouses_FloorNo(int buildingId, int cityId, int fxtCompanyId, bool self)
        {
            var access = Access(cityId, fxtCompanyId);
            var companyIds = string.IsNullOrEmpty(access.Item4) || self
                ? fxtCompanyId.ToString()
                : access.Item4;

            var strSql = @"
SELECT h.FloorNo
FROM FxtData_Industry.dbo.Dat_House_Industry h WITH (NOLOCK)
WHERE NOT EXISTS (
		SELECT HouseId
		FROM FxtData_Industry.dbo.Dat_House_Industry_sub hs WITH (NOLOCK)
		WHERE hs.cityId = h.cityId
			AND hs.houseId = h.houseId
			AND hs.fxtcompanyId = @fxtCompanyId
		)
	AND h.valid = 1
	AND h.CityId = @CityId
	AND h.BuildingId = @BuildingId
	AND h.FxtCompanyId IN (" + companyIds + @")
UNION
SELECT h.FloorNo
FROM FxtData_Industry.dbo.Dat_House_Industry_sub h WITH (NOLOCK)
WHERE h.valid = 1
	AND h.CityId = @CityId
	AND h.BuildingId = @buildingId
	AND h.FxtCompanyId = @fxtCompanyId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Query<int>(strSql, new { buildingId, cityId, fxtCompanyId }).AsQueryable();
            }
        }

        public DatHouseIndustry GetIndustryHouse(long houseId, int cityId, int fxtCompanyId)
        {
            var access = Access(cityId, fxtCompanyId);
            var companyIds = string.IsNullOrEmpty(access.Item4)
                ? fxtCompanyId.ToString()
                : access.Item4;

            var strSql = @"
SELECT h.*
FROM FxtData_Industry.dbo.Dat_House_Industry h WITH (NOLOCK)
WHERE NOT EXISTS (
		SELECT HouseId
		FROM FxtData_Industry.dbo.Dat_House_Industry_sub hs WITH (NOLOCK)
		WHERE hs.cityId = h.cityId
			AND hs.houseId = h.houseId
			AND hs.fxtcompanyId = @fxtCompanyId
		)
	AND h.CityId = @CityId
	AND h.FxtCompanyId IN (" + companyIds + @")
	AND h.houseId = @houseId
UNION
SELECT h.*
FROM FxtData_Industry.dbo.Dat_House_Industry_sub h WITH (NOLOCK)
WHERE h.valid = 1
	AND h.CityId = @CityId
	AND h.FxtCompanyId = @fxtCompanyId
	AND h.houseId = @houseId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Query<DatHouseIndustry>(strSql, new { houseId, cityId, fxtCompanyId }).FirstOrDefault();
            }
        }

        public string GetProjectName(long buildingId, int fxtCompanyId)
        {
            var strSql1 = @"
SELECT b.projectId
FROM FxtData_Industry.dbo.Dat_Building_Industry b WITH (NOLOCK)
WHERE NOT EXISTS (
		SELECT BuildingId
		FROM FxtData_Industry.dbo.Dat_Building_Industry_sub b1 WITH (NOLOCK)
		WHERE b1.cityId = b.cityId
			AND b1.fxtCompanyId = @fxtcompanyId
			AND b1.BuildingId = b.BuildingId
		)
	AND b.valid = 1
	AND b.buildingId = @buildingId
UNION
SELECT b.projectId
FROM FxtData_Industry.dbo.Dat_Building_Industry_sub b WITH (NOLOCK)
WHERE b.valid = 1
	AND b.buildingId = @buildingId";

            var strSql2 = @"
SELECT p.ProjectName
FROM FxtData_Industry.dbo.Dat_Project_Industry p WITH (NOLOCK)
WHERE NOT EXISTS (
		SELECT ProjectId
		FROM FxtData_Industry.dbo.Dat_Project_Industry_sub ps WITH (NOLOCK)
		WHERE p.CityId = ps.CityId
			AND ps.FxtCompanyId = @fxtCompanyId
			AND p.ProjectId = ps.ProjectId
		)
	AND p.valid = 1
	AND ProjectId = @ProjectId
UNION
SELECT p.ProjectName
FROM FxtData_Industry.dbo.Dat_Project_Industry_sub p WITH (NOLOCK)
WHERE p.valid = 1
	AND ProjectId = @ProjectId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                var projectId = conn.Query<long>(strSql1, new { buildingId, fxtCompanyId }).FirstOrDefault();
                return conn.Query<string>(strSql2, new { projectId, fxtCompanyId }).FirstOrDefault();
            }
        }

        public string GetBuildingName(long buildingId, int fxtCompanyId)
        {
            var strSql1 = @"
SELECT b.buildingName
FROM FxtData_Industry.dbo.Dat_Building_Industry b WITH (NOLOCK)
WHERE NOT EXISTS (
		SELECT BuildingId
		FROM FxtData_Industry.dbo.Dat_Building_Industry_sub b1 WITH (NOLOCK)
		WHERE b1.cityId = b.cityId
			AND b1.fxtCompanyId = @fxtcompanyId
			AND b1.BuildingId = b.BuildingId
		)
	AND b.valid = 1
	AND b.buildingId = @buildingId
UNION
SELECT b.buildingName
FROM FxtData_Industry.dbo.Dat_Building_Industry_sub b WITH (NOLOCK)
WHERE b.valid = 1
	AND b.buildingId = @buildingId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Query<string>(strSql1, new { buildingId, fxtCompanyId }).FirstOrDefault();
            }
        }

        public int UpdateFloorNum(int floorNo, string floorNum, int cityId, int fxtCompanyId, string saveUser)
        {
            const string strSql = @"update FxtData_Industry.dbo.Dat_House_Industry set floornum=@floornum,savedatetime = getdate(),saveuser = @saveuser where floorNo=@floorNo and cityId = @cityId and fxtCompanyId = @fxtCompanyId";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Execute(strSql, new { floorNo, floorNum, cityId, fxtCompanyId, saveUser });
            }
        }

        public int UpdateIndustryHouse(DatHouseIndustry datHouseIndustry, int currentCompanyId)
        {
            var fxtId = ConfigurationHelper.FxtCompanyId;
            datHouseIndustry.FxtCompanyId = currentCompanyId;

            var strSqlMainUpdate = @"update FxtData_Industry.dbo.Dat_House_Industry set floornum=@floornum,unitno = @unitno,houseNo = @houseNO,housename = @housename,purposecode = @purposecode,sjpurposecode = @sjpurposecode,buildingarea = @buildingarea,innerbuildingarea = @innerbuildingarea,frontcode = @frontcode,sightcode = @sightcode,unitprice = @unitprice,weight = @weight,isevalue = @isevalue,savedatetime = @savedatetime,saveuser = @saveuser,remarks = @remarks
where houseid = @houseid  and (fxtCompanyId= @fxtCompanyId or @fxtCompanyId=" + fxtId + ")";

            var strSqlSubAdd = @"insert into FxtData_Industry.dbo.Dat_House_Industry_Sub(houseId,buildingid,projectid,cityid,floorno,floornum,unitno,houseNo,housename,purposecode,sjpurposecode,buildingarea,innerbuildingarea,frontcode,sightcode,unitprice,weight,isevalue,fxtcompanyid,creator,createtime,savedatetime,saveuser,remarks) 
values(@houseId,@buildingid,@projectid,@cityid,@floorno,@floornum,@unitno,@houseNo,@housename,@purposecode,@sjpurposecode,@buildingarea,@innerbuildingarea,@frontcode,@sightcode,@unitprice,@weight,@isevalue,@fxtcompanyid,@creator,@createtime,@savedatetime,@saveuser,@remarks)";

            var strSqlSubUpdate = @"update FxtData_Industry.dbo.Dat_House_Industry_Sub set floornum=@floornum, unitno = @unitno,houseNo = @houseNo,housename = @housename,purposecode = @purposecode,sjpurposecode = @sjpurposecode,buildingarea = @buildingarea,innerbuildingarea = @innerbuildingarea,frontcode = @frontcode,sightcode = @sightcode,unitprice = @unitprice,weight = @weight,isevalue = @isevalue,savedatetime = @savedatetime,saveuser = @saveuser,remarks = @remarks
where houseid = @houseid and fxtCompanyId= @fxtCompanyId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                var r = conn.Execute(strSqlMainUpdate, datHouseIndustry);
                if (r != 0) return r;

                var r1 = conn.Execute(strSqlSubUpdate, datHouseIndustry);
                return r1 == 0 ? conn.Execute(strSqlSubAdd, datHouseIndustry) : r1;
            }
        }

        public int AddIndustryHouse(DatHouseIndustry datHouseIndustry)
        {
            const string strSql = @"insert into FxtData_Industry.dbo.Dat_House_Industry(buildingid,projectid,cityid,floorno,floornum,unitno,houseNo,housename,purposecode,sjpurposecode,buildingarea,innerbuildingarea,frontcode,sightcode,unitprice,weight,isevalue,fxtcompanyid,creator,remarks) 
values(@buildingid,@projectid,@cityid,@floorno,@floornum,@unitno,@houseNo,@housename,@purposecode,@sjpurposecode,@buildingarea,@innerbuildingarea,@frontcode,@sightcode,@unitprice,@weight,@isevalue,@fxtcompanyid,@creator,@remarks)";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Execute(strSql, datHouseIndustry);
            }
        }

        public int DeleteIndustryHouse(DatHouseIndustry datHouseIndustry, int currentCompanyId)
        {
            var fxtId = ConfigurationHelper.FxtCompanyId;
            datHouseIndustry.FxtCompanyId = currentCompanyId;
            datHouseIndustry.Valid = 0;

            var strSqlMainDelete = @"Update FxtData_Industry.dbo.Dat_House_Industry with(rowlock) set valid = 0,savedatetime = @savedatetime,saveuser = @saveuser where houseId = @houseId and (fxtCompanyId= @fxtCompanyId or @fxtCompanyId=" + fxtId + ")";
            var strSqlSubDelete = @"Update FxtData_Industry.dbo.Dat_House_Industry_Sub with(rowlock) set valid = 0,savedatetime = @savedatetime,saveuser = @saveuser where houseId = @houseId and fxtCompanyId= @fxtCompanyId";
            var strSqlSubAdd = @"insert into FxtData_Industry.dbo.Dat_House_Industry_Sub(houseId,buildingid,projectid,cityid,floorno,floornum,unitno,houseNo,housename,purposecode,sjpurposecode,buildingarea,innerbuildingarea,frontcode,sightcode,unitprice,weight,isevalue,fxtcompanyid,creator,createtime,savedatetime,saveuser,valid,remarks) 
values(@houseId,@buildingid,@projectid,@cityid,@floorno,@floornum,@unitno,@houseNo,@housename,@purposecode,@sjpurposecode,@buildingarea,@innerbuildingarea,@frontcode,@sightcode,@unitprice,@weight,@isevalue,@fxtcompanyid,@creator,@createtime,@savedatetime,@saveuser,@valid,@remarks)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                var r = conn.Execute(strSqlMainDelete, datHouseIndustry);
                if (r != 0) return r;

                var r1 = conn.Execute(strSqlSubDelete, datHouseIndustry);
                return r1 == 0 ? conn.Execute(strSqlSubAdd, datHouseIndustry) : r1;

            }
        }

        public DataTable HouseSelfDefineExport(DatHouseIndustry datHouse, List<string> houseAttr, int CityId, int FxtCompanyId, bool self = true)
        {
            try
            {
                List<SqlParameter> paramet = new List<SqlParameter>();
                var access = Access(CityId, FxtCompanyId);
                var companyIds = string.IsNullOrEmpty(access.Item4)
                    ? FxtCompanyId.ToString()
                    : access.Item4;

                string where = string.Empty;
                if (!string.IsNullOrEmpty(datHouse.ProjectName))
                {
                    where += " and ProjectName like @ProjectName";
                    paramet.Add(new SqlParameter("@ProjectName", datHouse.ProjectName));
                }
                if (!string.IsNullOrEmpty(datHouse.BuildingName))
                {
                    where += "  and BuildingName like @BuildingName";
                    paramet.Add(new SqlParameter("@BuildingName", datHouse.BuildingName));
                }

                string strSql = @"
select 
	houseTable.*
	,case when housetable.IsEValue = 1 then '是' else '否' end as IsEValueName
	,buildingTable.BuildingName
	,projectTable.ProjectName
	,projectTable.AreaName
	,c.CodeName as PurposeCodeName
	,c1.CodeName as SJPurposeCodeName
	,c2.CodeName as FrontCodeName
	,c3.CodeName as SightCodeName
from (
	select * from FxtData_Industry.dbo.Dat_House_Industry h with(nolock)
	where not exists(
		select HouseId from FxtData_Industry.dbo.Dat_House_Industry_sub hs with(nolock)
		where h.CityId = hs.CityId
		and h.FxtCompanyId = @fxtcompanyid
		and h.HouseId = hs.HouseId
	)
	and h.Valid = 1
	and h.CityId = @cityid
	and h.FxtCompanyId in (" + companyIds + @")
	union
	select * from FxtData_Industry.dbo.Dat_House_Industry_sub h with(nolock)
	where h.Valid = 1
	and h.CityId = @cityid
	and h.FxtCompanyId = @fxtcompanyid
)houseTable
inner join
(	
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
)buildingTable on buildingTable.BuildingId = houseTable.BuildingId and buildingTable.ProjectId = houseTable.ProjectId
inner join 
(
	SELECT p.*
		,a.AreaName
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
	SELECT p.*
		,a.AreaName
	FROM FxtData_Industry.dbo.Dat_Project_Industry_sub p WITH (NOLOCK)
	LEFT JOIN FxtDataCenter.dbo.SYS_SubArea_Industry sb WITH (NOLOCK) ON p.SubAreaId = sb.SubAreaId
	LEFT JOIN FxtDataCenter.dbo.SYS_Area a WITH (NOLOCK) ON p.AreaId = a.AreaId
	WHERE p.valid = 1
		AND p.CityId = @CityId
		AND p.FxtCompanyId = @fxtCompanyId
)projectTable on projectTable.ProjectId = buildingTable.ProjectId
left join FxtDataCenter.dbo.SYS_Code c with(nolock) on houseTable.PurposeCode = c.Code
left join FxtDataCenter.dbo.SYS_Code c1 with(nolock) on houseTable.SJPurposeCode = c1.Code
left join FxtDataCenter.dbo.SYS_Code c2 with(nolock) on houseTable.FrontCode = c2.Code
left join FxtDataCenter.dbo.SYS_Code c3 with(nolock) on houseTable.SightCode = c3.Code
where 1 = 1" + where;

                paramet.Add(new SqlParameter("@CityId", CityId));
                paramet.Add(new SqlParameter("@FxtCompanyId", FxtCompanyId));
                string paramList = string.Empty;
                for (int i = 0; i < houseAttr.Count; i++)
                {
                    paramList += houseAttr[i].Replace("&", " as ") + ",";
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

        public long GetIndustryHouseId(long buildingId, string houseName, int cityId, int fxtCompanyId)
        {
            var access = Access(cityId, fxtCompanyId);
            var companyIds = string.IsNullOrEmpty(access.Item4)
                ? fxtCompanyId.ToString()
                : access.Item4;

            var strSql = @"
SELECT h.houseId
FROM FxtData_Industry.dbo.Dat_House_Industry h WITH (NOLOCK)
WHERE NOT EXISTS (
		SELECT HouseId
		FROM FxtData_Industry.dbo.Dat_House_Industry_sub hs WITH (NOLOCK)
		WHERE hs.cityId = h.cityId
			AND hs.houseId = h.houseId
			AND hs.fxtcompanyId = @fxtCompanyId
		)
	AND h.valid = 1
	AND h.CityId = @CityId
	AND h.houseName = @houseName
	AND h.BuildingId = @BuildingId
	AND h.FxtCompanyId IN (" + companyIds + @")
UNION
SELECT h.houseId
FROM FxtData_Industry.dbo.Dat_House_Industry_sub h WITH (NOLOCK)
WHERE h.valid = 1
	AND h.CityId = @CityId
	AND h.houseName = @houseName
	AND h.BuildingId = @buildingId
	AND h.FxtCompanyId = @fxtCompanyId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Query<long>(strSql, new { buildingId, houseName, cityId, fxtCompanyId }).FirstOrDefault();
            }
        }

    }
}
