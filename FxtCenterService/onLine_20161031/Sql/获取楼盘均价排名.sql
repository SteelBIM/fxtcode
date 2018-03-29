declare @FxtCompanyId int
declare @CityName nvarchar(500)
declare @AreaName nvarchar(500)
declare @ProjectName nvarchar(500)

set @FxtCompanyId=25
set @CityName='深圳'
set @AreaName='南山'
set @ProjectName='fiug'

declare @cityid int
declare @areaid int
declare @projectid int
declare @avgprice int
declare @totalnum int
declare @projectavgprice int
declare @projectrank int
declare @projectrankper money
declare @usermonth datetime
declare @address nvarchar(500)

set @usermonth=datename(year,dateadd(month,-1,getdate()))+'-'+datename(month,dateadd(month,-1,getdate())) +'-01'  --上月

select top 1 @cityid=a.cityid,@cityname=a.cityname,@areaid=b.areaid,@areaname=b.areaname,@projectid=c.projectid,@projectname=c.projectname,@address=c.address
from fxtdatacenter.dbo.sys_city a with(nolock)
left join fxtdatacenter.dbo.sys_area b with(nolock)
on a.cityid=b.cityid
left join dbo.DAT_Project c with(nolock)
on b.cityid=c.cityid and b.areaid=c.areaid
where c.fxtcompanyid=@fxtcompanyid and c.valid=1 
	and (a.cityname=@cityname or a.cityname=@cityname+'市') 
	and (
		b.areaname=@areaname or b.areaname=@areaname+'区' or b.areaname=@areaname+'市' 
		or b.areaname=@areaname+'开发区' or b.areaname=@areaname+'县' or b.areaname=@areaname+'高新区'
	) 
	and c.projectname=@projectname

select @totalnum=count(*) from 
(
	select projectid from dbo.DAT_Project with(nolock) 
	where fxtcompanyid=@fxtcompanyid and cityid=@cityid and valid=1
	group by projectid
)a

select @avgprice=avgprice from dat_avgprice_month with(nolock)  
where cityid=@cityid and areaid=@areaid and avgpricedate = @usermonth

--查看该城市是否启用基准房价
if exists(select 1 from  fxtdatacenter.dbo.sys_evalueset with(nolock) where fxtcompanyid =@fxtcompanyid and cityid =@cityid and typecode =3006001 and valid = 1)
begin
	select
		@projectavgprice=projectavgprice,@projectrank=projectrank,@projectrankper=cast((@totalnum-projectrank)*100 as money)/cast(@totalnum as money)
	from 
	(
		select rank() over(order by projectavgprice desc)as projectrank,*
		from 
		(
			select distinct a.projectid,case when c.projectavgprice is not null then c.projectavgprice else isnull(b.projectavgprice,0) end projectavgprice
			from dbo.DAT_Project a with(nolock) 
			left join dbo.DAT_ProjectAvg b with(nolock)
			on a.cityid=b.cityid and a.projectid=b.projectid
			left join dbo.DAT_WeightProject c with(nolock) 
			on a.cityid=c.cityid and a.projectid=c.projectid
			where a.fxtcompanyid=@fxtcompanyid and a.cityid=@cityid and a.valid=1
		)t
	)p	
	where projectid=@projectid	
end
else
begin
	select
		@projectavgprice=projectavgprice,@projectrank=projectrank,@projectrankper=cast((@totalnum-projectrank)*100 as money)/cast(@totalnum as money)
	from 
	(
		select row_number()over(order by projectavgprice desc)as projectrank,*
		from 
		(
			select projectid,isnull(projectavgprice,0) projectavgprice
			from dbo.DAT_ProjectAvg with(nolock)
		)t
	)p	
	where projectid=@projectid
end

select 
	isnull(@cityid,0) cityid,@cityname cityname, isnull(@areaid,0) areaid,@areaname areaname,isnull(@projectid,0) projectid,@projectname projectname,
	isnull(@projectavgprice,0) projectavgprice,isnull(@projectrank,0) projectrank,cast(isnull(@projectrankper,0) as varchar(10))+'%' projectrankper,
	isnull(@avgprice,0) areaavgprice,isnull(@address,'') address
	




