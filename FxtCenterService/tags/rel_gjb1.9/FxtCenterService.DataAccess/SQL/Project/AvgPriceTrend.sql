declare @tbl_proavglist table(AvgPriceDate varchar(50),AvgPrice decimal(18,2),avgtype int)--均价走势

--均价值大于6条(相同细分类型)
if (select count(1) from dbo.DAT_ProjectAvgPrice with(nolock) where purposetype=@purposetype and BuildingAreaType=@buildingareatype and BuildingTypeCode=@buildingtypecode and cityid=@cityid and areaid=@areaid and projectid=@projectid and daterange=@daterange)>=6 --and AvgPriceDate>=@startdate and AvgPriceDate<=@enddate
begin
	--楼盘的均价走势(相同细分类型)
	insert into @tbl_proavglist(AvgPriceDate,AvgPrice,avgtype)
	select top 6 AvgPriceDate,(sum(avgprice)/COUNT(id)) as avgprice,1 FROM dbo.DAT_ProjectAvgPrice with(nolock) where  purposetype=@purposetype and BuildingAreaType=@buildingareatype and BuildingTypeCode=@buildingtypecode and cityid=@cityid and areaid=@areaid and projectid=@projectid and daterange=@daterange--and AvgPriceDate>=@startdate and AvgPriceDate<=@enddate 
	and ','+cast((select ShowCompanyId from dbo.privi_company_showdata with(nolock) where FxtCompanyId=@fxtcompanyid and Cityid=@cityid) as varchar)+',' like '%,' + cast(fxtcompanyid as varchar) + ',%'
	group by avgpricedate
	order by AvgPriceDate 
	--楼盘所在行政区均价走势(相同细分类型)
	insert into @tbl_proavglist(AvgPriceDate,AvgPrice,avgtype)
	select top 6 AvgPriceDate,(sum(avgprice)/COUNT(id)) as avgprice,2 FROM dbo.DAT_ProjectAvgPrice with(nolock) where  purposetype=@purposetype and BuildingAreaType=@buildingareatype and BuildingTypeCode=@buildingtypecode and cityid=@cityid and areaid=@areaid and ISNULL(projectid,0)=0 and daterange=@daterange --and AvgPriceDate>=@startdate and AvgPriceDate<=@enddate 
	and ','+cast((select ShowCompanyId from dbo.privi_company_showdata with(nolock) where FxtCompanyId=@fxtcompanyid and Cityid=@cityid) as varchar)+',' like '%,' + cast(fxtcompanyid as varchar) + ',%'
	group by avgpricedate
	order by AvgPriceDate 
	--楼盘所在城市的均价走势(相同细分类型)
	insert into @tbl_proavglist(AvgPriceDate,AvgPrice,avgtype)
	select top 6 AvgPriceDate,(sum(avgprice)/COUNT(id)) as avgprice,3 FROM dbo.DAT_ProjectAvgPrice with(nolock) where  purposetype=@purposetype and BuildingAreaType=@buildingareatype and BuildingTypeCode=@buildingtypecode and cityid=@cityid and ISNULL(areaid,0)=0 and ISNULL(projectid,0)=0 and daterange=@daterange --and AvgPriceDate>=@startdate and AvgPriceDate<=@enddate 
	and ','+cast((select ShowCompanyId from dbo.privi_company_showdata with(nolock) where FxtCompanyId=@fxtcompanyid and Cityid=@cityid) as varchar)+',' like '%,' + cast(fxtcompanyid as varchar) + ',%'
	group by avgpricedate
	order by AvgPriceDate 
