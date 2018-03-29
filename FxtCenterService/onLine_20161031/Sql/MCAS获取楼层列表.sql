use FXTProject

declare @cityid int
declare @fxtcompanyid int
declare @buildingid int
declare @typecode int
declare @strKey nvarchar(20)
declare @param nvarchar(20)

set @cityid=2
set @fxtcompanyid=25
set @buildingid=632294
set @typecode=1003036
set @strKey='%'
set @param='%%%'

declare @tb table(floorno int)
declare @tb1 table(floorno int,housecnt int)
insert into @tb 
select distinct floorno
from dbo.DAT_House_csj h with (nolock)
where cityid = @cityid
	and buildingid = @buildingid
	and valid = 1		
	and h.houseid not in (
		select houseid
		from dbo.DAT_House_csj_sub hs with (nolock)
		where h.houseid = hs.houseid
			and hs.fxtcompanyid = @fxtcompanyid
			--and hs.fxtcompanyid in (select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata where cityid = @cityid and fxtcompanyid = @fxtcompanyid and typecode = @typecode), ',') where value <> 25)
			and hs.cityid = h.cityid
		) 
	and h.fxtcompanyid in (
		select value
		from dbo.splittotable((
					select showcompanyid
					from fxtdatacenter.dbo.privi_company_showdata with (nolock)
					where cityid = @cityid
						and fxtcompanyid = @fxtcompanyid
						and typecode = @typecode
					), ',')
		)	
union	
select distinct floorno
from dbo.DAT_House_csj_sub h with (nolock)
where cityid = @cityid
	and buildingid = @buildingid
	and valid = 1
	and h.fxtcompanyid = @fxtcompanyid
	--and h.fxtcompanyid in (select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata where cityid = @cityid and fxtcompanyid = @fxtcompanyid and typecode = @typecode), ',') where value <> 25)

insert into @tb1
select floorno
	--,nominalfloor
	,count(houseid) as housecnt
from (
	select floorno
		--,nominalfloor
		,houseid
	from dbo.DAT_House_csj h with (nolock)
	where  cityid = @cityid 
		and buildingid = @buildingid
		and valid = 1
		and housename <> ''
		and h.houseid not in (
			select houseid
			from dbo.DAT_House_csj_sub hs with (nolock)
			where h.houseid = hs.houseid
				and hs.fxtcompanyid = @fxtcompanyid
				--and hs.fxtcompanyid in (select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata where cityid = @cityid and fxtcompanyid = @fxtcompanyid and typecode = @typecode), ',') where value <> 25)
				and hs.cityid = h.cityid
			)
		and h.fxtcompanyid in (
			select value
			from dbo.splittotable((
						select showcompanyid
						from fxtdatacenter.dbo.privi_company_showdata with (nolock)
						where cityid = @cityid
							and fxtcompanyid = @fxtcompanyid
							and typecode = @typecode
						), ',')
			)		
	union		
	select floorno
		--,nominalfloor
		,houseid
	from dbo.DAT_House_csj_sub h with (nolock)
	where cityid = @cityid
		and buildingid = @buildingid
		and valid = 1
		and housename <> ''
		and h.fxtcompanyid = @fxtcompanyid
		--and h.fxtcompanyid in (select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata where cityid = @cityid and fxtcompanyid = @fxtcompanyid and typecode = @typecode), ',') where value <> 25)
	) h
group by floorno
--,nominalfloor

select a.* 
--,c.nominalfloor
,isnull(c.housecnt, 0) as housecnt
from @tb a
left join @tb1 c 
on c.floorno = a.floorno
where 1 = 1
 order by  (case when a.FloorNo like @strKey then 0 else 1 end) asc ,floorno