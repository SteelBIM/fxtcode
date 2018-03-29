SELECT Id, ProjectId,
(select top 1 codename from sys_code where code=phototypecode and id=2009 ) as PhotoTypeName
,PhotoTypeCode, Path, PhotoDate, PhotoName, CityId, Valid, FxtCompanyId
from LNK_P_Photo  p
where 1=1 and valid = 1 and p.projectid = @projectid and (','+cast((select ShowCompanyId from dbo.privi_company_showdata with(nolock) where FxtCompanyId=@fxtcompanyid and Cityid=@cityid) as varchar)+',' like '%,' + cast(FxtCompanyId as varchar) + ',%') and
p.projectid not in (select projectid from LNK_P_Photo_sub ps with(nolock) where p.ProjectId=ps.ProjectId and ps.valid =1 
and 
','+cast((select ShowCompanyId from dbo.privi_company_showdata with(nolock) where FxtCompanyId=@fxtcompanyid and Cityid=@cityid) as varchar)+',' like '%,' + cast(ps.FxtCompanyId as varchar) + ',%'
)
union
SELECT Id, ProjectId,
(select top 1 codename from sys_code where code=phototypecode and id=2009 ) as PhotoTypeName
,PhotoTypeCode, Path, PhotoDate, PhotoName, CityId, Valid, FxtCompanyId
from LNK_P_Photo_sub  p
where 1=1 and valid = 1 and p.projectid = @projectid and (','+cast((select ShowCompanyId from dbo.privi_company_showdata with(nolock) where FxtCompanyId=@fxtcompanyid and Cityid=@cityid) as varchar)+',' like '%,' + cast(FxtCompanyId as varchar) + ',%')