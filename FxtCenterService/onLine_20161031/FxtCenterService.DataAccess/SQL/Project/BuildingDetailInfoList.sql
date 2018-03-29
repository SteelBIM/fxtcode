select b.*
from
--楼栋
(
			select FxtCompanyid,ProjectId, BuildingId, BuildingName, PurposeCode, StructureCode, BuildingTypeCode, TotalFloor, FloorHigh, SaleLicence, ElevatorRate, UnitsNumber, 
                      TotalNumber, TotalBuildArea, BuildDate, SaleDate, AveragePrice, AverageFloor, JoinDate, LicenceDate, OtherName, Weight , IsEValue, CityID, CreateTime, OldId, 
                       SalePrice, SaveDateTime, SaveUser, LocationCode, SightCode, FrontCode, StructureWeight, BuildingTypeWeight, YearWeight, PurposeWeight, LocationWeight, 
                      SightWeight, FrontWeight, X , Y , XYScale, Wall, IsElevator, SubAveragePrice, PriceDetail, BHouseTypeCode, BHouseTypeWeight, Creator, Distance, 
                      DistanceWeight, basement, Remark, ElevatorRateWeight, IsYard, YardWeight, Doorplate, RightCode, IsVirtual, FloorSpread, PodiumBuildingFloor, 
                      PodiumBuildingArea, TowerBuildingArea, BasementArea, BasementPurpose, HouseNumber, HouseArea, OtherNumber, OtherArea, innerFitmentCode, 
                      FloorHouseNumber, LiftNumber, LiftBrand, Facilities, PipelineGasCode, HeatingModeCode, WallTypeCode,
					  MaintenanceCode
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
			and b.valid = 1
			union
			select Fxt_CompanyId as FxtCompanyid, ProjectId, BuildingId, BuildingName, PurposeCode, StructureCode, BuildingTypeCode, TotalFloor, FloorHigh, SaleLicence, ElevatorRate, UnitsNumber, 
                      TotalNumber, TotalBuildArea, BuildDate, SaleDate, AveragePrice, AverageFloor, JoinDate, LicenceDate, OtherName, Weight , IsEValue, CityID, CreateTime, OldId, 
                       SalePrice, SaveDateTime, SaveUser, LocationCode, SightCode, FrontCode, StructureWeight, BuildingTypeWeight, YearWeight, PurposeWeight, LocationWeight, 
                      SightWeight, FrontWeight, X , Y , XYScale, Wall, IsElevator, SubAveragePrice, PriceDetail, BHouseTypeCode, BHouseTypeWeight, Creator, Distance, 
                      DistanceWeight, basement, Remark, ElevatorRateWeight, IsYard, YardWeight, Doorplate, RightCode, IsVirtual, FloorSpread, PodiumBuildingFloor, 
                      PodiumBuildingArea, TowerBuildingArea, BasementArea, BasementPurpose, HouseNumber, HouseArea, OtherNumber, OtherArea, innerFitmentCode, 
                      FloorHouseNumber, LiftNumber, LiftBrand, Facilities, PipelineGasCode, HeatingModeCode, WallTypeCode,
					  MaintenanceCode
			from @buildingtable_sub bb with(nolock)
			where bb.Fxt_CompanyId  = @FxtCompanyId
			and bb.CityID = @CityId
			and bb.valid = 1
			) b 
where ProjectId = @ProjectId
