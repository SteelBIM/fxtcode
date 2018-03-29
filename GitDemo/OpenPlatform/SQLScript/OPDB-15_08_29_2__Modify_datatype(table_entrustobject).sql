/*
Date: 20150829
Description: 修改字段类型
*/

USE openplatform;
ALTER TABLE `openplatform`.`entrustobject`   
  CHANGE `TotalFloor` `TotalFloor` VARCHAR(50) NULL   COMMENT '总楼层',
  CHANGE `Floor` `Floor` VARCHAR(50) NULL   COMMENT '楼层',
  CHANGE `RoomNum` `RoomNum` VARCHAR(50) NULL   COMMENT '房(房间数)',
  CHANGE `BalconyNum` `BalconyNum` VARCHAR(50) NULL   COMMENT '阳台(阳台数)';
