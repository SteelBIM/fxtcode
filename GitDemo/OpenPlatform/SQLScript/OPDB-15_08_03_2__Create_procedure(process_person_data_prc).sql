/*
Date: 20150803
Description: 读取接口中的Person信息，并将信息写入OPDB，返回PersonId
*/

USE openplatform;

DELIMITER @@@

CREATE PROCEDURE process_person_data_prc
    (IN v_personname VARCHAR(20)
    ,IN v_idnum VARCHAR(20)
    ,IN v_sexual INT
    ,IN v_birthday DATETIME
    ,IN v_phone1 CHAR(20)
    ,IN v_source INT
    ,IN v_contacts VARCHAR(100)
    ,IN v_relation VARCHAR(20)
    ,IN v_contractphone CHAR(20)
    ,IN v_maritalstatus TINYINT
    ,IN v_haschildren BIT(1)
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
    SELECT COUNT(1) INTO @v_count FROM person WHERE IDNum = v_idnum;
    IF @v_count > 0
        THEN
            UPDATE person
                SET person_name = v_personname
                    ,sexual = v_sexual
                    ,birthday = v_birthday
                    ,phone1 = v_phone1
                    ,source = v_source
                    ,contacts = v_contacts
                    ,relation = v_relation
                    ,contractphone = v_contractphone
                    ,maritalstatus = v_maritalstatus
                    ,haschildren = v_haschildren
                WHERE IDNum = v_idnum;
            COMMIT;
        ELSE
            INSERT INTO person(PersonId, PersonGUID, PersonName, IDNum, Sexual
                                ,Birthday, Phone1, Source, Contacts, Relation
                                ,ContractPhone, MaritalStatus, HasChildren)
                VALUES(NULL, UUID(), v_personname, v_idnum, v_sexual
                        ,v_birthday, v_phone1, v_source, v_contacts, v_relation
                        ,v_contractphone, v_maritalstatus, v_haschildren);
            COMMIT;
        
        -- 获取该IDNum对应的PersonId
        SELECT PersonId INTO v_personid FROM Person WHERE IDNum = v_idnum;
    END IF;
    
    SET @v_count = NULL;
    
END@@@
DELIMITER ;