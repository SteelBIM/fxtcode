select top 1 * from (
select HouseId,BuildingId, HouseName, Valid,FxtCompanyId,FloorNo,FrontCode,SightCode,BuildArea,'1' as Remark from @dat_house as tbl with(nolock) where 
BuildingId=@buildingid
and Cityid=@cityId 
and Valid=1
and HouseId=@houseid  
and not exists (select * from @dat_house_sub as tbl2 with(nolock) where tbl.HouseId=tbl2.HouseId and BuildingId=@buildingid and CityId=@cityId and FxtCompanyid=@fxtcompanyid  
) 
union
select HouseId,BuildingId, HouseName, Valid,FxtCompanyId,FloorNo,FrontCode,SightCode,BuildArea,'0' as Remark from @dat_house_sub with(nolock) 
where BuildingId=@buildingid and CityId=@cityId and Valid=1 and HouseId=@houseid   and 
 FxtCompanyid=@fxtcompanyid
 ) temp