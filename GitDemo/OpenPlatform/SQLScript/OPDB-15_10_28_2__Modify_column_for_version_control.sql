/*
Date: 20151028
Description: Change admin_interface_config.IfVersionId to not null
*/

USE openplatform;
ALTER TABLE admin_interface_config DROP FOREIGN KEY FK_aic_IfVersionId;
ALTER TABLE admin_interface_config DROP INDEX FK_aic_IfVersionId;
ALTER TABLE admin_interface_config MODIFY IfVersionId INT NOT NULL COMMENT '接口版本ID';
ALTER TABLE admin_interface_config ADD CONSTRAINT FK_aic_IfVersionId FOREIGN KEY (IfVersionId) REFERENCES admin_interface_version(Id);