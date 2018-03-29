SELECT *
FROM
  ( SELECT mb.buildingid ,
           mb.totalfloor AS floortotal ,
           mb.buildingname ,
           mb.builddate ,
           convert(int, round(@avgprice * mb.weight, 0)) averageprice ,
           mb.isevalue ,
           mb.weight ,
           mb.totalbuildarea ,
           c.codename ,
           purposecode ,
           buildingtypecode ,
           0 AS housetotal
   FROM ( --楼栋

         SELECT projectid ,
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
         FROM @table_building b WITH (nolock)
         WHERE VALID = 1
           AND cityid = @cityid
           AND projectid = @projectid
           AND buildingid NOT IN
             ( SELECT buildingid
              FROM @table_building_sub ps WITH (nolock)
              WHERE ps.projectid = b.projectid
                AND ps.fxt_companyid = @fxtcompanyid
                AND ps.cityid = b.cityid )
           AND ( ',' + cast(
                              ( SELECT showcompanyid
                               FROM fxtdatacenter.dbo.privi_company_showdata WITH (nolock)
                               WHERE fxtcompanyid = @fxtcompanyid
                                 AND cityid = @cityid
                                 AND typecode = @typecode ) AS varchar) + ',' LIKE '%,' + cast(b.fxtcompanyid AS varchar) + ',%' )
         UNION
         SELECT projectid ,
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
         FROM @table_building_sub b WITH (nolock)
         WHERE VALID = 1
           AND projectid = @projectid
           AND b.fxt_companyid = @fxtcompanyid
           AND b.cityid = @cityid ) mb
   LEFT JOIN fxtdatacenter.dbo.sys_code c WITH (nolock) ON mb.buildingtypecode = c.code
   GROUP BY mb.buildingid ,
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
WHERE 1 = 1 and t.BuildingName like @param
order by  (case when BuildingName like @key then 0 else 1 end) asc,buildingid desc