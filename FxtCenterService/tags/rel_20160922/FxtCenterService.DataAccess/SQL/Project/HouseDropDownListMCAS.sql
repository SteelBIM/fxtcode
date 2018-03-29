select * from (
SELECT HouseId,HouseName,UnitNo,BuildArea,IsEvalue,subhousetype,subhousearea,floorno,frontcode,sightcode,purposecode
FROM @table_house h WITH (NOLOCK)
WHERE BuildingId = @buildingid
	AND CityId = @cityid
	AND h.houseid in (
		select max(houseid) from @table_house
		where buildingid = @buildingid
		AND valid = 1
		AND cityid = @cityid
		@floornowhere
		AND housename <> ''
		AND h.fxtcompanyid IN (
		SELECT value
		FROM dbo.splittotable((
		SELECT showcompanyid
		FROM fxtdatacenter.dbo.privi_company_showdata
		WHERE cityid = @cityid
			AND fxtcompanyid = @fxtcompanyid
			AND typecode = @typecode
		), ','))
		group by HouseName,BuildingId,FloorNo,CityID,FxtCompanyId
	)
	AND h.HouseId NOT IN (
		SELECT HouseId
		FROM @table_house_sub hs WITH (NOLOCK)
		WHERE h.HouseId = hs.HouseId
			AND hs.FxtCompanyId = @fxtcompanyid
			--AND hs.FxtCompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT ShowCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid AND TypeCode = @typecode), ',') where value <> 25)
			AND hs.CityId = h.CityId
		)
	AND (
		',' + cast((
				SELECT showcompanyid
				FROM FxtDataCenter.dbo.Privi_Company_ShowData pc WITH (NOLOCK)
				WHERE pc.Fxtcompanyid = @fxtcompanyid
					AND pc.Cityid = @cityid
					AND pc.TypeCode = @typecode
				) AS VARCHAR) + ',' LIKE '%,' + cast(fxtcompanyid AS VARCHAR) + ',%'
		)
	@floornowhere
	AND HouseName <> ''
	AND valid = 1
UNION
SELECT HouseId,HouseName,UnitNo,BuildArea,IsEvalue,subhousetype,subhousearea,floorno,frontcode,sightcode,purposecode weight
FROM @table_house_sub h WITH (NOLOCK)
WHERE BuildingId = @buildingid
	AND valid = 1
	AND CityId = @cityid
	@floornowhere
	AND HouseName <> ''
	AND h.houseid in (
		select max(houseid) from @table_house
		where buildingid = @buildingid
		AND valid = 1
		AND cityid = @cityid
		@floornowhere
		AND housename <> ''
		AND h.fxtcompanyid IN (
		SELECT value
		FROM dbo.splittotable((
		SELECT showcompanyid
		FROM fxtdatacenter.dbo.privi_company_showdata
		WHERE cityid = @cityid
			AND fxtcompanyid = @fxtcompanyid
			AND typecode = @typecode
		), ','))
		group by HouseName,BuildingId,FloorNo,CityID,FxtCompanyId
	)
	AND h.FxtCompanyId = @fxtcompanyid
	--AND h.FxtCompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT ShowCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid AND TypeCode = @typecode), ',') where value <> 25)
)T
where 1 = 1 and HouseName like @key 
order by (case when HouseName like @param  then 0 else 1 end) asc