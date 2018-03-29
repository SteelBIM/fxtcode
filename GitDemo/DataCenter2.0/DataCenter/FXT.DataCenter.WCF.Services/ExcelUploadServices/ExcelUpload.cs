using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using System.Web;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Redis;
using FXT.DataCenter.WCF.Contract;
using System.Configuration;
using FXT.DataCenter.Infrastructure.Common.Common;
using System.Net;


namespace FXT.DataCenter.WCF.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public partial class ExcelUpload : IExcelUpload
    {
        readonly int _productTypeCode = ((int)EnumHelper.Codes.SysTypeCodeDataCenter);
        Exception _msg;

        private readonly IImportTask _importTask;
        private readonly IDropDownList _dropDownList;
        private readonly IProjectSample _projectSample;
        private readonly IDAT_Project _datProject;
        private readonly IDAT_Building _datBuilding;
        private readonly IDAT_House _datHouse;
        private readonly IDAT_Land _datLand;
        private readonly ILandCase _landCase;
        private readonly IDAT_Land_BasePrice _datLandPrice;
        private readonly IBusinessStreet _businessStreet;
        private readonly IDat_Building_Biz _businessBuilding;
        private readonly IBusinessCase _businessCase;
        private readonly IDynamicPriceSurvey _dynamicPriceSurvey;
        private readonly IDat_Floor_Biz _datFloorBiz;
        private readonly IDat_House_Biz _datHouseBiz;
        private readonly ICompany _company;
        private readonly IDAT_Company _datCompany;
        private readonly IBusinessStore _businessStore;
        private readonly IProjectCaseTask _projectCaseTask;
        private readonly IProjectCase _projectCase;
        private readonly IOfficeSubArea _officeSubArea;
        private readonly IOfficeProject _officeProject;
        private readonly IOfficeBuilding _officeBuilding;
        private readonly IOfficePeiTao _officePeiTao;
        private readonly IOfficeTenant _officeTenant;
        private readonly IOfficeCase _officeCase;
        private readonly IOfficeHouse _officeHouse;
        private readonly IOfficeDynamicPrice _officeDynamicPrice;
        private readonly IIndustrySubArea _industrySubArea;
        private readonly IIndustryProject _industryProject;
        private readonly IIndustryBuilding _industryBuilding;
        private readonly IIndustryPeiTao _industryPeiTao;
        private readonly IIndustryTenant _industryTenant;
        private readonly IIndustryCase _industryCase;
        private readonly IIndustryHouse _industryHouse;
        private readonly IIndustryDynamicPrice _industryDynamicPrice;
        private readonly IWaitBuildingProject _waitProject;
        private readonly IHumanProject _humanProject;
        private readonly IHumanHouse _humanHouse;
        private readonly ICodePrice _codePrice;
        private readonly IFloorPrice _floorPrice;
        private readonly IProjectOtherName _projectOtherName;
        private readonly IPropertyAddress _propertyAddress;

        public ExcelUpload(
            IImportTask importTask,
            IDropDownList dropDownList,
            IProjectSample projectSample,
            IDAT_Project datProject,
            IDAT_Building datBuilding,
            IDAT_House datHouse,
            IDAT_Land datLand,
            ILandCase landCase,
            IDAT_Land_BasePrice datLandPrice,
            IBusinessStreet businessStreet,
            IDat_Building_Biz businessBuilding,
            IBusinessCase businessCase,
            IDynamicPriceSurvey dynamicPriceSurvey,
            IDat_Floor_Biz datFloorBiz,
            IDat_House_Biz datHouseBiz,
            ICompany company,
            IDAT_Company datCompany,
            IBusinessStore businessStore,
            IProjectCaseTask projectCaseTask,
            IProjectCase projectCase,
            IOfficeSubArea officeSubArea,
            IOfficeProject officeProject,
            IOfficeBuilding officeBuilding,
            IOfficePeiTao officePeiTao,
            IOfficeTenant officeTenant,
            IOfficeCase officeCase,
            IOfficeHouse officeHouse,
            IOfficeDynamicPrice officeDynamicPrice,
            IIndustrySubArea industrySubArea,
            IIndustryProject industryProject,
            IIndustryBuilding industryBuilding,
            IIndustryPeiTao industryPeiTao,
            IIndustryTenant industryTenant,
            IIndustryCase industryCase,
            IIndustryHouse industryHouse,
            IIndustryDynamicPrice industryDynamicPrice,
            IWaitBuildingProject waitProject,
            IHumanProject humanProject,
            IHumanHouse humanHouse,
            ICodePrice codePrice,
            IFloorPrice floorPrice,
            IProjectOtherName projectOtherName,
            IPropertyAddress propertyAddress
            )
        {
            this._importTask = importTask;
            this._dropDownList = dropDownList;
            this._projectSample = projectSample;
            this._datProject = datProject;
            this._datBuilding = datBuilding;
            this._datLand = datLand;
            this._landCase = landCase;
            this._datLandPrice = datLandPrice;
            this._businessStreet = businessStreet;
            this._businessBuilding = businessBuilding;
            this._businessCase = businessCase;
            this._dynamicPriceSurvey = dynamicPriceSurvey;
            this._datFloorBiz = datFloorBiz;
            this._datHouseBiz = datHouseBiz;
            this._company = company;
            this._datCompany = datCompany;
            this._businessStore = businessStore;
            this._datHouse = datHouse;
            this._projectCase = projectCase;
            this._projectCaseTask = projectCaseTask;
            this._officeSubArea = officeSubArea;
            this._officeProject = officeProject;
            this._officeBuilding = officeBuilding;
            this._officePeiTao = officePeiTao;
            this._officeTenant = officeTenant;
            this._officeCase = officeCase;
            this._officeHouse = officeHouse;
            this._officeDynamicPrice = officeDynamicPrice;
            this._industrySubArea = industrySubArea;
            this._industryProject = industryProject;
            this._industryBuilding = industryBuilding;
            this._industryPeiTao = industryPeiTao;
            this._industryTenant = industryTenant;
            this._industryCase = industryCase;
            this._industryHouse = industryHouse;
            this._industryDynamicPrice = industryDynamicPrice;
            this._waitProject = waitProject;
            this._humanProject = humanProject;
            this._humanHouse = humanHouse;
            this._codePrice = codePrice;
            this._floorPrice = floorPrice;
            this._projectOtherName = projectOtherName;
            this._propertyAddress = propertyAddress;
        }

        /// <summary>
        /// 开始执行excel数据处理
        /// </summary>
        /// <param name="cityid">城市ID</param>
        /// <param name="fxtcompanyid">评估机构ID</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="userid">用户名</param>
        /// <param name="taskName">任务名称</param>
        /// <param name="type">类型</param>
        public void Start(int cityid, int fxtcompanyid, string filePath, string userid, string taskName, string type)
        {
            Factory(cityid, fxtcompanyid, filePath, userid, taskName, type);
        }
        /// <summary>
        /// 房号导入格式转换
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="newPath">转换后路径</param>
        public void HouseConvert(string filePath, string newPath)
        {
            this.HouseFormatConvert(filePath, newPath);
        }

        /// <summary>
        ///  方法工厂
        /// </summary>
        /// <param name="cityid">城市ID</param>
        /// <param name="fxtcompanyid">评估机构ID</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="userid">用户名</param>
        /// <param name="taskName">任务名称</param>
        /// <param name="type">类型</param>
        private void Factory(int cityid, int fxtcompanyid, string filePath, string userid, string taskName, string type)
        {
            switch (type)
            {
                case "Building":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, BuildingExcelUpload);
                    break;
                case "BusinessBuilding":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, BusinessBuildingExcelUpload);
                    break;
                case "BusinessCase":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, BusinessCaseExcelUpload);
                    break;
                case "BusinessDynamicPriceSurvey":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, BusinessDynamicPriceSurveyExcelUpload);
                    break;
                case "BusinessFloor":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, BusinessFloorExcelUpload);
                    break;
                case "BusinessHouse":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, BusinessHouseExcelUpload);
                    break;
                case "BusinessStore":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, BusinessStoreExcelUpload);
                    break;
                case "BusinessStreet":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, BusinessStreetExcelUpload);
                    break;
                case "HouseNo":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, HouseNoExcelUpload);
                    break;
                case "HouseNoNew":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, HouseNoNewExcelUpload);
                    break;
                case "LandCase":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, LandCaseExcelUpload);
                    break;
                case "LandInfo":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, LandInfoExcelUpload);
                    break;
                case "LandPrice":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, LandPriceExcelUpload);
                    break;
                case "ProjectCase":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, ProjectCaseExcelUpload);
                    break;
                case "Project":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, ProjectExcelUpload);
                    break;
                case "SampleProject":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, SampleProjectExcelUpload);
                    break;
                case "OfficeSubArea":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, OfficeSubAreaUpload);
                    break;
                case "OfficeProject":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, OfficeProjectUpload);
                    break;
                case "OfficeBuilding":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, OfficeBuildingUpload);
                    break;
                case "OfficeHouse":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, OfficeHouseExcelUpload);
                    break;
                case "OfficePeiTao":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, OfficePeiTaoUpload);
                    break;
                case "OfficeTenant":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, OfficeTenantUpload);
                    break;
                case "OfficeCase":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, OfficeCaseExcelUpload);
                    break;
                case "OfficeDynamicPrice":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, OfficeDynamicPriceExcelUpload);
                    break;
                case "IndustrySubArea":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, IndustrySubAreaUpload);
                    break;
                case "IndustryProject":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, IndustryProjectUpload);
                    break;
                case "IndustryBuilding":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, IndustryBuildingUpload);
                    break;
                case "IndustryHouse":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, IndustryHouseExcelUpload);
                    break;
                case "IndustryPeiTao":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, IndustryPeiTaoUpload);
                    break;
                case "IndustryTenant":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, IndustryTenantUpload);
                    break;
                case "IndustryCase":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, IndustryCaseExcelUpload);
                    break;
                case "IndustryDynamicPrice":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, IndustryDynamicPriceExcelUpload);
                    break;
                case "ProjectWeightRevised":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, ProjectWeightRevisedExcelUpload);
                    break;
                case "HumanProject":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, HumanProjectExcelUpload);
                    break;
                case "HumanHouse":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, HumanHouseExcelUpload);
                    break;
                case "ProjectPeiTao":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, ProjectPeiTaoExcelUpload);
                    break;
                case "HouseRatio":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, HouseRatioExcelUpload);
                    break;
                case "ProjectOtherName":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, ProjectOtherNameExcelUpload);
                    break;
                case "ProjectAvgPrice":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, ProjectAvgPriceExcelUpload);
                    break;
                case "PropertyAddress":
                    ActualStart(cityid, fxtcompanyid, filePath, userid, taskName, PropertyAddressExcelUpload);
                    break;
            }
        }

        /// <summary>
        /// 真实执行的方法
        /// </summary>
        /// <param name="cityid">城市ID</param>
        /// <param name="fxtcompanyid">评估机构ID</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="userid">用户名</param>
        /// <param name="taskName">任务名称</param>
        /// <param name="delegateMethod">委托方法</param>
        private static void ActualStart(int cityid, int fxtcompanyid, string filePath, string userid, string taskName, DelegateMethod delegateMethod)
        {
            delegateMethod(cityid, fxtcompanyid, filePath, userid, taskName);
        }

        /// <summary>
        /// 方法委托
        /// </summary>
        /// <param name="cityid">城市ID</param>
        /// <param name="fxtcompanyid">评估机构ID</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="userid">用户名</param>
        /// <param name="taskName">任务名称</param>
        public delegate void DelegateMethod(int cityid, int fxtcompanyid, string filePath, string userid, string taskName);

        #region 帮助程序

        /// <summary>
        /// 映射网站的根目录
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public static string MapPath(string strPath)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            strPath = strPath.Replace("/", "\\");
            if (strPath.StartsWith("\\"))
            {
                strPath = strPath.TrimStart('\\');
            }
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
        }

        /// <summary>
        /// 获取行政区缓存
        /// </summary>
        /// <returns></returns>
        private IEnumerable<SYS_Area> GetAreaCach(int cityId)
        {
            try
            {
                var con = RedisConnection.Connection;
                var database = con.GetDatabase();

                var key = "Share:SysArea:" + cityId;
                var areaCach = database.Get<List<SYS_Area>>(key);

                if (areaCach == null)
                {
                    var data = _dropDownList.GetAreaIds(cityId);
                    database.Set(key, data.ToList(), new TimeSpan(2, 0, 0));
                    areaCach = database.Get<List<SYS_Area>>(key);
                }

                return areaCach;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        /// <summary>
        /// 获取行政区ID
        /// </summary>
        /// <param name="cityId">城市Id</param>
        /// <param name="areaName">行政区名</param>
        /// <returns>行政区ID</returns>
        private int GetAreaId(int cityId, string areaName)
        {
            return _dropDownList.GetAreaIdByName(cityId, areaName);
        }

        /// <summary>
        /// 获取Code
        /// </summary>
        /// <param name="name">code名称</param>
        /// <param name="typeId">所属类型ID</param>
        /// <returns>code</returns>
        private int GetCodeByName(string name, params int[] typeId)
        {
            return _dropDownList.GetCodeByName(name, typeId);
        }

        private int GetSubCodeByCode(int code)
        {
            return _dropDownList.GetDictBySubCode(code).Select(m => m.id).Distinct().FirstOrDefault();
        }

        /// <summary>
        /// 获取公司ID
        /// </summary>
        /// <param name="cityId">城市ID</param>
        /// <param name="name">公司名称</param>
        /// <returns></returns>
        private int GetCompanyIdByName(int cityId, string name)
        {
            return _company.GetCompany_like(name, cityId).Select(m => m.CompanyId).FirstOrDefault();
        }

        /// <summary>
        /// 获取公司ID
        /// </summary>
        /// <param name="cityId">城市ID</param>
        /// <param name="name">公司名称</param>
        /// <returns></returns>
        private int AddCompany(DAT_Company dc)
        {
            return _company.AddCompany(dc);
        }

        /// <summary>
        /// 验证宗地号是否唯一
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="areaId"></param>
        /// <param name="fieldNo"></param>
        /// <returns></returns>
        private bool ValidFieldNo(int cityId, int areaId, string fieldNo)
        {
            return _businessBuilding.ValidFieldNo(cityId, areaId, fieldNo);
        }

        #region 住宅
        /// <summary>
        /// 获取楼盘ID
        /// </summary>
        /// <param name="fxtCompanyId">评估公司ID</param>
        /// <param name="cityId">城市ID</param>
        /// <param name="areaId">行政区ID</param>
        /// <param name="projectName">楼盘名称</param>
        /// <returns>楼盘ID(无记录，则返回-1)</returns>
        private IQueryable<DAT_Project> ProjectIdByName(int fxtCompanyId, int cityId, int areaId, string projectName)
        {
            var query = _datProject.GetProjectIdByName(cityId, areaId, fxtCompanyId, projectName);
            return query ?? new List<DAT_Project>().ToList().AsQueryable();
        }

        /// <summary>
        /// 获取楼栋ID
        /// </summary>
        /// <param name="projectId">楼盘ID</param>
        /// <param name="buildingName">楼栋名称</param>
        /// <param name="cityId">城市ID</param>
        /// <param name="fxtCompanyId">评估机构ID</param>
        /// <returns>楼栋ID</returns>
        private int BuildingIdByName(int projectId, string buildingName, int cityId, int fxtCompanyId)
        {
            return _datBuilding.GetBuildingId(projectId, buildingName, cityId, fxtCompanyId);
        }

        /// <summary>
        /// 获取房号ID
        /// </summary>
        private int HouseIdByName(int buildingId, string houseName, int cityId, int fxtCompanyId)
        {
            return _datHouse.GetHouseId(buildingId, houseName, cityId, fxtCompanyId);
        }

        /// <summary>
        /// 获取片区ID
        /// </summary>
        /// <param name="subAreaName"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        private int SubAreaIdByName(string subAreaName, int areaId)
        {
            return _dropDownList.GetSubAreaIdByName(subAreaName, areaId);
        }

        /// <summary>
        /// 获取text为是否的value值
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private int YesOrNo(string text)
        {
            switch (text.Trim())
            {
                case "是":
                    return 1;
                case "否":
                    return 0;
                default:
                    return -1;
            }
        }

        /// <summary>
        /// 获取楼层对应的建筑类型
        /// </summary>
        /// <param name="floorNumber"></param>
        /// <returns></returns>
        private int BuildingTypeCode(int floorNumber)
        {
            if (floorNumber < 4) return 2003001; //低层
            if (floorNumber >= 4 || floorNumber <= 8) return 2003002;//多层
            if (floorNumber >= 9 || floorNumber <= 12) return 2003003; //小高层
            if (floorNumber > 12) return 2003004; //高层

            return 0;
        }

        private SYS_ProjectMatch GetProjectMatchProjectId(string projectNetName, string netAreaName, int cityid, int fxtcompanyid)
        {
            return _projectOtherName.GetProjectMatchProjectId(projectNetName, netAreaName, cityid, fxtcompanyid);
        }

        private Dat_WaitProject GetWaitProjectMatchProjectId(string projectName, int cityid, int fxtcompanyid)
        {
            return _waitProject.GetSingleWaitProject(projectName, cityid, fxtcompanyid);
        }
        #endregion

        #region 商业
        /// <summary>
        /// 根据商圈名称获取商圈Id
        /// 刘晓博
        /// 2014-09-12
        /// </summary>
        /// <param name="subAreaName"></param>
        /// <returns></returns>
        private int GetSubAreaId(string subAreaName, int areaId)
        {
            return _dropDownList.GetBizSubAreaIdByName(subAreaName, areaId);
        }
        /// <summary>
        /// 根据商业街名称获取商业街Id
        /// 刘晓博
        /// 2014-10-14
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="areaId"></param>
        /// <param name="fxtcompanyId"></param>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public IQueryable<long> GetProjectId(int cityId, int areaId, int fxtcompanyId, string projectName)
        {
            var projectIds = _businessStreet.GetProjectId(cityId, fxtcompanyId, areaId, projectName);
            if (projectIds == null) return new List<long>().AsQueryable();
            return projectIds;
        }

        /// <summary>
        /// 获取商业楼栋ID
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <param name="projectId"></param>
        /// <param name="buildingName"></param>
        /// <returns></returns>
        private long GetBuildingId(int cityId, int fxtCompanyId, long projectId, string buildingName)
        {
            return _businessBuilding.GetBuildIdByName(projectId, buildingName, cityId, fxtCompanyId);
        }

        /// <summary>
        /// 验证楼栋名称是否存在
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="areaId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <param name="buildName"></param>
        /// <param name="subAreaId">商圈id</param>
        /// <returns></returns>
        private Dat_Building_Biz IsExistBuildName(int cityId, int areaId, int fxtCompanyId, string buildName, int subAreaId, int projectId)
        {
            return _businessBuilding.IsExistBuildName(cityId, areaId, fxtCompanyId, buildName, subAreaId, projectId);
        }

        /// <summary>
        /// 根据楼栋ID、物理层获取楼层Id
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="floorNo"></param>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        private long GetFloorIdByName(long buildingId, string floorNo, int cityId, int fxtCompanyId)
        {
            return _datFloorBiz.GetFloorIdByName(buildingId, floorNo, cityId, fxtCompanyId);
        }
        /// <summary>
        /// 房号名称是否存在
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="floorId"></param>
        /// <param name="houseName"></param>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        private bool IsExistHouseName(long buildingId, long floorId, string houseName, int cityId, int fxtCompanyId)
        {
            return _datHouseBiz.IsExistHouseName(buildingId, floorId, houseName, cityId, fxtCompanyId);
        }
        #endregion

        #region 办公
        /// <summary>
        /// 获取subAreaID
        /// </summary>
        private int GetSubAreaIdByName_office(string subAreaName, int areaId, int fxtCompanyId)
        {
            return _officeSubArea.GetSubAreaIdByName(subAreaName, areaId, fxtCompanyId);
        }
        /// <summary>
        /// 获取办公楼盘id
        /// </summary>
        private IQueryable<long> GetProjectIdByName_office(string projectName, int areaId, int cityId, int fxtCompanyId)
        {
            var query = _officeProject.GetProjectIdByName(projectName, areaId, cityId, fxtCompanyId);
            return query ?? new List<long>().AsQueryable();
        }

        private long GetPeiTaoIdByName_office(string peiTaoName, long projectId, int cityId, int fxtCompanyId)
        {
            return _officePeiTao.GetPeiTaoIdByName(peiTaoName, projectId, cityId, fxtCompanyId);
        }

        /// <summary>
        /// 获取办公楼栋ID
        /// </summary>
        /// <returns></returns>
        private long GetBuildingIdByName_Office(long projectId, string buildingName, int cityId, int companyId)
        {
            return _officeBuilding.GetOfficeBuildingId(projectId, -1, buildingName, cityId, companyId);
        }

        /// <summary>
        /// 获取办公房号ID
        /// </summary>
        /// <returns></returns>
        private long GetHouseIdByName_Office(long buildingId, string houseName, int cityId, int companyId)
        {
            return _officeBuilding.GetOfficeBuildingId(buildingId, -1, houseName, cityId, companyId);
        }

        private int GetCompanyIdByName_office(int cityId, string name)
        {
            return _company.GetCompany_office(name, cityId).Select(m => m.CompanyId).FirstOrDefault();
        }

        #endregion

        #region 工业
        /// <summary>
        /// 获取工业subAreaID
        /// </summary>
        private int GetSubAreaIdByName_Industry(string subAreaName, int areaId, int fxtCompanyId)
        {
            return _industrySubArea.GetSubAreaIdByName(subAreaName, areaId, fxtCompanyId);
        }
        /// <summary>
        /// 获取工业楼盘id
        /// </summary>
        private IQueryable<long> GetProjectIdByName_Industry(string projectName, int areaId, int cityId, int fxtCompanyId)
        {
            var query = _industryProject.GetProjectIdByName(projectName, areaId, cityId, fxtCompanyId);
            return query ?? new List<long>().AsQueryable();
        }
        /// <summary>
        /// 获取工业楼栋ID
        /// </summary>
        /// <returns></returns>
        private long GetBuildingIdByName_Industry(long projectId, string buildingName, int cityId, int companyId)
        {
            return _industryBuilding.GetIndustryBuildingId(projectId, -1, buildingName, cityId, companyId);
        }
        /// <summary>
        /// 获取工业房号ID
        /// </summary>
        /// <returns></returns>
        private long GetHouseIdByName_Industry(long buildingId, string houseName, int cityId, int companyId)
        {
            return _industryBuilding.GetIndustryBuildingId(buildingId, -1, houseName, cityId, companyId);
        }
        /// <summary>
        /// 获取工业项目配套
        /// </summary>
        /// <param name="peiTaoName"></param>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        private long GetPeiTaoIdByName_Industry(string peiTaoName, long projectId, int cityId, int fxtCompanyId)
        {
            return _industryPeiTao.GetPeiTaoIdByName(peiTaoName, projectId, cityId, fxtCompanyId);
        }
        /// <summary>
        /// 获取商家id，与office共用
        /// </summary>
        private int GetCompanyIdByName_Industry(int cityId, string name)
        {
            return _company.GetCompany_office(name, cityId).Select(m => m.CompanyId).FirstOrDefault();
        }
        #endregion

        #region 缓存

        /// <summary>
        /// 获取code缓存
        /// </summary>
        /// <returns></returns>
        private IEnumerable<SYS_Code> GetCodeCach()
        {
            var con = RedisConnection.Connection;
            var database = con.GetDatabase();

            var key = "Share:SysCode";
            var codeCach = database.Get<List<SYS_Code>>(key);

            if (codeCach == null || codeCach.Count == 0)
            {
                var data = _dropDownList.GetCodes();
                database.Set(key, data.ToList(), new TimeSpan(2, 0, 0));
                codeCach = database.Get<List<SYS_Code>>(key);
            }

            return codeCach;
        }

        /// <summary>
        /// 获取楼盘缓存
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        private IEnumerable<DAT_Project> GetProjectCach(int cityId, int fxtCompanyId)
        {
            var con = RedisConnection.Connection;
            var database = con.GetDatabase();

            var key = "House:Project:" + cityId + ":" + fxtCompanyId;
            var projectsCach = database.Get<List<DAT_Project>>(key);

            if (projectsCach == null || projectsCach.Count == 0)
            {
                var data = _datProject.GetProjectIds(cityId, fxtCompanyId);
                database.Set(key, data.ToList(), new TimeSpan(2, 0, 0));
                projectsCach = database.Get<List<DAT_Project>>(key);
            }

            return projectsCach;
        }

        //获取楼栋缓存
        private IEnumerable<DAT_Building> GetBuildingCach(int cityId, int fxtCompanyId)
        {
            var con = RedisConnection.Connection;
            var database = con.GetDatabase();

            var buildingKey = "House:Building:" + cityId + ":" + fxtCompanyId;
            var buildingCach = database.Get<List<DAT_Building>>(buildingKey);


            if (buildingCach == null || buildingCach.Count == 0)
            {
                var data = _datBuilding.GetBuildingIds(cityId, fxtCompanyId);
                database.Set(buildingKey, data.ToList(), new TimeSpan(2, 0, 0));
                buildingCach = database.Get<List<DAT_Building>>(buildingKey);
            }

            return buildingCach;
        }

        #endregion
        
        #region 用户中心API
        private CompanyProduct FxtUserCenterService_GetFPInfo(int fxtcompanyid, int cityid, int producttypecode, string username, out Exception msg, int procode)
        {
            string api = ConfigurationManager.AppSettings["fxtusercenterservice"];
            string appid = ConfigurationManager.AppSettings["usercenterserviceappid"];
            string apppwd = ConfigurationManager.AppSettings["usercenterserviceapppwd"];
            string appkey = ConfigurationManager.AppSettings["usercenterserviceappkey"];
            string time = DateTime.Now.ToString("yyyyMMddHHmmss");
            string signname = ConfigurationManager.AppSettings["signname"];
            string functionname = "companythirteen";

            string[] pwdArray = { appid, apppwd, signname, time, functionname };
            string code = EncryptHelper.GetMd5(pwdArray, appkey);

            CompanyProduct cp = null;
            msg = null;
            try
            {
                if (!string.IsNullOrEmpty(api))
                {
                    var sinfo = new { appid = appid, apppwd = apppwd, signname = signname, time = time, code = code, functionname = functionname };
                    var info = new
                    {
                        uinfo = new { username, token = "" },
                        appinfo = new ApplicationInfo(procode.ToString()),
                        funinfo = new { companyid = fxtcompanyid, producttypecode = producttypecode, cityid = cityid }
                    };
                    var post = "{\"sinfo\":\"" + JSONHelper.ObjectToJSON(sinfo).Replace("\"", "'") + "\",\"info\":\"" + JSONHelper.ObjectToJSON(info).Replace("\"", "'") + "\"}";
                    var str = APIPostBack(api, post, "application/json");
                    var rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                    if (rtn.returntype > 0)
                    {
                        cp = new CompanyProduct();
                        var returnSInfo = JSONHelper.JSONToObject<dynamic>(rtn.data.ToString())[0];
                        int ParentProductTypeCode = Convert.ToInt32(returnSInfo["parentproducttypecode"]);
                        int ParentShowDataCompanyId = Convert.ToInt32(returnSInfo["parentshowdatacompanyid"]);

                        cp.ParentProductTypeCode = ParentProductTypeCode;
                        cp.ParentShowDataCompanyId = ParentShowDataCompanyId;
                        return cp;
                    }
                    msg = new Exception(rtn.returntext.ToString());
                }
            }
            catch (Exception ex)
            {
                msg = ex;
            }
            return cp;
        }
        private string APIPostBack(string url, string post, string contentType)
        {
            byte[] postData = Encoding.UTF8.GetBytes(post);
            var client = new WebClient();
            client.Headers.Add("Content-Type", contentType);
            client.Headers.Add("ContentLength", postData.Length.ToString());
            //这里url要组装安全标记等参数
            var result = "";
            try
            {
                byte[] responseData = client.UploadData(url, "POST", postData);
                result = Encoding.UTF8.GetString(responseData);
                //找退出原因
                //LogHelper.Info(result);
            }
            catch (Exception ex)
            {
                result = JSONHelper.GetJson(null, 0, ex.Message, ex);
            }
            client.Dispose();
            return result;
        }
        #endregion
        #endregion

    }
}
