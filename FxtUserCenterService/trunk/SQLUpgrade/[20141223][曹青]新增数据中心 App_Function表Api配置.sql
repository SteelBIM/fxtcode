use FxtUserCenter
if not exists(select * from App_Function where AppId=1003104 and FunctionName='matchingdata')
begin
   insert App_Function(AppId,FunctionName,FunctionDesc)
   select 1003104,'matchingdata','��׼��¥��¥������ƥ��'
end
go

if not exists(select * from App_Function where AppId=1003104 and FunctionName='provincelist')
begin
   insert App_Function(AppId,FunctionName,FunctionDesc)
   select 1003104,'provincelist','��ȡʡ���б�'
end
go

if not exists(select * from App_Function where AppId=1003104 and FunctionName='citylist')
begin
   insert App_Function(AppId,FunctionName,FunctionDesc)
   select 1003104,'citylist','��ȡ�����б�'
end
go
