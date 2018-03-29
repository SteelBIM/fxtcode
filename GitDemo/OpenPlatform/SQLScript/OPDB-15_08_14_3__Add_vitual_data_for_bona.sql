/*
Date: 20150814
Description: 去除重复数据
*/

-- propertytransactionrecode
DROP TABLE IF EXISTS propertytransactionrecode_tmp;
CREATE TABLE propertytransactionrecode_tmp LIKE propertytransactionrecode;

INSERT INTO propertytransactionrecode_tmp SELECT * FROM propertytransactionrecode GROUP BY gjbobjid;

DROP TABLE IF EXISTS propertytransactionrecode;
ALTER TABLE propertytransactionrecode_tmp RENAME propertytransactionrecode;


-- EntrustObject
DROP TABLE IF EXISTS EntrustObject_tmp;
CREATE TABLE EntrustObject_tmp LIKE EntrustObject;

INSERT INTO EntrustObject_tmp SELECT * FROM EntrustObject GROUP BY gjbobjid;

DROP TABLE IF EXISTS EntrustObject;
ALTER TABLE EntrustObject_tmp RENAME EntrustObject;


-- appraiseobjectprice
DROP TABLE IF EXISTS appraiseobjectprice_tmp;
CREATE TABLE appraiseobjectprice_tmp LIKE appraiseobjectprice;

INSERT INTO appraiseobjectprice_tmp SELECT * FROM appraiseobjectprice GROUP BY gjbobjid;

DROP TABLE IF EXISTS appraiseobjectprice;
ALTER TABLE appraiseobjectprice_tmp RENAME appraiseobjectprice;


-- propertycertificate
DROP TABLE IF EXISTS propertycertificate_tmp;
CREATE TABLE propertycertificate_tmp LIKE propertycertificate;

INSERT INTO propertycertificate_tmp SELECT * FROM propertycertificate GROUP BY gjbobjid;

DROP TABLE IF EXISTS propertycertificate;
ALTER TABLE propertycertificate_tmp RENAME propertycertificate;
