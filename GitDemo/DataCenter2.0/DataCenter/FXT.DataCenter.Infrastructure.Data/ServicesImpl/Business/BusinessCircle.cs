using System.Data;
using System.Data.SqlClient;
using System.Linq;
using FXT.DataCenter.Domain.Models.DTO;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;

using Dapper;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class BusinessCircle : IBusinessCircle
    {

        public IQueryable<SYS_SubArea_Biz> GetSubAreaBiz(SYS_SubArea_Biz subAreaBiz, int pageIndex, int pageSize, out int totalCount, bool self = true)
        {
            string ptable, ctable, btable, comId, areaIds;
            Access(subAreaBiz.CityId, subAreaBiz.FxtCompanyId ?? -1, out ptable, out ctable, out btable, out comId);
            if (string.IsNullOrEmpty(comId)) comId = subAreaBiz.FxtCompanyId == null ? "" : "-1";
            GetAreaIds(subAreaBiz.CityId, out areaIds);

            if (self) comId = subAreaBiz.FxtCompanyId.ToString();

            var strSql = @"select sb.*,a.AreaName,c.CodeName as TypeName,(select dbo.Fun_GetBusinessCircleXYList(sb.SubAreaId,sb.AreaId,sb.FxtCompanyId,'|')) as LngOrLat
from FxtDataCenter.dbo.SYS_SubArea_Biz sb with(nolock)
left join FxtDataCenter.dbo.SYS_Area a with(nolock) on a.AreaId = sb.AreaId
left join FxtDataCenter.dbo.SYS_Code c with(nolock) on c.Code = sb.TypeCode
where sb.fxtCompanyId in (" + comId + ") and sb.AreaId in (" + areaIds + ")";

            if (!string.IsNullOrWhiteSpace(subAreaBiz.SubAreaName)) strSql += " and sb.SubAreaName = @subAreaName";
            //strSql += " order by sb.CreateDate desc";

            //分页SQL
            var pagenatedSql = @"select top " + pageSize + @" tt.*
                                from (
	                                select row_number() over (
			                                order by t.subAreaId desc
			                                ) rownumber
		                                ,t.*
	                                from (" + strSql + @") t ) tt
                                where tt.rownumber > (" + pageIndex + @" - 1) * " + pageSize;

            //总条数SQL
            var totalCountSql = "select count(1) from (" + strSql + ") as t1";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                totalCount = conn.Query<int>(totalCountSql, subAreaBiz).FirstOrDefault();
                return conn.Query<SYS_SubArea_Biz>(pagenatedSql, subAreaBiz).AsQueryable();
            }

        }

//        public int GetSubAreaId(int areaId, int fxtCompanyId, string name)
//        {

//            var strSql = @"select sb.SubAreaId
//from FxtDataCenter.dbo.SYS_SubArea_Biz sb with(nolock)
//where sb.SubAreaName = @name and sb.AreaId = @AreaId and sb.fxtcompanyId = @fxtcompanyId";

//            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter_Role))
//            {
//                return conn.Query<int>(strSql, new { name, areaId, fxtCompanyId }).FirstOrDefault();
//            }

