SELECT sa.SubAreaId,sa.SubAreaName,sa.AreaId,sa.X,sa.Y,sa.FxtCompanyId
  FROM [FxtDataCenter].[dbo].[SYS_SubArea_Biz] sa with(nolock)
  left join FxtDataCenter.dbo.SYS_Area a with(nolock) on sa.AreaId = a.AreaId
  where 1 = 1
