select @top BuildingId,BuildingName,Address,BizFloor
from
(
	select @top BuildingId,BuildingName,Address,BizFloor
	from FxtData_Biz.dbo.Dat_Building_Biz WITH (nolock)
	where ProjectId=@projectid 
		AND fxtcompanyid IN
        (
			SELECT value
			FROM fxtproject.dbo.splittotable(
			(
			SELECT showcompanyid
			FROM fxtdatacenter.dbo.privi_company_showdata
			WHERE cityid = @cityid AND fxtcompanyid = @fxtcompanyid AND typecode = @typecode), ',')
		) and Valid=1 $keylimit
	union
	select @top BuildingId,BuildingName,Address,BizFloor
	from FxtData_Biz.dbo.Dat_Building_Biz_sub WITH (nolock)
	where ProjectId=@projectid and fxtcompanyid = @fxtcompanyid and Valid=1 $keylimit
)a