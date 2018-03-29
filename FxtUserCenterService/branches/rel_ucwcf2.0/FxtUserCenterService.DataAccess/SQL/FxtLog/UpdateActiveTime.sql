UPDATE [FxtLog].[dbo].[SYS_Login] with(rowlock) SET [ActiveTime] =@ActiveTime
            WHERE PasCode=@PasCode 