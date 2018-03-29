/*
Date: 20150827
Description: 修改存储过程处理购房人数据
*/
USE openplatform;

DELIMITER $$

DROP PROCEDURE IF EXISTS process_buyer_data_prc$$

CREATE PROCEDURE process_buyer_data_prc(
	IN v_gjbobjid BIGINT	-- 估价宝委估对象Id
	,IN v_buyername VARCHAR(20)	-- 购房人姓名
	,IN v_idnum VARCHAR(20)	-- 身份证
	,IN v_phone CHAR(20)	-- 联系电话
	)
	MODIFIES SQL DATA
	SQL SECURITY INVOKER
	COMMENT '写入与更新购房人信息'
BEGIN
/*
若buyer表已存在该IDNum的人信息，则进行数据更新；
否则，则插入新数据
*/
    DECLARE v_count INT DEFAULT 0;
    IF v_idnum IS NOT NULL
	THEN
		SELECT COUNT(1)
			INTO v_count
			FROM buyer
			WHERE GJBObjId = v_gjbobjid
			AND IDNum = v_idnum;
	ELSE
		SELECT COUNT(1)
			INTO v_count
			FROM buyer
			WHERE GJBObjId = v_gjbobjid
			AND BuyerName = v_buyername;
	END IF;
		
    IF v_count > 0
        THEN
            UPDATE buyer
                SET buyername = v_buyername
                    ,idnum = v_idnum
                    ,phone = v_phone
                WHERE GJBObjId = v_gjbobjid
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
                                )
                VALUES(NULL
                        ,UUID()
                        ,v_gjbobjid
                        ,v_buyername
                        ,v_idnum
                        ,v_phone
                        ,NOW()
                        );
            COMMIT;
    END IF;
END$$

DELIMITER ;