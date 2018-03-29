using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;

namespace FXT.DataCenter.Domain.Services
{
    public interface IRole
    {

        /// <summary>
        /// 根据角色ID获取角色
        /// </summary>
        /// <returns></returns>
        IQueryable<SYS_Role> GetRolesBy(int roleId, int fxtcompanyid);

        /// <summary>
        /// 根据角色名称查询角色
        /// </summary>
        /// <returns></returns>
        IQueryable<SYS_Role> GetRolesBy(int fxtcompanyid, string roleName = null);

        /// <summary>
        /// 获取该公司下所有的角色
        /// </summary>
        /// <returns></returns>
        IQueryable<SYS_Role> GetRoles(int fxtcompanyid);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fxtcompanyid"></param>
        /// <param name="cityid"></param>
        /// <returns></returns>
        int DeleteRoleAndRoleMenuAndFucs(int id, int fxtcompanyid);

        /// <summary>
        /// 增加角色
        /// </summary>
        /// <param name="sys_role"></param>
        /// <returns></returns>
        int AddRole(SYS_Role sys_role);

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="sys_role"></param>
        /// <returns></returns>
        int UpdateRole(SYS_Role sys_role);

    }
}
