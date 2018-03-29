
use fxttemp
go
/***插入菜单信息
创建人:曾智磊,创建日期:2014-06-24
******************/
create proc SYS_Menu_Insert
(
   @parentId int,--父级ID,not null 默认0
   @menuName nvarchar(50),--页面名称,not null
   @remark nvarchar(200),--页面说明,null
   @url nvarchar(200),--页面链接,not null
   @typecode int,--页面类型code(页面or菜单)not null
   @moduleCode int--模块code,null
)
as
begin
	if not exists(select * from SYS_Menu where url=@url)
	begin
		insert into SYS_Menu (parentid,menuname,valid,remark,url,typecode,modulecode) 
		values(@parentId,@menuName,1,@remark,@url,@typecode,@moduleCode)   
	end
	else
	begin
	    print '此页面信息已经存在'
	end
end
go
exec SYS_Menu_Insert 0,'任务列表管理','用于任务查询、撤销等操作','/allotflowinfo/allotflowmanager',
1202001,null
go
exec SYS_Menu_Insert 0,'用户列表管理','用于用户查询、修改等操作','/userinfo/usermanager',
1202001,null
go
exec SYS_Menu_Insert 0,'小组列表管理','用于小组查询、修改、新增等操作','/departmentinfo/departmentmanager',
1202001,null
go
exec SYS_Menu_Insert 0,'任务详细信息页面','用于任务信息修改、审核等操作','/allotflowinfo/allotdetailed',
1202002,null
go
drop proc SYS_Menu_Insert
