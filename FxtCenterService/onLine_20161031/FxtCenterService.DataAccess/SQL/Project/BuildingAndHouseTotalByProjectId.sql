select COUNT(b.buildingid) as buildingtotal, 
(
	select COUNT(h.houseid)  from
	--楼栋
	(
			select BuildingId,ProjectId
			from @buildingtable b with(nolock)
			where not exists (
						select BuildingId from @buildingtable_sub bb where bb.BuildingId = b.BuildingId
							and b.fxtcompanyid = @FxtCompanyId
						)
			and b.fxtcompanyid in (
					select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata 
					where cityid = @CityId and fxtcompanyid = @FxtCompanyId and typecode = @TypeCode), ',') 
					)
			and b.CityID = @CityId
			and b.valid = 1
			union
			select BuildingId,ProjectId
			from @buildingtable_sub bb with(nolock)
			where bb.Fxt_CompanyId = @FxtCompanyId
			and bb.CityID = @CityId
			and bb.valid = 1
	) b 
	left join 
	--房号
	(	
		select 
		h.houseid,BuildingId
		from @housetable h with(nolock)
		where not exists(
					select HouseId from @housetable_sub hb with(nolock) where hb.HouseId = h.HouseId
						and hb.fxtcompanyid = @FxtCompanyId
					)
		and h.fxtcompanyid in (
							select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata 
							where cityid = @CityId and fxtcompanyid = @FxtCompanyId and typecode = @TypeCode), ',')
							)
		and h.CityID = @CityId
		and h.valid = 1
		union
		select hb.houseid,hb.BuildingId from @housetable_sub hb with(nolock)
		where  hb.fxtcompanyid = @FxtCompanyId
		and hb.CityID = @CityId
		and hb.valid = 1
	) as h on b.BuildingId = h.BuildingId
	where b.projectid = @projectid
) as housetotal
from
--楼栋
(
			select BuildingId,ProjectId
			from @buildingtable b with(nolock)
			where not exists (
						select BuildingId from @buildingtable_sub bb where bb.BuildingId = b.BuildingId
							and b.fxtcompanyid = @FxtCompanyId
						)
			and b.fxtcompanyid in (
					select value from fxtproject.dbo.splittotable((select showcompanyid from fxtdatacenter.dbo.privi_company_showdata 
					where cityid = @CityId and fxtcompanyid = @FxtCompanyId and typecode = @TypeCode), ',') 
					)
			and b.CityID = @CityId
			and b.valid = 1
			union
			select BuildingId,ProjectId
			from @buildingtable_sub bb with(nolock)
			where bb.Fxt_CompanyId = @FxtCompanyId
			and bb.CityID = @CityId
			and bb.valid = 1
) b 
right join 
(
	SELECT ProjectId
	FROM @projecttable AS p WITH (NOLOCK)
	WHERE NOT EXISTS (
			SELECT *
			FROM @projecttable_sub AS ps WITH (NOLOCK)
			WHERE p.ProjectId = ps.ProjectId
				AND ps.CityID = @CityId
				AND ps.Fxt_CompanyId = @FxtCompanyId
			)
		AND p.Valid = 1
		AND p.CityID = @CityId
		AND p.FxtCompanyId  IN (SELECT value FROM fxtproject.dbo.SplitToTable((SELECT ShowCompanyId FROM fxtdatacenter.dbo.Privi_Company_ShowData WHERE CityId = @CityId AND FxtCompanyId = @FxtCompanyId and TypeCode = @TypeCode), ','))
		AND p.ProjectId = @projectid
	UNION	
	SELECT ProjectId
	FROM @projecttable_sub p WITH (NOLOCK)
	WHERE p.Valid = 1
		AND p.CityID = @CityId
		AND p.Fxt_CompanyId = @FxtCompanyId
		AND p.ProjectId = @projectid
) p on b.ProjectId = p.ProjectId
where b.projectid = @projectid

