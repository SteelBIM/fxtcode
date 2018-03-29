select HouseId,HouseName,UnitNo,BuildArea,IsEvalue,subhousetype,subhousearea 
from @table_house h with(nolock) 
where BuildingId=@buildingid and CityId=@cityid and 
 (','+cast((select showcompanyid from  dbo.Privi_Company_ShowData pc with(nolock) where pc.Fxtcompanyid=@fxtcompanyid and pc.Cityid = @cityid) as varchar)+',' like '%,' + cast(fxtcompanyid as varchar) + ',%')
 and FloorNo=@floorno and HouseName<>'' and valid = 1 