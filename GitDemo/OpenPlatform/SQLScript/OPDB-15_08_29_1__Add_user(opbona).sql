/*
Date: 20150829
Description: 修改博纳接口用户权限, 添加流量控制接口用户EXECUTE权限
*/

DROP USER opbona;

GRANT SELECT ON openplatform.* TO 'opbona'@'%' IDENTIFIED BY 'Op!2015bo-na';