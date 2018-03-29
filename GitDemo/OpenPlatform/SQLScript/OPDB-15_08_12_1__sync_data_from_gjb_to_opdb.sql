/*
Date: 20150812
Description: 博纳接口，将估价宝数据同步至OPDB
*/

-- 委托业务临时表 Entrust_temp
USE openplatform;

DROP TABLE IF EXISTS Entrust_temp;
CREATE TABLE Entrust_temp(
	`EAId` BIGINT(20) NOT NULL AUTO_INCREMENT COMMENT '估价委托信息',
	`FXTCompanyId` INT(11) NOT NULL DEFAULT '0',
	`GJBEntrustId` BIGINT(20) NULL DEFAULT NULL COMMENT '估价宝委托Id',
	`AppraiseAgency` VARCHAR(100) NULL DEFAULT NULL COMMENT '评估机构',
    `EntrustCensusRegister` TINYINT(4) NULL DEFAULT NULL COMMENT '委托方户籍(1:本市户籍, 2:非本市户籍)',
    `EntrustPhone` VARCHAR(50) NULL DEFAULT NULL COMMENT '委托方电话',
    `LendingBank` VARCHAR(200) NULL DEFAULT NULL COMMENT '贷款银行',
    `Appraiser` VARCHAR(20) NULL DEFAULT NULL COMMENT '估价师（报告撰写）',
    `Assigner` VARCHAR(20) NULL DEFAULT NULL COMMENT '业务分配人',
    `EntrustIDNum` VARCHAR(50) NULL DEFAULT NULL COMMENT '委托人身份证',
    `ClientContact` VARCHAR(10) NULL DEFAULT NULL COMMENT '委托方联系人',
    `ClientContactPhone` CHAR(20) NULL DEFAULT NULL COMMENT '委托方联系人电话',
	`BuyingType` INT(11) NULL DEFAULT NULL COMMENT '贷款类型(1:抵押, 2:按揭)',
	`ApplicationStatus` INT(1) NULL DEFAULT NULL COMMENT '银行申请状态(1:申请前, 2:申请后)',
	`Guarantor4Mortgage` TINYINT(4) NULL DEFAULT NULL COMMENT '按揭贷款是否有共同保证人(1:是, 0:否)',
	`AppraiseStatus` TINYINT(4) NULL DEFAULT NULL COMMENT '房产评估状态(1:已完成, 0:未完成)',

	PRIMARY KEY (`EAId`)
)
COMMENT='估价委托信息'
COLLATE='utf8_general_ci'
ENGINE=InnoDB
;

-- 委估对象临时表entrustobject_temp
USE openplatform;

