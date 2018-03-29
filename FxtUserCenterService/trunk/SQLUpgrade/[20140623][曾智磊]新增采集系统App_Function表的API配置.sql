
use FxtUserCenter
if not exists(select * from App_Function where AppId=1003106 and FunctionName='getlnkpphotoseries')
begin
   insert App_Function(AppId,FunctionName,FunctionDesc)
   select 1003106,'getlnkpphotoseries','断点续传_获取上次已经上传照片大小'
end
go
if not exists(select * from App_Function where AppId=1003106 and FunctionName='uploadphotoseries')
begin
   insert App_Function(AppId,FunctionName,FunctionDesc)
   select 1003106,'uploadphotoseries','断点续传_上传文件'
end
go
if not exists(select * from App_Function where AppId=1003106 and FunctionName='setuploadphotoover')
begin
   insert App_Function(AppId,FunctionName,FunctionDesc)
   select 1003106,'setuploadphotoover','断点续传_设置全部图片上传完成'
end
go