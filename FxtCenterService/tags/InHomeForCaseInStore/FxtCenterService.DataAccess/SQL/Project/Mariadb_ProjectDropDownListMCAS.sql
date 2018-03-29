SELECT T.CityID
	,T.ProjectId
	,projectname
	,othername
	,areaid
	,subareaid
	,address
	,isevalue
	,usableyear
	,(
		SELECT AreaName
		FROM sys_area a 
		WHERE a.AreaId = T.AreaID
		) AS areaname
	,buildingnum AS buildingtotal
	,totalnum AS housetotal
	,(case when X = 0 or X is null then (select X from base_project p where p.FxtCompanyId = 25 and p.ProjectId = projectid and p.CityID = CityID limit 1) else x end) x
	,(case when y = 0 or y is null then (select y from base_project p where p.FxtCompanyId = 25 and p.ProjectId = projectid and p.CityID = CityID limit 1) else y end) y
	,photo.photocnt
	, 0 as avgprice
FROM (
	SELECT cityid
		,projectid
		,projectname
		,othername
		,areaid
		,subareaid
		,address
		,isevaluate AS isevalue
		,usableyear
		,buildingnumber AS buildingnum
		,totalnumber AS totalnum
		,x
		,y
		,PinYin
		,PinYinAll
	FROM base_project p 
	WHERE 1 = 1
		AND p.cityid = ?cityid
		AND NOT EXISTS (
			SELECT ps.projectid
			FROM base_project_sub ps
			WHERE ps.projectid = p.projectid
				AND ps.fxtcompanyid = ?fxtcompanyid
				AND ps.cityid = p.cityid
			)
		AND valid = 1
		AND CONCAT(',',(SELECT showcompanyid
                               FROM privi_company_show_data
                               WHERE fxtcompanyid = ?fxtcompanyid
                                 AND cityid = ?cityid
                                 AND typecode = ?typecode),',') LIKE CONCAT('%,',p.fxtcompanyid,',%')
	
	UNION ALL
	
	SELECT cityid
		,projectid
		,projectname
		,othername
		,areaid
		,subareaid
		,address
		,isevaluate AS isevalue
		,usableyear
		,buildingnumber AS buildingnum
		,totalnumber AS totalnum
		,x
		,y
		,PinYin
		,PinYinAll
	FROM base_project_sub p 
	WHERE 1 = 1
		AND p.cityid = ?cityid
		AND p.fxtcompanyid = ?fxtcompanyid
		AND valid = 1
	) T
LEFT JOIN (
	SELECT projectid
		,cityid
		,count(*) AS photocnt
	FROM (
		SELECT Id
			,projectid
			,CityId
		FROM base_lnk_p_photo p 
		WHERE 1 = 1
			AND NOT EXISTS (
				SELECT id
				FROM base_lnk_p_photo_sub ps 
				WHERE ps.id = p.id
					AND ps.cityid = ?cityid
					AND ps.fxtcompanyid = ?fxtcompanyid
				)
			AND p.valid = 1
			AND p.cityid = ?cityid
		    AND CONCAT(',',(SELECT showcompanyid
                               FROM privi_company_show_data
                               WHERE fxtcompanyid = ?fxtcompanyid
                                 AND cityid = ?cityid
                                 AND typecode = ?typecode),',') LIKE CONCAT('%,',p.fxtcompanyid,',%')
			AND p.phototypecode LIKE '2009%'
		
		UNION all
		
		SELECT Id
			,projectid
			,CityId
		FROM base_lnk_p_photo_sub p 
		WHERE 1 = 1
			AND p.valid = 1
			AND p.cityid = ?cityid
			AND p.fxtcompanyid = ?fxtcompanyid
			AND p.phototypecode LIKE '2009%'
		) t
	GROUP BY projectid
		,cityid
	) photo ON T.ProjectId = photo.projectid
	AND T.CityID = photo.cityid
WHERE 1 = 1
	AND (
		ProjectName LIKE ?param
		OR OtherName LIKE ?param
		OR PinYin LIKE ?param
		OR PinYinAll LIKE ?param
		OR Address LIKE ?param
		)
	?buildingwhere
	?areawhere
order by 
(case when ProjectName like ?strKey then 0 else 1 end) asc,ProjectId desc