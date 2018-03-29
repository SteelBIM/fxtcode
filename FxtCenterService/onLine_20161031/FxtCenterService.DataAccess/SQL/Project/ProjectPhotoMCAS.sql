declare @t int
set @t=@buildingid

SELECT Id
	,ProjectId
	,(SELECT TOP 1 codename FROM FxtDataCenter.dbo.SYS_Code WHERE code = phototypecode AND id = 2009) AS PhotoTypeName
	,PhotoTypeCode
	,Path
	,PhotoDate
	,PhotoName
	,CityId
	,Valid
	,FxtCompanyId
FROM fxtproject.dbo.LNK_P_Photo p
WHERE 1 = 1
	AND valid = 1
	and not exists (
		select Id from fxtproject.dbo.LNK_P_Photo_sub ps with(nolock)
		where p.Id = ps.Id
		and p.CityId = ps.CityId
		and ps.FxtCompanyId = @fxtcompanyid
		--and ps.FxtCompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT ShowCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid AND TypeCode = @typecode), ',') where value <> 25)
	)
	and p.ProjectId = @projectid
	and p.buildingid >= (case when @t=0 then 0 else 1 end )
	and 0 = (case when @t=0 then p.buildingid else 0 end )
	and p.CityId = @cityid
	and p.FxtCompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT ShowCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid and TypeCode = @typecode), ','))
UNION
SELECT Id
	,ProjectId
	,(SELECT TOP 1 codename FROM FxtDataCenter.dbo.SYS_Code WHERE code = phototypecode AND id = 2009) AS PhotoTypeName
	,PhotoTypeCode
	,Path
	,PhotoDate
	,PhotoName
	,CityId
	,Valid
	,FxtCompanyId
FROM fxtproject.dbo.LNK_P_Photo_sub p
WHERE 1 = 1
	AND valid = 1
	AND p.projectid = @projectid
	and p.buildingid >= (case when @t=0 then 0 else 1 end )
	and 0 = (case when @t=0 then p.buildingid else 0 end )
	AND CityId = @cityid
	and FxtCompanyId = @projectid
	--and FxtCompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT ShowCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid AND TypeCode = @typecode), ',') where value <> 25)