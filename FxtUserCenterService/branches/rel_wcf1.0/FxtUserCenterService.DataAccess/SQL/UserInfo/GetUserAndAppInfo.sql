select  ui.UserPwd,
ui.CompanyId,
ci.CompanyName,
ui.TrueName,
ci.SignName,
cp.ProductTypeCode,
ci.businessdb,
cp.weburl,
cp.overdate
froM dbo.UserInfo ui with(nolock)
left join dbo.CompanyInfo ci with(nolock)
on ui.CompanyId = ci.CompanyID
left join dbo.CompanyProduct cp with(nolock)
on ci.CompanyID = cp.CompanyId
where ui.Valid =1 and ci.Valid =1 and cp.Valid = 1 and ui.UserName = @username 
