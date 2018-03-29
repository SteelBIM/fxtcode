/*
Date: 20151015
Description: admin_company, admin_interface_version表部分字段，改为NOT NULL
*/

USE openplatform;

LOCK TABLES admin_company WRITE
	,admin_interface_version WRITE;

ALTER TABLE admin_company DROP FOREIGN KEY FK_IfVersionId;

ALTER TABLE `admin_company`
  MODIFY `CompanyName` VARCHAR(20) NOT NULL   COMMENT '公司名称',
  MODIFY `IfVersionId` INT NOT NULL   COMMENT '接口版本ID';

ALTER TABLE `admin_interface_version`   
  MODIFY `IfVersionName` VARCHAR(20) NOT NULL   COMMENT '接口版本名称',
  MODIFY `BaseUrl` VARCHAR(50) NOT NULL   COMMENT '基类地址';

ALTER TABLE admin_company ADD CONSTRAINT FK_IfVersionId FOREIGN KEY(IfVersionId) REFERENCES admin_interface_version(Id);

UNLOCK TABLES;