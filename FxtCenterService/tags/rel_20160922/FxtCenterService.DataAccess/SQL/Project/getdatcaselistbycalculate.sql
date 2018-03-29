select 
	b.AreaID,(select top 1 AreaName from FxtDataCenter.dbo.SYS_Area a with(nolock) where a.AreaId = b.AreaID) as AreaName
	,b.SubAreaId,(select top 1 SubAreaName from FxtDataCenter.dbo.SYS_SubArea sa with(nolock) where sa.SubAreaId = b.SubAreaId) as SubAreaName
	,b.ProjectName,a.BuildingName,a.HouseNo,CAST( a.floornumber as varchar) as floornumber,a.TotalFloor,a.BuildingDate,a.BuildingArea,a.unitprice,a.TotalPrice,
	codetypename=(select codename from FxtDataCenter.dbo.SYS_Code with(nolock) where code=a.casetypecode and a.casetypecode>0),a.CreateDate,a.caseid,a.casedate,a.sourcename,a.sourcelink
	,(select codename from FxtDataCenter.dbo.SYS_Code with(nolock) where code = c.StructureCode) as structurecodename
	,(
		case c.iselevator 
		when 1 then '是'
		when 0 then '否'
		else ''
		end
	)as iselevatorname
 from @casetable a with(nolock)
left join (	
	select projectId,ProjectName,FxtCompanyId,Address,AreaID,SubAreaId from @projecttable p with(nolock) 
	where cityid=@cityid 
		and valid=1 
		and projectId 
		not in
		(
			select projectId from @projectsubtable ps with(nolock) where Cityid = @cityid and ps.ProjectId = p.ProjectId and Fxt_CompanyId = @fxtcompanyid
		) 
	union 
	select projectId,ProjectName,Fxt_CompanyId AS FxtCompanyId,Address,AreaID,SubAreaId from @projectsubtable with(nolock) 
	where cityid = @cityid 
		and valid=1 
		and Fxt_CompanyId = @fxtcompanyid	
) b on a.projectid = b.projectid
left join (
	select IsElevator,StructureCode,buildingid from @buildingtable b with(nolock)
	where CityID = @cityid
		and Valid = 1
		and BuildingId not in(
			select buildingid from @buildingtable_sub bs with(nolock)
			where CityID = @cityid
				and bs.buildingid = b.buildingid
				and Fxt_CompanyId = @fxtcompanyid
			)
	union
	select IsElevator,StructureCode,buildingid from @buildingtable_sub with(nolock)
	where CityID = @cityid
		and valid=1 
		and Fxt_CompanyId = @fxtcompanyid
) c on c.buildingid = a.buildingid
inner JOIN FxtDataCenter.dbo.Privi_Company_ShowData s with(nolock) ON a.CityID = s.CityId AND s.FxtCompanyId = @fxtcompanyid AND s.CityId = @cityid and s.TypeCode = @typecode AND CHARINDEX(',' + CAST(b.FxtCompanyId AS VARCHAR(10)) + ',',',' + s.ShowCompanyId + ',') > 0
where 1=1 
	and a.casedate<=getdate() 
	and a.valid = 1
	and a.fxtcompanyid in 
	(
		SELECT value FROM FXTProject.dbo.SplitToTable((SELECT CaseCompanyId FROM FxtDataCenter.dbo.Privi_Company_ShowData WHERE CityId=@cityid AND FxtCompanyId=@fxtcompanyid and TypeCode = @typecode),',')
	)


