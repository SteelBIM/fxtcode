select top 1 * from (
select ProjectId, ProjectName, Valid,FxtCompanyId,0 as Fxt_CompanyId,Detail,CreateTime from @dat_project as tbl with(nolock) where
Cityid=@cityId 
and Valid=1
and ProjectName in (@projectnames)  @where
and  not exists (select * from @dat_project_sub as tbl2 with(nolock) where tbl.ProjectId=tbl2.ProjectId and  CityId=@cityId and Fxt_companyid=@fxtcompanyid 
) 
and 
 (','+cast((select showcompanyid from FxtDataCenter.dbo.Privi_Company_ShowData with(nolock) where Fxtcompanyid=@fxtcompanyid and Cityid = @cityId and TypeCode = @typecode) as varchar)+',' like '%,' + cast(tbl.FxtCompanyId as varchar) + ',%')
union
select ProjectId, ProjectName, Valid, Fxt_CompanyId as FxtCompanyId,Fxt_CompanyId,Detail,CreateTime from @dat_project_sub with(nolock) 
where  CityId=@cityId and Valid=1 and ProjectName in (@projectnames) @where and 
 Fxt_companyid=@fxtcompanyid
 ) temp