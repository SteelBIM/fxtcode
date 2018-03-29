select projectname,othername,a.areaname as subareaname,c1.codename as purposename,address,startdate,usableyear,buildingnum,totalnum,landarea,buildingarea
,c2.codename as buildingtypename,buildingdate,detail from @projecttable p with(nolock)
left join dbo.sys_area a with(nolock)
on p.areaid = a.areaid 
left join dbo.sys_code c1 with(nolock)
on c1.code = p.purposecode
left join dbo.sys_code c2 with(nolock)
on c2.code = p.buildingtypecode
where 1=1 and p.projectid not in (select projectid from @projecttable_sub ps with(nolock) where p.ProjectId=ps.ProjectId
and 
','+cast((select ShowCompanyId from dbo.privi_company_showdata with(nolock) where FxtCompanyId=@fxtcompanyid and Cityid=@cityid) as varchar)+',' like '%,' + cast(ps.Fxt_CompanyId as varchar) + ',%'
) and
','+cast((select ShowCompanyId from dbo.privi_company_showdata with(nolock) where FxtCompanyId=@fxtcompanyid and Cityid=@cityid) as varchar)+',' like '%,' + cast(p.FxtCompanyId as varchar) + ',%' and p.projectid = @projectid
union
select projectname,othername,a.areaname as subareaname,c1.codename as purposename,address,startdate,usableyear,buildingnum,totalnum,landarea,buildingarea
,c2.codename as buildingtypename,buildingdate,detail from @projecttable_sub p with(nolock)
left join dbo.sys_area a with(nolock)
on p.areaid = a.areaid 
left join dbo.sys_code c1 with(nolock)
on c1.code = p.purposecode
left join dbo.sys_code c2 with(nolock)
on c2.code = p.buildingtypecode
where 1=1 and p.projectid = @projectid and p.valid =1 
and 
','+cast((select ShowCompanyId from dbo.privi_company_showdata with(nolock) where FxtCompanyId=@fxtcompanyid and Cityid=@cityid) as varchar)+',' like '%,' + cast(p.Fxt_CompanyId as varchar) + ',%'