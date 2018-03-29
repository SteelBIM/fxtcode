select * from (
SELECT HouseId,HouseName,UnitNo,BuildArea,isevaluate as IsEvalue,subhousetype,subhousearea,floorno,frontcode,sightcode,purposecode
FROM ?table_house h 
WHERE BuildingId = ?buildingid
	AND CityId = ?cityid
	AND h.houseid in (
		select max(houseid) from ?table_house
		where buildingid = ?buildingid
		AND valid = 1
		AND cityid = ?cityid
		?floornowhere
		AND housename <> ''
		AND (CONCAT(',',(SELECT showcompanyid
                               FROM privi_company_show_data
                               WHERE fxtcompanyid = ?fxtcompanyid
                                 AND cityid = ?cityid
                                 AND typecode = ?typecode),',') LIKE CONCAT('%,',h.fxtcompanyid,',%'))	
		group by HouseName,BuildingId,FloorNo,CityID,FxtCompanyId
	)
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
                                 AND typecode = ?typecode),',') LIKE CONCAT('%,',fxtcompanyid,',%'))
	?floornowhere
	AND HouseName <> ''
	AND valid = 1
UNION
SELECT HouseId,HouseName,UnitNo,BuildArea,isevaluate as IsEvalue,subhousetype,subhousearea,floorno,frontcode,sightcode,purposecode weight
FROM ?table_house_sub h 
WHERE BuildingId = ?buildingid
	AND valid = 1
	AND CityId = ?cityid
	?floornowhere
	AND HouseName <> ''
	AND h.houseid in (
		select max(houseid) from ?table_house
		where buildingid = ?buildingid
		AND valid = 1
		AND cityid = ?cityid
		?floornowhere
		AND housename <> ''
		AND (CONCAT(',',(SELECT showcompanyid
                               FROM privi_company_show_data
                               WHERE fxtcompanyid = ?fxtcompanyid
                                 AND cityid = ?cityid
                                 AND typecode = ?typecode),',') LIKE CONCAT('%,',h.fxtcompanyid,',%'))
		group by HouseName,BuildingId,FloorNo,CityID,FxtCompanyId
	)
	AND h.FxtCompanyId = ?fxtcompanyid
)T
where 1 = 1 and HouseName like ?key 
order by (case when HouseName like ?param  then 0 else 1 end) asc