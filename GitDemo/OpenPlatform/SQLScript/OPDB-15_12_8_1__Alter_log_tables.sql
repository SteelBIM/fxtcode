/*
Date: 20151208
Description: 调整后台管理log表
	1. 原api_invoke_log表保持不变，后续做归档处理
	2. 新增api_invoke_log_day表，按天记录调用总数据
*/

USE openplatform;


ALTER TABLE api_invoke_log COMMENT '接口调用日志';


-- Create table: api_invoke_log_day
DROP TABLE IF EXISTS api_invoke_log_day;
CREATE TABLE `api_invoke_log_day` (
  `Id` BIGINT NOT NULL AUTO_INCREMENT COMMENT '日志ID',
  `CompanyId` INT NOT NULL COMMENT '公司ID',
  `InvokeDate` DATE COMMENT '调用时间（日）',
  `APIType` INT DEFAULT NULL COMMENT 'API类型(1:楼盘, 2:楼栋, 3:房号, 4:案例)',
  `ProductType` INT DEFAULT NULL COMMENT '产品类型',
  `TotalInvokeCount` BIGINT COMMENT '本日调用总次数',
  `TotalDataItem` BIGINT DEFAULT NULL COMMENT '本日传输数据总条数',

  PRIMARY KEY (`Id`)
)
ENGINE=INNODB
COMMENT='接口调用日志（按日统计）';


-- 将旧数据按天统计，并写入api_invoke_log_day

INSERT INTO api_invoke_log_day(InvokeDate, CompanyId, ProductType, APIType, TotalInvokeCount, TotalDataItem)
	SELECT DATE_FORMAT(ail1.InvokeTime, '%Y-%m-%d') InvokeDate
		,ail1.CompanyId
		,ail1.ProductType
		,ail1.APIType
		,COUNT(ail1.Id) TotalInvokeCount
		,SUM(ABS(ail1.DataItem)) TotalDataItem
		FROM api_invoke_log ail1
		GROUP BY InvokeDate, ail1.CompanyId, ail1.ProductType, ail1.APIType
		/*order by InvokeDate, TotalInvokeCount desc*/;
