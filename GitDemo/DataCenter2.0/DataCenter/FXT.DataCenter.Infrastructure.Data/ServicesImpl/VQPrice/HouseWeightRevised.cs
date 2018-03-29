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
    public class HouseWeightRevised : IHouseWeightRevised
    {
        public IQueryable<DatWeightHouse> GetWeightHouses(int ProjectId, int BuildingId, string HouseName, int CityId, int ParentShowDataCompanyId, int ParentProductTypeCode, int type, int pageIndex, int pageSize, out int totalCount, bool self = true)
        {
            //int typecode = 1003036;
            //int parentFxtCompanyId = 0, parentProductTypeCode = 0;
            //GetFPInfo(datWeightHouse.FxtCompanyId, typecode, datWeightHouse.CityId, out parentFxtCompanyId, out parentProductTypeCode);

            var access = Access(CityId);
            var houseTable = "FXTProject." + access.Item1;
            var weightHouse = "FXTProject." + access.Item2;

            var where = string.Empty;
            if (!string.IsNullOrEmpty(HouseName)) where += " and h.HouseName like @HouseName";

            var where1 = string.Empty;
            if (type == 0) where1 += " and Weight is null ";
            if (type == 1) where1 += " and Weight is not null ";

            var strSql = @"
select * from (
    select wh.Id,wh.FxtCompanyId,wh.CityId,wh.Weight,wh.AvgPrice,h1.buildingId,h1.houseId,h1.HouseName
    from (
        SELECT h.buildingId,h.cityId,h.HouseId,h.HouseName
        FROM " + houseTable + @" h WITH (NOLOCK)
        WHERE h.valid = 1
	        AND h.BuildingId = @BuildingId  
	        AND h.CityId = @CityId 
	        And (',' + cast((SELECT showcompanyid FROM fxtdatacenter.dbo.privi_company_showdata WITH (NOLOCK) WHERE fxtcompanyid = @fxtcompanyid AND cityid = @cityid AND TypeCode = @typecode) AS VARCHAR) + ',' LIKE '%,' + cast(h.fxtcompanyid AS VARCHAR) + ',%')
	        AND NOT EXISTS (
		        SELECT hs.cityId,hs.HouseId,hs.HouseName
		        FROM " + houseTable + @"_sub hs WITH (NOLOCK)
		        WHERE h.HouseId = hs.HouseId
			        AND hs.FxtCompanyId = @fxtCompanyId
			        AND hs.CityId = h.CityId
		        ) " + where + @"
        UNION
        SELECT h.buildingId,h.cityId,h.HouseId,h.HouseName
        FROM " + houseTable + @"_sub h WITH (NOLOCK)
        WHERE h.valid = 1
	        AND h.BuildingId = @BuildingId
	        AND h.CityId = @CityId
	        AND h.FxtCompanyId = @fxtCompanyId
            " + where + @"
    ) h1 
    left join " + weightHouse + @" wh with(nolock)
    on wh.HouseId = h1.HouseId
        and wh.buildingId= h1.buildingId 
        and wh.CityId = h1.CityID
        and wh.ProjectId = @ProjectId
        and wh.BuildingId = @BuildingId
        and wh.FxtCompanyId = @configfxtCompanyId
)T
where 1 = 1" + where1;

            //分页SQL
            var pagenatedSql = @"select top " + pageSize + @" tt.*
                                from (
	                                select row_number() over (
			                                order by t.houseId desc
			                                ) rownumber
		                                ,t.*
	                                from (" + strSql + @") t ) tt
                                where tt.rownumber > (" + pageIndex + @" - 1) * " + pageSize;

            //总条数SQL
            var totalCountSql = "select count(1) from (" + strSql + ") as t1";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                totalCount = conn.Query<int>(totalCountSql, new { HouseName = "%" + HouseName + "%", ProjectId, BuildingId, CityId, fxtCompanyId = ParentShowDataCompanyId, typecode = ParentProductTypeCode, configfxtCompanyId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId) }).FirstOrDefault();
                return conn.Query<DatWeightHouse>(pagenatedSql, new { HouseName = "%" + HouseName + "%", ProjectId, BuildingId, CityId, fxtCompanyId = ParentShowDataCompanyId, typecode = ParentProductTypeCode, configfxtCompanyId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId) }).AsQueryable();
            }
        }

        public DatWeightHouse GetWeightHouse(int projectId, int buildingId, int houseId, int cityId, int ParentShowDataCompanyId, int ParentProductTypeCode)
        {
            //int typecode = 1003036;
            //int parentFxtCompanyId = 0, parentProductTypeCode = 0;
            //GetFPInfo(fxtCompanyId, typecode, cityId, out parentFxtCompanyId, out parentProductTypeCode);

            var access = Access(cityId);
            var houseTable = "FXTProject." + access.Item1;
            var weightHouse = "FXTProject." + access.Item2;

            var strSql = @"
SELECT wh.Id
	,wh.FxtCompanyId
	,wh.CityId
	,wh.ProjectId
	,wh.Weight
	,wh.AvgPrice
	,h1.buildingId
	,h1.houseId
	,h1.HouseName
FROM (
	SELECT h.buildingId
		,h.cityId
		,h.HouseId
		,h.HouseName
	FROM " + houseTable + @" h WITH (NOLOCK)
	WHERE h.valid = 1
		AND h.BuildingId = @BuildingId
		AND h.CityId = @CityId
		AND (',' + cast((SELECT showcompanyid FROM fxtdatacenter.dbo.privi_company_showdata WITH (NOLOCK) WHERE fxtcompanyid = @fxtcompanyid AND cityid = @cityid AND TypeCode = @typecode) AS VARCHAR) + ',' LIKE '%,' + cast(h.fxtcompanyid AS VARCHAR) + ',%')
		AND NOT EXISTS (
			SELECT h.cityId
				,h.HouseId
				,h.HouseName
			FROM " + houseTable + @"_sub hs WITH (NOLOCK)
			WHERE h.HouseId = hs.HouseId
				AND hs.FxtCompanyId = @fxtCompanyId
				AND hs.CityId = h.CityId
			)	
	UNION	
	SELECT h.buildingId
		,h.cityId
		,h.HouseId
		,h.HouseName
	FROM " + houseTable + @"_sub h WITH (NOLOCK)
	WHERE h.valid = 1
		AND h.BuildingId = @BuildingId
		AND h.CityId = @CityId
		AND h.FxtCompanyId = @fxtCompanyId
	) h1
LEFT JOIN " + weightHouse + @" wh WITH (NOLOCK) ON wh.HouseId = h1.HouseId
	AND wh.buildingId = h1.buildingId
	AND wh.CityId = h1.CityID
	AND wh.ProjectId = @ProjectId
	AND wh.FxtCompanyId = @configfxtCompanyId
WHERE h1.buildingId = @buildingId
	AND h1.houseId = @houseId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<DatWeightHouse>(strSql, new { projectId, buildingId, houseId, cityId, fxtCompanyId = ParentShowDataCompanyId, typecode = ParentProductTypeCode, configfxtCompanyId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId) }).AsQueryable().FirstOrDefault();
            }
        }

        public int UpdateWeightHouse(DatWeightHouse datWeightHouse)
        {
            var access = Access(datWeightHouse.CityId);
            var weightHouse = access.Item2;

            var strSql = @"
update " + weightHouse + @" with(rowlock) set 
weight = @weight
,avgprice = @avgprice
,EvaluationCompanyId=@EvaluationCompanyId
,updatedate = getdate()
,updateuser = @updateuser
where id = @id";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(strSql, datWeightHouse);
            }
        }

        public int AddWeightHouse(DatWeightHouse datWeightHouse)
        {
            var access = Access(datWeightHouse.CityId);
            var weightHouse = access.Item2;

            var strSql = @"insert into " + weightHouse + @"(fxtcompanyid,cityid,projectid,buildingid,houseid,weight,avgprice,updatedate,EvaluationCompanyId,updateuser) 
values(@fxtcompanyid,@cityid,@projectid,@buildingid,@houseid,@weight,@avgprice,getdate(),@EvaluationCompanyId,@updateuser)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(strSql, datWeightHouse);
            }
        }

        public Tuple<string, string> Access(int cityid)
        {
            const string sql = @"SELECT HouseTable,WeightHouse FROM FxtDataCenter.dbo.SYS_City_Table WHERE CityId = @cityid";

            AccessedTable accessedTable;
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                accessedTable = conn.Query<AccessedTable>(sql, new { cityid }).AsQueryable().FirstOrDefault();
            }
            return accessedTable == null ? Tuple.Create("", "") : Tuple.Create(accessedTable.HouseTable, accessedTable.WeightHouse);
        }

        private class AccessedTable
        {
            public string HouseTable { get; set; }
            public string WeightHouse { get; set; }
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