DROP TABLE IF EXISTS entrustobject_temp;
CREATE TABLE  entrustobject_temp (
	`EOId` BIGINT(20) NOT NULL AUTO_INCREMENT,
    `GJBEntrustId` BIGINT(20) NULL DEFAULT NULL COMMENT '估价宝委托ID',
    `CityId` INT(11) NULL DEFAULT NULL COMMENT '城市',
    `AreaId` INT(10) NULL DEFAULT NULL COMMENT '行政区',
    `GJBObjId` BIGINT(20) NOT NULL DEFAULT '0' COMMENT '估价宝委估对象ID',
    `ProjectId` BIGINT(20) NULL DEFAULT NULL COMMENT '楼盘Id',
    `BuildingId` BIGINT(20) NULL DEFAULT NULL COMMENT '楼栋Id',
    `HouseId` BIGINT(20) NULL DEFAULT NULL COMMENT '房屋ID',
    `Address` VARCHAR(1000) NULL DEFAULT NULL COMMENT '地址',
    `LandValueInTermsOfPerUnitFloor` DECIMAL(18,2) NULL DEFAULT NULL COMMENT '楼面地价',
    `ProjectName` VARCHAR(50) NULL DEFAULT NULL COMMENT '楼盘名称',
    `BuildingName` VARCHAR(50) NULL DEFAULT NULL COMMENT '楼栋名称',
    `BuildingStructure` VARCHAR(100) NULL DEFAULT NULL COMMENT '建筑结构',
    `TotalFloor` SMALLINT(6) NULL DEFAULT NULL COMMENT '总楼层',
    `HouseName` VARCHAR(50) NULL DEFAULT NULL COMMENT '名称',
    `Floor` SMALLINT(6) NULL DEFAULT NULL COMMENT '楼层',
	`RoomNum` SMALLINT(6) UNSIGNED NULL DEFAULT NULL COMMENT '房(房间数)',
	`BalconyNum` SMALLINT(6) NULL DEFAULT NULL COMMENT '阳台(阳台数)',
    `BuildingArea` DECIMAL(18,4) NULL DEFAULT NULL COMMENT '建筑面积',
	`LandArea` DECIMAL(18,4) NULL DEFAULT NULL COMMENT '土地面积',
	`PracticalArea` DECIMAL(18,4) NULL DEFAULT NULL COMMENT '实用面积',
	`Fitment` VARCHAR(100) NULL DEFAULT NULL COMMENT '装修',
	`ObjectFullName` VARCHAR(200) NULL DEFAULT NULL COMMENT '估价宝委估对象全称',
    `TranDate` DATETIME NULL DEFAULT NULL COMMENT '交易日期',
    `PropertyCertificateRegisteDate` DATETIME NULL DEFAULT NULL COMMENT '房产证注册日期',
    `PrepareLoanAmount` DECIMAL(18,2) NULL DEFAULT NULL COMMENT '拟贷金额',
    `TranPrice` DECIMAL(18,4) NULL DEFAULT NULL COMMENT '交易价格',
    `PropertyCertificateNum` VARCHAR(50) NULL DEFAULT NULL COMMENT '房产证号',
    `LandCertificateDate` DATETIME NULL DEFAULT NULL COMMENT '土地所有权证注册日期',
	`LandCertificateArea` DECIMAL(18,4) NULL DEFAULT NULL COMMENT '土地所有权面积',
	`LandCertificateAddress` VARCHAR(100) NULL DEFAULT NULL COMMENT '土地证载地址',
    `IsFirstBuy` BIT(1) NULL DEFAULT NULL COMMENT '是否首次购房',
    `AutoPrice` DECIMAL(18,4) NULL DEFAULT NULL COMMENT '自动估价价格',
    `MainHouseUnitPrice` DECIMAL(18,2) NULL DEFAULT NULL COMMENT '主房单价',
    `MainHouseTotalPrice` DECIMAL(18,2) NULL DEFAULT NULL COMMENT '主房总价',
    `OutbuildingTotalPrice` DECIMAL(18,2) NULL DEFAULT NULL COMMENT '附属房屋总价',
	`LandUnitPrice` DECIMAL(18,2) NULL DEFAULT NULL COMMENT '土地单价',
	`LandTotalPrice` DECIMAL(18,2) NULL DEFAULT NULL COMMENT '土地总价',
	`AppraiseTotalPrice` DECIMAL(18,2) NULL DEFAULT NULL COMMENT '评估总价',
    `Birthday` DATETIME NULL DEFAULT NULL COMMENT '生日',
    `PersonName` VARCHAR(20) NULL DEFAULT NULL COMMENT '姓名',
    `IDNum` VARCHAR(20) NULL DEFAULT NULL COMMENT '身份证',
    `rightpercent` FLOAT NULL DEFAULT NULL,
    `Phone1` CHAR(20) NULL DEFAULT NULL,
	`Contacts` VARCHAR(100) NULL DEFAULT NULL COMMENT '产权人联系人',
	`Relation` VARCHAR(20) NULL DEFAULT NULL COMMENT '与产权人关系',
	`ContractPhone` CHAR(20) NULL DEFAULT NULL COMMENT '产权人联系人电话',
	PRIMARY KEY (`EOId`)
)
COMMENT='委估对象信息'
COLLATE='utf8_general_ci'
ENGINE=InnoDB
;

-- 将数据导入临时表EntrustObject_temp
LOAD DATA INFILE '/tmp/EntrustObject.txt' REPLACE INTO TABLE
EntrustObject_temp
FIELDS TERMINATED BY '\t' LINES TERMINATED BY '\n'
(
GJBEntrustId
,CityId
,AreaId
,GJBObjId
,ProjectId
,BuildingId
,HouseId
,Address
,LandValueInTermsOfPerUnitFloor
,ProjectName
,BuildingName
,BuildingStructure
,TotalFloor
,HouseName
,`Floor`
,RoomNum
,BalconyNum
,BuildingArea
,LandArea
,PracticalArea
,Fitment
,ObjectFullName
,TranDate
,PropertyCertificateRegisteDate
,PrepareLoanAmount
,TranPrice
,PropertyCertificateNum
,LandCertificateDate
,LandCertificateArea
,LandCertificateAddress
,IsFirstBuy
,AutoPrice
,MainHouseUnitPrice
,MainHouseTotalPrice
,OutbuildingTotalPrice
,LandUnitPrice
,LandTotalPrice
,AppraiseTotalPrice
,Birthday
,PersonName
,IDNum
,rightpercent
,Phone1
,Contacts
,Relation
,ContractPhone
);




-- 将数据导入临时表Entrust

