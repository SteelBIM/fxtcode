/*
Date: 20151208
Description: 每天定期归档api_invoke_log数据
*/

-- 开启event sheduler
SET GLOBAL event_scheduler = ON;


DELIMITER $$

USE openplatform$$


-- 新建存储过程：archive_api_invoke_log_per_day_prc
DROP PROCEDURE IF EXISTS archive_api_invoke_log_per_day_prc$$

CREATE DEFINER = 'root'@'localhost' PROCEDURE archive_api_invoke_log_per_day_prc(
IN v_start_date DATE 	-- 归档起始日期
,IN v_end_date DATE 	-- 归档截止日期
)
LANGUAGE SQL
MODIFIES SQL DATA
SQL SECURITY DEFINER
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
	INSERT INTO api_invoke_log_day(InvokeDate, CompanyId, ProductType, APIType, TotalInvokeCount, TotalDataItem)
	SELECT DATE_FORMAT(ail.InvokeTime, '%Y-%m-%d') InvokeDate
		,ail.CompanyId
		,ail.ProductType
		,ail.APIType
		,IFNULL(COUNT(ail.Id), 0) TotalInvokeCount
		,IFNULL(SUM(ABS(ail.DataItem)), 0) TotalDataItem
		FROM api_invoke_log ail
		WHERE DATE_FORMAT(ail.InvokeTime, '%Y-%m-%d') >= v_start_date
		AND DATE_FORMAT(ail.InvokeTime, '%Y-%m-%d') < v_end_date
		GROUP BY InvokeDate, ail.CompanyId, ail.ProductType, ail.APIType;
	COMMIT;
END$$


-- 新建EVENT
DROP EVENT IF EXISTS archive_api_invoke_log_per_day_event$$

CREATE DEFINER = 'root'@'localhost' EVENT archive_api_invoke_log_per_day_event
ON SCHEDULE
	AT '2015-12-10 01:00:00' + INTERVAL 1 DAY
		
ON COMPLETION PRESERVE
ENABLE
COMMENT '定期执行Procedure(archive_api_invoke_log_per_day_prc)'

DO
	BEGIN
		CALL archive_api_invoke_log_per_day_prc(NULL, NULL);
	END$$
	
DELIMITER ;



