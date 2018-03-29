
use fxttemp
go
/***����ɫ��Ӧ�˵���Ӧ������������
������:������,����:2014-06-24
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
        print '�˵����ɫ������'
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
			print '�����Ѵ���'
		end
	end
end
go
declare @roleId int --�鿱��
select @roleId=Id from SYS_Role where roleName='�鿱��' and valid=1 and cityid=0 and fxtcompanyid=0
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotflowmanager',1301001,0,0--�鿴�Լ�
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotflowmanager',1301016,0,0--�����鿱
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotdetailed',1301005,0,0--�޸��Լ�
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotdetailed',1301011,0,0--�����Լ�
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotdetailed',1301010,0,0--����
go
declare @roleId int --�����
select @roleId=Id from SYS_Role where roleName='�����' and valid=1 and cityid=0 and fxtcompanyid=0
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotflowmanager',1301002,0,0--�鿴С��
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotflowmanager',1301016,0,0--�����鿱
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotdetailed',1301019,0,0--���С��
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotdetailed',1301005,0,0--�޸��Լ�
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotdetailed',1301011,0,0--�����Լ�
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotdetailed',1301010,0,0--����
go
declare @roleId int --С�鳤
select @roleId=Id from SYS_Role where roleName='С�鳤' and valid=1 and cityid=0 and fxtcompanyid=0
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotflowmanager',1301002,0,0--�鿴С��
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotflowmanager',1301015,0,0--��������
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotflowmanager',1301016,0,0--�����鿱
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotflowmanager',1301014,0,0--��������
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotflowmanager',1301010,0,0--����
exec SYS_Role_Menu_Function_Insert @roleId,'/userinfo/usermanager',1301002,0,0--�鿴С��
exec SYS_Role_Menu_Function_Insert @roleId,'/departmentinfo/departmentmanager',1301002,0,0--�鿴С��
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotdetailed',1301019,0,0--���С��
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotdetailed',1301005,0,0--�޸��Լ�
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotdetailed',1301011,0,0--�����Լ�
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotdetailed',1301010,0,0--����
go
declare @roleId int --����Ա
select @roleId=Id from SYS_Role where roleName='����Ա' and valid=1 and cityid=0 and fxtcompanyid=0
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotflowmanager',1301003,0,0--�鿴ȫ��
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotflowmanager',1301015,0,0--��������
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotflowmanager',1301016,0,0--�����鿱
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotflowmanager',1301014,0,0--��������
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotflowmanager',1301010,0,0--����
exec SYS_Role_Menu_Function_Insert @roleId,'/userinfo/usermanager',1301003,0,0--�鿴ȫ��
exec SYS_Role_Menu_Function_Insert @roleId,'/userinfo/usermanager',1301007,0,0--�޸�ȫ��
exec SYS_Role_Menu_Function_Insert @roleId,'/departmentinfo/departmentmanager',1301003,0,0--�鿴ȫ��
exec SYS_Role_Menu_Function_Insert @roleId,'/departmentinfo/departmentmanager',1301009,0,0--ɾ��ȫ��
exec SYS_Role_Menu_Function_Insert @roleId,'/departmentinfo/departmentmanager',1301007,0,0--�޸�ȫ��
exec SYS_Role_Menu_Function_Insert @roleId,'/departmentinfo/departmentmanager',1301004,0,0--����
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotdetailed',1301020,0,0--���ȫ��
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotdetailed',1301007,0,0--�޸�ȫ��
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotdetailed',1301013,0,0--����ȫ��
exec SYS_Role_Menu_Function_Insert @roleId,'/allotflowinfo/allotdetailed',1301010,0,0--����
go
drop proc SYS_Role_Menu_Function_Insert