end
--存在均价值(不区分细分类型)
else if (select count(1) from dbo.DAT_ProjectAvgPrice with(nolock) where purposetype=@purposetype and cityid=@cityid and areaid=@areaid and projectid=@projectid and daterange=@daterange)>=6	--and AvgPriceDate>=@startdate and AvgPriceDate<=@enddate 
begin
	--楼盘的均价走势(不区分细分类型)
	insert into @tbl_proavglist(AvgPriceDate,AvgPrice,avgtype)
	select top 12 AvgPriceDate,(sum(avgprice)/COUNT(id)) as avgprice,1 FROM dbo.DAT_ProjectAvgPrice with(nolock) where purposetype=@purposetype and cityid=@cityid and areaid=@areaid and projectid=@projectid and daterange=@daterange --and AvgPriceDate>=@startdate and AvgPriceDate<=@enddate 
	and ','+cast((select ShowCompanyId from dbo.privi_company_showdata with(nolock) where FxtCompanyId=@fxtcompanyid and Cityid=@cityid) as varchar)+',' like '%,' + cast(fxtcompanyid as varchar) + ',%'
	group by avgpricedate
	order by AvgPriceDate 
	--楼盘所在行政区均价走势(不区分细分类型)
	insert into @tbl_proavglist(AvgPriceDate,AvgPrice,avgtype)
	select top 12 AvgPriceDate,(sum(avgprice)/COUNT(id)) as avgprice,2 FROM dbo.DAT_ProjectAvgPrice with(nolock) where purposetype=@purposetype and  cityid=@cityid and areaid=@areaid and ISNULL(projectid,0)=0 and daterange=@daterange  --and AvgPriceDate>=@startdate and AvgPriceDate<=@enddate 
	and ','+cast((select ShowCompanyId from dbo.privi_company_showdata with(nolock) where FxtCompanyId=@fxtcompanyid and Cityid=@cityid) as varchar)+',' like '%,' + cast(fxtcompanyid as varchar) + ',%'
	group by avgpricedate
	order by AvgPriceDate 
	--楼盘所在城市的均价走势(不区分细分类型)
	insert into @tbl_proavglist(AvgPriceDate,AvgPrice,avgtype)
	select top 12 AvgPriceDate,(sum(avgprice)/COUNT(id)) as avgprice,3 FROM dbo.DAT_ProjectAvgPrice with(nolock) where purposetype=@purposetype and  cityid=@cityid and ISNULL(areaid,0)=0 and ISNULL(projectid,0)=0 and daterange=@daterange --and AvgPriceDate>=@startdate and AvgPriceDate<=@enddate 
	and ','+cast((select ShowCompanyId from dbo.privi_company_showdata with(nolock) where FxtCompanyId=@fxtcompanyid and Cityid=@cityid) as varchar)+',' like '%,' + cast(fxtcompanyid as varchar) + ',%'
	group by avgpricedate
	order by AvgPriceDate 
end
--行政区均价(相同细分类型)
else if (select count(1) from dbo.DAT_ProjectAvgPrice with(nolock) where purposetype=@purposetype and  BuildingAreaType=@buildingareatype and BuildingTypeCode=@buildingtypecode and cityid=@cityid and areaid=@areaid and projectid=@projectid and daterange=@daterange)=0 --and AvgPriceDate>=@startdate and AvgPriceDate<=@enddate
begin
	--楼盘所在行政区均价走势(相同细分类型)
	insert into @tbl_proavglist(AvgPriceDate,AvgPrice,avgtype)
	select top 12 AvgPriceDate,(sum(avgprice)/COUNT(id)) as avgprice,2 FROM dbo.DAT_ProjectAvgPrice with(nolock) where purposetype=@purposetype and  BuildingAreaType=@buildingareatype and BuildingTypeCode=@buildingtypecode and cityid=@cityid and areaid=@areaid and ISNULL(projectid,0)=0 and daterange=@daterange --and AvgPriceDate>=@startdate and AvgPriceDate<=@enddate
	and ','+cast((select ShowCompanyId from dbo.privi_company_showdata with(nolock) where FxtCompanyId=@fxtcompanyid and Cityid=@cityid) as varchar)+',' like '%,' + cast(fxtcompanyid as varchar) + ',%'
	group by avgpricedate
	order by AvgPriceDate 
	--楼盘所在城市的均价走势(相同细分类型)
	insert into @tbl_proavglist(AvgPriceDate,AvgPrice,avgtype)
	select top 12 AvgPriceDate,(sum(avgprice)/COUNT(id)) as avgprice,3 FROM dbo.DAT_ProjectAvgPrice with(nolock) where purposetype=@purposetype and  BuildingAreaType=@buildingareatype and BuildingTypeCode=@buildingtypecode and cityid=@cityid and ISNULL(areaid,0)=0 and ISNULL(projectid,0)=0 and daterange=@daterange --and AvgPriceDate>=@startdate and AvgPriceDate<=@enddate
	and ','+cast((select ShowCompanyId from dbo.privi_company_showdata with(nolock) where FxtCompanyId=@fxtcompanyid and Cityid=@cityid) as varchar)+',' like '%,' + cast(fxtcompanyid as varchar) + ',%'
	group by avgpricedate
	order by AvgPriceDate 
end

select AvgPriceDate,AvgPrice,avgtype from @tbl_proavglist order by avgtype asc,avgpricedate asc
