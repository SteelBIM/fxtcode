declare @cityid int
declare @fxtcompanyid int
declare @typecode int

set @fxtcompanyid=25
set @typecode=1003002

---------------清空表数据---------------
truncate table FXTProject.dbo.DAT_ProjectAround

---------------DAT_Project-----------------
--定义一个游标
declare user_cur cursor for select cityid from FXTProject.dbo.DAT_Project with(nolock) group by CityID
--打开游标
open user_cur
fetch next from user_cur into @cityid

while @@fetch_status=0
begin
--读取游标
	IF (@cityid is not null)
	BEGIN
		insert into FXTProject.FXTProject.dbo.DAT_ProjectAround
		(
			CityID, AskProjectId, ProjectId, ProjectName, AreaID, SubAreaId, Address, SaleDate, AveragePrice, 
			ManagerPrice, CubageRate, GreenRate, X, Y, Distance, RemainYear,EndDate
		)
		select 
			CityID,AskProjectId,ProjectId,ProjectName,isnull(AreaID,0) AreaID,isnull(SubAreaId,0) SubAreaId, Address, SaleDate,isnull(AveragePrice,0) AveragePrice, 
			isnull(ManagerPrice,0) ManagerPrice,isnull(CubageRate,0) CubageRate,isnull(GreenRate,0) GreenRate,isnull(X,0) X,isnull(Y,0) Y, Distance,RemainYear,EndDate
		from
		(
			select row_number() over(partition by CityID,AskProjectId order by Distance) rn , * from 
			(
				----------------请求楼盘不存在xy坐标------------------
				select 
					CityID,ProjectId AskProjectId, ProjectId, ProjectName, AreaID, SubAreaId, Address, SaleDate, AveragePrice, 
					ManagerPrice, CubageRate, GreenRate, X, Y,0 Distance, RemainYear,EndDate
				from
				(
					select 
						CityID,ProjectId AskProjectId, ProjectId, ProjectName, AreaID, SubAreaId, Address, SaleDate, AveragePrice, 
						ManagerPrice, CubageRate, GreenRate, X, Y,0 Distance, 
						case when StartEndDate is not null then DATEDIFF(YEAR,GETDATE(),StartEndDate)
							when StartDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end),StartDate))
							when EndDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,EndDate))
							when SaleDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,SaleDate))
							else 0
						end RemainYear,
						case when EndDate is null AND SaleDate is not null then SaleDate
							else EndDate 
						end EndDate
					from FXTProject.dbo.DAT_Project p with(nolock)
					where not exists(
						select projectid from FXTProject.dbo.DAT_Project_sub ps with(nolock)
						where ps.projectid = p.projectid
							and ps.cityid = @cityid
							and ps.fxt_companyid = @fxtcompanyid

						)
						and valid = 1
						and cityid = @cityid
						and fxtcompanyid in (
							select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata where cityid = @cityid and fxtcompanyid = @fxtcompanyid and typecode = @typecode),',')
						)
					union
					select 
						CityID,ProjectId AskProjectId, ProjectId, ProjectName, AreaID, SubAreaId, Address, SaleDate, AveragePrice, 
						ManagerPrice, CubageRate, GreenRate, X, Y,0 Distance, 
						case when StartEndDate is not null then DATEDIFF(YEAR,GETDATE(),StartEndDate)
							when StartDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end),StartDate))
							when EndDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,EndDate))
							when SaleDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,SaleDate))
						end RemainYear,
						case when EndDate is null AND SaleDate is not null then SaleDate
							else EndDate 
						end EndDate
					from FXTProject.dbo.DAT_Project_sub ps with(nolock)
					where valid = 1
						and cityid = @cityid
						and fxt_companyid = @fxtcompanyid
				)t
				where 1=1 and (X is null or Y is null)
				union all
				----------------周边楼盘必须存在xy坐标------------------
				select
					a.CityID,AskProjectId, ProjectId, b.ProjectName, b.AreaID, b.SubAreaId, Address, SaleDate, AveragePrice, 
					ManagerPrice, CubageRate, GreenRate, X, Y,[fxtdatacenter].[dbo].[fngetdistance](xbase,ybase,x,y) Distance, RemainYear,EndDate
				from 
				(
					--------------------当前楼盘---------------------
					select CityID,AreaID,ProjectId AskProjectId,X xbase,Y ybase
					from FXTProject.dbo.DAT_Project p with(nolock)
					where not exists(
						select projectid from FXTProject.dbo.DAT_Project_sub ps with(nolock)
						where ps.projectid = p.projectid
							and ps.cityid = @cityid
							and ps.fxt_companyid = @fxtcompanyid
						)
						and valid = 1
						and cityid = @cityid
						and fxtcompanyid in (
							select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata where cityid = @cityid and fxtcompanyid = @fxtcompanyid and typecode = @typecode),',')
						)
					union
					select 
						CityID,AreaID,ProjectId AskProjectId,X xbase,Y ybase
					from FXTProject.dbo.DAT_Project_sub ps with(nolock)
					where valid = 1
						and cityid = @cityid
						and fxt_companyid = @fxtcompanyid
				)a
				right join
				(
					--------------------周边楼盘---------------------
					select 
						CityID, ProjectId, ProjectName, AreaID, SubAreaId, Address, SaleDate, AveragePrice, 
						ManagerPrice, CubageRate, GreenRate, X, Y,
						case when StartEndDate is not null then DATEDIFF(YEAR,GETDATE(),StartEndDate)
							when StartDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end),StartDate))
							when EndDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,EndDate))
							when SaleDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,SaleDate))
							else 0
						end RemainYear,
						case when EndDate is null AND SaleDate is not null then SaleDate
							else EndDate 
						end EndDate
					from FXTProject.dbo.DAT_Project p with(nolock)
					where not exists(
							select projectid from FXTProject.dbo.DAT_Project_sub ps with(nolock)
							where ps.projectid = p.projectid
							and ps.cityid = @cityid
							and ps.fxt_companyid = @fxtcompanyid
						)
						and valid = 1
						and cityid = @cityid
						and fxtcompanyid in (
							select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata where cityid = @cityid and fxtcompanyid = @fxtcompanyid and typecode = @typecode),',')
						)
						and x is not null and y is not null
						and 
						(
							StartEndDate is not null or StartDate is not null or EndDate is not null or SaleDate is not null
						)
					union
					select 
						CityID, ProjectId, ProjectName, AreaID, SubAreaId, Address, SaleDate, AveragePrice, 
						ManagerPrice, CubageRate, GreenRate, X, Y,
						case when StartEndDate is not null then DATEDIFF(YEAR,GETDATE(),StartEndDate)
							when StartDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end),StartDate))
							when EndDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,EndDate))
							when SaleDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,SaleDate))
							else 0
						end RemainYear,
						case when EndDate is null AND SaleDate is not null then SaleDate
							else EndDate 
						end EndDate
					from FXTProject.dbo.DAT_Project_sub ps with(nolock)
					where valid = 1
						and cityid = @cityid
						and fxt_companyid = @fxtcompanyid
						and x is not null and y is not null
						and 
						(
							StartEndDate is not null or StartDate is not null or EndDate is not null or SaleDate is not null
						)
				)b
				on a.CityID=b.CityID and a.AreaID=b.AreaID
				where RemainYear>0
			)S
		) T
		where rn <= 10 and Distance <= 5 
	END
	fetch next from user_cur into @cityid
