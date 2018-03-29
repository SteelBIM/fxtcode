/*
Date: 20151118
Description: 1. 删除op_bona库多余的Procedure和Function
	     2. 删除openplatform库的博纳表
*/

-- --------------------------------------------------------
-- 1. 删除op_bona库多余的Procedure和Function
-- --------------------------------------------------------

USE op_bona;

DROP PROCEDURE IF EXISTS add_data_for_flow_control_config_prc;
DROP PROCEDURE IF EXISTS process_flow_control_data;
DROP PROCEDURE IF EXISTS split_propertyowner_prc;
DROP PROCEDURE IF EXISTS update_property_data_prc;

DROP FUNCTION IF EXISTS get_split_string_func;
DROP FUNCTION IF EXISTS get_split_string_num_func;
DROP FUNCTION IF EXISTS rand_idnum_func;
DROP FUNCTION IF EXISTS rand_phone_func;


-- --------------------------------------------------------
-- 2. 删除openplatform库的博纳表
-- --------------------------------------------------------
USE openplatform;

DROP TABLE IF EXISTS appraise_object_price;
DROP TABLE IF EXISTS buyer;
DROP TABLE IF EXISTS customer;
DROP TABLE IF EXISTS entrust_appraise;
DROP TABLE IF EXISTS entrust_object;
DROP TABLE IF EXISTS person;
DROP TABLE IF EXISTS property_certificate;
DROP TABLE IF EXISTS property_owner;
DROP TABLE IF EXISTS property_transaction_recode;
DROP TABLE IF EXISTS survey_files;


