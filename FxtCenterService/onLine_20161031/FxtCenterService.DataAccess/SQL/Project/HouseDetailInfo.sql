select th.*
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