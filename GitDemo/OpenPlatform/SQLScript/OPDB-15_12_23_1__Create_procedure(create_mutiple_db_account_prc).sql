/*
Date: 20151223
Description: 新建存储过程mysql.create_mutiple_db_account_prc
	用途: 创建user名相同，包含多个host的account，
	      现有存储过程mysql.create_db_account_prc每次只能
	      新建包含一个host的user。
*/


-- procedure: create_mutiple_db_account_prc
DELIMITER $$

USE mysql$$

DROP PROCEDURE IF EXISTS create_mutiple_db_account_prc$$

CREATE DEFINER = 'root'@'localhost' PROCEDURE create_mutiple_db_account_prc(
	IN v_username CHAR(80)    -- 用户名
	,IN v_all_host CHAR(60)    -- HOST（多个host以逗号(',')分隔）
	,IN v_password CHAR(41)    -- 密码
	,IN v_privileges VARCHAR(255)    -- 权限列表: SELECT, INSERT, UPDATE, DELETE, etc.
	,IN v_object_type VARCHAR(20)    -- TABLE, FUNCTION, PROCEDURE(默认留空即可)
	,IN v_priv_level VARCHAR(255)    -- 权限level: *.*, db_name.*, db_name.tbl_name, etc.
	,IN v_description VARCHAR(255)    -- 帐号用途
	,IN v_key_str VARCHAR(255)    -- 加密字符串
	,IN v_create_type VARCHAR(10)    -- 新建用户(create), 删除用户(drop), 添加权限(add), 撤销权限(revoke), 查询用户信息及权限(query).
	)
	LANGUAGE SQL
	DETERMINISTIC
	MODIFIES SQL DATA
	SQL SECURITY DEFINER
	COMMENT '操作数据库帐号（新建、修改、删除、查询等），含多个host'

