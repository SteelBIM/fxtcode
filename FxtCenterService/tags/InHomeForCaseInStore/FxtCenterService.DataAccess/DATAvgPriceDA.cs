using CAS.Entity.FxtProject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace FxtCenterService.DataAccess
{
    public class DATAvgPriceDA : Base
    {
        /// <summary>
        /// 获取城市均价列表
        /// </summary>
        public static List<DatAvgPricePush> GetAvgPriceList(DateTime? avgpricedate)
        {
            string strSql = @"select CONVERT(nvarchar(7),AvgPriceDate,21) as statisdate,p.ProvinceId,p.ProvinceName,
m.CityId as citycode,c.CityName,m.AreaId as areacode,a.AreaName,AvgPrice as cityavgprice
from FXTProject.dbo.DAT_AvgPrice_Month m
left join FxtDataCenter.dbo.SYS_Area a on m.AreaId = a.AreaId
left join FxtDataCenter.dbo.SYS_City c on m.CityId = c.CityId
left join FxtDataCenter.dbo.SYS_Province p on c.ProvinceId = p.ProvinceId
where SubAreaId = 0 and ProjectId = 0 and BuildingAreaType = 0 ";

            List<SqlParameter> paramsList = new List<SqlParameter>();

            if (avgpricedate.HasValue)
            {
                strSql += "and AvgPriceDate >= @AvgPriceDate";

                paramsList.Add(new SqlParameter("@AvgPriceDate", avgpricedate));
            }

            List<DatAvgPricePush> entity = ExecuteToEntityList<DatAvgPricePush>(strSql, CommandType.Text, paramsList);
            
            return entity;
        }

        /// <summary>
        /// 获取最后一个月城市均价列表
        /// </summary>
        /// <returns></returns>
        public static List<DatAvgPricePush> GetAvgPriceLastMonthList()
        {
            string strSql = @"select CONVERT(nvarchar(7),ap.statisdate,21) as statisdate,ap.ProvinceId,ap.ProvinceName,ap.Provincecode,
ap.cityid,ap.CityName,ap.citycode,ap.areaid,ap.AreaName,ap.areacode,
	(
		select AvgPrice from FXTProject.dbo.DAT_AvgPrice_Month
		where id =
		(
			select MAX(id) from FXTProject.dbo.DAT_AvgPrice_Month apm with(nolock)
			where ap.statisdate = apm.AvgPriceDate and ap.cityid = apm.CityId and ap.areaid = apm.AreaId and apm.ProjectId = 0 and apm.SubAreaId = 0 and apm.BuildingAreaType = 0 
		)
	) cityavgprice
from 
(
	select max(AvgPriceDate) as statisdate,p.ProvinceId,p.ProvinceName,p.zipcode as Provincecode,
	m.CityId,c.CityName,c.zipcode as citycode,m.AreaId,a.AreaName,a.zipcode as areacode
	from FXTProject.dbo.DAT_AvgPrice_Month m with(nolock)
	left join FxtDataCenter.dbo.SYS_Area a with(nolock) on m.AreaId = a.AreaId
	left join FxtDataCenter.dbo.SYS_City c with(nolock) on m.CityId = c.CityId
	left join FxtDataCenter.dbo.SYS_Province p with(nolock) on c.ProvinceId = p.ProvinceId
	where SubAreaId = 0 and ProjectId = 0 and m.AreaId <> 0 and BuildingAreaType = 0 and a.AreaName is not null and c.CityName is not null
	group by p.ProvinceId,p.ProvinceName,p.zipcode,m.CityId,c.CityName,c.zipcode,m.AreaId,a.AreaName,a.zipcode
) ap 
order by cityid,areaid,statisdate";

            List<SqlParameter> paramsList = new List<SqlParameter>();

            List<DatAvgPricePush> entity = ExecuteToEntityList<DatAvgPricePush>(strSql, CommandType.Text, paramsList);

            return entity;
        }
    }
}
