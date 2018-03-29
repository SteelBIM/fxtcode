using System;
using System.Collections.Generic;
using System.Linq;
using FXT.DataCenter.Domain.Models.DTO;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using System.Data.SqlClient;
using System.Data;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;

using Dapper;
using System.Data.SqlTypes;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class LandCase : ILandCase
    {
        #region 案例查询
        public IQueryable<DAT_CaseLand> GetLandCases(DAT_CaseLand caseLand,int pageIndex,int pageSize,out int totalCount, bool self = true)
        {

            #region 按条件筛选拼接
            var sqlSelect = @"select dc.*,ss.SubAreaName from fxtland.dbo.DAT_CaseLand dc with(nolock) 
                               left join 
                               FxtDataCenter.dbo.SYS_SubArea ss with(nolock)
                               on dc.subareaid = ss.subareaid
                               where dc.valid =1 and  dc.cityid=@cityid ";

            var sqlWhere = string.Empty;

            //查询条件拼接
            if (caseLand.areaid != null && caseLand.areaid != -1) sqlWhere += " and dc.areaid = @areaid";
            if (caseLand.subareaid != null && caseLand.subareaid != -1) sqlWhere += " and dc.subareaid = @subareaid";
            if (!string.IsNullOrWhiteSpace(caseLand.landno)) sqlWhere += " and dc.landno =@landno";
            if (caseLand.landpurposecode != null && caseLand.landpurposecode != -1) sqlWhere += " and dc.landpurposecode = @landpurposecode";
            if (!string.IsNullOrWhiteSpace(caseLand.seller)) sqlWhere += " and dc.seller = @seller";
            if (!string.IsNullOrWhiteSpace(caseLand.winner)) sqlWhere += " and dc.winner = @winner";
            //if (caseLand.casedate != default(DateTime)) sqlWhere += " and dc.casedate = @casedate";
            if (caseLand.bargaintypecode != null && caseLand.bargaintypecode != -1) sqlWhere += " and dc.bargaintypecode = @bargaintypecode";
            if (caseLand.DateFrom.HasValue) sqlWhere += " and dc.startusabledate >= @dateFrom";
            if (caseLand.DateTo.HasValue) sqlWhere += " and dc.startusabledate-1 < @dateTo";
            if (caseLand.CaseDateFrom.HasValue) sqlWhere += " and dc.casedate >= @CaseDateFrom";
            if (caseLand.CaseDateTo.HasValue) sqlWhere += " and dc.casedate-1 < @CaseDateTo";
            if (caseLand.enddate.HasValue) sqlWhere += " and dc.enddate <= @enddate";

            //self为true，查看自己，否，查看全部
            sqlWhere += self ? " and dc.fxtCompanyid=@fxtCompanyid" : @" and
(dc.fxtCompanyid=@fxtCompanyid or dc.fxtCompanyid in(select [value] from FXTProject.dbo.[SplitToTable]((select LandCaseCompanyId from  FxtDataCenter.dbo.Privi_Company_ShowData where Cityid=@cityid and fxtCompanyid=@fxtCompanyid and typecode= 1003002),',')))";

            #endregion

            //拼接后的SQL语句
            var strSql = sqlSelect + sqlWhere;

            //分页SQL
            var pagenatedSql = @"select top "+pageSize+@" tt.*
                                from (
	                                select row_number() over (
			                                order by t.caseid desc
			                                ) rownumber
		                                ,t.*
	                                from ("+strSql+@") t ) tt
                                where tt.rownumber > ("+pageIndex+@" - 1) * "+pageSize;
          
            //总条数SQL
            var totalCountSql = "select count(1) from ("+strSql+") as t1";


            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                totalCount = conn.Query<int>(totalCountSql, caseLand).FirstOrDefault();
                return conn.Query<DAT_CaseLand>(pagenatedSql, caseLand).AsQueryable();
            }

        }

        public IQueryable<DAT_CaseLand> GetLandCaseByCaseId(int caseId, int fxtcompanyId, int cityId)
        {
            var strSql = @"select * from fxtland.dbo.DAT_CaseLand with(nolock) where valid = 1 and caseid = @caseid and
(fxtCompanyid=@fxtCompanyid or fxtCompanyid in(select [value] from FXTProject.dbo.[SplitToTable]((select LandCaseCompanyId from FxtDataCenter.dbo.Privi_Company_ShowData where Cityid=@cityid and fxtCompanyid=@fxtCompanyid and typecode= 1003002),',')))";
           

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<DAT_CaseLand>(strSql,new{caseId,fxtcompanyId,cityId}).AsQueryable();
            }
        }

        public IQueryable<LandCaseStatisticDTO> GetStatisticsData(string startDate, string endDate, string dataType, int areaid, int fxtCompanyId, int cityId)
        {
            return new List<LandCaseStatisticDTO>().AsQueryable();
        }

        #endregion

        #region 案例新增

        public int AddLandCase(DAT_CaseLand caseLand)
        {
            var strSql = @"INSERT INTO [FxtLand].[dbo].[DAT_CaseLand]                    (areaname,areaid,casedate,landno,landaddress,bargaintypecode,landsourcecode,usetypecode,bargainedby,bargaindate,minbargainprice,usableyear,startusabledate,winner,windate,landpurposedesc,landpurposecode,landarea,buildingarea,dealtotalprice,dealdate,cubagerate,mincubagerate,maxcubagerate,coverrate,maxcoverrate,bargainstatecode,developdegreecode,landunitprice,buildunitprice,heightlimited,planlimited,landusestatus,landclass,greenrage,mingreenrage,arrangestartdate,arrangeenddate,remark,creator,createdate,oldid,cityid,fxtcompanyid,savedatetime,saveuser,sourcename,sourcelink,sourcephone,subareaid,seller,enddate) 
values(@areaname,@areaid,@casedate,@landno,@landaddress,@bargaintypecode,@landsourcecode,@usetypecode,@bargainedby,@bargaindate,@minbargainprice,@usableyear,@startusabledate,@winner,@windate,@landpurposedesc,@landpurposecode,@landarea,@buildingarea,@dealtotalprice,@dealdate,@cubagerate,@mincubagerate,@maxcubagerate,@coverrate,@maxcoverrate,@bargainstatecode,@developdegreecode,@landunitprice,@buildunitprice,@heightlimited,@planlimited,@landusestatus,@landclass,@greenrage,@mingreenrage,@arrangestartdate,@arrangeenddate,@remark,@creator,@createdate,@oldid,@cityid,@fxtcompanyid,@savedatetime,@saveuser,@sourcename,@sourcelink,@sourcephone,@subareaid,@seller,@enddate)";

            using (IDbConnection conn = DapperAdapter.OpenConnection())
            {
                return conn.Execute(strSql, caseLand);
            }
        }

        #endregion

        #region 删除案例
        public int DeleteLandCase(int caseId)
        {
            var strSql = "update fxtland.dbo.DAT_CaseLand with(rowlock) set valid = 0 where caseid = @caseid";

            var parameter = new SqlParameter("@caseid", SqlDbType.Int) { Value = caseId };

            DBHelperSql.ConnectionString = ConfigurationHelper.FxtLand;
            return DBHelperSql.ExecuteNonQuery(strSql, parameter);
        }
        #endregion

        #region 修改案例
        public int UpdateLandCase(DAT_CaseLand caseLand)
        {
            caseLand.casedate = (caseLand.casedate == default(DateTime)) ? ((DateTime)SqlDateTime.MinValue) : caseLand.casedate;

            var strSql = @"update DAT_CaseLand set areaname = @areaname,areaid = @areaid,casedate = @casedate,landno = @landno,landaddress = @landaddress,bargaintypecode = @bargaintypecode,landsourcecode = @landsourcecode,usetypecode = @usetypecode,bargainedby = @bargainedby,bargaindate = @bargaindate,minbargainprice = @minbargainprice,usableyear = @usableyear,startusabledate = @startusabledate,winner = @winner,windate = @windate,landpurposedesc = @landpurposedesc,landpurposecode = @landpurposecode,landarea = @landarea,buildingarea = @buildingarea,dealtotalprice = @dealtotalprice,dealdate = @dealdate,cubagerate = @cubagerate,mincubagerate = @mincubagerate,maxcubagerate = @maxcubagerate,coverrate = @coverrate,maxcoverrate = @maxcoverrate,bargainstatecode = @bargainstatecode,developdegreecode = @developdegreecode,landunitprice = @landunitprice,buildunitprice = @buildunitprice,heightlimited = @heightlimited,planlimited = @planlimited,landusestatus = @landusestatus,landclass = @landclass,greenrage = @greenrage,mingreenrage = @mingreenrage,arrangestartdate = @arrangestartdate,arrangeenddate = @arrangeenddate,remark = @remark,savedatetime = @savedatetime,saveuser = @saveuser,sourcename = @sourcename,sourcelink = @sourcelink,sourcephone = @sourcephone,subareaid = @subareaid,seller = @seller,enddate = @enddate
where caseid = @caseid";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtLand))
            {
                return conn.Execute(strSql, caseLand);
            }
        }
        #endregion


        public List<int> GetAccessFxtCompanyId(int fxtCompanyId, int cityId)
        {
            var strSql = @"select [value] from FXTProject.dbo.[SplitToTable]((select LandCaseCompanyId from FxtDataCenter.dbo.Privi_Company_ShowData where Cityid=@cityid and fxtCompanyid=@fxtCompanyid and typecode= 1003002),',')";

            SqlParameter[] parameters = { 
                                            new SqlParameter("@cityid",SqlDbType.Int),
                                             new SqlParameter("@fxtCompanyid",SqlDbType.Int)
                                        };

            parameters[0].Value = cityId;
            parameters[1].Value = fxtCompanyId;

            DBHelperSql.ConnectionString = ConfigurationHelper.FxtDataCenter;
            var dt = DBHelperSql.ExecuteDataTable(strSql, parameters);
            var list = new List<int>();
            if (dt.Rows.Count <= 0) return list;
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                list.Add(Convert.ToInt32(dt.Rows[i][0]));
            }

            return list;

        }
    }
}
