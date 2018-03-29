using System.Linq;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Models.DTO;
using FXT.DataCenter.Domain.Services;

using System.Data;
using Dapper;
using System.Data.SqlClient;
using FXT.DataCenter.Infrastructure.Common.DBHelper;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class IndustrySubArea : IIndustrySubArea
    {
        private static void Access(int cityid, int fxtcompanyid, out string ptable, out string ctable, out string btable, out string comId)
        {
            var sql = @"
SELECT [ProjectTable]
	,[BuildingTable]
	,[HouseTable]
	,[CaseTable]
	,[QueryInfoTable]
	,[ReportTable]
	,[PrintTable]
	,[HistoryTable]
	,[QueryTaxTable]
	,s.IndustryCompanyId
FROM FxtDataCenter.dbo.[SYS_City_Table] c WITH (NOLOCK)
	,FxtDataCenter.dbo.Privi_Company_ShowData s WITH (NOLOCK)
WHERE c.CityId = @cityid
	AND c.CityId = s.CityId
	AND s.FxtCompanyId = @fxtcompanyid and typecode= 1003002";

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
                comId = dt.Rows[0]["IndustryCompanyId"].ToString();
            }
        }

        public IQueryable<SYS_SubArea_Industry> GetSubAreas(SYS_SubArea_Industry subArea, int pageIndex, int pageSize, out int totalCount)
        {
            var where = string.Empty;
            if (!(new[] { 0, -1 }).Contains(subArea.AreaId)) where += " and sa.AreaId = @AreaId";
            if (!string.IsNullOrEmpty(subArea.SubAreaName)) where += " and sa.SubAreaName like @SubAreaName";

            var strSql = @"
SELECT sa.*
	,a.AreaName
FROM FxtDataCenter.dbo.SYS_SubArea_Industry sa WITH (NOLOCK)
LEFT JOIN FxtDataCenter.dbo.SYS_Area a WITH (NOLOCK) ON sa.AreaId = a.AreaId
WHERE sa.FxtCompanyId = @fxtCompanyId
and a.CityId = @cityId" + where;

            //分页SQL
            var pagenatedSql = @"select top " + pageSize + @" tt.*
                                from (
	                                select row_number() over (
			                                order by t.SubAreaId desc
			                                ) rownumber
		                                ,t.*
	                                from (" + strSql + @") t ) tt
                                where tt.rownumber > (" + pageIndex + @" - 1) * " + pageSize;

            //总条数SQL
            var totalCountSql = "select count(1) from (" + strSql + ") as t1";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                totalCount = conn.Query<int>(totalCountSql, new { FxtCompanyId = subArea.FxtCompanyId, cityId = subArea.CityId, AreaId = subArea.AreaId, SubAreaName = "%" + subArea.SubAreaName + "%" }).FirstOrDefault();
                return conn.Query<SYS_SubArea_Industry>(pagenatedSql, new { FxtCompanyId = subArea.FxtCompanyId, cityId = subArea.CityId, AreaId = subArea.AreaId, SubAreaName = "%" + subArea.SubAreaName + "%" }).AsQueryable();
            }
        }

        public SYS_SubArea_Industry GetSubAreaById(int subAreaId)
        {
            string sql = @"
SELECT sa.*
	,a.AreaName
	,(SELECT fxtdatacenter.dbo.Fun_GetIndustryCircleXYList(sa.SubAreaId, sa.AreaId, sa.FxtCompanyId, '|')) AS LngOrLat
FROM FxtDataCenter.dbo.SYS_SubArea_Industry sa WITH (NOLOCK)
LEFT JOIN FxtDataCenter.dbo.SYS_Area a WITH (NOLOCK) ON sa.AreaId = a.AreaId
WHERE sa.SubAreaId = @SubAreaId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Query<SYS_SubArea_Industry>(sql, new { subAreaId }).FirstOrDefault();
            }
        }

        public bool IsExistSubAreaIndustry(int areaId, int subAreaId, string subAreaName, int fxtCompanyId)
        {
            var strSql = @"SELECT SubAreaId FROM FxtDataCenter.dbo.SYS_SubArea_Industry WITH (NOLOCK) WHERE FxtCompanyId = @FxtCompanyId AND AreaId = @AreaId and SubAreaName = @SubAreaName";
            strSql += subAreaId == -1 ? "" : " and SubAreaId !=@SubAreaId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<int>(strSql, new { areaId, fxtCompanyId, subAreaId, subAreaName }).Any();
            }
        }

        public int UpdateSubArea(SYS_SubArea_Industry subArea)
        {
            var strSqlMainUpdate = @"update FxtDataCenter.dbo.SYS_SubArea_Industry set SubAreaName = @SubAreaName,AreaLine = @AreaLine,Details = @Details,X=@X,Y=@Y,SaveUser=@SaveUser,SaveDate = @SaveDate
 where FxtCompanyId = @FxtCompanyId and AreaId = @AreaId and SubAreaId = @SubAreaId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Execute(strSqlMainUpdate, subArea);
            }
        }

        public int AddSubArea(SYS_SubArea_Industry subArea)
        {
            var strSql = @"insert into FxtDataCenter.dbo.SYS_SubArea_Industry(SubAreaName,AreaId,AreaLine,Details,X,Y,XYScale,FxtCompanyId,CreateDate,Creators,SaveUser,SaveDate)
 values(@SubAreaName,@AreaId,@AreaLine,@Details,@X,@Y,@XYScale,@FxtCompanyId,@CreateDate,@Creators,@SaveUser,@SaveDate)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                conn.Execute(strSql, subArea);
                const string sql = "select SubAreaId from FxtDataCenter.dbo.SYS_SubArea_Industry where SubAreaName = @SubAreaName and AreaId = @AreaId";
                return conn.Query<int>(sql, new { subArea.SubAreaName, subArea.AreaId }).FirstOrDefault();
            }
        }

        public IQueryable<SYS_SubArea_Industry> GetSubAreaNamesByAreaId(int areaId, int fxtCompanyId, int cityId)
        {
            string strSql = @"
SELECT subAreaId
	,subAreaName
FROM FxtDataCenter.dbo.SYS_SubArea_Industry sa WITH (NOLOCK)
LEFT JOIN fxtdatacenter.dbo.SYS_Area a WITH (NOLOCK) ON sa.AreaId = a.AreaId
WHERE sa.FxtCompanyId = @FxtCompanyId
	AND a.CityId = @CityId" + (areaId == -1 ? "" : " AND sa.areaId = @areaId");

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<SYS_SubArea_Industry>(strSql, new { areaId, fxtCompanyId, cityId }).AsQueryable();
            }
        }

        public int GetSubAreaIndustryCoordinate(int subAreaId, int areaId, int fxtCompanyId)
        {
            const string strSql = @"
SELECT count(1) FROM FxtDataCenter.dbo.SYS_SubArea_Industry_Coordinate
WHERE subareaid = @subareaid
	AND areaId = @areaId
	AND FxtCompanyId = @fxtCompanyId
    and Valid = 1";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<int>(strSql, new { subAreaId, areaId, fxtCompanyId }).FirstOrDefault();
            }
        }

        public int AddSubAreaIndustryCoordinate(SYS_SubArea_Industry_Coordinate subAreaIndustryCoordinate)
        {
            const string strSql = @"insert into FxtDataCenter.dbo.SYS_SubArea_Industry_Coordinate (subareaid,areaid,cityid,x,y,fxtcompanyid) 
values(@subareaid,@areaid,@cityid,@x,@y,@fxtcompanyid)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Execute(strSql, subAreaIndustryCoordinate);
            }
        }

        public int UpdateSubAreaIndustryCoordinate(int subAreaId, int areaId, int fxtCompanyId)
        {
            string strSql = @"
update FxtDataCenter.dbo.SYS_SubArea_Industry_Coordinate set Valid = 0
WHERE subareaid = @subareaid
	AND areaId = @areaId
	AND FxtCompanyId = @fxtCompanyId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Execute(strSql, new { subAreaId, areaId, fxtCompanyId });
            }
        }

        public int CanDelete(int subAreaId, int companyId)
        {
            var sql = @"
SELECT COUNT(*) FROM (
	SELECT *
	FROM FxtData_Industry.dbo.Dat_Project_Industry p
	WHERE NOT EXISTS (
			SELECT ProjectId
			FROM FxtData_Industry.dbo.Dat_Project_Industry_sub ps WITH (NOLOCK)
			WHERE ps.CityId = p.CityId
				AND ps.FxtCompanyId = @FxtCompanyId
				AND ps.ProjectId = p.ProjectId
			)
		AND p.Valid = 1
		AND p.FxtCompanyId = @FxtCompanyId
		AND p.SubAreaId = @SubAreaId	
	UNION	
	SELECT *
	FROM FxtData_Industry.dbo.Dat_Project_Industry_sub p
	WHERE p.Valid = 1
		AND p.FxtCompanyId = @FxtCompanyId
		AND p.SubAreaId = @SubAreaId
	) T";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Query<int>(sql, new { SubAreaId = subAreaId, FxtCompanyId = companyId }).FirstOrDefault();
            }
        }

        public int DeleteSubArea(SYS_SubArea_Industry subArea, int FxtCompanyId)
        {
            var sql = @"delete FxtDataCenter.dbo.SYS_SubArea_Industry where FxtCompanyId = @FxtCompanyId and SubAreaId = @SubAreaId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Execute(sql, new { SubAreaId = subArea.SubAreaId, FxtCompanyId = FxtCompanyId });
            }
        }

        public IQueryable<SubAreaIndustryStatisticDTO> GetSubAreaIndustryStatistic(int areaId, int fxtCompanyId, int cityId, bool self)
        {
            string ptable, ctable, btable, comId;
            Access(cityId, fxtCompanyId, out ptable, out ctable, out btable, out comId);
            if (string.IsNullOrEmpty(comId)) comId = fxtCompanyId.ToString();
            if (self) comId = fxtCompanyId.ToString();

            string where = areaId == -1 ? "" : " where T.AreaId = @areaId";

            string strSql = @"
select 
	a.AreaName
	,sa.SubAreaName + case when sa.FxtCompanyId = @fxtCompanyId then '' else '(others)' end as SubAreaName
	,T.projectCount
	,T.buildingCount
from (
	select 
		AreaId
		,SubAreaId
		,COUNT(distinct ProjectId) as projectCount
		,COUNT(BuildingId) as buildingCount
	from (
		select 
			projectTable.ProjectId
			,projectTable.AreaId
			,projectTable.SubAreaId
			,buildingTabel.BuildingId
		from (
			select * from FxtData_Industry.dbo.Dat_Project_Industry p with(nolock)
			where not exists(
				select ProjectId from FxtData_Industry.dbo.Dat_Project_Industry_sub ps with(nolock)
				where ps.CityId = p.CityId
				and ps.FxtCompanyId = @fxtCompanyId
				and ps.ProjectId = p.ProjectId
			)
			and p.Valid = 1
			and p.CityId = @CityId
			and p.FxtCompanyId in (" + comId + @")
			union
			select * from FxtData_Industry.dbo.Dat_Project_Industry_sub p with(nolock)
			where p.Valid = 1
			and p.CityId = @CityId
			and p.FxtCompanyId = @fxtCompanyId
		)projectTable
		left join 
		(
			select * from FxtData_Industry.dbo.Dat_Building_Industry b with(nolock)
			where not exists(
				select BuildingId from FxtData_Industry.dbo.Dat_Building_Industry_sub bs with(nolock)
				where bs.CityId = b.CityId
				and bs.FxtCompanyId = @fxtCompanyId
				and bs.ProjectId = b.ProjectId
				and bs.BuildingId = b.BuildingId
			)
			and b.Valid = 1
			and b.CityId = @CityId
			and b.FxtCompanyId in (" + comId + @")
			union
			select * from FxtData_Industry.dbo.Dat_Building_Industry_sub b with(nolock)
			where b.Valid = 1
			and b.CityId = @CityId
			and b.FxtCompanyId = @fxtCompanyId
		)buildingTabel on projectTable.ProjectId = buildingTabel.ProjectId
	)T
	group by AreaId,SubAreaId
)T
left join FxtDataCenter.dbo.SYS_Area a with(nolock) on t.AreaId = a.AreaId
left join FxtDataCenter.dbo.SYS_SubArea_Industry sa with(nolock) on t.SubAreaId = sa.SubAreaId and t.AreaId = sa.AreaId" + where;

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Query<SubAreaIndustryStatisticDTO>(strSql, new { areaId, cityId, fxtCompanyId }).AsQueryable();
            }
        }

        public int GetSubAreaIdByName(string name, int areaId, int fxtCompanyId)
        {
            var strSql = @"SELECT sb.SubAreaId FROM FxtDataCenter.dbo.SYS_SubArea_Industry sb WITH (NOLOCK) WHERE sb.SubAreaName =  @name and sb.AreaId = @areaId and FxtCompanyId = @FxtCompanyId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<int>(strSql, new { name, areaId, fxtCompanyId }).FirstOrDefault();
            }
        }

    }
}
