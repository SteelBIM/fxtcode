use FxtUserCenter
if not exists(select * from App_Function where AppId=1003106 and FunctionName='getallotsurveyoverproject')
begin
   insert App_Function(AppId,FunctionName,FunctionDesc)
   select 1003106,'getallotsurveyoverproject','获取已查勘任务楼盘'
end
go
if not exists(select * from App_Function where AppId=1003106 and FunctionName='getlnkpphotobyprojectid')
begin
   insert App_Function(AppId,FunctionName,FunctionDesc)
   select 1003106,'getlnkpphotobyprojectid','根据楼盘ID获取楼盘照片信息'
end