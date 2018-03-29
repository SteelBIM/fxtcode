/*
Date: 20151130
Description: 修改字段长度
*/


USE op_bona;

ALTER TABLE `entrust_appraise`   
  MODIFY `ClientContact` VARCHAR(50) NULL COMMENT '委托方联系人';