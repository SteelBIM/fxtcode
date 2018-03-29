select top 1 * from (
select BuildingId,ProjectId, BuildingName, Valid,FxtCompanyId,0 as Fxt_CompanyId,BuildingTypeCode from @dat_building as tbl with(nolock) where 
ProjectId=@projectid
and Cityid=@cityId 
and Valid=1
and BuildingId=@buildingid  
and not exists (select * from @dat_building_sub as tbl2 with(nolock) where tbl.BuildingId=tbl2.BuildingId and ProjectId=@projectid and CityId=@cityId and Fxt_companyid=@fxtcompanyid 
) 
union
select BuildingId,ProjectId, BuildingName, Valid, Fxt_CompanyId as FxtCompanyId,Fxt_CompanyId,BuildingTypeCode from @dat_building_sub with(nolock) 
where ProjectId=@projectid and CityId=@cityId and Valid=1 and BuildingId=@buildingid and 
 Fxt_companyid=@fxtcompanyid
 ) temp