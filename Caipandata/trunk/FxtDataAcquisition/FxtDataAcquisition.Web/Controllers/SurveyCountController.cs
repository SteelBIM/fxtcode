namespace FxtDataAcquisition.Web.Controllers
{
    using System.Web.Mvc;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Collections.Generic;

    using FxtDataAcquisition.Common;
    using FxtDataAcquisition.Web.Common;
    using FxtDataAcquisition.Domain.DTO;
    using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
    using FxtDataAcquisition.Domain.Models;
    using FxtDataAcquisition.Application.Services;
    using FxtDataAcquisition.Application.Interfaces;

    /// <summary>
    /// 查勘统计
    /// </summary>
    public class SurveyCountController : BaseController
    {
        public SurveyCountController(IAdminService unitOfWork)
            : base(unitOfWork)
        {
        }
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX, NowFunctionPageUrl = WebCommon.Url_SurveyCount_Index, OrNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_1, SYSCodeManager.FunOperCode_2, SYSCodeManager.FunOperCode_3 })]
        public ActionResult Index(LoginUser user)
        {
            var departmentFilter = PredicateBuilder.True<Department>();

            #region 权限
            List<int> functionCodes = _unitOfWork.FunctionService.GetAllBy(user.UserName, user.FxtCompanyId, user.NowCityId, WebCommon.Url_AllotFlowInfo_AllotFlowManager).Select(m => m.FunctionCode).ToList();

            if (functionCodes.Contains(SYSCodeManager.FunOperCode_3))//全部
            {
                departmentFilter = departmentFilter.And(m => (m.FK_CityId == user.NowCityId || m.FK_CityId == 0) && (m.Fk_CompanyId == user.FxtCompanyId || m.Fk_CompanyId == 0) && m.DValid == 1 );
            }
            else if (functionCodes.Contains(SYSCodeManager.FunOperCode_2))//小组内
            {
                var nowDepartment = _unitOfWork.DepartmentUserRepository.Get(m => (m.FxtCompanyID == user.FxtCompanyId || m.FxtCompanyID == 0) && (m.CityID == user.NowCityId || m.CityID == 0)  && m.UserName == user.UserName).FirstOrDefault();

                departmentFilter = departmentFilter.And(m => m.DepartmentId == nowDepartment.DepartmentID);
            }
            #endregion

            ViewBag.DepartmentList = _unitOfWork.DepartmentRepository.Get(departmentFilter).ToList();

            return View();
        }

        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult LoadData(int pageIndex, int pageSize, int departmentId, string uName, LoginUser user)
        {
            var result = new AjaxResult("");

            var surveystFilter = PredicateBuilder.True<AllotFlow>();

            surveystFilter = surveystFilter.And(m => m.CityId == user.NowCityId && m.FxtCompanyId == user.FxtCompanyId);

            #region 小组
            if (departmentId > 0)
            {
                //小组下所有用户
                var departmentUser = _unitOfWork.DepartmentUserRepository.Get() .Where(m => m.FxtCompanyID == user.FxtCompanyId && m.CityID == user.NowCityId && m.DepartmentID == departmentId).ToList();

                var userids = departmentUser.Select(m => m.UserName);

                if (userids.Count() > 0)
                {
                    surveystFilter = surveystFilter.And(m => userids.Contains(m.SurveyUserName));
                }
                else
                {
                    return AjaxJson(result);
                }
            }
            if (!uName.IsNullOrEmpty())
            {
                surveystFilter = surveystFilter.And(m => m.SurveyUserTrueName.Contains(uName));
            }
            #endregion

            #region 权限
            List<int> functionCodes = _unitOfWork.FunctionService.GetAllBy(user.UserName, user.FxtCompanyId, user.NowCityId, WebCommon.Url_AllotFlowInfo_AllotFlowManager).Select(m => m.FunctionCode).ToList();

            if (functionCodes.Contains(SYSCodeManager.FunOperCode_3))//全部
            {
            }
            else if (functionCodes.Contains(SYSCodeManager.FunOperCode_2))//小组内
            {
                var nowDepartment = _unitOfWork.DepartmentUserRepository.Get(m => (m.FxtCompanyID == user.FxtCompanyId || m.FxtCompanyID == 0) && (m.CityID == user.NowCityId || m.CityID == 0) && m.UserName == user.UserName).FirstOrDefault();

                //小组下所有用户
                var departmentUser = _unitOfWork.DepartmentUserRepository.Get() .Where(m => m.FxtCompanyID == user.FxtCompanyId && m.CityID == user.NowCityId && m.DepartmentID == nowDepartment.DepartmentID).ToList();

                var userids = departmentUser.Select(m => m.UserName);

                if (userids.Count() > 0)
                {
                    surveystFilter = surveystFilter.And(m => userids.Contains(m.SurveyUserName));
                }
                else
                {
                    return AjaxJson(result);
                }

            }
            #endregion

            var surveys = _unitOfWork.AllotFlowRepository.Get(surveystFilter);

            var projects = _unitOfWork.ProjectRepository.Get(m => m.CityID == user.NowCityId && m.FxtCompanyId == user.FxtCompanyId);

            var buildings = _unitOfWork.BuildingRepository.Get(m => m.CityID == user.NowCityId && m.FxtCompanyId == user.FxtCompanyId);

            var data = from s in surveys
                       group s by new { SurveyUserName = s.SurveyUserName, SurveyUserTrueName = s.SurveyUserTrueName } into us
                       where us.Key.SurveyUserName != "" && us.Key.SurveyUserName != null
                       select new SurveyCountDto()
                       {
                           ToSurveyCount = us.Where(m => m.StateCode == SYSCodeManager.STATECODE_2).Count(),
                           InTheSurveyCount = us.Where(m => m.StateCode == SYSCodeManager.STATECODE_4).Count(),
                           HaveSurveyCount = us.Where(m => m.StateCode == SYSCodeManager.STATECODE_5).Count(),
                           PendingApprovalCount = us.Where(m => m.StateCode == SYSCodeManager.STATECODE_6).Count(),
                           PassedApprovalCount = us.Where(m => m.StateCode == SYSCodeManager.STATECODE_8).Count(),
                           AlreadyStorageCount = us.Where(m => m.StateCode == SYSCodeManager.STATECODE_10).Count(),
                           SurveyUserName = us.Key.SurveyUserName,
                           SurveyUserTrueName = us.Key.SurveyUserTrueName,
                           AlreadyStorageBuildingCount = (from u in us
                                                          join p in projects on u.DatId equals p.ProjectId
                                                          join b in buildings on p.ProjectId equals b.ProjectId
                                                          where u.StateCode == SYSCodeManager.STATECODE_10
                                                          select b).Count()

                       };
            var recordcount = data.Count();

            var list = data.OrderByDescending(m => m.AlreadyStorageCount).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            result.Data = new { list = list, recordcount = recordcount };

            return AjaxJson(result);
        }
    }
}