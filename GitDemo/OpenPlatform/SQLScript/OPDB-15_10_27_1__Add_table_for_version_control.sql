/*
Date: 20151027
Description: 版本控制新建与修改表
*/

USE openplatform;

-- 修改admin_company表名和注释
DROP TABLE IF EXISTS admin_direct_company;
CREATE TABLE admin_direct_company LIKE admin_company;

ALTER TABLE `admin_direct_company`  
  CHANGE `Id` `Id` INT(11) NOT NULL AUTO_INCREMENT COMMENT '直接公司ID',
  CHANGE `CompanyName` `CompanyName` VARCHAR(50) NOT NULL COMMENT '直接公司名称';

ALTER TABLE `admin_direct_company` COMMENT='直接公司表';


-- 新建table: admin_indirect_company
DROP TABLE IF EXISTS admin_indirect_company;
CREATE TABLE `admin_indirect_company` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT COMMENT '间接公司ID',
  `CompanyName` VARCHAR(50) NOT NULL COMMENT '间接公司名称',
  `DirectCompanyId` INT(11) DEFAULT NULL COMMENT '直接公司ID',
  PRIMARY KEY (`Id`)
)
ENGINE=INNODB
DEFAULT CHARSET=utf8
COMMENT '间接公司表';

ALTER TABLE `admin_indirect_company` ADD CONSTRAINT `FK_aico_DirectCompanyId` FOREIGN KEY (`DirectCompanyId`) REFERENCES `admin_direct_company`(`Id`);


--  新建table: admin_interface_config
DROP TABLE IF EXISTS admin_interface_config;
CREATE TABLE `admin_interface_config`(
  `Id` INT NOT NULL AUTO_INCREMENT COMMENT '主键ID',
  `DirectCompanyId` INT COMMENT '直接公司ID',
  `IndirectCompanyId` INT COMMENT '间接公司ID',
  `IfVersionId` INT COMMENT '接口版本ID',
  `CreateDate` DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `UpdateDate` DATETIME ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
  PRIMARY KEY (`Id`),
  CONSTRAINT `FK_aic_IfVersionId` FOREIGN KEY (`IfVersionId`) REFERENCES `admin_interface_version`(`Id`),
  CONSTRAINT `FK_aic_DirectCompanyId` FOREIGN KEY (`DirectCompanyId`) REFERENCES `admin_direct_company`(`Id`),
  CONSTRAINT `FK_aic_IndirectCompanyId` FOREIGN KEY (`IndirectCompanyId`) REFERENCES `admin_indirect_company`(`Id`)
)
COMMENT='接口版本控制配置表';
