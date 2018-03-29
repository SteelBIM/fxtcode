select sa.appkey,ci.CompanyID,ci.CompanyName from FxtUserCenter.dbo.CompanyProduct_Safe sa with(nolock)
left join FxtUserCenter.dbo.CompanyProduct cp with(nolock)
on sa.CPid = cp.CPId
left join FxtUserCenter.dbo.CompanyInfo ci with(nolock)
on cp.CompanyId = ci.CompanyID
where  cp.ProductTypeCode = @producttypecode and sa.Valid=1 and cp.Valid=1 and (sa.WebUrl = @validate or sa.sn=@validate)
 and GETDATE() between cp.StartDate and cp.OverDate
 and GETDATE() between sa.startdate and sa.enddate 