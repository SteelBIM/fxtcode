/*
Date: 20151119
Description: 删除user: opii
*/

-- 删除
 CALL mysql.create_db_account_prc('opii', '%', NULL, NULL, NULL, NULL, NULL, NULL, 'drop');