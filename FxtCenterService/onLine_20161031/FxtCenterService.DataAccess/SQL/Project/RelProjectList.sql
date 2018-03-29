select * from
(
	select pd.ProjectId as P,project.*,
	CONVERT(int,FXTProject.dbo.fn_GetDistance(project.x,project.y,@x,@y)*1000) 'Distance'
	from 
	(
		SELECT CityID,ProjectId,ProjectName,X,Y,AreaID
		FROM @projecttable AS p WITH (NOLOCK)
		WHERE NOT EXISTS (
				SELECT *
				FROM @projecttable_sub AS ps WITH (NOLOCK)
				WHERE p.ProjectId = ps.ProjectId
					AND ps.CityID = @CityId
					AND ps.Fxt_CompanyId = @FxtCompanyId
				)
			AND p.Valid = 1
			AND p.CityID = @CityId
			AND p.FxtCompanyId  IN (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT ShowCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @CityId AND FxtCompanyId = @FxtCompanyId and TypeCode = @TypeCode), ','))
			and p.ProjectID = @ProjectId
		UNION	
		SELECT CityID,ProjectId,ProjectName,X,Y,AreaID
		FROM @projecttable_sub p WITH (NOLOCK)
		WHERE p.Valid = 1
			AND p.CityID = @CityId
			AND p.Fxt_CompanyId = @FxtCompanyId
			and p.ProjectID = @ProjectId
	) 
	pd 
	left join 
	(
		SELECT CityID,ProjectId,ProjectName,X,Y,AreaID
		FROM @projecttable AS p WITH (NOLOCK)
		WHERE NOT EXISTS (
				SELECT *
				FROM @projecttable_sub AS ps WITH (NOLOCK)
				WHERE p.ProjectId = ps.ProjectId
					AND ps.CityID = @CityId
					AND ps.Fxt_CompanyId = @FxtCompanyId
				)
			AND p.Valid = 1
			AND p.CityID = @CityId 
			AND p.FxtCompanyId  IN (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT ShowCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @CityId AND FxtCompanyId = @FxtCompanyId and TypeCode = @TypeCode), ','))
		UNION	
		SELECT CityID,ProjectId,ProjectName,X,Y,AreaID
		FROM @projecttable_sub p WITH (NOLOCK)
		WHERE p.Valid = 1
			AND p.CityID = @CityId
			AND p.Fxt_CompanyId = @FxtCompanyId
	) project on pd.CityID = project.CityID
) pt
where pt.Distance  <= 1000 and pt.Distance <> 0
order by Distance