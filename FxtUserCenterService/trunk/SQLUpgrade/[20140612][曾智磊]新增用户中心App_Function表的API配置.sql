use FxtUserCenter
if not exists(select * from App_Function where AppId=1003105 and FunctionName='cptcityids')
begin
   insert App_Function(AppId,FunctionName,FunctionDesc)
   select 1003105,'cptcityids','��ȡ��˾��ͨ��Ʒ�ĳ���ID'
end