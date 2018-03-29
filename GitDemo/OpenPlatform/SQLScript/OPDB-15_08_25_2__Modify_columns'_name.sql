/*
Date: 20150825
Description: 调整字段类型
*/

ALTER TABLE `openplatform`.`entrustobject`   
  CHANGE `LandUseType` `LandUseType` VARCHAR(50) NULL   COMMENT '土地使用权类型';