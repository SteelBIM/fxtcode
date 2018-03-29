using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Common.Common;
using FXT.DataCenter.Infrastructure.Common.CommonWeb;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using FXT.DataCenter.Infrastructure.Common.Dictionary;
using FXT.DataCenter.WebUI.Infrastructure.WebSecurity;
using Webdiyer.WebControls.Mvc;

namespace FXT.DataCenter.WebUI.Areas.Authorize.Controllers
{
    [Authorize]
    public class RoleController : BaseController
    {
        private readonly IRole _role;
        private readonly IMenu _menu;
        private readonly ILog _log;

        public RoleController(IRole role, IMenu menu, ILog log)
        {
            this._role = role;
            this._menu = menu;
            this._log = log;
        }
        //
        // GET: /Authorize/Role/

        public ActionResult Index(string RoleName, int? pageIndex)
        {
            var result = _role.GetRolesBy(Passport.Current.FxtCompanyId, RoleName);
            var role = result.ToPagedList(pageIndex ?? 1, 30);
            return View(role);
        }

        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            try
            {
                foreach (var item in ids)
                {
                    var role = _role.GetRolesBy(item, Passport.Current.FxtCompanyId).FirstOrDefault();
                    if (role.RoleName.Equals("管理员")) continue;
                    _role.DeleteRoleAndRoleMenuAndFucs(item, Passport.Current.FxtCompanyId);
                }

                //ids.ForEach(m => _role.DeleteRoleAndRoleMenuAndFucs(m, Passport.Current.FxtCompanyId, Passport.Current.CityId));

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.角色菜单, SYS_Code_Dict.操作.删除, string.Join(",", ids), "", "删除角色及关联的菜单", RequestHelper.GetIP());

                return Json(true);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Authorize/Role/Delete", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);

                return Json(false);
            }
        }

        public ActionResult Create()
        {
            int roleId = -5;//角色还没有创建
            GetPermission(roleId);
            this.ViewBag.RoleId = roleId.ToString();
            this.ViewBag.Readonly = "false";
            return View("Edit", new SYS_Role());
        }

        //新增保存
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var roleName = collection["RoleName"];
                var remark = collection["Remarks"];
                var roles2Menus = collection["Roles2Menus"];
                var funcs = collection["Funcs"];

                #region 添加角色
                var sysrole = new SYS_Role { RoleName = roleName, Remarks = remark, Valid = 1, FxtCompanyID = Passport.Current.FxtCompanyId, CityID = Passport.Current.CityId };
                var count = _role.GetRolesBy(Passport.Current.FxtCompanyId, roleName).Count();
                if (count > 0)
                {
                    return this.Back("不能添加相同的角色名！");
                }
                var result = _role.AddRole(sysrole);
                #endregion

                if (result >= 1)
                {
                    var firstOrDefault = _role.GetRolesBy(Passport.Current.FxtCompanyId, roleName).FirstOrDefault();
                    if (firstOrDefault != null)
                    {
                        var roleId = firstOrDefault.ID;

                        #region 给创建的角色分配菜单
                        if (roles2Menus == null)
                        {
                            return null;
                        }
                        var subMenus = roles2Menus.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        var _menus = new List<int>();
                        subMenus.ToList().ForEach(m => _menus.Add(int.Parse(m)));
                        var menus = _menu.GetMenusByMenuIds(_menus.ToArray());
                        menus.ToList().ForEach(_menus.Add);
                        var result1 = _menu.AddRoleMenus(Passport.Current.FxtCompanyId, roleId, _menus.ToArray());
                        #endregion

                        #region 给创建的角色分配页面上操作的功能
                        if (result1 > 0)
                        {
                            if (funcs != null)
                            {
                                var menuFuncs = funcs.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (var item in menuFuncs)
                                {
                                    var array = item.Split('#');
                                    var sysRoleMenu =
                                        _menu.GetRoleMenuByParams(roleId, int.Parse(array[0])).FirstOrDefault();
                                    if (sysRoleMenu == null) continue;
                                    var roleMenuId = sysRoleMenu.ID;
                                    _menu.AddRoleMenuFuncs(Passport.Current.FxtCompanyId, roleMenuId, int.Parse(array[1]));
                                }
                            }
                            else
                            {
                                return null;
                            }
                        }

                        #endregion

                        _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.角色菜单, SYS_Code_Dict.操作.新增, roleId.ToString(), "", "增加角色及分配菜单", RequestHelper.GetIP());
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Authorize/Role/Create", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }
        }

        //编辑
        [HttpGet]
        public ActionResult Edit(int roleId, string roleName)
        {
            if (roleName.Trim('?').Equals("管理员") && Passport.Current.FxtCompanyId.ToString() != ConfigurationHelper.FxtCompanyId)
            {
                return base.AuthorizeWarning("您无法对管理员通用权限进行修改！");
            }
            var role = _role.GetRolesBy(roleId, Passport.Current.FxtCompanyId).FirstOrDefault();
            this.ViewBag.RoleId = roleId.ToString();
            this.ViewBag.Readonly = "true";
            GetPermission(roleId);
            return View(role);
        }

        [HttpPost]
        //编辑保存
        public ActionResult Edit(FormCollection collection)
        {
            try
            {
                var roleId = collection["RoleId"];
                var roleName = collection["RoleName"];
                var remark = collection["Remarks"];
                var roles2Menus = collection["Roles2Menus"];
                var funcs = collection["Funcs"];

                #region 修改角色
                var sysrole = new SYS_Role
                {
                    ID = int.Parse(roleId),
                    RoleName = roleName,
                    Remarks = remark
                };
                _role.UpdateRole(sysrole);
                #endregion

                #region 修改角色对应的菜单
                _menu.DeleteRoleMenusFuncs(int.Parse(roleId));
                if (roles2Menus == null)
                {
                    return this.Back("请选择相应的权限！");
                }
                var subMenus = roles2Menus.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                var _menus = new List<int>();
                subMenus.ToList().ForEach(m => _menus.Add(int.Parse(m)));
                var menus = _menu.GetMenusByMenuIds(_menus.ToArray());
                menus.ToList().ForEach(_menus.Add);
                _menu.AddRoleMenus(Passport.Current.FxtCompanyId, int.Parse(roleId), _menus.ToArray());

                #endregion

                #region 修改角色对应的操纵功能

                if (funcs != null)
                {
                    var menuFuncs = funcs.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in menuFuncs)
                    {
                        var array = item.Split('#');
                        var sysRoleMenu = _menu.GetRoleMenuByParams(int.Parse(roleId), int.Parse(array[0]))
                            .FirstOrDefault();
                        if (sysRoleMenu == null) continue;
                        var roleMenuId = sysRoleMenu.ID;
                        _menu.AddRoleMenuFuncs(Passport.Current.FxtCompanyId, roleMenuId, int.Parse(array[1]));
                    }
                }
                #endregion

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.角色菜单, SYS_Code_Dict.操作.修改, roleId.ToString(), "", "修改角色及菜单", RequestHelper.GetIP());

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Authorize/Role/Edit", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return this.Back("操作失败！");
            }
        }

        //返回角色页面列表权限
        [NonAction]
        public void GetPermission(int roleId)
        {
            var menu = _menu.GetMenusByRoleId(roleId).Select(p => p.ID);
            var actualMenus = new List<SYS_Menu>();
            var menus = _menu.GetMenus().OrderBy(r => r.ModuleCode).ToList();//所有的页面列表

            foreach (var item in menus)
            {
                item.Selected = menu.Contains(item.ID);
                actualMenus.Add(item);
            }
            this.ViewBag.actualMenus = actualMenus;
        }

    }
}
