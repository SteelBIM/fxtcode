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
        public void IndustryTenantUpload(int cityid, int fxtcompanyid, string filePath, string userid, string taskName)
        {
            var taskId = 0;

            try
            {
                //在任务列表创建一条记录
                var task = new DAT_ImportTask()
                {
                    TaskName = taskName,
                    ImportType = SYS_Code_Dict.批量导入类型.工业租客信息,
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

                List<DatTenantIndustry> listTrue;//正确数据
                DataTable dtError;//格式错误数据
                DataFilter(cityid, fxtcompanyid, out listTrue, out dtError, data);

                //错误数据写入Excel
                var fileNamePath = string.Empty;
                if (dtError.Rows.Count > 0)
                {
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "工业租客错误数据.xlsx";
                    var folder = MapPath("NeedHandledFiles/FailureData/" + fxtcompanyid);
                    fileNamePath = Path.Combine(folder, fileName);
                    excelHelper.CreateExcel(dtError, fileNamePath, folder);
                }
                
                //正确数据写入表中
                foreach (var item in listTrue)
                {
                    var areaid = GetAreaId(cityid, item.AreaName);
                    var tenantId = GetCompanyIdByName_Industry(cityid, item.TenantName);

                    if (tenantId <= 0)
                    {
                        var company = new DAT_Company();
                        company.ChineseName = item.TenantName;
                        company.CreateDate = DateTime.Now;
                        company.CityId = cityid;
                        company.FxtCompanyId = fxtcompanyid;
                        _company.AddCompany(company);
                        tenantId = GetCompanyIdByName_Industry(cityid, item.TenantName);
                    }
                    item.TenantID = tenantId;
                    item.Creator = userid;
                    _industryTenant.AddTenantIndustry(item);
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
                LogHelper.WriteLog("IndustryTenantExcelUploadTask", "", userid, cityid, fxtcompanyid, ex);
            }
        }

        private void DataFilter(int cityId, int fxtCompanyId, out List<DatTenantIndustry> listTrue, out DataTable dtError, DataTable dt)
        {
            listTrue = new List<DatTenantIndustry>();
            dtError = new DataTable();
            foreach (DataColumn column in dt.Columns)
                dtError.Columns.Add(column.Caption);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var isSkip = false;
                var datTenant = new DatTenantIndustry();
                var dr = dtError.NewRow();

                datTenant.CityId = cityId;
                datTenant.FxtCompanyId = fxtCompanyId;
                datTenant.CreateTime = DateTime.Now;
                datTenant.SaveDateTime = DateTime.Now;

                var areaName = dt.Rows[i]["*行政区"].ToString().Trim();
                var areaId = GetAreaId(cityId, areaName);
                datTenant.AreaId = areaId;
                datTenant.AreaName = areaName;
                dr["*行政区"] = areaName;
                if (string.IsNullOrEmpty(areaName) || areaId <= 0)
                {
                    isSkip = true;
                    dr["*行政区"] = areaName + "#error";
                }
                
                var subAreaName = dt.Rows[i]["工业片区"].ToString().Trim();
                var subAreaId = GetSubAreaIdByName_Industry(subAreaName, areaId, fxtCompanyId);
                datTenant.SubAreaId = subAreaId;
                datTenant.SubAreaName = subAreaName;
                dr["工业片区"] = subAreaName;
                if (string.IsNullOrEmpty(subAreaName) || subAreaId <= 0)
                {
                    isSkip = true;
                    dr["工业片区"] = subAreaName + "#error";
                }

                var projectName = dt.Rows[i]["*工业楼盘"].ToString().Trim();
                var projectId = GetProjectIdByName_Industry(projectName, areaId, cityId, fxtCompanyId).FirstOrDefault();
                datTenant.ProjectId = projectId;
                datTenant.ProjectName = projectName;
                dr["*工业楼盘"] = projectName;
                if (string.IsNullOrEmpty(projectName) || projectId <= 0)
                {
                    isSkip = true;
                    dr["*工业楼盘"] = projectName + "#error";
                }

                var buildingName = dt.Rows[i]["*工业楼栋"].ToString().Trim();
                var buildingId = GetBuildingIdByName_Industry(projectId, buildingName, cityId, fxtCompanyId);
                datTenant.BuildingId = buildingId;
                datTenant.BuildingName = buildingName;
                dr["*工业楼栋"] = buildingName;
                if (string.IsNullOrEmpty(buildingName) || buildingId <= 0)
                {
                    isSkip = true;
                    dr["*工业楼栋"] = buildingName + "#error";
                }
                
                var floorNum = dt.Rows[i]["楼层"].ToString().Trim();
                datTenant.FloorNum = floorNum;
                dr["楼层"] = floorNum;
                if (floorNum.Length > 10)
                {
                    isSkip = true;
                    dr["楼层"] = floorNum + "#error";
                }

                var houseName = dt.Rows[i]["房号"].ToString().Trim();
                datTenant.HouseName = houseName;
                dr["房号"] = houseName;
                if (houseName.Length > 30)
                {
                    isSkip = true;
                    dr["房号"] = houseName + "#error";
                }

                var tenantName = dt.Rows[i]["*租客名称"].ToString().Trim();
                datTenant.TenantName = tenantName;
                dr["*租客名称"] = tenantName;
                if (string.IsNullOrEmpty(tenantName) || tenantName.Length > 200)
                {
                    isSkip = true;
                    dr["*租客名称"] = tenantName + "#error";
                }

                var isVacant = dt.Rows[i]["是否空置"].ToString().Trim();
                datTenant.IsVacant = isVacant == "是" ? 1 : (isVacant == "否" ? 0 : -1);
                dr["是否空置"] = isVacant;
                if (!string.IsNullOrEmpty(isVacant) && !(isVacant == "是" || isVacant == "否"))
                {
                    isSkip = true;
                    dr["是否空置"] = isVacant + "#error";
                }

                var buildingArea = dt.Rows[i]["租赁面积_平方米"].ToString().Trim();
                var buildingAreaValue = (decimal?)TryParseHelper.StrToDecimal(buildingArea);
                datTenant.BuildingArea = buildingAreaValue;
                dr["租赁面积_平方米"] = buildingArea;
                if (!string.IsNullOrEmpty(buildingArea) && buildingAreaValue == null)
                {
                    isSkip = true;
                    dr["租赁面积_平方米"] = buildingArea + "#error";
                }

                var rent = dt.Rows[i]["租金单价_元每平方米*月"].ToString().Trim();
                var rentValue = (decimal?)TryParseHelper.StrToDecimal(rent);
                datTenant.Rent = rentValue;
                dr["租金单价_元每平方米*月"] = rent;
                if (!string.IsNullOrEmpty(rent) && rentValue == null)
                {
                    isSkip = true;
                    dr["租金单价_元每平方米*月"] = rent + "#error";
                }

                var typeCodeName = dt.Rows[i]["*行业大类"].ToString().Trim();
                var typeCode = GetCodeByName(typeCodeName, SYS_Code_Dict._行业大类);
                datTenant.TypeCode = typeCode;
                dr["*行业大类"] = typeCodeName;
                if (string.IsNullOrEmpty(typeCodeName) || (!string.IsNullOrEmpty(typeCodeName) && typeCode <= 0))
                {
                    isSkip = true;
                    dr["*行业大类"] = typeCodeName + "#error";
                }

                var subTypeCodeName = dt.Rows[i]["行业小类"].ToString().Trim();
                var subCode = GetSubCodeByCode(typeCode);
                var subTypeCode = GetCodeByName(subTypeCodeName, subCode);
                datTenant.SubTypeCode = subTypeCode;
                dr["行业小类"] = subTypeCodeName;
                if (!string.IsNullOrEmpty(subTypeCodeName) && subTypeCode <= 0)
                {
                    isSkip = true;
                    dr["行业小类"] = subTypeCodeName + "#error";
                }

                var joinDate = dt.Rows[i]["进驻时间"].ToString().Trim();
                var joinDateValue = (DateTime?)TryParseHelper.StrToDateTime(joinDate);
                datTenant.JoinDate = joinDateValue;
                dr["进驻时间"] = joinDate;
                if (!string.IsNullOrEmpty(joinDate) && joinDateValue == null)
                {
                    isSkip = true;
                    dr["进驻时间"] = joinDate + "#error";
                }

                var surveyDate = dt.Rows[i]["*调查时间"].ToString().Trim();
                var surveyDateValue = (DateTime)TryParseHelper.StrToDateTime(surveyDate);
                datTenant.SurveyDate = surveyDateValue;
                dr["*调查时间"] = surveyDate;
                if (string.IsNullOrEmpty(surveyDate) || surveyDateValue == null)
                {
                    isSkip = true;
                    dr["*调查时间"] = surveyDate + "#error";
                }

                var surveyUser = dt.Rows[i]["调查人"].ToString().Trim();
                datTenant.SurveyUser = surveyUser;
                dr["调查人"] = surveyUser;
                if (surveyUser.Length > 50)
                {
                    isSkip = true;
                    dr["调查人"] = surveyUser + "#error";
                }

                var Remarks = dt.Rows[i]["备注"].ToString().Trim();
                datTenant.Remarks = Remarks;
                dr["备注"] = Remarks;
                if (Remarks.Length > 512)
                {
                    isSkip = true;
                    dr["备注"] = Remarks + "#error";
                }

                var isTypical = dt.Rows[i]["是否典型租客"].ToString().Trim();
                datTenant.IsTypical = isTypical == "是" ? 1 : (isTypical == "否" ? 0 : -1);
                dr["是否典型租客"] = isTypical;
                if (!string.IsNullOrEmpty(isTypical) && !(isTypical == "是" || isTypical == "否"))
                {
                    isSkip = true;
                    dr["是否典型租客"] = isTypical + "#error";
                }

                if (isSkip)//该行数据中，至少有一列数据格式错误
                {
                    dtError.Rows.Add(dr);
                }
                else
                {
                    listTrue.Add(datTenant);
                }
            }
        }
    }
}