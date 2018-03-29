/*
Date:20150804
Description: 将entrustobject表的Surveyor，IsSurvey
    移到EntrustObject表
*/

USE openplatform;

ALTER TABLE EntrustObject ADD `Surveyor` VARCHAR(20) NULL DEFAULT NULL COMMENT '查勘员';
ALTER TABLE EntrustObject ADD `IsSurvey` BIT(1) NULL DEFAULT NULL COMMENT '是否现场查勘';

UPDATE entrustobject
    SET Surveyor = '杨涛'
    ,IsSurvey = 1
    WHERE gjbobjid = 234344;
COMMIT;

ALTER TABLE entrustappraise DROP COLUMN Surveyor;
ALTER TABLE entrustappraise DROP COLUMN IsSurvey;