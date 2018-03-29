using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Infrastructure.Common.Common;
using FXT.DataCenter.Infrastructure.Common.Dictionary;
using FXT.DataCenter.Infrastructure.Common.NPOI;
using FXT.DataCenter.Infrastructure.Redis;
using StackExchange.Redis;

namespace FXT.DataCenter.WCF.Services
{
    public partial class ExcelUpload
    {

        public void BuildingExcelUpload(int cityid, int fxtcompanyid, string filePath, string userid, string taskName)
        {
            var taskId = 0;

            try
            {

                //在任务列表创建一条记录  iscomplete:0,代表否；1，代表是
                var task = new DAT_ImportTask
                {
                    //此处要加导入模块信息区分:住宅、商业
                    TaskName = taskName,
                    ImportType = SYS_Code_Dict.批量导入类型.住宅楼栋信息,
                    CityID = cityid,
                    FXTCompanyId = fxtcompanyid,
                    CreateDate = DateTime.Now,
                    Creator = userid,
                    IsComplete = 0,
                    SucceedNumber = 0,
                    DataErrNumber = 0,
                    NameErrNumber = 0,
                    FilePath = "",
                    Steps = 1
                };
                taskId = _importTask.AddTask(task);

                var excelHelper = new ExcelHandle(filePath);
                var data = excelHelper.ExcelToDataTable("Sheet1", true);

                var integer = Math.Floor(Convert.ToDouble(data.Rows.Count / 50));

                List<string> modifiedProperty;
                List<DAT_Building> listTrue;//正确数据
                DataTable dtError;//格式错误数据
                DataFilter(taskId, integer, cityid, fxtcompanyid, data, out listTrue, out dtError, out modifiedProperty);

                //错误数据写入Excel
                var fileNamePath = string.Empty;
                if (dtError.Rows.Count > 0)
                {
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "楼栋错误数据.xlsx";
                    var folder = MapPath("NeedHandledFiles/FailureData/" + fxtcompanyid);
                    fileNamePath = Path.Combine(folder, fileName);
                    excelHelper.CreateExcel(dtError, fileNamePath, folder);
                }

                var failureNum = 0;
                var index4True = 0;//用于统计进度
                //正确数据添加到Project表中
                listTrue.ForEach(m =>
                {
                    #region 更新进度
                    index4True++;
                    if (integer > 0)
                    {
                        if (index4True % integer == 0)
                        {
                            _importTask.TaskStepsIncreased(taskId);
                        }
                    }
                    #endregion

                    var isExist = _datBuilding.GetBuildingInfo(m.projectid, m.buildingname, m.cityid, fxtcompanyid);
                    if (isExist != null)//存在该楼栋名称则更新该楼栋信息
                    {
                        m.buildingid = isExist.buildingid;
                        m.saveuser = userid;
                        m.fxtcompanyid = isExist.fxtcompanyid;
                        var modifyResult = _datBuilding.UpdateBuilding4Excel(m, fxtcompanyid, modifiedProperty);
                        if (modifyResult <= 0) failureNum = failureNum + 1;
                    }
                    else//新增该楼栋信息
                    {
                        m.creator = userid;
                        var insertResult = _datBuilding.AddBuild(m);
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
                _importTask.UpdateTask(taskId, listTrue.Count - failureNum, dtError.Rows.Count, 0, relativePath, 1, 100);
            }

            catch (Exception ex)
            {
                _importTask.UpdateTask(taskId, 0, 0, 0, "", -1);
                LogHelper.WriteLog("BuildingExcelUpload", "", userid, cityid, fxtcompanyid, ex);
            }

        }

        private void DataFilter(int taskId, double integer, int cityId, int fxtCompanyId, DataTable dt, out List<DAT_Building> listTrue, out DataTable dtError, out List<string> modifiedProperty)
        {
            modifiedProperty = new List<string>();
            listTrue = new List<DAT_Building>();
            dtError = new DataTable();

            //从redis中取出数据
            var areaCach = GetAreaCach(cityId);
            var codeCach = GetCodeCach();

            foreach (DataColumn column in dt.Columns)
                dtError.Columns.Add(column.Caption);

            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var db = new DAT_Building();
                var dr = dtError.NewRow();
                db.fxtcompanyid = fxtCompanyId;
                db.cityid = cityId;
                db.createtime = DateTime.Now;
                var isError = false;

                var areaId = 0;
                var areaName = dt.Rows[i]["*行政区"].ToString().Trim();

                if (areaCach == null)
                {
                    areaId = GetAreaId(cityId, areaName);
                }
                else
                {
                    var obj = areaCach.FirstOrDefault(m => m.areaname == areaName);
                    areaId = obj == null ? -1 : obj.areaid;
                }

                dr["*行政区"] = areaName;
                if (string.IsNullOrEmpty(areaName) || areaId <= 0)
                {
                    isError = true;
                    dr["*行政区"] = (string.IsNullOrEmpty(areaName) ? "必填" : areaName) + "#error";
                }

                var projectName = dt.Rows[i]["*楼盘名称"].ToString().Trim();
                //var projectobj = projectsCach == null ? ProjectIdByName(fxtCompanyId, cityId, areaId, projectName).FirstOrDefault() : projectsCach.FirstOrDefault(m => m.projectname == projectName && m.areaid == areaId);
                //var projectId = projectobj == null ? 0 : projectobj.projectid;
                var projectobj = ProjectIdByName(fxtCompanyId, cityId, areaId, projectName).FirstOrDefault();
                var projectId = projectobj == null ? 0 : projectobj.projectid;
                db.projectid = projectId;
                dr["*楼盘名称"] = projectName;
                if (string.IsNullOrEmpty(projectName) || projectId <= 0)
                {
                    isError = true;
                    dr["*楼盘名称"] = (string.IsNullOrEmpty(projectName) ? "必填" : projectName) + "#error";
                }

                var buildingName = dt.Rows[i]["*楼栋名称"].ToString().Trim();
                db.buildingname = buildingName;
                dr["*楼栋名称"] = buildingName;
                if (string.IsNullOrEmpty(buildingName) || buildingName.Length > 148)
                {
                    isError = true;
                    dr["*楼栋名称"] = (string.IsNullOrEmpty(buildingName) ? "必填" : buildingName) + "#error";
                }

                var purposeName = dt.Rows[i]["*楼栋用途"].ToString().Trim();
                int purposeCode = -1;
                if (codeCach == null)
                {
                    purposeCode = GetCodeByName(purposeName, SYS_Code_Dict._土地用途);
                }
                else
                {
                    var purposeobj = codeCach.FirstOrDefault(m => m.codename == purposeName && m.id == SYS_Code_Dict._土地用途);
                    purposeCode = purposeobj == null ? -1 : purposeobj.code;
                }
                db.purposecode = purposeCode;
                dr["*楼栋用途"] = purposeName;
                if (string.IsNullOrEmpty(purposeName) || purposeCode == -1)
                {
                    isError = true;
                    dr["*楼栋用途"] = (string.IsNullOrEmpty(purposeName) ? "必填" : purposeName) + "#error";
                }

                var totalFloor = dt.Rows[i]["*总层数"].ToString().Trim();
                db.totalfloor = (int?)TryParseHelper.StrToInt32(totalFloor);
                dr["*总层数"] = totalFloor;
                if (string.IsNullOrEmpty(totalFloor) || (!string.IsNullOrEmpty(totalFloor) && TryParseHelper.StrToInt32(totalFloor) == null))
                {
                    isError = true;
                    dr["*总层数"] = (string.IsNullOrEmpty(totalFloor) ? "必填" : totalFloor) + "#error";
                }

                if (dt.Columns.Contains("*是否确认总层数"))
                {
                    var isTotalFloor = dt.Rows[i]["*是否确认总层数"].ToString().Trim();
                    var isTotalFloorValue = YesOrNo(isTotalFloor);
                    db.isTotalfloor = isTotalFloorValue;
                    dr["*是否确认总层数"] = isTotalFloor;
                    if (string.IsNullOrEmpty(isTotalFloor) || isTotalFloorValue == -1)
                    {
                        isError = true;
                        dr["*是否确认总层数"] = (string.IsNullOrEmpty(isTotalFloor) ? "必填" : isTotalFloor) + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("[isTotalfloor]=@isTotalfloor,");

                }

                if (dt.Columns.Contains("建筑类型"))
                {
                    var buildingTypeName = dt.Rows[i]["建筑类型"].ToString().Trim();
                    var buildingTypeCode = -1;
                    if (codeCach == null)
                    {
                        buildingTypeCode = GetCodeByName(buildingTypeName, SYS_Code_Dict._建筑类型);
                    }
                    else
                    {
                        var obj = codeCach.FirstOrDefault(m => m.codename == buildingTypeName && m.id == SYS_Code_Dict._建筑类型);
                        buildingTypeCode = obj == null ? -1 : obj.code;
                    }
                    db.buildingtypecode = buildingTypeCode;
                    dr["建筑类型"] = buildingTypeName;
                    if (!string.IsNullOrEmpty(buildingTypeName) && buildingTypeCode == -1)
                    {
                        isError = true;
                        dr["建筑类型"] = buildingTypeName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[BuildingTypeCode]=@BuildingTypeCode,");
                }


                if (dt.Columns.Contains("建筑结构"))
                {
                    var structureName = dt.Rows[i]["建筑结构"].ToString().Trim();
                    var structureCode = -1;
                    if (codeCach == null)
                    {
                        structureCode = GetCodeByName(structureName, SYS_Code_Dict._建筑结构);
                    }
                    else
                    {
                        var obj = codeCach.FirstOrDefault(m => m.codename == structureName && m.id == SYS_Code_Dict._建筑结构);
                        structureCode = obj == null ? -1 : obj.code;
                    }
                    db.structurecode = structureCode;
                    dr["建筑结构"] = structureName;
                    if (!string.IsNullOrEmpty(structureName) && structureCode == -1)
                    {
                        isError = true;
                        dr["建筑结构"] = structureName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("StructureCode=@StructureCode,");
                }


                if (dt.Columns.Contains("销售时间"))
                {
                    var saleDate = dt.Rows[i]["销售时间"].ToString().Trim();
                    db.saledate = (DateTime?)TryParseHelper.StrToDateTime(saleDate);
                    dr["销售时间"] = saleDate;
                    if (!string.IsNullOrEmpty(saleDate) && TryParseHelper.StrToDateTime(saleDate) == null)
                    {
                        isError = true;
                        dr["销售时间"] = saleDate + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[SaleDate]=@SaleDate,");
                }


                if (dt.Columns.Contains("销售均价"))
                {
                    var salePrice = dt.Rows[i]["销售均价"].ToString().Trim();
                    db.saleprice = (decimal?)TryParseHelper.StrToDecimal(salePrice);
                    dr["销售均价"] = salePrice;
                    if (!string.IsNullOrEmpty(salePrice) && TryParseHelper.StrToDecimal(salePrice) == null)
                    {
                        isError = true;
                        dr["销售均价"] = salePrice + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[SalePrice]=@SalePrice,");

                }


                if (dt.Columns.Contains("单元数"))
                {
                    var unitNum = dt.Rows[i]["单元数"].ToString().Trim();
                    db.unitsnumber = (int?)TryParseHelper.StrToInt32(unitNum);
                    dr["单元数"] = unitNum;
                    if (!string.IsNullOrEmpty(unitNum) && TryParseHelper.StrToInt32(unitNum) == null)
                    {
                        isError = true;
                        dr["单元数"] = unitNum + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[UnitsNumber]=@UnitsNumber,");
                }

                if (dt.Columns.Contains("建筑面积"))
                {
                    var buildingArea = dt.Rows[i]["建筑面积"].ToString().Trim();
                    db.totalbuildarea = (decimal?)TryParseHelper.StrToDecimal(buildingArea);
                    dr["建筑面积"] = buildingArea;
                    if (!string.IsNullOrEmpty(buildingArea) && TryParseHelper.StrToDecimal(buildingArea) == null)
                    {
                        isError = true;
                        dr["建筑面积"] = buildingArea + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[TotalBuildArea]=@TotalBuildArea,");
                }

                if (dt.Columns.Contains("楼栋别名"))
                {
                    var otherName = dt.Rows[i]["楼栋别名"].ToString().Trim();
                    db.othername = otherName;
                    dr["楼栋别名"] = otherName;
                    if (!string.IsNullOrEmpty(otherName) && otherName.Length > 50)
                    {
                        isError = true;
                        dr["楼栋别名"] = otherName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[OtherName]=@OtherName,");
                }

                if (dt.Columns.Contains("梯户比"))
                {
                    var elevatorRate = dt.Rows[i]["梯户比"].ToString().Trim();
                    db.elevatorrate = elevatorRate;
                    dr["梯户比"] = elevatorRate;
                    if (!string.IsNullOrEmpty(elevatorRate) && elevatorRate.Length > 50)
                    {
                        isError = true;
                        dr["梯户比"] = elevatorRate + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("ElevatorRate=@ElevatorRate,");
                }

                if (dt.Columns.Contains("是否带电梯"))
                {
                    var isElevator = dt.Rows[i]["是否带电梯"].ToString().Trim();
                    var isElevatorValue = YesOrNo(isElevator);
                    db.iselevator = isElevatorValue;
                    dr["是否带电梯"] = isElevator;
                    if (!string.IsNullOrEmpty(isElevator) && isElevatorValue == -1)
                    {
                        isError = true;
                        dr["是否带电梯"] = isElevator + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("IsElevator=@IsElevator,");
                }

                if (dt.Columns.Contains("外墙装修"))
                {
                    var wallName = dt.Rows[i]["外墙装修"].ToString().Trim();
                    var wall = -1;
                    if (codeCach == null)
                    {
                        wall = GetCodeByName(wallName, SYS_Code_Dict._外墙装修);
                    }
                    else
                    {
                        var obj = codeCach.FirstOrDefault(m => m.codename == wallName && m.id == SYS_Code_Dict._外墙装修);
                        wall = obj == null ? -1 : obj.code;
                    }
                    db.wall = wall;
                    dr["外墙装修"] = wallName;
                    if (!string.IsNullOrEmpty(wallName) && wall == -1)
                    {
                        isError = true;
                        dr["外墙装修"] = wallName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("Wall=@Wall,");
                }

                if (dt.Columns.Contains("销售许可证"))
                {
                    var saleLicence = dt.Rows[i]["销售许可证"].ToString().Trim();
                    db.salelicence = saleLicence;
                    dr["销售许可证"] = saleLicence;
                    if (!string.IsNullOrEmpty(saleLicence) && saleLicence.Length > 50)
                    {
                        isError = true;
                        dr["销售许可证"] = saleLicence + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("salelicence=@salelicence,");
                }

                if (dt.Columns.Contains("预售时间"))
                {
                    var licenceDate = dt.Rows[i]["预售时间"].ToString().Trim();
                    db.licencedate = (DateTime?)TryParseHelper.StrToDateTime(licenceDate);
                    dr["预售时间"] = licenceDate;
                    if (!string.IsNullOrEmpty(licenceDate) && TryParseHelper.StrToDateTime(licenceDate) == null)
                    {
                        isError = true;
                        dr["预售时间"] = licenceDate + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("licencedate=@licencedate,");
                }

                if (dt.Columns.Contains("楼栋朝向"))
                {
                    var frontName = dt.Rows[i]["楼栋朝向"].ToString().Trim();
                    var frontCode = -1;
                    if (codeCach == null)
                    {
                        frontCode = GetCodeByName(frontName, SYS_Code_Dict._朝向);
                    }
                    else
                    {
                        var obj = codeCach.FirstOrDefault(m => m.codename == frontName && m.id == SYS_Code_Dict._朝向);
                        frontCode = obj == null ? -1 : obj.code;
                    }
                    db.frontcode = frontCode;
                    dr["楼栋朝向"] = frontName;
                    if (!string.IsNullOrEmpty(frontName) && frontCode == -1)
                    {
                        isError = true;
                        dr["楼栋朝向"] = frontName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("FrontCode=@FrontCode,");
                }

                if (dt.Columns.Contains("楼栋景观"))
                {
                    var sightName = dt.Rows[i]["楼栋景观"].ToString().Trim();
                    var sightCode = -1;
                    if (codeCach == null)
                    {
                        sightCode = GetCodeByName(sightName, SYS_Code_Dict._景观);
                    }
                    else
                    {
                        var obj = codeCach.FirstOrDefault(m => m.codename == sightName && m.id == SYS_Code_Dict._景观);
                        sightCode = obj == null ? -1 : obj.code;
                    }
                    db.sightcode = sightCode;
                    dr["楼栋景观"] = sightName;
                    if (!string.IsNullOrEmpty(sightName) && sightCode == -1)
                    {
                        isError = true;
                        dr["楼栋景观"] = sightName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("SightCode=@SightCode,");
                }

                if (dt.Columns.Contains("楼栋系数"))
                {
                    var weight = dt.Rows[i]["楼栋系数"].ToString().Trim();
                    db.weight = (decimal?)TryParseHelper.StrToDecimal(weight);
                    dr["楼栋系数"] = weight;
                    if (!string.IsNullOrEmpty(weight) && TryParseHelper.StrToDecimal(weight) == null)
                    {
                        isError = true;
                        dr["楼栋系数"] = weight + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("Weight=@Weight,");
                }

                if (dt.Columns.Contains("层高"))
                {
                    var floorHigh = dt.Rows[i]["层高"].ToString().Trim();
                    db.floorhigh = (decimal?)TryParseHelper.StrToDecimal(floorHigh);
                    dr["层高"] = floorHigh;
                    if (!string.IsNullOrEmpty(floorHigh) && TryParseHelper.StrToDecimal(floorHigh) == null)
                    {
                        isError = true;
                        dr["层高"] = floorHigh + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[floorhigh]=@floorhigh,");
                }

                if (dt.Columns.Contains("总户数"))
                {
                    var totalNumber = dt.Rows[i]["总户数"].ToString().Trim();
                    db.totalnumber = (int?)TryParseHelper.StrToInt32(totalNumber);
                    dr["总户数"] = totalNumber;
                    if (!string.IsNullOrEmpty(totalNumber) && TryParseHelper.StrToInt32(totalNumber) == null)
                    {
                        isError = true;
                        dr["总户数"] = totalNumber + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[totalnumber]=@totalnumber,");
                }

                if (dt.Columns.Contains("建筑时间"))
                {
                    var buildDate = dt.Rows[i]["建筑时间"].ToString().Trim();
                    db.builddate = (DateTime?)TryParseHelper.StrToDateTime(buildDate);
                    dr["建筑时间"] = buildDate;
                    if (!string.IsNullOrEmpty(buildDate) && TryParseHelper.StrToDateTime(buildDate) == null)
                    {
                        isError = true;
                        dr["建筑时间"] = buildDate + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("builddate=@builddate,");
                }

                if (dt.Columns.Contains("楼栋均价"))
                {
                    var avgPrice = dt.Rows[i]["楼栋均价"].ToString().Trim();
                    db.averageprice = (decimal?)TryParseHelper.StrToDecimal(avgPrice);
                    dr["楼栋均价"] = avgPrice;
                    if (!string.IsNullOrEmpty(avgPrice) && TryParseHelper.StrToDecimal(avgPrice) == null)
                    {
                        isError = true;
                        dr["楼栋均价"] = avgPrice + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[averageprice]=@averageprice,");
                }

                if (dt.Columns.Contains("均价层"))
                {
                    var averageFloor = dt.Rows[i]["均价层"].ToString().Trim();
                    db.averagefloor = (int?)TryParseHelper.StrToInt32(averageFloor);
                    dr["均价层"] = averageFloor;
                    if (!string.IsNullOrEmpty(averageFloor) && TryParseHelper.StrToInt32(averageFloor) == null)
                    {
                        isError = true;
                        dr["均价层"] = averageFloor + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[averagefloor]=@averagefloor,");
                }

                if (dt.Columns.Contains("入伙时间"))
                {
                    var joinDate = dt.Rows[i]["入伙时间"].ToString().Trim();
                    db.joindate = (DateTime?)TryParseHelper.StrToDateTime(joinDate);
                    dr["入伙时间"] = joinDate;
                    if (!string.IsNullOrEmpty(joinDate) && TryParseHelper.StrToDateTime(joinDate) == null)
                    {
                        isError = true;
                        dr["入伙时间"] = joinDate + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("joindate=@joindate,");
                }

                if (dt.Columns.Contains("是否可估"))
                {
                    var isEValueText = dt.Rows[i]["是否可估"].ToString().Trim();
                    var isEValueValue = YesOrNo(isEValueText);
                    db.isevalue = isEValueValue;
                    dr["是否可估"] = isEValueText;
                    if (!string.IsNullOrEmpty(isEValueText) && isEValueValue == -1)
                    {
                        isError = true;
                        dr["是否可估"] = isEValueText + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[isevalue]=@isevalue,");

                }

                if (dt.Columns.Contains("楼栋位置"))
                {
                    var locationName = dt.Rows[i]["楼栋位置"].ToString().Trim();
                    var locationCode = -1;
                    if (codeCach == null)
                    {
                        locationCode = GetCodeByName(locationName, SYS_Code_Dict._楼栋位置);
                    }
                    else
                    {
                        var obj = codeCach.FirstOrDefault(m => m.codename == locationName && m.id == SYS_Code_Dict._楼栋位置);
                        locationCode = obj == null ? -1 : obj.code;
                    }
                    db.locationcode = locationCode;
                    dr["楼栋位置"] = locationName;
                    if (!string.IsNullOrEmpty(locationName) && locationCode <= 0)
                    {
                        isError = true;
                        dr["楼栋位置"] = locationName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[locationcode]=@locationcode,");
                }

                if (dt.Columns.Contains("楼栋结构修正价格"))
                {
                    var structureWeight = dt.Rows[i]["楼栋结构修正价格"].ToString().Trim();
                    db.structureweight = (decimal?)TryParseHelper.StrToDecimal(structureWeight);
                    dr["楼栋结构修正价格"] = structureWeight;
                    if (!string.IsNullOrEmpty(structureWeight) && TryParseHelper.StrToDecimal(structureWeight) == null)
                    {
                        isError = true;
                        dr["楼栋结构修正价格"] = structureWeight + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[structureweight]=@structureweight,");
                }

                if (dt.Columns.Contains("建筑类型修正价格"))
                {
                    var buildingTypeWeight = dt.Rows[i]["建筑类型修正价格"].ToString().Trim();
                    db.buildingtypeweight = (decimal?)TryParseHelper.StrToDecimal(buildingTypeWeight);
                    dr["建筑类型修正价格"] = buildingTypeWeight;
                    if (!string.IsNullOrEmpty(buildingTypeWeight) && TryParseHelper.StrToDecimal(buildingTypeWeight) == null)
                    {
                        isError = true;
                        dr["建筑类型修正价格"] = buildingTypeWeight + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[BuildingTypeWeight]=@buildingtypeweight,");
                }

                if (dt.Columns.Contains("年期修正价格"))
                {
                    var yearWeight = dt.Rows[i]["年期修正价格"].ToString().Trim();
                    db.yearweight = (decimal?)TryParseHelper.StrToDecimal(yearWeight);
                    dr["年期修正价格"] = yearWeight;
                    if (!string.IsNullOrEmpty(yearWeight) && TryParseHelper.StrToDecimal(yearWeight) == null)
                    {
                        isError = true;
                        dr["年期修正价格"] = yearWeight + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[yearweight]=@yearweight,");
                }

                if (dt.Columns.Contains("用途修正价格"))
                {
                    var purposeWeight = dt.Rows[i]["用途修正价格"].ToString().Trim();
                    db.purposeweight = (decimal?)TryParseHelper.StrToDecimal(purposeWeight);
                    dr["用途修正价格"] = purposeWeight;
                    if (!string.IsNullOrEmpty(purposeWeight) && TryParseHelper.StrToDecimal(purposeWeight) == null)
                    {
                        isError = true;
                        dr["用途修正价格"] = purposeWeight + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[PurposeWeight]=@purposeweight,");
                }

                if (dt.Columns.Contains("楼栋位置修正价格"))
                {
                    var locationWeight = dt.Rows[i]["楼栋位置修正价格"].ToString().Trim();
                    db.locationweight = (decimal?)TryParseHelper.StrToDecimal(locationWeight);
                    dr["楼栋位置修正价格"] = locationWeight;
                    if (!string.IsNullOrEmpty(locationWeight) && TryParseHelper.StrToDecimal(locationWeight) == null)
                    {
                        isError = true;
                        dr["楼栋位置修正价格"] = locationWeight + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[locationweight]=@locationweight,");
                }

                if (dt.Columns.Contains("景观修正价格"))
                {
                    var sightWeight = dt.Rows[i]["景观修正价格"].ToString().Trim();
                    db.sightweight = (decimal?)TryParseHelper.StrToDecimal(sightWeight);
                    dr["景观修正价格"] = sightWeight;
                    if (!string.IsNullOrEmpty(sightWeight) && TryParseHelper.StrToDecimal(sightWeight) == null)
                    {
                        isError = true;
                        dr["景观修正价格"] = sightWeight + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[sightweight]=@sightweight,");
                }

                if (dt.Columns.Contains("朝向修正价格"))
                {
                    var frontWeight = dt.Rows[i]["朝向修正价格"].ToString().Trim();
                    db.frontweight = (decimal?)TryParseHelper.StrToDecimal(frontWeight);
                    dr["朝向修正价格"] = frontWeight;
                    if (!string.IsNullOrEmpty(frontWeight) && TryParseHelper.StrToDecimal(frontWeight) == null)
                    {
                        isError = true;
                        dr["朝向修正价格"] = frontWeight + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[frontweight]=@frontweight,");
                }

                if (dt.Columns.Contains("经度"))
                {
                    var x = dt.Rows[i]["经度"].ToString().Trim();
                    db.x = (decimal?)TryParseHelper.StrToDecimal(x);
                    dr["经度"] = x;
                    if (!string.IsNullOrEmpty(x) && TryParseHelper.StrToDecimal(x) == null)
                    {
                        isError = true;
                        dr["经度"] = x + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[x]=@x,");
                }

                if (dt.Columns.Contains("纬度"))
                {
                    var y = dt.Rows[i]["纬度"].ToString().Trim();
                    db.y = (decimal?)TryParseHelper.StrToDecimal(y);
                    dr["纬度"] = y;
                    if (!string.IsNullOrEmpty(y) && TryParseHelper.StrToDecimal(y) == null)
                    {
                        isError = true;
                        dr["纬度"] = y + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[y]=@y,");
                }

                if (dt.Columns.Contains("比例尺"))
                {
                    var xyScale = dt.Rows[i]["比例尺"].ToString().Trim();
                    db.xyscale = (int?)TryParseHelper.StrToInt32(xyScale);
                    dr["比例尺"] = xyScale;
                    if (!string.IsNullOrEmpty(xyScale) && TryParseHelper.StrToInt32(xyScale) == null)
                    {
                        isError = true;
                        dr["比例尺"] = xyScale + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[xyscale]=@xyscale,");
                }

                if (dt.Columns.Contains("附属房屋均价"))
                {
                    var subAveragePrice = dt.Rows[i]["附属房屋均价"].ToString().Trim();
                    db.subaverageprice = (decimal?)TryParseHelper.StrToDecimal(subAveragePrice);
                    dr["附属房屋均价"] = subAveragePrice;
                    if (!string.IsNullOrEmpty(subAveragePrice) && TryParseHelper.StrToDecimal(subAveragePrice) == null)
                    {
                        isError = true;
                        dr["附属房屋均价"] = subAveragePrice + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[subaverageprice]=@subaverageprice,");
                }

                if (dt.Columns.Contains("价格系数说明"))
                {
                    var priceDetail = dt.Rows[i]["价格系数说明"].ToString().Trim();
                    db.pricedetail = priceDetail;
                    dr["价格系数说明"] = priceDetail;
                    if (!string.IsNullOrEmpty(priceDetail) && priceDetail.Length > 520)
                    {
                        isError = true;
                        dr["价格系数说明"] = priceDetail + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[pricedetail]=@pricedetail,");
                }

                if (dt.Columns.Contains("楼栋户型面积修正因素"))
                {
                    var bHouseTypeName = dt.Rows[i]["楼栋户型面积修正因素"].ToString().Trim();
                    db.bhousetypecode = (int?)TryParseHelper.StrToInt32(bHouseTypeName);
                    dr["楼栋户型面积修正因素"] = bHouseTypeName;
                    if (!string.IsNullOrEmpty(bHouseTypeName) && TryParseHelper.StrToInt32(bHouseTypeName) == null)
                    {
                        isError = true;
                        dr["楼栋户型面积修正因素"] = bHouseTypeName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[bhousetypecode]=@bhousetypecode,");
                }

                if (dt.Columns.Contains("楼栋户型面积修正价格"))
                {
                    var bHouseTypeWeight = dt.Rows[i]["楼栋户型面积修正价格"].ToString().Trim();
                    db.bhousetypeweight = (decimal?)TryParseHelper.StrToDecimal(bHouseTypeWeight);
                    dr["楼栋户型面积修正价格"] = bHouseTypeWeight;
                    if (!string.IsNullOrEmpty(bHouseTypeWeight) && TryParseHelper.StrToDecimal(bHouseTypeWeight) == null)
                    {
                        isError = true;
                        dr["楼栋户型面积修正价格"] = bHouseTypeWeight + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[bhousetypeweight]=@bhousetypeweight,");
                }

                if (dt.Columns.Contains("楼间距"))
                {
                    var distance = dt.Rows[i]["楼间距"].ToString().Trim();
                    db.distance = (int?)TryParseHelper.StrToInt32(distance);
                    dr["楼间距"] = distance;
                    if (!string.IsNullOrEmpty(distance) && TryParseHelper.StrToInt32(distance) == null)
                    {
                        isError = true;
                        dr["楼间距"] = distance + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[distance]=@distance,");
                }

                if (dt.Columns.Contains("楼间距系数"))
                {
                    var distanceWeight = dt.Rows[i]["楼间距系数"].ToString().Trim();
                    db.distanceweight = (decimal?)TryParseHelper.StrToDecimal(distanceWeight);
                    dr["楼间距系数"] = distanceWeight;
                    if (!string.IsNullOrEmpty(distanceWeight) && TryParseHelper.StrToDecimal(distanceWeight) == null)
                    {
                        isError = true;
                        dr["楼间距系数"] = distanceWeight + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[distanceweight]=@distanceweight,");
                }


                if (dt.Columns.Contains("地下室层数"))
                {
                    var basement = dt.Rows[i]["地下室层数"].ToString().Trim();
                    db.basement = (int?)TryParseHelper.StrToInt32(basement);
                    dr["地下室层数"] = basement;
                    if (!string.IsNullOrEmpty(basement) && TryParseHelper.StrToInt32(basement) == null)
                    {
                        isError = true;
                        dr["地下室层数"] = basement + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[basement]=@basement,");
                }

                if (dt.Columns.Contains("备注"))
                {
                    var remark = dt.Rows[i]["备注"].ToString().Trim();
                    db.remark = remark;
                    dr["备注"] = remark;
                    if (!string.IsNullOrEmpty(remark) && remark.Length > 500)
                    {
                        isError = true;
                        dr["备注"] = remark + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[remark]=@remark,");
                }

                if (dt.Columns.Contains("梯户比修正价格"))
                {
                    var elevatorRateWeight = dt.Rows[i]["梯户比修正价格"].ToString().Trim();
                    db.elevatorrateweight = (decimal?)TryParseHelper.StrToDecimal(elevatorRateWeight);
                    dr["梯户比修正价格"] = elevatorRateWeight;
                    if (!string.IsNullOrEmpty(elevatorRateWeight) && TryParseHelper.StrToDecimal(elevatorRateWeight) == null)
                    {
                        isError = true;
                        dr["梯户比修正价格"] = elevatorRateWeight + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[elevatorrateweight]=@elevatorrateweight,");
                }

                if (dt.Columns.Contains("带院子修正价格"))
                {
                    var yardWeight = dt.Rows[i]["带院子修正价格"].ToString().Trim();
                    db.yardweight = (decimal?)TryParseHelper.StrToDecimal(yardWeight);
                    dr["带院子修正价格"] = yardWeight;
                    if (!string.IsNullOrEmpty(yardWeight) && TryParseHelper.StrToDecimal(yardWeight) == null)
                    {
                        isError = true;
                        dr["带院子修正价格"] = yardWeight + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[yardweight]=@yardweight,");
                }

                if (dt.Columns.Contains("是否带院子"))
                {
                    var isYardText = dt.Rows[i]["是否带院子"].ToString().Trim();
                    var isYardValue = YesOrNo(isYardText);
                    db.isyard = isYardValue;
                    dr["是否带院子"] = isYardText;
                    if (!string.IsNullOrEmpty(isYardText) && isYardValue == -1)
                    {
                        isError = true;
                        dr["是否带院子"] = isYardText + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[isyard]=@isyard,");
                }

                if (dt.Columns.Contains("门牌号（地址）"))
                {
                    var doorplate = dt.Rows[i]["门牌号（地址）"].ToString().Trim();
                    db.doorplate = doorplate;
                    dr["门牌号（地址）"] = doorplate;
                    if (!string.IsNullOrEmpty(doorplate) && doorplate.Length > 200)
                    {
                        isError = true;
                        dr["门牌号（地址）"] = doorplate + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[doorplate]=@doorplate,");

                }

                if (dt.Columns.Contains("产权形式"))
                {
                    var rightName = dt.Rows[i]["产权形式"].ToString().Trim();
                    var rightCode = -1;
                    if (codeCach == null)
                    {
                        rightCode = GetCodeByName(rightName, SYS_Code_Dict._产权形式);
                    }
                    else
                    {
                        var obj = codeCach.FirstOrDefault(m => m.codename == rightName && m.id == SYS_Code_Dict._产权形式);
                        rightCode = obj == null ? -1 : obj.code;
                    }
                    db.RightCode = rightCode;
                    dr["产权形式"] = rightName;
                    if (!string.IsNullOrEmpty(rightName) && rightCode == -1)
                    {
                        isError = true;
                        dr["产权形式"] = rightName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[RightCode]=@RightCode,");
                }


                if (dt.Columns.Contains("是否虚拟楼栋"))
                {
                    var isVirtualText = dt.Rows[i]["是否虚拟楼栋"].ToString().Trim();
                    var isVirtualValue = YesOrNo(isVirtualText);
                    db.IsVirtual = isVirtualValue;
                    dr["是否虚拟楼栋"] = isVirtualText;
                    if (!string.IsNullOrEmpty(isVirtualText) && isVirtualValue == -1)
                    {
                        isError = true;
                        dr["是否虚拟楼栋"] = isVirtualText + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[IsVirtual]=@IsVirtual,");
                }


                if (dt.Columns.Contains("楼层分布"))
                {
                    var floorSpread = dt.Rows[i]["楼层分布"].ToString().Trim();
                    db.FloorSpread = floorSpread;
                    dr["楼层分布"] = floorSpread;
                    if (!string.IsNullOrEmpty(floorSpread) && floorSpread.Length > 200)
                    {
                        isError = true;
                        dr["楼层分布"] = floorSpread + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[FloorSpread]=@FloorSpread,");

                }


                if (dt.Columns.Contains("裙楼层数"))
                {
                    var podiumBuildingFloor = dt.Rows[i]["裙楼层数"].ToString().Trim();
                    db.PodiumBuildingFloor = (int?)TryParseHelper.StrToInt32(podiumBuildingFloor);
                    dr["裙楼层数"] = podiumBuildingFloor;
                    if (!string.IsNullOrEmpty(podiumBuildingFloor) && TryParseHelper.StrToInt32(podiumBuildingFloor) == null)
                    {
                        isError = true;
                        dr["裙楼层数"] = podiumBuildingFloor + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[PodiumBuildingFloor]=@PodiumBuildingFloor,");
                }

                if (dt.Columns.Contains("裙楼面积"))
                {
                    var podiumBuildingArea = dt.Rows[i]["裙楼面积"].ToString().Trim();
                    db.PodiumBuildingArea = (decimal?)TryParseHelper.StrToDecimal(podiumBuildingArea);
                    dr["裙楼面积"] = podiumBuildingArea;
                    if (!string.IsNullOrEmpty(podiumBuildingArea) && TryParseHelper.StrToDecimal(podiumBuildingArea) == null)
                    {
                        isError = true;
                        dr["裙楼面积"] = podiumBuildingArea + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[PodiumBuildingArea]=@PodiumBuildingArea,");
                }

                if (dt.Columns.Contains("塔楼面积"))
                {
                    var towerBuildingArea = dt.Rows[i]["塔楼面积"].ToString().Trim();
                    db.TowerBuildingArea = (decimal?)TryParseHelper.StrToDecimal(towerBuildingArea);
                    dr["塔楼面积"] = towerBuildingArea;
                    if (!string.IsNullOrEmpty(towerBuildingArea) && TryParseHelper.StrToDecimal(towerBuildingArea) == null)
                    {
                        isError = true;
                        dr["塔楼面积"] = towerBuildingArea + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[TowerBuildingArea]=@TowerBuildingArea,");
                }

                if (dt.Columns.Contains("地下室总面积"))
                {
                    var basementArea = dt.Rows[i]["地下室总面积"].ToString().Trim();
                    db.BasementArea = (decimal?)TryParseHelper.StrToDecimal(basementArea);
                    dr["地下室总面积"] = basementArea;
                    if (!string.IsNullOrEmpty(basementArea) && TryParseHelper.StrToDecimal(basementArea) == null)
                    {
                        isError = true;
                        dr["地下室总面积"] = basementArea + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[BasementArea]=@BasementArea,");
                }

                if (dt.Columns.Contains("地下室用途"))
                {
                    var basementPurpose = dt.Rows[i]["地下室用途"].ToString().Trim();
                    db.BasementPurpose = basementPurpose;
                    dr["地下室用途"] = basementPurpose;
                    if (!string.IsNullOrEmpty(basementPurpose) && basementPurpose.Length > 200)
                    {
                        isError = true;
                        dr["地下室用途"] = basementPurpose + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[BasementPurpose]=@BasementPurpose,");

                }

                if (dt.Columns.Contains("住宅套数"))
                {
                    var houseNumber = dt.Rows[i]["住宅套数"].ToString().Trim();
                    db.HouseNumber = (int?)TryParseHelper.StrToInt32(houseNumber);
                    dr["住宅套数"] = houseNumber;
                    if (!string.IsNullOrEmpty(houseNumber) && TryParseHelper.StrToInt32(houseNumber) == null)
                    {
                        isError = true;
                        dr["住宅套数"] = houseNumber + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[HouseNumber]=@HouseNumber,");
                }

                if (dt.Columns.Contains("住宅总面积"))
                {
                    var houseArea = dt.Rows[i]["住宅总面积"].ToString().Trim();
                    db.HouseArea = (decimal?)TryParseHelper.StrToDecimal(houseArea);
                    dr["住宅总面积"] = houseArea;
                    if (!string.IsNullOrEmpty(houseArea) && TryParseHelper.StrToDecimal(houseArea) == null)
                    {
                        isError = true;
                        dr["住宅总面积"] = houseArea + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[HouseArea]=@HouseArea,");
                }

                if (dt.Columns.Contains("非住宅套数"))
                {
                    var otherNumber = dt.Rows[i]["非住宅套数"].ToString().Trim();
                    db.OtherNumber = (int?)TryParseHelper.StrToInt32(otherNumber);
                    dr["非住宅套数"] = otherNumber;
                    if (!string.IsNullOrEmpty(otherNumber) && TryParseHelper.StrToInt32(otherNumber) == null)
                    {
                        isError = true;
                        dr["非住宅套数"] = otherNumber + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[OtherNumber]=@OtherNumber,");
                }

                if (dt.Columns.Contains("非住宅总面积"))
                {
                    var otherArea = dt.Rows[i]["非住宅总面积"].ToString().Trim();
                    db.OtherArea = (decimal?)TryParseHelper.StrToDecimal(otherArea);
                    dr["非住宅总面积"] = otherArea;
                    if (!string.IsNullOrEmpty(otherArea) && TryParseHelper.StrToDecimal(otherArea) == null)
                    {
                        isError = true;
                        dr["非住宅总面积"] = otherArea + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[OtherArea]=@OtherArea,");
                }


                if (dt.Columns.Contains("内部装修"))
                {
                    var innerFitmentName = dt.Rows[i]["内部装修"].ToString().Trim();
                    var innerFitmentCode = -1;
                    if (codeCach == null)
                    {
                        innerFitmentCode = GetCodeByName(innerFitmentName, SYS_Code_Dict._装修档次);
                    }
                    else
                    {
                        var obj = codeCach.FirstOrDefault(m => m.codename == innerFitmentName && m.id == SYS_Code_Dict._装修档次);
                        innerFitmentCode = obj == null ? -1 : obj.code;
                    }
                    db.innerFitmentCode = innerFitmentCode;
                    dr["内部装修"] = innerFitmentName;
                    if (!string.IsNullOrEmpty(innerFitmentName) && innerFitmentCode <= 0)
                    {
                        isError = true;
                        dr["内部装修"] = innerFitmentName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[innerFitmentCode]=@innerFitmentCode,");
                }

                if (dt.Columns.Contains("单层户数"))
                {
                    var floorHouseNumber = dt.Rows[i]["单层户数"].ToString().Trim();
                    db.FloorHouseNumber = (int?)TryParseHelper.StrToInt32(floorHouseNumber);
                    dr["单层户数"] = floorHouseNumber;
                    if (!string.IsNullOrEmpty(floorHouseNumber) && TryParseHelper.StrToInt32(floorHouseNumber) == null)
                    {
                        isError = true;
                        dr["单层户数"] = floorHouseNumber + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[FloorHouseNumber]=@FloorHouseNumber,");
                }

                if (dt.Columns.Contains("电梯数量"))
                {
                    var liftNumber = dt.Rows[i]["电梯数量"].ToString().Trim();
                    db.LiftNumber = (int?)TryParseHelper.StrToInt32(liftNumber);
                    dr["电梯数量"] = liftNumber;
                    if (!string.IsNullOrEmpty(liftNumber) && TryParseHelper.StrToInt32(liftNumber) == null)
                    {
                        isError = true;
                        dr["电梯数量"] = liftNumber + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[LiftNumber]=@LiftNumber,");
                }

                if (dt.Columns.Contains("电梯品牌"))
                {
                    var liftBrand = dt.Rows[i]["电梯品牌"].ToString().Trim();
                    db.LiftBrand = liftBrand;
                    dr["电梯品牌"] = liftBrand;
                    if (!string.IsNullOrEmpty(liftBrand) && liftBrand.Length > 50)
                    {
                        isError = true;
                        dr["电梯品牌"] = liftBrand + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[LiftBrand]=@LiftBrand,");

                }

                if (dt.Columns.Contains("设备设施"))
                {
                    var facilities = dt.Rows[i]["设备设施"].ToString().Trim();
                    db.Facilities = facilities;
                    dr["设备设施"] = facilities;
                    if (!string.IsNullOrEmpty(facilities) && facilities.Length > 200)
                    {
                        isError = true;
                        dr["设备设施"] = facilities + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[Facilities]=@Facilities,");

                }

                if (dt.Columns.Contains("管道燃气"))
                {
                    var pipelineGasName = dt.Rows[i]["管道燃气"].ToString().Trim();
                    var pipelineGasCode = -1;
                    if (codeCach == null)
                    {
                        pipelineGasCode = GetCodeByName(pipelineGasName, SYS_Code_Dict._管道燃气);
                    }
                    else
                    {
                        var obj = codeCach.FirstOrDefault(m => m.codename == pipelineGasName && m.id == SYS_Code_Dict._管道燃气);
                        pipelineGasCode = obj == null ? -1 : obj.code;
                    }
                    db.PipelineGasCode = pipelineGasCode;
                    dr["管道燃气"] = pipelineGasName;
                    if (!string.IsNullOrEmpty(pipelineGasName) && pipelineGasCode <= 0)
                    {
                        isError = true;
                        dr["管道燃气"] = pipelineGasName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[PipelineGasCode]=@PipelineGasCode,");
                }

                if (dt.Columns.Contains("采暖方式"))
                {
                    var heatingModeName = dt.Rows[i]["采暖方式"].ToString().Trim();
                    var heatingModeCode = -1;
                    if (codeCach == null)
                    {
                        heatingModeCode = GetCodeByName(heatingModeName, SYS_Code_Dict._采暖方式);
                    }
                    else
                    {
                        var obj = codeCach.FirstOrDefault(m => m.codename == heatingModeName && m.id == SYS_Code_Dict._采暖方式);
                        heatingModeCode = obj == null ? -1 : obj.code;
                    }
                    db.HeatingModeCode = heatingModeCode;
                    dr["采暖方式"] = heatingModeName;
                    if (!string.IsNullOrEmpty(heatingModeName) && heatingModeCode <= 0)
                    {
                        isError = true;
                        dr["采暖方式"] = heatingModeName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[HeatingModeCode]=@HeatingModeCode,");
                }

                if (dt.Columns.Contains("墙体类型"))
                {
                    var wallTypeName = dt.Rows[i]["墙体类型"].ToString().Trim();
                    var wallTypeCode = -1;
                    if (codeCach == null)
                    {
                        wallTypeCode = GetCodeByName(wallTypeName, SYS_Code_Dict._墙体类型);
                    }
                    else
                    {
                        var obj = codeCach.FirstOrDefault(m => m.codename == wallTypeName && m.id == SYS_Code_Dict._墙体类型);
                        wallTypeCode = obj == null ? -1 : obj.code;
                    }
                    db.WallTypeCode = wallTypeCode;
                    dr["墙体类型"] = wallTypeName;
                    if (!string.IsNullOrEmpty(wallTypeName) && wallTypeCode <= 0)
                    {
                        isError = true;
                        dr["墙体类型"] = wallTypeName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[WallTypeCode]=@WallTypeCode,");
                }

                if (dt.Columns.Contains("维护情况"))
                {
                    var maintenanceCodeName = dt.Rows[i]["维护情况"].ToString().Trim();
                    var maintenanceCode = -1;
                    if (codeCach == null)
                    {
                        maintenanceCode = GetCodeByName(maintenanceCodeName, SYS_Code_Dict._建筑等级);
                    }
                    else
                    {
                        var obj = codeCach.FirstOrDefault(m => m.codename == maintenanceCodeName && m.id == SYS_Code_Dict._建筑等级);
                        maintenanceCode = obj == null ? -1 : obj.code;
                    }
                    db.MaintenanceCode = maintenanceCode;
                    dr["维护情况"] = maintenanceCodeName;
                    if (!string.IsNullOrEmpty(maintenanceCodeName) && maintenanceCode <= 0)
                    {
                        isError = true;
                        dr["维护情况"] = maintenanceCodeName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[MaintenanceCode]=@MaintenanceCode,");
                }

                if (i == 0)
                {
                    modifiedProperty.Add("[purposecode]=@purposecode,");
                    modifiedProperty.Add("[totalfloor]=@totalfloor,");
                    modifiedProperty.Add("SaveDateTime=GetDate(),");
                    modifiedProperty.Add("[SaveUser]=@SaveUser,");
                    modifiedProperty.Add("[Valid]=1 ");
                }


                if (i > 0 && integer > 0)
                {
                    if (i % integer == 0)
                    {
                        _importTask.TaskStepsIncreased(taskId);
                    }
                }

                if (isError)
                {
                    dtError.Rows.Add(dr);
                }
                else
                {
                    listTrue.Add(db);
                }
            }
        }
    }
}
