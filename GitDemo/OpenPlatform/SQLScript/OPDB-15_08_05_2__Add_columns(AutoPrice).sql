/*
Date: 20150805
Description: appraiseobjectprice表添加字段AutoPrice(自动估价价格)
*/
ALTER TABLE appraiseobjectprice ADD AutoPrice DECIMAL(18, 4) COMMENT '自动估价价格';
UPDATE appraiseobjectprice SET AutoPrice = 33000.00 WHERE aopid = 1;
COMMIT;

-- 表EntrustObject字段IsSurvey注释修改
ALTER TABLE EntrustObject MODIFY `IsSurvey` BIT(1) NULL DEFAULT NULL COMMENT '是否现场查勘(0:否, 1:是)';