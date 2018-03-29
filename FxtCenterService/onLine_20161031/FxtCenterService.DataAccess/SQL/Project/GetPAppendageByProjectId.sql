select 
	a.*
	,c.CodeName as AppendageCodeName
	,c1.CodeName as ClassCodeName
	,case when a.IsInner = 1 then '是' else '否' end as IsInnerName
from (
	SELECT *
	FROM fxtproject.dbo.LNK_P_Appendage WITH (NOLOCK)
	WHERE ProjectId = @projectid
		AND CityId = @cityid
)a
left join FxtDataCenter.dbo.SYS_Code c with(nolock) on a.AppendageCode = c.Code
left join FxtDataCenter.dbo.SYS_Code c1 with(nolock) on a.ClassCode = c1.Code