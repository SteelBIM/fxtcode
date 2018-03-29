USE [FxtUserCenter]
GO

/****** Object:  Table [dbo].[SimplePassWord]    Script Date: 07/30/2014 14:29:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SimplePassWord](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[simplepassword] [nvarchar](30) NULL,
 CONSTRAINT [PK_SimplePassWord] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ºÚµ•√‹¬Î' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SimplePassWord', @level2type=N'COLUMN',@level2name=N'simplepassword'
GO


