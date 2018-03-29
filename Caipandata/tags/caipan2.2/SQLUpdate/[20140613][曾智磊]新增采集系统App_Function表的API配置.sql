use FxtUserCenter
if not exists(select * from App_Function where AppId=1003106 and FunctionName='getbuildingdetailedbyprojectid')
begin
   insert App_Function(AppId,FunctionName,FunctionDesc)
   select 1003106,'getbuildingdetailedbyprojectid','根据楼盘ID获取楼栋详细列表'
end
go
if not exists(select * from App_Function where AppId=1003106 and FunctionName='gethousedetailedbybuildingid')
begin
   insert App_Function(AppId,FunctionName,FunctionDesc)
   select 1003106,'gethousedetailedbybuildingid','根据楼栋ID获取房号详细列表'
end
