using AutoMapper;
using CAS.Common.Office;
using CAS.Entity.FxtProject;
using FxtDataAcquisition.Application.Interfaces;
using FxtDataAcquisition.Application.Services;
using FxtDataAcquisition.BLL;
using FxtDataAcquisition.Common;
using FxtDataAcquisition.Common.NPOI;
using FxtDataAcquisition.Domain.DTO;
using FxtDataAcquisition.Domain.DTO.FxtDataWcfDTO;
using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
using FxtDataAcquisition.Domain.Models;
using FxtDataAcquisition.DTODomain.NHibernate;
using FxtDataAcquisition.FxtAPI.FxtDataCenter.Manager;
using FxtDataAcquisition.NHibernate.Entities;
using FxtDataAcquisition.Web.Common;
using log4net;
using Newtonsoft.Json.Linq;
using Ninject;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FxtDataAcquisition.Web.Controllers
{
    public class AllotFlowInfoController : BaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AllotFlowInfoController));

        public AllotFlowInfoController(IAdminService unitOfWork)
            : base(unitOfWork)
        {
        }

        //
        // GET: /AllotFlowInfo/
        #region (AllotFlowManager.cshtml)
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult AllotFlowManager(int? statuscode, UserCenter_LoginUserInfo loginUserInfo)
        {
            List<SYSCode> colist = DataCenterCodeApi.GetCodeById(1035, loginUserInfo.UserName, loginUserInfo.SignName, loginUserInfo.AppList);
            List<FxtApi_SYSArea> areaList = DataCenterAreaApi.GetAreaByCityId(loginUserInfo.NowCityId, loginUserInfo.UserName, loginUserInfo.SignName, loginUserInfo.AppList);
            ViewBag.AreaList = areaList;

            #region 权限
            //获取当前用户在此页面所有的操作权限
            List<int> functionCodes = new List<int>();
            if (!statuscode.HasValue)
            {
                functionCodes = _unitOfWork.FunctionService.GetAllBy(loginUserInfo.UserName, loginUserInfo.FxtCompanyId, loginUserInfo.NowCityId, WebCommon.Url_AllotFlowInfo_AllotFlowManager)
                .Select(m => m.FunctionCode).ToList();
            }
            else if (statuscode == SYSCodeManager.STATECODE_1)
            {
                functionCodes = _unitOfWork.FunctionService.GetAllBy(loginUserInfo.UserName, loginUserInfo.FxtCompanyId, loginUserInfo.NowCityId, WebCommon.Url_AllotFlowInfo_AllotFlowManager_1035001)
                .Select(m => m.FunctionCode).ToList();
            }
            else if (statuscode == SYSCodeManager.STATECODE_2)
            {
                functionCodes = _unitOfWork.FunctionService.GetAllBy(loginUserInfo.UserName, loginUserInfo.FxtCompanyId, loginUserInfo.NowCityId, WebCommon.Url_AllotFlowInfo_AllotFlowManager_1035002)
                .Select(m => m.FunctionCode).ToList();
            }
            else if (statuscode == SYSCodeManager.STATECODE_4)
            {
                functionCodes = _unitOfWork.FunctionService.GetAllBy(loginUserInfo.UserName, loginUserInfo.FxtCompanyId, loginUserInfo.NowCityId, WebCommon.Url_AllotFlowInfo_AllotFlowManager_1035004)
                .Select(m => m.FunctionCode).ToList();
            }
            else if (statuscode == SYSCodeManager.STATECODE_5)
            {
                functionCodes = _unitOfWork.FunctionService.GetAllBy(loginUserInfo.UserName, loginUserInfo.FxtCompanyId, loginUserInfo.NowCityId, WebCommon.Url_AllotFlowInfo_AllotFlowManager_1035005)
                .Select(m => m.FunctionCode).ToList();
            }
            else if (statuscode == SYSCodeManager.STATECODE_6)
            {
                functionCodes = _unitOfWork.FunctionService.GetAllBy(loginUserInfo.UserName, loginUserInfo.FxtCompanyId, loginUserInfo.NowCityId, WebCommon.Url_AllotFlowInfo_AllotFlowManager_1035006)
                .Select(m => m.FunctionCode).ToList();
            }
            else if (statuscode == SYSCodeManager.STATECODE_8)
            {
                functionCodes = _unitOfWork.FunctionService.GetAllBy(loginUserInfo.UserName, loginUserInfo.FxtCompanyId, loginUserInfo.NowCityId, WebCommon.Url_AllotFlowInfo_AllotFlowManager_1035008)
                .Select(m => m.FunctionCode).ToList();
            }
            else if (statuscode == SYSCodeManager.STATECODE_10)
            {
                functionCodes = _unitOfWork.FunctionService.GetAllBy(loginUserInfo.UserName, loginUserInfo.FxtCompanyId, loginUserInfo.NowCityId, WebCommon.Url_AllotFlowInfo_AllotFlowManager_1035010)
                .Select(m => m.FunctionCode).ToList();
            }

            if (!functionCodes.Contains(SYSCodeManager.FunOperCode_1) && !functionCodes.Contains(SYSCodeManager.FunOperCode_2) && !functionCodes.Contains(SYSCodeManager.FunOperCode_3))
            {
                return new Ajax_JsonFormatResult_NotRight();
            }
            #endregion

            ViewBag.FunctionCodes = functionCodes;

            ViewBag.NowStatus = statuscode;

            ViewBag.IomportAllotRight = 0;

            return View();
        }
        /// <summary>
        /// 获取任务列表
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="status"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="isGetCount"></param>
        /// <returns></returns>
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult AllotFlowManager_GetList_Api(string projectName, string selectUserTrueName, int? areaId, int? subAreaId, int status, DateTime? startDate, DateTime? endDate
            , string functionCodes, int pageIndex, int pageSize, int isGetCount, UserCenter_LoginUserInfo loginUserInfo)
        {
            AjaxResult result = new AjaxResult("");

            #region 查询条件
            var projectFilter = PredicateBuilder.True<Project>();
            projectFilter = projectFilter.And(m => m.CityID == loginUserInfo.NowCityId && m.Valid == 1);
            if (!projectName.IsNullOrEmpty())
            {
                projectFilter = projectFilter.And(m => m.ProjectName.Contains(projectName));
            }
            if (areaId.HasValue && areaId > 0)
            {
                projectFilter = projectFilter.And(m => m.AreaID == areaId);
            }
            if (subAreaId.HasValue && subAreaId > 0)
            {
                projectFilter = projectFilter.And(m => m.SubAreaId == subAreaId);
            }

            var allotFlowFilter = PredicateBuilder.True<AllotFlow>();
            allotFlowFilter = allotFlowFilter.And(m => m.CityId == loginUserInfo.NowCityId);
            if (!selectUserTrueName.IsNullOrEmpty())
            {
                allotFlowFilter = allotFlowFilter.And(m => m.SurveyUserName.Contains(selectUserTrueName) || m.SurveyUserTrueName.Contains(selectUserTrueName));
            }
            if (status > 0)
            {
                allotFlowFilter = allotFlowFilter.And(m => m.StateCode == status);
            }
            if (startDate.HasValue && endDate.HasValue)
            {
                var date = endDate.Value.AddDays(1);
                allotFlowFilter = allotFlowFilter.And(m => m.StateDate >= startDate && m.StateDate <= date);
            }
            else if (endDate.HasValue)
            {
                var date = endDate.Value.AddDays(1);
                allotFlowFilter = allotFlowFilter.And(m => m.StateDate <= date);
            }
            else if (startDate.HasValue)
            {
                allotFlowFilter = allotFlowFilter.And(m => m.StateDate >= startDate);
            }
            #endregion

            #region 小组权限
            //根据操作权限
            if (functionCodes != null && functionCodes.Contains(SYSCodeManager.FunOperCode_3.ToString()))//查看公司全部(管理员+分配人+审核人)
            {
            }
            else if (functionCodes != null && functionCodes.Contains(SYSCodeManager.FunOperCode_2.ToString()))//查看小组内(组长)
            {
                var nowDepartmentUser = _unitOfWork.DepartmentUserRepository.Get(m => m.FxtCompanyID == loginUserInfo.FxtCompanyId
                    && m.CityID == loginUserInfo.NowCityId && m.UserName == loginUserInfo.UserName).FirstOrDefault();
                if (nowDepartmentUser != null)
                {
                    //小组下所有用户
                    var departmentUser = _unitOfWork.DepartmentUserRepository.Get()
                                            .Where(m => m.FxtCompanyID == loginUserInfo.FxtCompanyId && m.CityID == loginUserInfo.NowCityId
                                            && m.DepartmentID == nowDepartmentUser.DepartmentID);
                    allotFlowFilter = allotFlowFilter.And(m => departmentUser.Any(d => d.UserName.Contains(m.UserName))
                        || departmentUser.Any(d => d.UserName.Contains(m.SurveyUserName))
                        || m.SurveyUserName == loginUserInfo.UserName || m.UserName == loginUserInfo.UserName);
                }
            }
            else//查看自己(查勘员)
            {
                allotFlowFilter = allotFlowFilter.And(m => m.UserName == loginUserInfo.UserName || m.SurveyUserName == loginUserInfo.UserName);
            }
            #endregion

            Expression<Func<AllotSurvey, bool>> allotSurveyFilter = null;
            Expression<Func<AllotSurvey, object>> orderby = m => m.StateDate;
            int count = 0;
            var list = _unitOfWork.ProjectAllotFlowSurveyService.FindAll(projectFilter, allotFlowFilter, allotSurveyFilter, orderby, pageIndex, pageSize, out count).ToList();

            var areaList = DataCenterAreaApi.GetAreaByCityId(loginUserInfo.NowCityId, loginUserInfo.UserName, loginUserInfo.SignName, loginUserInfo.AppList);
            list.ForEach((m) =>
            {
                var area = areaList.Where(a => a.AreaId == m.AreaID).FirstOrDefault();
                m.AreaName = area == null ? "" : area.AreaName;
                var subAreaList = DataCenterAreaApi.GetSubAreaByAreaId(m.AreaID, loginUserInfo.UserName, loginUserInfo.SignName, loginUserInfo.AppList);
                var subArea = subAreaList.Where(a => a.SubAreaId == m.SubAreaId).FirstOrDefault();
                m.SubAreaName = subArea == null ? "" : subArea.SubAreaName;
            });

            result.Data = new { Count = count, List = list, StateCode = status };
            return AjaxJson(result);

        }
        /// <summary>
        /// 撤销任务
        /// </summary>
        /// <param name="allotIds"></param>
        /// <returns></returns>
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX, NowFunctionPageUrl = WebCommon.Url_AllotFlowInfo_AllotFlowManager_1035002, AndNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_15 })]
        public ActionResult AllotFlowManager_CancelAllotFlow_Api(string allotIds, UserCenter_LoginUserInfo loginUserInfo)
        {
            AjaxResult result = new AjaxResult("撤销任务成功");
            if (allotIds.IsNullOrEmpty())
            {
                result.Result = false;
                result.Message = "选择已分配的任务";
                return AjaxJson(result);
            }
            //数据提交操作
            long[] aIds = allotIds.Split(',').ConvertToLongs();
            result.Result = _unitOfWork.AllotFlowService.SetAllotStatus(loginUserInfo.UserName, loginUserInfo.NowCityId, aIds, SYSCodeManager.STATECODE_2
                , SYSCodeManager.STATECODE_1, " 从调度中心 <span class=\"red\">撤销分配</span>", userTrueName: loginUserInfo.TrueName) > 0;
            return AjaxJson(result);
        }
        /// <summary>
        /// 撤销查勘
        /// </summary>
        /// <param name="allotIds"></param>
        /// <returns></returns>
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX, NowFunctionPageUrl = WebCommon.Url_AllotFlowInfo_AllotFlowManager_1035004, AndNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_16 })]
        public ActionResult AllotFlowManager_CancelSurvey_Api(string allotIds, UserCenter_LoginUserInfo loginUserInfo)
        {
            AjaxResult result = new AjaxResult("撤销查勘成功");
            if (allotIds.IsNullOrEmpty())
            {
                result.Result = false;
                result.Message = "选择查勘中的任务";
                return AjaxJson(result);
            }
            //数据提交操作
            long[] aIds = allotIds.Split(',').ConvertToLongs();
            result.Result = _unitOfWork.AllotFlowService.SetAllotStatus(loginUserInfo.UserName, loginUserInfo.NowCityId, aIds, SYSCodeManager.STATECODE_4
                , SYSCodeManager.STATECODE_2, " 从调度中心 <span class=\"red\">撤销查勘</span>", userTrueName: loginUserInfo.TrueName) > 0;
            return AjaxJson(result);
        }
        /// <summary>
        /// 删除任务
        /// </summary>
        /// <returns></returns>
        //[Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX, NowFunctionPageUrl = WebCommon.Url_AllotFlowInfo_AllotFlowManager_1035001, AndNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_14 })]
        public ActionResult Delete(string allotIds, UserCenter_LoginUserInfo loginUserInfo)
        {
            AjaxResult result = new AjaxResult();
            result.Result = false;

            if (string.IsNullOrEmpty(allotIds))
            {
                result.Message = "请选择要删除的任务";
                return AjaxJson(result);
            }

            //List<int> functionCodes = _unitOfWork.FunctionService.GetAllBy(loginUserInfo.UserName, loginUserInfo.FxtCompanyId, loginUserInfo.NowCityId, WebCommon.Url_AllotFlowInfo_AllotFlowManager_1035001)
            //    .Select(m => m.FunctionCode).ToList();

            //数据提交操作
            long[] _allotIds = allotIds.Split(',').ConvertToLongs();
            foreach (var item in _allotIds)
            {
                var allotFlow = _unitOfWork.AllotFlowRepository.GetById(item);
                _unitOfWork.AllotFlowRepository.Delete(item);
                //删除楼盘
                var project = _unitOfWork.ProjectRepository.GetBy(m => m.ProjectId == allotFlow.DatId);
                if (project != null)
                {
                    _unitOfWork.ProjectRepository.Delete(allotFlow.DatId);
                    //删除楼栋
                    var buildings = _unitOfWork.BuildingRepository.Get(m => m.ProjectId == project.ProjectId);
                    if (buildings != null && buildings.Count() > 0)
                    {
                        foreach (var building in buildings)
                        {
                            _unitOfWork.BuildingRepository.Delete(building.BuildingId);
                            //删除单元室号
                            _unitOfWork.HouseRepository.Delete(m => m.BuildingId == building.BuildingId);
                            //删除房号
                            _unitOfWork.HouseDetailsRepository.Delete(m => m.BuildingId == building.BuildingId);
                        }
                    }
                }
                _unitOfWork.AllotSurveyRepository.Delete(m => m.AllotId == item);
                _unitOfWork.CheckRepository.Delete(m => m.AllotId == item);
            }
            _unitOfWork.Commit();
            result.Result = true;
            result.Message = "删除成功！";
            return AjaxJson(result);
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="allotIds"></param>
        /// <returns></returns>
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.OPEN, NowFunctionPageUrl = WebCommon.Url_AllotFlowInfo_AllotFlowManager_1035008, AndNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_13 })]
        public ActionResult Export(string allotIds)
        {
            DataTable table = new DataTable();
            table.Columns.Add("楼盘名称");
            table.Columns.Add("楼栋名称");
            table.Columns.Add("房号名称");
            table.Columns.Add("物理层");
            table.Columns.Add("实际层");
            table.Columns.Add("面积");
            table.Columns.Add("朝向");
            table.Columns.Add("户型");
            table.Columns.Add("噪音情况");
            table.Columns.Add("用途");
            table.Columns.Add("景观");
            table.Columns.Add("户型结构");
            table.Columns.Add("通风采光");
            table.Columns.Add("备注");
            int index = 0;

            var front = SYSCodeManager.GetFrontCodeList();
            var houseType = SYSCodeManager.GetHouseTypeCodeList();
            var noiseCode = SYSCodeManager.NoiseManager();
            var purposeCode = SYSCodeManager.HousePurposeCodeManager();
            var sightCode = SYSCodeManager.GetSightCodeList();
            var structureCode = SYSCodeManager.StructureCodeManager();
            var VDCode = SYSCodeManager.VDCodeManager();

            var ids = allotIds.Split(',').ConvertToLongs().ToList();
            var allotList = _unitOfWork.AllotFlowRepository.Get(m => ids.Contains(m.Id));
            var houses = allotList.SelectMany(m => m.Project.Buildings.SelectMany(b => b.HouseDetails.Select(h =>
                new
                {
                    ProjectName = m.Project.ProjectName,
                    BuildingName = b.BuildingName,
                    HouseName = h.HouseName,
                    BuildArea = h.BuildArea,
                    FloorNo = h.FloorNo,
                    NominalFloor = h.NominalFloor,
                    Remark = h.Remark,
                    FrontCode = h.FrontCode,
                    HouseTypeCode = h.HouseTypeCode,
                    NoiseCode = h.NoiseCode,
                    PurposeCode = h.PurposeCode,
                    SightCode = h.SightCode,
                    StructureCode = h.StructureCode,
                    VDCode = h.VDCode
                }))).ToList()
                .Select((h) =>
                {
                    var fc = front.FirstOrDefault(f => f.Code == h.FrontCode);
                    var ht = houseType.FirstOrDefault(f => f.Code == h.HouseTypeCode);
                    var nc = noiseCode.FirstOrDefault(f => f.Code == h.NoiseCode);
                    var pc = purposeCode.FirstOrDefault(f => f.Code == h.PurposeCode);
                    var sc = sightCode.FirstOrDefault(f => f.Code == h.SightCode);
                    var stc = structureCode.FirstOrDefault(f => f.Code == h.StructureCode);
                    var vdc = VDCode.FirstOrDefault(f => f.Code == h.VDCode);
                    DataRow row = table.NewRow();
                    row[0] = h.ProjectName;
                    row[1] = h.BuildingName;
                    row[2] = h.HouseName;
                    row[3] = h.FloorNo;
                    row[4] = h.NominalFloor;
                    row[5] = h.BuildArea;
                    row[6] = fc != null ? fc.CodeName : "";
                    row[7] = ht != null ? ht.CodeName : "";
                    row[8] = nc != null ? nc.CodeName : "";
                    row[9] = pc != null ? pc.CodeName : "";
                    row[10] = sc != null ? sc.CodeName : "";
                    row[11] = stc != null ? stc.CodeName : "";
                    row[12] = vdc != null ? vdc.CodeName : "";
                    row[13] = h.Remark;
                    table.Rows.Add(row);
                    index++;
                    return new ExportHouseDto()
                    {
                        ProjectName = h.ProjectName,
                        BuildingName = h.BuildingName,
                        HouseName = h.HouseName,
                        BuildArea = h.BuildArea,
                        FloorNo = h.FloorNo,
                        NominalFloor = h.NominalFloor,
                        Remark = h.Remark,
                        FrontCodeName = fc != null ? fc.CodeName : "",
                        HouseTypeCodeName = ht != null ? ht.CodeName : "",
                        NoiseCodeName = nc != null ? nc.CodeName : "",
                        PurposeCodeName = pc != null ? pc.CodeName : "",
                        SightCodeName = sc != null ? sc.CodeName : "",
                        StructureCodeName = stc != null ? stc.CodeName : "",
                        VDCodeName = vdc != null ? vdc.CodeName : "",
                    };
                }
                ).ToList();
            #region header 信息
            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            curContext.Response.AddHeader("content-disposition",
                                             "attachment;filename*=UTF-8''" +
                                             System.Web.HttpUtility.UrlEncode("批量导出楼盘楼栋房号信息", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xlsx");
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            curContext.Response.Charset = "UTF-8";
            #endregion

            using (var ms = ExcelHandle.RenderToExcel(table))
            {
                return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
            }
        }
        #endregion

        #region (AssignAllotToUser.cshtml)
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.OPEN, NowFunctionPageUrl = WebCommon.Url_AllotFlowInfo_AssignAllotToUser, AndNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_14 })]
        public ActionResult AssignAllotToUser(string allotIds, UserCenter_LoginUserInfo loginUserInfo)
        {
            var departmentList = _unitOfWork.DepartmentRepository.Get(m => (m.FxtCompanyId == loginUserInfo.FxtCompanyId || m.FxtCompanyId == 0)
                            && (m.FK_CityId == 0 || m.FK_CityId == loginUserInfo.NowCityId) && m.DValid == 1);
            ViewBag.DepartmentList = departmentList;
            ViewBag.NowDepartment = "-1";
            ViewBag.ViewType = "my";
            ViewBag.NowUserName = loginUserInfo.UserName;
            ViewBag.AllotIds = allotIds;
            //获取当前用户在此页面所有的操作权限
            List<int> functionCodes = _unitOfWork.FunctionService.GetAllBy(loginUserInfo.UserName, loginUserInfo.FxtCompanyId, loginUserInfo.NowCityId, WebCommon.Url_AllotFlowInfo_AssignAllotToUser)
                .Select(m => m.FunctionCode).ToList();

            var nowDepartment = _unitOfWork.DepartmentUserRepository.Get(m => m.FxtCompanyID == loginUserInfo.FxtCompanyId
    && m.CityID == loginUserInfo.NowCityId && m.UserName == loginUserInfo.UserName).FirstOrDefault();
            if (functionCodes.Contains(SYSCodeManager.FunOperCode_3))
            {
                ViewBag.NowDepartment = "0";
                ViewBag.ViewType = "all";
            }
            else if (functionCodes.Contains(SYSCodeManager.FunOperCode_2))
            {
                if (nowDepartment != null)
                {
                    ViewBag.NowDepartment = nowDepartment.DepartmentID.ToString();
                }
                ViewBag.ViewType = "department";
            }
            else
            {
                if (nowDepartment != null)
                {
                    ViewBag.NowDepartment = nowDepartment.DepartmentID.ToString();
                }
                ViewBag.ViewType = "my";
            }
            return View();
        }
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="keyWord"></param>
        /// <param name="roleId"></param>
        /// <param name="departmentId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="isGetCount"></param>
        /// <returns></returns>
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX, NowFunctionPageUrl = WebCommon.Url_AllotFlowInfo_AssignAllotToUser, AndNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_14 })]
        public ActionResult AssignAllotToUser_GetList_Api(string keyWord, string roleId, string departmentId, int pageIndex, int pageSize, int isGetCount, UserCenter_LoginUserInfo loginUserInfo)
        {
            int? _roleId = null;
            int? _departmentId = null;
            bool _isGetCount = true;
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
            int count = 0;
            List<UserInfoJoinRoleJoinDepartment> list = UserInfoManager.GetUserInfoJoinRoleJoinDepartmentByRoleIdAndDepartmentIdAndUserName(
                loginUserInfo.NowCityId, loginUserInfo.FxtCompanyId, keyWord, _roleId, _departmentId, pageIndex, pageSize,
                out count, loginUserInfo.UserName, loginUserInfo.SignName, loginUserInfo.AppList, _isGetCount);
            string json = "{{\"Count\":{0},\"List\":{1}}}";
            json = string.Format(json, count, list.ToJSONjss());
            Response.Write(json.MvcResponseJson());
            Response.End();
            return null;
        }
        /// <summary>
        /// 提交分配
        /// </summary>
        /// <param name="allotIds"></param>
        /// <param name="selectUserName"></param>
        /// <returns></returns>
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX, NowFunctionPageUrl = WebCommon.Url_AllotFlowInfo_AssignAllotToUser, AndNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_14 })]
        public ActionResult AssignAllotToUser_SubmitData_Api(string allotIds, string selectUserName, string selectUserTrueName, UserCenter_LoginUserInfo loginUserInfo)
        {
            AjaxResult result = new AjaxResult("分配成功");
            result.Result = false;
            if (allotIds.IsNullOrEmpty())
            {
                result.Message = "请选择用户";
                return AjaxJson(result);
            }
            if (string.IsNullOrEmpty(allotIds))
            {
                result.Message = "请选择要分配的任务";
                return AjaxJson(result);
            }
            //获取选择的用户信息
            //PriviDepartmentUser pdu = null;
            //IList<SYSRoleUser> roleUserList = new List<SYSRoleUser>();
            //UserCenter_UserInfo selectUser = UserInfoManager.GetUserInfoByUserName(cityId, companyId, selectUserName, loginUserInfo.UserName, loginUserInfo.SignName, appList, out pdu, out roleUserList);
            ////获取当前用户对当前页面拥有的权限
            List<int> functionCodes = _unitOfWork.FunctionService.GetAllBy(loginUserInfo.UserName, loginUserInfo.FxtCompanyId, loginUserInfo.NowCityId, WebCommon.Url_AllotFlowInfo_AssignAllotToUser)
                .Select(m => m.FunctionCode).ToList();
            //WebUserHelp.GetNowPageFunctionCodes(loginUserInfo.UserName, loginUserInfo.FxtCompanyId, WebCommon.Url_AllotFlowInfo_AllotFlowManager);

            var nowDepartment = _unitOfWork.DepartmentUserRepository.Get(m => m.FxtCompanyID == loginUserInfo.FxtCompanyId
                                && m.CityID == loginUserInfo.NowCityId && m.UserName == loginUserInfo.UserName).FirstOrDefault();
            var pdu = _unitOfWork.DepartmentUserRepository.Get(m => m.FxtCompanyID == loginUserInfo.FxtCompanyId
                                && m.CityID == loginUserInfo.NowCityId && m.UserName == selectUserName).FirstOrDefault();
            //PriviDepartmentUser nowDepartment = PriviDepartmentUserManager.GetDepartmentUserByUserName(cityId, companyId, loginUserInfo.UserName);
            if (functionCodes.Contains(SYSCodeManager.FunOperCode_3))//全部
            {
            }
            else if (functionCodes.Contains(SYSCodeManager.FunOperCode_2))//小组内
            {
                if (pdu == null || nowDepartment.DepartmentID != pdu.DepartmentID)
                {
                    result.Message = "您无此操作权限";
                    return AjaxJson(result);
                }
            }
            else//自己
            {
                if (selectUserName != loginUserInfo.UserName)
                {
                    result.Message = "您无此操作权限";
                    return AjaxJson(result);
                }
            }

            //数据提交操作
            long[] _allotIds = allotIds.Split(',').ConvertToLongs();
            result.Result = _unitOfWork.AllotFlowService.SetAllotStatus(loginUserInfo.UserName, loginUserInfo.NowCityId, _allotIds, SYSCodeManager.STATECODE_1
                , SYSCodeManager.STATECODE_2, "<span class=\"red\">分配给：</span>" + (selectUserTrueName.IsNullOrEmpty() ? selectUserName : selectUserTrueName)
                , loginUserInfo.TrueName, selectUserName, selectUserTrueName) > 0;

            //result.Result = DatAllotFlowManager.AssignAllotFlow(loginUserInfo.UserName, loginUserInfo.TrueName, loginUserInfo.FxtCompanyId, loginUserInfo.NowCityId, _allotIds, selectUserName, selectUserTrueName, out message) > 0;
            result.Result = true;
            return AjaxJson(result);

        }
        #endregion

        #region(AllotDetailed.cshtml)
        [Common.AuthorizeFilterAttribute(NowFunctionPageUrl = WebCommon.Url_AllotFlowInfo_AllotDetailed, OrNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_1, SYSCodeManager.FunOperCode_2, SYSCodeManager.FunOperCode_3 })]
        public ActionResult AllotDetailed(long allotId)
        {
            int cityId = WebUserHelp.GetNowCityId();
            UserCenter_LoginUserInfo loginUser = WebUserHelp.GetNowLoginUser();
            DatAllotFlow allot = DatAllotFlowManager.GetDatAllotFlowById(allotId);
            //log.Info("-data----------------1");
            if (allot == null)
            {
                return WebUserHelp.GetAction404Page();
            }
            ViewBag.ProjectId = 0;
            ViewBag.AllotId = 0;
            ViewBag.Status = 0;
            //log.Info("-data----------------2");
            if (allot != null && allot.DatType == SYSCodeManager.DATATYPECODE_1)
            {
                ViewBag.AllotId = allot.id;
                ViewBag.ProjectId = allot.DatId;
                ViewBag.Status = allot.StateCode;
            }
            //获取当前用户对任务列表页拥有的权限
            List<int> nowFunctionCodes = WebUserHelp.GetNowPageFunctionCodes(loginUser.UserName, loginUser.FxtCompanyId, WebCommon.Url_AllotFlowInfo_AllotDetailed);
            //登陆用户所在小组
            PriviDepartmentUser loginDepartment = PriviDepartmentUserManager.GetDepartmentUserByUserName(cityId, loginUser.FxtCompanyId, loginUser.UserName);
            //当前信息的发起用户所在小组
            PriviDepartmentUser infoStartDepartment = PriviDepartmentUserManager.GetDepartmentUserByUserName(cityId, allot.FxtCompanyId, allot.UserName);
            //当前信息的用户所在小组
            PriviDepartmentUser infoDepartment = PriviDepartmentUserManager.GetDepartmentUserByUserName(cityId, allot.FxtCompanyId, allot.SurveyUserName);
            #region 验证查看权限
            //是否用于查看权限
            if (!WebUserHelp.CheckNowPageViewFunctionCode(nowFunctionCodes.ToArray(), loginUser.UserName, allot.UserName, allot.SurveyUserName,
                loginDepartment == null ? 0 : loginDepartment.DepartmentID, infoStartDepartment == null ? 0 : infoStartDepartment.DepartmentID,
                infoDepartment == null ? 0 : infoDepartment.DepartmentID))
            {
                //log.Info("-data----------------3");
                return WebUserHelp.GetActionNotRightPage();//无权限
            }
            #endregion
            //获取当前用户对当前页拥有的权限
            //List<int> nowFunctionCodes2 = WebUserHelp.GetNowPageFunctionCodes(loginUser.UserName, loginUser.FxtCompanyId, WebCommon.Url_AllotFlowInfo_AllotDetailed);
            List<int> nowFunctionCodes2 = WebUserHelp.GetNowPageFunctionCodes(loginUser.UserName, loginUser.FxtCompanyId, WebCommon.Url_AllotFlowInfo_AllotDetailed);
            #region 验证操作权限
            ViewBag.Status1 = SYSCodeManager.STATECODE_5;
            ViewBag.Status2_1 = SYSCodeManager.STATECODE_6;
            ViewBag.Status2 = SYSCodeManager.STATECODE_7;
            ViewBag.Status3_1 = SYSCodeManager.STATECODE_8;
            ViewBag.Status3 = SYSCodeManager.STATECODE_9;
            ViewBag.Status4 = SYSCodeManager.STATECODE_10;
            ViewBag.IsRight = 0;
            //当前状态:已查勘(操作-自审)
            if (allot.StateCode == SYSCodeManager.STATECODE_5)
            {
                if (loginUser.UserName == allot.SurveyUserName)
                {
                    ViewBag.IsRight = 1;
                }
            }
            //当前状态:自审不通过(操作-撤回到已分配(待查勘))
            if (allot.StateCode == SYSCodeManager.STATECODE_7)
            {
                if (loginUser.UserName == allot.SurveyUserName)
                {
                    ViewBag.IsRight = 1;
                }
            }
            //当前状态:自审通过or审核不通过or审核通过(操作-审核or撤回到已查勘or入库)
            if (allot.StateCode == SYSCodeManager.STATECODE_6 || ViewBag.Status == SYSCodeManager.STATECODE_9 || ViewBag.Status == SYSCodeManager.STATECODE_8)
            {
                if (WebUserHelp.CheckNowPageAuditFunctionCode(nowFunctionCodes2.ToArray(), loginUser.UserName, allot.SurveyUserName,
                loginDepartment == null ? 0 : loginDepartment.DepartmentID, infoDepartment == null ? 0 : infoDepartment.DepartmentID))
                {
                    ViewBag.IsRight = 1;
                }
            }
            #endregion
            //获取采集时间
            ViewBag.DataTime = null;
            DatAllotSurvey allotSurvey = DatAllotSurveyManager.GetDatAllotSurveyLastByAllotIdAndStateCode(cityId, allotId, SYSCodeManager.STATECODE_5);
            if (allotSurvey != null && allotSurvey.StateDate != null)
            {
                ViewBag.DataTime = allotSurvey.StateDate;
            }
            //获取任务备注
            ViewBag.Remark = allot.Remark;
            //获取审核信息
            ViewBag.DatCheck = new DatCheck { CheckRemark1 = "", CheckRemark2 = "" };
            DatCheck datCheck = DATCheckManager.GetCheckByAllotId(allotId);
            if (datCheck != null)
            {
                ViewBag.DatCheck = datCheck;
            }

            return View();
        }
        /// <summary>
        /// 获取楼盘信息+楼栋列表
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult AllotDetailed_GetProjectInfo_Api(int projectId, UserCenter_LoginUserInfo loginUser)
        {
            string json = "null";
            int cityId = WebUserHelp.GetNowCityId();
            //楼盘信息
            DATProject project = DATProjectManager.GetProjectByProjectId(projectId, cityId);
            if (project == null)
            {
                Response.Write(json.MvcResponseJson());
                Response.End();
            }

            int photoCount = _unitOfWork.P_PhotoRepository.Get(m => m.ProjectId == projectId && m.CityId == cityId && (!m.BuildingId.HasValue || m.BuildingId == 0) && m.FxtCompanyId == loginUser.FxtCompanyId).Count();
            string statusName = "";
            SYSCode code = SYSCodeManager.GetAllotStatusCode(Convert.ToInt32(project.Status));
            if (code != null)
            {
                statusName = code.CodeName;
            }
            //楼栋信息
            IList<DATBuilding> buildingList = DATBuildingManager.GetBuildingByProjectId(projectId, cityId);
            var bList = new List<object>();
            foreach (DATBuilding b in buildingList)
            {
                var bObj = new
                {
                    fxtcompanyid = b.FxtCompanyId,
                    buildingid = b.BuildingId,
                    buildingname = b.BuildingName,
                    cityid = b.CityID,
                    doorplate = b.Doorplate,
                    elevatorrate = b.ElevatorRate,
                    totalfloor = Convert.ToInt32(b.TotalFloor)
                    ,
                    othername = b.OtherName,
                    structurecode = Convert.ToInt32(b.StructureCode),
                    locationcode = Convert.ToInt32(b.LocationCode),
                    averageprice = b.AveragePrice == null ? "" : b.AveragePrice.ToString(),
                    builddate = b.BuildDate == null ? "" : Convert.ToDateTime(b.BuildDate).ToString("yyyy-MM-dd"),
                    iselevator = b.IsElevator,
                    pricedetail = b.PriceDetail,
                    sightcode = Convert.ToInt32(b.SightCode)
                };
                bList.Add(bObj);
            }
            var pInfo = new { projectname = project.ProjectName, photocount = photoCount, x = project.X == null ? 0 : project.X, y = project.Y == null ? 0 : project.Y, statusname = statusName, buildinglist = bList };
            json = pInfo.ToJSONjss();
            Response.Write(json.MvcResponseJson());
            Response.End();
            return null;

        }
        /// <summary>
        /// 获取楼盘的楼栋、房号数
        /// </summary>
        /// <param name="buildingId"></param>
        /// <returns></returns>
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult AllotDetailed_GetHouseList_Api(int buildingId)
        {
            var result = new AjaxResult("");

            List<HouseDto> dtoList = _unitOfWork.HouseRepository.Get(m => m.BuildingId == buildingId).ToList()
                .Select((o) =>
                {
                    var dto = Mapper.Map<House, HouseDto>(o);
                    dto.UnitNo = _unitOfWork.HouseService.GetUnitNoByUnitNoStr(o.UnitNo);
                    dto.HouseNo = _unitOfWork.HouseService.GetHouseNoByUnitNoStr(o.UnitNo);
                    return dto;
                }).ToList();

            result.Data = dtoList;

            return AjaxJson(result);
        }

        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult GetProjectBuildingHouseTotal(int projectId, UserCenter_LoginUserInfo loginUserInfo)
        {
            var result = new AjaxResult("");
            var project = _unitOfWork.ProjectRepository.GetById(projectId);
            if (project.FxtProjectId.HasValue && project.FxtProjectId > 0)
            {

                DatProjectTotal total = DataCenterProjectApi.GetProjectBuildingHouseTotal(loginUserInfo.NowCityId, project.FxtProjectId.Value, loginUserInfo.UserName, loginUserInfo.SignName, loginUserInfo.AppList);
                result.Data = total;
                if (total.buildingtotal == 0)
                {
                    result.Result = false;
                }
            }
            else
            {
                result.Result = false;
            }

            return AjaxJson(result);
        }

        /// <summary>
        /// 提交数据
        /// </summary>
        /// <param name="allotId"></param>
        /// <param name="actionType">myaudit:自审通过,mynotaudit:自审不通过,audit:审核通过notaudit:审核不通过,notsurvey:撤销到已分配,issurvey:撤销到已查勘</param>
        /// <returns></returns>
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult AllotDetailed_SubmitData_Api(long allotId, string actionType, string remark)
        {
            if (actionType.Equals("myaudit"))//自审通过
            {
                AllotDetailed_MyAudit(allotId, remark);
            }
            else if (actionType.Equals("mynotaudit"))//自审不通过
            {
                AllotDetailed_MyNotAudit(allotId, remark);
            }
            else if (actionType.Equals("audit"))//审核通过
            {
                AllotDetailed_Audit(allotId, remark);
            }
            else if (actionType.Equals("notaudit"))//审核不通过
            {
                AllotDetailed_NotAudit(allotId, remark);
            }
            else if (actionType.Equals("notsurvey"))//撤销到已分配(待查勘)
            {
                AllotDetailed_NotSurvey(allotId);
            }
            else if (actionType.Equals("issurvey"))//撤销到已查勘
            {
                AllotDetailed_IsSurvey(allotId);
            }
            else if (actionType.Equals("importdata"))//导入到运维中心
            {
                AllotDetailed_ImportData(allotId);
            }
            return null;
        }
        //自审通过
        public void AllotDetailed_MyAudit(long allotId, string remark)
        {
            string json = "";
            //获取当前登录用户信息
            UserCenter_LoginUserInfo loginUserInfo = WebUserHelp.GetNowLoginUser();
            int cityId = WebUserHelp.GetNowCityId();
            int companyId = loginUserInfo.FxtCompanyId;
            //数据提交操作
            string message = "";
            int result = DatAllotFlowManager.SetAllotMyThroughAudit(loginUserInfo.UserName, loginUserInfo.TrueName, companyId, cityId, new long[] { allotId }, remark, out message);
            if (result != 1)
            {
                Response.Write(json.MvcResponseJson(result: 0, message: message));
                Response.End();
            }
            Response.Write(json.MvcResponseJson(result: 1, message: message));
            Response.End();
        }
        //自审不通过
        public void AllotDetailed_MyNotAudit(long allotId, string remark)
        {
            string json = "";
            //获取当前登录用户信息
            UserCenter_LoginUserInfo loginUserInfo = WebUserHelp.GetNowLoginUser();
            int cityId = WebUserHelp.GetNowCityId();
            int companyId = loginUserInfo.FxtCompanyId;
            //数据提交操作
            string message = "";
            int result = DatAllotFlowManager.SetAllotMyNotThroughAudit(loginUserInfo.UserName, loginUserInfo.TrueName, companyId, cityId, new long[] { allotId }, remark, out message);
            if (result != 1)
            {
                Response.Write(json.MvcResponseJson(result: 0, message: message));
                Response.End();
            }
            Response.Write(json.MvcResponseJson(result: 1, message: message));
            Response.End();
        }
        //审核通过
        public void AllotDetailed_Audit(long allotId, string remark)
        {
            string json = "";
            //获取当前登录用户信息
            UserCenter_LoginUserInfo loginUserInfo = WebUserHelp.GetNowLoginUser();
            int cityId = WebUserHelp.GetNowCityId();
            int companyId = loginUserInfo.FxtCompanyId;
            //数据提交操作
            string message = "";
            int result = DatAllotFlowManager.SetAllotThroughAudit(loginUserInfo.UserName, loginUserInfo.TrueName, companyId, cityId, new long[] { allotId }, remark, out message);
            if (result != 1)
            {
                Response.Write(json.MvcResponseJson(result: 0, message: message));
                Response.End();
            }
            Response.Write(json.MvcResponseJson(result: 1, message: message));
            Response.End();
        }
        //审核不通过
        public void AllotDetailed_NotAudit(long allotId, string remark)
        {
            string json = "";
            //获取当前登录用户信息
            UserCenter_LoginUserInfo loginUserInfo = WebUserHelp.GetNowLoginUser();
            int cityId = WebUserHelp.GetNowCityId();
            int companyId = loginUserInfo.FxtCompanyId;
            //数据提交操作
            string message = "";
            int result = DatAllotFlowManager.SetAllotNotThroughAudit(loginUserInfo.UserName, loginUserInfo.TrueName, companyId, cityId, new long[] { allotId }, remark, out message);
            if (result != 1)
            {
                Response.Write(json.MvcResponseJson(result: 0, message: message));
                Response.End();
            }
            Response.Write(json.MvcResponseJson(result: 1, message: message));
            Response.End();
        }
        //撤销到已分配
        public void AllotDetailed_NotSurvey(long allotId)
        {
            string json = "";
            //获取当前登录用户信息
            UserCenter_LoginUserInfo loginUserInfo = WebUserHelp.GetNowLoginUser();
            int cityId = WebUserHelp.GetNowCityId();
            int companyId = loginUserInfo.FxtCompanyId;
            //数据提交操作
            string message = "";
            int result = DatAllotFlowManager.SetAllotNotSurvey(loginUserInfo.UserName, loginUserInfo.TrueName, companyId, cityId, new long[] { allotId }, "从自审不通过撤销到已分配(待查勘)", out message);
            if (result != 1)
            {
                Response.Write(json.MvcResponseJson(result: 0, message: message));
                Response.End();
            }
            Response.Write(json.MvcResponseJson(result: 1, message: message));
            Response.End();
        }
        //撤销到已查勘
        public void AllotDetailed_IsSurvey(long allotId)
        {
            string json = "";
            //获取当前登录用户信息
            UserCenter_LoginUserInfo loginUserInfo = WebUserHelp.GetNowLoginUser();
            int cityId = WebUserHelp.GetNowCityId();
            int companyId = loginUserInfo.FxtCompanyId;
            //数据提交操作
            string message = "";
            int result = DatAllotFlowManager.SetAllotIsSurvey(loginUserInfo.UserName, loginUserInfo.TrueName, companyId, cityId, new long[] { allotId }, "从审核不通过撤销到已查勘", out message);
            if (result != 1)
            {
                Response.Write(json.MvcResponseJson(result: 0, message: message));
                Response.End();
            }
            Response.Write(json.MvcResponseJson(result: 1, message: message));
            Response.End();
        }
        public void AllotDetailed_ImportData(long allotId)
        {
            string json = "";
            List<UserCenter_Apps> appList = new List<UserCenter_Apps>();
            //获取当前登录用户信息
            UserCenter_LoginUserInfo loginUserInfo = WebUserHelp.GetNowLoginUser(out appList);
            int cityId = WebUserHelp.GetNowCityId();
            int companyId = loginUserInfo.FxtCompanyId;
            //数据提交操作
            string message = "";
            int result = _unitOfWork.AllotFlowService.ImportToDataCenter(allotId, cityId, companyId, loginUserInfo.UserName, loginUserInfo.TrueName, loginUserInfo.SignName, appList, out message);
            //DatAllotFlowManager.ImportToDataCenter(allotId, cityId, companyId, loginUserInfo.UserName, loginUserInfo.SignName, appList, out message); //.SetAllotIsSurvey(loginUserInfo.UserName, companyId, cityId, new long[] { allotId }, "从审核不通过撤销到已查勘", out message);
            if (result != 1)
            {
                Response.Write(json.MvcResponseJson(result: 0, message: message));
                Response.End();
            }
            Response.Write(json.MvcResponseJson(result: 1, message: message));
            Response.End();
        }
        #endregion

        #region (EditProject.cshtml)
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.OPEN, NowFunctionPageUrl = WebCommon.Url_AllotFlowInfo_EditProject, OrNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_1, SYSCodeManager.FunOperCode_2, SYSCodeManager.FunOperCode_3 })]
        public ActionResult EditProject(long allotId, int projectId, UserCenter_LoginUserInfo loginUser, int type = 0)
        {
            int provinceId = WebUserHelp.GetNowProvinceId();
            DatAllotFlow allot = DatAllotFlowManager.GetDatAllotFlowById(allotId);
            DATProject project = DATProjectManager.GetProjectByProjectId(projectId, loginUser.NowCityId);
            if (allot == null || project == null)
            {
                return WebUserHelp.GetAction404Page();//无此页面
            }
            //登陆用户所在小组
            PriviDepartmentUser loginDepartment = PriviDepartmentUserManager.GetDepartmentUserByUserName(loginUser.NowCityId, loginUser.FxtCompanyId, loginUser.UserName);
            //当前信息的发起用户所在小组
            PriviDepartmentUser infoStartDepartment = PriviDepartmentUserManager.GetDepartmentUserByUserName(loginUser.NowCityId, allot.FxtCompanyId, allot.UserName);
            //当前信息的用户所在小组
            PriviDepartmentUser infoDepartment = PriviDepartmentUserManager.GetDepartmentUserByUserName(loginUser.NowCityId, allot.FxtCompanyId, allot.SurveyUserName);
            #region 验证查看权限
            //获取当前用户对任务列表页拥有的权限(验证查看权限)
            List<int> nowFunctionCodes = WebUserHelp.GetNowPageFunctionCodes(loginUser.UserName, loginUser.FxtCompanyId, WebCommon.Url_AllotFlowInfo_EditProject);
            //是否用于查看权限
            if (!WebUserHelp.CheckNowPageViewFunctionCode(nowFunctionCodes.ToArray(), loginUser.UserName, allot.UserName, allot.SurveyUserName,
                loginDepartment == null ? 0 : loginDepartment.DepartmentID, infoStartDepartment == null ? 0 : infoStartDepartment.DepartmentID,
                infoDepartment == null ? 0 : infoDepartment.DepartmentID))
            {
                return WebUserHelp.GetActionNotRightPage();//无权限
            }
            #endregion
            ViewBag.IsUpdateRight = 0;
            #region 验证修改权限
            //获取当前用户对当前页拥有的权限
            List<int> nowFunctionCodes2 = WebUserHelp.GetNowPageFunctionCodes(loginUser.UserName, loginUser.FxtCompanyId, WebCommon.Url_AllotFlowInfo_EditProject);

            if (WebUserHelp.CheckNowPageUpdateFunctionCode(nowFunctionCodes2.ToArray(), loginUser.UserName, allot.SurveyUserName,
                loginDepartment == null ? 0 : loginDepartment.DepartmentID, infoDepartment == null ? 0 : infoDepartment.DepartmentID, allot.SurveyUserName)
                && allot.StateCode != SYSCodeManager.STATECODE_10
                //&& (allot.StateCode == SYSCodeManager.STATECODE_5 || allot.StateCode == SYSCodeManager.STATECODE_7 || allot.StateCode == SYSCodeManager.STATECODE_9)
                )
            {
                ViewBag.IsUpdateRight = 1;//有权限
            }
            #endregion
            //获取物业管理
            //IList<LNKPAppendage> lnkpaList = LNKPAppendageManager.GetLNKPAppendageByProjectId(cityId, project.ProjectId);
            IList<LNKPCompany> lnkpcList = LNKPCompanyManager.GetLNKPCompanyByProjectId(loginUser.NowCityId, project.ProjectId);
            List<FxtApi_SYSArea> areaList = SYSAreaManager.GetAreaByCityId(loginUser.NowCityId, loginUser.UserName, loginUser.SignName, loginUser.AppList);
            List<FxtApi_SYSSubArea> subAreaList = SYSSubAreaManager.GetSubAreaByAreaId(project.AreaID, loginUser.UserName, loginUser.SignName, loginUser.AppList);

            //主用途
            ViewBag.PurposeCode = SYSCodeManager.PurposeCodeManager();
            //产权形式
            ViewBag.RightCode = SYSCodeManager.RightCodeManager();
            //物业管理质量
            ViewBag.ManagerQuality = SYSCodeManager.LevelManager();
            //停车状况
            ViewBag.ParkingStatusCode = SYSCodeManager.GetAllParkingStatusList();
            LNKPAppendage parkingstatus = LNKPAppendageManager.GetLNKPAppendageByProjectId(project.CityID, project.ProjectId).Where(m => m.AppendageCode == SYSCodeManager.APPENDAGECODE_14).FirstOrDefault();
            ViewBag.ParkingStatus = parkingstatus;

            //获取楼盘照片
            IList<PPhoto> photoList = _unitOfWork.P_PhotoRepository.Get(m => m.ProjectId == projectId && m.CityId == loginUser.NowCityId && (!m.BuildingId.HasValue || m.BuildingId == 0) && m.FxtCompanyId == loginUser.FxtCompanyId).ToList();
            //LNKPPhotoManager.GetLNKPPhotoByProjectId(projectId, cityId, loginUser.FxtCompanyId);

            ViewBag.CityId = loginUser.NowCityId;
            ViewBag.ProvinceId = provinceId;
            ViewBag.ProjectId = projectId;
            //ViewBag.LnkPAList = lnkpaList;
            ViewBag.lnkPCList = lnkpcList;
            ViewBag.CompanyTypeCode1 = SYSCodeManager.COMPANYTYPECODE_1;
            ViewBag.CompanyTypeCode2 = SYSCodeManager.COMPANYTYPECODE_4;
            ViewBag.AppendageCode6 = SYSCodeManager.APPENDAGECODE_6;
            ViewBag.AppendageCode13 = SYSCodeManager.APPENDAGECODE_13;
            ViewBag.AppendageCode14 = SYSCodeManager.APPENDAGECODE_14;
            ViewBag.AppendageCode15 = SYSCodeManager.APPENDAGECODE_15;
            ViewBag.AppendageCode16 = SYSCodeManager.APPENDAGECODE_16;
            ViewBag.AppendageCode17 = SYSCodeManager.APPENDAGECODE_17;
            ViewBag.AppendageCode19 = SYSCodeManager.APPENDAGECODE_19;
            ViewBag.AreaList = areaList;
            ViewBag.SubAreaList = subAreaList;
            ViewBag.PhotoCount = photoList.Count();
            ViewBag.PhotoList = photoList;
            ViewBag.HouseX = Convert.ToDecimal(project.X);
            ViewBag.HouseY = Convert.ToDecimal(project.Y);
            ViewBag.UserX = Convert.ToDecimal(allot.X);
            ViewBag.UserY = Convert.ToDecimal(allot.Y);
            ViewBag.AllotId = allot.id;

            ViewBag.type = type;
            return View(project);
        }

        public ActionResult GetSubAreaSelect(int areaId)
        {
            int cityId = WebUserHelp.GetNowCityId();
            List<UserCenter_Apps> appList = new List<UserCenter_Apps>();
            UserCenter_LoginUserInfo loginUser = WebUserHelp.GetNowLoginUser(out appList);
            var query = SYSSubAreaManager.GetSubAreaByAreaId(areaId, loginUser.UserName, loginUser.SignName, appList);
            var result = new List<SelectListItem>();
            result.Add(new SelectListItem
            {
                Value = "0",
                Text = "---片区--",
            });
            foreach (var item in query)
            {
                result.Add(new SelectListItem
                {
                    Value = item.SubAreaId.ToString(),
                    Text = item.SubAreaName,
                }
                );
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult EditProject_SubmitData_Api(int projectId, int cityId, string projectJson, string lnkpaListJson, string developersCompany, string managerCompany)
        {
            string json = "";
            if (string.IsNullOrEmpty(projectJson))
            {
                Response.Write(json.MvcResponseJson(result: 0, message: "楼盘信息异常"));
                Response.End();
                return null;
            }
            JObject jobj = JObject.Parse(projectJson);
            if (jobj == null)
            {
                Response.Write(json.MvcResponseJson(result: 0, message: "楼盘信息异常"));
                Response.End();
                return null;
            }
            IList<LNKPAppendage> lnkpaList = new List<LNKPAppendage>();
            if (!string.IsNullOrEmpty(lnkpaListJson))
            {
                lnkpaList = lnkpaListJson.ParseJSONList<LNKPAppendage>();
                lnkpaList = (lnkpaList as List<LNKPAppendage>).DecodeField<LNKPAppendage>();
            }
            int classcode = CAS.Common.MVC4.StringHelper.TryGetInt(jobj.Value<string>("parkingstatus"));
            lnkpaList.Add(new LNKPAppendage()
            {
                CityId = cityId,
                ProjectId = projectId,
                ClassCode = classcode,
                AppendageCode = SYSCodeManager.APPENDAGECODE_14
            });

            developersCompany = developersCompany.DecodeField();
            managerCompany = managerCompany.DecodeField();
            var project = JsonHelp.ParseJSONjss<Project>(projectJson);
            //行政区 + 楼盘名称||已有的projectid||没有入库
            var exists = _unitOfWork.ProjectRepository.Get(m =>
                    (
                        (m.ProjectName == project.ProjectName && m.AreaID == project.AreaID)
                        && m.ProjectId != projectId
                    )
                    && m.Status != SYSCodeManager.STATECODE_10 && m.Valid == 1
                ).FirstOrDefault();
            if (exists != null)
            {
                Response.Write(json.MvcResponseJson(result: 0, message: "楼盘已存在"));
                Response.End();
                return null;
            }
            string message = "";
            int result = DATProjectManager.UpdateProjectInfo(projectId, cityId, WebUserHelp.GetNowLoginUser().UserName, jobj, lnkpaList, developersCompany, managerCompany, out message);
            if (result != 1)
            {
                Response.Write(json.MvcResponseJson(result: 0, message: message));
                Response.End();
                return null;
            }

            Response.Write(json.MvcResponseJson(result: 1, message: ""));
            Response.End();
            return null;
        }

        [AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult UploadProjectImage(int projectId)
        {
            var photoTypeList = SYSCodeManager.GetPhotoTypeCodeList();
            ViewBag.photoTypeList = photoTypeList;
            ViewBag.projectId = projectId;

            var coount = _unitOfWork.P_PhotoRepository.Get(m => m.ProjectId == projectId && (!m.BuildingId.HasValue || m.BuildingId == 0)).Count();
            ViewBag.photocount = 10 - coount;
            return View();
        }
        /// <summary>
        /// 上传楼盘图片
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="photoTypeCode">图片类型</param>
        /// <returns></returns>
        [AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX)]
        public async Task<ActionResult> OnUploadProjectImage(int projectId, int photoTypeCode, UserCenter_LoginUserInfo loginUser)
        {
            AjaxResult result = new AjaxResult("上传图片成功！");

            var coount = _unitOfWork.P_PhotoRepository.Get(m => m.ProjectId == projectId && (!m.BuildingId.HasValue || m.BuildingId == 0)).Count();
            if (coount < 10)
            {
                var file = Request.Files[0];
                string filename = file.FileName;
                //获取正式数据文件根目录
                string basePath2 = CommonUtility.GetConfigSetting("upload_DataAcquisition");

                //获取或创建正式数据目录
                string searchPath2 = _unitOfWork.PhotoService.GetProjectPhotoPath(basePath2, loginUser.FxtCompanyId, projectId, loginUser.NowCityId);
                //string folder2 = System.Web.Hosting.HostingEnvironment.MapPath(searchPath2);
                //if (!Directory.Exists(folder2))
                //{
                //    Directory.CreateDirectory(folder2);
                //}

                //string path = Path.Combine(folder2, filename);//组织正式文件路径
                string path2 = searchPath2 + "/" + filename;//组织正式文件虚拟路径
                //file.SaveAs(path);

                //上传图片到oss
                var r = await OssHelp.UpFileAsync(file.InputStream, path2);

                _unitOfWork.P_PhotoRepository.Insert(new PPhoto()
                {
                    BuildingId = 0,
                    CityId = loginUser.NowCityId,
                    FxtCompanyId = loginUser.FxtCompanyId,
                    Path = path2,
                    PhotoDate = DateTime.Now,
                    PhotoName = filename,
                    PhotoTypeCode = photoTypeCode,
                    ProjectId = projectId,
                    Valid = 1
                });
                _unitOfWork.Commit();
            }
            else
            {
                result.Message = "楼盘图片不能大于10张";
                result.Result = false;
            }
            return AjaxJson(result);
        }

        [AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult DeleteProjectImage(List<int> ids)
        {
            var result = new AjaxResult("删除成功！");
            _unitOfWork.P_PhotoRepository.Delete(m => ids.Contains(m.Id));
            _unitOfWork.Commit();
            return AjaxJson(result);
        }

        /// <summary>
        /// 楼盘定位
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        [AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult UploadProjectXY(int projectId, decimal x, decimal y, UserCenter_LoginUserInfo loginUser)
        {
            var result = new AjaxResult("定位成功！");

            var p = _unitOfWork.ProjectRepository.GetById(projectId);
            p.X = x;
            p.Y = y;
            p.UpdateDateTime = DateTime.Now;
            p.SaveDateTime = DateTime.Now;
            p.SaveUser = loginUser.UserName;

            _unitOfWork.ProjectRepository.Update(p);
            _unitOfWork.Commit();
            return AjaxJson(result);
        }

        #endregion

        #region (EditBuilding.cshtml)
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.OPEN, NowFunctionPageUrl = WebCommon.Url_AllotFlowInfo_EditBuilding, OrNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_1, SYSCodeManager.FunOperCode_2, SYSCodeManager.FunOperCode_3 })]
        public ActionResult EditBuilding(long allotId, int buildingId, int type = 0)
        {
            int cityId = WebUserHelp.GetNowCityId();
            int provinceId = WebUserHelp.GetNowProvinceId();
            List<UserCenter_Apps> appList = new List<UserCenter_Apps>();
            UserCenter_LoginUserInfo loginUser = WebUserHelp.GetNowLoginUser(out appList);
            DatAllotFlow allot = DatAllotFlowManager.GetDatAllotFlowById(allotId);
            DATBuilding building = DATBuildingManager.GetBuildingByBuildingId(buildingId, cityId) ?? new DATBuilding() { ProjectId = Convert.ToInt32(allot.DatId) };
            if (allot == null)
            {
                return WebUserHelp.GetAction404Page();//无此页面
            }
            //登陆用户所在小组
            PriviDepartmentUser loginDepartment = PriviDepartmentUserManager.GetDepartmentUserByUserName(cityId, loginUser.FxtCompanyId, loginUser.UserName);
            //当前信息的发起用户所在小组
            PriviDepartmentUser infoStartDepartment = PriviDepartmentUserManager.GetDepartmentUserByUserName(cityId, allot.FxtCompanyId, allot.UserName);
            //当前信息的用户所在小组
            PriviDepartmentUser infoDepartment = PriviDepartmentUserManager.GetDepartmentUserByUserName(cityId, allot.FxtCompanyId, allot.SurveyUserName);
            #region 验证查看权限
            //获取当前用户对任务列表页拥有的权限(验证查看权限)
            List<int> nowFunctionCodes = WebUserHelp.GetNowPageFunctionCodes(loginUser.UserName, loginUser.FxtCompanyId, WebCommon.Url_AllotFlowInfo_EditBuilding);
            //是否用于查看权限
            if (!WebUserHelp.CheckNowPageViewFunctionCode(nowFunctionCodes.ToArray(), loginUser.UserName, allot.UserName, allot.SurveyUserName,
                loginDepartment == null ? 0 : loginDepartment.DepartmentID, infoStartDepartment == null ? 0 : infoStartDepartment.DepartmentID,
                infoDepartment == null ? 0 : infoDepartment.DepartmentID))
            {
                return WebUserHelp.GetActionNotRightPage();//无权限
            }
            #endregion

            //获取楼盘照片
            IList<PPhoto> photoList = _unitOfWork.P_PhotoRepository.Get(m => m.BuildingId == buildingId && m.BuildingId > 0).ToList();
            ViewBag.PhotoCount = photoList.Count();
            ViewBag.PhotoList = photoList;
            //用途
            ViewBag.PurposeCode = SYSCodeManager.PurposeCodeManager();
            //
            ViewBag.MaintenanceCode = SYSCodeManager.LevelManager();

            ViewBag.IsUpdateRight = 0;
            #region 验证修改权限
            //获取当前用户对当前页拥有的权限
            List<int> nowFunctionCodes2 = WebUserHelp.GetNowPageFunctionCodes(loginUser.UserName, loginUser.FxtCompanyId, WebCommon.Url_AllotFlowInfo_EditBuilding);
            if (WebUserHelp.CheckNowPageUpdateFunctionCode(nowFunctionCodes2.ToArray(), loginUser.UserName, allot.SurveyUserName,
                loginDepartment == null ? 0 : loginDepartment.DepartmentID, infoDepartment == null ? 0 : infoDepartment.DepartmentID, allot.SurveyUserName)
                && (allot.StateCode != SYSCodeManager.STATECODE_10))
            //&& (allot.StateCode == SYSCodeManager.STATECODE_5 || allot.StateCode == SYSCodeManager.STATECODE_7 || allot.StateCode == SYSCodeManager.STATECODE_9))
            {
                ViewBag.IsUpdateRight = 1;//有权限
            }
            #endregion
            ViewBag.CityId = cityId;
            ViewBag.ProvinceId = provinceId;
            ViewBag.BuildingId = buildingId;
            ViewBag.HouseX = Convert.ToDecimal(building.X);
            ViewBag.HouseY = Convert.ToDecimal(building.Y);
            ViewBag.AllotId = allot.id;
            ViewBag.hdProjectId = allot.DatId;

            ViewBag.Edit =

            ViewBag.type = type;
            return View(building);
        }
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult EditBuilding_SubmitData_Api(int projectId, int buildingId, int cityId, string buildingJson, UserCenter_LoginUserInfo user)
        {
            string json = "";
            if (string.IsNullOrEmpty(buildingJson))
            {
                Response.Write(json.MvcResponseJson(result: 0, message: "楼栋信息异常"));
                Response.End();
                return null;
            }
            JObject jobj = JObject.Parse(buildingJson);
            if (jobj == null)
            {
                Response.Write(json.MvcResponseJson(result: 0, message: "楼栋信息异常"));
                Response.End();
                return null;
            }
            string message = "";
            var building = JsonHelp.ParseJSONjss<Building>(buildingJson);
            if (buildingId > 0)
            {
                var exists = _unitOfWork.BuildingRepository.GetBy(m => m.BuildingName == building.BuildingName && m.BuildingId == building.BuildingId);
                if (exists != null)
                {
                    Response.Write(json.MvcResponseJson(result: 0, message: "楼栋已存在"));
                    Response.End();
                    return null;
                }

                int result = DATBuildingManager.UpdateBuildingInfo(buildingId, cityId, WebUserHelp.GetNowLoginUser().UserName, jobj, out message);
                if (result != 1)
                {
                    Response.Write(json.MvcResponseJson(result: 0, message: message));
                    Response.End();
                    return null;
                }
                DATBuilding b = DATBuildingManager.GetBuildingByBuildingId(buildingId, cityId);

                if (b != null)
                {
                    var bObj = new { buildingid = b.BuildingId, buildingname = b.BuildingName, cityid = b.CityID, doorplate = b.Doorplate, elevatorrate = b.ElevatorRate, totalfloor = Convert.ToInt32(b.TotalFloor) };
                    json = bObj.ToJSONjss();
                }
                Response.Write(json.MvcResponseJson(result: 1, message: ""));
                Response.End();
            }
            else
            {
                var exists = _unitOfWork.BuildingRepository.GetBy(m => m.BuildingName == building.BuildingName && m.CityID == user.NowCityId && m.FxtCompanyId == user.FxtCompanyId && m.ProjectId == projectId);
                if (exists != null)
                {
                    Response.Write(json.MvcResponseJson(result: 0, message: "楼栋已存在"));
                    Response.End();
                    return null;
                }

                building.Valid = 1;
                building.CityID = user.NowCityId;
                building.FxtCompanyId = user.FxtCompanyId;
                building.Creator = user.UserName;
                building.CreateTime = DateTime.Now;
                building.ProjectId = projectId;
                building.AppId = Guid.NewGuid();

                _unitOfWork.BuildingRepository.Insert(building);
            }
            _unitOfWork.Commit();
            json = building.BuildingId.ToString();

            Response.Write(json.MvcResponseJson(result: 1, message: ""));
            Response.End();
            return null;
        }

        [AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult UploadBuildingImage(int projectId, int buildingId)
        {
            var photoTypeList = SYSCodeManager.GetPhotoTypeCodeList();
            ViewBag.photoTypeList = photoTypeList;
            ViewBag.projectId = projectId;
            ViewBag.buildingId = buildingId;

            var coount = _unitOfWork.P_PhotoRepository.Get(m => m.ProjectId == projectId && m.BuildingId == buildingId).Count();
            ViewBag.photocount = 5 - coount;
            return View();
        }

        [AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX)]
        public async Task<ActionResult> OnUploadBuildingImage(int projectId, int buildingId, int photoTypeCode, UserCenter_LoginUserInfo loginUser)
        {
            AjaxResult result = new AjaxResult("上传图片成功！");

            var coount = _unitOfWork.P_PhotoRepository.Get(m => m.ProjectId == projectId && m.BuildingId == buildingId).Count();
            if (coount < 5)
            {
                var file = Request.Files[0];
                string filename = file.FileName;
                //获取正式数据文件根目录
                string basePath2 = CommonUtility.GetConfigSetting("upload_DataAcquisition");

                //获取或创建正式数据目录
                string searchPath2 = _unitOfWork.PhotoService.GetProjectPhotoPath(basePath2, loginUser.FxtCompanyId, projectId, loginUser.NowCityId) + "/" + buildingId;
                string folder2 = System.Web.Hosting.HostingEnvironment.MapPath(searchPath2);
                //if (!Directory.Exists(folder2))
                //{
                //    Directory.CreateDirectory(folder2);
                //}

                //string path = Path.Combine(folder2, filename);//组织正式文件路径
                string path2 = searchPath2 + "/" + filename;//组织正式文件虚拟路径
                //file.SaveAs(path);
                //上传图片到oss
                var r = await OssHelp.UpFileAsync(file.InputStream, path2);

                _unitOfWork.P_PhotoRepository.Insert(new PPhoto()
                {
                    BuildingId = buildingId,
                    CityId = loginUser.NowCityId,
                    FxtCompanyId = loginUser.FxtCompanyId,
                    Path = path2,
                    PhotoDate = DateTime.Now,
                    PhotoName = filename,
                    PhotoTypeCode = photoTypeCode,
                    ProjectId = projectId,
                    Valid = 1
                });
                _unitOfWork.Commit();
            }
            else
            {
                result.Message = "楼栋图片不能大于5张";
                result.Result = false;
            }
            return Json(result);
        }

        [AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult DeleteBuilding(int buildingId)
        {
            var result = new AjaxResult("删除成功");
            _unitOfWork.BuildingRepository.Delete(buildingId);
            _unitOfWork.HouseRepository.Delete(m => m.BuildingId == buildingId);
            _unitOfWork.HouseDetailsRepository.Delete(m => m.BuildingId == buildingId);
            _unitOfWork.Commit();
            return AjaxJson(result);
        }

        [AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult UploadBuildingXY(int buildingId, decimal x, decimal y, UserCenter_LoginUserInfo loginUser)
        {
            var result = new AjaxResult("定位成功！");

            var b = _unitOfWork.BuildingRepository.GetById(buildingId);
            b.X = x;
            b.Y = y;
            b.SaveDateTime = DateTime.Now;
            b.SaveUser = loginUser.UserName;

            _unitOfWork.BuildingRepository.Update(b);
            _unitOfWork.Commit();
            return AjaxJson(result);
        }
        #endregion

        #region (EditHouse.cshtml)
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.OPEN, NowFunctionPageUrl = WebCommon.Url_AllotFlowInfo_EditHouse, OrNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_1, SYSCodeManager.FunOperCode_2, SYSCodeManager.FunOperCode_3 })]
        public ActionResult EditHouse(long allotId, int buildingId, int houseId)
        {
            int cityId = WebUserHelp.GetNowCityId();
            int provinceId = WebUserHelp.GetNowProvinceId();
            List<UserCenter_Apps> appList = new List<UserCenter_Apps>();
            UserCenter_LoginUserInfo loginUser = WebUserHelp.GetNowLoginUser(out appList);
            DatAllotFlow allot = DatAllotFlowManager.GetDatAllotFlowById(allotId);
            DATHouse house = DATHouseManager.GetHouseByHouseId(houseId, cityId) ?? new DATHouse();
            if (allot == null)
            {
                return WebUserHelp.GetAction404Page();//无此页面
            }
            //登陆用户所在小组
            PriviDepartmentUser loginDepartment = PriviDepartmentUserManager.GetDepartmentUserByUserName(cityId, loginUser.FxtCompanyId, loginUser.UserName);
            //当前信息的发起用户所在小组
            PriviDepartmentUser infoStartDepartment = PriviDepartmentUserManager.GetDepartmentUserByUserName(cityId, allot.FxtCompanyId, allot.UserName);
            //当前信息的用户所在小组
            PriviDepartmentUser infoDepartment = PriviDepartmentUserManager.GetDepartmentUserByUserName(cityId, allot.FxtCompanyId, allot.SurveyUserName);
            #region 验证查看权限
            //获取当前用户对任务列表页拥有的权限(验证查看权限)
            List<int> nowFunctionCodes = WebUserHelp.GetNowPageFunctionCodes(loginUser.UserName, loginUser.FxtCompanyId, WebCommon.Url_AllotFlowInfo_EditHouse);
            //是否用于查看权限
            if (!WebUserHelp.CheckNowPageViewFunctionCode(nowFunctionCodes.ToArray(), loginUser.UserName, allot.UserName, allot.SurveyUserName,
                loginDepartment == null ? 0 : loginDepartment.DepartmentID, infoStartDepartment == null ? 0 : infoStartDepartment.DepartmentID,
                infoDepartment == null ? 0 : infoDepartment.DepartmentID))
            {
                return WebUserHelp.GetActionNotRightPage();//无权限
            }
            #endregion

            //户型结构
            ViewBag.StructureCode = SYSCodeManager.StructureCodeManager();
            //通风采光
            ViewBag.VDCode = SYSCodeManager.VDCodeManager();
            //噪音情况
            ViewBag.Noise = SYSCodeManager.NoiseManager();
            //用途
            ViewBag.PurposeCode = SYSCodeManager.HousePurposeCodeManager();

            ViewBag.IsUpdateRight = 0;
            #region 验证修改权限
            //获取当前用户对当前页拥有的权限
            List<int> nowFunctionCodes2 = WebUserHelp.GetNowPageFunctionCodes(loginUser.UserName, loginUser.FxtCompanyId, WebCommon.Url_AllotFlowInfo_EditHouse);
            if (WebUserHelp.CheckNowPageUpdateFunctionCode(nowFunctionCodes2.ToArray(), loginUser.UserName, allot.SurveyUserName,
                loginDepartment == null ? 0 : loginDepartment.DepartmentID, infoDepartment == null ? 0 : infoDepartment.DepartmentID, allot.SurveyUserName)
                && (allot.StateCode != SYSCodeManager.STATECODE_10))
            {
                ViewBag.IsUpdateRight = 1;//有权限
            }
            #endregion
            ViewBag.CityId = cityId;
            ViewBag.ProvinceId = provinceId;
            ViewBag.UnitNo = DATHouseManager.GetUnitNoByUnitNoStr(house.UnitNo);
            ViewBag.HouseNo = DATHouseManager.GetHouseNoByUnitNoStr(house.UnitNo);
            ViewBag.HouseId = houseId;
            ViewBag.BuildingId = buildingId;
            return View(house);
        }

        public ActionResult EditHouse_SubmitData_Api(House house, UserCenter_LoginUserInfo user)
        {
            AjaxResult result = new AjaxResult("修改成功");
            var building = _unitOfWork.BuildingRepository.GetById(house.BuildingId);
            //房号终止层不能大于楼栋总层数
            if (house.EndFloorNo > building.TotalFloor)
            {
                result.Message = "终止层不能大于楼栋的总层数";
                result.Result = false;
                return AjaxJson(result);
            }

            if (house.HouseId > 0)
            {
                house.SaveUser = user.UserName;
                house.SaveDateTime = DateTime.Now;
                //_unitOfWork.HouseRepository.Update(house);
            }
            else
            {
                house.Valid = 1;
                house.CityID = user.NowCityId;
                house.FxtCompanyId = user.FxtCompanyId;
                house.CreateTime = DateTime.Now;
                house.Creator = user.UserName;
                house.AppId = Guid.NewGuid();
                _unitOfWork.HouseRepository.Insert(house);
                result.Message = "新增成功";
            }

            _unitOfWork.Commit();
            result.Data = new { houseid = house.HouseId };
            return AjaxJson(result);
        }

        public ActionResult DeleteHouse(List<int> allotIds)
        {
            var result = new AjaxResult("删除成功！");
            _unitOfWork.HouseRepository.Delete(m => allotIds.Contains(m.HouseId));
            _unitOfWork.HouseDetailsRepository.Delete(m => allotIds.Contains(m.HouseId));
            _unitOfWork.Commit();
            return AjaxJson(result);
        }

        #endregion

        #region Excel
        /// <summary>
        /// Excel任务导入
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.OPEN)]
        public ActionResult ExcelIn()
        {
            return View();
        }
        /// <summary>
        /// Excel任务导入提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.OPEN)]
        public ActionResult ExcelInPost(UserCenter_LoginUserInfo loginUser)
        {
            string msg = string.Empty;
            string msgRow = string.Empty;
            string msgExistRow = string.Empty;//任务已存在
            string path = string.Empty;//文件存放路径

            try
            {
                if (Request.Files != null)
                {
                    string ml = Server.MapPath("../Files");
                    if (!System.IO.Directory.Exists(ml))
                        System.IO.Directory.CreateDirectory(ml);
                    //HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
                    HttpPostedFileBase file = Request.Files[0];
                    string fileName = string.Empty;
                    if (file != null)
                    {
                        //for (int i = 0; i < files.Count; i++)
                        //{
                        string filename = System.IO.Path.GetFileName(file.FileName);
                        if (!string.IsNullOrEmpty(filename))
                        {
                            string filetype = filename.Substring(filename.LastIndexOf(".") + 1);
                            string filenametime = System.IO.Path.GetFileName(filename.Substring(0, filename.LastIndexOf(".")))
                                + DateTime.Now.ToString("yyyyMMddHHMMss") + "." + filetype;
                            path = Server.MapPath("../Files") + "\\" + filenametime;
                            file.SaveAs(path);
                            //files[i].SaveAs(path);
                            if (filetype.Contains("xls") || filetype.Contains("xlsx"))
                            {//判断excel文件
                                //DataTable dt = GetExcelToDataTable(path, "楼盘", 22);
                                var excelHelper = new ExcelHandle(path);
                                DataTable dt = excelHelper.ExcelToDataTable("楼盘", true);
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    int rowsnum = 2;
                                    foreach (DataRow row in dt.Rows)
                                    {
                                        #region 字段处理
                                        string projectName = string.Empty;//楼盘名称
                                        if (dt.Columns.Contains("楼盘名称"))
                                        {
                                            projectName = GetExcelField(row["楼盘名称"]).ToString();//楼盘名称
                                        }
                                        string area = string.Empty;//行政区
                                        if (dt.Columns.Contains("行政区"))
                                        {
                                            area = GetExcelField(row["行政区"]).ToString();//楼盘名称
                                        }
                                        string purposeCode = string.Empty;//用途
                                        if (dt.Columns.Contains("主用途"))
                                        {
                                            purposeCode = GetExcelField(row["主用途"]).ToString();//楼盘名称
                                        }
                                        if (!string.IsNullOrEmpty(projectName) && !string.IsNullOrEmpty(area) && !string.IsNullOrEmpty(purposeCode))
                                        {//插入 
                                            Project project = new Project();
                                            project.CityID = loginUser.NowCityId;
                                            project.CreateTime = DateTime.Now;
                                            project.SaveDateTime = DateTime.Now;
                                            project.Creator = loginUser.UserName;
                                            project.SaveUser = loginUser.UserName;
                                            project.Valid = 1;
                                            project.X = default(decimal);
                                            project.Y = default(decimal);
                                            project.Status = 1035001;
                                            project.FxtCompanyId = loginUser.FxtCompanyId;

                                            if (dt.Columns.Contains("运维中心库楼盘ID") && StringHelp.CheckInteger(row["运维中心库楼盘ID"].ToString()))
                                            {
                                                //检查是否已存在任务
                                                project.FxtProjectId = Convert.ToInt32(row["运维中心库楼盘ID"]);//运维中心库楼盘ID
                                                Project checkProject = _unitOfWork.ProjectRepository.Get(m => m.FxtProjectId == project.FxtProjectId && m.CityID == project.CityID).FirstOrDefault();
                                                if (checkProject != null)
                                                {
                                                    msgExistRow += rowsnum + ",";
                                                    rowsnum++;
                                                    continue;
                                                }
                                            }
                                            if (dt.Columns.Contains("行政区"))
                                            {
                                                int areaid = GetAreaId(project.CityID, row["行政区"].ToString());
                                                if (areaid > 0)
                                                    project.AreaID = areaid;//行政区
                                                else
                                                {
                                                    msgRow += rowsnum + ",";
                                                    rowsnum++;
                                                    continue;
                                                }
                                            }
                                            if (dt.Columns.Contains("楼盘名称"))
                                            {
                                                project.ProjectName = GetExcelField(row["楼盘名称"]).ToString();//楼盘名称
                                                //检查是否已存在任务
                                                Project checkProject = _unitOfWork.ProjectRepository.Get(m => m.CityID == project.CityID &&
                                                (m.ProjectName == project.ProjectName && m.AreaID == project.AreaID) && m.Status != SYSCodeManager.STATECODE_10).FirstOrDefault();
                                                if (checkProject != null)
                                                {
                                                    msgExistRow += rowsnum + ",";
                                                    rowsnum++;
                                                    continue;
                                                }
                                            }
                                            else
                                            {
                                                msgRow += rowsnum + ",";
                                                rowsnum++;
                                                continue;
                                            }
                                            if (dt.Columns.Contains("楼盘别名"))
                                            {
                                                project.OtherName = GetExcelField(row["楼盘别名"]).ToString();//别名
                                            }
                                            if (dt.Columns.Contains("楼盘地址"))
                                            {
                                                project.Address = GetExcelField(row["楼盘地址"]).ToString();//地址
                                            }

                                            if (dt.Columns.Contains("片区"))
                                            {
                                                int subAreaid = GetSubAreaId(project.AreaID, row["片区"].ToString());
                                                if (subAreaid > 0)
                                                {
                                                    project.SubAreaId = subAreaid;//片区
                                                }
                                            }
                                            if (dt.Columns.Contains("主用途"))
                                            {//用途
                                                List<SYSCode> list = SYSCodeManager.PurposeCodeManager();
                                                var id = (from e in list
                                                          where e.CodeName == row["主用途"].ToString()
                                                          select e.Code).ToList();
                                                if (id != null && id.Count > 0)
                                                {
                                                    project.PurposeCode = Convert.ToInt32(id[0]);
                                                }
                                            }
                                            if (dt.Columns.Contains("产权形式"))
                                            {//产权形式
                                                List<SYSCode> list = SYSCodeManager.RightCodeManager();
                                                var id = (from e in list
                                                          where e.CodeName == row["产权形式"].ToString()
                                                          select e.Code).ToList();
                                                if (id != null && id.Count > 0)
                                                {
                                                    project.RightCode = Convert.ToInt32(id[0]);
                                                }
                                            }
                                            if (dt.Columns.Contains("物业管理质量"))
                                            {//物业管理质量
                                                List<SYSCode> list = SYSCodeManager.LevelManager();
                                                var id = (from e in list
                                                          where e.CodeName == row["物业管理质量"].ToString()
                                                          select e.Code).ToList();
                                                if (id != null && id.Count > 0)
                                                {
                                                    project.ManagerQuality = Convert.ToInt32(id[0]);
                                                }
                                            }
                                            PAppendage parkingStatus = new PAppendage();
                                            if (dt.Columns.Contains("停车状况"))
                                            {//停车状况
                                                IList<SYSCode> list = SYSCodeManager.GetAllParkingStatusList();
                                                var id = (from e in list
                                                          where e.CodeName == row["停车状况"].ToString()
                                                          select e.Code).ToList();
                                                if (id != null && id.Count > 0)
                                                {
                                                    //project.ParkingStatus = Convert.ToInt32(id[0]);
                                                    parkingStatus.AppendageCode = 2008014;
                                                    parkingStatus.CityId = project.CityID;
                                                    parkingStatus.ClassCode = Convert.ToInt32(id[0]);
                                                }
                                            }
                                            if (dt.Columns.Contains("竣工日期") && StringHelp.CheckIsDate(row["竣工日期"].ToString()))
                                            {
                                                project.EndDate = Convert.ToDateTime(row["竣工日期"]);//竣工日期
                                            }

                                            if (dt.Columns.Contains("总栋数") && StringHelp.CheckInteger(row["总栋数"].ToString()))
                                            {
                                                project.BuildingNum = Convert.ToInt32(row["总栋数"]);//总栋数
                                            }
                                            if (dt.Columns.Contains("总套数") && StringHelp.CheckInteger(row["总套数"].ToString()))
                                            {
                                                project.TotalNum = Convert.ToInt32(row["总套数"]);//总套数
                                            }
                                            if (dt.Columns.Contains("车位数") && StringHelp.CheckInteger(row["车位数"].ToString()))
                                            {
                                                project.ParkingNumber = Convert.ToInt32(row["车位数"]);//	车位数
                                            }
                                            if (dt.Columns.Contains("项目概况"))
                                            {
                                                project.Detail = row["项目概况"].ToString();//项目概况
                                            }
                                            if (dt.Columns.Contains("四至朝向-东"))
                                            {
                                                project.East = row["四至朝向-东"].ToString();//四至朝向-东
                                            }
                                            if (dt.Columns.Contains("四至朝向-西"))
                                            {
                                                project.West = row["四至朝向-西"].ToString();//四至朝向-西
                                            }
                                            if (dt.Columns.Contains("四至朝向-南"))
                                            {
                                                project.South = row["四至朝向-南"].ToString();//四至朝向-南
                                            }
                                            if (dt.Columns.Contains("四至朝向-北"))
                                            {
                                                project.North = row["四至朝向-北"].ToString();//四至朝向-北
                                            }
                                            if (dt.Columns.Contains("经度") && StringHelp.CheckDecimal(row["经度"].ToString()))
                                            {
                                                project.X = Convert.ToDecimal(row["经度"]);//物业经度
                                            }
                                            if (dt.Columns.Contains("经度") && StringHelp.CheckDecimal(row["纬度"].ToString()))
                                            {
                                                project.Y = Convert.ToDecimal(row["纬度"]);//物业纬度
                                            }
                                            project = _unitOfWork.ProjectRepository.Insert(project);
                                            _unitOfWork.Commit();
                                            //row[24] row[25]
                                            PCompany kfs = null;
                                            if (dt.Columns.Contains("开发商"))
                                            {//开发商
                                                string value = row["开发商"].ToString();
                                                var kf = _unitOfWork.P_CompanyRepository.Get(m => m.ProjectId == project.ProjectId && m.CompanyType == 2001004
                                                && m.CityId == loginUser.NowCityId && m.CompanyName == value).FirstOrDefault();
                                                if (kf == null)
                                                {
                                                    kfs = new PCompany();
                                                    kfs.CityId = loginUser.NowCityId;
                                                    kfs.CompanyType = 2001001;
                                                    kfs.CompanyName = value;
                                                    kfs.ProjectId = project.ProjectId;
                                                }
                                            }
                                            PCompany wggs = null;
                                            if (dt.Columns.Contains("物管公司"))
                                            {//物管公司
                                                string value = row["物管公司"].ToString();
                                                var vgg = _unitOfWork.P_CompanyRepository.Get(m => m.ProjectId == project.ProjectId && m.CompanyType == 2001004
                                                && m.CityId == loginUser.NowCityId && m.CompanyName == value).FirstOrDefault();
                                                if (vgg == null)
                                                {
                                                    wggs = new PCompany();
                                                    wggs.CityId = loginUser.NowCityId;
                                                    wggs.CompanyType = 2001004;
                                                    wggs.CompanyName = value;
                                                    wggs.ProjectId = project.ProjectId;
                                                }
                                            }
                                            //任务表
                                            AllotFlow flow = new AllotFlow();
                                            flow.CityId = project.CityID;
                                            flow.FxtCompanyId = loginUser.FxtCompanyId;
                                            flow.DatType = SYSCodeManager.DATATYPECODE_1;
                                            flow.StateCode = SYSCodeManager.STATECODE_1;
                                            flow.StateDate = DateTime.Now;
                                            flow.CreateTime = DateTime.Now;
                                            flow.UserName = loginUser.UserName;
                                            flow.UserTrueName = loginUser.TrueName;
                                            flow.SurveyUserName = "";
                                            flow.SurveyUserTrueName = "";
                                            flow.DatId = project.ProjectId;

                                            flow = _unitOfWork.AllotFlowRepository.Insert(flow);

                                            //记录日志
                                            _unitOfWork.AllotSurveyRepository.Insert(new AllotSurvey()
                                            {
                                                AllotId = flow.Id,
                                                CityId = project.CityID,
                                                FxtCompanyId = loginUser.FxtCompanyId,
                                                UserName = loginUser.UserName,
                                                StateCode = SYSCodeManager.STATECODE_1,
                                                StateDate = DateTime.Now,
                                                CreateDate = DateTime.Now,
                                                TrueName = loginUser.TrueName,
                                                Remark = "<span class=\"red\">Excel导入任务</span>"
                                            });

                                            if (kfs != null) _unitOfWork.P_CompanyRepository.Insert(kfs);
                                            if (wggs != null) _unitOfWork.P_CompanyRepository.Insert(wggs);
                                            parkingStatus.ProjectId = project.ProjectId;
                                            _unitOfWork.P_AppendageRepository.Insert(parkingStatus);
                                            _unitOfWork.Commit();
                                        }
                                        else
                                        {
                                            msgRow += rowsnum + ",";
                                        }
                                        rowsnum++;
                                        #endregion
                                    }
                                    if (string.IsNullOrEmpty(msgRow) && string.IsNullOrEmpty(msgExistRow))
                                    {
                                        msg = "Excel导入成功！";
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(msgExistRow))
                                        {
                                            msg += "行号:" + msgExistRow.Substring(0, msgExistRow.Length - 1) + "导入失败！任务已存在!<br/>";
                                        }
                                        if (!string.IsNullOrEmpty(msgRow))
                                        {
                                            msg += "行号:" + msgRow.Substring(0, msgRow.Length - 1) + " 导入失败！可能原因：1.网站省份、城市不正确！2.Excel必填字段未填写！";
                                        }
                                    }
                                }
                                else
                                {
                                    msg = "Excel无数据！";
                                }
                            }
                            else
                            {
                                msg = "请导入excel格式的文件！";
                            }
                        }
                        else
                        {
                            msg = "请选择excel文件!";
                        }
                    }
                }
                else
                {
                    msg = "请选择Excel文件！";
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            TempData["msg"] = msg;
            return RedirectToAction("ExcelIn");
        }
        /// <summary>
        /// 任务详情导入
        /// </summary>
        /// <returns></returns>
        public ActionResult DetailExcelIn(string msg)
        {
            ViewBag.Msg = msg;
            return View();
        }
        /// <summary>
        /// 任务详情导入提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DetailExcelInPost()
        {
            string msg = string.Empty;
            string msgRow = string.Empty;
            string path = string.Empty;//文件存放路径
            try
            {
                if (Request.Files != null)
                {
                    string ml = Server.MapPath("../Files");
                    if (!System.IO.Directory.Exists(ml))
                        System.IO.Directory.CreateDirectory(ml);
                    HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
                    string fileName = string.Empty;

                    for (int i = 0; i < files.Count; i++)
                    {
                        string filename = System.IO.Path.GetFileName(files[i].FileName);
                        if (!string.IsNullOrEmpty(filename))
                        {
                            string filetype = filename.Substring(filename.LastIndexOf(".") + 1);
                            string filenametime = System.IO.Path.GetFileName(filename.Substring(0, filename.LastIndexOf(".")))
                                + DateTime.Now.ToString("yyyyMMddHHMMss") + "." + filetype;
                            path = Server.MapPath("../Files") + "\\" + filenametime;
                            files[i].SaveAs(path);
                            if (filetype.Contains("xls") || filetype.Contains("xlsx"))
                            {//判断excel文件
                                DataTable dtProject = GetExcelToDataTable(path, "楼盘", 35);
                                List<LNKPCompany> companylist = new List<LNKPCompany>();
                                DATProject project = GetExcelProject(dtProject, out companylist, out msg);

                                if (project != null && string.IsNullOrEmpty(msg))
                                {
                                    #region 字段处理
                                    DataTable dtBuilding = GetExcelToDataTable(path, "楼栋", 25);
                                    List<DATBuilding> buildList = GetExcelBuildingList(dtBuilding, out msg);
                                    if (buildList != null && buildList.Count > 0 && string.IsNullOrEmpty(msg))
                                    {
                                        DataTable dtHouse = GetExcelToDataTable(path, "房号", 14);
                                        List<DATHouse> houseList = GetExcelHouseList(dtHouse, out msg);
                                        int rows = DatAllotFlowManager.ImportTaskDetail(project, buildList, houseList, companylist);
                                        msg = "Excel导入成功！";
                                    }
                                    #endregion

                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(msg))
                                        msg = "Excel无数据！";
                                }
                                try
                                {
                                    if (System.IO.File.Exists(path))
                                    {
                                        System.IO.File.Delete(path);
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                            else
                            {
                                msg = "请导入excel格式的文件！";
                            }
                        }
                        else
                        {
                            msg = "请选择Excel文件！";
                        }
                    }
                }
                else
                {
                    msg = "请选择Excel文件！";
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return RedirectToAction("DetailExcelIn", new { msg = msg });
        }
        /// <summary>
        /// 任务详情导出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DetailExcelOut(int projectId)
        {
            DATProject project = new DATProject();
            IList<LNKPCompany> companyList = new List<LNKPCompany>();
            IList<DATBuilding> building = new List<DATBuilding>();
            IList<DATHouse> house = new List<DATHouse>();
            DatAllotFlowManager.ExportTaskDetail(projectId, out project, out companyList, out building, out house);
            string filepath = OutPutExcel(project, companyList, building, house);
            return Content(filepath);
        }
        /// <summary>
        /// 写入到excel
        /// </summary>
        /// <param name="project"></param>
        /// <param name="companyList"></param>
        /// <param name="building"></param>
        /// <param name="house"></param>
        /// <returns></returns>
        public string OutPutExcel(DATProject project, IList<LNKPCompany> companyList,
            IList<DATBuilding> building, IList<DATHouse> house)
        {
            string path = Server.MapPath("../OutExcel");
            string modelpath = System.IO.Path.Combine(path, "model.xlsx");//模版文件
            string filepath = System.IO.Path.Combine(path, project.ProjectName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
            string outpath = "/OutExcel/" + project.ProjectName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            System.IO.File.Copy(modelpath, filepath, true);
            if (project != null)
            {
                WriteProject(project, companyList, filepath);
                if (building != null && building.Count > 0)
                {
                    WriteBuilding(building, filepath);
                    if (house != null && house.Count > 0)
                        WriteHouse(house, filepath);
                }
            }
            return outpath;
        }
        /// <summary>
        /// 写入楼盘
        /// </summary>
        /// <param name="project"></param>
        /// <param name="companyList"></param>
        private void WriteProject(DATProject project, IList<LNKPCompany> companyList, string filepath)
        {
            ExcelHelper excel = new ExcelHelper(filepath);
            bool bl = excel.ChangeCurrentWorkSheet("楼盘");//楼盘
            #region 赋值给单元格
            //int areaid = GetAreaId(project.CityID, row[4].ToString());
            excel.SetCells(2, 1, GetIsNullField(project.ProjectId));
            excel.SetCells(2, 2, GetIsNullField(project.FxtProjectId));
            excel.SetCells(2, 3, GetIsNullField(project.ProjectName));
            excel.SetCells(2, 4, GetIsNullField(project.OtherName));
            excel.SetCells(2, 5, GetIsNullField(project.Address));
            if (project.AreaID > 0)
            {//行政区
                string areaname = GetAreaName(project.AreaID, project.CityID);
                if (!string.IsNullOrEmpty(areaname))
                    excel.SetCells(2, 6, areaname);
            }
            excel.SetCells(2, 7, GetIsNullField(project.SubAreaId));
            if (project.PurposeCode > 0)
            {//土地用途
                List<SYSCode> list = SYSCodeManager.PurposeCodeManager();
                var name = (from e in list
                            where e.Code == project.PurposeCode
                            select e.CodeName).ToList();
                if (name != null && name.Count > 0)
                {
                    excel.SetCells(2, 8, name[0].ToString());
                }
            }

            if (project.RightCode > 0)
            {//产权形式
                List<SYSCode> list = SYSCodeManager.RightCodeManager();
                var name = (from e in list
                            where e.Code == project.PurposeCode
                            select e.CodeName).ToList();
                if (name != null && name.Count > 0)
                {
                    excel.SetCells(2, 9, name[0].ToString());
                }
            }
            excel.SetCells(2, 10, GetIsNullField(project.FieldNo));
            excel.SetCells(2, 11, GetIsNullField(project.StartDate));
            excel.SetCells(2, 12, GetIsNullField(project.UsableYear));
            if (project.BuildingTypeCode > 0)
            {//主建筑类型
                List<SYSCode> list = SYSCodeManager.BuildingTypeCodeManager();
                var name = (from e in list
                            where e.Code == project.BuildingTypeCode
                            select e.CodeName).ToList();
                if (name != null && name.Count > 0)
                {
                    excel.SetCells(2, 13, name[0].ToString());
                }
            }

            excel.SetCells(2, 14, GetIsNullField(project.CubageRate));
            excel.SetCells(2, 15, GetIsNullField(project.GreenRate));
            excel.SetCells(2, 16, GetIsNullField(project.LandArea));
            excel.SetCells(2, 17, GetIsNullField(project.BuildingArea));
            excel.SetCells(2, 18, GetIsNullField(project.EndDate));
            excel.SetCells(2, 19, GetIsNullField(project.SaleDate));
            excel.SetCells(2, 20, GetIsNullField(project.BuildingDate));
            excel.SetCells(2, 21, GetIsNullField(project.JoinDate));
            excel.SetCells(2, 22, GetIsNullField(project.SalePrice));
            excel.SetCells(2, 23, GetIsNullField(project.BuildingNum));
            excel.SetCells(2, 24, GetIsNullField(project.TotalNum));
            excel.SetCells(2, 25, GetIsNullField(project.ParkingNumber));
            excel.SetCells(2, 28, GetIsNullField(project.ManagerPrice));
            excel.SetCells(2, 29, GetIsNullField(project.ManagerTel));
            excel.SetCells(2, 30, GetIsNullField(project.Detail));
            excel.SetCells(2, 31, GetIsNullField(project.East));
            excel.SetCells(2, 32, GetIsNullField(project.West));
            excel.SetCells(2, 33, GetIsNullField(project.South));
            excel.SetCells(2, 34, GetIsNullField(project.North));
            excel.SetCells(2, 35, GetIsNullField(project.X));
            excel.SetCells(2, 36, GetIsNullField(project.Y));

            foreach (LNKPCompany company in companyList)
            {
                if (company.LNKPCompanyPX.CompanyType == 2001001)//开发商
                {
                    excel.SetCells(2, 26, GetIsNullField(company.CompanyName));//
                }
                if (company.LNKPCompanyPX.CompanyType == 2001004)//物业管理
                {
                    excel.SetCells(2, 27, GetIsNullField(company.CompanyName));
                }
            }
            #endregion
            excel.SaveFile(filepath);
            excel.Quit();

        }
        /// <summary>
        /// 写入楼栋
        /// </summary>
        /// <param name="building"></param>
        /// <param name="filepath"></param>
        private void WriteBuilding(IList<DATBuilding> buildingList, string filepath)
        {
            ExcelHelper excel = new ExcelHelper(filepath);
            excel.ChangeCurrentWorkSheet("楼栋");//楼栋
            #region 楼栋写入
            for (int i = 0; i < buildingList.Count; i++)
            {
                DATBuilding buiding = buildingList[i];
                excel.SetCells(2 + i, 1, GetIsNullField(buiding.ProjectId));
                excel.SetCells(2 + i, 2, GetIsNullField(buiding.BuildingId));
                excel.SetCells(2 + i, 3, GetIsNullField(buiding.BuildingName));
                excel.SetCells(2 + i, 4, GetIsNullField(buiding.Doorplate));
                if (buiding.PurposeCode > 0)
                {//用途
                    List<SYSCode> list = SYSCodeManager.PurposeCodeManager();
                    var name = (from e in list
                                where e.Code == buiding.PurposeCode
                                select e.CodeName).ToList();
                    if (name != null && name.Count > 0)
                    {
                        excel.SetCells(2 + i, 5, name[0].ToString());
                    }
                }
                if (buiding.BuildingTypeCode > 0)
                {//主建筑类型
                    List<SYSCode> list = SYSCodeManager.BuildingTypeCodeManager();
                    var name = (from e in list
                                where e.Code == buiding.BuildingTypeCode
                                select e.CodeName).ToList();
                    if (name != null && name.Count > 0)
                    {
                        excel.SetCells(2 + i, 6, name[0].ToString());
                    }
                }
                if (buiding.StructureCode > 0)
                {//建筑结构   
                    IList<SYSCode> list = SYSCodeManager.GetStructureCodeList();
                    var name = (from e in list
                                where e.Code == buiding.StructureCode
                                select e.CodeName).ToList();
                    if (name != null && name.Count > 0)
                    {
                        excel.SetCells(2 + i, 7, name[0].ToString());
                    }
                }
                excel.SetCells(2 + i, 8, GetIsNullField(buiding.BuildDate));
                excel.SetCells(2 + i, 9, GetIsNullField(buiding.SaleDate));
                excel.SetCells(2 + i, 10, GetIsNullField(buiding.SalePrice));
                excel.SetCells(2 + i, 11, GetIsNullField(buiding.UnitsNumber));
                excel.SetCells(2 + i, 12, GetIsNullField(buiding.TotalFloor));
                excel.SetCells(2 + i, 13, GetIsNullField(buiding.TotalNumber));
                excel.SetCells(2 + i, 14, GetIsNullField(buiding.TotalBuildArea));
                excel.SetCells(2 + i, 15, GetIsNullField(buiding.OtherName));
                excel.SetCells(2 + i, 16, GetIsNullField(buiding.ElevatorRate));
                excel.SetCells(2 + i, 17, GetIsNullField(buiding.IsElevator));
                if (buiding.Wall > 0)
                {//外墙装修
                    IList<SYSCode> list = SYSCodeManager.GetWallCodeList();
                    var name = (from e in list
                                where e.Code == buiding.Wall
                                select e.CodeName).ToList();
                    if (name != null && name.Count > 0)
                    {
                        excel.SetCells(2 + i, 18, name[0].ToString());
                    }
                }
                excel.SetCells(2 + i, 19, GetIsNullField(buiding.SaleLicence));
                excel.SetCells(2 + i, 20, GetIsNullField(buiding.LicenceDate));
                if (buiding.LocationCode > 0)
                {//位置
                    IList<SYSCode> list = SYSCodeManager.GetBuildingLocationCodeList();
                    var name = (from e in list
                                where e.Code == buiding.LocationCode
                                select e.CodeName).ToList();
                    if (name != null && name.Count > 0)
                    {
                        excel.SetCells(2 + i, 21, name[0].ToString());
                    }
                }
                if (buiding.FrontCode > 0)
                {//朝向
                    IList<SYSCode> list = SYSCodeManager.GetFrontCodeList();
                    var name = (from e in list
                                where e.Code == buiding.FrontCode
                                select e.CodeName).ToList();
                    if (name != null && name.Count > 0)
                    {
                        excel.SetCells(2 + i, 22, name[0].ToString());
                    }
                }
                if (buiding.SightCode > 0)
                {//景观
                    IList<SYSCode> list = SYSCodeManager.GetSightCodeList();
                    var name = (from e in list
                                where e.Code == buiding.SightCode
                                select e.CodeName).ToList();
                    if (name != null && name.Count > 0)
                    {
                        excel.SetCells(2 + i, 23, name[0].ToString());
                    }
                }
                excel.SetCells(2 + i, 24, GetIsNullField(buiding.Weight));
                excel.SetCells(2 + i, 25, GetIsNullField(buiding.PriceDetail));
                excel.SetCells(2 + i, 26, GetIsNullField(buiding.Remark));


            }
            #endregion
            excel.SaveFile(filepath);
            excel.Quit();
        }
        /// <summary>
        /// 写入房号
        /// </summary>
        /// <param name="house"></param>
        /// <param name="filepath"></param>
        private void WriteHouse(IList<DATHouse> houseList, string filepath)
        {
            ExcelHelper excel = new ExcelHelper(filepath);
            excel.ChangeCurrentWorkSheet("房号");//房号
            #region 房号写入
            for (int i = 0; i < houseList.Count; i++)
            {
                DATHouse house = houseList[i];
                excel.SetCells(2 + i, 1, GetIsNullField(house.BuildingId));
                excel.SetCells(2 + i, 2, GetIsNullField(house.HouseId.ToString()));
                excel.SetCells(2 + i, 3, GetIsNullField(house.HouseName.ToString()));
                excel.SetCells(2 + i, 4, GetIsNullField(house.FloorNo.ToString()));
                excel.SetCells(2 + i, 5, GetIsNullField(house.UnitNo.ToString()));
                excel.SetCells(2 + i, 6, GetIsNullField(house.BuildArea.ToString()));
                excel.SetCells(2 + i, 7, "");
                if (house.HouseTypeCode > 0)
                {//户型 
                    IList<SYSCode> list = SYSCodeManager.GetHouseTypeCodeList();
                    var name = (from e in list
                                where e.Code == house.HouseTypeCode
                                select e.CodeName).ToList();
                    if (name != null && name.Count > 0)
                    {
                        excel.SetCells(2 + i, 8, name[0].ToString());
                    }
                }
                if (house.StructureCode > 0)
                {//户型结构 
                    IList<SYSCode> list = SYSCodeManager.GetHouseStructureCodeList();
                    var name = (from e in list
                                where e.Code == house.StructureCode
                                select e.CodeName).ToList();
                    if (name != null && name.Count > 0)
                    {
                        excel.SetCells(2 + i, 9, name[0].ToString());
                    }
                }
                excel.SetCells(2 + i, 10, GetIsNullField(house.SalePrice.ToString()));
                excel.SetCells(2 + i, 11, GetIsNullField(house.UnitPrice.ToString()));

                if (house.FrontCode > 0)
                {//朝向 
                    IList<SYSCode> list = SYSCodeManager.GetFrontCodeList();
                    var name = (from e in list
                                where e.Code == house.FrontCode
                                select e.CodeName).ToList();
                    if (name != null && name.Count > 0)
                    {
                        excel.SetCells(2 + i, 12, name[0].ToString());
                    }
                }
                if (house.SightCode > 0)
                {//景观 
                    IList<SYSCode> list = SYSCodeManager.GetSightCodeList();
                    var name = (from e in list
                                where e.Code == house.SightCode
                                select e.CodeName).ToList();
                    if (name != null && name.Count > 0)
                    {
                        excel.SetCells(2 + i, 13, name[0].ToString());
                    }
                }
                if (house.PurposeCode > 0)
                {//用途 
                    IList<SYSCode> list = SYSCodeManager.GetHousePurposeCodeList();
                    var name = (from e in list
                                where e.Code == house.PurposeCode
                                select e.CodeName).ToList();
                    if (name != null && name.Count > 0)
                    {
                        excel.SetCells(2 + i, 14, name[0].ToString());
                    }
                }
                excel.SetCells(2 + i, 15, GetIsNullField(house.Weight.ToString()));
            }
            #endregion
            excel.SaveFile(filepath);
            excel.Quit();
        }

        /// <summary>
        /// 获取楼盘数据
        /// </summary>
        /// <param name="dtProject"></param>
        /// <returns></returns>
        public static DATProject GetExcelProject(DataTable dtProject, out List<LNKPCompany> companyL, out string msg)
        {
            companyL = null;
            msg = "";
            DATProject project = new DATProject();
            DataRow row = dtProject.Rows[0];
            int projectid = 0;
            if (!string.IsNullOrEmpty(GetExcelField(row[0])) && StringHelp.CheckInteger(row[0].ToString()))
            {
                projectid = Convert.ToInt32(row[0].ToString());
            }
            string projectName = GetExcelField(row[2]);//楼盘名称
            string area = GetExcelField(row[5]);//行政区
            string purposeCode = GetExcelField(row[7]);//用途
            project.CityID = WebUserHelp.GetNowCityId();
            if (!string.IsNullOrEmpty(GetExcelField(row[5])))
            {
                int areaid = GetAreaId(project.CityID, row[5].ToString());
                if (areaid > 0)
                    project.AreaID = areaid;//行政区
            }

            if (!string.IsNullOrEmpty(projectName) && !string.IsNullOrEmpty(area)
                && !string.IsNullOrEmpty(purposeCode) && projectid > 0 && project.AreaID > 0)
            {
                #region 楼盘
                project.ProjectId = projectid;
                project.CreateTime = DateTime.Now;
                project.Valid = 1;
                project.Status = 1035001;
                project.FxtCompanyId = WebUserHelp.GetNowLoginUser().FxtCompanyId;
                if (!string.IsNullOrEmpty(GetExcelField(row[1])) && StringHelp.CheckInteger(row[1].ToString()))
                {
                    project.FxtProjectId = Convert.ToInt32(row[1]);//运维中心库楼盘ID
                }
                project.ProjectName = GetExcelField(row[2]).ToString();//楼盘名称
                project.OtherName = GetExcelField(row[3]).ToString();//别名
                project.Address = GetExcelField(row[4]).ToString();//地址

                if (!string.IsNullOrEmpty(GetExcelField(row[6])))
                {
                    //project.SubAreaId = row[5];//片区
                }
                if (!string.IsNullOrEmpty(GetExcelField(row[7])))
                {//用途
                    List<SYSCode> list = SYSCodeManager.PurposeCodeManager();
                    var id = (from e in list
                              where e.CodeName == row[7].ToString()
                              select e.Code).ToList();
                    if (id != null && id.Count > 0)
                    {
                        project.PurposeCode = Convert.ToInt32(id[0]);
                    }
                }
                if (!string.IsNullOrEmpty(GetExcelField(row[8])))
                {//产权形式
                    List<SYSCode> list = SYSCodeManager.RightCodeManager();
                    var id = (from e in list
                              where e.CodeName == row[8].ToString()
                              select e.Code).ToList();
                    if (id != null && id.Count > 0)
                    {
                        project.RightCode = Convert.ToInt32(id[0]);
                    }
                }
                project.FieldNo = GetExcelField(row[9]).ToString();//宗地号		
                if (!string.IsNullOrEmpty(GetExcelField(row[10])) && StringHelp.CheckIsDate(row[10].ToString()))
                {
                    project.StartDate = Convert.ToDateTime(row[10]);//土地起始日期
                }
                if (!string.IsNullOrEmpty(GetExcelField(row[11])) && StringHelp.CheckInteger(row[11].ToString()))
                {
                    project.UsableYear = Convert.ToInt32(row[11]);//土地年限
                }
                if (!string.IsNullOrEmpty(GetExcelField(row[12])))
                {//主建筑类型
                    List<SYSCode> list = SYSCodeManager.BuildingTypeCodeManager();
                    var id = (from e in list
                              where e.CodeName == row[12].ToString()
                              select e.Code).ToList();
                    if (id != null && id.Count > 0)
                    {
                        project.BuildingTypeCode = Convert.ToInt32(id[0]);
                    }
                }
                if (!string.IsNullOrEmpty(GetExcelField(row[13])) && StringHelp.CheckDecimal(row[13].ToString()))
                {
                    project.CubageRate = Convert.ToDecimal(row[13]);//容积率		
                }
                if (!string.IsNullOrEmpty(GetExcelField(row[14])) && StringHelp.CheckDecimal(row[14].ToString()))
                {
                    project.GreenRate = Convert.ToDecimal(row[14]);//绿化率		
                }
                if (!string.IsNullOrEmpty(GetExcelField(row[15])) && StringHelp.CheckDecimal(row[15].ToString()))
                {
                    project.LandArea = Convert.ToDecimal(row[15]);//占地面积		
                }
                if (!string.IsNullOrEmpty(GetExcelField(row[16])) && StringHelp.CheckDecimal(row[16].ToString()))
                {
                    project.BuildingArea = Convert.ToDecimal(row[16]);//建筑面积		
                }
                if (!string.IsNullOrEmpty(GetExcelField(row[17])) && StringHelp.CheckIsDate(row[17].ToString()))
                {
                    project.EndDate = Convert.ToDateTime(row[17]);//竣工日期
                }
                if (!string.IsNullOrEmpty(GetExcelField(row[18])) && StringHelp.CheckIsDate(row[18].ToString()))
                {
                    project.SaleDate = Convert.ToDateTime(row[18]);//开盘时间
                }
                if (!string.IsNullOrEmpty(GetExcelField(row[19])) && StringHelp.CheckIsDate(row[19].ToString()))
                {
                    project.BuildingDate = Convert.ToDateTime(row[19]);//	开工时间
                }
                if (!string.IsNullOrEmpty(GetExcelField(row[20])) && StringHelp.CheckIsDate(row[20].ToString()))
                {
                    project.JoinDate = Convert.ToDateTime(row[20]);//入伙时间
                }
                if (!string.IsNullOrEmpty(GetExcelField(row[21])) && StringHelp.CheckDecimal(row[21].ToString()))
                {
                    project.SalePrice = Convert.ToDecimal(row[21]);//开盘均价
                }
                if (!string.IsNullOrEmpty(GetExcelField(row[22])) && StringHelp.CheckInteger(row[22].ToString()))
                {
                    project.BuildingNum = Convert.ToInt32(row[22]);//总栋数
                }
                if (!string.IsNullOrEmpty(GetExcelField(row[23])) && StringHelp.CheckInteger(row[23].ToString()))
                {
                    project.TotalNum = Convert.ToInt32(row[23]);//总套数
                }
                if (!string.IsNullOrEmpty(GetExcelField(row[24])) && StringHelp.CheckInteger(row[24].ToString()))
                {
                    project.ParkingNumber = Convert.ToInt32(row[24]);//	车位数
                }
                project.ManagerPrice = GetExcelField(row[27]);//物管费
                project.ManagerTel = GetExcelField(row[28]);//物管电话
                project.Detail = GetExcelField(row[29]);//项目概况
                project.East = GetExcelField(row[30]);//四至朝向-东
                project.West = GetExcelField(row[31]);//四至朝向-西
                project.South = GetExcelField(row[32]);//四至朝向-南
                project.North = GetExcelField(row[33]);//四至朝向-北
                if (!string.IsNullOrEmpty(GetExcelField(row[34])) && StringHelp.CheckDecimal(row[34].ToString()))
                {
                    project.X = Convert.ToDecimal(row[34]);//物业经度
                }
                if (!string.IsNullOrEmpty(GetExcelField(row[35])) && StringHelp.CheckDecimal(row[35].ToString()))
                {
                    project.Y = Convert.ToDecimal(row[35]);//物业纬度
                }

                List<LNKPCompany> companyList = new List<LNKPCompany>();
                if (!string.IsNullOrEmpty(GetExcelField(row[25])))
                {//开发商
                    LNKPCompany company = new LNKPCompany();
                    ProjectPKCompanyTypePKCity lnkCompany = new ProjectPKCompanyTypePKCity();
                    lnkCompany.CityId = WebUserHelp.GetNowCityId();
                    lnkCompany.CompanyType = 2001001;
                    company.LNKPCompanyPX = lnkCompany;
                    company.CompanyName = row[25].ToString();
                    companyList.Add(company);
                }
                if (!string.IsNullOrEmpty(GetExcelField(row[26])))
                {//物管公司
                    LNKPCompany company = new LNKPCompany();
                    ProjectPKCompanyTypePKCity lnkCompany = new ProjectPKCompanyTypePKCity();
                    lnkCompany.CityId = WebUserHelp.GetNowCityId();
                    lnkCompany.CompanyType = 2001004;
                    company.LNKPCompanyPX = lnkCompany;
                    company.CompanyName = row[26].ToString();
                    companyList.Add(company);
                }
                companyL = companyList;
                #endregion
            }
            else
            {
                msg = "必填字段未填写!";
            }
            return project;
        }
        /// <summary>
        /// 获取楼栋列表
        /// </summary>
        /// <param name="project"></param>
        private static List<DATBuilding> GetExcelBuildingList(DataTable dtBuilding, out string msg)
        {
            msg = "";
            List<DATBuilding> buildList = new List<DATBuilding>();
            if (dtBuilding == null || dtBuilding.Rows.Count <= 0)
            {
                msg = "楼栋表没有数据!";
                return null;
            }
            #region 楼栋
            int rowsnum = 2;
            foreach (DataRow row in dtBuilding.Rows)
            {
                DATBuilding building = new DATBuilding();
                int bprojectid = 0;//采集库楼盘ID
                if (!string.IsNullOrEmpty(GetExcelField(row[0])) && StringHelp.CheckInteger(row[0].ToString()))
                {
                    bprojectid = Convert.ToInt32(row[0].ToString());
                }
                int buildid = 0;//采集库楼栋ID
                if (!string.IsNullOrEmpty(GetExcelField(row[1])) && StringHelp.CheckInteger(row[1].ToString()))
                {
                    buildid = Convert.ToInt32(row[1].ToString());
                }
                int totalfloor = 0;//总层数
                if (!string.IsNullOrEmpty(GetExcelField(row[11])) && StringHelp.CheckInteger(row[11].ToString()))
                {
                    totalfloor = Convert.ToInt32(row[11].ToString());
                }
                string buildname = GetExcelField(row[2]);//楼栋名称
                string doorplate = GetExcelField(row[3]);//门牌号
                int purposecode = 0;
                if (!string.IsNullOrEmpty(GetExcelField(row[4])))
                {//用途
                    List<SYSCode> list = SYSCodeManager.PurposeCodeManager();
                    var id = (from e in list
                              where e.CodeName == row[4].ToString()
                              select e.Code).ToList();
                    if (id != null && id.Count > 0)
                    {
                        purposecode = Convert.ToInt32(id[0]);
                    }
                }
                if (bprojectid > 0 && buildid > 0 && totalfloor > 0 && purposecode > 0
                    && !string.IsNullOrEmpty(buildname) && !string.IsNullOrEmpty(doorplate))
                {
                    #region 字段
                    building.ProjectId = bprojectid;
                    building.BuildingId = buildid;
                    building.BuildingName = buildname;
                    building.Doorplate = doorplate;
                    building.PurposeCode = purposecode;//居住
                    building.TotalFloor = totalfloor;

                    if (!string.IsNullOrEmpty(GetExcelField(row[5])))
                    {//主建筑类型
                        List<SYSCode> list = SYSCodeManager.BuildingTypeCodeManager();
                        var id = (from e in list
                                  where e.CodeName == row[5].ToString()
                                  select e.Code).ToList();
                        if (id != null && id.Count > 0)
                        {
                            building.BuildingTypeCode = Convert.ToInt32(id[0]);
                        }
                    }
                    if (!string.IsNullOrEmpty(GetExcelField(row[6])))
                    {//建筑结构   
                        IList<SYSCode> list = SYSCodeManager.GetStructureCodeList();
                        var id = (from e in list
                                  where e.CodeName == row[6].ToString()
                                  select e.Code).ToList();
                        if (id != null && id.Count > 0)
                        {
                            building.StructureCode = Convert.ToInt32(id[0]);
                        }
                    }
                    if (!string.IsNullOrEmpty(GetExcelField(row[7])) && StringHelp.CheckIsDate(row[7].ToString()))
                    {
                        building.BuildDate = Convert.ToDateTime(row[7]);//竣工日期
                    }
                    if (!string.IsNullOrEmpty(GetExcelField(row[8])) && StringHelp.CheckIsDate(row[8].ToString()))
                    {
                        building.SaleDate = Convert.ToDateTime(row[8]);//销售时间
                    }
                    if (!string.IsNullOrEmpty(GetExcelField(row[9])) && StringHelp.CheckDecimal(row[9].ToString()))
                    {
                        building.SalePrice = Convert.ToDecimal(row[9]);//销售均价
                    }
                    if (!string.IsNullOrEmpty(GetExcelField(row[10])) && StringHelp.CheckInteger(row[10].ToString()))
                    {
                        building.UnitsNumber = Convert.ToInt32(row[10]);//单元数
                    }
                    if (!string.IsNullOrEmpty(GetExcelField(row[12])) && StringHelp.CheckInteger(row[12].ToString()))
                    {
                        building.TotalNumber = Convert.ToInt32(row[12]);//总套数
                    }
                    if (!string.IsNullOrEmpty(GetExcelField(row[13])) && StringHelp.CheckDecimal(row[13].ToString()))
                    {
                        building.TotalBuildArea = Convert.ToDecimal(row[13]);//建筑面积
                    }
                    building.OtherName = GetExcelField(row[14]);//别名
                    building.ElevatorRate = GetExcelField(row[15]);//梯户比
                    if (!string.IsNullOrEmpty(GetExcelField(row[16])) && StringHelp.CheckInteger(row[16].ToString()))
                    {
                        building.IsElevator = Convert.ToInt32(row[16]);//是否带电梯
                    }
                    if (!string.IsNullOrEmpty(GetExcelField(row[17])))
                    {//外墙装修
                        IList<SYSCode> list = SYSCodeManager.GetWallCodeList();
                        var id = (from e in list
                                  where e.CodeName == row[17].ToString()
                                  select e.Code).ToList();
                        if (id != null && id.Count > 0)
                        {
                            building.Wall = Convert.ToInt32(id[0]);
                        }
                    }
                    building.SaleLicence = GetExcelField(row[18]);//预售证号
                    if (!string.IsNullOrEmpty(GetExcelField(row[19])) && StringHelp.CheckIsDate(row[19].ToString()))
                    {
                        building.LicenceDate = Convert.ToDateTime(row[19]);//批售时间
                    }
                    if (!string.IsNullOrEmpty(GetExcelField(row[20])) && StringHelp.CheckInteger(row[20].ToString()))
                    {//位置
                        IList<SYSCode> list = SYSCodeManager.GetBuildingLocationCodeList();
                        var id = (from e in list
                                  where e.CodeName == row[20].ToString()
                                  select e.Code).ToList();
                        if (id != null && id.Count > 0)
                        {
                            building.LocationCode = Convert.ToInt32(id[0]);
                        }
                    }
                    if (!string.IsNullOrEmpty(GetExcelField(row[21])))
                    {//朝向
                        IList<SYSCode> list = SYSCodeManager.GetFrontCodeList();
                        var id = (from e in list
                                  where e.CodeName == row[21].ToString()
                                  select e.Code).ToList();
                        if (id != null && id.Count > 0)
                        {
                            building.FrontCode = Convert.ToInt32(id[0]);
                        }
                    }
                    if (!string.IsNullOrEmpty(GetExcelField(row[22])))
                    {//景观
                        IList<SYSCode> list = SYSCodeManager.GetSightCodeList();
                        var id = (from e in list
                                  where e.CodeName == row[22].ToString()
                                  select e.Code).ToList();
                        if (id != null && id.Count > 0)
                        {
                            building.SightCode = Convert.ToInt32(id[0]);
                        }
                    }
                    if (!string.IsNullOrEmpty(GetExcelField(row[23])) && StringHelp.CheckDecimal(row[23].ToString()))
                    {
                        building.Weight = Convert.ToDecimal(row[23]);//价格权重系数
                    }
                    building.PriceDetail = GetExcelField(row[24]);//价格系数说明
                    building.Remark = GetExcelField(row[25]);//备注
                    buildList.Add(building);
                    #endregion
                }
                else
                {
                    msg += rowsnum + ",";
                }
                rowsnum++;
            }
            #endregion
            return buildList;
        }
        /// <summary>
        /// 获取房号信息
        /// </summary>
        /// <param name="dtHouse"></param>
        /// <returns></returns>
        private static List<DATHouse> GetExcelHouseList(DataTable dtHouse, out string msg)
        {
            msg = "";
            List<DATHouse> houseList = new List<DATHouse>();
            if (dtHouse == null || dtHouse.Rows.Count <= 0)
            {
                msg = "房号表没有数据!";
                return null;
            }
            #region 房号
            int rowsnum = 2;
            foreach (DataRow row in dtHouse.Rows)
            {
                DATHouse house = new DATHouse();
                int hbuildingId = 0;//采集库楼栋ID
                if (!string.IsNullOrEmpty(GetExcelField(row[0])) && StringHelp.CheckInteger(row[0].ToString()))
                {
                    hbuildingId = Convert.ToInt32(row[0].ToString());
                }
                int houseid = 0;//采集库房屋ID
                if (!string.IsNullOrEmpty(GetExcelField(row[1])) && StringHelp.CheckInteger(row[1].ToString()))
                {
                    houseid = Convert.ToInt32(row[1].ToString());
                }
                string houseName = GetExcelField(row[2]);//房号名称
                int floorno = 0;//所在楼层
                if (!string.IsNullOrEmpty(GetExcelField(row[3])) && StringHelp.CheckInteger(row[3].ToString()))
                {
                    floorno = Convert.ToInt32(row[3]);
                }
                decimal buildArea = 0;
                if (!string.IsNullOrEmpty(GetExcelField(row[5])) && StringHelp.CheckDecimal(row[5].ToString()))
                {//建筑面积
                    buildArea = Convert.ToDecimal(row[5]);
                }

                int houseTypeCode = 0;
                if (!string.IsNullOrEmpty(GetExcelField(row[7])))
                {//户型 
                    IList<SYSCode> list = SYSCodeManager.GetHouseTypeCodeList();
                    var id = (from e in list
                              where e.CodeName == row[7].ToString()
                              select e.Code).ToList();
                    if (id != null && id.Count > 0)
                    {
                        houseTypeCode = Convert.ToInt32(id[0]);
                    }
                }
                int frontCode = 0;
                if (!string.IsNullOrEmpty(GetExcelField(row[11])))
                {//朝向 
                    IList<SYSCode> list = SYSCodeManager.GetFrontCodeList();
                    var id = (from e in list
                              where e.CodeName == row[11].ToString()
                              select e.Code).ToList();
                    if (id != null && id.Count > 0)
                    {
                        frontCode = Convert.ToInt32(id[0]);
                    }
                }

                string unitNo = GetExcelField(row[4]);//所在(单元)室号
                if (hbuildingId > 0 && houseid > 0 && !string.IsNullOrEmpty(houseName)
                  && floorno > 0 && !string.IsNullOrEmpty(unitNo) && buildArea > 0
                  && frontCode > 0 && houseTypeCode > 0)
                {
                    #region 字段
                    house.BuildingId = hbuildingId;
                    house.HouseId = houseid;
                    house.HouseName = houseName;
                    house.FloorNo = floorno;
                    house.UnitNo = unitNo;
                    house.BuildArea = buildArea;
                    house.FrontCode = frontCode;
                    house.HouseTypeCode = houseTypeCode;
                    if (!string.IsNullOrEmpty(GetExcelField(row[8])))
                    {//户型结构 
                        IList<SYSCode> list = SYSCodeManager.GetHouseStructureCodeList();
                        var id = (from e in list
                                  where e.CodeName == row[8].ToString()
                                  select e.Code).ToList();
                        if (id != null && id.Count > 0)
                        {
                            house.StructureCode = Convert.ToInt32(id[0]);
                        }
                    }
                    if (!string.IsNullOrEmpty(GetExcelField(row[9])) && StringHelp.CheckDecimal(row[9].ToString()))
                    {
                        house.SalePrice = Convert.ToDecimal(row[9]);//总价
                    }
                    if (!string.IsNullOrEmpty(GetExcelField(row[10])) && StringHelp.CheckDecimal(row[10].ToString()))
                    {
                        house.UnitPrice = Convert.ToDecimal(row[10]);//单价
                    }
                    if (!string.IsNullOrEmpty(GetExcelField(row[12])))
                    {//景观 
                        IList<SYSCode> list = SYSCodeManager.GetSightCodeList();
                        var id = (from e in list
                                  where e.CodeName == row[12].ToString()
                                  select e.Code).ToList();
                        if (id != null && id.Count > 0)
                        {
                            house.SightCode = Convert.ToInt32(id[0]);
                        }
                    }
                    if (!string.IsNullOrEmpty(GetExcelField(row[13])))
                    {//用途 
                        IList<SYSCode> list = SYSCodeManager.GetHousePurposeCodeList();
                        var id = (from e in list
                                  where e.CodeName == row[13].ToString()
                                  select e.Code).ToList();
                        if (id != null && id.Count > 0)
                        {
                            house.PurposeCode = Convert.ToInt32(id[0]);
                        }
                    }
                    if (!string.IsNullOrEmpty(GetExcelField(row[14])) && StringHelp.CheckDecimal(row[14].ToString()))
                    {
                        house.Weight = Convert.ToDecimal(row[14]);//价格权重系数
                    }
                    houseList.Add(house);
                    #endregion
                }
                else
                {
                    msg += rowsnum + ",";
                }
                rowsnum++;
            }
            #endregion
            return houseList;
        }
        /// <summary>
        /// 除掉行里面都是空的行
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static DataTable GetExcelToDataTable(string path, string name, int colnum)
        {
            string falePath = Path.Combine(path);
            ExcelHandle excelHandle = new ExcelHandle(falePath);
            DataTable dt = excelHandle.ExcelToDataTable(name, true);
            //DataTable dt = CAS.Common.MVC4.ExcelHelpers.ExportExcelToDt(path, name);
            DataTable dtNew = dt.Clone();
            foreach (DataRow row in dt.Rows)
            {
                if (VaildRowNULL(row, colnum))
                {
                    DataRow newRow = dtNew.NewRow();
                    newRow.ItemArray = row.ItemArray;
                    dtNew.Rows.Add(newRow);
                }
            }
            return dtNew;
        }
        /// <summary>
        /// 验证行是否为null
        /// </summary>
        /// <param name="row"></param>
        private static bool VaildRowNULL(DataRow row, int colnum)
        {
            bool result = false;
            for (int i = 0; i < colnum; i++)
            {
                if (row[i] != null && row[i] != "{}" && !string.IsNullOrEmpty(row[i].ToString()))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
        /// <summary>
        /// 去掉excel里面的{}
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetExcelField(object obj)
        {
            if (obj == null || obj == "{}")
                obj = "";
            return obj.ToString();
        }
        /// <summary>
        /// 当为null的时候返回空字符
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetIsNullField(object obj)
        {
            if (obj == null)
                return "";
            else
                return obj.ToString();
        }
        /// <summary>
        /// 获取区域ID
        /// </summary>
        /// <param name="cityid"></param>
        /// <returns></returns>
        public static int GetAreaId(int cityid, string areaname)
        {
            int areaid = 0;
            List<UserCenter_Apps> appList = new List<UserCenter_Apps>();
            UserCenter_LoginUserInfo loginUserInfo = WebUserHelp.GetNowLoginUser(out appList);
            List<FxtApi_SYSArea> list = DataCenterAreaApi.GetAreaByCityId(cityid, loginUserInfo.UserName, loginUserInfo.SignName, appList);
            var o = (from e in list
                     where e.AreaName == areaname
                     select e.AreaId).ToList();
            if (o != null && o.Count() > 0)
            {
                areaid = Convert.ToInt32(o[0]);
            }
            return areaid;
        }
        /// <summary>
        /// 获取区域名字
        /// </summary>
        /// <param name="areaid"></param>
        /// <returns></returns>
        public static string GetAreaName(int areaid, int cityid)
        {
            string areaname = string.Empty;
            List<UserCenter_Apps> appList = new List<UserCenter_Apps>();
            UserCenter_LoginUserInfo loginUserInfo = WebUserHelp.GetNowLoginUser(out appList);
            List<FxtApi_SYSArea> list = DataCenterAreaApi.GetAreaByCityId(cityid, loginUserInfo.UserName, loginUserInfo.SignName, appList);
            var o = (from e in list
                     where e.AreaId == areaid
                     select e.AreaName).ToList();
            if (o != null && o.Count() > 0)
            {
                areaname = o[0].ToString();
            }
            return areaname;
        }

        /// <summary>
        /// 获取片区ID
        /// </summary>
        /// <param name="areaid"></param>
        /// <returns></returns>
        public static int GetSubAreaId(int areaid, string subAreaName)
        {
            int subAreaid = 0;
            List<UserCenter_Apps> appList = new List<UserCenter_Apps>();
            UserCenter_LoginUserInfo loginUserInfo = WebUserHelp.GetNowLoginUser(out appList);
            List<FxtApi_SYSSubArea> list = DataCenterAreaApi.GetSubAreaByAreaId(areaid, loginUserInfo.UserName, loginUserInfo.SignName, appList);
            var o = (from e in list
                     where e.SubAreaName == subAreaName
                     select e.SubAreaId).ToList();
            if (o != null && o.Count() > 0)
            {
                subAreaid = Convert.ToInt32(o[0]);
            }
            return subAreaid;
        }
        #endregion
    }
}
