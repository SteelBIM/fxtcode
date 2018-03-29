select * from(
select
tb.cityid,
tb.buildingid,
tb.projectid,
tb.buildingname,
case when tb.valid = 0 then 
	case when (tb.CreateTime < @startdate and tb.SaveDateTime BETWEEN @startdate AND @enddate) then 0 else -1 end
else
	case when (tb.SaveUser is not null AND tb.CreateTime< @startdate) then 2 else 1 end
end as opertedstate
from @table_building tb with(nolock) where
tb.buildingid not in (select buildingid from @table_building_sub ps where buildingid=ps.buildingid and (','+cast((select ShowCompanyId from FxtDataCenter.dbo.Privi_Company_ShowData with(nolock) where FxtCompanyId=@fxtcompanyid and Cityid=ps.cityid and TypeCode = @typecode) as varchar)+',' like '%,' + cast(ps.Fxt_CompanyId as varchar) + ',%') and ps.CityId=CityId)
And tb.SaveDateTime BETWEEN @startdate AND @enddate $projectidlimit
union
select
tb.cityid,
tb.buildingid,
tb.projectid,
tb.buildingname,
case when tb.valid = 0 then 
	case when (tb.CreateTime < @startdate and tb.SaveDateTime BETWEEN @startdate AND @enddate) then 0 else -1 end
else
	case when (tb.SaveUser is not null AND tb.CreateTime< @startdate) then 2 else 1 end
end as opertedstate
from @table_building_sub tb with(nolock) where
tb.Fxt_CompanyId=@fxtcompanyid
And tb.SaveDateTime BETWEEN @startdate AND @enddate $projectidlimit
) tb where opertedstate >= 0;