SELECT a.* 
,ifnull(c.housecnt, 0) AS housecnt
FROM (
	SELECT DISTINCT FloorNo
	FROM ?table_house h 
	WHERE BuildingId = ?buildingid
		AND valid = 1
		AND CityId = ?cityid
		AND NOT EXISTS (
			SELECT HouseId
			FROM ?table_house_sub hs 
			WHERE h.HouseId = hs.HouseId
				AND hs.FxtCompanyId = ?fxtcompanyid
				AND hs.CityId = h.CityId
			)
	    AND (CONCAT(',',(SELECT showcompanyid
                               FROM privi_company_show_data
                               WHERE fxtcompanyid = ?fxtcompanyid
                                 AND cityid = ?cityid
                                 AND typecode = ?typecode),',') LIKE CONCAT('%,',h.fxtcompanyid,',%'))	
	UNION	
	SELECT DISTINCT FloorNo
	FROM ?table_house_sub h 
	WHERE BuildingId = ?buildingid
		AND valid = 1
		AND CityId = ?cityid
		AND h.FxtCompanyId = ?fxtcompanyid
	) a
LEFT JOIN (
	SELECT floorno
		,count(houseid) AS housecnt
	FROM (
		SELECT floorno
			,houseid
		FROM ?table_house h 
		WHERE valid = 1
			AND buildingid = ?buildingid
			AND CityId = ?cityid
			AND HouseName <> ''
			AND NOT EXISTS (
				SELECT HouseId
				FROM ?table_house_sub hs 
				WHERE h.HouseId = hs.HouseId
					AND hs.FxtCompanyId = ?fxtcompanyid
					AND hs.CityId = h.CityId AND valid = 1
				)
		    AND (CONCAT(',',(SELECT showcompanyid
                               FROM privi_company_show_data
                               WHERE fxtcompanyid = ?fxtcompanyid
                                 AND cityid = ?cityid
                                 AND typecode = ?typecode),',') LIKE CONCAT('%,',h.FxtCompanyId,',%'))		
		UNION		
		SELECT floorno
			,houseid
		FROM ?table_house_sub h 
		WHERE BuildingId = ?buildingid
			AND valid = 1
			AND CityId = ?cityid
			AND HouseName <> ''
			AND h.FxtCompanyId = ?fxtcompanyid
		) h
	GROUP BY floorno
	) c ON c.floorno = a.floorno
where 1 = 1 and a.FloorNo like ?param
order by (case when a.FloorNo like ?strKey then 0 else 1 end) asc ,floorno
