using FxtDataAcquisition.Application.Interfaces;
using FxtDataAcquisition.Domain;
using FxtDataAcquisition.Domain.DTO;
using FxtDataAcquisition.Domain.Models;
using FxtDataAcquisition.FxtAPI.FxtDataWcf.Manager;
using FxtDataAcquisition.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using System.Diagnostics;
using log4net;
using CAS.Entity.DBEntity;

namespace FxtDataAcquisition.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHouseService _houseService;
        private readonly ITempletService _templetService;
        private readonly ICodeService _codeService;

        private static readonly ILog log = LogManager.GetLogger(typeof(ProjectService));

        public ProjectService(IUnitOfWork unitOfWork, IHouseService houseService, ITempletService templetService, ICodeService codeService)
        {
            this._unitOfWork = unitOfWork;
            this._houseService = houseService;
            this._templetService = templetService;
            this._codeService = codeService;
        }

        /// <summary>
        /// 获取楼盘详情
        /// </summary>
        /// <param name="projectid"></param>
        /// <param name="cityid"></param>
        /// <param name="allotid"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        public ProjectDto GetProjectDetals(int projectid, int cityid, long allotid, int fxtCompanyId)
        {
            //楼盘
            var project = _unitOfWork.ProjectRepository.Get(m => m.CityID == cityid && m.ProjectId == projectid).FirstOrDefault();
            //任务
            var alloFlow = _unitOfWork.AllotFlowRepository.Get(m => m.CityId == cityid && m.Id == allotid).FirstOrDefault();
            //公司
            var companys = _unitOfWork.P_CompanyRepository.Get(m => m.CityId == cityid && m.ProjectId == projectid);
            //物管公司
            var managerCompany = companys.Where(m => m.CompanyType == SYSCodeManager.COMPANYTYPECODE_4).FirstOrDefault();
            //开发商
            var developers = companys.Where(m => m.CompanyType == SYSCodeManager.COMPANYTYPECODE_1).FirstOrDefault();
            //停车状况
            var parkingStatus = _unitOfWork.P_AppendageRepository.Get(m => m.CityId == cityid && m.ProjectId == projectid &&
                m.AppendageCode == SYSCodeManager.APPENDAGECODE_14).FirstOrDefault();
            //照片数
            var photoCount = _unitOfWork.P_PhotoRepository.Get(
                m => m.CityId == cityid && m.ProjectId == projectid && m.FxtCompanyId == fxtCompanyId
                && (!m.BuildingId.HasValue || m.BuildingId == 0)).Count();

            ProjectDto projectDto = Mapper.Map<Project, ProjectDto>(project);
            projectDto.ParkingStatus = parkingStatus == null ? 0 : parkingStatus.ClassCode;
            projectDto.PhotoCount = photoCount;
            projectDto.Developers = developers == null ? "" : developers.CompanyName;
            projectDto.ManagerCompany = managerCompany == null ? "" : managerCompany.CompanyName;
            projectDto.StateDate = alloFlow.StateDate;
            projectDto.AllotFlowremark = alloFlow.Remark;
            projectDto.Allotid = alloFlow.Id;
            projectDto.TatolBuildingNum = _unitOfWork.BuildingRepository.Get().Count(m => m.ProjectId == projectid && m.CityID == cityid && m.FxtCompanyId == fxtCompanyId);

            return projectDto;
        }

        /// <summary>
        /// 上传任务
        /// </summary>
        /// <param name="allotId"></param>
        /// <param name="userName"></param>
        /// <param name="userTrueName"></param>
        /// <param name="fxtCompanyId"></param>
        /// <param name="cityId"></param>
        /// <param name="projectDto"></param>
        /// <param name="isValid"></param>
        /// <returns></returns>
        public int SetAllotProjectInfo(int allotId, string userName, string userTrueName, int fxtCompanyId, int cityId, ProjectApiDto projectDto, int isValid)
        {
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            //log.Info("Stopwatch开始计时，" + allotId);
            //int housecount = 0;
            //int housedetailcount = 0;

            string message = string.Empty;
            DateTime nowTime = DateTime.Now;

            #region 任务校验

            AllotFlow allotFlow = _unitOfWork.AllotFlowRepository.GetById(allotId);
            if (allotFlow == null)
            {
                //任务不存在
                return -5;
            }

            if (allotFlow.SurveyUserName != userName.ToLower())
            {
                //不是当前用户的任务
                return -2;
            }
            else if (allotFlow.StateCode != SYSCodeManager.STATECODE_4)
            {
                //不是查勘中任务
                return -3;
            }
            #endregion

            #region 默认模板校验
            var templetProject = _templetService.GetTempletDefult(SYSCodeManager.DATATYPECODE_1, fxtCompanyId);

            var templetBuilding = _templetService.GetTempletDefult(SYSCodeManager.DATATYPECODE_2, fxtCompanyId);

            var templetHouse = _templetService.GetTempletDefult(SYSCodeManager.DATATYPECODE_4, fxtCompanyId);

            if (templetProject == null)
            {
                return -6;
            }
            else if (templetBuilding == null)
            {
                return -7;
            }
            else if (templetHouse == null)
            {
                return -8;
            }
            #endregion

            #region (Project更新值)

            Project project = _unitOfWork.ProjectRepository.GetBy(m => m.ProjectId == allotFlow.DatId && m.Valid == 1);

            //获取实体属性
            var propertys = project.GetType().GetProperties();

            projectDto.Templet.FieldGroups.SelectMany(m => m.Fields).ToList()

                .ForEach((o) =>
                {
                    //取属性
                    var property = propertys.Where(pInfo => pInfo.Name == o.FieldName).FirstOrDefault();

                    if (property != null)
                    {
                        List<FxtDataAcquisition.Domain.Models.SYSCode> codes = new List<FxtDataAcquisition.Domain.Models.SYSCode>();

                        switch (o.FieldName)
                        {
                            case "PurposeCode":
                                codes = _codeService.PurposeCodeManager();
                                break;
                            case "MaintenanceCode":
                                codes = _codeService.LevelManager();
                                break;
                            case "BuildingTypeCode":
                                codes = _codeService.BuildingTypeCodeManager();
                                break;
                            case "StructureCode":
                                codes = _codeService.BuildingStructureCodeManager();
                                break;
                            case "Wall":
                                codes = _codeService.WallCodeManager();
                                break;
                            case "InnerFitmentCode":
                                codes = _codeService.InnerFitmentCodeManager();
                                break;
                            case "PipelineGasCode":
                                codes = _codeService.PipelineGasCodeManager();
                                break;
                            case "HeatingModeCode":
                                codes = _codeService.HeatingModeCodeManager();
                                break;
                            case "WallTypeCode":
                                codes = _codeService.WallTypeCodeManager();
                                break;
                            case "BHouseTypeCode":
                                codes = _codeService.BHousetypeCodeManager();
                                break;
                            default:
                                break;
                        }

                        switch (o.EdiTextType)
                        {
                            case 0:
                                //选择框
                                if (codes != null && codes.Count > 0)
                                {
                                    if (o.Type == "C")
                                    {
                                        if (!string.IsNullOrEmpty(o.Value))
                                        {
                                            var vs = o.Value.Split(',');

                                            var code = codes.Where(m => vs.Contains(m.CodeName));

                                            if (code != null && code.Count() > 0)
                                            {
                                                var value = string.Join(",", code.Select(m => m.Code));

                                                property.SetValue(project, value);
                                            }
                                            else
                                            {
                                                property.SetValue(project, "");
                                            }
                                        }
                                        else
                                        {
                                            property.SetValue(project, "");
                                        }
                                    }
                                }
                                else
                                {
                                    property.SetValue(project, o.Value);
                                }
                                break;
                            case 1:
                                //选择框
                                if (codes != null && codes.Count > 0)
                                {
                                    if (o.Type == "R")
                                    {
                                        var code = codes.FirstOrDefault(m => m.CodeName == o.Value);

                                        if (code != null)
                                        {
                                            property.SetValue(project, code.Code);
                                        }
                                        else
                                        {
                                            property.SetValue(project, 0);
                                        }
                                    }
                                }
                                else
                                {
                                    int iv = 0;
                                    if (int.TryParse(o.Value, out iv))
                                    {
                                        property.SetValue(project, o.Value);
                                    }
                                    else
                                    {
                                        if (o.IsNull > 0)
                                        {
                                            property.SetValue(project, null);
                                        }
                                    }
                                }
                                break;
                            case 2:
                                decimal dv = 0;
                                if (decimal.TryParse(o.Value, out dv))
                                {
                                    property.SetValue(project, o.Value);
                                }
                                else
                                {
                                    if (o.IsNull > 0)
                                    {
                                        property.SetValue(project, null);
                                    }
                                }
                                break;
                            case 3:
                                DateTime dtv = default(DateTime);
                                if (DateTime.TryParse(o.Value, out dtv))
                                {
                                    property.SetValue(project, o.Value);
                                }
                                else
                                {
                                    if (o.IsNull > 0)
                                    {
                                        property.SetValue(project, null);
                                    }
                                }
                                break;
                            default:
                                break;
                        }

                        
                        //if (value is DateTime)
                        //{
                        //    fieldDto.Value = DateTime.Parse(value.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        //}
                        //else
                        //{
                        //fieldDto.Value = value.ToString();

                        //if (codes != null && codes.Count > 0)
                        //{
                        //    //选项值
                        //    if (fieldDto.FieldType == 3)
                        //    {
                        //        var code = codes.FirstOrDefault(m => m.Code == int.Parse(fieldDto.Value));

                        //        fieldDto.Value = code == null ? "" : code.CodeName;
                        //    }
                        //    else if (fieldDto.FieldType == 5)
                        //    {
                        //        //多选
                        //        var vs = fieldDto.Value.Split(',').ConvertToIntList();

                        //        var codeList = codes.Where(m => vs.Contains(m.Code));

                        //        if (codeList != null)
                        //        {
                        //            var vsname = codeList.Select(m => m.CodeName);

                        //            fieldDto.Value = string.Join(",", vsname);
                        //        }
                        //    }

                        //    fieldDto.Choise = codes.Select(m => m.CodeName).ToList();
                        //}
                        //}
                    }
                    else
                    {
                        #region (停车状况)
                        if (o.FieldName == "ParkingStatus")
                        {
                            if (project.Appendages == null) project.Appendages = new List<PAppendage>();

                            PAppendage pAppend = _unitOfWork.P_AppendageRepository.GetBy(m => m.ProjectId == project.ProjectId && m.CityId == cityId && m.AppendageCode == SYSCodeManager.APPENDAGECODE_14);
                            //project.Appendages.Where(m => m.CityId == cityId && m.AppendageCode == SYSCodeManager.APPENDAGECODE_14).FirstOrDefault();

                            int iv = 0;

                            int.TryParse(o.Value, out iv);

                            if (pAppend == null)
                            {
                                PAppendage pAppendage = new PAppendage()
                                {
                                    CityId = cityId,
                                    ProjectId = project.ProjectId,
                                    ClassCode = iv,
                                    AppendageCode = SYSCodeManager.APPENDAGECODE_14
                                };

                                //project.Appendages.Add(pAppendage);
                                _unitOfWork.P_AppendageRepository.Insert(pAppendage);
                            }
                            else
                            {
                                pAppend.ClassCode = iv;

                                _unitOfWork.P_AppendageRepository.Update(pAppend);
                            }
                        }
                        #endregion

                        #region 关联公司
                        if (o.FieldName == "DevelopersCompany")
                        {
                            //更新开发商
                            if (!o.Value.IsNullOrEmpty()) { 

                                var developers = _unitOfWork.P_CompanyRepository.GetBy(m => m.ProjectId == project.ProjectId && m.CityId == cityId && m.CompanyType == SYSCodeApi.COMPANYTYPECODE_1);
                                //project.Companys.Where(m => m.CityId == cityId && m.CompanyType == SYSCodeApi.COMPANYTYPECODE_1).FirstOrDefault();

                                if (developers == null)
                                {
                                    PCompany pcompany = new PCompany()
                                    {
                                        CityId = cityId,
                                        CompanyName = o.Value,
                                        CompanyType = SYSCodeApi.COMPANYTYPECODE_1,
                                        ProjectId = project.ProjectId
                                    };

                                    //project.Companys.Add(pcompany);
                                    _unitOfWork.P_CompanyRepository.Insert(pcompany);
                                }
                                else
                                {
                                    developers.CompanyName = o.Value;

                                    _unitOfWork.P_CompanyRepository.Update(developers);
                                }
                            }
                            else
                            {
                                _unitOfWork.P_CompanyRepository.Delete(m => m.CityId == cityId && m.ProjectId == project.ProjectId && m.CompanyType == SYSCodeApi.COMPANYTYPECODE_1);
                            }
                        }
                        if (o.FieldName == "ManagerCompany")
                        {
                            
                            //更新物业管理公司
                            if (!o.Value.IsNullOrEmpty())
                            {
                                var managerCompany = _unitOfWork.P_CompanyRepository.GetBy(m => m.ProjectId == project.ProjectId && m.CityId == cityId && m.CompanyType == SYSCodeApi.COMPANYTYPECODE_4);
                                //project.Companys.Where(m => m.CityId == cityId && m.CompanyType == SYSCodeApi.COMPANYTYPECODE_4).FirstOrDefault();
                                if (managerCompany == null)
                                {
                                    PCompany pcompany = new PCompany()
                                    {
                                        CityId = cityId,
                                        CompanyName = o.Value,
                                        CompanyType = SYSCodeApi.COMPANYTYPECODE_4,
                                        ProjectId = project.ProjectId
                                    };
                                    //project.Companys.Add(pcompany);
                                    _unitOfWork.P_CompanyRepository.Insert(pcompany);
                                }
                                else
                                {
                                    managerCompany.CompanyName = o.Value;

                                    _unitOfWork.P_CompanyRepository.Update(managerCompany);
                                }
                            }
                            else
                            {
                                _unitOfWork.P_CompanyRepository.Delete(m => m.CityId == cityId && m.ProjectId == project.ProjectId && m.CompanyType == SYSCodeApi.COMPANYTYPECODE_4);
                            }
                        }
                        #endregion
                    }
                });


            //project = Mapper.Map<ProjectDto, Project>(projectDto);
            //allotFlow.Project = Mapper.Map<ProjectDto, Project>(projectDto);

            if (isValid == 0 && !CheckProjectObj(project, out message))
            {
                return 0;
            }
            //allotFlow.Project.EndDate = projectDto.Enddate;
            //allotFlow.Project.ManagerQuality = projectDto.Managerquality;
            //allotFlow.Project.ParkingStatus = projectDto.ParkingStatus;
            //allotFlow.Project.PhotoCount = projectDto.PhotoCount;
            //allotFlow.Project.FxtProjectId = projectDto.FxtProjectId;
            //allotFlow.Project.ProjectName = projectDto.ProjectName;
            //allotFlow.Project.OtherName = projectDto.OtherName;
            //allotFlow.Project.CityID = projectDto.CityID;
            //allotFlow.Project.AreaID = projectDto.AreaID;
            //allotFlow.Project.SubAreaId = projectDto.SubAreaId;
            //allotFlow.Project.Address = projectDto.Address;
            //allotFlow.Project.PurposeCode = projectDto.PurposeCode;
            //allotFlow.Project.RightCode = projectDto.RightCode;
            //allotFlow.Project.Detail = projectDto.Detail;
            //allotFlow.Project.X = projectDto.X;
            //allotFlow.Project.Y = projectDto.Y;
            //allotFlow.Project.BuildingNum = projectDto.BuildingNum;
            //allotFlow.Project.TotalNum = projectDto.Totalnum;
            //allotFlow.Project.ParkingNumber = projectDto.Parkingnumber;
            //allotFlow.Project.East = projectDto.East;
            //allotFlow.Project.West = projectDto.West;
            //allotFlow.Project.South = projectDto.South;
            //allotFlow.Project.North = projectDto.North;
            project.Status = SYSCodeManager.STATECODE_4;
            project.FxtCompanyId = fxtCompanyId;
            project.CityID = cityId;
            project.Valid = 1;
            project.SaveDateTime = nowTime;//最后修改时间
            project.SaveUser = userName;//修改人

            _unitOfWork.ProjectRepository.Update(project);

            #region (停车状况)

            //if (project.Appendages == null) project.Appendages = new List<PAppendage>();

            //PAppendage pAppend = _unitOfWork.P_AppendageRepository.GetBy(m => m.ProjectId == project.ProjectId && m.CityId == cityId && m.AppendageCode == SYSCodeManager.APPENDAGECODE_14);
            ////project.Appendages.Where(m => m.CityId == cityId && m.AppendageCode == SYSCodeManager.APPENDAGECODE_14).FirstOrDefault();

            //if (pAppend == null)
            //{
            //    PAppendage pAppendage = new PAppendage()
            //    {
            //        CityId = cityId,
            //        ProjectId = project.ProjectId,
            //        ClassCode = project.ParkingStatus,
            //        AppendageCode = SYSCodeManager.APPENDAGECODE_14
            //    };

            //    //project.Appendages.Add(pAppendage);
            //    _unitOfWork.P_AppendageRepository.Insert(pAppendage);
            //}
            //else
            //{
            //    pAppend.ClassCode = project.ParkingStatus;

            //    _unitOfWork.P_AppendageRepository.Update(pAppend);
            //}

            #endregion

            #region (Project更新关联公司)
            //f (project.Companys == null) project.Companys = new List<PCompany>();

            ////更新开发商
            //if (!string.IsNullOrEmpty(projectDto.Developers))
            //{

            //    var developers = _unitOfWork.P_CompanyRepository.GetBy(m => m.ProjectId == project.ProjectId && m.CityId == cityId && m.CompanyType == SYSCodeApi.COMPANYTYPECODE_1);
            //    //project.Companys.Where(m => m.CityId == cityId && m.CompanyType == SYSCodeApi.COMPANYTYPECODE_1).FirstOrDefault();

            //    if (developers == null)
            //    {
            //        PCompany pcompany = new PCompany()
            //        {
            //            CityId = cityId,
            //            CompanyName = projectDto.Developers,
            //            CompanyType = SYSCodeApi.COMPANYTYPECODE_1,
            //            ProjectId = project.ProjectId
            //        };

            //        //project.Companys.Add(pcompany);
            //        _unitOfWork.P_CompanyRepository.Insert(pcompany);
            //    }
            //    else
            //    {
            //        developers.CompanyName = projectDto.Developers;

            //        _unitOfWork.P_CompanyRepository.Update(developers);
            //    }
            //}
            //else
            //{
            //    _unitOfWork.P_CompanyRepository.Delete(m => m.CityId == cityId && m.ProjectId == project.ProjectId && m.CompanyType == SYSCodeApi.COMPANYTYPECODE_1);
            //}

            ////更新物业管理公司
            //if (!string.IsNullOrEmpty(projectDto.ManagerCompany))
            //{
            //    var managerCompany = _unitOfWork.P_CompanyRepository.GetBy(m => m.ProjectId == project.ProjectId && m.CityId == cityId && m.CompanyType == SYSCodeApi.COMPANYTYPECODE_4);
            //    //project.Companys.Where(m => m.CityId == cityId && m.CompanyType == SYSCodeApi.COMPANYTYPECODE_4).FirstOrDefault();
            //    if (managerCompany == null)
            //    {
            //        PCompany pcompany = new PCompany()
            //        {
            //            CityId = cityId,
            //            CompanyName = projectDto.ManagerCompany,
            //            CompanyType = SYSCodeApi.COMPANYTYPECODE_4,
            //            ProjectId = project.ProjectId
            //        };
            //        //project.Companys.Add(pcompany);
            //        _unitOfWork.P_CompanyRepository.Insert(pcompany);
            //    }
            //    else
            //    {
            //        managerCompany.CompanyName = projectDto.ManagerCompany;

            //        _unitOfWork.P_CompanyRepository.Update(managerCompany);
            //    }
            //}
            //else
            //{
            //    _unitOfWork.P_CompanyRepository.Delete(m => m.CityId == cityId && m.ProjectId == project.ProjectId && m.CompanyType == SYSCodeApi.COMPANYTYPECODE_4);
            //}
            #endregion

            #endregion

            #region(Building更新值)
            //if (projectDto.BuildingDtolist != null && projectDto.BuildingDtolist.Count > 0)
            //{
            //    int bid = 0;
            //    foreach (var buildingDto in projectDto.BuildingDtolist)
            //    {
            //        var t = false;
            //        //Stopwatch swbs = new Stopwatch();
            //        //swbs.Start();
            //        //验证楼栋字段数据
            //        if (!CheckBuildingObj(buildingDto, out message))
            //        {
            //            return 0;
            //        }
            //        //给building各属性赋值
            //        Building building = Mapper.Map<BuildingDto, Building>(buildingDto);
            //        building.Valid = 1;
            //        building.ProjectId = project.ProjectId;
            //        building.FxtCompanyId = allotFlow.FxtCompanyId;
            //        building.CityID = cityId;

            //        //删除楼栋
            //        if (buildingDto.Valid == 0)
            //        {
            //            var build = _unitOfWork.BuildingRepository.GetBy(m => m.AppId == building.AppId);
            //            if (build != null)
            //            {
            //                _unitOfWork.HouseRepository.Delete(m => m.BuildingId == build.BuildingId);
            //                _unitOfWork.HouseDetailsRepository.Delete(m => m.BuildingId == build.BuildingId);
            //                _unitOfWork.BuildingRepository.Delete(m => m.AppId == building.AppId);
            //            }
            //        }
            //        else
            //        {
            //            if (project.Buildings == null)
            //            {
            //                project.Buildings = new List<Building>();
            //            }
            //            Building eBuilding = _unitOfWork.BuildingRepository.GetBy(m => m.AppId == building.AppId && m.ProjectId == project.ProjectId && m.Valid == 1);
            //            //project.Buildings.Where(m => m.Valid == 1 && m.AppId == building.AppId).FirstOrDefault();

            //            if (eBuilding == null)
            //            {
            //                building.Creator = userName;
            //                building.CreateTime = nowTime;
            //                eBuilding = building;
            //                eBuilding.BuildingId = bid++;
            //                eBuilding.TempletId = templetBuilding.TempletId;
            //                //project.Buildings.Add(eBuilding);
            //                _unitOfWork.BuildingRepository.Insert(eBuilding);
            //            }
            //            else
            //            {
            //                eBuilding = building;
            //                //eBuilding.AppId = buildingDto.AppId;
            //                //eBuilding.BuildDate = buildingDto.BuildDate;
            //                //eBuilding.BuildingName = buildingDto.BuildingName;
            //                //eBuilding.ElevatorRate = buildingDto.ElevatorRate;
            //                //eBuilding.IsElevator = buildingDto.IsElevator;
            //                //eBuilding.LocationCode = buildingDto.LocationCode;
            //                //eBuilding.MaintenanceCode = buildingDto.MaintenanceCode;
            //                //eBuilding.OtherName = buildingDto.OtherName;
            //                //eBuilding.PurposeCode = buildingDto.PurposeCode;
            //                //eBuilding.Remark = buildingDto.Remark;
            //                //eBuilding.StructureCode = buildingDto.StructureCode;
            //                //eBuilding.TotalFloor = buildingDto.TotalFloor;
            //                //eBuilding.UnitsNumber = buildingDto.UnitsNumber;
            //                //eBuilding.X = buildingDto.X;
            //                //eBuilding.Y = buildingDto.Y;
            //                eBuilding.SaveUser = userName;
            //                eBuilding.SaveDateTime = nowTime;

            //                _unitOfWork.BuildingRepository.Update(eBuilding);
            //            }

            //            #region (设置house)
            //            if (buildingDto.HouseDtolist != null && buildingDto.HouseDtolist.Count > 0)
            //            {
            //                foreach (var houseDto in buildingDto.HouseDtolist)
            //                {
            //                    //验证房号字段数据
            //                    if (!CheckHouseObj(houseDto, out message))
            //                    {
            //                        return 0;
            //                    }
            //                    houseDto.HouseId = 0;
            //                    //housecount++;
            //                    House house = Mapper.Map<HouseDto, House>(houseDto);
            //                    //给house各属性赋值
            //                    house.BuildingId = eBuilding.BuildingId;
            //                    house.Valid = 1;
            //                    house.FxtCompanyId = fxtCompanyId;
            //                    house.CityID = cityId;
            //                    if (string.IsNullOrEmpty(houseDto.UnitNo) && string.IsNullOrEmpty(houseDto.HouseNo))
            //                    {
            //                        house.UnitNo = houseDto.HouseName.Replace(houseDto.FloorNo.ToString(), "$");
            //                    }
            //                    else
            //                    {
            //                        house.UnitNo = _houseService.SetHouseUnitNoAndHouseNo(house.UnitNo, houseDto.HouseNo);
            //                    }
            //                    //设置新增实体
            //                    if (houseDto.Valid == 0)
            //                    {
            //                        var hou = _unitOfWork.HouseRepository.GetBy(m => m.AppId == houseDto.AppId && m.BuildingId == eBuilding.BuildingId);
            //                        if (hou != null)
            //                        {
            //                            _unitOfWork.HouseDetailsRepository.Delete(m => m.HouseId == hou.HouseId);
            //                            _unitOfWork.HouseRepository.Delete(m => m.AppId == houseDto.AppId && m.BuildingId == eBuilding.BuildingId);
            //                        }
            //                    }
            //                    else
            //                    {
            //                        House eHouse = _unitOfWork.HouseRepository.GetBy(m => m.AppId == houseDto.AppId && m.BuildingId == eBuilding.BuildingId && m.Valid == 1);
            //                        //eBuilding.Houses == null ? null : eBuilding.Houses.Where(m => m.AppId == houseDto.AppId && m.BuildingId == eBuilding.BuildingId && m.Valid == 1).FirstOrDefault();
            //                        if (eHouse == null)
            //                        {
            //                            house.CreateTime = nowTime;
            //                            house.Creator = userName;
            //                            eHouse = house;
            //                            //if (eBuilding.Houses == null)
            //                            //{
            //                            //    eBuilding.Houses = new List<House>();
            //                            //}
            //                            eHouse.TempletId = templetHouse.TempletId;
            //                            //eBuilding.Houses.Add(eHouse);
            //                            _unitOfWork.HouseRepository.Insert(eHouse);
            //                        }
            //                        else
            //                        {
            //                            int houseid = eHouse.HouseId;
            //                            eHouse = house;
            //                            //eHouse.BuildArea = house.BuildArea;
            //                            //eHouse.EndFloorNo = house.EndFloorNo;
            //                            //eHouse.FloorNo = house.FloorNo;
            //                            //eHouse.FrontCode = house.FrontCode;
            //                            //eHouse.HouseName = house.HouseName;
            //                            //eHouse.HouseTypeCode = house.HouseTypeCode;
            //                            //eHouse.NoiseCode = house.NoiseCode;
            //                            //eHouse.NominalFloor = house.NominalFloor;
            //                            //eHouse.PurposeCode = house.PurposeCode;
            //                            //eHouse.Remark = house.Remark;
            //                            //eHouse.SightCode = house.SightCode;
            //                            //eHouse.Status = house.Status;
            //                            //eHouse.StructureCode = house.StructureCode;
            //                            //eHouse.UnitNo = house.UnitNo;
            //                            //eHouse.VDCode = house.VDCode;
            //                            eHouse.HouseId = houseid;
            //                            eHouse.SaveUser = userName;
            //                            eHouse.SaveDateTime = nowTime;
            //                            _unitOfWork.HouseRepository.Update(eHouse);
            //                        }
            //                        //生成房号
            //                        if (house.EndFloorNo >= house.FloorNo)
            //                        {
            //                            string unitNo = _houseService.GetUnitNoByUnitNoStr(house.UnitNo);
            //                            string houseNo = _houseService.GetHouseNoByUnitNoStr(house.UnitNo);

            //                            _unitOfWork.HouseDetailsRepository.Delete(m => m.HouseId == eHouse.HouseId);

            //                            //if (eHouse.HouseDetails == null)
            //                            //{
            //                            //    eHouse.HouseDetails = new List<HouseDetails>();
            //                            //}

            //                            for (int i = house.FloorNo; i <= house.EndFloorNo; i++)
            //                            {
            //                                //housedetailcount++;
            //                                List<int> cs = new List<int>(){
            //                    SYSCodeManager.HOUSEPURPOSECODE_5,SYSCodeManager.HOUSEPURPOSECODE_6,SYSCodeManager.HOUSEPURPOSECODE_8,SYSCodeManager.HOUSEPURPOSECODE_27
            //                };
            //                                HouseDetails detail = new HouseDetails();
            //                                detail = Mapper.Map<House, HouseDetails>(eHouse);
            //                                detail.FloorNo = i;
            //                                detail.NominalFloor = i.ToString();
            //                                detail.UnitNo = unitNo;
            //                                detail.RoomNo = houseNo;
            //                                detail.CreateTime = nowTime;
            //                                detail.Creator = userName;
            //                                if (detail.PurposeCode.HasValue && cs.Contains(detail.PurposeCode.Value))
            //                                {
            //                                    detail.HouseName = unitNo + houseNo;
            //                                }
            //                                else
            //                                {
            //                                    if (i < 0)
            //                                    {
            //                                        detail.HouseName = "-" + unitNo + -i + houseNo;
            //                                    }
            //                                    else
            //                                    {
            //                                        detail.HouseName = unitNo + i + houseNo;
            //                                    }
            //                                }

            //                                if (i != 0)
            //                                {
            //                                    //eHouse.HouseDetails.Add(detail);
            //                                    _unitOfWork.HouseDetailsRepository.Insert(detail);
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //            #endregion

            //        }

            //        //swbs.Stop();
            //        //log.Info("循环一个楼栋需要" + swbs.Elapsed.TotalMilliseconds + "ms.");
            //    }
            //}
            #endregion

            #region AllotFlow更新值
            //更新任务
            allotFlow.StateCode = SYSCodeManager.STATECODE_4;

            allotFlow.StateDate = nowTime;

            allotFlow.X = projectDto.X;

            allotFlow.Y = projectDto.Y;

            #endregion
            //sw.Stop();
            //TimeSpan ts2 = sw.Elapsed;
            //if (projectDto.BuildingDtolist != null)
            //{
            //    log.Info("楼栋：" + projectDto.BuildingDtolist.Count + "条.");
            //    log.Info("单元室号：" + housecount + "条.");
            //    log.Info("房号：" + housedetailcount + "条.");
            //}
            //log.Info("循环操作总共花费" + ts2.TotalMilliseconds + "ms.");
            //Stopwatch sw2 = new Stopwatch();
            //sw2.Start();

            //_unitOfWork.AllotFlowRepository.Update(allotFlow);

            int r = _unitOfWork.Commit();
            //TimeSpan ts3 = sw2.Elapsed;
            //log.Info("执行数据库操作总共花费" + ts3.TotalMilliseconds + "ms.");

            return r;
        }

        public static bool CheckProjectObj(Project project, out string message)
        {
            message = "";
            //
            if (project.X == null || project.Y == null)
            {
                message = "请输入查勘员经度和纬度";
                return false;
            }
            if (!StringHelp.CheckDecimal(Convert.ToString(project.X)) || !StringHelp.CheckDecimal(Convert.ToString(project.Y)))
            {
                message = "请输入正确的查勘员经度和纬度";
                return false;
            }
            //
            string projectname = string.IsNullOrEmpty(project.ProjectName) ? null : project.ProjectName.DecodeField();
            if (string.IsNullOrEmpty(projectname))
            {
                message = "请输入楼盘名";
                return false;
            }
            //
            if (project.CityID < 1 || project.AreaID < 1)
            {
                message = "请正确输入城市和行政区";
                return false;
            }
            //
            string address = string.IsNullOrEmpty(project.Address) ? null : project.Address.DecodeField();
            if (string.IsNullOrEmpty(address))
            {
                message = "请输入楼盘地址";
                return false;
            }
            //
            if (project.EndDate.HasValue && !StringHelp.CheckIsDate(project.EndDate.Value.ToString().DecodeField())) //(enddate == null || !StringHelp.CheckIsDate(enddate.ToString().DecodeField()))
            {
                message = "请输入正确的竣工时间";
                return false;
            }
            //
            if (project.BuildingArea.HasValue && !StringHelp.CheckDecimal(project.BuildingArea.Value.ToString().DecodeField()))//(buildingarea == null || !StringHelp.CheckDecimal(buildingarea.ToString().DecodeField()))
            {
                message = "请输入正确的建筑面积";
                return false;
            }
            //
            if (project.LandArea.HasValue && !StringHelp.CheckDecimal(project.LandArea.Value.ToString().DecodeField()))//(landarea == null || !StringHelp.CheckDecimal(landarea.ToString().DecodeField()))
            {
                message = "请输入正确的占地面积";
                return false;
            }
            //
            //object cubagerate = jobj["cubagerate"] == null || jobj.Value<string>("cubagerate") == "" ? null : jobj.Value<JValue>("cubagerate").Value;
            //if ( cubagerate != null && !StringHelp.CheckDecimal(cubagerate.ToString().DecodeField()))//(cubagerate == null || !StringHelp.CheckDecimal(cubagerate.ToString().DecodeField()))
            //{
            //    message = "请输入正确的容积率值";
            //    return false;
            //}
            //
            //object greenrate = jobj["greenrate"] == null || jobj.Value<string>("greenrate") == "" ? null : jobj.Value<JValue>("greenrate").Value;
            //if (greenrate != null && !StringHelp.CheckDecimal(greenrate.ToString().DecodeField()))//(greenrate == null || !StringHelp.CheckDecimal(greenrate.ToString().DecodeField()))
            //{
            //    message = "请输入正确的绿化率值";
            //    return false;
            //}
            //
            //string managerprice = jobj["managerprice"] == null ? null : jobj.Value<string>("managerprice").DecodeField();
            //if (string.IsNullOrEmpty(managerprice))
            //{
            //    message = "请输入物业管理费";
            //    return false;
            //}
            //
            //object parkingnumber = jobj["parkingnumber"] == null || jobj.Value<string>("parkingnumber") == "" ? null : jobj.Value<JValue>("parkingnumber").Value;
            //if (parkingnumber != null && !StringHelp.CheckInteger(parkingnumber.ToString().DecodeField()))//(parkingnumber == null || !StringHelp.CheckInteger(parkingnumber.ToString().DecodeField()))
            //{
            //    message = "请输入车位数量";
            //    return false;
            //}
            //
            if (project.TotalNum.HasValue && !StringHelp.CheckInteger(project.TotalNum.Value.ToString().DecodeField()))//(totalnum == null || !StringHelp.CheckInteger(totalnum.ToString().DecodeField()))
            {
                message = "请输入总户数";
                return false;
            }
            //
            if (project.BuildingNum.HasValue && !StringHelp.CheckInteger(project.BuildingNum.Value.ToString().DecodeField()))//(buildingnum == null || !StringHelp.CheckInteger(buildingnum.ToString().DecodeField()))
            {
                message = "请输入总栋数";
                return false;
            }
            //
            //object saledate = jobj["saledate"] == null || jobj.Value<string>("saledate") == "" ? null : jobj.Value<JValue>("saledate").Value;
            //if (saledate != null && !StringHelp.CheckIsDate(saledate.ToString().DecodeField()))//(saledate == null || !StringHelp.CheckIsDate(saledate.ToString().DecodeField()))
            //{
            //    message = "请输入开盘时间";
            //    return false;
            //}
            ////
            //object buildingdate = jobj["buildingdate"] == null || jobj.Value<string>("buildingdate") == "" ? null : jobj.Value<JValue>("buildingdate").Value;
            //if (buildingdate != null && !StringHelp.CheckIsDate(buildingdate.ToString()))//(buildingdate == null || !StringHelp.CheckIsDate(buildingdate.ToString()))
            //{
            //    message = "请输入开工时间";
            //    return false;
            //}
            //
            //if (project.StartDate.HasValue && !StringHelp.CheckIsDate(project.StartDate.Value.ToString().DecodeField()))//(statedate == null || !StringHelp.CheckIsDate(statedate.ToString().DecodeField()))
            //{
            //    message = "请输入采集时间";
            //    return false;
            //}
            return true;
        }

        private bool CheckBuildingObj(BuildingDto building, out string message)
        {
            message = "";
            //
            string buildingname = building.BuildingName == null ? null : building.BuildingName.DecodeField();
            if (string.IsNullOrEmpty(buildingname))
            {
                message = "请输入楼栋名称";
                return false;
            }
            if (buildingname.Length > 150)
            {
                message = "请输入楼栋名称长度不能大于150";
                return false;
            }
            //
            //string doorplate = building.Doorplate == null ? null : building.Doorplate.DecodeField();
            //if (doorplate != null && doorplate.Length > 200)
            //{
            //    message = "请输入门牌号长度不能大于200";
            //    return false;
            //}
            //
            string othername = building.OtherName == null ? null : building.OtherName.DecodeField();
            if (othername != null && othername.Length > 50)
            {
                message = "请输入楼栋别名长度不能大于50";
                return false;
            }
            //
            if (building.StructureCode.HasValue && !StringHelp.CheckInteger(building.StructureCode.Value.ToString().DecodeField()))
            {
                message = "请输入正确的建筑结构";
                return false;
            }
            //
            if (building.LocationCode.HasValue && !StringHelp.CheckInteger(building.LocationCode.Value.ToString().DecodeField()))
            {
                message = "请输入正确的位置";
                return false;
            }
            //
            //if (building.AveragePrice.HasValue && !StringHelp.CheckDecimal(building.AveragePrice.Value.ToString().DecodeField()))
            //{
            //    message = "请输入正确的楼栋均价";
            //    return false;
            //}
            //
            if (building.BuildDate.HasValue && !StringHelp.CheckIsDate(building.BuildDate.Value.ToString().DecodeField()))
            {
                message = "请输入正确的楼栋竣工时间(建筑时间)";
                return false;
            }
            //
            if (building.IsElevator.HasValue && !StringHelp.CheckInteger(building.IsElevator.Value.ToString().DecodeField()))
            {
                message = "请输入正确的楼栋是否带电梯";
                return false;
            }
            //
            string elevatorrate = string.IsNullOrEmpty(building.ElevatorRate) ? null : building.ElevatorRate.DecodeField();
            if (elevatorrate != null && elevatorrate.Length > 50)
            {
                message = "请输入楼栋梯户比字符不能大于50";
                return false;
            }
            //
            //string pricedetail = string.IsNullOrEmpty(building.PriceDetail) ? null : building.PriceDetail.DecodeField();
            //if (pricedetail != null && pricedetail.Length > 500)
            //{
            //    message = "请输入价格系数说明字符不能大于500";
            //    return false;
            //}
            //
            //if (building.SightCode.HasValue && !StringHelp.CheckInteger(building.SightCode.Value.ToString().DecodeField()))
            //{
            //    message = "请输入正确的景观值";
            //    return false;
            //}
            //
            if (!building.TotalFloor.HasValue || !StringHelp.CheckInteger(building.TotalFloor.Value.ToString().DecodeField()))
            {
                message = "请输入楼栋总层数";
                return false;
            }
            return true;
        }

        public static bool CheckHouseObj(HouseDto house, out string message)
        {

            message = "";
            //
            string unitno = string.IsNullOrEmpty(house.UnitNo) ? null : house.UnitNo.DecodeField();
            if (unitno != null && unitno.Length > 20)
            {
                message = "请输入房号单元名称字符长度不能大于20";
                return false;
            }
            //
            //object floorno = jobj["floorno"] == null ? null : jobj.Value<JValue>("floorno").Value;
            //int fno = 0;
            //if (floorno == null || !int.TryParse(floorno.ToString().DecodeField(), out fno))
            //{
            //    message = "请输入起始楼层";
            //    return false;
            //}
            //
            //string housename = jobj["housename"] == null ? null : jobj.Value<string>("housename").DecodeField();
            //if (housename == null)
            //{
            //    message = "请输入房号";
            //    return false;
            //}
            //if (housename.Length > 20)
            //{
            //    message = "请输入房号字符长度不能大于20";
            //    return false;
            //}
            //
            if (!house.FrontCode.HasValue || !StringHelp.CheckInteger(house.FrontCode.Value.ToString().DecodeField()))
            {
                message = "请输入朝向";
                return false;
            }
            //
            if (house.BuildArea.HasValue && !StringHelp.CheckDecimal(house.BuildArea.Value.ToString().DecodeField()))
            {
                message = "请输入正确的面积格式";
                return false;
            }
            //
            if (house.HouseTypeCode.HasValue && !StringHelp.CheckInteger(house.HouseTypeCode.Value.ToString().DecodeField()))
            {
                message = "请输入正确的户型";
                return false;
            }
            //
            if (house.SightCode.HasValue && !StringHelp.CheckInteger(house.SightCode.Value.ToString().DecodeField()))
            {
                message = "请输入景观";
                return false;
            }
            return true;
        }

        /// <summary>
        /// 导入数据（从数据中心导入数据到踩盘助手）
        /// </summary>
        /// <param name="projectList">数据中心楼盘</param>
        /// <param name="userName"></param>
        /// <param name="userTrueName"></param>
        /// <param name="message"></param>
        public bool ImputProject(List<DATProject> projectList, string userName, string userTrueName, out string message)
        {
            message = "";

            //模板
            var templet = _unitOfWork.TempletRepository.GetBy(m => m.DatType == SYSCodeManager.DATATYPECODE_1 && m.IsCurrent == true);

            if (templet == null)
            {
                message = "失败，未设置默认楼盘模板。";

                return false;
            }
            int pid = 0;
            int tid = 0;
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
                    message += item.projectname + ",";
                    continue;//任务重复
                }

                DateTime now = DateTime.Now;
                //楼盘
                Project project = Mapper.Map<DATProject, Project>(item);
                project.FxtProjectId = item.projectid;
                project.FxtCompanyId = item.fxtcompanyid;
                project.ProjectId = 0;
                project.Status = SYSCodeManager.STATECODE_1;
                project.CreateTime = now;
                project.X = item.projectx;
                project.Y = item.projecty;
                project.Valid = 1;
                project.TempletId = templet.TempletId;
                project.ProjectId = pid++;
                _unitOfWork.ProjectRepository.Insert(project);

                if (item.buildinglist != null && item.buildinglist.Count > 0)
                {
                    //模板
                    var templet2 = _unitOfWork.TempletRepository.GetBy(m => m.DatType == SYSCodeManager.DATATYPECODE_2 && m.IsCurrent == true);

                    if (templet2 == null)
                    {
                        message = "失败，未设置默认楼盘模板。";

                        return false;
                    }

                    //project.Buildings = project.Buildings == null ? new List<Building>() : project.Buildings;

                    //楼栋
                    foreach (var db in item.buildinglist)
                    {
                        var existsBuilding = _unitOfWork.BuildingRepository.GetBy(m => m.FxtBuildingId == db.buildingid && m.Valid == 1);

                        Building building = Mapper.Map<DATBuilding, Building>(db);
                        building.FxtBuildingId = db.buildingid;
                        building.ProjectId = project.ProjectId;
                        building.TempletId = templet2.TempletId;
                        building.Valid = 1;
                        if (existsBuilding == null)
                        {
                            building.CreateTime = now;
                            building.Creator = userName;
                            _unitOfWork.BuildingRepository.Insert(building);
                        }
                        else
                        {
                            building.SaveDateTime = now;
                            building.SaveUser = userName;
                            _unitOfWork.BuildingRepository.Update(building);
                        }

                        //project.Buildings.Add(building);
                    }
                }

                //_unitOfWork.Commit();
                //任务
                var allot = new AllotFlow()
                {
                    CityId = project.CityID,
                    CreateTime = now,
                    DatType = SYSCodeManager.DATATYPECODE_1,
                    FxtCompanyId = project.FxtCompanyId.Value,
                    UserName = userName,
                    UserTrueName = userTrueName,
                    StateCode = SYSCodeManager.STATECODE_1,
                    DatId = project.ProjectId,
                    StateDate = now,
                    Id = tid++
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
                    UserName = userName,
                    TrueName = userTrueName,
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

            return _unitOfWork.Commit() > 0;
        }

        /// <summary>
        /// 生成楼盘模板
        /// </summary>
        /// <param name="project">楼盘</param>
        /// <param name="templet">模板</param>
        /// <returns></returns>
        public TempletDto CreateProjectTempletDto(Project project, Templet templet)
        {
            TempletDto templetDto = new TempletDto();

            templetDto.TempletId = templet.TempletId;

            templetDto.TempletName = templet.TempletName;

            templetDto.FieldGroups = new List<FieldGroupDto>();

            //获取实体属性
            var propertys = project.GetType().GetProperties();

            foreach (var fieldGroup in templet.FieldGroups)
            {
                FieldGroupDto fieldGroupDto = new FieldGroupDto();

                fieldGroupDto.FieldGroupName = fieldGroup.FieldGroupName;

                fieldGroupDto.Fields = new List<FieldDto>();

                if (fieldGroup.Fields != null && fieldGroup.Fields.Count > 0)
                {
                    foreach (var field in fieldGroup.Fields)
                    {
                        FieldDto fieldDto = new FieldDto();
                        fieldDto.EdiTextType = field.EdiTextType;
                        fieldDto.FieldName = field.FieldName;
                        fieldDto.FieldType = field.FieldType;
                        fieldDto.Title = field.Title;
                        fieldDto.MaxLength = field.MaxLength;
                        fieldDto.MinLength = field.MinLength;
                        fieldDto.IsRequired = field.IsRequired;
                        fieldDto.IsNull = field.IsNull;

                        switch (field.FieldType)
                        {
                            case 1:
                                fieldDto.Type = "E";
                                break;
                            case 2:
                                fieldDto.Type = "T";
                                break;
                            case 3:
                                fieldDto.Type = "R";
                                break;
                            case 5:
                                fieldDto.Type = "C";
                                break;
                            case 6:
                                fieldDto.Type = "DT";
                                break;
                            default:
                                break;
                        }

                        List<FxtDataAcquisition.Domain.Models.SYSCode> codes = new List<FxtDataAcquisition.Domain.Models.SYSCode>();

                        switch (field.FieldName)
                        {
                            case "PurposeCode":
                                codes = _codeService.PurposeCodeManager();
                                break;
                            case "RightCode":
                                codes = _codeService.RightCodeManager();
                                break;
                            case "AppendageClass":
                                codes = _codeService.LevelManager();
                                break;
                            case "ManagerQuality":
                                codes = _codeService.LevelManager();
                                break;
                            case "ParkingStatus":
                                codes = _codeService.GetAllParkingStatusList();
                                break;
                            case "BuildingTypeCode":
                                codes = _codeService.BuildingTypeCodeManager();
                                break;
                            case "BuildingQuality":
                                codes = _codeService.LevelManager();
                                break;
                            case "HousingScale":
                                codes = _codeService.HousingScaleCodeManager();
                                break;
                            case "PlanPurpose":
                                codes = _codeService.PlanPurposeCodeManager();
                                break;
                            default:
                                break;
                        }

                        //取值
                        var property = propertys.Where(pInfo => pInfo.Name == field.FieldName).FirstOrDefault();

                        if (property != null)
                        {
                            var value = property.GetValue(project);

                            if (value != null)
                            {
                                if (value is DateTime)
                                {
                                    fieldDto.Value = DateTime.Parse(value.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                else
                                {
                                    fieldDto.Value = value.ToString();

                                    if (codes != null && codes.Count > 0)
                                    {
                                        //选项值
                                        if (fieldDto.FieldType == 3)
                                        {
                                            var code = codes.FirstOrDefault(m => m.Code == int.Parse(fieldDto.Value));

                                            fieldDto.Value = code == null ? "" : code.CodeName;
                                        }
                                        else if (fieldDto.FieldType == 5)
                                        {
                                            //多选
                                            var vs = fieldDto.Value.Split(',').ConvertToIntList();

                                            var codeList = codes.Where(m => vs.Contains(m.Code));

                                            if (codeList != null)
                                            {
                                                var vsname = codeList.Select(m => m.CodeName);

                                                fieldDto.Value = string.Join(",", vsname);
                                            }
                                        }

                                        fieldDto.Choise = codes.Select(m => m.CodeName).ToList();
                                    }
                                }
                            }
                        }

                        fieldGroupDto.Fields.Add(fieldDto);
                    }
                }

                templetDto.FieldGroups.Add(fieldGroupDto);
            }

            return templetDto;
        }
    }
}
