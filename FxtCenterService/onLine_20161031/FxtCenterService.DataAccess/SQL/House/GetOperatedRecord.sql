select * from(
select
h.cityid,
h.buildingid,
h.houseid,
h.housename,
case when h.valid = 0 then 
	case when (h.CreateTime < @startdate and h.SaveDateTime BETWEEN @startdate AND @enddate) then 0 else -1 end
else
	case when (h.SaveUser is not null AND h.CreateTime< @startdate) then 2 else 1 end
end as opertedstate
from @dat_house h with(nolock) where
not exists (select * from @dat_house_sub as tbl2 where HouseId=tbl2.HouseId and CityId=@cityId and FxtCompanyid=@fxtcompanyid)
And h.SaveDateTime BETWEEN @startdate AND @enddate $buildingidlimit
union
select
h.cityid,
h.buildingid,
h.houseid,
h.housename,
case when h.valid = 0 then 
	case when (h.CreateTime < @startdate and h.SaveDateTime BETWEEN @startdate AND @enddate) then 0 else -1 end
else
	case when (h.SaveUser is not null AND h.CreateTime< @startdate) then 2 else 1 end
end as opertedstate
from @dat_house_sub h with(nolock) where
h.FxtCompanyid=@fxtcompanyid
And h.SaveDateTime BETWEEN @startdate AND @enddate $buildingidlimit
) tb where opertedstate >= 0;