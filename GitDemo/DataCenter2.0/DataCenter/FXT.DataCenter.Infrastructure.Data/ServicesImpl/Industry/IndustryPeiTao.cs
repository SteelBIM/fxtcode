using System.Linq;
using System.Data;
using Dapper;
using System.Data.SqlClient;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Common.DBHelper;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class IndustryPeiTao : IIndustryPeiTao
    {
        #region access
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

        #endregion
        public IQueryable<DatPeiTaoIndustry> GetIndustryPeiTaos(DatPeiTaoIndustry peitaoIndustry, int pageIndex, int pageSize, out int totalCount, bool self)
        {
            string ptable, ctable, btable, comId;
            Access(peitaoIndustry.CityId ?? 0, peitaoIndustry.FxtCompanyId ?? 0, out ptable, out ctable, out btable, out comId);
            if (string.IsNullOrEmpty(comId))
            {
                comId = peitaoIndustry.FxtCompanyId.ToString();
            }
            if (self)
            {
                comId = peitaoIndustry.FxtCompanyId.ToString();
            }

            var where = string.Empty;
            if (!(new[] { 0, -1 }).Contains(peitaoIndustry.PeiTaoCode)) where += " and PeiTaoTable.PeiTaoCode = @PeiTaoCode";
            if (!string.IsNullOrEmpty(peitaoIndustry.PeiTaoName)) where += " and PeiTaoTable.PeiTaoName like @PeiTaoName";

            //查询全部字段时，select * 与 select a,b...效率差不多
            var strSql = @"
SELECT PeiTaoTable.*
	,ProjectTable.ProjectName
	,ProjectTable.AreaId
	,a.AreaName
	,c.CodeName AS PeiTaoCodeName
	,dc.ChineseName AS TenantName
FROM (
	SELECT pt.*
	FROM FxtData_Industry.dbo.Dat_PeiTao_Industry pt WITH (NOLOCK)
	WHERE 1 = 1
		AND NOT EXISTS (
			SELECT PeiTaoID
			FROM FxtData_Industry.dbo.Dat_PeiTao_Industry_sub pts WITH (NOLOCK)
			WHERE 1 = 1
				AND pts.CityId = pt.CityId
				AND pts.FxtCompanyId = @FxtCompanyId
				AND pts.ProjectId = pt.ProjectId
				AND pts.PeiTaoID = pt.PeiTaoID
			)
		AND pt.Valid = 1
		AND pt.CityId = @CityId
		AND pt.FxtCompanyId IN (" + comId + @")	
	UNION	
	SELECT pt.*
	FROM FxtData_Industry.dbo.Dat_PeiTao_Industry_sub pt WITH (NOLOCK)
	WHERE 1 = 1
		AND pt.Valid = 1
		AND pt.CityId = @CityId
		AND pt.FxtCompanyId = @FxtCompanyId
	) PeiTaoTable
INNER JOIN (
	SELECT *
	FROM FxtData_Industry.dbo.Dat_Project_Industry p WITH (NOLOCK)
	WHERE NOT EXISTS (
			SELECT ProjectId
			FROM FxtData_Industry.dbo.Dat_Project_Industry_sub ps WITH (NOLOCK)
			WHERE ps.ProjectId = p.ProjectId
				AND ps.FxtCompanyId = @FxtCompanyId
				AND ps.CityId = @CityId
			)
		AND p.Valid = 1
		AND p.FxtCompanyId IN (" + comId + @")
		AND p.CityId = @CityId
		AND p.ProjectId = @ProjectId	
	UNION	
	SELECT *
	FROM FxtData_Industry.dbo.Dat_Project_Industry_sub p WITH (NOLOCK)
	WHERE p.Valid = 1
		AND p.CityId = @CityId
		AND p.FxtCompanyId = @FxtCompanyId
		AND p.ProjectId = @ProjectId
	) ProjectTable ON PeiTaoTable.ProjectId = ProjectTable.ProjectId
LEFT JOIN FxtDataCenter.dbo.SYS_Code c WITH (NOLOCK) ON PeiTaoTable.PeiTaoCode = c.Code
LEFT JOIN FxtDataCenter.dbo.DAT_Company dc WITH (NOLOCK) ON PeiTaoTable.TenantID = dc.CompanyId
LEFT JOIN FxtDataCenter.dbo.SYS_Area a WITH (NOLOCK) ON ProjectTable.AreaId = a.AreaId
where 1 = 1" + where;

            //分页SQL
            var pagenatedSql = @"select top " + pageSize + @" tt.*
                                from (
	                                select row_number() over (
			                                order by t.PeiTaoID desc
			                                ) rownumber
		                                ,t.*
	                                from (" + strSql + @") t ) tt
                                where tt.rownumber > (" + pageIndex + @" - 1) * " + pageSize;

            //总条数SQL
            var totalCountSql = "select count(1) from (" + strSql + ") as t1";


            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                totalCount = conn.Query<int>(totalCountSql, new { peitaoIndustry.FxtCompanyId, peitaoIndustry.CityId, peitaoIndustry.ProjectId, PeiTaoName = "%" + peitaoIndustry.PeiTaoName + "%", peitaoIndustry.PeiTaoCode }).FirstOrDefault();
                return conn.Query<DatPeiTaoIndustry>(pagenatedSql, new { peitaoIndustry.FxtCompanyId, peitaoIndustry.CityId, peitaoIndustry.ProjectId, PeiTaoName = "%" + peitaoIndustry.PeiTaoName + "%", peitaoIndustry.PeiTaoCode }).AsQueryable();
            }
        }

        public DatPeiTaoIndustry GetPeiTaoById(long peitaoId, int fxtCompanyId)
        {
            const string strSql = @"
SELECT pt.*
	,p.ProjectName
	,c1.CodeName as PeiTaoCodeName
    ,c2.ChineseName as TenantName
FROM FxtData_Industry.dbo.Dat_PeiTao_Industry pt WITH (NOLOCK)
left join FxtData_Industry.dbo.Dat_Project_Industry p on pt.ProjectId = p.ProjectId
left join FxtDataCenter.dbo.SYS_Code c1 on pt.PeiTaoCode = c1.Code
left join fxtdatacenter.dbo.DAT_Company c2 on pt.TenantID = c2.CompanyId
WHERE NOT EXISTS (
		SELECT PeiTaoID
		FROM FxtData_Industry.dbo.Dat_PeiTao_Industry_sub pts WITH (NOLOCK)
		WHERE pts.cityId = pt.cityId
			AND pts.fxtCompanyId = @fxtCompanyId
            AND pts.ProjectId = pt.ProjectId
			AND pts.PeiTaoID = pt.PeiTaoID
		)
	AND pt.valid = 1
	AND pt.PeiTaoID = @PeiTaoID
UNION
SELECT pt.*
	,p.ProjectName
	,c1.CodeName as PeiTaoCodeName
    ,c2.ChineseName as TenantName
FROM FxtData_Industry.dbo.Dat_PeiTao_Industry_sub pt WITH (NOLOCK)
left join FxtData_Industry.dbo.Dat_Project_Industry p on pt.ProjectId = p.ProjectId
left join FxtDataCenter.dbo.SYS_Code c1 on pt.PeiTaoCode = c1.Code
left join fxtdatacenter.dbo.DAT_Company c2 on pt.TenantID = c2.CompanyId
WHERE pt.valid = 1
	AND pt.PeiTaoID = @PeiTaoID
	AND pt.fxtCompanyId = @fxtCompanyId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Query<DatPeiTaoIndustry>(strSql, new { PeiTaoID = peitaoId, fxtCompanyId = fxtCompanyId }).AsQueryable().FirstOrDefault();
            }
        }

        public bool IsExistIndustryPeiTao(long PeiTaoID, string PeiTaoName, long ProjectId, int cityId, int fxtCompanyId)
        {
            string ptable, ctable, btable, comId;
            Access(cityId, fxtCompanyId, out ptable, out ctable, out btable, out comId);
            if (string.IsNullOrEmpty(comId))
            {
                comId = fxtCompanyId.ToString();
            }

            var strWhere = PeiTaoID == -1 ? "" : " and pt.PeiTaoID != @PeiTaoID";

            var strSql = @"
SELECT PeiTaoID
FROM FxtData_Industry.dbo.Dat_PeiTao_Industry pt WITH (NOLOCK)
WHERE NOT EXISTS (
		SELECT PeiTaoID
		FROM FxtData_Industry.dbo.Dat_PeiTao_Industry_sub pts WITH (NOLOCK)
		WHERE pts.PeiTaoID = pt.PeiTaoID
			AND pts.cityId = pt.cityId
			AND pts.fxtCompanyId = @fxtCompanyId
			AND pts.projectId = pt.projectId
		)
	AND pt.valid = 1
	AND pt.ProjectId = @ProjectId
	AND pt.PeiTaoName = @PeiTaoName
	AND pt.CityId = @CityId
	AND pt.FxtCompanyId IN (" + comId + @")" + strWhere + @"
UNION
SELECT PeiTaoID
FROM FxtData_Industry.dbo.Dat_PeiTao_Industry_sub pt WITH (NOLOCK)
WHERE pt.valid = 1
	AND pt.ProjectId = @ProjectId
	AND pt.PeiTaoName = @peitaoName
	AND pt.CityId = @CityId
	AND pt.FxtCompanyId = @fxtCompanyId" + strWhere;

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Query<long>(strSql, new { PeiTaoID = PeiTaoID, PeiTaoName = PeiTaoName, ProjectId = ProjectId, CityId = cityId, fxtCompanyId = fxtCompanyId }).Any();
            }
        }

        public int UpdateIndustryPeiTao(DatPeiTaoIndustry peitaoIndustry, int currentCompanyId)
        {
            var strSqlMainUpdate = @"UPDATE FxtData_Industry.dbo.Dat_PeiTao_Industry WITH (ROWLOCK)
 SET PeiTaoCode = @PeiTaoCode,PeiTaoName = @PeiTaoName,Floor = @Floor,Location = @Location,BuildingArea = @BuildingArea,TenantID = @TenantID,Remarks = @Remarks,SaveDate = @SaveDate,SaveUser = @SaveUser
 WHERE PeiTaoID = @PeiTaoID and (FxtCompanyId = @FxtCompanyId or @fxtcompanyid = " + ConfigurationHelper.FxtCompanyId + ")";

            var strSqlSubAdd = @"INSERT INTO FxtData_Industry.dbo.Dat_PeiTao_Industry_Sub ([PeiTaoID],[ProjectId],[CityId],[PeiTaoCode],[PeiTaoName],[Floor],[Location],[BuildingArea],[TenantID],[Remarks],[FxtCompanyId],[CreateDate],[Creators],[SaveUser],[SaveDate])
 VALUES (@PeiTaoID,@ProjectId,@CityId,@PeiTaoCode,@PeiTaoName,@Floor,@Location,@BuildingArea,@TenantID,@Remarks,@FxtCompanyId,@CreateDate,@Creators,@SaveUser,@SaveDate)";

            var strSqlSubUpdate = @"UPDATE FxtData_Industry.dbo.Dat_PeiTao_Industry_Sub WITH (ROWLOCK)
 SET PeiTaoID = @PeiTaoID,ProjectId = @ProjectId,CityId = @CityId,PeiTaoCode = @PeiTaoCode,PeiTaoName = @PeiTaoName,Floor = @Floor,Location = @Location,BuildingArea = @BuildingArea,TenantID = @TenantID,Remarks = @Remarks,FxtCompanyId = @FxtCompanyId,SaveUser = @SaveUser,SaveDate = @SaveDate
 WHERE PeiTaoID = @PeiTaoID and fxtCompanyId = @fxtCompanyId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                peitaoIndustry.FxtCompanyId = currentCompanyId;
                var mainreturn = conn.Execute(strSqlMainUpdate, peitaoIndustry);

                if (mainreturn == 0)
                {
                    if (conn.Execute(strSqlSubUpdate, peitaoIndustry) == 0)
                    {
                        return conn.Execute(strSqlSubAdd, peitaoIndustry);
                    }
                    return 1;
                }
                return 1;
            }
        }

        public int AddIndustryPeiTao(DatPeiTaoIndustry peitaoIndustry)
        {
            var strSql = @"INSERT INTO FxtData_Industry.dbo.Dat_PeiTao_Industry ([ProjectId],[CityId],[PeiTaoCode],[PeiTaoName],[Floor],[Location],[BuildingArea],[TenantID],[Remarks],[FxtCompanyId],[CreateDate],[Creators],[SaveUser],[SaveDate])
 VALUES (@ProjectId,@CityId,@PeiTaoCode,@PeiTaoName,@Floor,@Location,@BuildingArea,@TenantID,@Remarks,@FxtCompanyId,@CreateDate,@Creators,@SaveUser,@SaveDate)";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                return conn.Execute(strSql, peitaoIndustry);
            }
        }

        public int DeleteIndustryPeiTao(DatPeiTaoIndustry peitaoIndustry, int currentCompanyId)
        {
            var strSqlMainDelete = @"UPDATE FxtData_Industry.dbo.Dat_PeiTao_Industry WITH (ROWLOCK) SET valid = 0,SaveDate = @SaveDate,SaveUser = @SaveUser
 WHERE PeiTaoID = @PeiTaoID and (FxtCompanyId = @FxtCompanyId or @fxtcompanyid = " + ConfigurationHelper.FxtCompanyId + ")";
            var strSqlSubDelete = @"UPDATE FxtData_Industry.dbo.Dat_PeiTao_Industry_Sub WITH (ROWLOCK) SET valid = 0,SaveDate = @SaveDate,SaveUser = @SaveUser
 WHERE PeiTaoID = @PeiTaoID and FxtCompanyId = @FxtCompanyId";
            var strSqlSubAdd = @"INSERT INTO FxtData_Industry.dbo.Dat_PeiTao_Industry_Sub (PeiTaoID,ProjectId,CityId,PeiTaoCode,PeiTaoName,Floor,Location,BuildingArea,TenantID,Remarks,FxtCompanyId,CreateDate,Creators,SaveUser,SaveDate,Valid)
 VALUES (@PeiTaoID,@ProjectId,@CityId,@PeiTaoCode,@PeiTaoName,@Floor,@Location,@BuildingArea,@TenantID,@Remarks,@FxtCompanyId,@CreateDate,@Creators,@SaveUser,@SaveDate,@Valid)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                peitaoIndustry.FxtCompanyId = currentCompanyId;
                var mainreturn = conn.Execute(strSqlMainDelete, peitaoIndustry);

                if (mainreturn == 0)
                {
                    if (conn.Execute(strSqlSubDelete, peitaoIndustry) == 0)
                    {
                        peitaoIndustry.Valid = 0;
                        return conn.Execute(strSqlSubAdd, peitaoIndustry);
                    }
                    return 1;
                }
                return 1;
            }
        }

        public long GetPeiTaoIdByName(string peitaoName, long projectId, int cityId, int companyId)
        {
            //var access = Access(cityId, companyId);
            //var companyIds = string.IsNullOrEmpty(access.Item4) ? companyId.ToString() : access.Item4;

            string ptable, ctable, btable, comId;
            Access(cityId, companyId, out ptable, out ctable, out btable, out comId);
            var companyIds = string.IsNullOrEmpty(comId) ? companyId.ToString() : comId;

            var strSql = @"
SELECT pt.PeiTaoID
FROM FxtData_Industry.dbo.Dat_PeiTao_Industry pt WITH (NOLOCK)
WHERE NOT EXISTS (
		SELECT PeiTaoID
		FROM FxtData_Industry.dbo.Dat_PeiTao_Industry_sub pts WITH (NOLOCK)
		WHERE pts.cityId = pt.cityId
			AND pts.fxtCompanyId = @CompanyId
			AND pts.ProjectId = pt.ProjectId
			AND pts.PeiTaoID = pt.PeiTaoID
		)
	AND pt.valid = 1
	AND pt.CityId = @CityId
	AND pt.ProjectId = @ProjectId
	AND pt.PeiTaoName = @PeiTaoName
	AND pt.FxtCompanyId IN (" + companyIds + @")
UNION
SELECT pt.PeiTaoID
FROM FxtData_Industry.dbo.Dat_PeiTao_Industry_sub pt WITH (NOLOCK)
WHERE pt.valid = 1
	AND pt.CityId = @CityId
	AND pt.FxtCompanyId = @CompanyId
	AND pt.ProjectId = @ProjectId
	AND pt.PeiTaoName = @PeiTaoName";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataIndustry))
            {
                var query = conn.Query<long>(strSql, new { ProjectId = projectId, PeiTaoName = peitaoName, CityId = cityId, CompanyId = companyId }).AsQueryable();
                return query.FirstOrDefault();
            }
        }

    }
}
