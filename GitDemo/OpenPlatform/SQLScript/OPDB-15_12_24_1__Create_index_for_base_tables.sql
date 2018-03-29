/*
Date: 20151224
Description: 新建部分索引
*/

USE openplatform;

-- base_project(CityId, ProjectId, FxtCompanyId, AreaId)
CREATE INDEX ix_base_project_1 ON base_project(CityId, ProjectId, FxtCompanyId, AreaId);

-- base_project_sub(CityId, ProjectId, FxtCompanyId, AreaId)
CREATE INDEX ix_base_project_sub_1 ON base_project_sub(CityId, ProjectId, FxtCompanyId, AreaId);


-- base_building(CityId, ProjectId, FxtCompanyId)
CREATE INDEX ix_base_building_1 ON base_building(CityId, ProjectId, FxtCompanyId);

-- base_building_sub(CityId, ProjectId, FxtCompanyId)
CREATE INDEX ix_base_building_sub_1 ON base_building_sub(CityId, ProjectId, FxtCompanyId);


-- base_case(CityId, ProjectId, BuildingId, FxtCompanyId)
CREATE INDEX ix_base_case_beijing_1 ON base_case_beijing(CityId, ProjectId, BuildingId, FxtCompanyId);
CREATE INDEX ix_base_case_fujian_1 ON base_case_fujian(CityId, ProjectId, BuildingId, FxtCompanyId);
CREATE INDEX ix_base_case_guangdong_1 ON base_case_guangdong(CityId, ProjectId, BuildingId, FxtCompanyId);
CREATE INDEX ix_base_case_guangxi_1 ON base_case_guangxi(CityId, ProjectId, BuildingId, FxtCompanyId);
CREATE INDEX ix_base_case_hainan_1 ON base_case_hainan(CityId, ProjectId, BuildingId, FxtCompanyId);
CREATE INDEX ix_base_case_hebei_1 ON base_case_hebei(CityId, ProjectId, BuildingId, FxtCompanyId);
CREATE INDEX ix_base_case_heilongjiang_1 ON base_case_heilongjiang(CityId, ProjectId, BuildingId, FxtCompanyId);
CREATE INDEX ix_base_case_hunan_1 ON base_case_hunan(CityId, ProjectId, BuildingId, FxtCompanyId);
CREATE INDEX ix_base_case_jiangsu_1 ON base_case_jiangsu(CityId, ProjectId, BuildingId, FxtCompanyId);
CREATE INDEX ix_base_case_jiangxi_1 ON base_case_jiangxi(CityId, ProjectId, BuildingId, FxtCompanyId);
CREATE INDEX ix_base_case_jilin_1 ON base_case_jilin(CityId, ProjectId, BuildingId, FxtCompanyId);
CREATE INDEX ix_base_case_liaoning_1 ON base_case_liaoning(CityId, ProjectId, BuildingId, FxtCompanyId);
CREATE INDEX ix_base_case_neimenggu_1 ON base_case_neimenggu(CityId, ProjectId, BuildingId, FxtCompanyId);
CREATE INDEX ix_base_case_shandong_1 ON base_case_shandong(CityId, ProjectId, BuildingId, FxtCompanyId);
CREATE INDEX ix_base_case_shanghai_1 ON base_case_shanghai(CityId, ProjectId, BuildingId, FxtCompanyId);
CREATE INDEX ix_base_case_tianjin_1 ON base_case_tianjin(CityId, ProjectId, BuildingId, FxtCompanyId);
CREATE INDEX ix_base_case_xb_1 ON base_case_xb(CityId, ProjectId, BuildingId, FxtCompanyId);
CREATE INDEX ix_base_case_yunnan_1 ON base_case_yunnan(CityId, ProjectId, BuildingId, FxtCompanyId);
CREATE INDEX ix_base_case_zb_1 ON base_case_zb(CityId, ProjectId, BuildingId, FxtCompanyId);
CREATE INDEX ix_base_case_zhejiang_1 ON base_case_zhejiang(CityId, ProjectId, BuildingId, FxtCompanyId);


