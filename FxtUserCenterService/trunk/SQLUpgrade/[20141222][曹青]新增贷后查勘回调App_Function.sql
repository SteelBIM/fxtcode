use FxtUserCenter
if not exists(select * from App_Function where AppId=1003023 and FunctionName='setcollateralsurvey')
begin
   insert App_Function(AppId,FunctionName,FunctionDesc)
   select 1003023,'setcollateralsurvey','����ѺƷ�鿱��Ϣ'
end
go

if not exists(select * from App_Function where AppId=1003021 and FunctionName='setquerysurvey')
begin
   insert App_Function(AppId,FunctionName,FunctionDesc)
   select 1003021,'setquerysurvey','����ѯ�۲鿱��Ϣ'
end
go
