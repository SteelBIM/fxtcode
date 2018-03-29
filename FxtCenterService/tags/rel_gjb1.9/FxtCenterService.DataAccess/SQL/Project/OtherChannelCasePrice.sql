declare @tbl_sourcenamelist table(SourceName varchar(100))--来源
insert into @tbl_sourcenamelist(SourceName)
select top 4 SourceName from (
--优先 搜房、安居客、新浪、房价网
select SourceName,(case when (SourceName like '%搜房%' or SourceName='%安居客%' or SourceName='%新浪%' or SourceName='%房价网%') then 1 else 0 end) as orderid
from @casetable c with(nolock)
where Valid=1 and isnull(c.SourceName,'') <> '' and c.projectId=@projectid and c.cityid=@cityid and c.unitprice>0 and c.buildingarea>0
and ','+cast((select ShowCompanyId from dbo.privi_company_showdata with(nolock) where FxtCompanyId=@fxtcompanyid and Cityid=@cityid) as varchar)+',' like '%,' + cast(fxtcompanyid as varchar) + ',%'
and c.purposecode=@purposecode and c.buildingtypecode=@buildingtypecode
and (case 
when(@buildingareatype = 8006005 and (c.buildingarea > 120)) then 1
when(@buildingareatype = 8006004 and (c.buildingarea > 90 and c.buildingarea <= 120)) then 1
when(@buildingareatype = 8006003 and (c.buildingarea > 60 and c.buildingarea <= 90)) then 1
when(@buildingareatype = 8006002 and (c.buildingarea > 30 and c.buildingarea <= 60)) then 1
when(@buildingareatype = 8006001 and (c.buildingarea <= 30)) then 1
else 0
end)=1
and casedate>=@startdate and casedate<@enddate
group by SourceName
having(count(caseid)) >= 3
) temp order by orderid desc

select convert(int,sum(unitprice*buildingarea)/sum(buildingarea)) avgprice,c.projectid,c.SourceName
 from @casetable c with(nolock)
where Valid=1 and c.projectId=@projectid and c.cityid=@cityid and c.unitprice>0 and c.buildingarea>0
and ','+cast((select ShowCompanyId from dbo.privi_company_showdata with(nolock) where FxtCompanyId=@fxtcompanyid and Cityid=@cityid) as varchar)+',' like '%,' + cast(fxtcompanyid as varchar) + ',%'
and c.purposecode=@purposecode and c.buildingtypecode=@buildingtypecode
and (case 
when(@buildingareatype = 8006005 and (c.buildingarea > 120)) then 1
when(@buildingareatype = 8006004 and (c.buildingarea > 90 and c.buildingarea <= 120)) then 1
when(@buildingareatype = 8006003 and (c.buildingarea > 60 and c.buildingarea <= 90)) then 1
when(@buildingareatype = 8006002 and (c.buildingarea > 30 and c.buildingarea <= 60)) then 1
when(@buildingareatype = 8006001 and (c.buildingarea <= 30)) then 1
else 0
end)=1
and casedate>=@startdate and casedate<@enddate
and SourceName in (select SourceName from @tbl_sourcenamelist)
group by ProjectId,SourceName