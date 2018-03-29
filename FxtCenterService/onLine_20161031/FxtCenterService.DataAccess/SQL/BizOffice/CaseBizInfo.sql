SELECT cb.Id
	,cb.CityId
	,c.CityName
	,a.AreaName
	,sa.SubAreaName
	,cb.ProjectName
	,cb.BuildingName
	,cb.Address
	,CASE WHEN cb.HouseType = 1119001 THEN '住宅底商'
		WHEN cb.HouseType = 1110001 THEN '商业街商铺'
		WHEN cb.HouseType = 1107002 THEN '临街门面'
		WHEN cb.HouseType = 1119002 THEN '写字楼配套'
		WHEN cb.HouseType = 1118004 THEN '购物中心/百货'
		WHEN cb.HouseType = 1118006 THEN '宾馆酒店'
		WHEN cb.HouseType = 1124001 THEN '旅游点商铺'
		WHEN cb.HouseType = 1118002 THEN '主题卖场'
		WHEN cb.HouseType = 1118011 THEN '其他'
		END AS HouseTypeName
	,cb.BuildingArea
	,code.codename AS CaseTypeCodeName
	,cb.CaseDate
	,cb.UnitPrice
	,cb.TotalPrice
	,cb.RentRate
	,code3.CodeName as RentTypeCodeName
	,REPLACE(REPLACE(REPLACE((
					SELECT CodeName + ','
					FROM FxtDataCenter.dbo.SYS_Code c4
					WHERE ('|' + REPLACE(cb.BizCode, ',', '|') + '|') LIKE '%|' + CONVERT(NVARCHAR(128), c4.Code) + '|%'
					ORDER BY CASE WHEN c4.CodeName = '其他' THEN 1 ELSE 0 END
					FOR XML PATH('')
					) + ',', ',,', ''), '专业服务-', ''), '体验式服务-', '') AS BizCodeName
	,code1.codename AS FitmentName
	,code2.CodeName as TrafficTypeName
	,project.TrafficDetails
	,cb.SourceName
	,cb.SourceLink
	,project.IsTypical
	,cb.FloorNo
	,cb.TotalFloor
	,cb.ManagerPrice
	,(select codename from FxtDataCenter.dbo.SYS_Code with(nolock) where code = b.StructureCode) as structurecodename
	,(
		case b.iselevator 
		when 1 then '是'
		when 0 then '否'
		else ''
		end
	)as iselevatorname
FROM [FxtData_Biz].[dbo].[Dat_Case_Biz] cb WITH (NOLOCK)
left join (
	select * from FxtData_Biz.dbo.Dat_Project_Biz p with(nolock)
	where not exists(
		select ProjectId from FxtData_Biz.dbo.Dat_Project_Biz_sub ps with(nolock)
		where ps.CityId = p.CityId
		and ps.FxtCompanyId = @fxtcompanyid
		and ps.ProjectId = p.ProjectId
	)
	and p.Valid = 1
	and p.CityId = @cityid
	and p.FxtCompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT BizCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId=@cityid AND FxtCompanyId=@fxtcompanyid and TypeCode = @typecode),','))
	union
	select * from FxtData_Biz.dbo.Dat_Project_Biz_sub p with(nolock)
	where p.Valid = 1
	and p.CityId = @cityid
	and p.FxtCompanyId = @fxtcompanyid
)project on cb.ProjectId = project.ProjectId
left join (
	select IsElevator,StructureCode,buildingid from @buildingtable b with(nolock)
	where CityID = @cityid
		and Valid = 1
		and BuildingId not in(
			select buildingid from @buildingtable_sub bs with(nolock)
			where CityID = @cityid
				and bs.buildingid = b.buildingid
				and Fxt_CompanyId = @fxtcompanyid
			)
	union
	select IsElevator,StructureCode,buildingid from @buildingtable_sub with(nolock)
	where CityID = @cityid
		and valid=1 
		and Fxt_CompanyId = @fxtcompanyid
) b on b.buildingid = cb.buildingid
LEFT JOIN FxtDataCenter.dbo.SYS_City c WITH (NOLOCK) ON cb.CityId = c.CityId
LEFT JOIN FxtDataCenter.dbo.SYS_Area a WITH (NOLOCK) ON cb.AreaId = a.AreaId
LEFT JOIN FxtDataCenter.dbo.SYS_SubArea_Biz sa WITH (NOLOCK) ON cb.SubAreaId = sa.SubAreaId
LEFT JOIN FxtDataCenter.dbo.SYS_Code code WITH (NOLOCK) ON cb.CaseTypeCode = code.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code code1 WITH (NOLOCK) ON cb.Fitment = code1.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code code2 WITH (NOLOCK) ON project.TrafficType = code2.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code code3 WITH (NOLOCK) ON cb.RentTypeCode = code3.Code
WHERE 1 = 1
and cb.Valid = 1
and cb.FxtCompanyId in (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT BizCaseCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId=@cityid AND FxtCompanyId=@fxtcompanyid and TypeCode = @typecode),','))
and cb.CityId = @cityid