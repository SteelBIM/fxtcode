select 
	T.CityId
	,T.AreaId
	,ISNULL(a.AreaName,'全市') as AreaName
	,(Case when a.X IS null and AreaName IS null then (select top(1) X from FxtDataCenter.dbo.SYS_City where CityId = @cityid) else a.X end) as X
	,(Case when a.Y IS null and AreaName IS null then (select top(1) Y from FxtDataCenter.dbo.SYS_City where CityId = @cityid) else a.Y end) as Y
	,AvgPriceDate
	,AvgPrice
	,case when oldmonthAvgPrice is null or oldmonthAvgPrice <= 0 then 0 else CONVERT(numeric(18,2),(CONVERT(numeric(18,0),AvgPrice) - CONVERT(numeric(18,0),oldmonthAvgPrice) ) / CONVERT(numeric(18,0),oldmonthAvgPrice) * 100) end as linkratio
	,case when oldyearAvgPrice is null or oldyearAvgPrice <= 0 then 0 else CONVERT(numeric(18,2),(CONVERT(numeric(18,0),AvgPrice) - CONVERT(numeric(18,0),oldyearAvgPrice) ) / CONVERT(numeric(18,0),oldyearAvgPrice) * 100) end as yearbasis
from (
	select a.*,b.AvgPrice as oldmonthAvgPrice,c.AvgPrice as oldyearAvgPrice from (		
		select * from DAT_AvgPrice_Month
		where CityId = @cityid
		and AvgPriceDate between @datefrom and @dateto
		and SubAreaId = 0
		and ProjectId = 0
	)a
	left join (	
		select * from DAT_AvgPrice_Month
		where CityId = @cityid
		and AvgPriceDate between DATEADD(MM,-1,@datefrom) and DATEADD(MM,-1,@dateto)
		and SubAreaId = 0
		and ProjectId = 0
	)b on a.AvgPriceDate = DATEADD(mm,1,b.AvgPriceDate) and a.AreaId = b.AreaId
	left join (	
		select * from DAT_AvgPrice_Month
		where CityId = @cityid
		and AvgPriceDate between DATEADD(YY,-1,@datefrom) and DATEADD(YY,-1,@dateto)
		and SubAreaId = 0
		and ProjectId = 0
	)c on a.AvgPriceDate = DATEADD(YY,1,c.AvgPriceDate) and a.AreaId = c.AreaId
)T
left join FxtDataCenter.dbo.SYS_Area a with(nolock) on T.AreaId = a.AreaId
order by AvgPriceDate