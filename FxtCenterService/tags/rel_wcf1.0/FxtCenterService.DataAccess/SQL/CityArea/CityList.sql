select CityId, CityName, ProvinceId, CityCode, GIS_ID, ProjectCount, PriceBP, PriceCJ, IsCase, IsEValue, OldId, CaseMonth, X, Y, XYScale, Alias
from dbo.SYS_City with(nolock)
where 1=1