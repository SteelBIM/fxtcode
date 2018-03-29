select b.ProjectName,a.BuildingName,a.HouseNo,a.FloorNumber,a.TotalFloor,a.BuildingDate,a.BuildingArea,a.unitprice,a.TotalPrice,
codetypename=(select codename from dbo.sys_code with(nolock) where code=a.casetypecode and a.casetypecode>0),a.CreateDate,a.caseid,a.casedate
 from @casetable a with(nolock)
inner join (	
	select projectId,ProjectName,FxtCompanyId,Address from @projecttable with(nolock) where cityid=@cityid and valid=1 and projectId not in
	(select projectId from @projectsubtable with(nolock) where Cityid = @cityid and Fxt_CompanyId = @fxtcompanyid) 
	union 
	select projectId,ProjectName,Fxt_CompanyId AS FxtCompanyId,Address from @projectsubtable with(nolock) where cityid = @cityid and valid=1 and Fxt_CompanyId = @fxtcompanyid	
) b on a.projectid = b.projectid
inner JOIN dbo.privi_company_showdata s with(nolock) ON a.CityID = s.CityId AND s.FxtCompanyId = @fxtcompanyid AND s.CityId = @cityid AND CHARINDEX(',' + CAST(b.FxtCompanyId AS VARCHAR(10)) + ',',',' + s.ShowCompanyId + ',') > 0
where 1=1 and a.casedate<=getdate() and a.valid = 1