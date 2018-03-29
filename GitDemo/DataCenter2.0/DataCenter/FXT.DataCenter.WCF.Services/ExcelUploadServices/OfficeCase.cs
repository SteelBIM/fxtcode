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


        public void OfficeCaseExcelUpload(int cityid, int fxtcompanyid, string filePath, string userid, string taskName)
        {
            var taskId = 0;

            try
            {
                //在任务列表创建一条记录
                var task = new DAT_ImportTask()
                {
                    TaskName = taskName,
                    ImportType = SYS_Code_Dict.批量导入类型.办公案例,
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

                List<DatCaseOffice> listTrue;//正确数据
                DataTable dtError;//格式错误数据
                DataFilter(cityid, fxtcompanyid, out listTrue, out dtError, data);

                //错误数据写入Excel
                var fileNamePath = string.Empty;
                if (dtError.Rows.Count > 0)
                {
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "办公案例格式错误数据.xlsx";
                    var folder = MapPath("NeedHandledFiles/FailureData/" + fxtcompanyid);
                    fileNamePath = Path.Combine(folder, fileName);
                    excelHelper.CreateExcel(dtError, fileNamePath, folder);

                }

                var failureNum = 0;

                //正确数据写入表中
                //listTrue.ForEach(m => _businessCase.AdddatCaseOffice(m));
                foreach (var item in listTrue)
                {
                    item.Creator = userid;
                    item.SaveDateTime = DateTime.Now;
                    var insertResult = _officeCase.AddOfficeCase(item);
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
                LogHelper.WriteLog("OfficeCaseExcelUploadTask", "", userid, cityid, fxtcompanyid, ex);
            }

        }
        private void DataFilter(int cityId, int fxtCompanyId, out List<DatCaseOffice> listTrue, out DataTable dtError, DataTable dt)
        {

            listTrue = new List<DatCaseOffice>();
            dtError = new DataTable();

            foreach (DataColumn column in dt.Columns)
                dtError.Columns.Add(column.Caption);


            for (var i = 0; i < dt.Rows.Count; i++)
            {

                var isError = false;
                var datCaseOffice = new DatCaseOffice();
                var dr = dtError.NewRow();

                var areaName = dt.Rows[i]["行政区"].ToString().Trim();
                var areaId = GetAreaId(cityId, areaName);
                dr["行政区"] = areaName;
                if (!string.IsNullOrEmpty(areaName) && areaId < 0)
                {
                    isError = true;
                    dr["行政区"] = areaName + "#error";
                }

                var projectName = dt.Rows[i]["*办公楼盘"].ToString().Trim();
                var projectIds = GetProjectIdByName_office(projectName, areaId, cityId, fxtCompanyId);
                var projectId = projectIds.FirstOrDefault();
                dr["*办公楼盘"] = projectName;
                datCaseOffice.ProjectId = projectId;
                datCaseOffice.ProjectName = projectName;
                if (string.IsNullOrEmpty(projectName) || projectIds.Count() > 1 || projectId == 0)
                {
                    isError = true;
                    dr["*办公楼盘"] = projectName + "#error";
                }

                var buildingName = dt.Rows[i]["办公楼栋"].ToString().Trim();
                var buildingId = GetBuildingIdByName_Office(projectId, buildingName, cityId, fxtCompanyId);
                dr["办公楼栋"] = buildingName;
                datCaseOffice.BuildingId = buildingId;
                datCaseOffice.BuildingName = buildingName;
                if (!string.IsNullOrEmpty(buildingName) && buildingName.Length > 50)
                {
                    isError = true;
                    dr["办公楼栋"] = buildingName + "#error";
                }

                var houseName = dt.Rows[i]["房号"].ToString().Trim();
                var houseId = GetHouseIdByName_Office(buildingId, houseName, cityId, fxtCompanyId);
                dr["房号"] = houseName;
                datCaseOffice.HouseId = houseId;
                datCaseOffice.HouseName = houseName;

                var buildingAreaStr = dt.Rows[i]["*建筑面积_平方米"].ToString().Trim();
                var buildingArea = TryParseHelper.StrToDecimal(buildingAreaStr, -1);
                dr["*建筑面积_平方米"] = buildingAreaStr;
                datCaseOffice.BuildingArea = buildingArea;
                if (string.IsNullOrEmpty(buildingAreaStr) || buildingArea <= 0)
                {
                    isError = true;
                    dr["*建筑面积_平方米"] = buildingAreaStr + "#error";
                }

                var unitPriceStr = dt.Rows[i]["*单价_元每平方米"].ToString().Trim();
                var unitPrice = TryParseHelper.StrToDecimal(unitPriceStr, -1);
                dr["*单价_元每平方米"] = unitPriceStr;
                datCaseOffice.UnitPrice = unitPrice;
                if (string.IsNullOrEmpty(unitPriceStr) || unitPrice <= 0)
                {
                    isError = true;
                    dr["*单价_元每平方米"] = unitPriceStr + "#error";
                }

                var totalPriceStr = dt.Rows[i]["*总价_元"].ToString().Trim();
                var totalPrice = TryParseHelper.StrToDecimal(totalPriceStr, -1);
                dr["*总价_元"] = totalPriceStr;
                datCaseOffice.TotalPrice = totalPrice;
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
                datCaseOffice.CaseDate = TryParseHelper.StrToDateTime(casedate) == null ? (DateTime)SqlDateTime.MinValue : (DateTime)TryParseHelper.StrToDateTime(casedate);
                dr["*案例时间"] = casedate;
                if (string.IsNullOrEmpty(casedate) || TryParseHelper.StrToDateTime(casedate) == null)
                {
                    isError = true;
                    dr["*案例时间"] = casedate + "#error";
                }

                var caseTypeName = dt.Rows[i]["*案例类型"].ToString().Trim();
                var caseTypeCode = GetCodeByName(caseTypeName, SYS_Code_Dict._案例类型);
                datCaseOffice.CaseTypeCode = caseTypeCode;
                dr["*案例类型"] = caseTypeName;
                if (!string.IsNullOrEmpty(caseTypeName) && caseTypeCode == -1)
                {
                    isError = true;
                    dr["*案例类型"] = caseTypeName + "#error";
                }

                var rentRateStr = dt.Rows[i]["约定租金增长率_百分比每年"].ToString().Trim();
                var rentRate = (decimal?)TryParseHelper.StrToDecimal(rentRateStr);
                dr["约定租金增长率_百分比每年"] = rentRateStr;
                datCaseOffice.RentRate = rentRate;
                if (!string.IsNullOrEmpty(rentRateStr) && rentRate == -1)
                {
                    isError = true;
                    dr["约定租金增长率_百分比每年"] = rentRateStr + "#error";
                }

                var managerPriceStr = dt.Rows[i]["物业费_元/平方米*月"].ToString().Trim();
                var managerPrice = (decimal?)TryParseHelper.StrToDecimal(managerPriceStr);
                dr["物业费_元/平方米*月"] = managerPriceStr;
                datCaseOffice.ManagerPrice = managerPrice;
                if (!string.IsNullOrEmpty(managerPriceStr) && managerPrice == -1)
                {
                    isError = true;
                    dr["物业费_元/平方米*月"] = managerPriceStr + "#error";
                }

                var officeTypeName = dt.Rows[i]["办公楼等级"].ToString().Trim();
                var officeType = GetCodeByName(officeTypeName, SYS_Code_Dict._办公楼等级);
                dr["办公楼等级"] = officeTypeName;
                datCaseOffice.OfficeType = officeType;
                if (!string.IsNullOrEmpty(officeTypeName) && officeType < 1)
                {
                    isError = true;
                    dr["办公楼等级"] = officeTypeName + "#error";
                }

                var fitmentName = dt.Rows[i]["装修情况"].ToString().Trim();
                fitmentName = fitmentName == "精装修" ? "精装" : (fitmentName == "简装修" ? "简装" : fitmentName);
                var fitment = GetCodeByName(fitmentName, SYS_Code_Dict._装修情况);
                dr["装修情况"] = fitmentName;
                datCaseOffice.Fitment = fitment;
                if (!string.IsNullOrEmpty(fitmentName) && fitment < 1)
                {
                    isError = true;
                    dr["装修情况"] = fitmentName + "#error";
                }

                var floorNo = dt.Rows[i]["所在楼层"].ToString().Trim();
                dr["所在楼层"] = floorNo;
                datCaseOffice.FloorNo = floorNo;
                if (!string.IsNullOrEmpty(floorNo) && floorNo.Length > 128)
                {
                    isError = true;
                    dr["所在楼层"] = floorNo + "#error";
                }

                var totalFloorStr = dt.Rows[i]["总楼层"].ToString().Trim();
                var totalFloor = (int?)TryParseHelper.StrToInt32(totalFloorStr);
                dr["总楼层"] = totalFloorStr;
                datCaseOffice.TotalFloor = totalFloor;
                if (!string.IsNullOrEmpty(totalFloorStr) && totalFloor == -1)
                {
                    isError = true;
                    dr["总楼层"] = totalFloorStr + "#error";
                }

                var agencyCompany = dt.Rows[i]["中介公司"].ToString().Trim();
                dr["中介公司"] = agencyCompany;
                datCaseOffice.AgencyCompany = agencyCompany;
                if (!string.IsNullOrEmpty(agencyCompany) && agencyCompany.Length > 128)
                {
                    isError = true;
                    dr["中介公司"] = agencyCompany + "#error";
                }

                var agent = dt.Rows[i]["中介人员"].ToString().Trim();
                dr["中介人员"] = agent;
                datCaseOffice.Agent = agent;
                if (!string.IsNullOrEmpty(agent) && agent.Length > 128)
                {
                    isError = true;
                    dr["中介人员"] = agent + "#error";
                }

                var agencyTel = dt.Rows[i]["中介电话"].ToString().Trim();
                dr["中介电话"] = agencyTel;
                datCaseOffice.AgencyTel = agencyTel;
                if (!string.IsNullOrEmpty(agencyTel) && agencyTel.Length > 128)
                {
                    isError = true;
                    dr["中介电话"] = agencyTel + "#error";
                }

                var sourceName = dt.Rows[i]["来源名称"].ToString().Trim();
                dr["来源名称"] = sourceName;
                datCaseOffice.SourceName = sourceName;
                if (!string.IsNullOrEmpty(sourceName) && sourceName.Length > 100)
                {
                    isError = true;
                    dr["来源名称"] = sourceName + "#error";
                }

                var sourceLink = dt.Rows[i]["来源链接"].ToString().Trim();
                dr["来源链接"] = sourceLink;
                datCaseOffice.SourceLink = sourceLink;
                if (!string.IsNullOrEmpty(sourceLink) && sourceLink.Length > 200)
                {
                    isError = true;
                    dr["来源链接"] = sourceLink + "#error";
                }

                var sourcePhone = dt.Rows[i]["来源电话"].ToString().Trim();
                dr["来源电话"] = sourcePhone;
                datCaseOffice.SourcePhone = sourcePhone;
                if (!string.IsNullOrEmpty(sourcePhone) && sourcePhone.Length > 50)
                {
                    isError = true;
                    dr["来源电话"] = sourcePhone + "#error";
                }

                datCaseOffice.CityId = cityId;
                datCaseOffice.FxtCompanyId = fxtCompanyId;
                datCaseOffice.CreateTime = DateTime.Now;

                if (isError)//该行数据中，至少有一列数据格式错误
                {
                    dtError.Rows.Add(dr);
                }
                else
                {
                    listTrue.Add(datCaseOffice);
                }

            }
        }
    }
}
