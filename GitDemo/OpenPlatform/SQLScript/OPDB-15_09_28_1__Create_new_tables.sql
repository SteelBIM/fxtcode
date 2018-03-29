/*
Date: 20150928
Description: 新建公司管理与接口版本管理表
*/
-- admin_company
DROP TABLE IF EXISTS admin_company;
CREATE TABLE `admin_company`(  
  `Id` INT NOT NULL AUTO_INCREMENT COMMENT '自增主键',
  `CompanyName` VARCHAR(20) COMMENT '公司名称',
  `IfVersionId` INT COMMENT '接口版本ID',
  PRIMARY KEY (`Id`)
)
COMMENT='公司管理';

-- admin_interface_version
DROP TABLE IF EXISTS admin_interface_version;
CREATE TABLE `admin_interface_version`(  
  `Id` INT NOT NULL AUTO_INCREMENT COMMENT '自增主键',
  `IfVersionName` VARCHAR(20) COMMENT '接口版本名称',
  `BaseUrl` VARCHAR(50) COMMENT '基类地址',
  `Description` VARCHAR(100) COMMENT '描述',
  PRIMARY KEY (`Id`)
)
COMMENT='接口版本管理';



