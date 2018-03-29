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
    public class PropertyAddress : IPropertyAddress
    {
        public IQueryable<LNK_P_PAddress> GetPropertyAddress(string projectname, int cityId, int fxtCompanyId)
        {
            string ptable, comId;
            Access(cityId, fxtCompanyId, out ptable, out comId);
            if (string.IsNullOrEmpty(comId)) comId = fxtCompanyId.ToString();
            if (ptable == "")
            {
                return new List<LNK_P_PAddress>().AsQueryable();
            }
            ptable = "FXTProject." + ptable;

            string where = string.Empty;
            if (!string.IsNullOrWhiteSpace(projectname))
            {
                where += " and (ProjectName like @projectname or OtherName like @projectname)";
            }

            var strSql = @"
SELECT pa.*
	,p.AreaID
	,a.AreaName
	,p.ProjectName
    ,p.OtherName
	,p.FxtCompanyId as projectfxtcompanyid
	,p.Creator as projectcreator
FROM (
	SELECT p.ProjectId,p.ProjectName,p.OtherName,p.AreaID,p.FxtCompanyId,p.Creator
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
	SELECT p.ProjectId,p.ProjectName,p.OtherName,p.AreaID,p.Fxt_CompanyId,p.Creator
	FROM " + ptable + @"_sub p with(nolock)
	WHERE p.Valid = 1
		AND p.CityID = @cityId
		AND p.Fxt_CompanyId = @fxtCompanyId
	) p
INNER JOIN 	(
	SELECT * FROM FXTProject.dbo.LNK_P_PAddress
	WHERE cityid = @cityid
	) pa
 ON p.projectId = pa.ProjectId
LEFT JOIN FxtDataCenter.dbo.SYS_Area a WITH (NOLOCK) ON p.AreaID = a.AreaId
where 1 = 1" + where;

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<LNK_P_PAddress>(strSql, new { projectname = "%" + projectname + "%", cityId, fxtCompanyId }).AsQueryable();
            }
        }

        public LNK_P_PAddress GetPropertyAddressById(int id, int cityId, int fxtCompanyId)
        {
            string ptable, comId;
            Access(cityId, fxtCompanyId, out ptable, out comId);
            if (string.IsNullOrEmpty(comId)) comId = fxtCompanyId.ToString();
            if (ptable == "")
            {
                return new LNK_P_PAddress();
            }
            ptable = "FXTProject." + ptable;

            var strSql = @"
SELECT pa.*
	,p.AreaID
	,a.AreaName
	,p.ProjectName
    ,p.OtherName
	,p.FxtCompanyId as projectfxtcompanyid
	,p.Creator as projectcreator
FROM (
	SELECT p.ProjectId,p.ProjectName,p.OtherName,p.AreaID,p.FxtCompanyId,p.Creator
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
	SELECT p.ProjectId,p.ProjectName,p.OtherName,p.AreaID,p.Fxt_CompanyId,p.Creator
	FROM " + ptable + @"_sub p with(nolock)
	WHERE p.Valid = 1
		AND p.CityID = @cityId
		AND p.Fxt_CompanyId = @fxtCompanyId
	) p
INNER JOIN 	(
	SELECT * FROM FXTProject.dbo.LNK_P_PAddress
	WHERE cityid = @cityid
	) pa
 ON p.projectId = pa.ProjectId
LEFT JOIN FxtDataCenter.dbo.SYS_Area a WITH (NOLOCK) ON p.AreaID = a.AreaId
where 1 = 1
and Id = @id";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<LNK_P_PAddress>(strSql, new { id, cityId, fxtCompanyId }).AsQueryable().FirstOrDefault();
            }
        }

        public LNK_P_PAddress IsExistPropertyAddressByProjectid(int ProjectId, string PropertyAddress, int cityId, int fxtCompanyId)
        {
            string ptable, comId;
            Access(cityId, fxtCompanyId, out ptable, out comId);
            if (string.IsNullOrEmpty(comId)) comId = fxtCompanyId.ToString();
            if (ptable == "")
            {
                return new LNK_P_PAddress();
            }
            ptable = "FXTProject." + ptable;

            var strSql = @"
SELECT pa.*
	,p.AreaID
	,a.AreaName
	,p.ProjectName
    ,p.OtherName
	,p.FxtCompanyId as projectfxtcompanyid
	,p.Creator as projectcreator
FROM (
	SELECT p.ProjectId,p.ProjectName,p.OtherName,p.AreaID,p.FxtCompanyId,p.Creator
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
	SELECT p.ProjectId,p.ProjectName,p.OtherName,p.AreaID,p.Fxt_CompanyId,p.Creator
	FROM " + ptable + @"_sub p with(nolock)
	WHERE p.Valid = 1
		AND p.CityID = @cityId
		AND p.Fxt_CompanyId = @fxtCompanyId
	) p
INNER JOIN 	(
	SELECT * FROM FXTProject.dbo.LNK_P_PAddress
	WHERE cityid = @cityid
	) pa
 ON p.projectId = pa.ProjectId
LEFT JOIN FxtDataCenter.dbo.SYS_Area a WITH (NOLOCK) ON p.AreaID = a.AreaId
where 1 = 1
and p.ProjectId = @ProjectId
and PropertyAddress = @PropertyAddress";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<LNK_P_PAddress>(strSql, new { ProjectId, PropertyAddress, cityId, fxtCompanyId }).AsQueryable().FirstOrDefault();
            }
        }

        public int AddPropertyAddress(LNK_P_PAddress pa)
        {
            var strSql = @"insert into FXTProject.dbo.LNK_P_PAddress(CityId,fxtcompanyid,ProjectId,PropertyAddress,Creator,CreateDateTime)
values(@CityId,@fxtcompanyid,@ProjectId,@PropertyAddress,@Creator,@CreateDateTime)";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(strSql, pa);
            }
        }

        public int UpdatePropertyAddress(LNK_P_PAddress pa)
        {
            var strSql = @"update FXTProject.dbo.LNK_P_PAddress set ProjectId = @projectid,PropertyAddress = @propertyaddress,Creator = @creator,CreateDateTime = @createdatetime where Id = @id";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(strSql, pa);
            }
        }

        public int DeletePropertyAddress(int id)
        {
            var strSql = @"delete FXTProject.dbo.LNK_P_PAddress where Id = @id";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(strSql, new { id });
            }
        }
        private static void Access(int cityid, int fxtcompanyid, out string ptable, out string comId)
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
                comId = "";
            }
            else
            {
                ptable = dt.Rows[0]["ProjectTable"].ToString();
                comId = dt.Rows[0]["ShowCompanyId"].ToString();
            }
        }
    }
}
