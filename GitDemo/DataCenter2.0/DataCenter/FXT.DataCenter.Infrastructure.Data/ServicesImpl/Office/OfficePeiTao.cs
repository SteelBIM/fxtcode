using System;
using System.Linq;
using System.Data;
using Dapper;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using System.Data.SqlClient;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;


namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class OfficePeiTao : IOfficePeiTao
    {
        #region access
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

        public Tuple<string, string, string, string> Access(int cityid, int fxtcompanyid)
        {
            const string sql = @"SELECT [ProjectTable],[BuildingTable],[HouseTable],[CaseTable],[QueryInfoTable],[ReportTable],[PrintTable],[HistoryTable],[QueryTaxTable],s.OfficeCompanyId as ShowCompanyId FROM FxtDataCenter.dbo.[SYS_City_Table] c with(nolock),[Privi_Company_ShowData] s with(nolock) where c.CityId=@cityid  and c.CityId=s.CityId and s.FxtCompanyId=@fxtcompanyid";

            AccessedTable accessedTable;

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                accessedTable = conn.Query<AccessedTable>(sql, new { cityid, fxtcompanyid }).AsQueryable().FirstOrDefault();
            }

            return accessedTable == null ? Tuple.Create("", "", "", "") : Tuple.Create(accessedTable.ProjectTable, accessedTable.CaseTable, accessedTable.BuildingTable, accessedTable.ShowCompanyId);
        }
        private class AccessedTable
        {
            public string ProjectTable { get; set; }
            public string CaseTable { get; set; }
            public string BuildingTable { get; set; }
            public string ShowCompanyId { get; set; }
        }
        #endregion
        public IQueryable<DatOfficePeiTao> GetOfficePeiTaos(DatOfficePeiTao datOfficePeiTao, int pageIndex, int pageSize, out int totalCount, bool self)
        {
            string ptable, ctable, btable, comId;
            Access(datOfficePeiTao.CityId ?? 0, datOfficePeiTao.FxtCompanyId ?? 0, out ptable, out ctable, out btable, out comId);
            if (string.IsNullOrEmpty(comId))
            {
                comId = datOfficePeiTao.FxtCompanyId.ToString();
            }
            if (self)
            {
                comId = datOfficePeiTao.FxtCompanyId.ToString();
            }

            var where = string.Empty;
            if (!(new[] { 0, -1 }).Contains(datOfficePeiTao.PeiTaoCode)) where += " and pt.PeiTaoCode = @PeiTaoCode";
            if (!string.IsNullOrEmpty(datOfficePeiTao.PeiTaoName)) where += " and pt.PeiTaoName like @PeiTaoName";

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
	FROM FxtData_Office.dbo.Dat_Office_PeiTao pt WITH (NOLOCK)
	WHERE 1 = 1
		AND NOT EXISTS (
			SELECT PeiTaoID
			FROM FxtData_Office.dbo.Dat_Office_PeiTao_sub pts WITH (NOLOCK)
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
	FROM FxtData_Office.dbo.Dat_Office_PeiTao_sub pt WITH (NOLOCK)
	WHERE 1 = 1
		AND pt.Valid = 1
		AND pt.CityId = @CityId
		AND pt.FxtCompanyId = @FxtCompanyId
	) PeiTaoTable
INNER JOIN (
	SELECT *
	FROM FxtData_Office.dbo.Dat_Project_Office p WITH (NOLOCK)
	WHERE NOT EXISTS (
			SELECT ProjectId
			FROM FxtData_Office.dbo.Dat_Project_Office_sub ps WITH (NOLOCK)
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
	FROM fxtdata_office.dbo.Dat_Project_Office_sub p WITH (NOLOCK)
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


            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                totalCount = conn.Query<int>(totalCountSql, new { datOfficePeiTao.FxtCompanyId, datOfficePeiTao.CityId, datOfficePeiTao.ProjectId, PeiTaoName = "%" + datOfficePeiTao.PeiTaoName + "%", datOfficePeiTao.PeiTaoCode }).FirstOrDefault();
                return conn.Query<DatOfficePeiTao>(pagenatedSql, new { datOfficePeiTao.FxtCompanyId, datOfficePeiTao.CityId, datOfficePeiTao.ProjectId, PeiTaoName = "%" + datOfficePeiTao.PeiTaoName + "%", datOfficePeiTao.PeiTaoCode }).AsQueryable();
            }
        }

        public DatOfficePeiTao GetPeiTaoById(long officePeiTaoId, int fxtCompanyId)
        {
            const string strSql = @"SELECT pt.*
	                                    ,p.ProjectName
	                                    ,c1.CodeName as PeiTaoCodeName
                                        ,c2.ChineseName as TenantName
                                    FROM FxtData_Office.dbo.Dat_Office_PeiTao pt WITH (NOLOCK)
                                    left join FxtData_Office.dbo.Dat_Project_Office p on pt.ProjectId = p.ProjectId
                                    left join FxtDataCenter.dbo.SYS_Code c1 on pt.PeiTaoCode = c1.Code
                                    left join fxtdatacenter.dbo.DAT_Company c2 on pt.TenantID = c2.CompanyId
                                    WHERE NOT EXISTS (
		                                    SELECT PeiTaoID
		                                    FROM FxtData_Office.dbo.Dat_Office_PeiTao_sub pts WITH (NOLOCK)
		                                    WHERE pts.cityId = pt.cityId
			                                    --AND pts.fxtCompanyId = pt.fxtCompanyId
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
                                    FROM FxtData_Office.dbo.Dat_Office_PeiTao_sub pt WITH (NOLOCK)
                                    left join FxtData_Office.dbo.Dat_Project_Office p on pt.ProjectId = p.ProjectId
                                    left join FxtDataCenter.dbo.SYS_Code c1 on pt.PeiTaoCode = c1.Code
                                    left join fxtdatacenter.dbo.DAT_Company c2 on pt.TenantID = c2.CompanyId
                                    WHERE pt.valid = 1
	                                    AND pt.PeiTaoID = @PeiTaoID
	                                    AND pt.fxtCompanyId = @fxtCompanyId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                return conn.Query<DatOfficePeiTao>(strSql, new { PeiTaoID = officePeiTaoId, fxtCompanyId = fxtCompanyId }).AsQueryable().FirstOrDefault();
            }
        }

        //public IQueryable<DatOfficePeiTaoTenant> GetTenantNameList(int fxtCompanyId, int cityId)
        //{
        //    string ptable, ctable, btable, comId;
        //    Access(cityId, fxtCompanyId, out ptable, out ctable, out btable, out comId);
        //    if (string.IsNullOrEmpty(comId))
        //    {
        //        comId = fxtCompanyId.ToString();
        //    }
        //    if (ptable == "" || comId == "")
        //    {
        //        return new List<DatOfficePeiTaoTenant>().AsQueryable();
        //    }

        //    string sql = "select CompanyId as TenantId,chineseName as TenantName,CityId,FxtCompanyId from fxtdatacenter.dbo.DAT_Company where CityId = @CityId and FxtCompanyId in (" + comId + ")";

        //    using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter_Role))
        //    {
        //        return conn.Query<DatOfficePeiTaoTenant>(sql, new { cityId }).AsQueryable();
        //    }
        //}

        public bool IsExistOfficePeiTao(long PeiTaoID, string PeiTaoName, long ProjectId, int cityId, int fxtCompanyId)
        {
            string ptable, ctable, btable, comId;
            Access(cityId, fxtCompanyId, out ptable, out ctable, out btable, out comId);
            if (string.IsNullOrEmpty(comId))
            {
                comId = fxtCompanyId.ToString();
            }

            var strWhere = PeiTaoID == -1 ? "" : " and pt.PeiTaoID != @PeiTaoID";

            var strSql = @" SELECT PeiTaoID
                            FROM FxtData_Office.dbo.Dat_Office_PeiTao pt WITH (NOLOCK)
                            WHERE NOT EXISTS (
		                            SELECT PeiTaoID
		                            FROM FxtData_Office.dbo.Dat_Office_PeiTao_sub pts WITH (NOLOCK)
		                            WHERE pts.PeiTaoID = pt.PeiTaoID
			                            AND pts.cityId = pt.cityId
			                            AND pts.fxtCompanyId = @fxtCompanyId
			                            AND pts.projectId = pt.projectId
		                            )
	                            AND pt.valid = 1
	                            AND pt.ProjectId = @ProjectId
	                            AND pt.PeiTaoName = @PeiTaoName
	                            AND pt.CityId = @CityId
	                            AND pt.FxtCompanyId IN ("
                            + comId + @")"
                            + strWhere + @"
                            UNION
                            SELECT PeiTaoID
                            FROM FxtData_Office.dbo.Dat_Office_PeiTao_sub pt WITH (NOLOCK)
                            WHERE pt.valid = 1
	                            AND pt.ProjectId = @ProjectId
	                            AND pt.PeiTaoName = @peitaoName
	                            AND pt.CityId = @CityId
	                            AND pt.FxtCompanyId = @fxtCompanyId"
                            + strWhere;

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                return conn.Query<long>(strSql, new { PeiTaoID = PeiTaoID, PeiTaoName = PeiTaoName, ProjectId = ProjectId, CityId = cityId, fxtCompanyId = fxtCompanyId }).Any();
            }
        }

        public int UpdateOfficePeiTao(DatOfficePeiTao datOfficePeiTao, int currentCompanyId)
        {
            var strSqlMainUpdate = @"UPDATE FxtData_Office.dbo.Dat_Office_PeiTao WITH (ROWLOCK)
                                    SET PeiTaoCode = @PeiTaoCode,PeiTaoName = @PeiTaoName,Floor = @Floor,Location = @Location,BuildingArea = @BuildingArea,TenantID = @TenantID,Remarks = @Remarks,SaveDate = @SaveDate,SaveUser = @SaveUser
                                    WHERE PeiTaoID = @PeiTaoID and (FxtCompanyId = @FxtCompanyId or @fxtcompanyid = " + ConfigurationHelper.FxtCompanyId + ")";

            var strSqlSubAdd = @"INSERT INTO FxtData_Office.dbo.Dat_Office_PeiTao_Sub ([PeiTaoID],[ProjectId],[CityId],[PeiTaoCode],[PeiTaoName],[Floor],[Location],[BuildingArea],[TenantID],[Remarks],[FxtCompanyId],[CreateDate],[Creators],[SaveUser],[SaveDate])
                                 VALUES (@PeiTaoID,@ProjectId,@CityId,@PeiTaoCode,@PeiTaoName,@Floor,@Location,@BuildingArea,@TenantID,@Remarks,@FxtCompanyId,@CreateDate,@Creators,@SaveUser,@SaveDate)";

            var strSqlSubUpdate = @"UPDATE FxtData_Office.dbo.Dat_Office_PeiTao_Sub WITH (ROWLOCK)
                                    SET PeiTaoID = @PeiTaoID,ProjectId = @ProjectId,CityId = @CityId,PeiTaoCode = @PeiTaoCode,PeiTaoName = @PeiTaoName,Floor = @Floor,Location = @Location,BuildingArea = @BuildingArea,TenantID = @TenantID,Remarks = @Remarks,FxtCompanyId = @FxtCompanyId,SaveUser = @SaveUser,SaveDate = @SaveDate
                                    WHERE PeiTaoID = @PeiTaoID and fxtCompanyId = @fxtCompanyId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                datOfficePeiTao.FxtCompanyId = currentCompanyId;
                var mainreturn = conn.Execute(strSqlMainUpdate, datOfficePeiTao);

                if (mainreturn == 0)
                {
                    if (conn.Execute(strSqlSubUpdate, datOfficePeiTao) == 0)
                    {
                        return conn.Execute(strSqlSubAdd, datOfficePeiTao);
                    }
                    return 1;
                }
                return 1;
            }
        }

        public int AddOfficePeiTao(DatOfficePeiTao datOfficePeiTao)
        {
            var strSql = @"INSERT INTO FxtData_Office.dbo.Dat_Office_PeiTao ([ProjectId],[CityId],[PeiTaoCode],[PeiTaoName],[Floor],[Location],[BuildingArea],[TenantID],[Remarks],[FxtCompanyId],[CreateDate],[Creators],[SaveUser],[SaveDate])
                           VALUES (@ProjectId,@CityId,@PeiTaoCode,@PeiTaoName,@Floor,@Location,@BuildingArea,@TenantID,@Remarks,@FxtCompanyId,@CreateDate,@Creators,@SaveUser,@SaveDate)";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                return conn.Execute(strSql, datOfficePeiTao);
            }
        }

        public int DeleteOfficePeiTao(DatOfficePeiTao datOfficePeiTao, int currentCompanyId, int ProductTypeCode)
        {
            var strSqlMainDelete = @"UPDATE FxtData_Office.dbo.Dat_Office_PeiTao WITH (ROWLOCK)
                                     SET valid = 0,SaveDate = @SaveDate,SaveUser = @SaveUser
                                     WHERE PeiTaoID = @PeiTaoID and (FxtCompanyId = @FxtCompanyId or @fxtcompanyid = " + ConfigurationHelper.FxtCompanyId + ")";
            var strSqlSubDelete = @"UPDATE FxtData_Office.dbo.Dat_Office_PeiTao_Sub WITH (ROWLOCK)
                                    SET valid = 0,SaveDate = @SaveDate,SaveUser = @SaveUser
                                    WHERE PeiTaoID = @PeiTaoID and FxtCompanyId = @FxtCompanyId";
            var strSqlSubAdd = @"INSERT INTO FxtData_Office.dbo.Dat_Office_PeiTao_Sub (PeiTaoID,ProjectId,CityId,PeiTaoCode,PeiTaoName,Floor,Location,BuildingArea,TenantID,Remarks,FxtCompanyId,CreateDate,Creators,SaveUser,SaveDate,Valid)
                                 VALUES (@PeiTaoID,@ProjectId,@CityId,@PeiTaoCode,@PeiTaoName,@Floor,@Location,@BuildingArea,@TenantID,@Remarks,@FxtCompanyId,@CreateDate,@Creators,@SaveUser,@SaveDate,@Valid)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                datOfficePeiTao.FxtCompanyId = currentCompanyId;
                var mainreturn = conn.Execute(strSqlMainDelete, datOfficePeiTao);

                if (mainreturn == 0)
                {
                    if (conn.Execute(strSqlSubDelete, datOfficePeiTao) == 0)
                    {
                        datOfficePeiTao.Valid = 0;
                        return conn.Execute(strSqlSubAdd, datOfficePeiTao);
                    }
                    return 1;
                }
                return 1;
            }
        }

        public long GetPeiTaoIdByName(string peitaoName, long projectId, int cityId, int companyId)
        {
            var access = Access(cityId, companyId);
            var companyIds = string.IsNullOrEmpty(access.Item4) ? companyId.ToString() : access.Item4;

            var strSql = @" SELECT pt.PeiTaoID
                            FROM FxtData_Office.dbo.Dat_Office_PeiTao pt WITH (NOLOCK)
                            WHERE NOT EXISTS (
		                            SELECT PeiTaoID
		                            FROM FxtData_Office.dbo.Dat_Office_PeiTao_sub pts WITH (NOLOCK)
		                            WHERE pts.cityId = pt.cityId
			                            --AND pts.fxtCompanyId = pt.fxtCompanyId
			                            AND pts.fxtCompanyId = @CompanyId
			                            AND pts.ProjectId = pt.ProjectId
			                            AND pts.PeiTaoID = pt.PeiTaoID
		                            )
	                            AND pt.valid = 1
	                            AND pt.CityId = @CityId
	                            AND pt.ProjectId = @ProjectId
	                            AND pt.PeiTaoName = @PeiTaoName
	                            AND pt.FxtCompanyId IN ("
                            + companyIds + @")
                            UNION
                            SELECT pt.PeiTaoID
                            FROM FxtData_Office.dbo.Dat_Office_PeiTao_sub pt WITH (NOLOCK)
                            WHERE pt.valid = 1
	                            AND pt.CityId = @CityId
	                            AND pt.FxtCompanyId = @CompanyId
	                            AND pt.ProjectId = @ProjectId
	                            AND pt.PeiTaoName = @PeiTaoName";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataOffice))
            {
                var query = conn.Query<long>(strSql, new { ProjectId = projectId, PeiTaoName = peitaoName, CityId = cityId, CompanyId = companyId }).AsQueryable();
                return query.FirstOrDefault();
            }
        }

    }
}
