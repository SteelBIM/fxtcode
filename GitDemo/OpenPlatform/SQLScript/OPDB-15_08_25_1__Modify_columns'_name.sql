/*
Date: 20150825
Description: 调整字段名称
*/

ALTER TABLE `openplatform`.`entrustobject`   
  CHANGE `landusetype` `LandUseType` INT(11) NULL   COMMENT '土地使用权类型',
  CHANGE `Cabletelevision` `CableTelevision` VARCHAR(10) CHARSET utf8 COLLATE utf8_general_ci NULL   COMMENT '有线电视',
  CHANGE `loclat` `LocLat` FLOAT NULL   COMMENT '纬度',
  CHANGE `loclng` `LocLng` FLOAT NULL   COMMENT '经度',
  CHANGE `survey_house_age` `SurveyHouseAge` VARCHAR(50) CHARSET utf8 COLLATE utf8_general_ci NULL   COMMENT '楼龄',
  CHANGE `SingleBalcony_area` `SingleBalconyArea` VARCHAR(50) CHARSET utf8 COLLATE utf8_general_ci NULL   COMMENT '单层阳台面积',
  CHANGE `TallBalcony_area` `TallBalconyArea` VARCHAR(50) CHARSET utf8 COLLATE utf8_general_ci NULL   COMMENT '高挑阳台面积',
  CHANGE `survey_house_kitchen_cupboards` `SurveyHouseKitchenCupboards` VARCHAR(50) CHARSET utf8 COLLATE utf8_general_ci NULL   COMMENT '厨房-吊柜',
  CHANGE `road_name` `RoadName` VARCHAR(50) CHARSET utf8 COLLATE utf8_general_ci NULL   COMMENT '路名',
  CHANGE `road_width` `RoadWidth` VARCHAR(50) CHARSET utf8 COLLATE utf8_general_ci NULL   COMMENT '路宽',
  CHANGE `road_traffic_flow` `RoadTrafficFlow` VARCHAR(50) CHARSET utf8 COLLATE utf8_general_ci NULL   COMMENT '道路及车流量';
