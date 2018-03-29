/*
Date: 20151116
Description: 添加Table: privi_company_show_data，
	迁移自FxtDataCenter.dbo.Privi_Company_ShowData
*/


USE `openplatform`;

/*Table structure for table `privi_company_show_data` */

CREATE TABLE IF NOT EXISTS `privi_company_show_data` (
  `Id` BIGINT(20) DEFAULT NULL,
  `FxtCompanyId` INT(11) DEFAULT NULL,
  `CityId` INT(11) DEFAULT NULL,
  `ShowCompanyId` LONGTEXT,
  `TypeCode` INT(11) DEFAULT NULL,
  `CaseCompanyId` VARCHAR(100) DEFAULT NULL,
  `BizCompanyId` VARCHAR(100) DEFAULT NULL,
  `BizCaseCompanyId` VARCHAR(100) DEFAULT NULL,
  `OfficeCompanyId` VARCHAR(100) DEFAULT NULL,
  `OfficeCaseCompanyId` VARCHAR(100) DEFAULT NULL,
  `LandCompanyId` VARCHAR(100) DEFAULT NULL,
  `LandCaseCompanyId` VARCHAR(100) DEFAULT NULL,
  `IndustryCompanyId` VARCHAR(100) DEFAULT NULL,
  `IndustryCaseCompanyId` VARCHAR(100) DEFAULT NULL,
  `StampCompanyId` VARCHAR(100) DEFAULT NULL
) 
COMMENT '(src: FxtDataCenter.dbo.Privi_Company_ShowData)';