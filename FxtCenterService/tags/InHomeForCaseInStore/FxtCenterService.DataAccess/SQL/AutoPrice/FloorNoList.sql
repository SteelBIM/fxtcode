select a.*,isnull(c.housecnt,0) as housecnt from (
select distinct FloorNo from @table_house h with(nolock) where BuildingId=@buildingid and valid=1 and CityId=@cityid  
and h.HouseId not in (select HouseId from @table_house_sub hs with(nolock) where h.HouseId=hs.HouseId 
and hs.FxtCompanyId=@fxtcompanyid  and hs.CityId=h.CityId) 
AND h.FxtCompanyId  IN (SELECT value FROM  dbo.SplitToTable((SELECT ShowCompanyId FROM FxtDataCenter.dbo.Privi_Company_ShowData WHERE CityId=@cityid AND FxtCompanyId=@fxtcompanyid and TypeCode = @typecode),',')) 
union select distinct FloorNo from @table_house_sub h with(nolock) where BuildingId=@buildingid and valid=1 
and CityId=@cityid  and h.FxtCompanyId=@fxtcompanyid ) a
left join 
(
select floorno,count(houseid) as housecnt from(
select floorno,houseid from @table_house h with(nolock) where  valid=1  
and buildingid = @buildingid and CityId=@cityid and HouseName<>'' and h.HouseId not in (select HouseId from @table_house_sub hs 
with(nolock) where h.HouseId=hs.HouseId and hs.FxtCompanyId=@fxtcompanyid and hs.CityId=h.CityId)  
AND h.FxtCompanyId IN (SELECT value FROM  dbo.SplitToTable((SELECT ShowCompanyId FROM FxtDataCenter.dbo.Privi_Company_ShowData WHERE CityId=@cityid AND FxtCompanyId=@fxtcompanyid and TypeCode = @typecode),',')) 
union 
select floorno,houseid from @table_house_sub h with(nolock) where BuildingId=@buildingid 
and valid=1  and CityId=@cityid and HouseName<>'' and h.FxtCompanyId=@fxtcompanyid) h group by floorno) c
on c.floorno = a.floorno 
where 1 = 1 and a.FloorNo like @param
