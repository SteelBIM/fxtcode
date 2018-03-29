/*
Date:20150806
Description: 将字段数据类型更改为VARCHAR类型
*/

ALTER TABLE entrustobject MODIFY `BusLineNum` VARCHAR(100) NULL DEFAULT NULL COMMENT '公交线路数量';
ALTER TABLE entrustobject MODIFY `HousingLocation` VARCHAR(100) NULL DEFAULT NULL COMMENT '房产位置';
ALTER TABLE entrustobject MODIFY `PublicFacilitiesNum` VARCHAR(100) NULL DEFAULT NULL COMMENT '公共配套设施数量';


UPDATE entrustobject SET BusLineNum = '>=6条', HousingLocation = '5km<与市级商圈辐射半径<=10km', PublicFacilitiesNum = '公共配套设施>8' WHERE EOID =1;

COMMIT;