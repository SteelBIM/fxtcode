/*
Date: 20151014
Description: 博纳新增字段
*/
USE openplatform;

-- person表添加字段
ALTER TABLE person ADD OwnerCensusRegister TINYINT COMMENT '产权人户籍(1:本市户籍, 0:非本市户籍)';

-- 存储过程添加参数
-- process_buyer_data_prc
DELIMITER $$

USE `openplatform`$$

DROP PROCEDURE IF EXISTS `process_buyer_data_prc`$$

CREATE PROCEDURE `process_buyer_data_prc`(
	IN v_gjbobjid BIGINT	-- 估价宝委估对象Id
	,IN v_buyername VARCHAR(20)	-- 购房人姓名
	,IN v_idnum VARCHAR(20)	-- 身份证
	,IN v_phone CHAR(20)	-- 联系电话
	,IN v_isfirstcall BOOLEAN	-- 是否本委估对象在本轮第一次插入数据
	,IN v_fxtcompanyid INT 	-- 评估公司ID
	,IN v_buyercensusregister TINYINT    -- 买房人户籍
	)
    MODIFIES SQL DATA
    SQL SECURITY INVOKER
    COMMENT '写入与更新购房人信息'
BEGIN
	
	DECLARE v_count INT DEFAULT 0;
	DECLARE v_eoid BIGINT DEFAULT NULL;
	
	-- 获取EOId
	SELECT EOId INTO v_eoid
		FROM entrust_object
		WHERE gjbobjid = v_gjbobjid
		AND FXTCompanyId = v_fxtcompanyid;
	
	/*
	若是本委估对象在本轮第一次插入数据，则删除原表数据后插入；
	若不是则直接插入数据。
	*/
	IF v_isfirstcall
		THEN
			DELETE FROM buyer WHERE EOId = v_eoid;
			COMMIT;
	END IF;
	
	/*
	若buyer表已存在该IDNum的人信息，则进行数据更新；
	否则，则插入新数据
	*/
    IF v_idnum IS NOT NULL
	THEN
		SELECT COUNT(1)
			INTO v_count
			FROM buyer
			WHERE EOId = v_eoid
			AND IDNum = v_idnum;

	    IF v_count > 0
		THEN
		    UPDATE buyer
			SET buyername = v_buyername
			    ,idnum = v_idnum
			    ,phone = v_phone
			    ,buyercensusregister = v_buyercensusregister
			WHERE EOId = v_eoid
			AND IDNum = v_idnum;
		    COMMIT;
		ELSE
		    INSERT INTO buyer(Bid
					,BuyerGUID
					,GJBObjId
					,BuyerName
					,IDNum
					,Phone
					,CreateDate
					,EOId
					,buyercensusregister
					)
			VALUES(NULL
				,UUID()
				,v_gjbobjid
				,v_buyername
				,v_idnum
				,v_phone
				,NOW()
				,v_eoid
				,v_buyercensusregister
				);
		    COMMIT;
	    END IF;
	    
	ELSE
		SELECT COUNT(1)
			INTO v_count
			FROM buyer
			WHERE EOId = v_eoid
			AND BuyerName = v_buyername;
			
	    IF v_count > 0
		THEN
		    UPDATE buyer
			SET buyername = v_buyername
			    ,idnum = v_idnum
			    ,phone = v_phone
			    ,buyercensusregister = v_buyercensusregister
			WHERE EOId = v_eoid
			AND BuyerName = v_buyername;
		    COMMIT;
		ELSE
		    INSERT INTO buyer(Bid
					,BuyerGUID
					,GJBObjId
					,BuyerName
					,IDNum
					,Phone
					,CreateDate
					,EOId
					,buyercensusregister
					)
			VALUES(NULL
				,UUID()
				,v_gjbobjid
				,v_buyername
				,v_idnum
				,v_phone
				,NOW()
				,v_eoid
				,v_buyercensusregister
				);
		    COMMIT;
	    END IF;
	END IF;
		
END$$

DELIMITER ;


-- process_entrust_data_prc
DELIMITER $$

USE `openplatform`$$

DROP PROCEDURE IF EXISTS `process_entrust_data_prc`$$

CREATE PROCEDURE `process_entrust_data_prc`(
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
    ,IN v_valid TINYINT	-- 委估业务数据是否有效
    ,IN v_entrustandpropertyownerrelation VARCHAR(20)	-- 委托人和产权人关系
    ,IN v_borrowerispropertyowner TINYINT	-- 借款人是否为产权人
    ,IN v_appraisepurpose VARCHAR(150)	-- 评估目的
    ,IN v_realestateagency VARCHAR(150)    -- 中介公司
    ,IN v_realestatebroker VARCHAR(50)      -- 中介经纪人
    )
    MODIFIES SQL DATA
    SQL SECURITY INVOKER
    COMMENT '写入委托业务信息'
