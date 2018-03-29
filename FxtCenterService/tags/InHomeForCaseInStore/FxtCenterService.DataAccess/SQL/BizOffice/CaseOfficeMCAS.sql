SELECT
	co.Id
	,co.CityId
	,c.CityName
	,p.AreaName
	,p.SubAreaName
	,p.address
	,co.ProjectName
	,co.FloorNo
	,co.TotalFloor
	,co.BuildingArea
	,co.UnitPrice
	,co.TotalPrice
	,co.ManagerPrice
	,code.CodeName as CaseTypeCodeName
	,co.CaseDate
	,code1.CodeName as FitmentName
	,code2.CodeName as OfficeTypeName
	,co.SourceName
	,co.SourceLink
FROM [FxtData_Office].[dbo].[Dat_Case_Office] co with(nolock)
inner join (
	select T.ProjectId,T.AreaId,a.AreaName,T.SubAreaId,sa.SubAreaName,T.Address from (
		select * from [FxtData_Office].[dbo].[Dat_Project_Office] p with(nolock)
		where not exists (
			select ProjectId from [FxtData_Office].[dbo].[Dat_Project_Office_sub] ps with(nolock)
			where ps.ProjectId = p.ProjectId
			and ps.CityId = @cityid
			and ps.FxtCompanyId = @fxtcompanyid
			--and ps.FxtCompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT OfficeCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid AND TypeCode = @typecode), ',') where value <> 25)
		)
		and p.Valid = 1
		and p.FxtCompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT OfficeCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId=@cityid AND FxtCompanyId=@fxtcompanyid and TypeCode = @typecode),','))
		and p.CityId = @cityid
		union
		select * from [FxtData_Office].[dbo].[Dat_Project_Office_sub] p with(nolock)
		where p.Valid = 1
		and p.FxtCompanyId = @fxtcompanyid
		--and p.FxtCompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT OfficeCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid AND TypeCode = @typecode), ',') where value <> 25)
		and p.CityId = @cityid
	)T
	left join FxtDataCenter.dbo.SYS_Area a with(nolock) on T.AreaId = a.AreaId
	left join FxtDataCenter.dbo.SYS_SubArea_Office sa with(nolock) on T.SubAreaId = sa.SubAreaId
)p on co.ProjectId = p.ProjectId
left join FxtDataCenter.dbo.SYS_City c with(nolock) on co.CityId = c.CityId
left join FxtDataCenter.dbo.SYS_Code code with(nolock) on co.CaseTypeCode = code.Code
left join FxtDataCenter.dbo.SYS_Code code1 with(nolock) on co.Fitment = code1.Code
left join FxtDataCenter.dbo.SYS_Code code2 with(nolock) on co.OfficeType = code2.Code
where 1 = 1
and co.Valid = 1
and co.FxtCompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT OfficeCaseCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId=@cityid AND FxtCompanyId=@fxtcompanyid and TypeCode = @typecode),','))
and co.CityId = @cityid