select top 1 * from (
select ProjectId, ProjectName, Valid,FxtCompanyId from @dat_project with(nolock) where ProjectName=@projectname and Cityid=@cityId @where
and ProjectId not in (select ProjectId from @dat_project_sub with(nolock) where ProjectName=@projectname and CityId=@cityId @where and 
 (','+cast((select showcompanyid from  dbo.Privi_Company_ShowData with(nolock) where Fxtcompanyid=@fxtcompanyid and Cityid = @cityId) as varchar)+',' like '%,' + cast(Fxt_CompanyId as varchar) + ',%')
 ) 
union
select ProjectId, ProjectName, Valid, Fxt_CompanyId as FxtCompanyId from @dat_project_sub with(nolock) where ProjectName=@projectname and CityId=@cityId @where and 
 (','+cast((select showcompanyid from  dbo.Privi_Company_ShowData with(nolock) where Fxtcompanyid=@fxtcompanyid and Cityid = @cityId) as varchar)+',' like '%,' + cast(Fxt_CompanyId as varchar) + ',%')
 ) temp