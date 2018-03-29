select 
tb1.ProjectId,
tb1.CityID,
tb3.zipcode as CityZipcode,
tb3.CityName,
tb1.AreaID,
tb2.AreaName,
tb1.ProjectName +'['+tb2.AreaName+']' as ProjectName,
tb4.Code as PurposeCode,
tb4.CodeName as PurposeCodeName,
tb5.Code as BuildingTypeCode,
tb5.CodeName as BuildingTypeCodeName,
tb1.Address,
tb6.SubAreaId,
tb6.SubAreaName,
tb1.EndDate,
tb1.SaleDate
from (
	SELECT 
	p.ProjectId,
	p.CityID,
	p.AreaID,
	p.ProjectName,
	p.PurposeCode,
	p.BuildingTypeCode,
	p.Address,
	p.SubAreaId,
	p.SaleDate,
	p.EndDate
	FROM @table_project p with(nolock)
	where p.[Valid]=1 and p.[CityId]=@cityid $keylimit 
	and p.ProjectId not in (select ProjectId from @table_project_sub ps with(nolock) where ps.Valid=1 and p.ProjectId=ps.ProjectId and ps.Fxt_CompanyId=@fxtcompanyid and ps.CityId=p.CityId) 
	AND p.FxtCompanyId  IN (SELECT value FROM  dbo.SplitToTable((SELECT ShowCompanyId FROM FxtDataCenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId=@fxtcompanyid and TypeCode = @typecode),',')) 
	union 
	SELECT 
	p.ProjectId,
	p.CityID,
	p.AreaID,
	p.ProjectName,
	p.PurposeCode,
	p.BuildingTypeCode,
	p.Address,
	p.SubAreaId,
	p.SaleDate,
	p.EndDate
	FROM @table_project_sub p with(nolock)
	where p.[Valid]=1 and p.[CityId]=@cityid $keylimit 
	and p.Fxt_CompanyId=@fxtcompanyid
) tb1
left join FxtDataCenter.dbo.SYS_Area tb2 on tb1.AreaID = tb2.AreaId
left join FxtDataCenter.dbo.SYS_City tb3 on tb1.CityID = tb3.CityID
left join FxtDataCenter.dbo.SYS_Code tb4 on tb1.PurposeCode = tb4.Code
left join FxtDataCenter.dbo.SYS_Code tb5 on tb1.BuildingTypeCode = tb5.Code
left join FxtDataCenter.dbo.SYS_SubArea tb6 on tb1.SubAreaId = tb6.SubAreaId