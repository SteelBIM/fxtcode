/*
Date: 20151204
Description: 新建Table(migration_tables)，记录MSSQL已迁移至MySQL表信息
*/

USE openplatform;

-- 新建表
DROP TABLE IF EXISTS migration_tables;
CREATE TABLE `migration_tables` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT COMMENT '主键ID',
  `OriginalDatabase` VARCHAR(100) NOT NULL DEFAULT 'FXTProject' COMMENT '源数据库',
  `OriginalTable` VARCHAR(100) NOT NULL COMMENT '源表',
  `NewDatabase` VARCHAR(100) NOT NULL DEFAULT 'openplatform' COMMENT '目标数据库',
  `NewTable` VARCHAR(100) NOT NULL COMMENT '目标表',
  `WhereClause` VARCHAR(255) DEFAULT NULL COMMENT '源表迁移至新表的数据筛选条件',
  `Extra` VARCHAR(255) DEFAULT NULL COMMENT '其他额外信息',
  `CreateDate` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `UpdateDate` DATETIME DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
  PRIMARY KEY (`Id`)
)
ENGINE=INNODB
COMMENT='MSSQL已迁移至MySQL表信息';


-- 插入数据
START TRANSACTION;
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_AvgPrice_Day', 'openplatform', 'base_avg_price_day', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_AvgPrice_Month', 'openplatform', 'base_avg_price_month', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Building', 'openplatform', 'base_building', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Building_csj', 'openplatform', 'base_building', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Building_hbh', 'openplatform', 'base_building', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Building_xb', 'openplatform', 'base_building', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Building_zb', 'openplatform', 'base_building', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Building_zsj', 'openplatform', 'base_building', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Building_sub', 'openplatform', 'base_building_sub', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Building_csj_sub', 'openplatform', 'base_building_sub', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Building_hbh_sub', 'openplatform', 'base_building_sub', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Building_xb_sub', 'openplatform', 'base_building_sub', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Building_zb_sub', 'openplatform', 'base_building_sub', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Building_zsj_sub', 'openplatform', 'base_building_sub', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Case_hbh', 'openplatform', 'base_case_beijing', 'CityID IN (1)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Case_zsj', 'openplatform', 'base_case_fujian', 'CityID IN (109, 110, 111, 113, 278, 279, 280, 281, 282, 159)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Case_sz', 'openplatform', 'base_case_guangdong', 'CityID IN (6)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Case_zsj', 'openplatform', 'base_case_guangdong', 'CityID IN (7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 20, 21, 22, 214, 215, 216, 217, 218)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Case_zsj', 'openplatform', 'base_case_guangxi', 'CityID IN (159, 160, 161, 162, 163, 164, 197, 198, 199, 200, 201, 202, 203, 204)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Case_zsj', 'openplatform', 'base_case_hainan', 'CityID IN (165, 166, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 391, 392, 393, 394)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Case_hbh', 'openplatform', 'base_case_hebei', 'CityID IN (37, 38, 39, 40, 41, 42, 43, 44, 47, 244, 245)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Case_xb', 'openplatform', 'base_case_hebei', 'CityID IN (36)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Case_hbh', 'openplatform', 'base_case_heilongjiang', 'CityID IN (23, 24, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Case_csj', 'openplatform', 'base_case_hunan', 'CityID IN (67)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Case_zsj', 'openplatform', 'base_case_hunan', 'CityID IN (68, 69, 70, 71, 72, 73, 190, 261, 262, 263, 264, 265, 266)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Case_csj', 'openplatform', 'base_case_jiangsu', 'CityID IN (84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 97, 268, 269)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Case_zsj', 'openplatform', 'base_case_jiangxi', 'CityID IN (151, 152, 153, 154, 155, 374, 375, 376, 377, 378, 379)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Case_hbh', 'openplatform', 'base_case_jilin', 'CityID IN (25, 26, 206, 230, 231, 232, 233, 234, 235, 236)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Case_hbh', 'openplatform', 'base_case_liaoning', 'CityID IN (27, 28, 29, 31, 32, 33, 34, 35, 237, 238, 239, 240, 241, 242)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Case_hbh', 'openplatform', 'base_case_neimenggu', 'CityID IN (140, 141, 142, 143, 144, 329, 330, 331, 332, 333, 334, 335)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Case_hbh', 'openplatform', 'base_case_shandong', 'CityID IN (81, 120, 121, 122, 123, 124, 125, 127, 128, 131, 132, 133, 207, 300, 301, 302, 303)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Case_csj', 'openplatform', 'base_case_shanghai', 'CityID IN (2)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Case_hbh', 'openplatform', 'base_case_tianjin', 'CityID IN (3)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Case_xb', 'openplatform', 'base_case_xb', 'CityID <> 36');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Case_zsj', 'openplatform', 'base_case_yunnan', 'CityID IN (157, 158, 192, 193, 194, 195, 380, 381, 382, 383, 385, 386, 387, 388, 389, 390)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Case_zb', 'openplatform', 'base_case_zb', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Case_csj', 'openplatform', 'base_case_zb', 'CityID IN (101)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Case_csj', 'openplatform', 'base_case_zhejiang', 'CityID IN (74, 75, 76, 77, 80, 83, 187, 188, 189, 267)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Case_hbh', 'openplatform', 'base_case_zhejiang', 'CityID IN (130)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Company', 'openplatform', 'base_company', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_hbh', 'openplatform', 'base_house_beijing', 'CityID IN (1)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_zsj', 'openplatform', 'base_house_fujian', 'CityID IN (109, 110, 111, 113, 278, 279, 280, 281, 282, 159)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_sz', 'openplatform', 'base_house_guangdong', 'CityID IN (6)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_zsj', 'openplatform', 'base_house_guangdong', 'CityID IN (7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 20, 21, 22, 214, 215, 216, 217, 218)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_zsj', 'openplatform', 'base_house_guangxi', 'CityID IN (159, 160, 161, 162, 163, 164, 197, 198, 199, 200, 201, 202, 203, 204)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_zsj', 'openplatform', 'base_house_hainan', 'CityID IN (165, 166, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 391, 392, 393, 394)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_hbh', 'openplatform', 'base_house_hebei', 'CityID IN (37, 38, 39, 40, 41, 42, 43, 44, 47, 244, 245)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_xb', 'openplatform', 'base_house_hebei', 'CityID IN (36)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_hbh', 'openplatform', 'base_house_heilongjiang', 'CityID IN (23, 24, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_csj', 'openplatform', 'base_house_hunan', 'CityID IN (67)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_zsj', 'openplatform', 'base_house_hunan', 'CityID IN (68, 69, 70, 71, 72, 73, 190, 261, 262, 263, 264, 265, 266)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_csj', 'openplatform', 'base_house_jiangsu', 'CityID IN (84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 97, 268, 269)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_zsj', 'openplatform', 'base_house_jiangxi', 'CityID IN (151, 152, 153, 154, 155, 374, 375, 376, 377, 378, 379)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_hbh', 'openplatform', 'base_house_jilin', 'CityID IN (25, 26, 206, 230, 231, 232, 233, 234, 235, 236)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_hbh', 'openplatform', 'base_house_liaoning', 'CityID IN (27, 28, 29, 31, 32, 33, 34, 35, 237, 238, 239, 240, 241, 242)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_hbh', 'openplatform', 'base_house_neimenggu', 'CityID IN (140, 141, 142, 143, 144, 329, 330, 331, 332, 333, 334, 335)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_hbh', 'openplatform', 'base_house_shandong', 'CityID IN (81, 120, 121, 122, 123, 124, 125, 127, 128, 131, 132, 133, 207, 300, 301, 302, 303)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_csj', 'openplatform', 'base_house_shanghai', 'CityID IN (2)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_hbh', 'openplatform', 'base_house_tianjin', 'CityID IN (3)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_xb', 'openplatform', 'base_house_xb', 'CityID <> 36');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_zsj', 'openplatform', 'base_house_yunnan', 'CityID IN (157, 158, 192, 193, 194, 195, 380, 381, 382, 383, 385, 386, 387, 388, 389, 390)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_zb', 'openplatform', 'base_house_zb', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_csj', 'openplatform', 'base_house_zb', 'CityID IN (101)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_csj', 'openplatform', 'base_house_zhejiang', 'CityID IN (74, 75, 76, 77, 80, 83, 187, 188, 189, 267)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_hbh', 'openplatform', 'base_house_zhejiang', 'CityID IN (130)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_hbh_sub', 'openplatform', 'base_house_beijing_sub', 'CityID IN (1)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_zsj_sub', 'openplatform', 'base_house_fujian_sub', 'CityID IN (109, 110, 111, 113, 278, 279, 280, 281, 282, 159)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_sz_sub', 'openplatform', 'base_house_guangdong_sub', 'CityID IN (6)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_zsj_sub', 'openplatform', 'base_house_guangdong_sub', 'CityID IN (7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 20, 21, 22, 214, 215, 216, 217, 218)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_zsj_sub', 'openplatform', 'base_house_guangxi_sub', 'CityID IN (159, 160, 161, 162, 163, 164, 197, 198, 199, 200, 201, 202, 203, 204)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_zsj_sub', 'openplatform', 'base_house_hainan_sub', 'CityID IN (165, 166, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 391, 392, 393, 394)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_hbh_sub', 'openplatform', 'base_house_hebei_sub', 'CityID IN (37, 38, 39, 40, 41, 42, 43, 44, 47, 244, 245)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_xb_sub', 'openplatform', 'base_house_hebei_sub', 'CityID IN (36)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_hbh_sub', 'openplatform', 'base_house_heilongjiang_sub', 'CityID IN (23, 24, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_csj_sub', 'openplatform', 'base_house_hunan_sub', 'CityID IN (67)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_zsj_sub', 'openplatform', 'base_house_hunan_sub', 'CityID IN (68, 69, 70, 71, 72, 73, 190, 261, 262, 263, 264, 265, 266)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_csj_sub', 'openplatform', 'base_house_jiangsu_sub', 'CityID IN (84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 97, 268, 269)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_zsj_sub', 'openplatform', 'base_house_jiangxi_sub', 'CityID IN (151, 152, 153, 154, 155, 374, 375, 376, 377, 378, 379)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_hbh_sub', 'openplatform', 'base_house_jilin_sub', 'CityID IN (25, 26, 206, 230, 231, 232, 233, 234, 235, 236)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_hbh_sub', 'openplatform', 'base_house_liaoning_sub', 'CityID IN (27, 28, 29, 31, 32, 33, 34, 35, 237, 238, 239, 240, 241, 242)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_hbh_sub', 'openplatform', 'base_house_neimenggu_sub', 'CityID IN (140, 141, 142, 143, 144, 329, 330, 331, 332, 333, 334, 335)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_hbh_sub', 'openplatform', 'base_house_shandong_sub', 'CityID IN (81, 120, 121, 122, 123, 124, 125, 127, 128, 131, 132, 133, 207, 300, 301, 302, 303)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_csj_sub', 'openplatform', 'base_house_shanghai_sub', 'CityID IN (2)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_hbh_sub', 'openplatform', 'base_house_tianjin_sub', 'CityID IN (3)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_xb_sub', 'openplatform', 'base_house_xb_sub', 'CityID <> 36');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_zsj_sub', 'openplatform', 'base_house_yunnan_sub', 'CityID IN (157, 158, 192, 193, 194, 195, 380, 381, 382, 383, 385, 386, 387, 388, 389, 390)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_zb_sub', 'openplatform', 'base_house_zb_sub', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_csj_sub', 'openplatform', 'base_house_zb_sub', 'CityID IN (101)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_csj_sub', 'openplatform', 'base_house_zhejiang_sub', 'CityID IN (74, 75, 76, 77, 80, 83, 187, 188, 189, 267)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_House_hbh_sub', 'openplatform', 'base_house_zhejiang_sub', 'CityID IN (130)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_P_B_H_Count', 'openplatform', 'base_count_p_b_h', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Project', 'openplatform', 'base_project', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Project_csj', 'openplatform', 'base_project', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Project_hbh', 'openplatform', 'base_project', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Project_xb', 'openplatform', 'base_project', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Project_zb', 'openplatform', 'base_project', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Project_zsj', 'openplatform', 'base_project', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Project_sub', 'openplatform', 'base_project_sub', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Project_csj_sub', 'openplatform', 'base_project_sub', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Project_hbh_sub', 'openplatform', 'base_project_sub', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Project_xb_sub', 'openplatform', 'base_project_sub', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Project_zb_sub', 'openplatform', 'base_project_sub', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_Project_zsj_sub', 'openplatform', 'base_project_sub', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_SampleAvgPrice_Month', 'openplatform', 'base_sample_avg_price_month', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_SampleProject', 'openplatform', 'base_sample_project', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_SampleProject_Weight', 'openplatform', 'base_sample_project_weight', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_SubHousePrice', 'openplatform', 'base_sub_house_price', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_SubHousePrice_csj', 'openplatform', 'base_sub_house_price', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_SubHousePrice_hbh', 'openplatform', 'base_sub_house_price', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_SubHousePrice_xb', 'openplatform', 'base_sub_house_price', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_SubHousePrice_zb', 'openplatform', 'base_sub_house_price', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_SubHousePrice_zsj', 'openplatform', 'base_sub_house_price', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_WaitProject', 'openplatform', 'base_wait_project', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_WeightBuilding', 'openplatform', 'base_weight_building', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_WeightBuilding_csj', 'openplatform', 'base_weight_building', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_WeightBuilding_hbh', 'openplatform', 'base_weight_building', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_WeightBuilding_xb', 'openplatform', 'base_weight_building', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_WeightBuilding_zb', 'openplatform', 'base_weight_building', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_WeightBuilding_zsj', 'openplatform', 'base_weight_building', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_WeightHouse_hbh', 'openplatform', 'base_weight_house_beijing', 'CityID IN (1)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_WeightHouse_zsj', 'openplatform', 'base_weight_house_fujian', 'CityID IN (109, 110, 111, 113, 278, 279, 280, 281, 282, 159)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_WeightHouse_sz', 'openplatform', 'base_weight_house_guangdong', 'CityID IN (6)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_WeightHouse_zsj', 'openplatform', 'base_weight_house_guangdong', 'CityID IN (7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 20, 21, 22, 214, 215, 216, 217, 218)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_WeightHouse_zsj', 'openplatform', 'base_weight_house_guangxi', 'CityID IN (159, 160, 161, 162, 163, 164, 197, 198, 199, 200, 201, 202, 203, 204)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_WeightHouse_zsj', 'openplatform', 'base_weight_house_hainan', 'CityID IN (165, 166, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 391, 392, 393, 394)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_WeightHouse_hbh', 'openplatform', 'base_weight_house_hebei', 'CityID IN (37, 38, 39, 40, 41, 42, 43, 44, 47, 244, 245)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_WeightHouse_xb', 'openplatform', 'base_weight_house_hebei', 'CityID IN (36)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_WeightHouse_hbh', 'openplatform', 'base_weight_house_heilongjiang', 'CityID IN (23, 24, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_WeightHouse_csj', 'openplatform', 'base_weight_house_hunan', 'CityID IN (67)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_WeightHouse_zsj', 'openplatform', 'base_weight_house_hunan', 'CityID IN (68, 69, 70, 71, 72, 73, 190, 261, 262, 263, 264, 265, 266)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_WeightHouse_csj', 'openplatform', 'base_weight_house_jiangsu', 'CityID IN (84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 97, 268, 269)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_WeightHouse_zsj', 'openplatform', 'base_weight_house_jiangxi', 'CityID IN (151, 152, 153, 154, 155, 374, 375, 376, 377, 378, 379)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_WeightHouse_hbh', 'openplatform', 'base_weight_house_jilin', 'CityID IN (25, 26, 206, 230, 231, 232, 233, 234, 235, 236)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_WeightHouse_hbh', 'openplatform', 'base_weight_house_liaoning', 'CityID IN (27, 28, 29, 31, 32, 33, 34, 35, 237, 238, 239, 240, 241, 242)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_WeightHouse_hbh', 'openplatform', 'base_weight_house_neimenggu', 'CityID IN (140, 141, 142, 143, 144, 329, 330, 331, 332, 333, 334, 335)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_WeightHouse_hbh', 'openplatform', 'base_weight_house_shandong', 'CityID IN (81, 120, 121, 122, 123, 124, 125, 127, 128, 131, 132, 133, 207, 300, 301, 302, 303)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_WeightHouse_csj', 'openplatform', 'base_weight_house_shanghai', 'CityID IN (2)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_WeightHouse_hbh', 'openplatform', 'base_weight_house_tianjin', 'CityID IN (3)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_WeightHouse_xb', 'openplatform', 'base_weight_house_xb', 'CityID <> 36');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_WeightHouse_zsj', 'openplatform', 'base_weight_house_yunnan', 'CityID IN (157, 158, 192, 193, 194, 195, 380, 381, 382, 383, 385, 386, 387, 388, 389, 390)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_WeightHouse_zb', 'openplatform', 'base_weight_house_zb', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_WeightHouse_csj', 'openplatform', 'base_weight_house_zb', 'CityID IN (101)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_WeightHouse_csj', 'openplatform', 'base_weight_house_zhejiang', 'CityID IN (74, 75, 76, 77, 80, 83, 187, 188, 189, 267)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_WeightHouse_hbh', 'openplatform', 'base_weight_house_zhejiang', 'CityID IN (130)');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_WeightProject', 'openplatform', 'base_weight_project', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_WeightProject_csj', 'openplatform', 'base_weight_project', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_WeightProject_hbh', 'openplatform', 'base_weight_project', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_WeightProject_xb', 'openplatform', 'base_weight_project', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_WeightProject_zb', 'openplatform', 'base_weight_project', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_WeightProject_zsj', 'openplatform', 'base_weight_project', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'LNK_City_Company', 'openplatform', 'base_lnk_city_company', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'LNK_P_Appendage', 'openplatform', 'base_lnk_p_appendage', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'LNK_P_B_Price', 'openplatform', 'base_lnk_p_b_price', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'LNK_P_Company', 'openplatform', 'base_lnk_p_company', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'LNK_P_Photo', 'openplatform', 'base_lnk_p_photo', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'LNK_P_Photo_sub', 'openplatform', 'base_lnk_p_photo_sub', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'SYS_Area', 'openplatform', 'sys_area', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'SYS_Area_Coordinate', 'openplatform', 'sys_area_coordinate', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'SYS_AreaLine', 'openplatform', 'sys_area_line', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'SYS_BuildingYearRange', 'openplatform', 'base_config_building_year_range', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'SYS_City', 'openplatform', 'sys_city', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'SYS_City_Table', 'openplatform', 'sys_city_table', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'SYS_Code', 'openplatform', 'sys_code', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'sys_CodePrice', 'openplatform', 'base_config_code_price', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'Sys_DiscountPrice', 'openplatform', 'base_config_discount_price', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'SYS_FloorPrice', 'openplatform', 'base_config_floor_price', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'SYS_FrontPrice', 'openplatform', 'base_config_front_price', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'Sys_ModulusPrice', 'openplatform', 'base_config_modulus_price', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'SYS_ProjectMatch', 'openplatform', 'base_config_project_match', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'SYS_PropertyAreaRange', 'openplatform', 'base_config_property_area_range', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'SYS_Province', 'openplatform', 'sys_province', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'SYS_SubArea', 'openplatform', 'sys_subarea', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'SYS_SubArea_Coordinate', 'openplatform', 'sys_subarea_coordinate', '');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FxtDataCenter', 'Privi_Company_ShowData', 'openplatform', 'privi_company_show_data', NULL);
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FxtUserCenter', 'CompanyInfo', 'openplatform', 'admin_indirect_company', '仅迁移了部分列');
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FxtDataCenter', 'Sys_EvalueSet', 'openplatform', 'sys_evalue_set', NULL);
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'DAT_ProjectAvgPrice', 'openplatform', 'base_project_avg_price', NULL);
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FxtUserCenter', 'ProductInfo', 'openplatform', 'usercenter_product_info', NULL);
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FxtUserCenter', 'CompanyProduct', 'openplatform', 'usercenter_company_product', NULL);
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'Dat_ProjectAvg', 'openplatform', 'base_project_avg', NULL);
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'Dat_ProjectAvg_csj', 'openplatform', 'base_project_avg', NULL);
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'Dat_ProjectAvg_hbh', 'openplatform', 'base_project_avg', NULL);
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'Dat_ProjectAvg_xb', 'openplatform', 'base_project_avg', NULL);
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'Dat_ProjectAvg_zb', 'openplatform', 'base_project_avg', NULL);
INSERT INTO migration_tables (OriginalDatabase, OriginalTable, NewDatabase, NewTable, WhereClause) VALUES('FXTProject', 'Dat_ProjectAvg_zsj', 'openplatform', 'base_project_avg', NULL);
COMMIT;