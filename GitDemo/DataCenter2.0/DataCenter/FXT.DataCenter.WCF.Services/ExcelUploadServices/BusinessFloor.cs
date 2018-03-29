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
        public void BusinessFloorExcelUpload(int cityid, int fxtcompanyid, string filePath, string userid,
            string taskName)
        {
            var taskId = 0;

            try
            {
                //在任务列表创建一条记录  iscomplete:0,代表否；1，代表是
                var task = new DAT_ImportTask()
                {
                    //此处要加导入模块信息:住宅、商业
                    TaskName = taskName,
                    ImportType = SYS_Code_Dict.批量导入类型.商业楼层信息,
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

                List<Dat_Floor_Biz> correctData;//正确数据
                DataTable dataFormatError;//格式错误数据
                DataFilter(userid, cityid, fxtcompanyid, out correctData, out dataFormatError, data);
                //错误数据写入excel
                var fileNamePath = string.Empty;
                if (dataFormatError.Rows.Count > 0)
                {
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "商业楼层格式错误数据.xlsx";
                    var folder = MapPath("NeedHandledFiles/FailureData/" + fxtcompanyid);
                    fileNamePath = Path.Combine(folder, fileName);
                    excelHelper.CreateExcel(dataFormatError, fileNamePath, folder);
                }

                var failureNum = 0;

                //正确数据添加到数据表
                //correctData.ForEach(m => _datFloorBiz.AddDat_Floor_Biz(m));
                foreach (var item in correctData)
                {
                    bool isFloorNo = true;
                    isFloorNo = _datFloorBiz.ValidFloor((item.FloorNo).ToString(), "FloorNo", (item.BuildingId).ToString());
                    if (isFloorNo)
                    {
                        item.FloorId = _datFloorBiz.IsExistFloor((item.FloorNo).ToString(), "FloorNo", (item.BuildingId).ToString());
                        var updateResult = _datFloorBiz.UpdateDat_Floor_Biz(item, fxtcompanyid);
                        if (updateResult <= 0) failureNum = failureNum + 1;
                    }
                    else
                    {
                        var insertResult = _datFloorBiz.AddDat_Floor_Biz(item);
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
                //更新任务状态
                _importTask.UpdateTask(taskId, correctData.Count - failureNum, dataFormatError.Rows.Count, 0, relativePath, 1);
            }
            catch (Exception ex)
            {
                _importTask.UpdateTask(taskId, 0, 0, 0, "", -1);
                LogHelper.WriteLog("BusinessFloorExeclUpload", "", userid, cityid, fxtcompanyid, ex);
            }


        }

        private void DataFilter(string userId, int cityId, int fxtCompanyId, out List<Dat_Floor_Biz> listTrue, out DataTable dtError, DataTable dt)
        {
            listTrue = new List<Dat_Floor_Biz>();
            dtError = new DataTable();
            foreach (DataColumn column in dt.Columns)
                dtError.Columns.Add(column.Caption);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                bool IsSkip = false;
                var floor_biz = new Dat_Floor_Biz();
                DataRow dr = dtError.NewRow();
                #region 必填字段
                floor_biz.CityId = cityId;
                floor_biz.FxtCompanyId = fxtCompanyId;
                floor_biz.Valid = 1;
                floor_biz.CreateTime = DateTime.Now;
                floor_biz.Creator = userId;
                floor_biz.SaveUser = userId;
                #endregion

                var areaName = dt.Rows[i]["*行政区"].ToString().Trim();
                var areaId = GetAreaId(cityId, areaName);
                dr["*行政区"] = areaName;
                if (string.IsNullOrEmpty(areaName) || areaId == -1)
                {
                    IsSkip = true;
                    dr["*行政区"] = areaName + "#error";
                }

                var subAreaName = dt.Rows[i]["商圈"].ToString().Trim();
                var subAreaId = GetSubAreaId(subAreaName, areaId);
                dr["商圈"] = subAreaName;
                if (!string.IsNullOrEmpty(subAreaName) && new[] { 0, -1 }.Contains(subAreaId))
                {
                    IsSkip = true;
                    dr["商圈"] = subAreaName + "#error";
                }
                var projectName = dt.Rows[i]["*商业街"].ToString().Trim();
                var projectIds = GetProjectId(cityId, areaId, fxtCompanyId, projectName);
                var projectId = projectIds.FirstOrDefault();
                dr["*商业街"] = projectName;
                if (string.IsNullOrEmpty(projectName) || projectName.Length < 1 || projectId <= 0)
                {
                    IsSkip = true;
                    dr["*商业街"] = projectName + "#error";
                }
                var buildName = dt.Rows[i]["*楼栋名称"].ToString().Trim();
                var BuildingId = GetBuildingId(cityId, fxtCompanyId, projectId, buildName);
                floor_biz.BuildingId = BuildingId;
                dr["*楼栋名称"] = buildName;
                if (string.IsNullOrEmpty(buildName) || BuildingId == -1)
                {
                    IsSkip = true;
                    dr["*楼栋名称"] = buildName + "#error";
                }

                var BuildingBizTypeName = dt.Rows[i]["*楼层商业类型"].ToString().Trim();
                var BuildingBizType = GetCodeByName(BuildingBizTypeName, SYS_Code_Dict._商业细分类型);
                floor_biz.BuildingBizType = BuildingBizType;
                dr["*楼层商业类型"] = BuildingBizTypeName;
                if (string.IsNullOrEmpty(BuildingBizTypeName) || BuildingBizType == -1)
                {
                    IsSkip = true;
                    dr["*楼层商业类型"] = BuildingBizTypeName + "#error";
                }

                var FloorNo = dt.Rows[i]["*物理层"].ToString().Trim();
                floor_biz.FloorNo = (int)TryParseHelper.StrToInt32(FloorNo);
                //bool isFloorNo = true;
                //if (BuildingId != -1)
                //{
                //    isFloorNo = _datFloorBiz.ValidFloor(FloorNo, "FloorNo", BuildingId.ToString().Trim());
                //}
                dr["*物理层"] = FloorNo;
                if (string.IsNullOrEmpty(FloorNo) || TryParseHelper.StrToInt32(FloorNo) == null)
                {
                    IsSkip = true;
                    dr["*物理层"] = FloorNo + "#error";
                }

                var FloorNum = dt.Rows[i]["*实际层"].ToString().Trim();
                floor_biz.FloorNum = FloorNum;
                //bool isFloorNum = true;
                //if (BuildingId != -1)
                //{
                //    isFloorNum = _datFloorBiz.ValidFloor(FloorNum, "FloorNum", BuildingId.ToString().Trim());
                //}
                dr["*实际层"] = FloorNum;
                if (string.IsNullOrEmpty(FloorNum) || FloorNum.Length > 10)
                {
                    IsSkip = true;
                    dr["*实际层"] = FloorNum + "#error";
                }

                var BuildingArea = dt.Rows[i]["面积"].ToString().Trim();
                floor_biz.BuildingArea = (decimal?)TryParseHelper.StrToDecimal(BuildingArea);
                dr["面积"] = BuildingArea;
                if (!string.IsNullOrEmpty(BuildingArea) && TryParseHelper.StrToDecimal(BuildingArea) == null)
                {
                    IsSkip = true;
                    dr["面积"] = BuildingArea + "#error";
                }

                var FloorHigh = dt.Rows[i]["层高"].ToString().Trim();
                floor_biz.FloorHigh = (decimal?)TryParseHelper.StrToDecimal(FloorHigh);
                dr["层高"] = FloorHigh;
                if (!string.IsNullOrEmpty(FloorHigh) && TryParseHelper.StrToDecimal(FloorHigh) == null)
                {
                    IsSkip = true;
                    dr["层高"] = FloorHigh + "#error";
                }

                var RentSaleTypeName = dt.Rows[i]["租售方式"].ToString().Trim();
                var RentSaleType = GetCodeByName(RentSaleTypeName, SYS_Code_Dict._经营方式);
                floor_biz.RentSaleType = RentSaleType;
                dr["租售方式"] = RentSaleTypeName;
                if (!string.IsNullOrEmpty(RentSaleTypeName) && RentSaleType == -1)
                {
                    IsSkip = true;
                    dr["租售方式"] = RentSaleTypeName + "#error";
                }

                var BizTypeName = dt.Rows[i]["主营类型"].ToString().Trim();
                var BizType = GetCodeByName(BizTypeName, SYS_Code_Dict._商业细分类型);
                floor_biz.BizType = BizType;
                dr["主营类型"] = BizTypeName;
                if (!string.IsNullOrEmpty(BizTypeName) && BizType == -1)
                {
                    IsSkip = true;
                    dr["主营类型"] = BizTypeName + "#error";
                }

                var Remarks = dt.Rows[i]["备注"].ToString().Trim();
                floor_biz.Remarks = Remarks;
                dr["备注"] = Remarks;
                if (!string.IsNullOrEmpty(Remarks) && Remarks.Length > 500)
                {
                    IsSkip = true;
                    dr["备注"] = Remarks + "#error";
                }

                if (dt.Columns.Contains("楼层均价"))
                {
                    dr["楼层均价"] = dt.Rows[i]["楼层均价"].ToString();
                    decimal averageprice = 0;
                    if (!string.IsNullOrEmpty(dt.Rows[i]["楼层均价"].ToString()))
                    {
                        if (!decimal.TryParse(dt.Rows[i]["楼层均价"].ToString(), out averageprice))
                        {
                            IsSkip = true;
                            dr["楼层均价"] = dt.Rows[i]["楼层均价"].ToString() + "#error";
                        }
                        else
                        {
                            floor_biz.AveragePrice = averageprice;
                        }
                    }
                }
                decimal weight = 1;
                if (dt.Columns.Contains("价格系数"))
                {
                    dr["价格系数"] = dt.Rows[i]["价格系数"].ToString();
                    if (!string.IsNullOrEmpty(dt.Rows[i]["价格系数"].ToString()) && !decimal.TryParse(dt.Rows[i]["价格系数"].ToString(), out weight))
                    {
                        IsSkip = true;
                        dr["价格系数"] = dt.Rows[i]["价格系数"].ToString() + "#error";
                    }
                    else if (weight < 0 || weight > 99999.9999M)
                    {
                        IsSkip = true;
                        dr["价格系数"] = dt.Rows[i]["价格系数"].ToString() + "#error";
                    }
                    else
                    {
                        floor_biz.Weight = weight;
                    }
                }
                else
                {
                    floor_biz.Weight = weight;
                }

                if (IsSkip)//该行数据中，至少有一列数据格式错误
                {
                    dtError.Rows.Add(dr);
                }
                else
                {
                    listTrue.Add(floor_biz);
                }

            }


        }
    }
}
