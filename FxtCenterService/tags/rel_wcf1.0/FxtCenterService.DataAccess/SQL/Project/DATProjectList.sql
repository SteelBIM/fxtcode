SELECT [ProjectId],[ProjectName],[SubAreaId]
,SubAreaName=(SELECT TOP 1 SubAreaName FROM SYS_SubArea s where s.SubAreaId=p.SubAreaId)
,[FieldNo],[PurposeCode]
,PurposeName=(SELECT TOP 1 CodeName FROM SYS_Code WHERE Code=PurposeCode AND Id=1001)
,[Address],[LandArea],[StartDate],[UsableYear],[BuildingArea],[SalableArea],[CubageRate],[GreenRate],[BuildingDate],[CoverDate],[SaleDate],[JoinDate],[EndDate],[InnerSaleDate],[RightCode],[ParkingNumber],[AveragePrice],[ManagerTel],[ManagerPrice],[TotalNum],[BuildingNum],[Detail],[BuildingTypeCode]
,BuildingTypeName=(SELECT TOP 1 CodeName FROM FXTProject.dbo.SYS_Code WHERE Code=BuildingTypeCode AND Id=2003)
,[UpdateDateTime],[OfficeArea],[OtherArea],[PlanPurpose],[PriceDate],[IsComplete],[OtherName],[SaveDateTime],[SaveUser],[Weight],[BusinessArea],[IndustryArea],[IsEValue],[PinYin],[CityID],[AreaID],[OldId],[CreateTime],[AreaLineId],[Valid],[SalePrice],[FxtCompanyId],[PinYinAll],[X],[Y],[XYScale]
FROM @table_dat_project as p
where 1=1 @valid and projectid not in (select projectid from @table_dat_project_sub ps where p.ProjectId=ps.ProjectId and 
 (','+cast((select showcompanyid from dbo.privi_company_showdata pc with(nolock) where pc.Fxtcompanyid=@fxtcompanyid and pc.cityid = ps.CityId) as varchar)+',' like '%,' + cast(ps.Fxt_CompanyId as varchar) + ',%')
 and ps.CityId=p.CityId) @where
union 
SELECT [ProjectId],[ProjectName],[SubAreaId]
,SubAreaName=(SELECT TOP 1 SubAreaName FROM SYS_SubArea s where s.SubAreaId=p.SubAreaId),[FieldNo],[PurposeCode]
,PurposeName=(SELECT TOP 1 CodeName FROM SYS_Code WHERE Code=PurposeCode AND Id=1001)
,[Address],[LandArea],[StartDate],[UsableYear],[BuildingArea],[SalableArea],[CubageRate],[GreenRate],[BuildingDate],[CoverDate],[SaleDate],[JoinDate],[EndDate],[InnerSaleDate],[RightCode],[ParkingNumber],[AveragePrice],[ManagerTel],[ManagerPrice],[TotalNum],[BuildingNum],[Detail],[BuildingTypeCode]
,BuildingTypeName=(SELECT TOP 1 CodeName FROM FXTProject.dbo.SYS_Code WHERE Code=BuildingTypeCode AND Id=2003)
,[UpdateDateTime],[OfficeArea],[OtherArea],[PlanPurpose],[PriceDate],[IsComplete],[OtherName],[SaveDateTime],[SaveUser],[Weight],[BusinessArea],[IndustryArea],[IsEValue],[PinYin],[CityID],[AreaID],[OldId],[CreateTime],[AreaLineId],[Valid],[SalePrice],[Fxt_CompanyId] as FxtCompanyId,[PinYinAll],[X],[Y],[XYScale]
FROM @table_dat_project_sub as p
where 1=1 @valid and 
 (','+cast((select showcompanyid from dbo.privi_company_showdata pc with(nolock) where pc.FxtCompanyId=@fxtcompanyid and pc.cityid = p.CityId) as varchar)+',' like '%,' + cast(p.Fxt_CompanyId as varchar) + ',%')
 @where