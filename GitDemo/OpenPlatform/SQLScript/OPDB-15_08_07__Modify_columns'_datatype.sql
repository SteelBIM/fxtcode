/*
Date:20150807
Description: 将字段数据类型更改为VARCHAR类型
*/

ALTER TABLE entrustobject MODIFY `BuildingStructure` VARCHAR(100) NULL DEFAULT NULL COMMENT '建筑结构';
ALTER TABLE entrustobject MODIFY `Fitment` VARCHAR(100) NULL DEFAULT NULL COMMENT '装修';
ALTER TABLE propertytransactionrecode MODIFY `FinancingPurpose` VARCHAR(100) NULL DEFAULT NULL COMMENT '融资目的';


UPDATE entrustobject SET BuildingStructure = '框架结构', Fitment = '简易' WHERE EOID =1;
UPDATE propertytransactionrecode SET FinancingPurpose = '按揭购房' WHERE tranid = '1';


COMMIT;