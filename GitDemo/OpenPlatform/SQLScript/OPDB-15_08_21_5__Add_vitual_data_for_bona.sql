/*
Date: 20150821
Description: 博纳数据补充
*/

DROP TEMPORARY TABLE IF EXISTS entrustobject_tmp;
CREATE TEMPORARY TABLE entrustobject_tmp LIKE entrustobject;
INSERT INTO entrustobject_tmp SELECT * FROM entrustobject;

UPDATE entrustobject eo1
    SET IsSurvey =
        (SELECT CASE eoid%3
            WHEN 0 THEN 1
            WHEN 1 THEN 0
            WHEN 2 THEN 0
            END
        FROM entrustobject_tmp eo2
        WHERE eo2.eoid = eo1.eoid
        );
COMMIT;