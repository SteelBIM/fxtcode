/*
Date: 20150804
Description: 读取接口中的Person信息，并将信息写入OPDB，返回PersonId
*/

USE openplatform;
DROP PROCEDURE IF EXISTS process_person_data_prc;

DELIMITER $$

CREATE PROCEDURE process_person_data_prc
    (IN v_personname VARCHAR(20)    -- 姓名
    ,IN v_idnum VARCHAR(20) -- 身份证
    ,IN v_sexual INT    -- 性别
    ,IN v_birthday DATETIME -- 生日
    ,IN v_phone1 CHAR(20)
    ,IN v_contacts VARCHAR(100) -- 产权人联系人
    ,IN v_relation VARCHAR(20)  -- 与产权人关系
    ,IN v_contractphone CHAR(20)    -- 产权人联系人电话
    ,IN v_maritalstatus TINYINT -- 婚姻状况(0: 未婚, 1: 已婚, 2: 离异/丧偶)
    ,IN v_haschildren BIT(1)    -- 有无子女(0: 无, 1: 有)
    ,OUT v_personid BIGINT
    )
    MODIFIES SQL DATA
    SQL SECURITY INVOKER
    COMMENT '写入Person信息'

BEGIN
    /*
    若Person表已存在该IDNum的人信息，则进行数据更新；
    否则，则插入新数据
    */
    DECLARE v_count INT DEFAULT 0;
    SELECT COUNT(1) INTO v_count FROM person WHERE IDNum = v_idnum;
    
    IF v_count > 0
        THEN
            UPDATE person
                SET person_name = v_personname
                    ,sexual = v_sexual
                    ,birthday = v_birthday
                    ,phone1 = v_phone1
                    ,contacts = v_contacts
                    ,relation = v_relation
                    ,contractphone = v_contractphone
                    ,maritalstatus = v_maritalstatus
                    ,haschildren = v_haschildren
                WHERE IDNum = v_idnum;
            COMMIT;
        ELSE
            INSERT INTO person(PersonId
                                ,PersonGUID
                                ,PersonName
                                ,IDNum
                                ,Sexual
                                ,Birthday
                                ,Phone1
                                ,Source
                                ,Contacts
                                ,Relation
                                ,ContractPhone
                                ,MaritalStatus
                                ,HasChildren
                                )
                VALUES(NULL
                        ,UUID()
                        ,v_personname
                        ,v_idnum
                        ,v_sexual
                        ,v_birthday
                        ,v_phone1
                        ,v_contacts
                        ,v_relation
                        ,v_contractphone
                        ,v_maritalstatus
                        ,v_haschildren
                        );
            COMMIT;
    END IF;
     
    -- 获取该IDNum对应的PersonId
    SELECT PersonId INTO v_personid FROM Person WHERE IDNum = v_idnum;
    
END$$
DELIMITER ;