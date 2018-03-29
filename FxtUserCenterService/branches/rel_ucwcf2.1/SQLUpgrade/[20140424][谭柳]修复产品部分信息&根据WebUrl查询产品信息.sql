--修复产品部分信息
INSERT INTO [FxtUserCenter].[dbo].[App_Function]
           ([AppId]
           ,[FunctionName]
           ,[CallBackUrl]
           ,[CallBackFormat]
           ,[Parame1]
           ,[Parame2]
           ,[Parame3]
           ,[Parame4]
           ,[FunctionDesc])
     VALUES
           (1003105
           ,'modifyproductpartinfo'
           ,''
           ,''
           ,''
           ,''
           ,''
           ,''
           ,'修复产品部分信息')

---根据WebUrl查询产品信息
INSERT INTO [FxtUserCenter].[dbo].[App_Function]
           ([AppId]
           ,[FunctionName]
           ,[CallBackUrl]
           ,[CallBackFormat]
           ,[Parame1]
           ,[Parame2]
           ,[Parame3]
           ,[Parame4]
           ,[FunctionDesc])
     VALUES
           (1003105
           ,'productinfobyurl'
           ,''
           ,''
           ,''
           ,''
           ,''
           ,''
           ,'根据WebUrl查询产品信息')

select *from [FxtUserCenter].[dbo].[App_Function]
