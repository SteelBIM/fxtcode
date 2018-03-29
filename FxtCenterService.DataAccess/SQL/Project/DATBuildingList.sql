select mb.buildingid,mb.totalfloor AS FloorTotal,sum(houseTotal) HouseTotal,mb.BuildingName,mb.BuildDate,convert(int,round(@avgprice*mb.Weight,0)) AveragePrice,mb.isevalue,mb.Weight,mb.totalbuildarea,c.codename,purposecode,buildingtypecode from (--楼栋
select projectid,BuildingId,totalfloor, BuildingName,BuildDate,AveragePrice,isevalue,Weight,totalbuildarea,buildingtypecode,purposecode  from @table_building b with(nolock) where valid=1 and cityid=@cityid and projectid=@projectid 
and BuildingId not in(select BuildingId from @table_building_sub ps with(nolock) where ps.ProjectId=b.ProjectId and ps.Fxt_CompanyId=@fxtcompanyid and ps.CityId=b.CityId)
and  (','+cast((select showcompanyid from FxtDataCenter.dbo.Privi_Company_ShowData with(nolock) where Fxtcompanyid=@fxtcompanyid and Cityid = @cityid and TypeCode = @typecode) as varchar)+',' like '%,' + cast(b.fxtcompanyid as varchar) + ',%')
union 
select projectid,BuildingId,totalfloor, BuildingName,BuildDate,AveragePrice,isevalue,Weight,totalbuildarea,buildingtypecode,purposecode  from @table_building_sub b with(nolock) where valid=1 and projectid=@projectid 
and b.Fxt_CompanyId=@fxtcompanyid  
 ) mb left join
(select h.buildingid,h.floorno,count(h.houseid) houseTotal from (--楼栋
select projectid,BuildingId, BuildingName,BuildDate,AveragePrice,isevalue,Weight,purposecode  from @table_building b with(nolock) where valid=1 and cityid=@cityid and projectid=@projectid 
and BuildingId not in(select BuildingId from @table_building_sub ps with(nolock) where ps.ProjectId=b.ProjectId and ps.Fxt_CompanyId=@fxtcompanyid and ps.CityId=b.CityId)
and  ( ','+cast((select showcompanyid from FxtDataCenter.dbo.Privi_Company_ShowData with(nolock) where Fxtcompanyid=@fxtcompanyid and Cityid = @cityid and TypeCode = @typecode) as varchar)+',' like '%,' + cast(b.fxtcompanyid as varchar) + ',%')
union
 select projectid,BuildingId, BuildingName,BuildDate,AveragePrice,isevalue,Weight,purposecode from @table_building_sub b with(nolock) where valid=1 and projectid=@projectid 
and b.Fxt_CompanyId=@fxtcompanyid  
 ) b left join 

(--房号
select houseid,buildingid,floorno from @table_House ph with(nolock)
where valid=1 and cityid = @cityid and
ph.HouseId not in (select HouseId from @table_house_sub phs with(nolock) where ph.HouseId=phs.HouseId) 
and (','+cast((select showcompanyid from FxtDataCenter.dbo.Privi_Company_ShowData pc with(nolock) where pc.Fxtcompanyid=@fxtcompanyid and pc.Cityid = @cityid and TypeCode = @typecode) as varchar)+',' like '%,' + cast(fxtcompanyid as varchar) + ',%')
union
select houseid,buildingid,floorno from @table_house_sub ph with(nolock) where valid=1  and CityId=@CityId and ph.FxtCompanyId=@FxtCompanyId
 ) h 
on b.buildingid = h.buildingid
group by h.FloorNo,h.buildingid) hs
on mb.buildingid = hs.buildingid
left join FxtDataCenter.dbo.SYS_Code c with(nolock)
on mb.buildingtypecode = c.code
group by mb.buildingid,mb.BuildingName,mb.BuildDate,mb.AveragePrice,mb.isevalue,mb.Weight,mb.totalbuildarea,c.codename,mb.totalfloor,mb.purposecode,mb.buildingtypecode