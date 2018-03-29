select top 1 * from (
select ProjectId, ProjectName,PurposeCode, Valid,FxtCompanyId,0 as Fxt_CompanyId,Detail from @dat_project as tbl with(nolock) where
Cityid=@cityId 
and Valid=1
and ProjectId=@projectid  
and  not exists (select * from @dat_project_sub as tbl2 with(nolock) where tbl.ProjectId=tbl2.ProjectId and  CityId=@cityId and Fxt_companyid=@fxtcompanyid 
)
union
select ProjectId, ProjectName,PurposeCode, Valid, Fxt_CompanyId as FxtCompanyId,Fxt_CompanyId,Detail from @dat_project_sub with(nolock) 
where  CityId=@cityId and Valid=1 and ProjectId=@projectid  and 
 Fxt_companyid=@fxtcompanyid
 ) temp