/*
Date: 20151010
Description: 博纳接口bug修复，委估业务、委估对象及相关信息，关联逻辑修改
*/

-- ----------------------------------------------------------------------
-- 1. 修改表结构，添加EOId
-- 各委估对象相关信息通过此主键ID与委估对象关联
-- ----------------------------------------------------------------------

-- entrust_object
ALTER TABLE entrust_object ADD COLUMN FXTCompanyId INT NOT NULL COMMENT '评估公司ID' AFTER GJBEntrustId;
ALTER TABLE entrust_object ADD COLUMN EAId BIGINT NOT NULL COMMENT '委托业务主键ID' AFTER GJBEntrustId;

-- appraise_object_price
ALTER TABLE appraise_object_price ADD COLUMN EOId BIGINT NOT NULL COMMENT '委估对象主键ID' AFTER GJBObjId;

-- buyer
ALTER TABLE buyer ADD COLUMN EOId BIGINT NOT NULL COMMENT '委估对象主键ID' AFTER GJBObjId;

-- property_certificate
ALTER TABLE property_certificate ADD COLUMN EOId BIGINT NOT NULL COMMENT '委估对象主键ID' AFTER GJBObjId;

-- property_owner
ALTER TABLE property_owner ADD COLUMN EOId BIGINT NOT NULL COMMENT '委估对象主键ID' AFTER GJBObjId;

-- property_transaction_recode
ALTER TABLE property_transaction_recode ADD COLUMN EOId BIGINT NOT NULL COMMENT '委估对象主键ID' AFTER GJBObjId;

-- survey_files
ALTER TABLE survey_files ADD COLUMN EOId BIGINT NOT NULL COMMENT '委估对象主键ID' AFTER GJBObjId;


-- ----------------------------------------------------------------------
-- 2. 修改存储过程
-- 调整委托业务、委估对象、委估对象信息关联ID
-- ----------------------------------------------------------------------

-- process_entrustobject_data_prc
DELIMITER $$

DROP PROCEDURE IF EXISTS `process_entrustobject_data_prc`$$

