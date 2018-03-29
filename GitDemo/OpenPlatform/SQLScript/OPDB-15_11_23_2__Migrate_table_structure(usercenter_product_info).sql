/*
Date: 20151123
Description: 迁移表FxtUserCenter.dbo.ProductInfo至OPDB
*/

USE openplatform;

DROP TABLE IF EXISTS usercenter_product_info;

CREATE TABLE `usercenter_product_info` (
  `ProductTypeCode` INT NOT NULL,
  `ProductName` VARCHAR(50) NOT NULL,
  `ProductDesc` VARCHAR(200) DEFAULT NULL,
  `ProductCurVer` VARCHAR(10) DEFAULT NULL,
  `ProductWebUrl` VARCHAR(200) DEFAULT NULL,
  `FtpPath` VARCHAR(255) DEFAULT NULL,
  `FtpUser` VARCHAR(20) DEFAULT NULL,
  `FtpPassword` VARCHAR(50) DEFAULT NULL,
  PRIMARY KEY (`ProductTypeCode`)
)
COMMENT '产品CODE表(src: FxtUserCenter.dbo.ProductInfo)';
