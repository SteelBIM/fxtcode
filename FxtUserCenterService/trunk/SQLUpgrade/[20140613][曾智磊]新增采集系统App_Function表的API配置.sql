use FxtUserCenter
if not exists(select * from App_Function where AppId=1003106 and FunctionName='getbuildingdetailedbyprojectid')
begin
   insert App_Function(AppId,FunctionName,FunctionDesc)
   select 1003106,'getbuildingdetailedbyprojectid','����¥��ID��ȡ¥����ϸ�б�'
end
go
if not exists(select * from App_Function where AppId=1003106 and FunctionName='gethousedetailedbybuildingid')
begin
   insert App_Function(AppId,FunctionName,FunctionDesc)
   select 1003106,'gethousedetailedbybuildingid','����¥��ID��ȡ������ϸ�б�'
end
