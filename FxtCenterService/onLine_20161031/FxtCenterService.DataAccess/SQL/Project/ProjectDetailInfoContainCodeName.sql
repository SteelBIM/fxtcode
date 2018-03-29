select ProjectId, ProjectName, p.SubAreaId, FieldNo, PurposeCode, p.Address, LandArea, StartDate, UsableYear, BuildingArea, SalableArea, CubageRate, GreenRate, 
	BuildingDate, CoverDate, SaleDate, JoinDate, 
	case when @buildingid>0 and (
		select builddate
		from @buildingtable b with(nolock)
		where not exists (
			select BuildingId from @buildingtable_sub bb 
			where bb.BuildingId = b.BuildingId 
				and b.fxtcompanyid = @FxtCompanyId
			)
			and b.fxtcompanyid in (
				select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata 
				where cityid = @CityId and fxtcompanyid = @FxtCompanyId and typecode = @TypeCode), ',') 
			)
			and b.CityID = @CityId
			and b.BuildingId = @BuildingId
			and b.valid = 1
		union
		select builddate
		from @buildingtable_sub bb with(nolock)
		where bb.Fxt_CompanyId  = @FxtCompanyId
			and bb.CityID = @CityId
			and bb.BuildingId = @BuildingId
			and bb.valid = 1
	) IS null
	then
	(
		select builddate
		from @buildingtable b with(nolock)
		where not exists (
			select BuildingId from @buildingtable_sub bb where bb.BuildingId = b.BuildingId
				and b.fxtcompanyid = @FxtCompanyId
			)
			and b.fxtcompanyid in (
					select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata 
					where cityid = @CityId and fxtcompanyid = @FxtCompanyId and typecode = @TypeCode), ',') 
			)
			and b.CityID = @CityId
			and b.BuildingId = @BuildingId
			and b.valid = 1
		union
		select builddate
		from @buildingtable_sub bb with(nolock)
		where bb.Fxt_CompanyId  = @FxtCompanyId
			and bb.CityID = @CityId
			and bb.BuildingId = @BuildingId
			and bb.valid = 1
	)
	else EndDate
	end EndDate,
	InnerSaleDate, RightCode, ParkingNumber, AveragePrice, ManagerTel, ManagerPrice, TotalNum, BuildingNum,
	Detail, BuildingTypeCode, UpdateDateTime, OfficeArea, OtherArea, PlanPurpose, PriceDate, IsComplete, OtherName, SaveDateTime, SaveUser, ProjectWeight, 
	BusinessArea, IndustryArea, IsEValue, PinYin, p.CityID, p.AreaID, p.OldId, CreateTime, AreaLineId, SalePrice, PinYinAll, ProjectX,ProjectY, p.XYScale, Creator, 
	IsEmpty, TotalId, East, West, South, North, BuildingQuality, HousingScale, BuildingDetail, HouseDetail, BasementPurpose, ManagerQuality, Facilities, 
	AppendageClass, RegionalAnalysis, Wrinkle, Aversion,FxtCompanyId,parkingdesc
	,a.AreaName,sa.SubAreaName,c5.CodeName as AppendageClassName,c6.CodeName as ProjectPurposeCodeName
	,c7.CodeName as ProjectTypeCodeName,c8.CodeName as ProjectRightCodeName
	,c20.CodeName as ProjectBuildingQualityName,c21.CodeName as ProjectHousingScaleName
	,c22.CodeName as ProjectManagerQualityName
	,DeveCompanyName = (SELECT top 1 ChineseName FROM FXTProject.dbo.LNK_P_Company pc ,FxtDataCenter.dbo.DAT_Company d WHERE pc.CompanyId = d.CompanyId and d.FxtCompanyId = p.FxtCompanyId AND pc.CityId = @CityId AND pc.ProjectId = p.ProjectId AND PC.CompanyType = 2001001)
	,ManagerCompanyName = (SELECT top 1 ChineseName FROM FXTProject.dbo.LNK_P_Company pc,FxtDataCenter.dbo.DAT_Company d WHERE pc.CompanyId = d.CompanyId and d.FxtCompanyId = p.FxtCompanyId AND pc.CityId = @CityId AND pc.ProjectId = p.ProjectId AND PC.CompanyType = 2001004)
	,parkingstatus = (select top 1 ClassCode from [FXTProject].[dbo].LNK_P_Appendage pa where pa.AppendageCode = 2008014 and pa.CityId = 6 and pa.ProjectId = p.ProjectId )
	--楼盘
