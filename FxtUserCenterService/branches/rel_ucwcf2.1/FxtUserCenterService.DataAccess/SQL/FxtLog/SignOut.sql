UPDATE [FxtLog].[dbo].[SYS_Login] with(rowlock) SET [LogOutDate] =@LogOutDate
                           WHERE PasCode=@PasCode and cityid = @cityid