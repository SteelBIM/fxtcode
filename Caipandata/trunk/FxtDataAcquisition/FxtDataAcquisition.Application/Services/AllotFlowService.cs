namespace FxtDataAcquisition.Application.Services
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Linq;
    using System.Configuration;
    using System.Collections.Generic;

    using log4net;
    using AutoMapper;

    using FxtDataAcquisition.Common;
    using FxtDataAcquisition.Domain;
    using FxtDataAcquisition.Domain.DTO;
    using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
    using FxtDataAcquisition.Domain.Models;
    using FxtDataAcquisition.Application.Interfaces;
    using FxtDataAcquisition.FxtAPI.FxtDataCenter.Manager;

    public class AllotFlowService : IAllotFlowService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AllotFlowService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public int SetAllotStatus(string userName, int cityId, long[] allotIds, int oldCode, int stateCode, string remark
            , string userTrueName = "", string surveyUserName = "", string surveyUserTrueName = "", string checkRemark = "")
        {
            //var allotFlows = _unitOfWork.AllotFlowRepository.Get(m => allotIds.Contains(m.Id) && (m.UserName == userName || m.SurveyUserName == userName));
            var allotFlows = _unitOfWork.AllotFlowRepository.Get(m => allotIds.Contains(m.Id));
            //原状态错误
            var checkState = allotFlows.Where(m => m.StateCode != oldCode);
            if (checkState != null && checkState.Count() > 0)
            {
                return 0;
            }

            foreach (var allotFlow in allotFlows)
            {

                //var project = _unitOfWork.ProjectRepository.GetBy(m => m.ProjectId == allotFlow.DatId && m.CityID == cityId);

                DateTime time = DateTime.Now;
                //更新任务
                allotFlow.StateCode = stateCode;
                allotFlow.StateDate = time;

                //更新楼盘
                allotFlow.Project.Status = stateCode;
                allotFlow.Project.SaleDate = time;
                allotFlow.Project.SaveUser = userName;
                //更新任务记录
                AllotSurvey survey = new AllotSurvey();
                survey.AllotId = allotFlow.Id;
                survey.CityId = allotFlow.CityId;
                survey.FxtCompanyId = allotFlow.FxtCompanyId;
                survey.CreateDate = time;
                survey.StateDate = time;
                survey.StateCode = stateCode;
                survey.UserName = userName;
                survey.TrueName = userTrueName;
                survey.Remark = remark;

                //撤销任务
                if (stateCode == SYSCodeManager.STATECODE_1)
                {
                    //allotFlow.UserName = "";
                    //allotFlow.UserTrueName = "";
                    allotFlow.SurveyUserName = "";
                    allotFlow.SurveyUserTrueName = "";
                }
                //分配任务
                if (oldCode == SYSCodeManager.STATECODE_1 && stateCode == SYSCodeManager.STATECODE_2)
                {
                    allotFlow.UserName = userName;
                    allotFlow.UserTrueName = userTrueName;
                    allotFlow.SurveyUserName = surveyUserName;
                    allotFlow.SurveyUserTrueName = surveyUserTrueName;
                }

                _unitOfWork.AllotFlowRepository.Update(allotFlow);
                //_unitOfWork.ProjectRepository.Update(project);
                _unitOfWork.AllotSurveyRepository.Insert(survey);

            }
            return _unitOfWork.Commit();
        }

        /// <summary>
        /// 新增任务
        /// </summary>
        /// <param name="project">楼盘</param>
        /// <param name="developersCompany">开发商</param>
        /// <param name="managerCompany">物管公司</param>
        /// <returns>任务已存在返回任务状态，1.成功，0失败,-1未设置模板</returns>
        public ProjectAllotFlowSurveyDto AddAllot(Project project, string developersCompany, string managerCompany, string remark, int cityId, int fxtCompanyId, string userName, string userTrueName, int status)
        {
            ProjectAllotFlowSurveyDto dto = new ProjectAllotFlowSurveyDto();

            DateTime now = DateTime.Now;

            //模板
            var templet = _unitOfWork.TempletRepository.GetBy(m => m.FxtCompanyId == fxtCompanyId && m.DatType == SYSCodeManager.DATATYPECODE_1 && m.IsCurrent == true && m.Vaild == 1);

            if (templet == null)
            {
                dto.AllotState = -1;

                return dto;
            }

            project.CityID = cityId;
            project.Status = status;
            project.FxtCompanyId = fxtCompanyId;
            project.CreateTime = now;
            project.Creator = userName;
            project.Valid = 1;
            project.TempletId = templet.TempletId;
            project = _unitOfWork.ProjectRepository.Insert(project);
            _unitOfWork.Commit();
            //任务
            var allot = new AllotFlow()
            {
                CityId = project.CityID,
                CreateTime = now,
                DatType = SYSCodeManager.DATATYPECODE_1,
                FxtCompanyId = project.FxtCompanyId.Value,
                UserName = userName,
                UserTrueName = userTrueName,
                StateCode = status,
                DatId = project.ProjectId,
                StateDate = now,
                SurveyUserName = status == SYSCodeManager.STATECODE_2 ? userName : "",
                SurveyUserTrueName = status == SYSCodeManager.STATECODE_2 ? userTrueName : ""
            };
            allot = _unitOfWork.AllotFlowRepository.Insert(allot);
            _unitOfWork.Commit();
            //任务记录
            var allotSurvey = new AllotSurvey()
            {
                AllotId = allot.Id,
                CityId = allot.CityId,
                FxtCompanyId = fxtCompanyId,
                CreateDate = now,
                StateCode = status,
                StateDate = now,
                UserName = userName,
                TrueName = userTrueName,
                Remark = remark
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
            if (!string.IsNullOrEmpty(developersCompany))
            {
                var developcompany = _unitOfWork.P_CompanyRepository.Get(m => m.CompanyName == developersCompany && m.CompanyType == SYSCodeManager.COMPANYTYPECODE_1
                    && m.ProjectId == project.ProjectId && m.CityId == project.CityID).FirstOrDefault();
                if (developcompany == null)
                {
                    developcompany = new PCompany();
                    developcompany.CityId = project.CityID;
                    developcompany.CompanyName = developersCompany;
                    developcompany.CompanyType = SYSCodeManager.COMPANYTYPECODE_1;
                    developcompany.ProjectId = project.ProjectId;
                    _unitOfWork.P_CompanyRepository.Insert(developcompany);
                }
            }
            //物管公司
            if (!string.IsNullOrEmpty(managerCompany))
            {
                var managercompany = _unitOfWork.P_CompanyRepository.Get(m => m.CompanyName == managerCompany && m.CompanyType == SYSCodeManager.COMPANYTYPECODE_4
                    && m.ProjectId == project.ProjectId && m.CityId == project.CityID).FirstOrDefault();
                if (managercompany == null)
                {
                    managercompany = new PCompany();
                    managercompany.CityId = project.CityID;
                    managercompany.CompanyName = managerCompany;
                    managercompany.CompanyType = SYSCodeManager.COMPANYTYPECODE_4;
                    managercompany.ProjectId = project.ProjectId;
                    _unitOfWork.P_CompanyRepository.Insert(managercompany);
                }
            }

            dto.AllotId = allot.Id;
            dto.ProjectId = project.ProjectId;
            dto.AllotState = _unitOfWork.Commit() > 0 ? 1 : 0;
            return dto;
        }


        public Project ExistsAllot(Project project)
        {
            //行政区 + 楼盘名称||已有的projectid||没有入库
            var exists = _unitOfWork.ProjectRepository.Get(m =>
                    (
                        (m.ProjectName == project.ProjectName && m.AreaID == project.AreaID)
                        || m.ProjectId == project.ProjectId
                    )
                    && m.Status != SYSCodeManager.STATECODE_10 && m.Valid == 1
                ).FirstOrDefault();
            return exists;
        }

        public int ImportToDataCenter(long allowId, int cityId, int companyId, string userName, string userTrueName, string signName, List<Apps> appList, out string message)
        {
            AllotFlow allot = _unitOfWork.AllotFlowRepository.GetById(allowId);
            if (allot == null || allot.StateCode != SYSCodeManager.STATECODE_8)
            {
                message = "入库失败:该任务不存在或者该任务状态不为审核通过";
                return 0;
            }
            if (allot.DatType == SYSCodeManager.DATATYPECODE_1)
            {
                //Project proj = _unitOfWork.ProjectRepository.GetBy(m => m.ProjectId == allot.DatId && m.CityID == cityId);

                //ProjectDto project = Mapper.Map<Project, ProjectDto>(proj);

                //List<PCompany> lnkpc = _unitOfWork.P_CompanyRepository.Get(m => m.ProjectId == allot.Project.ProjectId && m.CityId == cityId).ToList();

                //List<PAppendage> lnkpa = _unitOfWork.P_AppendageRepository.Get(m => m.ProjectId == allot.Project.ProjectId && m.CityId == cityId).ToList();

                //if (lnkpa != null && lnkpa.Count > 0)
                //{
                //    lnkpa = lnkpa.Select(m => new PAppendage()
                //    {
                //        Address = m.Address,
                //        AppendageCode = m.AppendageCode,
                //        Area = m.Area,
                //        CityId = m.CityId,
                //        ClassCode = m.ClassCode,
                //        Distance = m.Distance,
                //        Id = m.Id,
                //        IsInner = m.IsInner,
                //        P_AName = m.P_AName,
                //        ProjectId = m.ProjectId,
                //        Uid = m.Uid,
                //        X = m.X,
                //        Y = m.Y
                //    }).ToList();
                //}

                //var bList = _unitOfWork.BuildingRepository.Get(m => m.ProjectId == proj.ProjectId && m.CityID == cityId);
                //var hList = _unitOfWork.HouseDetailsRepository.Get();
                //List<HouseDetails> houseList = (from b in bList
                //                                join h in hList on b.BuildingId equals h.BuildingId
                //                                select h).ToList();
                //List<Building> buildingList = bList.ToList();
                
                List<Building> resultBuildings = new List<Building>();//返回的楼栋ids
                int fxtprojectId = DataCenterProjectApi.ImportProjectData(allot.Project, ref resultBuildings, userName, signName, appList, out message);
                if (fxtprojectId <= 0)
                {
                    message = "入库失败:导入楼盘信息异常";
                    return 0;
                }

                //_unitOfWork.ProjectRepository.Update(proj);
                //上传照片
                List<PPhoto> photoList = _unitOfWork.P_PhotoRepository.Get(m => m.ProjectId == allot.Project.ProjectId && m.CityId == cityId && m.FxtCompanyId == companyId).ToList();
                foreach (PPhoto pObj in photoList)
                {
                    //FileStream fStream = new FileStream(System.Web.HttpContext.Current.Server.MapPath(pObj.Path), FileMode.Open, FileAccess.Read);
                    var path = ConfigurationManager.AppSettings["OssDownload"] + pObj.Path;
                    using (WebClient client = new WebClient())
                    {

                        var data = client.DownloadData(path);
                        //BinaryReader bReader = new BinaryReader(fStream);//将文件了加载成二进制数据
                        //long length = bReader.BaseStream.Length;//当前文件的总大小
                        ////创建一个用于存储要上传文件内容的字节对象
                        //byte[] data = new byte[length];
                        ////将流中读取指定字节数加载到到字节对象data
                        //bReader.Read(data, 0, Convert.ToInt32(length));
                        pObj.PhotoTypeCode = pObj.PhotoTypeCode.HasValue ? pObj.PhotoTypeCode.Value : 0;
                        long fxtbuildingid = 0;
                        //楼栋图片
                        if (pObj.BuildingId.HasValue && pObj.BuildingId > 0)
                        {
                            var nowBuilding = resultBuildings.Where(m => m.BuildingId == pObj.BuildingId).FirstOrDefault();
                            if (nowBuilding != null)
                            {
                                fxtbuildingid = Convert.ToInt64(nowBuilding.FxtBuildingId);
                            }
                        }
                        if (DataCenterProjectApi.AddProjectPhoto(fxtprojectId, fxtbuildingid, cityId, pObj.PhotoTypeCode.Value, new FileInfo(System.Web.HttpContext.Current.Server.MapPath(pObj.Path)).Name, pObj.PhotoName, data, userName, signName, appList, out message) != 1)
                        {
                            message = "入库失败:上传楼盘照片失败(id:" + pObj.Id + ")";
                            return 0;
                        }
                    }
                }

                if (resultBuildings != null && resultBuildings.Count > 0)
                {
                    foreach (var item in resultBuildings)
                    {
                        var building = _unitOfWork.BuildingRepository.GetById(item.BuildingId);
                        if (building.FxtBuildingId != item.FxtBuildingId)
                        {
                            building.FxtBuildingId = item.FxtBuildingId;
                            _unitOfWork.BuildingRepository.Update(building);
                        }
                    }
                }

                //设置为已入库
                allot.Project.FxtProjectId = fxtprojectId;
                allot.Project.Status = SYSCodeManager.STATECODE_10;
            }
            //设置为已入库
            allot.StateCode = SYSCodeManager.STATECODE_10;
            allot.StateDate = DateTime.Now;
            _unitOfWork.AllotFlowRepository.Update(allot);
            //记录日志
            _unitOfWork.AllotSurveyRepository.Insert(new AllotSurvey()
            {
                AllotId = allowId,
                CityId = cityId,
                FxtCompanyId = companyId,
                UserName = userName,
                StateCode = SYSCodeManager.STATECODE_10,
                StateDate = DateTime.Now,
                CreateDate = DateTime.Now,
                TrueName = userTrueName,
                Remark = "<span class=\"red\">入库</span>"
            });
            message = "入库成功！";
            return _unitOfWork.Commit() > 0 ? 1 : 0;
        }



    }
}