LOAD DATA INFILE '/tmp/Entrust.txt' REPLACE INTO TABLE
Entrust_temp
FIELDS TERMINATED BY '\t' LINES TERMINATED BY '\n'
(
FXTCompanyId
,GJBEntrustId
,AppraiseAgency
,EntrustCensusRegister
,EntrustPhone
,LendingBank
,Appraiser
,Assigner
,EntrustIDNum
,ClientContact
,ClientContactPhone
,BuyingType
,ApplicationStatus
,Guarantor4Mortgage
,AppraiseStatus
);


-- 将临时表数据插入正式表Entrust
INSERT INTO EntrustAppraise
(FXTCompanyId
,GJBEntrustId
,AppraiseAgency
,EntrustCensusRegister
,EntrustPhone
,LendingBank
,Appraiser
,Assigner
,EntrustIDNum
,ClientContact
,ClientContactPhone
,BuyingType
,ApplicationStatus
,Guarantor4Mortgage
,AppraiseStatus
)
SELECT FXTCompanyId
	,GJBEntrustId
	,AppraiseAgency
	,EntrustCensusRegister
	,EntrustPhone
	,LendingBank
	,Appraiser
	,Assigner
	,EntrustIDNum
	,ClientContact
	,ClientContactPhone
	,BuyingType
	,ApplicationStatus
	,Guarantor4Mortgage
	,AppraiseStatus
FROM Entrust_temp;
COMMIT;

-- 将临时表数据插入正式表EntrustObject
INSERT INTO EntrustObject
	(GJBEntrustId
	,CityId
	,AreaId
	,GJBObjId
	,ProjectId
	,BuildingId
	,HouseId
	,Address
	,LandValueInTermsOfPerUnitFloor
	,ProjectName
	,BuildingName
	,BuildingStructure
	,TotalFloor
	,HouseName
	,`Floor`
	,RoomNum
	,BalconyNum
	,BuildingArea
	,LandArea
	,PracticalArea
	,Fitment
	,ObjectFullName
	)
SELECT GJBEntrustId
	,CityId
	,AreaId
	,GJBObjId
	,ProjectId
	,BuildingId
	,HouseId
	,Address
	,LandValueInTermsOfPerUnitFloor
	,ProjectName
	,BuildingName
	,BuildingStructure
	,TotalFloor
	,HouseName
	,`Floor`
	,RoomNum
	,BalconyNum
	,BuildingArea
	,LandArea
	,PracticalArea
	,Fitment
	,ObjectFullName
FROM entrustobject_temp
WHERE GJBEntrustId <> 0;
COMMIT;

-- 将临时表数据插入正式表propertytransactionrecode
INSERT INTO propertytransactionrecode
	(
	GJBObjId
	,TranDate
	,PrepareLoanAmount
	,TranPrice
    ,IsFirstBuy
	)
SELECT
	GJBObjId
	,TranDate
	,PrepareLoanAmount
	,TranPrice
    ,IsFirstBuy
FROM entrustobject_temp
WHERE GJBEntrustId <> 0;
COMMIT;


-- 将临时表数据插入正式表PropertyCertificate
INSERT INTO PropertyCertificate
	(
	GJBObjId
	,PropertyCertificateRegisteDate
	,PropertyCertificateNum
	,LandCertificateDate
	,LandCertificateArea
	,LandCertificateAddress
	)
SELECT 
	GJBObjId
	,PropertyCertificateRegisteDate
	,PropertyCertificateNum
	,LandCertificateDate
	,LandCertificateArea
	,LandCertificateAddress
FROM entrustobject_temp
WHERE GJBEntrustId <> 0;
COMMIT;


-- 将临时表数据插入正式表appraiseobjectprice
INSERT INTO appraiseobjectprice
	(
	GJBObjId
	,AutoPrice
	,MainHouseUnitPrice
	,MainHouseTotalPrice
	,OutbuildingTotalPrice
	,LandUnitPrice
	,LandTotalPrice
	,AppraiseTotalPrice
	)
SELECT 
	GJBObjId
	,AutoPrice
	,MainHouseUnitPrice
	,MainHouseTotalPrice
	,OutbuildingTotalPrice
	,LandUnitPrice
	,LandTotalPrice
	,AppraiseTotalPrice
FROM entrustobject_temp
WHERE GJBEntrustId <> 0;
COMMIT;


-- 将临时表数据插入正式表person
INSERT INTO person
	(
    PersonGUID
	,Birthday
	,PersonName
	,IDNum
	,Phone1
	,Contacts
	,Relation
	,ContractPhone
	)
SELECT
    UUID()
	,Birthday
	,PersonName
	,IDNum
	,Phone1
	,Contacts
	,Relation
	,ContractPhone
FROM entrustobject_temp
WHERE GJBEntrustId <> 0
GROUP BY personname;
COMMIT;