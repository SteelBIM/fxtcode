SELECT		
	case when T1.CityId = 0 then T2.CityID
	when T1.CityId <> 0 and T2.CityID is not null then T1.CityId end as cityid
FROM (
	select distinct CityId from FxtUserCenter.dbo.CompanyProduct WITH (NOLOCK)
	where CompanyId = @companyid and ProductTypeCode = @productCode  and Valid = 1 and OverDate > GETDATE()
)T1
left join (
	select distinct CityId from FxtUserCenter.dbo.CompanyProduct_Module WITH (NOLOCK)
	where CompanyId = @companyid AND ProductTypeCode = @productCode AND Valid = 1 AND OverDate > getdate()
)T2 on T1.CityId = T2.CityID or T1.CityId = 0 or T2.CityID = 0