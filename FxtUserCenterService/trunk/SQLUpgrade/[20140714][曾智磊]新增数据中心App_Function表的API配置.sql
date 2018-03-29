use FxtUserCenter
if not exists(select * from App_Function where AppId=1003104 and FunctionName='importprojectdata')
begin
   insert App_Function(AppId,FunctionName,FunctionDesc)
   select 1003104,'importprojectdata','从采集系统导入楼盘信息到数据中心'
end
go
use FxtUserCenter
if not exists(select * from App_Function where AppId=1003104 and FunctionName='addprojectphoto')
begin
   insert App_Function(AppId,FunctionName,FunctionDesc)
   select 1003104,'addprojectphoto','给楼盘新增照片'
end

