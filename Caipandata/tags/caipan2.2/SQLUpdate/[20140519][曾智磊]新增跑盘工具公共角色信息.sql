use FxtTemp

if not exists (select * from SYS_Role where RoleName='�鿱��' and CityID=0 and FxtCompanyID=0)
begin
   insert SYS_Role(RoleName,Remarks,Valid,CreateTime,CityID,FxtCompanyID)
   select '�鿱��','�鿱��',1,getdate(),0,0
end
go
if not exists (select * from SYS_Role where RoleName='������' and CityID=0 and FxtCompanyID=0)
begin
   insert SYS_Role(RoleName,Remarks,Valid,CreateTime,CityID,FxtCompanyID)
   select '������','������',1,getdate(),0,0
end
go
if not exists (select * from SYS_Role where RoleName='�����' and CityID=0 and FxtCompanyID=0)
begin
   insert SYS_Role(RoleName,Remarks,Valid,CreateTime,CityID,FxtCompanyID)
   select '�����','�����',1,getdate(),0,0
end
go
if not exists (select * from SYS_Role where RoleName='����Ա' and CityID=0 and FxtCompanyID=0)
begin
   insert SYS_Role(RoleName,Remarks,Valid,CreateTime,CityID,FxtCompanyID)
   select '����Ա','����Ա',1,getdate(),0,0
end