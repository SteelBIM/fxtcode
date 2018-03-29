select a.Id, CityId, FxtCompanyId, AreaId, SubAreaId, ProjectId, AvgPriceDate, AvgPrice, BuildingAreaType, PurposeType, BuildingTypeCode, CreateTime, JSFS
,careat.codename as buildingareatypename,cpurpose.codename as purposetypename,cbuildt.codename as buildingtypecodename
from DAT_ProjectAvgPrice a with(nolock)
left join sys_code careat with(nolock) on careat.code=a.buildingareatype
left join sys_code cpurpose with(nolock) on cpurpose.code=a.purposetype
left join sys_code cbuildt with(nolock) on cbuildt.code=a.buildingtypecode
where a.id in (
select max(id) from DAT_ProjectAvgPrice with(nolock)
where 1=1 and CityId=@cityid and projectid=@projectid 
and AvgPriceDate>=@startdate and AvgPriceDate<=@enddate 
and (','+cast((select showcompanyid from dbo.privi_company_showdata pc with(nolock) where pc.FxtCompanyId=@fxtcompanyid and pc.cityid = @cityid) as varchar)+',' like '%,' + cast(a.FxtCompanyId as varchar) + ',%')
#where#
group by buildingareatype,purposetype,buildingtypecode,projectid
)