select  @top a.CaseID,a.ProjectId,a.BuildingId,a.HouseId,a.CompanyId,a.CaseDate,a.PurposeCode,convert(nvarchar(10),a.FloorNumber) as FloorNumber,a.BuildingName,a.HouseNo,a.BuildingArea,a.UsableArea,a.FrontCode,a.UnitPrice,a.MoneyUnitCode,a.SightCode,a.CaseTypeCode,a.StructureCode,a.BuildingTypeCode,a.HouseTypeCode,a.CreateDate,a.Creator,a.Remark,a.TotalPrice,a.OldID,a.CityID,a.Valid,a.FXTCompanyId,a.TotalFloor,a.RemainYear,a.Depreciation,a.FitmentCode,a.SurveyId,a.SaveDateTime,a.SaveUser,a.SourceName,a.SourceLink,a.SourcePhone,a.AreaId,a.AreaName,a.BuildingDate,a.ZhuangXiu,a.SubHouse,a.PeiTao
,c.codename purposename
,frontname=(SELECT CodeName FROM FxtDataCenter.dbo.SYS_Code WITH(NOLOCK) WHERE Code=a.FrontCode)
,moneyunitcodename=(select codename from FxtDataCenter.dbo.SYS_Code with(nolock) where code=a.moneyunitcode and a.moneyunitcode>0)
,sightcodename=(select codename from FxtDataCenter.dbo.SYS_Code with(nolock) where code=a.sightcode and a.sightcode>0)
,codetypename=(select codename from FxtDataCenter.dbo.SYS_Code with(nolock) where code=a.casetypecode and a.casetypecode>0)
,structurecodename=(select codename from FxtDataCenter.dbo.SYS_Code with(nolock) where code=a.structurecode and a.structurecode>0)
,buildingtypecodename=(select codename from FxtDataCenter.dbo.SYS_Code with(nolock) where code=a.buildingtypecode and a.buildingtypecode>0)
,housetypecodename=(select codename from FxtDataCenter.dbo.SYS_Code with(nolock) where code=a.housetypecode and a.housetypecode>0)
,fitmentcodename=(select codename from FxtDataCenter.dbo.SYS_Code with(nolock) where code=a.fitmentcode and a.fitmentcode>0)
,cityname=(select cityname from FxtDataCenter.dbo.SYS_City with(nolock) where cityid=a.cityid and a.cityid>0)
,fxtcompanyname=(select companyname from dbo.privi_company with(nolock) where companyid=a.fxtcompanyid and a.fxtcompanyid>0)
,b.projectname
 from @casetable a with(nolock)
inner join (	
	select projectId,ProjectName,FxtCompanyId from @projecttable with(nolock) where cityid=@cityid and valid=1 and projectId not in
	(select projectId from @projectsubtable with(nolock) where Cityid = @cityid and Fxt_CompanyId = @fxtcompanyid) 
	union 
	select projectId,ProjectName,Fxt_CompanyId AS FxtCompanyId from @projectsubtable with(nolock) where cityid = @cityid and valid=1 and Fxt_CompanyId = @fxtcompanyid	
) b on a.projectid = b.projectid
inner JOIN FxtDataCenter.dbo.Privi_Company_ShowData s with(nolock) ON a.CityID = s.CityId AND s.FxtCompanyId = @fxtcompanyid and s.TypeCode = @typecode AND s.CityId = @cityid AND CHARINDEX(',' + CAST(b.FxtCompanyId AS VARCHAR(10)) + ',',',' + s.ShowCompanyId + ',') > 0
left join FxtDataCenter.dbo.SYS_Code c with(nolock) on c.code = a.purposecode
where 1=1 and a.casedate<=getdate() and a.valid = 1
