/*
Date: 20150825
Description: 新增买房人数据
*/

INSERT INTO buyer(BuyerGUID, GJBObjId, BuyerName, IDNum, Phone, CreateDate)
	VALUES(UUID(), 998, '李友全', '681368199410251863', '15602356953', NOW())
		,(UUID(), 998, '李静', '146914198004096419', '15502563626', NOW())
		,(UUID(), 929, '吴晓静', '135853198305165339', '15502563626', NOW());
COMMIT;