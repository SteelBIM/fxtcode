using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Infrastructure.Common.Common;
using FXT.DataCenter.Infrastructure.Common.Dictionary;
using FXT.DataCenter.Infrastructure.Common.NPOI;


namespace FXT.DataCenter.WCF.Services
{
    public partial class ExcelUpload
    {
        public void ProjectPeiTaoExcelUpload(int cityid, int fxtcompanyid, string filePath, string userid, string taskName)
        {
            var taskId = 0;

            try
            {
                //在任务列表创建一条记录
                var task = new DAT_ImportTask()
                {
                    TaskName = taskName,
                    ImportType = SYS_Code_Dict.批量导入类型.住宅项目配套,
                    CityID = cityid,
                    FXTCompanyId = fxtcompanyid,
                    CreateDate = DateTime.Now,
                    Creator = userid,
                    IsComplete = 0,
                    SucceedNumber = 0,
                    DataErrNumber = 0,
                    NameErrNumber = 0,
                    FilePath = "",
                    Steps = 1
                };
                taskId = _importTask.AddTask(task);

                var excelHelper = new ExcelHandle(filePath);
                var data = excelHelper.ExcelToDataTable("Sheet1", true);

                var integer = Math.Floor(Convert.ToDouble(data.Rows.Count / 50));

                List<LNK_P_Appendage> listTrue;//正确数据
                DataTable dtError;//格式错误数据
                DataFilter(taskId, integer, cityid, fxtcompanyid, out listTrue, out dtError, data);

                //错误数据写入Excel
                var fileNamePath = string.Empty;
                if (dtError.Rows.Count > 0)
                {
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "住宅项目配套错误数据.xlsx";
                    var folder = MapPath("NeedHandledFiles/FailureData/" + fxtcompanyid);
                    fileNamePath = Path.Combine(folder, fileName);
                    excelHelper.CreateExcel(dtError, fileNamePath, folder);

                }

                //正确数据写入表中
                var failureNum = 0;
                var index4True = 0;//用于统计进度
                foreach (var item in listTrue)
                {
                    #region 更新进度
                    index4True++;
                    if (integer > 0)
                    {
                        if (index4True % integer == 0)
                        {
                            _importTask.TaskStepsIncreased(taskId);
                        }
                    }
                    #endregion

                    var result = _datProject.AddProjectAppendage(item);
                    if (result <= 0) failureNum = failureNum + 1;
                }

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
                _importTask.UpdateTask(taskId, listTrue.Count - failureNum, dtError.Rows.Count, 0, relativePath, 1, 100);
            }

            catch (Exception ex)
            {
                _importTask.UpdateTask(taskId, 0, 0, 0, "", -1);
                LogHelper.WriteLog("ProjectPeiTaoUpload", "", userid, cityid, fxtcompanyid, ex);
            }
        }

        private void DataFilter(int taskId, double integer, int cityId, int fxtCompanyId, out List<LNK_P_Appendage> listTrue, out DataTable dtError, DataTable dt)
        {
            listTrue = new List<LNK_P_Appendage>();
            dtError = new DataTable();
            foreach (DataColumn column in dt.Columns)
                dtError.Columns.Add(column.Caption);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var isSkip = false;
                var peiTao = new LNK_P_Appendage { cityid = cityId };
                var dr = dtError.NewRow();

                var areaId = 0;
                var areaName = dt.Rows[i]["*行政区"].ToString().Trim();
                areaId = GetAreaId(cityId, areaName);
                dr["*行政区"] = areaName;
                if (string.IsNullOrEmpty(areaName) || areaId <= 0)
                {
                    isSkip = true;
                    dr["*行政区"] = (string.IsNullOrEmpty(areaName) ? "必填" : areaName) + "#error";
                }

                var projectName = dt.Rows[i]["*楼盘"].ToString().Trim();
                var obj = ProjectIdByName(fxtCompanyId, cityId, areaId, projectName).FirstOrDefault();
                var projectId = obj == null ? 0 : obj.projectid;
                peiTao.projectid = projectId;
                dr["*楼盘"] = projectName;
                if (string.IsNullOrEmpty(projectName) || projectId <= 0)
                {
                    isSkip = true;
                    dr["*楼盘"] = (string.IsNullOrEmpty(projectName) ? "必填" : projectName) + "#error";
                }

                var peiTaoCodeName = dt.Rows[i]["*配套类型"].ToString().Trim();
                var peiTaoCode = GetCodeByName(peiTaoCodeName, SYS_Code_Dict._住宅项目配套类型);
                peiTao.appendagecode = peiTaoCode;
                peiTao.AppendageName = peiTaoCodeName;
                dr["*配套类型"] = peiTaoCodeName;
                if (!string.IsNullOrEmpty(peiTaoCodeName) && peiTaoCode <= 0)
                {
                    isSkip = true;
                    dr["*配套类型"] = (string.IsNullOrEmpty(peiTaoCodeName) ? "必填" : peiTaoCodeName) + "#error";
                }

                var isInnerText = dt.Rows[i]["*是否内部"].ToString().Trim();
                peiTao.isinner = isInnerText != "否";
                dr["*是否内部"] = isInnerText;

                var buildingArea = dt.Rows[i]["面积"].ToString().Trim();
                var buildingAreaValue = (decimal?)TryParseHelper.StrToDecimal(buildingArea);
                peiTao.area = buildingAreaValue;
                dr["面积"] = buildingArea;
                if (!string.IsNullOrEmpty(buildingArea) && buildingAreaValue == null)
                {
                    isSkip = true;
                    dr["面积"] = buildingArea + "#error";
                }

                var peiTaoName = dt.Rows[i]["配套信息"].ToString().Trim();
                peiTao.p_aname = peiTaoName;
                dr["配套信息"] = peiTaoName;
                if (!string.IsNullOrEmpty(peiTaoName) && peiTaoName.Length > 255)
                {
                    isSkip = true;
                    dr["配套信息"] = peiTaoName + "#error";
                }

                var peiTaoClassName = dt.Rows[i]["配套等级"].ToString().Trim();
                var peiTaoClassCode = GetCodeByName(peiTaoClassName, SYS_Code_Dict._建筑等级);
                peiTao.classcode = peiTaoClassCode;
                peiTao.ClassName = peiTaoClassName;
                dr["配套等级"] = peiTaoClassName;
                if (!string.IsNullOrEmpty(peiTaoClassName) && peiTaoClassCode <= 0)
                {
                    isSkip = true;
                    dr["配套等级"] = peiTaoClassName + "#error";
                }

                if (isSkip)//该行数据中，至少有一列数据格式错误
                {
                    dtError.Rows.Add(dr);
                }
                else
                {
                    listTrue.Add(peiTao);
                }

                if (i > 0 && integer > 0)
                {
                    if (i % integer == 0)
                    {
                        _importTask.TaskStepsIncreased(taskId);
                    }
                }
            }
        }
    }
}
