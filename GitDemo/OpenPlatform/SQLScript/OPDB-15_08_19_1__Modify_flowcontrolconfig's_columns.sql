/*
Date: 20150819
Description: 流量控制字段修改
*/

ALTER TABLE flowcontrolconfig MODIFY `APIType` INT(1) NULL DEFAULT NULL COMMENT 'API类型(1:楼盘, 2:楼栋, 3:房号, 4:案例)';
ALTER TABLE flowcontrolconfig DROP COLUMN StartDate;
