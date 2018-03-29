/*
Date: 20150831
Description: 修改EntrustCensusRegister字段CODE表示含义
*/
USE openplatform;

ALTER TABLE `openplatform`.`entrustappraise`   
  CHANGE `EntrustCensusRegister` `EntrustCensusRegister` TINYINT(4) NULL   COMMENT '委托方户籍(1:本市户籍, 0:非本市户籍)';
  
  
-- 更新历史数据
START TRANSACTION;
UPDATE entrustappraise SET EntrustCensusRegister = 0 WHERE EntrustCensusRegister = 2;
COMMIT;

