select FloorId,FloorNo
from FxtData_Biz.dbo.Dat_Floor_Biz WITH (nolock)
where BuildingId=@buildingid 
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
select FloorId,FloorNo
from FxtData_Biz.dbo.Dat_Floor_Biz_sub WITH (nolock)
where BuildingId=@buildingid and fxtcompanyid = @fxtcompanyid  and Valid=1 $keylimit