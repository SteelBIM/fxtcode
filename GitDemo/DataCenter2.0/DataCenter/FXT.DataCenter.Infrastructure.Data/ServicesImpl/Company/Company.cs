using System.Linq;
using System.Data;
using Dapper;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class Company : ICompany
    {

        public DAT_Company GetCompanyById(int companyId)
        {
            string strSql = @"select c.*,s.CodeName as CompanyTypeName from FxtDataCenter.dbo.DAT_Company c with(nolock)
 left join FxtDataCenter.dbo.SYS_Code s with(nolock) on s.Code = c.CompanyTypeCode where c.companyid=@companyid and c.valid=1 ";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<DAT_Company>(strSql, new { companyid = companyId }).FirstOrDefault();
            }
        }

        public IQueryable<DAT_Company> GetCompany_like(string companyName, int cityId)
        {
            string strSql = @"select c.*,s.CodeName as CompanyTypeName from FxtDataCenter.dbo.DAT_Company c with(nolock)
 left join FxtDataCenter.dbo.SYS_Code s with(nolock) on s.Code = c.CompanyTypeCode where c.cityId = @cityId and c.valid = 1";

            if (!string.IsNullOrWhiteSpace(companyName)) strSql += " and  chinesename like @companyname";
            strSql += " order by CreateDate desc";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<DAT_Company>(strSql, new { companyname = "%" + companyName + "%", cityId }).AsQueryable();
            }
        }

        public IQueryable<DAT_Company> GetCompany_office(string companyName, int cityId)
        {
            string strSql = @"select c.*,s.CodeName as CompanyTypeName from FxtDataCenter.dbo.DAT_Company c with(nolock)
 left join FxtDataCenter.dbo.SYS_Code s with(nolock) on s.Code = c.CompanyTypeCode where c.cityId = @cityId and c.valid = 1";

            if (!string.IsNullOrWhiteSpace(companyName)) strSql += " and  chinesename like @companyname";
            strSql += " order by CreateDate desc";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<DAT_Company>(strSql, new { companyName, cityId }).AsQueryable();
            }
        }


        public IQueryable<DAT_Company> GetCompanyNameList(int cityId)
        {
            var strSql = "select companyId,chineseName from FxtDataCenter.dbo.DAT_Company with(nolock) where  valid = 1 and cityId=@cityId";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<DAT_Company>(strSql, new { cityId }).AsQueryable();
            }
        }

        public int AddCompany(DAT_Company dc)
        {
            var strSql = @"insert into FxtDataCenter.dbo.DAT_Company (chinesename,englishname,companytypecode,cityid,address,telephone,fax,website,createdate,cothername,brand,fromcountry,fromcity,naturecode,industrycode,subindustrycode,scalecode,registcapital,standingcode,groupid,groupname,fxtcompanyid) 
values(@chinesename,@englishname,@companytypecode,@cityid,@address,@telephone,@fax,@website,@createdate,@cothername,@brand,@fromcountry,@fromcity,@naturecode,@industrycode,@subindustrycode,@scalecode,@registcapital,@standingcode,@groupid,@groupname,@fxtcompanyid)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                var ret = conn.Execute(strSql, dc);
                return ret;
            }
        }

        public int UpdateCompany(DAT_Company dc)
        {
            var strSql = @"update FxtDataCenter.dbo.DAT_Company with(rowlock) set chinesename = @chinesename,englishname = @englishname,companytypecode = @companytypecode,address = @address,telephone = @telephone,fax = @fax,website = @website,cothername = @cothername,brand = @brand,fromcountry = @fromcountry,fromcity = @fromcity,naturecode = @naturecode,industrycode = @industrycode,subindustrycode = @subindustrycode,scalecode = @scalecode,registcapital = @registcapital,standingcode = @standingcode,groupid = @groupid,groupname = @groupname where companyid = @companyid";


            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Execute(strSql, dc);
            }
        }

        public int DeleteCompany(int companyId)
        {
            string strSql = "Update FxtDataCenter.dbo.DAT_Company with(rowlock) set valid=0  where companyid = @companyid";


            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Execute(strSql, new { companyid = companyId });
            }
        }

        public string CompanyStatistcs(int companyId)
        {
            var strSql1 = "select COUNT(*) from dbo.DAT_Land with(nolock) where (LandOwnerId=@companyid or LandUseId = @companyid)  and valid = 1";
            var strSql2 = " select COUNT(*) from dbo.LNK_P_Company with(nolock) where CompanyId = @companyid";

            int count1, count2;


            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtLand))
            {
                count1 = conn.Query<int>(strSql1, new { companyid = companyId }).FirstOrDefault();
            }

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                count2 = conn.Query<int>(strSql2, new { companyid = companyId }).FirstOrDefault();
            }

            return count1 + "," + count2;
        }

        public bool CompanyIsExit(string chineseName, int companyId)
        {
            var strSql = "select * from FxtDataCenter.dbo.DAT_Company with(nolock) where ChineseName = @chineseName and valid = 1";
            if (companyId > 0) strSql += " and companyId!=@companyId";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<DAT_Company>(strSql, new { chineseName, companyId }).Any();
            }
        }


    }
}
