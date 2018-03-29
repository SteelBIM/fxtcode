select a.*,dpc.ChineseName as developcompanyname,pmc.ChineseName as managercompanyname from (
select [projectid],[projectname] ,a.[areaname],p.isevalue,p.ParkingNumber,p.ManagerPrice,p.address,a.areaid,p.enddate,p.X,p.Y  from @table_project p with(nolock),FxtDataCenter.dbo.SYS_Area a with(nolock)where p.[valid]=1 and p.[cityid]=@cityid and p.[areaid]=a.[areaid] 
and p.projectid = @projectid
and p.projectid not in (select projectid from @table_project_sub ps with(nolock) where p.projectid=ps.projectid and ps.fxt_companyid=@fxtcompanyid and ps.cityid=p.cityid) and p.fxtcompanyid  in (select value from  dbo.splittotable((select showcompanyid from FxtDataCenter.dbo.Privi_Company_ShowData where cityid=@cityid and fxtcompanyid=@fxtcompanyid and TypeCode = @typecode),',')) 
union 
select [projectid],[projectname] ,a.[areaname],p.isevalue,p.ParkingNumber,p.ManagerPrice,p.address,a.areaid,p.enddate,p.X,p.Y  from @table_project_sub p with(nolock),FxtDataCenter.dbo.SYS_Area a with(nolock)where p.[valid]=1 and p.[cityid]=@cityid and p.[areaid]=a.[areaid] 
and p.projectid  = @projectid and p.fxt_companyid=@fxtcompanyid  )a
left join dbo.LNK_P_Company dp with(nolock)
on dp.ProjectId = a.ProjectId and dp.CompanyType = 2001001 and dp.CityId = @cityid
left join dbo.DAT_Company dpc with(nolock)
on dp.CompanyId = dpc.CompanyId
left join dbo.LNK_P_Company pm with(nolock)
on pm.ProjectId = a.ProjectId and pm.CompanyType = 2001004 and pm.CityId = @cityid
left join dbo.DAT_Company pmc with(nolock)
on pm.CompanyId = pmc.CompanyId