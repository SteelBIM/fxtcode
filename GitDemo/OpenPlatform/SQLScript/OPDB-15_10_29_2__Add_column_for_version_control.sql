/*
Date: 20151029
Description: 新增字段admin_interface_config.Valid
*/

USE openplatform;

ALTER TABLE admin_interface_config ADD Valid BIT(1) DEFAULT 1 COMMENT '是否启用此版本' AFTER IfVersionId;