BEGIN
    /*
    若表entrust_appraise已存在该委托业务信息，则进行数据更新；
    否则，则插入新数据
    */
    DECLARE v_count INT  DEFAULT 0;
    SELECT COUNT(1) INTO v_count
        FROM entrust_appraise
        WHERE gjbentrustid = v_gjbentrustid;
    IF v_count > 0 
        THEN
            UPDATE entrust_appraise
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
                    ,valid = v_valid
                    ,entrustandpropertyownerrelation = v_entrustandpropertyownerrelation
                    ,borrowerispropertyowner = v_borrowerispropertyowner
                    ,appraisepurpose = v_appraisepurpose
                    ,realestateagency = v_realestateagency
                    ,realestatebroker = v_realestatebroker
                WHERE gjbentrustid = v_gjbentrustid;
            COMMIT;
        ELSE
            INSERT INTO entrust_appraise(eaid
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
                                        ,valid
                                        ,entrustandpropertyownerrelation
                                        ,borrowerispropertyowner
                                        ,appraisepurpose
                                        ,realestateagency
                                        ,realestatebroker
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
                    ,v_valid
                    ,v_entrustandpropertyownerrelation
                    ,v_borrowerispropertyowner
                    ,v_appraisepurpose
                    ,v_realestateagency
                    ,v_realestatebroker
                    );
            COMMIT;
    END IF;
END$$

DELIMITER ;


-- process_person_data_prc
DELIMITER $$

USE `openplatform`$$

DROP PROCEDURE IF EXISTS `process_person_data_prc`$$

CREATE PROCEDURE `process_person_data_prc`(
    IN `v_personname` VARCHAR(20)    -- 姓名
    , IN `v_idnum` VARCHAR(20) -- 身份证
    , IN `v_sexual` INT    -- 性别
    , IN `v_birthday` DATETIME -- 生日
    , IN `v_phone1` CHAR(20)
    , IN `v_contacts` VARCHAR(100) -- 产权人联系人
    , IN `v_relation` VARCHAR(20)  -- 与产权人关系
    , IN `v_contractphone` CHAR(20)    -- 产权人联系人电话
    , IN `v_maritalstatus` VARCHAR(20) -- 婚姻状况
    , IN `v_haschildren` VARCHAR(5)    -- 有无子女
    , IN v_ownercensusregister TINYINT    -- 产权人户籍
    , OUT `v_personid` BIGINT
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
                SET personname = v_personname
                    ,sexual = v_sexual
                    ,birthday = v_birthday
                    ,phone1 = v_phone1
                    ,contacts = v_contacts
                    ,relation = v_relation
                    ,contractphone = v_contractphone
                    ,maritalstatus = v_maritalstatus
                    ,haschildren = v_haschildren
                    ,ownercensusregister = v_ownercensusregister
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
                                ,ownercensusregister
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
                        ,v_ownercensusregister
                        );
            COMMIT;
    END IF;
     
    -- 获取该IDNum对应的PersonId
    SELECT PersonId INTO v_personid FROM Person WHERE IDNum = v_idnum;
END$$

DELIMITER ;


-- process_property_data_prc
DELIMITER $$

USE `openplatform`$$

DROP PROCEDURE IF EXISTS `process_property_data_prc`$$

CREATE PROCEDURE `process_property_data_prc`(
     IN `v_gjbobjid` BIGINT   -- 估价宝委估对象Id
    ,IN `v_personname` VARCHAR(20)    -- 姓名
    ,IN `v_idnum` VARCHAR(20) -- 身份证
    ,IN `v_rightpercent` FLOAT    -- 所有权比例
    ,IN `v_phone1` CHAR(20)   -- 电话
    ,IN `v_contacts` VARCHAR(100) -- 产权人联系人
    ,IN `v_relation` VARCHAR(20)  -- 与产权人关系
    ,IN `v_contractphone` CHAR(20)    -- 产权人联系人电话
    ,IN `v_maritalstatus` VARCHAR(20) -- 婚姻状况
    ,IN `v_haschildren` VARCHAR(5)    -- 有无子女 
    ,IN `v_isfirstcall` BOOLEAN	-- 是否本委估对象在本轮第一次插入数据
    ,IN v_fxtcompanyid INT 	-- 评估公司ID
    ,IN v_ownercensusregister TINYINT    -- 产权人户籍
    )
    MODIFIES SQL DATA
    SQL SECURITY INVOKER
    COMMENT '写入产权信息'
BEGIN

    DECLARE v_count INT DEFAULT 0;
    DECLARE v_personid BIGINT;    -- 产权人Id(src:Person)
    DECLARE v_eoid BIGINT DEFAULT NULL;
    
    -- 获取EOId
    SELECT EOId INTO v_eoid
	FROM entrust_object
	WHERE gjbobjid = v_gjbobjid
	AND FXTCompanyId = v_fxtcompanyid;
    
    /*
    若是本委估对象在本轮第一次插入数据，则删除原表数据后插入；
    若不是则直接插入数据。
    */
    IF v_isfirstcall
	THEN
		DELETE FROM property_owner WHERE EOId = v_eoid;
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
                                ,v_ownercensusregister
                                ,v_personid
                                );

    -- 写入产权信息property_owner
     /*
    若表中已存在该人的该委估对象产权信息，则进行数据更新；
    否则，插入新数据
    */

    SELECT COUNT(1) INTO v_count
        FROM property_owner
        WHERE personid = v_personid
        AND EOId = v_eoid;

    IF v_count > 0
        THEN
            UPDATE property_owner
                SET rightpercent = v_rightpercent
                WHERE personid = v_personid
                AND EOId = v_eoid;
        ELSE
            INSERT INTO property_owner(poid
                                    ,personid
                                    ,gjbobjid
                                    ,rightpercent
                                    ,createdate
                                    ,EOId
                                    )
                VALUES(NULL
                    ,v_personid
                    ,v_gjbobjid
                    ,v_rightpercent
                    ,NOW()
                    ,v_eoid
                    );
            COMMIT;
    END IF;
END$$

DELIMITER ;