BEGIN

	DECLARE v_delimiter VARCHAR(10) DEFAULT ',';	-- host分隔符
	DECLARE v_str_num INT DEFAULT 0;	-- host个数
	DECLARE i INT DEFAULT 1;	-- 计数器
	DECLARE v_host VARCHAR(20);	-- 单个host
	
	-- 将参数值的NULL替换为' '，以免字符串拼接时出错
	SET v_username = IFNULL(v_username, '');
	SET v_all_host = IFNULL(v_all_host, '120.25.3.60, 120.25.59.211, 10.170.15.106, 10.170.131.53');
	SET v_password = IFNULL(v_password, '');
	SET v_privileges = IFNULL(v_privileges, '');
	SET v_object_type = IFNULL(v_object_type, '');
	SET v_priv_level = IFNULL(v_priv_level, '');
	SET v_description = IFNULL(v_description, '');
	SET v_key_str = IFNULL(v_key_str, '');
	SET v_create_type = IFNULL(v_create_type, '');
	
	
	-- 获取host个数
	SET v_str_num = CHAR_LENGTH(v_all_host) - CHAR_LENGTH(REPLACE(v_all_host, v_delimiter, '')) + 1;
	
	
	-- 根据v_create_type，来新建、修改或者删除用户
	CASE UPPER(v_create_type)
		WHEN 'CREATE' THEN
			WHILE i <= v_str_num DO
				SET v_host = TRIM(SUBSTRING_INDEX(SUBSTRING_INDEX(v_all_host, v_delimiter, i), v_delimiter, -1));
				
				-- 校验用户是否存在
				-- ...
				
				/*
				-- 新建用户（仅新建）
				SET @create_sql = CONCAT('CREATE USER ', "'", v_username, "'" , '@', "'", v_host, "'", ' IDENTIFIED BY ', "'", v_password, "'");
				PREPARE create_sql FROM @create_sql;
				EXECUTE create_sql;
				*/
				
				-- 授予权限（含新建）
				SET @grant_sql = CONCAT('GRANT ', v_privileges, ' ON ', v_object_type, ' ' , v_priv_level
							,' TO ', "'", v_username, "'" , '@', "'", v_host, "'", ' IDENTIFIED BY ', "'", v_password, "'");
				PREPARE grant_sql FROM @grant_sql;
				EXECUTE grant_sql;

				-- 将帐号、密码信息写入userinfo表
				INSERT INTO mysql.userinfo(username, `host`, `password`, description)
					VALUES(v_username, v_host, AES_ENCRYPT(v_password, v_key_str), v_description);
				COMMIT;
				
				SET i = i + 1;
			END WHILE;
			
			-- 查询user, userinfo表用户信息, 及显示权限
			SELECT username
				,`host`
				,AES_DECRYPT(`password`, v_key_str) 'password'
				,description
				,createdate
				,updatedate
				FROM mysql.userinfo
				WHERE username = v_username
				ORDER BY username, `host`;
				
			SELECT *
				FROM mysql.user
				WHERE `user` = v_username
				ORDER BY `user`, `host`;

		WHEN 'DROP' THEN
			WHILE i <= v_str_num DO
				SET v_host = TRIM(SUBSTRING_INDEX(SUBSTRING_INDEX(v_all_host, v_delimiter, i), v_delimiter, -1));

				-- 校验用户是否存在
				-- ...
				
				-- 删除用户
				SET @drop_sql = CONCAT('DROP USER ', "'", v_username, "'" , '@', "'", v_host, "'");
				PREPARE drop_sql FROM @drop_sql;
				EXECUTE drop_sql;
				
				-- 删除userinfo表用户信息
				DELETE FROM mysql.userinfo WHERE username = v_username AND `host` = v_host;
				COMMIT;
				
				SET i = i + 1;
			END WHILE;
			
			-- 查询已删除user
			SELECT username
				,`host`
				,AES_DECRYPT(`password`, v_key_str) 'password'
				,description
				,createdate
				,updatedate
				FROM mysql.userinfo
				WHERE username = v_username
				ORDER BY username, `host`;
				
			SELECT *
				FROM mysql.user
				WHERE `user` = v_username
				ORDER BY `user`, `host`;
			
			
		WHEN 'ADD' THEN
			WHILE i <= v_str_num DO
				SET v_host = TRIM(SUBSTRING_INDEX(SUBSTRING_INDEX(v_all_host, v_delimiter, i), v_delimiter, -1));
				
				-- 添加权限
				SET @grant_sql = CONCAT('GRANT ', v_privileges, ' ON ', v_object_type, ' ' , v_priv_level
							,' TO ', "'", v_username, "'" , '@', "'", v_host, "'");
				PREPARE grant_sql FROM @grant_sql;
				EXECUTE grant_sql;
				
				SET i = i + 1;
			END WHILE;
		
		WHEN 'REVOKE' THEN
			WHILE i <= v_str_num DO
				SET v_host = TRIM(SUBSTRING_INDEX(SUBSTRING_INDEX(v_all_host, v_delimiter, i), v_delimiter, -1));
				
				-- 撤销权限
				SET @revoke_sql = CONCAT('REVOKE ', v_privileges, ' ON ', v_object_type, ' ' , v_priv_level
							,' FROM ', "'", v_username, "'" , '@', "'", v_host, "'");
				PREPARE revoke_sql FROM @revoke_sql;
				EXECUTE revoke_sql;

				SET i = i + 1;
			END WHILE;
		
		WHEN 'QUERY' THEN
		-- 查询user, userinfo表用户信息, 及显示权限
		-- 如果没有输入用户名，则默认查询所有用户信息
			IF (v_username IS NULL OR v_username = '')
				THEN
					SELECT username
						,`host`
						,AES_DECRYPT(`password`, v_key_str) 'password'
						,description
						,createdate
						,updatedate
						FROM mysql.userinfo
						ORDER BY username, `host`;
						
					SELECT *
						FROM mysql.user
						ORDER BY `user`, `host`;
				ELSE	
					SELECT username
						,`host`
						,AES_DECRYPT(`password`, v_key_str) 'password'
						,description
						,createdate
						,updatedate
						FROM mysql.userinfo
						WHERE username = v_username
						ORDER BY `host`;
						
					SELECT *
						FROM mysql.user
						WHERE `user` = v_username
						ORDER BY `host`;
						
			END IF;
		
		ELSE
			SELECT CONCAT("VALUE ERROR: ", v_create_type);
			
	END CASE;
	
	IF UPPER(v_create_type) IN ('CREATE', 'ADD', 'REVOKE', 'QUERY')
		AND v_username IS NOT NULL
		AND v_username <> ''
		THEN
			-- 查询用户权限SQL
			WHILE i <= v_str_num DO
				SET v_host = TRIM(SUBSTRING_INDEX(SUBSTRING_INDEX(v_all_host, v_delimiter, i), v_delimiter, -1));
				
				SET @show_grants_sql = CONCAT('SHOW GRANTS FOR ', "'", v_username, "'", '@', "'", v_host, "'");
				PREPARE	show_grants_sql FROM @show_grants_sql;
				EXECUTE show_grants_sql;
				
				SET i = i + 1;
			END WHILE;
	END IF;

END$$

DELIMITER ;