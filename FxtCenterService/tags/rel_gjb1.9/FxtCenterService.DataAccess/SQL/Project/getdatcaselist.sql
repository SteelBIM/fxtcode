select  @top a.*
,c.codename purposename
,frontname=(SELECT CodeName FROM dbo.SYS_Code WITH(NOLOCK) WHERE Code=a.FrontCode)
,moneyunitcodename=(select codename from dbo.sys_code with(nolock) where code=a.moneyunitcode and a.moneyunitcode>0)
,sightcodename=(select codename from dbo.sys_code with(nolock) where code=a.sightcode and a.sightcode>0)
,codetypename=(select codename from dbo.sys_code with(nolock) where code=a.casetypecode and a.casetypecode>0)
,structurecodename=(select codename from dbo.sys_code with(nolock) where code=a.structurecode and a.structurecode>0)
,buildingtypecodename=(select codename from dbo.sys_code with(nolock) where code=a.buildingtypecode and a.buildingtypecode>0)
,housetypecodename=(select codename from dbo.sys_code with(nolock) where code=a.housetypecode and a.housetypecode>0)
,fitmentcodename=(select codename from dbo.sys_code with(nolock) where code=a.fitmentcode and a.fitmentcode>0)
,cityname=(select cityname from dbo.sys_city with(nolock) where cityid=a.cityid and a.cityid>0)
,fxtcompanyname=(select companyname from dbo.privi_company with(nolock) where companyid=a.fxtcompanyid and a.fxtcompanyid>0)
,b.projectname
 from @casetable a with(nolock)
inner join (	
	select projectId,ProjectName,FxtCompanyId from @projecttable with(nolock) where cityid=@cityid and valid=1 and projectId not in
	(select projectId from @projectsubtable with(nolock) where Cityid = @cityid and Fxt_CompanyId = @fxtcompanyid) 
	union 
	select projectId,ProjectName,Fxt_CompanyId AS FxtCompanyId from @projectsubtable with(nolock) where cityid = @cityid and valid=1 and Fxt_CompanyId = @fxtcompanyid	
) b on a.projectid = b.projectid
inner JOIN dbo.privi_company_showdata s with(nolock) ON a.CityID = s.CityId AND s.FxtCompanyId = @fxtcompanyid AND s.CityId = @cityid AND CHARINDEX(',' + CAST(b.FxtCompanyId AS VARCHAR(10)) + ',',',' + s.ShowCompanyId + ',') > 0
left join dbo.sys_code c with(nolock) on c.code = a.purposecode
where 1=1 and a.casedate<=getdate() and a.valid = 1
