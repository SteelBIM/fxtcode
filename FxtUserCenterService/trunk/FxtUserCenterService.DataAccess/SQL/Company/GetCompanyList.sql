select c.CompanyID, c.CompanyName, c.CompanyCode, c.CreateDate, c.Valid, c.BusinessDB, c.SMSLoginName, c.SMSLoginPassword, c.SMSSendName, c.wxid, c.wxname, c.SignName, c.CityId, c.ShortName, c.Telephone, c.WebUrl, c.Address, c.EMail, c.LinkMan, c.CompanyTypeCode
from dbo.companyinfo c with(nolock)
@prowhere
where c.Valid =1 