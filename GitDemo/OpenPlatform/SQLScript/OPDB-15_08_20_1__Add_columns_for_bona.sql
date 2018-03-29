/*
Date: 20150820
Description: 博纳新增预留字段80/118
*/
USE openplatform;

DROP TABLE IF EXISTS buyer;
CREATE TABLE buyer(BId BIGINT AUTO_INCREMENT
    ,BuyerGUID CHAR(64) COMMENT 'GUID'
    ,GJBObjId BIGINT COMMENT '估价宝委估对象Id(src:EntrustObject)'
    ,BuyerName VARCHAR(20) COMMENT '购房人姓名'
    ,IDNum VARCHAR(20) COMMENT '身份证'
    ,Phone CHAR(20) COMMENT '联系电话'
    ,CreateDate DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
    ,PRIMARY KEY(BId)
    )
    COMMENT '买房人信息';

-- 新增字段80
ALTER TABLE entrustobject ADD `OnlyLivingRoom` INT COMMENT '是否唯一住房'
,ADD `Number` VARCHAR(100) COMMENT '层户数'
,ADD `CompleteTime` VARCHAR(20) COMMENT '竣工时间'
,ADD `Wall` VARCHAR(50) COMMENT '外墙装修'
,ADD `Front` VARCHAR(50) COMMENT '朝向'
,ADD `Sight` VARCHAR(50) COMMENT '景观'
,ADD `PropertyPrice` VARCHAR(100) COMMENT '物管费'
,ADD `Villa` VARCHAR(20) COMMENT '小区规模 别墅栋数'
,ADD `AverageHouse` VARCHAR(20) COMMENT '小区规模 普通住宅栋数'
,ADD `NotAverageHouse` VARCHAR(20) COMMENT '小区规模 非普通住宅栋数'
,ADD `GreenEnvironment` VARCHAR(50) COMMENT '绿化环境'
,ADD `AirQuality` VARCHAR(50) COMMENT '空气质量'
,ADD `landusetype` INT COMMENT '土地使用权类型'
,ADD `HouseStruct` VARCHAR(50) COMMENT '户型结构'
,ADD `HallCount` VARCHAR(100) COMMENT '厅'
,ADD `BathroomCount` VARCHAR(100) COMMENT '卫'
,ADD `HasKitchen` VARCHAR(10) COMMENT '厨房'
,ADD `Terrace` DECIMAL(18,2) COMMENT '露台面积'
,ADD `Roof` DECIMAL(18,2) COMMENT '天台面积'
,ADD `Garden` DECIMAL(18,2) COMMENT '入户花园面积'
,ADD `StructNewProbability` VARCHAR(50) COMMENT '结构成新率'
,ADD `LayerHigh` VARCHAR(50) COMMENT '层高'
,ADD `Ventilation` VARCHAR(50) COMMENT '通风采光'
,ADD `NoisePollution` VARCHAR(50) COMMENT '噪声污染'
,ADD `DecorationProbabilit` VARCHAR(50) COMMENT '装修成新率'
,ADD `LvYear` VARCHAR(50) COMMENT '装修年代'
,ADD `ParlorCeiling` VARCHAR(50) COMMENT '客厅-天花'
,ADD `ParlorWall` VARCHAR(50) COMMENT '客厅-墙面'
,ADD `ParlorGround` VARCHAR(50) COMMENT '客厅-地面'
,ADD `BedroomCeiling` VARCHAR(50) COMMENT '卧室-天花'
,ADD `BedroomWall` VARCHAR(50) COMMENT '卧室-墙面'
,ADD `BedroomGround` VARCHAR(50) COMMENT '卧室-地面'
,ADD `KitchenCeiling` VARCHAR(50) COMMENT '厨房-天花'
,ADD `KitchenWall` VARCHAR(50) COMMENT '厨房-墙面'
,ADD `KitchenGround` VARCHAR(50) COMMENT '厨房-地面'
,ADD `KitchenDesk` VARCHAR(50) COMMENT '厨房-工作台'
,ADD `ToiletsCeiling` VARCHAR(50) COMMENT '洗手间-天花'
,ADD `ToiletsWall` VARCHAR(50) COMMENT '洗手间-墙面'
,ADD `ToiletsGround` VARCHAR(50) COMMENT '洗手间-地面'
,ADD `ToiletsHealth` VARCHAR(50) COMMENT '洗手间-卫生洁具'
,ADD `ToiletsBath` VARCHAR(50) COMMENT '洗手间-浴具'
,ADD `Toilet` VARCHAR(50) COMMENT '洗手间座便器'
,ADD `BigDoor` VARCHAR(50) COMMENT '入户门'
,ADD `InDoor` VARCHAR(50) COMMENT '内门'
,ADD `RoomDoor` VARCHAR(50) COMMENT '房门'
,ADD `Window` VARCHAR(50) COMMENT '窗'
,ADD `IntelligentSystems` VARCHAR(10) COMMENT '智能系统'
,ADD `SmokeSystems` VARCHAR(10) COMMENT '烟感报警'
,ADD `SpraySystems` VARCHAR(10) COMMENT '自动喷淋'
,ADD `GasSystems` VARCHAR(10) COMMENT '管道燃气'
,ADD `IntercomSystems` VARCHAR(10) COMMENT '对讲系统'
,ADD `Broadband` VARCHAR(10) COMMENT '宽带'
,ADD `Cabletelevision` VARCHAR(10) COMMENT '有线电视'
,ADD `Phone` VARCHAR(10) COMMENT '电话'
,ADD `Heating` VARCHAR(10) COMMENT '暖气'
,ADD `ClientElevator` VARCHAR(50) COMMENT '客梯'
,ADD `CabinBrand` VARCHAR(50) COMMENT '客梯品牌'
,ADD `LadderBrand` VARCHAR(50) COMMENT '消防梯品牌'
,ADD `IsUpCarLocation` VARCHAR(100) COMMENT '地上车位'
,ADD `IsDownCarLocation` VARCHAR(100) COMMENT '地下车位'
,ADD `IsCar` VARCHAR(10) COMMENT '车位是否充足'
,ADD `CarOccupy` VARCHAR(20) COMMENT '车户比'
,ADD `Movement` VARCHAR(50) COMMENT '运动场所'
,ADD `Club` VARCHAR(50) COMMENT '会所'
,ADD `HealthCenter` VARCHAR(50) COMMENT '社康中心'
,ADD `PostOffice` VARCHAR(50) COMMENT '邮局'
,ADD `Bank` VARCHAR(50) COMMENT '银行'
,ADD `Market` VARCHAR(50) COMMENT '商场/菜市场'
,ADD `HighSchool` VARCHAR(50) COMMENT '中学'
,ADD `PrimarySchool` VARCHAR(50) COMMENT '小学'
,ADD `Nursery` VARCHAR(50) COMMENT '幼儿园'
,ADD `TrafficConvenient` VARCHAR(50) COMMENT '交通便捷度'
,ADD `BusDistance` VARCHAR(100) COMMENT '离公交站台距离'
,ADD `Metro` VARCHAR(100) COMMENT '地铁线路'
,ADD `SubwayDistance` VARCHAR(100) COMMENT '离地铁站距离'
,ADD `TrafficManagement` VARCHAR(50) COMMENT '是否有交通管制'
,ADD `Side` VARCHAR(100) COMMENT '周边住宅'
,ADD `Environment` VARCHAR(150) COMMENT '小区及周边环境'
,ADD `loclat` FLOAT COMMENT '经度'
,ADD `loclng` FLOAT COMMENT '纬度'
;