end
close user_cur
--摧毁游标
deallocate user_cur

---------------DAT_Project_csj-----------------
--定义一个游标
declare user_cur cursor for select cityid from FXTProject.dbo.DAT_Project_csj with(nolock) group by CityID
--打开游标
open user_cur
fetch next from user_cur into @cityid
while @@fetch_status=0
begin
--读取游标
	IF (@cityid is not null)
	BEGIN
		insert into FXTProject.FXTProject.dbo.DAT_ProjectAround
		(
			CityID, AskProjectId, ProjectId, ProjectName, AreaID, SubAreaId, Address, SaleDate, AveragePrice, 
			ManagerPrice, CubageRate, GreenRate, X, Y, Distance, RemainYear,EndDate
		)
		select 
			CityID,AskProjectId,ProjectId,ProjectName,isnull(AreaID,0) AreaID,isnull(SubAreaId,0) SubAreaId, Address, SaleDate,isnull(AveragePrice,0) AveragePrice, 
			isnull(ManagerPrice,0) ManagerPrice,isnull(CubageRate,0) CubageRate,isnull(GreenRate,0) GreenRate,isnull(X,0) X,isnull(Y,0) Y, Distance,RemainYear,EndDate
		from
		(
			select row_number() over(partition by CityID,AskProjectId order by Distance) rn , * from 
			(
				----------------请求楼盘不存在xy坐标------------------
				select 
					CityID,ProjectId AskProjectId, ProjectId, ProjectName, AreaID, SubAreaId, Address, SaleDate, AveragePrice, 
					ManagerPrice, CubageRate, GreenRate, X, Y,0 Distance, RemainYear,EndDate
				from
				(
					select 
						CityID,ProjectId AskProjectId, ProjectId, ProjectName, AreaID, SubAreaId, Address, SaleDate, AveragePrice, 
						ManagerPrice, CubageRate, GreenRate, X, Y,0 Distance, 
						case when StartEndDate is not null then DATEDIFF(YEAR,GETDATE(),StartEndDate)
							when StartDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end),StartDate))
							when EndDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,EndDate))
							when SaleDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,SaleDate))
							else 0
						end RemainYear,
						case when EndDate is null AND SaleDate is not null then SaleDate
							else EndDate 
						end EndDate
					from FXTProject.dbo.DAT_Project_csj p with(nolock)
					where not exists(
						select projectid from FXTProject.dbo.DAT_Project_csj_sub ps with(nolock)
						where ps.projectid = p.projectid
							and ps.cityid = @cityid
							and ps.fxt_companyid = @fxtcompanyid

						)
						and valid = 1
						and cityid = @cityid
						and fxtcompanyid in (
							select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata where cityid = @cityid and fxtcompanyid = @fxtcompanyid and typecode = @typecode),',')
						)
					union
					select 
						CityID,ProjectId AskProjectId, ProjectId, ProjectName, AreaID, SubAreaId, Address, SaleDate, AveragePrice, 
						ManagerPrice, CubageRate, GreenRate, X, Y,0 Distance, 
						case when StartEndDate is not null then DATEDIFF(YEAR,GETDATE(),StartEndDate)
							when StartDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end),StartDate))
							when EndDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,EndDate))
							when SaleDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,SaleDate))
						end RemainYear,
						case when EndDate is null AND SaleDate is not null then SaleDate
							else EndDate 
						end EndDate
					from FXTProject.dbo.DAT_Project_csj_sub ps with(nolock)
					where valid = 1
						and cityid = @cityid
						and fxt_companyid = @fxtcompanyid
				)t
				where 1=1 and (X is null or Y is null)
				union all
				----------------周边楼盘必须存在xy坐标------------------
				select
					a.CityID,AskProjectId, ProjectId, b.ProjectName, b.AreaID, b.SubAreaId, Address, SaleDate, AveragePrice, 
					ManagerPrice, CubageRate, GreenRate, X, Y,[fxtdatacenter].[dbo].[fngetdistance](xbase,ybase,x,y) Distance, RemainYear,EndDate
				from 
				(
					--------------------当前楼盘---------------------
					select CityID,AreaID,ProjectId AskProjectId,X xbase,Y ybase
					from FXTProject.dbo.DAT_Project_csj p with(nolock)
					where not exists(
						select projectid from FXTProject.dbo.DAT_Project_csj_sub ps with(nolock)
						where ps.projectid = p.projectid
							and ps.cityid = @cityid
							and ps.fxt_companyid = @fxtcompanyid
						)
						and valid = 1
						and cityid = @cityid
						and fxtcompanyid in (
							select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata where cityid = @cityid and fxtcompanyid = @fxtcompanyid and typecode = @typecode),',')
						)
					union
					select 
						CityID,AreaID,ProjectId AskProjectId,X xbase,Y ybase
					from FXTProject.dbo.DAT_Project_csj_sub ps with(nolock)
					where valid = 1
						and cityid = @cityid
						and fxt_companyid = @fxtcompanyid
				)a
				right join
				(
					--------------------周边楼盘---------------------
					select 
						CityID, ProjectId, ProjectName, AreaID, SubAreaId, Address, SaleDate, AveragePrice, 
						ManagerPrice, CubageRate, GreenRate, X, Y,
						case when StartEndDate is not null then DATEDIFF(YEAR,GETDATE(),StartEndDate)
							when StartDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end),StartDate))
							when EndDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,EndDate))
							when SaleDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,SaleDate))
							else 0
						end RemainYear,
						case when EndDate is null AND SaleDate is not null then SaleDate
							else EndDate 
						end EndDate
					from FXTProject.dbo.DAT_Project_csj p with(nolock)
					where not exists(
							select projectid from FXTProject.dbo.DAT_Project_csj_sub ps with(nolock)
							where ps.projectid = p.projectid
							and ps.cityid = @cityid
							and ps.fxt_companyid = @fxtcompanyid
						)
						and valid = 1
						and cityid = @cityid
						and fxtcompanyid in (
							select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata where cityid = @cityid and fxtcompanyid = @fxtcompanyid and typecode = @typecode),',')
						)
						and x is not null and y is not null
						and 
						(
							StartEndDate is not null or StartDate is not null or EndDate is not null or SaleDate is not null
						)
					union
					select 
						CityID, ProjectId, ProjectName, AreaID, SubAreaId, Address, SaleDate, AveragePrice, 
						ManagerPrice, CubageRate, GreenRate, X, Y,
						case when StartEndDate is not null then DATEDIFF(YEAR,GETDATE(),StartEndDate)
							when StartDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end),StartDate))
							when EndDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,EndDate))
							when SaleDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,SaleDate))
							else 0
						end RemainYear,
						case when EndDate is null AND SaleDate is not null then SaleDate
							else EndDate 
						end EndDate
					from FXTProject.dbo.DAT_Project_csj_sub ps with(nolock)
					where valid = 1
						and cityid = @cityid
						and fxt_companyid = @fxtcompanyid
						and x is not null and y is not null
						and 
						(
							StartEndDate is not null or StartDate is not null or EndDate is not null or SaleDate is not null
						)
				)b
				on a.CityID=b.CityID and a.AreaID=b.AreaID
				where RemainYear>0
			)S
		) T
		where rn <= 10 and Distance <= 5 
	END
	fetch next from user_cur into @cityid
