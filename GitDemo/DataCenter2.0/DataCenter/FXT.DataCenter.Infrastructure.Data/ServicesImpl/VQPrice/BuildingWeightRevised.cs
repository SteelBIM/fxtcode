using System;
using System.Data;
using System.Linq;
using Dapper;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;
using System.Data.SqlClient;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class BuildingWeightRevised : IBuildingWeightRevised
    {
        public IQueryable<DatWeightBuilding> GetWeightBuildings(int ProjectId, string BuildingName, int CityId, int ParentShowDataCompanyId, int ParentProductTypeCode, int type, int pageIndex, int pageSize, out int totalCount, bool self = true)
        {
            //int typecode = 1003036;
            //int parentFxtCompanyId = 0, parentProductTypeCode = 0;
            //GetFPInfo(datWeightBuilding.FxtCompanyId, typecode, datWeightBuilding.CityId, out parentFxtCompanyId, out parentProductTypeCode);

            var access = Access(CityId);
            var buildingTable = "FXTProject." + access.Item1;
            var weightBuilding = "FXTProject." + access.Item2;

            var where = string.Empty;
            if (!string.IsNullOrEmpty(BuildingName)) where += " and b.BuildingName like @BuildingName";

            var where1 = string.Empty;
            if (type == 0) where1 += " and wb.Weight is null ";
            if (type == 1) where1 += " and wb.Weight is not null ";

            var strSql = @"
SELECT b1.buildingName
	,b1.projectId
	,b1.buildingId
	,wb.Id
	,wb.FxtCompanyId
	,wb.Weight
	,wb.AvgPrice
FROM (
	SELECT b.CityID
		,b.FxtCompanyId
		,b.projectId
		,b.BuildingId
		,b.BuildingName
	FROM " + buildingTable + @" b WITH (NOLOCK)
	WHERE NOT EXISTS (
			SELECT BuildingId
			FROM " + buildingTable + @"_sub b1 WITH (NOLOCK)
			WHERE b.BuildingId = b1.BuildingId
				AND b1.CityID = @cityId
				AND b1.Fxt_CompanyId = @fxtCompanyId
			)
		AND b.Valid = 1
		AND b.CityID = @cityId
		AND (',' + cast((SELECT showcompanyid FROM fxtdatacenter.dbo.privi_company_showdata WITH (NOLOCK) WHERE fxtcompanyid = @fxtcompanyid AND cityid = @cityid AND TypeCode = @typecode) AS VARCHAR) + ',' LIKE '%,' + cast(b.fxtcompanyid AS VARCHAR) + ',%')
		AND b.ProjectId = @ProjectId " + where + @"	
	UNION	
	SELECT b.CityID
		,b.Fxt_CompanyId AS FxtCompanyId
		,b.projectId
		,b.BuildingId
		,b.BuildingName
	FROM " + buildingTable + @"_sub b WITH (NOLOCK)
	WHERE b.Valid = 1
		AND b.CityID = @cityId
		AND b.Fxt_CompanyId = @fxtCompanyId
		AND b.ProjectId = @ProjectId " + where + @"
	) b1
LEFT JOIN " + weightBuilding + @" wb WITH (NOLOCK) ON wb.BuildingId = b1.BuildingId
	AND wb.CityId = b1.CityID
	AND wb.FxtCompanyId = @configfxtCompanyId" + where1;

            //分页SQL
            var pagenatedSql = @"select top " + pageSize + @" tt.*
                                from (
	                                select row_number() over (
			                                order by t.buildingId desc
			                                ) rownumber
		                                ,t.*
	                                from (" + strSql + @") t ) tt
                                where tt.rownumber > (" + pageIndex + @" - 1) * " + pageSize;

            //总条数SQL
            var totalCountSql = "select count(1) from (" + strSql + ") as t1";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                totalCount = conn.Query<int>(totalCountSql, new { BuildingName = "%" + BuildingName + "%", ProjectId, CityId, fxtCompanyId = ParentShowDataCompanyId, typecode = ParentProductTypeCode, configfxtCompanyId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId) }).FirstOrDefault();
                return conn.Query<DatWeightBuilding>(pagenatedSql, new { BuildingName = "%" + BuildingName + "%", ProjectId, CityId, fxtCompanyId = ParentShowDataCompanyId, typecode = ParentProductTypeCode, configfxtCompanyId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId) }).AsQueryable();
            }
        }

        public Tuple<string, string> Access(int cityid)
        {
            const string sql = @"SELECT BuildingTable,WeightBuilding FROM FxtDataCenter.dbo.SYS_City_Table WHERE CityId = @cityid";

            AccessedTable accessedTable;
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                accessedTable = conn.Query<AccessedTable>(sql, new { cityid }).AsQueryable().FirstOrDefault();
            }

            return accessedTable == null ? Tuple.Create("", "") : Tuple.Create(accessedTable.BuildingTable, accessedTable.WeightBuilding);
        }

        private class AccessedTable
        {
            public string BuildingTable { get; set; }
            public string WeightBuilding { get; set; }
        }

        //private static void GetFPInfo(int fxtcompanyid, int producttypecode, int cityid, out int ParentFxtCompanyId, out int ParentProductTypeCode)
        //{
        //    string sql = @"select ParentShowDataCompanyId,ParentProductTypeCode from CompanyProduct where CompanyId = @fxtcompanyid and ProductTypeCode = @producttypecode and cityid = @cityid and Valid = 1";
        //    SqlParameter[] parameter = { new SqlParameter("@fxtcompanyid", SqlDbType.Int), new SqlParameter("@producttypecode", SqlDbType.Int), new SqlParameter("@cityid", SqlDbType.Int) };
        //    parameter[0].Value = fxtcompanyid;
        //    parameter[1].Value = producttypecode;
        //    parameter[2].Value = cityid;

        //    DBHelperSql.ConnectionString = ConfigurationHelper.FxtUserCenter;
        //    var dt = DBHelperSql.ExecuteDataTable(sql, parameter);
        //    if (dt.Rows.Count == 0)
        //    {
        //        ParentFxtCompanyId = 0;
        //        ParentProductTypeCode = 0;
        //    }
        //    else
        //    {
        //        ParentFxtCompanyId = int.Parse(dt.Rows[0]["ParentShowDataCompanyId"].ToString());
        //        ParentProductTypeCode = int.Parse(dt.Rows[0]["ParentProductTypeCode"].ToString());
        //    }
        //}
    }
}
