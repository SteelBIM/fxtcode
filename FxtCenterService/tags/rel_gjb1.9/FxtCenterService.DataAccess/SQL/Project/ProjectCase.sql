select c.ProjectId,p.ProjectName,c.BuildingArea,c.totalprice,
isnull(c.UnitPrice,0) as UnitPrice,cd.codename as SourceTypeName,
Convert(varchar(10),c.CaseDate,120) CaseDate,cp.codename as PurposeName,ce.codename as BuildingTypeName,c.BuildingName,c.HouseNo 
from @table_project as p with(nolock) ,@table_case as c with(nolock) left join sys_Code as ce with(nolock) on ce.code=c.BuildingTypeCode,sys_code as cd with(nolock),
sys_code as cp with(nolock)  where c.ProjectId=p.ProjectId and p.valid=1  and c.valid=1 and c.caseTypeCode=cd.code and cp.code=c.PurposeCode and c.projectid = @projectid and c.casedate between @startdate and @enddate and  
 (','+cast((select showcompanyid from  dbo.Privi_Company_ShowData pc with(nolock) where pc.Fxtcompanyid=@fxtcompanyid and pc.Cityid = @cityid) as varchar)+',' like '%,' + cast(c.fxtcompanyid as varchar) + ',%')
