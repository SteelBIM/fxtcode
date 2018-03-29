use FxtTemp
if not exists (select * from SYS_Role where RoleName='小组长' and CityID=0 and FxtCompanyID=0)
begin
   insert SYS_Role(RoleName,Remarks,Valid,CreateTime,CityID,FxtCompanyID)
   select '小组长','小组长',1,getdate(),0,0
end