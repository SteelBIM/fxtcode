using System.Linq;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Models.DTO;
using FXT.DataCenter.Domain.Services;

using System.Data;
using Dapper;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using System.Data.SqlClient;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class OfficeSubArea : IOfficeSubArea
    {
        //cityid
        public IQueryable<SYS_SubArea_Office> GetSubAreaNamesByAreaId(int areaId, int fxtCompanyId, int cityId)
        {
            string strSql = @"
SELECT subAreaId
	,subAreaName
FROM FxtDataCenter.dbo.SYS_SubArea_Office sa WITH (NOLOCK)
LEFT JOIN fxtdatacenter.dbo.SYS_Area a WITH (NOLOCK) ON sa.AreaId = a.AreaId
WHERE sa.FxtCompanyId = @FxtCompanyId
	AND a.CityId = @CityId" + (areaId == -1 ? "" : " AND sa.areaId = @areaId");

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<SYS_SubArea_Office>(strSql, new { areaId, fxtCompanyId, cityId }).AsQueryable();
            }
        }

        public int GetSubAreaIdByName(string name, int areaId, int fxtCompanyId)
        {
            var strSql = @"SELECT sb.SubAreaId FROM FxtDataCenter.dbo.SYS_SubArea_Office sb WITH (NOLOCK) WHERE sb.SubAreaName =  @name and sb.AreaId = @areaId and FxtCompanyId = @FxtCompanyId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<int>(strSql, new { name, areaId, fxtCompanyId }).FirstOrDefault();
            }

        }

        public IQueryable<SYS_SubArea_Office> GetSubAreas(SYS_SubArea_Office subAreaOffice, int pageIndex, int pageSize, out int totalCount)
        {
            var where = string.Empty;
            if (!(new[] { 0, -1 }).Contains(subAreaOffice.AreaId)) where += " and sa.AreaId = @AreaId";
            if (!string.IsNullOrEmpty(subAreaOffice.SubAreaName)) where += " and sa.SubAreaName like @SubAreaName";
            if (!(new[] { 0, -1 }).Contains(subAreaOffice.TypeCode ?? -1)) where += "  and sa.TypeCode = @TypeCode";

            var strSql = @"
SELECT sa.*
	,a.AreaName
	,c.CodeName AS TypeCodeName
FROM FxtDataCenter.dbo.SYS_SubArea_Office sa WITH (NOLOCK)
LEFT JOIN FxtDataCenter.dbo.SYS_Area a WITH (NOLOCK) ON sa.AreaId = a.AreaId
LEFT JOIN FxtDataCenter.dbo.SYS_Code c WITH (NOLOCK) ON sa.TypeCode = c.Code
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

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                totalCount = conn.Query<int>(totalCountSql, new { FxtCompanyId = subAreaOffice.FxtCompanyId, cityId = subAreaOffice.CityId, AreaId = subAreaOffice.AreaId, TypeCode = subAreaOffice.TypeCode, SubAreaName = "%" + subAreaOffice.SubAreaName + "%" }).FirstOrDefault();
                return conn.Query<SYS_SubArea_Office>(pagenatedSql, new { FxtCompanyId = subAreaOffice.FxtCompanyId, cityId = subAreaOffice.CityId, AreaId = subAreaOffice.AreaId, TypeCode = subAreaOffice.TypeCode, SubAreaName = "%" + subAreaOffice.SubAreaName + "%" }).AsQueryable();
            }
        }

        public SYS_SubArea_Office GetSubAreaById(int subAreaId)
        {
            string sql = @"
SELECT sa.*
	,a.AreaName
	,c.CodeName AS TypeCodeName
	,(SELECT fxtdatacenter.dbo.Fun_GetOfficeCircleXYList(sa.SubAreaId, sa.AreaId, sa.FxtCompanyId, '|')) AS LngOrLat
FROM FxtDataCenter.dbo.SYS_SubArea_Office sa WITH (NOLOCK)
LEFT JOIN FxtDataCenter.dbo.SYS_Area a WITH (NOLOCK) ON sa.AreaId = a.AreaId
LEFT JOIN FxtDataCenter.dbo.SYS_Code c WITH (NOLOCK) ON sa.TypeCode = c.Code
WHERE sa.SubAreaId = @SubAreaId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                return conn.Query<SYS_SubArea_Office>(sql, new { subAreaId }).FirstOrDefault();
            }
        }

        public int UpdateSubArea(SYS_SubArea_Office subAreaOffice)
        {
            var strSqlMainUpdate = @"update FxtDataCenter.dbo.SYS_SubArea_Office set SubAreaName = @SubAreaName,AreaLine = @AreaLine,Details = @Details,TypeCode = @TypeCode,X=@X,Y=@Y,SaveUser=@SaveUser,SaveDate = @SaveDate
 where FxtCompanyId = @FxtCompanyId and AreaId = @AreaId and SubAreaId = @SubAreaId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                return conn.Execute(strSqlMainUpdate, new { SubAreaName = subAreaOffice.SubAreaName, AreaLine = subAreaOffice.AreaLine, Details = subAreaOffice.Details, TypeCode = subAreaOffice.TypeCode, X = subAreaOffice.X, Y = subAreaOffice.Y, SaveUser = subAreaOffice.SaveUser, SaveDate = subAreaOffice.SaveDate, FxtCompanyId = subAreaOffice.FxtCompanyId, AreaId = subAreaOffice.AreaId, SubAreaId = subAreaOffice.SubAreaId });
            }
        }

        public int AddSubArea(SYS_SubArea_Office subAreaOffice)
        {
            var sql = @"insert into FxtDataCenter.dbo.SYS_SubArea_Office(SubAreaName,AreaId,AreaLine,Details,TypeCode,X,Y,XYScale,FxtCompanyId,CreateDate,Creators,SaveUser,SaveDate)
 values(@SubAreaName,@AreaId,@AreaLine,@Details,@TypeCode,@X,@Y,@XYScale,@FxtCompanyId,@CreateDate,@Creators,@SaveUser,@SaveDate)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                return conn.Execute(sql, subAreaOffice);
            }
        }

        public int CanDelete(int subAreaId, int companyId)
        {
            var sql = @"
SELECT COUNT(*) FROM (
	SELECT *
	FROM FxtData_Office.dbo.Dat_Project_Office p
	WHERE NOT EXISTS (
			SELECT ProjectId
			FROM FxtData_Office.dbo.Dat_Project_Office_sub ps WITH (NOLOCK)
			WHERE ps.CityId = p.CityId
				AND ps.FxtCompanyId = @FxtCompanyId
				AND ps.ProjectId = p.ProjectId
			)
		AND p.Valid = 1
		AND p.FxtCompanyId = @FxtCompanyId
		AND p.SubAreaId = @SubAreaId	
	UNION	
	SELECT *
	FROM FxtData_Office.dbo.Dat_Project_Office_sub p
	WHERE p.Valid = 1
		AND p.FxtCompanyId = @FxtCompanyId
		AND p.SubAreaId = @SubAreaId
	) T";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                return conn.Query<int>(sql, new { SubAreaId = subAreaId, FxtCompanyId = companyId }).FirstOrDefault();
            }
        }

        public int DeleteSubArea(SYS_SubArea_Office subAreaOffices, int FxtCompanyId)
        {
            var sql = @"delete fxtdatacenter.dbo.SYS_SubArea_Office where FxtCompanyId = @FxtCompanyId and SubAreaId = @SubAreaId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                return conn.Execute(sql, new { SubAreaId = subAreaOffices.SubAreaId, FxtCompanyId = FxtCompanyId });
            }
        }

        public IQueryable<SubAreaOfficeStatisticDTO> GetSubAreaOfficeStatistic(int areaId, int fxtCompanyId, int cityId, bool self)
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
			select * from FxtData_Office.dbo.Dat_Project_Office p with(nolock)
			where not exists(
				select ProjectId from FxtData_Office.dbo.Dat_Project_Office_sub ps with(nolock)
				where ps.CityId = p.CityId
				and ps.FxtCompanyId = @fxtCompanyId
				and ps.ProjectId = p.ProjectId
			)
			and p.Valid = 1
			and p.CityId = @CityId
			and p.FxtCompanyId in (" + comId + @")
			union
			select * from FxtData_Office.dbo.Dat_Project_Office_sub p with(nolock)
			where p.Valid = 1
			and p.CityId = @CityId
			and p.FxtCompanyId = @fxtCompanyId
		)projectTable
		left join 
		(
			select * from FxtData_Office.dbo.Dat_Building_Office b with(nolock)
			where not exists(
				select BuildingId from FxtData_Office.dbo.Dat_Building_Office_sub bs with(nolock)
				where bs.CityId = b.CityId
				and bs.FxtCompanyId = @fxtCompanyId
				and bs.ProjectId = b.ProjectId
				and bs.BuildingId = b.BuildingId
			)
			and b.Valid = 1
			and b.CityId = @CityId
			and b.FxtCompanyId in (" + comId + @")
			union
			select * from FxtData_Office.dbo.Dat_Building_Office_sub b with(nolock)
			where b.Valid = 1
			and b.CityId = @CityId
			and b.FxtCompanyId = @fxtCompanyId
		)buildingTabel on projectTable.ProjectId = buildingTabel.ProjectId
	)T
	group by AreaId,SubAreaId
)T
left join FxtDataCenter.dbo.SYS_Area a with(nolock) on t.AreaId = a.AreaId
left join FxtDataCenter.dbo.SYS_SubArea_Office sa with(nolock) on t.SubAreaId = sa.SubAreaId and t.AreaId = sa.AreaId" + where;

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                return conn.Query<SubAreaOfficeStatisticDTO>(strSql, new { areaId, cityId, fxtCompanyId }).AsQueryable();
            }
        }

        private static void Access(int cityid, int fxtcompanyid, out string ptable, out string ctable, out string btable, out string comId)
        {
            var sql = @"SELECT [ProjectTable],[BuildingTable],[HouseTable],[CaseTable],[QueryInfoTable],[ReportTable],[PrintTable],[HistoryTable],[QueryTaxTable],s.OfficeCompanyId FROM FxtDataCenter.dbo.[SYS_City_Table] c with(nolock),[Privi_Company_ShowData] s with(nolock) where c.CityId=@cityid  and c.CityId=s.CityId and s.FxtCompanyId=@fxtcompanyid and typecode= 1003002";

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
                comId = dt.Rows[0]["OfficeCompanyId"].ToString();
            }
        }

        public bool IsExistSubAreaOffice(int areaId, int subAreaId, string subAreaName, int fxtCompanyId)
        {
            var strSql = @"SELECT SubAreaId FROM FxtDataCenter.dbo.SYS_SubArea_Office WITH (NOLOCK) WHERE FxtCompanyId = @FxtCompanyId AND AreaId = @AreaId and SubAreaName = @SubAreaName";
            strSql += subAreaId == -1 ? "" : " and SubAreaId !=@SubAreaId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<int>(strSql, new { areaId, fxtCompanyId, subAreaId, subAreaName }).Any();
            }
        }

        public int GetSubAreaOfficeCoordinate(int subAreaId, int areaId, int fxtCompanyId)
        {
            const string strSql = @"
SELECT count(1) FROM FxtDataCenter.dbo.SYS_SubArea_Office_Coordinate
WHERE subareaid = @subareaid
	AND areaId = @areaId
	AND FxtCompanyId = @fxtCompanyId
    and Valid = 1";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<int>(strSql, new { subAreaId, areaId, fxtCompanyId }).FirstOrDefault();
            }
        }

        public int AddSubAreaOfficeCoordinate(SYS_SubArea_Office_Coordinate subAreaOfficeCoordinate)
        {
            const string strSql = @"insert into FxtDataCenter.dbo.SYS_SubArea_Office_Coordinate (subareaid,areaid,cityid,x,y,fxtcompanyid) 
values(@subareaid,@areaid,@cityid,@x,@y,@fxtcompanyid)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Execute(strSql, subAreaOfficeCoordinate);
            }
        }

        public int UpdateSubAreaOfficeCoordinate(int subAreaId, int areaId, int fxtCompanyId)
        {
            string strSql = @"
update FxtDataCenter.dbo.SYS_SubArea_Office_Coordinate set Valid = 0
WHERE subareaid = @subareaid
	AND areaId = @areaId
	AND FxtCompanyId = @fxtCompanyId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Execute(strSql, new { subAreaId, areaId, fxtCompanyId });
            }
        }

    }
}
