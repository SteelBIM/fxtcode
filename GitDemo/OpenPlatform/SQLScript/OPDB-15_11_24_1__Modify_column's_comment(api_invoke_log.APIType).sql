/*
Date: 20151124
Description: 修改表列注释：api_invoke_log.APIType
*/

USE openplatform;

ALTER TABLE api_invoke_log MODIFY APIType INT(1) DEFAULT NULL COMMENT 'API类型(1:楼盘, 2:楼栋, 3:房号, 4:案例)'