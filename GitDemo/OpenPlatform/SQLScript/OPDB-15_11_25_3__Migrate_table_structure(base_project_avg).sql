/*
Date: 20151125
Description: 迁移表结构：
	1. FXTProject.dbo.Dat_ProjectAvg（含5个分表）

*/

USE openplatform;

DROP TABLE IF EXISTS base_project_avg;

/*==============================================================*/
/* Table: base_project_avg                                      */
/*==============================================================*/
CREATE TABLE base_project_avg
(
   ProjectAvgId         INT NOT NULL COMMENT '主键iD，自增长',
   FxtCompanyId         INT NOT NULL COMMENT '公司ID',
   CityId               INT NOT NULL COMMENT '城市ID',
   AreaId               INT COMMENT '区域ID',
   SubAreaId            INT COMMENT '片区Id',
   ProjectAvgPrice      INT NOT NULL COMMENT '楼盘案例均价',
   CaseMaxPrice         INT NOT NULL COMMENT '案例最高价',
   CaseMinPrice         INT NOT NULL COMMENT '案例最低价',
   CaseCount            INT NOT NULL COMMENT '案例数',
   ProjectGained        DECIMAL(8,4) COMMENT '楼盘涨幅比',
   UseMonth             DATETIME NOT NULL COMMENT '那个月的案例均价',
   CreateDate           DATETIME NOT NULL COMMENT '创建时间',
   UpdateDate           DATETIME NOT NULL COMMENT '修改时间',
   UpdateUser           VARCHAR(50) COMMENT '修改人',
   ProjectId            INT,
   Valid                INT,
   StartDate            DATETIME,
   EndDate              DATETIME,
   IsEValue             TINYINT DEFAULT 0,
   PRIMARY KEY (ProjectAvgId, CityId)
)
COMMENT '(src: FXTProject.dbo.Dat_ProjectAvg)';
