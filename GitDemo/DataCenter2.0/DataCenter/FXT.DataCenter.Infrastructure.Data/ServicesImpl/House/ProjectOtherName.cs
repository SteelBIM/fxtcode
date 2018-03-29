using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using Dapper;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class ProjectOtherName : IProjectOtherName
    {
        public IQueryable<SYS_ProjectMatch> GetProjectMatchById(int id, int cityId, int fxtCompanyId)
        {
            string ptable, ctable, btable, comId;
            Access(cityId, fxtCompanyId, out ptable, out ctable, out btable, out comId);
            if (string.IsNullOrEmpty(comId)) comId = fxtCompanyId.ToString();
            if (ptable == "" || ctable == "")
            {
                return new List<SYS_ProjectMatch>().AsQueryable();
            }

            var strSql = @"
SELECT sp.Id
	,sp.ProjectNameId
	,sp.NetAreaName
	,sp.NetName
	,sp.CityId
	,sp.FXTCompanyId
	,p1.AreaID
	,a.AreaName
	,ISNULL(p1.ProjectName, sp.ProjectName) AS ProjectName
    ,sp.Creator
FROM (
	SELECT *
	FROM FXTProject.dbo.SYS_ProjectMatch
	WHERE cityid = @cityid
		AND FXTCompanyId = @fxtCompanyId
	) sp
INNER JOIN (
	SELECT p.ProjectId
		,p.ProjectName
		,p.AreaID
	FROM " + ptable + @" p with(nolock)
	WHERE p.CityID = @cityId
		AND p.Valid = 1
		AND p.FxtCompanyId IN (" + comId + @")
		AND NOT EXISTS (
			SELECT ProjectId
			FROM " + ptable + @"_sub p1 with(nolock)
			WHERE p.ProjectId = p1.ProjectId
				AND p1.CityID = @cityId
				AND p1.Fxt_CompanyId = @fxtCompanyId
			)	
	UNION	
	SELECT p.ProjectId
		,p.ProjectName
		,p.AreaID
	FROM " + ptable + @"_sub p with(nolock)
	WHERE p.Valid = 1
		AND p.CityID = @cityId
		AND p.Fxt_CompanyId = @fxtCompanyId
	) p1 ON p1.projectId = sp.ProjectNameId
LEFT JOIN FxtDataCenter.dbo.SYS_Area a WITH (NOLOCK) ON p1.AreaID = a.AreaId
where id=@id";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<SYS_ProjectMatch>(strSql, new { id, cityId, fxtCompanyId }).AsQueryable();
            }
        }

        public IQueryable<SYS_ProjectMatch> GetProjectMatch(SYS_ProjectMatch pm)
        {
            string ptable, ctable, btable, comId;
            Access(pm.CityId ?? 1, pm.FXTCompanyId ?? 1, out ptable, out ctable, out btable, out comId);
            if (string.IsNullOrEmpty(comId)) comId = pm.FXTCompanyId == null ? "" : pm.FXTCompanyId.ToString();
            if (ptable == "" || ctable == "")
            {
                return new List<SYS_ProjectMatch>().AsQueryable();
            }
            ptable = "FXTProject." + ptable;
            string strSql = @"
SELECT sp.Id
	,sp.ProjectNameId
	,sp.NetAreaName
	,sp.NetName
	,sp.CityId
	,sp.FXTCompanyId
	,sp.Creator
	,sp.CreateTime
	,p1.AreaID
	,a.AreaName
	,ISNULL(p1.ProjectName, sp.ProjectName) AS ProjectName
	,p1.Creator as projectCreator
    ,p1.FxtCompanyId as projectFxtCompanyId
FROM (
	SELECT *
	FROM FXTProject.dbo.SYS_ProjectMatch
	WHERE cityid = @cityid
		AND FXTCompanyId = @fxtCompanyId
	) sp
INNER JOIN (
	SELECT p.ProjectId
		,p.ProjectName
		,p.AreaID
		,p.Creator,p.FxtCompanyId
	FROM " + ptable + @" p with(nolock)
	WHERE p.CityID = @cityId
		AND p.Valid = 1
		AND p.FxtCompanyId IN (" + comId + @")
		AND NOT EXISTS (
			SELECT ProjectId
			FROM " + ptable + @"_sub p1 with(nolock)
			WHERE p.ProjectId = p1.ProjectId
				AND p1.CityID = @cityId
				AND p1.Fxt_CompanyId = @fxtCompanyId
			)	
	UNION	
	SELECT p.ProjectId
		,p.ProjectName
		,p.AreaID
		,p.Creator,p.Fxt_CompanyId
	FROM " + ptable + @"_sub p with(nolock)
	WHERE p.Valid = 1
		AND p.CityID = @cityId
		AND p.Fxt_CompanyId = @fxtCompanyId
	) p1 ON p1.projectId = sp.ProjectNameId
LEFT JOIN FxtDataCenter.dbo.SYS_Area a WITH (NOLOCK) ON p1.AreaID = a.AreaId
where sp.NetName like @NetName
or p1.ProjectName like @ProjectName";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<SYS_ProjectMatch>(strSql, new { cityid = pm.CityId, fxtcompanyid = pm.FXTCompanyId, NetName = "%" + pm.NetName + "%", ProjectName = "%" + pm.ProjectName + "%" }).AsQueryable();
            }
        }

        //        public bool IsNetAndSysExist(string netName, int cityId, int fxtCompanyId, int? id = null)
        //        {
        //            string sql = @"select * from SYS_ProjectMatch where NetName=@netName
        //and cityId = @cityId and fxtCompanyId = @fxtCompanyId" + (id == null ? "" : " and id != @id");

        //            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
        //            {
        //                var result = conn.Query<SYS_ProjectMatch>(sql, new { netName, cityId, fxtCompanyId, id });
        //                return result.Any();
        //            }
        //        }


        public int AddProjectOtherName(SYS_ProjectMatch pm)
        {
            var strSql = @"insert into FXTProject.dbo.SYS_ProjectMatch (projectnameid,netname,projectname,cityid,fxtcompanyid,NetAreaName,Creator,CreateTime) 
values(@projectnameid,@netname,@projectname,@cityid,@fxtcompanyid,@NetAreaName,@Creator,@CreateTime)";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(strSql, pm);
            }
        }

        public int UpdateProjectOtherName(SYS_ProjectMatch pm)
        {
            var strSql = @"
update FXTProject.dbo.SYS_ProjectMatch with(rowlock) 
set projectnameid = @projectnameid,netname = @netname,projectname = @projectname,cityid = @cityid,fxtcompanyid = @fxtcompanyid,NetAreaName = @netareaname,SaveUser = @saveuser,SaveTime = @savetime
where id = @id";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(strSql, pm);
            }
        }

        public SYS_ProjectMatch GetProjectMatchProjectId(string projectNetName, string netAreaName, int cityid, int fxtcompanyid)
        {
            string ptable, ctable, btable, comId;
            Access(cityid, fxtcompanyid, out ptable, out ctable, out btable, out comId);
            if (string.IsNullOrEmpty(comId)) comId = fxtcompanyid.ToString();
            if (ptable == "" || ctable == "")
            {
                return new SYS_ProjectMatch();
            }
            var strSql = @"
select top 1 sp.* from (
    select * from [FXTProject].[dbo].[SYS_ProjectMatch]
    where CityId = @cityid
    and FXTCompanyId = @fxtcompanyid
    and NetName = @projectNetName
    and NetAreaName = @netAreaName
	) sp
INNER JOIN (
	SELECT p.ProjectId
		,p.ProjectName
		,p.AreaID
	FROM " + ptable + @" p with(nolock)
	WHERE p.CityID = @cityId
		AND p.Valid = 1
		AND p.FxtCompanyId IN (" + comId + @")
		AND NOT EXISTS (
			SELECT ProjectId
			FROM " + ptable + @"_sub p1 with(nolock)
			WHERE p.ProjectId = p1.ProjectId
				AND p1.CityID = @cityId
				AND p1.Fxt_CompanyId = @fxtCompanyId
			)	
	UNION	
	SELECT p.ProjectId
		,p.ProjectName
		,p.AreaID
	FROM " + ptable + @"_sub p with(nolock)
	WHERE p.Valid = 1
		AND p.CityID = @cityId
		AND p.Fxt_CompanyId = @fxtCompanyId
	) p1 ON p1.projectId = sp.ProjectNameId
order by Id desc";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<SYS_ProjectMatch>(strSql, new { projectNetName, netAreaName, cityid, fxtcompanyid }).FirstOrDefault();
            }
        }

        public int DeleteProjectOtherName(int id)
        {
            var strSql = @"delete from SYS_ProjectMatch with(rowlock) where id = @id";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(strSql, new { id });
            }
        }

        public int DeleteProjectOtherName(string netName, string netAreaName, int cityid, int fxtcompanyid)
        {
            var strSql = @"delete from FXTProject.dbo.SYS_ProjectMatch where NetName=@netName and NetAreaName = @netAreaName and cityId = @cityId and fxtCompanyId = @fxtCompanyId";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(strSql, new { netName, netAreaName, cityid, fxtcompanyid });
            }
        }

        private static void Access(int cityid, int fxtcompanyid, out string ptable, out string ctable, out string btable, out string comId)
        {
            var sql = @"SELECT [ProjectTable],[BuildingTable],[HouseTable],[CaseTable],[QueryInfoTable],[ReportTable],[PrintTable],[HistoryTable],[QueryTaxTable],s.ShowCompanyId FROM FxtDataCenter.dbo.[SYS_City_Table] c with(nolock),FxtDataCenter.dbo.[Privi_Company_ShowData] s with(nolock) where c.CityId=@cityid  and c.CityId=s.CityId and s.FxtCompanyId=@fxtcompanyid and typecode= 1003002";

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
                comId = dt.Rows[0]["ShowCompanyId"].ToString();
            }
        }
    }
}
