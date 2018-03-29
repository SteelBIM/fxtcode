
use FxtUserCenter
go
if not exists(select * from App_Function where AppId=1003106 and FunctionName='getfxtbuildingdetailedbyfxtprojectid')
begin
   insert App_Function(AppId,FunctionName,FunctionDesc)
   select 1003106,'getfxtbuildingdetailedbyfxtprojectid','根据正式库楼盘ID获取正式库楼栋详细列表'
end
go
if not exists(select * from App_Function where AppId=1003106 and FunctionName='getfxthousedetailedbyfxtbuildingid')
begin
   insert App_Function(AppId,FunctionName,FunctionDesc)
   select 1003106,'getfxthousedetailedbyfxtbuildingid','根据正式库楼栋ID获取房号详细列表'
end
go