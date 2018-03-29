--周边楼盘价格、环比涨跌幅
declare @fivemiles numeric(18, 14),@max_x numeric(18, 14),@min_x numeric(18, 14),@max_y numeric(18, 14),@min_y numeric(18, 14)
declare @x numeric(18, 14),@y numeric(18, 14)
declare @tbl_prolist table(projectid int,projectname varchar(100),distince decimal(18,2),projectx decimal(18, 14),projecty decimal(18, 14))--符合条件的楼盘
set @fivemiles = 0.05 --5公里偏差
set @x = @@x
set @y =  @@y


--通过物业坐标定位
set @fivemiles=0.05 --5公里偏差
set @max_x = @@x + @fivemiles 
set @min_x = @@x - @fivemiles 
set @max_y = @@y + @fivemiles
set @min_y = @@y - @fivemiles
--物业未定位，获取楼盘坐标
if( @x <= 0 or  @y <= 0)
	begin
		select @x=ISNULL(x,0),@y=ISNULL(y,0), @max_x = (isnull(x,0) + @fivemiles) ,@min_x= (isnull(x,0) - @fivemiles) ,@max_y = (isnull(y,0) + @fivemiles),@min_y = (isnull(y,0) - @fivemiles)
		from ( 
		select p.x,p.y from @projecttable p with(nolock)
		where 1=1 and p.projectid=@projectid
		and p.projectid not in (select projectid from @projectsubtable ps with(nolock) where p.ProjectId=ps.ProjectId and ps.valid =1 
		and 
		','+cast((select ShowCompanyId from dbo.privi_company_showdata with(nolock) where FxtCompanyId=@fxtcompanyid and Cityid=@cityid) as varchar)+',' like '%,' + cast(ps.Fxt_CompanyId as varchar) + ',%'
		) and
		','+cast((select ShowCompanyId from dbo.privi_company_showdata with(nolock) where FxtCompanyId=@fxtcompanyid and Cityid=@cityid) as varchar)+',' like '%,' + cast(p.FxtCompanyId as varchar) + ',%'
		union
		select p.x,p.y from @projectsubtable p with(nolock)
		where 1=1 and p.projectid=@projectid
		and p.valid =1 
		and 
		','+cast((select ShowCompanyId from dbo.privi_company_showdata with(nolock) where FxtCompanyId=@fxtcompanyid and Cityid=@cityid) as varchar)+',' like '%,' + cast(p.Fxt_CompanyId as varchar) + ',%'
		) temp
	end

if (@max_x > 0 and @min_x > 0 and @max_y> 0 and @min_y > 0)
	begin
	
	insert into @tbl_prolist (projectid,projectname,distince,projectx,projecty)	values (@projectid,@projectname,0,@x,@y)--包括自己

	--查询周边5公里范围内楼盘
	insert into @tbl_prolist (projectid,projectname,distince,projectx,projecty)
	select projectid,projectname,distince,x,y from ( 
	select p.projectid,p.projectname,p.x,p.y
	, (2 * 6378.137 * ASIN(SQRT(power(sin( (@@y*pi()/180-p.y*pi()/180)/2),2)+cos(@@y*pi()/180)*cos(p.y*pi()/180)* power(sin( (@@x*pi()/180-p.x*pi()/180)/2),2)))*1000) as distince from (
	select p.projectid,p.projectname,p.x,p.y from @projecttable p with(nolock)
	where 1=1 
	and p.x >= @min_x and p.x <= @max_x and p.y >= @min_y and p.y <= @max_y --边为5公里的正方形之内
	and p.projectid not in (select projectid from @projectsubtable ps with(nolock) where p.ProjectId=ps.ProjectId and ps.valid =1 
	and 
	','+cast((select ShowCompanyId from dbo.privi_company_showdata with(nolock) where FxtCompanyId=@fxtcompanyid and Cityid=@cityid) as varchar)+',' like '%,' + cast(ps.Fxt_CompanyId as varchar) + ',%'
	) and
	','+cast((select ShowCompanyId from dbo.privi_company_showdata with(nolock) where FxtCompanyId=@fxtcompanyid and Cityid=@cityid) as varchar)+',' like '%,' + cast(p.FxtCompanyId as varchar) + ',%'
	union
	select p.projectid,p.projectname,p.x,p.y from @projectsubtable p with(nolock)
	where 1=1 
	and p.x >= @min_x and p.x <= @max_x and p.y >= @min_y and p.y <= @max_y --边为5公里的正方形之内	
	and p.valid =1 
	and 
	','+cast((select ShowCompanyId from dbo.privi_company_showdata with(nolock) where FxtCompanyId=@fxtcompanyid and Cityid=@cityid) as varchar)+',' like '%,' + cast(p.Fxt_CompanyId as varchar) + ',%'
	) p
	) temp 
	where distince <= 5000 --5公里范围内（包括自己）
	order by distince asc

	if(select count(*) from @tbl_prolist)>0
		begin
			--楼盘均价、环比涨跌幅
			select projectid,projectname,avgprice,preavgprice,projectx,projecty
			,(case when preavgprice > 0 then cast((convert(decimal(18,2),(avgprice-preavgprice))/convert(decimal(18,2),preavgprice))*100 as decimal(18,2)) else 0 end) as changepercent			
			from (		
			select p.projectid,p.projectname,p.projectx,p.projecty,avgprice
			--上月均价
			,(select top 1 avgprice from dbo.DAT_ProjectAvgPrice pre with(nolock) where pre.projectid=t.projectid and pre.avgpricedate = @preavgdate and t.PurposeType=@purposetype and ISNULL(t.BuildingAreaType,0)=0 and ISNULL(t.BuildingTypeCode,0)=0 and daterange=@daterange order by pre.id desc) as preavgprice
			from dbo.DAT_ProjectAvgPrice t with(nolock)
			inner join @tbl_prolist p on p.projectid = t.projectid
			inner join (
			select t.projectid,max(id) maxid
			from dbo.DAT_ProjectAvgPrice t with(nolock)
			inner join @tbl_prolist p on p.projectid = t.projectid
			where t.avgpricedate = @avgdate and t.PurposeType=@purposetype and ISNULL(t.BuildingAreaType,0)=0 and ISNULL(t.BuildingTypeCode,0)=0 and daterange=@daterange
			group by t.projectid
			) temp on temp.projectid=t.projectid and temp.maxid=t.id			
			) t
		end
end
