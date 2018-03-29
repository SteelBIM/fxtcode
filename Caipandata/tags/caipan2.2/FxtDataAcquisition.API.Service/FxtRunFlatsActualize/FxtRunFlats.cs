using AutoMapper;
using FxtDataAcquisition.API.Contract.FxtRunFlatsInterface;
using FxtDataAcquisition.Application.Interfaces;
using FxtDataAcquisition.Application.Services;
using FxtDataAcquisition.Common;
using FxtDataAcquisition.Domain.DTO;
using FxtDataAcquisition.Domain.DTO.FxtDataWcfDTO;
using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
using FxtDataAcquisition.Domain.Models;
using FxtDataAcquisition.Framework.Ioc;
using FxtDataAcquisition.FxtAPI.FxtDataCenter.Manager;
using FxtDataAcquisition.FxtAPI.FxtDataWcf.Manager;
using FxtDataAcquisition.FxtAPI.FxtUserCenter.Manager;
using log4net;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace FxtDataAcquisition.API.Service.FxtRunFlatsActualize
{
    /// <summary>
    /// 无纸化住宅物业信息采集系统
    /// </summary>
    public class FxtRunFlats : IFxtRunFlats
    {
        public string UserName
        {
            get;
            set;
        }
        public string SignName
        {
            get;
            set;
        }
        public int FxtCompanyId
        {
            get;
            set;
        }
        public FxtRunFlats()
        {
            //UserName = "admin@fxt";
            //SignName = "4106DEF5-A760-4CD7-A6B2-8250420FCB18";
            //FxtCompanyId = 25;
        }
        public static readonly ILog log = LogManager.GetLogger(typeof(FxtRunFlats));
        IAdminService _unitOfWork = new StandardKernel(new AdminServiceBinder()).Get<IAdminService>();

        #region Dat_AllotSurvey(任务信息)
        /// <summary>
        /// 新增任务
        /// </summary>
        /// <param name="areaid"></param>
        /// <param name="projectname"></param>
        /// <returns></returns>
        public ResultData AddAllot(int cityid, int areaid, string areaname, string username, string usertruename, string projectname)
        {
            ResultData result = new ResultData("新增任务成功！！！");

            Project project = new Project()
            {
                AreaID = areaid,
                ProjectName = projectname
            };
            ProjectAllotFlowSurveyDto projectDao = new ProjectAllotFlowSurveyDto();
            //检查是否已存在任务
            Project exists = _unitOfWork.AllotFlowService.ExistsAllot(project);
            if (exists != null)
            {
                if (exists.Status == SYSCodeManager.STATECODE_1)
                {
                    //检查权限
                    List<int> functionCodes = _unitOfWork.FunctionService.GetAllBy(username, FxtCompanyId, cityid, "/AllotFlowInfo/AllotFlowManager")
                            .Select(m => m.FunctionCode).ToList();
                    if (functionCodes.Contains(SYSCodeManager.FunOperCode_10))
                    {
                        //有权限创建
                        //var ret = _unitOfWork.AllotFlowService.AddAllot(project, "", "", cityid, FxtCompanyId, username, usertruename, SYSCodeManager.STATECODE_4);
                        var p = _unitOfWork.ProjectRepository.GetBy(m => m.ProjectName == project.ProjectName && m.AreaID == project.AreaID);
                        var ret = _unitOfWork.AllotFlowRepository.GetBy(m => m.DatId == p.ProjectId);
                        _unitOfWork.AllotFlowService.SetAllotStatus(username, cityid, new long[] { ret.Id }, SYSCodeManager.STATECODE_1, SYSCodeManager.STATECODE_2, "  从app <span class=\"red\">创建任务</span>", usertruename, username, usertruename);

                        projectDao.ProjectId = exists.ProjectId;
                        var allot = _unitOfWork.AllotFlowRepository.Get(m => m.DatId == exists.ProjectId).FirstOrDefault();
                        projectDao.AllotId = allot.Id;
                    }
                    else
                    {
                        var allot = _unitOfWork.AllotFlowRepository.Get(m => m.DatId == exists.ProjectId).FirstOrDefault();
                        var surveytateDate = _unitOfWork.AllotSurveyRepository.Get(m => m.AllotId == allot.Id).Max(m => m.StateDate);
                        var survey = _unitOfWork.AllotSurveyRepository.Get(m => m.AllotId == allot.Id && m.StateDate == surveytateDate).FirstOrDefault();
                        result.returntext = "新建失败！该项目已在调度中心，处于未分配状态。";
                        result.returntype = 0;
                    }
                }
                else
                {
                    var allot = _unitOfWork.AllotFlowRepository.Get(m => m.DatId == exists.ProjectId).FirstOrDefault();
                    var surveytateDate = _unitOfWork.AllotSurveyRepository.Get(m => m.AllotId == allot.Id).Max(m => m.StateDate);
                    var survey = _unitOfWork.AllotSurveyRepository.Get(m => m.AllotId == allot.Id && m.StateDate == surveytateDate).FirstOrDefault();
                    string msg = "新建失败！该项目已在调度中心，处于";
                    if (survey.StateCode == SYSCodeManager.STATECODE_2)
                    {
                        msg += "已分配状态。于" + survey.StateDate.Value.ToString("yyyy-MM-dd") + "分配给【" + allot.SurveyUserTrueName + "】";
                    }
                    else if (survey.StateCode == SYSCodeManager.STATECODE_4)
                    {
                        msg += "查勘中状态。于" + survey.StateDate.Value.ToString("yyyy-MM-dd") + "查勘人【" + survey.TrueName + "】";
                    }
                    else if (survey.StateCode == SYSCodeManager.STATECODE_5)
                    {
                        msg += "已查勘状态。查勘人【" + survey.TrueName + "】于" + survey.StateDate.Value.ToString("yyyy-MM-dd") + "已完成查勘";
                    }
                    else if (survey.StateCode == SYSCodeManager.STATECODE_6)
                    {
                        msg += "待审批状态。查勘人【" + survey.TrueName + "】于" + survey.StateDate.Value.ToString("yyyy-MM-dd") + "已完成自审";
                    }
                    else if (survey.StateCode == SYSCodeManager.STATECODE_8)
                    {
                        msg += "审批已通过状态。查勘人【" + survey.TrueName + "】于" + survey.StateDate.Value.ToString("yyyy-MM-dd") + "已完成审批";
                    }
                    result.returntext = msg;
                    result.returntype = 0;
                }

            }
            else
            {
                projectDao = _unitOfWork.AllotFlowService.AddAllot(project, "", "", " 从app <span class=\"red\">创建任务</span>", cityid, FxtCompanyId, username, usertruename, SYSCodeManager.STATECODE_2);
                if (projectDao.AllotState > 1)
                {
                    var allot = _unitOfWork.AllotFlowRepository.Get(m => m.DatId == projectDao.ProjectId).FirstOrDefault();
                    var surveytateDate = _unitOfWork.AllotSurveyRepository.Get(m => m.AllotId == allot.Id).Max(m => m.StateDate);
                    var survey = _unitOfWork.AllotSurveyRepository.Get(m => m.AllotId == allot.Id && m.StateDate == surveytateDate).FirstOrDefault();
                    string msg = "新建失败！该项目已在调度中心，处于";
                    if (survey.StateCode == SYSCodeManager.STATECODE_1)
                    {
                        msg += "未分配状态。";
                    }
                    else if (survey.StateCode == SYSCodeManager.STATECODE_2)
                    {
                        msg += "已分配状态。于" + survey.StateDate.Value.ToString("yyyy-MM-dd") + "分配给【" + allot.SurveyUserTrueName + "】";
                    }
                    else if (survey.StateCode == SYSCodeManager.STATECODE_4)
                    {
                        msg += "查勘中状态。于" + survey.StateDate.Value.ToString("yyyy-MM-dd") + "查勘人【" + survey.TrueName + "】";
                    }
                    else if (survey.StateCode == SYSCodeManager.STATECODE_5)
                    {
                        msg += "已查勘状态。查勘人【" + survey.TrueName + "】于" + survey.StateDate.Value.ToString("yyyy-MM-dd") + "已完成查勘";
                    }
                    else if (survey.StateCode == SYSCodeManager.STATECODE_6)
                    {
                        msg += "待审批状态。查勘人【" + survey.TrueName + "】于" + survey.StateDate.Value.ToString("yyyy-MM-dd") + "已完成自审";
                    }
                    else if (survey.StateCode == SYSCodeManager.STATECODE_8)
                    {
                        msg += "审批已通过状态。查勘人【" + survey.TrueName + "】于" + survey.StateDate.Value.ToString("yyyy-MM-dd") + "已完成审批";
                    }
                    result.returntext = msg;
                    result.returntype = 0;
                }
                else if (projectDao.AllotState == 0)
                {
                    result.returntype = 0;
                    result.returntext = "新增任务失败。";
                }
            }

            result.data = new { areaid = areaid, areaname = areaname, projectname = projectname, projectid = projectDao.ProjectId, allotid = projectDao.AllotId }.ToJSONjss();
            return result;
        }

        /// <summary>
        /// 设置新任务为查勘中*
        /// </summary>
        /// <param name="username"></param>
        /// <param name="cityid"></param>
        /// <param name="allotid"></param>
        /// <returns></returns>
        public ResultData SetAllotSurveyingStatus(string username, string usertruename, int cityid, long allotid)
        {
            int result = _unitOfWork.AllotFlowService.SetAllotStatus(username, cityid, new long[] { allotid }
                       , SYSCodeManager.STATECODE_2, SYSCodeManager.STATECODE_4, "<span class=\"red\">开始查勘</span>", userTrueName: usertruename);
            ResultData resultdata = new ResultData(result >= 1 ? 1 : -1, "");
            return resultdata;
        }

        /// <summary>
        /// 从待分配设置新任务为待查勘*
        /// </summary>
        /// <param name="username"></param>
        /// <param name="cityid"></param>
        /// <param name="allotid"></param>
        /// <returns></returns>
        public ResultData SetAllotSurveyingStatusByToDistribution(string username, string usertruename, int cityid, long allotid)
        {
            int result = _unitOfWork.AllotFlowService.SetAllotStatus(username, cityid, new long[] { allotid }
                       , SYSCodeManager.STATECODE_1, SYSCodeManager.STATECODE_2, "<span class=\"red\">从附近搜索楼盘，并分配给自己</span>", usertruename, username, usertruename);
            ResultData resultdata = new ResultData(result >= 1 ? 1 : -1, "");
            return resultdata;
        }
        /// <summary>
        /// 设置撤销待查勘的任务
        /// </summary>
        /// <param name="username"></param>
        /// <param name="companyid"></param>
        /// <param name="cityid"></param>
        /// <param name="allotid"></param>
        /// <returns></returns>
        public ResultData SetCancelToSurvey(string username, string usertruename, int companyid, int cityid, long allotid)
        {
            int result = _unitOfWork.AllotFlowService.SetAllotStatus(username, cityid, new long[] { allotid }
            , SYSCodeManager.STATECODE_2, SYSCodeManager.STATECODE_1, " 从app <span class=\"red\">撤销待查勘</span>", userTrueName: usertruename);
            ResultData resultdata = new ResultData(result >= 1 ? 1 : -1, "");
            return resultdata;
        }

        /// <summary>
        /// 设置撤销查勘中的任务
        /// </summary>
        /// <param name="username"></param>
        /// <param name="companyid"></param>
        /// <param name="cityid"></param>
        /// <param name="allotid"></param>
        /// <returns></returns>
        public ResultData SetCancelSurvey(string username, string usertruename, int companyid, int cityid, long allotid)
        {
            int result = _unitOfWork.AllotFlowService.SetAllotStatus(username, cityid, new long[] { allotid }
            , SYSCodeManager.STATECODE_4, SYSCodeManager.STATECODE_2, " 从app <span class=\"red\">撤销查勘中</span>", userTrueName: usertruename);
            ResultData resultdata = new ResultData(result >= 1 ? 1 : -1, "");
            return resultdata;
        }
        #endregion

        #region DAT_Project(楼盘信息)
        /// <summary>
        /// 提交查勘数据(楼盘,楼栋,房号)
        /// </summary>
        /// <param name="allotid">任务ID</param>
        /// <param name="data">查勘数据</param>
        /// <returns></returns>
        public ResultData SubmitAllotSurveyData(string username, string usertruename, int cityid, int allotid, string data, int isvalid = 0)
        {
            long returnAllotId = allotid;
            int returnProjectId = 0;
            try
            {

                var project = JsonHelp.ParseJSONjss<ProjectDto>(data);
                int result = _unitOfWork.ProjectService.SetAllotProjectInfo(allotid, username, usertruename, FxtCompanyId, cityid, project, isvalid, out returnAllotId, out returnProjectId);
                //log.Info(result);
                ResultData resultData = new ResultData(1, "提交成功");
                if (result == -2)
                {
                    return new ResultData(-2, "不是当前用户的任务");
                }
                else if (result == -3)
                {
                    return new ResultData(-2, "不是查勘中任务");
                }
                else if (result == -4)
                {
                    return new ResultData(-4, "楼盘已存在");
                }
                else if (result == -5)
                {
                    return new ResultData(-2, "任务不存在");
                }
                else if (result < 1)
                {
                    return new ResultData(0, "请填写必填数据");
                }

                resultData.data = new { allotid = returnAllotId, projectid = returnProjectId }.ToJSONjss();
                return resultData;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                ILog log = LogManager.GetLogger(typeof(ProjectService));
                log.Error("实体验证失败：" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 获取查勘任务-楼盘(未查勘)*
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cityId"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="applistjson">用户权限信息</param>
        /// <returns></returns>
        public ResultData GetAllotSurveyProject(string username, int cityid, string signname, string applistjson, int pageindex, int pagesize, string searchname)
        {
            //IAdminService _unitOfWork = new StandardKernel(new AdminServiceBinder()).Get<IAdminService>();
            List<UserCenter_Apps> appList = applistjson.ParseJSONList<UserCenter_Apps>();//用户权限信息

            Expression<Func<Project, bool>> projectFilter = m => m.CityID == cityid && m.Valid == 1;
            Expression<Func<AllotFlow, bool>> allotFlowFilter = m => m.CityId == cityid
                && (m.StateCode == SYSCodeManager.STATECODE_2 || m.StateCode == SYSCodeManager.STATECODE_3)
                && (m.SurveyUserName == username);
            Expression<Func<AllotSurvey, bool>> allotSurveyFilter = null;
            Expression<Func<AllotSurvey, object>> orderby = m => m.StateDate;

            if (!string.IsNullOrEmpty(searchname))
            {
                projectFilter = m => (m.PinYin.Contains(searchname) || m.ProjectName.Contains(searchname)) && m.CityID == cityid;
            }

            int records = 0;
            var data = _unitOfWork.ProjectAllotFlowSurveyService.FindAll(projectFilter, allotFlowFilter, allotSurveyFilter, orderby, pageindex, pagesize, out records).ToList();

            var areaList = DataCenterAreaApi.GetAreaByCityId(cityid, username, signname, appList);
            data.ForEach((m) =>
            {
                var area = areaList.Where(a => a.AreaId == m.AreaID).FirstOrDefault();
                m.AreaName = area == null ? "" : area.AreaName;
                var subAreaList = DataCenterAreaApi.GetSubAreaByAreaId(m.AreaID, username, signname, appList);
                var subArea = subAreaList.Where(a => a.SubAreaId == m.SubAreaId).FirstOrDefault();
                m.SubAreaName = subArea == null ? "" : subArea.SubAreaName;
            });

            var json = JsonHelp.ToJSONjss(data);
            ResultData result = new ResultData(json);
            return result;
        }
        /// <summary>
        /// 获取查勘任务-楼盘(查勘中)*
        /// </summary>
        /// <param name="username"></param>
        /// <param name="cityid"></param>
        /// <returns></returns>
        public ResultData GetAllotSurveyingProject(string username, int cityid, int pageindex = 1, int pagesize = 1000)
        {
            Expression<Func<Project, bool>> projectFilter = m => m.CityID == cityid && m.Valid == 1;
            Expression<Func<AllotFlow, bool>> allotFlowFilter = m => m.CityId == cityid
                && (m.StateCode == SYSCodeManager.STATECODE_4) && (m.SurveyUserName == username);
            Expression<Func<AllotSurvey, bool>> allotSurveyFilter = null;
            Expression<Func<AllotSurvey, object>> orderby = m => m.StateDate;

            int records = 0;
            var data = _unitOfWork.ProjectAllotFlowSurveyService.FindAll(projectFilter, allotFlowFilter, allotSurveyFilter, orderby, pageindex, pagesize, out records);
            var json = JsonHelp.ToJson(data);

            ResultData result = new ResultData(json);
            return result;
        }
        /// <summary>
        /// 获取已查勘列表
        /// </summary>
        /// <param name="username"></param>
        /// <param name="cityid"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="applistjson">用户权限信息</param>
        /// <returns></returns>
        public ResultData GetAllotSurveyOverProject(string username, int cityid, string signname, string applistjson, int pageindex, int pagesize)
        {
            List<UserCenter_Apps> appList = applistjson.ParseJSONList<UserCenter_Apps>();//用户权限信息

            Expression<Func<Project, bool>> projectFilter = m => m.CityID == cityid && m.Valid == 1;
            Expression<Func<AllotFlow, bool>> allotFlowFilter = m => m.CityId == cityid
                && (m.StateCode == SYSCodeManager.STATECODE_5) && (m.SurveyUserName == username);
            Expression<Func<AllotSurvey, bool>> allotSurveyFilter = null;
            Expression<Func<AllotSurvey, object>> orderby = m => m.StateDate;

            int records = 0;
            var data = _unitOfWork.ProjectAllotFlowSurveyService.FindAll(projectFilter, allotFlowFilter, allotSurveyFilter, orderby, pageindex, pagesize, out records).ToList();

            var areaList = DataCenterAreaApi.GetAreaByCityId(cityid, username, signname, appList);
            data.ForEach((m) =>
            {
                var area = areaList.Where(a => a.AreaId == m.AreaID).FirstOrDefault();
                m.AreaName = area == null ? "" : area.AreaName;
                var subAreaList = DataCenterAreaApi.GetSubAreaByAreaId(m.AreaID, username, signname, appList);
                var subArea = subAreaList.Where(a => a.SubAreaId == m.SubAreaId).FirstOrDefault();
                m.SubAreaName = subArea == null ? "" : subArea.SubAreaName;
            });

            var json = JsonHelp.ToJson(data);

            ResultData result = new ResultData(json);
            return result;
        }

        /// <summary>
        /// 根据楼盘ID获取楼盘详细信息*
        /// </summary>
        /// <param name="projectid"></param>
        /// <param name="cityid"></param>
        /// <param name="allotid">楼盘对应的任务ID</param>
        /// <returns></returns>
        public ResultData GetProjectByProjectId(int projectid, int cityid, long allotid, string username, string signname, string applistjson)
        {
            var project = _unitOfWork.ProjectService.GetProjectDetals(projectid, cityid, allotid, FxtCompanyId);

            List<UserCenter_Apps> appList = applistjson.ParseJSONList<UserCenter_Apps>();//用户权限信息
            var area = DataCenterAreaApi.GetAreaByCityId(cityid, username, signname, appList).FirstOrDefault(m => m.AreaId == project.AreaID);
            if (area != null)
            {
                project.AreaName = area.AreaName;
                var subArea = DataCenterAreaApi.GetSubAreaByAreaId(project.AreaID, username, signname, appList).FirstOrDefault(m => m.SubAreaId == project.SubAreaId);
                if (subArea != null)
                {
                    project.SubAreaName = subArea.SubAreaName;
                }
            }

            var json = JsonHelp.ToJSONjss(project);
            ResultData result = new ResultData(json);
            return result;
        }

        /// <summary>
        /// 附近楼盘
        /// </summary>
        /// <param name="cityid"></param>
        /// <param name="areaid"></param>
        /// <returns></returns>
        public ResultData GetAllotSurveyProjectNearby(int areaid)
        {
            var allot = _unitOfWork.AllotFlowRepository.Get(m => m.StateCode == SYSCodeManager.STATECODE_1 && m.Project.AreaID == areaid)
                .Select(m =>
                    new ProjectAllotFlowSurveyDto()
                    {
                        AllotId = m.Id,
                        ProjectId = m.Project.ProjectId,
                        projectName = m.Project.ProjectName,
                        X = m.Project.X,
                        Y = m.Project.Y
                    });
            var json = JsonHelp.ToJson(allot);

            ResultData result = new ResultData(json);

            return result;
        }

        #endregion

        #region DAT_Building(楼栋信息)

        /// <summary>
        /// 根据楼盘ID获取楼栋详细列表*
        /// </summary>
        /// <param name="projectid"></param>
        /// <param name="cityid"></param>
        /// <returns></returns>
        public ResultData GetBuildingDetailedByProjectId(int projectid, int cityid)
        {
            var buildingList = _unitOfWork.BuildingService.GetBuildingDetals(projectid, cityid, FxtCompanyId);
            string json = buildingList.ToJSONjss();
            ResultData result = new ResultData(json);
            return result;
        }

        /// <summary>
        /// 根据楼栋ID获取楼栋信息*
        /// </summary>
        /// <param name="buildingid"></param>
        /// <param name="cityid"></param>
        /// <returns></returns>
        public ResultData GetBuildingByBuildingId(int buildingid, int cityid)
        {
            var building = _unitOfWork.BuildingRepository.GetBy(m => m.BuildingId == buildingid && m.CityID == cityid && m.FxtCompanyId == FxtCompanyId && m.Valid == 1);
            var buildingDto = Mapper.Map<Building, BuildingDto>(building);

            string json = buildingDto.ToJSONjss();
            ResultData result = new ResultData(json);
            return result;
        }

        #endregion

        #region DAT_House(房号信息)
        /// <summary>
        /// 根据楼栋ID获取房号列表*
        /// </summary>
        /// <param name="projectid"></param>
        /// <param name="cityid"></param>
        /// <returns></returns>
        public ResultData GetHouseByBuildingId(int buildingid, int cityid, int floorno)
        {
            var houses = _unitOfWork.HouseService.GetHouseByBuildingIdAndFloorNo(buildingid, cityid, floorno);
            string json = houses.ToJSONjss();
            ResultData result = new ResultData(json);
            return result;
        }

        #endregion

        #region SYS_Code (基础数据)
        /// <summary>
        /// 获取配套类型CODE*
        /// </summary>
        /// <returns></returns>
        public ResultData GetAppendageCodeList()
        {
            IList<SYSCode> list = SYSCodeManager.GetAppendageCodeList();
            string json = list.ToJson();
            ResultData result = new ResultData(json);
            return result;

        }
        /// <summary>
        /// 获取建筑结构CODE*
        /// </summary>
        /// <returns></returns>
        public ResultData GetStructureCodeList()
        {
            IList<SYSCode> list = SYSCodeManager.GetStructureCodeList();
            string json = list.ToJson();
            ResultData result = new ResultData(json);
            return result;

        }
        /// <summary>
        /// 获取楼栋位置CODE选项*
        /// </summary>
        /// <returns></returns>
        public ResultData GetBuildingLocationCodeList()
        {
            IList<SYSCode> list = SYSCodeManager.GetBuildingLocationCodeList();
            string json = list.ToJson();
            ResultData result = new ResultData(json);
            return result;
        }
        /// <summary>
        /// 获取朝向CODE选项*
        /// </summary>
        /// <returns></returns>
        public ResultData GetFrontCodeList()
        {
            IList<SYSCode> list = SYSCodeManager.GetFrontCodeList();
            string json = list.ToJson();
            ResultData result = new ResultData(json);
            return result;
        }
        /// <summary>
        /// 获取户型CODE选项*
        /// </summary>
        /// <returns></returns>
        public ResultData GetHouseTypeCodeList()
        {
            IList<SYSCode> list = SYSCodeManager.GetHouseTypeCodeList();
            string json = list.ToJson();
            ResultData result = new ResultData(json);
            return result;
        }
        /// <summary>
        /// 获取等级Code选项*
        /// </summary>
        /// <returns></returns>
        public ResultData GetClassCodeList()
        {
            IList<SYSCode> list = SYSCodeManager.GetClassCodeList();
            string json = list.ToJson();
            ResultData result = new ResultData(json);
            return result;
        }

        /// <summary>
        /// 获取用途(楼盘、楼栋)Code选项*
        /// </summary>
        /// <returns></returns>
        public ResultData GetPurposeCodeList()
        {
            IList<SYSCode> list = SYSCodeManager.PurposeCodeManager();
            string json = list.ToJson();
            ResultData result = new ResultData(json);
            return result;
        }

        /// <summary>
        /// 获取产权形式Code选项*
        /// </summary>
        /// <returns></returns>
        public ResultData GetRightCodeList()
        {
            IList<SYSCode> list = SYSCodeManager.RightCodeManager();
            string json = list.ToJson();
            ResultData result = new ResultData(json);
            return result;
        }

        /// <summary>
        /// 获取景观Code选项*
        /// </summary>
        /// <returns></returns>
        public ResultData GetSightCodeList()
        {
            IList<SYSCode> list = SYSCodeManager.GetSightCodeList();
            string json = list.ToJson();
            ResultData result = new ResultData(json);
            return result;
        }

        /// <summary>
        /// 获取噪音Code选项*
        /// </summary>
        /// <returns></returns>
        public ResultData GetNoiseCodeList()
        {
            IList<SYSCode> list = SYSCodeManager.NoiseManager();
            string json = list.ToJson();
            ResultData result = new ResultData(json);
            return result;
        }

        /// <summary>
        /// 获取房号用途Code选项*
        /// </summary>
        /// <returns></returns>
        public ResultData GetHousePurposeCodeList()
        {
            IList<SYSCode> list = SYSCodeManager.HousePurposeCodeManager();
            string json = list.ToJson();
            ResultData result = new ResultData(json);
            return result;
        }

        /// <summary>
        /// 获取户型结构Code选项*
        /// </summary>
        /// <returns></returns>
        public ResultData GetHouseStructureCodeList()
        {
            IList<SYSCode> list = SYSCodeManager.StructureCodeManager();
            string json = list.ToJson();
            ResultData result = new ResultData(json);
            return result;
        }

        /// <summary>
        /// 获取通风采光Code选项*
        /// </summary>
        /// <returns></returns>
        public ResultData GetVDCodeeList()
        {
            IList<SYSCode> list = SYSCodeManager.VDCodeManager();
            string json = list.ToJson();
            ResultData result = new ResultData(json);
            return result;
        }

        #endregion

        #region SYS_City(城市)
        /// <summary>
        /// 获取城市信息*
        /// </summary>
        /// <param name="cityname"></param>
        /// <returns></returns>
        public ResultData GetCityByName(string cityname)
        {
            FxtApi_SYSCity syscity = SYSCityApi.GetCityByCityName(cityname);

            string json = syscity.ToJson();
            ResultData result = new ResultData(json);
            return result;
        }

        /// <summary>
        /// 获取当前公司开通产品城市（省份）列表
        /// </summary>
        /// <param name="applistjson">用户权限信息</param>
        /// <returns></returns>
        public ResultData GetProvinceCityList(string username, string signname, string applistjson)
        {

            List<UserCenter_Apps> appList = applistjson.ParseJSONList<UserCenter_Apps>();//用户权限信息
            List<FxtApi_SYSProvince> provinceList = _unitOfWork.CityService.GetProvinceCityListBy(username, signname, appList);
            ResultData result = new ResultData(provinceList.ToJSONjss());
            return result;
        }

        #endregion

        #region SYS_Area(行政区)
        /// <summary>
        /// 根据城市ID获取行政区*
        /// </summary>
        /// <param name="cityid"></param>
        /// <param name="applistjson">当前用户拥有的api信息</param>
        /// <returns></returns>
        public ResultData GetAreaByCityId(int cityid, string applistjson)
        {
            List<UserCenter_Apps> appList = applistjson.ParseJSONList<UserCenter_Apps>();
            IList<FxtApi_SYSArea> list = DataCenterAreaApi.GetAreaByCityId(cityid, UserName, SignName, appList);
            string json = list.ToJson();
            ResultData result = new ResultData(json);
            return result;
        }
        /// <summary>
        /// 根据ID获取行政区信息
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public ResultData GetAreaByAreaId(int areaid)
        {
            FxtApi_SYSArea obj = SYSAreaApi.GetAreaByAreaId(areaid);
            string json = obj.ToJson();
            ResultData result = new ResultData(json);
            return result;
        }
        /// <summary>
        /// 根据多个ID获取多个行政区信息
        /// </summary>
        /// <param name="areaIds"></param>
        /// <returns></returns>
        public ResultData GetAreaByAreaIds(string areaids)
        {
            int[] ids = areaids.ConvertToInts(',');
            List<FxtApi_SYSArea> obj = SYSAreaApi.GetAreaByAreaIds(ids);
            string json = obj.ToJson();
            ResultData result = new ResultData(json);
            return result;
        }
        #endregion

        #region 获取片区
        /// <summary>
        /// 根据行政区ID获取所有片区信息
        /// </summary>
        /// <param name="areaid"></param>
        /// <param name="applistjson"></param>
        /// <returns></returns>
        public ResultData GetSubAreaByAreaId(int areaid, string applistjson)
        {
            List<UserCenter_Apps> appList = applistjson.ParseJSONList<UserCenter_Apps>();
            IList<FxtApi_SYSSubArea> list = DataCenterAreaApi.GetSubAreaByAreaId(areaid, UserName, SignName, appList);
            string json = list.ToJson();
            ResultData result = new ResultData(json);
            return result;
        }

        #endregion

        #region SYS_Role_User(用户角色)

        /// <summary>
        /// 获取用户权限
        /// </summary>
        /// <param name="username"></param>
        /// <param name="companyid"></param>
        /// <param name="cityid"></param>
        /// <returns></returns>
        public ResultData GetFunction(string username, int companyid, int cityid)
        {
            var functions = _unitOfWork.FunctionService.GetAllBy(username, companyid, cityid, "/AllotFlowInfo/AllotFlowManager");
            ResultData result = new ResultData(functions.Select(m => m.FunctionCode).ToJson());
            return result;
        }
        #endregion

        #region LNK_P_Photo(楼盘照片)
        /// <summary>
        /// 根据楼盘ID获取楼照片信息
        /// </summary>
        /// <param name="projectid"></param>
        /// <param name="cityid"></param>
        /// <returns></returns>
        public ResultData GetLNKPPhotoByProjectId(int projectid, int buildingid, int cityid)
        {
            IQueryable<PPhoto> data = null;
            if (buildingid > 0)
            {
                data = _unitOfWork.P_PhotoRepository.Get(m => m.ProjectId == projectid && m.CityId == cityid
                        && m.FxtCompanyId == FxtCompanyId && m.BuildingId == buildingid);
            }
            else
            {
                data = _unitOfWork.P_PhotoRepository.Get(m => m.ProjectId == projectid && m.CityId == cityid
                        && m.FxtCompanyId == FxtCompanyId && (m.BuildingId == 0 || !m.BuildingId.HasValue));
            }

            var json = JsonHelp.ToJson(data);
            ResultData result = new ResultData(json);
            return result;
        }
        #endregion

        #region (基础数据整合)
        /// <summary>
        /// 获取所有基础数据*
        /// </summary>
        /// <returns></returns>
        public ResultData GetBaseData()
        {
            //配套类型
            int id = 2008;
            IList<SYSCode> appendageCodeList = SYSCodeManager.GetAppendageCodeList();
            //appendageCodeList.Add(new SYSCode { ID = id, Code = 2008005, CodeName = "银行", CodeType = "配套", Remark = "1", SubCode = null });
            //appendageCodeList.Add(new SYSCode { ID = id, Code = 2008006, CodeName = "学校", CodeType = "配套", Remark = "2", SubCode = null });
            //appendageCodeList.Add(new SYSCode { ID = id, Code = 2008007, CodeName = "医院", CodeType = "配套", Remark = "3", SubCode = null });


            //建筑结构
            int id2 = 2010;
            IList<SYSCode> structureCodeList = SYSCodeManager.GetStructureCodeList();
            //structureCodeList.Add(new SYSCode { ID = id2, Code = 2010007, CodeName = "钢混结构", CodeType = "建筑结构", Remark = "1", SubCode = null });
            //structureCodeList.Add(new SYSCode { ID = id2, Code = 2010008, CodeName = "混合结构", CodeType = "建筑结构", Remark = "2", SubCode = null });
            //structureCodeList.Add(new SYSCode { ID = id2, Code = 2010009, CodeName = "内浇外砌", CodeType = "建筑结构", Remark = "3", SubCode = null });

            //楼栋位置
            int id3 = 2011;
            IList<SYSCode> buildingLocationCodeList = SYSCodeManager.GetBuildingLocationCodeList();
            //buildingLocationCodeList.Add(new SYSCode { ID = id3, Code = 2011001, CodeName = "无特别因素", CodeType = "楼栋位置", Remark = "1", SubCode = null });
            //buildingLocationCodeList.Add(new SYSCode { ID = id3, Code = 2011002, CodeName = "临公园、绿地", CodeType = "楼栋位置", Remark = "2", SubCode = null });
            //buildingLocationCodeList.Add(new SYSCode { ID = id3, Code = 2011003, CodeName = "临江、河、湖", CodeType = "楼栋位置", Remark = "3", SubCode = null });


            //朝向
            int id4 = 2004;
            IList<SYSCode> frontCodeList = SYSCodeManager.GetFrontCodeList();
            //frontCodeList.Add(new SYSCode { ID = id4, Code = 2004001, CodeName = "东", CodeType = "朝向", Remark = "1", SubCode = null });
            //frontCodeList.Add(new SYSCode { ID = id4, Code = 2004002, CodeName = "南", CodeType = "朝向", Remark = "2", SubCode = null });
            //frontCodeList.Add(new SYSCode { ID = id4, Code = 2004003, CodeName = "西", CodeType = "朝向", Remark = "3", SubCode = null });


            //户型
            int id5 = 4001;
            IList<SYSCode> houseTypeCodeList = SYSCodeManager.GetHouseTypeCodeList();// new List<SYSCode>();
            //houseTypeCodeList.Add(new SYSCode { ID = id5, Code = 4001003, CodeName = "一室一厅", CodeType = "户型", Remark = "1", SubCode = null });
            //houseTypeCodeList.Add(new SYSCode { ID = id5, Code = 4001004, CodeName = "两室一厅", CodeType = "户型", Remark = "2", SubCode = null });
            //houseTypeCodeList.Add(new SYSCode { ID = id5, Code = 4001005, CodeName = "两室两厅", CodeType = "户型", Remark = "3", SubCode = null });


            //等级
            int id6 = 1012;
            IList<SYSCode> classCodeList = SYSCodeManager.GetClassCodeList();// new List<SYSCode>();
            //classCodeList.Add(new SYSCode { ID = id6, Code = 1012001, CodeName = "优", CodeType = "等级", Remark = "1", SubCode = null });
            //classCodeList.Add(new SYSCode { ID = id6, Code = 1012002, CodeName = "良", CodeType = "等级", Remark = "2", SubCode = null });
            //classCodeList.Add(new SYSCode { ID = id6, Code = 1012003, CodeName = "一般", CodeType = "等级", Remark = "3", SubCode = null });


            //景观
            int id7 = 2006;
            IList<SYSCode> sightCodeList = SYSCodeManager.GetSightCodeList();// new List<SYSCode>();
            //sightCodeList.Add(new SYSCode { ID = id7, Code = 2006007, CodeName = "山景", CodeType = "景观", Remark = "7", SubCode = null });
            //sightCodeList.Add(new SYSCode { ID = id7, Code = 2006008, CodeName = "江景", CodeType = "景观", Remark = "8", SubCode = null });
            //sightCodeList.Add(new SYSCode { ID = id7, Code = 2006009, CodeName = "湖景", CodeType = "景观", Remark = "9", SubCode = null });

            //照片类型
            IList<SYSCode> photoTypeCodeList = SYSCodeManager.GetPhotoTypeCodeList();

            //省份
            List<object> provinceList = new List<object>();
            List<FxtApi_SYSProvince> provinceList2 = SYSProvinceApi.GetAllProvince();
            foreach (FxtApi_SYSProvince obj in provinceList2)
            {
                var province = new
                {
                    provinceid = obj.ProvinceId,
                    provincename = obj.ProvinceName
                };
                provinceList.Add(province);
            }
            //var sysprovince = new
            //{
            //    provinceid = 5,
            //    provincename = "广东省"
            //};
            //var sysprovince2 = new
            //{
            //    provinceid = 12,
            //    provincename = "湖南省"
            //};     
            //DataTable tbl1 = new DataTable();//大表
            //DataTable tbl2 = new DataTable();//小表
            //tbl2.Copy();
            //foreach (DataRow row in tbl2.Rows)
            //{
            //    DataRow[] _row = tbl1.Select("ID=" + Convert.ToInt32(row["ID"])).FirstOrDefault;
            //    if (_row != null)
            //    {
            //        tbl1.Rows.Remove(_row);
            //        //相应的逻辑操作
            //    }
            //}
            //城市
            List<object> cityList = new List<object>();
            List<FxtApi_SYSCity> cityList2 = SYSCityApi.GetAllCity();
            foreach (FxtApi_SYSCity obj in cityList2)
            {
                var city = new
                {
                    cityid = obj.CityId,
                    cityname = obj.CityName,
                    provinceid = obj.ProvinceId
                };
                cityList.Add(city);
            }
            //var syscity = new
            //{
            //    cityid = 6,
            //    cityname = "深圳市",
            //    provinceid = 5
            //};
            //var syscity2 = new
            //{
            //    cityid = 7,
            //    cityname = "广州市",
            //    provinceid = 5
            //};

            //DataBase db = new DataBase();
            //IList<LNKPCompany> testlist = db.DB.GetListCustom<LNKPCompany>(
            //   (Expression<Func<LNKPCompany, bool>>)
            //   (tbl =>
            //       tbl.LNKPCompanyPX.ProjectId > 0
            //   )).ToList<LNKPCompany>();
            //db.Close();

            var resultJson = new
            {
                appendagecode = appendageCodeList,//配套类型
                structurecode = structureCodeList,//建筑类型
                buildinglocationcode = buildingLocationCodeList,//楼栋位置
                frontcode = frontCodeList,//朝向
                housetypecode = houseTypeCodeList,//户型
                classcode = classCodeList,//code等级
                sightcode = sightCodeList,//景观
                phototypecode = photoTypeCodeList,//照片类型
                province = provinceList,//省份
                city = cityList//,//城市
            };
            string json = resultJson.ToJson();
            ResultData result = new ResultData(json);
            return result;
        }

        public ResultData GetUserIcon(string username)
        {
            var url = string.Empty;
            var userInfo = _unitOfWork.SysUserInfoRepository.GetById(username);
            if (userInfo != null)
            {
                //string path = ConfigurationManager.AppSettings["OssDownload"];
                url = userInfo.IconUrl;
            }
            return new ResultData { data = url, returntext = "", returntype = 1 };

        }
        #endregion

        #region 意见反馈

        /// <summary>
        /// 提交意见反馈
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="content">反馈内容</param>
        /// <returns></returns>
        public ResultData SetFeedback(string username, string content)
        {
            var feedback = _unitOfWork.SysFeedbackRepository.Insert(
                new Feedback()
                {
                    AddTime = DateTime.Now,
                    Content = content,
                    UserName = username
                });
            var result = _unitOfWork.Commit();
            if (result > 0)
            {
                return new ResultData { data = feedback.Id.ToString(), returntext = "", returntype = 1 };
            }
            else
            {
                return new ResultData { data = "", returntext = "", returntype = 0 };
            }
        }

        /// <summary>
        /// 获取意见反馈列表
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="content">反馈内容</param>
        /// <returns></returns>
        public ResultData GetFeedbackList(string username)
        {
            var feedbacks = _unitOfWork.SysFeedbackRepository.Get().OrderByDescending(m => m.AddTime).ToList();
            var fdto = Mapper.Map<List<Feedback>, List<FeedbackDto>>(feedbacks);
            var result = _unitOfWork.Commit();

            return new ResultData { data = fdto.ToJson(), returntext = "", returntype = 0 };
        }
        #endregion

        #region 图片上传
        /// <summary>
        /// 获取已经上传文件大小
        /// </summary>
        /// <param name="allotId"></param>
        /// <param name="projectid"></param>
        /// <param name="cityid"></param>
        /// <param name="typecode"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public ResultData GetLNKPPhotoSeries(long allotid, int projectid, int cityid, int typecode, string filename)
        {
            filename = filename.DecodeField();
            long dataLeng = 0;
            //获取断点续传临时文件根目录
            string basePath = CommonUtility.GetConfigSetting("upload_DataAcquisition_Temporary");
            string returnFileName = filename;
            //获取组织好的断点续传的文件路径和文件名
            string searchPath = _unitOfWork.PhotoService.GetProjectPhotoPathTemporary(basePath, allotid, FxtCompanyId, projectid, cityid, typecode, filename, out returnFileName);
            if (!string.IsNullOrEmpty(searchPath))
            {
                string folder = System.Web.Hosting.HostingEnvironment.MapPath(searchPath);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string path = Path.Combine(folder, returnFileName);
                if (File.Exists(path))
                {
                    FileInfo file = new FileInfo(path);
                    dataLeng = file.Length;
                }
            }
            return new ResultData { returntype = 1, data = dataLeng.ToString(), returntext = "" };
        }

        private static void EnsurePathExists(string dir)
        {
            // Set to folder path we must ensure exists.
            try
            {
                // If the directory doesn't exist, create it.
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
            }
            catch (Exception)
            {
                // Fail silently
            }
        }

        /// <summary>
        /// 进行断点续传
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="allotId"></param>
        /// <param name="projectid"></param>
        /// <param name="cityid"></param>
        /// <param name="typeCode"></param>
        /// <param name="filename"></param>
        /// <param name="npos"></param>
        /// <returns></returns>
        public ResultData UpLoadPhotoSeries(System.IO.Stream stream, long allotid, int projectid, int cityid, int typecode, string filename, long npos)
        {
            filename.DecodeField();
            //获取断点续传临时文件根目录
            string basePath = CommonUtility.GetConfigSetting("upload_DataAcquisition_Temporary");
            string returnFileName = filename;
            string searchPath = _unitOfWork.PhotoService.GetProjectPhotoPathTemporary(basePath, allotid, FxtCompanyId, projectid, cityid, typecode, filename, out returnFileName);
            string folder = System.Web.Hosting.HostingEnvironment.MapPath(searchPath);
            //无此文件夹时创建文件夹
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            string path = Path.Combine(folder, returnFileName);
            long nowFileSize = npos;
            //断点保存文件
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                //偏移指针
                fs.Seek(npos, SeekOrigin.Begin);
                long ByteLength = WebOperationContext.Current.IncomingRequest.ContentLength;
                byte[] fileContent = new byte[ByteLength];
                //获取上传成功后当前文件大小
                nowFileSize = npos + fileContent.Length;
                stream.Read(fileContent, 0, fileContent.Length);
                fs.Write(fileContent, 0, fileContent.Length);
                fs.Flush();
            }
            return new ResultData { data = nowFileSize.ToString(), returntext = "", returntype = 1 };
        }
        /// <summary>
        /// 设置断点续传成功
        /// </summary>
        /// <param name="allotid"></param>
        /// <param name="projectid"></param>
        /// <param name="cityid"></param>
        /// <returns></returns>
        public async Task<ResultData> SetUpLoadPhotoOver(long allotid, int projectid, int cityid, string username, string usertruename, int state)
        {
            try
            {
                int photosCount = 0;
                //获取断点续传临时文件根目录
                string basePath1 = CommonUtility.GetConfigSetting("upload_DataAcquisition_Temporary");
                //获取正式数据文件根目录
                string basePath2 = CommonUtility.GetConfigSetting("upload_DataAcquisition");
                //获取断点续传临时文件当前图片目录
                string fileName = "";
                string searchPath1 = _unitOfWork.PhotoService.GetProjectPhotoPathTemporary(basePath1, allotid, FxtCompanyId, projectid, cityid, 0, fileName, out fileName);
                string folder1 = System.Web.Hosting.HostingEnvironment.MapPath(searchPath1);
                //临时目录存在图片
                if (Directory.Exists(folder1))
                {
                    //获取或创建正式数据目录
                    string searchPath2 = _unitOfWork.PhotoService.GetProjectPhotoPath(basePath2, FxtCompanyId, projectid, cityid);
                    string folder2 = System.Web.Hosting.HostingEnvironment.MapPath(searchPath2);
                    if (!Directory.Exists(folder2))
                    {
                        Directory.CreateDirectory(folder2);
                    }
                    DirectoryInfo di = new DirectoryInfo(folder1);
                    FileInfo[] files = di.GetFiles();
                    //解压
                    if (files.Length > 0)
                    {
                        var zipPath = files[0].FullName;
                        ZipHelper.UnZipFile(folder1, files[0].FullName);
                        //删除
                        File.Delete(zipPath);

                        DirectoryInfo di2 = new DirectoryInfo(Path.Combine(folder1, projectid.ToString()));
                        //遍历临时目录
                        var photos = await GetAllTemporary(di2, folder2, searchPath2, projectid, 0, cityid, _unitOfWork);

                        //将信息插入数据库
                        if (photos != null && photos.Count > 0)
                        {
                            //if (!_unitOfWork.PhotoService.Delete(projectid, 0, cityid, FxtCompanyId))
                            //{
                            //    return new ResultData { data = "0", returntext = "删除原始图片失败", returntype = 0 };
                            //}
                            foreach (var item in photos)
                            {
                                _unitOfWork.P_PhotoRepository.Insert(item);
                            }
                            //if (LNKPPhotoManager.Insert(photos))
                            //{
                            //删除断点续传缓存文件
                            Directory.Delete(folder1, true);
                            photosCount = photos.Count;
                            //}
                            //return new ResultData { data = "0", returntext = "将信息插入数据库失败", returntype = 0 };
                        }
                    }

                }

                string remark = string.Empty;
                int stateCode = SYSCodeManager.STATECODE_5;
                if (state == 1)//已查勘
                {
                    stateCode = SYSCodeManager.STATECODE_5;
                    remark = "<span class=\"red\">完成查勘</span>";
                }
                else if (state == 2)//待查勘
                {
                    stateCode = SYSCodeManager.STATECODE_2;
                    remark = " 从app <span class=\"red\">撤销查勘</span>";
                }
                DateTime now = DateTime.Now;
                var project = _unitOfWork.ProjectRepository.Get(m => m.ProjectId == projectid && m.CityID == cityid && m.FxtCompanyId == FxtCompanyId && m.Valid == 1).FirstOrDefault();
                project.StartDate = now;
                project.Status = stateCode;
                _unitOfWork.ProjectRepository.Update(project);
                var allot = _unitOfWork.AllotFlowRepository.Get(m => m.DatId == projectid && m.CityId == cityid && m.FxtCompanyId == FxtCompanyId).FirstOrDefault();
                allot.StateDate = now;
                allot.StateCode = stateCode;
                _unitOfWork.AllotFlowRepository.Update(allot);
                _unitOfWork.AllotSurveyRepository.Insert(
                    new AllotSurvey()
                    {
                        StateCode = stateCode,
                        StateDate = now,
                        CreateDate = now,
                        CityId = cityid,
                        FxtCompanyId = FxtCompanyId,
                        AllotId = allot.Id,
                        UserName = username,
                        TrueName = usertruename,
                        Remark = remark
                    }
                    );

                _unitOfWork.Commit();

                return new ResultData { data = photosCount.ToString(), returntext = "", returntype = 1 };

            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private async Task<List<PPhoto>> GetAllTemporary(DirectoryInfo dir, string folder2, string searchPath2, int projectid, int buildingid, int cityid, IAdminService _unitOfWork)//递归临时文件
        {
            List<PPhoto> photos = new List<PPhoto>();
            FileInfo[] allFile = dir.GetFiles();
            //if (allFile.Length == 0)
            //{
            if (buildingid != 0)
            {
                //删除楼栋图片
                _unitOfWork.P_PhotoRepository.Delete(m => m.ProjectId == projectid && m.BuildingId == buildingid && m.CityId == cityid && m.FxtCompanyId == FxtCompanyId);
            }
            else
            {
                if (allFile.Length > 0)
                {
                    //删除楼盘图片
                    _unitOfWork.P_PhotoRepository.Delete(m => m.ProjectId == projectid && (!m.BuildingId.HasValue || m.BuildingId == 0) && m.CityId == cityid && m.FxtCompanyId == FxtCompanyId);
                }
                else
                {
                    var project = _unitOfWork.ProjectRepository.GetById(projectid);
                    if (!project.PhotoCount.HasValue || project.PhotoCount < 1)
                    {
                        //删除楼盘图片
                        _unitOfWork.P_PhotoRepository.Delete(m => m.ProjectId == projectid && (!m.BuildingId.HasValue || m.BuildingId == 0) && m.CityId == cityid && m.FxtCompanyId == FxtCompanyId);
                    }
                }
            }
            //}
            //else {
            foreach (FileInfo fileInfo in allFile)
            {
                string _fileName = fileInfo.Name;//获取临时文件名
                string _fileName2 = CommonUtility.GetRndString(20) + "_" + _fileName;
                string path = Path.Combine(folder2, _fileName2);//组织正式文件路径
                string path2 = searchPath2 + "/" + _fileName2;//组织正式文件虚拟路径
                //File.Copy(fileInfo.FullName, path);//copy文件到正式文件夹
                //上传图片到oss
                var result = await OssHelp.UpFileAsync(fileInfo.FullName, path2);

                //照片信息插入实体
                int typeCode = _unitOfWork.PhotoService.GetProjectPhotoInfoByFileName(_fileName);
                PPhoto obj = new PPhoto()
                {
                    ProjectId = projectid,
                    BuildingId = buildingid,
                    CityId = cityid,
                    FxtCompanyId = FxtCompanyId,
                    PhotoTypeCode = typeCode,
                    Path = path2,
                    PhotoName = fileInfo.Name.Replace(System.IO.Path.GetExtension(fileInfo.Name), ""),
                    PhotoDate = DateTime.Now,
                    Valid = 1
                };
                photos.Add(obj);
            }
            //}
            DirectoryInfo[] allDir = dir.GetDirectories();
            foreach (DirectoryInfo d in allDir)
            {
                var folder = Path.Combine(folder2, d.Name);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                var searchPath = searchPath2 + "/" + d.Name;
                string[] buildingIds = d.Name.Split('_');
                Guid bid = new Guid(buildingIds[0]);
                var building = _unitOfWork.BuildingRepository.Get(m => m.AppId == bid && m.ProjectId == projectid
                               && m.CityID == cityid && m.FxtCompanyId == FxtCompanyId).FirstOrDefault();
                if (building != null)
                {
                    var ps = await GetAllTemporary(d, folder, searchPath, projectid, building.BuildingId, cityid, _unitOfWork);
                    photos = photos.Concat(ps).ToList();
                }
            }
            return photos;
        }


        public async Task<ResultData> UpLoadUserIcon(Stream stream, string username, string filename)
        {
            //获取文件根目录
            string basePath = CommonUtility.GetConfigSetting("upload_DataAcquisition_UserPhoto");
            //string folder  = ConfigurationManager.AppSettings["OssDownload"] + basePath + "/" + username;
            //string folder = System.Web.Hosting.HostingEnvironment.MapPath(basePath + "/" + username);
            //无此文件夹时创建文件夹
            //if (!Directory.Exists(folder))
            //{
            //    Directory.CreateDirectory(folder);
            //}
            string fname = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + filename;
            //folder = folder + "\\" + fname;
            //basePath = basePath + "/" + username + "/" + fname;
            basePath = basePath + "/" + fname;
            //using (StreamWriter sw = new StreamWriter(folder))
            //{
            //    stream.CopyTo(sw.BaseStream);
            //}

            //上传图片到oss
            var result = await OssHelp.UpFileAsync(stream, basePath);

            var user = _unitOfWork.SysUserInfoRepository.GetById(username);
            if (user != null)
            {
                user.IconUrl = basePath;
                user.UpdateTime = DateTime.Now;
                user.UpdateUser = username;
                user.Valid = 1;
                _unitOfWork.SysUserInfoRepository.Update(user);
            }
            else
            {
                _unitOfWork.SysUserInfoRepository.Insert(
                    new SYS_UserInfo()
                    {
                        UserName = username,
                        IconUrl = basePath,
                        UpdateTime = DateTime.Now,
                        UpdateUser = username,
                        Valid = 1
                    });
            }
            _unitOfWork.Commit();
            return new ResultData { data = basePath, returntext = "", returntype = 1 };
        }

        /// <summary>
        /// 设置清理断点续传数据
        /// </summary>
        /// <param name="allotid"></param>
        /// <param name="projectid"></param>
        /// <param name="cityid"></param>
        /// <returns></returns>
        public ResultData SetUpLoadPhotoDelete(long allotid, int projectid, int cityid)
        {
            //获取断点续传临时文件根目录
            string basePath1 = CommonUtility.GetConfigSetting("upload_DataAcquisition_Temporary");
            //获取断点续传临时文件当前图片目录
            string fileName = "";
            string searchPath1 = _unitOfWork.PhotoService.GetProjectPhotoPathTemporary(basePath1, allotid, FxtCompanyId, projectid, cityid, 0, fileName, out fileName);
            string folder1 = System.Web.Hosting.HostingEnvironment.MapPath(searchPath1);
            //临时目录存在图片
            if (Directory.Exists(folder1))
            {
                //删除断点续传缓存文件
                Directory.Delete(folder1, true);
            }
            return new ResultData { data = "", returntext = "", returntype = 1 };
        }
        #endregion

    }
}
