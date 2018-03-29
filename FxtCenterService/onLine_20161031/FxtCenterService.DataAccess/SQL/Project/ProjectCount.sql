select COUNT(projectid) 
FROM (
	SELECT projectid
	FROM @projecttable p WITH (NOLOCK)
	WHERE 1 = 1
		AND p.cityid = @cityid
		AND p.projectid NOT IN (
			SELECT ps.projectid
			FROM @projecttable_sub ps
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
	
	SELECT projectid
	FROM @projecttable_sub p WITH (NOLOCK)
	WHERE 1 = 1
		AND p.cityid = @cityid
		AND p.fxt_companyid = @fxtcompanyid
		AND valid = 1
	) T