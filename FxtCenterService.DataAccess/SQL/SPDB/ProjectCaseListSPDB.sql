declare @areaid int;
declare @x numeric(18,14);
declare @y numeric(18,14);
declare @tb_project table(
cityid int,
areaid int,
subareaid int,
projectid int,
projectname nvarchar(80),
address nvarchar(1000),
saledate datetime,
averageprice decimal(18,4),
managerprice nvarchar(50),
cubagerate numeric(18, 2),
greenrate numeric(18, 2),
x numeric(18, 14),
y numeric(18, 14),
distance float);
declare @selfCaseCount int;

select @areaid = AreaID,@x = X,@y = Y from(
	select 
		ProjectId,AreaID,X,Y
	from @projecttable p with(nolock)
	where not exists(
		select ProjectId from @projecttable_sub ps with(nolock)
		where ps.ProjectId = p.ProjectId
		and ps.CityID = @cityid
		and ps.Fxt_CompanyId = @fxtcompanyid
	)
	and Valid = 1
	and CityID = @cityid
	and FxtCompanyId in (
		select value from fxtproject.dbo.SplitToTable((select ShowCompanyId from FxtDataCenter.dbo.Privi_Company_ShowData where CityID = @cityid and FxtCompanyId = @fxtcompanyid and TypeCode = @typecode),',')
	)
	and ProjectId = @projectid
	union
	select 
		ProjectId,AreaID,X,Y
	from @projecttable_sub ps with(nolock)
	where Valid = 1
	and CityID = @cityid
	and Fxt_CompanyId = @fxtcompanyid
	and ProjectId = @projectid
)p 
select @selfCaseCount = count(*) from @table_case c where c.Valid = 1 and c.ProjectId = @projectid
and c.CityID = @cityid
and c.FXTCompanyId in (
	select value from fxtproject.dbo.SplitToTable((select CaseCompanyId from FxtDataCenter.dbo.Privi_Company_ShowData where CityID = @cityid and FxtCompanyId = @fxtcompanyid and TypeCode = @typecode),',')
)
and c.CaseDate between @datebegin and @dateend
and c.BuildingArea between @buildingarea - 20  and @buildingarea + 20
and c.CaseTypeCode in ($casetypecodelimit) $casetypecodeappend
$purposecodelimit $floornolimit $housenolimit $buildingtypecodelimit $housetypecode;
if (@x is null or @y is null or @selfCaseCount>=5)
begin
	insert into @tb_project
	select 
	    cityid,areaid,subareaid,projectid,projectname,address,saledate,averageprice,managerprice,cubagerate,greenrate,x,y,null
	from dbo.DAT_Project p with(nolock)
	where not exists(
		select projectid from dbo.DAT_Project_sub ps with(nolock)
		where ps.projectid = p.projectid
		and ps.cityid = @cityid
		and ps.fxt_companyid = @fxtcompanyid
	)
	and valid = 1
	and cityid = @cityid
	and fxtcompanyid in (
		select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata where cityid = @cityid and fxtcompanyid = @fxtcompanyid and typecode = @typecode),',')
	)
	and ProjectId = @projectid
	union
	select 
	    cityid,areaid,subareaid,projectid,projectname,address,saledate,averageprice,managerprice,cubagerate,greenrate,x,y,null
	from dbo.DAT_Project_sub ps with(nolock)
	where valid = 1
	and cityid = @cityid
	and fxt_companyid = @fxtcompanyid
	and ProjectId = @projectid
end
else 
begin
	insert into @tb_project
	select 
	    cityid,areaid,subareaid,projectid,projectname,address,saledate,averageprice,managerprice,cubagerate,greenrate,x,y,[fxtdatacenter].[dbo].[fngetdistance](x,y,@x,@y)
	from dbo.DAT_Project p with(nolock)
	where not exists(
		select projectid from dbo.DAT_Project_sub ps with(nolock)
		where ps.projectid = p.projectid
		and ps.cityid = @cityid
		and ps.fxt_companyid = @fxtcompanyid
	)
	and valid = 1
	and cityid = @cityid
	and fxtcompanyid in (
		select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata where cityid = @cityid and fxtcompanyid = @fxtcompanyid and typecode = @typecode),',')
	)
	and AreaID = @areaid and X is not null and y is not null
	union
	select 
		cityid,areaid,subareaid,projectid,projectname,address,saledate,averageprice,managerprice,cubagerate,greenrate,x,y,[fxtdatacenter].[dbo].[fngetdistance](x,y,@x,@y)
	from dbo.DAT_Project_sub ps with(nolock)
	where valid = 1
	and cityid = @cityid
	and fxt_companyid = @fxtcompanyid
	and AreaID = @areaid and X is not null and y is not null
end;
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
tb.BuildingDate as EndDate,
ISNULL(tb.ManagerPrice,0) as ManagerPrice,
ISNULL(tb.CubageRate,0) as CubageRate,
ISNULL(tb.GreenRate,0) as GreenRate,
ISNULL(tb.x,0) as X,
ISNULL(tb.y,0) as Y
from 
(
	select top 5
		p.*,buildingarea,frontcode,zhuangxiu,totalfloor,housetypecode,casedate,totalprice,unitprice,sourcename,floornumber,remainyear,purposecode
		,buildingname,houseno,casetypecode,buildingdate
		,(case when buildingtypecode > 0 then buildingtypecode else case when totalfloor between 1 and 3 then 2003001
		when totalfloor between 4 and 7 then 2003002
		when totalfloor between 8 and 12 then 2003003
		when totalfloor >= 13 then 2003004
		else 0 end end) as buildingtypecode,case when p.projectid = @projectid then null else [fxtdatacenter].[dbo].[fngetdistance](p.x,p.y,@x,@y) end as distance
	from @tb_project p inner join @table_case c on p.projectid = c.ProjectId
	where c.Valid = 1
	and c.CityID = @cityid
	and c.FXTCompanyId in (
		select value from fxtproject.dbo.SplitToTable((select CaseCompanyId from FxtDataCenter.dbo.Privi_Company_ShowData where CityID = @cityid and FxtCompanyId = @fxtcompanyid and TypeCode = @typecode),',')
	)
	and c.CaseDate between @datebegin and @dateend
	and c.BuildingArea between @buildingarea - 20  and @buildingarea + 20
	and c.CaseTypeCode in ($casetypecodelimit) $casetypecodeappend
	$purposecodelimit $floornolimit $housenolimit $buildingtypecodelimit $housetypecode
	order by distance asc
	,CaseDate asc
	,(case when CaseTypeCode = 3001002 then 1
		when CaseTypeCode = 3001005 then 2
		when CaseTypeCode = 3001003 then 3
		when CaseTypeCode = 3001004 then 4
		when CaseTypeCode = 3001001 then 5
		else 6 end) asc
) tb
left join FxtDataCenter.dbo.SYS_City t1 on tb.CityID = t1.CityId
left join FxtDataCenter.dbo.SYS_Area t2 on tb.AreaID = t2.AreaId
left join FxtDataCenter.dbo.SYS_SubArea t3 on tb.SubAreaId = t3.SubAreaId
left join FxtDataCenter.dbo.SYS_Code t4 on tb.BuildingTypeCode = t4.Code
left join FxtDataCenter.dbo.SYS_Code t5 on tb.FrontCode = t5.Code
left join FxtDataCenter.dbo.SYS_Code t6 on tb.HouseTypeCode = t6.Code
left join FxtDataCenter.dbo.SYS_Code t7 on tb.PurposeCode = t7.Code