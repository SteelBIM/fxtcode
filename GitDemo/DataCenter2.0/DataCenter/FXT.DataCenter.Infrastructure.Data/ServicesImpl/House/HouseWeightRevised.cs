using System;
using System.Data;
using System.Linq;
using Dapper;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class HouseWeightRevised : IHouseWeightRevised
    {

        public IQueryable<DatWeightHouse> GetWeightHouses(DatWeightHouse datWeightHouse, int pageIndex, int pageSize, out int totalCount, bool self = true)
        {
            var access = Access(datWeightHouse.CityId, datWeightHouse.FxtCompanyId);
            var houseTable = access.Item1;
            var weightHouse = access.Item5;

            var companyIds = string.IsNullOrEmpty(access.Item2) || self
                ? datWeightHouse.FxtCompanyId.ToString()
                : access.Item2;

            var where = string.Empty;
            if (!string.IsNullOrEmpty(datWeightHouse.HouseName)) where += " and h.HouseName like @HouseName";

            var where1 = string.Empty;
            if (datWeightHouse.Type == 0) where1 += " and wh.Weight is null ";
            if (datWeightHouse.Type == 1) where1 += " and wh.Weight is not null ";

            var strSql = @"select wh.[Id],wh.[FxtCompanyId],wh.[CityId],wh.[Weight],wh.[AvgPrice],h1.buildingId,h1.houseId,h1.HouseName
                            from (SELECT h.buildingId,h.cityId,h.HouseId,h.HouseName
                            FROM " + houseTable + @" h WITH (NOLOCK)
                            WHERE h.valid = 1
	                            AND h.BuildingId = @BuildingId  
	                            AND h.CityId = @CityId 
	                            And h.FxtCompanyId in (" + companyIds + @")
	                            AND NOT EXISTS (
		                            SELECT hs.cityId,hs.HouseId,hs.HouseName
		                            FROM " + houseTable + @"_sub hs WITH (NOLOCK)
		                            WHERE h.HouseId = hs.HouseId
			                            AND hs.FxtCompanyId = @FxtCompanyId
			                            AND hs.CityId = h.CityId
		                            )
                            " + where + @"

                            UNION

                            SELECT h.buildingId,h.cityId,h.HouseId,h.HouseName
                            FROM " + houseTable + @"_sub h WITH (NOLOCK)
                            WHERE h.valid = 1
	                            AND h.BuildingId = @BuildingId
	                            AND h.CityId = @CityId
	                            AND h.FxtCompanyId = @FxtCompanyId
                                        " + where + @") h1
 
                left join FXTProject." + weightHouse + @" wh with(nolock)
                                        on wh.HouseId = h1.HouseId
                                        and wh.buildingId= h1.buildingId 
                                        and wh.CityId = h1.CityID
                                        and wh.ProjectId = @ProjectId
                                        and wh.BuildingId=@BuildingId
                                        and wh.FxtCompanyId = @fxtCompanyId " + where1;

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
                totalCount = conn.Query<int>(totalCountSql, new { HouseName = "%" + datWeightHouse.HouseName + "%", datWeightHouse.ProjectId, datWeightHouse.BuildingId, datWeightHouse.CityId, datWeightHouse.FxtCompanyId }).FirstOrDefault();
                return conn.Query<DatWeightHouse>(pagenatedSql, new { HouseName = "%" + datWeightHouse.HouseName + "%", datWeightHouse.ProjectId, datWeightHouse.BuildingId, datWeightHouse.CityId, datWeightHouse.FxtCompanyId }).AsQueryable();
            }
        }

        public DatWeightHouse GetWeightHouse(int projectId, int buildingId, int houseId, int cityId, int fxtCompanyId)
        {
            var access = Access(cityId, fxtCompanyId);
            var houseTable = access.Item1;
            var weightHouse = access.Item5;

            var companyIds = string.IsNullOrEmpty(access.Item2)
                ? fxtCompanyId.ToString()
                : access.Item2;

            var strSql = @"select wh.[Id],wh.[FxtCompanyId],wh.[CityId],wh.[ProjectId],wh.[Weight],wh.[AvgPrice],h1.buildingId,h1.houseId,h1.HouseName
                            from (SELECT h.buildingId, h.cityId,h.HouseId,h.HouseName
                            FROM " + houseTable + @" h WITH (NOLOCK)
                            WHERE h.valid = 1
	                            AND h.BuildingId = @BuildingId  
	                            AND h.CityId = @CityId 
	                            And h.FxtCompanyId in (" + companyIds + @")
	                            AND NOT EXISTS (
		                            SELECT h.cityId,h.HouseId,h.HouseName
		                            FROM " + houseTable + @"_sub hs WITH (NOLOCK)
		                            WHERE h.HouseId = hs.HouseId
			                            AND hs.FxtCompanyId = @FxtCompanyId
			                            AND hs.CityId = h.CityId
		                            )
                            UNION

                            SELECT h.buildingId, h.cityId,h.HouseId,h.HouseName
                            FROM " + houseTable + @"_sub h WITH (NOLOCK)
                            WHERE h.valid = 1
	                            AND h.BuildingId = @BuildingId
	                            AND h.CityId = @CityId
	                            AND h.FxtCompanyId = @FxtCompanyId) h1
                left join FXTProject." + weightHouse + @" wh with(nolock) 
                                        on wh.HouseId = h1.HouseId
                                        and wh.buildingId= h1.buildingId 
                                        and wh.CityId = h1.CityID
                                        and wh.ProjectId = @ProjectId
                                        and wh.FxtCompanyId = @fxtCompanyId
                                        where  h1.buildingId=@buildingId and h1.houseId=@houseId ";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<DatWeightHouse>(strSql, new { projectId, buildingId, houseId, cityId, fxtCompanyId }).AsQueryable().FirstOrDefault();
            }

        }

        public long GetWeightHouseByHouseId(int projectId, int buildingId, int houseId, int cityId, int fxtCompanyId)
        {
            var access = Access(cityId, fxtCompanyId);
            var weightHouse = access.Item5;

            var strSql = @"SELECT [Id] FROM [FXTProject]." + weightHouse + " with(nolock) where projectId= @projectId and buildingId = @buildingId and houseId= @houseId and cityId=@cityId and fxtcompanyId=@fxtcompanyId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<long>(strSql, new { projectId, buildingId, houseId, cityId, fxtCompanyId }).FirstOrDefault();
            }
        }

        public int UpdateWeightHouse(DatWeightHouse datWeightHouse)
        {
            var access = Access(datWeightHouse.CityId, datWeightHouse.FxtCompanyId);
            var weightHouse = access.Item5;

            var strSql = @"update " + weightHouse + @" with(rowlock) set weight = @weight,avgprice = @avgprice,updatedate = getdate() where id = @id";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(strSql, datWeightHouse);
            }
        }

        public int AddWeightHouse(DatWeightHouse datWeightHouse)
        {
            var access = Access(datWeightHouse.CityId, datWeightHouse.FxtCompanyId);
            var weightHouse = access.Item5;

            var strSql = @"insert into " + weightHouse + @"(fxtcompanyid,cityid,projectid,buildingid,houseid,weight,avgprice,updatedate) 
values(@fxtcompanyid,@cityid,@projectid,@buildingid,@houseid,@weight,@avgprice,getdate())";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(strSql, datWeightHouse);
            }
        }

        public int DeleteWeightHouse(int projectId, int buildingId, int houseId, int cityId, int fxtCompanyId)
        {
            var access = Access(cityId, fxtCompanyId);
            var weightHouse = access.Item5;

            var strSql = @"delete from " + weightHouse + @" where projectId=@projectId and buildingId=@buildingId and houseId=@houseId and cityId=@cityId and fxtCompanyId=@fxtCompanyId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(strSql, new { });
            }
        }

        #region 公共

        public Tuple<string, string, string, string, string> Access(int cityid, int fxtcompanyid)
        {
            const string sql = @"SELECT [ProjectTable],[BuildingTable],[HouseTable],[CaseTable],[WeightProject],[WeightBuilding],[WeightHouse],s.ShowCompanyId FROM FxtDataCenter.dbo.[SYS_City_Table] c with(nolock),FxtDataCenter.dbo.[Privi_Company_ShowData] s with(nolock) where c.CityId=@cityid  and c.CityId=s.CityId and s.FxtCompanyId=@fxtcompanyid and typecode= 1003002";

            AccessedTable accessedTable;

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                accessedTable = conn.Query<AccessedTable>(sql, new { cityid, fxtcompanyid }).AsQueryable().FirstOrDefault();
            }

            return accessedTable == null ? Tuple.Create("", "", "", "", "") : Tuple.Create(accessedTable.HouseTable, accessedTable.ShowCompanyId, accessedTable.WeightProject, accessedTable.WeightBuilding, accessedTable.WeightHouse);
        }

        private class AccessedTable
        {
            public string HouseTable { get; set; }
            public string ShowCompanyId { get; set; }
            public string WeightProject { get; set; }
            public string WeightBuilding { get; set; }
            public string WeightHouse { get; set; }
        }

        #endregion
    }
}
