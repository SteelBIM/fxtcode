use FxtTemp
if not exists (select * from SYS_Role where RoleName='С�鳤' and CityID=0 and FxtCompanyID=0)
begin
   insert SYS_Role(RoleName,Remarks,Valid,CreateTime,CityID,FxtCompanyID)
   select 'С�鳤','С�鳤',1,getdate(),0,0
end