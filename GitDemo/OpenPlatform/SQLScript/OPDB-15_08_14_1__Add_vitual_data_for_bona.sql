-- 新建产权数据临时表，并将原始产权数据插入
DROP TABLE IF EXISTS property_tmp;
CREATE TABLE property_tmp(ID INT AUTO_INCREMENT PRIMARY KEY
    ,GJBObjId BIGINT(20) COMMENT '估价宝委估对象ID'
    ,`PersonName` VARCHAR(20) NULL COMMENT '姓名'
    ,`Birthday` DATETIME NULL DEFAULT NULL COMMENT '生日');
    
INSERT INTO property_tmp(GJBObjId, PersonName, Birthday)
    SELECT GJBObjId
        ,PersonName
        ,Birthday
        FROM entrustobject_temp
        WHERE GJBEntrustId <> 0
        GROUP BY PersonName, GJBObjId;


-- 拆分多个产权人，并重新写回原表
-- get_split_string_num_func
DELIMITER $$

DROP FUNCTION IF EXISTS get_split_string_num_func$$

CREATE FUNCTION get_split_string_num_func(
    v_string VARCHAR(2000)
    ,v_delimiter VARCHAR(10)
    )
    RETURNS INT
    LANGUAGE SQL
    DETERMINISTIC
    NO SQL
    SQL SECURITY INVOKER
    COMMENT '获取分割字符串数量'
BEGIN
    RETURN CHAR_LENGTH(v_string) - CHAR_LENGTH(REPLACE(v_string, v_delimiter, '')) +1;
END$$

DELIMITER ;


-- get_split_string_func
DELIMITER $$

DROP FUNCTION IF EXISTS get_split_string_func$$
CREATE FUNCTION get_split_string_func(
    v_string VARCHAR(2000)
    ,v_delimiter VARCHAR(10)
    ,v_order INT
    )
    RETURNS VARCHAR(200) CHARSET utf8
    LANGUAGE SQL
    DETERMINISTIC
    SQL SECURITY INVOKER
    COMMENT '获取第n个分割的字符串'
BEGIN
    
    DECLARE v_char_len_before INT DEFAULT 0;
    DECLARE v_char_len INT DEFAULT 0;
    
    SET v_char_len_before = CHAR_LENGTH(SUBSTRING_INDEX(v_string, v_delimiter, v_order - 1));
    SET v_char_len = CHAR_LENGTH(SUBSTRING_INDEX(v_string, v_delimiter, v_order));
    RETURN TRIM(LEADING v_delimiter FROM SUBSTR(v_string, v_char_len_before + 1, v_char_len - v_char_len_before));
    
END$$

DELIMITER ;


-- split_propertyowner_prc
DELIMITER $$

DROP PROCEDURE IF EXISTS split_propertyowner_prc$$

CREATE PROCEDURE split_propertyowner_prc()
    LANGUAGE SQL
    SQL SECURITY INVOKER
    COMMENT '拆分多个产权人'
BEGIN
    DECLARE v_person VARCHAR(255) DEFAULT '';
    DECLARE v_objid INT DEFAULT 0;
    DECLARE v_birth DATETIME;
    
    DECLARE v_count INT DEFAULT 0;
    DECLARE i INT DEFAULT 1;
    
    DECLARE v_done INT DEFAULT FALSE;
    DECLARE person_cur CURSOR FOR SELECT GJBObjId, personname, Birthday FROM property_tmp WHERE personname LIKE '%、%';
    DECLARE CONTINUE HANDLER FOR NOT FOUND SET v_done = TRUE;
    
    OPEN person_cur;
    
    loop_property: LOOP
        FETCH person_cur INTO v_objid, v_person, v_birth;
        
        IF v_done THEN
            LEAVE loop_property;
        END IF;
        
        SET v_count = get_split_string_num_func(v_person, '、');
        
        WHILE i <= v_count DO
            INSERT INTO property_tmp(GJBObjId, PersonName, Birthday) VALUES(v_objid, get_split_string_func(v_person, '、', i), v_birth);
            COMMIT;
            SET i = i + 1;
        END WHILE;
        
        DELETE FROM property_tmp WHERE GJBObjId = v_objid AND PersonName = v_person;
        COMMIT;
        
        SET v_count = 0;
        SET i = 1;
        
    END LOOP;
    CLOSE person_cur;
    
END$$
DELIMITER ;

-- 调用以上存储过程
CALL split_propertyowner_prc;

-- 去除重复的委估对象、产权人数据
ALTER TABLE property_tmp RENAME property_tmp1;
CREATE TABLE property_tmp(GJBObjId BIGINT(20) COMMENT '估价宝委估对象ID'
    ,`PersonName` VARCHAR(20) NULL COMMENT '姓名'
    ,`Birthday` DATETIME NULL DEFAULT NULL COMMENT '生日'
    ,PRIMARY KEY(GJBObjId, PersonName)
    );
    
INSERT INTO property_tmp(GJBObjId, PersonName, Birthday)
    SELECT GJBObjId, PersonName, Birthday
    FROM property_tmp1
    GROUP BY PersonName, GJBObjId;
COMMIT;

DROP TABLE property_tmp1;

-- 插入Person数据
DELETE FROM person WHERE personid NOT IN (1, 2);
INSERT INTO person(PersonGUID, PersonName, Birthday)
    SELECT UUID(), a.PersonName, a.Birthday
    FROM (SELECT PersonName, Birthday FROM property_tmp GROUP BY PersonName) a;
COMMIT;