CREATE PROCEDURE `process_entrustobject_data_prc`(
    IN v_gjbobjid BIGINT(20)    -- 估价宝委估对象ID
    ,IN v_gjbentrustid BIGINT(20)    -- 估价宝委托ID
    ,IN v_houseid BIGINT(20)    -- 房屋ID
    ,IN v_buildingid BIGINT(20)    -- 楼栋Id
    ,IN v_projectid BIGINT(20)    -- 楼盘Id
    ,IN v_cityid INT(11)    -- 城市
    ,IN v_areaid INT(10)    -- 行政区
    ,IN v_address VARCHAR(1000)    -- 地址
    ,IN v_landvalueintermsofperunitfloor DECIMAL(18,2)    -- 楼面地价
    ,IN v_projectname VARCHAR(50)    -- 楼盘名称
    ,IN v_buildingname VARCHAR(50)    -- 楼栋名称
    ,IN v_buildingstructure VARCHAR(100)    -- 建筑结构
    ,IN v_totalfloor VARCHAR(50)    -- 总楼层
    ,IN v_housename VARCHAR(50)    -- 房号
    ,IN v_floor VARCHAR(50)    -- 楼层
    ,IN v_roomnum VARCHAR(50)    -- 房(房间数)
    ,IN v_balconynum VARCHAR(50)    -- 阳台(阳台数)
    ,IN v_buildingarea DECIMAL(18,4)    -- 建筑面积
    ,IN v_landarea DECIMAL(18,4)    -- 土地面积
    ,IN v_practicalarea DECIMAL(18,4)    -- 实用面积
    ,IN v_fitment VARCHAR(100)    -- 装修
    ,IN v_objectfullname VARCHAR(200)    -- 估价宝委估对象全称
    ,IN v_trandate DATETIME    -- 交易日期
    ,IN v_propertycertificateregisteprice DECIMAL(18,4)    -- 房产证证载价格
    ,IN `v_propertycertificateregistedate` DATETIME    -- 房产证注册日期
    ,IN `v_prepareloanamount` DECIMAL(18,2)    -- 拟贷金额
    ,IN `v_tranprice` DECIMAL(18,4)    -- 交易价格
    ,IN `v_propertycertificatenum` VARCHAR(50)    -- 房产证号
    ,IN `v_landcertificatedate` DATETIME    -- 土地所有权证注册日期
    ,IN `v_landcertificatearea` DECIMAL(18,4)    -- 土地使用权面积
    ,IN `v_landcertificateaddress` VARCHAR(100)    -- 土地证载地址
    ,IN `v_isfirstbuy` BIT(1)    -- 是否首次购房
    ,IN v_surveyor VARCHAR(20)    -- 查勘员
    ,IN v_issurvey BIT(1)    -- 是否现场查勘(0:否, 1:是)
    ,IN v_surveybegintime DATETIME    -- 查勘开始时间
    ,IN v_surveyendtime DATETIME    -- 查勘结束时间
    ,IN `v_financingpurpose` VARCHAR(100)    -- 融资目的
    ,IN v_usage VARCHAR(50)    -- 使用情况(1: 自住, 2: 出租或空置)
    ,IN v_decorationvalue DECIMAL(18,4)    -- 装修价值
    ,IN v_buslinenum VARCHAR(100)    -- 公交线路数量
    ,IN v_housinglocation VARCHAR(100)    -- 房产位置
    ,IN v_publicfacilitiesnum VARCHAR(100)    -- 公共配套设施数量
    ,IN v_autoprice DECIMAL(18,4)  -- 自动估价价格
    ,IN `v_mainhouseunitprice` DECIMAL(18,2)    -- 主房单价
    ,IN `v_mainhousetotalprice` DECIMAL(18,2)    -- 主房总价
    ,IN `v_outbuildingtotalprice` DECIMAL(18,2)    -- 附属房屋总价
    ,IN `v_landunitprice` DECIMAL(18,2)    -- 土地单价
    ,IN `v_landtotalprice` DECIMAL(18,2)    -- 土地总价
    ,IN `v_appraisetotalprice` DECIMAL(18,2)    -- 评估总价
    ,IN `v_valuedate` DATETIME    -- 价值时点
    -- , IN `v_landcertificatenum` VARCHAR(50)    -- 土地所有权证号
    ,IN v_buildingdate VARCHAR(50)   -- 建筑年代
    ,IN v_valid TINYINT	-- 委估对象数据是否有效
    ,IN v_fxtcompanyid INT 	-- 评估公司ID
    )
    MODIFIES SQL DATA
    SQL SECURITY INVOKER
    COMMENT '写入委估对象、房产证、价格、交易信息'
