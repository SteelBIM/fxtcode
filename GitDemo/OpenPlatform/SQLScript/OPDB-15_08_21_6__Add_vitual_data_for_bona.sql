/*
Date: 20150821
Description: 博纳数据生成
*/

-- roomnum
UPDATE entrustobject
    SET roomnum = TRUNCATE(RAND(eoid)*5, 0) + 1
    WHERE roomnum = 0
    OR roomnum IS NULL;
COMMIT;


-- BuildingDate
DROP TEMPORARY TABLE IF EXISTS entrustobject_tmp;
CREATE TEMPORARY TABLE entrustobject_tmp LIKE entrustobject;
INSERT INTO entrustobject_tmp SELECT * FROM entrustobject;

UPDATE entrustobject eo1
    SET BuildingDate =
        (SELECT CASE TRUNCATE((ROUND(eoid) * 10), 0) % 3
            WHEN 0 THEN '80年代'
            WHEN 1 THEN '90年代'
            WHEN 2 THEN CONCAT(2000 + TRUNCATE(RAND(eoid)*14, 0), '年')
            END
        FROM entrustobject_tmp eo2
        WHERE eo2.eoid = eo1.eoid
        );
COMMIT;

-- DecorationValue
UPDATE entrustobject
    SET DecorationValue = TRUNCATE(RAND(eoid) * 100000, 4)
    WHERE DecorationValue IS NULL;
COMMIT;

-- Usage
DROP TEMPORARY TABLE IF EXISTS entrustobject_tmp;
CREATE TEMPORARY TABLE entrustobject_tmp LIKE entrustobject;
INSERT INTO entrustobject_tmp SELECT * FROM entrustobject;

UPDATE entrustobject eo1
    SET `Usage` =
        (SELECT CASE TRUNCATE(((ROUND(eoid) + 10000) * 5), 0) % 3
            WHEN 0 THEN '自住'
            WHEN 1 THEN '自住'
            WHEN 2 THEN '出租或空置'
            END
        FROM entrustobject_tmp eo2
        WHERE eo2.eoid = eo1.eoid
        );
COMMIT;