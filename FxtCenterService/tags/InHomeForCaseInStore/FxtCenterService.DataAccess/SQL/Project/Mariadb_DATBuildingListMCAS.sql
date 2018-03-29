SELECT *
FROM
  ( SELECT mb.buildingid ,
           mb.totalfloor AS floortotal ,
           mb.buildingname ,
           mb.builddate ,
           convert(round(?avgprice * mb.weight, 0),int) averageprice ,
           mb.IsEvaluate as isevalue ,
           mb.weight ,
           mb.totalbuildarea ,
           c.codename ,
           purposecode ,
           buildingtypecode ,
           0 AS housetotal
   FROM ( 
         SELECT projectid ,
                buildingid ,
                totalfloor ,
                buildingname ,
                builddate ,
                averageprice ,
                IsEvaluate ,
                weight ,
                totalbuildarea ,
                buildingtypecode ,
                purposecode
         FROM base_building b 
         WHERE VALID = 1
           AND cityid = ?cityid
           AND projectid = ?projectid
           AND NOT EXISTS
             ( SELECT buildingid
              FROM base_building_sub ps 
              WHERE ps.projectid = b.projectid
                AND ps.FxtCompanyId = ?fxtcompanyid
                AND ps.cityid = b.cityid)
           AND (CONCAT(',',(SELECT showcompanyid
                               FROM privi_company_show_data
                               WHERE fxtcompanyid = ?fxtcompanyid
                                 AND cityid = ?cityid
                                 AND typecode = ?typecode),',') LIKE CONCAT('%,',b.fxtcompanyid,',%'))
         UNION
         SELECT projectid ,
                buildingid ,
                totalfloor ,
                buildingname ,
                builddate ,
                averageprice ,
                IsEvaluate ,
                weight ,
                totalbuildarea ,
                buildingtypecode ,
                purposecode
         FROM base_building_sub b 
         WHERE VALID = 1
           AND projectid = ?projectid
           AND b.FxtCompanyId = ?fxtcompanyid
           AND b.cityid = ?cityid ) mb
   LEFT JOIN sys_code c  ON mb.buildingtypecode = c.code
   GROUP BY mb.buildingid ,
            mb.buildingname ,
            mb.builddate ,
            mb.averageprice ,
            mb.IsEvaluate ,
            mb.weight ,
            mb.totalbuildarea ,
            c.codename ,
            mb.totalfloor ,
            mb.purposecode ,
            mb.buildingtypecode)t
WHERE 1 = 1 and t.BuildingName like ?param
order by  (case when BuildingName like ?key then 0 else 1 end) asc,buildingid desc