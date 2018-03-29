/*
Date: 20150814
Description: 修改电话号码填充方式
*/

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
        SET v_pre = CONCAT(v_pre, TRUNCATE(RAND(v_num * 1000 * i) * 10, 0));
    END WHILE;
    
    RETURN v_pre;
    
END$$
DELIMITER ;

-- -----------------------------------------------
UPDATE person
    SET phone1 = rand_phone_func(personid)
        ,ContractPhone = rand_phone_func(personid + 1000);
COMMIT;