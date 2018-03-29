/*
Date: 20150915
Description: 博纳新增字段
*/

-- 修改注释
ALTER TABLE person MODIFY Relation VARCHAR(20) CHARSET utf8 COLLATE utf8_general_ci NULL COMMENT '产权人联系人与产权人关系';

-- 添加字段
ALTER TABLE entrust_appraise ADD EntrustAndPropertyOwnerRelation VARCHAR(20) COMMENT '委托人和产权人关系' AFTER ClientContactPhone;

ALTER TABLE entrust_appraise ADD BorrowerIsPropertyOwner TINYINT COMMENT '借款人是否为产权人';