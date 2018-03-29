using System;
using System.Linq;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class Role : IRole
    {
        public IQueryable<SYS_Role> GetRoles(int fxtcompanyid)
        {
            string strSql = "select * from FxtDataCenter.dbo.SYS_Role with(nolock) where fxtcompanyid = @fxtcompanyid";  //去掉fxtcompanyid=0，不允许评估机构可配置管理员权限           
            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<SYS_Role>(strSql, new { fxtcompanyid }).AsQueryable();
            }
        }

        public int DeleteRoleAndRoleMenuAndFucs(int id, int fxtcompanyid)
        {
            string strSql4 = @"delete from FxtDataCenter.dbo.SYS_Role with(rowlock) where id = @roleId and fxtcompanyid = @fxtcompanyid ";
            string strSql3 = @"delete from FxtDataCenter.dbo.SYS_Role_User with(rowlock) where RoleID = @roleId and fxtcompanyid = @fxtcompanyid ";
            string strSql2 = @"delete from FxtDataCenter.dbo.SYS_Role_Menu with(rowlock) where roleId = @roleId and fxtcompanyid = @fxtcompanyid ";
            string strSql1 = @"delete from FxtDataCenter.dbo.SYS_Role_Menu_Function with(rowlock) where rolemenuid in (select id from FxtDataCenter.dbo.SYS_Role_Menu with(nolock) where roleid = @roleId) and fxtcompanyid = @fxtcompanyid";

            SqlParameter[] parameter = { new SqlParameter("@roleId", SqlDbType.Int), new SqlParameter("@fxtcompanyid", SqlDbType.Int) };
            parameter[0].Value = id;
            parameter[1].Value = fxtcompanyid;
            using (var conn = new SqlConnection(ConfigurationHelper.FxtDataCenter))
            {
                conn.Open();
                //启用事务
                var tran = conn.BeginTransaction();
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.Transaction = tran;
                foreach (var p in parameter.Where(p => p != null))
                {
                    cmd.Parameters.Add(p);
                }

                try
                {
                    cmd.CommandText = strSql1;
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = strSql2;
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = strSql3;
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = strSql4;
                    cmd.ExecuteNonQuery();
                    tran.Commit();
                    return 4;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return -1;
                }
            }
        }

        public int AddRole(SYS_Role sys_role)
        {
            const string strSql = @"INSERT INTO [FxtDataCenter].[dbo].[SYS_Role]
                            ([RoleName]
                            ,[Remarks]
                            ,[FxtCompanyID])
                        VALUES
                            (@RoleName,@Remarks,@FxtCompanyID)";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Execute(strSql, sys_role);
            }
        }

        public IQueryable<SYS_Role> GetRolesBy(int fxtcompanyid, string roleName = null)
        {
            var fxtCompanyIdWhere = string.Empty;
            if (fxtcompanyid.ToString() == ConfigurationHelper.FxtCompanyId)
            {
                fxtCompanyIdWhere = " or fxtcompanyid=0";
            }

            var strSql = "select * from FxtDataCenter.dbo.SYS_Role with(nolock) where (fxtcompanyid = @fxtcompanyid " + fxtCompanyIdWhere + " ) ";
            if (!string.IsNullOrWhiteSpace(roleName))
            {
                strSql += " and rolename like @roleName";
            }

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<SYS_Role>(strSql, new { fxtcompanyid, roleName = "%" + roleName + "%" }).AsQueryable();
            }
        }

        public IQueryable<SYS_Role> GetRolesBy(int roleId, int fxtcompanyid)
        {
            var strSql = "select * from FxtDataCenter.dbo.SYS_Role with(nolock) where id = @roleId";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return conn.Query<SYS_Role>(strSql, new { roleId, fxtcompanyid }).AsQueryable();
            }
        }

        public int UpdateRole(SYS_Role sys_role)
        {
            string strSql = @"update FxtDataCenter.dbo.SYS_Role with(rowlock) set remarks = @remarks where id = @id";

            SqlParameter[] parameters = { new SqlParameter("@id", SqlDbType.Int), new SqlParameter("@remarks", SqlDbType.NVarChar, 200) };
            parameters[0].Value = sys_role.ID;
            parameters[1].Value = sys_role.Remarks;

            DBHelperSql.ConnectionString = ConfigurationHelper.FxtDataCenter;
            return DBHelperSql.ExecuteNonQuery(strSql, parameters);
        }
    }
}
