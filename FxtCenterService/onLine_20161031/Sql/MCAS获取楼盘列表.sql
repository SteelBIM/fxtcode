use FXTProject

declare @cityid int
declare @fxtcompanyid int
declare @buildingid int
declare @typecode int
declare @strKey nvarchar(20)
declare @param nvarchar(20)

set @cityid=6
set @fxtcompanyid=25
set @typecode=1003036
set @strKey='%'
set @param='%%%'


select  top 15 t.cityid
	,t.projectid
	,projectname
	,othername
	,areaid
	,subareaid
	,address
	,isevalue
	,usableyear
	,(
		select areaname
		from fxtdatacenter.dbo.sys_area a with (nolock)
		where a.areaid = t.areaid
		) as areaname
	,buildingnum as buildingtotal
	,totalnum as housetotal
	,(case when x = 0 or x is null then (select top 1 x from dbo.DAT_Project p where p.fxtcompanyid = 25 and p.projectid = t.projectid and p.cityid = t.cityid) else x end) x
	,(case when y = 0 or y is null then (select top 1 y from dbo.DAT_Project p where p.fxtcompanyid = 25 and p.projectid = t.projectid and p.cityid = t.cityid) else y end) y
	--,x
	--,y
	,photo.photocnt
	, 0 as avgprice
	--,wp.avgprice
	,areazipcode=(select zipcode from fxtdatacenter.dbo.sys_area with(nolock) where cityid=t.cityid and areaid=t.areaid)
from (
	select cityid
		,projectid
		,projectname
		,othername
		,areaid
		,subareaid
		,address
		,isevalue
		,usableyear
		,buildingnum
		,totalnum
		,x
		,y
		,pinyin
		,pinyinall
	from dbo.DAT_Project p with (nolock)
	where 1 = 1
		and p.cityid = @cityid
		and p.projectid not in (
			select ps.projectid
			from dbo.DAT_Project_sub ps
			where ps.projectid = p.projectid
				and ps.fxt_companyid = @fxtcompanyid
				and ps.cityid = p.cityid
			)
		and valid = 1
		and (
			',' + cast((
					select showcompanyid
					from fxtdatacenter.dbo.privi_company_showdata with (nolock)
					where fxtcompanyid = @fxtcompanyid
						and cityid = @cityid
						and typecode = @typecode
					) as varchar) + ',' like '%,' + cast(p.fxtcompanyid as varchar) + ',%'
			)
	
	union
	
	select cityid
		,projectid
		,projectname
		,othername
		,areaid
		,subareaid
		,address
		,isevalue
		,usableyear
		,buildingnum
		,totalnum
		,x
		,y
		,pinyin
		,pinyinall
	from dbo.DAT_Project_sub p with (nolock)
	where 1 = 1
		and p.cityid = @cityid
		and p.fxt_companyid = @fxtcompanyid
		and valid = 1
	) t
left join (
	select projectid
		,cityid
		,count(*) as photocnt
	from (
		select id
			,projectid
			,cityid
		from lnk_p_photo p with (nolock)
		where 1 = 1
			and not exists (
				select id
				from lnk_p_photo_sub ps with (nolock)
				where ps.id = p.id
					and ps.cityid = @cityid
					and ps.fxtcompanyid = @fxtcompanyid
				)
			and p.valid = 1
			and p.cityid = @cityid
			and p.fxtcompanyid in (
				select value
				from fxtproject.dbo.splittotable((
							select showcompanyid
							from fxtdatacenter.dbo.privi_company_showdata
							where cityid = @cityid
								and fxtcompanyid = @fxtcompanyid
								and typecode = @typecode
							), ',')
				)
			and p.phototypecode like '2009%'
		
		union
		
		select id
			,projectid
			,cityid
		from lnk_p_photo_sub p with (nolock)
		where 1 = 1
			and p.valid = 1
			and p.cityid = @cityid
			and p.fxtcompanyid = @fxtcompanyid
			and p.phototypecode like '2009%'
		) t
	group by projectid
		,cityid
	) photo on t.projectid = photo.projectid
	and t.cityid = photo.cityid
--left join (
--	select wp.projectid
--		,(
--			case 
--			when wp.projectavgprice > 0
--			then wp.projectavgprice
--			else (select isnull(ap.projectavgprice,0) from dbo.DAT_ProjectAvg ap with (nolock) where ap.projectid = wp.projectid and fxtcompanyid = 25 and valid = 1 and usemonth = convert(nvarchar(7),dateadd(mm,-1,getdate()),121) + '-01' ) 
--			end
--			) as avgprice
--	from dbo.DAT_WeightProject wp with (nolock) 
--	where wp.fxtcompanyid = 25
--	) wp
--	on t.projectid = wp.projectid 
where 1 = 1
	and (
		[projectname] like @param
		or [othername] like @param
		or [pinyin] like @param
		or [pinyinall] like @param
		or [address] like @param
		)
	
	
	--@pricewhere
order by --@priceorderby 
(case when [projectname] like @strkey then 0 else 1 end) asc,projectid desc



