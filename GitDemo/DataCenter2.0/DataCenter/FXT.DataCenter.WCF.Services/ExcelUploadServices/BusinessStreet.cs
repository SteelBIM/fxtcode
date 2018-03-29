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

        public void BusinessStreetExcelUpload(int cityid, int fxtcompanyid, string filePath, string userid,
            string taskName)
        {
            var taskId = 0;

            try
            {
                //在任务列表创建一条记录
                var task = new DAT_ImportTask()
                {
                    TaskName = taskName,
                    ImportType = SYS_Code_Dict.批量导入类型.商业街信息,
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

                List<Dat_Project_Biz> listTrue;//正确数据
                DataTable dtError;//格式错误数据
                DataFilter(userid, cityid, fxtcompanyid, out listTrue, out dtError, data);

                //错误数据写入Excel
                var fileNamePath = string.Empty;
                if (dtError.Rows.Count > 0)
                {
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "商业街格式错误数据.xlsx";
                    var folder = MapPath("NeedHandledFiles/FailureData/" + fxtcompanyid);
                    fileNamePath = Path.Combine(folder, fileName);
                    excelHelper.CreateExcel(dtError, fileNamePath, folder);
                }

                var failureNum = 0;

                //正确数据写入表中
                // listTrue.ForEach(m => _businessStore.AddprojectBiz(m));
                foreach (var item in listTrue)
                {
                    var projectId = _businessStreet.ValidateProjectBiz(item.AreaId, -1, item.ProjectName, item.CityId,
                        item.FxtCompanyId);
                    if (projectId > 0) //存在，则更新，不存在，则新增
                    {
                        item.ProjectId = projectId;
                        var modifyResult = _businessStreet.UpdateProjectBiz(item, fxtcompanyid);
                        if (modifyResult <= 0) failureNum = failureNum + 1;
                    }
                    else
                    {
                        item.CreateTime = DateTime.Now;
                        item.Creator = userid;
                        var insertResult = _businessStreet.AddProjectBiz(item);
                        if (insertResult <= 0) failureNum = failureNum + 1;
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
                _importTask.UpdateTask(taskId, listTrue.Count - failureNum, dtError.Rows.Count, 0, relativePath, 1);
            }
            catch (Exception ex)
            {
                _importTask.UpdateTask(taskId, 0, 0, 0, "", -1);
                LogHelper.WriteLog("BusinessStreetExcelUploadTask", "", userid, cityid, fxtcompanyid, ex);
            }
        }

        private void DataFilter(string userId, int cityId, int fxtCompanyId, out List<Dat_Project_Biz> listTrue, out DataTable dtError, DataTable dt)
        {

            listTrue = new List<Dat_Project_Biz>();
            dtError = new DataTable();

            foreach (DataColumn column in dt.Columns)
                dtError.Columns.Add(column.Caption);


            for (var i = 0; i < dt.Rows.Count; i++)
            {

                var isError = false;
                var projectBiz = new Dat_Project_Biz();
                var dr = dtError.NewRow();

                var areaName = dt.Rows[i]["*行政区"].ToString().Trim();
                var areaId = GetAreaId(cityId, areaName);
                dr["*行政区"] = areaName;
                projectBiz.AreaId = areaId;
                if (string.IsNullOrEmpty(areaName) || areaId == -1)
                {
                    isError = true;
                    dr["*行政区"] = areaName + "#error";
                }

                var subAreaName = dt.Rows[i]["商圈"].ToString().Trim();
                var subAreaId = GetSubAreaId(subAreaName, areaId);
                dr["商圈"] = subAreaName;
                projectBiz.SubAreaId = subAreaId;
                if (!string.IsNullOrEmpty(subAreaName) && new[] { 0, -1 }.Contains(subAreaId))
                {
                    isError = true;
                    dr["商圈"] = subAreaName + "#error";
                }

                var projectName = dt.Rows[i]["*商业街"].ToString().Trim();
                dr["*商业街"] = projectName;
                projectBiz.ProjectName = projectName;
                if (string.IsNullOrEmpty(projectName) || projectName.Length >= 100)
                {
                    isError = true;
                    dr["*商业街"] = projectName + "#error";
                }
                else
                {
                    projectBiz.PinYin = StringHelper.GetPYString(projectName);
                    projectBiz.PinYinAll = StringHelper.GetAllPYString(projectName);
                }

                var correlationTypeName = dt.Rows[i]["与商圈关联度"].ToString().Trim();
                var correlationType = GetCodeByName(correlationTypeName, SYS_Code_Dict._商圈关联度);
                projectBiz.CorrelationType = correlationType;
                dr["与商圈关联度"] = correlationTypeName;
                if (!string.IsNullOrEmpty(correlationTypeName) && correlationType == -1)
                {
                    isError = true;
                    dr["与商圈关联度"] = correlationTypeName + "#error";
                }


                var otherName = dt.Rows[i]["别名"].ToString().Trim();
                dr["别名"] = otherName;
                projectBiz.OtherName = otherName;
                if (otherName.Length >= 100)
                {
                    isError = true;
                    dr["别名"] = otherName + "#error";
                }

                var address = dt.Rows[i]["地址"].ToString().Trim();
                dr["地址"] = address;
                projectBiz.Address = address;
                if (address.Length >= 200)
                {
                    isError = true;
                    dr["地址"] = address + "#error";
                }


                var trafficTypeName = dt.Rows[i]["交通便捷度"].ToString().Trim();
                var trafficType = GetCodeByName(trafficTypeName, SYS_Code_Dict._交通便捷度);
                projectBiz.TrafficType = trafficType;
                dr["交通便捷度"] = trafficTypeName;
                if (!string.IsNullOrEmpty(trafficTypeName) && trafficType == -1)
                {
                    isError = true;
                    dr["交通便捷度"] = trafficTypeName + "#error";
                }

                var trafficDetails = dt.Rows[i]["交通便捷度描述"].ToString().Trim();
                dr["交通便捷度描述"] = trafficDetails;
                projectBiz.TrafficDetails = trafficDetails;
                if (trafficDetails.Length >= 100)
                {
                    isError = true;
                    dr["交通便捷度描述"] = trafficDetails + "#error";
                }



                var parkingLevelName = dt.Rows[i]["停车便捷度"].ToString().Trim();
                var parkingLevel = GetCodeByName(parkingLevelName, SYS_Code_Dict._交通便捷度);
                projectBiz.ParkingLevel = parkingLevel;
                dr["停车便捷度"] = parkingLevelName;
                if (!string.IsNullOrEmpty(parkingLevelName) && parkingLevel == -1)
                {
                    isError = true;
                    dr["停车便捷度"] = parkingLevelName + "#error";
                }


                var details = dt.Rows[i]["概况"].ToString().Trim();
                dr["概况"] = details;
                projectBiz.Details = details;
                if (details.Length >= 500)
                {
                    isError = true;
                    dr["概况"] = details + "#error";
                }

                var openDate = dt.Rows[i]["开业时间"].ToString().Trim();
                dr["开业时间"] = openDate;
                projectBiz.OpenDate = openDate;
                if (openDate.Length >= 50)
                {
                    isError = true;
                    dr["开业时间"] = openDate + "#error";
                }

                if (dt.Columns.Contains("土地起始日期"))
                {
                    var startDate = dt.Rows[i]["土地起始日期"].ToString().Trim();
                    projectBiz.StartDate = (DateTime?)TryParseHelper.StrToDateTime(startDate);
                    dr["土地起始日期"] = startDate;
                    if (!string.IsNullOrEmpty(startDate) && (DateTime?)TryParseHelper.StrToDateTime(startDate) == null)
                    {
                        isError = true;
                        dr["土地起始日期"] = startDate + "#error";
                    }
                }

                if (dt.Columns.Contains("土地终止日期"))
                {
                    var startendDate = dt.Rows[i]["土地终止日期"].ToString().Trim();
                    projectBiz.StartEndDate = (DateTime?)TryParseHelper.StrToDateTime(startendDate);
                    dr["土地终止日期"] = startendDate;
                    if (!string.IsNullOrEmpty(startendDate) && (DateTime?)TryParseHelper.StrToDateTime(startendDate) == null)
                    {
                        isError = true;
                        dr["土地终止日期"] = startendDate + "#error";
                    }
                }

                var areaDetails = dt.Rows[i]["区位情况"].ToString().Trim();
                dr["区位情况"] = areaDetails;
                projectBiz.AreaDetails = areaDetails;
                if (areaDetails.Length >= 500)
                {
                    isError = true;
                    dr["区位情况"] = areaDetails + "#error";
                }

                var east = dt.Rows[i]["四至东"].ToString().Trim();
                dr["四至东"] = east;
                projectBiz.East = east;
                if (east.Length >= 100)
                {
                    isError = true;
                    dr["四至东"] = east + "#error";
                }

                var south = dt.Rows[i]["四至南"].ToString().Trim();
                dr["四至南"] = south;
                projectBiz.south = south;
                if (south.Length >= 100)
                {
                    isError = true;
                    dr["四至南"] = south + "#error";
                }


                var west = dt.Rows[i]["四至西"].ToString().Trim();
                dr["四至西"] = west;
                projectBiz.west = west;
                if (west.Length >= 100)
                {
                    isError = true;
                    dr["四至西"] = west + "#error";
                }

                var north = dt.Rows[i]["四至北"].ToString().Trim();
                dr["四至北"] = north;
                projectBiz.north = north;
                if (north.Length >= 100)
                {
                    isError = true;
                    dr["四至北"] = north + "#error";
                }

                var X = dt.Rows[i]["经度"].ToString().Trim();
                projectBiz.X = (decimal?)TryParseHelper.StrToDecimal(X);
                dr["经度"] = X;
                if (!string.IsNullOrEmpty(X) && TryParseHelper.StrToDecimal(X) == null)
                {
                    isError = true;
                    dr["经度"] = X + "#error";
                }

                var Y = dt.Rows[i]["纬度"].ToString().Trim();
                projectBiz.Y = (decimal?)TryParseHelper.StrToDecimal(Y);
                dr["纬度"] = Y;
                if (!string.IsNullOrEmpty(Y) && TryParseHelper.StrToDecimal(Y) == null)
                {
                    isError = true;
                    dr["纬度"] = Y + "#error";
                }

                var IsTypicalName = dt.Rows[i]["是否标杆"].ToString().Trim();
                var IsTypical = -1;
                if (IsTypicalName == "是") IsTypical = 1;
                else if (IsTypicalName == "否") IsTypical = 0;
                projectBiz.IsTypical = IsTypical;
                dr["是否标杆"] = IsTypicalName;
                if (!string.IsNullOrEmpty(IsTypicalName) && IsTypical == -1)
                {
                    isError = true;
                    dr["是否标杆"] = IsTypicalName + "#error";
                }

                var Remarks = dt.Rows[i]["备注"].ToString().Trim();
                projectBiz.Remarks = Remarks;
                dr["备注"] = Remarks;
                if (!string.IsNullOrEmpty(Remarks) && Remarks.Length > 512)
                {
                    isError = true;
                    dr["备注"] = Remarks + "#error";
                }

                if (dt.Columns.Contains("项目均价"))
                {
                    dr["项目均价"] = dt.Rows[i]["项目均价"].ToString();
                    decimal averageprice = 0;
                    if (!string.IsNullOrEmpty(dt.Rows[i]["项目均价"].ToString()))
	                {
                        if (!decimal.TryParse(dt.Rows[i]["项目均价"].ToString(),out averageprice))
	                    {
                            isError = true;
                            dr["项目均价"] = dt.Rows[i]["项目均价"].ToString() + "#error";
	                    }
                        else
                        {
                            projectBiz.AveragePrice = averageprice;  
                        }
	                }
                }
                decimal weight = 1;
                if (dt.Columns.Contains("价格系数"))
                {
                    dr["价格系数"] = dt.Rows[i]["价格系数"].ToString();
                    if (!string.IsNullOrEmpty(dt.Rows[i]["价格系数"].ToString()) && !decimal.TryParse(dt.Rows[i]["价格系数"].ToString(), out weight))
                    {
                        isError = true;
                        dr["价格系数"] = dt.Rows[i]["价格系数"].ToString() + "#error";
                    }
                    else if (weight < 0 || weight > 99999.9999M)
                    {
                        isError = true;
                        dr["价格系数"] = dt.Rows[i]["价格系数"].ToString() + "#error";
                    }
                    else
                    {
                        projectBiz.Weight = weight;
                    }
                }
                else
                {
                    projectBiz.Weight = weight;
                }

                projectBiz.CityId = cityId;
                projectBiz.FxtCompanyId = fxtCompanyId;
                projectBiz.Creator = userId;
                projectBiz.SaveUser = userId;

                if (isError)//该行数据中，至少有一列数据格式错误
                {
                    dtError.Rows.Add(dr);
                }
                else
                {
                    listTrue.Add(projectBiz);
                }
            }
        }
    }
}
