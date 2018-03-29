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
