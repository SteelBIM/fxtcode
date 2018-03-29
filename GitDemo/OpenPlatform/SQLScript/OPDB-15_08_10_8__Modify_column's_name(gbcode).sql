/*
Date: 20150810
Description: 将字段AreaCode名称更改为GBCode
*/

ALTER TABLE sysarea CHANGE COLUMN `AreaCode` GBCode VARCHAR(10) NULL DEFAULT NULL COMMENT '行政区国标码';