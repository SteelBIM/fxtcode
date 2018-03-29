/*
Date:20150730
Description: 修改表EntrustObject主键，及删除关联外键GJBObjId
*/

-- --------------------------------------------------------
-- --------------------------------------------------------
-- 1. 博纳接口新增DB用户，只有查询OPDB的权限
-- --------------------------------------------------------
-- --------------------------------------------------------

GRANT SELECT ON openplatform.* TO 'opbona'@'%' IDENTIFIED BY 'Op!2015.qry';


-- --------------------------------------------------------
-- --------------------------------------------------------
-- 2. 调整估价宝委估对象Id相关的一些外键
-- --------------------------------------------------------
-- --------------------------------------------------------

-- 2.1 删除外键GJBObjId
ALTER TABLE AppraiseObjectPrice DROP FOREIGN KEY FK_AO1;
ALTER TABLE PropertyOwner DROP FOREIGN KEY FK_PO1;
ALTER TABLE PropertyCertificate DROP FOREIGN KEY FK_PC1;
ALTER TABLE PropertyTransactionRecode DROP FOREIGN KEY FK_PTR1;

-- 2.2 EntrustObject表更换主键EOId
ALTER TABLE EntrustObject ADD EOId BIGINT FIRST;
UPDATE EntrustObject SET EOId = 1 WHERE GJBObjId = 234344;
COMMIT;

ALTER TABLE EntrustObject DROP PRIMARY KEY;
ALTER TABLE EntrustObject MODIFY EOId BIGINT AUTO_INCREMENT PRIMARY KEY;
