
use FxtUserCenter
if not exists(select * from App_Function where AppId=1003106 and FunctionName='getlnkpphotoseries')
begin
   insert App_Function(AppId,FunctionName,FunctionDesc)
   select 1003106,'getlnkpphotoseries','�ϵ�����_��ȡ�ϴ��Ѿ��ϴ���Ƭ��С'
end
go
if not exists(select * from App_Function where AppId=1003106 and FunctionName='uploadphotoseries')
begin
   insert App_Function(AppId,FunctionName,FunctionDesc)
   select 1003106,'uploadphotoseries','�ϵ�����_�ϴ��ļ�'
end
go
if not exists(select * from App_Function where AppId=1003106 and FunctionName='setuploadphotoover')
begin
   insert App_Function(AppId,FunctionName,FunctionDesc)
   select 1003106,'setuploadphotoover','�ϵ�����_����ȫ��ͼƬ�ϴ����'
end
go