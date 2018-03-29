use FXTProject

declare @cityid int
declare @fxtcompanyid int
declare @projectid int
declare @avgprice int
declare @typecode int
declare @key nvarchar(20)
declare @param nvarchar(20)

set @cityid=3
set @fxtcompanyid=25
set @projectid=31311
set @avgprice=0
set @typecode=1003036
set @key='%'
set @param='%%%'


select *
from
  ( select mb.buildingid ,
           mb.totalfloor as floortotal ,
           mb.buildingname ,
           mb.builddate ,
           convert(int, round(@avgprice * mb.weight, 0)) averageprice ,
           mb.isevalue ,
           mb.weight ,
           mb.totalbuildarea ,
           c.codename ,
           purposecode ,
           buildingtypecode ,
           0 as housetotal
   from ( --Â¥¶°

         select projectid ,
                buildingid ,
                totalfloor ,
                buildingname ,
                builddate ,
                averageprice ,
                isevalue ,
                weight ,
                totalbuildarea ,
                buildingtypecode ,
                purposecode
         from dbo.DAT_Building_hbh b with (nolock)
         where valid = 1
           and cityid = @cityid
           and projectid = @projectid
           and buildingid not in
             ( select buildingid
              from dbo.DAT_Building_hbh_sub ps with (nolock)
              where ps.projectid = b.projectid
                and ps.fxt_companyid = @fxtcompanyid
                and ps.cityid = b.cityid )
           and ( ',' + cast(
                              ( select showcompanyid
                               from fxtdatacenter.dbo.privi_company_showdata with (nolock)
                               where fxtcompanyid = @fxtcompanyid
                                 and cityid = @cityid
                                 and typecode = @typecode ) as varchar) + ',' like '%,' + cast(b.fxtcompanyid as varchar) + ',%' )
         union
         select projectid ,
                buildingid ,
                totalfloor ,
                buildingname ,
                builddate ,
                averageprice ,
                isevalue ,
                weight ,
                totalbuildarea ,
                buildingtypecode ,
                purposecode
         from dbo.DAT_Building_hbh_sub b with (nolock)
         where valid = 1
           and projectid = @projectid
           and b.fxt_companyid = @fxtcompanyid
           and b.cityid = @cityid ) mb
   left join fxtdatacenter.dbo.sys_code c with (nolock) on mb.buildingtypecode = c.code
   group by mb.buildingid ,
            mb.buildingname ,
            mb.builddate ,
            mb.averageprice ,
            mb.isevalue ,
            mb.weight ,
            mb.totalbuildarea ,
            c.codename ,
            mb.totalfloor ,
            mb.purposecode ,
            mb.buildingtypecode)t
where 1 = 1 and t.buildingname like @param
order by  (case when buildingname like @key then 0 else 1 end) asc,buildingid desc