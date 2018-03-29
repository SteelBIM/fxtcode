using System;
using System.Data;
using System.Linq;
using Dapper;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class BuildingWeightRevised : IBuildingWeightRevised
    {

        public IQueryable<DatWeightBuilding> GetWeightBuildings(DatWeightBuilding datWeightBuilding, int pageIndex, int pageSize, out int totalCount, bool self = true)
        {
            var access = Access(datWeightBuilding.CityId, datWeightBuilding.FxtCompanyId);
            var buildingTable = access.Item1;
            var weightBuilding = access.Item4;
            var weightHouse = access.Item5;

            var companyIds = string.IsNullOrEmpty(access.Item2) || self
                ? datWeightBuilding.FxtCompanyId.ToString()
                : access.Item2;

            var where = string.Empty;
            if (!string.IsNullOrEmpty(datWeightBuilding.BuildingName)) where += " and b.BuildingName like @BuildingName";

            var where1 = string.Empty;
            if (datWeightBuilding.Type == 0) where1 += " and wb.Weight is null ";
            if (datWeightBuilding.Type == 1) where1 += " and wb.Weight is not null ";

            var strSql = @"select b1.buildingName,b1.projectId,b1.buildingId,wb.[Id],wb.[FxtCompanyId],wb.[Weight],wb.[AvgPrice],
(select count(1) from FXTProject." + weightHouse + @" h with(nolock) where h.buildingId=wb.buildingId  and h.cityId=wb.cityId and h.fxtCompanyId= wb.[FxtCompanyId]) as HouseNum
from (select b.CityID,b.FxtCompanyId,b.projectId,b.BuildingId,b.BuildingName 
            from FXTProject." + buildingTable + @" b with(nolock)
            where b.Valid = 1
            and b.CityID= @cityId
            and b.FxtCompanyId in (" + companyIds + @")
            and b.ProjectId=@ProjectId
            and not exists(select BuildingId from FXTProject." + buildingTable + @"_sub b1 with(nolock)
            where b.BuildingId = b1.BuildingId and b1.CityID=@cityId and b1.Fxt_CompanyId =@fxtCompanyId)
            " + where + @"
            union
            select b.CityID,b.Fxt_CompanyId as FxtCompanyId,b.projectId,b.BuildingId,b.BuildingName 
            from FXTProject." + buildingTable + @"_sub b with(nolock)
            where b.Valid = 1
            and b.CityID= @cityId
            and b.Fxt_CompanyId = @fxtCompanyId
            and b.ProjectId=@ProjectId
            " + where + @") b1 
left join FXTProject." + weightBuilding + @" wb with(nolock)
            on wb.BuildingId = b1.BuildingId 
            and wb.CityId = b1.CityID
            and wb.FxtCompanyId = @fxtCompanyId " + where1;

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
                totalCount = conn.Query<int>(totalCountSql, new { BuildingName = "%" + datWeightBuilding.BuildingName + "%",datWeightBuilding.ProjectId, datWeightBuilding.CityId, datWeightBuilding.FxtCompanyId }).FirstOrDefault();
                return conn.Query<DatWeightBuilding>(pagenatedSql, new { BuildingName = "%" + datWeightBuilding.BuildingName + "%", datWeightBuilding.ProjectId, datWeightBuilding.CityId, datWeightBuilding.FxtCompanyId }).AsQueryable();
            }
        }

        public DatWeightBuilding GetWeightBuilding(int projectId, int buildingId, int cityId, int fxtCompanyId)
        {
            var access = Access(cityId, fxtCompanyId);
            var buildingTable = access.Item1;
            var weightBuilding = access.Item4;
            var companyIds = string.IsNullOrEmpty(access.Item2)
                ? fxtCompanyId.ToString()
                : access.Item2;

            var strSql = @"select b1.buildingName,b1.projectId,b1.buildingId,wb.[Id],wb.[CityId],wb.[FxtCompanyId],wb.[Weight],wb.[AvgPrice] 
from (select b.CityID,b.projectId,b.BuildingId,b.BuildingName 
            from FXTProject." + buildingTable + @" b with(nolock)
            where b.Valid = 1
            and b.CityID= @cityId
            and b.FxtCompanyId in (" + companyIds + @")
            and b.ProjectId = @ProjectId
            and not exists(select BuildingId from FXTProject." + buildingTable + @"_sub b1 with(nolock)
            where b.BuildingId = b1.BuildingId and b1.CityID=@cityId and b1.Fxt_CompanyId =@fxtCompanyId)
           
            union
            select b.CityID,b.projectId,b.BuildingId,b.BuildingName 
            from FXTProject." + buildingTable + @"_sub b with(nolock)
            where b.Valid = 1
            and b.CityID= @cityId
            and b.Fxt_CompanyId = @fxtCompanyId
            and b.ProjectId = @ProjectId) b1

left join FXTProject." + weightBuilding + @" wb with(nolock)
            on wb.buildingId = b1.buildingId 
            and wb.projectId = b1.projectId
where b1.projectId = @projectId and b1.buildingId=@buildingId ";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<DatWeightBuilding>(strSql, new { projectId,buildingId,cityId, fxtCompanyId }).AsQueryable().FirstOrDefault();
            }
        }

        public long GetWeightBuildingByBuildingId(int projectId,int buildingId, int cityId, int fxtCompanyId)
        {
            var access = Access(cityId, fxtCompanyId);
            var weightBuilding = access.Item4;

            var strSql = @"SELECT [Id] FROM [FXTProject]." + weightBuilding + " with(nolock) where projectId= @projectId and buildingId = @buildingId and cityId=@cityId and fxtcompanyId=@fxtcompanyId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<long>(strSql, new { projectId,buildingId,cityId, fxtCompanyId }).FirstOrDefault();
            }
        }


        public int UpdateWeightBuilding(DatWeightBuilding datWeightBuilding)
        {
            var access = Access(datWeightBuilding.CityId, datWeightBuilding.FxtCompanyId);
            var weightBuilding = access.Item4;

            var strSql = @"update FXTProject." + weightBuilding + @" with(rowlock) set weight = @weight,avgprice = @avgprice,updatedate = getDate()
where id = @id";


            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(strSql, datWeightBuilding);
            }
        }

        public int AddWeightBuilding(DatWeightBuilding datWeightBuilding)
        {
            var access = Access(datWeightBuilding.CityId, datWeightBuilding.FxtCompanyId);
            var weightBuilding = access.Item4;

            var strSql = @"insert into FXTProject." + weightBuilding + @"(fxtcompanyid,cityid,projectid,buildingid,weight,avgprice,updatedate) 
values(@fxtcompanyid,@cityid,@projectid,@buildingid,@weight,@avgprice,getdate())";


            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(strSql, datWeightBuilding);
            }
        }

        public int DeleteWeightBuilding(int projectId,int buildingId, int cityId, int fxtCompanyId)
        {
            var access = Access(cityId, fxtCompanyId);
            var weightBuilding = access.Item4;

            var strSql = @"delete from FXTProject." + weightBuilding + " where  projectId = @projectId and buildingId=@buildingId and cityId=@cityId and fxtCompanyId=@fxtCompanyId";


            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(strSql, new { projectId, buildingId,cityId, fxtCompanyId });
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

            return accessedTable == null ? Tuple.Create("", "", "", "", "") : Tuple.Create(accessedTable.BuildingTable, accessedTable.ShowCompanyId, accessedTable.WeightProject, accessedTable.WeightBuilding, accessedTable.WeightHouse);  
        }

        private class AccessedTable
        {
            public string BuildingTable { get; set; }
            public string ShowCompanyId { get; set; }
            public string WeightProject { get; set; }
            public string WeightBuilding { get; set; }
            public string WeightHouse { get; set; }
        }

        #endregion
    }
}
