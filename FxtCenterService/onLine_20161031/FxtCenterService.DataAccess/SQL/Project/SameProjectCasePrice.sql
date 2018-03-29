select * into #temptable from (
	select p.projectid,p.projectname,p.cityid,p.x,p.y,enddate,buildingtypecode
	from @projecttable p with (nolock)
	where 1 = 1
		and not exists(
			select projectid from @projecttable_sub ps with (nolock)
			where p.projectid = ps.projectid
				and cityid = @cityid
				and fxt_companyid = @fxtcompanyid
			)
		and valid = 1
		and cityid = @cityid
		and fxtcompanyid in (select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata where fxtcompanyid = @fxtcompanyid and cityid = @cityid and typecode = @typecode),','))
	union
	select p.projectid,p.projectname,p.cityid,p.x,p.y,enddate,buildingtypecode
	from @projecttable_sub p with (nolock)
	where 1 = 1
		and p.valid = 1
		and cityid = @cityid
		and fxt_companyid = @fxtcompanyid
)t	

--projectid int,projectname varchar(100),avgprice int,preavgprice int,changepercent decimal(18,2),projectx decimal(18, 14),projecty decimal(18, 14))--查询结果
select top(5)
	ProjectId
	,ProjectName
	,projectx
	,projecty
	,ISNULL(weightprojectavgprice,projectavgprice) as preavgprice
	,0 as changepercent
	,0 as avgprice
from (
	select 
		projectid
		,projectname
		,x as projectx
		,y as projecty
		,distance
		,(select top(1) ProjectAvgPrice from @projectavgtable where CityID = @cityid and ProjectId = t.ProjectId and UseMonth = CONVERT(nvarchar(7),DATEADD(MM,-1,GETDATE()),121) + '-01' and valid = 1) as projectavgprice
		,(select top(1) ProjectAvgPrice from @weightproject where CityID = @cityid and ProjectId = t.ProjectId) as weightprojectavgprice
	from(
		select *
		from(
			select
				t.*
				,p.projectid as sampleprojectid
				,p.projectname as sampleprojectname
				,p.buildingtypecode as samplebuildingtypecode
				,p.enddate as sampleenddate
				,p.x as samplex
				,p.y as sampley
				,fxtdatacenter.dbo.fngetdistance(t.x,t.y,p.x,p.y) * 1000 as distance
			from (
				select * from (
					select * from #temptable
					where 1 = 1
					and projectid <> @projectid
					and (case when ((@buildingtypecode = 0) or (@buildingtypecode > 0 and buildingtypecode = @buildingtypecode)) then 1 else 0 end) = 1
					and x > 0 and y > 0
				)t
			)t 
			left join (select * from #temptable where projectid = @projectid)p on t.cityid = p.cityid
		)t
		where distance <= 3000
	)t
)T
where weightprojectavgprice > 0 or projectavgprice > 0
order by distance asc

drop table #temptable