end
close user_cur
--摧毁游标
deallocate user_cur

---------------DAT_Project_hbh-----------------
--定义一个游标
declare user_cur cursor for select cityid from FXTProject.dbo.DAT_Project_hbh with(nolock) group by CityID
--打开游标
open user_cur
fetch next from user_cur into @cityid
while @@fetch_status=0
begin
--读取游标
	IF (@cityid is not null)
	BEGIN
		insert into FXTProject.FXTProject.dbo.DAT_ProjectAround
		(
			CityID, AskProjectId, ProjectId, ProjectName, AreaID, SubAreaId, Address, SaleDate, AveragePrice, 
			ManagerPrice, CubageRate, GreenRate, X, Y, Distance, RemainYear,EndDate
		)
		select 
			CityID,AskProjectId,ProjectId,ProjectName,isnull(AreaID,0) AreaID,isnull(SubAreaId,0) SubAreaId, Address, SaleDate,isnull(AveragePrice,0) AveragePrice, 
			isnull(ManagerPrice,0) ManagerPrice,isnull(CubageRate,0) CubageRate,isnull(GreenRate,0) GreenRate,isnull(X,0) X,isnull(Y,0) Y, Distance,RemainYear,EndDate
		from
		(
			select row_number() over(partition by CityID,AskProjectId order by Distance) rn , * from 
			(
				----------------请求楼盘不存在xy坐标------------------
				select 
					CityID,ProjectId AskProjectId, ProjectId, ProjectName, AreaID, SubAreaId, Address, SaleDate, AveragePrice, 
					ManagerPrice, CubageRate, GreenRate, X, Y,0 Distance, RemainYear,EndDate
				from
				(
					select 
						CityID,ProjectId AskProjectId, ProjectId, ProjectName, AreaID, SubAreaId, Address, SaleDate, AveragePrice, 
						ManagerPrice, CubageRate, GreenRate, X, Y,0 Distance, 
						case when StartEndDate is not null then DATEDIFF(YEAR,GETDATE(),StartEndDate)
							when StartDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end),StartDate))
							when EndDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,EndDate))
							when SaleDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,SaleDate))
							else 0
						end RemainYear,
						case when EndDate is null AND SaleDate is not null then SaleDate
							else EndDate 
						end EndDate
					from FXTProject.dbo.DAT_Project_hbh p with(nolock)
					where not exists(
						select projectid from FXTProject.dbo.DAT_Project_hbh_sub ps with(nolock)
						where ps.projectid = p.projectid
							and ps.cityid = @cityid
							and ps.fxt_companyid = @fxtcompanyid

						)
						and valid = 1
						and cityid = @cityid
						and fxtcompanyid in (
							select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata where cityid = @cityid and fxtcompanyid = @fxtcompanyid and typecode = @typecode),',')
						)
					union
					select 
						CityID,ProjectId AskProjectId, ProjectId, ProjectName, AreaID, SubAreaId, Address, SaleDate, AveragePrice, 
						ManagerPrice, CubageRate, GreenRate, X, Y,0 Distance, 
						case when StartEndDate is not null then DATEDIFF(YEAR,GETDATE(),StartEndDate)
							when StartDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end),StartDate))
							when EndDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,EndDate))
							when SaleDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,SaleDate))
						end RemainYear,
						case when EndDate is null AND SaleDate is not null then SaleDate
							else EndDate 
						end EndDate
					from FXTProject.dbo.DAT_Project_hbh_sub ps with(nolock)
					where valid = 1
						and cityid = @cityid
						and fxt_companyid = @fxtcompanyid
				)t
				where 1=1 and (X is null or Y is null)
				union all
				----------------周边楼盘必须存在xy坐标------------------
				select
					a.CityID,AskProjectId, ProjectId, b.ProjectName, b.AreaID, b.SubAreaId, Address, SaleDate, AveragePrice, 
					ManagerPrice, CubageRate, GreenRate, X, Y,[fxtdatacenter].[dbo].[fngetdistance](xbase,ybase,x,y) Distance, RemainYear,EndDate
				from 
				(
					--------------------当前楼盘---------------------
					select CityID,AreaID,ProjectId AskProjectId,X xbase,Y ybase
					from FXTProject.dbo.DAT_Project_hbh p with(nolock)
					where not exists(
						select projectid from FXTProject.dbo.DAT_Project_hbh_sub ps with(nolock)
						where ps.projectid = p.projectid
							and ps.cityid = @cityid
							and ps.fxt_companyid = @fxtcompanyid
						)
						and valid = 1
						and cityid = @cityid
						and fxtcompanyid in (
							select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata where cityid = @cityid and fxtcompanyid = @fxtcompanyid and typecode = @typecode),',')
						)
					union
					select 
						CityID,AreaID,ProjectId AskProjectId,X xbase,Y ybase
					from FXTProject.dbo.DAT_Project_hbh_sub ps with(nolock)
					where valid = 1
						and cityid = @cityid
						and fxt_companyid = @fxtcompanyid
				)a
				right join
				(
					--------------------周边楼盘---------------------
					select 
						CityID, ProjectId, ProjectName, AreaID, SubAreaId, Address, SaleDate, AveragePrice, 
						ManagerPrice, CubageRate, GreenRate, X, Y,
						case when StartEndDate is not null then DATEDIFF(YEAR,GETDATE(),StartEndDate)
							when StartDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end),StartDate))
							when EndDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,EndDate))
							when SaleDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,SaleDate))
							else 0
						end RemainYear,
						case when EndDate is null AND SaleDate is not null then SaleDate
							else EndDate 
						end EndDate
					from FXTProject.dbo.DAT_Project_hbh p with(nolock)
					where not exists(
							select projectid from FXTProject.dbo.DAT_Project_hbh_sub ps with(nolock)
							where ps.projectid = p.projectid
							and ps.cityid = @cityid
							and ps.fxt_companyid = @fxtcompanyid
						)
						and valid = 1
						and cityid = @cityid
						and fxtcompanyid in (
							select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata where cityid = @cityid and fxtcompanyid = @fxtcompanyid and typecode = @typecode),',')
						)
						and x is not null and y is not null
						and 
						(
							StartEndDate is not null or StartDate is not null or EndDate is not null or SaleDate is not null
						)
					union
					select 
						CityID, ProjectId, ProjectName, AreaID, SubAreaId, Address, SaleDate, AveragePrice, 
						ManagerPrice, CubageRate, GreenRate, X, Y,
						case when StartEndDate is not null then DATEDIFF(YEAR,GETDATE(),StartEndDate)
							when StartDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end),StartDate))
							when EndDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,EndDate))
							when SaleDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,SaleDate))
							else 0
						end RemainYear,
						case when EndDate is null AND SaleDate is not null then SaleDate
							else EndDate 
						end EndDate
					from FXTProject.dbo.DAT_Project_hbh_sub ps with(nolock)
					where valid = 1
						and cityid = @cityid
						and fxt_companyid = @fxtcompanyid
						and x is not null and y is not null
						and 
						(
							StartEndDate is not null or StartDate is not null or EndDate is not null or SaleDate is not null
						)
				)b
				on a.CityID=b.CityID and a.AreaID=b.AreaID
				where RemainYear>0
			)S
		) T
		where rn <= 10 and Distance <= 5 
	END
	fetch next from user_cur into @cityid
