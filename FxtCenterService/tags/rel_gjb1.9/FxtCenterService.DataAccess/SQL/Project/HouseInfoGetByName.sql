select top 1 * from (
select HouseId,BuildingId, HouseName, Valid,FxtCompanyId,'1' as Remark from @dat_house as tbl with(nolock) where 
BuildingId=@buildingid
and Cityid=@cityId 
and Valid=1
and HouseName=@housename  
and not exists (select * from @dat_house_sub as tbl2 with(nolock) where tbl.HouseId=tbl2.HouseId and BuildingId=@buildingid and CityId=@cityId and FxtCompanyid=@fxtcompanyid  
) 
and 
 (','+cast((select showcompanyid from  dbo.Privi_Company_ShowData with(nolock) where Fxtcompanyid=@fxtcompanyid and Cityid = @cityId) as varchar)+',' like '%,' + cast(tbl.FxtCompanyId as varchar) + ',%')
union
select HouseId,BuildingId, HouseName, Valid,FxtCompanyId,'0' as Remark from @dat_house_sub with(nolock) 
where BuildingId=@buildingid and CityId=@cityId and Valid=1 and HouseName=@housename  and 
 FxtCompanyid=@fxtcompanyid
 ) temp