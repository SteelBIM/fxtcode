/*
Date: 20150813
Description: 修改存储过程
*/

-- process_entrust_data_prc
DELIMITER $$
DROP PROCEDURE IF EXISTS process_entrust_data_prc$$

CREATE PROCEDURE process_entrust_data_prc(
    IN v_fxtcompanyid INT(11)
    ,IN v_gjbentrustid BIGINT(20)    -- 估价宝委托Id
    ,IN v_entrustcensusregister TINYINT(4)    -- 委托方户籍(1:本市户籍, 2:非本市户籍)
    ,IN v_entrustphone VARCHAR(50)    -- 委托方电话
    ,IN v_entrustidnum VARCHAR(50)    -- 委托人身份证
    ,IN v_clientcontact VARCHAR(10)    -- 委托方联系人
    ,IN v_clientcontactphone CHAR(20)    -- 委托方联系人电话
    ,IN v_buyingtype INT(11)    -- 贷款类型(1:抵押, 2:按揭)
    ,IN v_lendingbank VARCHAR(200)    -- 贷款银行
    ,IN v_guarantor4mortgage TINYINT(4)    -- 按揭贷款是否有共同保证人(1:是, 0:否)
    ,IN v_appraiseagency VARCHAR(100)    -- 评估机构
    ,IN v_appraiser VARCHAR(20)    -- 估价师（报告撰写）
    ,IN v_assigner VARCHAR(20)    -- 业务分配人
    ,IN v_applicationstatus INT(1)    -- 银行申请状态(1:申请前, 2:申请后)
    ,IN v_appraisestatus INT(1)    -- 房产评估状态(0:未完成, 1:已完成)
    )
    
    LANGUAGE SQL
	NOT DETERMINISTIC
	MODIFIES SQL DATA
	SQL SECURITY INVOKER
	COMMENT '写入委托业务信息'
BEGIN
    /*
    若表EntrustAppraise已存在该委托业务信息，则进行数据更新；
    否则，则插入新数据
    */
    DECLARE v_count INT  DEFAULT 0;
    SELECT COUNT(1) INTO v_count
        FROM entrustappraise
        WHERE gjbentrustid = v_gjbentrustid;
    IF v_count > 0 
        THEN
            UPDATE entrustappraise
                SET fxtcompanyid = v_fxtcompanyid
                    ,gjbentrustid = v_gjbentrustid
                    ,buyingtype = v_buyingtype
                    ,entrustidnum = v_entrustidnum
                    ,entrustcensusregister = v_entrustcensusregister
                    ,entrustphone = v_entrustphone
                    ,clientcontact = v_clientcontact
                    ,clientcontactphone = v_clientcontactphone
                    ,updatedate = NOW()
                    ,appraiseagency = v_appraiseagency
                    ,appraiser = v_appraiser
                    ,assigner = v_assigner
                    ,applicationstatus = v_applicationstatus
                    ,appraisestatus = v_appraisestatus
                    ,lendingbank = v_lendingbank
                    ,guarantor4mortgage = v_guarantor4mortgage
                WHERE gjbentrustid = v_gjbentrustid;
            COMMIT;
        ELSE
            INSERT INTO entrustappraise(eaid
                                        ,fxtcompanyid
                                        ,gjbentrustid
                                        ,buyingtype
                                        ,entrustidnum
                                        ,entrustcensusregister
                                        ,entrustphone
                                        ,clientcontact
                                        ,clientcontactphone
                                        ,createdate
                                        ,appraiseagency
                                        ,appraiser
                                        ,assigner
                                        ,applicationstatus
                                        ,appraisestatus
                                        ,lendingbank
                                        ,guarantor4mortgage
                                        )
                VALUES(NULL
                    ,v_fxtcompanyid
                    ,v_gjbentrustid
                    ,v_buyingtype
                    ,v_entrustidnum
                    ,v_entrustcensusregister
                    ,v_entrustphone
                    ,v_clientcontact
                    ,v_clientcontactphone
                    ,NOW()
                    ,v_appraiseagency
                    ,v_appraiser
                    ,v_assigner
                    ,v_applicationstatus
                    ,v_appraisestatus
                    ,v_lendingbank
                    ,v_guarantor4mortgage
                    );
            COMMIT;
    END IF;