-- 插入propertyowner数据
INSERT INTO propertyowner(GJBObjId, PersonId, CreateDate)
    SELECT pt.GJBObjId, p.PersonId, DATE_ADD('2015-08-12 14:00:00',  INTERVAL  FLOOR(1 + (RAND() * 10800))   SECOND )
    FROM property_tmp pt
        ,person p
    WHERE p.personname = pt.PersonName;

-- 随机生成身份证号
DELIMITER $$

DROP FUNCTION IF EXISTS rand_idnum_func$$
CREATE FUNCTION rand_idnum_func(v_num INT)
    RETURNS CHAR(18) CHARSET utf8
    LANGUAGE SQL
    NO SQL
    SQL SECURITY INVOKER
    COMMENT '随机生成身份证号'
BEGIN
    DECLARE i INT DEFAULT 0;
    DECLARE v_pre CHAR(6) DEFAULT '';   -- 前缀
    DECLARE v_suf CHAR(4) DEFAULT '';   -- 后缀
    DECLARE v_birth CHAR(8) DEFAULT ''; -- 生日
    
    WHILE i < 5 DO
        SET i = i +1;
        SET v_pre = CONCAT(v_pre, TRUNCATE(RAND(v_num + i)* 10, 0));
    END WHILE;
    
    SET v_birth = DATE_FORMAT(DATE_ADD('1990-06-25',  INTERVAL  FLOOR(1 - (RAND(v_num) * 10000)) DAY), '%Y%m%d');
    
    SET i = 0;
    WHILE i < 4 DO
        SET i = i +1;
        SET v_suf = CONCAT(v_suf, TRUNCATE(RAND(v_num - i)* 10, 0));
    END WHILE;
    
    RETURN CONCAT(v_pre, v_birth, v_suf);
    
END$$
DELIMITER ;

-- 更新身份证数据
UPDATE  person
    SET idnum = rand_idnum_func(personid);
COMMIT;

-- 更新产权比例数据
DELIMITER $$

DROP PROCEDURE IF EXISTS update_property_data_prc$$
CREATE PROCEDURE update_property_data_prc()
    LANGUAGE SQL
    MODIFIES SQL DATA
    SQL SECURITY INVOKER
    COMMENT '更新rightpercent数据'

BEGIN
    DECLARE v_gjbobjid BIGINT DEFAULT 0;
    DECLARE v_count INT DEFAULT 0;
    DECLARE i INT DEFAULT 0;
    DECLARE v_done INT DEFAULT FALSE;
    DECLARE obj_count_cur CURSOR FOR SELECT GJBObjId, COUNT(GJBObjId) FROM propertyowner GROUP BY GJBObjId;
    DECLARE CONTINUE HANDLER FOR NOT FOUND SET v_done = TRUE;
    
    OPEN obj_count_cur;
    
    update_loop: LOOP
        FETCH obj_count_cur INTO v_gjbobjid, v_count;
        
        IF v_done THEN
            LEAVE update_loop;
        END IF;
        
        UPDATE propertyowner SET rightpercent = 1.00/v_count WHERE GJBObjId = v_gjbobjid;
        COMMIT;
    END LOOP;
        
END$$
DELIMITER ;

-- 调用update_property_data_prc
CALL update_property_data_prc;

-- 更新Person数据
UPDATE person
    SET sexual = 1
    WHERE personid%4 = 0;
UPDATE person
    SET sexual = 0
    WHERE personid%4 = 1;
UPDATE person
    SET sexual = 0
    WHERE personid%4 = 2;
UPDATE person
    SET sexual = 1
    WHERE personid%4 = 3;
COMMIT;
-- ---------------------------------------
UPDATE person
    SET MaritalStatus = '未婚'
        ,HasChildren = '无'
    WHERE personid%3 = 0;
UPDATE person
    SET MaritalStatus = '已婚'
        ,HasChildren = '无'
    WHERE personid%3 = 1;
UPDATE person
    SET MaritalStatus = '已婚'
        ,HasChildren = '有'
    WHERE personid%3 = 2;
COMMIT;
-- ---------------------------------
UPDATE person
    SET Relation = '中介'
    WHERE personid%4 = 0;
UPDATE person
    SET Relation = '父母'
    WHERE personid%4 = 1;
UPDATE person
    SET Relation = '配偶'
    WHERE personid%4 = 2;
UPDATE person
    SET Relation = '朋友'
    WHERE personid%4 = 3;
COMMIT;

-- -----------------------------------------------
DROP TEMPORARY TABLE IF EXISTS person_tmp;
CREATE TEMPORARY TABLE  person_tmp LIKE person;
INSERT INTO person_tmp SELECT * FROM person;

UPDATE person p1
    SET p1.contacts =
    (
    SELECT p2.personname
        FROM person_tmp p2
        WHERE p2.PersonId = p1.PersonId - 1
    );
COMMIT;

-- ---------------------------------------------
DELIMITER $$

DROP FUNCTION IF EXISTS rand_phone_func$$
CREATE FUNCTION rand_phone_func(v_num INT)
    RETURNS CHAR(11) CHARSET utf8
    LANGUAGE SQL
    NO SQL
    SQL SECURITY INVOKER
    COMMENT '随机生成手机号'
BEGIN
    DECLARE i INT DEFAULT 0;
    DECLARE v_pre CHAR(11) DEFAULT '185';   -- 前缀
    
    
    WHILE i < 8 DO
        SET i = i +1;
        SET v_pre = CONCAT(v_pre, TRUNCATE(RAND(v_num + i)* 10, 0));
    END WHILE;
    
    RETURN v_pre;
    
END$$
DELIMITER ;

-- -----------------------------------------------
UPDATE person
    SET phone1 = rand_phone_func(personid)
        ,ContractPhone = rand_phone_func(personid + 10000);
COMMIT;