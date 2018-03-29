/*
Date: 20151221
Description: 更正migration_tables表OriginalTable字段部分数据
*/

USE openplatform;

UPDATE migration_tables
    SET OriginalTable = REPLACE(OriginalTable, '_sz', '')
    WHERE OriginalTable LIKE '%sz%';

UPDATE migration_tables
    SET OriginalTable = 'Dat_WaitProject'
    WHERE OriginalTable = 'DAT_WaitProject';

UPDATE migration_tables
    SET OriginalTable = 'Sys_FloorPrice'
    WHERE OriginalTable = 'SYS_FloorPrice';

COMMIT;