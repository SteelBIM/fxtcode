/*
Date: 20151110
Description: admin_company添加字段
*/

USE openplatform;

ALTER TABLE admin_company ADD IsDirect BIT(1) DEFAULT 0 COMMENT '是否为直接公司(1: 是, 0: 否)';