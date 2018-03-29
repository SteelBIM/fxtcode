/*
Date: 20151105
Description: 修改字段注释
*/

USE openplatform;

ALTER TABLE entrust_appraise MODIFY BusinessStateCode INT NULL COMMENT '委托业务状态(src: sys_code)';