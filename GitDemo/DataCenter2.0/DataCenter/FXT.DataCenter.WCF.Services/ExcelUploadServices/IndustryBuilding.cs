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
        public void IndustryBuildingUpload(int cityid, int fxtcompanyid, string filePath, string userid, string taskName)
        {
            var taskId = 0;
            try
            {
                //在任务列表创建一条记录
                var task = new DAT_ImportTask()
                {
                    TaskName = taskName,
                    ImportType = SYS_Code_Dict.批量导入类型.工业楼栋信息,
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

                List<DatBuildingIndustry> listTrue;//正确数据
                DataTable dtError;//格式错误数据
                DataFilter(userid,cityid, fxtcompanyid, out listTrue, out dtError, data);

                //错误数据写入Excel
                var fileNamePath = string.Empty;
                if (dtError.Rows.Count > 0)
                {
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "工业楼栋错误数据.xlsx";
                    var folder = MapPath("NeedHandledFiles/FailureData/" + fxtcompanyid);
                    fileNamePath = Path.Combine(folder, fileName);
                    excelHelper.CreateExcel(dtError, fileNamePath, folder);
                }
                
                //正确数据写入表中
                foreach (var item in listTrue)
                {
                    var buildingId = GetBuildingIdByName_Industry(item.ProjectId, item.BuildingName, cityid, fxtcompanyid);
                    if (buildingId <= 0)
                    {
                        item.CreateTime = DateTime.Now;
                        item.Creator = userid;
                        _industryBuilding.AddIndustryBuilding(item);
                    }
                    else
                    {
                        item.SaveUser = userid;
                        item.BuildingId = buildingId;
                        _industryBuilding.UpdateIndustryBuilding(item, fxtcompanyid);
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
                LogHelper.WriteLog("IndustryBuildingExcelUploadTask", "", userid, cityid, fxtcompanyid, ex);
            }
        }

        private void DataFilter(string userId,int cityId, int fxtCompanyId, out List<DatBuildingIndustry> listTrue, out DataTable dtError, DataTable dt)
        {
            listTrue = new List<DatBuildingIndustry>();
            dtError = new DataTable();
            foreach (DataColumn column in dt.Columns)
                dtError.Columns.Add(column.Caption);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var isSkip = false;
                var datBuilding = new DatBuildingIndustry();
                var dr = dtError.NewRow();

                datBuilding.CityId = cityId;
                datBuilding.FxtCompanyId = fxtCompanyId;
                datBuilding.Creator = userId;
                datBuilding.SaveUser = userId;

                var areaName = dt.Rows[i]["*行政区"].ToString().Trim();
                var areaId = GetAreaId(cityId, areaName);
                dr["*行政区"] = areaName;
                if (string.IsNullOrEmpty(areaName) || areaId <= 0)
                {
                    isSkip = true;
                    dr["*行政区"] = areaName + "#error";
                }
                
                var projectName = dt.Rows[i]["*工业楼盘"].ToString().Trim();
                var projectIds = GetProjectIdByName_Industry(projectName, areaId, cityId, fxtCompanyId);
                var projectId = projectIds.FirstOrDefault();
                datBuilding.ProjectId = projectId;
                dr["*工业楼盘"] = projectName;
                if (string.IsNullOrEmpty(projectName) || projectId <= 0)
                {
                    isSkip = true;
                    dr["*工业楼盘"] = projectName + "#error";
                }
                
                var buildingName = dt.Rows[i]["*工业楼栋"].ToString().Trim();
                datBuilding.BuildingName = buildingName;
                dr["*工业楼栋"] = buildingName;
                if (string.IsNullOrEmpty(buildingName))
                {
                    isSkip = true;
                    dr["*工业楼栋"] = buildingName + "#error";
                }

                var otherName = dt.Rows[i]["别名"].ToString().Trim();
                datBuilding.OtherName = otherName;
                dr["别名"] = otherName;
                if (otherName.Length > 48)
                {
                    isSkip = true;
                    dr["别名"] = otherName + "#error";
                }

                var industryTypeName = dt.Rows[i]["工业类型"].ToString().Trim();
                var industryTypeCode = GetCodeByName(industryTypeName, SYS_Code_Dict._工业类型);
                datBuilding.IndustryType = industryTypeCode;
                dr["工业类型"] = industryTypeName;
                if (!string.IsNullOrEmpty(industryTypeName) && industryTypeCode <= 0)
                {
                    isSkip = true;
                    dr["工业类型"] = industryTypeName + "#error";
                }

                var purposeName = dt.Rows[i]["用途"].ToString().Trim();
                var purposeCode = GetCodeByName(purposeName, SYS_Code_Dict._土地用途);
                datBuilding.PurposeCode = purposeCode;
                dr["用途"] = purposeName;
                if (!string.IsNullOrEmpty(purposeName) && purposeCode <= 0)
                {
                    isSkip = true;
                    dr["用途"] = purposeName + "#error";
                }

                var structureName = dt.Rows[i]["建筑结构"].ToString().Trim();
                var structureCode = GetCodeByName(structureName, SYS_Code_Dict._建筑结构);
                datBuilding.StructureCode = structureCode;
                dr["建筑结构"] = structureName;
                if (!string.IsNullOrEmpty(structureName) && structureCode <= 0)
                {
                    isSkip = true;
                    dr["建筑结构"] = structureName + "#error";
                }

                var buildingTypeName = dt.Rows[i]["建筑类型"].ToString().Trim();
                var buildingTypeCode = GetCodeByName(buildingTypeName, SYS_Code_Dict._建筑类型);
                datBuilding.BuildingTypeCode = buildingTypeCode;
                dr["建筑类型"] = buildingTypeName;
                if (!string.IsNullOrEmpty(buildingTypeName) && buildingTypeCode <= 0)
                {
                    isSkip = true;
                    dr["建筑类型"] = buildingTypeName + "#error";
                }

                var totalFloorText = dt.Rows[i]["总楼层"].ToString().Trim();
                var totalFloorValue = (int?)TryParseHelper.StrToInt32(totalFloorText);
                datBuilding.TotalFloor = totalFloorValue;
                dr["总楼层"] = totalFloorText;
                if (!string.IsNullOrEmpty(totalFloorText) && totalFloorValue == null)
                {
                    isSkip = true;
                    dr["总楼层"] = totalFloorText + "#error";
                }

                var totalHeightText = dt.Rows[i]["总高度"].ToString().Trim();
                var totalHeightValue = (int?)TryParseHelper.StrToInt32(totalHeightText);
                datBuilding.TotalHigh = totalHeightValue;
                dr["总高度"] = totalHeightText;
                if (!string.IsNullOrEmpty(totalHeightText) && totalHeightValue == null)
                {
                    isSkip = true;
                    dr["总高度"] = totalFloorText + "#error";
                }

                var totalBuildingAreaText = dt.Rows[i]["总建筑面积_平方米"].ToString().Trim();
                var totalBuildingAreaValue = (decimal?)TryParseHelper.StrToDecimal(totalBuildingAreaText);
                datBuilding.BuildingArea = totalBuildingAreaValue;
                dr["总建筑面积_平方米"] = totalBuildingAreaText;
                if (!string.IsNullOrEmpty(totalBuildingAreaText) && totalBuildingAreaValue == null)
                {
                    isSkip = true;
                    dr["总建筑面积_平方米"] = totalBuildingAreaText + "#error";
                }

                var endDateText = dt.Rows[i]["竣工日期"].ToString().Trim();
                var endDateValue = (DateTime?)TryParseHelper.StrToDateTime(endDateText);
                datBuilding.EndDate = endDateValue;
                dr["竣工日期"] = endDateText;
                if (!string.IsNullOrEmpty(endDateText) && endDateValue == null)
                {
                    isSkip = true;
                    dr["竣工日期"] = endDateText + "#error";
                }

                var saleDateText = dt.Rows[i]["销售日期"].ToString().Trim();
                var saleDateValue = (DateTime?)TryParseHelper.StrToDateTime(saleDateText);
                datBuilding.SaleDate = saleDateValue;
                dr["销售日期"] = saleDateText;
                if (!string.IsNullOrEmpty(saleDateText) && saleDateValue == null)
                {
                    isSkip = true;
                    dr["销售日期"] = saleDateText + "#error";
                }

                var rentSaleTypeName = dt.Rows[i]["租售方式"].ToString().Trim();
                var rentSaleTypeCode = GetCodeByName(rentSaleTypeName, SYS_Code_Dict._经营方式);
                datBuilding.RentSaleType = rentSaleTypeCode;
                dr["租售方式"] = rentSaleTypeName;
                if (!string.IsNullOrEmpty(rentSaleTypeName) && rentSaleTypeCode <= 0)
                {
                    isSkip = true;
                    dr["租售方式"] = rentSaleTypeName + "#error";
                }

                var industryAreaText = dt.Rows[i]["工业面积_平方米"].ToString().Trim();
                var industryAreaValue = (decimal?)TryParseHelper.StrToDecimal(industryAreaText);
                datBuilding.IndustryArea = industryAreaValue;
                dr["工业面积_平方米"] = industryAreaText;
                if (!string.IsNullOrEmpty(industryAreaText) && industryAreaValue == null)
                {
                    isSkip = true;
                    dr["工业面积_平方米"] = industryAreaText + "#error";
                }

                var industryFloorText = dt.Rows[i]["工业总层数"].ToString().Trim();
                var industryFloorValue = (int?)TryParseHelper.StrToInt32(industryFloorText);
                datBuilding.IndustryFloor = industryFloorValue;
                dr["工业总层数"] = industryFloorText;
                if (!string.IsNullOrEmpty(industryFloorText) && industryFloorValue == null)
                {
                    isSkip = true;
                    dr["工业总层数"] = industryFloorText + "#error";
                }

                var podiumBuildingNumText = dt.Rows[i]["裙楼层数"].ToString().Trim();
                var podiumBuildingNumTextValue = (int?)TryParseHelper.StrToInt32(podiumBuildingNumText);
                datBuilding.PodiumBuildingNum = podiumBuildingNumTextValue;
                dr["裙楼层数"] = podiumBuildingNumText;
                if (!string.IsNullOrEmpty(podiumBuildingNumText) && podiumBuildingNumTextValue == null)
                {
                    isSkip = true;
                    dr["裙楼层数"] = podiumBuildingNumText + "#error";
                }

                var basementNumText = dt.Rows[i]["地下室层数"].ToString().Trim();
                var basementNumTextValue = (int?)TryParseHelper.StrToInt32(basementNumText);
                datBuilding.BasementNum = basementNumTextValue;
                dr["地下室层数"] = basementNumText;
                if (!string.IsNullOrEmpty(basementNumText) && basementNumTextValue == null)
                {
                    isSkip = true;
                    dr["地下室层数"] = basementNumText + "#error";
                }

                var functional = dt.Rows[i]["功能分布"].ToString().Trim();
                datBuilding.Functional = functional;
                dr["功能分布"] = functional;
                if (!string.IsNullOrEmpty(functional) && functional.Length > 200)
                {
                    isSkip = true;
                    dr["功能分布"] = functional + "#error";
                }

                var lobbyAreaText = dt.Rows[i]["大堂面积_平方米"].ToString().Trim();
                var lobbyAreaValue = (decimal?)TryParseHelper.StrToDecimal(lobbyAreaText);
                datBuilding.LobbyArea = lobbyAreaValue;
                dr["大堂面积_平方米"] = lobbyAreaText;
                if (!string.IsNullOrEmpty(lobbyAreaText) && lobbyAreaValue == null)
                {
                    isSkip = true;
                    dr["大堂面积_平方米"] = lobbyAreaText + "#error";
                }

                var lobbyHighText = dt.Rows[i]["大堂层高"].ToString().Trim();
                var lobbyHighValue = (decimal?)TryParseHelper.StrToDecimal(lobbyHighText);
                datBuilding.LobbyHigh = lobbyHighValue;
                dr["大堂层高"] = lobbyHighText;
                if (!string.IsNullOrEmpty(lobbyHighText) && lobbyHighValue == null)
                {
                    isSkip = true;
                    dr["大堂层高"] = lobbyHighText + "#error";
                }

                var lobbyFitmentName = dt.Rows[i]["大堂装修"].ToString().Trim();
                var lobbyFitmentCode = GetCodeByName(lobbyFitmentName, SYS_Code_Dict._客厅装修);
                datBuilding.LobbyFitment = lobbyFitmentCode;
                dr["大堂装修"] = lobbyFitmentName;
                if (!string.IsNullOrEmpty(lobbyFitmentName) && lobbyFitmentCode <= 0)
                {
                    isSkip = true;
                    dr["大堂装修"] = lobbyFitmentName + "#error";
                }

                var liftNum = dt.Rows[i]["客梯数量"].ToString().Trim();
                datBuilding.LiftNum = (int?)TryParseHelper.StrToInt32(liftNum);
                dr["客梯数量"] = liftNum;
                if (!string.IsNullOrEmpty(liftNum) && TryParseHelper.StrToInt32(liftNum) == null)
                {
                    isSkip = true;
                    dr["客梯数量"] = liftNum + "#error";
                }

                var liftFitmentName = dt.Rows[i]["客梯装修"].ToString().Trim();
                var liftFitment = GetCodeByName(liftFitmentName, SYS_Code_Dict._客厅装修);
                datBuilding.LiftFitment = liftFitment;
                dr["客梯装修"] = liftFitmentName;
                if (!string.IsNullOrEmpty(liftFitmentName) && liftFitment <= 0)
                {
                    isSkip = true;
                    dr["客梯装修"] = liftFitmentName + "#error";
                }

                var liftBrand = dt.Rows[i]["电梯品牌"].ToString().Trim();
                datBuilding.LiftBrand = liftBrand;
                dr["电梯品牌"] = liftBrand;
                if (!string.IsNullOrEmpty(liftBrand) && liftBrand.Length > 50)
                {
                    isSkip = true;
                    dr["电梯品牌"] = liftBrand + "#error";
                }

                var toiletBrand = dt.Rows[i]["卫浴品牌"].ToString().Trim();
                datBuilding.ToiletBrand = toiletBrand;
                dr["卫浴品牌"] = toiletBrand;
                if (!string.IsNullOrEmpty(toiletBrand) && toiletBrand.Length > 50)
                {
                    isSkip = true;
                    dr["卫浴品牌"] = toiletBrand + "#error";
                }

                var publicFitmentName = dt.Rows[i]["公共区域装修"].ToString().Trim();
                var publicFitment = GetCodeByName(publicFitmentName, SYS_Code_Dict._客厅装修);
                datBuilding.PublicFitment = publicFitment;
                dr["公共区域装修"] = publicFitmentName;
                if (!string.IsNullOrEmpty(publicFitmentName) && publicFitment <= 0)
                {
                    isSkip = true;
                    dr["公共区域装修"] = publicFitmentName + "#error";
                }

                var wallFitmentName = dt.Rows[i]["外墙装修"].ToString().Trim();
                var wallFitment = GetCodeByName(wallFitmentName, SYS_Code_Dict._办公外墙装修);
                datBuilding.WallFitment = wallFitment;
                dr["外墙装修"] = wallFitmentName;
                if (!string.IsNullOrEmpty(wallFitmentName) && wallFitment <= 0)
                {
                    isSkip = true;
                    dr["外墙装修"] = wallFitmentName + "#error";
                }

                var floorHighText = dt.Rows[i]["标准层层高"].ToString().Trim();
                var floorHighValue = (decimal?)TryParseHelper.StrToDecimal(floorHighText);
                datBuilding.FloorHigh = floorHighValue;
                dr["标准层层高"] = floorHighText;
                if (!string.IsNullOrEmpty(floorHighText) && floorHighValue == null)
                {
                    isSkip = true;
                    dr["标准层层高"] = floorHighText + "#error";
                }

                var X = dt.Rows[i]["经度"].ToString().Trim();
                datBuilding.X = (decimal?)TryParseHelper.StrToDecimal(X);
                dr["经度"] = X;
                if (!string.IsNullOrEmpty(X) && TryParseHelper.StrToDecimal(X) == null)
                {
                    isSkip = true;
                    dr["经度"] = X + "#error";
                }

                var Y = dt.Rows[i]["纬度"].ToString().Trim();
                datBuilding.Y = (decimal?)TryParseHelper.StrToDecimal(Y);
                dr["纬度"] = Y;
                if (!string.IsNullOrEmpty(Y) && TryParseHelper.StrToDecimal(Y) == null)
                {
                    isSkip = true;
                    dr["纬度"] = Y + "#error";
                }

                var remarks = dt.Rows[i]["备注"].ToString().Trim();
                datBuilding.Remarks = remarks;
                dr["备注"] = remarks;
                if (remarks.Length > 512)
                {
                    isSkip = true;
                    dr["备注"] = remarks + "#error";
                }

                var averagePrice = dt.Rows[i]["楼栋均价_元每平方米"].ToString().Trim();
                var averagePriceValue = (decimal?)TryParseHelper.StrToDecimal(averagePrice);
                datBuilding.AveragePrice = averagePriceValue;
                dr["楼栋均价_元每平方米"] = averagePrice;
                if (!string.IsNullOrEmpty(averagePrice) && averagePriceValue == null)
                {
                    isSkip = true;
                    dr["楼栋均价_元每平方米"] = averagePrice + "#error";
                }

                var priceDetail = dt.Rows[i]["价格系数说明"].ToString().Trim();
                datBuilding.PriceDetail = priceDetail;
                dr["价格系数说明"] = priceDetail;
                if (priceDetail.Length > 512)
                {
                    isSkip = true;
                    dr["价格系数说明"] = priceDetail + "#error";
                }

                if (isSkip)//该行数据中，至少有一列数据格式错误
                {
                    dtError.Rows.Add(dr);
                }
                else
                {
                    listTrue.Add(datBuilding);
                }
            }
        }
    }
}
