using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
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
        public void IndustryCaseExcelUpload(int cityid, int fxtcompanyid, string filePath, string userid, string taskName)
        {
            var taskId = 0;

            try
            {
                //在任务列表创建一条记录
                var task = new DAT_ImportTask()
                {
                    TaskName = taskName,
                    ImportType = SYS_Code_Dict.批量导入类型.工业案例,
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

                List<DatCaseIndustry> listTrue;//正确数据
                DataTable dtError;//格式错误数据
                DataFilter(userid,cityid, fxtcompanyid, out listTrue, out dtError, data);

                //错误数据写入Excel
                var fileNamePath = string.Empty;
                if (dtError.Rows.Count > 0)
                {
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "工业案例格式错误数据.xlsx";
                    var folder = MapPath("NeedHandledFiles/FailureData/" + fxtcompanyid);
                    fileNamePath = Path.Combine(folder, fileName);
                    excelHelper.CreateExcel(dtError, fileNamePath, folder);
                }

                var failureNum = 0;

                //正确数据写入表中
                foreach (var item in listTrue)
                {
                    item.Creator = userid;
                    var insertResult = _industryCase.AddIndustryCase(item);
                    if (insertResult <= 0) failureNum = failureNum + 1;
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
                _importTask.UpdateTask(taskId, listTrue.Count - failureNum, dtError.Rows.Count, 0, relativePath, 1);
            }
            catch (Exception ex)
            {
                _importTask.UpdateTask(taskId, 0, 0, 0, "", -1);
                LogHelper.WriteLog("IndustryCaseExcelUploadTask", "", userid, cityid, fxtcompanyid, ex);
            }

        }
        private void DataFilter(string userId,int cityId, int fxtCompanyId, out List<DatCaseIndustry> listTrue, out DataTable dtError, DataTable dt)
        {

            listTrue = new List<DatCaseIndustry>();
            dtError = new DataTable();

            foreach (DataColumn column in dt.Columns)
                dtError.Columns.Add(column.Caption);


            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var isError = false;
                var datCase = new DatCaseIndustry();
                var dr = dtError.NewRow();

                var areaName = dt.Rows[i]["*行政区"].ToString().Trim();
                var areaId = GetAreaId(cityId, areaName);
                datCase.AreaId = areaId;
                dr["*行政区"] = areaName;
                if (string.IsNullOrEmpty(areaName) || (!string.IsNullOrEmpty(areaName) && areaId <= 0))
                {
                    isError = true;
                    dr["*行政区"] = areaName + "#error";
                }

                var subAreaName = dt.Rows[i]["工业片区"].ToString().Trim();
                var subAreaId = GetSubAreaIdByName_Industry(subAreaName, areaId, fxtCompanyId);
                dr["工业片区"] = subAreaName;
                datCase.SubAreaId = subAreaId;
                if ((!string.IsNullOrEmpty(subAreaName) && subAreaId < 0))
                {
                    isError = true;
                    dr["工业片区"] = subAreaName + "#error";
                }

                var projectName = dt.Rows[i]["*工业楼盘"].ToString().Trim();
                var projectIds = GetProjectIdByName_Industry(projectName, areaId, cityId, fxtCompanyId);
                var projectId = projectIds.FirstOrDefault();
                dr["*工业楼盘"] = projectName;
                datCase.ProjectId = projectId;
                datCase.ProjectName = projectName;
                if (string.IsNullOrEmpty(projectName))
                {
                    isError = true;
                    dr["*工业楼盘"] = projectName + "#error";
                }

                var buildingName = dt.Rows[i]["工业楼栋"].ToString().Trim();
                var buildingId = GetBuildingIdByName_Industry(projectId, buildingName, cityId, fxtCompanyId);
                dr["工业楼栋"] = buildingName;
                datCase.BuildingId = buildingId;
                datCase.BuildingName = buildingName;
                if (!string.IsNullOrEmpty(buildingName) && buildingName.Length > 50)
                {
                    isError = true;
                    dr["工业楼栋"] = buildingName + "#error";
                }

                var houseName = dt.Rows[i]["工业房号"].ToString().Trim();
                var houseId = GetHouseIdByName_Industry(buildingId, houseName, cityId, fxtCompanyId);
                dr["工业房号"] = houseName;
                datCase.HouseId = houseId;
                datCase.HouseName = houseName;

                var address = dt.Rows[i]["地址"].ToString().Trim();
                dr["地址"] = address;
                datCase.Address = address;
                if (!string.IsNullOrEmpty(address) && address.Length > 512)
                {
                    isError = true;
                    dr["地址"] = address + "#error";
                }

                var buildingAreaStr = dt.Rows[i]["*建筑面积_平方米"].ToString().Trim();
                var buildingArea = TryParseHelper.StrToDecimal(buildingAreaStr, -1);
                dr["*建筑面积_平方米"] = buildingAreaStr;
                datCase.BuildingArea = buildingArea;
                if (string.IsNullOrEmpty(buildingAreaStr) || buildingArea <= 0)
                {
                    isError = true;
                    dr["*建筑面积_平方米"] = buildingAreaStr + "#error";
                }

                var unitPriceStr = dt.Rows[i]["*单价_元每平方米"].ToString().Trim();
                var unitPrice = TryParseHelper.StrToDecimal(unitPriceStr, -1);
                dr["*单价_元每平方米"] = unitPriceStr;
                datCase.UnitPrice = unitPrice;
                if (string.IsNullOrEmpty(unitPriceStr) || unitPrice <= 0)
                {
                    isError = true;
                    dr["*单价_元每平方米"] = unitPriceStr + "#error";
                }

                var totalPriceStr = dt.Rows[i]["*总价_元"].ToString().Trim();
                var totalPrice = TryParseHelper.StrToDecimal(totalPriceStr, -1);
                dr["*总价_元"] = totalPriceStr;
                datCase.TotalPrice = totalPrice;
                if (string.IsNullOrEmpty(totalPriceStr) || totalPrice == -1)
                {
                    isError = true;
                    dr["*总价_元"] = totalPriceStr + "#error";
                }
                else if (totalPrice > 0 && buildingArea > 0 && unitPrice > 0 && Math.Abs(buildingArea * unitPrice - totalPrice) > (decimal)0.01)
                {
                    isError = true;
                    dr["*总价_元"] = "总价计算有误差" + totalPrice + "#error";
                }

                var casedate = dt.Rows[i]["*案例时间"].ToString().Trim();
                datCase.CaseDate = TryParseHelper.StrToDateTime(casedate) == null ? (DateTime)SqlDateTime.MinValue : (DateTime)TryParseHelper.StrToDateTime(casedate);
                dr["*案例时间"] = casedate;
                if (string.IsNullOrEmpty(casedate) || TryParseHelper.StrToDateTime(casedate) == null)
                {
                    isError = true;
                    dr["*案例时间"] = casedate + "#error";
                }

                var caseTypeName = dt.Rows[i]["*案例类型"].ToString().Trim();
                var caseTypeCode = GetCodeByName(caseTypeName, SYS_Code_Dict._案例类型);
                datCase.CaseTypeCode = caseTypeCode;
                dr["*案例类型"] = caseTypeName;
                if (!string.IsNullOrEmpty(caseTypeName) && caseTypeCode == -1)
                {
                    isError = true;
                    dr["*案例类型"] = caseTypeName + "#error";
                }

                var rentRateStr = dt.Rows[i]["约定租金增长率_百分比每年"].ToString().Trim();
                var rentRate = (decimal?)TryParseHelper.StrToDecimal(rentRateStr);
                dr["约定租金增长率_百分比每年"] = rentRateStr;
                datCase.RentRate = rentRate;
                if (!string.IsNullOrEmpty(rentRateStr) && rentRate == -1)
                {
                    isError = true;
                    dr["约定租金增长率_百分比每年"] = rentRateStr + "#error";
                }

                var managerPriceStr = dt.Rows[i]["物业费_元/平方米*月"].ToString().Trim();
                var managerPrice = (decimal?)TryParseHelper.StrToDecimal(managerPriceStr);
                dr["物业费_元/平方米*月"] = managerPriceStr;
                datCase.ManagerPrice = managerPrice;
                if (!string.IsNullOrEmpty(managerPriceStr) && managerPrice == -1)
                {
                    isError = true;
                    dr["物业费_元/平方米*月"] = managerPriceStr + "#error";
                }

                var floorNo = dt.Rows[i]["所在楼层"].ToString().Trim();
                dr["所在楼层"] = floorNo;
                datCase.FloorNo = floorNo;
                if (!string.IsNullOrEmpty(floorNo) && floorNo.Length > 128)
                {
                    isError = true;
                    dr["所在楼层"] = floorNo + "#error";
                }

                var totalFloorStr = dt.Rows[i]["总楼层"].ToString().Trim();
                var totalFloor = (int?)TryParseHelper.StrToInt32(totalFloorStr);
                dr["总楼层"] = totalFloorStr;
                datCase.TotalFloor = totalFloor;
                if (!string.IsNullOrEmpty(totalFloorStr) && totalFloor == -1)
                {
                    isError = true;
                    dr["总楼层"] = totalFloorStr + "#error";
                }

                var agencyCompany = dt.Rows[i]["中介公司"].ToString().Trim();
                dr["中介公司"] = agencyCompany;
                datCase.AgencyCompany = agencyCompany;
                if (!string.IsNullOrEmpty(agencyCompany) && agencyCompany.Length > 128)
                {
                    isError = true;
                    dr["中介公司"] = agencyCompany + "#error";
                }

                var agent = dt.Rows[i]["中介人员"].ToString().Trim();
                dr["中介人员"] = agent;
                datCase.Agent = agent;
                if (!string.IsNullOrEmpty(agent) && agent.Length > 128)
                {
                    isError = true;
                    dr["中介人员"] = agent + "#error";
                }

                var agencyTel = dt.Rows[i]["中介电话"].ToString().Trim();
                dr["中介电话"] = agencyTel;
                datCase.AgencyTel = agencyTel;
                if (!string.IsNullOrEmpty(agencyTel) && agencyTel.Length > 128)
                {
                    isError = true;
                    dr["中介电话"] = agencyTel + "#error";
                }

                var sourceName = dt.Rows[i]["来源名称"].ToString().Trim();
                dr["来源名称"] = sourceName;
                datCase.SourceName = sourceName;
                if (!string.IsNullOrEmpty(sourceName) && sourceName.Length > 100)
                {
                    isError = true;
                    dr["来源名称"] = sourceName + "#error";
                }

                var sourceLink = dt.Rows[i]["来源链接"].ToString().Trim();
                dr["来源链接"] = sourceLink;
                datCase.SourceLink = sourceLink;
                if (!string.IsNullOrEmpty(sourceLink) && sourceLink.Length > 200)
                {
                    isError = true;
                    dr["来源链接"] = sourceLink + "#error";
                }

                var sourcePhone = dt.Rows[i]["来源电话"].ToString().Trim();
                dr["来源电话"] = sourcePhone;
                datCase.SourcePhone = sourcePhone;
                if (!string.IsNullOrEmpty(sourcePhone) && sourcePhone.Length > 50)
                {
                    isError = true;
                    dr["来源电话"] = sourcePhone + "#error";
                }

                datCase.CityId = cityId;
                datCase.FxtCompanyId = fxtCompanyId;
                datCase.CreateTime = DateTime.Now;
                datCase.Creator = userId;
                datCase.SaveUser = userId;

                if (isError)//该行数据中，至少有一列数据格式错误
                {
                    dtError.Rows.Add(dr);
                }
                else
                {
                    listTrue.Add(datCase);
                }
            }
        }
    }
}
