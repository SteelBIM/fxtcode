



if  @projectid<=0
 begin 

--通过地址查询
if  @address is not null and  @address!=''
	begin 
	(select top 1 projectid,projectname,[address] from @table_project pro with(nolock) where [address]=@address   
	 and 
 (','+cast((select showcompanyid from dbo.privi_company_showdata pc with(nolock) where pc.fxtcompanyid=@fxtcompanyid and pc.cityid = pro.cityid)
  as varchar)+',' like '%,' + cast(pro.fxtcompanyid as varchar) + ',%')
	union all  
	select top 1 projectid,projectname,[address] from @table_project_sub  pro with(nolock) where [address]=@address
	 and 
 (','+cast((select showcompanyid from dbo.privi_company_showdata pc with(nolock) where pc.fxtcompanyid=@fxtcompanyid and pc.cityid = pro.cityid)
  as varchar)+',' like '%,' + cast(pro.fxt_companyid as varchar) + ',%')
  )
end 
	
	

--通过楼盘查询
else if  @projectname is not null and  @projectname!=''
	--通过楼盘匹配
	begin
		select top 1 projectid,projectname,[address]
		from @table_project pro with(nolock) where projectname=@projectname	and 
 (','+cast((select showcompanyid from dbo.privi_company_showdata pc with(nolock) where pc.fxtcompanyid=@fxtcompanyid and pc.cityid = pro.cityid)
  as varchar)+',' like '%,' + cast(pro.fxtcompanyid as varchar) + ',%')
		union all  
		select top 1 projectid,projectname,[address] from @table_project_sub  pro with(nolock) where projectname=@projectname	and 
 (','+cast((select showcompanyid from dbo.privi_company_showdata pc with(nolock) where pc.fxtcompanyid=@fxtcompanyid and pc.cityid = pro.cityid)
  as varchar)+',' like '%,' + cast(pro.fxt_companyid as varchar) + ',%')
		union all
		select top 1 projectnameid,netname,'' [address]
		 from dbo.sys_projectmatch pro with(nolock) where netname=@projectname	 and cityid=@cityid  
		and 
 (','+cast((select showcompanyid from dbo.privi_company_showdata pc with(nolock) where pc.fxtcompanyid=@fxtcompanyid and pc.cityid = pro.cityid)
  as varchar)+',' like '%,' + cast(pro.fxtcompanyid as varchar) + ',%')
	end

end 



	
--获取楼栋id
if @projectid>0  and @buildingname is not null and  @buildingname!='' 
	begin 
		select buildingid, buildingname
		 from @table_building pro with(nolock) where
			 projectid=@projectid and buildingname=@buildingname
			 and 
 (','+cast((select showcompanyid from dbo.privi_company_showdata pc with(nolock) where pc.fxtcompanyid=@fxtcompanyid and pc.cityid = pro.cityid)
  as varchar)+',' like '%,' + cast(pro.fxtcompanyid as varchar) + ',%')
		union all  
		select  buildingid, buildingname 
		 from @table_building_sub  pro with(nolock) where 
			projectid=@projectid and buildingname=@buildingname
			and 
 (','+cast((select showcompanyid from dbo.privi_company_showdata pc with(nolock) where pc.fxtcompanyid=@fxtcompanyid and pc.cityid = pro.cityid)
  as varchar)+',' like '%,' + cast(pro.fxt_companyid as varchar) + ',%')

  
	end


--获取房号id
if @buildingid >0 and @housename is not null and  @housename!=''  
	begin 
		
		select top 1 housename,houseid from @table_house pro with(nolock) where buildingid=@buildingid and housename=@housename 
		and 
 (','+cast((select showcompanyid from dbo.privi_company_showdata pc with(nolock) where pc.fxtcompanyid=@fxtcompanyid and pc.cityid = pro.cityid)
  as varchar)+',' like '%,' + cast(pro.fxtcompanyid as varchar) + ',%')
	
		select  top 1  housename,houseid from @table_house_sub pro with(nolock) where buildingid=@buildingid and housename=@housename
		and 
 (','+cast((select showcompanyid from dbo.privi_company_showdata pc with(nolock) where pc.fxtcompanyid=@fxtcompanyid and pc.cityid = pro.cityid)
  as varchar)+',' like '%,' + cast(pro.fxtcompanyid as varchar) + ',%')
		
	end


