using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Infrastructure.Common.Common;
using FXT.DataCenter.Infrastructure.Common.Dictionary;
using FXT.DataCenter.Infrastructure.Common.NPOI;


namespace FXT.DataCenter.WCF.Services
{
    public partial class ExcelUpload
    {
        public void SampleProjectExcelUpload(int cityid, int fxtcompanyid, string filePath, string userid, string taskName)
        {
            var taskId = 0;

            try
            {
                //在任务列表创建一条记录
                var task = new DAT_ImportTask()
                {
                    TaskName = taskName,
                    ImportType = SYS_Code_Dict.批量导入类型.样本楼盘,
                    CityID = cityid,
                    FXTCompanyId = fxtcompanyid,
                    CreateDate = DateTime.Now,
                    Creator = userid,
                    IsComplete = 0,
                    SucceedNumber = 0,
                    DataErrNumber = 0,
                    NameErrNumber = 0,
                    FilePath = ""
                };
                taskId = _importTask.AddTask(task);

                var excelHelper = new ExcelHandle(filePath);

                #region 样本楼盘

                List<DAT_SampleProject> listTrue;//正确数据
                DataTable dtError;//格式错误数据
                var data = excelHelper.ExcelToDataTable("样本楼盘", true);
                DataFilter(cityid, fxtcompanyid, data, out listTrue, out dtError);

                #endregion

                #region 关联楼盘

                List<DAT_SampleProject_Weight> listTrue1;//正确数据
                DataTable dtError1;//格式错误数据
                var data1 = excelHelper.ExcelToDataTable("关联楼盘", true);
                DataFilter(cityid, fxtcompanyid, data1, out listTrue1, out dtError1);

                #endregion
                
                //错误数据写入Excel
                var fileNamePath = string.Empty;
                if (dtError.Rows.Count > 0 || dtError1.Rows.Count > 0)
                {
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "样本楼盘错误数据.xlsx";
                    var folder = MapPath("NeedHandledFiles/FailureData/" + fxtcompanyid);
                    fileNamePath = Path.Combine(folder, fileName);
                    excelHelper.CreateExcel(dtError, dtError1, "样本楼盘", "关联楼盘", fileNamePath, folder);
                }

                //数据插入/修改失败数量
                var failureNum = 0;

                //样本楼盘
                foreach (var item in listTrue)
                {
                    var id = _projectSample.SampleProjectIsExit(item.ProjectId, item.CityId, item.FxtCompanyId);
                    if (id > 0)//存在该楼盘名称则更新该楼盘信息
                    {
                        item.Id = id;
                        var modifyResult = _projectSample.UpdateProjectSample(item);
                        if (modifyResult <= 0) failureNum = failureNum + 1;
                    }
                    else//新增该楼栋信息
                    {
                        var insertResult = _projectSample.AddProjectSample(item);
                        if (insertResult <= 0) failureNum = failureNum + 1;
                    }
                }

                //关联楼盘
                foreach (var item in listTrue1)
                {
                    var sampleProjectWeight = _projectSample.GetProjectSampleWeight(item.ProjectId, item.CityId, item.FxtCompanyId, item.BuildingTypeCode).FirstOrDefault();
                    if (sampleProjectWeight == null)
                    {
                        var insertResult = _projectSample.AddProjectSampleWeight(item);
                        if (insertResult <= 0) failureNum = failureNum + 1;
                    }
                    else
                    {
                        failureNum = failureNum + 1;
                    }
                }

                //更新任务状态
                //更新任务状态
                //var tmpRootDir = AppDomain.CurrentDomain.BaseDirectory;//获取程序根目录
                //var relativePath = fileNamePath.Replace(tmpRootDir, ""); //转换成相对路径
                var indexPath = fileNamePath.IndexOf("NeedHandledFiles");
                var relativePath = string.Empty;
                if (indexPath >= 0)
                {
                    relativePath = fileNamePath.Substring(indexPath);
                    relativePath = relativePath.Replace(@"\", @"/");
                }
                _importTask.UpdateTask(taskId, (listTrue.Count + listTrue1.Count) - failureNum, (dtError.Rows.Count + dtError1.Rows.Count), 0, relativePath, 1);
            }
            catch (Exception ex)
            {
                _importTask.UpdateTask(taskId, 0, 0, 0, "", -1);
                LogHelper.WriteLog("SampleProjectExcelUpload", "", userid, cityid, fxtcompanyid, ex);
            }
        }

        private void DataFilter(int cityId, int fxtCompanyId, DataTable dt, out List<DAT_SampleProject> listTrue, out DataTable dtError)
        {
            listTrue = new List<DAT_SampleProject>();
            dtError = new DataTable();

            foreach (DataColumn column in dt.Columns)
                dtError.Columns.Add(column.Caption);


            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var dp = new DAT_SampleProject();
                var dr = dtError.NewRow();
                dp.FxtCompanyId = fxtCompanyId;
                dp.CityId = cityId;
                var isError = false;

                var areaId = 0;
                var areaName = dt.Rows[i]["*行政区"].ToString().Trim();
                areaId = GetAreaId(cityId, areaName);
                dp.AreaId = areaId;
                dr["*行政区"] = areaName;
                if (string.IsNullOrEmpty(areaName) || areaId <= 0)
                {
                    isError = true;
                    dr["*行政区"] = (string.IsNullOrEmpty(areaName) ? "必填" : areaName) + "#error";
                }

                var projectName = dt.Rows[i]["*样本楼盘"].ToString().Trim();
                var projectIds = ProjectIdByName(fxtCompanyId, cityId, areaId, projectName).Select(m => m.projectid);
                var projectId = projectIds.FirstOrDefault();
                dp.ProjectId = projectId;
                dr["*样本楼盘"] = projectName;
                if (string.IsNullOrEmpty(projectName) || projectName.Length > 80 || projectId <= 0)
                {
                    isError = true;
                    dr["*样本楼盘"] = (string.IsNullOrEmpty(projectName) ? "必填" : projectName) + "#error";
                }

                var purposeName = dt.Rows[i]["*土地用途"].ToString().Trim();
                var purposeCode = GetCodeByName(purposeName, SYS_Code_Dict._土地用途);
                dp.PurposeCode = purposeCode;
                dr["*土地用途"] = purposeName;
                if (purposeName.Length < 2 || purposeCode == -1)
                {
                    isError = true;
                    dr["*土地用途"] = (string.IsNullOrEmpty(purposeName) ? "必填" : purposeName) + "#error";
                }

                var buildingTypeName = dt.Rows[i]["*建筑类型"].ToString().Trim();
                var buildingTypeCode = GetCodeByName(buildingTypeName, SYS_Code_Dict._建筑类型);
                dp.BuildingTypeCode = buildingTypeCode;
                dr["*建筑类型"] = buildingTypeName;
                if (!string.IsNullOrEmpty(buildingTypeName) && buildingTypeCode == -1)
                {
                    isError = true;
                    dr["*建筑类型"] = (string.IsNullOrEmpty(buildingTypeName) ? "必填" : buildingTypeName) + "#error";
                }
                
                var startDate = dt.Rows[i]["*竣工日期"].ToString().Trim();
                dp.BuildingDate = Convert.ToDateTime(TryParseHelper.StrToDateTime(startDate, DateTime.Now.ToShortDateString()));
                dr["*竣工日期"] = startDate;
                if (!string.IsNullOrEmpty(startDate) && (DateTime?)TryParseHelper.StrToDateTime(startDate) == null)
                {
                    isError = true;
                    dr["*竣工日期"] = (string.IsNullOrEmpty(startDate) ? "必填" : startDate) + "#error";
                }

                var subAreaName = dt.Rows[i]["片区"].ToString().Trim();
                var subAreaId = SubAreaIdByName(subAreaName, areaId);
                dp.SubAreaId = subAreaId;
                dr["片区"] = subAreaName;
                if (subAreaName.Length > 1 && subAreaId <= 0)
                {
                    isError = true;
                    dr["片区"] = subAreaName + "#error";
                }

                if (isError)
                {
                    dtError.Rows.Add(dr);
                }
                else
                {
                    listTrue.Add(dp);
                }
            }
        }

