/*
Date: 20151113
Description: 调整版本控制表结构
*/

USE openplatform;

-- ----------------------------------------
-- Drop Table: admin_company  -------------
-- ----------------------------------------

-- 删除相关外键
ALTER TABLE `openplatform`.`admin_company_relation`   
  DROP INDEX `IndirectCompanyId`,
  DROP INDEX `FK_acr_DirectCompanyId`,
  DROP FOREIGN KEY `FK_acr_DirectCompanyId`,
  DROP FOREIGN KEY `FK_acr_IndirectCompanyId`;

ALTER TABLE `openplatform`.`admin_company_relation` 
  ADD CONSTRAINT `FK_admin_company_relation_DirectCompanyId` FOREIGN KEY (`DirectCompanyId`) REFERENCES `openplatform`.`admin_direct_company`(`CompanyId`);

ALTER TABLE `openplatform`.`admin_company_relation` 
  ADD CONSTRAINT `FK_admin_company_relation_IndirectCompanyId` FOREIGN KEY (`IndirectCompanyId`) REFERENCES `openplatform`.`admin_indirect_company`(`CompanyId`);
 
ALTER TABLE `openplatform`.`admin_product`   
  DROP INDEX `FK_ap_CompanyId`,
  DROP FOREIGN KEY `FK_ap_CompanyId`;


-- 删除表
DROP TABLE IF EXISTS admin_company;


-- -----------------------------------------------------
-- Drop Column: admin_interface_config.CompanyId  ------
-- -----------------------------------------------------

ALTER TABLE admin_interface_config DROP COLUMN CompanyId;
