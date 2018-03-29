using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using System.Text;
using System.Net;
using FXT.DataCenter.Infrastructure.Common.Common;
using System;
using System.Configuration;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class CompanyProduct_ModuleDAL : ICompanyProduct_Module
    {
        //        public IQueryable<CompanyProduct_Module> GetAccessedModules(string companyId, string productCode, string userName)
        //        {
        //            const string sql = @"
        //select
        //	T.cityid,CreateDate,CompanyId,ProductTypeCode,ModuleCode,ParentModuleCode
        //	,(select CodeName from FXTDataCenter.dbo.SYS_Code c with(nolock) where c.Code = ModuleCode) as CodeName
        //from(
        //	SELECT		
        //		case when CM.CityId = 0 then SR.CityID
        //		when CM.CityId <> 0 and SR.CityID is not null then CM.CityId end as cityid
        //		,CreateDate,CompanyId,ProductTypeCode,ModuleCode,ParentModuleCode
        //	FROM (
        //		SELECT
        //			case when T1.CityId = 0 then T2.CityID
        //			when T1.CityId <> 0 and T2.CityID is not null then T1.CityId end as cityid
        //			,CreateDate,CompanyId,ProductTypeCode,ModuleCode,ParentModuleCode
        //		FROM (
        //			select distinct CityId from FxtUserCenter.dbo.CompanyProduct WITH (NOLOCK)
        //			where CompanyId = @companyId and ProductTypeCode = @productCode and Valid = 1 and OverDate > GETDATE()
        //		)T1
        //		left join (
        //			select CityId,CreateDate,CompanyId,ProductTypeCode,ModuleCode,ParentModuleCode
        //			from FxtUserCenter.dbo.CompanyProduct_Module WITH (NOLOCK)
        //			where CompanyId = @companyId AND ProductTypeCode = @productCode AND Valid = 1 AND OverDate > getdate()
        //		)T2 on T1.CityId = T2.CityID or T1.CityId = 0 or T2.CityID = 0
        //	)CM
        //	left join (
        //		select distinct CityID 
        //		from FxtDataCenter.dbo.SYS_Role_User with(nolock)
        //		where FxtCompanyID = @companyId
        //		and UserName = @userName
        //	)SR on CM.CityId = SR.CityID or CM.CityId = 0 or SR.CityID = 0
        //)T
        //where T.cityid is not null";

        //            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtUserCenter))
        //            {
        //                return conn.Query<CompanyProduct_Module>(sql, new { companyId, productCode, userName }).AsQueryable();
        //            }
        //        }

        public List<int> GetAccessedModules(string companyId, string userName)
        {
            const string sql = @"select distinct CityID from FxtDataCenter.dbo.SYS_Role_User with(nolock) where FxtCompanyID = @companyId and UserName = @userName";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<int>(sql, new { companyId, userName }).AsQueryable().ToList();
            }
        }

        //        public IQueryable<CompanyProduct_Module> GetAccessedCities(string companyId, string productCode, string userName)
        //        {
        //            const string sql = @"
        //select
        //	T.cityid
        //	,c.CityName 
        //	,p.ProvinceName
        //from(
        //	SELECT		
        //		case when CM.CityId = 0 then SR.CityID
        //		when CM.CityId <> 0 and SR.CityID is not null then CM.CityId end as cityid
        //	FROM (
        //		SELECT
        //			case when T1.CityId = 0 then T2.CityID
        //			when T1.CityId <> 0 and T2.CityID is not null then T1.CityId end as cityid
        //		FROM (
        //			select distinct CityId from FxtUserCenter.dbo.CompanyProduct WITH (NOLOCK)
        //			where CompanyId = @companyId and ProductTypeCode = @productCode and Valid = 1 and OverDate > GETDATE()
        //		)T1
        //		left join (
        //			select distinct CityId from FxtUserCenter.dbo.CompanyProduct_Module WITH (NOLOCK)
        //			where CompanyId = @companyId AND ProductTypeCode = @productCode AND Valid = 1 AND OverDate > getdate()
        //		)T2 on T1.CityId = T2.CityID or T1.CityId = 0 or T2.CityID = 0
        //	)CM
        //	left join (
        //		select distinct CityID from FxtDataCenter.dbo.SYS_Role_User with(nolock)
        //		where FxtCompanyID = @companyId
        //		and UserName = @userName
        //	)SR on CM.CityId = SR.CityID or CM.CityId = 0 or SR.CityID = 0
        //)T
        //,FxtDataCenter.dbo.SYS_City c
        //,FxtDataCenter.dbo.SYS_Province p
        //where T.cityid is not null and c.CityId = T.cityid and c.ProvinceId = p.ProvinceId";

        //            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtUserCenter))
        //            {
        //                return conn.Query<CompanyProduct_Module>(sql, new { companyId, productCode, userName }).AsQueryable();
        //            }
        //        }

        public List<CompanyProduct_Module> FxtUserCenterService_GetCPM(int fxtcompanyid, int productTypeCode)
        {
            string sql = @"
SELECT
	case when T1.CityId = 0 then T2.CityID
	when T1.CityId <> 0 and T2.CityID is not null then T1.CityId end as cityid
	,CreateDate,CompanyId,ProductTypeCode,ModuleCode,ParentModuleCode
FROM (
	select distinct CityId from FxtUserCenter.dbo.CompanyProduct WITH (NOLOCK)
	where CompanyId = @companyId and ProductTypeCode = @productCode and Valid = 1 and OverDate > GETDATE()
)T1
left join (
	select CityId,CreateDate,CompanyId,ProductTypeCode,ModuleCode,ParentModuleCode
	from FxtUserCenter.dbo.CompanyProduct_Module WITH (NOLOCK)
	where CompanyId = @companyId AND ProductTypeCode = @productCode AND Valid = 1 AND OverDate > getdate()
)T2 on T1.CityId = T2.CityID or T1.CityId = 0 or T2.CityID = 0";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtUserCenter))
            {
                return conn.Query<CompanyProduct_Module>(sql, new { companyId = fxtcompanyid, productCode = productTypeCode }).AsQueryable().ToList();
            }
        }

        public string GetCityName(int cityId)
        {
            const string sql = @"select CityName from FxtDataCenter.dbo.SYS_City with(nolock) where CityId = @CityId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<string>(sql, new { cityId }).FirstOrDefault();
            }

        }

        public SYS_City GetProvinceName(int cityId)
        {
            const string sql = @"select cityid,CityName,c.ProvinceId,ProvinceName from FxtDataCenter.dbo.SYS_City c with(nolock),FxtDataCenter.dbo.SYS_Province p with(nolock) where c.ProvinceId = p.ProvinceId and c.CityId = @CityId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<SYS_City>(sql, new { cityId }).FirstOrDefault();
            }
        }

        public IEnumerable<SYS_City> GetProvinceNames(IEnumerable<int> cityIds)
        {
            const string sql = @"select cityid,CityName,c.ProvinceId,ProvinceName from FxtDataCenter.dbo.SYS_City c with(nolock),FxtDataCenter.dbo.SYS_Province p with(nolock) where c.ProvinceId = p.ProvinceId and c.CityId in @cityIds";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<SYS_City>(sql, new { cityIds });
            }
        }

        public IQueryable<SYS_City> GetCities()
        {
            string sql = @"
select
	c.ProvinceId
	,c.cityid
	,c.CityName
	,p.ProvinceName
from FxtDataCenter.dbo.SYS_City c with(nolock)
left join FxtDataCenter.dbo.SYS_Province p with(nolock)
on c.ProvinceId = p.ProvinceId
order by ProvinceId,CityId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<SYS_City>(sql).AsQueryable();
            }
        }

        public string GetCodeName(int code)
        {
            const string sql = @"select CodeName from FxtDataCenter.dbo.SYS_Code with(nolock) where Code = @code";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<string>(sql, new { code }).FirstOrDefault();
            }
        }

        public Dictionary<string, string> GetCodeNames(IEnumerable<int> codes)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            const string sql = @"select Code,CodeName from FxtDataCenter.dbo.SYS_Code with(nolock) where Code in @codes";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                var dbResult = conn.Query<dynamic>(sql, new { codes = codes });
                foreach (var item in dbResult)
                {
                    result.Add(item.Code.ToString(), item.CodeName.ToString());
                }
            }
            return result;
        }

        public Dictionary<string, string> GetCityNames(List<int> cityIds)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            const string sql = @"select CityId,CityName from FxtDataCenter.dbo.SYS_City with(nolock) where CityId in @CityIds";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                var dbResult = conn.Query<dynamic>(sql, new { CityIds = cityIds });
                foreach (var item in dbResult)
                {
                    result.Add(item.CityId.ToString(), item.CityName.ToString());
                }
            }
            return result;
        }
    }
}
