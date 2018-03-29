using System.Data.SqlClient;
using System.Linq;
using System.Data;
using Dapper;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Common.DBHelper;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class WaitBuildingProject : IWaitBuildingProject
    {

        public IQueryable<Dat_WaitProject> GetWaitProjectById(int id)
        {
            var strSql = "select * from FXTProject.dbo.DAT_waitProject with(nolock) where WaitProjectId = @WaitProjectId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<Dat_WaitProject>(strSql, new { WaitProjectId = id }).AsQueryable();
            }
        }

        public IQueryable<Dat_WaitProject> GetWaitProject(string name, int cityid, int fxtcompanyid)
        {
            string ptable, ctable, btable, comId;
            Access(cityid, fxtcompanyid, out ptable, out ctable, out btable, out comId);
            if (string.IsNullOrEmpty(comId)) comId = fxtcompanyid.ToString();

            var strSql = "select * from FXTProject.dbo.DAT_waitProject with(nolock) where cityid=@cityId and fxtcompanyid = @fxtcompanyid";
            if (!string.IsNullOrWhiteSpace(name)) strSql += " and WaitProjectName like @WaitProjectName";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<Dat_WaitProject>(strSql, new { WaitProjectName = "%" + name + "%", cityId = cityid, fxtcompanyid }).AsQueryable();
            }
        }

        public Dat_WaitProject GetSingleWaitProject(string name, int cityid, int fxtcompanyid)
        {
            var strSql = "select * from FXTProject.dbo.DAT_waitProject with(nolock) where cityid=@cityId and fxtcompanyid=@fxtCompanyId ";
            if (!string.IsNullOrWhiteSpace(name)) strSql += " and WaitProjectName = @WaitProjectName";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Query<Dat_WaitProject>(strSql, new { WaitProjectName = name, cityId = cityid, fxtCompanyId = fxtcompanyid }).FirstOrDefault();
            }
        }

        public int AddWaitProject(Dat_WaitProject wp)
        {
            var strSql = @"insert into FXTProject.dbo.Dat_WaitProject (waitprojectname,cityid,fxtcompanyid,userid,createdate) 
values(@waitprojectname,@cityid,@fxtcompanyid,@userid,@createdate);
SELECT SCOPE_IDENTITY() AS Id;";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                //int ret = conn.Execute(strSql, wp);
                //if (ret <= 0) return ret;
                //var identity = conn.Query("SELECT @@IDENTITY AS Id").Single();
                //return (int)identity.Id;
                var identity = conn.Query(strSql, wp).Single();
                return (int)identity.Id;
            }
        }

        public int UpdateWaitProject(int id, string name)
        {
            string strSql = @"update FXTProject.dbo.Dat_WaitProject with(rowlock) set waitprojectname = @waitprojectname where waitprojectid=@waitprojectid ";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(strSql, new { waitprojectid = id, waitprojectname = name });
            }
        }

        public int DeleteWaitProject(int id)
        {
            string strSql = @"delete from FXTProject.dbo.Dat_WaitProject with(rowlock)
where waitprojectid = @waitprojectid ";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return conn.Execute(strSql, new { waitprojectid = id });
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
