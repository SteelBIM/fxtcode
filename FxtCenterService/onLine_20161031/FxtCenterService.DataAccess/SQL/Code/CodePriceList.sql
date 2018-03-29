select ID, CityID, Code, SubCode, CodeName, Price, PurposeCode, TypeCode
from dbo.sys_CodePrice with(nolock)
where 1=1