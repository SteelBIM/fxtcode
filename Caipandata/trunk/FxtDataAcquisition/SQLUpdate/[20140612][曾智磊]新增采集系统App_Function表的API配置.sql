use FxtUserCenter
go
---����App_Function������Ϣ--
create proc InsertAppFunction
(
   @appId int,
   @functionName varchar(50),
   @functionDesc nvarchar(200)
)
as
begin
	if not exists(select * from App_Function where AppId=@appId and FunctionName=@functionName)
	begin
	  insert App_Function(AppId,FunctionName,FunctionDesc)
	  select @appId,@functionName,@functionDesc
	end
end
go
use FxtUserCenter
declare @appId int
set @appId=1003106
exec InsertAppFunction @appId,'getallotbyallotid','����ID��ȡ������Ϣ'
exec InsertAppFunction @appId,'setreceiveallot','�����������Ѿ�����'
exec InsertAppFunction @appId,'setallotsurveyingstatus','��������Ϊ�鿱��'
exec InsertAppFunction @appId,'setcancelsurvey','���ó����鿱�е�����'
exec InsertAppFunction @appId,'submitallotsurveydata','�ύ�鿱����(¥��,¥��,����)'
exec InsertAppFunction @appId,'getallotsurveyproject','��ȡδ�鿱����¥��'
exec InsertAppFunction @appId,'getnewallotcount','��ȡ���������'
exec InsertAppFunction @appId,'getallotsurveyingproject','��ȡ�鿱������¥��'
exec InsertAppFunction @appId,'getfxtprojectbylikename','ģ����ѯ��ʽ��¥����Ϣ'
exec InsertAppFunction @appId,'getprojectbyprojectid','����¥��ID��ȡ¥����ϸ��Ϣ'
exec InsertAppFunction @appId,'getfxtprojectbyfxtprojectid','������ʽ��¥��ID��ȡ��ʽ��¥����ϸ��Ϣ'
exec InsertAppFunction @appId,'getbuildingbyprojectid','����¥��ID��ȡ¥���б�'
exec InsertAppFunction @appId,'getbuildingbybuildingid','����¥��ID��ȡ¥����Ϣ'
exec InsertAppFunction @appId,'getfxtbuildingbyfxtprojectid','������ʽ��¥��ID��ȡ��ʽ��¥���б�'
exec InsertAppFunction @appId,'getfxtbuildingbyfxtbuildingid','������ʽ��¥��ID��ȡ��ʽ��¥����Ϣ'
exec InsertAppFunction @appId,'gethousebybuildingid','����¥��ID��ȡ�����б�'
exec InsertAppFunction @appId,'gethousebyhouseid','���ݷ���ID��ȡ������Ϣ'
exec InsertAppFunction @appId,'getfxthousebyfxtbuildingid','������ʽ��¥��ID��ȡ��ʽ�ⷿ���б�'
exec InsertAppFunction @appId,'getfxthousebyfxthouseid','������ʽ�ⷿ��ID��ȡ��ʽ�ⷿ����Ϣ'
exec InsertAppFunction @appId,'getappendagecodelist','��ȡ��������CODEѡ��'
exec InsertAppFunction @appId,'getstructurecodelist','��ȡ�����ṹCODEѡ��'
exec InsertAppFunction @appId,'getbuildinglocationcodelist','��ȡ¥��λ��CODEѡ��'
exec InsertAppFunction @appId,'getfrontcodelist','��ȡ����CODEѡ��'
exec InsertAppFunction @appId,'gethousetypecodelist','��ȡ����CODEѡ��'
exec InsertAppFunction @appId,'getclasscodelist','��ȡ�ȼ�CODEѡ��'
exec InsertAppFunction @appId,'getcitybyname','��ȡ������Ϣ'
exec InsertAppFunction @appId,'getareabycityid','��ȡ�������б�'
exec InsertAppFunction @appId,'getareabyareaid','����ID��ȡ��������Ϣ'
exec InsertAppFunction @appId,'getareabyareaids','���ݶ��ID��ȡ�������б�'
exec InsertAppFunction @appId,'getbasedata','��ȡ���л�������'
exec InsertAppFunction @appId,'setuseradminrole','���û����ù���Ա��ɫ'
go
drop proc InsertAppFunction