select top 1 * from FxtUserCenter.dbo.CompanyProduct
where CompanyId = @companyid
and ProductTypeCode = @producttypecode
and (CityId = 0 or CityId = @cityid)
and StartDate <= GETDATE()
and OverDate >= GETDATE()
and Valid = 1
order by CityId desc