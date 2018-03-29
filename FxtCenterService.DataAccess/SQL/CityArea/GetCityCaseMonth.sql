--默认为3个月
select ISNULL(MAX(CaseMonth),3) from FxtDataCenter.dbo.SYS_City with(nolock) where CityId=@cityid
