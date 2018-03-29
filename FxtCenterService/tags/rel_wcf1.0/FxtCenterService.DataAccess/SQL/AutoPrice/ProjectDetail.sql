select a.*,isnull(c.casecnt,0) as casecnt from (
SELECT [ProjectId],[ProjectName] +'['+a.[AreaName]+']' ProjectName,p.IsEValue,p.Address,a.areaid, p.buildingdate,p.PinYinALL,p.PinYin,p.averageprice,p.[Weight],p.enddate  FROM @table_project p with(nolock),[dbo].SYS_Area a with(nolock)where p.[Valid]=1 and p.[CityId]=@cityid and p.[AreaID]=a.[AreaID] 
and projectid = @projectid
and p.ProjectId not in (select ProjectId from @table_project_sub ps with(nolock) where p.ProjectId=ps.ProjectId and ps.Fxt_CompanyId=@fxtcompanyid and ps.CityId=p.CityId) AND p.FxtCompanyId  IN (SELECT value FROM  dbo.SplitToTable((SELECT ShowCompanyId FROM dbo.Privi_Company_ShowData WHERE CityId=@cityid AND FxtCompanyId=@fxtcompanyid),',')) 
union 
SELECT [ProjectId],[ProjectName] +'['+a.[AreaName]+']' ProjectName,p.IsEValue,p.Address,a.areaid,p.buildingdate,p.PinYinALL,p.PinYin,p.averageprice,p.[Weight],p.enddate  FROM @table_project_sub p with(nolock),[dbo].SYS_Area a with(nolock)where p.[Valid]=1 and p.[CityId]=@cityid and p.[AreaID]=a.[AreaID] 
and projectid = @projectid and p.Fxt_CompanyId=@fxtcompanyid  )a
left join 
(select count(1) casecnt,projectid from @casetable with(nolock) where casedate between dateadd(mm,-@caseMonth,getdate()) and getdate() group by projectid) c
on a.ProjectId =c.projectid
