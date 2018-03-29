using FxtDataAcquisition.Application.Interfaces;
using FxtDataAcquisition.Application.Services;
using FxtDataAcquisition.BLL;
using FxtDataAcquisition.Common;
using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
using FxtDataAcquisition.Domain.Models;
using FxtDataAcquisition.NHibernate.Entities;
using FxtDataAcquisition.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace FxtDataAcquisition.Web.Controllers
{
    public class DepartmentInfoController : BaseController
    {
        public DepartmentInfoController(IAdminService unitOfWork)
            : base(unitOfWork)
        {
        }

        #region (DepartmentManager.cshtml)
        /// <summary>
        /// 小组列表页面
        /// </summary>
        /// <returns></returns>

        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX, NowFunctionPageUrl = WebCommon.Url_DepartmentInfo_DepartmentManager, OrNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_1, SYSCodeManager.FunOperCode_2, SYSCodeManager.FunOperCode_3 })]
        public ActionResult DepartmentManager(UserCenter_LoginUserInfo UserInfo)
        {
            ViewBag.CompanyName = UserInfo.CompanyName;
            //获取当前用户所在组
            var nowDep = _unitOfWork.DepartmentUserRepository.GetBy(m => m.CityID == UserInfo.NowCityId && m.FxtCompanyID == UserInfo.FxtCompanyId && m.UserName == UserInfo.UserName);
            ViewBag.NowDepartmentId = nowDep == null ? 0 : nowDep.DepartmentID;

            //获得当前用户在当前页指定的权限
            ViewBag.NowFunctionCodes = _unitOfWork.FunctionService.GetAllBy(UserInfo.UserName, UserInfo.FxtCompanyId, UserInfo.NowCityId, WebCommon.Url_DepartmentInfo_DepartmentManager)
                                        .Select(m => m.FunctionCode).ToList();

            return View();
        }
        /// <summary>
        /// 获取小组列表
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="isGetCount"></param>
        /// <returns></returns>
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult DepartmentManager_GetList_Api(string keyword, UserCenter_LoginUserInfo loginUserInfo, int pageIndex = 1, int pageSize = 10)
        {
            AjaxResult result = new AjaxResult("");

            var filter = PredicateBuilder.True<Department>();

            filter = filter.And(m => (m.FK_CityId == loginUserInfo.NowCityId || m.FK_CityId == 0) && (m.Fk_CompanyId == loginUserInfo.FxtCompanyId || m.Fk_CompanyId == 0) && m.DValid == 1);

            if (!keyword.IsNullOrEmpty())
            {
                filter = filter.And(m => m.DepartmentName.Contains(keyword));
            }

            int count = _unitOfWork.DepartmentRepository.Get(filter).Count();

            var list = _unitOfWork.DepartmentRepository.Get(filter).OrderBy(m => m.DepartmentId).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            result.Data = new { count = count, list = list };

            return AjaxJson(result);

        }
        /// <summary>
        /// 删除小组
        /// </summary>
        /// <param name="departmentIds"></param>
        /// <returns></returns>
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX, NowFunctionPageUrl = WebCommon.Url_DepartmentInfo_DepartmentManager, AndNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_9 })]
        public ActionResult DepartmentManager_Delete_Api(string departmentIds, UserCenter_LoginUserInfo UserInfo)
        {
            AjaxResult result = new AjaxResult("删除成功！");

            if (departmentIds.IsNullOrEmpty())
            {
                result.Message = "选择要删除的信息";
                result.Result = false;
                return AjaxJson(result);
            }

            var dIds = departmentIds.ConvertToIntList(',');

            if (dIds == null || dIds.Count < 1)
            {
                result.Message = "选择要删除的信息";
                result.Result = false;
                return AjaxJson(result);
            }

            var ds = _unitOfWork.DepartmentRepository.Get(m => dIds.Contains(m.DepartmentId)).ToList();

            foreach (var item in ds)
            {
                item.DValid = 0;

                _unitOfWork.DepartmentRepository.Update(item);
            }

            _unitOfWork.Commit();

            return AjaxJson(result);

            //UserCenter_LoginUserInfo loginUserInfo = WebUserHelp.GetNowLoginUser();
            //string json = "";
            //departmentIds = departmentIds.TrimBlank();
            //if (string.IsNullOrEmpty(departmentIds))
            //{
            //    Response.Write(json.MvcResponseJson(result: 0, message: "选择要删除的信息"));
            //    Response.End();
            //    return null;
            //}
            //int[] _departmentIds = departmentIds.ConvertToInts(',');
            //if (_departmentIds == null || _departmentIds.Length < 1)
            //{
            //    Response.Write(json.MvcResponseJson(result: 0, message: "选择要删除的信息"));
            //    Response.End();
            //    return null;
            //}
            //string message = "";
            //List<int> functionCodes = WebUserHelp.GetNowPageFunctionCodes(loginUserInfo.UserName, loginUserInfo.FxtCompanyId, WebCommon.Url_DepartmentInfo_DepartmentManager);
            ////权限为能删除全部
            //if (functionCodes.Contains(SYSCodeManager.FunOperCode_9))
            //{
            //}
            //else if (functionCodes.Contains(SYSCodeManager.FunOperCode_8))//权限删除自己
            //{
            //    PriviDepartment nowDep = PriviDepartmentManager.GetDepartmentByUserName(WebUserHelp.GetNowCityId(), loginUserInfo.FxtCompanyId, loginUserInfo.UserName);
            //    if (nowDep != null || !_departmentIds.Contains(nowDep.DepartmentId))
            //    {
            //        Response.Write(json.MvcResponseJson(result: 0, message: "您无权限删除除自己之外的小组"));
            //        Response.End();
            //        return null;
            //    }
            //}
            //else
            //{
            //    Response.Write(json.MvcResponseJson(result: 0, message: "您无删除权限"));
            //    Response.End();
            //    return null;
            //}
            //bool result = PriviDepartmentManager.DeleteDepartment(_departmentIds, out message);
            ////失败
            //if (!result)
            //{
            //    Response.Write(json.MvcResponseJson(result: 0, message: "删除失败:" + message));
            //    Response.End();
            //    return null;
            //}
            ////成功
            //Response.Write(json.MvcResponseJson(result: 1, message: ""));
            //Response.End();
            //return null;
        }
        #endregion

        #region SetDepartment.cshtml
        /// <summary>
        /// 获取小组编辑页面
        /// </summary>
        /// <param name="departmentId">是否为修改</param>
        /// <param name="companyName"></param>
        /// <returns></returns>
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.OPEN)]
        public ActionResult SetDepartment(string departmentId, string companyName)
        {
            UserCenter_LoginUserInfo loginUserInfo = WebUserHelp.GetNowLoginUser();
            int result = 1;
            companyName = companyName.DecodeField();
            if (departmentId.CheckInteger())//修改
            {
                List<int> functionCodes = WebUserHelp.GetNowPageFunctionCodes(loginUserInfo.UserName, loginUserInfo.FxtCompanyId, WebCommon.Url_DepartmentInfo_DepartmentManager);
                PriviDepartment department = PriviDepartmentManager.GetDepartmentById(Convert.ToInt32(departmentId));
                if (functionCodes.Contains(SYSCodeManager.FunOperCode_7))
                { }
                else if (functionCodes.Contains(SYSCodeManager.FunOperCode_5))//修改自己
                {
                    PriviDepartment nowDep = PriviDepartmentManager.GetDepartmentByUserName(WebUserHelp.GetNowCityId(), loginUserInfo.FxtCompanyId, loginUserInfo.UserName);
                    if (nowDep == null || !nowDep.DepartmentId.ToString().Equals(departmentId))
                    {
                        return WebUserHelp.GetActionNotRightPage();//无权限
                    }
                }
                else
                {
                    return WebUserHelp.GetActionNotRightPage();//无权限
                }
                if (department != null)
                {
                    ViewBag.DepartmentName = department.DepartmentName;
                    ViewBag.DepartmentId = departmentId;
                }
                else
                {
                    result = 0;
                }
            }
            else//新增
            {
                if (!WebUserHelp.CheckNowPageFunctionCode(loginUserInfo.UserName, loginUserInfo.FxtCompanyId, WebCommon.Url_DepartmentInfo_DepartmentManager, SYSCodeManager.FunOperCode_4))
                {
                    return WebUserHelp.GetActionNotRightPage();//无权限
                }
            }
            ViewBag.Result = result;
            ViewBag.CompanyName = companyName;
            return View();
        }

        /// <summary>
        /// 提交编辑后的小组信息
        /// </summary>
        /// <param name="departmentId">是否为修改</param>
        /// <param name="departmentName"></param>
        /// <returns></returns>
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult SetDepartment_SubmitData_Api(string departmentId, string departmentName)
        {
            string json = "";
            departmentName = departmentName.DecodeField().TrimBlank();
            if (string.IsNullOrEmpty(departmentName))
            {
                Response.Write(json.MvcResponseJson(result: 0, message: "请填写组名"));
                Response.End();
                return null;
            }
            string message = "";
            bool result = true;
            UserCenter_LoginUserInfo loginUserInfo = WebUserHelp.GetNowLoginUser();
            //如果是修改
            if (departmentId.CheckInteger())
            {
                List<int> functionCodes = WebUserHelp.GetNowPageFunctionCodes(loginUserInfo.UserName, loginUserInfo.FxtCompanyId, WebCommon.Url_DepartmentInfo_DepartmentManager);
                PriviDepartment department = PriviDepartmentManager.GetDepartmentById(Convert.ToInt32(departmentId));
                if (functionCodes.Contains(SYSCodeManager.FunOperCode_7))//修改全部
                { }
                else if (functionCodes.Contains(SYSCodeManager.FunOperCode_5))//修改自己
                {
                    PriviDepartment nowDep = PriviDepartmentManager.GetDepartmentByUserName(WebUserHelp.GetNowCityId(), loginUserInfo.FxtCompanyId, loginUserInfo.UserName);
                    if (nowDep == null || !nowDep.DepartmentId.ToString().Equals(departmentId))
                    {
                        Response.Write(json.MvcResponseJson(result: 0, message: "无权限修改此信息"));
                        Response.End();
                        return null;
                    }
                }
                else//无修改权限
                {
                    Response.Write(json.MvcResponseJson(result: 0, message: "无权限修改此信息"));
                    Response.End();
                    return null;
                }
                result = PriviDepartmentManager.UpdateDepartment(Convert.ToInt32(departmentId), departmentName, out message);
            }
            else//新增
            {
                if (!WebUserHelp.CheckNowPageFunctionCode(loginUserInfo.UserName, loginUserInfo.FxtCompanyId, WebCommon.Url_DepartmentInfo_DepartmentManager, SYSCodeManager.FunOperCode_4))
                {
                    Response.Write(json.MvcResponseJson(result: 0, message: "无权限新增信息"));
                    Response.End();
                    return null;
                }
                int cityId = WebUserHelp.GetNowCityId();
                int companyId = loginUserInfo.FxtCompanyId;
                string loginusername = loginUserInfo.UserName;
                string loginsignname = loginUserInfo.SignName;
                PriviDepartment department = PriviDepartmentManager.InsertDepartment(cityId, companyId, departmentName, out message);
                if (department == null)
                {
                    result = false;
                }
                else
                {
                    json = department.EncodeField<PriviDepartment>().ToJSONjss();
                }
            }
            //失败
            if (!result)
            {
                Response.Write(json.MvcResponseJson(result: 0, message: "提交失败:" + message));
                Response.End();
                return null;
            }
            //成功
            Response.Write(json.MvcResponseJson(result: 1, message: ""));
            Response.End();
            return null;
        }

        #endregion
    }
}