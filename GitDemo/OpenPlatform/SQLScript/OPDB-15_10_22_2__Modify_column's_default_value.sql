/*
Date: 20151022
Description: 修改字段默认值
*/
USE openplatform;

ALTER TABLE admin_statistics MODIFY InvokedTime DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT '调用时间';