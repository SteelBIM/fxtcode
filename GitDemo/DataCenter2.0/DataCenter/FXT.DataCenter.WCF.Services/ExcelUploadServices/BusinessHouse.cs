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
        public void BusinessHouseExcelUpload(int cityid, int fxtcompanyid, string filePath, string userid,
            string taskName)
        {
            var taskId = 0;

            try
            {
                //在任务列表创建一条记录  iscomplete:0,代表否；1，代表是
                var task = new DAT_ImportTask
                {
                    //此处要加导入模块信息:住宅、商业
                    TaskName = taskName,
                    ImportType = SYS_Code_Dict.批量导入类型.商业房号信息,
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

                List<Dat_House_Biz> correctData;//正确数据
                DataTable dataFormatError;//格式错误数据
                DataFilter(userid, cityid, fxtcompanyid, out correctData, out dataFormatError, data);
                //错误数据写入excel
                string fileNamePath = string.Empty;
                if (dataFormatError.Rows.Count > 0)
                {
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "商业房号格式错误数据.xlsx";
                    var folder = MapPath("NeedHandledFiles/FailureData/" + fxtcompanyid);
                    fileNamePath = Path.Combine(folder, fileName);
                    excelHelper.CreateExcel(dataFormatError, fileNamePath, folder);
                }

                var failureNum = 0;

                //正确数据添加到数据表
                //correctData.ForEach(m => _datHouseBiz.AddDat_House_Biz(m));
                foreach (var item in correctData)
                {
                    var isExist = IsExistHouseName(item.BuildingId, item.FloorId, item.HouseName, item.CityId, item.FxtCompanyId);
                    if (isExist)
                    {
                        item.HouseId = _datHouseBiz.IsExistHouseId(item.BuildingId, item.FloorId, item.HouseName, item.CityId, item.FxtCompanyId);
                        var modifyResult = _datHouseBiz.UpdateDat_House_Biz(item, fxtcompanyid);
                        if (modifyResult <= 0) failureNum = failureNum + 1;
                    }
                    else
                    {
                        var modifyResult = _datHouseBiz.AddDat_House_Biz(item);
                        if (modifyResult <= 0) failureNum = failureNum + 1;
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

                if (fxtcompanyid == 25 || fxtcompanyid == 365)
                {
                    LogHelper.WriteLog("ProjectExcelUpload", "", userid, cityid, fxtcompanyid, new Exception("test "));
                }

                //更新任务状态
                _importTask.UpdateTask(taskId, correctData.Count - failureNum, dataFormatError.Rows.Count, 0, relativePath, 1);
            }
            catch (Exception ex)
            {
                _importTask.UpdateTask(taskId, 0, 0, 0, ex.Message, -1);
                LogHelper.WriteLog("BusinessHouseExeclUpload", "", userid, cityid, fxtcompanyid, ex);
            }
        }

        private void DataFilter(string userId, int cityId, int fxtCompanyId, out List<Dat_House_Biz> listTrue, out DataTable dtError, DataTable dt)
        {
            listTrue = new List<Dat_House_Biz>();
            dtError = new DataTable();
            foreach (DataColumn column in dt.Columns)
                dtError.Columns.Add(column.Caption);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                bool IsSkip = false;
                var house_biz = new Dat_House_Biz();
                DataRow dr = dtError.NewRow();
                #region 必填字段
                house_biz.CityId = cityId;
                house_biz.FxtCompanyId = fxtCompanyId;
                house_biz.Valid = 1;
                house_biz.CreateTime = DateTime.Now;
                house_biz.Creator = userId;
                house_biz.SaveUser = userId;
                #endregion

                var areaName = dt.Rows[i]["*行政区"].ToString().Trim();
                var areaId = GetAreaId(cityId, areaName);
                dr["*行政区"] = areaName;
                if (string.IsNullOrEmpty(areaName.Trim()) || areaId == 0)
                {
                    IsSkip = true;
                    dr["*行政区"] = areaName + "#error";
                }

                var subAreaName = dt.Rows[i]["商圈"].ToString().Trim();
                var subAreaId = GetSubAreaId(areaName, areaId);
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
                house_biz.BuildingId = BuildingId;
                dr["*楼栋名称"] = buildName;
                if (string.IsNullOrEmpty(buildName) || BuildingId <= 0)
                {
                    IsSkip = true;
                    dr["*楼栋名称"] = buildName + "#error";
                }

                var floorNo = dt.Rows[i]["*物理层"].ToString().Trim();
                var FloorId = GetFloorIdByName(BuildingId, floorNo, cityId, fxtCompanyId);
                house_biz.FloorId = FloorId;
                dr["*物理层"] = floorNo;
                if (string.IsNullOrEmpty(floorNo) || FloorId <= 0)
                {
                    IsSkip = true;
                    dr["*物理层"] = floorNo + "#error";
                }

                var houseName = dt.Rows[i]["*房号名称"].ToString().Trim();
                //var isExist = IsExistHouseName(BuildingId, FloorId, houseName, cityId, fxtCompanyId);
                house_biz.HouseName = houseName;
                dr["*房号名称"] = houseName;
                if (string.IsNullOrEmpty(houseName))
                {
                    IsSkip = true;
                    dr["*房号名称"] = houseName + "#error";
                }

                var UnitNo = dt.Rows[i]["*室号"].ToString().Trim();
                house_biz.UnitNo = UnitNo;
                dr["*室号"] = UnitNo;
                if (string.IsNullOrEmpty(UnitNo) || UnitNo.Length > 20)
                {
                    IsSkip = true;
                    dr["*室号"] = UnitNo + "#error";
                }

                var BizCutOffName = dt.Rows[i]["*商业阻隔"].ToString().Trim();
                var BizCutOff = GetCodeByName(BizCutOffName, SYS_Code_Dict._商业阻隔);
                house_biz.BizCutOff = BizCutOff;
                dr["*商业阻隔"] = BizCutOffName;
                if (string.IsNullOrEmpty(BizCutOffName) || BizCutOff == -1)
                {
                    IsSkip = true;
                    dr["*商业阻隔"] = BizCutOffName + "#error";
                }

                var BizHouseTypeName = dt.Rows[i]["*商铺类型"].ToString().Trim();
                var BizHouseType = GetCodeByName(BizHouseTypeName, SYS_Code_Dict._商铺类型);
                house_biz.BizHouseType = BizHouseType;
                dr["*商铺类型"] = BizHouseTypeName;
                if (string.IsNullOrEmpty(BizHouseTypeName) || BizHouseType == -1)
                {
                    IsSkip = true;
                    dr["*商铺类型"] = BizHouseTypeName + "#error";
                }

                var BizHouseLocationName = dt.Rows[i]["*商铺位置类型"].ToString().Trim();
                var BizHouseLocation = GetCodeByName(BizHouseLocationName, GetSubCodeByCode(BizHouseType));//商铺位置类型1107、1112、1113、1129
                house_biz.BizHouseLocation = BizHouseLocation;
                dr["*商铺位置类型"] = BizHouseLocationName;
                if (string.IsNullOrEmpty(BizHouseLocationName) || BizHouseLocation == -1)
                {
                    IsSkip = true;
                    dr["*商铺位置类型"] = BizHouseLocationName + "#error";
                }

                var PurposeCodeName = dt.Rows[i]["证载用途"].ToString().Trim();
                var PurposeCode = GetCodeByName(PurposeCodeName, SYS_Code_Dict._居住用途);
                house_biz.PurposeCode = PurposeCode;
                dr["证载用途"] = PurposeCodeName;
                if (!string.IsNullOrEmpty(PurposeCodeName) && PurposeCode == -1)
                {
                    IsSkip = true;
                    dr["证载用途"] = PurposeCodeName + "#error";
                }

                var SJPurposeCodeName = dt.Rows[i]["实际用途"].ToString().Trim();
                var SJPurposeCode = GetCodeByName(SJPurposeCodeName, SYS_Code_Dict._居住用途);
                house_biz.SJPurposeCode = SJPurposeCode;
                dr["实际用途"] = SJPurposeCodeName;
                if (!string.IsNullOrEmpty(SJPurposeCodeName) && SJPurposeCode == -1)
                {
                    IsSkip = true;
                    dr["实际用途"] = SJPurposeCodeName + "#error";
                }

                var BuildingArea = dt.Rows[i]["建筑面积"].ToString().Trim();
                house_biz.BuildingArea = (decimal?)TryParseHelper.StrToDecimal(BuildingArea);
                dr["建筑面积"] = BuildingArea;
                if (!string.IsNullOrEmpty(BuildingArea) && TryParseHelper.StrToDecimal(BuildingArea) == null)
                {
                    IsSkip = true;
                    dr["建筑面积"] = BuildingArea + "#error";
                }

                var InnerBuildingArea = dt.Rows[i]["套内面积"].ToString().Trim();
                house_biz.InnerBuildingArea = (decimal?)TryParseHelper.StrToDecimal(InnerBuildingArea);
                dr["套内面积"] = InnerBuildingArea;
                if (!string.IsNullOrEmpty(InnerBuildingArea) && TryParseHelper.StrToDecimal(InnerBuildingArea) == null)
                {
                    IsSkip = true;
                    dr["套内面积"] = InnerBuildingArea + "#error";
                }

                var FrontCodeName = dt.Rows[i]["朝向"].ToString().Trim();
                var FrontCode = GetCodeByName(FrontCodeName, SYS_Code_Dict._朝向);
                house_biz.FrontCode = FrontCode;
                dr["朝向"] = FrontCodeName;
                if (!string.IsNullOrEmpty(FrontCodeName) && FrontCode == -1)
                {
                    IsSkip = true;
                    dr["朝向"] = FrontCodeName + "#error";
                }
                var ShapeName = dt.Rows[i]["平面形状"].ToString().Trim();
                var Shape = GetCodeByName(ShapeName, SYS_Code_Dict._平面形状);
                house_biz.Shape = Shape;
                dr["平面形状"] = ShapeName;
                if (!string.IsNullOrEmpty(ShapeName) && Shape == -1)
                {
                    IsSkip = true;
                    dr["平面形状"] = ShapeName + "#error";
                }

                var Width = dt.Rows[i]["开间"].ToString().Trim();
                house_biz.Width = (decimal?)TryParseHelper.StrToDecimal(Width);
                dr["开间"] = Width;
                if (!string.IsNullOrEmpty(Width) && TryParseHelper.StrToDecimal(Width) == null)
                {
                    IsSkip = true;
                    dr["开间"] = Width + "#error";
                }

                var Length = dt.Rows[i]["进深"].ToString().Trim();
                house_biz.Length = (decimal?)TryParseHelper.StrToDecimal(Length);
                dr["进深"] = Length;
                if (!string.IsNullOrEmpty(Length) && TryParseHelper.StrToDecimal(Length) == null)
                {
                    IsSkip = true;
                    dr["进深"] = Length + "#error";
                }

                var IsMezzanineName = dt.Rows[i]["有无夹层"].ToString().Trim();
                var IsMezzanine = -1;
                if (IsMezzanineName == "有") IsMezzanine = 1;
                else if (IsMezzanineName == "无") IsMezzanine = 0;
                house_biz.IsMezzanine = IsMezzanine;
                dr["有无夹层"] = IsMezzanineName;
                if (!string.IsNullOrEmpty(IsMezzanineName) && IsMezzanine == -1)
                {
                    IsSkip = true;
                    dr["有无夹层"] = IsMezzanineName + "#error";
                }

                var Location = dt.Rows[i]["临街位置描述"].ToString().Trim();
                house_biz.Location = Location;
                dr["临街位置描述"] = Location;
                if (!string.IsNullOrEmpty(Location) && Location.Length > 20)
                {
                    IsSkip = true;
                    dr["临街位置描述"] = Location + "#error";
                }

                var FlowTypeName = dt.Rows[i]["人流量"].ToString().Trim();
                var FlowType = GetCodeByName(FlowTypeName, SYS_Code_Dict._人流量);
                house_biz.FlowType = FlowType;
                dr["人流量"] = FlowTypeName;
                if (!string.IsNullOrEmpty(FlowTypeName) && FlowType == -1)
                {
                    IsSkip = true;
                    dr["人流量"] = FlowTypeName + "#error";
                }
                var DoorNum = dt.Rows[i]["开门数"].ToString().Trim();
                house_biz.DoorNum = (int?)TryParseHelper.StrToInt32(DoorNum);
                dr["开门数"] = DoorNum;
                if (!string.IsNullOrEmpty(DoorNum) && (int?)TryParseHelper.StrToInt32(DoorNum) == null)
                {
                    IsSkip = true;
                    dr["开门数"] = DoorNum + "#error";
                }

                var RentRate = dt.Rows[i]["出租率"].ToString().Trim();
                house_biz.RentRate = (decimal?)TryParseHelper.StrToDecimal(RentRate);
                dr["出租率"] = RentRate;
                if (!string.IsNullOrEmpty(RentRate) && TryParseHelper.StrToDecimal(RentRate) == null)
                {
                    IsSkip = true;
                    dr["出租率"] = RentRate + "#error";
                }

                var UnitPrice = dt.Rows[i]["单价"].ToString().Trim();
                house_biz.UnitPrice = (decimal?)TryParseHelper.StrToDecimal(UnitPrice);
                dr["单价"] = UnitPrice;
                if (!string.IsNullOrEmpty(UnitPrice) && TryParseHelper.StrToDecimal(UnitPrice) == null)
                {
                    IsSkip = true;
                    dr["单价"] = UnitPrice + "#error";
                }

                var Weight = dt.Rows[i]["价格系数"].ToString().Trim();
                house_biz.Weight = (decimal?)TryParseHelper.StrToDecimal(Weight);
                dr["价格系数"] = Weight;
                if (!string.IsNullOrEmpty(Weight) && TryParseHelper.StrToDecimal(Weight) == null)
                {
                    IsSkip = true;
                    dr["价格系数"] = Weight + "#error";
                }

                var IsEValueName = dt.Rows[i]["是否可估"].ToString().Trim();
                var IsEValue = -1;
                if (IsEValueName == "是") IsEValue = 1;
                else if (IsEValueName == "否") IsEValue = 0;
                house_biz.IsEValue = IsEValue;
                dr["是否可估"] = IsEValueName;
                if (!string.IsNullOrEmpty(IsEValueName) && IsEValue == -1)
                {
                    IsSkip = true;
                    dr["是否可估"] = IsEValueName + "#error";
                }

                var Remarks = dt.Rows[i]["备注"].ToString().Trim();
                house_biz.Remarks = Remarks;
                dr["备注"] = Remarks;
                if (!string.IsNullOrEmpty(Remarks) && Remarks.Length > 500)
                {
                    IsSkip = true;
                    dr["备注"] = Remarks + "#error";
                }

                if (IsSkip)//该行数据中，至少有一列数据格式错误
                {
                    dtError.Rows.Add(dr);
                }
                else
                {
                    listTrue.Add(house_biz);
                }

            }


        }
    }
}
