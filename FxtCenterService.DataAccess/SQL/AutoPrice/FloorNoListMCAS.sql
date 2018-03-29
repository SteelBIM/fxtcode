SELECT a.* 
--,c.NominalFloor
,isnull(c.housecnt, 0) AS housecnt
FROM (
	SELECT DISTINCT FloorNo
	FROM @table_house h WITH (NOLOCK)
	WHERE BuildingId = @buildingid
		AND valid = 1
		AND CityId = @cityid
		AND h.HouseId NOT IN (
			SELECT HouseId
			FROM @table_house_sub hs WITH (NOLOCK)
			WHERE h.HouseId = hs.HouseId
				AND hs.FxtCompanyId = @fxtcompanyid
				--AND hs.FxtCompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT ShowCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid AND TypeCode = @typecode), ',') where value <> 25)
				AND hs.CityId = h.CityId
			) 
		AND h.FxtCompanyId IN (
			SELECT value
			FROM dbo.SplitToTable((
						SELECT ShowCompanyId
						FROM FxtDataCenter.dbo.Privi_Company_ShowData
						WHERE CityId = @cityid
							AND FxtCompanyId = @fxtcompanyid
							AND TypeCode = @typecode
						), ',')
			)	
	UNION	
	SELECT DISTINCT FloorNo
	FROM @table_house_sub h WITH (NOLOCK)
	WHERE BuildingId = @buildingid
		AND valid = 1
		AND CityId = @cityid
		AND h.FxtCompanyId = @fxtcompanyid
		--AND h.FxtCompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT ShowCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid AND TypeCode = @typecode), ',') where value <> 25)
	) a
LEFT JOIN (
	SELECT floorno
		--,NominalFloor
		,count(houseid) AS housecnt
	FROM (
		SELECT floorno
			--,NominalFloor
			,houseid
		FROM @table_house h WITH (NOLOCK)
		WHERE valid = 1
			AND buildingid = @buildingid
			AND CityId = @cityid
			AND HouseName <> ''
			AND h.HouseId NOT IN (
				SELECT HouseId
				FROM @table_house_sub hs WITH (NOLOCK)
				WHERE h.HouseId = hs.HouseId
					AND hs.FxtCompanyId = @fxtcompanyid
					--AND hs.FxtCompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT ShowCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid AND TypeCode = @typecode), ',') where value <> 25)
					AND hs.CityId = h.CityId
				)
			AND h.FxtCompanyId IN (
				SELECT value
				FROM dbo.SplitToTable((
							SELECT ShowCompanyId
							FROM FxtDataCenter.dbo.Privi_Company_ShowData
							WHERE CityId = @cityid
								AND FxtCompanyId = @fxtcompanyid
								AND TypeCode = @typecode
							), ',')
				)		
		UNION		
		SELECT floorno
			--,NominalFloor
			,houseid
		FROM @table_house_sub h WITH (NOLOCK)
		WHERE BuildingId = @buildingid
			AND valid = 1
			AND CityId = @cityid
			AND HouseName <> ''
			AND h.FxtCompanyId = @fxtcompanyid
			--AND h.FxtCompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT ShowCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid AND TypeCode = @typecode), ',') where value <> 25)
		) h
	GROUP BY floorno
	--,NominalFloor
	) c ON c.floorno = a.floorno
where 1 = 1 and a.FloorNo like @param
