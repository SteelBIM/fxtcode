update CompanyProduct with(rowlock) set LogoPath=@logopath,SmallLogoPath=@smalllogopath,
TitleName=@titlename,Telephone=@telephone,bg_pic=@bgpic,homepage=@homepage,twodimensionalcode=@twodimensionalcode
where CompanyId = @companyid and ProductTypeCode = @systypecode