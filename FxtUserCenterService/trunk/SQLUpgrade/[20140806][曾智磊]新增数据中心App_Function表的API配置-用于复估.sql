use FxtUserCenter
if not exists(select * from App_Function where AppId=1003104 and FunctionName='collateralreassessment')
begin
   insert App_Function(AppId,FunctionName,FunctionDesc)
   select 1003104,'collateralreassessment','获取押品复估价格'
end
go

