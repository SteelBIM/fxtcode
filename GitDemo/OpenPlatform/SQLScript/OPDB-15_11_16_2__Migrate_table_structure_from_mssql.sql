/*
Date: 20151116
Description: MSSQL迁移新表至MySQL
	1. FxtDataCenter.dbo.Sys_EvalueSet
	2. 
*/

USE openplatform;

-- sys_evalue_set
DROP TABLE IF EXISTS sys_evalue_set;
CREATE TABLE `sys_evalue_set` (
  `Id` INT(11) DEFAULT NULL,
  `FxtCompanyId` INT(11) DEFAULT NULL,
  `CityId` INT(11) DEFAULT NULL,
  `PurposeCode` INT(11) DEFAULT NULL,
  `TypeCode` INT(11) DEFAULT NULL,
  `ValueCode` INT(11) DEFAULT NULL,
  `Value1` DOUBLE DEFAULT NULL,
  `Value2` DOUBLE DEFAULT NULL,
  `Valid` TINYINT(4) DEFAULT NULL COMMENT '(src: valid)',
  `CreateDate` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `UpdateDate` DATETIME NULL ON UPDATE CURRENT_TIMESTAMP,
  `CreateUser` VARCHAR(50) DEFAULT NULL,
  `UpdateUser` VARCHAR(50) DEFAULT NULL
)
COMMENT '(src: FxtDataCenter.dbo.Sys_EvalueSet)';


-- base_project_avg_price
DROP TABLE IF EXISTS base_project_avg_price;
CREATE TABLE `base_project_avg_price` (
  `Id` BIGINT(20) DEFAULT NULL,
  `CityId` INT(11) DEFAULT NULL,
  `FxtCompanyId` INT(11) DEFAULT NULL,
  `AreaId` INT(11) DEFAULT NULL,
  `SubAreaId` INT(11) DEFAULT NULL,
  `ProjectId` INT(11) DEFAULT NULL,
  `AvgPriceDate` VARCHAR(20) DEFAULT NULL,
  `AvgPrice` INT(11) DEFAULT NULL,
  `BuildingAreaType` INT(11) DEFAULT NULL,
  `PurposeType` INT(11) DEFAULT NULL,
  `BuildingTypeCode` INT(11) DEFAULT NULL,
  `CreateDate` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '(src: FXTProject.dbo.DAT_ProjectAvgPrice.CreateTime)',
  `JSFS` VARCHAR(200) DEFAULT NULL,
  `DateRange` INT(11) DEFAULT NULL
)
COMMENT '(src: FXTProject.dbo.DAT_ProjectAvgPrice)';
