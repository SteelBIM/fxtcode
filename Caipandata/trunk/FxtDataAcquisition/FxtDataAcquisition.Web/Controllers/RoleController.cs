namespace FxtDataAcquisition.Web.Controllers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Linq;
    using System.Collections.Generic;

    using FxtDataAcquisition.Web.Common;
    using FxtDataAcquisition.Domain.Models;
    using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
    using FxtDataAcquisition.Application.Services;
    using FxtDataAcquisition.Application.Interfaces;

    public class RoleController : BaseController
    {
        public RoleController(IAdminService unitOfWork)
            : base(unitOfWork)
        {

        }
        // GET: Role
        //[AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX, NowFunctionPageUrl = WebCommon.Url_Role_Index, AndNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_3})]
        public ActionResult Index(LoginUser loginUser)
        {
            var roles = _unitOfWork.SysRoleRepository.Get(m => m.Valid == 1).ToList();

            var userRoles = _unitOfWork.SysRoleUserRepository.Get(m => m.UserName == loginUser.UserName && (m.CityID == loginUser.NowCityId || m.CityID == 0) && (m.FxtCompanyID == loginUser.FxtCompanyId || m.FxtCompanyID == 0));

            var myRoles = userRoles.Select(m => m.Role).ToList();

            ViewBag.Roles = myRoles;
            
            return View(roles);
        }

        //[AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX, NowFunctionPageUrl = WebCommon.Url_Role_Index, AndNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_3})]
        public ActionResult MenuFuncton(SYS_Role role, LoginUser loginUser)
        {
            ViewBag.Menus = _unitOfWork.SysMenuRepository.Get(m => m.Valid == 1 && (m.ModuleCode == 1001 || m.TypeCode == 1202002)).ToList();

            return View(role);
        }

        //[AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX, NowFunctionPageUrl = WebCommon.Url_Role_Index, AndNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_3 })]
        public ActionResult Save(SYS_Role role, int[] menu, string[] function)
        {
            var result = new AjaxResult("保存成功！");

            #region 菜单
            if (menu == null) menu = new int[0];

            var menuDelete = role.RoleMenus.Select(m => m.MenuID).Except(menu).ToList();

            var menuAdd = menu.Except(role.RoleMenus.Select(m => m.MenuID)).ToList();

            if (menuDelete != null && menuDelete.Count() > 0)
            {
                _unitOfWork.SysRoleMenuRepository.Delete(m => menuDelete.Contains(m.MenuID) && m.RoleID == role.Id);
            }
            if (menuAdd != null && menuAdd.Count() > 0)
            {
                foreach (var item in menuAdd)
                {
                    role.RoleMenus.Add(new SYS_Role_Menu()
                    {
                        CityID = 0,
                        FxtCompanyID = role.FxtCompanyID,
                        MenuID = item,
                        RoleID = role.Id,
                    });
                }
            }
            #endregion

            #region 权限
            if (function == null) function = new string[0];
            //循环所有菜单
            var menus = _unitOfWork.SysMenuRepository.Get(m => m.Valid == 1);

            if (menus != null && menus.Count() > 0)
            {
                foreach (var ms in menus)
                {
                    //处理权限
                    var tFunctions = function.Where(m => m.StartsWith(ms.Id.ToString() + "#"));
                    //新的权限
                    var roleMenuFunctions = new List<SYS_Role_Menu_Function>();

                    if (tFunctions != null && tFunctions.Count() > 0)
                    {
                        foreach (var tf in tFunctions)
                        {
                            var menuFuncs = tf.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries);

                            roleMenuFunctions.Add(new SYS_Role_Menu_Function()
                            {
                                RoleMenuID = int.Parse(menuFuncs[1]),
                                FunctionCode = int.Parse(menuFuncs[2])
                            }
                            );
                        }
                    }

                    var tRoleMenu = role.RoleMenus.Where(m => m.MenuID == ms.Id && m.RoleID == role.Id).FirstOrDefault();

                    if (tRoleMenu != null)
                    {
                        if (tRoleMenu.Functions == null && roleMenuFunctions.Count == 0) continue;

                        if (tRoleMenu.Functions == null ) tRoleMenu.Functions = new List<SYS_Role_Menu_Function>();
                        
                        var old = tRoleMenu.Functions.Select(m => new SYS_Role_Menu_Function() { RoleMenuID = m.RoleMenuID, FunctionCode = m.FunctionCode }).ToList();

                        var now = roleMenuFunctions.Where(m => m.RoleMenuID == tRoleMenu.Id).ToList();

                        var functionDelete = old.Except(now).ToList();

                        var functionAdd = now.Except(old).ToList();

                        if (functionDelete != null && functionDelete.Count() > 0)
                        {
                            var fds = functionDelete.Select(m=>m.FunctionCode);

                            _unitOfWork.SysRoleMenuFunctionRepository.Delete(m => fds.Contains(m.FunctionCode) && m.RoleMenuID == tRoleMenu.Id);
                        }

                        if (functionAdd != null && functionAdd.Count() > 0)
                        {
                            foreach (var fa in functionAdd)
                            {
                                tRoleMenu.Functions.Add(new SYS_Role_Menu_Function()
                                {
                                    CityID = 0,
                                    FxtCompanyID = role.FxtCompanyID,
                                    FunctionCode = fa.FunctionCode,
                                    RoleMenuID = tRoleMenu.Id,
                                    Valid = 1
                                });
                            }

                        }
                    }
                }


            }
            #endregion

            _unitOfWork.Commit();

            return AjaxJson(result);
        }
    }
}