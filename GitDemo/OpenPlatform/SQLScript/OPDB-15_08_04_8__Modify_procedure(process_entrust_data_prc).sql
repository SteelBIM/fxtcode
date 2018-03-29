/*
Date: 20150804
Description: 修改存储过程process_entrust_data_prc
*/

DELIMITER $$
DROP PROCEDURE IF EXISTS process_entrust_data_prc$$

CREATE PROCEDURE process_entrust_data_prc
/*
2. 将委托业务信息写入表EntrustAppraise
*/
    (IN v_fxtcompanyid INT
    ,IN v_gjbentrustid BIGINT   -- 估价宝委托Id
    ,IN v_clientcontact VARCHAR(20)    -- 委托方联系人
    ,IN v_entrustphone VARCHAR(50)   -- 委托方电话
    ,IN v_entrustidnum VARCHAR(50) -- 委托人身份证
    ,IN v_buyingtype INT    -- 贷款类型：按揭、抵押(src: SysCode)
    ,IN v_appraiseagency VARCHAR(100)   -- 评估机构
    ,IN v_appraiser VARCHAR(20) -- 估价师（报告撰写）
    ,IN v_assigner VARCHAR(20)  -- 业务分配人
    ,IN v_applicationstatus INT -- 银行申请状态(0:申请前，1:申请后, -1:抵押)
    ,IN v_appraisestatus INT    -- 房产评估状态(0:未完成, 1:已完成)
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
                    ,entrustphone = v_entrustphone
                    ,clientcontact = v_clientcontact
                    ,updatedate = NOW()
                    ,appraiseagency = v_appraiseagency
                    ,appraiser = v_appraiser
                    ,assigner = v_assigner
                    ,applicationstatus = v_applicationstatus
                    ,appraisestatus = v_appraisestatus
                WHERE gjbentrustid = v_gjbentrustid;
            COMMIT;
        ELSE
            INSERT INTO entrustappraise(eaid
                                        ,fxtcompanyid
                                        ,gjbentrustid
                                        ,buyingtype
                                        ,entrustidnum
                                        ,entrustphone
                                        ,clientcontact
                                        ,createdate
                                        ,appraiseagency
                                        ,appraiser
                                        ,assigner
                                        ,applicationstatus
                                        ,appraisestatus
                                        )
                VALUES(NULL
                    ,v_fxtcompanyid
                    ,v_gjbentrustid
                    ,v_buyingtype
                    ,v_entrustidnum
                    ,v_entrustphone
                    ,v_clientcontact
                    ,NOW()
                    ,v_appraiseagency
                    ,v_appraiser
                    ,v_assigner
                    ,v_applicationstatus
                    ,v_appraisestatus
                    );
            COMMIT;
    END IF;
END$$
DELIMITER ;