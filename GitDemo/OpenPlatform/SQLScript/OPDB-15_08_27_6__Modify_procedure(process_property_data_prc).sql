/*
Date: 20150827
Description: 添加v_isfirstcall参数，删除委估对象的产权人数据，并重新插入
*/

DELIMITER $$

DROP PROCEDURE IF EXISTS process_property_data_prc$$
CREATE PROCEDURE process_property_data_prc(IN `v_gjbobjid` BIGINT   -- 估价宝委估对象Id

    , IN `v_personname` VARCHAR(20)    -- 姓名

    , IN `v_idnum` VARCHAR(20) -- 身份证

    , IN `v_rightpercent` FLOAT    -- 所有权比例

    , IN `v_phone1` CHAR(20)   -- 电话

    , IN `v_contacts` VARCHAR(100) -- 产权人联系人

    , IN `v_relation` VARCHAR(20)  -- 与产权人关系

    , IN `v_contractphone` CHAR(20)    -- 产权人联系人电话

    , IN `v_maritalstatus` VARCHAR(20) -- 婚姻状况

    , IN `v_haschildren` VARCHAR(5)    -- 有无子女
    
    ,IN `v_isfirstcall` BOOLEAN	-- 是否本委估对象在本轮第一次插入数据

    )
    MODIFIES SQL DATA
    SQL SECURITY INVOKER
    COMMENT '写入产权信息'
BEGIN

    DECLARE v_count INT DEFAULT 0;

    DECLARE v_personid BIGINT;    -- 产权人Id(src:Person)
    
    /*
    若是本委估对象在本轮第一次插入数据，则删除原表数据后插入；
    若不是则直接插入数据。
    */
    IF v_isfirstcall
	THEN
		DELETE FROM PropertyOwner WHERE gjbobjid = v_gjbobjid;
		COMMIT;
    END IF;
    
    
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