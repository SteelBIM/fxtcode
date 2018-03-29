select
c.CaseID
,a.AreaName
,p.ProjectName
,c1.CodeName as PurposeCodeName
,c2.CodeName as BuildingTypeCodeName
,c.BuildingName
,c.HouseNo
,c.BuildingArea
,c.UnitPrice
,c.TotalPrice
,c3.CodeName as FrontCodeName
,c.ZhuangXiu
--,c4.CodeName as StructureCodeName
,c.TotalFloor
,c.FloorNumber
,c5.CodeName as HouseTypeCodeName
,c.BuildingDate
,c.SourceName
,c.SourceLink
,c6.CodeName as CaseTypeCodeName
,c.CaseDate
,p.StartDate
,p.UsableYear
,p.CubageRate
,p.GreenRate
,c.SubHouse
,c.PeiTao
,p.Wrinkle
,p.Aversion
,(select codename from FxtDataCenter.dbo.SYS_Code with(nolock) where code = b.StructureCode) as structurecodename
,(
	case b.iselevator 
	when 1 then '是'
	when 0 then '否'
	else ''
	end
)as iselevatorname,
p.address
from @casetable c with(nolock)
inner join (
	select CityID,AreaID,ProjectId,ProjectName,FxtCompanyId,StartDate,UsableYear,CubageRate,GreenRate,Wrinkle,Aversion,address
	from @projecttable p with(nolock)
	where not exists(
		select ProjectId from @projecttable_sub ps with(nolock)
		where ps.ProjectId = p.ProjectId
		and ps.CityID = @cityid
		and ps.Fxt_CompanyId = @fxtcompanyid
	)
	and p.Valid = 1
	and p.CityID = @cityid
	and p.FxtCompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT ShowCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid and TypeCode = @typecode), ','))
	union
	select CityID,AreaID,ProjectId,ProjectName,Fxt_CompanyId,StartDate,UsableYear,CubageRate,GreenRate,Wrinkle,Aversion,address
	from @projecttable_sub p with(nolock)
	where p.Valid = 1
	and p.CityID = @cityid
	and p.Fxt_CompanyId = @fxtcompanyid	
)p on c.ProjectId = p.ProjectId
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
) b on b.buildingid = c.buildingid
left join FxtDataCenter.dbo.SYS_Code c1 with(nolock) on c.PurposeCode = c1.Code
left join FxtDataCenter.dbo.SYS_Code c2 with(nolock) on c.BuildingTypeCode = c2.Code
left join FxtDataCenter.dbo.SYS_Code c3 with(nolock) on c.FrontCode = c3.Code
left join FxtDataCenter.dbo.SYS_Code c4 with(nolock) on c.StructureCode = c4.Code
left join FxtDataCenter.dbo.SYS_Code c5 with(nolock) on c.HouseTypeCode = c5.Code
left join FxtDataCenter.dbo.SYS_Code c6 with(nolock) on c.CaseTypeCode = c6.Code
left join FxtDataCenter.dbo.SYS_Area a with(nolock) on p.AreaID = a.AreaId
where c.valid = 1
and c.CityID = @cityid
and c.FXTCompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT CaseCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid and TypeCode = @typecode), ','))