END$$
DELIMITER ;

-- --------------------------
DELIMITER $$

-- process_person_data_prc
DROP PROCEDURE IF EXISTS process_person_data_prc$$

CREATE PROCEDURE `process_person_data_prc`(IN `v_personname` VARCHAR(20)    -- 姓名
    , IN `v_idnum` VARCHAR(20) -- 身份证
    , IN `v_sexual` INT    -- 性别
    , IN `v_birthday` DATETIME -- 生日
    , IN `v_phone1` CHAR(20)
    , IN `v_contacts` VARCHAR(100) -- 产权人联系人
    , IN `v_relation` VARCHAR(20)  -- 与产权人关系
    , IN `v_contractphone` CHAR(20)    -- 产权人联系人电话
    , IN `v_maritalstatus` VARCHAR(20) -- 婚姻状况
    , IN `v_haschildren` VARCHAR(5)    -- 有无子女
    , OUT `v_personid` BIGINT
    )
	LANGUAGE SQL
	NOT DETERMINISTIC
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
                SET personname = v_personname
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

-- process_property_data_prc
DROP PROCEDURE IF EXISTS process_property_data_prc$$

CREATE PROCEDURE `process_property_data_prc`(IN `v_gjbobjid` BIGINT   -- 估价宝委估对象Id
    , IN `v_personname` VARCHAR(20)    -- 姓名
    , IN `v_idnum` VARCHAR(20) -- 身份证
    , IN `v_rightpercent` FLOAT    -- 所有权比例
    , IN `v_phone1` CHAR(20)   -- 电话
    , IN `v_contacts` VARCHAR(100) -- 产权人联系人
    , IN `v_relation` VARCHAR(20)  -- 与产权人关系
    , IN `v_contractphone` CHAR(20)    -- 产权人联系人电话
    , IN `v_maritalstatus` VARCHAR(20) -- 婚姻状况
    , IN `v_haschildren` VARCHAR(5)    -- 有无子女
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

-- ----------------------------
-- process_files_data_prc
DELIMITER $$

DROP PROCEDURE IF EXISTS process_files_data_prc$$

CREATE PROCEDURE process_files_data_prc(
    IN `v_gjbobjid` BIGINT    -- 估价宝委估对象Id
    , IN `v_name` VARCHAR(150)    -- 名称
    , IN `v_path` VARCHAR(200)    -- 文件路径全称（包含文件名称）
    , IN `v_uptime` DATETIME    -- 上传时间
    , IN `v_smallimgpath` VARCHAR(200)    -- 文件缩略图路径全称（包含文件名称）
    , IN `v_annextypecode` INT    -- 附件大类
    , IN `v_annextypesubcode` INT UNSIGNED    -- 附件小类
    , IN `v_imagetype` VARCHAR(50)    -- 照片类型（可以自定义，存文本）
    , IN `v_filesize` INT    -- 文件大小
    , IN `v_flietypecode` INT    -- 文件类型
    , IN `v_filesubtypecode` INT    -- 文件子类型
    , IN `v_createdate` DATETIME    -- 创建时间
    , IN `v_remark` VARCHAR(200)    -- 备注
    , IN `v_filecreatedate` DATETIME    -- 文件(照片)生成时间
    )
	LANGUAGE SQL
	NOT DETERMINISTIC
	MODIFIES SQL DATA
	SQL SECURITY INVOKER
	COMMENT '写入图片信息'
BEGIN
    INSERT INTO SurveyFiles(id
                            ,gjbobjid
                            ,name
                            ,path
                            ,uptime
                            ,smallimgpath
                            ,annextypecode
                            ,annextypesubcode
                            ,imagetype
                            ,filesize
                            ,flietypecode
                            ,filesubtypecode
                            ,createdate
                            ,remark
                            ,filecreatedate
                            )
        VALUES(NULL
            ,v_gjbobjid
            ,v_name
            ,v_path
            ,v_uptime
            ,v_smallimgpath
            ,v_annextypecode
            ,v_annextypesubcode
            ,v_imagetype
            ,v_filesize
            ,v_flietypecode
            ,v_filesubtypecode
            ,v_createdate
            ,v_remark
            ,v_filecreatedate
            );
    COMMIT;

END$$

DELIMITER ;