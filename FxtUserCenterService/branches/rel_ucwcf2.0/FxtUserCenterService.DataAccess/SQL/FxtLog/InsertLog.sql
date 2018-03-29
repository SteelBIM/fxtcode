INSERT INTO [FxtLog].[dbo].[SYS_Log]
([SysType],[CityId],[FxtCompanyId],[UserId],[UserName],[LogType],[EventType],[ObjectId],[ObjectName],[Remarks],[WebIP]) 
values
(@SysType,@CityId,@FxtCompanyId,@UserId,@UserName,@LogType,@EventType,@ObjectId,@ObjectName,@Remarks,@WebIP)