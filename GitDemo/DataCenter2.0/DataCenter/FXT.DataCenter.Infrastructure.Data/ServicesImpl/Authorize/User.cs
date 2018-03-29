using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class User : IUser
    {
        //public IQueryable<SYS_User> GetUsers(int fxtCompanyId, string userName = null)
        //{
        //    string where = string.Empty;
        //    if (!string.IsNullOrWhiteSpace(userName))
        //    {
        //        where = "and (username like @username or truename like @username)";
        //    }
        //    string strSql = string.Format(@"select * from [UserInfo] with(nolock) where companyid = @fxtcompanyid and valid = 1 {0} order by CreateDate desc", where);

        //    using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtUserCenter))
        //    {
        //        return conn.Query<SYS_User>(strSql, new { fxtCompanyId, username = "%" + userName + "%" }).AsQueryable();
        //    }
        //}

        public IQueryable<SYS_Role> GetRolesByUserName(string userName, int fxtCompanyId, int cityId)
        {
            string where = string.Empty;
            if (cityId > 0) { where = " and (CityID = @cityId or CityID = 0)"; }
            string strSql = @"
select
	CityID
	,(select CityName from FxtDataCenter.dbo.SYS_City c with(nolock) where c.CityId = sru.CityID) as CityName
	,RoleID as ID
	,(select RoleName from FxtDataCenter.dbo.SYS_Role r with(nolock) where r.ID = sru.RoleID) as RoleName
from FxtDataCenter.dbo.SYS_Role_User sru with(nolock)
where FxtCompanyID = @fxtCompanyId
and UserName = @userName" + where;

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<SYS_Role>(strSql, new { userName, fxtCompanyId, cityId }).AsQueryable();
            }
        }

//        public IQueryable<SYS_Role_User> GetCityRolesByUserName(string userName, int fxtCompanyId, int productCode)
//        {
//            string strSql = @"
//select
//	T.cityid
//	,c.CityName
//	,RoleID
//	,(select RoleName from FxtDataCenter.dbo.SYS_Role r with(nolock) where r.ID = RoleID) as RoleName
//from(
//	SELECT
//		case when CM.CityId = 0 then SR.CityID
//		when CM.CityId <> 0 and SR.CityID is not null then CM.CityId end as cityid
//		,SR.RoleID
//	FROM (
//		select T.cityid
//		from(
//			SELECT		
//				case when T1.CityId = 0 then T2.CityID
//				when T1.CityId <> 0 and T2.CityID is not null then T1.CityId end as cityid
//			FROM (
//				select distinct CityId from FxtUserCenter.dbo.CompanyProduct WITH (NOLOCK)
//				where CompanyId = @fxtcompanyid and ProductTypeCode = @productCode and Valid = 1 and OverDate > GETDATE()
//			)T1
//			left join (
//				select distinct CityId from FxtUserCenter.dbo.CompanyProduct_Module WITH (NOLOCK)
//				where CompanyId = @fxtcompanyid AND ProductTypeCode = @productCode AND Valid = 1 AND OverDate > getdate()
//			)T2 on T1.CityId = T2.CityID or T1.CityId = 0 or T2.CityID = 0
//		)T
//		where T.cityid is not null
//	)CM
//	left join (
//		select CityID,RoleID from FxtDataCenter.dbo.SYS_Role_User with(nolock)
//		where FxtCompanyID = @fxtcompanyid
//		and UserName = @userName
//	)SR on CM.CityId = SR.CityID or CM.CityId = 0 or SR.CityID = 0
//)T
//,FxtDataCenter.dbo.SYS_City c
//where T.cityid is not null and c.CityId = T.cityid";

//            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
//            {
//                return conn.Query<SYS_Role_User>(strSql, new { userName, fxtCompanyId, productCode }).AsQueryable();
//            }
//        }

        public IQueryable<SYS_Role_User> GetCityRolesByUserName(string userName, int fxtCompanyId)
        {
            string strSql = @"
select 
    CityID
    ,ISNULL((select CityName from FxtDataCenter.dbo.SYS_City c with(nolock) where c.CityId = r.CityID),'') as CityName
    ,RoleID
    ,(select RoleName from FxtDataCenter.dbo.SYS_Role r with(nolock) where r.ID = RoleID) as RoleName
from FxtDataCenter.dbo.SYS_Role_User r with(nolock)
where FxtCompanyID = @fxtcompanyid and UserName = @userName";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<SYS_Role_User>(strSql, new { userName, fxtCompanyId }).AsQueryable();
            }
        }

        public IQueryable<SYS_Menu> GetMenusByUserName(string userName, int fxtCompanyId, int cityId)
        {
            var strSql = @"
SELECT *
FROM FxtDataCenter.dbo.SYS_Menu m WITH (NOLOCK)
INNER JOIN FxtDataCenter.dbo.SYS_Role_Menu rm WITH (NOLOCK) ON rm.MenuID = m.ID
INNER JOIN FxtDataCenter.dbo.SYS_Role r WITH (NOLOCK) ON r.ID = rm.RoleID
INNER JOIN FxtDataCenter.dbo.SYS_Role_User ru WITH (NOLOCK) ON ru.RoleID = r.ID
WHERE 1 = 1 and ru.UserName = @userName
	AND ru.fxtcompanyid = @fxtCompanyId
	AND (ru.cityid = @cityId or ru.CityID = 0)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<SYS_Menu>(strSql, new { userName, fxtCompanyId, cityId }).AsQueryable();
            }
        }

        public int AddUserRoles(List<SYS_Role_User> sru)
        {
            string sql = @"INSERT INTO FxtDataCenter.dbo.SYS_Role_User(RoleID,UserName,CityID,FxtCompanyID) VALUES ({0},'{1}',{2},{3})";
            DBHelperSql.ConnectionString = ConfigurationHelper.FxtDataCenter;
            using (var conn = new SqlConnection(ConfigurationHelper.FxtDataCenter))
            {
                conn.Open();
                //启用事务
                var tran = conn.BeginTransaction();
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.Transaction = tran;

                try
                {
                    foreach (var i in sru)
                    {
                        cmd.CommandText = string.Format(sql, i.RoleID, i.UserName, i.CityID, i.FxtCompanyID);
                        cmd.ExecuteNonQuery();
                    }
                    tran.Commit();
                    return 2;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        public int DeleteUserRoles(int fxtCompanyId, string userName)
        {
            var strSql = @"delete from FxtDataCenter.dbo.SYS_Role_User with(rowlock) where UserName = @username AND FxtCompanyID = @fxtcompanyid";
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Execute(strSql, new { fxtCompanyId, userName });
            }
        }
    }
}
