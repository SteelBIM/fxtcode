/*
Date: 20150810
Description: 修改PropertyCertificateNum允许NULL
*/

ALTER TABLE propertycertificate MODIFY `PropertyCertificateNum` VARCHAR(50)  NULL COMMENT '房产证号';