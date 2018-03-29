using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;

namespace FXT.DataCenter.Domain.Services
{
    public interface IUser
    {
        ///// <summary>
        ///// 获取所有的用户
        ///// </summary>
        ///// <returns></returns>
        //IQueryable<SYS_User> GetUsers(int fxtCompanyId);

        /// <summary>
        /// 根据用户名查询用户
        /// </summary>
        /// <param name="fxtCompanyId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        //IQueryable<SYS_User> GetUsers(int fxtCompanyId, string userName = null);

        /// <summary>
        /// 根据用户名查询对应的角色
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="fxtcompanyId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        IQueryable<SYS_Role> GetRolesByUserName(string userName, int fxtcompanyId, int cityId);

        /// <summary>
        /// 根据用户名查询对应的角色
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="fxtcompanyId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        //IQueryable<SYS_Role_User> GetCityRolesByUserName(string userName, int fxtcompanyId, int productCode);

        IQueryable<SYS_Role_User> GetCityRolesByUserName(string userName, int fxtCompanyId);

        /// <summary>
        /// 根据用户名查询菜单
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="fxtcompanyId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        IQueryable<SYS_Menu> GetMenusByUserName(string userName, int fxtcompanyId, int cityId);

        /// <summary>
        /// 给用户分配角色
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="userName"></param>
        /// <param name="roleId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        int AddUserRoles(List<SYS_Role_User> sru);

        /// <summary>
        /// 删除用户对应的角色
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="fxtCompanyId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        int DeleteUserRoles(int fxtCompanyId, string userName);

    }
}