BEGIN
    DECLARE v_count INT  DEFAULT 0;
    DECLARE v_eaid BIGINT DEFAULT NULL;
    DECLARE v_eoid BIGINT DEFAULT NULL;
       
    -- 获取EAId
    SELECT EAId  INTO v_eaid
	FROM entrust_appraise
	WHERE GJBEntrustId = v_gjbentrustid
	AND FXTCompanyId = v_fxtcompanyid;
    
    /*
    若表entrust_object已存在该委估对象信息，则进行数据更新；
    否则，则插入新数据
    */
    SELECT COUNT(1) INTO v_count
        FROM entrust_object
        WHERE gjbobjid = v_gjbobjid
        AND FXTCompanyId = v_fxtcompanyid;
        
    IF v_count > 0
        THEN
            UPDATE entrust_object
                SET objectfullname = v_objectfullname
                    ,gjbentrustid = v_gjbentrustid
                    ,houseid = v_houseid
                    ,buildingid = v_buildingid
                    ,projectid = v_projectid
                    ,projectname = v_projectname
                    ,cityid = v_cityid
                    ,landvalueintermsofperunitfloor = v_landvalueintermsofperunitfloor
                    ,address = v_address
                    ,areaid = v_areaid
                    ,buildingname = v_buildingname
                    ,totalfloor = v_totalfloor
                    ,buildingstructure = v_buildingstructure
                    ,housename = v_housename
                    ,buildingarea = v_buildingarea
                    ,`floor` = v_floor
                    ,roomnum = v_roomnum
                    ,balconynum = v_balconynum
                    ,landarea = v_landarea
                    ,practicalarea = v_practicalarea
                    ,fitment = v_fitment
                    ,`usage` = v_usage
                    ,decorationvalue = v_decorationvalue
                    ,buslinenum = v_buslinenum
                    ,housinglocation = v_housinglocation
                    ,publicfacilitiesnum = v_publicfacilitiesnum
                    ,surveyor = v_surveyor
                    ,issurvey = v_issurvey
                    ,surveybegintime = v_surveybegintime
                    ,surveyendtime = v_surveyendtime
                    ,buildingdate = v_buildingdate
                    ,valid = v_valid
                    ,FXTCompanyId = v_fxtcompanyid
                    ,EAId = v_eaid
            WHERE gjbobjid =  v_gjbobjid
            AND FXTCompanyId = v_fxtcompanyid;
            COMMIT;
        ELSE
            INSERT INTO entrust_object(eoid
                                    ,gjbobjid
                                    ,objectfullname
                                    ,gjbentrustid
                                    ,houseid
                                    ,buildingid
                                    ,projectid
                                    ,projectname
                                    ,cityid
                                    ,landvalueintermsofperunitfloor
                                    ,address
                                    ,areaid
                                    ,buildingname
                                    ,totalfloor
                                    ,buildingstructure
                                    ,housename
                                    ,buildingarea
                                    ,`floor`
                                    ,roomnum
                                    ,balconynum
                                    ,landarea
                                    ,practicalarea
                                    ,fitment
                                    ,createdate
                                    ,`usage`
                                    ,decorationvalue
                                    ,buslinenum
                                    ,housinglocation
                                    ,publicfacilitiesnum
                                    ,surveyor
                                    ,issurvey
                                    ,surveybegintime
                                    ,surveyendtime
                                    ,buildingdate
                                    ,valid
                                    ,FXTCompanyId
                                    ,EAId
                                    )
                VALUES (NULL
                        ,v_gjbobjid
                        ,v_objectfullname
                        ,v_gjbentrustid
                        ,v_houseid
                        ,v_buildingid
                        ,v_projectid
                        ,v_projectname
                        ,v_cityid
                        ,v_landvalueintermsofperunitfloor
                        ,v_address
                        ,v_areaid
                        ,v_buildingname
                        ,v_totalfloor
                        ,v_buildingstructure
                        ,v_housename
                        ,v_buildingarea
                        ,v_floor
                        ,v_roomnum
                        ,v_balconynum
                        ,v_landarea
                        ,v_practicalarea
                        ,v_fitment
                        ,NOW()
                        ,v_usage
                        ,v_decorationvalue
                        ,v_buslinenum
                        ,v_housinglocation
                        ,v_publicfacilitiesnum
                        ,v_surveyor
                        ,v_issurvey
                        ,v_surveybegintime
                        ,v_surveyendtime
                        ,v_buildingdate
                        ,v_valid
                        ,v_fxtcompanyid
                        ,v_eaid
                        );
        COMMIT;
    END IF;
    
    -- 获取EOId
    SELECT EOId INTO v_eoid
	FROM entrust_object
	WHERE gjbobjid = v_gjbobjid
        AND FXTCompanyId = v_fxtcompanyid;
    
    /*
    若表property_certificate已存在该委估对象的房产证信息，则进行数据更新；
    否则，则插入新数据
    */
    SET v_count = 0;
    SELECT COUNT(1) INTO v_count
        FROM property_certificate
        WHERE EOId = v_eoid;
    IF v_count > 0
        THEN
            UPDATE property_certificate
                SET propertycertificatenum =  v_propertycertificatenum
                    ,houseid =  v_houseid
                    -- ,landcertificatenum =  v_landcertificatenum
                    ,propertycertificateregistedate =  v_propertycertificateregistedate
                    ,propertycertificateregisteprice =  v_propertycertificateregisteprice
                    ,landcertificatedate =  v_landcertificatedate
                    ,landcertificatearea =  v_landcertificatearea
                    ,landcertificateaddress =  v_landcertificateaddress
            WHERE EOId = v_eoid;
            COMMIT;
        ELSE
            INSERT INTO property_certificate(pcid
                                        ,propertycertificatenum
                                        ,gjbobjid
                                        ,houseid
                                        -- ,landcertificatenum
                                        ,propertycertificateregistedate
                                        ,propertycertificateregisteprice
                                        ,landcertificatedate
                                        ,landcertificatearea
                                        ,landcertificateaddress
                                        ,createdate
                                        ,EOId
                                        )
                VALUES(NULL
                    ,v_propertycertificatenum
                    ,v_gjbobjid
                    ,v_houseid
                    -- ,v_landcertificatenum
                    ,v_propertycertificateregistedate
                    ,v_propertycertificateregisteprice
                    ,v_landcertificatedate
                    ,v_landcertificatearea
                    ,v_landcertificateaddress
                    ,NOW()
                    ,v_eoid
                    );
        COMMIT;
    END IF;
    /*
    若表appraise_object_price已存在该委估对象价格信息，则进行数据更新；
    否则，则插入新数据
    */
    SET v_count = 0;
    SELECT COUNT(1) INTO v_count
        FROM appraise_object_price
        WHERE EOId = v_eoid;
    IF v_count > 0
        THEN
            UPDATE appraise_object_price
                SET valuedate =  v_valuedate
                    ,mainhouseunitprice =  v_mainhouseunitprice
                    ,mainhousetotalprice =  v_mainhousetotalprice
                    ,outbuildingtotalprice =  v_outbuildingtotalprice
                    ,landunitprice =  v_landunitprice
                    ,landtotalprice =  v_landtotalprice
                    ,appraisetotalprice =  v_appraisetotalprice
                    ,autoprice = v_autoprice
                WHERE EOId = v_eoid;
            COMMIT;
        ELSE
            INSERT INTO appraise_object_price(aopid
                                            ,gjbobjid
                                            ,valuedate
                                            ,mainhouseunitprice
                                            ,mainhousetotalprice
                                            ,outbuildingtotalprice
                                            ,landunitprice
                                            ,landtotalprice
                                            ,appraisetotalprice
                                            ,autoprice
                                            ,EOId
                                            )
                VALUES(NULL
                    ,v_gjbobjid
                    ,v_valuedate
                    ,v_mainhouseunitprice
                    ,v_mainhousetotalprice
                    ,v_outbuildingtotalprice
                    ,v_landunitprice
                    ,v_landtotalprice
                    ,v_appraisetotalprice
                    ,v_autoprice
                    ,v_eoid
                    );
            COMMIT;
    END IF;
    
    /*
    若表property_transaction_recode已存在该委估对象交易信息，则进行数据更新；
    否则，则插入新数据
    */
    SET v_count = 0;
    SELECT COUNT(1) INTO v_count
        FROM property_transaction_recode
        WHERE EOId = v_eoid;
    
    IF v_count > 0
        THEN
            UPDATE property_transaction_recode
                SET trandate =  v_trandate
                    ,tranprice =  v_tranprice
                    ,isfirstbuy =  v_isfirstbuy
                    ,prepareloanamount =  v_prepareloanamount
                    ,financingpurpose =  v_financingpurpose
                WHERE EOId = v_eoid;
            COMMIT;
        ELSE
            INSERT INTO property_transaction_recode(tranid
                                                ,gjbobjid
                                                ,trandate
                                                ,tranprice
                                                ,isfirstbuy
                                                ,prepareloanamount
                                                ,createdate
                                                ,financingpurpose
                                                ,EOId
                                                )
                VALUES(NULL
                    ,v_gjbobjid
                    ,v_trandate
                    ,v_tranprice
                    ,v_isfirstbuy
                    ,v_prepareloanamount
                    ,NOW()
                    ,v_financingpurpose
                    ,v_eoid
                    );
            COMMIT;
    END IF;
   
