declare @maxunitprice decimal(18,2);
declare @minunitprice decimal(18,2);
declare @avgprice decimal(18,0);

select 
	@maxunitprice = MAX(UnitPrice),
	@minunitprice = MIN(UnitPrice),
	@avgprice = SUM(BuildingArea * UnitPrice)/SUM(BuildingArea)
from 
(
	select 	
	c.casetypecode,
	c.buildingarea,
	c.unitprice
	from (
		select projectid
		from @projecttable p with(nolock)
		where not exists(
			select projectid from @projectsubtable ps with(nolock)
			where ps.projectid = p.projectid
			and ps.cityid = @cityid
			and ps.fxt_companyid = @fxtcompanyid
		)
		and p.valid = 1
		and p.cityid = @cityid
		and p.fxtcompanyid in (select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata where cityid = @cityid and fxtcompanyid = @fxtcompanyid and typecode = @typecode), ','))
		$projectlimit
		union 
		select projectid
		from @projecttable_sub p with(nolock)
		where p.valid = 1
		and p.cityid = @cityid
		and p.fxt_companyid = @fxtcompanyid
		$projectlimit
	)t inner join (
		select 
		ProjectId,
		buildingarea,
		unitprice,
		totalprice,
		casetypecode
		from @casetable c with(nolock)
		where c.valid = 1
		and c.cityid = @cityid
		and c.fxtcompanyid in (select value from fxtproject.dbo.splittotable((select casecompanyid from fxtdatacenter.dbo.privi_company_showdata where cityid = @cityid and fxtcompanyid = @fxtcompanyid and typecode = @typecode), ','))
		and c.casedate between @casedatefrom and @casedateto
		and  BuildingArea > 0 
		and UnitPrice > 0
		$caselimit
	)c on t.projectid = c.projectid
)tb

select @maxunitprice as maxunitprice,@minunitprice as minunitprice,@avgprice as avgprice;