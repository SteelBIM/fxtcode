using System;
using System.Data;
using System.Linq;
using Dapper;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class ProjectWeightRevised : IProjectWeightRevised
    {

        public IQueryable<DatWeightProject> GetWeightProjects(DatWeightProject datWeightProject, int pageIndex, int pageSize, out int totalCount, bool self = true)
        {
            var access = Access(datWeightProject.CityId, datWeightProject.FxtCompanyId);
            var projectTable = access.Item1;
            var weightProject = access.Item3;
            var weightBuilding = access.Item4;

            var companyIds = string.IsNullOrEmpty(access.Item2) || self
                ? datWeightProject.FxtCompanyId.ToString()
                : access.Item2;

            var day = int.Parse(ConfigurationHelper.AvgPriceUpdatedDate);
            var point = DateTime.Now.ToString("yyyy-MM") + "-" + day + " 00:00:00";

            var where = string.Empty;
            if (!string.IsNullOrEmpty(datWeightProject.ProjectName)) where += " and p.ProjectName like @ProjectName";

            var where1 = string.Empty;
            if (datWeightProject.Type == 0) where1 += " where wp.LowLayerWeight is null ";
            if (datWeightProject.Type == 1) where1 += " where wp.LowLayerWeight is not null ";

            var strSql = @"select p1.ProjectId, p1.ProjectName,a.AreaName, wp.[Id],wp.cityId,wp.[FxtCompanyId],wp.[LowLayerPrice],wp.[MultiLayerPrice],wp.[SmallHighLayerPrice],wp.[HighLayerPrice],wp.[ProjectAvgPrice],(case when wp.updateDate >= '" + point + @"' then '否' else '是' end) as IsExpire
,(select count(1) from FXTProject." + weightBuilding + @" b with(nolock) where b.projectId=wp.ProjectId and b.cityId=wp.cityId and b.fxtCompanyId= wp.[FxtCompanyId]) as BuildingNum
             from ( select p.CityID,p.AreaId,p.ProjectId,p.ProjectName 
                    from FXTProject." + projectTable + @" p with(nolock)
                    where p.Valid = 1
                    and p.CityID= @cityId
                    and p.FxtCompanyId in (" + companyIds + @")
                    and not exists(select ProjectId from FXTProject." + projectTable + @"_sub p1 with(nolock)
                    where p.ProjectId = p1.ProjectId and p1.CityID=@cityId and p1.Fxt_CompanyId =@fxtCompanyId)
                    " + where + @"
                    union
                    select p.CityID,p.AreaId,p.ProjectId,p.ProjectName 
                    from FXTProject." + projectTable + @"_sub p with(nolock)
                    where p.Valid = 1
                    and p.CityID= @cityId
                    and p.Fxt_CompanyId = @fxtCompanyId
                    " + where + @") p1
            left join FXTProject." + weightProject + @" wp with(nolock)
                      on wp.ProjectId = p1.ProjectId 
                      and wp.CityId=p1.cityId
                      and wp.FxtCompanyId = @fxtCompanyId
            left join fxtdatacenter.dbo.sys_area a on a.areaId = p1.areaId " + where1;

            //分页SQL
            var pagenatedSql = @"select top " + pageSize + @" tt.*
                                from (
	                                select row_number() over (
			                                order by t.ProjectId desc
			                                ) rownumber
		                                ,t.*
	                                from (" + strSql + @") t ) tt
                                where tt.rownumber > (" + pageIndex + @" - 1) * " + pageSize;

            //总条数SQL
            var totalCountSql = "select count(1) from (" + strSql + ") as t1";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                totalCount = conn.Query<int>(totalCountSql, new { ProjectName = "%" + datWeightProject.ProjectName + "%", datWeightProject.CityId, datWeightProject.FxtCompanyId }).FirstOrDefault();
                return conn.Query<DatWeightProject>(pagenatedSql, new { ProjectName = "%" + datWeightProject.ProjectName + "%", datWeightProject.CityId, datWeightProject.FxtCompanyId }).AsQueryable();
            }
        }

        public DatWeightProject GetWeightProject(int id, int projectId, int cityId, int fxtCompanyId)
        {
            var access = Access(cityId, fxtCompanyId);
            var projectTable = access.Item1;
            var weightProject = access.Item3;
            var companyIds = string.IsNullOrEmpty(access.Item2)
                ? fxtCompanyId.ToString()
                : access.Item2;

            var strSql = @"select wp.[Id],wp.[FxtCompanyId],wp.[CityId],wp.[LowLayerWeight],wp.[MultiLayerWeight],wp.[SmallHighLayerWeight],wp.[HighLayerWeight],wp.[ProjectAvgPrice],wp.[LowLayerPrice],wp.[MultiLayerPrice],wp.[SmallHighLayerPrice],wp.[HighLayerPrice],wp.[SingleVillaPrice],wp.[PlatoonVillaPrice],wp.[MoveBackHousePrice],wp.[UpdateDate],wp.[EvaluationCompanyId],p1.ProjectId,p1.ProjectName,a.AreaName,dc.ChineseName as EvaluationCompanyName
from (select p.areaId, p.ProjectId,p.ProjectName 
            from FXTProject." + projectTable + @" p with(nolock)
            where p.Valid = 1
            and p.CityID= @cityId
            and p.FxtCompanyId in (" + companyIds + @")
            and not exists(select ProjectId from FXTProject." + projectTable + @"_sub p1 with(nolock)
            where p.ProjectId = p1.ProjectId and p1.CityID=@cityId and p1.Fxt_CompanyId =@fxtCompanyId)
            union
            select p.areaId, p.ProjectId,p.ProjectName 
            from FXTProject." + projectTable + @"_sub p with(nolock)
            where p.Valid = 1
            and p.CityID= @cityId
            and p.Fxt_CompanyId = @fxtCompanyId) p1
left join FXTProject." + weightProject + @" wp with(nolock)
          on wp.ProjectId = p1.ProjectId
          and wp.cityId=@cityId
          and wp.fxtCompanyId=@fxtCompanyId
          and wp.id=@id
left join fxtdatacenter.dbo.sys_area a with(nolock)
          on a.areaId = p1.areaId
left join FxtDataCenter.dbo.DAT_Company dc with(nolock)
          on dc.companyId= wp.EvaluationCompanyId
where p1.projectId = @projectId ";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<DatWeightProject>(strSql, new { id, projectId, cityId, fxtCompanyId }).AsQueryable().FirstOrDefault();
            }
        }

        public long GetWeightProjectByProjectId(int projectId, int cityId, int fxtCompanyId)
        {
            var access = Access(cityId, fxtCompanyId);
            var weightProject = access.Item3;

            var strSql = @"SELECT [Id] FROM [FXTProject]." + weightProject + " with(nolock) where projectId= @projectId and cityId=@cityId and fxtcompanyId=@fxtcompanyId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<long>(strSql, new { projectId, cityId, fxtCompanyId }).FirstOrDefault();

            }
        }

        public IQueryable<DatWeightProject> GetNotUpdatedAvrPriceProjects(DatWeightProject datWeightProject, bool self = true)
        {
            var day = int.Parse(ConfigurationHelper.AvgPriceUpdatedDate);
            var point = DateTime.Now.ToString("yyyy-MM") + "-" + day;

            var access = Access(datWeightProject.CityId, datWeightProject.FxtCompanyId);
            var projectTable = access.Item1;
            var weightProject = access.Item3;
            var weightBuilding = access.Item4;

            var companyIds = string.IsNullOrEmpty(access.Item2) || self
                ? datWeightProject.FxtCompanyId.ToString()
                : access.Item2;

            var strSql = @"select p1.ProjectId, p1.ProjectName,a.AreaName, wp.[Id],wp.cityId,wp.[FxtCompanyId],wp.[LowLayerPrice],wp.[MultiLayerPrice],wp.[SmallHighLayerPrice],wp.[HighLayerPrice],wp.[ProjectAvgPrice]
,(select count(1) from FXTProject." + weightBuilding + @" b with(nolock) where b.projectId=p1.ProjectId and b.cityId=wp.cityId and b.fxtCompanyId= wp.[FxtCompanyId]) as BuildingNum
             from ( select p.CityID,p.AreaId,p.ProjectId,p.ProjectName 
                    from FXTProject." + projectTable + @" p with(nolock)
                    where p.Valid = 1
                    and p.CityID= @cityId
                    and p.FxtCompanyId in (" + companyIds + @")
                    and not exists(select ProjectId from FXTProject." + projectTable + @"_sub p1 with(nolock)
                    where p.ProjectId = p1.ProjectId and p1.CityID=@cityId and p1.Fxt_CompanyId =@fxtCompanyId)
                   
                    union
                    select p.CityID,p.AreaId,p.ProjectId,p.ProjectName 
                    from FXTProject." + projectTable + @"_sub p with(nolock)
                    where p.Valid = 1
                    and p.CityID= @cityId
                    and p.Fxt_CompanyId = @fxtCompanyId) p1
            left join FXTProject." + weightProject + @" wp with(nolock)
                      on wp.ProjectId = p1.ProjectId 
                      and wp.CityId=p1.cityId
                      and wp.FxtCompanyId = @fxtCompanyId
                      and wp.updateDate < '" + point + @"'
            left join fxtdatacenter.dbo.sys_area a on a.areaId = p1.areaId ";


            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<DatWeightProject>(strSql, new { datWeightProject.CityId, datWeightProject.FxtCompanyId }).AsQueryable();
            }
        }



        public string GetProjectName(int projectId, int cityId, int fxtCompanyId)
        {
            var access = Access(cityId, fxtCompanyId);
            var projectTable = access.Item1;
            var companyIds = string.IsNullOrEmpty(access.Item2)
                ? fxtCompanyId.ToString()
                : access.Item2;

            var strSql = @"select p.ProjectName 
                    from FXTProject." + projectTable + @" p with(nolock)
                    where p.Valid = 1
                    and p.CityID= @cityId
                    and p.FxtCompanyId in (" + companyIds + @")
                    and p.ProjectId = @projectId
                    and not exists(select ProjectId from FXTProject." + projectTable + @"_sub p1 with(nolock)
                    where p.ProjectId = p1.ProjectId and p1.CityID=@cityId and p1.Fxt_CompanyId =@fxtCompanyId)
                   
                    union
                    select p.ProjectName 
                    from FXTProject." + projectTable + @"_sub p with(nolock)
                    where p.Valid = 1
                    and p.CityID= @cityId
                    and p.Fxt_CompanyId = @fxtCompanyId
                    and p.ProjectId = @projectId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<string>(strSql, new { projectId, cityId, fxtCompanyId }).FirstOrDefault();
            }
        }

        public int UpdateWeightProject(DatWeightProject datWeightProject)
        {
            var access = Access(datWeightProject.CityId, datWeightProject.FxtCompanyId);
            var weightProject = access.Item3;

            var strSql = @"update FXTProject." + weightProject + @" with(rowlock) set projectavgprice = @projectavgprice,lowlayerprice = @lowlayerprice,multilayerprice = @multilayerprice,smallhighlayerprice = @smallhighlayerprice,highlayerprice = @highlayerprice,singlevillaprice = @singlevillaprice,platoonvillaprice = @platoonvillaprice,movebackhouseprice = @movebackhouseprice,EvaluationCompanyId=@EvaluationCompanyId,updatedate = getdate()
where id = @id";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(strSql, datWeightProject);
            }
        }

        public int UpdateWeightProjectAvgPrice(DatWeightProject datWeightProject)
        {
            var access = Access(datWeightProject.CityId, datWeightProject.FxtCompanyId);
            var weightProject = access.Item3;

            var strSql = @"update FXTProject." + weightProject + @" with(rowlock) set projectavgprice = @projectavgprice,lowlayerprice = @lowlayerprice,multilayerprice = @multilayerprice,smallhighlayerprice = @smallhighlayerprice,highlayerprice = @highlayerprice,updatedate = getdate()
where ProjectId = @ProjectId and CityId=@CityId and FxtCompanyId = @FxtCompanyId";
            
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(strSql, datWeightProject);
            }
        }

        public int AddWeightProject(DatWeightProject datWeightProject)
        {
            var access = Access(datWeightProject.CityId, datWeightProject.FxtCompanyId);
            var weightProject = access.Item3;

            var strSql = @"insert into FXTProject." + weightProject + @" (fxtcompanyid,cityid,projectid,lowlayerweight,multilayerweight,smallhighlayerweight,highlayerweight,projectavgprice,lowlayerprice,multilayerprice,smallhighlayerprice,highlayerprice,singlevillaprice,platoonvillaprice,movebackhouseprice,updatedate,evaluationcompanyid) 
values(@fxtcompanyid,@cityid,@projectid,@lowlayerweight,@multilayerweight,@smallhighlayerweight,@highlayerweight,@projectavgprice,@lowlayerprice,@multilayerprice,@smallhighlayerprice,@highlayerprice,@singlevillaprice,@platoonvillaprice,@movebackhouseprice,getdate(),@evaluationcompanyid)";
            
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(strSql, datWeightProject);
            }
        }

        public int DeleteWeightProject(int projectId, int cityId, int fxtCompanyId)
        {
            var access = Access(cityId, fxtCompanyId);
            var weightProject = access.Item3;

            var strSql = @"delete from FXTProject." + weightProject + " where projectId = @projectId and cityId=@cityId and fxtCompanyId=@fxtCompanyId";
            
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(strSql, new { projectId, cityId, fxtCompanyId });
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

            return accessedTable == null ? Tuple.Create("", "", "", "", "") : Tuple.Create(accessedTable.ProjectTable, accessedTable.ShowCompanyId, accessedTable.WeightProject, accessedTable.WeightBuilding, accessedTable.WeightHouse);
        }

        private class AccessedTable
        {
            public string ProjectTable { get; set; }
            public string ShowCompanyId { get; set; }
            public string WeightProject { get; set; }
            public string WeightBuilding { get; set; }
            public string WeightHouse { get; set; }
        }

        #endregion

    }
}
