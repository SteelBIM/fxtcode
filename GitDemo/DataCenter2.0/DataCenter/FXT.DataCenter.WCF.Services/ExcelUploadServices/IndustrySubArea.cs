using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Infrastructure.Common.Dictionary;
using FXT.DataCenter.Infrastructure.Common.Common;
using FXT.DataCenter.Infrastructure.Common.NPOI;


namespace FXT.DataCenter.WCF.Services
{
    public partial class ExcelUpload
    {
        public void IndustrySubAreaUpload(int cityid, int fxtcompanyid, string filePath, string userid, string taskName)
        {
            var taskId = 0;

            try
            {
                //在任务列表创建一条记录
                var task = new DAT_ImportTask()
                {
                    TaskName = taskName,
                    ImportType = SYS_Code_Dict.批量导入类型.工业片区,
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
                var data = excelHelper.ExcelToDataTable("Sheet1", true);

                List<SYS_SubArea_Industry> listTrue;//正确数据
                DataTable dtError;//格式错误数据
                DataFilter(cityid, fxtcompanyid, out listTrue, out dtError, data);

                //错误数据写入Excel
                var fileNamePath = string.Empty;
                if (dtError.Rows.Count > 0)
                {
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "工业片区错误数据.xlsx";
                    var folder = MapPath("NeedHandledFiles/FailureData/" + fxtcompanyid);
                    fileNamePath = Path.Combine(folder, fileName);
                    excelHelper.CreateExcel(dtError, fileNamePath, folder);

                }

                //正确数据写入表中
                foreach (var item in listTrue)
                {
                    var areaid = GetAreaId(cityid, item.AreaName);
                    var subAreaId = GetSubAreaIdByName_Industry(item.SubAreaName, areaid, fxtcompanyid);

                    if (subAreaId <= 0)
                    {
                        item.Creators = userid;
                        _industrySubArea.AddSubArea(item);
                    }
                    else
                    {
                        item.SaveUser = userid;
                        item.SubAreaId = subAreaId;
                        _industrySubArea.UpdateSubArea(item);
                    }
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
                _importTask.UpdateTask(taskId, listTrue.Count, dtError.Rows.Count, 0, relativePath, 1);
            }

            catch (Exception ex)
            {
                _importTask.UpdateTask(taskId, 0, 0, 0, "", -1);
                LogHelper.WriteLog("IndustrySubAreaExcelUploadTask", "", userid, cityid, fxtcompanyid, ex);
            }
        }

        private void DataFilter(int cityId, int fxtCompanyId, out List<SYS_SubArea_Industry> listTrue, out DataTable dtError, DataTable dt)
        {
            listTrue = new List<SYS_SubArea_Industry>();
            dtError = new DataTable();
            foreach (DataColumn column in dt.Columns)
                dtError.Columns.Add(column.Caption);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var isSkip = false;
                var datSubArea = new SYS_SubArea_Industry();
                var dr = dtError.NewRow();

                datSubArea.FxtCompanyId = fxtCompanyId;
                datSubArea.CreateDate = DateTime.Now;
                datSubArea.SaveDate = DateTime.Now;

                var areaName = dt.Rows[i]["*行政区"].ToString().Trim();
                var areaId = GetAreaId(cityId, areaName);
                datSubArea.AreaId = areaId;
                datSubArea.AreaName = areaName;
                dr["*行政区"] = areaName;
                if (string.IsNullOrEmpty(areaName) || areaId == -1)
                {
                    isSkip = true;
                    dr["*行政区"] = areaName + "#error";
                }

                var subAreaName = dt.Rows[i]["*工业片区"].ToString().Trim();
                datSubArea.SubAreaName = subAreaName;
                dr["*工业片区"] = subAreaName;
                if (string.IsNullOrEmpty(subAreaName) || (!string.IsNullOrEmpty(subAreaName) && subAreaName.Length > 30))
                {
                    isSkip = true;
                    dr["*工业片区"] = subAreaName + "#error";
                }

                var AreaLine = dt.Rows[i]["环线"].ToString().Trim();
                datSubArea.AreaLine = AreaLine;
                dr["环线"] = AreaLine;
                if (!(string.IsNullOrEmpty(AreaLine)) && AreaLine.Length > 30)
                {
                    isSkip = true;
                    dr["环线"] = AreaLine + "#error";
                }

                var Details = dt.Rows[i]["描述"].ToString().Trim();
                datSubArea.Details = Details;
                dr["描述"] = Details;
                if (!(string.IsNullOrEmpty(Details)) && Details.Length > 600)
                {
                    isSkip = true;
                    dr["描述"] = Details + "#error";
                }

                string 经度 = dt.Rows[i]["经度"].ToString().Trim();
                datSubArea.X = (decimal?)TryParseHelper.StrToDecimal(经度);
                dr["经度"] = 经度;
                if (!string.IsNullOrEmpty(经度) && TryParseHelper.StrToDecimal(经度) == null)
                {
                    isSkip = true;
                    dr["经度"] = 经度 + "#error";
                }

                string 纬度 = dt.Rows[i]["纬度"].ToString().Trim();
                datSubArea.Y = (decimal?)TryParseHelper.StrToDecimal(纬度);
                dr["纬度"] = 纬度;
                if (!string.IsNullOrEmpty(纬度) && TryParseHelper.StrToDecimal(纬度) == null)
                {
                    isSkip = true;
                    dr["纬度"] = 纬度 + "#error";
                }

                if (isSkip)//该行数据中，至少有一列数据格式错误
                {
                    dtError.Rows.Add(dr);
                }
                else
                {
                    listTrue.Add(datSubArea);
                }
            }
        }
    }
}
