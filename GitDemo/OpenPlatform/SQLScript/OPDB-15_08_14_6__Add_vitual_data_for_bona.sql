USE openplatform;

-- entrustappraise
DROP TEMPORARY TABLE IF EXISTS entrustappraise_tmp;
CREATE TEMPORARY TABLE entrustappraise_tmp LIKE entrustappraise;
INSERT INTO entrustappraise_tmp SELECT * FROM entrustappraise;

UPDATE entrustappraise ea1
    SET BuyingType =
        (SELECT CASE EAId%2
            WHEN 0 THEN 1
            WHEN 1 THEN 2
            END
        FROM entrustappraise_tmp ea2
        WHERE ea2.eaid = ea1.eaid
        )
       ,EntrustIDNum = rand_idnum_func(eaid);
COMMIT;

-- 
DROP TEMPORARY TABLE IF EXISTS entrustappraise_tmp;
CREATE TEMPORARY TABLE entrustappraise_tmp LIKE entrustappraise;
INSERT INTO entrustappraise_tmp SELECT * FROM entrustappraise;

UPDATE entrustappraise ea1
    SET EntrustCensusRegister =
	(SELECT CASE EAId%3
            WHEN 0 THEN 1
            WHEN 1 THEN 2
            WHEN 2 THEN 2
            END
        FROM entrustappraise_tmp ea2
        WHERE ea2.eaid = ea1.eaid
	);
COMMIT;

-- 
DROP TEMPORARY TABLE IF EXISTS entrustappraise_tmp;
CREATE TEMPORARY TABLE entrustappraise_tmp LIKE entrustappraise;
INSERT INTO entrustappraise_tmp SELECT * FROM entrustappraise;

UPDATE entrustappraise ea1
    SET Guarantor4Mortgage =
	(SELECT CASE EAId%3
            WHEN 0 THEN 1
            WHEN 1 THEN 0
            WHEN 2 THEN 0
            END
        FROM entrustappraise_tmp ea2
        WHERE ea2.eaid = ea1.eaid
	);
COMMIT;

-- person
UPDATE person
	SET Contacts = '王杰'
	WHERE Contacts IS NULL;
COMMIT;