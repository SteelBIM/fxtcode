/*
Date: 20151021
Description: 后台管理新建表，及修改字段
*/

USE openplatform;

-- 删除字段admin_company.IfVersionId
ALTER TABLE `admin_company`   
  DROP COLUMN `IfVersionId`, 
  DROP INDEX `FK_IfVersionId`,
  DROP FOREIGN KEY `FK_IfVersionId`;
  
  
-- 新建表
-- admin_product
DROP TABLE IF EXISTS admin_product;
CREATE TABLE admin_product(
	Id INT AUTO_INCREMENT COMMENT '主键ID'
	,ProductName VARCHAR(50) COMMENT '产品名称'
	,IfVersionId INT COMMENT '绑定的接口版本ID' 
	,CompanyId INT COMMENT '所属的公司ID'
	,PRIMARY KEY(Id)
	,CONSTRAINT FK_IfVersionId FOREIGN KEY(IfVersionId) REFERENCES admin_interface_version(Id)
	,CONSTRAINT FK_CompanyId FOREIGN KEY(CompanyId) REFERENCES admin_company(Id)
	)
	COMMENT '产品管理';

-- admin_statistics
DROP TABLE IF EXISTS admin_statistics;
CREATE TABLE admin_statistics(
	Id BIGINT AUTO_INCREMENT COMMENT '主键ID'
	,CompanyId INT COMMENT '公司ID'
	,ProductId INT COMMENT '产品ID'
	,InvokedTime DATETIME COMMENT '调用时间'
	,IP VARCHAR(20) COMMENT '调用端IP'
	,PRIMARY KEY(Id)
	)
	COMMENT '产品使用统计';