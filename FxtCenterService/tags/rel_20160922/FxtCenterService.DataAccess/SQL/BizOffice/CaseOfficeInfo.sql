SELECT
	co.Id
	,co.CityId
	,c.CityName
	,p.AreaName
	,p.SubAreaName
	,co.ProjectName
	,co.BuildingName
	,co.HouseName
	,code3.CodeName as PurposeCodeName
	,co.BuildingArea
	,co.UnitPrice
	,co.TotalPrice
	,code.CodeName as CaseTypeCodeName
	,co.CaseDate
	,code1.CodeName as FitmentName
	,code2.CodeName as OfficeTypeName
	,co.FloorNo
	,co.TotalFloor
	,co.RentRate
	,co.ManagerPrice
	,co.SourceName
	,co.SourceLink
	,code4.CodeName as BuildingTypeName
	,p.UsableYear
	,p.StartDate
	,p.BuildingArea as TotalBuildingArea
	,p.CubageRate
	,p.GreenRate
	,p.BuildingNum
	,p.EndDate
	,p.OfficeArea
	,p.BizArea
	,p.IndustryArea
	,code5.CodeName as TrafficTypeName
	,p.TrafficDetails
	,code6.CodeName as ParkingLevelName
	,code7.CodeName as ParkingTypeName
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
FROM [FxtData_Office].[dbo].[Dat_Case_Office] co with(nolock)
left join (
	select T.ProjectId,T.AreaId,a.AreaName,T.SubAreaId,sa.SubAreaName,T.PurposeCode,T.BuildingType,T.UsableYear,T.StartDate,T.BuildingArea,T.CubageRate,T.GreenRate,T.BuildingNum,T.EndDate,T.OfficeArea,T.BizArea,T.IndustryArea,T.TrafficType,T.TrafficDetails,T.ParkingLevel,T.ParkingType,T.Details,T.East,T.west,T.south,T.north from (
		select * from [FxtData_Office].[dbo].[Dat_Project_Office] p with(nolock)
		where not exists (
			select ProjectId from [FxtData_Office].[dbo].[Dat_Project_Office_sub] ps with(nolock)
			where ps.ProjectId = p.ProjectId
			and ps.CityId = @cityid
			and ps.FxtCompanyId = @fxtcompanyid
		)
		and p.Valid = 1
		and p.FxtCompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT OfficeCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId=@cityid AND FxtCompanyId=@fxtcompanyid and TypeCode = @typecode),','))
		and p.CityId = @cityid
		union
		select * from [FxtData_Office].[dbo].[Dat_Project_Office_sub] p with(nolock)
		where p.Valid = 1
		and p.FxtCompanyId = @fxtcompanyid
		and p.CityId = @cityid
	)T
	left join FxtDataCenter.dbo.SYS_Area a with(nolock) on T.AreaId = a.AreaId
	left join FxtDataCenter.dbo.SYS_SubArea_Office sa with(nolock) on T.SubAreaId = sa.SubAreaId
)p on co.ProjectId = p.ProjectId
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
) b on b.buildingid = co.buildingid
left join FxtDataCenter.dbo.SYS_City c with(nolock) on co.CityId = c.CityId
left join FxtDataCenter.dbo.SYS_Code code with(nolock) on co.CaseTypeCode = code.Code
left join FxtDataCenter.dbo.SYS_Code code1 with(nolock) on co.Fitment = code1.Code
left join FxtDataCenter.dbo.SYS_Code code2 with(nolock) on co.OfficeType = code2.Code
left join FxtDataCenter.dbo.SYS_Code code3 with(nolock) on p.PurposeCode = code3.Code
left join FxtDataCenter.dbo.SYS_Code code4 with(nolock) on p.BuildingType = code4.Code
left join FxtDataCenter.dbo.SYS_Code code5 with(nolock) on p.TrafficType = code5.Code
left join FxtDataCenter.dbo.SYS_Code code6 with(nolock) on p.ParkingLevel = code6.Code
left join FxtDataCenter.dbo.SYS_Code code7 with(nolock) on p.ParkingType = code7.Code
where 1 = 1
and co.Valid = 1
and co.FxtCompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT OfficeCaseCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId=@cityid AND FxtCompanyId=@fxtcompanyid and TypeCode = @typecode),','))
and co.CityId = @cityid