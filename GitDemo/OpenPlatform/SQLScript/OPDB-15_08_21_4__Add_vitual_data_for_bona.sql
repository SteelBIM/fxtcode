
-- mysqldump -utyoung -padmin123 openplatform entrust_temp | mysql -h 192.168.0.33 -u dbmanager -p'op2015!manager' openplatform
-- 更新entrustobject表SurveyBeginTime，SurveyEndTime数据
UPDATE entrustobject eo
    SET eo.SurveyBeginTime
        =(SELECT et.SurveyBeginDate
            FROM entrust_temp et
            WHERE et.GJBEntrustId = eo.GJBEntrustId
            AND eo.SurveyBeginTime IS NOT NULL
            GROUP BY et.GJBEntrustId
        );
        
COMMIT;

UPDATE entrustobject eo
    SET eo.SurveyEndTime
        =(SELECT et.SurveyEndDate
            FROM entrust_temp et
            WHERE et.GJBEntrustId = eo.GJBEntrustId
            AND eo.SurveyEndTime IS NOT NULL
            GROUP BY et.GJBEntrustId
        );
        
COMMIT;

UPDATE entrustobject
    SET SurveyBeginTime = DATE_ADD('2014-08-25 00:00:00',  INTERVAL  FLOOR(1 + (RAND(eoid) * 100))   HOUR )
        ,SurveyEndTime = DATE_ADD('2014-08-25 00:00:00',  INTERVAL  FLOOR(1 + (RAND(eoid) * 100))   HOUR )
    WHERE SurveyBeginTime IS NULL
    OR SurveyBeginTime = '0000-00-00 00:00:00'
    OR SurveyEndTime IS NULL
    OR SurveyEndTime = '0000-00-00 00:00:00';
COMMIT;