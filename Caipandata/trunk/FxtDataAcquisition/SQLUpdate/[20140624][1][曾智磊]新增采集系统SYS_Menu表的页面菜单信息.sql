
use fxttemp
go
/***����˵���Ϣ
������:������,��������:2014-06-24
******************/
create proc SYS_Menu_Insert
(
   @parentId int,--����ID,not null Ĭ��0
   @menuName nvarchar(50),--ҳ������,not null
   @remark nvarchar(200),--ҳ��˵��,null
   @url nvarchar(200),--ҳ������,not null
   @typecode int,--ҳ������code(ҳ��or�˵�)not null
   @moduleCode int--ģ��code,null
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
	    print '��ҳ����Ϣ�Ѿ�����'
	end
end
go
exec SYS_Menu_Insert 0,'�����б����','���������ѯ�������Ȳ���','/allotflowinfo/allotflowmanager',
1202001,null
go
exec SYS_Menu_Insert 0,'�û��б����','�����û���ѯ���޸ĵȲ���','/userinfo/usermanager',
1202001,null
go
exec SYS_Menu_Insert 0,'С���б����','����С���ѯ���޸ġ������Ȳ���','/departmentinfo/departmentmanager',
1202001,null
go
exec SYS_Menu_Insert 0,'������ϸ��Ϣҳ��','����������Ϣ�޸ġ���˵Ȳ���','/allotflowinfo/allotdetailed',
1202002,null
go
drop proc SYS_Menu_Insert
