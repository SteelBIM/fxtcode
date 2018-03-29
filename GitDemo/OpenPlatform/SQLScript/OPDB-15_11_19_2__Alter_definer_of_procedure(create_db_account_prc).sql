/*
Date: 20151119
Description: 修改存储过程create_db_user_prc的DEFINER
*/


-- procedure: create_db_user_prc
DELIMITER $$

USE mysql$$

DROP PROCEDURE IF EXISTS create_db_account_prc$$
		
CREATE DEFINER = 'root'@'localhost' PROCEDURE create_db_account_prc(
	IN v_username CHAR(80)    -- 用户名
	,IN v_host CHAR(60)    -- HOST
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
	COMMENT '操作数据库帐号（新建、修改、删除、查询等）'

BEGIN
	-- 将参数值的NULL替换为' '，以免字符串拼接时出错
	SET v_username = IFNULL(v_username, '');
	SET v_host = IFNULL(v_host, '');
	SET v_password = IFNULL(v_password, '');
	SET v_privileges = IFNULL(v_privileges, '');
	SET v_object_type = IFNULL(v_object_type, '');
	SET v_priv_level = IFNULL(v_priv_level, '');
	SET v_description = IFNULL(v_description, '');
	SET v_key_str = IFNULL(v_key_str, '');
	SET v_create_type = IFNULL(v_create_type, '');
	
	-- 查询用户权限SQL
	SET @show_grants_sql = CONCAT('SHOW GRANTS FOR ', "'", v_username, "'", '@', "'", v_host, "'");
	PREPARE	show_grants_sql FROM @show_grants_sql;

	
	-- 根据v_create_type，来新建、修改或者删除用户
	CASE UPPER(v_create_type)
		WHEN 'CREATE' THEN
			
			-- 校验用户是否存在
			
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

			-- 查询user, userinfo表用户信息, 及显示权限
			SELECT username
				,`host`
				,AES_DECRYPT(`password`, v_key_str) 'password'
				,description
				,createdate
				,updatedate
				FROM mysql.userinfo
				WHERE username = v_username
				AND `host` = v_host;
				
			SELECT *
				FROM mysql.user
				WHERE `user` = v_username
				AND `host` = v_host;
				
			EXECUTE show_grants_sql;

		WHEN 'DROP' THEN
			-- 校验用户是否存在
			
			
			-- 删除用户
			SET @drop_sql = CONCAT('DROP USER ', "'", v_username, "'" , '@', "'", v_host, "'");
			PREPARE drop_sql FROM @drop_sql;
			EXECUTE drop_sql;
			
			-- 删除userinfo表用户信息
			DELETE FROM mysql.userinfo WHERE username = v_username AND `host` = v_host;
			COMMIT;
			
		WHEN 'ADD' THEN
			-- 添加权限
			SET @grant_sql = CONCAT('GRANT ', v_privileges, ' ON ', v_object_type, ' ' , v_priv_level
						,' TO ', "'", v_username, "'" , '@', "'", v_host, "'");
			PREPARE grant_sql FROM @grant_sql;
			EXECUTE grant_sql;
			
			-- 显示权限
			EXECUTE show_grants_sql;
		
		WHEN 'REVOKE' THEN
			-- 撤销权限
			SET @revoke_sql = CONCAT('REVOKE ', v_privileges, ' ON ', v_object_type, ' ' , v_priv_level
						,' FROM ', "'", v_username, "'" , '@', "'", v_host, "'");
			PREPARE revoke_sql FROM @revoke_sql;
			EXECUTE revoke_sql;
			
			-- 显示权限
			EXECUTE show_grants_sql;
		
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
						ORDER BY username;
						
					SELECT *
						FROM mysql.user
						ORDER BY `user`;
				ELSE	
					SELECT username
						,`host`
						,AES_DECRYPT(`password`, v_key_str) 'password'
						,description
						,createdate
						,updatedate
						FROM mysql.userinfo
						WHERE username = v_username
						AND `host` = v_host;
						
					SELECT *
						FROM mysql.user
						WHERE `user` = v_username
						AND `host` = v_host;
						
					EXECUTE show_grants_sql;
				END IF;
		
		ELSE
			SELECT CONCAT("VALUE ERROR: ", v_create_type);
			
	END CASE;
END$$

DELIMITER ;