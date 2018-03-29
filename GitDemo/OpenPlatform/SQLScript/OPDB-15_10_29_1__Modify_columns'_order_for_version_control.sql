/*
Date: 20151029
Description:  更改字段顺序
*/

USE openplatform;

ALTER TABLE admin_interface_config MODIFY COLUMN CompanyId INT NOT NULL COMMENT '公司ID' AFTER Id;