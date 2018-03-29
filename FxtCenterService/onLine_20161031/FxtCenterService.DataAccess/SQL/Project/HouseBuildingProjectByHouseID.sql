select th.*,b.*
,c1.CodeName as BuildingRightCodeName,c2.CodeName as BuildingPurposeCodeName
,c3.CodeName as BuildingTypeCodeName,c4.CodeName as BuildingStructureCodeName
,c10.CodeName as BuildingStructureCodeName,c11.CodeName as BuildingSightCodeName
,c12.CodeName as BuildingFrontCodeName,c13.CodeName as BuildingWallName
,c14.CodeName as BuildinginnerFitmentCodeName,c15.CodeName as BuildingPipelineGasCodeCodeName
,c16.CodeName as BuildingHeatingModeCodeName,c17.CodeName as BuildingWallTypeCodeName
,p.*
,a.AreaName,sa.SubAreaName,c5.CodeName as AppendageClassName,c6.CodeName as ProjectPurposeCodeName
,c7.CodeName as ProjectTypeCodeName,c8.CodeName as ProjectRightCodeName
,c20.CodeName as ProjectBuildingQualityName,c21.CodeName as ProjectHousingScaleName
,c22.CodeName as ProjectManagerQualityName
,DeveCompanyName = (SELECT ChineseName FROM FXTProject.dbo.LNK_P_Company pc ,FxtDataCenter.dbo.DAT_Company d WHERE pc.CompanyId = d.CompanyId AND pc.CityId = 6 AND pc.ProjectId = p.ProjectId AND PC.CompanyType = 2001001)
,ManagerCompanyName = (SELECT ChineseName FROM FXTProject.dbo.LNK_P_Company pc,FxtDataCenter.dbo.DAT_Company d WHERE pc.CompanyId = d.CompanyId AND pc.CityId = 6 AND pc.ProjectId = p.ProjectId AND PC.CompanyType = 2001004)
from(	
	select 
	HouseId, HouseName, HouseTypeCode, FloorNo, UnitNo, BuildArea, FrontCode, SightCode, UnitPrice, SalePrice, Weight as HouseWeight, PhotoName, h.Remark, 
                      StructureCode, TotalPrice, PurposeCode, IsEValue, CityID, OldId, CreateTime, SaveDateTime, SaveUser, IsShowBuildingArea, InnerBuildingArea, 
                      SubHouseType, SubHouseArea, Creator, NominalFloor, VDCode, FitmentCode, Cookroom as HouseCookroom, Balcony, Toilet
	,c1.CodeName as StructureCodeName,c2.CodeName as FrontCodeName
	,c3.CodeName as SightCodeName,c4.CodeName as HouseTypeCodeName,c5.CodeName as PurposeCodeName
	,c6.CodeName as SubHouseTypeName,c7.CodeName as FitmentCodeName
	,c8.CodeName as VDCodeName,(case h.Cookroom when 1 then '有' else '无' end) as Cookroom
	from @housetable h with(nolock)
	left join FxtDataCenter.dbo.SYS_Code c1 with(nolock) on h.StructureCode = c1.Code--户型结构
	left join FxtDataCenter.dbo.SYS_Code c2 with(nolock) on h.FrontCode = c2.Code--朝向
	left join FxtDataCenter.dbo.SYS_Code c3 with(nolock) on h.SightCode = c3.Code--景观
	left join FxtDataCenter.dbo.SYS_Code c4 with(nolock) on h.HouseTypeCode = c4.Code--户型
	left join FxtDataCenter.dbo.SYS_Code c5 with(nolock) on h.PurposeCode = c5.Code--用途
	left join FxtDataCenter.dbo.SYS_Code c6 with(nolock) on h.SubHouseType = c6.Code--房屋附属类型
	left join FxtDataCenter.dbo.SYS_Code c7 with(nolock) on h.FitmentCode = c7.Code--装修
	left join FxtDataCenter.dbo.SYS_Code c8 with(nolock) on h.VDCode = c8.Code--通风采光
	where not exists(
				select HouseId from @housetable_sub hb with(nolock) where hb.HouseId = h.HouseId
					and hb.fxtcompanyid = @FxtCompanyId
					--and hb.fxtcompanyid in (
					--	select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata 
					--	where cityid = @CityId and fxtcompanyid = @FxtCompanyId and typecode = @TypeCode), ',')
					--	)
				)
	and h.fxtcompanyid in (
						select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata 
						where cityid = @CityId and fxtcompanyid = @FxtCompanyId and typecode = @TypeCode), ',')
						)
	and h.HouseId = @HouseId
	and h.CityID = @CityId
	and h.valid = 1
	union
	select 
	HouseId, HouseName, HouseTypeCode, FloorNo, UnitNo, BuildArea, FrontCode, SightCode, UnitPrice, SalePrice, Weight as HouseWeight, PhotoName, hb.Remark, 
                      StructureCode, TotalPrice, PurposeCode, IsEValue, CityID, OldId, CreateTime, SaveDateTime, SaveUser, IsShowBuildingArea, InnerBuildingArea, 
                      SubHouseType, SubHouseArea, Creator, NominalFloor, VDCode, FitmentCode, Cookroom as HouseCookroom, Balcony, Toilet
	,c1.CodeName as StructureCodeName,c2.CodeName as FrontCodeName
	,c3.CodeName as SightCodeName,c4.CodeName as HouseTypeCodeName,c5.CodeName as PurposeCodeName
	,c6.CodeName as SubHouseTypeName,c7.CodeName as FitmentCodeName
	,c8.CodeName as VDCodeName,(case hb.Cookroom when 1 then '有' else '无' end) as Cookroom
	from @housetable_sub hb with(nolock)
	left join FxtDataCenter.dbo.SYS_Code c1 with(nolock) on hb.StructureCode = c1.Code
	left join FxtDataCenter.dbo.SYS_Code c2 with(nolock) on hb.FrontCode = c2.Code
	left join FxtDataCenter.dbo.SYS_Code c3 with(nolock) on hb.SightCode = c3.Code
	left join FxtDataCenter.dbo.SYS_Code c4 with(nolock) on hb.HouseTypeCode = c4.Code
	left join FxtDataCenter.dbo.SYS_Code c5 with(nolock) on hb.PurposeCode = c5.Code
	left join FxtDataCenter.dbo.SYS_Code c6 with(nolock) on hb.SubHouseType = c6.Code
	left join FxtDataCenter.dbo.SYS_Code c7 with(nolock) on hb.FitmentCode = c7.Code
	left join FxtDataCenter.dbo.SYS_Code c8 with(nolock) on hb.VDCode = c8.Code
	where  hb.fxtcompanyid = @FxtCompanyId
	--where  hb.fxtcompanyid in (
	--					select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata 
	--					where cityid = @CityId and fxtcompanyid = @FxtCompanyId and typecode = @TypeCode), ',') 
	--					)
	and hb.HouseId = @HouseId
	and hb.CityID = @CityId
	and hb.valid = 1
) as th
--楼栋
, (
			select BuildingId, BuildingName, PurposeCode, StructureCode, BuildingTypeCode, TotalFloor, FloorHigh, SaleLicence, ElevatorRate, UnitsNumber, 
                      TotalNumber, TotalBuildArea, BuildDate, SaleDate, AveragePrice, AverageFloor, JoinDate, LicenceDate, OtherName, Weight as BuildingWeight, IsEValue, CityID, CreateTime, OldId, 
                       SalePrice, SaveDateTime, SaveUser, LocationCode, SightCode, FrontCode, StructureWeight, BuildingTypeWeight, YearWeight, PurposeWeight, LocationWeight, 
                      SightWeight, FrontWeight, X as BuildingX, Y as BuildingY, XYScale, Wall, IsElevator, SubAveragePrice, PriceDetail, BHouseTypeCode, BHouseTypeWeight, Creator, Distance, 
                      DistanceWeight, basement, Remark, ElevatorRateWeight, IsYard, YardWeight, Doorplate, RightCode, IsVirtual, FloorSpread, PodiumBuildingFloor, 
                      PodiumBuildingArea, TowerBuildingArea, BasementArea, BasementPurpose, HouseNumber, HouseArea, OtherNumber, OtherArea, innerFitmentCode, 
                      FloorHouseNumber, LiftNumber, LiftBrand, Facilities, PipelineGasCode, HeatingModeCode, WallTypeCode
			from @buildingtable b with(nolock)
			where not exists (
						select BuildingId from @buildingtable_sub bb where bb.BuildingId = b.BuildingId
							and b.fxtcompanyid = @FxtCompanyId
							--and b.fxtcompanyid in (
							--select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata 
							--where cityid = @CityId and fxtcompanyid = @FxtCompanyId and typecode = @TypeCode), ',') 
							--)
						)
			and b.fxtcompanyid in (
					select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata 
					where cityid = @CityId and fxtcompanyid = @FxtCompanyId and typecode = @TypeCode), ',') 
					)
			and b.CityID = @CityId
			and b.BuildingId = @BuildingId
			and b.valid = 1
			union
			select BuildingId, BuildingName, PurposeCode, StructureCode, BuildingTypeCode, TotalFloor, FloorHigh, SaleLicence, ElevatorRate, UnitsNumber, 
                      TotalNumber, TotalBuildArea, BuildDate, SaleDate, AveragePrice, AverageFloor, JoinDate, LicenceDate, OtherName, Weight as BuildingWeight, IsEValue, CityID, CreateTime, OldId, 
                       SalePrice, SaveDateTime, SaveUser, LocationCode, SightCode, FrontCode, StructureWeight, BuildingTypeWeight, YearWeight, PurposeWeight, LocationWeight, 
                      SightWeight, FrontWeight, X as BuildingX, Y as BuildingY, XYScale, Wall, IsElevator, SubAveragePrice, PriceDetail, BHouseTypeCode, BHouseTypeWeight, Creator, Distance, 
                      DistanceWeight, basement, Remark, ElevatorRateWeight, IsYard, YardWeight, Doorplate, RightCode, IsVirtual, FloorSpread, PodiumBuildingFloor, 
                      PodiumBuildingArea, TowerBuildingArea, BasementArea, BasementPurpose, HouseNumber, HouseArea, OtherNumber, OtherArea, innerFitmentCode, 
                      FloorHouseNumber, LiftNumber, LiftBrand, Facilities, PipelineGasCode, HeatingModeCode, WallTypeCode
			from @buildingtable_sub bb with(nolock)
			where bb.Fxt_CompanyId = @FxtCompanyId
			--where bb.Fxt_CompanyId in (
			--		select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata 
			--		where cityid = @CityId and fxtcompanyid = @FxtCompanyId and typecode = @TypeCode), ',') 
			--		)
			and bb.CityID = @CityId
			and bb.BuildingId = @BuildingId
			and bb.valid = 1
			) b 
