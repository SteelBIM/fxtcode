use FxtTemp
declare @roleId int
select @roleId=ID from sys_role where RoleName='∑÷≈‰»À' and CityID=0 and FxtCompanyId=0
delete SYS_Role_User where roleId=@roleId
delete sys_role where id=@roleId