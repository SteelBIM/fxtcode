SELECT c.*
FROM (
	SELECT floorno
		,case when REPLACE(NominalFloor,'层','') = '' then cast(FloorNo as varchar(20)) 
		when REPLACE(NominalFloor,'层','') <> '' then REPLACE(NominalFloor,'层','')
		end as NominalFloor
	FROM (
		SELECT floorno
			,NominalFloor
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
			,NominalFloor
			,houseid
		FROM @table_house_sub h WITH (NOLOCK)
		WHERE BuildingId = @buildingid
			AND valid = 1
			AND CityId = @cityid
			AND HouseName <> ''
			AND h.FxtCompanyId = @fxtcompanyid
		) h
	GROUP BY floorno
	,REPLACE(NominalFloor,'层','')
	) c 
where 1 = 1 and c.FloorNo like @param
GROUP by c.FloorNo,c.NominalFloor
