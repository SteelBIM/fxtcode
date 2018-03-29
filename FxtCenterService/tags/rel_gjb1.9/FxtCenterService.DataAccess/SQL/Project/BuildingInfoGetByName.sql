select top 1 * from (
select BuildingId,ProjectId, BuildingName, Valid,FxtCompanyId,0 as Fxt_CompanyId from @dat_building as tbl with(nolock) where 
ProjectId=@projectid
and Cityid=@cityId 
and Valid=1
and BuildingName=@buildingname  
and not exists (select * from @dat_building_sub as tbl2 with(nolock) where tbl.BuildingId=tbl2.BuildingId and ProjectId=@projectid and CityId=@cityId and Fxt_companyid=@fxtcompanyid 
) 
and 
 (','+cast((select showcompanyid from  dbo.Privi_Company_ShowData with(nolock) where Fxtcompanyid=@fxtcompanyid and Cityid = @cityId) as varchar)+',' like '%,' + cast(tbl.FxtCompanyId as varchar) + ',%')
union
select BuildingId,ProjectId, BuildingName, Valid, Fxt_CompanyId as FxtCompanyId,Fxt_CompanyId from @dat_building_sub with(nolock) 
where ProjectId=@projectid and CityId=@cityId and Valid=1 and BuildingName=@buildingname  and 
 Fxt_companyid=@fxtcompanyid
 ) temp