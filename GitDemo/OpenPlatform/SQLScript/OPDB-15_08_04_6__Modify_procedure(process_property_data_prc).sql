/*
Date: 20150804
Description: 修改存储过程process_property_data_prc
*/

USE openplatform;

DELIMITER $$
DROP PROCEDURE IF EXISTS process_property_data_prc$$

CREATE PROCEDURE process_property_data_prc
/*
1. 将产权人信息写入表PropertyOwner
*/
    (IN v_gjbobjid BIGINT   -- 估价宝委估对象Id
    ,IN v_personname VARCHAR(20)    -- 姓名
    ,IN v_idnum VARCHAR(20) -- 身份证
    ,IN v_rightpercent FLOAT    -- 所有权比例
    ,IN v_phone1 CHAR(20)   -- 电话
    ,IN v_contacts VARCHAR(100) -- 产权人联系人
    ,IN v_relation VARCHAR(20)  -- 与产权人关系
    ,IN v_contractphone CHAR(20)    -- 产权人联系人电话
    ,IN v_maritalstatus TINYINT -- 婚姻状况(0: 未婚, 1: 已婚, 2: 离异/丧偶)
    ,IN v_haschildren BIT(1)    -- 有无子女(0: 无, 1: 有)
    )
    LANGUAGE SQL
    NOT DETERMINISTIC
    MODIFIES SQL DATA
    SQL SECURITY INVOKER
    COMMENT '写入产权信息'
BEGIN
    DECLARE v_count INT DEFAULT 0;
    DECLARE v_personid BIGINT;    -- 产权人Id(src:Person)
    
    -- 写入Person信息,并返回PersonId
    CALL process_person_data_prc(v_personname
                                ,v_idnum
                                ,NULL
                                ,NULL
                                ,v_phone1
                                ,v_contacts
                                ,v_relation
                                ,v_contractphone
                                ,v_maritalstatus
                                ,v_haschildren
                                ,v_personid
                                );
    
    -- 写入产权信息PropertyOwner
     /*
    若表中已存在该人的该委估对象产权信息，则进行数据更新；
    否则，插入新数据
    */
    SELECT COUNT(1) INTO v_count
        FROM PropertyOwner
        WHERE personid = v_personid
        AND gjbobjid = v_gjbobjid;
    
    IF v_count > 0
        THEN
            UPDATE PropertyOwner
                SET rightpercent = v_rightpercent
                WHERE personid = v_personid
                AND gjbobjid = v_gjbobjid;
        ELSE
            INSERT INTO PropertyOwner(poid
                                    ,personid
                                    ,gjbobjid
                                    ,rightpercent
                                    ,createdate
                                    )
                VALUES(NULL
                    ,v_personid
                    ,v_gjbobjid
                    ,v_rightpercent
                    ,NOW()
                    );
            COMMIT;
    END IF;
END$$  
DELIMITER ;  