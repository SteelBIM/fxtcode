select 
	ci.Id
	,a.AreaName
	,sa.SubAreaName
	,ci.ProjectName
	,ci.BuildingName
	,ci.HouseName
	,c.CodeName AS PurposeCodeName
	,ci.BuildingArea
	,ci.UnitPrice
	,ci.TotalPrice
	,c1.CodeName as CaseTypeCodeName
	,ci.CaseDate
	,ci.FloorNo
	,ci.TotalFloor
	,ci.RentRate
	,ci.SourceName
	,ci.SourceLink
	,ci.Address
	,c5.CodeName as BuildingTypeName
	,p.UsableYear
	,p.StartDate
	,p.BuildingArea as totalBuildingArea
	,p.CubageRate
	,p.GreenRate
	,p.BuildingNum
	,p.EndDate
	,p.OfficeArea
	,p.BizArea
	,p.IndustryArea
	,c2.CodeName as TrafficTypeName
	,p.TrafficDetails
	,c3.CodeName as ParkingLevelName
	,c4.CodeName as ParkingTypeName
	,p.Details
	,p.East
	,p.west
	,p.south
	,p.north
	,(select codename from FxtDataCenter.dbo.SYS_Code with(nolock) where code = b.StructureCode) as structurecodename
	,(
		case b.iselevator 
		when 1 then '是'
		when 0 then '否'
		else ''
		end
	)as iselevatorname
from FxtData_Industry.dbo.Dat_Case_Industry ci with(nolock)
left join (
	select * from FxtData_Industry.dbo.Dat_Project_Industry p with(nolock)
	where not exists(
		select ProjectId from FxtData_Industry.dbo.Dat_Project_Industry_sub ps with(nolock)
		where ps.ProjectId = p.ProjectId
		and ps.CityId = @cityid
		and ps.FxtCompanyId = @fxtcompanyid
	)
	and p.Valid = 1
	and p.CityId = @cityid
	and p.FxtCompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT IndustryCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId=@cityid AND FxtCompanyId=@fxtcompanyid and TypeCode = @typecode),','))
	union
	select * from FxtData_Industry.dbo.Dat_Project_Industry_sub p with(nolock)
	where p.Valid = 1
	and p.CityId = @cityid
	and p.FxtCompanyId = @fxtcompanyid
)p on ci.ProjectId = p.ProjectId
left join (
	select IsElevator,StructureCode,buildingid from @buildingtable b with(nolock)
	where CityID = @cityid
		and Valid = 1
		and BuildingId not in(
			select buildingid from @buildingtable_sub bs with(nolock)
			where CityID = @cityid
				and bs.buildingid = b.buildingid
				and Fxt_CompanyId = @fxtcompanyid
			)
	union
	select IsElevator,StructureCode,buildingid from @buildingtable_sub with(nolock)
	where CityID = @cityid
		and valid=1 
		and Fxt_CompanyId = @fxtcompanyid
) b on b.buildingid = ci.buildingid
left join FxtDataCenter.dbo.SYS_Area a with(nolock) on ci.AreaId = a.AreaId
left join FxtDataCenter.dbo.SYS_SubArea_Industry sa with(nolock) on ci.SubAreaId = sa.SubAreaId
left join FxtDataCenter.dbo.SYS_Code c with(nolock) on p.PurposeCode = c.Code
left join FxtDataCenter.dbo.SYS_Code c1 with(nolock) on ci.CaseTypeCode = c1.Code
left join FxtDataCenter.dbo.SYS_Code c2 with(nolock) on p.TrafficType = c2.Code
left join FxtDataCenter.dbo.SYS_Code c3 with(nolock) on p.ParkingLevel = c3.Code
left join FxtDataCenter.dbo.SYS_Code c4 with(nolock) on p.ParkingType = c4.Code
left join FxtDataCenter.dbo.SYS_Code c5 with(nolock) on p.BuildingType = c5.Code
where ci.Valid = 1
and ci.CityId = @cityid
and ci.FxtCompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT IndustryCaseCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId=@cityid AND FxtCompanyId=@fxtcompanyid and TypeCode = @typecode),','))