/*
Date: 20151116
Description: 博纳数据和基础库分离，包括接口访问帐号分离
	1. 新建博纳专用库：op_bona
		(1)表和相关存储过程同时分离，包含以下表：
		   appraise_object_price,
		   buyer,
		   customer,
		   entrust_appraise,
		   entrust_object,
		   person,
		   property_certificate,
		   property_owner,
		   property_transaction_recode,
		   survey_files
		(2)包含以下存储过程：
		   process_buyer_data_prc,
		   process_entrust_data_prc,
		   process_entrustobject_data_prc,
		   process_files_data_prc,
		   process_person_data_prc,
		   process_property_data_prc
		   
	2. 数据库访问账号分离
		(1)新建博纳接口访问帐号
		(2)新建其他接口帐号
*/

-- ------------------------------------------------------
-- 新建博纳数据库：op_bona ------------------------------
-- ------------------------------------------------------

CREATE DATABASE IF NOT EXISTS op_bona;


-- ------------------------------------------------------
-- 将博纳表结构、数据、相关存储过程导出 -----------------
-- 并迁移到op_bona数据库 --------------------------------
-- ------------------------------------------------------
/*

shell > cd /tmp
shell > mysqldump -uroot -pdbadmin123 --routines --events --triggers openplatform \
	appraise_object_price buyer customer entrust_appraise entrust_object \
	person property_certificate property_owner property_transaction_recode survey_files \
	| mysql -uroot -pdbadmin123 op_bona

*/

-- ------------------------------------------------------
-- 为博纳接口新建数据库帐号 -----------------------------
-- 为其他接口新建数据库帐号 -----------------------------
-- ------------------------------------------------------

-- 单独在另一脚本新建帐号

