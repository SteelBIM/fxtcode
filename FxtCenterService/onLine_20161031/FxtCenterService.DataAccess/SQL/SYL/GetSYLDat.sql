SELECT DataDate,c.CityId,c.CityName,AreaName,SubAreaName,HouseHisAvg,HouseHisMax,HouseHisMin,HouseCur,HouseFurAvg,HouseFurMax,HouseFurMin,BizHisAvg,BizHisMax,BizHisMin,BizCur,BizFurAvg,BizFurMax,BizFurMin,OfficeHisAvg,OfficeHisMax,OfficeHisMin,OfficeCur,OfficeFurAvg,OfficeFurMax,OfficeFurMin
  FROM FxtDataCenter.dbo.SYL_Dat_tzsyl s with(nolock)
  left join FxtDataCenter.dbo.SYS_City c with(nolock) on s.CityId = c.CityId
  where s.CityId = @cityid
  and DataDate = (select MAX(DataDate) FROM FxtDataCenter.dbo.SYL_Dat_tzsyl where CityId = @cityid)