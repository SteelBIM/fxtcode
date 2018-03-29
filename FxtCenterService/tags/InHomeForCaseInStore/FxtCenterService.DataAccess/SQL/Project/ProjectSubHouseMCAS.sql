select 
(select ProvinceName from FxtDataCenter.dbo.SYS_Province with(nolock) where ProvinceId in (select top 1 ProvinceId from FxtDataCenter.dbo.SYS_City with(nolock) where CityId = @cityid)) as ProvinceName
,(select top 1 CityName from FxtDataCenter.dbo.SYS_City with(nolock) where CityId = @cityid) as CityName
,P.ProjectId
,P.ProjectName
,P.AreaID
,(select top 1 AreaName from FxtDataCenter.dbo.SYS_Area a with(nolock) where AreaID = P.AreaID) as AreaName
,P.Address
,B.BuildingName
,B.BuildDate
,(select top 1 CodeName from FxtDataCenter.dbo.SYS_Code with(nolock) where Code = B.StructureCode) as StructureCodeName
,H.HouseName
,(select top 1 CodeName from FxtDataCenter.dbo.SYS_Code with(nolock) where Code = H.PurposeCode) as PurposeCodeName
,(select top 1 CodeName from FxtDataCenter.dbo.SYS_Code with(nolock) where Code = H.HouseTypeCode) as HouseTypeCodeName
,(select top 1 CodeName from FxtDataCenter.dbo.SYS_Code with(nolock) where Code = H.SubHouseType) as SubHouseTypeName
,H.SubHouseArea
,PS.SubHouseUnitPrice
,H.SubHouseArea * Ps.SubHouseUnitPrice as SubHouseTotalPrice
from (
	select ProjectId,ProjectName,AreaId,Address
	from @projecttable p with(nolock)
	where not exists(
		select ProjectId from @projecttable_sub ps with(nolock)
		where ps.ProjectId = p.ProjectId
		and ps.CityID = @cityid
		and ps.Fxt_CompanyId = @fxtcompanyid
		--and ps.Fxt_CompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT ShowCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid AND TypeCode = @typecode), ',') where value <> 25)
		and ps.ProjectId = @projectid
			)
			and Valid = 1
		and CityID = @cityid
		and (
			',' + cast((
					SELECT showcompanyid
					FROM fxtdatacenter.dbo.privi_company_showdata WITH (NOLOCK)
					WHERE fxtcompanyid = @fxtcompanyid
						AND cityid = @cityid
						AND TypeCode = @typecode
					) AS VARCHAR) + ',' LIKE '%,' + cast(p.fxtcompanyid AS VARCHAR) + ',%'
				) 
			and ProjectId = @projectid
		union 
		select ProjectId,ProjectName,AreaId,Address
		from @projecttable_sub h with(nolock)
			where Valid = 1
		and CityID = @cityid
		and Fxt_CompanyId = @fxtcompanyid
		--and Fxt_CompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT ShowCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid AND TypeCode = @typecode), ',') where value <> 25)
		and ProjectId = @projectid
)P
inner join (
	select BuildingId,BuildingName,ProjectId,StructureCode,BuildDate
	from @buildingtable b with(nolock)
	where not exists(
		select BuildingId from @buildingtable_sub bs with(nolock)
		where bs.BuildingId = b.BuildingId
		and bs.CityID = @cityid
		and bs.Fxt_CompanyId = @fxtcompanyid
		--and bs.Fxt_CompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT ShowCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid AND TypeCode = @typecode), ',') where value <> 25)
		and bs.BuildingId = @buildingid
			)
			and Valid = 1
		and CityID = @cityid
		and (
			',' + cast((
					SELECT showcompanyid
					FROM fxtdatacenter.dbo.privi_company_showdata WITH (NOLOCK)
					WHERE fxtcompanyid = @fxtcompanyid
						AND cityid = @cityid
						AND TypeCode = @typecode
					) AS VARCHAR) + ',' LIKE '%,' + cast(b.fxtcompanyid AS VARCHAR) + ',%'
				) 
			and BuildingId = @buildingid
		union 
		select BuildingId,BuildingName,ProjectId,StructureCode,BuildDate
		from @buildingtable_sub h with(nolock)
			where Valid = 1
		and CityID = @cityid
		and Fxt_CompanyId = @fxtcompanyid
		--and Fxt_CompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT ShowCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid AND TypeCode = @typecode), ',') where value <> 25)
		and BuildingId = @buildingid
)B on P.ProjectId = B.ProjectId
inner join (
	select HouseId,BuildingId,HouseName,HouseTypeCode,PurposeCode,SubHouseType,SubHouseArea
	from @housetable h with(nolock)
	where not exists(
		select * from @housetable_sub hs with(nolock)
		    where hs.HouseId = h.HouseId
		and hs.CityID = @cityid
		and hs.FxtCompanyId = @fxtcompanyid
		--and hs.FxtCompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT ShowCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid AND TypeCode = @typecode), ',') where value <> 25)
		and hs.HouseId = @houseid
			)
			and Valid = 1
		and CityID = @cityid
		and (
			',' + cast((
					SELECT showcompanyid
					FROM fxtdatacenter.dbo.privi_company_showdata WITH (NOLOCK)
					WHERE fxtcompanyid = @fxtcompanyid
						AND cityid = @cityid
						AND TypeCode = @typecode
					) AS VARCHAR) + ',' LIKE '%,' + cast(h.fxtcompanyid AS VARCHAR) + ',%'
				) 
			and HouseId = @houseid
		union 
		select HouseId,BuildingId,HouseName,HouseTypeCode,PurposeCode,SubHouseType,SubHouseArea
		from @housetable_sub h with(nolock)
			where Valid = 1
		and CityID = @cityid
		and FxtCompanyId = @fxtcompanyid
		--and FxtCompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT ShowCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid AND TypeCode = @typecode), ',') where value <> 25)
		and HouseId = @houseid
)H on B.BuildingId = H.BuildingId
left join (
	select * from @subHousePrice with(nolock)
	where CityId = @cityid
	and FxtCompanyId = @fxtcompanyid
	and ProjectId = @projectid
)PS
on H.SubHouseType = PS.SubHouseType