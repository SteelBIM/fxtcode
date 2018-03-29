  --沟通反馈
  Insert Into [FxtUserCenter].[dbo].[App_Function](
		[AppId]
      ,[FunctionName]
      ,[CallBackUrl]
      ,[CallBackFormat]
      ,[Parame1]
      ,[Parame2]
      ,[Parame3]
      ,[Parame4]
      ,[FunctionDesc])
  Values(1003100,'feedback','','','','','','','沟通反馈')
  
  --开始查勘时调用的接口
  Insert Into [FxtUserCenter].[dbo].[App_Function](
		[AppId]
      ,[FunctionName]
      ,[CallBackUrl]
      ,[CallBackFormat]
      ,[Parame1]
      ,[Parame2]
      ,[Parame3]
      ,[Parame4]
      ,[FunctionDesc])
  Values(1003100,'startsurvey','','','','','','','开始查勘时调用的接口')