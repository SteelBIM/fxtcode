/*
Date: 20150817
Description: 将身份证号修改为18位
*/

-- 修改随机生成身份证号函数
DELIMITER $$

DROP FUNCTION IF EXISTS rand_idnum_func$$

CREATE FUNCTION rand_idnum_func(v_num INT)
	RETURNS char(18) CHARSET utf8
	LANGUAGE SQL
	NOT DETERMINISTIC
	NO SQL
	SQL SECURITY INVOKER
	COMMENT '随机生成身份证号'
BEGIN
    DECLARE i INT DEFAULT 0;
    DECLARE v_pre CHAR(6) DEFAULT '';   -- 前缀
    DECLARE v_suf CHAR(4) DEFAULT '';   -- 后缀
    DECLARE v_birth CHAR(8) DEFAULT ''; -- 生日
    
    WHILE i < 6 DO
        SET i = i +1;
        IF TRUNCATE(RAND(v_num + i)* 10, 0) <> 0
            THEN
                SET v_pre = CONCAT(v_pre, TRUNCATE(RAND(v_num + i)* 10, 0));
            ELSE
                SET v_pre = CONCAT(v_pre, TRUNCATE(RAND(v_num + i)* 10, 0) + i);
        END IF;
    END WHILE;
    
    SET v_birth = DATE_FORMAT(DATE_ADD('2005-06-25',  INTERVAL  FLOOR(1 - (RAND(v_num) * 10000)) DAY), '%Y%m%d');
    
    SET i = 0;
    WHILE i < 4 DO
        SET i = i +1;
        IF TRUNCATE(RAND(v_num - i)* 10, 0) <> 0
            THEN 
                SET v_suf = CONCAT(v_suf, TRUNCATE(RAND(v_num - i)* 10, 0));
            ELSE
                SET v_suf = CONCAT(v_suf, TRUNCATE(RAND(v_num - i)* 10, 0) + i);
        END IF;
    END WHILE;
    
    RETURN CONCAT(v_pre, v_birth, v_suf);
    
END$$

DELIMITER ;


-- 更新身份证数据
UPDATE  person
    SET idnum = rand_idnum_func(personid);
COMMIT;