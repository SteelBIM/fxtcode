namespace FxtDataAcquisition.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Collections.Generic;

    using log4net;

    using FxtDataAcquisition.Common;
    using FxtDataAcquisition.Web.Common;
    using FxtDataAcquisition.Domain.Models;
    using FxtDataAcquisition.Domain.DTO.FxtDataWcfDTO;
    using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
    using FxtDataAcquisition.Application.Services;
    using FxtDataAcquisition.Application.Interfaces;
    using FxtDataAcquisition.FxtAPI.FxtDataCenter.Manager;

    /// <summary>
    /// 任务导入
    /// </summary>
    public class DatabaseCallController : BaseController
    {
        public DatabaseCallController(IAdminService unitOfWork)
            : base(unitOfWork)
        {
        }

        public static readonly ILog log = LogManager.GetLogger(typeof(DatabaseCallController));

        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX, NowFunctionPageUrl = WebCommon.Url_DatabaseCall_Index, OrNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_3, SYSCodeManager.FunOperCode_2 })]
        public ActionResult Index(LoginUser loginUser)
        {
            List<SYSCode> colist = DataCenterCodeApi.GetCodeById(1035, loginUser.UserName, loginUser.SignName, loginUser.AppList);

            List<FxtApi_SYSArea> areaList = DataCenterAreaApi.GetAreaByCityId(loginUser.NowCityId, loginUser.UserName, loginUser.SignName, loginUser.AppList);

            //获取当前用户在此页面所有的操作权限
            List<int> functionCodes = _unitOfWork.FunctionService.GetAllBy(loginUser.UserName, loginUser.FxtCompanyId, loginUser.NowCityId, WebCommon.Url_AllotFlowInfo_AllotFlowManager).Select(m => m.FunctionCode).ToList();

            ViewBag.AreaList = areaList;

            ViewBag.FunctionCodes = functionCodes;

            return View();
        }

        /// <summary>
        /// 楼盘列表
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="areaId"></param>
        /// <param name="subAreaId"></param>
        /// <param name="functionCodes"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="isGetCount"></param>
        /// <returns></returns>
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX, NowFunctionPageUrl = WebCommon.Url_DatabaseCall_Index, OrNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_3, SYSCodeManager.FunOperCode_2 })]
        public ActionResult GetList_Api(string projectName, int areaId, int subAreaId, string functionCodes, int pageIndex, int pageSize, int isGetCount, LoginUser loginUserInfo)
        {
            var result = new AjaxResult("");

            var list = DataCenterProjectApi.GetProject(projectName, loginUserInfo.NowCityId, areaId, subAreaId, pageIndex, pageSize, loginUserInfo.UserName, loginUserInfo.SignName, loginUserInfo.AppList).ToList();

            list.ForEach((o) =>
            {
                var project = _unitOfWork.ProjectRepository.GetBy(m => m.FxtCompanyId == o.fxtcompanyid && m.CityID == o.cityid && m.FxtProjectId == o.projectid && m.Status != SYSCodeManager.STATECODE_10 && m.Valid == 1);

                if (project != null)
                {
                    o.valid = 0;
                }
            });

            result.Data = list;

            return Json(result);
        }

        public ActionResult ProjectInfo(int projectId, LoginUser loginUserInfo)
        {
            var project = DataCenterProjectApi.GetProjectInfo(loginUserInfo.NowCityId, projectId, loginUserInfo.UserName, loginUserInfo.SignName, loginUserInfo.AppList).FirstOrDefault();

            //停车状况
            ViewBag.ParkingStatusCode = _unitOfWork.CodeService.GetAllParkingStatusList();

            return View(project);
        }

        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.ACTION, NowFunctionPageUrl = WebCommon.Url_DatabaseCall_Index, OrNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_3, SYSCodeManager.FunOperCode_2 })]
        public ActionResult BuildingIndex(int projectId, LoginUser loginUser)
        {
            //获取当前用户在此页面所有的操作权限
            List<int> functionCodes = _unitOfWork.FunctionService.GetAllBy(loginUser.UserName, loginUser.FxtCompanyId, loginUser.NowCityId, WebCommon.Url_AllotFlowInfo_AllotFlowManager).Select(m => m.FunctionCode).ToList();

            ViewBag.FunctionCodes = functionCodes;

            ViewBag.ProjectID = projectId;

            return View();
        }

        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX, NowFunctionPageUrl = WebCommon.Url_DatabaseCall_Index, OrNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_3, SYSCodeManager.FunOperCode_2 })]
        public ActionResult GetBuildingList_Api(string buildingName, int projectId, string functionCodes, int pageIndex, int pageSize, int isGetCount, LoginUser loginUserInfo)
        {
            IList<CAS.Entity.DBEntity.DATBuilding> list = DataCenterBuildingApi.GetBuilding(buildingName, loginUserInfo.NowCityId, projectId, pageIndex, pageSize, loginUserInfo.UserName, loginUserInfo.SignName, loginUserInfo.AppList);
            string json = "{{\"Count\":{0},\"List\":{1}}}";
            int count = list.Count;
            json = string.Format(json, count, (list as List<CAS.Entity.DBEntity.DATBuilding>).ToJSONjss());
            return Content("");
        }

        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX, NowFunctionPageUrl = WebCommon.Url_DatabaseCall_Index, AndNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_10 })]
        public ActionResult CallAll(LoginUser loginUserInfo)
        {
            string mes = string.Empty;
            List<CAS.Entity.DBEntity.DATProject> projectList = DataCenterProjectApi.GetProjectBuildingHouseAll(loginUserInfo.NowCityId, loginUserInfo.UserName, loginUserInfo.SignName, loginUserInfo.AppList);
            //DatabaseCallManager.Call(projectList);
            if (projectList != null && projectList.Count > 0)
            {
                _unitOfWork.ProjectService.ImputProject(projectList, loginUserInfo.UserName, loginUserInfo.TrueName, out mes);
            }
            if (!string.IsNullOrEmpty(mes))
            {
                //mes += "部分任务导入完成，楼盘：";
                mes += ",不能重复导入。";
            }
            return Json(new { result = 1, message = mes });
        }

        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX, NowFunctionPageUrl = WebCommon.Url_DatabaseCall_Index, AndNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_10 })]
        public ActionResult Call(string ids, LoginUser loginUserInfo)
        {
            string mes = string.Empty;

            List<CAS.Entity.DBEntity.DATProject> projectList = DataCenterProjectApi.GetProjectBuildingHouse(loginUserInfo.NowCityId, ids, loginUserInfo.UserName, loginUserInfo.SignName, loginUserInfo.AppList);
            //log.Info("数据中心楼盘:" + projectList.Count);
            if (projectList != null && projectList.Count > 0)
            {
                _unitOfWork.ProjectService.ImputProject(projectList, loginUserInfo.UserName, loginUserInfo.TrueName, out mes);
            }

            if (!string.IsNullOrEmpty(mes))
            {
                //mes += "部分任务导入完成，楼盘：";
                mes += ",不能重复导入。";
            }
            return Json(new { result = 1, message = mes });
        }

        public ActionResult Add(LoginUser loginUserInfo)
        {
            List<FxtApi_SYSArea> areaList = DataCenterAreaApi.GetAreaByCityId(loginUserInfo.NowCityId, loginUserInfo.UserName, loginUserInfo.SignName, loginUserInfo.AppList);

            ViewBag.AreaList = areaList;

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
            ViewBag.PlanPurpose = _unitOfWork.CodeService.PurposeCodeManager();

            ViewBag.CityId = loginUserInfo.NowCityId;

            var templet = _unitOfWork.TempletRepository.GetBy(m => m.FxtCompanyId == loginUserInfo.FxtCompanyId && m.DatType == SYSCodeManager.DATATYPECODE_1 && m.Vaild == 1 && m.IsCurrent == true);

            if (templet == null)
            {
                throw new Exception("未设置楼盘模板！");
            }

            return View(templet);
        }
        [HttpPost]
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX, NowFunctionPageUrl = WebCommon.Url_DatabaseCall_Index, AndNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_4 })]
        public ActionResult Add(Project project, string developersCompany, string managerCompany, LoginUser loginUserInfo)
        {
            AjaxResult result = new AjaxResult("新增任务成功！！！");

            //检查是否已存在任务
            Project exists = _unitOfWork.AllotFlowService.ExistsAllot(project);

            if (exists == null)
            {
                var ret = _unitOfWork.AllotFlowService.AddAllot(project, developersCompany, managerCompany, " 从调度中心 <span class=\"red\">创建任务</span>",

                            loginUserInfo.NowCityId, loginUserInfo.FxtCompanyId, loginUserInfo.UserName, loginUserInfo.TrueName, SYSCodeManager.STATECODE_1);

                if (ret.AllotState == 0)
                {
                    result.Result = false;

                    result.Message = "新增任务失败。";
                }
                else if (ret.AllotState == -1)
                {
                    result.Result = false;

                    result.Message = "失败，未设置模板。";
                }
            }
            else
            {
                result.Result = false;

                result.Message = "任务重复";
            }

            return AjaxJson(result);
        }
    }
}
