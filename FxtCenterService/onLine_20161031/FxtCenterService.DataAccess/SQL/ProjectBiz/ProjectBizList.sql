select @top CityId,AreaId,AreaName,ProjectId,ProjectName,Address 
from
(
	select @top a.CityId,a.AreaId,b.AreaName,a.ProjectId,a.ProjectName,a.Address from FxtData_Biz.dbo.Dat_Project_Biz a WITH (nolock)
	left join FxtDataCenter.dbo.SYS_Area b WITH (nolock) 
	on a.CityId=b.CityId and a.AreaId=b.AreaId
	where a.CityId=@cityid and a.Valid=1
		AND a.fxtcompanyid IN
        (
			SELECT value
			FROM fxtproject.dbo.splittotable(
			(
			SELECT showcompanyid
			FROM fxtdatacenter.dbo.privi_company_showdata
			WHERE cityid = @cityid AND fxtcompanyid = @fxtcompanyid AND typecode = @typecode), ',')
		) $keylimit
	union
	select @top a.CityId,a.AreaId,b.AreaName,a.ProjectId,a.ProjectName,a.Address from FxtData_Biz.dbo.Dat_Project_Biz_sub a WITH (nolock)
	left join FxtDataCenter.dbo.SYS_Area b WITH (nolock) 
	on a.CityId=b.CityId and a.AreaId=b.AreaId
	where a.CityId=@cityid and a.fxtcompanyid = @fxtcompanyid and a.Valid=1 $keylimit
)a