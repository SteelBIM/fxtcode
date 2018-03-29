/*
Date: 20151125
Description: 迁移表结构：
	1. 向表base_weight_project添加新增的字段：UpdateUser, UseMonth
*/

USE openplatform;

ALTER TABLE `base_weight_project`   
  ADD `UpdateUser` VARCHAR(50) NULL   COMMENT '修改人是谁' AFTER `EvaluationCompanyId`,
  ADD `UseMonth` DATETIME NULL AFTER `UpdateUser`;