/*
Date: 20150805
Description: 博纳接口新增字段
*/

ALTER TABLE entrustappraise ADD EntrustCensusRegister TINYINT  COMMENT '委托方户籍(1:本市户籍, 2:非本市户籍)' AFTER EntrustIDNum;
ALTER TABLE entrustappraise ADD ClientContactPhone CHAR(20) COMMENT '委托方联系人电话' AFTER ClientContact;
ALTER TABLE entrustappraise ADD LendingBank VARCHAR(200) COMMENT '贷款银行';
ALTER TABLE entrustappraise ADD Guarantor4Mortgage TINYINT COMMENT '按揭贷款是否有共同保证人(1:是, 0:否)';
-- ALTER TABLE entrustappraise ADD MortgageLoanAmount DECIMAL(18, 4) COMMENT '申请银行按揭贷款金额';


UPDATE entrustappraise
    SET EntrustCensusRegister = 1
        ,ClientContactPhone = '13563256756'
        ,LendingBank = '招商银行南海支行'
        ,Guarantor4Mortgage = 0
    WHERE EAID = 1;
COMMIT;