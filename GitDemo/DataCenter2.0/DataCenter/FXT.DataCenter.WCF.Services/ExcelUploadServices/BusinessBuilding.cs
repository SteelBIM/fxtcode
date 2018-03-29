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

        public void BusinessBuildingExcelUpload(int cityid, int fxtcompanyid, string filePath, string userid, string taskName)
        {
            var taskId = 0;

            try
            {
                //在任务列表创建一条记录  iscomplete:0,代表否；1，代表是
                var task = new DAT_ImportTask
                {
                    //此处要加导入模块信息:住宅、商业
                    TaskName = taskName,
                    ImportType = SYS_Code_Dict.批量导入类型.商业楼栋信息,
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

                List<Dat_Building_Biz> correctData;//正确数据
                DataTable dataFormatError;//格式错误数据
                DataFilter(userid, cityid, fxtcompanyid, out correctData, out dataFormatError, data);
                //错误数据写入excel
                string fileNamePath = string.Empty;
                if (dataFormatError.Rows.Count > 0)
                {
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "商业楼栋格式错误数据.xlsx";
                    var folder = MapPath("NeedHandledFiles/FailureData/" + fxtcompanyid);
                    fileNamePath = Path.Combine(folder, fileName);
                    excelHelper.CreateExcel(dataFormatError, fileNamePath, folder);

                }

                var failureNum = 0;
                //正确数据添加到数据表

                correctData.ForEach(m =>
                {
                    var isExist = IsExistBuildName(Convert.ToInt32(m.CityId), m.AreaId, fxtcompanyid, m.BuildingName, m.SubAreaId, (int)m.ProjectId);
                    if (isExist != null)//存在该楼栋名称则更新该楼栋信息
                    {
                        m.BuildingId = isExist.BuildingId;
                        var modifyResult = _businessBuilding.UpdateDat_Building_Biz(m, fxtcompanyid);
                        if (modifyResult <= 0) failureNum = failureNum + 1;
                    }
                    else//新增该楼栋信息
                    {
                        var insertResult = _businessBuilding.AddDat_Building_Biz(m);
                        if (insertResult <= 0) failureNum = failureNum + 1;
                    }
                });
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
                _importTask.UpdateTask(taskId, 0, 0, 0, ex.Message, -1);
                LogHelper.WriteLog("BusinessBuildExeclUpload", "", userid, cityid, fxtcompanyid, ex);
            }
        }

        private void DataFilter(string userId, int cityId, int fxtCompanyId, out List<Dat_Building_Biz> listTrue, out DataTable dtError, DataTable dt)
        {
            listTrue = new List<Dat_Building_Biz>();
            dtError = new DataTable();
            foreach (DataColumn column in dt.Columns)
                dtError.Columns.Add(column.Caption);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var isSkip = false;
                var buildBiz = new Dat_Building_Biz();
                DataRow dr = dtError.NewRow();
                #region 必填字段
                buildBiz.CityId = cityId;
                buildBiz.FxtCompanyId = fxtCompanyId;
                buildBiz.Valid = 1;
                buildBiz.CreateTime = DateTime.Now;
                buildBiz.IsBenchmarks = 1;
                buildBiz.Creator = userId;
                buildBiz.SaveUser = userId;

                var areaName = dt.Rows[i]["*行政区"].ToString().Trim();
                var areaId = GetAreaId(cityId, areaName);
                buildBiz.AreaId = areaId;
                dr["*行政区"] = areaName;
                if (string.IsNullOrEmpty(areaName) || areaId == -1)
                {
                    isSkip = true;
                    dr["*行政区"] = areaName + "#error";
                }

                var subAreaName = dt.Rows[i]["商圈"].ToString().Trim();
                var subAreaId = GetSubAreaId(subAreaName, areaId);
                buildBiz.SubAreaId = subAreaId;
                dr["商圈"] = subAreaName;
                if (!string.IsNullOrEmpty(subAreaName) && subAreaId <= 0)
                {
                    isSkip = true;
                    dr["商圈"] = subAreaName + "#error";
                }

                var projectName = dt.Rows[i]["*商业街"].ToString().Trim();
                var projectId = GetProjectId(cityId, areaId, fxtCompanyId, projectName).FirstOrDefault();
                buildBiz.ProjectId = Convert.ToInt32(projectId);
                dr["*商业街"] = projectName;
                if (string.IsNullOrEmpty(projectName) || projectName.Length < 1 || projectId <= 0)
                {
                    isSkip = true;
                    dr["*商业街"] = projectName + "#error";
                }

                var buildName = dt.Rows[i]["*楼栋名称"].ToString().Trim();
                buildBiz.BuildingName = buildName;
                //bool ExistBuildName = false;
                //if (subAreaId > 0)
                //{
                //    ExistBuildName = IsExistBuildName(_cityId, areaId, _fxtCompanyId, buildName, subAreaId);
                //}
                dr["*楼栋名称"] = buildName;
                //if (string.IsNullOrEmpty(buildName) || ExistBuildName == false)
                //{
                if (string.IsNullOrEmpty(buildName))
                {
                    isSkip = true;
                    dr["*楼栋名称"] = buildName + "#error";
                }

                var BuildingBizTypeName = dt.Rows[i]["*楼栋商业类型"].ToString().Trim();
                var BuildingBizType = GetCodeByName(BuildingBizTypeName, SYS_Code_Dict._楼栋商业类型);
                buildBiz.BuildingBizType = BuildingBizType;
                dr["*楼栋商业类型"] = BuildingBizTypeName;
                if (string.IsNullOrEmpty(BuildingBizTypeName) || BuildingBizType == -1)
                {
                    isSkip = true;
                    dr["*楼栋商业类型"] = BuildingBizTypeName + "#error";
                }

                var ProRoadName = dt.Rows[i]["*临路类型"].ToString().Trim();
                var ProRoad = GetCodeByName(ProRoadName, SYS_Code_Dict._临路类型);
                buildBiz.ProRoad = ProRoad;
                dr["*临路类型"] = ProRoadName;
                if (string.IsNullOrEmpty(ProRoadName) || ProRoad == -1)
                {
                    isSkip = true;
                    dr["*临路类型"] = ProRoadName + "#error";
                }


                var BizCutOffName = dt.Rows[i]["*商业阻隔"].ToString().Trim();
                var BizCutOff = GetCodeByName(BizCutOffName, SYS_Code_Dict._商业阻隔);
                buildBiz.BizCutOff = BizCutOff;
                dr["*商业阻隔"] = BizCutOffName;
                if (string.IsNullOrEmpty(BizCutOffName) || BizCutOff == -1)
                {
                    isSkip = true;
                    dr["*商业阻隔"] = BizCutOffName + "#error";
                }

                var FlowsName = dt.Rows[i]["*人流量"].ToString().Trim();
                var Flows = GetCodeByName(FlowsName, SYS_Code_Dict._人流量);
                buildBiz.Flows = Flows;
                dr["*人流量"] = FlowsName;
                if (string.IsNullOrEmpty(FlowsName) || Flows == -1)
                {
                    isSkip = true;
                    dr["*人流量"] = FlowsName + "#error";
                }
                #endregion

                var CorrelationTypeName = dt.Rows[i]["与商圈的关联度"].ToString().Trim();
                var CorrelationType = GetCodeByName(CorrelationTypeName, SYS_Code_Dict._商圈关联度);
                buildBiz.CorrelationType = CorrelationType;
                dr["与商圈的关联度"] = CorrelationTypeName;
                if (!string.IsNullOrEmpty(CorrelationTypeName) && CorrelationType == -1)
                {
                    isSkip = true;
                    dr["与商圈的关联度"] = CorrelationTypeName + "#error";
                }

                var Address = dt.Rows[i]["地址"].ToString().Trim();
                buildBiz.Address = Address;
                dr["地址"] = Address;
                if (!string.IsNullOrEmpty(Address) && Address.Length > 200)
                {
                    isSkip = true;
                    dr["地址"] = Address + "#error";
                }

                var FieldNo = dt.Rows[i]["宗地号"].ToString().Trim();
                bool validFieldNo = ValidFieldNo(cityId, areaId, FieldNo);
                buildBiz.FieldNo = FieldNo;
                dr["宗地号"] = FieldNo;
                if (!string.IsNullOrEmpty(FieldNo) && validFieldNo == false)
                {
                    isSkip = true;
                    dr["宗地号"] = FieldNo + "#error";
                }

                var StructureCodeName = dt.Rows[i]["建筑结构"].ToString().Trim();
                var StructureCode = GetCodeByName(StructureCodeName, SYS_Code_Dict._建筑结构);
                buildBiz.StructureCode = StructureCode;
                dr["建筑结构"] = StructureCodeName;
                if (!string.IsNullOrEmpty(StructureCodeName) && StructureCode == -1)
                {
                    isSkip = true;
                    dr["建筑结构"] = StructureCodeName + "#error";
                }

                var BuildingTypeCodeName = dt.Rows[i]["建筑类型"].ToString().Trim();
                var BuildingTypeCode = GetCodeByName(BuildingTypeCodeName, SYS_Code_Dict._建筑类型);
                buildBiz.BuildingTypeCode = BuildingTypeCode;
                dr["建筑类型"] = BuildingTypeCodeName;
                if (!string.IsNullOrEmpty(BuildingTypeCodeName) && BuildingTypeCode == -1)
                {
                    isSkip = true;
                    dr["建筑类型"] = BuildingTypeCodeName + "#error";
                }

                var ManagerPrice = dt.Rows[i]["管理费"].ToString().Trim();
                buildBiz.ManagerPrice = ManagerPrice;
                dr["管理费"] = ManagerPrice;
                if (!string.IsNullOrEmpty(ManagerPrice) && ManagerPrice.Length > 50)
                {
                    isSkip = true;
                    dr["管理费"] = ManagerPrice + "#error";
                }

                var ManagerTel = dt.Rows[i]["管理处电话"].ToString().Trim();
                buildBiz.ManagerTel = ManagerTel;
                dr["管理处电话"] = ManagerTel;
                if (!string.IsNullOrEmpty(ManagerTel) && ManagerTel.Length > 50)
                {
                    isSkip = true;
                    dr["管理处电话"] = ManagerTel + "#error";
                }

                var BuildingArea = dt.Rows[i]["总建筑面积"].ToString().Trim();
                buildBiz.BuildingArea = (decimal?)TryParseHelper.StrToDecimal(BuildingArea);
                dr["总建筑面积"] = BuildingArea;
                if (!string.IsNullOrEmpty(BuildingArea) && TryParseHelper.StrToDecimal(BuildingArea) == null)
                {
                    isSkip = true;
                    dr["总建筑面积"] = BuildingArea + "#error";
                }

                var EndDate = dt.Rows[i]["竣工日期"].ToString().Trim();
                buildBiz.EndDate = (DateTime?)TryParseHelper.StrToDateTime(EndDate);
                dr["竣工日期"] = EndDate;
                if (!string.IsNullOrEmpty(EndDate) && TryParseHelper.StrToDateTime(EndDate) == null)
                {
                    isSkip = true;
                    dr["竣工日期"] = EndDate + "#error";
                }

                var OpenDate = dt.Rows[i]["开业日期"].ToString().Trim();
                buildBiz.OpenDate = (DateTime?)TryParseHelper.StrToDateTime(OpenDate);
                dr["开业日期"] = OpenDate;
                if (!string.IsNullOrEmpty(OpenDate) && TryParseHelper.StrToDateTime(OpenDate) == null)
                {
                    isSkip = true;
                    dr["开业日期"] = OpenDate + "#error";
                }

                var RentSaleTypeName = dt.Rows[i]["租售方式"].ToString().Trim();
                var RentSaleType = GetCodeByName(RentSaleTypeName, SYS_Code_Dict._经营方式);
                buildBiz.RentSaleType = RentSaleType;
                dr["租售方式"] = RentSaleTypeName;
                if (!string.IsNullOrEmpty(RentSaleTypeName) && RentSaleType == -1)
                {
                    isSkip = true;
                    dr["租售方式"] = RentSaleTypeName + "#error";
                }

                var BizArea = dt.Rows[i]["商业总面积"].ToString().Trim();
                buildBiz.BizArea = (decimal?)TryParseHelper.StrToDecimal(BizArea);
                dr["商业总面积"] = BizArea;
                if (!string.IsNullOrEmpty(BizArea) && TryParseHelper.StrToDecimal(BizArea) == null)
                {
                    isSkip = true;
                    dr["商业总面积"] = BizArea + "#error";
                }

                var BizFloor = dt.Rows[i]["商业总层数"].ToString().Trim();
                buildBiz.BizFloor = (int?)TryParseHelper.StrToInt32(BizFloor);
                dr["商业总层数"] = BizFloor;
                if (!string.IsNullOrEmpty(BizFloor) && TryParseHelper.StrToInt32(BizFloor) == null)
                {
                    isSkip = true;
                    dr["商业总层数"] = BizFloor + "#error";
                }

                var UpFloorNum = dt.Rows[i]["地上商业层数"].ToString().Trim();
                buildBiz.UpFloorNum = (int?)TryParseHelper.StrToInt32(UpFloorNum);
                dr["地上商业层数"] = UpFloorNum;
                if (!string.IsNullOrEmpty(UpFloorNum) && TryParseHelper.StrToInt32(UpFloorNum) == null)
                {
                    isSkip = true;
                    dr["地上商业层数"] = UpFloorNum + "#error";
                }

                var DownFloorNum = dt.Rows[i]["地下商业层数"].ToString().Trim();
                buildBiz.DownFloorNum = (int?)TryParseHelper.StrToInt32(DownFloorNum);
                dr["地下商业层数"] = DownFloorNum;
                if (!string.IsNullOrEmpty(DownFloorNum) && TryParseHelper.StrToInt32(DownFloorNum) == null)
                {
                    isSkip = true;
                    dr["地下商业层数"] = DownFloorNum + "#error";
                }

                var Functional = dt.Rows[i]["功能分布"].ToString().Trim();
                buildBiz.Functional = Functional;
                dr["功能分布"] = Functional;
                if (!string.IsNullOrEmpty(Functional) && Functional.Length > 200)
                {
                    isSkip = true;
                    dr["功能分布"] = Functional + "#error";
                }

                var BizNum = dt.Rows[i]["商铺总数"].ToString().Trim();
                buildBiz.BizNum = (int?)TryParseHelper.StrToInt32(BizNum);
                dr["商铺总数"] = BizNum;
                if (!string.IsNullOrEmpty(BizNum) && TryParseHelper.StrToInt32(BizNum) == null)
                {
                    isSkip = true;
                    dr["商铺总数"] = BizNum + "#error";
                }

                var LiftNum = dt.Rows[i]["客梯数量"].ToString().Trim();
                buildBiz.LiftNum = (int?)TryParseHelper.StrToInt32(LiftNum);
                dr["客梯数量"] = LiftNum;
                if (!string.IsNullOrEmpty(LiftNum) && TryParseHelper.StrToInt32(LiftNum) == null)
                {
                    isSkip = true;
                    dr["客梯数量"] = LiftNum + "#error";
                }

                var LiftFitmentName = dt.Rows[i]["客梯装修"].ToString().Trim();
                var LiftFitment = GetCodeByName(LiftFitmentName, SYS_Code_Dict._客厅装修);
                buildBiz.LiftFitment = LiftFitment;
                dr["客梯装修"] = LiftFitmentName;
                if (!string.IsNullOrEmpty(LiftFitmentName) && LiftFitment == -1)
                {
                    isSkip = true;
                    dr["客梯装修"] = LiftFitmentName + "#error";
                }

                var LiftBrand = dt.Rows[i]["电梯品牌"].ToString().Trim();
                buildBiz.LiftBrand = LiftBrand;
                dr["电梯品牌"] = LiftBrand;
                if (!string.IsNullOrEmpty(LiftBrand) && LiftBrand.Length > 50)
                {
                    isSkip = true;
                    dr["电梯品牌"] = LiftBrand + "#error";
                }

                var EscalatorsNum = dt.Rows[i]["扶手电梯数量"].ToString().Trim();
                buildBiz.EscalatorsNum = (int?)TryParseHelper.StrToInt32(EscalatorsNum);
                dr["扶手电梯数量"] = EscalatorsNum;
                if (!string.IsNullOrEmpty(EscalatorsNum) && TryParseHelper.StrToInt32(EscalatorsNum) == null)
                {
                    isSkip = true;
                    dr["扶手电梯数量"] = EscalatorsNum + "#error";
                }

                var EscalatorsBrand = dt.Rows[i]["扶手电梯品牌"].ToString().Trim();
                buildBiz.EscalatorsBrand = EscalatorsBrand;
                dr["扶手电梯品牌"] = EscalatorsBrand;
                if (!string.IsNullOrEmpty(EscalatorsBrand) && EscalatorsBrand.Length > 50)
                {
                    isSkip = true;
                    dr["扶手电梯品牌"] = EscalatorsBrand + "#error";
                }

                var ToiletBrand = dt.Rows[i]["卫浴品牌"].ToString().Trim();
                buildBiz.ToiletBrand = ToiletBrand;
                dr["卫浴品牌"] = ToiletBrand;
                if (!string.IsNullOrEmpty(ToiletBrand) && ToiletBrand.Length > 50)
                {
                    isSkip = true;
                    dr["卫浴品牌"] = ToiletBrand + "#error";
                }

                var PublicFitmentName = dt.Rows[i]["公共区域装修"].ToString().Trim();
                var PublicFitment = GetCodeByName(PublicFitmentName, SYS_Code_Dict._客厅装修);
                buildBiz.PublicFitment = PublicFitment;
                dr["公共区域装修"] = PublicFitmentName;
                if (!string.IsNullOrEmpty(PublicFitmentName) && PublicFitment == -1)
                {
                    isSkip = true;
                    dr["公共区域装修"] = PublicFitmentName + "#error";
                }

                var WallFitmentName = dt.Rows[i]["外墙装修"].ToString().Trim();
                var WallFitment = GetCodeByName(WallFitmentName, SYS_Code_Dict._外墙装修);
                buildBiz.WallFitment = WallFitment;
                dr["外墙装修"] = WallFitmentName;
                if (!string.IsNullOrEmpty(WallFitmentName) && WallFitment == -1)
                {
                    isSkip = true;
                    dr["外墙装修"] = WallFitmentName + "#error";
                }

                var TrafficTypeName = dt.Rows[i]["交通便捷度"].ToString().Trim();
                var TrafficType = GetCodeByName(TrafficTypeName, SYS_Code_Dict._交通便捷度);
                buildBiz.TrafficType = TrafficType;
                dr["交通便捷度"] = TrafficTypeName;
                if (!string.IsNullOrEmpty(TrafficTypeName) && TrafficType == -1)
                {
                    isSkip = true;
                    dr["交通便捷度"] = TrafficTypeName + "#error";
                }

                var TrafficDetails = dt.Rows[i]["交通便捷度描述"].ToString().Trim();
                buildBiz.TrafficDetails = TrafficDetails;
                dr["交通便捷度描述"] = TrafficDetails;
                if (!string.IsNullOrEmpty(TrafficDetails) && TrafficDetails.Length > 100)
                {
                    isSkip = true;
                    dr["交通便捷度描述"] = TrafficDetails + "#error";
                }

                var ParkingLevelName = dt.Rows[i]["停车便捷度"].ToString().Trim();
                var ParkingLevel = GetCodeByName(ParkingLevelName, SYS_Code_Dict._交通便捷度);
                buildBiz.ParkingLevel = ParkingLevel;
                dr["停车便捷度"] = ParkingLevelName;
                if (!string.IsNullOrEmpty(ParkingLevelName) && ParkingLevel == -1)
                {
                    isSkip = true;
                    dr["停车便捷度"] = ParkingLevelName + "#error";
                }

                var AirConditionTypeName = dt.Rows[i]["空调系统类型"].ToString().Trim();
                var AirConditionType = GetCodeByName(AirConditionTypeName, SYS_Code_Dict._空调系统类型);
                buildBiz.AirConditionType = AirConditionType;
                dr["空调系统类型"] = AirConditionTypeName;
                if (!string.IsNullOrEmpty(AirConditionTypeName) && AirConditionType == -1)
                {
                    isSkip = true;
                    dr["空调系统类型"] = AirConditionTypeName + "#error";
                }

                var Details = dt.Rows[i]["商业描述"].ToString().Trim();
                buildBiz.Details = Details;
                dr["商业描述"] = Details;
                if (!string.IsNullOrEmpty(Details) && Details.Length > 500)
                {
                    isSkip = true;
                    dr["商业描述"] = Details + "#error";
                }

                var East = dt.Rows[i]["四至东"].ToString().Trim();
                buildBiz.East = East;
                dr["四至东"] = East;
                if (!string.IsNullOrEmpty(East) && East.Length > 100)
                {
                    isSkip = true;
                    dr["四至东"] = East + "#error";
                }

                var south = dt.Rows[i]["四至南"].ToString().Trim();
                buildBiz.south = south;
                dr["四至南"] = south;
                if (!string.IsNullOrEmpty(south) && south.Length > 100)
                {
                    isSkip = true;
                    dr["四至南"] = south + "#error";
                }

                var west = dt.Rows[i]["四至西"].ToString().Trim();
                buildBiz.west = west;
                dr["四至西"] = west;
                if (!string.IsNullOrEmpty(west) && west.Length > 100)
                {
                    isSkip = true;
                    dr["四至西"] = west + "#error";
                }

                var north = dt.Rows[i]["四至北"].ToString().Trim();
                buildBiz.north = north;
                dr["四至北"] = north;
                if (!string.IsNullOrEmpty(north) && north.Length > 100)
                {
                    isSkip = true;
                    dr["四至北"] = north + "#error";
                }

                var BizTypeName = dt.Rows[i]["主营商业种类"].ToString().Trim();
                var BizType = GetCodeByName(BizTypeName, SYS_Code_Dict._商业细分类型);
                buildBiz.BizType = BizType;
                dr["主营商业种类"] = BizTypeName;
                if (!string.IsNullOrEmpty(BizTypeName) && BizType == -1)
                {
                    isSkip = true;
                    dr["主营商业种类"] = BizTypeName + "#error";
                }

                var BusinessHours = dt.Rows[i]["营业时间"].ToString().Trim();
                buildBiz.BusinessHours = BusinessHours;
                dr["营业时间"] = BusinessHours;
                if (!string.IsNullOrEmpty(BusinessHours) && BusinessHours.Length > 50)
                {
                    isSkip = true;
                    dr["营业时间"] = BusinessHours + "#error";
                }

                var CustomerTypeName = dt.Rows[i]["客户消费定位"].ToString().Trim();
                var CustomerType = GetCodeByName(CustomerTypeName, SYS_Code_Dict._消费定位);
                buildBiz.CustomerType = CustomerType;
                dr["客户消费定位"] = CustomerTypeName;
                if (!string.IsNullOrEmpty(CustomerTypeName) && CustomerType == -1)
                {
                    isSkip = true;
                    dr["客户消费定位"] = CustomerTypeName + "#error";
                }


                var X = dt.Rows[i]["经度"].ToString().Trim();
                buildBiz.X = (decimal?)TryParseHelper.StrToDecimal(X);
                dr["经度"] = X;
                if (!string.IsNullOrEmpty(X) && TryParseHelper.StrToDecimal(X) == null)
                {
                    isSkip = true;
                    dr["经度"] = X + "#error";
                }

                var Y = dt.Rows[i]["纬度"].ToString().Trim();
                buildBiz.Y = (decimal?)TryParseHelper.StrToDecimal(Y);
                dr["纬度"] = Y;
                if (!string.IsNullOrEmpty(Y) && TryParseHelper.StrToDecimal(Y) == null)
                {
                    isSkip = true;
                    dr["纬度"] = Y + "#error";
                }
                var Remarks = dt.Rows[i]["备注"].ToString().Trim();
                buildBiz.Remarks = Remarks;
                dr["备注"] = Remarks;
                if (!string.IsNullOrEmpty(Remarks) && Remarks.Length > 500)
                {
                    isSkip = true;
                    dr["备注"] = Remarks + "#error";
                }

                if (dt.Columns.Contains("楼栋均价"))
                {
                    dr["楼栋均价"] = dt.Rows[i]["楼栋均价"].ToString();
                    decimal averageprice = 0;
                    if (!string.IsNullOrEmpty(dt.Rows[i]["楼栋均价"].ToString()))
                    {
                        if (!decimal.TryParse(dt.Rows[i]["楼栋均价"].ToString(), out averageprice))
                        {
                            isSkip = true;
                            dr["楼栋均价"] = dt.Rows[i]["楼栋均价"].ToString() + "#error";
                        }
                        else
                        {
                            buildBiz.AveragePrice = averageprice;
                        }
                    }
                }
                decimal weight = 1;
                if (dt.Columns.Contains("价格系数"))
                {
                    dr["价格系数"] = dt.Rows[i]["价格系数"].ToString();
                    if (!string.IsNullOrEmpty(dt.Rows[i]["价格系数"].ToString()) && !decimal.TryParse(dt.Rows[i]["价格系数"].ToString(), out weight))
                    {
                        isSkip = true;
                        dr["价格系数"] = dt.Rows[i]["价格系数"].ToString() + "#error";
                    }
                    else if (weight<0 || weight>99999.9999M)
                    {
                        isSkip = true;
                        dr["价格系数"] = dt.Rows[i]["价格系数"].ToString() + "#error";                        
                    }
                    else
                    {
                        buildBiz.Weight = weight;
                    }
                }
                else
                {
                    buildBiz.Weight = weight;
                }

                if (isSkip)//该行数据中，至少有一列数据格式错误
                {
                    dtError.Rows.Add(dr);
                }
                else
                {
                    listTrue.Add(buildBiz);
                }

            }

        }
    }
}
