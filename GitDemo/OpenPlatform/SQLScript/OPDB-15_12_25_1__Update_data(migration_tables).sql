/*
Date:20151225
Description: 恢复migration_tables的失效数据为有效数据
*/

UPDATE migration_tables SET Valid = 1 WHERE Valid = 0;
COMMIT;

