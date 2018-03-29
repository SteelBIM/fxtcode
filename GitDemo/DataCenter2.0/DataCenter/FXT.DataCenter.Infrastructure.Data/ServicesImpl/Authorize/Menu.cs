using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class Menu : IMenu
    {
        public IQueryable<SYS_Menu> GetMenus()
        {
            var strSql = "select * from dbo.SYS_Menu with(nolock) where parentid !=0";

            //刘晓博2014-10-18修改
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return con.Query<SYS_Menu>(strSql).AsQueryable();
            }

        }

        public IQueryable<SYS_Menu> GetMenuByTypeCode(int modulecode)
        {
            var strSql = "select * from dbo.SYS_Menu with(nolock) where modulecode =@modulecode";

            var parameter = new SqlParameter("@modulecode", SqlDbType.Int) {Value = modulecode};
            
            //刘晓博2014-10-18修改
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return con.Query<SYS_Menu>(strSql, new {modulecode }).AsQueryable();
            }
        }

        public IQueryable<SYS_Role_Menu> GetRoleMenus(int roleId)
        {
            var strSql = "select * from dbo.SYS_Role_Menu with(nolock) where RoleID = @RoleID";

            var parameter = new SqlParameter("@RoleID", SqlDbType.Int);
            parameter.Value = roleId;
          
            //刘晓博2014-10-18修改
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return con.Query<SYS_Role_Menu>(strSql, new { RoleID = roleId }).AsQueryable();
            }
        }

        public IQueryable<SYS_Menu> GetMenusByRoleId(int roleId)
        {
            var strSql = @"select m.* from dbo.SYS_Menu m with(nolock)
                              inner join SYS_Role_Menu rm with(nolock)
                              on rm.menuid = m.Id 
                              where rm.RoleId  = @RoleID";

            //刘晓博2014-10-18修改
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return con.Query<SYS_Menu>(strSql, new { RoleID = roleId }).AsQueryable();
            }
        }


        public int AddRoleMenus(int fxtCompanyId, int roleId, params int[] menuId)
        {
            var strSql1 = @"delete from SYS_Role_Menu with(rowlock) where roleId = "+roleId;

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
                    cmd.CommandText = strSql1;
                    cmd.ExecuteNonQuery();
                    foreach (var t in menuId)
                    {
                        cmd.CommandText = GetSql2(roleId, t, fxtCompanyId);
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

        public string GetSql2(int roleId,int menuId,int fxtcompanyId) {
            return string.Format(@"INSERT INTO [FxtDataCenter].[dbo].[SYS_Role_Menu]
                               ([RoleID]
                               ,[MenuID]
                               ,[FxtCompanyID])
                         VALUES
                               ({0},{1},{2})", roleId, menuId, fxtcompanyId);
        }


        public IQueryable<SYS_Role_Menu_Function> GetFunctionsByMenuId(int menuId)
        {
            string strSql = @"select rmf.*,c.CodeName from dbo.SYS_Role_Menu_Function rmf with(nolock)
                            inner join dbo.SYS_Role_Menu rm with(nolock)
                            on rm.ID = rmf.RoleMenuID 
                            inner join dbo.SYS_Code c with(nolock)
                            on c.Code = rmf.FunctionCode
                            where rm.MenuID = @menuid ";

            //刘晓博2014-10-18修改
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return con.Query<SYS_Role_Menu_Function>(strSql, new { menuid = menuId }).AsQueryable();
            }
        }


        public IQueryable<SYS_Role_Menu_Function> GetFunctionsByParams(int menuId, int roleId)
        {
            string strSql = @"select rmf.*,c.CodeName from dbo.SYS_Role_Menu_Function rmf with(nolock) 
                            inner join dbo.SYS_Role_Menu rm with(nolock)
                            on rm.ID = rmf.RoleMenuID 
                            inner join dbo.SYS_Code c with(nolock)
                            on c.Code = rmf.FunctionCode
                            where rm.MenuID = @menuid and rm.RoleId = @roleid";

            //刘晓博2014-10-18修改
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return con.Query<SYS_Role_Menu_Function>(strSql, new { menuid = menuId, roleid = roleId }).AsQueryable();
            }
        }


        public int DeleteRoleMenus(int roleId)
        {
            var strSql = @"delete from SYS_Role_Menu with(rowlock) where roleId = @roleId";

            //刘晓博2014-10-18修改
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return con.Execute(strSql, new { roleId = roleId });
            }
        }

        public int DeleteRoleMenusFuncs(int roleId)
        {
            var strSql2 = @"delete from SYS_Role_Menu with(rowlock) where roleId = @roleId ";
            var strSql1 = @"delete from dbo.SYS_Role_Menu_Function with(rowlock)
                               where rolemenuid in (select id from dbo.SYS_Role_Menu with(nolock) where roleid = @roleId)";

            var parameter = new SqlParameter("@roleId", SqlDbType.Int);
            parameter.Value = roleId;

            using (var conn = new SqlConnection(ConfigurationHelper.FxtDataCenter))
            {

                conn.Open();
                //启用事务
                var tran = conn.BeginTransaction();
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.Transaction = tran;
                cmd.Parameters.Add(parameter);
                try
                {
                    cmd.CommandText = strSql1;
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = strSql2;
                    cmd.ExecuteNonQuery();
                    tran.Commit();

                    return 2;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return -1;
                }
            }

        }

        public IQueryable<SYS_Role_Menu> GetRoleMenuByParams(int roleId, params int[] menuIds)
        {

            var menuid = string.Join(",",menuIds);
            var strSql = string.Format(@"select * from dbo.sys_role_menu with(nolock) where roleid = {0} 
                                            and menuid in ({1})",roleId,menuid);
          
            //刘晓博2014-10-18修改
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return con.Query<SYS_Role_Menu>(strSql).AsQueryable();
            }

        }


        public int AddRoleMenuFuncs(int fxtCompanyId, int roleMenuId, int functionCode)
        {
            var strSql = @"INSERT INTO [FxtDataCenter].[dbo].[SYS_Role_Menu_Function]
                               ([RoleMenuID]
                               ,[FunctionCode]
                               ,[FxtCompanyID])
                         VALUES
                               (@RoleMenuID,@FunctionCode,@FxtCompanyID)";

            //刘晓博2014-10-18修改
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return con.Execute(strSql, new { RoleMenuID = roleMenuId, FunctionCode = functionCode, FxtCompanyID=fxtCompanyId });
            }
        }


        public IQueryable<SYS_Func> GetFuncs()
        {
            var strSql = @"select Code,CodeName from sys_code with(nolock) where ID = 1201";

            //刘晓博2014-10-18修改
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return con.Query<SYS_Func>(strSql).AsQueryable();
            }
        }


        public IQueryable<SYS_Menu> GetMainMenus()
        {
            var strSql = @"select * from SYS_Menu where ParentID =0";

            //刘晓博2014-10-18修改
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                return con.Query<SYS_Menu>(strSql).AsQueryable();
            }
        }

        public List<int> GetMenusByMenuIds(params int[] menuIds)
        {
            var menuid = string.Join(",",menuIds);

            string strSql = string.Format(@"select ParentID from dbo.SYS_Menu with(nolock) where id in ({0}) group by ParentID ", menuid);

            DBHelperSql.ConnectionString = ConfigurationHelper.FxtDataCenter;
            var dt = DBHelperSql.ExecuteDataTable(strSql);
            var result = new List<int>();
            if (dt.Rows.Count <= 0) return result;
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                result.Add(Convert.ToInt32(dt.Rows[i][0]));
            }

            return result;

        }
    }
}
