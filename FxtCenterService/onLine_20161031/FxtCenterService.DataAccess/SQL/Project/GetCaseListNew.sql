select
	r,casesum,MaxUnitPrice,MinUnitPrice,convert(numeric(18,0),(case when ctotalbuildingarea > 0 then ctotalprice / ctotalbuildingarea else 0 end)) as cunitprice,AreaId,ProjectId,ProjectName,CaseDate,CaseTypeCode,CaseTypeCodeName,BuildingArea,UnitPrice,TotalPrice,BuildingTypeCode,PurposeCode,PurposeCodeName,SourceName
from(
	select
		ROW_NUMBER() over(order by ProjectId,CaseDate desc) as r
		,COUNT(1) over() as casesum
		,Max(UnitPrice) over() as MaxUnitPrice
		,Min(UnitPrice) over() as MinUnitPrice
		,SUM(case when UnitPrice > 0 and BuildingArea > 0 then UnitPrice * BuildingArea else 0 end) over() as ctotalprice
		,SUM(case when UnitPrice > 0 and BuildingArea > 0 then BuildingArea else 0 end) over() as ctotalbuildingarea
		,*
	from(
		SELECT T.*,c1.CodeName AS CaseTypeCodeName,c2.CodeName AS PurposeCodeName
		FROM (
			SELECT T.AreaId,T.ProjectId,T.ProjectName,C.CaseDate,C.CaseTypeCode,C.BuildingArea,C.UnitPrice,C.TotalPrice,C.BuildingTypeCode,C.PurposeCode,C.SourceName
			FROM (
				SELECT ProjectId,ProjectName,AreaID FROM @projecttable p WITH (NOLOCK)
				WHERE NOT EXISTS (
						SELECT ProjectId
						FROM @projectsubtable ps WITH (NOLOCK)
						WHERE ps.ProjectId = p.ProjectId
							AND ps.CityID = @cityid
							AND ps.Fxt_CompanyId = @fxtcompanyid
						)
					AND p.Valid = 1
					AND p.CityID = @cityid
					AND p.FxtCompanyId IN (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT ShowCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid AND TypeCode = @typecode), ','))
				UNION
				SELECT ProjectId,ProjectName,AreaID FROM @projectsubtable p WITH (NOLOCK)
				WHERE p.Valid = 1
					AND p.CityID = @cityid
					AND p.Fxt_CompanyId = @fxtcompanyid
				) T
			INNER JOIN (
				SELECT ProjectId,CaseDate,CaseTypeCode,BuildingArea,UnitPrice,TotalPrice,BuildingTypeCode,PurposeCode,SourceName FROM @casetable c WITH (NOLOCK)
				WHERE c.Valid = 1
					AND c.CityID = @cityid
					AND c.FXTCompanyId IN (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT CaseCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @cityid AND FxtCompanyId = @fxtcompanyid AND TypeCode = @typecode), ','))
					AND c.CaseDate BETWEEN @casedatefrom AND @casedateto
				) C ON T.ProjectId = C.ProjectId
			) T
		LEFT JOIN FxtDataCenter.dbo.SYS_Code c1 WITH (NOLOCK) ON T.CaseTypeCode = c1.Code
		LEFT JOIN FxtDataCenter.dbo.SYS_Code c2 WITH (NOLOCK) ON T.PurposeCode = c2.Code
		WHERE 1 = 1 $where
	)T
)T
where r between (((@pageindex - 1) * @pagerecord) + 1) and (@pageindex * @pagerecord)