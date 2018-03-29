select ci.businessdb,ci.companyid,ci.companycode,ci.companyname,ci.smsloginname,ci.smsloginpassword,ci.smssendname,ci.wxid,ci.wxname,ci.signname from dbo.companyinfo ci with(nolock)
where ci.Valid =1 