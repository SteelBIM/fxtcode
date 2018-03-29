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
        public void IndustryProjectUpload(int cityid, int fxtcompanyid, string filePath, string userid, string taskName)
        {
            var taskId = 0;

            try
            {
                //在任务列表创建一条记录
                var task = new DAT_ImportTask()
                {
                    TaskName = taskName,
                    ImportType = SYS_Code_Dict.批量导入类型.工业楼盘信息,
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

                List<DatProjectIndustry> listTrue;//正确数据
                DataTable dtError;//格式错误数据
                DataFilter(cityid, fxtcompanyid, out listTrue, out dtError, data);

                //错误数据写入Excel
                var fileNamePath = string.Empty;
                if (dtError.Rows.Count > 0)
                {
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "工业楼盘错误数据.xlsx";
                    var folder = MapPath("NeedHandledFiles/FailureData/" + fxtcompanyid);
                    fileNamePath = Path.Combine(folder, fileName);
                    excelHelper.CreateExcel(dtError, fileNamePath, folder);

                }
                
                //正确数据写入表中
                foreach (var item in listTrue)
                {
                    var areaid = GetAreaId(cityid, item.AreaName);
                    var projectId = GetProjectIdByName_Industry(item.ProjectName, areaid, cityid, fxtcompanyid).FirstOrDefault();

                    if (projectId <= 0)
                    {
                        item.Creator = userid;
                        _industryProject.AddProjectIndustry(item);
                    }
                    else
                    {
                        item.SaveUser = userid;
                        item.ProjectId = projectId;
                        _industryProject.UpdateProjectIndustry(item, fxtcompanyid);
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
                LogHelper.WriteLog("IndustryProjectExcelUploadTask", "", userid, cityid, fxtcompanyid, ex);
            }
        }

        private void DataFilter(int cityId, int fxtCompanyId, out List<DatProjectIndustry> listTrue, out DataTable dtError, DataTable dt)
        {
            listTrue = new List<DatProjectIndustry>();
            dtError = new DataTable();
            foreach (DataColumn column in dt.Columns)
                dtError.Columns.Add(column.Caption);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var isSkip = false;
                var datIndustryProject = new DatProjectIndustry();
                var dr = dtError.NewRow();

                datIndustryProject.CityId = cityId;
                datIndustryProject.FxtCompanyId = fxtCompanyId;
                datIndustryProject.CreateTime = DateTime.Now;
                datIndustryProject.SaveDateTime = DateTime.Now;

                var areaName = dt.Rows[i]["*行政区"].ToString().Trim();
                var areaId = GetAreaId(cityId, areaName);
                datIndustryProject.AreaId = areaId;
                datIndustryProject.AreaName = areaName;
                dr["*行政区"] = areaName;
                if (string.IsNullOrEmpty(areaName) || areaId == -1)
                {
                    isSkip = true;
                    dr["*行政区"] = areaName + "#error";
                }
                
                var subAreaName = dt.Rows[i]["工业片区"].ToString().Trim();
                var subAreaId = string.IsNullOrEmpty(subAreaName) ? -1 : GetSubAreaIdByName_Industry(subAreaName, areaId, fxtCompanyId);
                datIndustryProject.SubAreaId = subAreaId;
                datIndustryProject.SubAreaName = subAreaName;
                dr["工业片区"] = subAreaName;
                if (!string.IsNullOrEmpty(subAreaName) && subAreaId <= 0)
                {
                    isSkip = true;
                    dr["工业片区"] = subAreaName + "#error";
                }
                
                var projectName = dt.Rows[i]["*工业楼盘"].ToString().Trim();
                datIndustryProject.ProjectName = projectName;
                dr["*工业楼盘"] = projectName;
                if (string.IsNullOrEmpty(projectName) || (!string.IsNullOrEmpty(projectName) && projectName.Length > 100))
                {
                    isSkip = true;
                    dr["*工业楼盘"] = projectName + "#error";
                }
                else
                {
                    datIndustryProject.PinYin = StringHelper.GetPYString(projectName);
                    datIndustryProject.PinYinAll = StringHelper.GetAllPYString(projectName);
                }

                var purposeCodeName = dt.Rows[i]["主用途"].ToString().Trim();
                var purposeCode = GetCodeByName(purposeCodeName, SYS_Code_Dict._土地用途);
                datIndustryProject.PurposeCode = purposeCode;
                datIndustryProject.PurposeCodeName = purposeCodeName;
                dr["主用途"] = purposeCodeName;
                if (!string.IsNullOrEmpty(purposeCodeName) && purposeCode <= 0)
                {
                    isSkip = true;
                    dr["主用途"] = purposeCodeName + "#error";
                }

                var CorrelationTypeName = dt.Rows[i]["与片区的关联度"].ToString().Trim();
                var CorrelationType = GetCodeByName(CorrelationTypeName, SYS_Code_Dict._商圈关联度);
                datIndustryProject.CorrelationType = CorrelationType;
                datIndustryProject.CorrelationTypeName = CorrelationTypeName;
                dr["与片区的关联度"] = CorrelationTypeName;
                if (!string.IsNullOrEmpty(CorrelationTypeName) && CorrelationType <= 0)
                {
                    isSkip = true;
                    dr["与片区的关联度"] = CorrelationTypeName + "#error";
                }

                var otherName = dt.Rows[i]["别名"].ToString().Trim();
                datIndustryProject.OtherName = otherName;
                dr["别名"] = otherName;
                if (!(string.IsNullOrEmpty(otherName)) && otherName.Length > 100)
                {
                    isSkip = true;
                    dr["别名"] = otherName + "#error";
                }

                var address = dt.Rows[i]["地址"].ToString().Trim();
                datIndustryProject.Address = address;
                dr["地址"] = address;
                if (!(string.IsNullOrEmpty(address)) && address.Length > 200)
                {
                    isSkip = true;
                    dr["地址"] = address + "#error";
                }

                var fieldNo = dt.Rows[i]["宗地号"].ToString().Trim();
                datIndustryProject.FieldNo = fieldNo;
                dr["宗地号"] = fieldNo;
                if (!(string.IsNullOrEmpty(fieldNo)) && fieldNo.Length > 100)
                {
                    isSkip = true;
                    dr["宗地号"] = fieldNo + "#error";
                }

                var LandArea = dt.Rows[i]["土地面积_平方米"].ToString().Trim();
                var LandAreaValue = (decimal?)TryParseHelper.StrToDecimal(LandArea);
                datIndustryProject.LandArea = LandAreaValue;
                dr["土地面积_平方米"] = LandArea;
                if (!string.IsNullOrEmpty(LandArea) && LandAreaValue == null)
                {
                    isSkip = true;
                    dr["土地面积_平方米"] = LandArea + "#error";
                }

                var StartDate = dt.Rows[i]["土地起始日期"].ToString().Trim();
                var StartDateValue = (DateTime?)TryParseHelper.StrToDateTime(StartDate);
                datIndustryProject.StartDate = StartDateValue;
                dr["土地起始日期"] = StartDate;
                if (!string.IsNullOrEmpty(StartDate) && StartDateValue == null)
                {
                    isSkip = true;
                    dr["土地起始日期"] = StartDate + "#error";
                }

                var StartEndDate = dt.Rows[i]["土地终止日期"].ToString().Trim();
                var StartEndDateValue = (DateTime?)TryParseHelper.StrToDateTime(StartEndDate);
                datIndustryProject.StartEndDate = StartEndDateValue;
                dr["土地终止日期"] = StartEndDateValue;
                if (!string.IsNullOrEmpty(StartEndDate) && StartEndDateValue == null)
                {
                    isSkip = true;
                    dr["土地终止日期"] = StartEndDate + "#error";
                }

                var UsableYear = dt.Rows[i]["土地使用年限_年"].ToString().Trim();
                var UsableYearValue = (int?)TryParseHelper.StrToInt32(UsableYear);
                datIndustryProject.UsableYear = UsableYearValue;
                dr["土地使用年限_年"] = UsableYear;
                if (!string.IsNullOrEmpty(UsableYear) && UsableYearValue == null)
                {
                    isSkip = true;
                    dr["土地使用年限_年"] = UsableYear + "#error";
                }

                var BuildingArea = dt.Rows[i]["总建筑面积_平方米"].ToString().Trim();
                var BuildingAreaValue = (decimal?)TryParseHelper.StrToDecimal(BuildingArea);
                datIndustryProject.BuildingArea = BuildingAreaValue;
                dr["总建筑面积_平方米"] = BuildingArea;
                if (!string.IsNullOrEmpty(BuildingArea) && BuildingAreaValue == null)
                {
                    isSkip = true;
                    dr["总建筑面积_平方米"] = BuildingArea + "#error";
                }

                var CubageRate = dt.Rows[i]["容积率"].ToString().Trim();
                var CubageRateValue = (decimal?)TryParseHelper.StrToDecimal(CubageRate);
                datIndustryProject.CubageRate = CubageRateValue;
                dr["容积率"] = CubageRate;
                if (!string.IsNullOrEmpty(CubageRate) && CubageRateValue == null)
                {
                    isSkip = true;
                    dr["容积率"] = CubageRate + "#error";
                }

                var GreenRate = dt.Rows[i]["绿化率_百分比"].ToString().Trim();
                var GreenRateValue = (decimal?)TryParseHelper.StrToDecimal(GreenRate);
                datIndustryProject.GreenRate = GreenRateValue;
                dr["绿化率_百分比"] = GreenRate;
                if (!string.IsNullOrEmpty(GreenRate) && GreenRateValue == null)
                {
                    isSkip = true;
                    dr["绿化率_百分比"] = GreenRate + "#error";
                }

                var BuildingNum = dt.Rows[i]["总栋数_栋"].ToString().Trim();
                var BuildingNumValue = (int?)TryParseHelper.StrToInt32(BuildingNum);
                datIndustryProject.BuildingNum = BuildingNumValue;
                dr["总栋数_栋"] = BuildingNum;
                if (!string.IsNullOrEmpty(BuildingNum) && BuildingNumValue == null)
                {
                    isSkip = true;
                    dr["总栋数_栋"] = BuildingNum + "#error";
                }

                var BuildingTypeName = dt.Rows[i]["建筑类型"].ToString().Trim();
                var BuildingType = GetCodeByName(BuildingTypeName, SYS_Code_Dict._建筑类型);
                datIndustryProject.BuildingType = BuildingType;
                datIndustryProject.BuildingTypeName = BuildingTypeName;
                dr["建筑类型"] = BuildingTypeName;
                if (!string.IsNullOrEmpty(BuildingTypeName) && BuildingType <= 0)
                {
                    isSkip = true;
                    dr["建筑类型"] = BuildingTypeName + "#error";
                }

                var IndustryTypeName = dt.Rows[i]["工业类型"].ToString().Trim();
                var IndustryType = GetCodeByName(IndustryTypeName, SYS_Code_Dict._工业类型);
                datIndustryProject.IndustryType = IndustryType;
                datIndustryProject.IndustryTypeName = IndustryTypeName;
                dr["工业类型"] = IndustryTypeName;
                if (!string.IsNullOrEmpty(IndustryTypeName) && IndustryType <= 0)
                {
                    isSkip = true;
                    dr["工业类型"] = IndustryTypeName + "#error";
                }

                var EndDate = dt.Rows[i]["竣工时间"].ToString().Trim();
                var EndDateValue = (DateTime?)TryParseHelper.StrToDateTime(EndDate);
                datIndustryProject.EndDate = EndDateValue;
                dr["竣工时间"] = EndDate;
                if (!string.IsNullOrEmpty(EndDate) && EndDateValue == null)
                {
                    isSkip = true;
                    dr["竣工时间"] = EndDate + "#error";
                }

                var SaleDate = dt.Rows[i]["销售时间"].ToString().Trim();
                var SaleDateValue = (DateTime?)TryParseHelper.StrToDateTime(SaleDate);
                datIndustryProject.SaleDate = SaleDateValue;
                dr["销售时间"] = SaleDate;
                if (!string.IsNullOrEmpty(SaleDate) && SaleDateValue == null)
                {
                    isSkip = true;
                    dr["销售时间"] = SaleDate + "#error";
                }

                var OfficeArea = dt.Rows[i]["办公面积_平方米"].ToString().Trim();
                var OfficeAreaValue = (decimal?)TryParseHelper.StrToDecimal(OfficeArea);
                datIndustryProject.OfficeArea = OfficeAreaValue;
                dr["办公面积_平方米"] = OfficeArea;
                if (!string.IsNullOrEmpty(OfficeArea) && OfficeAreaValue == null)
                {
                    isSkip = true;
                    dr["办公面积_平方米"] = OfficeArea + "#error";
                }

                var BizArea = dt.Rows[i]["商业面积_平方米"].ToString().Trim();
                var BizAreaValue = (decimal?)TryParseHelper.StrToDecimal(BizArea);
                datIndustryProject.BizArea = BizAreaValue;
                dr["商业面积_平方米"] = BizArea;
                if (!string.IsNullOrEmpty(BizArea) && BizAreaValue == null)
                {
                    isSkip = true;
                    dr["商业面积_平方米"] = BizArea + "#error";
                }

                var IndustryArea = dt.Rows[i]["工业面积_平方米"].ToString().Trim();
                var IndustryAreaValue = (decimal?)TryParseHelper.StrToDecimal(IndustryArea);
                datIndustryProject.IndustryArea = IndustryAreaValue;
                dr["工业面积_平方米"] = IndustryArea;
                if (!string.IsNullOrEmpty(IndustryArea) && IndustryAreaValue == null)
                {
                    isSkip = true;
                    dr["工业面积_平方米"] = IndustryArea + "#error";
                }

                var ManagerPrice = dt.Rows[i]["管理费"].ToString().Trim();
                datIndustryProject.ManagerPrice = ManagerPrice;
                dr["管理费"] = ManagerPrice;
                if (!(string.IsNullOrEmpty(ManagerPrice)) && ManagerPrice.Length > 50)
                {
                    isSkip = true;
                    dr["管理费"] = ManagerPrice + "#error";
                }

                var ManagerTel = dt.Rows[i]["管理处电话"].ToString().Trim();
                datIndustryProject.ManagerPrice = ManagerTel;
                dr["管理处电话"] = ManagerTel;
                if (!(string.IsNullOrEmpty(ManagerTel)) && ManagerTel.Length > 50)
                {
                    isSkip = true;
                    dr["管理处电话"] = ManagerTel + "#error";
                }

                var TrafficTypeName = dt.Rows[i]["交通便捷度"].ToString().Trim();
                var TrafficType = GetCodeByName(TrafficTypeName, SYS_Code_Dict._交通便捷度);
                datIndustryProject.TrafficType = TrafficType;
                datIndustryProject.TrafficTypeName = TrafficTypeName;
                dr["交通便捷度"] = TrafficTypeName;
                if (!string.IsNullOrEmpty(TrafficTypeName) && TrafficType <= 0)
                {
                    isSkip = true;
                    dr["交通便捷度"] = TrafficTypeName + "#error";
                }

                var TrafficDetails = dt.Rows[i]["交通便捷度描述"].ToString().Trim();
                datIndustryProject.TrafficDetails = TrafficDetails;
                dr["交通便捷度描述"] = TrafficDetails;
                if (!(string.IsNullOrEmpty(TrafficDetails)) && TrafficDetails.Length > 100)
                {
                    isSkip = true;
                    dr["交通便捷度描述"] = TrafficDetails + "#error";
                }

                var ParkingLevelName = dt.Rows[i]["停车便捷度"].ToString().Trim();
                var ParkingLevel = GetCodeByName(ParkingLevelName, SYS_Code_Dict._交通便捷度);
                datIndustryProject.ParkingLevel = ParkingLevel;
                datIndustryProject.ParkingLevelName = ParkingLevelName;
                dr["停车便捷度"] = ParkingLevelName;
                if (!string.IsNullOrEmpty(ParkingLevelName) && ParkingLevel <= 0)
                {
                    isSkip = true;
                    dr["停车便捷度"] = ParkingLevelName + "#error";
                }

                var ParkingTypeName = dt.Rows[i]["车位类型"].ToString().Trim();
                var ParkingType = GetCodeByName(ParkingTypeName, SYS_Code_Dict._停车位类型);
                datIndustryProject.ParkingType = ParkingType;
                datIndustryProject.ParkingTypeName = ParkingTypeName;
                dr["车位类型"] = ParkingTypeName;
                if (!string.IsNullOrEmpty(ParkingTypeName) && ParkingType <= 0)
                {
                    isSkip = true;
                    dr["车位类型"] = ParkingTypeName + "#error";
                }

                var ParkingPrice = dt.Rows[i]["停车费"].ToString().Trim();
                datIndustryProject.ParkingPrice = ParkingPrice;
                dr["停车费"] = ParkingPrice;
                if (!(string.IsNullOrEmpty(ParkingPrice)) && ParkingPrice.Length > 50)
                {
                    isSkip = true;
                    dr["停车费"] = ParkingPrice + "#error";
                }

                var RentSaleTypeName = dt.Rows[i]["租售方式"].ToString().Trim();
                var RentSaleType = GetCodeByName(RentSaleTypeName, SYS_Code_Dict._经营方式);
                datIndustryProject.RentSaleType = RentSaleType;
                datIndustryProject.RentSaleTypeName = RentSaleTypeName;
                dr["租售方式"] = RentSaleTypeName;
                if (!string.IsNullOrEmpty(RentSaleTypeName) && RentSaleType <= 0)
                {
                    isSkip = true;
                    dr["租售方式"] = RentSaleTypeName + "#error";
                }

                var AirConditionTypeName = dt.Rows[i]["空调系统类型"].ToString().Trim();
                var AirConditionType = GetCodeByName(AirConditionTypeName, SYS_Code_Dict._空调系统类型);
                datIndustryProject.AirConditionType = AirConditionType;
                datIndustryProject.AirConditionTypeName = AirConditionTypeName;
                dr["空调系统类型"] = AirConditionTypeName;
                if (!string.IsNullOrEmpty(AirConditionTypeName) && AirConditionType <= 0)
                {
                    isSkip = true;
                    dr["空调系统类型"] = AirConditionTypeName + "#error";
                }

                var Details = dt.Rows[i]["项目概况"].ToString().Trim();
                datIndustryProject.Details = Details;
                dr["项目概况"] = Details;
                if (!(string.IsNullOrEmpty(Details)) && Details.Length > 500)
                {
                    isSkip = true;
                    dr["项目概况"] = Details + "#error";
                }

                var East = dt.Rows[i]["四至东"].ToString().Trim();
                datIndustryProject.East = East;
                dr["四至东"] = East;
                if (!(string.IsNullOrEmpty(East)) && East.Length > 100)
                {
                    isSkip = true;
                    dr["四至东"] = East + "#error";
                }

                var south = dt.Rows[i]["四至南"].ToString().Trim();
                datIndustryProject.south = south;
                dr["四至南"] = south;
                if (!(string.IsNullOrEmpty(south)) && south.Length > 100)
                {
                    isSkip = true;
                    dr["四至南"] = south + "#error";
                }

                var west = dt.Rows[i]["四至西"].ToString().Trim();
                datIndustryProject.west = west;
                dr["四至西"] = west;
                if (!(string.IsNullOrEmpty(west)) && west.Length > 100)
                {
                    isSkip = true;
                    dr["四至西"] = west + "#error";
                }

                var north = dt.Rows[i]["四至北"].ToString().Trim();
                datIndustryProject.north = north;
                dr["四至北"] = north;
                if (!(string.IsNullOrEmpty(north)) && north.Length > 100)
                {
                    isSkip = true;
                    dr["四至北"] = north + "#error";
                }

                var X = dt.Rows[i]["经度"].ToString().Trim();
                datIndustryProject.X = (decimal?)TryParseHelper.StrToDecimal(X);
                dr["经度"] = X;
                if (!string.IsNullOrEmpty(X) && TryParseHelper.StrToDecimal(X) == null)
                {
                    isSkip = true;
                    dr["经度"] = X + "#error";
                }

                var Y = dt.Rows[i]["纬度"].ToString().Trim();
                datIndustryProject.Y = (decimal?)TryParseHelper.StrToDecimal(Y);
                dr["纬度"] = Y;
                if (!string.IsNullOrEmpty(Y) && TryParseHelper.StrToDecimal(Y) == null)
                {
                    isSkip = true;
                    dr["纬度"] = Y + "#error";
                }

                var Remarks = dt.Rows[i]["备注"].ToString().Trim();
                datIndustryProject.Remarks = Remarks;
                dr["备注"] = Remarks;
                if (!(string.IsNullOrEmpty(Remarks)) && Remarks.Length > 512)
                {
                    isSkip = true;
                    dr["备注"] = Remarks + "#error";
                }

                var AveragePrice = dt.Rows[i]["楼盘均价_元每平方米"].ToString().Trim();
                var AveragePriceValue = (decimal?)TryParseHelper.StrToDecimal(AveragePrice);
                datIndustryProject.AveragePrice = AveragePriceValue;
                dr["楼盘均价_元每平方米"] = AveragePrice;
                if (!string.IsNullOrEmpty(AveragePrice) && AveragePriceValue == null)
                {
                    isSkip = true;
                    dr["楼盘均价_元每平方米"] = AveragePrice + "#error";
                }

                if (isSkip)//该行数据中，至少有一列数据格式错误
                {
                    dtError.Rows.Add(dr);
                }
                else
                {
                    listTrue.Add(datIndustryProject);
                }
            }
        }    
    }
}
