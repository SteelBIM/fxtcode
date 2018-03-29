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
        public void BusinessStoreExcelUpload(int cityid, int fxtcompanyid, string filePath, string userid,
            string taskName)
        {
            var taskId = 0;

            try
            {
                //在任务列表创建一条记录
                var task = new DAT_ImportTask()
                {
                    TaskName = taskName,
                    ImportType = SYS_Code_Dict.批量导入类型.商铺,
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

                List<Dat_Tenant_Biz> listTrue;//正确数据
                DataTable dtError;//格式错误数据
                DataFilter(userid, cityid, fxtcompanyid, out listTrue, out dtError, data);

                //错误数据写入Excel
                var fileNamePath = string.Empty;
                if (dtError.Rows.Count > 0)
                {
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "商铺格式错误数据.xlsx";
                    var folder = MapPath("NeedHandledFiles/FailureData/" + fxtcompanyid);
                    fileNamePath = Path.Combine(folder, fileName);
                    excelHelper.CreateExcel(dtError, fileNamePath, folder);

                }

                var failureNum = 0;

                //正确数据写入表中
                // listTrue.ForEach(m => _businessStore.AddTenantBiz(m));
                foreach (var item in listTrue)
                {
                    var insertResult = _businessStore.AddTenantBiz(item);
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
                LogHelper.WriteLog("BusinessStoreExcelUploadTask", "", userid, cityid, fxtcompanyid, ex);
            }

        }

        private void DataFilter(string userId, int cityId, int fxtCompanyId, out List<Dat_Tenant_Biz> listTrue, out DataTable dtError, DataTable dt)
        {

            listTrue = new List<Dat_Tenant_Biz>();
            dtError = new DataTable();

            foreach (DataColumn column in dt.Columns)
                dtError.Columns.Add(column.Caption);


            for (var i = 0; i < dt.Rows.Count; i++)
            {

                var isError = false;
                var tenantBiz = new Dat_Tenant_Biz();
                var dr = dtError.NewRow();

                var areaName = dt.Rows[i]["*行政区"].ToString().Trim();
                var areaId = GetAreaId(cityId, areaName);
                dr["*行政区"] = areaName;
                tenantBiz.AreaId = areaId;
                if (string.IsNullOrEmpty(areaName) || areaId == -1)
                {
                    isError = true;
                    dr["*行政区"] = areaName + "#error";
                }

                var subAreaName = dt.Rows[i]["商圈"].ToString().Trim();
                var subAreaId = GetSubAreaId(areaName, areaId);
                dr["商圈"] = subAreaName;
                tenantBiz.SubAreaBizId = subAreaId;
                if (string.IsNullOrEmpty(subAreaName.Trim()) || subAreaId == -1)
                {
                    isError = true;
                    dr["商圈"] = subAreaName + "#error";
                }

                var projectName = dt.Rows[i]["*商业街"].ToString().Trim();
                var projectIds = GetProjectId(cityId, areaId, fxtCompanyId, projectName);
                var projectId = projectIds.FirstOrDefault();
                dr["*商业街"] = projectName;
                tenantBiz.ProjectId = projectId;
                if (string.IsNullOrEmpty(projectName) || projectId <= 0)
                {
                    isError = true;
                    dr["*商业街"] = projectName + "#error";
                }

                var buildingName = dt.Rows[i]["*商业楼栋"].ToString().Trim();
                var buildingId = GetBuildingId(cityId, fxtCompanyId, projectId, buildingName);
                dr["*商业楼栋"] = buildingName;
                tenantBiz.BuildingId = buildingId;
                if (string.IsNullOrEmpty(buildingName) || buildingId <= 0)
                {
                    isError = true;
                    dr["*商业楼栋"] = buildingName + "#error";
                }


                var houseName = dt.Rows[i]["*商业房号"].ToString().Trim();
                var houseId = _datHouseBiz.GetHouseId(buildingId, houseName, cityId, fxtCompanyId);
                dr["*商业房号"] = houseName;
                tenantBiz.HouseId = houseId;
                if (string.IsNullOrEmpty(houseName) || houseId == 0)
                {
                    isError = true;
                    dr["*商业房号"] = houseName + "#error";
                }

                var vacant = dt.Rows[i]["*是否空置"].ToString().Trim();
                var isVacant = vacant == "是" ? 1 : vacant == "否" ? 0 : -1;
                tenantBiz.IsVacant = isVacant;
                dr["*是否空置"] = vacant;
                if (string.IsNullOrEmpty(vacant) || isVacant == -1)
                {
                    isError = true;
                    dr["*是否空置"] = vacant + "#error";
                }


                var bizHouseName = dt.Rows[i]["*商铺名称"].ToString().Trim();
                dr["*商铺名称"] = bizHouseName;
                tenantBiz.BizHouseName = bizHouseName;
                if (string.IsNullOrEmpty(bizHouseName) || bizHouseName.Length > 50)
                {
                    isError = true;
                    dr["*商铺名称"] = bizHouseName + "#error";
                }

                var brandName = dt.Rows[i]["*品牌名称"].ToString().Trim();
                dr["*品牌名称"] = brandName;
                tenantBiz.BrandName = brandName;
                if (string.IsNullOrEmpty(brandName) || brandName.Length > 100)
                {
                    isError = true;
                    dr["*品牌名称"] = brandName + "#error";
                }

                bool flag = true;
                var bizName = dt.Rows[i]["*经营业态大类"].ToString().Trim();
                var bizCode = GetCodeByName(bizName, SYS_Code_Dict._经营业态大类);
                tenantBiz.BizCode = bizCode;
                dr["*经营业态大类"] = bizName;
                if (string.IsNullOrEmpty(bizName) || bizCode == -1)
                {
                    flag = false;
                    isError = true;
                    dr["*经营业态大类"] = bizName + "#error";
                }

                if (flag)
                {
                    var subBizName = dt.Rows[i]["经营业态小类"].ToString().Trim();
                    var subBizCode = GetSubCodeByCode(bizCode);
                    tenantBiz.BizSubCode = subBizCode;
                    dr["经营业态小类"] = subBizName;
                    if (string.IsNullOrEmpty(subBizName) || subBizCode == -1)
                    {
                        isError = true;
                        dr["经营业态小类"] = subBizName + "#error";
                    }
                }

                var salePrice = dt.Rows[i]["销售单价_元/平方米"].ToString().Trim();
                tenantBiz.SaleUnitPrice = (decimal?)TryParseHelper.StrToDecimal(salePrice);
                dr["销售单价_元/平方米"] = salePrice;
                if (!string.IsNullOrEmpty(salePrice) && TryParseHelper.StrToDecimal(salePrice) == null)
                {
                    isError = true;
                    dr["销售单价_元/平方米"] = salePrice + "#error";
                }

                var rent = dt.Rows[i]["租金单价_元*天/平方米"].ToString().Trim();
                tenantBiz.Rent = (decimal?)TryParseHelper.StrToDecimal(rent);
                dr["租金单价_元*天/平方米"] = rent;
                if (!string.IsNullOrEmpty(rent) && TryParseHelper.StrToDecimal(rent) == null)
                {
                    isError = true;
                    dr["租金单价_元*天/平方米"] = rent + "#error";
                }

                var buildingArea = dt.Rows[i]["租赁面积"].ToString().Trim();
                tenantBiz.BuildingArea = (decimal?)TryParseHelper.StrToDecimal(buildingArea);
                dr["租赁面积"] = buildingArea;
                if (!string.IsNullOrEmpty(buildingArea) && TryParseHelper.StrToDecimal(buildingArea) == null)
                {
                    isError = true;
                    dr["租赁面积"] = buildingArea + "#error";
                }


                var rentTypeName = dt.Rows[i]["租金方式"].ToString().Trim();
                var rentTypeCode = GetCodeByName(rentTypeName, SYS_Code_Dict._租金方式);
                tenantBiz.RentTypeCode = rentTypeCode;
                dr["租金方式"] = rentTypeName;
                if (!string.IsNullOrEmpty(rentTypeName) && rentTypeCode == -1)
                {
                    isError = true;
                    dr["租金方式"] = rentTypeName + "#error";
                }

                var tenantName = dt.Rows[i]["租客"].ToString().Trim();
                var tenantId = GetCompanyIdByName(cityId, tenantName);
                tenantBiz.BizCode = rentTypeCode;
                dr["租客"] = tenantName;
                if (!string.IsNullOrEmpty(tenantName) && tenantId == -1)
                {
                    isError = true;
                    dr["租客"] = tenantName + "#error";
                }

                var bizTypeName = dt.Rows[i]["消费定位"].ToString().Trim();
                var bizTypeCode = GetCodeByName(bizTypeName, SYS_Code_Dict._消费定位);
                tenantBiz.BizType = bizTypeCode;
                dr["消费定位"] = bizTypeName;
                if (!string.IsNullOrEmpty(bizTypeName) && bizTypeCode == -1)
                {
                    isError = true;
                    dr["消费定位"] = bizTypeName + "#error";
                }

                var joinDate = dt.Rows[i]["进驻时间"].ToString().Trim();
                tenantBiz.JoinDate = (DateTime?)TryParseHelper.StrToDateTime(joinDate);
                dr["进驻时间"] = joinDate;
                if (!string.IsNullOrEmpty(joinDate) && (DateTime?)TryParseHelper.StrToDateTime(joinDate) == null)
                {
                    isError = true;
                    dr["进驻时间"] = joinDate + "#error";
                }

                var surveyDate = dt.Rows[i]["调查时间"].ToString().Trim();
                tenantBiz.SurveyDate = (DateTime?)TryParseHelper.StrToDateTime(joinDate);
                dr["调查时间"] = joinDate;
                if (!string.IsNullOrEmpty(surveyDate) && (DateTime?)TryParseHelper.StrToDateTime(surveyDate) == null)
                {
                    isError = true;
                    dr["调查时间"] = surveyDate + "#error";
                }

                var surveyUser = dt.Rows[i]["调查人"].ToString().Trim();
                tenantBiz.SurveyUser = surveyUser;
                dr["调查人"] = surveyUser;
                if (!string.IsNullOrEmpty(surveyUser) && surveyUser.Length > 50)
                {
                    isError = true;
                    dr["调查人"] = surveyUser + "#error";
                }

                var remarks = dt.Rows[i]["备注"].ToString().Trim();
                tenantBiz.Remarks = remarks;
                dr["备注"] = remarks;
                if (!string.IsNullOrEmpty(remarks) && remarks.Length > 512)
                {
                    isError = true;
                    dr["备注"] = remarks + "#error";
                }

                tenantBiz.CityId = cityId;
                tenantBiz.FxtCompanyId = fxtCompanyId;
                tenantBiz.Creator = userId;
                tenantBiz.SaveUser = userId;


                if (isError)//该行数据中，至少有一列数据格式错误
                {
                    dtError.Rows.Add(dr);
                }
                else
                {
                    listTrue.Add(tenantBiz);
                }

            }


        }
    }
}
