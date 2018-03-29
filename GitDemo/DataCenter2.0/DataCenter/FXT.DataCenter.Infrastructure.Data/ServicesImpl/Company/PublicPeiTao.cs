using System.Linq;
using System.Data;
using Dapper;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class PublicPeiTao : IPublicPeiTao
    {
        public IQueryable<Dat_PeiTao> GetPeiTaoList(int cityId, int typeCode)
        {
            string strSql = @"select c.*,s.CodeName as CompanyTypeName from FxtDataCenter.dbo.DAT_Company c with(nolock)
 left join FxtDataCenter.dbo.SYS_Code s with(nolock) on s.Code = c.CompanyTypeCode where c.companyid=@companyid and c.valid=1 ";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<Dat_PeiTao>(strSql, new { cityId = cityId }).AsQueryable();
            }
        }
    }
}
