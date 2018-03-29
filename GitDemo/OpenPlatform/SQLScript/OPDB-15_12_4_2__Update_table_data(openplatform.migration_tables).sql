/*
Date: 20151204
Description: 更新表migration_tables数据
*/

USE openplatform;

UPDATE migration_tables SET Extra = '仅迁移了部分列', WhereClause = NULL WHERE OriginalDatabase = 'FxtUserCenter' AND OriginalTable = 'CompanyInfo';


UPDATE migration_tables SET WhereClause = NULL WHERE WhereClause = '';

COMMIT;