left join FxtDataCenter.dbo.SYS_Code c1 with(nolock) on b.RightCode = c1.Code--产权形式
left join FxtDataCenter.dbo.SYS_Code c2 with(nolock) on b.PurposeCode = c2.Code--用途
left join FxtDataCenter.dbo.SYS_Code c3 with(nolock) on b.BuildingTypeCode = c3.Code--建筑类型
left join FxtDataCenter.dbo.SYS_Code c4 with(nolock) on b.StructureCode = c4.Code--建筑结构
left join FxtDataCenter.dbo.SYS_Code c10 with(nolock) on b.LocationCode = c10.Code--楼栋位置
left join FxtDataCenter.dbo.SYS_Code c11 with(nolock) on b.SightCode = c11.Code--楼栋景观
left join FxtDataCenter.dbo.SYS_Code c12 with(nolock) on b.FrontCode = c12.Code--楼栋朝向
left join FxtDataCenter.dbo.SYS_Code c13 with(nolock) on b.Wall = c13.Code--外墙装修
left join FxtDataCenter.dbo.SYS_Code c14 with(nolock) on b.innerFitmentCode = c14.Code--内部装修
left join FxtDataCenter.dbo.SYS_Code c15 with(nolock) on b.PipelineGasCode = c15.Code--管道燃气
left join FxtDataCenter.dbo.SYS_Code c16 with(nolock) on b.HeatingModeCode = c16.Code--采暖方式
left join FxtDataCenter.dbo.SYS_Code c17 with(nolock) on b.WallTypeCode = c17.Code--墙体类型
--楼盘
, (
			select ProjectId, ProjectName, SubAreaId, FieldNo, PurposeCode, Address, LandArea, StartDate, UsableYear, BuildingArea, SalableArea, CubageRate, GreenRate, 
                      BuildingDate, CoverDate, SaleDate, JoinDate, EndDate, InnerSaleDate, RightCode, ParkingNumber, AveragePrice, ManagerTel, ManagerPrice, TotalNum, BuildingNum,
                       Detail, BuildingTypeCode, UpdateDateTime, OfficeArea, OtherArea, PlanPurpose, PriceDate, IsComplete, OtherName, SaveDateTime, SaveUser, Weight as ProjectWeight, 
                      BusinessArea, IndustryArea, IsEValue, PinYin, CityID, AreaID, OldId, CreateTime, AreaLineId, SalePrice, PinYinAll, X as ProjectX, Y as ProjectY, XYScale, Creator, 
                      IsEmpty, TotalId, East, West, South, North, BuildingQuality, HousingScale, BuildingDetail, HouseDetail, BasementPurpose, ManagerQuality, Facilities, 
                      AppendageClass, RegionalAnalysis, Wrinkle, Aversion
			from @projecttable p with(nolock)
			where not exists (
						select ProjectId from @projecttable_sub pb where pb.ProjectId = p.ProjectId
						and pb.Fxt_CompanyId = @FxtCompanyId
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
			and p.ProjectId =@ProjectId
			and p.valid = 1
			union
			select ProjectId, ProjectName, SubAreaId, FieldNo, PurposeCode, Address, LandArea, StartDate, UsableYear, BuildingArea, SalableArea, CubageRate, GreenRate, 
                      BuildingDate, CoverDate, SaleDate, JoinDate, EndDate, InnerSaleDate, RightCode, ParkingNumber, AveragePrice, ManagerTel, ManagerPrice, TotalNum, BuildingNum,
                       Detail, BuildingTypeCode, UpdateDateTime, OfficeArea, OtherArea, PlanPurpose, PriceDate, IsComplete, OtherName, SaveDateTime, SaveUser, Weight as ProjectWeight, 
                      BusinessArea, IndustryArea, IsEValue, PinYin, CityID, AreaID, OldId, CreateTime, AreaLineId, SalePrice, PinYinAll, X as ProjectX, Y as ProjectY, XYScale, Creator, 
                      IsEmpty, TotalId, East, West, South, North, BuildingQuality, HousingScale, BuildingDetail, HouseDetail, BasementPurpose, ManagerQuality, Facilities, 
                      AppendageClass, RegionalAnalysis, Wrinkle, Aversion
			from @projecttable_sub pb with(nolock)
			where  pb.Fxt_CompanyId = @FxtCompanyId
			--where  pb.Fxt_CompanyId in (
			--	select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata 
			--	where cityid = @CityId and fxtcompanyid = @FxtCompanyId and typecode = @TypeCode), ',') 
			--	)
			and pb.CityID = @CityId
			and pb.ProjectId =@ProjectId
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