/*
Date: 20150806
Description: 博纳接口新增字段
*/

ALTER TABLE entrustobject ADD SurveyBeginTime DATETIME COMMENT '查勘开始时间';
ALTER TABLE entrustobject ADD SurveyEndTime DATETIME COMMENT '查勘结束时间';
ALTER TABLE sysarea ADD AreaCode VARCHAR(10) COMMENT '行政区Code';


UPDATE entrustobject SET SurveyBeginTime = '2015-07-01 10:20:01', SurveyEndTime = NOW() WHERE EOID = 1;