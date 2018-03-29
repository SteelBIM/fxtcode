select * from(
select
CityID,
ProjectId,
ProjectName,
AreaID,
CreateTime,
SaveDateTime,
Valid,
case when valid = 0 then 
	case when (CreateTime < @startdate and SaveDateTime BETWEEN @startdate AND @enddate) then 0 else -1 end
else
	case when (SaveUser is not null AND CreateTime< @startdate) then 2 else 1 end
end as opertedstate
from @table_project p with(nolock)
where p.CityId=@cityid
AND p.ProjectId NOT IN (select ProjectId from @table_project_sub ps where p.ProjectId=ps.ProjectId and ps.Fxt_CompanyId = @fxtcompanyid and ps.CityId=p.CityId) 
AND p.FxtCompanyId IN (SELECT value FROM dbo.SplitToTable((SELECT ShowCompanyId FROM FxtDataCenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid AND TypeCode = @typecode),',')) 
And ((CreateTime BETWEEN @startdate AND @enddate) OR (SaveDateTime BETWEEN @startdate AND @enddate)) $appendlimit
union
select
cityid,
projectid,
projectname,
areaid,
CreateTime,
SaveDateTime,
Valid,
case when valid = 0 then 
	case when (CreateTime < @startdate and SaveDateTime BETWEEN @startdate AND @enddate) then 0 else -1 end
else
	case when (SaveUser is not null AND CreateTime< @startdate) then 2 else 1 end
end as opertedstate
FROM @table_project_sub p with(nolock)
where p.CityId = @cityid and p.Fxt_CompanyId = @fxtcompanyid
And ((CreateTime BETWEEN @startdate AND @enddate) OR (SaveDateTime BETWEEN @startdate AND @enddate)) $appendlimit
) tb where opertedstate >= 0;