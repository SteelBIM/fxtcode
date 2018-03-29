/*
Date: 20151208
Description: 修改log表统计字段非空及默认值
*/

ALTER TABLE `openplatform`.`api_invoke_log_day`   
  CHANGE `InvokeDate` `InvokeDate` DATE DEFAULT '0000-00-00'  NOT NULL   COMMENT '调用时间（日）',
  CHANGE `TotalInvokeCount` `TotalInvokeCount` BIGINT(20) DEFAULT 0  NOT NULL   COMMENT '本日调用总次数',
  CHANGE `TotalDataItem` `TotalDataItem` BIGINT(20) DEFAULT 0  NOT NULL   COMMENT '本日传输数据总条数';


ALTER TABLE `openplatform`.`api_invoke_log`   
  CHANGE `InvokeTime` `InvokeTime` DATETIME DEFAULT CURRENT_TIMESTAMP  NOT NULL   COMMENT '调用时间';
