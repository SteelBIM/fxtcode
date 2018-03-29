using System;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using Dapper;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class DAT_Land_BasePriceDAL : IDAT_Land_BasePrice
    {
        #region 添加
        public int AddLand_BasePrice(DAT_Land_BasePrice modal)
        {
            string sql = @"insert into FxtLand.dbo.DAT_Land_BasePrice with(rowlock) (FxtCompanyId,cityid,purposecode,landclass,landunitprice_avg,landunitprice_min,landunitprice_max,buildingunitprice_avg,buildingunitprice_min,buildingunitprice_max,pricedate,AreaId,SubAreaId,Remarks,DocumentNo) 
values (@fxtcompanyid,@cityid,@purposecode,@landclass,@landunitprice_avg,@landunitprice_min,@landunitprice_max,@buildingunitprice_avg,@buildingunitprice_min,@buildingunitprice_max,@pricedate,@AreaId,@SubAreaId,@Remarks,@DocumentNo)";
            try
            {
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtLand))
                {
                    int result = con.Execute(sql, modal);
                    return result;
                }
            }
            catch
            {
                return 0;
            }
        }

        #endregion

        #region 修改
        public int UpdateLand_BasePrice(DAT_Land_BasePrice modal)
        {
            string sql = @"update FxtLand.dbo.DAT_Land_BasePrice with(rowlock) set 
                                cityid = @cityid,purposecode = @purposecode,landclass = @landclass,
                                landunitprice_avg = @landunitprice_avg,landunitprice_min = @landunitprice_min,landunitprice_max = @landunitprice_max,
                                buildingunitprice_avg = @buildingunitprice_avg,buildingunitprice_min = @buildingunitprice_min,
                                buildingunitprice_max = @buildingunitprice_max,pricedate = @pricedate,
                                AreaId=@AreaId,SubAreaId=@SubAreaId,Remarks=@Remarks,DocumentNo=@DocumentNo where id = @id";
            try
            {
                DBHelperSql.ConnectionString = ConfigurationHelper.FxtLand;
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtLand))
                {
                    int result = con.Execute(sql, modal);
                    return result;
                }
            }
            catch
            {

                return 0;
            }
        }
        #endregion
        #region 删除
        public bool DeleteLand_BasePrice(int id)
        {
            string sql = "update FxtLand.dbo.DAT_Land_BasePrice with(rowlock) set valid =0 where id = @Id ";
            try
            {
                DBHelperSql.ConnectionString = ConfigurationHelper.FxtLand;
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtLand))
                {
                    int result = con.Execute(sql, new { Id = id });
                    if (result > 0)
                        return true;
                    else
                        return false;
                }

            }
            catch
            {

                return false;
            }
        }

        #endregion
        #region 查询
        public IQueryable<DAT_Land_BasePrice> GetLand_BasePrice(DAT_Land_BasePrice mode, int pageIndex, int pageSize, out int totalCount, bool self = true)
        {
            string sql = @"
SELECT land.Id
	,land.FxtCompanyId
	,land.CityID AS cityId
	,PurposeCode
	,LandClass
	,LandUnitPrice_avg
	,LandUnitPrice_min
	,LandUnitPrice_max
	,BuildingUnitPrice_avg
	,BuildingUnitPrice_min
	,BuildingUnitPrice_max
	,PriceDate
	,land.Valid AS valid
	,com.ChineseName
	,city.CityName
	,code.CodeName AS LandClassName
	,landcode.CodeName AS PurposeCodeName
	,land.AreaId
	,land.SubAreaId
	,Remarks
	,DocumentNo
	,are.AreaName
	,sarea.SubAreaName
FROM FxtLand.dbo.DAT_Land_BasePrice land WITH (NOLOCK)
LEFT JOIN FxtDataCenter.dbo.DAT_Company com WITH (NOLOCK) ON land.FxtCompanyId = com.CompanyId
LEFT JOIN FxtDataCenter.dbo.SYS_City city WITH (NOLOCK) ON land.CityID = city.CityId
LEFT JOIN FxtDataCenter.dbo.SYS_Code code WITH (NOLOCK) ON land.LandClass = code.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Code landcode WITH (NOLOCK) ON land.PurposeCode = landcode.Code
LEFT JOIN FxtDataCenter.dbo.SYS_Area are WITH (NOLOCK) ON are.AreaId = land.AreaId
LEFT JOIN FxtDataCenter.dbo.SYS_SubArea sarea WITH (NOLOCK) ON sarea.SubAreaId = land.SubAreaId
WHERE land.Valid = 1
	AND land.cityid = @cityid ";

            if (mode.purposecode > 0)
            {
                sql += " and (land.PurposeCode=@PurposeCode)";

            }
            //查看自己
            if (self)
            {
                sql += " and land.fxtCompanyid=@fxtCompanyid ";
            }
            else  //查看全部
            {
                sql += " and (land.fxtCompanyid=@fxtCompanyid or land.fxtCompanyid in(select [value] from fxtproject.dbo.[SplitToTable]((select LandCompanyId from FxtDataCenter.dbo.Privi_Company_ShowData with(nolock) where Cityid=@cityid and fxtCompanyid=@fxtCompanyid and typecode= 1003002),',')))";
            }

            if (mode.landclass > 0)
            {
                sql += " and (land.LandClass=@LandClass)";

            }
            if (mode.areaid > 0)
            {
                sql += " and (land.AreaId=@AreaId)";

            }
            if (mode.pricedate > DateTime.MinValue)
            {
                sql += "  and (land.PriceDate>@pricedate)";

            }
            if (mode.priceenddate > DateTime.MinValue)
            {
                sql += "  and (land.PriceDate<=@priceenddate)";

            }
            string pageSql = "";
            //if (pageIndex == 1)
            //{
            //    pageSql = "select top " + pageSize + "  * from (" + sql + ") as t1 order by t1.PriceDate desc";
            //}
            //else
            //{
            //    pageSql = "select top " + pageSize + "  * from (" + sql + ") as t1 " +
            //    " where (t1.Id>(select max(t3.Id)" +
            //    " from (select top " + (pageIndex - 1) * pageSize + " t2.Id from (" + sql + ") as t2 order by t2.Id) as" +
            //    " t3)) order by t1.PriceDate desc";
            //}
            pageSql = "select top " + pageSize + " tt.*";
            pageSql += " from (";
            pageSql += " select ROW_NUMBER() over(order by Id desc) rownumber,t.*";
            pageSql += " from (" + sql + ") t";
            pageSql += " ) tt";
            pageSql += " where tt.rownumber>" + (pageIndex - 1) * pageSize + "";
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtLand))
            {
                //总记录数
                string countSql = "select count(1) from (" + sql + ") as t1";
                #region 总记录数
                totalCount = con.Query<int>(countSql, new
                {
                    cityid = mode.cityid,
                    fxtCompanyid = mode.fxtcompanyid,
                    PurposeCode = mode.purposecode,
                    LandClass = mode.landclass,
                    AreaId = mode.areaid,
                    pricedate = Convert.ToDateTime(mode.pricedate).ToString("yyyy-MM-dd") + " 00:00:00",
                    priceenddate = Convert.ToDateTime(mode.priceenddate).ToString("yyyy-MM-dd") + " 23:59:59"
                }).FirstOrDefault();
                #endregion

                //楼盘列表
                #region 土地基准地价列表
                var landList = con.Query<DAT_Land_BasePrice>(pageSql, new
                {
                    cityid = mode.cityid,
                    fxtCompanyid = mode.fxtcompanyid,
                    PurposeCode = mode.purposecode,
                    LandClass = mode.landclass,
                    AreaId = mode.areaid,
                    pricedate = Convert.ToDateTime(mode.pricedate).ToString("yyyy-MM-dd") + " 00:00:00",
                    priceenddate = Convert.ToDateTime(mode.priceenddate).ToString("yyyy-MM-dd") + " 23:59:59"
                }).AsQueryable();
                #endregion
                return landList;
            }
        }


        public IQueryable<DAT_Land_BasePrice> GetLand_BasePriceExeclImport(DAT_Land_BasePrice mode, bool self = true)
        {
            DBHelperSql.ConnectionString = ConfigurationHelper.FxtLand;
            string sql = @"select land.Id, land.FxtCompanyId, land.CityID as cityId, 
                            PurposeCode, LandClass, LandUnitPrice_avg, 
                            LandUnitPrice_min, LandUnitPrice_max, 
                            BuildingUnitPrice_avg, BuildingUnitPrice_min,
                            BuildingUnitPrice_max, PriceDate, land.Valid as valid,com.ChineseName,city.CityName,code.CodeName as LandClassName,
                            landcode.CodeName as PurposeCodeName,land.AreaId,land.SubAreaId,Remarks,DocumentNo,are.AreaName,sarea.SubAreaName 
                            from FxtLand.dbo.DAT_Land_BasePrice land with(nolock)
                            left join FxtDataCenter.dbo.DAT_Company com with(nolock) on land.FxtCompanyId=com.CompanyId
                            left join FxtDataCenter.dbo.SYS_City city with(nolock) on land.CityID=city.CityId
                            left join FxtDataCenter.dbo.SYS_Code code with(nolock) on land.LandClass=code.Code
                            left join FxtDataCenter.dbo.SYS_Code landcode with(nolock) on land.PurposeCode=landcode.Code
                            left join FxtDataCenter.dbo.SYS_Area are with(nolock) on are.AreaId=land.AreaId
                            left join FxtDataCenter.dbo.SYS_SubArea sarea with(nolock) on sarea.SubAreaId=land.SubAreaId
                            where land.Valid=1 and land.cityid=@cityid ";

            if (mode.purposecode > 0)
            {
                sql += " and (land.PurposeCode=@PurposeCode)";

            }
            //查看自己
            if (self)
            {
                sql += " and land.fxtCompanyid=@fxtCompanyid ";
            }
            else  //查看全部
            {
                sql += " and (land.fxtCompanyid=@fxtCompanyid or land.fxtCompanyid in(select [value] from fxtproject.dbo.[SplitToTable]((select LandCompanyId from FxtDataCenter.dbo.Privi_Company_ShowData with(nolock) where Cityid=@cityid and fxtCompanyid=@fxtCompanyid and typecode= 1003002),',')))";
            }

            if (mode.landclass > 0)
            {
                sql += " and (land.LandClass=@LandClass)";

            }
            if (mode.areaid > 0)
            {
                sql += " and (land.AreaId=@AreaId)";

            }
            if (mode.pricedate > DateTime.MinValue)
            {
                sql += "  and (land.PriceDate>@startPriceDate)";

            }
            if (mode.priceenddate > DateTime.MinValue)
            {
                sql += "  and (land.PriceDate<=@endPriceDate)";

            }
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtLand))
            {
                var landList = con.Query<DAT_Land_BasePrice>(sql, new
                {
                    cityid = mode.cityid,
                    fxtCompanyid = mode.fxtcompanyid,
                    PurposeCode = mode.purposecode,
                    LandClass = mode.landclass,
                    AreaId = mode.areaid,
                    startPriceDate = mode.pricedate + " 00:00:00",
                    endPriceDate = mode.priceenddate + " 23:59:59"
                }).AsQueryable();
                return landList;
            }
        }
        /// <summary>
        /// 根据Id得到土地基准地价
        /// </summary>
        /// <param name="Id">ID</param>
        /// <param name="cityId">cityId</param>
        /// <param name="fxtCompanyid">fxtCompanyid</param>
        /// <returns></returns>
        public DAT_Land_BasePrice GetAllLand_BasePriceByLandId(int landId, int cityId, int fxtCompanyid)
        {
            DAT_Land_BasePrice land = new DAT_Land_BasePrice();
            DBHelperSql.ConnectionString = ConfigurationHelper.FxtLand;
            string sql = @"select land.Id, land.FxtCompanyId, land.CityID as cityId, 
                            PurposeCode, LandClass, LandUnitPrice_avg, 
                            LandUnitPrice_min, LandUnitPrice_max, 
                            BuildingUnitPrice_avg, BuildingUnitPrice_min,
                            BuildingUnitPrice_max, PriceDate, land.Valid as valid,com.ChineseName,city.CityName,code.CodeName as LandClassName,
                            landcode.CodeName as PurposeCodeName,land.AreaId,land.SubAreaId,Remarks,DocumentNo,are.AreaName,sarea.SubAreaName 
                            from FxtLand.dbo.DAT_Land_BasePrice land with(nolock)
                            left join FxtDataCenter.dbo.DAT_Company com with(nolock) on land.FxtCompanyId=com.CompanyId
                            left join FxtDataCenter.dbo.SYS_City city with(nolock) on land.CityID=city.CityId
                            left join FxtDataCenter.dbo.SYS_Code code with(nolock) on land.LandClass=code.Code
                            left join FxtDataCenter.dbo.SYS_Code landcode with(nolock) on land.PurposeCode=landcode.Code
                            left join FxtDataCenter.dbo.SYS_Area are with(nolock) on are.AreaId=land.AreaId
                            left join FxtDataCenter.dbo.SYS_SubArea sarea with(nolock) on sarea.SubAreaId=land.SubAreaId
                            where land.cityid=@Cityid and land.Valid=1 and land.Id=@landId and land.fxtCompanyid=@fxtCompanyid ";
            DBHelperSql.ConnectionString = ConfigurationHelper.FxtLand;
            SqlParameter[] parameter = {
                                           new SqlParameter("@Cityid", SqlDbType.Int),
                                           new SqlParameter("@landId", SqlDbType.Int),
                                           new SqlParameter("@fxtCompanyid", SqlDbType.Int),
                                       };
            parameter[0].Value = cityId;
            parameter[1].Value = landId;
            parameter[2].Value = fxtCompanyid;

            land = SqlModelHelper<DAT_Land_BasePrice>.GetSingleObjectBySql(sql, parameter);
            return land;
        }
        #endregion
    }
}
