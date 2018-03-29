select 
	T.*
	,a.AreaName
	,c1.CodeName as CaseTypeCodeName
	,c2.CodeName as BuildingTypeCodeName
	,c3.CodeName as PurposeCodeName
from (
	select 	
		T.AreaId
		,T.ProjectId
		,T.ProjectName
		,C.CaseID
		,C.CaseDate
		,C.CaseTypeCode
		,C.BuildingArea
		,C.UnitPrice
		,C.TotalPrice
		,C.BuildingTypeCode
		,C.PurposeCode		
		,C.SourceName
	from (
		select ProjectId,ProjectName,SubAreaId,FieldNo,PurposeCode,Address,LandArea,StartDate,UsableYear,BuildingArea,SalableArea,CubageRate,GreenRate,BuildingDate,CoverDate,SaleDate,JoinDate,EndDate,InnerSaleDate,RightCode,ParkingNumber,AveragePrice,ManagerTel,ManagerPrice,TotalNum,BuildingNum,Detail,BuildingTypeCode,UpdateDateTime,OfficeArea,OtherArea,PlanPurpose,PriceDate,IsComplete,OtherName,SaveDateTime,SaveUser,Weight,BusinessArea,IndustryArea,IsEValue,PinYin,CityID,AreaID,OldId,CreateTime,AreaLineId,Valid,SalePrice,PinYinAll,X,Y,XYScale,Creator,IsEmpty,TotalId,East,West,South,North,BuildingQuality,HousingScale,BuildingDetail,HouseDetail,BasementPurpose,ManagerQuality,Facilities,AppendageClass,RegionalAnalysis,Wrinkle,Aversion,FxtCompanyId
		from @projecttable p with(nolock)
		where not exists(
			select ProjectId from @projectsubtable ps with(nolock)
			where ps.ProjectId = p.ProjectId
			and ps.CityID = @cityid
			and ps.Fxt_CompanyId = @fxtcompanyid
		)
		and p.Valid = 1
		and p.CityID = @cityid
		and p.FxtCompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT ShowCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid and TypeCode = @typecode), ','))
		union 
		select ProjectId,ProjectName,SubAreaId,FieldNo,PurposeCode,Address,LandArea,StartDate,UsableYear,BuildingArea,SalableArea,CubageRate,GreenRate,BuildingDate,CoverDate,SaleDate,JoinDate,EndDate,InnerSaleDate,RightCode,ParkingNumber,AveragePrice,ManagerTel,ManagerPrice,TotalNum,BuildingNum,Detail,BuildingTypeCode,UpdateDateTime,OfficeArea,OtherArea,PlanPurpose,PriceDate,IsComplete,OtherName,SaveDateTime,SaveUser,Weight,BusinessArea,IndustryArea,IsEValue,PinYin,CityID,AreaID,OldId,CreateTime,AreaLineId,Valid,SalePrice,PinYinAll,X,Y,XYScale,Creator,IsEmpty,TotalId,East,West,South,North,BuildingQuality,HousingScale,BuildingDetail,HouseDetail,BasementPurpose,ManagerQuality,Facilities,AppendageClass,RegionalAnalysis,Wrinkle,Aversion,Fxt_CompanyId
		from @projectsubtable p with(nolock)
		where p.Valid = 1
		and p.CityID = @cityid
		and p.Fxt_CompanyId = @fxtcompanyid
	)T
	inner join (
		select * from @casetable c with(nolock)
		where c.Valid = 1
		and c.CityID = @cityid
		and c.FXTCompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT CaseCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid and TypeCode = @typecode), ','))
		and c.CaseDate between @casedatefrom and @casedateto
	)C on T.ProjectId = C.ProjectId
)T
left join FxtDataCenter.dbo.SYS_Area a with(nolock) on T.AreaID = a.AreaId
left join FxtDataCenter.dbo.SYS_Code c1 with(nolock) on T.CaseTypeCode = c1.Code
left join FxtDataCenter.dbo.SYS_Code c2 with(nolock) on T.BuildingTypeCode = c2.Code
left join FxtDataCenter.dbo.SYS_Code c3 with(nolock) on T.PurposeCode = c3.Code
where 1 = 1