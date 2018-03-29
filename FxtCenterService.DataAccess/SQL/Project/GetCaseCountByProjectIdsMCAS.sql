select pb.*,(
				select count(*) from DAT_Case db WITH (NOLOCK) where db.ProjectId = pb.ProjectId 
				and db.casedate BETWEEN DATEADD(MONTH, -@months, convert(NVARCHAR(10), GETDATE(), 121))	AND convert(NVARCHAR(10),  GETDATE(), 121)
				AND db.valid = 1
				AND db.FXTCompanyId IN (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT CaseCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid and TypeCode = @typecode), ','))
				AND db.BuildingArea > 0
				AND db.UnitPrice > 0
			) as CaseCount from (
	SELECT projectId
		,ProjectName
		,FxtCompanyId
	FROM DAT_Project p WITH (NOLOCK)
	WHERE NOT EXISTS (
			SELECT ProjectId
			FROM DAT_Project_sub ps WITH (NOLOCK)
			WHERE ps.ProjectId = p.ProjectId
				AND ps.CityID = @cityid
				AND ps.Fxt_CompanyId = @fxtcompanyid
				--AND ps.Fxt_CompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT ShowCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid AND TypeCode = @typecode), ',') where value <> 25)
			)
		AND p.CityID = @cityid
		AND p.Valid = 1
		AND p.FxtCompanyId IN (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT ShowCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid and TypeCode = @typecode), ','))
		AND p.ProjectId in (@projectids)	
	UNION	
	SELECT projectId
		,ProjectName
		,Fxt_CompanyId AS FxtCompanyId
	FROM DAT_Project_sub p WITH (NOLOCK)
	WHERE p.CityID = @cityid
		AND p.Valid = 1
		AND p.Fxt_CompanyId = @fxtcompanyid
		--AND p.Fxt_CompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT ShowCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid AND TypeCode = @typecode), ',') where value <> 25)
		AND p.ProjectId in (@projectids)
) pb 