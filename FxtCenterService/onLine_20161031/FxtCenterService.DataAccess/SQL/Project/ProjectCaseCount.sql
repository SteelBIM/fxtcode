select  count(1) from @casetable a with(nolock)
inner JOIN FxtDataCenter.dbo.Privi_Company_ShowData s with(nolock) ON s.FxtCompanyId = @fxtcompanyid AND s.CityId = @cityid and s.TypeCode = @typecode AND CHARINDEX(',' + CAST(a.FxtCompanyId AS VARCHAR(10)) + ',',',' + s.ShowCompanyId + ',') > 0
where 1=1 and a.projectid=@projectid and a.cityid=@cityid and purposecode=@purposecode and a.casedate>=@startdate and a.casedate<=@enddate and a.valid = 1
