select a.*,'' as TrueName,c.CityName from (
select CityId,UserId,ProjectId,BuildingId,HouseId,ProjectName,BuildingName,HouseName,UnitPrice,QueryDate,BuildingArea,TotalPrice,Qid from DAT_QueryHistory with(nolock) where HouseId>0 and ((UserId in (@username) and type=1003001) or (UserId in (@wxopenid) and type=1003110))
union all 
select CityId,UserId,ProjectId,BuildingId,HouseId,ProjectName,BuildingName,HouseName,UnitPrice,QueryDate,BuildingArea,TotalPrice,Qid from DAT_QueryHistory_csj with(nolock) where HouseId>0 and ((UserId in (@username) and type=1003001) or (UserId in (@wxopenid) and type=1003110))
union all 
select CityId,UserId,ProjectId,BuildingId,HouseId,ProjectName,BuildingName,HouseName,UnitPrice,QueryDate,BuildingArea,TotalPrice,Qid from DAT_QueryHistory_hbh with(nolock) where HouseId>0 and ((UserId in (@username) and type=1003001) or (UserId in (@wxopenid) and type=1003110))
union all 
select CityId,UserId,ProjectId,BuildingId,HouseId,ProjectName,BuildingName,HouseName,UnitPrice,QueryDate,BuildingArea,TotalPrice,Qid from DAT_QueryHistory_xb with(nolock) where HouseId>0 and ((UserId in (@username) and type=1003001) or (UserId in (@wxopenid) and type=1003110))
union all 
select CityId,UserId,ProjectId,BuildingId,HouseId,ProjectName,BuildingName,HouseName,UnitPrice,QueryDate,BuildingArea,TotalPrice,Qid from DAT_QueryHistory_zb with(nolock) where HouseId>0 and ((UserId in (@username) and type=1003001) or (UserId in (@wxopenid) and type=1003110))
union all 
select CityId,UserId,ProjectId,BuildingId,HouseId,ProjectName,BuildingName,HouseName,UnitPrice,QueryDate,BuildingArea,TotalPrice,Qid from DAT_QueryHistory_zsj with(nolock) where HouseId>0 and ((UserId in (@username) and type=1003001) or (UserId in (@wxopenid) and type=1003110))
)a 
left join FxtDataCenter.dbo.SYS_City c with(nolock)
on c.CityId = a.CityId