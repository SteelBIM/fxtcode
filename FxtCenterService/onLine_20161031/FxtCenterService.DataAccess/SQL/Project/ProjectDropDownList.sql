SELECT @top p.cityid,
              p.projectid,
              p.projectname,
              p.areaid,
              p.subareaid,
              p.address,
              p.isevalue,
              p.usableyear,
              a.areaname,
              0 AS avgprice,
              p.buildingnum AS buildingtotal,
              p.totalnum AS housetotal,
              p.x,
              p.y,
              photo.photocnt
FROM
  ( SELECT p.cityid,
           p.projectid,
           p.projectname,
           p.areaid,
           p.subareaid,
           p.address,
           p.isevalue,
           p.usableyear,
           p.buildingnum AS buildingtotal,
           p.totalnum AS housetotal,
           p.x,
           p.y,
           p.OtherName,
           p.PinYin,
           p.PinYinAll,
           p.buildingnum,
           p.totalnum
   FROM @table_project p with(nolock)
   WHERE 1=1
     AND p.cityid = @cityid
     AND p.projectid NOT IN
       (SELECT ps.projectid
        FROM @table_project_sub ps
        WHERE ps.projectid = p.projectid
          AND ps.fxt_companyid = @fxtcompanyid
          AND ps.cityid = p.cityid)
     AND VALID =1
     AND (','+cast(
                     (SELECT showcompanyid
                      FROM fxtdatacenter.dbo.privi_company_showdata WITH(nolock)
                      WHERE fxtcompanyid=@fxtcompanyid
                        AND cityid = @cityid
                        AND typecode = @typecode) AS varchar)+',' LIKE '%,' + cast(p.fxtcompanyid AS varchar) + ',%')
   UNION SELECT p.cityid,
                p.projectid,
                p.projectname,
                p.areaid,
                p.subareaid,
                p.address,
                p.isevalue,
                p.usableyear,
                p.buildingnum AS buildingtotal,
                p.totalnum AS housetotal,
                p.x,
                p.y,
                p.OtherName,
                p.PinYin,
                p.PinYinAll,
                p.buildingnum,
                p.totalnum
   FROM @table_project_sub p WITH(nolock)
   WHERE 1=1
     AND p.cityid = @cityid
     AND p.fxt_companyid = @fxtcompanyid
     AND VALID =1 ) p
LEFT JOIN fxtdatacenter.dbo.sys_area a with(nolock) ON a.areaid = p.areaid
LEFT JOIN
  (SELECT projectid,
          cityid,
          count(*) AS photocnt
   FROM
     (SELECT *
      FROM lnk_p_photo p WITH (nolock)
      WHERE 1 = 1
        AND NOT EXISTS
          (SELECT id
           FROM lnk_p_photo_sub ps WITH (nolock)
           WHERE ps.id = p.id
             AND ps.cityid = @cityid
             AND ps.fxtcompanyid = @fxtcompanyid)
        AND p.valid = 1
        AND p.cityid = @cityid
        AND p.fxtcompanyid IN
          (SELECT value
           FROM fxtproject.dbo.splittotable(
                                              (SELECT showcompanyid
                                               FROM fxtdatacenter.dbo.privi_company_showdata
                                               WHERE cityid = @cityid
                                                 AND fxtcompanyid = @fxtcompanyid
                                                 AND typecode = @typecode), ','))
        AND p.phototypecode LIKE '2009%'
      UNION SELECT *
      FROM lnk_p_photo_sub p WITH (nolock)
      WHERE 1 = 1
        AND p.valid = 1
        AND p.cityid = @cityid
        AND p.fxtcompanyid = @fxtcompanyid
        AND p.phototypecode LIKE '2009%') t
   GROUP BY projectid,
            cityid) photo ON p.projectid = photo.projectid
AND p.cityid = photo.cityid
WHERE ([PinYin] like @param or [OtherName] like @param or [ProjectName] like @param  or [PinYinAll] LIKE @param @addlike)
order by (case when [ProjectName] like @strKey then 0 else 1 end) asc