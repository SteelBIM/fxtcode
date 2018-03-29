/*
Date: 20151125
Description: 迁移表结构
*/

USE openplatform;

DROP TABLE IF EXISTS usercenter_company_product;

CREATE TABLE `usercenter_company_product` (
  `CompanyId` INT(11) DEFAULT NULL COMMENT '机构ID',
  `ProductTypeCode` INT(11) DEFAULT NULL COMMENT '产品CODE',
  `CurrentVersion` VARCHAR(20) DEFAULT NULL COMMENT '当前版本',
  `StartDate` DATETIME DEFAULT NULL COMMENT '生效时间',
  `OverDate` DATETIME DEFAULT NULL COMMENT '到期时间',
  `WebUrl` VARCHAR(100) DEFAULT NULL COMMENT '产品网址',
  `APIUrl` VARCHAR(100) DEFAULT NULL COMMENT 'API',
  `OutAPIUrl` VARCHAR(100) DEFAULT NULL,
  `MsgServer` VARCHAR(100) DEFAULT NULL COMMENT '消息服务器',
  `CreateDate` DATETIME DEFAULT NULL COMMENT '创建日期',
  `Valid` TINYINT(4) DEFAULT NULL COMMENT '有效',
  `CPId` INT(11) DEFAULT NULL,
  `AppAbbreviation` VARCHAR(50) DEFAULT NULL COMMENT '应用简称',
  `CityId` INT(11) DEFAULT NULL COMMENT '产品所在城市',
  `WebUrl1` VARCHAR(100) DEFAULT NULL COMMENT '产品网址1',
  `LogoPath` VARCHAR(100) DEFAULT NULL COMMENT '产品LOGO,52X280',
  `SmallLogoPath` VARCHAR(100) DEFAULT NULL COMMENT '产品小LOGO,52X52',
  `TitleName` VARCHAR(100) DEFAULT NULL COMMENT '对外显示的产品名称',
  `IsExportHose` INT(11) DEFAULT NULL COMMENT '是否可以导出数据中心数据',
  `Telephone` VARCHAR(50) DEFAULT NULL COMMENT '产品联系电话',
  `ShowSubHouse` INT(11) DEFAULT NULL COMMENT '是否显示附属房屋',
  `IsShowDiscountPrice` INT(11) DEFAULT NULL COMMENT '是否显示折扣价',
  `MapHeight` DECIMAL(9,2) DEFAULT NULL,
  `MapWidth` DECIMAL(9,2) DEFAULT NULL,
  `AutoMakeName` INT(11) DEFAULT NULL COMMENT '是否自动生成物业全称；1自动（楼盘名称+楼栋名称+（栋）+楼层+层+房号名称），0原始（地址+楼盘名称+楼栋名称+房号名称）',
  `IsDeleteTrue` INT(11) DEFAULT NULL COMMENT '是否直接删除数据',
  `SkinPath` VARCHAR(100) DEFAULT NULL COMMENT '皮肤',
  `BG_Pic` VARCHAR(100) DEFAULT NULL COMMENT '登录背景图,630X560',
  `HomePage` VARCHAR(200) DEFAULT NULL COMMENT '评估机构主页',
  `TwoDimensionalCode` VARCHAR(200) DEFAULT NULL COMMENT '二维码图片',
  `MaxAccountNumber` INT(11) DEFAULT NULL
  ,PRIMARY KEY(CompanyId, ProductTypeCode, CityId)
)
COMMENT '(src: FxtUserCenter.dbo.CompanyProduct)';
