/*
Date: 20150804
Description: 在表EntrustAppraise增加字段：EntrustIDNum(委托人身份证)，EntrustPhone(委托方电话)
*/


USE openplatform;

ALTER TABLE EntrustAppraise ADD EntrustIDNum VARCHAR(50) COMMENT '委托人身份证' AFTER ClientPersonId;
ALTER TABLE EntrustAppraise ADD EntrustPhone VARCHAR(50) COMMENT '委托方电话'  AFTER EntrustIDNum;
UPDATE EntrustAppraise SET EntrustIDNum = 441621198307231211, EntrustPhone = '18664556666' WHERE eaid = 1;
COMMIT;

ALTER TABLE EntrustAppraise DROP COLUMN ClientPersonId;