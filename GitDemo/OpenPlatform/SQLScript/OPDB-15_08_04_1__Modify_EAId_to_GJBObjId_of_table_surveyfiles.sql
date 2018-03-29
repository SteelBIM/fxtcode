/*
Date: 20150804
Description: 将surveyfiles表的EAId，更改为GJBObjId
*/

USE openplatform;

ALTER TABLE surveyfiles CHANGE COLUMN EAId GJBObjId BIGINT COMMENT '估价宝委估对象Id';

UPDATE surveyfiles SET GJBObjId = 234344 WHERE GJBObjId = 1;

COMMIT;