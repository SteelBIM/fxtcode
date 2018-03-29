namespace FxtDataAcquisition.Web.Controllers
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Web.Mvc;
    using System.Collections.Generic;

    using FxtDataAcquisition.Common;
    using FxtDataAcquisition.Web.Common;
    using FxtDataAcquisition.Domain.DTO;
    using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
    using FxtDataAcquisition.Domain.Models;
    using FxtDataAcquisition.FxtAPI.FxtUserCenter.Manager;
    using FxtDataAcquisition.Application.Services;
    using FxtDataAcquisition.Application.Interfaces;

    public class UserInfoController : BaseController
    {
        public UserInfoController(IAdminService unitOfWork)
            : base(unitOfWork)
        {
        }
        //
        // GET: /UserManager/

        public ActionResult Index()
        {
            return View();
        }
        #region (UserManager.cshtml)
        /// <summary>
        /// 查询用户列表页面
        /// </summary>
        /// <returns></returns>
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX, NowFunctionPageUrl = WebCommon.Url_UserInfo_UserManager, OrNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_1, SYSCodeManager.FunOperCode_2, SYSCodeManager.FunOperCode_3 })]
        public ActionResult UserManager(LoginUser user)
        {
            ViewBag.DepartmentList = _unitOfWork.DepartmentRepository.Get(m => m.FK_CityId == user.NowCityId && m.Fk_CompanyId == user.FxtCompanyId && m.DValid == 1).ToList();

            ViewBag.RoleList = _unitOfWork.SysRoleRepository.Get(m => (m.CityID == user.NowCityId || m.CityID == 0) && (m.FxtCompanyID == user.FxtCompanyId || m.FxtCompanyID == 0) && m.Valid == 1).ToList();

            var updateRight = _unitOfWork.FunctionService.GetAllBy(user.UserName, user.FxtCompanyId, user.NowCityId, WebCommon.Url_UserInfo_UserManager) .FirstOrDefault(m => m.FunctionCode == SYSCodeManager.FunOperCode_7);

            if (updateRight != null)
            {
                ViewBag.UpdateRight = 1;
            }

            return View();
        }
        /// <summary>
        /// 查询用户列表
        /// </summary>
        /// <param name="keyWord"></param>
        /// <param name="roleId"></param>
        /// <param name="departmentId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="isGetCount"></param>
        /// <returns></returns>
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult UserManager_GetList_Api(string keyWord, int? roleId, int? departmentId, int pageIndex, int pageSize, LoginUser user)
        {
            var result = new AjaxResult("");

            List<UserInfoDto> list = new List<UserInfoDto>();

            List<int> functionCodes = _unitOfWork.FunctionService.GetAllBy(user.UserName, user.FxtCompanyId, user.NowCityId, WebCommon.Url_AllotFlowInfo_AllotFlowManager) .Select(m => m.FunctionCode).ToList();

            int count = 0;

            if (functionCodes.Contains(SYSCodeManager.FunOperCode_3))//全部
            {
                list = _unitOfWork.UserService.GetUserInfo(keyWord, roleId, departmentId, pageIndex, pageSize, out count, user.NowCityId, user.FxtCompanyId, user.UserName, user.SignName, user.AppList);
            }
            else if (functionCodes.Contains(SYSCodeManager.FunOperCode_2))//小组内
            {
                var nowDepartment = _unitOfWork.DepartmentUserRepository.Get(m => (m.FxtCompanyID == user.FxtCompanyId || m.FxtCompanyID == 0) && (m.CityID == user.NowCityId || m.CityID == 0) && m.UserName == user.UserName).FirstOrDefault();

                list = _unitOfWork.UserService.GetUserInfo(keyWord, roleId, nowDepartment.DepartmentID, pageIndex, pageSize, out count, user.NowCityId, user.FxtCompanyId, user.UserName, user.SignName, user.AppList);
            }

            result.Data = new { Count = count, List = list };

            return AjaxJson(result);
        }
        #endregion


        #region (EditUser.cshtml)
        /// <summary>
        /// 编辑的用户信息页面
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.OPEN, NowFunctionPageUrl = WebCommon.Url_UserInfo_UserManager, AndNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_7 })]
        public ActionResult EditUser(string userName, LoginUser userInfo)
        {
            int count = 0;

            var user = UserCenterUserInfoApi.GetUserListByCompanyId(userInfo.FxtCompanyId, new string[] { userName }, "", null, 1, 1, out count, userInfo.UserName, userInfo.SignName, userInfo.AppList).FirstOrDefault();

            ViewBag.DepartmentList = _unitOfWork.DepartmentRepository.Get(m => (m.FK_CityId == userInfo.NowCityId || m.FK_CityId == 0) && (m.Fk_CompanyId == userInfo.FxtCompanyId || m.Fk_CompanyId == 0) && m.DValid == 1).ToList();

            ViewBag.Department = _unitOfWork.DepartmentUserRepository.GetBy(m => m.UserName == user.UserName && (m.CityID == userInfo.NowCityId || m.CityID == 0) && (m.FxtCompanyID == userInfo.FxtCompanyId || m.FxtCompanyID == 0));

            ViewBag.RoleList = _unitOfWork.SysRoleRepository.Get(m => (m.CityID == userInfo.NowCityId || m.CityID == 0) && (m.FxtCompanyID == userInfo.FxtCompanyId || m.FxtCompanyID == 0) && m.Valid == 1).ToList();

            ViewBag.RoleUser = _unitOfWork.SysRoleUserRepository.Get(m => m.UserName == user.UserName && (m.CityID == userInfo.NowCityId || m.CityID == 0) && (m.FxtCompanyID == userInfo.FxtCompanyId || m.FxtCompanyID == 0)).Select(m => m.RoleID).ToList();

            var roleAdmin = _unitOfWork.SysRoleRepository.GetBy(m => m.CityID == 0 && m.FxtCompanyID == 0 && m.Valid == 1 && m.RoleName == "管理员");

            ViewBag.RoleAdminId = roleAdmin == null ? 0 : roleAdmin.Id;

            return View(user);
        }
        /// <summary>
        /// 提交编辑的用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="departmentId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX, NowFunctionPageUrl = WebCommon.Url_UserInfo_UserManager, AndNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_7 })]
        public ActionResult EditUser_SubmitData_Api(string userName, string truename, int departmentId, List<int> roleIds, LoginUser userInfo)
        {
            var result = new AjaxResult("提交成功！");

            if (userName.IsNullOrEmpty())
            {
                result.Result = false;

                result.Message = "请填写用户名";

                return AjaxJson(result);
            }

            #region 小组
            var departmentUser = _unitOfWork.DepartmentUserRepository.GetBy(m => m.UserName == userName && (m.CityID == userInfo.NowCityId || m.CityID == 0) && (m.FxtCompanyID == userInfo.FxtCompanyId || m.FxtCompanyID == 0));

            if (departmentUser == null)
            {
                var department = _unitOfWork.DepartmentRepository.GetById(departmentId);//必须找到小组

                departmentUser = _unitOfWork.DepartmentUserRepository.Insert(new DepartmentUser()
                                {
                                    CityID = department.FK_CityId,
                                    CreateDate = DateTime.Now,
                                    DepartmentID = departmentId,
                                    FxtCompanyID = department.Fk_CompanyId,
                                    UserName = userName
                                });
            }
            else
            {
                departmentUser.DepartmentID = departmentId;
            }
            #endregion

            #region 角色
            List<string> roleNames = new List<string>();

            _unitOfWork.SysRoleUserRepository.Delete(m => m.UserName == userName && (m.CityID == userInfo.NowCityId || m.CityID == 0) && (m.FxtCompanyID == userInfo.FxtCompanyId || m.FxtCompanyID == 0));

            if (roleIds != null && roleIds.Count > 0)
            {
                roleIds.ForEach((o) =>
                {
                    var role = _unitOfWork.SysRoleRepository.GetById(o);//必须找到角色

                    _unitOfWork.SysRoleUserRepository.Insert(new SYS_Role_User()
                    {
                        CityID = role.CityID,
                        FxtCompanyID = role.FxtCompanyID,
                        RoleID = o,
                        TrueName = truename,
                        UserName = userName
                    });

                    roleNames.Add(role.RoleName);
                });
            }
            #endregion

            _unitOfWork.Commit();

            result.Data = new { username = userName, truename = truename, departmentname = departmentUser.Department.DepartmentName, rolename = string.Join(",", roleNames) };

            return AjaxJson(result);
        }
        #endregion

    }
}
