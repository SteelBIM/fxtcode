select HouseId,HouseName,UnitNo,BuildArea,IsEvalue,subhousetype,subhousearea,floorno,frontcode,sightcode,purposecode
from @table_house h with(nolock) 
where BuildingId=@buildingid and CityId=@cityid
	AND h.HouseId not in (select HouseId from @table_house_sub hs with(nolock) where h.HouseId=hs.HouseId and hs.FxtCompanyId=@FxtCompanyId and hs.CityId=h.CityId)
 and (','+cast((select showcompanyid from FxtDataCenter.dbo.Privi_Company_ShowData pc with(nolock) where pc.Fxtcompanyid=@fxtcompanyid and pc.Cityid = @cityid and pc.TypeCode = @typecode) as varchar)+',' like '%,' + cast(fxtcompanyid as varchar) + ',%')
 and FloorNo=@floorno and HouseName<>'' and valid = 1 
  union 
select HouseId,HouseName,UnitNo,BuildArea,IsEvalue,subhousetype,subhousearea,floorno,frontcode,sightcode,purposecode weight from @table_house_sub h with(nolock) where BuildingId=@buildingid 
and valid=1  and CityId=@cityid and FloorNo=@floorno and HouseName<>'' and h.FxtCompanyId=@fxtcompanyid 