//        }

        public IQueryable<SYS_SubArea_Biz> GetSubAreaBizByAreaId(int areaId, int cityId, int fxtcompanyId)
        {
            const string strSql = @"
SELECT subAreaId
	,subAreaName
FROM FxtDataCenter.dbo.SYS_SubArea_Biz sa
LEFT JOIN fxtdatacenter.dbo.SYS_Area a ON sa.AreaId = a.AreaId
WHERE (
		sa.AreaId = @areaId
		OR @areaid = - 1
		)
	AND a.CityId = @cityId
	AND sa.FxtCompanyId = @fxtcompanyId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<SYS_SubArea_Biz>(strSql, new { areaId, cityId, fxtcompanyId }).AsQueryable();
            }

        }


        public int AddSubAreaBiz(SYS_SubArea_Biz subAreaBiz)
        {
            const string strSql = @"insert into SYS_SubArea_Biz (subareaname,areaid,arealine,details,typecode,x,y,xyscale,fxtcompanyid,createdate,creators,saveuser,savedate) 
values(@subareaname,@areaid,@arealine,@details,@typecode,@x,@y,@xyscale,@fxtcompanyid,@createdate,@creators,@saveuser,@savedate)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                conn.Execute(strSql, subAreaBiz);
                const string sql = "select SubAreaId from SYS_SubArea_Biz where SubAreaName = @SubAreaName and AreaId = @AreaId";
                return conn.Query<int>(sql, new { subAreaBiz.SubAreaName, subAreaBiz.AreaId }).FirstOrDefault();
            }
        }

        public int UpdateSubAreaBiz(SYS_SubArea_Biz subAreaBiz)
        {

            const string strSql = @"update SYS_SubArea_Biz set subareaname = @subareaname,areaid = @areaid,arealine = @arealine,details = @details,typecode = @typecode,x = @x,y = @y,xyscale = @xyscale,saveuser = @saveuser,savedate = @savedate
where subareaid = @subareaid";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Execute(strSql, subAreaBiz);
            }
        }

        public int DeleteSubAreaBiz(int id)
        {
            const string strSql = @"delete from SYS_SubArea_Biz where subareaid = @subareaid";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Execute(strSql, new { subareaid = id });
            }
        }


        public SYS_SubArea_Biz GetSubAreaBizById(int id)
        {
            const string strSql = @"select sb.*,a.AreaName,c.CodeName as TypeName,(select dbo.Fun_GetBusinessCircleXYList(sb.SubAreaId,sb.AreaId,sb.FxtCompanyId,'|')) as LngOrLat 
from FxtDataCenter.dbo.SYS_SubArea_Biz sb with(nolock)
left join FxtDataCenter.dbo.SYS_Area a with(nolock) on a.AreaId = sb.AreaId
left join FxtDataCenter.dbo.SYS_Code c with(nolock) on c.Code = sb.TypeCode
where sb.subAreaId = @Id";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<SYS_SubArea_Biz>(strSql, new { id }).FirstOrDefault();
            }
        }

        public int AddSubAreaBizCoordinate(SYS_SubArea_Biz_Coordinate subAreaBizCoordinate)
        {
            const string strSql = @"insert into SYS_SubArea_Biz_Coordinate (subareaid,areaid,cityid,x,y,fxtcompanyid) 
values(@subareaid,@areaid,@cityid,@x,@y,@fxtcompanyid)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Execute(strSql, subAreaBizCoordinate);
            }
        }

        public int UpdateSubAreaBizCoordinate(SYS_SubArea_Biz_Coordinate subAreaBizCoordinate)
        {
            const string strSqlDelete = @"Update SYS_SubArea_Biz_Coordinate set valid =0 where subareaid = @subareaid and areaid = @areaid and FxtCompanyId = @fxtCompanyId";
            const string strSqlAdd = @"insert into SYS_SubArea_Biz_Coordinate (subareaid,areaid,cityid,x,y,fxtcompanyid) 
values(@subareaid,@areaid,@cityid,@x,@y,@fxtcompanyid)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                conn.Execute(strSqlDelete, subAreaBizCoordinate);
                return conn.Execute(strSqlAdd, subAreaBizCoordinate);
            }
        }

        public int GetSubAreaBizCoordinate(int subAreaId, int areaId, int fxtCompanyId)
        {
            const string strSql = @"select count(1) from SYS_SubArea_Biz_Coordinate where subareaid = @subareaid and areaId= @areaId and FxtCompanyId = @fxtCompanyId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<int>(strSql, new { subAreaId, areaId, fxtCompanyId }).FirstOrDefault();
            }
        }
        
        public IQueryable<SubAreaBizStatisticDTO> GetSubAreaBizStatistic(int areaId, int fxtCompanyId, int cityId)
        {
            string areaIds;
            GetAreaIds(cityId, out areaIds);
            if (!(new[] { 0, -1 }).Contains(areaId)) areaIds = areaId.ToString();



            var strSql = @"select s.AreaName
	                            ,(
		                            select COUNT(sb.subareaId)
		                            from FxtDataCenter.dbo.SYS_SubArea_Biz sb
		                            where s.areaId = sb.areaId
			                            and sb.FxtCompanyId = @fxtCompanyId
		                            ) as SubAreaBizQuantity
	                            ,(
		                            select COUNT(pb.ProjectId)
		                            from FxtData_Biz.dbo.Dat_Project_Biz pb with (nolock)
		                            where pb.AreaId = s.AreaId
			                            and pb.FxtCompanyId = @fxtCompanyId
                                      
		                            ) as ProjectBizQuantity
	                            ,(
		                            select COUNT(bb.BuildingId)
		                            from FxtData_Biz.dbo.Dat_Project_Biz pb with (nolock)
			                            ,FxtData_Biz.dbo.Dat_Building_Biz bb with (nolock)
		                            where bb.ProjectId = pb.ProjectId
			                            and pb.AreaId = s.AreaId
			                            and pb.FxtCompanyId = @fxtCompanyId
                                      
		                            ) as BuildingBizQuantity
                            from FxtDataCenter.dbo.Sys_Area s with (nolock)
                            where s.AreaId in (" + areaIds + @")
                            group by s.areaId
	                            ,s.AreaName";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<SubAreaBizStatisticDTO>(strSql, new { fxtCompanyId }).AsQueryable();
            }
        }
        
        #region 公共
        private static void Access(int cityid, int fxtcompanyid, out string ptable, out string ctable, out string btable, out string comId)
        {
            var sql = @"SELECT [ProjectTable],[BuildingTable],[HouseTable],[CaseTable],[QueryInfoTable],[ReportTable],[PrintTable],[HistoryTable],[QueryTaxTable],s.BizCompanyId FROM FxtDataCenter.dbo.[SYS_City_Table] c with(nolock),[Privi_Company_ShowData] s with(nolock) where c.CityId=@cityid  and c.CityId=s.CityId and s.FxtCompanyId=@fxtcompanyid and typecode= 1003002";

            SqlParameter[] parameter = { 
                                           new SqlParameter("@cityid",SqlDbType.Int),
                                           new SqlParameter("@fxtcompanyid",SqlDbType.Int)
                                       };
            parameter[0].Value = cityid;
            parameter[1].Value = fxtcompanyid;

            DBHelperSql.ConnectionString = ConfigurationHelper.FxtDataCenter;
            var dt = DBHelperSql.ExecuteDataTable(sql, parameter);
            if (dt.Rows.Count == 0)
            {
                ptable = "";
                ctable = "";
                btable = "";
                comId = "";
            }
            else
            {
                ptable = dt.Rows[0]["ProjectTable"].ToString();
                ctable = dt.Rows[0]["CaseTable"].ToString();
                btable = dt.Rows[0]["BuildingTable"].ToString();
                comId = dt.Rows[0]["BizCompanyId"].ToString();
            }

        }

        private static void GetAreaIds(int cityId, out string areaIds)
        {
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                const string strSql = " select AreaId from FxtDataCenter.dbo.SYS_Area with(nolock) where CityId =@cityId ";
                var result = conn.Query<int>(strSql, new { cityId });
                areaIds = string.Join(",", result);
            }
        }

        #endregion
        
        public bool IsExistSubAreaBiz(int areaId, int fxtCompanyId, int subAreaId, string subAreaName)
        {
            var strSql = @"select SubAreaId from FxtDataCenter.dbo.SYS_SubArea_Biz with(nolock) where AreaId = @AreaId and FxtCompanyId=@FxtCompanyId and SubAreaName = @SubAreaName  ";
            strSql += subAreaId == -1 ? "" : " and SubAreaId !=@SubAreaId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<int>(strSql, new { areaId, fxtCompanyId, subAreaId, subAreaName }).Any();
            }
        }
    }
}
