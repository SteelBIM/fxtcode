select CompanyID, CompanyName, CompanyCode, CreateDate, Valid, BusinessDB, SMSLoginName, SMSLoginPassword, SMSSendName, wxid, wxname, SignName, CityId, ShortName, Telephone, WebUrl, Address, EMail, LinkMan, CompanyTypeCode
from dbo.companyinfo c with(nolock)
where c.Valid =1 