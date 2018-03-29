select projectname,othername,a.areaname as subareaname,c1.codename as purposename,address,startdate,usableyear,buildingnum,totalnum,landarea,buildingarea,p.x,p.y
,c2.codename as buildingtypename,buildingdate,enddate,detail from @projecttable p with(nolock)
left join FxtDataCenter.dbo.SYS_Area a with(nolock)
on p.areaid = a.areaid 
left join FxtDataCenter.dbo.SYS_Code c1 with(nolock)
on c1.code = p.purposecode
left join FxtDataCenter.dbo.SYS_Code c2 with(nolock)
on c2.code = p.buildingtypecode
where 1=1 and p.projectid not in (select projectid from @projecttable_sub ps with(nolock) where p.ProjectId=ps.ProjectId
and 
','+cast((select ShowCompanyId from FxtDataCenter.dbo.Privi_Company_ShowData with(nolock) where FxtCompanyId=@fxtcompanyid and Cityid=@cityid and TypeCode = @typecode) as varchar)+',' like '%,' + cast(ps.Fxt_CompanyId as varchar) + ',%'
) and
','+cast((select ShowCompanyId from FxtDataCenter.dbo.Privi_Company_ShowData with(nolock) where FxtCompanyId=@fxtcompanyid and Cityid=@cityid and TypeCode = @typecode) as varchar)+',' like '%,' + cast(p.FxtCompanyId as varchar) + ',%' and p.projectid = @projectid
union
select projectname,othername,a.areaname as subareaname,c1.codename as purposename,address,startdate,usableyear,buildingnum,totalnum,landarea,buildingarea,p.x,p.y
,c2.codename as buildingtypename,buildingdate,enddate,detail from @projecttable_sub p with(nolock)
left join FxtDataCenter.dbo.SYS_Area a with(nolock)
on p.areaid = a.areaid 
left join FxtDataCenter.dbo.SYS_Code c1 with(nolock)
on c1.code = p.purposecode
left join FxtDataCenter.dbo.SYS_Code c2 with(nolock)
on c2.code = p.buildingtypecode
where 1=1 and p.projectid = @projectid and p.valid =1 
and 
','+cast((select ShowCompanyId from FxtDataCenter.dbo.Privi_Company_ShowData with(nolock) where FxtCompanyId=@fxtcompanyid and Cityid=@cityid and TypeCode = @typecode) as varchar)+',' like '%,' + cast(p.Fxt_CompanyId as varchar) + ',%'