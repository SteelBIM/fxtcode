using FxtDataAcquisition.BLL;
using FxtDataAcquisition.FxtAPI.FxtDataCenter.Manager;
using FxtDataAcquisition.Web.Common;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FxtDataAcquisition.Common;
using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
using FxtDataAcquisition.Domain.Models;
using FxtDataAcquisition.Domain.DTO.FxtDataWcfDTO;
using FxtDataAcquisition.Application.Services;
using FxtDataAcquisition.Application.Interfaces;
using AutoMapper;

namespace FxtDataAcquisition.Web.Controllers
{
    /// <summary>
    /// 数据库调出
    /// </summary>
    public class DatabaseCallController : BaseController
    {
        public DatabaseCallController(IAdminService unitOfWork)
            : base(unitOfWork)
        {
        }

        public static readonly ILog log = LogManager.GetLogger(typeof(DatabaseCallController));

        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX, NowFunctionPageUrl = WebCommon.Url_DatabaseCall_Index, OrNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_3, SYSCodeManager.FunOperCode_2 })]
        public ActionResult Index(UserCenter_LoginUserInfo loginUser)
        {
            List<SYSCode> colist = DataCenterCodeApi.GetCodeById(1035, loginUser.UserName, loginUser.SignName, loginUser.AppList);

            List<FxtApi_SYSArea> areaList = DataCenterAreaApi.GetAreaByCityId(loginUser.NowCityId, loginUser.UserName, loginUser.SignName, loginUser.AppList);

            ViewBag.AreaList = areaList;

            //获取当前用户在此页面所有的操作权限
            List<int> functionCodes = WebUserHelp.GetNowPageFunctionCodes(loginUser.UserName, loginUser.FxtCompanyId, WebCommon.Url_AllotFlowInfo_AllotFlowManager);

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
        public ActionResult GetList_Api(string projectName, int areaId, int subAreaId, string functionCodes, int pageIndex, int pageSize, int isGetCount, UserCenter_LoginUserInfo loginUserInfo)
        {
            var result = new AjaxResult("");

            var list = DataCenterProjectApi.GetProject(projectName, loginUserInfo.NowCityId, areaId, subAreaId, pageIndex, pageSize, loginUserInfo.UserName, loginUserInfo.SignName, loginUserInfo.AppList).ToList();

            list.ForEach((o) =>
            {
                var project = _unitOfWork.ProjectRepository.GetBy(m => m.FxtCompanyId == o.fxtcompanyid && m.CityID == o.cityid && 
                    m.FxtProjectId == o.projectid && m.Status != SYSCodeManager.STATECODE_10 && m.Valid == 1);
                if (project != null)
                {
                    o.valid = 0;
                }
            });

            result.Data = list;

            return Json(result);
        }

        public ActionResult ProjectInfo(int projectId, UserCenter_LoginUserInfo loginUserInfo)
        {
            var project = DataCenterProjectApi.GetProjectInfo(loginUserInfo.NowCityId, projectId, loginUserInfo.UserName, loginUserInfo.SignName, loginUserInfo.AppList).FirstOrDefault();

            //停车状况
            ViewBag.ParkingStatusCode = SYSCodeManager.GetAllParkingStatusList();

            return View(project);
        }

        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.ACTION, NowFunctionPageUrl = WebCommon.Url_DatabaseCall_Index, OrNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_3, SYSCodeManager.FunOperCode_2 })]
        public ActionResult BuildingIndex(int projectId, UserCenter_LoginUserInfo loginUserInfo)
        {
            //获取当前用户在此页面所有的操作权限
            List<int> functionCodes = WebUserHelp.GetNowPageFunctionCodes(loginUserInfo.UserName, loginUserInfo.FxtCompanyId, WebCommon.Url_AllotFlowInfo_AllotFlowManager);

            ViewBag.FunctionCodes = functionCodes;

            ViewBag.ProjectID = projectId;

            return View();
        }

        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX, NowFunctionPageUrl = WebCommon.Url_DatabaseCall_Index, OrNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_3, SYSCodeManager.FunOperCode_2 })]
        public ActionResult GetBuildingList_Api(string buildingName, int projectId, string functionCodes, int pageIndex, int pageSize, int isGetCount, UserCenter_LoginUserInfo loginUserInfo)
        {
            IList<CAS.Entity.DBEntity.DATBuilding> list = DataCenterBuildingApi.GetBuilding(buildingName, loginUserInfo.NowCityId, projectId, pageIndex, pageSize, loginUserInfo.UserName, loginUserInfo.SignName, loginUserInfo.AppList);
            string json = "{{\"Count\":{0},\"List\":{1}}}";
            int count = list.Count;
            json = string.Format(json, count, (list as List<CAS.Entity.DBEntity.DATBuilding>).ToJSONjss());
            return Content(json.MvcResponseJson());
        }

        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX, NowFunctionPageUrl = WebCommon.Url_DatabaseCall_Index, AndNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_10 })]
        public ActionResult CallAll(UserCenter_LoginUserInfo loginUserInfo)
        {
            string mes = string.Empty;
            List<CAS.Entity.DBEntity.DATProject> projectList = DataCenterProjectApi.GetProjectBuildingHouseAll(loginUserInfo.NowCityId, loginUserInfo.UserName, loginUserInfo.SignName, loginUserInfo.AppList);
            //DatabaseCallManager.Call(projectList);
            if (projectList != null && projectList.Count > 0)
            {
                foreach (var item in projectList)
                {
                    //行政区 + 楼盘名称||已有的projectid||没有入库
                    var exists = _unitOfWork.ProjectRepository.Get(m =>
                        (m.ProjectName == item.projectname ||
                        m.ProjectName == item.areaname + item.projectname ||
                        m.FxtProjectId == item.projectid) && m.Status != SYSCodeManager.STATECODE_10
                        ).FirstOrDefault();
                    if (exists != null)
                    {
                        mes += item.projectname + ",";
                        continue;//任务重复
                    }

                    DateTime now = DateTime.Now;
                    //楼盘
                    Project project = Mapper.Map<CAS.Entity.DBEntity.DATProject, Project>(item);
                    project.FxtProjectId = item.projectid;
                    project.FxtCompanyId = item.fxtcompanyid;
                    project.ProjectId = 0;
                    project.Status = SYSCodeManager.STATECODE_1;
                    project.CreateTime = now;
                    project.X = item.projectx;
                    project.Y = item.projecty;
                    project.Valid = 1;
                    project = _unitOfWork.ProjectRepository.Insert(project);
                    //_unitOfWork.Commit();
                    //任务
                    var allot = new AllotFlow()
                    {
                        CityId = project.CityID,
                        CreateTime = now,
                        DatType = SYSCodeManager.DATATYPECODE_1,
                        FxtCompanyId = project.FxtCompanyId.Value,
                        UserName = loginUserInfo.UserName,
                        UserTrueName = loginUserInfo.TrueName,
                        StateCode = SYSCodeManager.STATECODE_1,
                        DatId = project.ProjectId,
                        StateDate = now
                    };
                    allot = _unitOfWork.AllotFlowRepository.Insert(allot);
                    //_unitOfWork.Commit();
                    //任务记录
                    var allotSurvey = new AllotSurvey()
                    {
                        AllotId = allot.Id,
                        CityId = allot.CityId,
                        CreateDate = now,
                        StateCode = SYSCodeManager.STATECODE_1,
                        StateDate = now,
                        UserName = loginUserInfo.UserName,
                        TrueName = loginUserInfo.TrueName,
                        Remark = "从调度中心（数据中心） <span class=\"red\">导入任务</span>"
                    };
                    allotSurvey = _unitOfWork.AllotSurveyRepository.Insert(allotSurvey);
                    //停车状况
                    var parkingStatus = _unitOfWork.P_AppendageRepository.Get(m => m.AppendageCode == SYSCodeManager.APPENDAGECODE_14 && m.CityId == project.CityID && m.ProjectId == project.ProjectId).FirstOrDefault();
                    if (parkingStatus != null)
                    {
                        parkingStatus.ClassCode = project.ParkingStatus;
                        _unitOfWork.P_AppendageRepository.Update(parkingStatus);
                    }
                    else
                    {
                        parkingStatus = new PAppendage();
                        parkingStatus.ClassCode = project.ParkingStatus;
                        parkingStatus.AppendageCode = SYSCodeManager.APPENDAGECODE_14;
                        parkingStatus.CityId = project.CityID;
                        parkingStatus.ProjectId = project.ProjectId;
                        parkingStatus.IsInner = true;
                        _unitOfWork.P_AppendageRepository.Insert(parkingStatus);
                    }
                    //开发商
                    if (!string.IsNullOrEmpty(item.devecompanyname))
                    {
                        var developcompany = _unitOfWork.P_CompanyRepository.Get(m => m.CompanyName == item.devecompanyname && m.CompanyType == SYSCodeManager.COMPANYTYPECODE_1
                            && m.ProjectId == project.ProjectId && m.CityId == project.CityID).FirstOrDefault();
                        if (developcompany == null)
                        {
                            developcompany = new PCompany();
                            developcompany.CityId = project.CityID;
                            developcompany.CompanyName = item.devecompanyname;
                            developcompany.CompanyType = SYSCodeManager.COMPANYTYPECODE_1;
                            developcompany.ProjectId = project.ProjectId;
                            _unitOfWork.P_CompanyRepository.Insert(developcompany);
                        }
                    }
                    //物管公司
                    if (!string.IsNullOrEmpty(item.managercompanyname))
                    {
                        var managercompany = _unitOfWork.P_CompanyRepository.Get(m => m.CompanyName == item.devecompanyname && m.CompanyType == SYSCodeManager.COMPANYTYPECODE_4
                            && m.ProjectId == project.ProjectId && m.CityId == project.CityID).FirstOrDefault();
                        if (managercompany == null)
                        {
                            managercompany = new PCompany();
                            managercompany.CityId = project.CityID;
                            managercompany.CompanyName = item.devecompanyname;
                            managercompany.CompanyType = SYSCodeManager.COMPANYTYPECODE_4;
                            managercompany.ProjectId = project.ProjectId;
                            _unitOfWork.P_CompanyRepository.Insert(managercompany);
                        }
                    }
                }
                _unitOfWork.Commit();
            }
            if (!string.IsNullOrEmpty(mes))
            {
                //mes += "部分任务导入完成，楼盘：";
                mes += ",不能重复导入。";
            }
            return Json(new { result = 1, message = mes });
        }

        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX, NowFunctionPageUrl = WebCommon.Url_DatabaseCall_Index, AndNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_10 })]
        public ActionResult Call(string ids, UserCenter_LoginUserInfo loginUserInfo)
        {
            string mes = string.Empty;
            try
            {
                List<CAS.Entity.DBEntity.DATProject> projectList = DataCenterProjectApi.GetProjectBuildingHouse(loginUserInfo.NowCityId, ids, loginUserInfo.UserName, loginUserInfo.SignName, loginUserInfo.AppList);
                //log.Info("数据中心楼盘:" + projectList.Count);
                if (projectList != null && projectList.Count > 0)
                {
                    foreach (var item in projectList)
                    {
                        //行政区 + 楼盘名称||已有的projectid||没有入库
                        var exists = _unitOfWork.ProjectRepository.Get(m =>
                            (m.ProjectName == item.projectname ||
                            m.ProjectName == item.areaname + item.projectname ||
                            m.FxtProjectId == item.projectid) && m.Status != SYSCodeManager.STATECODE_10
                            ).FirstOrDefault();
                        if (exists != null)
                        {
                            mes += item.projectname + ",";
                            continue;//任务重复
                        }
                        DateTime now = DateTime.Now;
                        //楼盘
                        Project project = Mapper.Map<CAS.Entity.DBEntity.DATProject, Project>(item);
                        project.FxtProjectId = item.projectid;
                        project.FxtCompanyId = item.fxtcompanyid;
                        project.Status = SYSCodeManager.STATECODE_1;
                        project.ProjectId = 0;
                        project.CreateTime = now;
                        project.X = item.projectx;
                        project.Y = item.projecty;
                        project.Valid = 1;
                        //_unitOfWork.ProjectRepository.Insert(project);
                        project = _unitOfWork.ProjectRepository.Insert(project);
                        _unitOfWork.Commit();
                        //任务
                        var allot = new AllotFlow()
                        {
                            CityId = project.CityID,
                            CreateTime = now,
                            DatType = SYSCodeManager.DATATYPECODE_1,
                            FxtCompanyId = project.FxtCompanyId.Value,
                            UserName = loginUserInfo.UserName,
                            UserTrueName = loginUserInfo.TrueName,
                            StateCode = SYSCodeManager.STATECODE_1,
                            DatId = project.ProjectId,
                            StateDate = now
                        };
                        allot = _unitOfWork.AllotFlowRepository.Insert(allot);
                        //_unitOfWork.Commit();
                        //任务记录
                        var allotSurvey = new AllotSurvey()
                        {
                            AllotId = allot.Id,
                            CityId = allot.CityId,
                            CreateDate = now,
                            StateCode = SYSCodeManager.STATECODE_1,
                            StateDate = now,
                            UserName = loginUserInfo.UserName,
                            TrueName = loginUserInfo.TrueName,
                            Remark = "从调度中心（数据中心） <span class=\"red\">导入任务</span>"
                        };
                        allotSurvey = _unitOfWork.AllotSurveyRepository.Insert(allotSurvey);
                        //停车状况
                        var parkingStatus = _unitOfWork.P_AppendageRepository.Get(m => m.AppendageCode == SYSCodeManager.APPENDAGECODE_14 && m.CityId == project.CityID && m.ProjectId == project.ProjectId).FirstOrDefault();
                        if (parkingStatus != null)
                        {
                            parkingStatus.ClassCode = project.ParkingStatus;
                            _unitOfWork.P_AppendageRepository.Update(parkingStatus);
                        }
                        else
                        {
                            parkingStatus = new PAppendage();
                            parkingStatus.ClassCode = project.ParkingStatus;
                            parkingStatus.AppendageCode = SYSCodeManager.APPENDAGECODE_14;
                            parkingStatus.CityId = project.CityID;
                            parkingStatus.ProjectId = project.ProjectId;
                            parkingStatus.IsInner = true;
                            _unitOfWork.P_AppendageRepository.Insert(parkingStatus);
                        }
                        //开发商
                        if (!string.IsNullOrEmpty(item.devecompanyname))
                        {
                            var developcompany = _unitOfWork.P_CompanyRepository.Get(m => m.CompanyName == item.devecompanyname && m.CompanyType == SYSCodeManager.COMPANYTYPECODE_1
                                && m.ProjectId == project.ProjectId && m.CityId == project.CityID).FirstOrDefault();
                            if (developcompany == null)
                            {
                                developcompany = new PCompany();
                                developcompany.CityId = project.CityID;
                                developcompany.CompanyName = item.devecompanyname;
                                developcompany.CompanyType = SYSCodeManager.COMPANYTYPECODE_1;
                                developcompany.ProjectId = project.ProjectId;
                                _unitOfWork.P_CompanyRepository.Insert(developcompany);
                            }
                        }
                        //物管公司
                        if (!string.IsNullOrEmpty(item.managercompanyname))
                        {
                            var managercompany = _unitOfWork.P_CompanyRepository.Get(m => m.CompanyName == item.managercompanyname && m.CompanyType == SYSCodeManager.COMPANYTYPECODE_4
                                && m.ProjectId == project.ProjectId && m.CityId == project.CityID).FirstOrDefault();
                            if (managercompany == null)
                            {
                                managercompany = new PCompany();
                                managercompany.CityId = project.CityID;
                                managercompany.CompanyName = item.managercompanyname;
                                managercompany.CompanyType = SYSCodeManager.COMPANYTYPECODE_4;
                                managercompany.ProjectId = project.ProjectId;
                                _unitOfWork.P_CompanyRepository.Insert(managercompany);
                            }
                        }

                    }
                    _unitOfWork.Commit();

                }
            }
            catch (Exception ex)
            {
                log.Error(ex);

            }
            if (!string.IsNullOrEmpty(mes))
            {
                //mes += "部分任务导入完成，楼盘：";
                mes += ",不能重复导入。";
            }
            return Json(new { result = 1, message = mes });
        }

        public ActionResult Add(UserCenter_LoginUserInfo loginUserInfo)
        {
            List<FxtApi_SYSArea> areaList = SYSAreaManager.GetAreaByCityId(loginUserInfo.NowCityId, loginUserInfo.UserName, loginUserInfo.SignName, loginUserInfo.AppList);
            ViewBag.AreaList = areaList;
            //主用途
            ViewBag.PurposeCode = SYSCodeManager.PurposeCodeManager();
            //产权形式
            ViewBag.RightCode = SYSCodeManager.RightCodeManager();
            //物业管理质量
            ViewBag.ManagerQuality = SYSCodeManager.LevelManager();
            //停车状况
            ViewBag.ParkingStatusCode = SYSCodeManager.GetAllParkingStatusList();

            ViewBag.CityId = loginUserInfo.NowCityId;
            return View();
        }
        [HttpPost]
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX, NowFunctionPageUrl = WebCommon.Url_DatabaseCall_Index, AndNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_4 })]
        public ActionResult Add(string projectJson, string developersCompany, string managerCompany, UserCenter_LoginUserInfo loginUserInfo)
        {
            AjaxResult result = new AjaxResult("新增任务成功！！！");
            Project project = JsonHelp.ParseJSONjss<Project>(projectJson);
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
