--默认为3个月
select ISNULL(MAX(CaseMonth),3) from dbo.SYS_City with(nolock) where CityId=@cityid