end
close user_cur
--摧毁游标
deallocate user_cur

---------------DAT_Project_xb-----------------
--定义一个游标
declare user_cur cursor for select cityid from FXTProject.dbo.DAT_Project_xb with(nolock) group by CityID
--打开游标
open user_cur
fetch next from user_cur into @cityid
while @@fetch_status=0
begin
--读取游标
	IF (@cityid is not null)
	BEGIN
		insert into FXTProject.FXTProject.dbo.DAT_ProjectAround
		(
			CityID, AskProjectId, ProjectId, ProjectName, AreaID, SubAreaId, Address, SaleDate, AveragePrice, 
			ManagerPrice, CubageRate, GreenRate, X, Y, Distance, RemainYear,EndDate
		)
		select 
			CityID,AskProjectId,ProjectId,ProjectName,isnull(AreaID,0) AreaID,isnull(SubAreaId,0) SubAreaId, Address, SaleDate,isnull(AveragePrice,0) AveragePrice, 
			isnull(ManagerPrice,0) ManagerPrice,isnull(CubageRate,0) CubageRate,isnull(GreenRate,0) GreenRate,isnull(X,0) X,isnull(Y,0) Y, Distance,RemainYear,EndDate
		from
		(
			select row_number() over(partition by CityID,AskProjectId order by Distance) rn , * from 
			(
				----------------请求楼盘不存在xy坐标------------------
				select 
					CityID,ProjectId AskProjectId, ProjectId, ProjectName, AreaID, SubAreaId, Address, SaleDate, AveragePrice, 
					ManagerPrice, CubageRate, GreenRate, X, Y,0 Distance, RemainYear,EndDate
				from
				(
					select 
						CityID,ProjectId AskProjectId, ProjectId, ProjectName, AreaID, SubAreaId, Address, SaleDate, AveragePrice, 
						ManagerPrice, CubageRate, GreenRate, X, Y,0 Distance, 
						case when StartEndDate is not null then DATEDIFF(YEAR,GETDATE(),StartEndDate)
							when StartDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end),StartDate))
							when EndDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,EndDate))
							when SaleDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,SaleDate))
							else 0
						end RemainYear,
						case when EndDate is null AND SaleDate is not null then SaleDate
							else EndDate 
						end EndDate
					from FXTProject.dbo.DAT_Project_xb p with(nolock)
					where not exists(
						select projectid from FXTProject.dbo.DAT_Project_xb_sub ps with(nolock)
						where ps.projectid = p.projectid
							and ps.cityid = @cityid
							and ps.fxt_companyid = @fxtcompanyid

						)
						and valid = 1
						and cityid = @cityid
						and fxtcompanyid in (
							select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata where cityid = @cityid and fxtcompanyid = @fxtcompanyid and typecode = @typecode),',')
						)
					union
					select 
						CityID,ProjectId AskProjectId, ProjectId, ProjectName, AreaID, SubAreaId, Address, SaleDate, AveragePrice, 
						ManagerPrice, CubageRate, GreenRate, X, Y,0 Distance, 
						case when StartEndDate is not null then DATEDIFF(YEAR,GETDATE(),StartEndDate)
							when StartDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end),StartDate))
							when EndDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,EndDate))
							when SaleDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,SaleDate))
						end RemainYear,
						case when EndDate is null AND SaleDate is not null then SaleDate
							else EndDate 
						end EndDate
					from FXTProject.dbo.DAT_Project_xb_sub ps with(nolock)
					where valid = 1
						and cityid = @cityid
						and fxt_companyid = @fxtcompanyid
				)t
				where 1=1 and (X is null or Y is null)
				union all
				----------------周边楼盘必须存在xy坐标------------------
				select
					a.CityID,AskProjectId, ProjectId, b.ProjectName, b.AreaID, b.SubAreaId, Address, SaleDate, AveragePrice, 
					ManagerPrice, CubageRate, GreenRate, X, Y,[fxtdatacenter].[dbo].[fngetdistance](xbase,ybase,x,y) Distance, RemainYear,EndDate
				from 
				(
					--------------------当前楼盘---------------------
					select CityID,AreaID,ProjectId AskProjectId,X xbase,Y ybase
					from FXTProject.dbo.DAT_Project_xb p with(nolock)
					where not exists(
						select projectid from FXTProject.dbo.DAT_Project_xb_sub ps with(nolock)
						where ps.projectid = p.projectid
							and ps.cityid = @cityid
							and ps.fxt_companyid = @fxtcompanyid
						)
						and valid = 1
						and cityid = @cityid
						and fxtcompanyid in (
							select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata where cityid = @cityid and fxtcompanyid = @fxtcompanyid and typecode = @typecode),',')
						)
					union
					select 
						CityID,AreaID,ProjectId AskProjectId,X xbase,Y ybase
					from FXTProject.dbo.DAT_Project_xb_sub ps with(nolock)
					where valid = 1
						and cityid = @cityid
						and fxt_companyid = @fxtcompanyid
				)a
				right join
				(
					--------------------周边楼盘---------------------
					select 
						CityID, ProjectId, ProjectName, AreaID, SubAreaId, Address, SaleDate, AveragePrice, 
						ManagerPrice, CubageRate, GreenRate, X, Y,
						case when StartEndDate is not null then DATEDIFF(YEAR,GETDATE(),StartEndDate)
							when StartDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end),StartDate))
							when EndDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,EndDate))
							when SaleDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,SaleDate))
							else 0
						end RemainYear,
						case when EndDate is null AND SaleDate is not null then SaleDate
							else EndDate 
						end EndDate
					from FXTProject.dbo.DAT_Project_xb p with(nolock)
					where not exists(
							select projectid from FXTProject.dbo.DAT_Project_xb_sub ps with(nolock)
							where ps.projectid = p.projectid
							and ps.cityid = @cityid
							and ps.fxt_companyid = @fxtcompanyid
						)
						and valid = 1
						and cityid = @cityid
						and fxtcompanyid in (
							select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata where cityid = @cityid and fxtcompanyid = @fxtcompanyid and typecode = @typecode),',')
						)
						and x is not null and y is not null
						and 
						(
							StartEndDate is not null or StartDate is not null or EndDate is not null or SaleDate is not null
						)
					union
					select 
						CityID, ProjectId, ProjectName, AreaID, SubAreaId, Address, SaleDate, AveragePrice, 
						ManagerPrice, CubageRate, GreenRate, X, Y,
						case when StartEndDate is not null then DATEDIFF(YEAR,GETDATE(),StartEndDate)
							when StartDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end),StartDate))
							when EndDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,EndDate))
							when SaleDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,SaleDate))
							else 0
						end RemainYear,
						case when EndDate is null AND SaleDate is not null then SaleDate
							else EndDate 
						end EndDate
					from FXTProject.dbo.DAT_Project_xb_sub ps with(nolock)
					where valid = 1
						and cityid = @cityid
						and fxt_companyid = @fxtcompanyid
						and x is not null and y is not null
						and 
						(
							StartEndDate is not null or StartDate is not null or EndDate is not null or SaleDate is not null
						)
				)b
				on a.CityID=b.CityID and a.AreaID=b.AreaID
				where RemainYear>0
			)S
		) T
		where rn <= 10 and Distance <= 5 
	END
	fetch next from user_cur into @cityid
