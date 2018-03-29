use FxtUserCenter
if not exists(select * from App_Function where AppId=1003105 and FunctionName='cptcityids')
begin
   insert App_Function(AppId,FunctionName,FunctionDesc)
   select 1003105,'cptcityids','获取公司开通产品的城市ID'
end