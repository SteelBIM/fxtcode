select CaseID, ProjectId, BuildingId, HouseId, CompanyId, CaseDate, PurposeCode, FloorNumber, BuildingName, HouseNo, BuildingArea
,UnitPrice, a.CityID, Valid, a.FXTCompanyId, AreaId, AreaName
 from @casetable a with(nolock)
inner JOIN FxtDataCenter.dbo.Privi_Company_ShowData s with(nolock) ON a.CityID = s.CityId AND s.FxtCompanyId = @fxtcompanyid AND s.CityId = @cityid and s.TypeCode = @typecode AND CHARINDEX(',' + CAST(a.FxtCompanyId AS VARCHAR(10)) + ',',',' + s.ShowCompanyId + ',') > 0
where valid=1 and a.cityid=@cityid and casedate >= @startdate and casedate <= @enddate