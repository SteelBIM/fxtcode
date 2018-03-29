select 
	cl.CaseID
	,cl.AreaName
	,(select top 1 SubAreaName from FxtDataCenter.dbo.SYS_SubArea_Office with(nolock) where SubAreaId = cl.SubAreaId) as SubAreaName
	,cl.LandNo
	,cl.CaseDate
	,c.CodeName as BargainTypeCodeName
	,cl.BuildUnitPrice
	,cl.LandUnitPrice
	,cl.LandArea
	,cl.BuildingArea
	,c6.codename as LandPurposeCodeName
	,cl.StartUsableDate
	,cl.EndDate
	,cl.WinDate
	,c1.CodeName as BargainStateCodeName
	,cl.DealDate
	,c2.CodeName as DevelopDegreeCodeName
	,cl.UsableYear
	,cl.LandAddress
	,cl.CubageRate
	,cl.GreenRage
	,cl.CoverRate
	,c3.CodeName as LandClassName
	,c4.CodeName as LandShapeCodeName
	,l.Traffic
	,l.Infrastructure
	,l.PublicService
	,c5.CodeName as EnvironmentCodeName
	,l.LandDetail
	,l.East
	,l.West
	,l.South
	,l.North
	,cl.LandUseStatus
	,cl.PlanLimited
	,l.BusinessCenterDistance
	,cl.SourceName
	,cl.SourceLink
	,cl.DealTotalPrice
	,cl.MinBargainPrice
	,cl.LandPurposeDesc
	,cl.BargainDate
	,c7.CodeName as LandSourceCodeName
	,cl.ArrangeStartDate
	,cl.ArrangeEndDate
from FxtLand.dbo.DAT_CaseLand cl with(nolock)
left join (
	select * from FxtLand.dbo.DAT_Land l with(nolock)
	where not exists (
		select LandId from FxtLand.dbo.DAT_Land_sub ls with(nolock)
		where ls.LandId = l.LandId
		and ls.CityID = l.CityID
		and ls.FxtCompanyId = @fxtcompanyid
	)
	and l.Valid = 1
	and l.CityID = @cityid
	and l.FxtCompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT LandCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId=@cityid AND FxtCompanyId=@fxtcompanyid and TypeCode = @typecode),','))
	union 
	select * from FxtLand.dbo.DAT_Land_sub l with(nolock)
	where l.Valid = 1
	and l.CityID = @cityid
	and l.FxtCompanyId = @fxtcompanyid
)l on cl.LandNo = l.LandNo
left join FxtDataCenter.dbo.SYS_Code c with(nolock) on cl.BargainTypeCode = c.Code
left join FxtDataCenter.dbo.SYS_Code c1 with(nolock) on cl.BargainStateCode = c1.Code
left join FxtDataCenter.dbo.SYS_Code c2 with(nolock) on cl.DevelopDegreeCode = c2.Code
left join FxtDataCenter.dbo.SYS_Code c3 with(nolock) on cl.LandClass = c3.Code
left join FxtDataCenter.dbo.SYS_Code c4 with(nolock) on l.LandShapeCode = c4.Code
left join FxtDataCenter.dbo.SYS_Code c5 with(nolock) on l.EnvironmentCode = c5.Code
left join FxtDataCenter.dbo.SYS_Code c6 with(nolock) on cl.LandPurposeCode = c6.Code
left join FxtDataCenter.dbo.SYS_Code c7 with(nolock) on cl.LandSourceCode = c7.Code
where cl.Valid = 1
and cl.CityID = @cityid
and cl.FXTCompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT LandCaseCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId=@cityid AND FxtCompanyId=@fxtcompanyid and TypeCode = @typecode),','))