end
close user_cur
--摧毁游标
deallocate user_cur

---------------DAT_Project_zb-----------------
--定义一个游标
declare user_cur cursor for select cityid from FXTProject.dbo.DAT_Project_zb with(nolock) group by CityID
--打开游标
open user_cur
fetch next from user_cur into @cityid
while @@fetch_status=0
begin
--读取游标
	IF (@cityid is not null)
	BEGIN
		insert into FXTProject.FXTProject.dbo.DAT_ProjectAround
		(
			CityID, AskProjectId, ProjectId, ProjectName, AreaID, SubAreaId, Address, SaleDate, AveragePrice, 
			ManagerPrice, CubageRate, GreenRate, X, Y, Distance, RemainYear,EndDate
		)
		select 
			CityID,AskProjectId,ProjectId,ProjectName,isnull(AreaID,0) AreaID,isnull(SubAreaId,0) SubAreaId, Address, SaleDate,isnull(AveragePrice,0) AveragePrice, 
			isnull(ManagerPrice,0) ManagerPrice,isnull(CubageRate,0) CubageRate,isnull(GreenRate,0) GreenRate,isnull(X,0) X,isnull(Y,0) Y, Distance,RemainYear,EndDate
		from
		(
			select row_number() over(partition by CityID,AskProjectId order by Distance) rn , * from 
			(
				----------------请求楼盘不存在xy坐标------------------
				select 
					CityID,ProjectId AskProjectId, ProjectId, ProjectName, AreaID, SubAreaId, Address, SaleDate, AveragePrice, 
					ManagerPrice, CubageRate, GreenRate, X, Y,0 Distance, RemainYear,EndDate
				from
				(
					select 
						CityID,ProjectId AskProjectId, ProjectId, ProjectName, AreaID, SubAreaId, Address, SaleDate, AveragePrice, 
						ManagerPrice, CubageRate, GreenRate, X, Y,0 Distance, 
						case when StartEndDate is not null then DATEDIFF(YEAR,GETDATE(),StartEndDate)
							when StartDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end),StartDate))
							when EndDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,EndDate))
							when SaleDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,SaleDate))
							else 0
						end RemainYear,
						case when EndDate is null AND SaleDate is not null then SaleDate
							else EndDate 
						end EndDate
					from FXTProject.dbo.DAT_Project_zb p with(nolock)
					where not exists(
						select projectid from FXTProject.dbo.DAT_Project_zb_sub ps with(nolock)
						where ps.projectid = p.projectid
							and ps.cityid = @cityid
							and ps.fxt_companyid = @fxtcompanyid

						)
						and valid = 1
						and cityid = @cityid
						and fxtcompanyid in (
							select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata where cityid = @cityid and fxtcompanyid = @fxtcompanyid and typecode = @typecode),',')
						)
					union
					select 
						CityID,ProjectId AskProjectId, ProjectId, ProjectName, AreaID, SubAreaId, Address, SaleDate, AveragePrice, 
						ManagerPrice, CubageRate, GreenRate, X, Y,0 Distance, 
						case when StartEndDate is not null then DATEDIFF(YEAR,GETDATE(),StartEndDate)
							when StartDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end),StartDate))
							when EndDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,EndDate))
							when SaleDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,SaleDate))
						end RemainYear,
						case when EndDate is null AND SaleDate is not null then SaleDate
							else EndDate 
						end EndDate
					from FXTProject.dbo.DAT_Project_zb_sub ps with(nolock)
					where valid = 1
						and cityid = @cityid
						and fxt_companyid = @fxtcompanyid
				)t
				where 1=1 and (X is null or Y is null)
				union all
				----------------周边楼盘必须存在xy坐标------------------
				select
					a.CityID,AskProjectId, ProjectId, b.ProjectName, b.AreaID, b.SubAreaId, Address, SaleDate, AveragePrice, 
					ManagerPrice, CubageRate, GreenRate, X, Y,[fxtdatacenter].[dbo].[fngetdistance](xbase,ybase,x,y) Distance, RemainYear,EndDate
				from 
				(
					--------------------当前楼盘---------------------
					select CityID,AreaID,ProjectId AskProjectId,X xbase,Y ybase
					from FXTProject.dbo.DAT_Project_zb p with(nolock)
					where not exists(
						select projectid from FXTProject.dbo.DAT_Project_zb_sub ps with(nolock)
						where ps.projectid = p.projectid
							and ps.cityid = @cityid
							and ps.fxt_companyid = @fxtcompanyid
						)
						and valid = 1
						and cityid = @cityid
						and fxtcompanyid in (
							select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata where cityid = @cityid and fxtcompanyid = @fxtcompanyid and typecode = @typecode),',')
						)
					union
					select 
						CityID,AreaID,ProjectId AskProjectId,X xbase,Y ybase
					from FXTProject.dbo.DAT_Project_zb_sub ps with(nolock)
					where valid = 1
						and cityid = @cityid
						and fxt_companyid = @fxtcompanyid
				)a
				right join
				(
					--------------------周边楼盘---------------------
					select 
						CityID, ProjectId, ProjectName, AreaID, SubAreaId, Address, SaleDate, AveragePrice, 
						ManagerPrice, CubageRate, GreenRate, X, Y,
						case when StartEndDate is not null then DATEDIFF(YEAR,GETDATE(),StartEndDate)
							when StartDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end),StartDate))
							when EndDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,EndDate))
							when SaleDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,SaleDate))
							else 0
						end RemainYear,
						case when EndDate is null AND SaleDate is not null then SaleDate
							else EndDate 
						end EndDate
					from FXTProject.dbo.DAT_Project_zb p with(nolock)
					where not exists(
							select projectid from FXTProject.dbo.DAT_Project_zb_sub ps with(nolock)
							where ps.projectid = p.projectid
							and ps.cityid = @cityid
							and ps.fxt_companyid = @fxtcompanyid
						)
						and valid = 1
						and cityid = @cityid
						and fxtcompanyid in (
							select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata where cityid = @cityid and fxtcompanyid = @fxtcompanyid and typecode = @typecode),',')
						)
						and x is not null and y is not null
						and 
						(
							StartEndDate is not null or StartDate is not null or EndDate is not null or SaleDate is not null
						)
					union
					select 
						CityID, ProjectId, ProjectName, AreaID, SubAreaId, Address, SaleDate, AveragePrice, 
						ManagerPrice, CubageRate, GreenRate, X, Y,
						case when StartEndDate is not null then DATEDIFF(YEAR,GETDATE(),StartEndDate)
							when StartDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end),StartDate))
							when EndDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,EndDate))
							when SaleDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,SaleDate))
							else 0
						end RemainYear,
						case when EndDate is null AND SaleDate is not null then SaleDate
							else EndDate 
						end EndDate
					from FXTProject.dbo.DAT_Project_zb_sub ps with(nolock)
					where valid = 1
						and cityid = @cityid
						and fxt_companyid = @fxtcompanyid
						and x is not null and y is not null
						and 
						(
							StartEndDate is not null or StartDate is not null or EndDate is not null or SaleDate is not null
						)
				)b
				on a.CityID=b.CityID and a.AreaID=b.AreaID
				where RemainYear>0
			)S
		) T
		where rn <= 10 and Distance <= 5 
	END
	fetch next from user_cur into @cityid
