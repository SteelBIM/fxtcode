select appkey from CompanyInfo ci with(nolock)
join dbo.companyproduct cp with(nolock)
on ci.CompanyID = cp.CompanyId
left join dbo.Product_App cpa with(nolock)
on cp.producttypecode =cpa.producttypecode 
left join dbo.app_function cpaf with(nolock)
on cpa.appid = cpaf.appid
where cpa.appid = @appid 
and cpa.apppwd = @apppwd 
and cp.producttypecode=@systypecode
and ci.SignName = @signname
and cpaf.functionname = @functionname 
 and not exists(
	select 1 from dbo.product_app_black pab with(nolock)
	where cp.producttypecode =pab.producttypecode 
	and cp.CompanyId=pab.CompanyId
	and cpa.appid=pab.appid
	)
 and not exists(
	select 1 from dbo.app_function_black afb with(nolock)
	where cpaf.functionname =afb.functionname 
	and cp.CompanyId=afb.CompanyId
	and cpa.appid=afb.appid
	and cp.producttypecode=afb.producttypecode
	and afb.SplaType=@splatype
	)

--where cpa.appid = @appid and cpa.apppwd = @apppwd and cp.producttypecode=@systypecode and ci.SignName = @signname
 --and cpaf.functionname = @functionname 
