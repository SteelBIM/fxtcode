/*
Date: 20150805
Description: 修改表entrustappraise字段ApplicationStatus、AppraiseStatus注释
*/

ALTER TABLE entrustappraise MODIFY `ApplicationStatus` INT(1) NULL DEFAULT NULL COMMENT '银行申请状态(1:申请后, 2:申请前，3:抵押)';
ALTER TABLE entrustappraise MODIFY `AppraiseStatus` INT(1) NULL DEFAULT NULL COMMENT '房产评估状态(1:已完成, 2:未完成)';