end
close user_cur
--摧毁游标
deallocate user_cur

---------------DAT_Project_zsj-----------------
--定义一个游标
declare user_cur cursor for select cityid from FXTProject.dbo.DAT_Project_zsj with(nolock) group by CityID
--打开游标
open user_cur
fetch next from user_cur into @cityid
while @@fetch_status=0
begin
--读取游标
	IF (@cityid is not null)
	BEGIN
		insert into FXTProject.FXTProject.dbo.DAT_ProjectAround
		(
			CityID, AskProjectId, ProjectId, ProjectName, AreaID, SubAreaId, Address, SaleDate, AveragePrice, 
			ManagerPrice, CubageRate, GreenRate, X, Y, Distance, RemainYear,EndDate
		)
		select 
			CityID,AskProjectId,ProjectId,ProjectName,isnull(AreaID,0) AreaID,isnull(SubAreaId,0) SubAreaId, Address, SaleDate,isnull(AveragePrice,0) AveragePrice, 
			isnull(ManagerPrice,0) ManagerPrice,isnull(CubageRate,0) CubageRate,isnull(GreenRate,0) GreenRate,isnull(X,0) X,isnull(Y,0) Y, Distance,RemainYear,EndDate
		from
		(
			select row_number() over(partition by CityID,AskProjectId order by Distance) rn , * from 
			(
				----------------请求楼盘不存在xy坐标------------------
				select 
					CityID,ProjectId AskProjectId, ProjectId, ProjectName, AreaID, SubAreaId, Address, SaleDate, AveragePrice, 
					ManagerPrice, CubageRate, GreenRate, X, Y,0 Distance, RemainYear,EndDate
				from
				(
					select 
						CityID,ProjectId AskProjectId, ProjectId, ProjectName, AreaID, SubAreaId, Address, SaleDate, AveragePrice, 
						ManagerPrice, CubageRate, GreenRate, X, Y,0 Distance, 
						case when StartEndDate is not null then DATEDIFF(YEAR,GETDATE(),StartEndDate)
							when StartDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end),StartDate))
							when EndDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,EndDate))
							when SaleDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,SaleDate))
							else 0
						end RemainYear,
						case when EndDate is null AND SaleDate is not null then SaleDate
							else EndDate 
						end EndDate
					from FXTProject.dbo.DAT_Project_zsj p with(nolock)
					where not exists(
						select projectid from FXTProject.dbo.DAT_Project_zsj_sub ps with(nolock)
						where ps.projectid = p.projectid
							and ps.cityid = @cityid
							and ps.fxt_companyid = @fxtcompanyid

						)
						and valid = 1
						and cityid = @cityid
						and fxtcompanyid in (
							select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata where cityid = @cityid and fxtcompanyid = @fxtcompanyid and typecode = @typecode),',')
						)
					union
					select 
						CityID,ProjectId AskProjectId, ProjectId, ProjectName, AreaID, SubAreaId, Address, SaleDate, AveragePrice, 
						ManagerPrice, CubageRate, GreenRate, X, Y,0 Distance, 
						case when StartEndDate is not null then DATEDIFF(YEAR,GETDATE(),StartEndDate)
							when StartDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end),StartDate))
							when EndDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,EndDate))
							when SaleDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,SaleDate))
						end RemainYear,
						case when EndDate is null AND SaleDate is not null then SaleDate
							else EndDate 
						end EndDate
					from FXTProject.dbo.DAT_Project_zsj_sub ps with(nolock)
					where valid = 1
						and cityid = @cityid
						and fxt_companyid = @fxtcompanyid
				)t
				where 1=1 and (X is null or Y is null)
				union all
				----------------周边楼盘必须存在xy坐标------------------
				select
					a.CityID,AskProjectId, ProjectId, b.ProjectName, b.AreaID, b.SubAreaId, Address, SaleDate, AveragePrice, 
					ManagerPrice, CubageRate, GreenRate, X, Y,[fxtdatacenter].[dbo].[fngetdistance](xbase,ybase,x,y) Distance, RemainYear,EndDate
				from 
				(
					--------------------当前楼盘---------------------
					select CityID,AreaID,ProjectId AskProjectId,X xbase,Y ybase
					from FXTProject.dbo.DAT_Project_zsj p with(nolock)
					where not exists(
						select projectid from FXTProject.dbo.DAT_Project_zsj_sub ps with(nolock)
						where ps.projectid = p.projectid
							and ps.cityid = @cityid
							and ps.fxt_companyid = @fxtcompanyid
						)
						and valid = 1
						and cityid = @cityid
						and fxtcompanyid in (
							select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata where cityid = @cityid and fxtcompanyid = @fxtcompanyid and typecode = @typecode),',')
						)
					union
					select 
						CityID,AreaID,ProjectId AskProjectId,X xbase,Y ybase
					from FXTProject.dbo.DAT_Project_zsj_sub ps with(nolock)
					where valid = 1
						and cityid = @cityid
						and fxt_companyid = @fxtcompanyid
				)a
				right join
				(
					--------------------周边楼盘---------------------
					select 
						CityID, ProjectId, ProjectName, AreaID, SubAreaId, Address, SaleDate, AveragePrice, 
						ManagerPrice, CubageRate, GreenRate, X, Y,
						case when StartEndDate is not null then DATEDIFF(YEAR,GETDATE(),StartEndDate)
							when StartDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end),StartDate))
							when EndDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,EndDate))
							when SaleDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,SaleDate))
							else 0
						end RemainYear,
						case when EndDate is null AND SaleDate is not null then SaleDate
							else EndDate 
						end EndDate
					from FXTProject.dbo.DAT_Project_zsj p with(nolock)
					where not exists(
							select projectid from FXTProject.dbo.DAT_Project_zsj_sub ps with(nolock)
							where ps.projectid = p.projectid
							and ps.cityid = @cityid
							and ps.fxt_companyid = @fxtcompanyid
						)
						and valid = 1
						and cityid = @cityid
						and fxtcompanyid in (
							select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata where cityid = @cityid and fxtcompanyid = @fxtcompanyid and typecode = @typecode),',')
						)
						and x is not null and y is not null
						and 
						(
							StartEndDate is not null or StartDate is not null or EndDate is not null or SaleDate is not null
						)
					union
					select 
						CityID, ProjectId, ProjectName, AreaID, SubAreaId, Address, SaleDate, AveragePrice, 
						ManagerPrice, CubageRate, GreenRate, X, Y,
						case when StartEndDate is not null then DATEDIFF(YEAR,GETDATE(),StartEndDate)
							when StartDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end),StartDate))
							when EndDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,EndDate))
							when SaleDate is not null then DATEDIFF(YEAR,GETDATE(),DATEADD(YEAR,(case when UsableYear is not null and UsableYear>0 and UsableYear<120 then UsableYear else 70 end)-3,SaleDate))
							else 0
						end RemainYear,
						case when EndDate is null AND SaleDate is not null then SaleDate
							else EndDate 
						end EndDate
					from FXTProject.dbo.DAT_Project_zsj_sub ps with(nolock)
					where valid = 1
						and cityid = @cityid
						and fxt_companyid = @fxtcompanyid
						and x is not null and y is not null
						and 
						(
							StartEndDate is not null or StartDate is not null or EndDate is not null or SaleDate is not null
						)
				)b
				on a.CityID=b.CityID and a.AreaID=b.AreaID
				where RemainYear>0
			)S
		) T
		where rn <= 10 and Distance <= 5 
	END
	fetch next from user_cur into @cityid
end
close user_cur
--摧毁游标
deallocate user_cur

