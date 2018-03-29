/*
Date: 20150810
Description: 修改表propertycertificate主键
*/

ALTER TABLE propertycertificate ADD PCId BIGINT FIRST;

DROP TABLE IF EXISTS propertycertificate_tmp;
CREATE TABLE `propertycertificate_tmp` (
	`PCId` BIGINT(20) NULL DEFAULT NULL AUTO_INCREMENT,
	`PropertyCertificateNum` VARCHAR(50) NOT NULL COMMENT '房产证号',
	`GJBObjId` BIGINT(20) NULL DEFAULT NULL COMMENT '估价宝委估对象ID',
	`HouseId` BIGINT(20) NULL DEFAULT NULL COMMENT '房屋ID',
	`LandCertificateNum` VARCHAR(50) NULL DEFAULT NULL COMMENT '土地所有权证号',
	`PropertyCertificateRegisteDate` DATETIME NULL DEFAULT NULL COMMENT '房产证注册日期',
	`PropertyCertificateRegistePrice` DECIMAL(18,4) NULL DEFAULT NULL COMMENT '房产证证载价格',
	`LandCertificateDate` DATETIME NULL DEFAULT NULL COMMENT '土地所有权证注册日期',
	`LandCertificateArea` DECIMAL(18,4) NULL DEFAULT NULL COMMENT '土地所有权面积',
	`LandCertificateAddress` VARCHAR(100) NULL DEFAULT NULL COMMENT '土地证载地址',
	`CreateDate` DATETIME NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建日期',
	PRIMARY KEY (`PCId`),
	INDEX `FK_PC1` (`GJBObjId`)
)
COMMENT='房产证信息'
COLLATE='utf8_general_ci'
ENGINE=InnoDB
;


INSERT INTO propertycertificate_tmp SELECT * FROM propertycertificate;
COMMIT;

DROP TABLE propertycertificate;
ALTER TABLE propertycertificate_tmp RENAME propertycertificate;