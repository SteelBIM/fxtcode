/*
Date: 20151118
Description:  privi_company_show_data表添加主键
*/

ALTER TABLE privi_company_show_data ADD PRIMARY KEY(FxtCompanyId, CityId, TypeCode);