--select id,CityId,ProjectNameId,NetAreaName,NetName from FXTProject.dbo.SYS_ProjectMatch 
--where FXTCompanyId = @fxtcompanyid and CityId = @cityid
select pm.id,pm.CityId,pm.ProjectNameId,pm.NetAreaName,pm.NetName
from FXTProject.dbo.SYS_ProjectMatch pm inner join 
(
	SELECT projectid,CityId
	FROM @table_dat_project p WITH (NOLOCK)
	WHERE p.cityid = @cityid AND p.valid = 1 and p.fxtcompanyid in (
		select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata 
		where cityid = P.CityID and fxtcompanyid = @fxtcompanyid and typecode = @typecode), ',')
	)
	union
	SELECT projectid,CityId
	FROM @table_dat_project_sub p WITH (NOLOCK)
	WHERE p.cityid = @cityid AND p.fxt_companyid = @fxtcompanyid AND p.valid = 1	
)p on pm.CityId = p.CityId and pm.ProjectNameId = p.ProjectID
where pm.FXTCompanyId = @fxtcompanyid and pm.CityId = @cityid