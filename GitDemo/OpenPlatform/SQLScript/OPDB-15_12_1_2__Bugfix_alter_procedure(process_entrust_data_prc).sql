/*
Date: 20151201
Description: 修复存储过程bug：
	1. process_entrust_data_prc：UPDATE数据时，WHERE条件添加 fxtcompanyid = v_fxtcompanyid
*/


-- process_entrust_data_prc
DELIMITER $$

USE `op_bona`$$

DROP PROCEDURE IF EXISTS `process_entrust_data_prc`$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `process_entrust_data_prc`(
    IN v_fxtcompanyid INT(11)
    ,IN v_gjbentrustid BIGINT(20)    -- 估价宝委托Id
    ,IN v_entrustcensusregister TINYINT(4)    -- 委托方户籍(1:本市户籍, 2:非本市户籍)
    ,IN v_entrustphone VARCHAR(50)    -- 委托方电话
    ,IN v_entrustidnum VARCHAR(50)    -- 委托人身份证
    ,IN v_clientcontact VARCHAR(50)    -- 委托方联系人
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
    ,IN v_businessstatecode INT 	-- 委托业务状态
    )
    MODIFIES SQL DATA
    COMMENT '写入委托业务信息'
BEGIN
    /*
    若表entrust_appraise已存在该委托业务信息，则进行数据更新；
    否则，则插入新数据
    */
    DECLARE v_count INT  DEFAULT 0;
    SELECT COUNT(1) INTO v_count
        FROM entrust_appraise
        WHERE gjbentrustid = v_gjbentrustid
        AND fxtcompanyid = v_fxtcompanyid;
    
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
                    ,businessstatecode = v_businessstatecode
                WHERE gjbentrustid = v_gjbentrustid
                AND fxtcompanyid = v_fxtcompanyid;
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
                                        ,businessstatecode
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
                    ,v_businessstatecode
                    );
            COMMIT;
    END IF;
END$$

DELIMITER ;