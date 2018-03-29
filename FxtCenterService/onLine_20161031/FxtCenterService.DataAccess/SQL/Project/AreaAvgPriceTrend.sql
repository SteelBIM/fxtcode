declare @tbl_proavglist table(AvgPriceDate datetime,AvgPrice decimal(18,2),avgtype int)--均价走势

--楼盘所在行政区均价走势
insert into @tbl_proavglist(AvgPriceDate,AvgPrice,avgtype)
select top @topcnt AvgPriceDate, AvgPrice, 2
from dbo.DAT_AvgPrice_Month
where cityid=@cityid and areaid=@areaid and ISNULL(projectid,0)=0
and AvgPriceDate>=@startdate and AvgPriceDate<=@enddate
order by AvgPriceDate desc
