/*
Date: 20151221
Description: 修改EVENT：每天自动归档api_invoke_log。
	bugfix: 原EVENT定义错误，只执行了一次。
*/

USE openplatform;

-- 归档旧日志
CALL archive_api_invoke_log_per_day_prc ('2015-12-07', '2015-12-21');


-- 修改EVENT
DELIMITER $$

ALTER DEFINER = 
    `root` @`localhost` EVENT `archive_api_invoke_log_per_day_event` 
    ON SCHEDULE
	EVERY 1 DAY
	STARTS '2015-12-22 01:00:00'
    ON COMPLETION PRESERVE
    ENABLE
    COMMENT '定期执行Procedure(archive_api_invoke_log_per_day_prc)' DO 
    BEGIN
        CALL archive_api_invoke_log_per_day_prc (NULL, NULL);
END $$

DELIMITER ;

