/*
Date: 20151221
Descrition: api_invoke_log添加版本说明字段（ServerVersion）
*/


USE openplatform;


-- 添加字段
ALTER TABLE `api_invoke_log`   
  ADD `ServerVersion` VARCHAR(50) NULL COMMENT '接口版本说明' AFTER `ProductType`;
  
ALTER TABLE `api_invoke_log_day`   
  ADD `ServerVersion` VARCHAR(50) NULL COMMENT '接口版本说明' AFTER `InvokeDate`;


-- 修改归档Procedure
-- procedure
DELIMITER $$

USE `openplatform`$$

DROP PROCEDURE IF EXISTS `archive_api_invoke_log_per_day_prc`$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `archive_api_invoke_log_per_day_prc`(
IN v_start_date DATE 	-- 归档起始日期
,IN v_end_date DATE 	-- 归档截止日期
)
    MODIFIES SQL DATA
    COMMENT '按天统计api_invoke_log，并写入表api_invoke_log_day'
BEGIN
	SET v_start_date = IFNULL(v_start_date, ADDDATE(CURRENT_DATE, -1));
	SET v_end_date = IFNULL(v_end_date, CURRENT_DATE);
	
	START TRANSACTION;
	-- 删除已插入的当前日期范围的统计数据，避免数据重复
	DELETE FROM api_invoke_log_day
		WHERE InvokeDate >= v_start_date
		AND InvokeDate < v_end_date;
	
	-- 写入当前日期范围的统计数据
	INSERT INTO api_invoke_log_day(InvokeDate, CompanyId, ServerVersion, ProductType, APIType, TotalInvokeCount, TotalDataItem)
	SELECT DATE_FORMAT(ail.InvokeTime, '%Y-%m-%d') InvokeDate
		,ail.CompanyId
		,ail.ServerVersion
		,ail.ProductType
		,ail.APIType
		,IFNULL(COUNT(ail.Id), 0) TotalInvokeCount
		,IFNULL(SUM(ABS(ail.DataItem)), 0) TotalDataItem
		FROM api_invoke_log ail
		WHERE DATE_FORMAT(ail.InvokeTime, '%Y-%m-%d') >= v_start_date
		AND DATE_FORMAT(ail.InvokeTime, '%Y-%m-%d') < v_end_date
		GROUP BY InvokeDate, ail.CompanyId, ail.ServerVersion, ail.ProductType, ail.APIType;
	COMMIT;
END$$

DELIMITER ;
