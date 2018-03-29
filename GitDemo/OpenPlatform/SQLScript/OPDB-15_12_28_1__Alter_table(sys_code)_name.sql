/*
Date: 20151228
Description: 修改sys_code表名
*/

USE openplatform;

ALTER TABLE sys_code RENAME sys_code_old;
ALTER TABLE sys_code_tmp RENAME sys_code;