END$$

DELIMITER ;


-- process_buyer_data_prc
DELIMITER $$

DROP PROCEDURE IF EXISTS `process_buyer_data_prc`$$

CREATE PROCEDURE `process_buyer_data_prc`(
	IN v_gjbobjid BIGINT	-- 估价宝委估对象Id
	,IN v_buyername VARCHAR(20)	-- 购房人姓名
	,IN v_idnum VARCHAR(20)	-- 身份证
	,IN v_phone CHAR(20)	-- 联系电话
	,IN v_isfirstcall BOOLEAN	-- 是否本委估对象在本轮第一次插入数据
	,IN v_fxtcompanyid INT 	-- 评估公司ID
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
					)
			VALUES(NULL
				,UUID()
				,v_gjbobjid
				,v_buyername
				,v_idnum
				,v_phone
				,NOW()
				,v_eoid
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
					)
			VALUES(NULL
				,UUID()
				,v_gjbobjid
				,v_buyername
				,v_idnum
				,v_phone
				,NOW()
				,v_eoid
				);
		    COMMIT;
	    END IF;
	END IF;
		
END$$

DELIMITER ;


-- process_files_data_prc
DELIMITER $$

DROP PROCEDURE IF EXISTS `process_files_data_prc`$$

CREATE PROCEDURE `process_files_data_prc`(
    IN `v_gjbobjid` BIGINT    -- 估价宝委估对象Id
    ,IN `v_name` VARCHAR(150)    -- 名称
    ,IN `v_path` VARCHAR(200)    -- 文件路径全称（包含文件名称）
    ,IN `v_uptime` DATETIME    -- 上传时间
    ,IN `v_smallimgpath` VARCHAR(200)    -- 文件缩略图路径全称（包含文件名称）
    ,IN `v_annextypecode` INT    -- 附件大类
    ,IN `v_annextypesubcode` INT UNSIGNED    -- 附件小类
    ,IN `v_imagetype` VARCHAR(50)    -- 照片类型（可以自定义，存文本）
    ,IN `v_filesize` INT    -- 文件大小
    ,IN `v_flietypecode` INT    -- 文件类型
    ,IN `v_filesubtypecode` INT    -- 文件子类型
    ,IN `v_createdate` DATETIME    -- 创建时间
    ,IN `v_remark` VARCHAR(200)    -- 备注
    ,IN `v_filecreatedate` DATETIME    -- 文件(照片)生成时间
    ,IN v_fxtcompanyid INT 	-- 评估公司ID
    )
    MODIFIES SQL DATA
    SQL SECURITY INVOKER
    COMMENT '写入图片信息'
BEGIN

	DECLARE v_eoid BIGINT DEFAULT NULL;
	
	-- 获取EOId
	SELECT EOId INTO v_eoid
		FROM entrust_object
		WHERE gjbobjid = v_gjbobjid
		AND FXTCompanyId = v_fxtcompanyid;
	
	-- 删除历史文件
	DELETE FROM survey_files WHERE EOId = v_eoid;
	COMMIT;
	
	-- 插入新数据
    INSERT INTO survey_files(id
                            ,gjbobjid
                            ,NAME
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
                            ,EOId
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
            ,v_eoid
            );
    COMMIT;

END$$

DELIMITER ;


-- process_property_data_prc
DELIMITER $$

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