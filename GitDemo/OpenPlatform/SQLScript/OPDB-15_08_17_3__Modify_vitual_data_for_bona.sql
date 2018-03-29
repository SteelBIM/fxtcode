/*
Date: 20150817
Description: 修改人物生日信息
*/

UPDATE person
    SET birthday = SUBSTRING(idnum, 7, 8);
COMMIT;