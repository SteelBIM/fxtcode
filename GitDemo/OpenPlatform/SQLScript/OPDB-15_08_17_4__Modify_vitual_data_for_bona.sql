/*
Date: 20150817
Description: 更改entrustappraise身份证号数据为18位
*/


-- SELECT EntrustIDNum, LENGTH(EntrustIDNum) FROM entrustappraise WHERE LENGTH(EntrustIDNum) <> 18;

UPDATE entrustappraise
    SET EntrustIDNum = rand_idnum_func(eaid);
COMMIT;