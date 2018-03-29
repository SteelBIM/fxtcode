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
	SurveyBeginDate DATETIME COMMENT '查勘开始时间',
    SurveyEndDate DATETIME COMMENT '查勘结束时间',
	PRIMARY KEY (`EAId`)
)
COMMENT='估价委托信息'
COLLATE='utf8_general_ci'
ENGINE=InnoDB
;


/*
-- 将数据导入临时表Entrust

LOAD DATA INFILE '/tmp/EntrustObject_150821.txt' REPLACE INTO TABLE
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
,SurveyBeginDate
,SurveyEndDate
);
*/

