select B.BuildDate,H.*,c.CodeName as purposecodename,c1.CodeName as housetypecodename from (
	select BuildingId,BuildDate from @table_building b with(nolock)
	where not exists(
		select * from @table_building_sub bs with(nolock)
		where bs.BuildingId = b.BuildingId
		and bs.CityID = @cityid
		and bs.Fxt_CompanyId = @fxtcompanyid
		and bs.BuildingId = @buildingid
	)
	and Valid = 1
	and CityID = @cityid
	and FxtCompanyId in (
			SELECT value
			FROM dbo.splittotable((
						SELECT showcompanyid
						FROM fxtdatacenter.dbo.privi_company_showdata
						WHERE cityid = @cityid
							AND fxtcompanyid = @fxtcompanyid
							AND typecode = @typecode
						), ','))
	and BuildingId = @buildingid
	union
	select BuildingId,BuildDate from @table_building_sub b with(nolock)
	where Valid = 1
	and CityID = @cityid
	and Fxt_CompanyId = @fxtcompanyid
	and BuildingId = @buildingid
)B
inner join (
	SELECT  
		BuildingId
		,houseid
		,housename
		,buildarea
		,isevalue
		,weight
		,PurposeCode
		,HouseTypeCode
		,SubHouseType
		,SubHouseArea
	FROM @table_house h WITH (NOLOCK)
	WHERE buildingid = @buildingid
		AND valid = 1
		AND cityid = @cityid
		@floorno
		AND housename <> ''
		@key
		AND h.houseid NOT IN (
			SELECT houseid
			FROM @table_house_sub hs WITH (NOLOCK)
			WHERE h.houseid = hs.houseid
				AND hs.fxtcompanyid = @fxtcompanyid
				AND hs.cityid = h.cityid
			)
		AND h.fxtcompanyid IN (
			SELECT value
			FROM dbo.splittotable((
						SELECT showcompanyid
						FROM fxtdatacenter.dbo.privi_company_showdata
						WHERE cityid = @cityid
							AND fxtcompanyid = @fxtcompanyid
							AND typecode = @typecode
						), ','))
	UNION
	SELECT 
		BuildingId
		,houseid
		,housename
		,buildarea
		,isevalue
		,weight
		,PurposeCode
		,HouseTypeCode
		,SubHouseType
		,SubHouseArea
	FROM @table_house_sub h WITH (NOLOCK)
	WHERE buildingid = @buildingid
		AND valid = 1
		AND cityid = @cityid
		@floorno
		AND housename <> ''
		AND h.fxtcompanyid = @fxtcompanyid
		@key
)H on B.BuildingId = H.BuildingId
left join FxtDataCenter.dbo.SYS_Code c on H.PurposeCode = c.Code
left join FxtDataCenter.dbo.SYS_Code c1 on H.HouseTypeCode = c1.Code