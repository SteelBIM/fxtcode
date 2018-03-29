-----获取楼盘周边楼盘案例
declare @projectid int,@cityid int,@fxtcompanyid int
declare @buildingareatype int,@buildingtypecode int,@areaid int --面积段CODE、建筑类型Code、行政区ID
declare @fivemiles numeric(18, 14),@max_x numeric(18, 14),@min_x numeric(18, 14),@max_y numeric(18, 14),@min_y numeric(18, 14)
declare @tbl_prolist table(projectid int,distince decimal(18,2))--符合条件的楼盘
declare @tbl_caselist table(datatype int ,unitprice decimal(18,2),buildingareatype int,buildingarea decimal(18,2),buildingtypecode int)
set @projectid = @@projectid --楼盘ID
set @cityid =  @@cityid	--城市ID
set @fxtcompanyid =  @@fxtcompanyid	--公司ID
set @buildingareatype = @@buildingareatype	--面积段CODE
set @buildingtypecode = @@buildingtypecode	--建筑类型Code
set @areaid = @@areaid	--行政区ID
--通过物业坐标定位
set @fivemiles=0.05 --5公里偏差
set @max_x = @@x + @fivemiles 
set @min_x = @@x - @fivemiles 
set @max_y = @@y + @fivemiles
set @min_y = @@y - @fivemiles
--物业未定位，获取楼盘坐标
if( @@x <= 0 or  @@y <= 0)
	begin
		select @max_x = (isnull(x,0) + @fivemiles) ,@min_x= (isnull(x,0) - @fivemiles) ,@max_y = (isnull(y,0) + @fivemiles),@min_y = (isnull(y,0) - @fivemiles)
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

	--查询周边5公里范围内的楼盘
	insert into @tbl_prolist (projectid,distince)
	select projectid,distince from ( 
	select p.projectid
	, (2 * 6378.137 * ASIN(SQRT(power(sin( (@@y*pi()/180-p.y*pi()/180)/2),2)+cos(@@y*pi()/180)*cos(p.y*pi()/180)* power(sin( (@@x*pi()/180-p.x*pi()/180)/2),2)))*1000) as distince from (
	select p.projectid,p.x,p.y from @projecttable p with(nolock)
	where 1=1 
	and p.x >= @min_x and p.x <= @max_x and p.y >= @min_y and p.y <= @max_y --边为5公里的正方形之内
	and p.projectid not in (select projectid from @projectsubtable ps with(nolock) where p.ProjectId=ps.ProjectId and ps.valid =1 
	and 
	','+cast((select ShowCompanyId from dbo.privi_company_showdata with(nolock) where FxtCompanyId=@fxtcompanyid and Cityid=@cityid) as varchar)+',' like '%,' + cast(ps.Fxt_CompanyId as varchar) + ',%'
	) and
	','+cast((select ShowCompanyId from dbo.privi_company_showdata with(nolock) where FxtCompanyId=@fxtcompanyid and Cityid=@cityid) as varchar)+',' like '%,' + cast(p.FxtCompanyId as varchar) + ',%'
	union
	select p.projectid,p.x,p.y from @projectsubtable p with(nolock)
	where 1=1 
	and p.x >= @min_x and p.x <= @max_x and p.y >= @min_y and p.y <= @max_y --边为5公里的正方形之内
	and p.valid =1 
	and 
	','+cast((select ShowCompanyId from dbo.privi_company_showdata with(nolock) where FxtCompanyId=@fxtcompanyid and Cityid=@cityid) as varchar)+',' like '%,' + cast(p.Fxt_CompanyId as varchar) + ',%'
	) p
	) temp 
	where projectid<>@projectid and distince <= 5000 --5公里范围内
	order by distince asc

	if(select count(1) from @tbl_prolist)>0
		begin
			--3个月之间，符合条件楼盘,建筑类型、面积段一样条件、竣工时间与楼盘时间偏差3年的10条案例	
			insert into @tbl_caselist(datatype,unitprice,buildingtypecode,buildingarea)
			select top 10 1,c.unitprice,buildingtypecode,buildingarea			
			from @casetable c with(nolock)
			inner join @tbl_prolist t on t.projectid=c.projectid
			where c.purposecode=1002001 and c.buildingtypecode=@buildingtypecode
			and ','+cast((select ShowCompanyId from dbo.privi_company_showdata with(nolock) where FxtCompanyId=@fxtcompanyid and Cityid=@cityid) as varchar)+',' like '%,' + cast(fxtcompanyid as varchar) + ',%'
			and (case 
			when(@buildingareatype = 8006005 and (c.buildingarea > 120)) then 1
			when(@buildingareatype = 8006004 and (c.buildingarea > 90 and c.buildingarea <= 120)) then 1
			when(@buildingareatype = 8006003 and (c.buildingarea > 60 and c.buildingarea <= 90)) then 1
			when(@buildingareatype = 8006002 and (c.buildingarea > 30 and c.buildingarea <= 60)) then 1
			when(@buildingareatype = 8006001 and (c.buildingarea <= 30)) then 1
			else 0
			end)=1
			and casedate >= @startdate and casedate < @enddate	
			and buildingdate > @buildingstartdate and buildingdate < @buildingenddate--竣工时间偏差3年
			order by t.distince asc
		
			if((select count(1) from @tbl_caselist)<10) --无数据
				begin
					--3个月之间,符合条件楼盘,建筑类型一样条件、竣工时间与楼盘时间偏差3年的10条案例	
					insert into @tbl_caselist(datatype,unitprice,buildingtypecode,buildingarea)
					select top 10 2,c.unitprice,buildingtypecode,buildingarea			
					from @casetable c with(nolock)
					inner join @tbl_prolist t on t.projectid=c.projectid
					where c.purposecode=1002001 --普通住宅
					and ','+cast((select ShowCompanyId from dbo.privi_company_showdata with(nolock) where FxtCompanyId=@fxtcompanyid and Cityid=@cityid) as varchar)+',' like '%,' + cast(fxtcompanyid as varchar) + ',%'
					and c.buildingtypecode=@buildingtypecode
					and casedate >= @startdate and casedate < @enddate
					and buildingdate > @buildingstartdate and buildingdate < @buildingenddate--竣工时间偏差3年
					order by t.distince asc
				end
		end

	if(select count(1) from @tbl_caselist)<10 --无数据
		begin
			delete @tbl_prolist where 1=1
			--楼盘所在行政区楼盘
			insert into @tbl_prolist (projectid,distince)		
			select p.projectid,0 from (
			select p.projectid from @projecttable p with(nolock)
			where 1=1 
			and p.areaid=@areaid
			and p.projectid not in (select projectid from @projectsubtable ps with(nolock) where p.ProjectId=ps.ProjectId and ps.valid =1 
			and 
			','+cast((select ShowCompanyId from dbo.privi_company_showdata with(nolock) where FxtCompanyId=@fxtcompanyid and Cityid=@cityid) as varchar)+',' like '%,' + cast(ps.Fxt_CompanyId as varchar) + ',%'
			) and
			','+cast((select ShowCompanyId from dbo.privi_company_showdata with(nolock) where FxtCompanyId=@fxtcompanyid and Cityid=@cityid) as varchar)+',' like '%,' + cast(p.FxtCompanyId as varchar) + ',%'
			union
			select p.projectid from @projectsubtable p with(nolock)
			where 1=1 
			and p.areaid=@areaid
			and p.valid =1 
			and 
			','+cast((select ShowCompanyId from dbo.privi_company_showdata with(nolock) where FxtCompanyId=@fxtcompanyid and Cityid=@cityid) as varchar)+',' like '%,' + cast(p.Fxt_CompanyId as varchar) + ',%'
			) p where projectid<>@projectid

			if(select count(1) from @tbl_prolist)>0
				begin
					--3个月之间，符合条件楼盘,建筑类型、面积段一样条件、竣工时间与楼盘时间偏差3年的10条案例
					insert into @tbl_caselist(datatype,unitprice,buildingtypecode,buildingarea)
					select top 10 3,c.unitprice,buildingtypecode,buildingarea			
					from @casetable c with(nolock)
					inner join @tbl_prolist t on t.projectid=c.projectid
					where c.purposecode=1002001 and c.buildingtypecode=@buildingtypecode
					and ','+cast((select ShowCompanyId from dbo.privi_company_showdata with(nolock) where FxtCompanyId=@fxtcompanyid and Cityid=@cityid) as varchar)+',' like '%,' + cast(fxtcompanyid as varchar) + ',%'
					and (case 
					when(@buildingareatype = 8006005 and (c.buildingarea > 120)) then 1
					when(@buildingareatype = 8006004 and (c.buildingarea > 90 and c.buildingarea <= 120)) then 1
					when(@buildingareatype = 8006003 and (c.buildingarea > 60 and c.buildingarea <= 90)) then 1
					when(@buildingareatype = 8006002 and (c.buildingarea > 30 and c.buildingarea <= 60)) then 1
					when(@buildingareatype = 8006001 and (c.buildingarea <= 30)) then 1
					else 0
					end)=1
					and casedate >=@startdate  and casedate < @enddate
					and buildingdate > @buildingstartdate and buildingdate < @buildingenddate--竣工时间偏差3年
					order by t.distince asc
				end
		end

	--更新面积段CODE
	update @tbl_caselist set buildingareatype=(case 
	when(buildingarea > 120) then 8006005
	when(buildingarea > 90 and buildingarea <= 120) then 8006004
	when(buildingarea > 60 and buildingarea <= 90) then 8006003
	when(buildingarea > 30 and buildingarea <= 60) then 8006002
	when(buildingarea <= 30) then 8006001		
	end)
end

select * from @tbl_caselist --数据