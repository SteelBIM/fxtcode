namespace FxtDataAcquisition.Web.Controllers
{
    using System;
    using System.IO;
    using System.Web;
    using System.Web.Mvc;
    using System.Data;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using log4net;
    using AutoMapper;
    using CAS.Entity.FxtProject;

    using FxtDataAcquisition.Common;
    using FxtDataAcquisition.Common.NPOI;
    using FxtDataAcquisition.Web.Common;
    using FxtDataAcquisition.Domain.Models;
    using FxtDataAcquisition.Domain.DTO;
    using FxtDataAcquisition.Domain.DTO.FxtDataWcfDTO;
    using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
    using FxtDataAcquisition.FxtAPI.FxtDataWcf.Manager;
    using FxtDataAcquisition.FxtAPI.FxtDataCenter.Manager;
    using FxtDataAcquisition.Application.Services;
    using FxtDataAcquisition.Application.Interfaces;

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

        /// <summary>
        /// 任务页面
        /// </summary>
        /// <param name="statuscode"></param>
        /// <param name="loginUserInfo"></param>
        /// <returns></returns>
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult AllotFlowManager(int? statuscode, LoginUser loginUserInfo)
        {
            List<SYSCode> colist = DataCenterCodeApi.GetCodeById(1035, loginUserInfo.UserName, loginUserInfo.SignName, loginUserInfo.AppList);

            List<FxtApi_SYSArea> areaList = DataCenterAreaApi.GetAreaByCityId(loginUserInfo.NowCityId, loginUserInfo.UserName, loginUserInfo.SignName, loginUserInfo.AppList);

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
                return AjaxJson(new AjaxResult()
                {
                    Result = true,
                    Code = "302",
                    Message = "无权限"
                }); ;
            }
            #endregion

            ViewBag.AreaList = areaList;

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
            , string functionCodes, int pageIndex, int pageSize, LoginUser loginUserInfo)
        {
            AjaxResult result = new AjaxResult("");

            #region 查询条件
            var projectFilter = PredicateBuilder.True<Project>();
            projectFilter = projectFilter.And(m => m.CityID == loginUserInfo.NowCityId && m.FxtCompanyId == loginUserInfo.FxtCompanyId && m.Valid == 1);
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
        public ActionResult AllotFlowManager_CancelAllotFlow_Api(string allotIds, LoginUser loginUserInfo)
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
        public ActionResult AllotFlowManager_CancelSurvey_Api(string allotIds, LoginUser loginUserInfo)
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
        public ActionResult Delete(string allotIds)
        {
            AjaxResult result = new AjaxResult();

            result.Result = false;

            if (string.IsNullOrEmpty(allotIds))
            {
                result.Message = "请选择要删除的任务";

                return AjaxJson(result);
            }

            #region 删除任务
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
            #endregion

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
            table.Columns.Add("附属房屋类型");
            table.Columns.Add("附属房屋面积");
            table.Columns.Add("面积确认");
            table.Columns.Add("装修");
            table.Columns.Add("有无厨房");
            table.Columns.Add("阳台数");
            table.Columns.Add("洗手间数");
            table.Columns.Add("备注");
            int index = 0;

            var front = _unitOfWork.CodeService.HouseFrontCodeManager();
            var houseType = _unitOfWork.CodeService.HouseTypeCodeManager();
            var noiseCode = _unitOfWork.CodeService.NoiseManager();
            var purposeCode = _unitOfWork.CodeService.HousePurposeCodeManager();
            var sightCode = _unitOfWork.CodeService.HouseSightCodeManager();
            var structureCode = _unitOfWork.CodeService.StructureCodeManager();
            var VDCode = _unitOfWork.CodeService.VDCodeManager();
            var subHouseCode = _unitOfWork.CodeService.HouseSubHouseTypeManager();
            var fitmentCode = _unitOfWork.CodeService.HouseFitmentCodeTypeManager();

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
                    VDCode = h.VDCode,
                    SubHouseType = h.SubHouseType,
                    SubHouseArea = h.SubHouseArea,
                    IsShowBuildingArea = h.IsShowBuildingArea,
                    FitmentCode = h.FitmentCode,
                    Cookroom = h.Cookroom,
                    Balcony = h.Balcony,
                    Toilet = h.Toilet
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
                    var sht = subHouseCode.FirstOrDefault(f => f.Code == h.SubHouseType);
                    var fic = fitmentCode.FirstOrDefault(f => f.Code == h.FitmentCode);
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
                    row[13] = sht != null ? sht.CodeName : "";
                    row[14] = h.SubHouseArea;
                    if (!h.IsShowBuildingArea.HasValue || h.IsShowBuildingArea < 0)
                    {
                        row[15] = "";
                    }
                    else if (h.IsShowBuildingArea == 1)
                    {
                        row[15] = "是";
                    }
                    else if (h.IsShowBuildingArea == 0)
                    {
                        row[15] = "否";
                    }
                    row[16] = fic != null ? fic.CodeName : "";
                    if (!h.Cookroom.HasValue || h.Cookroom < 0)
                    {
                        row[17] = "";
                    }
                    else if (h.Cookroom == 1)
                    {
                        row[17] = "有";
                    }
                    else if (h.Cookroom == 0)
                    {
                        row[17] = "无";
                    }
                    row[18] = h.Balcony;
                    row[19] = h.Toilet;

                    row[20] = h.Remark;
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
        public ActionResult AssignAllotToUser(string allotIds, LoginUser loginUserInfo)
        {
            var departmentList = _unitOfWork.DepartmentRepository.Get(m => (m.FxtCompanyId == loginUserInfo.FxtCompanyId || m.FxtCompanyId == 0) && (m.FK_CityId == 0 || m.FK_CityId == loginUserInfo.NowCityId) && m.DValid == 1);

            ViewBag.DepartmentList = departmentList;
            ViewBag.NowDepartment = "-1";
            ViewBag.ViewType = "my";
            ViewBag.NowUserName = loginUserInfo.UserName;
            ViewBag.AllotIds = allotIds;
            //获取当前用户在此页面所有的操作权限
            List<int> functionCodes = _unitOfWork.FunctionService.GetAllBy(loginUserInfo.UserName, loginUserInfo.FxtCompanyId, loginUserInfo.NowCityId, WebCommon.Url_AllotFlowInfo_AssignAllotToUser)
                .Select(m => m.FunctionCode).ToList();

            var nowDepartment = _unitOfWork.DepartmentUserRepository.Get(m => m.FxtCompanyID == loginUserInfo.FxtCompanyId && m.CityID == loginUserInfo.NowCityId && m.UserName == loginUserInfo.UserName).FirstOrDefault();

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
        public ActionResult AssignAllotToUser_GetList_Api(string keyWord, int? roleId, int? departmentId, int pageIndex, int pageSize, int isGetCount, LoginUser loginUserInfo)
        {
            var result = new AjaxResult("");

            int count = 0;

            var list = _unitOfWork.UserService.GetUserInfo(keyWord, roleId, departmentId, pageIndex, pageSize, out count, loginUserInfo.NowCityId, loginUserInfo.FxtCompanyId, loginUserInfo.UserName, loginUserInfo.SignName, loginUserInfo.AppList);

            result.Data = new { Count = count, List = list };

            return AjaxJson(result);
        }
        /// <summary>
        /// 提交分配
        /// </summary>
        /// <param name="allotIds"></param>
        /// <param name="selectUserName"></param>
        /// <returns></returns>
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX, NowFunctionPageUrl = WebCommon.Url_AllotFlowInfo_AssignAllotToUser, AndNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_14 })]
        public ActionResult AssignAllotToUser_SubmitData_Api(string allotIds, string selectUserName, string selectUserTrueName, LoginUser loginUserInfo)
        {
            AjaxResult result = new AjaxResult("分配成功");

            if (allotIds.IsNullOrEmpty())
            {
                result.Result = false;
                result.Message = "请选择用户";
                return AjaxJson(result);
            }

            if (string.IsNullOrEmpty(allotIds))
            {
                result.Result = false;
                result.Message = "请选择要分配的任务";
                return AjaxJson(result);
            }

            //获取当前用户对当前页面拥有的权限
            List<int> functionCodes = _unitOfWork.FunctionService.GetAllBy(loginUserInfo.UserName, loginUserInfo.FxtCompanyId, loginUserInfo.NowCityId, WebCommon.Url_AllotFlowInfo_AssignAllotToUser)

                .Select(m => m.FunctionCode).ToList();

            var nowDepartment = _unitOfWork.DepartmentUserRepository.Get(m => m.FxtCompanyID == loginUserInfo.FxtCompanyId && m.CityID == loginUserInfo.NowCityId && m.UserName == loginUserInfo.UserName).FirstOrDefault();

            var pdu = _unitOfWork.DepartmentUserRepository.Get(m => m.FxtCompanyID == loginUserInfo.FxtCompanyId && m.CityID == loginUserInfo.NowCityId && m.UserName == selectUserName).FirstOrDefault();

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

            return AjaxJson(result);
        }
        #endregion

        #region(AllotDetailed.cshtml)
        [Common.AuthorizeFilterAttribute(NowFunctionPageUrl = WebCommon.Url_AllotFlowInfo_AllotDetailed, OrNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_1, SYSCodeManager.FunOperCode_2, SYSCodeManager.FunOperCode_3 })]
        public ActionResult AllotDetailed(AllotFlow allot, LoginUser loginUser)
        {
            //获取当前用户对任务列表页拥有的权限
            var nowFunctionCodes = _unitOfWork.FunctionService.GetAllBy(loginUser.UserName, loginUser.FxtCompanyId, loginUser.NowCityId, WebCommon.Url_AllotFlowInfo_AllotDetailed).Select(m => m.FunctionCode).ToList();
            //登陆用户所在小组
            var loginDepartment = _unitOfWork.DepartmentUserRepository.GetBy(m => m.CityID == loginUser.NowCityId && m.FxtCompanyID == loginUser.FxtCompanyId && m.UserName == loginUser.UserName);
            //当前信息的发起用户所在小组
            var infoStartDepartment = _unitOfWork.DepartmentUserRepository.GetBy(m => m.CityID == loginUser.NowCityId && m.FxtCompanyID == allot.FxtCompanyId && m.UserName == allot.UserName);
            //当前信息的用户所在小组
            var infoDepartment = _unitOfWork.DepartmentUserRepository.GetBy(m => m.CityID == loginUser.NowCityId && m.FxtCompanyID == allot.FxtCompanyId && m.UserName == allot.SurveyUserName);
            #region 验证查看权限

            //是否用于查看权限
            if (!WebUserHelp.CheckNowPageViewFunctionCode(nowFunctionCodes.ToArray(), loginUser.UserName, allot.UserName, allot.SurveyUserName,
                loginDepartment == null ? 0 : loginDepartment.DepartmentID, infoStartDepartment == null ? 0 : infoStartDepartment.DepartmentID,
                infoDepartment == null ? 0 : infoDepartment.DepartmentID))
            {
                return WebUserHelp.GetActionNotRightPage();//无权限
            }
            #endregion
            #region 验证操作权限
            ViewBag.IsRight = 0;
            //当前状态:已查勘(操作-自审)
            if (allot.StateCode == SYSCodeManager.STATECODE_5)
            {
                if (loginUser.UserName == allot.SurveyUserName)
                {
                    ViewBag.IsRight = 1;
                }
            }
            else if (allot.StateCode == SYSCodeManager.STATECODE_6 || allot.StateCode == SYSCodeManager.STATECODE_8)
            {
                //当前状态:自审通过or审核不通过or审核通过(操作-审核or撤回到已查勘or入库)
                if (WebUserHelp.CheckNowPageAuditFunctionCode(nowFunctionCodes.ToArray(), loginUser.UserName, allot.SurveyUserName,
                loginDepartment == null ? 0 : loginDepartment.DepartmentID, infoDepartment == null ? 0 : infoDepartment.DepartmentID))
                {
                    ViewBag.IsRight = 1;
                }
            }
            #endregion
            //获取采集时间
            var allotSurvey = allot.AllotSurveys.Where(m => m.StateCode == SYSCodeManager.STATECODE_5).OrderByDescending(m => m.StateDate).FirstOrDefault();
            if (allotSurvey != null)
            {
                ViewBag.DataTime = allotSurvey.StateDate;
            }
            //获取审核信息
            if (allot.StateCode == SYSCodeManager.STATECODE_5 || allot.StateCode == SYSCodeManager.STATECODE_6 || allot.StateCode == SYSCodeManager.STATECODE_8 && allot.Checks != null)
            {
                var datCheck = allot.Checks.OrderByDescending(m => m.Id).FirstOrDefault();
                if (datCheck != null)
                {
                    ViewBag.DatCheck = datCheck;
                }
            }

            return View(allot);
        }
        /// <summary>
        /// 获取楼盘信息+楼栋列表
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult AllotDetailed_GetProjectInfo_Api(Project project, LoginUser loginUser)
        {
            AjaxResult result = new AjaxResult("");

            int photoCount = _unitOfWork.P_PhotoRepository.Get(m => m.ProjectId == project.ProjectId && m.CityId == loginUser.NowCityId && (!m.BuildingId.HasValue || m.BuildingId == 0) && m.FxtCompanyId == loginUser.FxtCompanyId).Count();

            SYSCode code = _unitOfWork.CodeService.AllotStatusCodeManager().Where(m => m.Code == project.Status.Value).FirstOrDefault();

            //楼栋信息
            var bList = project.Buildings.Select((b) =>
            {
                return new
                {
                    fxtcompanyid = b.FxtCompanyId,
                    buildingid = b.BuildingId,
                    buildingname = b.BuildingName,
                    cityid = b.CityID,
                    doorplate = b.Doorplate,
                    elevatorrate = b.ElevatorRate,
                    totalfloor = b.TotalFloor.HasValue ? b.TotalFloor.Value : 0,
                    othername = b.OtherName,
                    structurecode = b.StructureCode.HasValue ? b.StructureCode.Value : 0,
                    locationcode = b.LocationCode.HasValue ? b.LocationCode.Value : 0,
                    averageprice = b.AveragePrice.HasValue ? b.AveragePrice.Value.ToString() : "",
                    builddate = b.BuildDate.HasValue ? b.BuildDate.Value.ToString("yyyy-MM-dd") : "",
                    iselevator = b.IsElevator.HasValue ? b.IsElevator.Value : 0,
                    pricedetail = b.PriceDetail,
                    sightcode = b.SightCode.HasValue ? b.SightCode.Value : 0
                }; ;
            }).ToList();

            var pInfo = new { projectname = project.ProjectName, photocount = photoCount, x = project.X == null ? 0 : project.X, y = project.Y == null ? 0 : project.Y, statusname = code == null ? "" : code.CodeName, buildinglist = bList };

            result.Data = pInfo;

            return AjaxJson(result);
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
        public ActionResult GetProjectBuildingHouseTotal(int projectId, LoginUser loginUserInfo)
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
        public ActionResult AllotDetailed_SubmitData_Api(long allotId, string actionType, string remark, LoginUser loginUser)
        {
            AjaxResult result = new AjaxResult("操作成功！");

            if (actionType.Equals("myaudit"))//自审通过
            {
                result.Result = AllotDetailed_MyAudit(allotId, remark, loginUser);
            }
            else if (actionType.Equals("mynotaudit"))//自审不通过
            {
                result.Result = AllotDetailed_MyNotAudit(allotId, remark, loginUser);
            }
            else if (actionType.Equals("audit"))//审核通过
            {
                result.Result = AllotDetailed_Audit(allotId, remark, loginUser);
            }
            else if (actionType.Equals("notaudit"))//审核不通过
            {
                result.Result = AllotDetailed_NotAudit(allotId, remark, loginUser);
            }
            else if (actionType.Equals("notsurvey"))//撤销到已分配(待查勘)
            {
                result.Result = AllotDetailed_NotSurvey(allotId, loginUser);
            }
            else if (actionType.Equals("issurvey"))//撤销到已查勘
            {
                result.Result = AllotDetailed_IsSurvey(allotId, loginUser);
            }
            else if (actionType.Equals("importdata"))//导入到运维中心
            {
                string message = string.Empty;

                result.Result = AllotDetailed_ImportData(allotId, loginUser, out message);

                result.Message = message;
            }

            return AjaxJson(result);
        }
        //自审通过
        public bool AllotDetailed_MyAudit(long allotId, string remark, LoginUser loginUser)
        {

            _unitOfWork.AllotFlowService.SetAllotStatus(loginUser.UserName, loginUser.NowCityId, new long[] { allotId }, SYSCodeManager.STATECODE_5, SYSCodeManager.STATECODE_6,

               "<span class=\"red\">自审通过</span>" + (remark.IsNullOrEmpty() ? "" : ",说明:" + remark), loginUser.TrueName);

            return setCheck(SYSCodeManager.STATECODE_6, allotId, remark, loginUser);

        }
        //自审不通过
        public bool AllotDetailed_MyNotAudit(long allotId, string remark, LoginUser loginUser)
        {
            _unitOfWork.AllotFlowService.SetAllotStatus(loginUser.UserName, loginUser.NowCityId, new long[] { allotId }, SYSCodeManager.STATECODE_5, SYSCodeManager.STATECODE_2,

                "<span class=\"red\">自审不通过</span>" + (remark.IsNullOrEmpty() ? "" : ",说明:" + remark) + "<br/>任务已设置为<span class=\"red\">待查勘</span>", loginUser.TrueName);

            return setCheck(SYSCodeManager.STATECODE_7, allotId, remark, loginUser);
        }
        //审核通过
        public bool AllotDetailed_Audit(long allotId, string remark, LoginUser loginUser)
        {
            _unitOfWork.AllotFlowService.SetAllotStatus(loginUser.UserName, loginUser.NowCityId, new long[] { allotId }, SYSCodeManager.STATECODE_6, SYSCodeManager.STATECODE_8,

                "<span class=\"red\">审核通过</span>" + (remark.IsNullOrEmpty() ? "" : ",说明:" + remark), loginUser.TrueName);

            return setCheck(SYSCodeManager.STATECODE_8, allotId, remark, loginUser);
        }
        //审核不通过
        public bool AllotDetailed_NotAudit(long allotId, string remark, LoginUser loginUser)
        {
            _unitOfWork.AllotFlowService.SetAllotStatus(loginUser.UserName, loginUser.NowCityId, new long[] { allotId }, SYSCodeManager.STATECODE_6, SYSCodeManager.STATECODE_5,

                "<span class=\"red\">审核不通过</span>" + (remark.IsNullOrEmpty() ? "" : ",说明:" + remark) + "<br/>任务已设置为<span class=\"red\">已查勘</span>", loginUser.TrueName);

            return setCheck(SYSCodeManager.STATECODE_9, allotId, remark, loginUser);
        }
        //撤销到已分配
        public bool AllotDetailed_NotSurvey(long allotId, LoginUser loginUser)
        {
            return _unitOfWork.AllotFlowService.SetAllotStatus(loginUser.UserName, loginUser.NowCityId, new long[] { allotId }, SYSCodeManager.STATECODE_5, SYSCodeManager.STATECODE_2, "从自审不通过撤销到已分配(待查勘)", loginUser.TrueName)

                > 0;
        }
        //撤销到已查勘
        public bool AllotDetailed_IsSurvey(long allotId, LoginUser loginUser)
        {
            return _unitOfWork.AllotFlowService.SetAllotStatus(loginUser.UserName, loginUser.NowCityId, new long[] { allotId }, SYSCodeManager.STATECODE_8, SYSCodeManager.STATECODE_5, "从审核不通过撤销到已查勘", loginUser.TrueName)

                > 0;
        }
        //入库
        public bool AllotDetailed_ImportData(long allotId, LoginUser loginUser, out string message)
        {
            return _unitOfWork.AllotFlowService.ImportToDataCenter(allotId, loginUser.NowCityId, loginUser.FxtCompanyId, loginUser.UserName, loginUser.TrueName, loginUser.SignName, loginUser.AppList, out message)

                > 0;
        }

        public bool setCheck(int stateCode, long allotId, string remark, LoginUser loginUser)
        {
            var allotFlow = _unitOfWork.AllotFlowRepository.GetById(allotId);
            var ch = _unitOfWork.CheckRepository.GetBy(m => m.AllotId == allotId);
            if (ch != null)
            {
                if (stateCode == SYSCodeManager.STATECODE_6 || stateCode == SYSCodeManager.STATECODE_7)
                {
                    ch.CheckState1 = stateCode;
                    ch.CheckRemark1 = remark;
                    ch.CheckDate1 = DateTime.Now;
                }
                if (stateCode == SYSCodeManager.STATECODE_8 || stateCode == SYSCodeManager.STATECODE_9)
                {
                    ch.CheckState2 = stateCode;
                    ch.CheckRemark2 = remark;
                    ch.CheckDate2 = DateTime.Now;
                }

                _unitOfWork.CheckRepository.Update(ch);
            }
            else
            {
                _unitOfWork.CheckRepository.Insert(new Check()
                {
                    AllotId = allotFlow.Id,
                    CityId = allotFlow.CityId,
                    FxtCompanyId = allotFlow.FxtCompanyId,
                    DatType = SYSCodeManager.DATATYPECODE_1,
                    DatId = allotFlow.DatId,
                    CheckUserName1 = loginUser.UserName,
                    CheckState1 = stateCode,
                    CheckRemark1 = remark,
                    CheckDate1 = DateTime.Now
                });
            }

            return _unitOfWork.Commit() > 0;
        }
        #endregion

        #region (EditProject.cshtml)
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.OPEN, NowFunctionPageUrl = WebCommon.Url_AllotFlowInfo_EditProject, OrNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_1, SYSCodeManager.FunOperCode_2, SYSCodeManager.FunOperCode_3 })]
        public ActionResult EditProject(AllotFlow allot, LoginUser loginUser, int type = 0)
        {
            //登陆用户所在小组
            var loginDepartment = _unitOfWork.DepartmentUserRepository.GetBy(m => m.CityID == loginUser.NowCityId && m.FxtCompanyID == loginUser.FxtCompanyId && m.UserName == loginUser.UserName);
            //当前信息的发起用户所在小组
            var infoStartDepartment = _unitOfWork.DepartmentUserRepository.GetBy(m => m.CityID == loginUser.NowCityId && m.FxtCompanyID == allot.FxtCompanyId && m.UserName == allot.UserName);
            //当前信息的用户所在小组
            var infoDepartment = _unitOfWork.DepartmentUserRepository.GetBy(m => m.CityID == loginUser.NowCityId && m.FxtCompanyID == allot.FxtCompanyId && m.UserName == allot.SurveyUserName);
            #region 验证查看权限
            //获取当前用户对任务列表页拥有的权限(验证查看权限)
            List<int> nowFunctionCodes = _unitOfWork.FunctionService.GetAllBy(loginUser.UserName, loginUser.FxtCompanyId, loginUser.NowCityId, WebCommon.Url_AllotFlowInfo_EditProject).Select(m => m.FunctionCode).ToList();
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

            if (WebUserHelp.CheckNowPageUpdateFunctionCode(nowFunctionCodes.ToArray(), loginUser.UserName, allot.SurveyUserName,
                loginDepartment == null ? 0 : loginDepartment.DepartmentID, infoDepartment == null ? 0 : infoDepartment.DepartmentID, allot.SurveyUserName)
                && allot.StateCode != SYSCodeManager.STATECODE_10
                //&& (allot.StateCode == SYSCodeManager.STATECODE_5 || allot.StateCode == SYSCodeManager.STATECODE_7 || allot.StateCode == SYSCodeManager.STATECODE_9)
                )
            {
                ViewBag.IsUpdateRight = 1;//有权限
            }
            #endregion
            //获取物业管理
            var lnkpcList = _unitOfWork.P_CompanyRepository.Get(m => m.CityId == loginUser.NowCityId && m.ProjectId == allot.Project.ProjectId).ToList();
            List<FxtApi_SYSArea> areaList = DataCenterAreaApi.GetAreaByCityId(loginUser.NowCityId, loginUser.UserName, loginUser.SignName, loginUser.AppList);
            List<FxtApi_SYSSubArea> subAreaList = DataCenterAreaApi.GetSubAreaByAreaId(allot.Project.AreaID, loginUser.UserName, loginUser.SignName, loginUser.AppList);

            //主用途
            ViewBag.PurposeCode = _unitOfWork.CodeService.PurposeCodeManager();
            //产权形式
            ViewBag.RightCode = _unitOfWork.CodeService.RightCodeManager();
            //物业管理质量
            ViewBag.ManagerQuality = _unitOfWork.CodeService.LevelManager();
            //停车状况
            ViewBag.ParkingStatusCode = _unitOfWork.CodeService.GetAllParkingStatusList();
            //主建筑物类型
            ViewBag.BuildingTypeCode = _unitOfWork.CodeService.BuildingTypeCodeManager();
            //建筑质量
            ViewBag.BuildingQuality = _unitOfWork.CodeService.LevelManager();
            //小区规模
            ViewBag.HousingScale = _unitOfWork.CodeService.HousingScaleCodeManager();
            //配套等级
            ViewBag.AppendageClass = _unitOfWork.CodeService.LevelManager();
            //土地规划用途
            ViewBag.PlanPurpose = _unitOfWork.CodeService.PlanPurposeCodeManager();

            //获取楼盘照片
            List<PPhoto> photoList = _unitOfWork.P_PhotoRepository.Get(m => m.ProjectId == allot.Project.ProjectId && m.CityId == loginUser.NowCityId && (!m.BuildingId.HasValue || m.BuildingId == 0) && m.FxtCompanyId == loginUser.FxtCompanyId).ToList();

            ViewBag.lnkPCList = lnkpcList;
            ViewBag.AreaList = areaList;
            ViewBag.SubAreaList = subAreaList;
            ViewBag.PhotoList = photoList;
            ViewBag.type = type;

            //var templet = allot.Project.TempletContent.ParseJSONjss<TempletDto>();

            //ViewBag.Templet = templet;

            return View(allot);
        }

        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult EditProject_SubmitData_Api(Project project, List<int> PlanPurpose, string DevelopersCompany, string ManagerCompany, LoginUser loginUser)
        {
            var result = new AjaxResult("");

            #region 楼盘
            //行政区 + 楼盘名称||已有的projectid||没有入库
            var exists = _unitOfWork.ProjectRepository.Get(m =>
                    (
                        (m.ProjectName == project.ProjectName && m.AreaID == project.AreaID)
                        && m.ProjectId != project.ProjectId
                    )
                    && m.Status != SYSCodeManager.STATECODE_10 && m.Valid == 1

                ).FirstOrDefault();

            if (exists != null)
            {
                result.Result = false;

                result.Message = "楼盘已存在";

                return AjaxJson(result);
            }

            project.SaveDateTime = DateTime.Now;//最后修改时间

            project.SaveUser = loginUser.UserName;//修改人

            if (PlanPurpose != null)
            {
                project.PlanPurpose = StringHelp.ConvertToString(PlanPurpose.ToArray());
            }
            else
            {
                project.PlanPurpose = string.Empty;
            }

            #endregion

            #region 配套
            var isEx = project.Appendages.Where(m => m.AppendageCode == SYSCodeManager.APPENDAGECODE_14).FirstOrDefault();
            if (isEx != null)
            {
                isEx.ClassCode = project.ParkingNumber;
            }
            else
            {
                project.Appendages.Add(new PAppendage()
                {
                    CityId = 0,
                    ProjectId = 0,
                    ClassCode = project.ParkingNumber,
                    AppendageCode = SYSCodeManager.APPENDAGECODE_14
                });
            }
            #endregion

            #region 关联公司

            if (!DevelopersCompany.IsNullOrEmpty())
            {
                var developers = project.Companys.Where(m => m.CompanyType == SYSCodeApi.COMPANYTYPECODE_1).FirstOrDefault();
                if (developers != null)
                {
                    developers.CompanyName = DevelopersCompany;
                }
                else
                {
                    project.Companys.Add(new PCompany()
                    {
                        CityId = project.CityID,
                        ProjectId = project.ProjectId,
                        CompanyName = DevelopersCompany,
                        CompanyType = SYSCodeApi.COMPANYTYPECODE_1
                    });
                }

            }

            if (!ManagerCompany.IsNullOrEmpty())
            {
                var manager = project.Companys.Where(m => m.CompanyType == SYSCodeApi.COMPANYTYPECODE_4).FirstOrDefault();
                if (manager != null)
                {
                    manager.CompanyName = ManagerCompany;
                }
                else
                {
                    project.Companys.Add(new PCompany()
                    {
                        CityId = project.CityID,
                        ProjectId = project.ProjectId,
                        CompanyName = ManagerCompany,
                        CompanyType = SYSCodeApi.COMPANYTYPECODE_4
                    });
                }
            }

            #endregion

            _unitOfWork.Commit();

            return AjaxJson(result);
        }

        public ActionResult GetSubAreaSelect(int areaId)
        {
            int cityId = WebUserHelp.GetNowCityId();
            List<Apps> appList = new List<Apps>();
            LoginUser loginUser = WebUserHelp.GetNowLoginUser(out appList);
            var query = DataCenterAreaApi.GetSubAreaByAreaId(areaId, loginUser.UserName, loginUser.SignName, appList);
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

        [AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult UploadProjectImage(int projectId)
        {
            var photoTypeList = _unitOfWork.CodeService.PhotoTypeCodeManager();

            ViewBag.photoTypeList = photoTypeList;

            ViewBag.projectId = projectId;

            var coount = _unitOfWork.P_PhotoRepository.Get(m => m.ProjectId == projectId && (!m.BuildingId.HasValue || m.BuildingId == 0)).Count();

            ViewBag.photocount = 20 - coount;

            return View();
        }
        /// <summary>
        /// 上传楼盘图片
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="photoTypeCode">图片类型</param>
        /// <returns></returns>
        [AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX)]
        public async Task<ActionResult> OnUploadProjectImage(int projectId, int photoTypeCode, LoginUser loginUser)
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

                //string path = Path.Combine(folder2, filename);//组织正式文件路径
                string path2 = searchPath2 + "/" + filename;//组织正式文件虚拟路径

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
        public ActionResult UploadProjectXY(Project project, LoginUser loginUser)
        {
            var result = new AjaxResult("定位成功！");

            project.UpdateDateTime = DateTime.Now;

            project.SaveDateTime = DateTime.Now;

            project.SaveUser = loginUser.UserName;

            _unitOfWork.Commit();

            return AjaxJson(result);
        }

        #endregion

        #region (EditBuilding.cshtml)
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.OPEN, NowFunctionPageUrl = WebCommon.Url_AllotFlowInfo_EditBuilding, OrNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_1, SYSCodeManager.FunOperCode_2, SYSCodeManager.FunOperCode_3 })]
        public ActionResult EditBuilding(long allotId, Building building, LoginUser loginUser, int type = 0)
        {
            var allot = _unitOfWork.AllotFlowRepository.GetById(allotId);

            //登陆用户所在小组
            var loginDepartment = _unitOfWork.DepartmentUserRepository.GetBy(m => m.CityID == loginUser.NowCityId && m.FxtCompanyID == loginUser.FxtCompanyId && m.UserName == loginUser.UserName);
            //当前信息的发起用户所在小组
            var infoStartDepartment = _unitOfWork.DepartmentUserRepository.GetBy(m => m.CityID == loginUser.NowCityId && m.FxtCompanyID == allot.FxtCompanyId && m.UserName == allot.UserName);
            //当前信息的用户所在小组
            var infoDepartment = _unitOfWork.DepartmentUserRepository.GetBy(m => m.CityID == loginUser.NowCityId && m.FxtCompanyID == allot.FxtCompanyId && m.UserName == allot.SurveyUserName);
            #region 验证查看权限
            //获取当前用户对任务列表页拥有的权限(验证查看权限)
            List<int> nowFunctionCodes = _unitOfWork.FunctionService.GetAllBy(loginUser.UserName, loginUser.FxtCompanyId, loginUser.NowCityId, WebCommon.Url_AllotFlowInfo_EditBuilding).Select(m => m.FunctionCode).ToList();
            //是否用于查看权限
            if (!WebUserHelp.CheckNowPageViewFunctionCode(nowFunctionCodes.ToArray(), loginUser.UserName, allot.UserName, allot.SurveyUserName,
                loginDepartment == null ? 0 : loginDepartment.DepartmentID, infoStartDepartment == null ? 0 : infoStartDepartment.DepartmentID,
                infoDepartment == null ? 0 : infoDepartment.DepartmentID))
            {
                return WebUserHelp.GetActionNotRightPage();//无权限
            }
            #endregion

            //获取楼盘照片
            IList<PPhoto> photoList = _unitOfWork.P_PhotoRepository.Get(m => m.BuildingId == building.BuildingId && m.BuildingId > 0).ToList();
            ViewBag.PhotoList = photoList;
            ViewBag.PhotoCount = photoList.Count();
            //用途
            ViewBag.PurposeCode = _unitOfWork.CodeService.PurposeCodeManager();
            //维护情况
            ViewBag.MaintenanceCode = _unitOfWork.CodeService.LevelManager();
            //建筑类型
            ViewBag.BuildingTypeCode = _unitOfWork.CodeService.BuildingTypeCodeManager();
            //建筑结构
            ViewBag.StructureCode = _unitOfWork.CodeService.BuildingStructureCodeManager();
            //位置
            //ViewBag.LocationCode = SYSCodeManager.LocationCodeManager();
            //外墙装修
            ViewBag.WallCode = _unitOfWork.CodeService.WallCodeManager();
            //产权形式
            //ViewBag.RightCode = SYSCodeManager.RightCodeManager();
            //内墙装修
            ViewBag.InnerFitmentCode = _unitOfWork.CodeService.InnerFitmentCodeManager();
            //管道燃气
            ViewBag.PipelineGasCode = _unitOfWork.CodeService.PipelineGasCodeManager();
            //采暖方式
            ViewBag.HeatingModeCode = _unitOfWork.CodeService.HeatingModeCodeManager();
            //墙体类型
            ViewBag.WallTypeCode = _unitOfWork.CodeService.WallTypeCodeManager();
            //户型面积
            ViewBag.BHousetypeCode = _unitOfWork.CodeService.BHousetypeCodeManager();

            ViewBag.IsUpdateRight = 0;
            #region 验证修改权限
            //获取当前用户对当前页拥有的权限
            if (WebUserHelp.CheckNowPageUpdateFunctionCode(nowFunctionCodes.ToArray(), loginUser.UserName, allot.SurveyUserName,
                loginDepartment == null ? 0 : loginDepartment.DepartmentID, infoDepartment == null ? 0 : infoDepartment.DepartmentID, allot.SurveyUserName)
                && (allot.StateCode != SYSCodeManager.STATECODE_10))
            //&& (allot.StateCode == SYSCodeManager.STATECODE_5 || allot.StateCode == SYSCodeManager.STATECODE_7 || allot.StateCode == SYSCodeManager.STATECODE_9))
            {
                ViewBag.IsUpdateRight = 1;//有权限
            }
            #endregion

            ViewBag.AllotId = allot.Id;

            ViewBag.ProjectId = allot.DatId;

            ViewBag.type = type;

            if (building.BuildingId < 1)
            {
                var templet = _unitOfWork.TempletRepository.GetBy(m => m.FxtCompanyId == loginUser.FxtCompanyId && m.DatType == SYSCodeManager.DATATYPECODE_2 && m.Vaild == 1 && m.IsCurrent == true);

                if (templet == null)
                {
                    throw new Exception("未设置楼栋模板！");
                }

                building.Templet = templet;

                building.TempletId = templet.TempletId;
            }

            return View(building);
        }
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult EditBuilding_SubmitData_Api(Building building, LoginUser user)
        {
            var result = new AjaxResult("提交成功！");

            if (building.BuildingId > 0)
            {
                var exists = _unitOfWork.BuildingRepository.GetBy(m => m.BuildingName == building.BuildingName && m.BuildingId != building.BuildingId && m.ProjectId == building.ProjectId);

                if (exists != null)
                {
                    result.Message = "楼栋已存在";

                    result.Result = false;

                    return AjaxJson(result);
                }

                building.SaveDateTime = DateTime.Now;

                building.SaveUser = user.UserName;

                var bObj = new { buildingid = building.BuildingId, buildingname = building.BuildingName, cityid = building.CityID, doorplate = building.Doorplate, elevatorrate = building.ElevatorRate, totalfloor = building.TotalFloor };
            }
            else
            {
                var exists = _unitOfWork.BuildingRepository.GetBy(m => m.BuildingName == building.BuildingName && m.CityID == user.NowCityId && m.FxtCompanyId == user.FxtCompanyId && m.ProjectId == building.ProjectId);

                if (exists != null)
                {
                    result.Message = "楼栋已存在";

                    result.Result = false;

                    return AjaxJson(result);
                }

                building.Valid = 1;

                building.CityID = user.NowCityId;

                building.FxtCompanyId = user.FxtCompanyId;

                building.Creator = user.UserName;

                building.CreateTime = DateTime.Now;

                building.AppId = Guid.NewGuid();

                _unitOfWork.BuildingRepository.Insert(building);
            }

            _unitOfWork.Commit();

            return AjaxJson(result);
        }

        [AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult UploadBuildingImage(int projectId, int buildingId)
        {
            var photoTypeList = _unitOfWork.CodeService.PhotoTypeCodeManager();

            ViewBag.photoTypeList = photoTypeList;

            ViewBag.projectId = projectId;

            ViewBag.buildingId = buildingId;

            var coount = _unitOfWork.P_PhotoRepository.Get(m => m.ProjectId == projectId && m.BuildingId == buildingId).Count();

            ViewBag.photocount = 10 - coount;

            return View();
        }

        [AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX)]
        public async Task<ActionResult> OnUploadBuildingImage(int projectId, int buildingId, int photoTypeCode, LoginUser loginUser)
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
        public ActionResult UploadBuildingXY(int buildingId, decimal x, decimal y, LoginUser loginUser)
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
        public ActionResult EditHouse(long allotId, int buildingId, House house, LoginUser loginUser)
        {
            var allot = _unitOfWork.AllotFlowRepository.GetById(allotId);

            //登陆用户所在小组
            var loginDepartment = _unitOfWork.DepartmentUserRepository.GetBy(m => m.CityID == loginUser.NowCityId && m.FxtCompanyID == loginUser.FxtCompanyId && m.UserName == loginUser.UserName);
            //当前信息的发起用户所在小组
            var infoStartDepartment = _unitOfWork.DepartmentUserRepository.GetBy(m => m.CityID == loginUser.NowCityId && m.FxtCompanyID == allot.FxtCompanyId && m.UserName == allot.UserName);
            //当前信息的用户所在小组
            var infoDepartment = _unitOfWork.DepartmentUserRepository.GetBy(m => m.CityID == loginUser.NowCityId && m.FxtCompanyID == allot.FxtCompanyId && m.UserName == allot.SurveyUserName);
            #region 验证查看权限
            //获取当前用户对任务列表页拥有的权限(验证查看权限)
            List<int> nowFunctionCodes = _unitOfWork.FunctionService.GetAllBy(loginUser.UserName, loginUser.FxtCompanyId, loginUser.NowCityId, WebCommon.Url_AllotFlowInfo_EditBuilding).Select(m => m.FunctionCode).ToList();
            //是否用于查看权限
            if (!WebUserHelp.CheckNowPageViewFunctionCode(nowFunctionCodes.ToArray(), loginUser.UserName, allot.UserName, allot.SurveyUserName,
                loginDepartment == null ? 0 : loginDepartment.DepartmentID, infoStartDepartment == null ? 0 : infoStartDepartment.DepartmentID,
                infoDepartment == null ? 0 : infoDepartment.DepartmentID))
            {
                return WebUserHelp.GetActionNotRightPage();//无权限
            }
            #endregion

            //户型结构
            ViewBag.StructureCode = _unitOfWork.CodeService.StructureCodeManager();
            //通风采光
            ViewBag.VDCode = _unitOfWork.CodeService.VDCodeManager();
            //噪音情况
            ViewBag.Noise = _unitOfWork.CodeService.NoiseManager();
            //用途
            ViewBag.PurposeCode = _unitOfWork.CodeService.HousePurposeCodeManager();
            //朝向
            ViewBag.FrontCode = _unitOfWork.CodeService.HouseFrontCodeManager();
            //景观
            ViewBag.SightCode = _unitOfWork.CodeService.HouseSightCodeManager();
            //户型
            ViewBag.HouseTypeCode = _unitOfWork.CodeService.HouseTypeCodeManager();
            //附属房屋类型
            ViewBag.SubHouseType = _unitOfWork.CodeService.HouseSubHouseTypeManager();
            //装修
            ViewBag.FitmentCode = _unitOfWork.CodeService.HouseFitmentCodeTypeManager();

            #region 验证修改权限
            //获取当前用户对当前页拥有的权限
            if (WebUserHelp.CheckNowPageUpdateFunctionCode(nowFunctionCodes.ToArray(), loginUser.UserName, allot.SurveyUserName,
                loginDepartment == null ? 0 : loginDepartment.DepartmentID, infoDepartment == null ? 0 : infoDepartment.DepartmentID, allot.SurveyUserName)
                && (allot.StateCode != SYSCodeManager.STATECODE_10))
            {
                ViewBag.IsUpdateRight = 1;//有权限
            }
            else
            {
                ViewBag.IsUpdateRight = 0;
            }
            #endregion

            ViewBag.UnitNo = _unitOfWork.HouseService.GetUnitNoByUnitNoStr(house.UnitNo);

            ViewBag.HouseNo = _unitOfWork.HouseService.GetHouseNoByUnitNoStr(house.UnitNo);

            ViewBag.BuildingId = buildingId;

            if (house.HouseId < 1)
            {
                var templet = _unitOfWork.TempletRepository.GetBy(m => m.FxtCompanyId == loginUser.FxtCompanyId && m.DatType == SYSCodeManager.DATATYPECODE_4 && m.Vaild == 1 && m.IsCurrent == true);

                if (templet == null)
                {
                    throw new Exception("未设置房号模板！");
                }

                house.Templet = templet;

                house.TempletId = templet.TempletId;
            }

            //默认模板

            return View(house);
        }

        public ActionResult EditHouse_SubmitData_Api(House house, LoginUser user)
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
        public ActionResult ExcelInPost(LoginUser loginUser)
        {
            string msg = string.Empty;
            string msgRow = string.Empty;
            string msgExistRow = string.Empty;//任务已存在
            string path = string.Empty;//文件存放路径

            //模板
            var templet = _unitOfWork.TempletRepository.GetBy(m => m.DatType == SYSCodeManager.DATATYPECODE_1 && m.IsCurrent == true);

            if (templet == null)
            {
                return Json(new { result = 1, message = "失败，未设置模板。" });
            }

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
                                    int indexProject = 0;
                                    int indexallt = 0;
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
                                            project.ProjectId = indexProject++;
                                            project.CityID = loginUser.NowCityId;
                                            project.CreateTime = DateTime.Now;
                                            project.SaveDateTime = DateTime.Now;
                                            project.Creator = loginUser.UserName;
                                            project.SaveUser = loginUser.UserName;
                                            project.Valid = 1;
                                            project.TempletId = templet.TempletId;
                                            project.X = default(decimal);
                                            project.Y = default(decimal);
                                            project.Status = 1035001;
                                            project.FxtCompanyId = loginUser.FxtCompanyId;

                                            #region 绑定楼盘信息

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
                                            {
                                                var list = _unitOfWork.CodeService.PurposeCodeManager();

                                                var code = list.FirstOrDefault(m => m.CodeName == row["主用途"].ToString());

                                                if (code != null)
                                                {
                                                    project.PurposeCode = code.Code;
                                                }
                                            }

                                            if (dt.Columns.Contains("产权形式"))
                                            {
                                                var list = _unitOfWork.CodeService.RightCodeManager();

                                                var code = list.FirstOrDefault(m => m.CodeName == row["产权形式"].ToString());

                                                if (code != null)
                                                {
                                                    project.RightCode = code.Code;
                                                }
                                            }

                                            if (dt.Columns.Contains("物业管理质量"))
                                            {
                                                List<SYSCode> list = _unitOfWork.CodeService.LevelManager();

                                                var code = list.FirstOrDefault(m => m.CodeName == row["物业管理质量"].ToString());

                                                if (code != null)
                                                {
                                                    project.ManagerQuality = code.Code;
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


                                            //project = _unitOfWork.ProjectRepository.Insert(project);
                                            //_unitOfWork.Commit();
                                            //row[24] row[25]



                                            if (dt.Columns.Contains("入伙日期") && StringHelp.CheckIsDate(row["入伙日期"].ToString()))
                                            {
                                                project.JoinDate = Convert.ToDateTime(row["入伙日期"]);//入伙日期
                                            }

                                            if (dt.Columns.Contains("宗地号"))
                                            {
                                                project.FieldNo = GetExcelField(row["宗地号"]).ToString();//宗地号
                                            }

                                            if (dt.Columns.Contains("内部认购日期") && StringHelp.CheckIsDate(row["内部认购日期"].ToString()))
                                            {
                                                project.InnerSaleDate = Convert.ToDateTime(row["内部认购日期"]);//入伙日期
                                            }

                                            if (dt.Columns.Contains("土地使用年限") && StringHelp.CheckInteger(row["土地使用年限"].ToString()))
                                            {
                                                project.UsableYear = Convert.ToInt32(row["土地使用年限"]);//入伙日期
                                            }

                                            if (dt.Columns.Contains("土地起始日期") && StringHelp.CheckIsDate(row["土地起始日期"].ToString()))
                                            {
                                                project.StartDate = Convert.ToDateTime(row["土地起始日期"]);//入伙日期
                                            }

                                            if (dt.Columns.Contains("开盘日期") && StringHelp.CheckIsDate(row["开盘日期"].ToString()))
                                            {
                                                project.SaleDate = Convert.ToDateTime(row["开盘日期"]);//入伙日期
                                            }

                                            if (dt.Columns.Contains("封顶日期") && StringHelp.CheckIsDate(row["封顶日期"].ToString()))
                                            {
                                                project.CoverDate = Convert.ToDateTime(row["封顶日期"]);//入伙日期
                                            }

                                            if (dt.Columns.Contains("开工日期") && StringHelp.CheckIsDate(row["开工日期"].ToString()))
                                            {
                                                project.BuildingDate = Convert.ToDateTime(row["开工日期"]);//入伙日期
                                            }

                                            if (dt.Columns.Contains("主建筑物类型"))
                                            {
                                                var list = _unitOfWork.CodeService.BuildingTypeCodeManager();

                                                var code = list.FirstOrDefault(m => m.CodeName == row["主建筑物类型"].ToString());

                                                if (code != null)
                                                {
                                                    project.BuildingTypeCode = code.Code;
                                                }
                                            }

                                            if (dt.Columns.Contains("建筑面积") && StringHelp.CheckDecimal(row["建筑面积"].ToString()))
                                            {
                                                project.BuildingArea = Convert.ToDecimal(row["建筑面积"]);
                                            }

                                            if (dt.Columns.Contains("其他用途面积") && StringHelp.CheckDecimal(row["其他用途面积"].ToString()))
                                            {
                                                project.OtherArea = Convert.ToDecimal(row["其他用途面积"]);
                                            }

                                            if (dt.Columns.Contains("建筑质量"))
                                            {
                                                var list = _unitOfWork.CodeService.LevelManager();

                                                var code = list.FirstOrDefault(m => m.CodeName == row["建筑质量"].ToString());

                                                if (code != null)
                                                {
                                                    project.BuildingQuality = code.Code;
                                                }
                                            }

                                            if (dt.Columns.Contains("办公面积") && StringHelp.CheckDecimal(row["办公面积"].ToString()))
                                            {
                                                project.OfficeArea = Convert.ToDecimal(row["办公面积"]);
                                            }

                                            if (dt.Columns.Contains("工业面积") && StringHelp.CheckDecimal(row["工业面积"].ToString()))
                                            {
                                                project.IndustryArea = Convert.ToDecimal(row["工业面积"]);
                                            }

                                            if (dt.Columns.Contains("商业面积") && StringHelp.CheckDecimal(row["商业面积"].ToString()))
                                            {
                                                project.BusinessArea = Convert.ToDecimal(row["商业面积"]);
                                            }

                                            if (dt.Columns.Contains("小区规模"))
                                            {
                                                var list = _unitOfWork.CodeService.HousingScaleCodeManager();

                                                var code = list.FirstOrDefault(m => m.CodeName == row["小区规模"].ToString());

                                                if (code != null)
                                                {
                                                    project.HousingScale = code.Code;
                                                }
                                            }

                                            if (dt.Columns.Contains("可销售面积") && StringHelp.CheckDecimal(row["可销售面积"].ToString()))
                                            {
                                                project.SalableArea = Convert.ToDecimal(row["可销售面积"]);
                                            }

                                            if (dt.Columns.Contains("占地面积") && StringHelp.CheckDecimal(row["占地面积"].ToString()))
                                            {
                                                project.LandArea = Convert.ToDecimal(row["占地面积"]);
                                            }

                                            if (dt.Columns.Contains("物业费"))
                                            {
                                                project.ManagerPrice = row["物业费"].ToString();
                                            }

                                            if (dt.Columns.Contains("容积率") && StringHelp.CheckDecimal(row["容积率"].ToString()))
                                            {
                                                project.CubageRate = Convert.ToDecimal(row["容积率"]);
                                            }

                                            if (dt.Columns.Contains("绿化率") && StringHelp.CheckDecimal(row["绿化率"].ToString()))
                                            {
                                                project.GreenRate = Convert.ToDecimal(row["绿化率"]);
                                            }

                                            if (dt.Columns.Contains("拼音简写"))
                                            {
                                                project.PinYin = row["拼音简写"].ToString();
                                            }

                                            if (dt.Columns.Contains("项目均价") && StringHelp.CheckDecimal(row["项目均价"].ToString()))
                                            {
                                                project.AveragePrice = Convert.ToDecimal(row["项目均价"]);
                                            }

                                            if (dt.Columns.Contains("开盘均价") && StringHelp.CheckDecimal(row["开盘均价"].ToString()))
                                            {
                                                project.SalePrice = Convert.ToDecimal(row["开盘均价"]);
                                            }

                                            if (dt.Columns.Contains("管理处电话"))
                                            {
                                                project.ManagerTel = row["管理处电话"].ToString();
                                            }

                                            if (dt.Columns.Contains("土地规划用途"))
                                            {
                                                var list = _unitOfWork.CodeService.PlanPurposeCodeManager();

                                                string planPurpose = row["土地规划用途"].ToString();

                                                if (!planPurpose.IsNullOrEmpty())
                                                {
                                                    string[] planPurposes = planPurpose.Split(',');

                                                    List<string> pps = new List<string>();

                                                    for (int i = 0; i < planPurposes.Length; i++)
                                                    {
                                                        var code = list.FirstOrDefault(m => m.CodeName == planPurposes[i]);

                                                        if (code != null)
                                                        {
                                                            pps.Add(code.Code.ToString());
                                                        }
                                                    }

                                                    project.PlanPurpose = string.Join(",", pps);
                                                }
                                            }

                                            if (dt.Columns.Contains("楼盘名称全拼"))
                                            {
                                                project.PinYinAll = row["楼盘名称全拼"].ToString();
                                            }

                                            if (dt.Columns.Contains("地下室用途"))
                                            {
                                                project.BasementPurpose = row["地下室用途"].ToString();
                                            }

                                            if (dt.Columns.Contains("区域分析"))
                                            {
                                                project.RegionalAnalysis = row["区域分析"].ToString();
                                            }

                                            if (dt.Columns.Contains("是否完成基础数据"))
                                            {
                                                string isComplete = row["是否完成基础数据"].ToString();

                                                if (isComplete == "是")
                                                {
                                                    project.IsComplete = 1;
                                                }
                                                else if (isComplete == "否")
                                                {
                                                    project.IsComplete = 0;

                                                }
                                            }

                                            if (dt.Columns.Contains("不利因素"))
                                            {
                                                project.Aversion = row["不利因素"].ToString();
                                            }

                                            if (dt.Columns.Contains("有利因素"))
                                            {
                                                project.Wrinkle = row["有利因素"].ToString();
                                            }

                                            if (dt.Columns.Contains("是否可估"))
                                            {
                                                string isEValue = row["是否可估"].ToString();

                                                if (isEValue == "是")
                                                {
                                                    project.IsEValue = 1;
                                                }
                                                else if (isEValue == "否")
                                                {
                                                    project.IsEValue = 0;

                                                }
                                            }

                                            if (dt.Columns.Contains("车位描述"))
                                            {
                                                project.ParkingDesc = row["车位描述"].ToString();
                                            }

                                            if (dt.Columns.Contains("配套等级"))
                                            {
                                                var list = _unitOfWork.CodeService.LevelManager();

                                                var code = list.FirstOrDefault(m => m.CodeName == row["配套等级"].ToString());

                                                if (code != null)
                                                {
                                                    project.AppendageClass = code.Code;
                                                }
                                            }

                                            if (dt.Columns.Contains("设备设施"))
                                            {
                                                project.Facilities = row["设备设施"].ToString();
                                            }

                                            if (dt.Columns.Contains("楼栋备注"))
                                            {
                                                project.BuildingDetail = row["楼栋备注"].ToString();
                                            }

                                            if (dt.Columns.Contains("房号备注"))
                                            {
                                                project.HouseDetail = row["房号备注"].ToString();
                                            }
                                            #endregion

                                            _unitOfWork.ProjectRepository.Insert(project);

                                            if (dt.Columns.Contains("开发商"))
                                            {
                                                string value = row["开发商"].ToString();

                                                //var kf = _unitOfWork.P_CompanyRepository.Get(m => m.ProjectId == project.ProjectId && m.CompanyType == 2001001 && m.CityId == loginUser.NowCityId && m.CompanyName == value).FirstOrDefault();

                                                //if (kf == null)
                                                //{
                                                PCompany kfs = new PCompany();
                                                kfs.CityId = loginUser.NowCityId;
                                                kfs.CompanyType = 2001001;
                                                kfs.CompanyName = value;
                                                kfs.ProjectId = project.ProjectId;
                                                _unitOfWork.P_CompanyRepository.Insert(kfs);
                                                //}
                                            }

                                            if (dt.Columns.Contains("物管公司"))
                                            {
                                                string value = row["物管公司"].ToString();

                                                //var vgg = _unitOfWork.P_CompanyRepository.Get(m => m.ProjectId == project.ProjectId && m.CompanyType == 2001004 && m.CityId == loginUser.NowCityId && m.CompanyName == value).FirstOrDefault();

                                                //if (vgg == null)
                                                //{
                                                PCompany wggs = new PCompany();
                                                wggs.CityId = loginUser.NowCityId;
                                                wggs.CompanyType = 2001004;
                                                wggs.CompanyName = value;
                                                wggs.ProjectId = project.ProjectId;
                                                _unitOfWork.P_CompanyRepository.Insert(wggs);
                                                //}
                                            }


                                            if (dt.Columns.Contains("停车状况"))
                                            {
                                                var list = _unitOfWork.CodeService.GetAllParkingStatusList();

                                                var code = list.FirstOrDefault(m => m.CodeName == row["停车状况"].ToString());

                                                if (code != null)
                                                {
                                                    PAppendage parkingStatus = new PAppendage();
                                                    parkingStatus.AppendageCode = SYSCodeManager.APPENDAGECODE_14;
                                                    parkingStatus.CityId = project.CityID;
                                                    parkingStatus.ClassCode = code.Code;
                                                    parkingStatus.ProjectId = project.ProjectId;
                                                    _unitOfWork.P_AppendageRepository.Insert(parkingStatus);
                                                }
                                            }


                                            //任务表
                                            AllotFlow flow = new AllotFlow();
                                            flow.Id = indexallt++;
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

                                            _unitOfWork.AllotFlowRepository.Insert(flow);
                                            //flow = _unitOfWork.AllotFlowRepository.Insert(flow);

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
                                        }
                                        else
                                        {
                                            msgRow += rowsnum + ",";
                                        }
                                        rowsnum++;
                                        #endregion
                                    }

                                    _unitOfWork.Commit();


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
        /// Excel模板
        /// </summary>
        /// <returns></returns>
        public ActionResult ExcelOut(LoginUser user)
        {
            var templet = _unitOfWork.TempletService.GetTempletDefult(SYSCodeManager.DATATYPECODE_1, user.FxtCompanyId);

            var fields = templet.FieldGroups.SelectMany(m => m.Fields).Select(m => m.Title).ToList();

            string fileNamePath = Server.MapPath("~/OutExcel/楼盘任务导入格式.xlsx");

            using (var ms = ExcelHandle.RenderToExcelHiddenColumn(fileNamePath, fields))
            {
                return new FileContentResult(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
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
        /// 获取区域ID
        /// </summary>
        /// <param name="cityid"></param>
        /// <returns></returns>
        public static int GetAreaId(int cityid, string areaname)
        {
            int areaid = 0;
            List<Apps> appList = new List<Apps>();
            LoginUser loginUserInfo = WebUserHelp.GetNowLoginUser(out appList);
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
            List<Apps> appList = new List<Apps>();
            LoginUser loginUserInfo = WebUserHelp.GetNowLoginUser(out appList);
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
            List<Apps> appList = new List<Apps>();
            LoginUser loginUserInfo = WebUserHelp.GetNowLoginUser(out appList);
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
