select c.ProjectId,p.ProjectName,c.BuildingArea,c.totalprice,c.CaseID,
isnull(c.UnitPrice,0) as UnitPrice,cd.codename as SourceTypeName,
Convert(varchar(10),c.CaseDate,120) CaseDate,cp.codename as PurposeName,ce.codename as BuildingTypeName,c.BuildingName,c.HouseNo 
from @table_project as p with(nolock) ,@table_case as c with(nolock) left join FxtDataCenter.dbo.SYS_Code as ce with(nolock) on ce.code=c.BuildingTypeCode,FxtDataCenter.dbo.SYS_Code as cd with(nolock),
FxtDataCenter.dbo.SYS_Code as cp with(nolock)  where c.ProjectId=p.ProjectId and p.valid=1  and c.valid=1 and c.caseTypeCode=cd.code and cp.code=c.PurposeCode and c.projectid = @projectid and c.casedate between @startdate and @enddate and  
 (','+cast((select CaseCompanyId from FxtDataCenter.dbo.Privi_Company_ShowData pc with(nolock) where pc.Fxtcompanyid=@fxtcompanyid and pc.Cityid = @cityid and pc.TypeCode = @typecode) as varchar)+',' like '%,' + cast(c.fxtcompanyid as varchar) + ',%')
