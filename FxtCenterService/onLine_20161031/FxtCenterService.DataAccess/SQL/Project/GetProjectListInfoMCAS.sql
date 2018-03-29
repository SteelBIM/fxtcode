SELECT project.*,photo.photocnt
FROM (
	SELECT CityID,ProjectId,ProjectName,X,Y
	FROM @projecttable AS p WITH (NOLOCK)
	WHERE NOT EXISTS (
			SELECT *
			FROM @projectsubtable AS ps WITH (NOLOCK)
			WHERE p.ProjectId = ps.ProjectId
				AND ps.CityID = @cityId
				AND ps.Fxt_CompanyId = @fxtcompanyid
				--AND ps.Fxt_CompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT ShowCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid AND TypeCode = @typecode), ',') where value <> 25)
			)
		AND p.Valid = 1
		AND p.CityID = @cityId
		AND p.FxtCompanyId  IN (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT ShowCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid and TypeCode = @typecode), ','))
		AND p.ProjectId = @projectid	
	UNION	
	SELECT CityID,ProjectId,ProjectName,X,Y
	FROM @projectsubtable p WITH (NOLOCK)
	WHERE p.Valid = 1
		AND p.CityID = @cityId
		AND p.Fxt_CompanyId = @fxtcompanyid
		--AND p.Fxt_CompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT ShowCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid AND TypeCode = @typecode), ',') where value <> 25)
		AND p.ProjectId = @projectid
	) project
LEFT JOIN (
	SELECT ProjectId,CityId,count(*) AS photocnt
	FROM (
		SELECT *
		FROM LNK_P_Photo p WITH (NOLOCK)
		WHERE 1 = 1
			AND NOT EXISTS (
				SELECT Id
				FROM LNK_P_Photo_sub ps WITH (NOLOCK)
				WHERE ps.Id = p.Id
					AND ps.CityId = @cityid
					AND ps.FxtCompanyId = @fxtcompanyid
					--AND ps.FxtCompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT ShowCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid AND TypeCode = @typecode), ',') where value <> 25)
					AND ps.ProjectId = @projectid
				)
			AND p.Valid = 1
			AND p.CityId = @cityid
			AND p.FxtCompanyId IN (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT ShowCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid and TypeCode = @typecode), ','))
			AND p.ProjectId = @projectid
			AND p.PhotoTypeCode LIKE '2009%'		
		UNION		
		SELECT *
		FROM LNK_P_Photo_sub p WITH (NOLOCK)
		WHERE 1 = 1
			AND p.Valid = 1
			AND p.CityId = @cityid
			AND p.FxtCompanyId = @fxtcompanyid
			--AND p.FxtCompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT ShowCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid AND TypeCode = @typecode), ',') where value <> 25)
			AND p.ProjectId = @projectid
			AND p.PhotoTypeCode LIKE '2009%'
		) T
	GROUP BY ProjectId,CityId
) photo ON project.ProjectId = photo.ProjectId AND project.CityID = photo.CityId
