--周边同质楼盘均价对比
declare @fivemiles numeric(18, 14),@max_x numeric(18, 14),@min_x numeric(18, 14),@max_y numeric(18, 14),@min_y numeric(18, 14)
declare @x numeric(18, 14),@y numeric(18, 14)
declare @tbl_prolist table(projectid int,projectname varchar(100),cityid int,distince decimal(18,2),projectx decimal(18, 14),projecty decimal(18, 14))--符合条件的楼盘
declare @tbl_caselist table(projectid int,projectname varchar(100),avgprice int,preavgprice int,changepercent decimal(18,2),projectx decimal(18, 14),projecty decimal(18, 14))--查询结果
--通过物业坐标定位
set @fivemiles=0.03 --3公里偏差
set @max_x = @@x + @fivemiles 
set @min_x = @@x - @fivemiles 
set @max_y = @@y + @fivemiles
set @min_y = @@y - @fivemiles
set @x = @@x
set @y =  @@y
--物业未定位，获取楼盘坐标
if( @@x <= 0 or  @@y <= 0)
	begin
		select @max_x = (isnull(x,0) + @fivemiles) ,@min_x= (isnull(x,0) - @fivemiles) ,@max_y = (isnull(y,0) + @fivemiles),@min_y = (isnull(y,0) - @fivemiles)
		from ( 
		select p.x,p.y from @projecttable p with(nolock)
		where 1=1 and p.projectid=@projectid
		and p.projectid not in (select projectid from @projectsubtable ps with(nolock) where p.ProjectId=ps.ProjectId 
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
	
	--查询周边3公里范围内，案例数大于等于3条的楼盘
	insert into @tbl_prolist (projectid,projectname,cityid,distince,projectx,projecty)
	select projectid,projectname,cityid,distince,x,y from ( 
	select p.projectid,p.projectname,p.cityid,p.x,p.y
	, (2 * 6378.137 * ASIN(SQRT(power(sin( (@@y*pi()/180-p.y*pi()/180)/2),2)+cos(@@y*pi()/180)*cos(p.y*pi()/180)* power(sin( (@@x*pi()/180-p.x*pi()/180)/2),2)))*1000) as distince from (
	select p.projectid,p.projectname,p.cityid,p.x,p.y from @projecttable p with(nolock)
	where 1=1 
	and p.x >= @min_x and p.x <= @max_x and p.y >= @min_y and p.y <= @max_y --边为5公里的正方形之内
	and p.projectid not in (select projectid from @projectsubtable ps with(nolock) where p.ProjectId=ps.ProjectId 
	and 
	','+cast((select ShowCompanyId from dbo.privi_company_showdata with(nolock) where FxtCompanyId=@fxtcompanyid and Cityid=@cityid) as varchar)+',' like '%,' + cast(ps.Fxt_CompanyId as varchar) + ',%'
	) and
	','+cast((select ShowCompanyId from dbo.privi_company_showdata with(nolock) where FxtCompanyId=@fxtcompanyid and Cityid=@cityid) as varchar)+',' like '%,' + cast(p.FxtCompanyId as varchar) + ',%'
	union
	select p.projectid,p.projectname,p.cityid,p.x,p.y from @projectsubtable p with(nolock)
	where 1=1 
	and p.x >= @min_x and p.x <= @max_x and p.y >= @min_y and p.y <= @max_y --边为3公里的正方形之内	
	and p.valid =1 
	and 
	','+cast((select ShowCompanyId from dbo.privi_company_showdata with(nolock) where FxtCompanyId=@fxtcompanyid and Cityid=@cityid) as varchar)+',' like '%,' + cast(p.Fxt_CompanyId as varchar) + ',%'
	) p
	) temp 
	where  projectid<>@projectid and distince <= 3000 --3公里范围内
	and (select count(1) from @casetable c with(nolock)
	where c.projectid=temp.projectid and c.cityid=temp.cityid and c.purposecode=1002001 
	--传递了建筑类型则过滤建筑类型
	and (case when ((@buildingtypecode=0) or (@buildingtypecode>0 and c.buildingtypecode=@buildingtypecode)) then 1 else 0 end)=1
	and casedate>=@startdate and casedate<=@enddate
	)>=3 --案例数大于等于3条
	order by distince asc

	if(select count(*) from @tbl_prolist)>0
		begin	
			--获取最近的5个楼盘价格
			insert into @tbl_caselist(projectid,avgprice)	
			--3个月之间，按距离获取前5个符合条件楼盘,建筑类型一样条件、竣工时间与楼盘时间偏差3年的案例均价			
			select top 5 c.projectid,convert(int,sum(unitprice*buildingarea)/sum(buildingarea)) avgprice		
			from @casetable c with(nolock)
			inner join @tbl_prolist t on t.projectid=c.projectid
			where c.purposecode=1002001 and c.unitprice>0 and c.buildingarea>0 and c.cityid=t.cityid
			--传递了建筑类型则过滤建筑类型
			and (case when ((@buildingtypecode=0) or (@buildingtypecode>0 and c.buildingtypecode=@buildingtypecode)) then 1 else 0 end)=1
			and ','+cast((select ShowCompanyId from dbo.privi_company_showdata with(nolock) where fxtcompanyid =@fxtcompanyid and cityid=@cityid) as varchar)+',' like '%,' + cast(fxtcompanyid as varchar) + ',%'
			and casedate>=@startdate and casedate<=@enddate
			and isnumeric (buildingdate)=1 and (REPLACE(buildingdate,'年','')) > @buildingstartdate and (REPLACE(buildingdate,'年','')) < @buildingenddate --竣工时间偏差3年
			group by c.projectid
			order by min(t.distince) asc
			
			if(select count(*) from @tbl_caselist)>0
			begin	
				--更新楼盘坐标、上月楼盘价格
				update t set t.projectname=p.projectname,t.projectx=p.projectx,t.projecty=p.projecty,t.preavgprice=temp.preavgprice
				,t.changepercent=(case when temp.preavgprice > 0 then cast((convert(decimal(18,2),(avgprice-temp.preavgprice))/convert(decimal(18,2),temp.preavgprice))*100 as decimal(18,2)) else 0 end)
				from @tbl_caselist t
				inner join @tbl_prolist p on t.projectid=p.projectid
				left join (
				select c.projectid,convert(int,sum(unitprice*buildingarea)/sum(buildingarea)) preavgprice		
				from @casetable c with(nolock)
				inner join @tbl_caselist t on t.projectid=c.projectid
				inner join @tbl_prolist p on t.projectid=p.projectid
				where c.purposecode=1002001 and c.unitprice>0 and c.buildingarea>0 and c.cityid=p.cityid
				--传递了建筑类型则过滤建筑类型
				and (case when ((@buildingtypecode=0) or (@buildingtypecode>0 and c.buildingtypecode=@buildingtypecode)) then 1 else 0 end)=1
				and ','+cast((select ShowCompanyId from dbo.privi_company_showdata with(nolock) where fxtcompanyid =@fxtcompanyid and cityid=@cityid) as varchar)+',' like '%,' + cast(fxtcompanyid as varchar) + ',%'
				and casedate>=dateadd(month,-1,@startdate) and casedate<=dateadd(month,-1,@enddate)
				and isnumeric (buildingdate)=1 and (REPLACE(buildingdate,'年','')) > @buildingstartdate and (REPLACE(buildingdate,'年','')) < @buildingenddate --竣工时间偏差3年
				group by c.projectid
				)temp on temp.projectid=t.projectid

				--增加当前楼盘(获取坐标)
				insert into @tbl_caselist(projectid,projectx,projecty) values(@projectid,@x,@y)
			end
		end
end

select * from @tbl_caselist




