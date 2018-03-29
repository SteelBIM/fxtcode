using System;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Common.DBHelper;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class ProjectSample : IProjectSample
    {

        public IQueryable<DAT_SampleProject> GetProjectSample(DAT_SampleProject sp)
        {
            string ptable, ctable, btable, comId;
            Access(sp.CityId, sp.FxtCompanyId, out ptable, out ctable, out btable, out comId);
            if (string.IsNullOrEmpty(comId)) comId = sp.FxtCompanyId.ToString();

            ptable = "fxtproject." + ptable;
            var strSql = @"
select sp.*,p1.ProjectName,pc.CodeName as PurposeName,bc.CodeName as BuildingTypeName,a.AreaName,s.SubAreaName,p1.Creator as projectCreator,p1.FxtCompanyId as projectFxtCompanyId
from fxtproject.dbo.DAT_SampleProject sp with(nolock)
left join FxtDataCenter.dbo.SYS_Code pc on pc.Code = sp.PurposeCode
left join FxtDataCenter.dbo.SYS_Code bc on bc.Code = sp.BuildingTypeCode
left join FxtDataCenter.dbo.SYS_Area a on a.AreaId = sp.AreaId
left join FxtDataCenter.dbo.SYS_SubArea s on s.SubAreaId = sp.SubAreaId
,(select p.ProjectId,p.ProjectName,p.Creator,p.FxtCompanyId
            from " + ptable + @" p
            where p.Valid = 1
            and p.CityID= @cityId
            and p.FxtCompanyId in (" + comId + @")
            and not exists(select ProjectId from " + ptable + @"_sub p1 
            where p.ProjectId = p1.ProjectId and p1.CityID=@cityId and p1.Fxt_CompanyId =@fxtCompanyId)
            union
            select p.ProjectId,p.ProjectName,p.Creator,p.Fxt_CompanyId
            from " + ptable + @"_sub p
            where p.Valid = 1
            and p.CityID= @cityId
            and p.Fxt_CompanyId = @fxtCompanyId) p1
where p1.projectId = sp.projectId and sp.valid = 1 and  sp.cityid = @CityId and sp.fxtcompanyid=@fxtCompanyId";

            if (!(new[] { 0, -1 }).Contains(sp.AreaId)) strSql += " and sp.AreaId=@AreaId";
            if (!(new[] { 0, -1 }).Contains(sp.PurposeCode)) strSql += " and sp.PurposeCode=@PurposeCode";
            if (!(new[] { 0, -1 }).Contains(sp.BuildingTypeCode)) strSql += " and sp.BuildingTypeCode=@BuildingTypeCode";
            if (sp.BuildingDate != default(DateTime)) strSql += " and sp.BuildingDate=@BuildingDate";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<DAT_SampleProject>(strSql, sp).AsQueryable();
            }
        }

        public IQueryable<DAT_SampleProject_Weight> GetProjectSampleWeightById(int sampleProjecId, int cityId, int fxtCompanyId)
        {

            string ptable, ctable, btable, comId;
            Access(cityId, fxtCompanyId, out ptable, out ctable, out btable, out comId);
            if (string.IsNullOrEmpty(comId)) comId = fxtCompanyId.ToString();

            var strSql = @"select spw.id, spw.Weight,(select top 1 CodeName from FxtDataCenter.dbo.SYS_Code where Code = spw.BuildingTypeCode) as BuildingTypeCodeName,p.ProjectName, p1.ProjectName as SampleProjectName 
                           from [DAT_SampleProject_Weight] spw with(nolock)
                           ,(select p.ProjectId,p.ProjectName 
                                    from " + ptable + @" p
                                    where p.Valid = 1
                                    and p.CityID= @cityId
                                    and p.FxtCompanyId in (" + comId + @")
                                    and not exists(select ProjectId from " + ptable + @"_sub p1 
                                    where p.ProjectId = p1.ProjectId and p1.CityID=@cityId and p1.Fxt_CompanyId =@fxtCompanyId)
                                    union
                                    select p.ProjectId,p.ProjectName 
                                    from " + ptable + @"_sub p
                                    where p.Valid = 1
                                    and p.CityID= @cityId
                                    and p.Fxt_CompanyId = @fxtCompanyId) p
                            ,(select p.ProjectId,p.ProjectName 
                                    from " + ptable + @" p
                                    where p.Valid = 1
                                    and p.CityID= @cityId
                                    and p.FxtCompanyId in (" + comId + @")
                                    and not exists(select ProjectId from " + ptable + @"_sub p1 
                                    where p.ProjectId = p1.ProjectId and p1.CityID=@cityId and p1.Fxt_CompanyId =@fxtCompanyId)
                                    union
                                    select p.ProjectId,p.ProjectName 
                                    from " + ptable + @"_sub p
                                    where p.Valid = 1
                                    and p.CityID= @cityId
                                    and p.Fxt_CompanyId = @fxtCompanyId) p1
                          where p.projectid = spw.projectid
                          and p1.ProjectId = spw.sampleprojectid 
                          and spw.sampleprojectid = @SampleProjectId
                          and spw.FxtCompanyId=@fxtCompanyId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<DAT_SampleProject_Weight>(strSql, new { SampleProjectId = sampleProjecId, cityId, fxtCompanyId }).AsQueryable();
            }
        }

        public IQueryable<DAT_SampleProject_Weight> GetProjectSampleWeights(IQueryable<int> sampleProjecIds, int cityId, int fxtCompanyId)
        {
            var projectIds = string.Join(",", sampleProjecIds);
            if (string.IsNullOrEmpty(projectIds)) projectIds = "0";

            string ptable, ctable, btable, comId;
            Access(cityId, fxtCompanyId, out ptable, out ctable, out btable, out comId);
            if (string.IsNullOrEmpty(comId)) comId = fxtCompanyId.ToString();

            var strSql = @"
SELECT spw.Weight
	,(SELECT TOP 1 CodeName FROM FxtDataCenter.dbo.SYS_Code WHERE Code = spw.BuildingTypeCode) AS BuildingTypeCodeName
	,(select top 1 AreaName from FxtDataCenter.dbo.SYS_Area a where a.AreaId = p.AreaID) as AreaName
	,p.ProjectName
	,(select top 1 AreaName from FxtDataCenter.dbo.SYS_Area a where a.AreaId = p1.AreaID) as SampleAreaName
	,p1.ProjectName AS SampleProjectName
FROM FXTProject.dbo.DAT_SampleProject_Weight spw WITH (NOLOCK)
	,(
		SELECT p.ProjectId
			,p.ProjectName
			,p.AreaID
		FROM " + ptable + @" p
		WHERE p.Valid = 1
			AND p.CityID = @cityId
			AND p.FxtCompanyId IN (" + comId + @")
			AND NOT EXISTS (
				SELECT ProjectId
				FROM " + ptable + @"_sub p1
				WHERE p.ProjectId = p1.ProjectId
					AND p1.CityID = @cityId
					AND p1.Fxt_CompanyId = @fxtCompanyId
				)		
		UNION		
		SELECT p.ProjectId
			,p.ProjectName
			,p.AreaID
		FROM " + ptable + @"_sub p
		WHERE p.Valid = 1
			AND p.CityID = @cityId
			AND p.Fxt_CompanyId = @fxtCompanyId
		) p
	,(
		SELECT p.ProjectId
			,p.ProjectName
			,p.AreaID
		FROM " + ptable + @" p
		WHERE p.Valid = 1
			AND p.CityID = @cityId
			AND p.FxtCompanyId IN (" + comId + @")
			AND NOT EXISTS (
				SELECT ProjectId
				FROM " + ptable + @"_sub p1
				WHERE p.ProjectId = p1.ProjectId
					AND p1.CityID = @cityId
					AND p1.Fxt_CompanyId = @fxtCompanyId
				)		
		UNION		
		SELECT p.ProjectId
			,p.ProjectName
			,p.AreaID
		FROM " + ptable + @"_sub p
		WHERE p.Valid = 1
			AND p.CityID = @cityId
			AND p.Fxt_CompanyId = @fxtCompanyId
		) p1
WHERE p.projectid = spw.projectid
	AND p1.ProjectId = spw.sampleprojectid
	AND spw.FxtCompanyId = @fxtCompanyId
    and spw.sampleprojectid in (" + projectIds + ")";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<DAT_SampleProject_Weight>(strSql, new { cityId, fxtCompanyId }).AsQueryable();
            }
        }

        public IQueryable<DAT_SampleProject_Weight> GetProjectSampleWeight(int projectId, int cityId, int fxtCompanyId, int buildingTypeCode)
        {
            string ptable, ctable, btable, comId;
            Access(cityId, fxtCompanyId, out ptable, out ctable, out btable, out comId);
            if (string.IsNullOrEmpty(comId)) comId = fxtCompanyId.ToString();

            var strSql = @"select p1.ProjectName as SampleProjectName from [DAT_SampleProject_Weight] spw with(nolock)  
                             ,(select p.ProjectId,p.ProjectName 
                                    from " + ptable + @" p
                                    where p.Valid = 1
                                    and p.CityID= @cityId
                                    and p.FxtCompanyId in (" + comId + @")
                                    and not exists(select ProjectId from " + ptable + @"_sub p1 
                                    where p.ProjectId = p1.ProjectId and p1.CityID=@cityId and p1.Fxt_CompanyId =@fxtCompanyId)
                                    union
                                    select p.ProjectId,p.ProjectName 
                                    from " + ptable + @"_sub p
                                    where p.Valid = 1
                                    and p.CityID= @cityId
                                    and p.Fxt_CompanyId = @fxtCompanyId) p1
                   where p1.ProjectId = spw.sampleprojectid 
                    and spw.projectId=@projectId 
                    and spw.cityId = @cityId 
                    and spw.fxtCompanyId = @fxtCompanyId and spw.buildingTypeCode = @buildingTypeCode";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<DAT_SampleProject_Weight>(strSql, new { projectId, cityId, fxtCompanyId, buildingTypeCode }).AsQueryable();
            }
        }

        public IQueryable<DAT_SampleProject> GetProjectSample(int id, int cityid, int fxtcompanyid)
        {
            string ptable, ctable, btable, comId;
            Access(cityid, fxtcompanyid, out ptable, out ctable, out btable, out comId);
            if (string.IsNullOrEmpty(comId)) comId = fxtcompanyid.ToString();

            var strSql = @"select sp.*,p.ProjectName,pc.CodeName as PurposeName,bc.CodeName as BuildingTypeName,a.AreaName,s.SubAreaName 
from DAT_SampleProject sp with(nolock)
left join FxtDataCenter.dbo.SYS_Code pc on pc.Code = sp.PurposeCode
left join FxtDataCenter.dbo.SYS_Code bc on bc.Code = sp.BuildingTypeCode
left join FxtDataCenter.dbo.SYS_Area a on a.AreaId = sp.AreaId
left join FxtDataCenter.dbo.SYS_SubArea s on s.SubAreaId = sp.SubAreaId  
,(select p.ProjectId,p.ProjectName 
            from " + ptable + @" p
            where p.Valid = 1
            and p.CityID= @cityId
            and p.FxtCompanyId in (" + comId + @")
            and not exists(select ProjectId from " + ptable + @"_sub p1 
            where p.ProjectId = p1.ProjectId and p1.CityID=@cityId and p1.Fxt_CompanyId =@fxtCompanyId)
            union
            select p.ProjectId,p.ProjectName 
            from " + ptable + @"_sub p
            where p.Valid = 1
            and p.CityID= @cityId
            and p.Fxt_CompanyId = @fxtCompanyId) p
where p.ProjectId = sp.ProjectId and  sp.valid =1 and  sp.cityid = @CityId and sp.fxtcompanyid = @FxtCompanyId and sp.Id = @id";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<DAT_SampleProject>(strSql, new { id, CityId = cityid, FxtCompanyId = fxtcompanyid }).AsQueryable();
            }
        }


        public int AddProjectSample(DAT_SampleProject sp)
        {
            var strSql = @"insert into DAT_SampleProject(projectid,cityid,fxtcompanyid,purposecode,buildingtypecode,buildingdate,areaid,subareaid,casenumber) 
values(@projectid,@cityid,@fxtcompanyid,@purposecode,@buildingtypecode,@buildingdate,@areaid,@subareaid,@casenumber)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(strSql, sp);
            }
        }

        public int UpdateProjectSample(DAT_SampleProject sp)
        {
            var strSql = @"update DAT_SampleProject with(rowlock) set projectid = @projectid,purposecode = @purposecode,buildingtypecode = @buildingtypecode,buildingdate = @buildingdate,areaid = @areaid,subareaid = @subareaid,casenumber = @casenumber,valid = 1 where id = @id";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(strSql, sp);
            }
        }

        public int DeleteProjectSample(int id)
        {
            //var  strSql = "  delete from DAT_SampleProject_Weight with(rowlock) where sampleProjectId = (select projectId from DAT_SampleProject with(nolock) where id = @id and valid = 1)";
            //var strSql = "update DAT_SampleProject with(rowlock) set valid = 0 where id = @id";
            var strSql = "delete from DAT_SampleProject with(rowlock) where id = @id"; //直接删除，在执行的作业里，没有判断valid=0。直接删除。
            var weightStrSql = @"delete from FXTProject.dbo.DAT_SampleProject_Weight with(rowlock) where SampleProjectId in (select ProjectId from FXTProject.dbo.DAT_SampleProject with(nolock) where id = @id and valid = 1)";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                var weightResult = conn.Execute(weightStrSql, new { id });
                if (weightResult < 0)
                {
                    return weightResult;
                }
                var result = conn.Execute(strSql, new { id });
                return result;
            }
        }


        public int AddProjectSampleWeight(DAT_SampleProject_Weight spw)
        {
            var strSql = @"insert into DAT_SampleProject_Weight (cityid,fxtcompanyid,projectid,sampleprojectid,weight,buildingTypeCode) 
values(@cityid,@fxtcompanyid,@projectid,@sampleprojectid,@weight,@buildingTypeCode)";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(strSql, spw);
            }
        }

        public int UpdateProjectSampleWeight(DAT_SampleProject_Weight spw)
        {
            var strSql = @"update DAT_SampleProject_Weight set projectid = @projectid,sampleprojectid = @sampleprojectid,weight = @weight,buildingTypeCode = @buildingTypeCode where id = @id";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(strSql, spw);
            }
        }

        public int DeleteProjectSampleWeight(int id)
        {
            var strSql = @"delete from DAT_SampleProject_Weight  where id = @id";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(strSql, new { id });
            }
        }

        public int GetProjectIdByName(int areaId, string name, int cityid, int fxtcompanyid)
        {
            string ptable, ctable, btable, comId;
            Access(cityid, fxtcompanyid, out ptable, out ctable, out btable, out comId);
            if (string.IsNullOrEmpty(comId)) comId = fxtcompanyid.ToString();

            var strSql = @"select p.ProjectId
                            from " + ptable + @" p
                            where p.Valid = 1
                            and p.areaId = @areaId
                            and p.ProjectName = @projectname
                            and p.CityID= @cityId
                            and p.FxtCompanyId in (" + comId + @")
                            and not exists(select ProjectId from " + ptable + @"_sub p1 
                            where p.ProjectId = p1.ProjectId and p1.CityID=@cityId and p1.Fxt_CompanyId =@fxtCompanyId)
                            union
                            select p.ProjectId 
                            from " + ptable + @"_sub p
                            where p.Valid = 1
                            and p.areaId = @areaId
                            and p.ProjectName = @projectname
                            and p.CityID= @cityId
                            and p.Fxt_CompanyId = @fxtCompanyId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                var query = conn.Query<int>(strSql, new { projectname = name, cityid, fxtcompanyid, areaId });
                return query == null ? -1 : query.FirstOrDefault();
            }
        }

        public IQueryable<DAT_Project> GetProjects(int cityid, int fxtcompanyid)
        {
            string ptable, ctable, btable, comId;
            Access(cityid, fxtcompanyid, out ptable, out ctable, out btable, out comId);
            if (string.IsNullOrEmpty(comId)) comId = fxtcompanyid.ToString();

            var strSql = @"select p.ProjectId,p.ProjectName 
            from " + ptable + @" p
            where p.Valid = 1
            and p.CityID= @cityId
            and p.FxtCompanyId in (" + comId + @")
            and not exists(select ProjectId from " + ptable + @"_sub p1 
            where p.ProjectId = p1.ProjectId and p1.CityID=@cityId and p1.Fxt_CompanyId =@fxtCompanyId)
            union
            select p.ProjectId,p.ProjectName 
            from " + ptable + @"_sub p
            where p.Valid = 1
            and p.CityID= @cityId
            and p.Fxt_CompanyId = @fxtCompanyId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<DAT_Project>(strSql, new { cityid, fxtcompanyid }).AsQueryable();

            }
        }

        public int SampleProjectIsExit(int projectId, int cityId, int fxtCompanyId)
        {
            var strSql = @"select id
                            from DAT_SampleProject with(rowlock)
                            where CityID = @cityId
	                            and FxtCompanyId = @fxtCompanyId and projectId = @projectId";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<int>(strSql, new { cityId, fxtCompanyId, projectId }).FirstOrDefault();

            }
        }

        public DAT_Project GetProjectInfo(int projectId, int cityId, int fxtCompanyId)
        {
            string ptable, ctable, btable, comId;
            Access(cityId, fxtCompanyId, out ptable, out ctable, out btable, out comId);
            if (string.IsNullOrEmpty(comId)) comId = fxtCompanyId.ToString();

            var strSql = @"
SELECT ProjectId,AreaID,(select top 1 AreaName from FxtDataCenter.dbo.SYS_Area with(nolock) where AreaID = p.AreaID) as AreaName,PurposeCode,BuildingTypeCode,convert(nvarchar(10),EndDate,121) as EndDate,SubAreaId,(select top 1 SubAreaName from FxtDataCenter.dbo.SYS_SubArea with(nolock) where SubAreaId = p.SubAreaId) as SubAreaName
FROM " + ptable + @" p
WHERE p.Valid = 1
	AND p.ProjectId = @projectid
	AND p.CityID = @cityId
	AND p.FxtCompanyId IN (" + comId + @")
	AND NOT EXISTS (
		SELECT ProjectId
		FROM " + ptable + @"_sub p1
		WHERE p.ProjectId = p1.ProjectId
			AND p1.CityID = @cityId
			AND p1.Fxt_CompanyId = @fxtCompanyId
		)
UNION
SELECT ProjectId,AreaID,(select top 1 AreaName from FxtDataCenter.dbo.SYS_Area with(nolock) where AreaID = p.AreaID) as AreaName,PurposeCode,BuildingTypeCode,convert(nvarchar(10),EndDate,121) as EndDate,SubAreaId,(select top 1 SubAreaName from FxtDataCenter.dbo.SYS_SubArea with(nolock) where SubAreaId = p.SubAreaId) as SubAreaName
FROM " + ptable + @"_sub p
WHERE p.Valid = 1
	AND p.ProjectId = @projectid
	AND p.CityID = @cityId
	AND p.Fxt_CompanyId = @fxtCompanyId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<DAT_Project>(strSql, new { projectId, cityId, fxtCompanyId }).AsQueryable().FirstOrDefault();
            }
        }


        #region 公共方法
        private static void Access(int cityid, int fxtcompanyid, out string ptable, out string ctable, out string btable, out string comId)
        {
            var sql = @"SELECT [ProjectTable],[BuildingTable],[HouseTable],[CaseTable],[QueryInfoTable],[ReportTable],[PrintTable],[HistoryTable],[QueryTaxTable],s.ShowCompanyId FROM FxtDataCenter.dbo.[SYS_City_Table] c with(nolock),FxtDataCenter.dbo.[Privi_Company_ShowData] s with(nolock) where c.CityId=@cityid  and c.CityId=s.CityId and s.FxtCompanyId=@fxtcompanyid and typecode= 1003002";

            SqlParameter[] parameter = { 
                                           new SqlParameter("@cityid",SqlDbType.Int),
                                           new SqlParameter("@fxtcompanyid",SqlDbType.Int)
                                       };
            parameter[0].Value = cityid;
            parameter[1].Value = fxtcompanyid;

            DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
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
                comId = dt.Rows[0]["ShowCompanyId"].ToString();
            }

        }
        #endregion

    }
}
