/*
Date: 20150805
Description: 修改表entrustappraise字段ApplicationStatus、BuyingType注释
*/

ALTER TABLE entrustappraise MODIFY `ApplicationStatus` INT(1) NULL DEFAULT NULL COMMENT '银行申请状态(1:申请后, 2:申请前, 3:抵押)';

ALTER TABLE entrustappraise MODIFY `BuyingType` INT(11) NULL DEFAULT NULL COMMENT '贷款类型(1:按揭, 2:抵押)';