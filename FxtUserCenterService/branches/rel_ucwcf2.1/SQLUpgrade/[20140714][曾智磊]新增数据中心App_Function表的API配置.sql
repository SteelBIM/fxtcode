use FxtUserCenter
if not exists(select * from App_Function where AppId=1003104 and FunctionName='importprojectdata')
begin
   insert App_Function(AppId,FunctionName,FunctionDesc)
   select 1003104,'importprojectdata','�Ӳɼ�ϵͳ����¥����Ϣ����������'
end
go
use FxtUserCenter
if not exists(select * from App_Function where AppId=1003104 and FunctionName='addprojectphoto')
begin
   insert App_Function(AppId,FunctionName,FunctionDesc)
   select 1003104,'addprojectphoto','��¥��������Ƭ'
end

