select HouseId,HouseName,BuildArea,IsEvalue,weight from @table_house h with(nolock) where BuildingId=@BuildingId and 
valid=1  and CityId=@CityId and FloorNo=@FloorNo and HouseName<>'' @key and 
h.HouseId not in (select HouseId from @table_house_sub hs with(nolock) where h.HouseId=hs.HouseId 
and hs.FxtCompanyId=@FxtCompanyId and hs.CityId=h.CityId)  AND h.FxtCompanyId IN (SELECT value FROM  dbo.SplitToTable((SELECT ShowCompanyId FROM dbo.Privi_Company_ShowData WHERE CityId=@cityid AND FxtCompanyId=@fxtcompanyid),','))
 union 
select HouseId,HouseName,BuildArea,IsEvalue,weight from @table_house_sub h with(nolock) where BuildingId=@BuildingId 
and valid=1  and CityId=@CityId and FloorNo=@FloorNo and HouseName<>'' and h.FxtCompanyId=@FxtCompanyId @key