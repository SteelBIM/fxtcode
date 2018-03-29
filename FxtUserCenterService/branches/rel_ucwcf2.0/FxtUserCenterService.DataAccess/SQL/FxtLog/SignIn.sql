INSERT INTO [FxtLog].[dbo].[SYS_Login] with(rowlock)
([UserId],[FxtCompanyId],[LoginDate],[IPAddress],[PasCode],[SysTypeCode],[CityId],[BrowserType],[ActiveTime])
VALUES
(@UserId,@FxtCompanyId,@LoginDate,@IPAddress,@PasCode,@SysTypeCode,@CityId,@BrowserType,@activeTime)