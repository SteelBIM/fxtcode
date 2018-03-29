select cs.*,ci.companyName from  FxtDataCenter.dbo.privi_company_showdata cs with(nolock) 
inner join FxtUserCenter.dbo.CompanyInfo ci with(nolock) on ci.companyId = cs.fxtcompanyId 
where cs.cityId = @cityId and cs.typecode = @typecode
