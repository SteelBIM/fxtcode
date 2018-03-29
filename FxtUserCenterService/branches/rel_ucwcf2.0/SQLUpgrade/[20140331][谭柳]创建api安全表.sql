use fxtusercenter
--公司标识码
alter table companyinfo add signname varchar(50) null default(newid())

/*
产品的接口配置表（通用）
公司的sianname其实是作为每个公司的唯一KEY使用
*/
create table
dbo.Product_App
(
ProductTypeCode int not null,--云估价产品CODE
AppId int not null,--云估价的API接口CODE
AppPwd varchar(32) not null,--API接口密码，意义不大
AppKey varchar(50) not null,--API接口key
ApiUrl varchar(50) not null --接口URL,
)

/*
公司产品接口配置表（黑名单）
*/
create table
dbo.Product_App_Black
(
ProductTypeCode int not null,--云估价产品CODE
AppId int not null,--云估价的API接口CODE
CompanyId int not null--黑名单公司
)


/*
接口功能配置表（通用）
*/
create table
dbo.App_Function
(
AppId int not null,--云估价的API接口CODE
FunctionName varchar(50) not null,--功能名称
CallBackUrl varchar(200) null,--回调地址
CallBackFormat varchar(500) null,--回调返回数据格式
Parame1 varchar(50) null,--回调需要的参数
Parame2 varchar(50) null,--回调需要的参数
Parame3 varchar(50) null,--回调需要的参数
Parame4 varchar(50) null--回调需要的参数
)

/*
接口功能配置表（黑名单）
*/
create table
dbo.App_Function_Black
(
AppId int not null,--云估价的API接口CODE
FunctionName varchar(50) not null,--功能名称
CompanyId int not null,--黑名单公司
ProductTypeCode int not null,--云估价产品CODE
SplaType varchar(30) null--平台(android、ios、pc)
)
