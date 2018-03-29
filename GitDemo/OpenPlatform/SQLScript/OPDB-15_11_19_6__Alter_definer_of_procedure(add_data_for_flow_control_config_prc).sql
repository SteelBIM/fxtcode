/*
Date: 20151119
Description: 修改存储过程的DEFINER
*/


DELIMITER $$

USE `openplatform`$$

DROP PROCEDURE IF EXISTS `add_data_for_flow_control_config_prc`$$

CREATE DEFINER = 'root'@'localhost' PROCEDURE `add_data_for_flow_control_config_prc`(
	IN v_companyid INT 	-- 公司ID
--	,in v_apitype int 	-- API类型(1:楼盘, 2:楼栋, 3:房号, 4:案例)
--	,in v_maxdataitem int 	-- 最大可访问数据条数
--	,in v_maxcount int 	-- 最大可访问接口次数
	)
    LANGUAGE SQL
    MODIFIES SQL DATA
    SQL SECURITY DEFINER
    COMMENT '向流量配置表添加新增公司配置数据'
BEGIN
	-- 若公司配置信息已存在，则删除已有数据，重新插入；否则，直接插入。
	DECLARE v_count INT DEFAULT 0;
	
	SELECT COUNT(1) INTO v_count
		FROM flow_control_config
		WHERE CompanyId = v_companyid;
	
	IF v_count > 0
		THEN
			START TRANSACTION;
			
			DELETE FROM flow_control_config WHERE CompanyId = v_companyid;
			
			-- 插入4个API类型的配置数据
			INSERT INTO flow_control_config(CompanyId, APIType, MAXDataItem, MAXCount) VALUES
				(v_companyid, 1, -1, 100000) 
				,(v_companyid, 2, -1, 200000)
				,(v_companyid, 3, -1, 500000)
				,(v_companyid, 4, -1, 500000);
			COMMIT;
			
		ELSE
			START TRANSACTION;
			-- 插入4个API类型的配置数据
			INSERT INTO flow_control_config(CompanyId, APIType, MAXDataItem, MAXCount) VALUES
				(v_companyid, 1, -1, 100000) 
				,(v_companyid, 2, -1, 200000)
				,(v_companyid, 3, -1, 500000)
				,(v_companyid, 4, -1, 500000);
			COMMIT;
		END IF;
		
		
END$$

DELIMITER ;