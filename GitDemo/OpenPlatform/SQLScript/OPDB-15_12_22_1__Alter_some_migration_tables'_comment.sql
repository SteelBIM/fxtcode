/*
Date: 20151222
Description: 修改已迁移表的部分注释
*/


ALTER TABLE `openplatform`.`admin_indirect_company`   
  CHANGE `UpdateDate` `UpdateDate` DATETIME ON UPDATE CURRENT_TIMESTAMP NULL   COMMENT '修改时间（新增）';
  
