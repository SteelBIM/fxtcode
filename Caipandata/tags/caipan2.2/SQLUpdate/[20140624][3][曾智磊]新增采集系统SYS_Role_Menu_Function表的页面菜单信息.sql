
use fxttemp
go
/***给角色对应菜单对应操作新增数据
创建人:曾智磊,日期:2014-06-24
****/
create proc SYS_Role_Menu_Function_Insert
(
    @roleId int,
    @menuUrl nvarchar(200),
    @functionCode int,
    @cityId int,
    @fxtCompanyId int
)
as
begin
    declare @menuId int
    declare @roleMenuId int
    select @menuId=id from SYS_Menu where URL=@menuUrl
    select @roleMenuId=id from SYS_Role_Menu where menuId=@menuId and roleId=@roleId and cityId=@cityId and fxtCompanyId=@fxtCompanyId
    if(@menuId is null or @roleMenuId is null)
    begin
        print '菜单或角色不存在'
    end
    else
    begin
		if not exists(select * from SYS_Role_Menu_Function where RoleMenuID=@roleMenuId and FunctionCode=@functionCode and cityID=@cityId and fxtCompanyId=@fxtCompanyId)
		begin
			insert into SYS_Role_Menu_Function (rolemenuid,functioncode,valid,cityid,fxtcompanyid) 
			values(@roleMenuId,@functionCode,1,@cityId,@fxtCompanyId)
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
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotflowmanager',1301001,0,0--查看自己
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotflowmanager',1301016,0,0--撤销查勘
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotdetailed',1301005,0,0--修改自己
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotdetailed',1301011,0,0--导出自己
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotdetailed',1301010,0,0--导入
go
declare @roleId int --审核人
select @roleId=Id from SYS_Role where roleName='审核人' and valid=1 and cityid=0 and fxtcompanyid=0
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotflowmanager',1301002,0,0--查看小组
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotflowmanager',1301016,0,0--撤销查勘
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotdetailed',1301019,0,0--审核小组
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotdetailed',1301005,0,0--修改自己
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotdetailed',1301011,0,0--导出自己
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotdetailed',1301010,0,0--导入
go
declare @roleId int --小组长
select @roleId=Id from SYS_Role where roleName='小组长' and valid=1 and cityid=0 and fxtcompanyid=0
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotflowmanager',1301002,0,0--查看小组
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotflowmanager',1301015,0,0--撤销任务
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotflowmanager',1301016,0,0--撤销查勘
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotflowmanager',1301014,0,0--分配任务
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotflowmanager',1301010,0,0--导入
exec SYS_Role_Menu_Function_Insert @roleId,'/userinfo/usermanager',1301002,0,0--查看小组
exec SYS_Role_Menu_Function_Insert @roleId,'/departmentinfo/departmentmanager',1301002,0,0--查看小组
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotdetailed',1301019,0,0--审核小组
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotdetailed',1301005,0,0--修改自己
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotdetailed',1301011,0,0--导出自己
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotdetailed',1301010,0,0--导入
go
declare @roleId int --管理员
select @roleId=Id from SYS_Role where roleName='管理员' and valid=1 and cityid=0 and fxtcompanyid=0
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotflowmanager',1301003,0,0--查看全部
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotflowmanager',1301015,0,0--撤销任务
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotflowmanager',1301016,0,0--撤销查勘
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotflowmanager',1301014,0,0--分配任务
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotflowmanager',1301010,0,0--导入
exec SYS_Role_Menu_Function_Insert @roleId,'/userinfo/usermanager',1301003,0,0--查看全部
exec SYS_Role_Menu_Function_Insert @roleId,'/userinfo/usermanager',1301007,0,0--修改全部
exec SYS_Role_Menu_Function_Insert @roleId,'/departmentinfo/departmentmanager',1301003,0,0--查看全部
exec SYS_Role_Menu_Function_Insert @roleId,'/departmentinfo/departmentmanager',1301009,0,0--删除全部
exec SYS_Role_Menu_Function_Insert @roleId,'/departmentinfo/departmentmanager',1301007,0,0--修改全部
exec SYS_Role_Menu_Function_Insert @roleId,'/departmentinfo/departmentmanager',1301004,0,0--新增
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotdetailed',1301020,0,0--审核全部
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotdetailed',1301007,0,0--修改全部
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotdetailed',1301013,0,0--导出全部
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotdetailed',1301010,0,0--导入
go
drop proc SYS_Role_Menu_Function_Insert