/*
Date: 20151221
Description: 表migration_tables添加字段Valid
*/

USE openplatform;

ALTER TABLE migration_tables ADD Valid BIT(1) NOT NULL DEFAULT 1 COMMENT '数据是否有效' AFTER Extra;


UPDATE migration_tables
    SET Valid = 0
    WHERE OriginalTable IN ('Privi_Company_ShowData', 'Sys_EvalueSet', 'ProductInfo', 'CompanyProduct');
    
COMMIT;