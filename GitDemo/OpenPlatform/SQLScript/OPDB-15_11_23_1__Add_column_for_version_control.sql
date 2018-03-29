/*
Date: 20151123
Description: 后台管理，添加字段：版本路径
*/

ALTER TABLE admin_interface_version ADD Path VARCHAR(255) COMMENT '版本目录';