        private void DataFilter(int cityId, int fxtCompanyId, DataTable dt, out List<DAT_SampleProject_Weight> listTrue, out DataTable dtError)
        {
            listTrue = new List<DAT_SampleProject_Weight>();
            dtError = new DataTable();

            foreach (DataColumn column in dt.Columns)
                dtError.Columns.Add(column.Caption);


            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var dp = new DAT_SampleProject_Weight();
                var dr = dtError.NewRow();
                dp.FxtCompanyId = fxtCompanyId;
                dp.CityId = cityId;
                var isError = false;

                var sampleareaId = 0;
                var sampleAreaName = dt.Rows[i]["*样本楼盘行政区"].ToString().Trim();
                sampleareaId = GetAreaId(cityId, sampleAreaName);
                dr["*样本楼盘行政区"] = sampleAreaName;
                if (string.IsNullOrEmpty(sampleAreaName) || sampleareaId == -1)
                {
                    isError = true;
                    dr["*样本楼盘行政区"] = (string.IsNullOrEmpty(sampleAreaName) ? "必填" : sampleAreaName) + "#error";
                }
                
                var projectName = dt.Rows[i]["*样本楼盘"].ToString().Trim();
                var projectIds = ProjectIdByName(fxtCompanyId, cityId, sampleareaId, projectName).Select(m => m.projectid);
                var projectId = projectIds.FirstOrDefault();
                dp.SampleProjectId = projectId;
                dr["*样本楼盘"] = projectName;
                if (string.IsNullOrEmpty(projectName) || projectName.Length > 80 || projectId <= 0)
                {
                    isError = true;
                    dr["*样本楼盘"] = (string.IsNullOrEmpty(projectName) ? "必填" : projectName) + "#error";
                }

                var areaId = 0;
                var areaName = dt.Rows[i]["*关联楼盘行政区"].ToString().Trim();
                areaId = GetAreaId(cityId, areaName);
                dr["*关联楼盘行政区"] = areaName;
                if (string.IsNullOrEmpty(areaName) || areaId <= 0)
                {
                    isError = true;
                    dr["*关联楼盘行政区"] = (string.IsNullOrEmpty(areaName) ? "必填" : areaName) + "#error";
                }
                
                var projectName1 = dt.Rows[i]["*关联楼盘"].ToString().Trim();
                var projectIds1 = ProjectIdByName(fxtCompanyId, cityId, areaId, projectName1).Select(m => m.projectid);
                var projectId1 = projectIds1.FirstOrDefault();
                dp.ProjectId = projectId1;
                dr["*关联楼盘"] = projectName1;
                if (string.IsNullOrEmpty(projectName1) || projectName1.Length > 80 || projectId1 <= 0)
                {
                    isError = true;
                    dr["*关联楼盘"] = (string.IsNullOrEmpty(projectName1) ? "必填" : projectName1) + "#error";
                }

                var weight = dt.Rows[i]["*系数"].ToString().Trim();
                dp.Weight = (decimal)TryParseHelper.StrToDecimal(weight);
                dr["*系数"] = weight;
                if (string.IsNullOrEmpty(weight) || TryParseHelper.StrToDecimal(weight) == null)
                {
                    isError = true;
                    dr["*系数"] = (string.IsNullOrEmpty(weight) ? "必填" : weight) + "#error";
                }

                var buildingTypeName = dt.Rows[i]["*建筑类型"].ToString().Trim();
                var buildingTypeCode = GetCodeByName(buildingTypeName, SYS_Code_Dict._建筑类型);
                dp.BuildingTypeCode = buildingTypeCode;
                dr["*建筑类型"] = buildingTypeName;
                if (!string.IsNullOrEmpty(buildingTypeName) && buildingTypeCode == -1)
                {
                    isError = true;
                    dr["*建筑类型"] = (string.IsNullOrEmpty(buildingTypeName) ? "必填" : buildingTypeName) + "#error";
                }

                if (isError)
                {
                    dtError.Rows.Add(dr);
                }
                else
                {
                    listTrue.Add(dp);
                }
            }
        }
    }
}
