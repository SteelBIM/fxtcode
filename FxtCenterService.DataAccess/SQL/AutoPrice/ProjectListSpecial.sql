select distinct a.*
from (
SELECT [ProjectId],[ProjectName] +'['+a.[AreaName]+']' ProjectName,a.areaid,p.[Address],p.x,p.y,isevalue,
[PinYin],[OtherName],[PinYinAll]
FROM @table_project p with(nolock),FxtDataCenter.dbo.SYS_Area a with(nolock)where p.[Valid]=1 and p.[CityId]=@cityid and p.[AreaID]=a.[AreaID] 
and p.ProjectId not in (select ProjectId from @table_project_sub ps with(nolock) where p.ProjectId=ps.ProjectId and ps.Fxt_CompanyId=@fxtcompanyid and ps.CityId=p.CityId) AND p.FxtCompanyId  IN (SELECT value FROM  dbo.SplitToTable((SELECT ShowCompanyId FROM FxtDataCenter.dbo.Privi_Company_ShowData WHERE CityId=@cityid AND FxtCompanyId=@fxtcompanyid and TypeCode = @typecode),',')) 
union 
SELECT [ProjectId],[ProjectName] +'['+a.[AreaName]+']' ProjectName,a.areaid,p.[Address],p.x,p.y,isevalue,
[PinYin],[OtherName],[PinYinAll]
FROM @table_project_sub p with(nolock),FxtDataCenter.dbo.SYS_Area a with(nolock)where p.[Valid]=1 and p.[CityId]=@cityid and p.[AreaID]=a.[AreaID] 
and p.Fxt_CompanyId=@fxtcompanyid
)a left join
(select ProjectId,Doorplate
from @table_building b with(nolock) 
where valid=1 and 
CityID=@cityid and 
BuildingId not in(select BuildingId from @table_building_sub ps with(nolock) where ps.ProjectId=b.ProjectId and ps.Fxt_CompanyId=@fxtcompanyid and ps.CityId=b.CityId) AND 
b.FxtCompanyId IN (SELECT value FROM  dbo.SplitToTable((SELECT ShowCompanyId FROM FxtDataCenter.dbo.Privi_Company_ShowData WHERE CityId=@cityid AND FxtCompanyId=@fxtcompanyid and TypeCode = @typecode),','))
union 
select ProjectId,Doorplate
from @table_building_sub b with(nolock)
where valid=1 and 
CityID=@cityid and 
b.Fxt_CompanyId=@fxtcompanyid
) b on a.ProjectId = b.ProjectId
where 1=1
and (a.[PinYin] like @key or a.[Address] like @address or a.[ProjectName] like @key or a.[OtherName] like @key or a.[PinYinAll] like @key or b.[Doorplate] like @key)