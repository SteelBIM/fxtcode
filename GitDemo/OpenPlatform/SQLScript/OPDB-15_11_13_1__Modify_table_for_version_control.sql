/*
Date: 20151113
Description: 调整版本控制表结构
*/

USE openplatform;

-- ----------------------------------------
-- Create Table: admin_direct_company  ----
-- ----------------------------------------

DROP TABLE IF EXISTS admin_direct_company;

CREATE TABLE admin_direct_company (
  CompanyId INT NOT NULL AUTO_INCREMENT COMMENT '公司ID'
  ,CompanyName VARCHAR(100) NOT NULL COMMENT '公司名称'
  ,CreateDate DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间'
  ,UpdateDate DATETIME DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP COMMENT '修改时间'
  ,PRIMARY KEY (CompanyId)
)
COMMENT='直接公司';

-- 插入旧表数据
INSERT INTO admin_direct_company(CompanyId, CompanyName)
	(
	SELECT Id, CompanyName FROM admin_company WHERE IsDirect = 1
	);


-- ----------------------------------------
-- Create Table: admin_indirect_company  --
-- ----------------------------------------

DROP TABLE IF EXISTS admin_indirect_company;

CREATE TABLE admin_indirect_company (
	CompanyId INT NOT NULL AUTO_INCREMENT COMMENT '公司ID'
	,CompanyName VARCHAR(100) NOT NULL COMMENT '公司名称'
	,CompanyCode VARCHAR(20) COMMENT '公司代码(用户名后缀)'
	,CompanyTypeCode INT COMMENT '公司类型'
	,CreateDate DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间'
	,UpdateDate DATETIME DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP COMMENT '修改时间'
	,PRIMARY KEY(CompanyId)
)
COMMENT '间接公司';

-- 插入旧表数据
INSERT INTO admin_indirect_company(CompanyId, CompanyName)
	(
	SELECT Id, CompanyName FROM admin_company WHERE IsDirect = 0
	);
	
-- ----------------------------------------
-- Alter Table: admin_interface_config  --
-- ----------------------------------------

--  删除原始外键、索引
ALTER TABLE admin_interface_config
	DROP INDEX CompanyId
	,DROP FOREIGN KEY FK_aic_CompanyId;
	
-- 新增列和外键
ALTER TABLE admin_interface_config
	ADD DirectCompanyId INT COMMENT '直接公司ID'
	,ADD IndirectCompanyId INT COMMENT '间接公司ID'
	,ADD CONSTRAINT FK_admin_interface_config_DirectCompanyId FOREIGN KEY(DirectCompanyId) REFERENCES admin_direct_company(CompanyId)
	,ADD CONSTRAINT FK_admin_interface_config_IndirectCompanyId FOREIGN KEY(IndirectCompanyId) REFERENCES admin_indirect_company(CompanyId);
	
ALTER TABLE admin_interface_config
	MODIFY DirectCompanyId INT COMMENT '直接公司ID' AFTER Id
	,MODIFY IndirectCompanyId INT COMMENT '间接公司ID' AFTER DirectCompanyId;
	
-- 修复原始数据
-- 全局设置
/*
START TRANSACTION;
UPDATE admin_interface_config AS aic1
	,(SELECT Id
		,CompanyId
		FROM admin_interface_config
		) AS aic2
	SET aic1.DirectCompanyId = aic2.CompanyId
	WHERE aic1.Id = aic2.Id
	AND EXISTS
		(SELECT 1
			FROM admin_company ac
			WHERE ac.Id = aic2.CompanyId
			AND ac.IsDirect = 1
		);
COMMIT;

-- 个性化设置
START TRANSACTION;
UPDATE admin_interface_config AS aic1
	,(SELECT Id
		,CompanyId
		FROM admin_interface_config
		) AS aic2
	SET aic1.IndirectCompanyId = aic2.CompanyId
		,aic1.DirectCompanyId =
			(SELECT acr.DirectCompanyId
				FROM admin_company_relation acr
				WHERE acr.IndirectCompanyId = aic2.CompanyId
			)
	WHERE aic1.Id = aic2.Id
	AND EXISTS
		(SELECT 1
			FROM admin_company ac
			WHERE ac.Id = aic2.CompanyId
			AND ac.IsDirect = 0
		);
COMMIT;
*/