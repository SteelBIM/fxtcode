/*
Date: 20151022
Description: 修改字段注释
*/

USE openplatform;

ALTER TABLE property_transaction_recode MODIFY PrepareLoanAmount DECIMAL(18, 4) NULL COMMENT '拟贷金额(金额: 万元)';