/*
Date: 20151225
Description: 新建表auth_direct_company
*/


SET @@foreign_key_checks = 0;
USE openplatform;

DROP TABLE IF EXISTS auth_direct_company;

CREATE TABLE `auth_direct_company` (
  `DirectCompanyId` INT(11) NOT NULL COMMENT '直接公司id',
  `ClientId` CHAR(150) NOT NULL COMMENT '直接公司标识',
  `ClientSecret` VARCHAR(240) NOT NULL COMMENT '直接公司密码',
  `Scope` VARCHAR(300) DEFAULT NULL COMMENT '直接公司所属权限',
  `ExpireTime` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '过期时间',
  `UpdateTime` TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE CURRENT_TIMESTAMP COMMENT '更新token时间',
  `Token` VARCHAR(150) DEFAULT NULL COMMENT 'token密钥',
  PRIMARY KEY (`DirectCompanyId`),
  CONSTRAINT `Fk_auth_direct_company_DirectCompanyId` FOREIGN KEY (`DirectCompanyId`) REFERENCES `admin_direct_company` (`CompanyId`)
)
COMMENT '开放平台接口认证表';


INSERT INTO openplatform.auth_direct_company (
    DirectCompanyId
    , ClientId
    , ClientSecret
    , Scope
    , ExpireTime
    , UpdateTime
    , Token
) 
VALUES
    (
        25
        , 123
        , 'CF7D47EF9508AECA'
        , NULL
        , '2021-06-14 19:41:08'
        , '0000-00-00 00:00:00'
        , 'ke/Kb+NgyUihBepGdtaugQ=='
    ) ;

COMMIT;

SET @@foreign_key_checks = 1;
