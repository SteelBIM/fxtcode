use FxtUserCenter
if not exists(select * from App_Function where AppId=1003106 and FunctionName='getallotsurveyoverproject')
begin
   insert App_Function(AppId,FunctionName,FunctionDesc)
   select 1003106,'getallotsurveyoverproject','��ȡ�Ѳ鿱����¥��'
end
go
if not exists(select * from App_Function where AppId=1003106 and FunctionName='getlnkpphotobyprojectid')
begin
   insert App_Function(AppId,FunctionName,FunctionDesc)
   select 1003106,'getlnkpphotobyprojectid','����¥��ID��ȡ¥����Ƭ��Ϣ'
end