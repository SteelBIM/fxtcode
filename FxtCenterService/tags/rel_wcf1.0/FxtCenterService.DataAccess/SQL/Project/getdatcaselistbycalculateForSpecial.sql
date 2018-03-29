select b.ProjectName,a.BuildingName,a.HouseNo,a.FloorNumber,a.TotalFloor,a.BuildingDate,a.BuildingArea,a.unitprice,a.TotalPrice,
codetypename=(select codename from dbo.sys_code with(nolock) where code=a.casetypecode and a.casetypecode>0),a.CreateDate,a.caseid,a.casedate,a.cityid
,frontname=(SELECT CodeName FROM dbo.SYS_Code WITH(NOLOCK) WHERE Code=a.FrontCode)
,structurecodename=(select codename from dbo.sys_code with(nolock) where code=a.structurecode and a.structurecode>0)
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
 from @casetable a with(nolock)
inner join (	
	select projectId,ProjectName,FxtCompanyId, address from @projecttable with(nolock) where cityid=@cityid and valid=1 and projectId not in
	(select projectId from @projectsubtable with(nolock) where Cityid = @cityid and Fxt_CompanyId = @fxtcompanyid) 
	union 
	select projectId,ProjectName,Fxt_CompanyId AS FxtCompanyId,address from @projectsubtable with(nolock) where cityid = @cityid and valid=1 and Fxt_CompanyId = @fxtcompanyid	
) b on a.projectid = b.projectid
inner JOIN dbo.privi_company_showdata s with(nolock) ON a.CityID = s.CityId AND s.FxtCompanyId = @fxtcompanyid AND s.CityId = @cityid AND CHARINDEX(',' + CAST(b.FxtCompanyId AS VARCHAR(10)) + ',',',' + s.ShowCompanyId + ',') > 0
left join Dat_Case_hbh_dhhy h with(nolock) on h.caseid = a.caseid and h.cityid = a.cityid
	where 1=1 and a.casedate<=getdate() and a.valid = 1