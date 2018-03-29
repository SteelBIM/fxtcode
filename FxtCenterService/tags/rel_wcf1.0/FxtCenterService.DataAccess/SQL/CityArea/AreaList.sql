select AreaId, AreaName, CityId, ConstructionCount, GIS_ID, AreaPlacePicName, OldId, X, Y, XYScale, OrderId
from dbo.SYS_Area with(nolock)
where 1=1