/*
Date: 20151030
Description: 删除创建、更新时间默认值
*/

USE openplatform;

ALTER TABLE admin_interface_config
	MODIFY CreateDate DATETIME COMMENT '创建时间'
	,MODIFY UpdateDate DATETIME COMMENT '更新时间';