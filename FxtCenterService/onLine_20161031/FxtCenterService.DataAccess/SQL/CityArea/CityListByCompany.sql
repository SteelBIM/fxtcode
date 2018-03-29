select ct.*
from FxtUserCenter.dbo.companyinfo c with(nolock) 
left join FxtUserCenter.dbo.CompanyProduct p with(nolock) 
on c.companyid=p.companyid
and p.StartDate <= GETDATE()
and p.OverDate >= GETDATE()
and p.Valid = 1
left join FxtDataCenter.dbo.SYS_City ct with(nolock) on p.CityId = ct.CityId or p.CityId = 0
where 1=1 and p.ProductTypeCode = @productcode and c.signname=@signname and p.CityId > 0