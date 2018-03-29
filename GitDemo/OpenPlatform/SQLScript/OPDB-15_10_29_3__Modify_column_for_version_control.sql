/*
Date: 20151029
Description: 新增字段admin_interface_config.Valid
*/

USE openplatform;

ALTER TABLE admin_interface_config CHANGE Valid IsEnableOPAPI BIT(1) DEFAULT 1 COMMENT '是否开启开放平台接口' AFTER IfVersionId;
