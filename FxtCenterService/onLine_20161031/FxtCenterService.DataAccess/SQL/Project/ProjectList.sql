SELECT 
	T.ProjectId
	,projectname
	,othername
	,areaid
	,subareaid
	,t.address
	,landarea
	,buildingarea
	,cubagerate
	,enddate
	,(case when X = 0 or X is null then (select top 1 X from @table_project p where p.FxtCompanyId = 25 and p.ProjectId = projectid and p.CityID = CityID) else x end) x
	,(case when y = 0 or y is null then (select top 1 y from @table_project p where p.FxtCompanyId = 25 and p.ProjectId = projectid and p.CityID = CityID) else y end) y
	,dpc.ChineseName as developcompanyname
	,(
		SELECT AreaName
		FROM FxtDataCenter.dbo.SYS_Area a WITH (NOLOCK)
		WHERE a.AreaId = T.AreaID
		) AS areaname
FROM (
	SELECT cityid
		,projectid
		,projectname
		,othername
		,areaid
		,subareaid
		,address
		,landarea
		,buildingarea
		,cubagerate
		,enddate
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
		,landarea
		,buildingarea
		,cubagerate
		,enddate
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
left join dbo.LNK_P_Company dp with(nolock)
on dp.ProjectId = T.ProjectId and dp.CompanyType = 2001001 and dp.CityId = @cityid
left join dbo.DAT_Company dpc with(nolock)
on dp.CompanyId = dpc.CompanyId

WHERE 1 = 1
	AND (
		[ProjectName] LIKE @param
		OR [OtherName] LIKE @param
		OR [PinYin] LIKE @param
		OR [PinYinAll] LIKE @param
		OR t.[Address] LIKE @param
		)
	@areawhere
--order by (case when [ProjectName] like @strKey then 0 else 1 end) asc
