/*
Date: 20151214
Description: 迁移表结构
*/

DROP TABLE IF EXISTS base_config_fitment_price;

/*==============================================================*/
/* Table: base_config_fitment_price                             */
/*==============================================================*/
CREATE TABLE base_config_fitment_price
(
   ID                   INT NOT NULL AUTO_INCREMENT,
   CityID               INT NOT NULL,
   FitmentCode          INT COMMENT 'FitmentCode(6026)',
   FitmentPrice         DECIMAL(18,2) NOT NULL COMMENT '装修单价',
   PurposeCode          INT COMMENT '用途Code（1002）',
   SubCode              INT COMMENT '当需要两个code值确定系数时使用',
   TypeCode             INT COMMENT '修正系数类型（1033）',
   FxtCompanyId         INT,
   PRIMARY KEY (ID)
);

ALTER TABLE base_config_fitment_price COMMENT '(src: FXTProject.dbo.sys_FitmentPrice)';
INSERT INTO openplatform.migration_tables (
    OriginalDatabase
    , OriginalTable
    , NewDatabase
    , NewTable

) 
VALUES
    (
        'FXTProject'
        , 'sys_FitmentPrice'
        , 'openplatform'
        , 'base_config_fitment_price'
    ) ;