-- base_house(BuildingId, CityId, FloorNo, FxtCompanyId)
CREATE INDEX ix_base_house_beijing_1 ON base_house_beijing(BuildingId, CityId, FloorNo, FxtCompanyId);
CREATE INDEX ix_base_house_fujian_1 ON base_house_fujian(BuildingId, CityId, FloorNo, FxtCompanyId);
CREATE INDEX ix_base_house_guangdong_1 ON base_house_guangdong(BuildingId, CityId, FloorNo, FxtCompanyId);
CREATE INDEX ix_base_house_guangxi_1 ON base_house_guangxi(BuildingId, CityId, FloorNo, FxtCompanyId);
CREATE INDEX ix_base_house_hainan_1 ON base_house_hainan(BuildingId, CityId, FloorNo, FxtCompanyId);
CREATE INDEX ix_base_house_hebei_1 ON base_house_hebei(BuildingId, CityId, FloorNo, FxtCompanyId);
CREATE INDEX ix_base_house_heilongjiang_1 ON base_house_heilongjiang(BuildingId, CityId, FloorNo, FxtCompanyId);
CREATE INDEX ix_base_house_hunan_1 ON base_house_hunan(BuildingId, CityId, FloorNo, FxtCompanyId);
CREATE INDEX ix_base_house_jiangsu_1 ON base_house_jiangsu(BuildingId, CityId, FloorNo, FxtCompanyId);
CREATE INDEX ix_base_house_jiangxi_1 ON base_house_jiangxi(BuildingId, CityId, FloorNo, FxtCompanyId);
CREATE INDEX ix_base_house_jilin_1 ON base_house_jilin(BuildingId, CityId, FloorNo, FxtCompanyId);
CREATE INDEX ix_base_house_liaoning_1 ON base_house_liaoning(BuildingId, CityId, FloorNo, FxtCompanyId);
CREATE INDEX ix_base_house_neimenggu_1 ON base_house_neimenggu(BuildingId, CityId, FloorNo, FxtCompanyId);
CREATE INDEX ix_base_house_shandong_1 ON base_house_shandong(BuildingId, CityId, FloorNo, FxtCompanyId);
CREATE INDEX ix_base_house_shanghai_1 ON base_house_shanghai(BuildingId, CityId, FloorNo, FxtCompanyId);
CREATE INDEX ix_base_house_tianjin_1 ON base_house_tianjin(BuildingId, CityId, FloorNo, FxtCompanyId);
CREATE INDEX ix_base_house_xb_1 ON base_house_xb(BuildingId, CityId, FloorNo, FxtCompanyId);
CREATE INDEX ix_base_house_yunnan_1 ON base_house_yunnan(BuildingId, CityId, FloorNo, FxtCompanyId);
CREATE INDEX ix_base_house_zb_1 ON base_house_zb(BuildingId, CityId, FloorNo, FxtCompanyId);
CREATE INDEX ix_base_house_zhejiang_1 ON base_house_zhejiang(BuildingId, CityId, FloorNo, FxtCompanyId);


-- base_house_sub(BuildingId, CityId, FloorNo, FxtCompanyId)
CREATE INDEX ix_base_house_beijing_sub_1 ON base_house_beijing_sub(BuildingId, CityId, FloorNo, FxtCompanyId);
CREATE INDEX ix_base_house_fujian_sub_1 ON base_house_fujian_sub(BuildingId, CityId, FloorNo, FxtCompanyId);
CREATE INDEX ix_base_house_guangdong_sub_1 ON base_house_guangdong_sub(BuildingId, CityId, FloorNo, FxtCompanyId);
CREATE INDEX ix_base_house_guangxi_sub_1 ON base_house_guangxi_sub(BuildingId, CityId, FloorNo, FxtCompanyId);
CREATE INDEX ix_base_house_hainan_sub_1 ON base_house_hainan_sub(BuildingId, CityId, FloorNo, FxtCompanyId);
CREATE INDEX ix_base_house_hebei_sub_1 ON base_house_hebei_sub(BuildingId, CityId, FloorNo, FxtCompanyId);
CREATE INDEX ix_base_house_heilongjiang_sub_1 ON base_house_heilongjiang_sub(BuildingId, CityId, FloorNo, FxtCompanyId);
CREATE INDEX ix_base_house_hunan_sub_1 ON base_house_hunan_sub(BuildingId, CityId, FloorNo, FxtCompanyId);
CREATE INDEX ix_base_house_jiangsu_sub_1 ON base_house_jiangsu_sub(BuildingId, CityId, FloorNo, FxtCompanyId);
CREATE INDEX ix_base_house_jiangxi_sub_1 ON base_house_jiangxi_sub(BuildingId, CityId, FloorNo, FxtCompanyId);
CREATE INDEX ix_base_house_jilin_sub_1 ON base_house_jilin_sub(BuildingId, CityId, FloorNo, FxtCompanyId);
CREATE INDEX ix_base_house_liaoning_sub_1 ON base_house_liaoning_sub(BuildingId, CityId, FloorNo, FxtCompanyId);
CREATE INDEX ix_base_house_neimenggu_sub_1 ON base_house_neimenggu_sub(BuildingId, CityId, FloorNo, FxtCompanyId);
CREATE INDEX ix_base_house_shandong_sub_1 ON base_house_shandong_sub(BuildingId, CityId, FloorNo, FxtCompanyId);
CREATE INDEX ix_base_house_shanghai_sub_1 ON base_house_shanghai_sub(BuildingId, CityId, FloorNo, FxtCompanyId);
CREATE INDEX ix_base_house_tianjin_sub_1 ON base_house_tianjin_sub(BuildingId, CityId, FloorNo, FxtCompanyId);
CREATE INDEX ix_base_house_xb_sub_1 ON base_house_xb_sub(BuildingId, CityId, FloorNo, FxtCompanyId);
CREATE INDEX ix_base_house_yunnan_sub_1 ON base_house_yunnan_sub(BuildingId, CityId, FloorNo, FxtCompanyId);
CREATE INDEX ix_base_house_zb_sub_1 ON base_house_zb_sub(BuildingId, CityId, FloorNo, FxtCompanyId);
CREATE INDEX ix_base_house_zhejiang_sub_1 ON base_house_zhejiang_sub(BuildingId, CityId, FloorNo, FxtCompanyId);
