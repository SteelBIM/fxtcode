
use FxtUserCenter
go
if not exists(select * from App_Function where AppId=1003106 and FunctionName='getfxtbuildingdetailedbyfxtprojectid')
begin
   insert App_Function(AppId,FunctionName,FunctionDesc)
   select 1003106,'getfxtbuildingdetailedbyfxtprojectid','������ʽ��¥��ID��ȡ��ʽ��¥����ϸ�б�'
end
go
if not exists(select * from App_Function where AppId=1003106 and FunctionName='getfxthousedetailedbyfxtbuildingid')
begin
   insert App_Function(AppId,FunctionName,FunctionDesc)
   select 1003106,'getfxthousedetailedbyfxtbuildingid','������ʽ��¥��ID��ȡ������ϸ�б�'
end
go