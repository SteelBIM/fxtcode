/*
Date: 20150811
Description: 修改数据类型
*/

ALTER TABLE person MODIFY `MaritalStatus` VARCHAR(20) NULL DEFAULT NULL COMMENT '婚姻状况';
ALTER TABLE person MODIFY `HasChildren` VARCHAR(5) NULL DEFAULT NULL COMMENT '有无子女';

UPDATE person SET MaritalStatus = '未婚', HasChildren = '无' WHERE personid = 1;
UPDATE person SET MaritalStatus = '已婚', HasChildren = '有' WHERE personid = 2;

COMMIT;