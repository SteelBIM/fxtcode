use FxtUserCenter
if not exists(select * from App_Function where AppId=1003104 and FunctionName='matchingdata')
begin
   insert App_Function(AppId,FunctionName,FunctionDesc)
   select 1003104,'matchingdata','标准化楼盘楼栋房号匹配'
end
go

if not exists(select * from App_Function where AppId=1003104 and FunctionName='provincelist')
begin
   insert App_Function(AppId,FunctionName,FunctionDesc)
   select 1003104,'provincelist','获取省份列表'
end
go

if not exists(select * from App_Function where AppId=1003104 and FunctionName='citylist')
begin
   insert App_Function(AppId,FunctionName,FunctionDesc)
   select 1003104,'citylist','获取城市列表'
end
go
