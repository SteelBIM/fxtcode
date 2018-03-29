select 
cpa.AppId,
cpa.AppKey,
cpa.AppPwd,
cpa.ApiUrl,
ui.isinner
froM dbo.UserInfo ui with(nolock)
left join dbo.CompanyInfo ci with(nolock)
on ui.CompanyId = ci.CompanyID
left join dbo.CompanyProduct cp with(nolock)
on ci.CompanyID = cp.CompanyId
left join dbo.Product_App cpa with(nolock)
on cp.producttypecode =cpa.producttypecode
where ui.Valid =1 and ci.Valid =1 and cp.Valid = 1 
and not exists(
	select 1 from dbo.product_app_black pab with(nolock)
	where cp.producttypecode =pab.producttypecode 
	and cp.CompanyId=pab.CompanyId
	and cpa.appid=pab.appid
	)
and ui.UserName = @username
