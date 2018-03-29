
use fxtTemp   
go          
/***����ɫ��Ӧ�˵�ҳ����������
������:������,����:2014-06-24
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
        print '�˵�������'
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
			print '�����Ѵ���'
		end
	end
end
go
declare @roleId int --�鿱��
select @roleId=Id from SYS_Role where roleName='�鿱��' and valid=1 and cityid=0 and fxtcompanyid=0
exec SYS_Role_Menu_Insert @roleId,'/allotflowinfo/allotflowmanager',0,0
exec SYS_Role_Menu_Insert @roleId,'/allotflowinfo/allotdetailed',0,0
go
declare @roleId int --�����
select @roleId=Id from SYS_Role where roleName='�����' and valid=1 and cityid=0 and fxtcompanyid=0
exec SYS_Role_Menu_Insert @roleId,'/allotflowinfo/allotflowmanager',0,0
exec SYS_Role_Menu_Insert @roleId,'/allotflowinfo/allotdetailed',0,0
go
declare @roleId int --С�鳤
select @roleId=Id from SYS_Role where roleName='С�鳤' and valid=1 and cityid=0 and fxtcompanyid=0
exec SYS_Role_Menu_Insert @roleId,'/allotflowinfo/allotflowmanager',0,0
exec SYS_Role_Menu_Insert @roleId,'/userinfo/usermanager',0,0
exec SYS_Role_Menu_Insert @roleId,'/departmentinfo/departmentmanager',0,0
exec SYS_Role_Menu_Insert @roleId,'/allotflowinfo/allotdetailed',0,0
go
declare @roleId int --����Ա
select @roleId=Id from SYS_Role where roleName='����Ա' and valid=1 and cityid=0 and fxtcompanyid=0
exec SYS_Role_Menu_Insert @roleId,'/allotflowinfo/allotflowmanager',0,0
exec SYS_Role_Menu_Insert @roleId,'/userinfo/usermanager',0,0
exec SYS_Role_Menu_Insert @roleId,'/departmentinfo/departmentmanager',0,0
exec SYS_Role_Menu_Insert @roleId,'/allotflowinfo/allotdetailed',0,0
go

drop proc SYS_Role_Menu_Insert
          