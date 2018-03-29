/*
Date: 20150821
Description: 博纳新增预留字段9/118
*/

USE openplatform;

ALTER TABLE entrustobject ADD `survey_house_age` VARCHAR(50) COMMENT '楼龄'
,ADD `SingleBalcony` VARCHAR(100) COMMENT '单层阳台个数'
,ADD `SingleBalcony_area` VARCHAR(50) COMMENT '单层阳台面积'
,ADD `TallBalcony` VARCHAR(100) COMMENT '高挑阳台个数'
,ADD `TallBalcony_area` VARCHAR(50) COMMENT '高挑阳台面积'
,ADD `survey_house_kitchen_cupboards` VARCHAR(50) COMMENT '厨房-吊柜（没有这个字段）'
,ADD `road_name` VARCHAR(50) COMMENT '路名'
,ADD `road_width` VARCHAR(50) COMMENT '路宽'
,ADD `road_traffic_flow` VARCHAR(50) COMMENT '道路及车流量'
;
