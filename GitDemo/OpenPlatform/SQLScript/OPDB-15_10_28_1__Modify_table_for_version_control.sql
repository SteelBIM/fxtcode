/*
Date: 20151028
Description: 修改版本控制相关表
*/

USE openplatform;

-- table: admin_company
ALTER TABLE admin_product
	DROP FOREIGN KEY FK_CompanyId
	,DROP INDEX FK_CompanyId;

ALTER TABLE admin_company MODIFY Id INT AUTO_INCREMENT COMMENT '公司ID';
ALTER TABLE admin_product ADD CONSTRAINT FK_ap_CompanyId FOREIGN KEY(CompanyId) REFERENCES admin_company(Id);


-- table: admin_interface_config

ALTER TABLE admin_interface_config DROP FOREIGN KEY FK_aic_DirectCompanyId;
ALTER TABLE admin_interface_config DROP FOREIGN KEY FK_aic_IndirectCompanyId;
ALTER TABLE admin_interface_config DROP COLUMN DirectCompanyId;
ALTER TABLE admin_interface_config DROP COLUMN IndirectCompanyId;
ALTER TABLE admin_interface_config ADD CompanyId INT NOT NULL UNIQUE COMMENT '公司ID' AFTER IfVersionId;
ALTER TABLE admin_interface_config ADD CONSTRAINT FK_aic_CompanyId FOREIGN KEY(CompanyId) REFERENCES admin_company(Id);


-- drop table
DROP TABLE IF EXISTS admin_indirect_company;
DROP TABLE IF EXISTS admin_direct_company;

-- create table
CREATE TABLE admin_company_relation(
	id INT AUTO_INCREMENT COMMENT '主键ID'
	,DirectCompanyId INT NOT NULL COMMENT '直接公司ID'
	,IndirectCompanyId INT NOT NULL UNIQUE COMMENT '间接公司ID'
	,PRIMARY KEY(Id)
	,CONSTRAINT FK_acr_DirectCompanyId FOREIGN KEY(DirectCompanyId) REFERENCES admin_company(Id)
	,CONSTRAINT FK_acr_IndirectCompanyId FOREIGN KEY(IndirectCompanyId) REFERENCES admin_company(Id)
	)
	COMMENT '公司关系表';



