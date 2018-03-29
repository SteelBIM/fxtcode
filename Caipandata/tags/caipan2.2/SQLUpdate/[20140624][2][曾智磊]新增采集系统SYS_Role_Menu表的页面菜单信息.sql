
use fxtTemp   
go          
/***给角色对应菜单页面新增数据
创建人:曾智磊,日期:2014-06-24
****/
create proc SYS_Role_Menu_Insert
(
	@roleId int,
	@menuUrl nvarchar(200),
	@cityId int,
	@fxtCompanyId int
)
as
begin
    declare @menuId int
    select @menuId=id from SYS_Menu where URL=@menuUrl
    if(@menuId is null)
    begin
        print '菜单不存在'
    end
    else
    begin
		if not exists(select * from SYS_Role_Menu where roleId=@roleId and menuId=@menuId and cityID=@cityId and fxtCompanyId=@fxtCompanyId)
		begin
			insert into SYS_Role_Menu (roleid,menuid,cityid,fxtcompanyid) 
			values(@roleId,@menuId,@cityId,@fxtCompanyId)
		end
		else
		begin
			print '数据已存在'
		end
	end
end
go
declare @roleId int --查勘人
select @roleId=Id from SYS_Role where roleName='查勘人' and valid=1 and cityid=0 and fxtcompanyid=0
exec SYS_Role_Menu_Insert @roleId,'/allotflowinfo/allotflowmanager',0,0
exec SYS_Role_Menu_Insert @roleId,'/allotflowinfo/allotdetailed',0,0
go
declare @roleId int --审核人
select @roleId=Id from SYS_Role where roleName='审核人' and valid=1 and cityid=0 and fxtcompanyid=0
exec SYS_Role_Menu_Insert @roleId,'/allotflowinfo/allotflowmanager',0,0
exec SYS_Role_Menu_Insert @roleId,'/allotflowinfo/allotdetailed',0,0
go
declare @roleId int --小组长
select @roleId=Id from SYS_Role where roleName='小组长' and valid=1 and cityid=0 and fxtcompanyid=0
exec SYS_Role_Menu_Insert @roleId,'/allotflowinfo/allotflowmanager',0,0
exec SYS_Role_Menu_Insert @roleId,'/userinfo/usermanager',0,0
exec SYS_Role_Menu_Insert @roleId,'/departmentinfo/departmentmanager',0,0
exec SYS_Role_Menu_Insert @roleId,'/allotflowinfo/allotdetailed',0,0
go
declare @roleId int --管理员
select @roleId=Id from SYS_Role where roleName='管理员' and valid=1 and cityid=0 and fxtcompanyid=0
exec SYS_Role_Menu_Insert @roleId,'/allotflowinfo/allotflowmanager',0,0
exec SYS_Role_Menu_Insert @roleId,'/userinfo/usermanager',0,0
exec SYS_Role_Menu_Insert @roleId,'/departmentinfo/departmentmanager',0,0
exec SYS_Role_Menu_Insert @roleId,'/allotflowinfo/allotdetailed',0,0
go

drop proc SYS_Role_Menu_Insert
          