select top 100 * from fzuums.dbo.[User]  --�û���
select top 100 * from fzuums.dbo.[Tb_UserTelInviTation]  --�û������롢�ֻ���
select top 100 * from fzuums.dbo.[Tb_Class]  --�༶��
select top 100 * from fzuums.dbo.[Tb_Relation]  --ѧ���༶��
select top 100 * from fzuums.dbo.[Tb_UserClass]  --��ʦ�༶��
select top 100 * from MOD_MetaDatabase.dbo.[tb_Code_School]  --ѧУ��
select top 100 * from MOD_MetaDatabase.dbo.[tb_Code_District]  --�����

USE FZ_SynchronousStudy
GO
----------------------�û����༶��ѧУ��Ϣ����----------------------
select 
	a.UserID,b.InvitationNum UserNum,a.UserName,a.PassWord UserPwd,a.UserType,a.TrueName,a.TelePhone, --����(b.TelePhone)
	a.Regdate,
	c.ClassID,d.ClassName,d.ClassNum,d.GradeID,a.SubjectID,e.SchoolID SchID,e.SchoolName SchName, --������ֻ�����༶��ѧУID,�������û���ѧУID��
	e.DistrictID AreaID,
	f.UserID TchID,g.TrueName TchName,f.SubjectID TchSubjectID
into MOD2IBS_temp
from  fzuums.dbo.[User] a
left join  fzuums.dbo.[Tb_UserTelInviTation] b
on a.UserID=b.UserID
left join fzuums.dbo.[Tb_Relation] c
on a.UserID=c.SelfID
left join fzuums.dbo.[Tb_Class] d
on c.ClassID=c.OtherID
left join MOD_MetaDatabase.dbo.[tb_Code_School] e
on d.SchoolID=e.SchoolID
left join fzuums.dbo.[Tb_UserClass] f
on c.ClassID=f.ClassID
left join fzuums.dbo.[User] g
on f.UserID=g.UserID
UNION ALL
select 
	a.UserID,b.InvitationNum UserNum,a.UserName,a.PassWord UserPwd,a.UserType,a.TrueName,a.TelePhone, --����(b.TelePhone)
	a.Regdate,
	c.ClassID,d.ClassName,d.ClassNum,d.GradeID,a.SubjectID,e.SchoolID SchID,e.SchoolName SchName, --������ֻ�����༶��ѧУID,�������û���ѧУID��
	e.DistrictID AreaID,
	f.UserID TchID,g.TrueName TchName,f.SubjectID TchSubjectID
from  fzuums.dbo.[User] a
left join  fzuums.dbo.[Tb_UserTelInviTation] b
on a.UserID=b.UserID
left join fzuums.dbo.[Tb_Relation] c
on a.UserID=c.OtherID
left join fzuums.dbo.[Tb_Class] d
on c.ClassID=c.SelfID
left join MOD_MetaDatabase.dbo.[tb_Code_School] e
on d.SchoolID=e.SchoolID
left join fzuums.dbo.[Tb_UserClass] f
on c.ClassID=f.ClassID
left join fzuums.dbo.[User] g
on f.UserID=g.UserID




----------------------�༶����ʦ��Ϣ����----------------------
select 
	a.ClassID,c.ClassName,c.ClassNum,
	b.UserID TchID,b.TrueName TchName,a.SubjectID TchSubjectID
into MOD2IBS_temptch
from  fzuums.dbo.[Tb_UserClass] a
left join fzuums.dbo.[User] b
on a.UserID=b.UserID
left join fzuums.dbo.[Tb_Class] c
on a.ClassID=c.ID




----------------------������Ϣ����----------------------
select  
	a.SchoolID,a.SchoolName,b.ID AreaID,b.CodeName AreaName	
into MOD2IBS_temparea
from  MOD_MetaDatabase.dbo.[tb_Code_School] a
left join MOD_MetaDatabase.dbo.[tb_Code_District] b
on a.DistrictID=b.ID
union all
select  
	'' SchoolID,'' SchoolName,b.ID AreaID,b.CodeName AreaName
from  MOD_MetaDatabase.dbo.[tb_Code_District] b
where LevelCode=1 or LevelCode=2

----------------------ɾ��----------------------
drop table MOD2IBS_temp
drop table MOD2IBS_temptch
drop table MOD2IBS_temparea