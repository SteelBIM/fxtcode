USE openplatform;

-- entrustappraise
UPDATE entrustappraise
	SET entrustIDNum = NULL
	WHERE entrustIDNum = 'NULL';
COMMIT;

UPDATE entrustappraise
	SET clientContact = NULL
	WHERE clientContact = 'NULL';
COMMIT;

UPDATE entrustappraise
	SET clientContactPhone = NULL
	WHERE clientContactPhone = 'NULL';
COMMIT;

UPDATE entrustappraise
	SET assigner = NULL
	WHERE assigner = 'NULL';
COMMIT;

-- entrustObject
UPDATE propertycertificate
	SET landCertificateAddress = NULL
	WHERE landCertificateAddress = 'NULL';
COMMIT;

