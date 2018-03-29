declare @CityId int
declare @AreaId int
declare @ProjectId int
declare @AvgPrice int
declare @TotalNum int
declare @ProjectAvgPrice int
declare @ProjectRank int
declare @ProjectRankPer money
declare @UserMonth datetime
declare @Address nvarchar(500)

set @UserMonth=DateName(YEAR,DATEADD(Month,-1,GETDATE()))+'-'+DateName(MONTH,DATEADD(Month,-1,GETDATE())) +'-01'  --上月

select top 1 @CityId=a.CityId,@CityName=a.CityName,@AreaId=b.AreaId,@AreaName=b.AreaName,@ProjectId=c.ProjectId,@ProjectName=c.ProjectName,@Address=c.Address
from FxtDataCenter.dbo.SYS_City a with(nolock)
left join FxtDataCenter.dbo.SYS_Area b with(nolock)
on a.CityId=b.CityId
left join @projecttable c with(nolock)
on b.CityId=c.CityID and b.AreaId=c.AreaID
where c.FxtCompanyId=@FxtCompanyId and c.Valid=1 
	and (a.CityName=@CityName or a.CityName=@CityName+'市')  
	and (
		b.AreaName=@AreaName or b.AreaName=@AreaName+'区' or b.AreaName=@AreaName+'市' 
		or b.AreaName=@AreaName+'开发区' or b.AreaName=@AreaName+'县' or b.AreaName=@AreaName+'高新区'
	) 
	and c.ProjectName=@ProjectName

select @TotalNum=COUNT(*) from 
(
	select ProjectId from @projecttable with(nolock) 
	where FxtCompanyId=@FxtCompanyId and CityID=@CityId and Valid=1
	group by ProjectId
)a

select @AvgPrice=AvgPrice from DAT_AvgPrice_Month with(nolock)  
where CityId=@CityId and AreaId=@AreaId and AvgPriceDate = @UserMonth

--查看该城市是否启用基准房价
if exists(select 1 from  FxtDataCenter.dbo.Sys_EvalueSet with(nolock) where FxtCompanyId =@FxtCompanyId and CityId =@CityId and TypeCode =3006001 and valid = 1)
begin
	select
		@ProjectAvgPrice=ProjectAvgPrice,@ProjectRank=ProjectRank,@ProjectRankPer=cast((@TotalNum-ProjectRank+1)*100 as money)/cast(@TotalNum as money)
	from 
	(
		select rank() over(order by ProjectAvgPrice desc)as ProjectRank,*
		from 
		(
			select distinct a.ProjectId,case when c.ProjectAvgPrice is not null then c.ProjectAvgPrice else ISNULL(b.ProjectAvgPrice,0) end ProjectAvgPrice
			from @projecttable a with(nolock) 
			left join @projectavgtable b with(nolock)
			on a.CityID=b.CityId and a.ProjectId=b.ProjectId
			left join @projectweighttable c with(nolock) 
			on a.CityID=c.CityId and a.ProjectId=c.ProjectId
			where a.FxtCompanyId=@FxtCompanyId and a.CityID=@CityId and a.Valid=1
		)t
	)p	
	where ProjectId=@ProjectId	
end
else
begin
	select
		@ProjectAvgPrice=ProjectAvgPrice,@ProjectRank=ProjectRank,@ProjectRankPer=cast((@TotalNum-ProjectRank+1)*100 as money)/cast(@TotalNum as money)
	from 
	(
		select rank()over(order by ProjectAvgPrice desc)as ProjectRank,*
		from 
		(
			select ProjectId,ISNULL(ProjectAvgPrice,0) ProjectAvgPrice
			from @projectavgtable with(nolock)
		)t
	)p	
	where ProjectId=@ProjectId
end

select 
	isnull(@cityid,0) cityid,@cityname cityname, isnull(@areaid,0) areaid,@areaname areaname,isnull(@projectid,0) projectid,@projectname projectname,
	isnull(@projectavgprice,0) projectavgprice,isnull(@projectrank,0) projectrank,cast(isnull(@projectrankper,0) as varchar(10))+'%' projectrankper,
	isnull(@avgprice,0) areaavgprice,isnull(@address,'') address
	

