/*
Date: 20150814
Description: 填充部分空数据
*/

-- propertytransactionrecode
UPDATE propertytransactionrecode
    SET TranDate = DATE_ADD('2010-08-12 14:00:00',  INTERVAL  FLOOR(1 + (RAND() * 1000))   DAY )
        ,TranPrice = ROUND(RAND(tranid)*1000000, 4)
        ,PrepareLoanAmount = ROUND(RAND(tranid)*1000000 - RAND(tranid)*100000, 2);
COMMIT;


DROP TEMPORARY TABLE IF EXISTS propertytransactionrecode_tmp;
CREATE TEMPORARY TABLE propertytransactionrecode_tmp LIKE propertytransactionrecode;
INSERT INTO propertytransactionrecode_tmp SELECT * FROM propertytransactionrecode;

UPDATE propertytransactionrecode ptr1
    SET FinancingPurpose =
        (SELECT CASE tranid%5
            WHEN 0 THEN '按揭购房'
            WHEN 1 THEN '经营周转'
            WHEN 2 THEN '个人消费'
            WHEN 3 THEN '股票融资'
            WHEN 4 THEN '其他'
            END
        FROM propertytransactionrecode_tmp ptr2
        WHERE ptr2.TranId = ptr1.TranId
        );
COMMIT;

UPDATE propertytransactionrecode ptr1
    SET isfirstbuy =
        (SELECT CASE tranid%2
            WHEN 0 THEN 0
            WHEN 1 THEN 1
            END
        FROM propertytransactionrecode_tmp ptr2
        WHERE ptr2.TranId = ptr1.TranId
        );
COMMIT;
    
-- propertycertificate
UPDATE propertycertificate
    SET PropertyCertificateRegisteDate = DATE_ADD('2010-05-25 14:00:00',  INTERVAL  FLOOR(1 + (RAND() * 1000))   DAY )
        ,LandCertificateDate = DATE_ADD('2010-03-16 14:00:00',  INTERVAL  FLOOR(1 + (RAND() * 1000))   DAY )
        ,PropertyCertificateRegistePrice = ROUND(RAND(pcid)*1000000, 4)
        ,LandCertificateArea = ROUND(RAND(pcid + 5)*50, 4);
        
COMMIT;

-- appraiseobjectprice
UPDATE appraiseobjectprice
    SET ValueDate = DATE_ADD('2011-02-11 14:00:00',  INTERVAL  FLOOR(1 + (RAND() * 1000))   DAY );
COMMIT;