select convert(int,(sum(c.unitprice)/count(c.projectid))) avgprice,c.projectid,c.SourceName
--云估价均价
,(select top 1 AvgPrice from dbo.DAT_ProjectAvgPrice
where ProjectId=@projectid and BuildingAreaType=@buildingareatype and PurposeType=@purposecode and BuildingTypeCode=@buildingtypecode and AvgPriceDate>=@avgstartdate and AvgPrice <=@avgenddate and daterange=@daterange) as ygjavgprice
 from @casetable c with(nolock)
where Valid=1 and c.projectId=@projectid
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
and SourceName in (
select top 4 SourceName from (
--优先 搜房、安居客、新浪、房价网
select SourceName,(case when (SourceName like '%搜房%' or SourceName='%安居客%' or SourceName='%新浪%' or SourceName='%房价网%') then 1 else 0 end) as orderid
from @casetable c with(nolock)
where Valid=1 and isnull(c.SourceName,'') <> '' and c.projectId=@projectid
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
)
group by ProjectId,SourceName