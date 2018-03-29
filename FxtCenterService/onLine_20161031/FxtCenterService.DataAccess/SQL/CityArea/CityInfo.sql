select CityId, CityName,CaseMonth,p.ProvinceId,p.ProvinceName,c.zipcode
from FxtDataCenter.dbo.SYS_City c with(nolock)
left join FxtDataCenter.dbo.SYS_Province p with(nolock) 
on c.ProvinceId = p.ProvinceId
where 1=1 @cityidwhere @citynamewhere