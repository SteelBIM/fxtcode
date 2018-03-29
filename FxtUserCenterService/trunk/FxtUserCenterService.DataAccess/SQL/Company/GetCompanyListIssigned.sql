select CompanyID,CompanyName,issigned,ShortName,CompanyCode,sc.CityId,sc.CityName,sp.Alias as ProvinceName,WebUrl,Telephone,Fax,Address,joindate From CompanyInfo ci with(nolock)
left join FxtDataCenter.dbo.SYS_City sc with(nolock)
on ci.CityId = sc.CityId
left join FxtDataCenter.dbo.SYS_Province sp with(nolock)
on sc.ProvinceId = sp.ProvinceId
where 1=1 and ci.issigned = 1