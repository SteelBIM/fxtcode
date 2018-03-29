select a.HouseId,a.HouseName,a.SJPurposeCode,b.CodeName SJPurposeName,BuildingArea
from FxtData_Biz.dbo.Dat_House_Biz a WITH (nolock)
left join FxtDataCenter.dbo.SYS_Code b WITH (nolock)
on a.SJPurposeCode=b.Code
where FloorId=@floorId 
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
select a.HouseId,a.HouseName,a.SJPurposeCode,b.CodeName SJPurposeName,BuildingArea
from FxtData_Biz.dbo.Dat_House_Biz a WITH (nolock)
left join FxtDataCenter.dbo.SYS_Code b WITH (nolock)
on a.SJPurposeCode=b.Code
where FloorId=@floorId and fxtcompanyid = @fxtcompanyid and Valid=1 $keylimit