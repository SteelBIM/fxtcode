use FxtUserCenter
go
---给表App_Function插入信息--
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
exec InsertAppFunction @appId,'getallotbyallotid','根据ID获取任务信息'
exec InsertAppFunction @appId,'setreceiveallot','设置新任务已经接收'
exec InsertAppFunction @appId,'setallotsurveyingstatus','设置任务为查勘中'
exec InsertAppFunction @appId,'setcancelsurvey','设置撤销查勘中的任务'
exec InsertAppFunction @appId,'submitallotsurveydata','提交查勘数据(楼盘,楼栋,房号)'
exec InsertAppFunction @appId,'getallotsurveyproject','获取未查勘任务楼盘'
exec InsertAppFunction @appId,'getnewallotcount','获取新任务个数'
exec InsertAppFunction @appId,'getallotsurveyingproject','获取查勘中任务楼盘'
exec InsertAppFunction @appId,'getfxtprojectbylikename','模糊查询正式库楼盘信息'
exec InsertAppFunction @appId,'getprojectbyprojectid','根据楼盘ID获取楼盘详细信息'
exec InsertAppFunction @appId,'getfxtprojectbyfxtprojectid','根据正式库楼盘ID获取正式库楼盘详细信息'
exec InsertAppFunction @appId,'getbuildingbyprojectid','根据楼盘ID获取楼栋列表'
exec InsertAppFunction @appId,'getbuildingbybuildingid','根据楼栋ID获取楼栋信息'
exec InsertAppFunction @appId,'getfxtbuildingbyfxtprojectid','根据正式库楼盘ID获取正式库楼栋列表'
exec InsertAppFunction @appId,'getfxtbuildingbyfxtbuildingid','根据正式库楼栋ID获取正式库楼栋信息'
exec InsertAppFunction @appId,'gethousebybuildingid','根据楼栋ID获取房号列表'
exec InsertAppFunction @appId,'gethousebyhouseid','根据房号ID获取房号信息'
exec InsertAppFunction @appId,'getfxthousebyfxtbuildingid','根据正式库楼栋ID获取正式库房号列表'
exec InsertAppFunction @appId,'getfxthousebyfxthouseid','根据正式库房号ID获取正式库房号信息'
exec InsertAppFunction @appId,'getappendagecodelist','获取配套类型CODE选项'
exec InsertAppFunction @appId,'getstructurecodelist','获取建筑结构CODE选项'
exec InsertAppFunction @appId,'getbuildinglocationcodelist','获取楼栋位置CODE选项'
exec InsertAppFunction @appId,'getfrontcodelist','获取朝向CODE选项'
exec InsertAppFunction @appId,'gethousetypecodelist','获取户型CODE选项'
exec InsertAppFunction @appId,'getclasscodelist','获取等级CODE选项'
exec InsertAppFunction @appId,'getcitybyname','获取城市信息'
exec InsertAppFunction @appId,'getareabycityid','获取行政区列表'
exec InsertAppFunction @appId,'getareabyareaid','根据ID获取行政区信息'
exec InsertAppFunction @appId,'getareabyareaids','根据多个ID获取行政区列表'
exec InsertAppFunction @appId,'getbasedata','获取所有基础数据'
exec InsertAppFunction @appId,'setuseradminrole','给用户设置管理员角色'
go
drop proc InsertAppFunction