/*
Date: 20150829
Description: 新建userinfo表，保存数据库帐号信息
*/

USE mysql;

DROP TABLE IF EXISTS userinfo;

CREATE TABLE userinfo(id INT AUTO_INCREMENT PRIMARY KEY
	,username CHAR(80) NOT NULL
	,`password` BLOB NOT NULL
	,description VARCHAR(200) NOT NULL
	,createdate DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
	,updatetime DATETIME ON UPDATE CURRENT_TIMESTAMP
	)
	ENGINE = MYISAM
	COMMENT '开放平台数据库用户帐号信息记录';