/*
Date: 20151119
Description: 授予updateadmin用户SUPER权限
*/

CALL mysql.create_db_account_prc(
	'updateadmin'
	,'%'
	,NULL
	,'SUPER'
	,NULL
	,'*.*'
	,NULL
	,NULL
	,'ADD'
	);
	