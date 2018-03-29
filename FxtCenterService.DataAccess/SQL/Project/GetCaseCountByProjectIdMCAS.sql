SELECT count(*)
FROM @casetable a WITH (NOLOCK)
INNER JOIN (
	SELECT projectId
		,ProjectName
		,FxtCompanyId
	FROM @projecttable p WITH (NOLOCK)
	WHERE NOT EXISTS (
			SELECT ProjectId
			FROM @projectsubtable ps WITH (NOLOCK)
			WHERE ps.ProjectId = p.ProjectId
				AND ps.CityID = @cityid
				AND ps.Fxt_CompanyId = @fxtcompanyid
				--AND ps.Fxt_CompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT ShowCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid AND TypeCode = @typecode), ',') where value <> 25)
			)
		AND p.CityID = @cityid
		AND p.Valid = 1
		AND p.FxtCompanyId IN (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT ShowCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid and TypeCode = @typecode), ','))
		AND p.ProjectId = @projectid	
	UNION	
	SELECT projectId
		,ProjectName
		,Fxt_CompanyId AS FxtCompanyId
	FROM @projectsubtable p WITH (NOLOCK)
	WHERE p.CityID = @cityid
		AND p.Valid = 1
		AND p.Fxt_CompanyId = @fxtcompanyid
		--AND p.Fxt_CompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT ShowCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid AND TypeCode = @typecode), ',') where value <> 25)
		AND p.ProjectId = @projectid
	) b ON a.projectid = b.projectid
WHERE 1 = 1
	AND a.valid = 1
	AND a.FXTCompanyId IN (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT CaseCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid and TypeCode = @typecode), ','))
	AND a.ProjectId = @projectid
	AND a.casedate BETWEEN DATEADD(MONTH, - @months, convert(NVARCHAR(10), GETDATE(), 121))	AND convert(NVARCHAR(10), GETDATE(), 121)
	AND a.BuildingArea > 0
	AND a.UnitPrice > 0
