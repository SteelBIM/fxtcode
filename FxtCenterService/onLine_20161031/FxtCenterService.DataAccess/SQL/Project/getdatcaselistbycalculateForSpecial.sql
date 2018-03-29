select 
b.AreaID
,(select top 1 AreaName from FxtDataCenter.dbo.SYS_Area a with(nolock) where a.AreaId = b.AreaID) as AreaName
,b.SubAreaId
,(select top 1 SubAreaName from FxtDataCenter.dbo.SYS_SubArea sa with(nolock) where sa.SubAreaId = b.SubAreaId) as SubAreaName
,b.ProjectName,a.BuildingName,a.HouseNo,CAST( a.floornumber as varchar) as floornumber,a.TotalFloor,a.BuildingDate,a.BuildingArea,a.unitprice,a.TotalPrice,
codetypename=(select codename from FxtDataCenter.dbo.SYS_Code with(nolock) where code=a.casetypecode and a.casetypecode>0),a.CreateDate,a.caseid,a.casedate,a.cityid
,frontname=(SELECT CodeName FROM FxtDataCenter.dbo.SYS_Code WITH(NOLOCK) WHERE Code=a.FrontCode)
--,structurecodename=(select codename from FxtDataCenter.dbo.SYS_Code with(nolock) where code=a.structurecode and a.structurecode>0)
--,(select codename from FxtDataCenter.dbo.SYS_Code with(nolock) where code = c.StructureCode) as buildingstructurecodename,
,(
		case c.IsElevator 
		when 1 then '有'
		when 0 then '无'
		else ''
		end
	)as iselevatorname
,b.address
,a.remark
,h.PlanPurpose
,h.WallType
,h.Window
,h.UnitDoor
,h.UnitWall
,h.Banister
,h.UnitTread
,h.Doors
,h.HeatingType
,h.HouseTypeDetail
,h.ZhuangXiuType
,h.JuJia
,h.LandDetail
,h.LandArea
,h.LandRight
,h.LandOver
,h.sizhi
,h.BuildingRemark
,h.OutWall
,h.NuanGai
,h.LinJie
,h.QW_JTBJD
,h.QW_YLJGJL
,h.QW_JYJGJL
,h.QW_FWSSJL
 from @casetable a with(nolock)
left join (	
	select projectId,ProjectName,FxtCompanyId, address,AreaID,SubAreaId from @projecttable p with(nolock) 
	where cityid=@cityid 
		and valid=1 
		and projectId not in
		(
			select projectId from @projectsubtable ps with(nolock) where Cityid = @cityid and ps.ProjectId = p.ProjectId and Fxt_CompanyId = @fxtcompanyid
			) 
	union 
	select projectId,ProjectName,Fxt_CompanyId AS FxtCompanyId,address,AreaID,SubAreaId from @projectsubtable with(nolock) 
	where cityid = @cityid 
		and valid=1 
		and Fxt_CompanyId = @fxtcompanyid	
) b on a.projectid = b.projectid
--left join (
--	select IsElevator,StructureCode,buildingid from @buildingtable b with(nolock)
--	where CityID = @cityid
--		and Valid = 1
--		and BuildingId not in(
--			select buildingid from @buildingtable_sub bs with(nolock)
--			where CityID = @cityid
--				and bs.buildingid = b.buildingid
--				and Fxt_CompanyId = @fxtcompanyid
--			)
--	union
--	select IsElevator,StructureCode,buildingid from @buildingtable_sub with(nolock)
--	where CityID = @cityid
--		and valid=1 
--		and Fxt_CompanyId = @fxtcompanyid
--) c on c.buildingid = a.buildingid
inner JOIN FxtDataCenter.dbo.Privi_Company_ShowData s with(nolock) ON a.CityID = s.CityId AND s.FxtCompanyId = @fxtcompanyid AND s.CityId = @cityid and s.TypeCode = @typecode AND CHARINDEX(',' + CAST(b.FxtCompanyId AS VARCHAR(10)) + ',',',' + s.ShowCompanyId + ',') > 0
left join Dat_Case_hbh_dhhy h with(nolock) 
	on h.caseid = a.caseid and h.cityid = a.cityid
where 1=1 and a.casedate<=getdate() and a.valid = 1