select @top p.cityid,p.projectid,p.projectname,p.areaid,p.subareaid,p.address,p.isevalue,p.usableyear,a.areaname,ad.avgprice,p.buildingnum as buildingtotal,p.totalnum as housetotal,p.x,p.y  from @table_project p with(nolock)
left join sys_area a with(nolock) on a.areaid = p.areaid
left join (select avd.avgprice,avd.projectid from dbo.dat_avgprice_day  avd join
(select max(avgpricedate) maxdate,projectid from dbo.dat_avgprice_day where cityid = @cityid and buildingareatype = 1005000  group by projectid ) a
on avd.avgpricedate = a.maxdate and avd.projectid = a.projectid and avd.buildingareatype = 1005000 and avd.cityid =@cityid ) ad
on ad.projectid = p.projectid  
where 1=1 and p.cityid = @cityid and p.projectid not in (select ps.projectid from @table_project_sub ps where ps.projectid = p.projectid and ps.fxt_companyid = @fxtcompanyid and ps.cityid = p.cityid) and valid =1
and (','+cast((select showcompanyid from dbo.privi_company_showdata with(nolock) where fxtcompanyid=@fxtcompanyid and cityid = @cityid) as varchar)+',' like '%,' + cast(p.fxtcompanyid as varchar) + ',%') @where
union
select @top p.cityid,p.projectid,p.projectname,p.areaid,p.subareaid,p.address,p.isevalue,p.usableyear,a.areaname,ad.avgprice,p.buildingnum as buildingtotal,p.totalnum as housetotal,p.x,p.y   from @table_project_sub p with(nolock)
left join sys_area a with(nolock) on a.areaid = p.areaid
left join (select avd.avgprice,avd.projectid from dbo.dat_avgprice_day  avd join
(select max(avgpricedate) maxdate,projectid from dbo.dat_avgprice_day where cityid = @cityid and buildingareatype = 1005000  group by projectid ) a
on avd.avgpricedate = a.maxdate and avd.projectid = a.projectid and avd.buildingareatype = 1005000 and avd.cityid =@cityid ) ad
on ad.projectid = p.projectid 
where 1=1 and p.cityid = @cityid and p.fxt_companyid = @fxtcompanyid and valid =1 @where