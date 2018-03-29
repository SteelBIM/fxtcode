select distinct a.*
from (
SELECT [ProjectId],[ProjectName] +'['+a.[AreaName]+']' ProjectName,a.areaid,p.[Address],p.x,p.y,isevalue,
[PinYin],[OtherName],[PinYinAll]
FROM @table_project p with(nolock),FxtDataCenter.dbo.SYS_Area a with(nolock)where p.[Valid]=1 and p.[CityId]=@cityid and p.[AreaID]=a.[AreaID] 
and p.ProjectId not in (select ProjectId from @table_project_sub ps with(nolock) where p.ProjectId=ps.ProjectId and ps.Fxt_CompanyId=@fxtcompanyid and ps.CityId=p.CityId) AND p.FxtCompanyId  IN (SELECT value FROM  dbo.SplitToTable((SELECT ShowCompanyId FROM FxtDataCenter.dbo.Privi_Company_ShowData WHERE CityId=@cityid AND FxtCompanyId=@fxtcompanyid and TypeCode = @typecode),',')) 
and ([pinyin] like @key or [address] like @address or [projectname] like @key or [othername] like @key or [pinyinall] like @key )
union 
SELECT [ProjectId],[ProjectName] +'['+a.[AreaName]+']' ProjectName,a.areaid,p.[Address],p.x,p.y,isevalue,
[PinYin],[OtherName],[PinYinAll]
FROM @table_project_sub p with(nolock),FxtDataCenter.dbo.SYS_Area a with(nolock)where p.[Valid]=1 and p.[CityId]=@cityid and p.[AreaID]=a.[AreaID] 
and p.Fxt_CompanyId=@fxtcompanyid
and ([pinyin] like @key or [address] like @address or [projectname] like @key or [othername] like @key or [pinyinall] like @key )
union
SELECT [ProjectId],[ProjectName] +'['+a.[AreaName]+']' ProjectName,a.areaid,p.[Address],p.x,p.y,isevalue,
[PinYin],[OtherName],[PinYinAll]
FROM @table_project p with(nolock),FxtDataCenter.dbo.SYS_Area a with(nolock)where p.[Valid]=1 and p.[CityId]=@cityid and p.[AreaID]=a.[AreaID] 
and p.ProjectId in (
	select projectid
	from @table_building b with(nolock) 
	where valid=1 and cityid=@cityid and buildingid not in(
		select buildingid from @table_building_sub ps with(nolock)
		where ps.projectid=b.projectid and ps.fxt_companyid=@fxtcompanyid and ps.cityid=b.cityid
	) and b.fxtcompanyid in (select value from  dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata where cityid=@cityid and fxtcompanyid=@fxtcompanyid and typecode = @typecode),','))
	and  [doorplate] like @key
	union 
	select projectid
	from @table_building_sub b with(nolock)
	where valid=1 and cityid=@cityid and b.fxt_companyid=@fxtcompanyid and  [doorplate] like @key
)
and p.ProjectId not in (select ProjectId from @table_project_sub ps with(nolock) where p.ProjectId=ps.ProjectId and ps.Fxt_CompanyId=@fxtcompanyid and ps.CityId=p.CityId) AND p.FxtCompanyId  IN (SELECT value FROM  dbo.SplitToTable((SELECT ShowCompanyId FROM FxtDataCenter.dbo.Privi_Company_ShowData WHERE CityId=@cityid AND FxtCompanyId=@fxtcompanyid and TypeCode = @typecode),',')) 
union 
SELECT [ProjectId],[ProjectName] +'['+a.[AreaName]+']' ProjectName,a.areaid,p.[Address],p.x,p.y,isevalue,
[PinYin],[OtherName],[PinYinAll]
FROM @table_project_sub p with(nolock),FxtDataCenter.dbo.SYS_Area a with(nolock)where p.[Valid]=1 and p.[CityId]=@cityid and p.[AreaID]=a.[AreaID] 
and p.Fxt_CompanyId=@fxtcompanyid
and p.ProjectId in (
	select projectid
	from @table_building b with(nolock) 
	where valid=1 and cityid=@cityid and buildingid not in(
		select buildingid from @table_building_sub ps with(nolock)
		where ps.projectid=b.projectid and ps.fxt_companyid=@fxtcompanyid and ps.cityid=b.cityid
	) and b.fxtcompanyid in (select value from  dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata where cityid=@cityid and fxtcompanyid=@fxtcompanyid and typecode = @typecode),','))
	and  [doorplate] like @key
	union 
	select projectid
	from @table_building_sub b with(nolock)
	where valid=1 and cityid=@cityid and b.fxt_companyid=@fxtcompanyid
	and  [doorplate] like @key)
)a
where 1=1