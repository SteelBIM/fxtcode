select top 1 ci.CompanyName,ci.CompanyCode,ci.SMSSendName,cp.* from CompanyInfo ci with(nolock)
left join CompanyProduct cp with(nolock)
on ci.CompanyID = cp.CompanyId
where ci.Valid= 1 and cp.Valid =1
and (cp.WebUrl = @weburl or cp.WebUrl1 =@weburl1)
order by OverDate desc