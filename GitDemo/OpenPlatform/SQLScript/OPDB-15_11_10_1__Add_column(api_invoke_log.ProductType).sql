/*
Date: 20151110
Description: 日志表新增字段
*/

USE openplatform;

ALTER TABLE api_invoke_log ADD ProductType INT NULL COMMENT '产品类型';


ALTER TABLE api_invoke_log MODIFY ProductType INT NULL COMMENT '产品类型' AFTER APIType;