use FxtUserCenter
if not exists(select * from App_Function where AppId=1003023 and FunctionName='setcollateralsurvey')
begin
   insert App_Function(AppId,FunctionName,FunctionDesc)
   select 1003023,'setcollateralsurvey','更新押品查勘信息'
end
go

if not exists(select * from App_Function where AppId=1003021 and FunctionName='setquerysurvey')
begin
   insert App_Function(AppId,FunctionName,FunctionDesc)
   select 1003021,'setquerysurvey','更新询价查勘信息'
end
go
