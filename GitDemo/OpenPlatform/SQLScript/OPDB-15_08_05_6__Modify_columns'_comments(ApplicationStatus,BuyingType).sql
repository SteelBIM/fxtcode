/*
Date: 20150805
Description: 修改表entrustappraise列注释
*/

ALTER TABLE entrustappraise MODIFY `BuyingType` INT(11) NULL DEFAULT NULL COMMENT '贷款类型(1:抵押, 2:按揭)';
ALTER TABLE entrustappraise MODIFY `ApplicationStatus` INT(1) NULL DEFAULT NULL COMMENT '银行申请状态(1:申请前, 2:申请后)';
UPDATE entrustappraise SET BuyingType = 2, ApplicationStatus = 1 WHERE EAID = 1;
COMMIT;