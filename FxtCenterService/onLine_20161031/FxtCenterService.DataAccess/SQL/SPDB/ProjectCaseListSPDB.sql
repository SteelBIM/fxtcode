select
	tb.CityID,
	t1.zipcode as CityZipcode,
	t1.CityName,
	ISNULL(tb.AreaID,0) as AreaID,
	t2.AreaName,
	ISNULL(tb.SubAreaId,0) as SubAreaId,
	t3.SubAreaName,
	tb.ProjectId,
	tb.ProjectName,
	ISNULL(tb.BuildingArea,0) as BuildingArea,
	ISNULL(t4.Code,-1) as BuildingTypeCode,
	t4.CodeName as BuildingTypeCodeName,
	ISNULL(t5.Code,-1) as FrontCode,
	t5.CodeName as FrontCodeName,
	tb.ZhuangXiu,
	ISNULL(tb.TotalFloor,0) as TotalFloor,
	ISNULL(t6.Code,-1) as HouseTypeCode,
	t6.CodeName as HouseTypeCodeName,
	tb.CaseDate,
	ISNULL(tb.TotalPrice,0) as TotalPrice,
	ISNULL(tb.UnitPrice,0) as UnitPrice,
	tb.SourceName,
	ISNULL(tb.FloorNumber,0) as FloorNumber,
	ISNULL(tb.RemainYear,0) as RemainYear,
	tb.Address,
	tb.PurposeCode,
	t7.CodeName as PurposeCodeName,
	tb.SaleDate,
	ISNULL(tb.AveragePrice,0) as AveragePrice,
	CAST(DateName(YEAR,tb.EndDate) AS nvarchar(10))+'年' EndDate,
	ISNULL(tb.ManagerPrice,0) as ManagerPrice,
	ISNULL(tb.CubageRate,0) as CubageRate,
	ISNULL(tb.GreenRate,0) as GreenRate,
	ISNULL(tb.x,0) as X,
	ISNULL(tb.y,0) as Y
from 
(
	select top 5
		p.cityid,p.areaid,p.subareaid,p.projectid,p.projectname,p.address,p.saledate,p.averageprice,p.managerprice,p.cubagerate,p.greenrate,p.x,p.y,p.remainyear,
		buildingarea,buildingtypecode,frontcode,zhuangxiu,floornumber,totalfloor,housetypecode,casedate,totalprice,unitprice,purposecode,sourcename
		,buildingname,houseno,casetypecode,p.distance,p.enddate
	from DAT_ProjectAround p with(nolock)
	inner join 
	(
		select
			cityid,projectid,buildingarea,frontcode,zhuangxiu,totalfloor,housetypecode,casedate,totalprice,unitprice,sourcename,floornumber,
			remainyear,purposecode,buildingname,houseno,casetypecode,buildingdate,
			case when buildingtypecode > 0 then buildingtypecode 
			else 
				case when totalfloor between 1 and 3 then 2003001
				when totalfloor between 4 and 7 then 2003002
				when totalfloor between 8 and 12 then 2003003
				when totalfloor >= 13 then 2003004
				else 0 
				end 
			end as buildingtypecode
		from @table_case with(nolock)
		where 1 = 1
			and valid = 1
			and cityid = @cityid
			and ProjectId in (
				select ProjectId from DAT_ProjectAround with(nolock) where CityID=@cityid and askProjectId=@projectid
			)
			and fxtcompanyid in (
				select value from fxtproject.dbo.splittotable((select casecompanyid from fxtdatacenter.dbo.privi_company_showdata where cityid = @cityid and fxtcompanyid = @fxtcompanyid and typecode = @typecode),',')
			)
			and casedate between @datebegin and @dateend
			and buildingarea between @buildingarea - 20  and @buildingarea + 20
			and casetypecode in (3001001,3001003,3001004) 
			and PurposeCode in (1002001)
	)c on p.projectid = c.projectid 
	where 1 = 1 and p.CityID=@cityid and p.askProjectId=@projectid
	order by 
		distance asc,casedate asc,
		(case when casetypecode = 3001002 then 1
		when casetypecode = 3001005 then 2
		when casetypecode = 3001003 then 3
		when casetypecode = 3001004 then 4
		when casetypecode = 3001001 then 5
		else 6 end) asc
) tb
left join fxtdatacenter.dbo.sys_city t1 on tb.cityid = t1.cityid
left join fxtdatacenter.dbo.sys_area t2 on tb.areaid = t2.areaid
left join fxtdatacenter.dbo.sys_subarea t3 on tb.subareaid = t3.subareaid
left join fxtdatacenter.dbo.sys_code t4 on tb.buildingtypecode = t4.code
left join fxtdatacenter.dbo.sys_code t5 on tb.frontcode = t5.code
left join fxtdatacenter.dbo.sys_code t6 on tb.housetypecode = t6.code
left join fxtdatacenter.dbo.sys_code t7 on tb.purposecode = t7.code
