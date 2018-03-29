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
        public void IndustryHouseExcelUpload(int cityid, int fxtcompanyid, string filePath, string userid, string taskName)
        {
            var taskId = 0;

            try
            {
                //在任务列表创建一条记录
                var task = new DAT_ImportTask()
                {
                    TaskName = taskName,
                    ImportType = SYS_Code_Dict.批量导入类型.工业房号信息,
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

                List<DatHouseIndustry> listTrue;//正确数据
                DataTable dtError;//格式错误数据
                DataFilter(cityid, fxtcompanyid, out listTrue, out dtError, data);

                //错误数据写入Excel
                var fileNamePath = string.Empty;
                if (dtError.Rows.Count > 0)
                {
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "工业房号错误数据.xlsx";
                    var folder = MapPath("NeedHandledFiles/FailureData/" + fxtcompanyid);
                    fileNamePath = Path.Combine(folder, fileName);
                    excelHelper.CreateExcel(dtError, fileNamePath, folder);

                }

                //正确数据写入表中
                foreach (var item in listTrue)
                {
                    long houseId = _industryHouse.GetIndustryHouseId(item.BuildingId, item.HouseName, item.CityId, item.FxtCompanyId);
                    if (houseId <= 0)
                    {
                        item.Creator = userid;
                        _industryHouse.AddIndustryHouse(item);
                    }
                    else
                    {
                        item.SaveUser = userid;
                        item.SaveDateTime = DateTime.Now;
                        item.HouseId = houseId;
                        _industryHouse.UpdateIndustryHouse(item, fxtcompanyid);
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
                LogHelper.WriteLog("IndustryHouseExcelUploadTask", "", userid, cityid, fxtcompanyid, ex);
            }
        }

        private void DataFilter(int cityId, int fxtCompanyId, out List<DatHouseIndustry> listTrue, out DataTable dtError, DataTable dt)
        {
            listTrue = new List<DatHouseIndustry>();
            dtError = new DataTable();
            foreach (DataColumn column in dt.Columns)
                dtError.Columns.Add(column.Caption);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var isSkip = false;
                var datHouse = new DatHouseIndustry();
                var dr = dtError.NewRow();

                datHouse.CityId = cityId;
                datHouse.FxtCompanyId = fxtCompanyId;
                

                var areaName = dt.Rows[i]["*行政区"].ToString().Trim();
                var areaId = GetAreaId(cityId, areaName);
                dr["*行政区"] = areaName;
                if (string.IsNullOrEmpty(areaName) || areaId <= 0)
                {
                    isSkip = true;
                    dr["*行政区"] = areaName + "#error";
                }

                var projectName = dt.Rows[i]["*工业楼盘"].ToString().Trim();
                var projectIds = GetProjectIdByName_Industry(projectName, areaId, cityId, fxtCompanyId);
                var projectId = projectIds.FirstOrDefault();
                datHouse.ProjectId = projectId;
                dr["*工业楼盘"] = projectName;
                if (string.IsNullOrEmpty(projectName) || projectId <= 0)
                {
                    isSkip = true;
                    dr["*工业楼盘"] = projectName + "#error";
                }

                var buildingName = dt.Rows[i]["*工业楼栋"].ToString().Trim();
                var buildingId = GetBuildingIdByName_Industry(projectId, buildingName, cityId, fxtCompanyId);
                datHouse.BuildingId = buildingId;
                dr["*工业楼栋"] = buildingName;
                if (string.IsNullOrEmpty(buildingName) || buildingId <= 0)
                {
                    isSkip = true;
                    dr["*工业楼栋"] = buildingName + "#error";
                }

                var unitNo = dt.Rows[i]["单元号"].ToString().Trim();
                datHouse.UnitNo = unitNo;
                dr["单元号"] = unitNo;
                if (!string.IsNullOrEmpty(unitNo) && unitNo.Length > 20)
                {
                    isSkip = true;
                    dr["单元号"] = unitNo + "#error";
                }

                var floorNo = dt.Rows[i]["*物理层"].ToString().Trim();
                datHouse.FloorNo = TryParseHelper.StrToInt32(floorNo, -1);
                dr["*物理层"] = floorNo;
                if (TryParseHelper.StrToInt32(floorNo, -1) <= 0)
                {
                    isSkip = true;
                    dr["*物理层"] = floorNo + "#error";
                }

                var floorNum = dt.Rows[i]["*实际层"].ToString().Trim();
                datHouse.FloorNum = floorNum;
                dr["*实际层"] = floorNum;
                if (string.IsNullOrEmpty(floorNum) || floorNum.Length > 10)
                {
                    isSkip = true;
                    dr["*实际层"] = floorNum + "#error";
                }

                var houseNo = dt.Rows[i]["*室号"].ToString().Trim();
                datHouse.HouseNo = houseNo;
                dr["*室号"] = houseNo;
                if (string.IsNullOrEmpty(houseNo) || houseNo.Length > 20)
                {
                    isSkip = true;
                    dr["*室号"] = houseNo + "#error";
                }

                var houseName = dt.Rows[i]["*房号名称"].ToString().Trim();
                datHouse.HouseName = houseName;
                dr["*房号名称"] = houseName;
                if (string.IsNullOrEmpty(houseName) || houseName.Length > 30)
                {
                    isSkip = true;
                    dr["*房号名称"] = houseName + "#error";
                }

                var purposeName = dt.Rows[i]["证载用途"].ToString().Trim();
                var purposeCode = GetCodeByName(purposeName, SYS_Code_Dict._居住用途);
                datHouse.PurposeCode = purposeCode;
                dr["证载用途"] = purposeName;
                if (!string.IsNullOrEmpty(purposeName) && purposeCode <= 0)
                {
                    isSkip = true;
                    dr["证载用途"] = purposeName + "#error";
                }

                var sjPurposeName = dt.Rows[i]["实际用途"].ToString().Trim();
                var sjPurposeCode = GetCodeByName(sjPurposeName, SYS_Code_Dict._居住用途);
                datHouse.SJPurposeCode = sjPurposeCode;
                dr["实际用途"] = sjPurposeName;
                if (!string.IsNullOrEmpty(sjPurposeName) && sjPurposeCode <= 0)
                {
                    isSkip = true;
                    dr["实际用途"] = sjPurposeName + "#error";
                }

                var buildingArea = dt.Rows[i]["建筑面积_平方米"].ToString().Trim();
                datHouse.BuildingArea = (decimal?)TryParseHelper.StrToDecimal(buildingArea);
                dr["建筑面积_平方米"] = buildingArea;
                if (!string.IsNullOrEmpty(buildingArea) && TryParseHelper.StrToDecimal(buildingArea) == null)
                {
                    isSkip = true;
                    dr["建筑面积_平方米"] = buildingArea + "#error";
                }

                var innerBuildingArea = dt.Rows[i]["套内面积_平方米"].ToString().Trim();
                datHouse.InnerBuildingArea = (decimal?)TryParseHelper.StrToDecimal(innerBuildingArea);
                dr["套内面积_平方米"] = innerBuildingArea;
                if (!string.IsNullOrEmpty(innerBuildingArea) && TryParseHelper.StrToDecimal(innerBuildingArea) == null)
                {
                    isSkip = true;
                    dr["套内面积_平方米"] = innerBuildingArea + "#error";
                }

                var frontName = dt.Rows[i]["朝向"].ToString().Trim();
                var frontCode = GetCodeByName(frontName, SYS_Code_Dict._朝向);
                datHouse.FrontCode = frontCode;
                dr["朝向"] = frontName;
                if (!string.IsNullOrEmpty(frontName) && frontCode == -1)
                {
                    isSkip = true;
                    dr["朝向"] = frontName + "#error";
                }

                var sightName = dt.Rows[i]["景观"].ToString().Trim();
                var sightCode = GetCodeByName(sightName, SYS_Code_Dict._景观);
                datHouse.SightCode = sightCode;
                dr["景观"] = sightName;
                if (!string.IsNullOrEmpty(sightName) && sightCode == -1)
                {
                    isSkip = true;
                    dr["景观"] = sightName + "#error";
                }

                var unitPrice = dt.Rows[i]["单价_元每平方米"].ToString().Trim();
                datHouse.UnitPrice = (decimal?)TryParseHelper.StrToDecimal(unitPrice);
                dr["单价_元每平方米"] = unitPrice;
                if (!string.IsNullOrEmpty(unitPrice) && TryParseHelper.StrToDecimal(unitPrice) == null)
                {
                    isSkip = true;
                    dr["单价_元每平方米"] = unitPrice + "#error";
                }

                var weight = dt.Rows[i]["价格系数"].ToString().Trim();
                datHouse.Weight = (decimal?)TryParseHelper.StrToDecimal(weight);
                dr["价格系数"] = weight;
                if (!string.IsNullOrEmpty(weight) && TryParseHelper.StrToDecimal(weight) == null)
                {
                    isSkip = true;
                    dr["价格系数"] = weight + "#error";
                }

                var isEvalue = dt.Rows[i]["是否可估"].ToString().Trim();
                datHouse.IsEValue = isEvalue == "否" ? 0 : 1;
                dr["是否可估"] = isEvalue;
                if (!string.IsNullOrEmpty(isEvalue) && !(isEvalue == "是" || isEvalue == "否"))
                {
                    isSkip = true;
                    dr["是否可估"] = isEvalue + "#error";
                }

                var remarks = dt.Rows[i]["备注"].ToString().Trim();
                datHouse.Remarks = remarks;
                dr["备注"] = remarks;
                if (!string.IsNullOrEmpty(remarks) && remarks.Length > 512)
                {
                    isSkip = true;
                    dr["备注"] = remarks + "#error";
                }

                if (isSkip)//该行数据中，至少有一列数据格式错误
                {
                    dtError.Rows.Add(dr);
                }
                else
                {
                    listTrue.Add(datHouse);
                }
            }
        }
    }
}
