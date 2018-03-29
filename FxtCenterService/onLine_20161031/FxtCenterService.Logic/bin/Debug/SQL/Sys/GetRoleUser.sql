SELECT [ID]
      ,[RoleID]
      ,[UserName]
      ,[CityID]
      ,[FxtCompanyID]
  FROM [FxtDataCenter].[dbo].[SYS_Role_User]
  where UserName = @UserName
  and FxtCompanyID in (0,@FxtCompanyId)