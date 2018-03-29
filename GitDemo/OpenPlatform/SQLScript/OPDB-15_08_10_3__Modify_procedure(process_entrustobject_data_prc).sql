/*
Date: 20150810
Description: 修改存储过程process_entrustobject_data_prc，
    将v_usage的数据类型有TINYINT更改为VARCHAR(50)
*/


-- process_entrustobject_data_prc
DELIMITER $$
DROP PROCEDURE IF EXISTS process_entrustobject_data_prc$$

CREATE PROCEDURE process_entrustobject_data_prc(
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
    ,IN v_totalfloor SMALLINT(6)    -- 总楼层
    ,IN v_housename VARCHAR(50)    -- 房号
    ,IN v_floor SMALLINT(6)    -- 楼层
    ,IN v_roomnum SMALLINT(6)    -- 房(房间数)
    ,IN v_balconynum SMALLINT(6)    -- 阳台(阳台数)
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
    , IN `v_valuedate` DATETIME    -- 价值时点
    -- , IN `v_landcertificatenum` VARCHAR(50)    -- 土地所有权证号
    )
    
	LANGUAGE SQL
	NOT DETERMINISTIC
	MODIFIES SQL DATA
	SQL SECURITY INVOKER
	COMMENT '写入委估对象、房产证、价格、交易信息'
BEGIN
    DECLARE v_count INT  DEFAULT 0;
       
    /*
    若表EntrustObject已存在该委估对象信息，则进行数据更新；
    否则，则插入新数据
    */
    SELECT COUNT(1) INTO v_count
        FROM entrustobject
        WHERE gjbobjid = v_gjbobjid;
        
    IF v_count > 0
        THEN
            UPDATE entrustobject
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
            WHERE gjbobjid =  v_gjbobjid;
            COMMIT;
        ELSE
            INSERT INTO entrustobject(eoid
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
                        );
        COMMIT;
    END IF;
    
    /*
    若表PropertyCertificate已存在该委估对象的房产证信息，则进行数据更新；
    否则，则插入新数据
    */
    SET v_count = 0;
    SELECT COUNT(1) INTO v_count
        FROM propertycertificate
        WHERE gjbobjid =  v_gjbobjid;
    IF v_count > 0
        THEN
            UPDATE propertycertificate
                SET propertycertificatenum =  v_propertycertificatenum
                    ,houseid =  v_houseid
                    -- ,landcertificatenum =  v_landcertificatenum
                    ,propertycertificateregistedate =  v_propertycertificateregistedate
                    ,propertycertificateregisteprice =  v_propertycertificateregisteprice
                    ,landcertificatedate =  v_landcertificatedate
                    ,landcertificatearea =  v_landcertificatearea
                    ,landcertificateaddress =  v_landcertificateaddress
            WHERE gjbobjid =  v_gjbobjid;
            COMMIT;
        ELSE
            INSERT INTO propertycertificate(propertycertificatenum
                                        ,gjbobjid
                                        ,houseid
                                        -- ,landcertificatenum
                                        ,propertycertificateregistedate
                                        ,propertycertificateregisteprice
                                        ,landcertificatedate
                                        ,landcertificatearea
                                        ,landcertificateaddress
                                        ,createdate
                                        )
                VALUES(v_propertycertificatenum
                    ,v_gjbobjid
                    ,v_houseid
                    -- ,v_landcertificatenum
                    ,v_propertycertificateregistedate
                    ,v_propertycertificateregisteprice
                    ,v_landcertificatedate
                    ,v_landcertificatearea
                    ,v_landcertificateaddress
                    ,NOW()
                    );
        COMMIT;
    END IF;

    /*
    若表AppraiseObjectPrice已存在该委估对象价格信息，则进行数据更新；
    否则，则插入新数据
    */
    SET v_count = 0;
    SELECT COUNT(1) INTO v_count
        FROM AppraiseObjectPrice
        WHERE gjbobjid =  v_gjbobjid;
    IF v_count > 0
        THEN
            UPDATE AppraiseObjectPrice
                SET valuedate =  v_valuedate
                    ,mainhouseunitprice =  v_mainhouseunitprice
                    ,mainhousetotalprice =  v_mainhousetotalprice
                    ,outbuildingtotalprice =  v_outbuildingtotalprice
                    ,landunitprice =  v_landunitprice
                    ,landtotalprice =  v_landtotalprice
                    ,appraisetotalprice =  v_appraisetotalprice
                    ,autoprice = v_autoprice
                WHERE gjbobjid =  v_gjbobjid;
            COMMIT;
        ELSE
            INSERT INTO AppraiseObjectPrice(aopid
                                            ,gjbobjid
                                            ,valuedate
                                            ,mainhouseunitprice
                                            ,mainhousetotalprice
                                            ,outbuildingtotalprice
                                            ,landunitprice
                                            ,landtotalprice
                                            ,appraisetotalprice
                                            ,autoprice
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
                    );
            COMMIT;
    END IF;
    
    /*
    若表PropertyTransactionRecode已存在该委估对象交易信息，则进行数据更新；
    否则，则插入新数据
    */
    SET v_count = 0;
    SELECT COUNT(1) INTO v_count
        FROM PropertyTransactionRecode
        WHERE gjbobjid =  v_gjbobjid;
    
    IF v_count > 0
        THEN
            UPDATE PropertyTransactionRecode
                SET trandate =  v_trandate
                    ,tranprice =  v_tranprice
                    ,isfirstbuy =  v_isfirstbuy
                    ,prepareloanamount =  v_prepareloanamount
                    ,financingpurpose =  v_financingpurpose
                WHERE gjbobjid =  v_gjbobjid;
            COMMIT;
        ELSE
            INSERT INTO PropertyTransactionRecode(tranid
                                                ,gjbobjid
                                                ,trandate
                                                ,tranprice
                                                ,isfirstbuy
                                                ,prepareloanamount
                                                ,createdate
                                                ,financingpurpose
                                                )
                VALUES(NULL
                    ,v_gjbobjid
                    ,v_trandate
                    ,v_tranprice
                    ,v_isfirstbuy
                    ,v_prepareloanamount
                    ,NOW()
                    ,v_financingpurpose
                    );
            COMMIT;
    END IF;
   
END$$
DELIMITER ;