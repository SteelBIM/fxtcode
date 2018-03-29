select mb.buildingid,mb.totalfloor AS FloorTotal,sum(houseTotal) HouseTotal,mb.BuildingName,mb.BuildDate,convert(int,round(@avgprice*mb.Weight,0)) AveragePrice,mb.isevalue,mb.Weight,mb.totalbuildarea,c.codename from (--楼栋
select projectid,BuildingId,totalfloor, BuildingName,BuildDate,AveragePrice,isevalue,Weight,totalbuildarea,buildingtypecode  from @table_building b with(nolock) where cityid=@cityid and projectid=@projectid and   
 (','+cast((select showcompanyid from dbo.privi_company_showdata with(nolock) where Fxtcompanyid=@fxtcompanyid and Cityid = @cityid) as varchar)+',' like '%,' + cast(b.fxtcompanyid as varchar) + ',%')
 ) mb left join
(select h.buildingid,h.floorno,count(h.houseid) houseTotal from (--楼栋
select projectid,BuildingId, BuildingName,BuildDate,AveragePrice,isevalue,Weight  from @table_building b with(nolock) where cityid=@cityid and projectid=@projectid and  
 ( ','+cast((select showcompanyid from dbo.privi_company_showdata with(nolock) where Fxtcompanyid=@fxtcompanyid and Cityid = @cityid) as varchar)+',' like '%,' + cast(b.fxtcompanyid as varchar) + ',%')
 ) b left join 

(--房号
select houseid,buildingid,floorno from @table_House with(nolock)
where cityid = @cityid and  
 (','+cast((select showcompanyid from dbo.privi_company_showdata pc with(nolock) where pc.Fxtcompanyid=@fxtcompanyid and pc.Cityid = @cityid) as varchar)+',' like '%,' + cast(fxtcompanyid as varchar) + ',%')
 ) h 
on b.buildingid = h.buildingid
group by h.FloorNo,h.buildingid) hs
on mb.buildingid = hs.buildingid
left join sys_code c with(nolock)
on mb.buildingtypecode = c.code
group by mb.buildingid,mb.BuildingName,mb.BuildDate,mb.AveragePrice,mb.isevalue,mb.Weight,mb.totalbuildarea,c.codename,mb.totalfloor