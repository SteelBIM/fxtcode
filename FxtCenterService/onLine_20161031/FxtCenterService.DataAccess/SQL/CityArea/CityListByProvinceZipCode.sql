select c.CityId,c.CityName,c.zipcode,p.zipcode as pzipcode from FxtDataCenter.dbo.SYS_City c 
left join FxtDataCenter.dbo.SYS_Province p 
on c.ProvinceId = p.ProvinceId 
where c.zipcode is not null and c.zipcode <> ''
and p.zipcode is not null and p.zipcode <> ''