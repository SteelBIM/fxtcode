select a.Id, CityId, FxtCompanyId, AreaId, SubAreaId, ProjectId, AvgPriceDate, AvgPrice, BuildingAreaType, PurposeType, BuildingTypeCode, CreateTime, JSFS
,careat.codename as buildingareatypename,cpurpose.codename as purposetypename,cbuildt.codename as buildingtypecodename
from DAT_ProjectAvgPrice a with(nolock)
left join FxtDataCenter.dbo.SYS_Code careat with(nolock) on careat.code=a.buildingareatype
left join FxtDataCenter.dbo.SYS_Code cpurpose with(nolock) on cpurpose.code=a.purposetype
left join FxtDataCenter.dbo.SYS_Code cbuildt with(nolock) on cbuildt.code=a.buildingtypecode
where a.id in (
select max(id) from DAT_ProjectAvgPrice with(nolock)
where 1=1 and CityId=@cityid and projectid=@projectid 
and AvgPriceDate>=@startdate and AvgPriceDate<=@enddate 
and (','+cast((select showcompanyid from FxtDataCenter.dbo.Privi_Company_ShowData pc with(nolock) where pc.FxtCompanyId=@fxtcompanyid and pc.cityid = @cityid and pc.TypeCode = @typecode) as varchar)+',' like '%,' + cast(a.FxtCompanyId as varchar) + ',%')
#where#
group by buildingareatype,purposetype,buildingtypecode,projectid
)