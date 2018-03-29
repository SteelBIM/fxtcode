/*
Date:20150803
Description:  添加博纳所需字段
*/

-- --------------------------------------------------------
-- --------------------------------------------------------
-- 1. 新增博纳字段
-- --------------------------------------------------------
-- --------------------------------------------------------
USE openplatform;

-- PropertyTransactionRecode
ALTER TABLE PropertyTransactionRecode ADD FinancingPurpose INT COMMENT '融资目的(Src:SysCode)';

-- EntrustObject
ALTER TABLE EntrustObject ADD `Usage` TINYINT COMMENT '使用情况(1: 自住, 2: 出租或空置)';
ALTER TABLE EntrustObject ADD DecorationValue DECIMAL(18, 4) COMMENT '装修价值(金额: 万元)';
ALTER TABLE EntrustObject ADD BusLineNum INT COMMENT '公交线路数量';
ALTER TABLE EntrustObject ADD HousingLocation FLOAT COMMENT '房产位置(单位: KM)';
ALTER TABLE EntrustObject ADD PublicFacilitiesNum INT COMMENT '公共配套设施数量';

-- Person
ALTER TABLE Person ADD Contacts VARCHAR(100) COMMENT '产权人联系人';
ALTER TABLE Person ADD Relation VARCHAR(20) COMMENT '与产权人关系';
ALTER TABLE Person ADD ContractPhone CHAR(20) COMMENT '产权人联系人电话';
ALTER TABLE Person ADD MaritalStatus TINYINT COMMENT '婚姻状况(0: 未婚, 1: 已婚, 2: 离异/丧偶)';
ALTER TABLE Person ADD HasChildren BIT(1) COMMENT '有无子女(0: 无, 1: 有)';
    
    
-- 定义“融资目的”取值CODE
INSERT INTO SysCode VALUES (NULL, 20003001, '按揭购房', 20003, '融资目的')
    ,(NULL, 20003002, '经营周转', 20003, '融资目的')
    ,(NULL, 20003003, '个人消费', 20003, '融资目的')
    ,(NULL, 20003004, '股票融资', 20003, '融资目的')
    ,(NULL, 20003005, '其他', 20003, '融资目的');

-- 新增字段写入数据
UPDATE PropertyTransactionRecode SET FinancingPurpose = 20003001 WHERE tranid = 1;

UPDATE EntrustObject SET `Usage` = 1
    ,DecorationValue = 6.52
    ,BusLineNum = 20
    ,HousingLocation = 4.5
    ,PublicFacilitiesNum = 10
    WHERE GJBObjid = 234344;

UPDATE Person SET Contacts = '王杰'
    ,Relation = '兄弟'
    ,ContractPhone = '13345268679'
    ,MaritalStatus = 0
    ,HasChildren = 0
    WHERE personid = 1;

UPDATE Person SET Contacts = '张梁'
    ,Relation = '夫妻'
    ,ContractPhone = '18756326895'
    ,MaritalStatus = 1
    ,HasChildren = 1
    WHERE personid = 2;

COMMIT;