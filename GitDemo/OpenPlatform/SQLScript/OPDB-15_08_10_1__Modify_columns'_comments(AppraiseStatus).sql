/*
Date: 201508010
Description: 修改表entrustappraise列注释
*/

ALTER TABLE EntrustAppraise MODIFY `AppraiseStatus` TINYINT NULL DEFAULT NULL COMMENT '房产评估状态(1:已完成, 0:未完成)';