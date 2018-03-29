ALTER TABLE CompanyProduct
	ADD MaxAccountNumber INT
	
UPDATE CompanyProduct SET MaxAccountNumber=1000 WHERE ProductTypeCode=1003018