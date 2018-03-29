/*
Date: 20150810
Description: 修改表entrustobject列属性
*/

ALTER TABLE entrustobject MODIFY `Usage` VARCHAR(50) NULL DEFAULT NULL COMMENT '使用情况(自住, 出租或空置)';
UPDATE EntrustObject SET `Usage` = '自住' WHERE EOID = 1;
COMMIT;