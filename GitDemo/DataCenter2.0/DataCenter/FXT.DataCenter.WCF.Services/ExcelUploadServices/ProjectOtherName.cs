using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Infrastructure.Common.Common;
using FXT.DataCenter.Infrastructure.Common.Dictionary;
using FXT.DataCenter.Infrastructure.Common.NPOI;
using FXT.DataCenter.Infrastructure.Redis;
using StackExchange.Redis;

namespace FXT.DataCenter.WCF.Services
{
    public partial class ExcelUpload
    {
        public void ProjectOtherNameExcelUpload(int cityid, int fxtcompanyid, string filePath, string userid, string taskName)
        {
            var taskId = 0;
            try
            {
                var task = new DAT_ImportTask
                {
                    TaskName = taskName,
                    ImportType = SYS_Code_Dict.批量导入类型.楼盘别名,
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

                List<SYS_ProjectMatch> listTrue;//正确数据
                DataTable dtError;//格式错误数据
                DataFilter(taskId, integer, cityid, fxtcompanyid, data, out listTrue, out dtError);

                //错误数据写入Excel
                var fileNamePath = string.Empty;
                if (dtError.Rows.Count > 0)
                {
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "楼盘别名错误数据.xlsx";
                    var folder = MapPath("NeedHandledFiles/FailureData/" + fxtcompanyid);
                    fileNamePath = Path.Combine(folder, fileName);
                    excelHelper.CreateExcel(dtError, fileNamePath, folder);
                }

                var failureNum = 0;
                var index4True = 0;//用于统计进度
                //正确数据添加到Project表中
                listTrue.ForEach(m =>
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

                    _projectOtherName.DeleteProjectOtherName(m.NetName, m.NetAreaName, cityid, fxtcompanyid);
                    m.Creator = userid;
                    m.CreateTime = DateTime.Now;
                    var insertResult = _projectOtherName.AddProjectOtherName(m);
                    if (insertResult <= 0) failureNum = failureNum + 1;
                });
                //更新任务状态
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
                LogHelper.WriteLog("ProjectOtherNameExcelUpload", "", userid, cityid, fxtcompanyid, ex);
            }
        }

        private void DataFilter(int taskId, double integer, int cityId, int fxtCompanyId, DataTable dt, out List<SYS_ProjectMatch> listTrue, out DataTable dtError)
        {
            listTrue = new List<SYS_ProjectMatch>();
            dtError = new DataTable();

            //从redis中取出数据
            var areaCach = GetAreaCach(cityId);
            var codeCach = GetCodeCach();

            foreach (DataColumn column in dt.Columns)
                dtError.Columns.Add(column.Caption);

            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var db = new SYS_ProjectMatch();
                var dr = dtError.NewRow();
                db.FXTCompanyId = fxtCompanyId;
                db.CityId = cityId;
                var isError = false;

                var areaId = 0;
                var areaName = dt.Rows[i]["*系统名行政区"].ToString().Trim();

                if (areaCach == null)
                {
                    areaId = GetAreaId(cityId, areaName);
                }
                else
                {
                    var obj = areaCach.FirstOrDefault(m => m.areaname == areaName);
                    areaId = obj == null ? 0 : obj.areaid;
                }

                db.AreaName = areaName;
                dr["*系统名行政区"] = areaName;
                if (string.IsNullOrEmpty(areaName) || areaId <= 0)
                {
                    isError = true;
                    dr["*系统名行政区"] = (string.IsNullOrEmpty(areaName) ? "必填" : areaName) + "#error";
                }

                var projectName = dt.Rows[i]["*系统名"].ToString().Trim();
                var projectobj = ProjectIdByName(fxtCompanyId, cityId, areaId, projectName).FirstOrDefault();
                var projectId = projectobj == null ? 0 : projectobj.projectid;
                db.ProjectNameId = projectId;
                db.ProjectName = projectName;
                dr["*系统名"] = projectName;
                if (string.IsNullOrEmpty(projectName) || projectId <= 0)
                {
                    isError = true;
                    dr["*系统名"] = (string.IsNullOrEmpty(projectName) ? "必填" : projectName) + "#error";
                }

                var NetAreaName = dt.Rows[i]["网络名行政区"].ToString().Trim();
                db.NetAreaName = NetAreaName;
                dr["网络名行政区"] = NetAreaName;
                if (!string.IsNullOrEmpty(NetAreaName) && NetAreaName.Length > 50)
                {
                    isError = true;
                    dr["网络名行政区"] = NetAreaName + "#error";
                }

                var NetName = dt.Rows[i]["*网络名"].ToString().Trim();
                db.NetName = NetName;
                dr["*网络名"] = NetName;
                if (string.IsNullOrEmpty(NetName) || NetName.Length > 100)
                {
                    isError = true;
                    dr["*网络名"] = (string.IsNullOrEmpty(NetName) ? "必填" : NetName) + "#error";
                }
                if (i > 0 && integer > 0)
                {
                    if (i % integer == 0)
                    {
                        _importTask.TaskStepsIncreased(taskId);
                    }
                }

                if (isError)
                {
                    dtError.Rows.Add(dr);
                }
                else
                {
                    listTrue.Add(db);
                }
            }
        }
    }
}
