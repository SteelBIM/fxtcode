SELECT @top T.CityID
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
		FROM FxtDataCenter.dbo.SYS_Area a WITH (NOLOCK)
		WHERE a.AreaId = T.AreaID
		) AS areaname
	,buildingnum AS buildingtotal
	,totalnum AS housetotal
	,(case when X = 0 or X is null then (select top 1 X from @table_project p where p.FxtCompanyId = 25 and p.ProjectId = T.projectid and p.CityID = T.CityID) else x end) x
	,(case when y = 0 or y is null then (select top 1 y from @table_project p where p.FxtCompanyId = 25 and p.ProjectId = T.projectid and p.CityID = T.CityID) else y end) y
	--,x
	--,y
	,photo.photocnt
	, 0 as avgprice
	--,wp.avgprice
FROM (
	SELECT cityid
		,projectid
		,projectname
		,othername
		,areaid
		,subareaid
		,address
		,isevalue
		,usableyear
		,buildingnum
		,totalnum
		,x
		,y
		,PinYin
		,PinYinAll
	FROM @table_project p WITH (NOLOCK)
	WHERE 1 = 1
		AND p.cityid = @cityid
		AND p.projectid NOT IN (
			SELECT ps.projectid
			FROM @table_project_sub ps
			WHERE ps.projectid = p.projectid
				AND ps.fxt_companyid = @fxtcompanyid
				AND ps.cityid = p.cityid
			)
		AND valid = 1
		AND (
			',' + cast((
					SELECT showcompanyid
					FROM fxtdatacenter.dbo.privi_company_showdata WITH (NOLOCK)
					WHERE fxtcompanyid = @fxtcompanyid
						AND cityid = @cityid
						AND typecode = @typecode
					) AS VARCHAR) + ',' LIKE '%,' + cast(p.fxtcompanyid AS VARCHAR) + ',%'
			)
	
	UNION
	
	SELECT cityid
		,projectid
		,projectname
		,othername
		,areaid
		,subareaid
		,address
		,isevalue
		,usableyear
		,buildingnum
		,totalnum
		,x
		,y
		,PinYin
		,PinYinAll
	FROM @table_project_sub p WITH (NOLOCK)
	WHERE 1 = 1
		AND p.cityid = @cityid
		AND p.fxt_companyid = @fxtcompanyid
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
		FROM lnk_p_photo p WITH (NOLOCK)
		WHERE 1 = 1
			AND NOT EXISTS (
				SELECT id
				FROM lnk_p_photo_sub ps WITH (NOLOCK)
				WHERE ps.id = p.id
					AND ps.cityid = @cityid
					AND ps.fxtcompanyid = @fxtcompanyid
				)
			AND p.valid = 1
			AND p.cityid = @cityid
			AND p.fxtcompanyid IN (
				SELECT value
				FROM fxtproject.dbo.splittotable((
							SELECT showcompanyid
							FROM fxtdatacenter.dbo.privi_company_showdata
							WHERE cityid = @cityid
								AND fxtcompanyid = @fxtcompanyid
								AND typecode = @typecode
							), ',')
				)
			AND p.phototypecode LIKE '2009%'
		
		UNION
		
		SELECT Id
			,projectid
			,CityId
		FROM lnk_p_photo_sub p WITH (NOLOCK)
		WHERE 1 = 1
			AND p.valid = 1
			AND p.cityid = @cityid
			AND p.fxtcompanyid = @fxtcompanyid
			AND p.phototypecode LIKE '2009%'
		) t
	GROUP BY projectid
		,cityid
	) photo ON T.ProjectId = photo.projectid
	AND T.CityID = photo.cityid
--left join (
--	select wp.ProjectId
--		,(
--			case 
--			when wp.ProjectAvgPrice > 0
--			then wp.ProjectAvgPrice
--			else (select isnull(ap.ProjectAvgPrice,0) from @dat_projectavg ap WITH (NOLOCK) where ap.ProjectId = wp.ProjectId and FxtCompanyId = 25 and valid = 1 and usemonth = CONVERT(nvarchar(7),DATEADD(MM,-1,GETDATE()),121) + '-01' ) 
--			end
--			) AS avgprice
--	from @dat_weightproject wp WITH (NOLOCK) 
--	where wp.FxtCompanyId = 25
--	) wp
--	on T.ProjectId = wp.ProjectId 
WHERE 1 = 1
	AND (
		[ProjectName] LIKE @param
		OR [OtherName] LIKE @param
		OR [PinYin] LIKE @param
		OR [PinYinAll] LIKE @param
		OR [Address] LIKE @param
		)
	@buildingwhere
	@areawhere
	--@pricewhere
order by --@priceorderby 
(case when [ProjectName] like @strKey then 0 else 1 end) asc,ProjectId desc
