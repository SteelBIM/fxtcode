/*
Date: 20151013
Description: admin_company(IfVersionId)外键关联到admin_interface_version(Id)
*/

ALTER TABLE admin_company ADD CONSTRAINT FK_IfVersionId FOREIGN KEY(IfVersionId) REFERENCES admin_interface_version(Id);