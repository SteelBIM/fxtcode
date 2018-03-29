use FxtTemp

if not exists (select * from SYS_Role where RoleName='查勘人' and CityID=0 and FxtCompanyID=0)
begin
   insert SYS_Role(RoleName,Remarks,Valid,CreateTime,CityID,FxtCompanyID)
   select '查勘人','查勘人',1,getdate(),0,0
end
go
if not exists (select * from SYS_Role where RoleName='分配人' and CityID=0 and FxtCompanyID=0)
begin
   insert SYS_Role(RoleName,Remarks,Valid,CreateTime,CityID,FxtCompanyID)
   select '分配人','分配人',1,getdate(),0,0
end
go
if not exists (select * from SYS_Role where RoleName='审核人' and CityID=0 and FxtCompanyID=0)
begin
   insert SYS_Role(RoleName,Remarks,Valid,CreateTime,CityID,FxtCompanyID)
   select '审核人','审核人',1,getdate(),0,0
end
go
if not exists (select * from SYS_Role where RoleName='管理员' and CityID=0 and FxtCompanyID=0)
begin
   insert SYS_Role(RoleName,Remarks,Valid,CreateTime,CityID,FxtCompanyID)
   select '管理员','管理员',1,getdate(),0,0
end