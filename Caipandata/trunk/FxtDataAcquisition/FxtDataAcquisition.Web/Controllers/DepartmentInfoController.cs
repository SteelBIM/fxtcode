using FxtDataAcquisition.Application.Interfaces;
namespace FxtDataAcquisition.Web.Controllers
{
    using System.Web.Mvc;
    using System.Linq;
    using System.Linq.Expressions;

    using FxtDataAcquisition.Common;
    using FxtDataAcquisition.Web.Common;
    using FxtDataAcquisition.Application.Services;
    using FxtDataAcquisition.Domain.Models;
    using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;

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
        public ActionResult DepartmentManager(LoginUser UserInfo)
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
        public ActionResult DepartmentManager_GetList_Api(string keyword, LoginUser loginUserInfo, int pageIndex = 1, int pageSize = 10)
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

            return AjaxJson(result, false);

        }
        /// <summary>
        /// 删除小组
        /// </summary>
        /// <param name="departmentIds"></param>
        /// <returns></returns>
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX, NowFunctionPageUrl = WebCommon.Url_DepartmentInfo_DepartmentManager, AndNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_9 })]
        public ActionResult DepartmentManager_Delete_Api(string departmentIds, LoginUser UserInfo)
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
        }
        #endregion

        #region SetDepartment.cshtml
        /// <summary>
        /// 获取小组编辑页面
        /// </summary>
        /// <param name="departmentId">是否为修改</param>
        /// <param name="companyName"></param>
        /// <returns></returns>
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.OPEN, NowFunctionPageUrl = WebCommon.Url_DepartmentInfo_DepartmentManager, OrNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_7, SYSCodeManager.FunOperCode_5, SYSCodeManager.FunOperCode_4 })]
        public ActionResult SetDepartment(Department department, string companyName)
        {
            ViewBag.CompanyName = companyName;

            return View(department ?? new Department());
        }

        /// <summary>
        /// 提交编辑后的小组信息
        /// </summary>
        /// <param name="departmentId">是否为修改</param>
        /// <param name="departmentName"></param>
        /// <returns></returns>
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX, NowFunctionPageUrl = WebCommon.Url_DepartmentInfo_DepartmentManager, OrNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_7, SYSCodeManager.FunOperCode_5, SYSCodeManager.FunOperCode_4 })]
        public ActionResult SetDepartment_SubmitData_Api(Department department, LoginUser userInfo)
        {
            var result = new AjaxResult("提交成功！");

            if (department.DepartmentName.IsNullOrEmpty())
            {
                result.Result = false;
                result.Message = "请填写组名";
                return AjaxJson(result);
            }

            if (department.DepartmentId < 1)
            {
                department.FK_CityId = userInfo.NowCityId;
                department.FxtCompanyId = userInfo.FxtCompanyId;
                department.Fk_CompanyId = userInfo.FxtCompanyId;
                department.DValid = 1;
                department.FK_DepTypeCode = 5005003;
                _unitOfWork.DepartmentRepository.Insert(department);
                result.Data = department;
            }

            _unitOfWork.Commit();

            return AjaxJson(result);
        }

        #endregion
    }
}