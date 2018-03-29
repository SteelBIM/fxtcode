SELECT [ProjectId],[ProjectName],[SubAreaId]
,SubAreaName=(SELECT TOP 1 SubAreaName FROM FxtDataCenter.dbo.SYS_SubArea s where s.SubAreaId=p.SubAreaId),[FieldNo],[PurposeCode]
,areaname = (select top 1 areaname from fxtdatacenter.dbo.sys_area s where s.areaid=p.areaid)
,PurposeName=(SELECT TOP 1 CodeName FROM FxtDataCenter.dbo.SYS_Code WHERE Code=PurposeCode AND Id=1001)
,[Address],[LandArea],[StartDate],[UsableYear],[BuildingArea],[SalableArea],[CubageRate],[GreenRate],[BuildingDate],[CoverDate],[SaleDate],[JoinDate],[EndDate],[InnerSaleDate],[RightCode],[ParkingNumber],[AveragePrice],[ManagerTel],[ManagerPrice],[TotalNum],[BuildingNum],[Detail],[BuildingTypeCode]
,BuildingTypeName=(SELECT TOP 1 CodeName FROM FxtDataCenter.dbo.SYS_Code WHERE Code=BuildingTypeCode AND Id=2003)
,[UpdateDateTime],[OfficeArea],[OtherArea],[PlanPurpose],[PriceDate],[IsComplete],[OtherName],[SaveDateTime],[SaveUser],[Weight],[BusinessArea],[IndustryArea],[IsEValue],[PinYin],[CityID],[AreaID],[OldId],[CreateTime],[AreaLineId],[Valid],[SalePrice],[FxtCompanyId],[PinYinAll],[X],[Y],[XYScale]
FROM @table_dat_project as p
where 1=1 @valid 
--and projectid not in (select projectid from @table_dat_project_sub ps where p.ProjectId=ps.ProjectId 
 	and not exists(
		select ProjectId from @table_dat_project_sub ps with(nolock)
		where ps.ProjectId = p.ProjectId
		and ps.Fxt_CompanyId = @fxtcompanyid
		and ps.CityID = P.CityID
	)
--and 
-- (','+cast((select showcompanyid from FxtDataCenter.dbo.Privi_Company_ShowData pc with(nolock) where pc.Fxtcompanyid=@fxtcompanyid and pc.cityid = ps.CityId and pc.TypeCode = @typecode) as varchar)+',' like '%,' + cast(ps.Fxt_CompanyId as varchar) + ',%')
-- and ps.CityId=p.CityId)
 and p.fxtcompanyid in (
		select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata 
		where cityid = P.CityID and fxtcompanyid = @fxtcompanyid and typecode = @typecode), ',')
	)
  @where
union 
SELECT [ProjectId],[ProjectName],[SubAreaId]
,SubAreaName=(SELECT TOP 1 SubAreaName FROM FxtDataCenter.dbo.SYS_SubArea s where s.SubAreaId=p.SubAreaId),[FieldNo],[PurposeCode]
,areaname = (select top 1 areaname from fxtdatacenter.dbo.sys_area s where s.areaid=p.areaid)
,PurposeName=(SELECT TOP 1 CodeName FROM FxtDataCenter.dbo.SYS_Code WHERE Code=PurposeCode AND Id=1001)
,[Address],[LandArea],[StartDate],[UsableYear],[BuildingArea],[SalableArea],[CubageRate],[GreenRate],[BuildingDate],[CoverDate],[SaleDate],[JoinDate],[EndDate],[InnerSaleDate],[RightCode],[ParkingNumber],[AveragePrice],[ManagerTel],[ManagerPrice],[TotalNum],[BuildingNum],[Detail],[BuildingTypeCode]
,BuildingTypeName=(SELECT TOP 1 CodeName FROM FxtDataCenter.dbo.SYS_Code WHERE Code=BuildingTypeCode AND Id=2003)
,[UpdateDateTime],[OfficeArea],[OtherArea],[PlanPurpose],[PriceDate],[IsComplete],[OtherName],[SaveDateTime],[SaveUser],[Weight],[BusinessArea],[IndustryArea],[IsEValue],[PinYin],[CityID],[AreaID],[OldId],[CreateTime],[AreaLineId],[Valid],[SalePrice],[Fxt_CompanyId] as FxtCompanyId,[PinYinAll],[X],[Y],[XYScale]
FROM @table_dat_project_sub as p
where 1=1 @valid 
and p.fxt_companyid = @fxtcompanyid
--and 
 --(','+cast((select showcompanyid from FxtDataCenter.dbo.Privi_Company_ShowData pc with(nolock) where pc.FxtCompanyId=@fxtcompanyid and pc.cityid = p.CityId and pc.TypeCode = @typecode) as varchar)+',' like '%,' + cast(p.Fxt_CompanyId as varchar) + ',%')
 -- and p.Fxt_CompanyId in (
	--	select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata 
	--	where cityid = P.CityID and fxtcompanyid = @fxtcompanyid and typecode = @typecode), ',')
	--)

 @where