from (
	select ProjectId, ProjectName, SubAreaId, FieldNo, PurposeCode, p.Address, LandArea, StartDate, UsableYear, BuildingArea, SalableArea, CubageRate, GreenRate, 
		BuildingDate, CoverDate, SaleDate, JoinDate, EndDate, InnerSaleDate, RightCode, ParkingNumber, AveragePrice, ManagerTel, ManagerPrice, TotalNum, BuildingNum,
		Detail, BuildingTypeCode, UpdateDateTime, OfficeArea, OtherArea, PlanPurpose, PriceDate, IsComplete, OtherName, SaveDateTime, SaveUser, Weight as ProjectWeight, 
		BusinessArea, IndustryArea, IsEValue, PinYin, p.CityID, AreaID, OldId, CreateTime, AreaLineId, SalePrice, PinYinAll, X as ProjectX, Y as ProjectY, XYScale, Creator, 
		IsEmpty, TotalId, East, West, South, North, BuildingQuality, HousingScale, BuildingDetail, HouseDetail, BasementPurpose, ManagerQuality, Facilities, 
		AppendageClass, RegionalAnalysis, Wrinkle, Aversion,FxtCompanyId,parkingdesc
	from @projecttable p with(nolock)
	where not exists (
				select ProjectId from @projecttable_sub pb where pb.ProjectId = p.ProjectId
				and pb.Fxt_CompanyId = @FxtCompanyId
				and pb.cityid = @CityId
				--and pb.Fxt_CompanyId in (
				--	select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata 
				--	where cityid = @CityId and fxtcompanyid = @FxtCompanyId and typecode = @TypeCode), ',') 
				--)
				) 
	and p.fxtcompanyid in (
		select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata 
		where cityid = @CityId and fxtcompanyid = @FxtCompanyId and typecode = @TypeCode), ',')
		)
	and p.CityID = @CityId
	@ProjectId
	and p.valid = 1

	union
	select pb.ProjectId, ProjectName, SubAreaId, FieldNo, PurposeCode, pb.Address, LandArea, StartDate, UsableYear, BuildingArea, SalableArea, CubageRate, GreenRate, 
        BuildingDate, CoverDate, SaleDate, JoinDate, EndDate, InnerSaleDate, RightCode, ParkingNumber, AveragePrice, ManagerTel, ManagerPrice, TotalNum, BuildingNum,
        Detail, BuildingTypeCode, UpdateDateTime, OfficeArea, OtherArea, PlanPurpose, PriceDate, IsComplete, OtherName, SaveDateTime, SaveUser, Weight as ProjectWeight, 
        BusinessArea, IndustryArea, IsEValue, PinYin, pb.CityID, AreaID, OldId, CreateTime, AreaLineId, SalePrice, PinYinAll, X as ProjectX, Y as ProjectY, XYScale, Creator, 
        IsEmpty, TotalId, East, West, South, North, BuildingQuality, HousingScale, BuildingDetail, HouseDetail, BasementPurpose, ManagerQuality, Facilities, 
        AppendageClass, RegionalAnalysis, Wrinkle, Aversion,Fxt_CompanyId as FxtCompanyId,parkingdesc
	from @projecttable_sub pb with(nolock)
	where  pb.Fxt_CompanyId = @FxtCompanyId
	--where  pb.Fxt_CompanyId in (
	--	select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata 
	--	where cityid = @CityId and fxtcompanyid = @FxtCompanyId and typecode = @TypeCode), ',') 
	--	)
	and pb.CityID = @CityId
	@ProjectId
	and pb.valid = 1
) p 
left join FxtDataCenter.dbo.SYS_Area a with(nolock) on p.AreaID = a.AreaId
left join FxtDataCenter.dbo.SYS_SubArea sa with(nolock) on p.SubAreaId = sa.SubAreaId
left join FxtDataCenter.dbo.SYS_Code c5 with(nolock) on p.AppendageClass = c5.Code--配套等级
left join FxtDataCenter.dbo.SYS_Code c6 with(nolock) on p.PurposeCode = c6.Code--主用途
left join FxtDataCenter.dbo.SYS_Code c7 with(nolock) on p.BuildingTypeCode = c7.Code--主建筑物类型
left join FxtDataCenter.dbo.SYS_Code c8 with(nolock) on p.RightCode = c8.Code--产权形式
left join FxtDataCenter.dbo.SYS_Code c20 with(nolock) on p.BuildingQuality = c20.Code--建筑质量
left join FxtDataCenter.dbo.SYS_Code c21 with(nolock) on p.HousingScale = c21.Code--小区规模
left join FxtDataCenter.dbo.SYS_Code c22 with(nolock) on p.ManagerQuality = c22.Code--物业管理质量