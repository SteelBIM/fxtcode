declare @tb table(floorno int)
declare @tb1 table(floorno int,housecnt int)
insert into @tb 
SELECT DISTINCT FloorNo
FROM @table_house h WITH (NOLOCK)
WHERE CityId = @cityid
	AND BuildingId = @buildingid
	AND valid = 1		
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
					FROM FxtDataCenter.dbo.Privi_Company_ShowData WITH (NOLOCK)
					WHERE CityId = @cityid
						AND FxtCompanyId = @fxtcompanyid
						AND TypeCode = @typecode
					), ',')
		)	
UNION	
SELECT DISTINCT FloorNo
FROM @table_house_sub h WITH (NOLOCK)
WHERE CityId = @cityid
	AND BuildingId = @buildingid
	AND valid = 1
	AND h.FxtCompanyId = @fxtcompanyid
	--AND h.FxtCompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT ShowCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid AND TypeCode = @typecode), ',') where value <> 25)

insert into @tb1
SELECT floorno
	--,NominalFloor
	,count(houseid) AS housecnt
FROM (
	SELECT floorno
		--,NominalFloor
		,houseid
	FROM @table_house h WITH (NOLOCK)
	WHERE  CityId = @cityid 
		AND buildingid = @buildingid
		AND valid = 1
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
						FROM FxtDataCenter.dbo.Privi_Company_ShowData WITH (NOLOCK)
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
	WHERE CityId = @cityid
		AND BuildingId = @buildingid
		AND valid = 1
		AND HouseName <> ''
		AND h.FxtCompanyId = @fxtcompanyid
		--AND h.FxtCompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT ShowCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid AND TypeCode = @typecode), ',') where value <> 25)
	) h
GROUP BY floorno
--,NominalFloor

SELECT a.* 
--,c.NominalFloor
,isnull(c.housecnt, 0) AS housecnt
FROM @tb a
LEFT JOIN @tb1 c 
ON c.floorno = a.floorno
where 1 = 1
