using FxtDataAcquisition.Application.Interfaces;
using FxtDataAcquisition.Application.Services;
using FxtDataAcquisition.BLL;
using FxtDataAcquisition.Common;
using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
using FxtDataAcquisition.DTODomain.NHibernate;
using FxtDataAcquisition.NHibernate.Entities;
using FxtDataAcquisition.Web.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace FxtDataAcquisition.Web.Controllers
{
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
        public ActionResult UserManager()
        {

            UserCenter_LoginUserInfo loginUserInfo =  WebUserHelp.GetNowLoginUser();
            int cityId = WebUserHelp.GetNowCityId();
            int companyId = loginUserInfo.FxtCompanyId;
            IList<PriviDepartment> departmentList = PriviDepartmentManager.GetDepartmentByCompanyId(cityId, companyId);
            IList<SYSRole> roleList = SYSRoleManager.GetSYSRoleByCompanyId(0, 0);
            ViewBag.DepartmentList = departmentList;
            ViewBag.RoleList = roleList;
            //获取是否有修改用户权限
            ViewBag.UpdateRight = 0;
            bool updateRight = WebUserHelp.CheckNowPageFunctionCode(loginUserInfo.UserName, loginUserInfo.FxtCompanyId, WebCommon.Url_UserInfo_UserManager, SYSCodeManager.FunOperCode_7);
            if (updateRight)
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
        public ActionResult UserManager_GetList_Api(string keyWord, string roleId, string departmentId, int pageIndex, int pageSize, int isGetCount, UserCenter_LoginUserInfo loginUserInfo)
        {
            int? _roleId = null;
            int? _departmentId = null;
            int companyId = loginUserInfo.FxtCompanyId;
            bool _isGetCount = true;
            string username = loginUserInfo.UserName;
            string signname = loginUserInfo.SignName;

            if (!string.IsNullOrEmpty(roleId) && roleId != "0")
            {
                _roleId = Convert.ToInt32(roleId);
            }
            if (!string.IsNullOrEmpty(departmentId) && departmentId != "0")
            {
                _departmentId = Convert.ToInt32(departmentId);
            }
            if (isGetCount == 0)
            {
                _isGetCount = false;
            }

            List<int> functionCodes = _unitOfWork.FunctionService.GetAllBy(loginUserInfo.UserName, loginUserInfo.FxtCompanyId, loginUserInfo.NowCityId, WebCommon.Url_AllotFlowInfo_AllotFlowManager)
                .Select(m => m.FunctionCode).ToList();
            if (functionCodes.Contains(SYSCodeManager.FunOperCode_3))//全部
            {
            }
            else if (functionCodes.Contains(SYSCodeManager.FunOperCode_2))//小组内
            {
                var nowDepartment = _unitOfWork.DepartmentUserRepository.Get(m => (m.FxtCompanyID == loginUserInfo.FxtCompanyId || m.FxtCompanyID == 0)
                                    && (m.CityID == loginUserInfo.NowCityId || m.CityID == 0)
                                    && m.UserName == loginUserInfo.UserName).FirstOrDefault();
                _departmentId = nowDepartment.DepartmentID;
            }

            int count = 0;
            List<UserInfoJoinRoleJoinDepartment> list = UserInfoManager.GetUserInfoJoinRoleJoinDepartmentByRoleIdAndDepartmentIdAndUserName(loginUserInfo.NowCityId, companyId, keyWord, _roleId, _departmentId, pageIndex, pageSize, out count, username, signname, loginUserInfo.AppList, _isGetCount);
            string json = "{{\"Count\":{0},\"List\":{1}}}";
            json = string.Format(json, count, list.ToJSONjss());
            Response.Write(json.MvcResponseJson());
            Response.End();
            return null;
        }
        #endregion


        #region (EditUser.cshtml)
        /// <summary>
        /// 编辑的用户信息页面
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.OPEN,NowFunctionPageUrl = WebCommon.Url_UserInfo_UserManager, AndNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_7 })]
        public ActionResult EditUser(string userName)
        {
            ViewBag.DeparUrl = WebCommon.Url_DepartmentInfo_DepartmentManager;
            //获取用户信息
            PriviDepartmentUser pdu = null;
            IList<SYSRoleUser> roleUserList = new List<SYSRoleUser>();
            List<UserCenter_Apps> appList = new List<UserCenter_Apps>();
            UserCenter_LoginUserInfo loginUserInfo = WebUserHelp.GetNowLoginUser(out appList);
            int cityId = WebUserHelp.GetNowCityId();
            int companyId = loginUserInfo.FxtCompanyId;
            string loginusername = loginUserInfo.UserName;
            string loginsignname = loginUserInfo.SignName;
            UserCenter_UserInfo userInfo = UserInfoManager.GetUserInfoByUserName(cityId, companyId, userName, loginsignname, loginsignname,appList, out pdu, out roleUserList);
            if (userInfo != null)
            {
                //输出给页面变量
                ViewBag.CompanyName = userInfo.CompanyName;
                ViewBag.UserName = userInfo.UserName;
                ViewBag.TrueName = userInfo.TrueName;
                ViewBag.Mebile = userInfo.Mobile;
                if (pdu != null)
                {
                    ViewBag.DepartmentId = pdu.DepartmentID;
                }
                ViewBag.RoleUserList = roleUserList;
            }
            //获取小组+角色基础信息
            IList<SYSRole> roleList = SYSRoleManager.GetSYSRoleByCompanyId(cityId, companyId);
            IList<PriviDepartment> pdList = PriviDepartmentManager.GetDepartmentByCompanyId(cityId, companyId);
            ViewBag.DepartmentList = pdList;
            ViewBag.RoleList = roleList;
            return View();
        }
        /// <summary>
        /// 提交编辑的用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="departmentId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX, NowFunctionPageUrl = WebCommon.Url_UserInfo_UserManager, AndNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_7 })]
        public ActionResult EditUser_SubmitData_Api(string userName, string truename, string departmentId, string roleIds)
        {
            string json="";
            int? _departmentId = null;
            int[] _roleIds = null;
            if (string.IsNullOrEmpty(userName))
            {
                Response.Write(json.MvcResponseJson(result: 0, message: "请填写用户名"));
                Response.End();
                return null;
            }
            //获取提交数据
            UserCenter_LoginUserInfo loginUserInfo = WebUserHelp.GetNowLoginUser();
            int cityId = WebUserHelp.GetNowCityId();
            int companyId = loginUserInfo.FxtCompanyId;
            string loginusername = loginUserInfo.UserName;
            string loginsignname = loginUserInfo.SignName;
            if (StringHelp.CheckInteger(departmentId)&&departmentId!="0")
            {
                _departmentId = Convert.ToInt32(departmentId);
            }
            _roleIds = roleIds.ConvertToInts(',');
            //提交数据
            string message="";
            bool result = UserInfoManager.SetUserInfo(cityId, companyId, userName, truename, _departmentId, _roleIds, out message);
            if (!result)
            {
                Response.Write(json.MvcResponseJson(result: 0, message: "提交失败:"+message));
                Response.End();
                return null;
            }
            IList<SYSRole> roleList = SYSRoleManager.GetSYSRoleByRoleIds(_roleIds);
            StringBuilder roleNameSb = new StringBuilder("");
            string departmentName = "";
            foreach (SYSRole roleInfo in roleList)
            {
                roleNameSb.Append(roleInfo.RoleName).Append(",");
            }
            if (Convert.ToInt32(_departmentId) > 0)
            {
                PriviDepartment pd = PriviDepartmentManager.GetDepartmentById(Convert.ToInt32(_departmentId));
                if (pd != null)
                {
                    departmentName = pd.DepartmentName;
                }
            }
            json = string.Format("{{\"username\":\"{0}\",\"truename\":\"{1}\",\"departmentname\":\"{2}\",\"rolename\":\"{3}\"}}", userName, truename, departmentName, roleNameSb.ToString());
            Response.Write(json.MvcResponseJson(result: 1, message: ""));
            Response.End();
            return null;
        }
        #endregion

    }
}
