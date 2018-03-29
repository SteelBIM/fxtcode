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
        public void IndustryDynamicPriceExcelUpload(int cityid, int fxtcompanyid, string filePath, string userid, string taskName)
        {
            var taskId = 0;

            try
            {
                //在任务列表创建一条记录
                var task = new DAT_ImportTask()
                {
                    TaskName = taskName,
                    ImportType = SYS_Code_Dict.批量导入类型.工业动态价格,
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

                List<DatPBPriceIndustry> listTrue;//正确数据
                DataTable dtError;//格式错误数据
                DataFilter(userid,cityid, fxtcompanyid, out listTrue, out dtError, data);

                //错误数据写入Excel
                var fileNamePath = string.Empty;
                if (dtError.Rows.Count > 0)
                {
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "工业动态价格调查错误数据.xlsx";
                    var folder = MapPath("NeedHandledFiles/FailureData/" + fxtcompanyid);
                    fileNamePath = Path.Combine(folder, fileName);
                    excelHelper.CreateExcel(dtError, fileNamePath, folder);
                }

                var failureNum = 0;

                //正确数据添加到表中
                foreach (var item in listTrue)
                {
                    item.CityId = cityid;
                    item.FxtCompanyId = fxtcompanyid;
                    item.Creator = userid;
                    item.CreateTime = DateTime.Now;
                    var insertResult = _industryDynamicPrice.AddDynamicPriceSurvey(item);
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
                LogHelper.WriteLog("IndustryDynamicPriceExcelUploadTask", "", userid, cityid, fxtcompanyid, ex);
            }
        }

        private void DataFilter(string userId,int cityId, int fxtCompanyId, out List<DatPBPriceIndustry> listTrue, out DataTable dtError, DataTable dt)
        {

            listTrue = new List<DatPBPriceIndustry>();
            dtError = new DataTable();

            foreach (DataColumn column in dt.Columns)
                dtError.Columns.Add(column.Caption);

            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var isError = false;
                var dynamicPriceSurvey = new DatPBPriceIndustry
                {
                    CreateTime = DateTime.Now,
                    Creator = userId,
                    SaveDateTime = DateTime.Now,
                    SaveUser = userId
                };

                var dr = dtError.NewRow();

                var areaName = dt.Rows[i]["*行政区"].ToString().Trim();
                var areaId = GetAreaId(cityId, areaName);
                dr["*行政区"] = areaName;
                if (string.IsNullOrEmpty(areaName) || areaId == -1)
                {
                    isError = true;
                    dr["*行政区"] = areaName + "#error";
                }

                var projectName = dt.Rows[i]["*工业楼盘"].ToString().Trim();
                var projectId = GetProjectIdByName_Industry(projectName, areaId, cityId, fxtCompanyId).FirstOrDefault();
                dr["*工业楼盘"] = projectName;
                dynamicPriceSurvey.ProjectId = projectId;
                if (string.IsNullOrEmpty(projectName) || projectId == 0)
                {
                    isError = true;
                    dr["*工业楼盘"] = projectName + "#error";
                }

                var buildingName = dt.Rows[i]["工业楼栋"].ToString().Trim();
                var buildingId = GetBuildingIdByName_Industry(projectId, buildingName, cityId, fxtCompanyId);
                dr["工业楼栋"] = buildingName;
                dynamicPriceSurvey.BuildingId = buildingId == -1 ? 0 : buildingId;
                if (!string.IsNullOrEmpty(buildingName) && buildingId == -1)
                {
                    isError = true;
                    dr["工业楼栋"] = buildingName + "#error";
                }
                
                var surveyTypeName = dt.Rows[i]["*调查方式"].ToString().Trim();
                var surveyTypeCode = GetCodeByName(surveyTypeName, SYS_Code_Dict._调查方式);
                dynamicPriceSurvey.SurveyTypeCode = surveyTypeCode;
                dr["*调查方式"] = surveyTypeName;
                if (string.IsNullOrEmpty(surveyTypeName) || surveyTypeCode == -1)
                {
                    isError = true;
                    dr["*调查方式"] = surveyTypeName + "#error";
                }

                var avgRentStr = dt.Rows[i]["*平均租金_元每平方米*日"].ToString().Trim();
                var avgRent = TryParseHelper.StrToDecimal(avgRentStr, -1);
                dr["*平均租金_元每平方米*日"] = avgRentStr;
                dynamicPriceSurvey.AvgRent = avgRent;
                if (string.IsNullOrEmpty(avgRentStr))
                {
                    isError = true;
                    dr["*平均租金_元每平方米*日"] = avgRentStr + "#error";
                }

                var rent1 = dt.Rows[i]["最低日租金_元每平方米"].ToString().Trim();
                dynamicPriceSurvey.Rent1 = (decimal?)TryParseHelper.StrToDecimal(rent1);
                dr["最低日租金_元每平方米"] = rent1;
                if (!string.IsNullOrEmpty(rent1) && TryParseHelper.StrToDecimal(rent1) == null)
                {
                    isError = true;
                    dr["最低日租金_元每平方米"] = rent1 + "#error";
                }

                var rent2 = dt.Rows[i]["最高日租金_元每平方米"].ToString().Trim();
                dynamicPriceSurvey.Rent2 = (decimal?)TryParseHelper.StrToDecimal(rent2);
                dr["最高日租金_元每平方米"] = rent2;
                if (!string.IsNullOrEmpty(rent2) && TryParseHelper.StrToDecimal(rent2) == null)
                {
                    isError = true;
                    dr["最高日租金_元每平方米"] = rent2 + "#error";
                }

                var avgSalePrice = dt.Rows[i]["平均售价_元每平方米"].ToString().Trim();
                dynamicPriceSurvey.AvgSalePrice = (decimal?)TryParseHelper.StrToDecimal(avgSalePrice);
                dr["平均售价_元每平方米"] = avgSalePrice;
                if (!string.IsNullOrEmpty(avgSalePrice) && TryParseHelper.StrToDecimal(avgSalePrice) == null)
                {
                    isError = true;
                    dr["平均售价_元每平方米"] = avgSalePrice + "#error";
                }

                var salePrice1 = dt.Rows[i]["最低售价_元每平方米"].ToString().Trim();
                dynamicPriceSurvey.SalePrice1 = (decimal?)TryParseHelper.StrToDecimal(salePrice1);
                dr["最低售价_元每平方米"] = salePrice1;
                if (!string.IsNullOrEmpty(salePrice1) && TryParseHelper.StrToDecimal(salePrice1) == null)
                {
                    isError = true;
                    dr["最低售价_元每平方米"] = salePrice1 + "#error";
                }

                var salePrice2 = dt.Rows[i]["最高售价_元每平方米"].ToString().Trim();
                dynamicPriceSurvey.SalePrice2 = (decimal?)TryParseHelper.StrToDecimal(salePrice2);
                dr["最高售价_元每平方米"] = salePrice2;
                if (!string.IsNullOrEmpty(salePrice2) && TryParseHelper.StrToDecimal(salePrice2) == null)
                {
                    isError = true;
                    dr["最高售价_元每平方米"] = salePrice2 + "#error";
                }

                var rentSaleRate = dt.Rows[i]["平均租售比_月租金/销售单价"].ToString().Trim();
                dynamicPriceSurvey.RentSaleRate = (decimal?)TryParseHelper.StrToDecimal(rentSaleRate);
                dr["平均租售比_月租金/销售单价"] = rentSaleRate;
                if (!string.IsNullOrEmpty(rentSaleRate) && TryParseHelper.StrToDecimal(rentSaleRate) == null)
                {
                    isError = true;
                    dr["平均租售比_月租金/销售单价"] = rentSaleRate + "#error";
                }

                var tenantArea = dt.Rows[i]["已租面积_平方米"].ToString().Trim();
                dynamicPriceSurvey.TenantArea = (decimal?)TryParseHelper.StrToDecimal(tenantArea);
                dr["已租面积_平方米"] = tenantArea;
                if (!string.IsNullOrEmpty(tenantArea) && TryParseHelper.StrToDecimal(tenantArea) == null)
                {
                    isError = true;
                    dr["已租面积_平方米"] = tenantArea + "#error";
                }

                var vacantArea = dt.Rows[i]["空置面积_平方米"].ToString().Trim();
                dynamicPriceSurvey.VacantArea = (decimal?)TryParseHelper.StrToDecimal(vacantArea);
                dr["空置面积_平方米"] = vacantArea;
                if (!string.IsNullOrEmpty(vacantArea) && TryParseHelper.StrToDecimal(vacantArea) == null)
                {
                    isError = true;
                    dr["空置面积_平方米"] = vacantArea + "#error";
                }

                var vacantRate = dt.Rows[i]["空置率_百分比"].ToString().Trim();
                dynamicPriceSurvey.VacantRate = (decimal?)TryParseHelper.StrToDecimal(vacantRate);
                dr["空置率_百分比"] = vacantRate;
                if (!string.IsNullOrEmpty(vacantRate) && TryParseHelper.StrToDecimal(vacantRate) == null)
                {
                    isError = true;
                    dr["空置率_百分比"] = vacantArea + "#error";
                }

                var managerPrice = dt.Rows[i]["管理费_元每平方米*月"].ToString().Trim();
                dynamicPriceSurvey.ManagerPrice = (decimal?)TryParseHelper.StrToDecimal(managerPrice);
                dr["管理费_元每平方米*月"] = managerPrice;
                if (!string.IsNullOrEmpty(managerPrice) && TryParseHelper.StrToDecimal(managerPrice) == null)
                {
                    isError = true;
                    dr["管理费_元每平方米*月"] = managerPrice + "#error";
                }

                string surveyDate = dt.Rows[i]["*调查时间"].ToString().Trim();
                var surveyDateValue = (DateTime?)TryParseHelper.StrToDateTime(surveyDate);
                dynamicPriceSurvey.SurveyDate = surveyDateValue;
                dr["*调查时间"] = surveyDate;
                if (string.IsNullOrEmpty(surveyDate) || surveyDateValue == null)
                {
                    isError = true;
                    dr["*调查时间"] = "时间错误" + surveyDate + "#error";
                }

                var surveyUser = dt.Rows[i]["调查人"].ToString().Trim();
                dynamicPriceSurvey.SurveyUser = surveyUser.Length > 50 ? surveyUser.Substring(0, 50) : surveyUser;
                dr["调查人"] = surveyUser;


                if (isError)//该行数据中，至少有一列数据格式错误
                {
                    dtError.Rows.Add(dr);
                }
                else
                {
                    listTrue.Add(dynamicPriceSurvey);
                }
            }
        }
    }
}
