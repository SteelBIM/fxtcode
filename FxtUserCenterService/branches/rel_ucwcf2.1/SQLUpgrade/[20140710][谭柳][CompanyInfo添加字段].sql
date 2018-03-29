USE [FxtUserCenter]
GO

alter table [dbo].[CompanyInfo]
add EnglishName varchar(10)


alter table [dbo].[CompanyInfo]
add OtherName nvarchar(50)


alter table [dbo].[CompanyInfo]
add Address nvarchar(100)


alter table [dbo].[CompanyInfo]
add Fax nvarchar(50)


alter table [dbo].[CompanyInfo]
add LegalMan nvarchar(50)


alter table [dbo].[CompanyInfo]
add FK_UserTypeCode int


alter table [dbo].[CompanyInfo]
add HouseAptitudeCode int


alter table [dbo].[CompanyInfo]
add HouseAptitudeFile nvarchar(100)


alter table [dbo].[CompanyInfo]
add LandAptitudeCode nvarchar(100)


alter table [dbo].[CompanyInfo]
add LandAptitudeFile nvarchar(100)


alter table [dbo].[CompanyInfo]
add AssetAptitudeCode int


alter table [dbo].[CompanyInfo]
add AssetAptitudeFile int


alter table [dbo].[CompanyInfo]
add OrganizationCode nvarchar(10)



alter table [dbo].[CompanyInfo]
add Logo nvarchar(100)



alter table [dbo].[CompanyInfo]
add RegNumber nvarchar(20)


alter table [dbo].[CompanyInfo]
add RegBeginDate datetime


alter table [dbo].[CompanyInfo]
add RegEndDate datetime


alter table [dbo].[CompanyInfo]
add CreateDate datetime


alter table [dbo].[CompanyInfo]
add Suspended int


alter table [dbo].[CompanyInfo]
add BusinessNumber nvarchar(50)


alter table [dbo].[CompanyInfo]
add BusinessFile nvarchar(100)