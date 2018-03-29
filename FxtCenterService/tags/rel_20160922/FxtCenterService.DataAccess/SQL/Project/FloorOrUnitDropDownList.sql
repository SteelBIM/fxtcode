select h.@filed,count(h.houseid) as houseTotal from 
(select buildingid,houseid,@filed from @table_house with(nolock) where valid=1 and  
 (','+cast((select showcompanyid from FxtDataCenter.dbo.Privi_Company_ShowData pc with(nolock) where pc.Fxtcompanyid=@fxtcompanyid and pc.Cityid = @cityid and pc.TypeCode = @typecode) as varchar)+',' like '%,' + cast(fxtcompanyid as varchar) + ',%')
 and buildingid =@buildingid ) h
--and houseid not in (select houseid from @table_house_sub where buildingid =@buildingid and fxtcompanyid = @fxtcompanyid and cityid = @cityid)
--union
--select buildingid,houseid,@filed from @table_house_sub with(nolock) where buildingid =@buildingid and fxtcompanyid = @fxtcompanyid and cityid = @cityid and valid = 1) h
group by h.@filed