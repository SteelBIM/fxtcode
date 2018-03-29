/*
Date: 20151118
Description: 博纳帐号添加部分基础表查询权限
*/

CALL mysql.create_db_account_prc('opbona', '%', NULL, 'SELECT', NULL, 'openplatform.sys_area', NULL, NULL, 'add');
-- CALL mysql.create_db_account_prc('opbona', '%', NULL, 'SELECT', NULL, 'openplatform.sys_area_coordinate', NULL, NULL, 'add');
-- CALL mysql.create_db_account_prc('opbona', '%', NULL, 'SELECT', NULL, 'openplatform.sys_area_line', NULL, NULL, 'add');
CALL mysql.create_db_account_prc('opbona', '%', NULL, 'SELECT', NULL, 'openplatform.sys_city', NULL, NULL, 'add');
CALL mysql.create_db_account_prc('opbona', '%', NULL, 'SELECT', NULL, 'openplatform.sys_code', NULL, NULL, 'add');
CALL mysql.create_db_account_prc('opbona', '%', NULL, 'SELECT', NULL, 'openplatform.sys_province', NULL, NULL, 'add');
-- CALL mysql.create_db_account_prc('opbona', '%', NULL, 'SELECT', NULL, 'openplatform.sys_subarea', NULL, NULL, 'add');
-- CALL mysql.create_db_account_prc('opbona', '%', NULL, 'SELECT', NULL, 'openplatform.sys_subarea_coordinate', NULL, NULL, 'add');