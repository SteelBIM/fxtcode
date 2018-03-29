using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;

namespace FXT.DataCenter.Domain.Services
{
   public interface IMenu
    {
       /// <summary>
       /// 根据菜单ID获取它的父级ID集合
       /// </summary>
       /// <param name="menuIds">菜单ID</param>
       /// <returns>父级ID集合</returns>
       List<int> GetMenusByMenuIds(params int[] menuIds);

       /// <summary>
       /// 根据角色ID、菜单ID，获取 SYS_Role_Menu对象集合
       /// </summary>
       /// <param name="roleId">角色Id</param>
       /// <param name="menuIds">菜单Id</param>
       /// <returns></returns>
       IQueryable<SYS_Role_Menu> GetRoleMenuByParams(int roleId, params int[] menuIds);

       /// <summary>
       /// 根据菜单code获取菜单信息
       /// </summary>
       /// <param name="typeCode">菜单code</param>
       /// <returns></returns>
       IQueryable<SYS_Menu> GetMenuByTypeCode(int typeCode);

       /// <summary>
       /// 根据角色ID 获取其拥有的菜单
       /// </summary>
       /// <param name="roleId">角色Id</param>
       /// <returns></returns>
       IQueryable<SYS_Menu> GetMenusByRoleId(int roleId);

       /// <summary>
       /// 获取所有的页面菜单
       /// </summary>
       /// <returns></returns>
       IQueryable<SYS_Menu> GetMenus();

       /// <summary>
       /// 获取一级菜单(土地，住宅，商业，办公等)
       /// </summary>
       /// <returns></returns>
       IQueryable<SYS_Menu> GetMainMenus();

       /// <summary>
       /// 根据页面ID获取功能列表
       /// </summary>
       /// <param name="menuId">菜单Id</param>
       /// <returns></returns>
       IQueryable<SYS_Role_Menu_Function> GetFunctionsByMenuId(int menuId);

       /// <summary>
       /// 根据角色ID，页面ID 获取其在这个页面具有的功能
       /// </summary>
       /// <param name="menuId">菜单Id</param>
       /// <param name="roleId">角色Id</param>
       /// <returns></returns>
       IQueryable<SYS_Role_Menu_Function> GetFunctionsByParams(int menuId, int roleId);

       /// <summary>
       /// 根据角色ID获取角色菜单
       /// </summary>
       /// <param name="roleId">角色Id</param>
       /// <returns></returns>
       IQueryable<SYS_Role_Menu> GetRoleMenus(int roleId);

       /// <summary>
       /// 获取所有的功能点
       /// </summary>
       /// <returns></returns>
       IQueryable<SYS_Func> GetFuncs();

       /// <summary>
       /// 给角色分配菜单
       /// </summary>
       /// <param name="fxtCompanyId">评估机构ID</param>
       /// <param name="roleId">角色Id</param>
       /// <param name="menuId">菜单Id</param>
       /// <returns></returns>
       int AddRoleMenus(int fxtCompanyId,int roleId, params int[] menuId);

       /// <summary>
       /// 给角色分配功能
       /// </summary>
       /// <param name="fxtCompanyId">评估机构ID</param>
       /// <param name="roleMenuId">角色菜单ID</param>
       /// <param name="functionCode">功能CODE</param>
       /// <returns></returns>
       int AddRoleMenuFuncs(int fxtCompanyId,int roleMenuId,int functionCode);

       /// <summary>
       /// 根据角色ID删除相关的菜单权限
       /// </summary>
       /// <param name="roleId">角色ID</param>
       /// <returns></returns>
       int DeleteRoleMenus(int roleId);

       /// <summary>
       /// 删除角色关联的功能
       /// </summary>
       /// <param name="roleId">角色ID</param>
       /// <returns></returns>
       int DeleteRoleMenusFuncs(int roleId);


    }
}
