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
        public void ProjectExcelUpload(int cityid, int fxtcompanyid, string filePath, string userid, string taskName)
        {
            var taskId = 0;
            //LogHelper.WriteLog("ProjectExcelUpload", "", userid, cityid, fxtcompanyid, new Exception("WCF NLog test info." + DateTime.Now));
            try
            {
                //在任务列表创建一条记录
                var task = new DAT_ImportTask()
                {
                    TaskName = taskName,
                    ImportType = SYS_Code_Dict.批量导入类型.住宅楼盘信息,
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
                List<DAT_Project> listTrue;//正确数据
                DataTable dtError;//格式错误数据
                DataFilter(taskId, integer, cityid, fxtcompanyid, data, out listTrue, out dtError, out modifiedProperty);

                //错误数据写入Excel
                var fileNamePath = string.Empty;
                if (dtError.Rows.Count > 0)
                {
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "楼盘错误数据.xlsx";
                    var folder = MapPath("NeedHandledFiles/FailureData/" + fxtcompanyid.ToString());
                    fileNamePath = Path.Combine(folder, fileName);
                    excelHelper.CreateExcel(dtError, fileNamePath, folder);

                }

                var failureNum = 0;

                var index4True = 0;//用于统计进度
                foreach (var item in listTrue)
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

                    var projectId = _datProject.IsProjectExist(item.cityid, item.areaid, fxtcompanyid, item.projectname);
                    if (projectId > 0)//存在该楼盘名称则更新该楼盘信息
                    {
                        item.projectid = projectId;
                        item.saveuser = userid;
                        var modifyResult = _datProject.UpdateProjectInfo4Excel(item, fxtcompanyid, modifiedProperty);
                        if (modifyResult <= 0) failureNum = failureNum + 1;
                    }
                    else//新增该楼盘信息
                    {
                        item.creator = userid;
                        item.weight = item.weight == null ? 1 : item.weight;
                        var insertResult = _datProject.AddProject(item);
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

                //LogHelper.WriteLog("ProjectExcelUpload", "", userid, cityid, fxtcompanyid, new Exception("zheshi yige ceshi " + relativePath + "#" + fileNamePath));

                _importTask.UpdateTask(taskId, listTrue.Count - failureNum, dtError.Rows.Count, 0, relativePath, 1, 100);

            }
            catch (Exception ex)
            {
                _importTask.UpdateTask(taskId, 0, 0, 0, "", -1);
                LogHelper.WriteLog("ProjectExcelUpload", "", userid, cityid, fxtcompanyid, ex);
            }
        }

        private void DataFilter(int taskId, double integer, int cityId, int fxtCompanyId, DataTable dt, out List<DAT_Project> listTrue, out DataTable dtError, out List<string> modifiedProperty)
        {
            modifiedProperty = new List<string>();
            listTrue = new List<DAT_Project>();
            dtError = new DataTable();

            //从redis中取出数据
            var areaCach = GetAreaCach(cityId);
            var codeCach = GetCodeCach();


            foreach (DataColumn column in dt.Columns)
                dtError.Columns.Add(column.Caption);

            var count = dt.Rows.Count;

            for (var i = 0; i < count; i++)
            {
                var dp = new DAT_Project();
                var dr = dtError.NewRow();
                dp.fxtcompanyid = fxtCompanyId;
                dp.cityid = cityId;
                dp.valid = 1;
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
                    areaId = obj == null ? 0 : obj.areaid;
                }
                dp.areaid = areaId;
                dr["*行政区"] = areaName;
                if (string.IsNullOrEmpty(areaName) || areaId <= 0)
                {
                    isError = true;
                    dr["*行政区"] = (string.IsNullOrEmpty(areaName) ? "必填" : areaName) + "#error";
                }

                var projectName = dt.Rows[i]["*楼盘名称"].ToString().Trim();
                dp.projectname = projectName;
                dr["*楼盘名称"] = projectName;
                if (string.IsNullOrEmpty(projectName) || projectName.Length > 80)
                {
                    isError = true;
                    dr["*楼盘名称"] = (string.IsNullOrEmpty(projectName) ? "必填" : projectName) + "#error";
                }

                var purposeName = dt.Rows[i]["*主用途"].ToString().Trim();
                int purposeCode = -1;
                if (codeCach == null)
                {
                    purposeCode = GetCodeByName(purposeName, SYS_Code_Dict._土地用途);
                }
                else
                {
                    var obj = codeCach.FirstOrDefault(m => m.codename == purposeName && m.id == SYS_Code_Dict._土地用途);
                    purposeCode = obj == null ? -1 : obj.code;
                }
                dp.purposecode = purposeCode;
                dr["*主用途"] = purposeName;
                if (purposeName.Length < 2 || purposeCode == -1)
                {
                    isError = true;
                    dr["*主用途"] = (string.IsNullOrEmpty(purposeName) ? "必填" : purposeName) + "#error";
                }

                if (dt.Columns.Contains("楼盘别名"))
                {
                    var otherName = dt.Rows[i]["楼盘别名"].ToString().Trim();
                    dp.othername = otherName;
                    dr["楼盘别名"] = otherName;
                    if (otherName.Length > 80)
                    {
                        isError = true;
                        dr["楼盘别名"] = otherName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[OtherName]=@OtherName,");
                }

                if (dt.Columns.Contains("楼盘地址"))
                {
                    var address = dt.Rows[i]["楼盘地址"].ToString().Trim();
                    dp.address = address;
                    dr["楼盘地址"] = address;
                    if (address.Length > 600)
                    {
                        isError = true;
                        dr["楼盘地址"] = address + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[Address]=@Address,");

                }

                if (dt.Columns.Contains("片区"))
                {
                    var subAreaName = dt.Rows[i]["片区"].ToString().Trim();
                    var subAreaId = SubAreaIdByName(subAreaName, areaId);
                    dp.subareaid = subAreaId;
                    dr["片区"] = subAreaName;
                    if (subAreaName.Length > 1 && subAreaId <= 0)
                    {
                        isError = true;
                        dr["片区"] = subAreaName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[SubAreaId]=@SubAreaId,");
                }

                if (dt.Columns.Contains("宗地号"))
                {
                    var landNo = dt.Rows[i]["宗地号"].ToString().Trim();
                    //bool flag = _datProject.ValidFieldNo(cityId, fxtCompanyId, landNo);
                    dp.fieldno = landNo;
                    dr["宗地号"] = landNo;
                    if (!(string.IsNullOrEmpty(landNo)) && landNo.Length > 100)
                    {
                        isError = true;
                        dr["宗地号"] = landNo + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[FieldNo]=@FieldNo,");
                }

                if (dt.Columns.Contains("土地起始日期"))
                {
                    var startDate = dt.Rows[i]["土地起始日期"].ToString().Trim();
                    dp.startdate = (DateTime?)TryParseHelper.StrToDateTime(startDate);
                    dr["土地起始日期"] = startDate;
                    if (!string.IsNullOrEmpty(startDate) && (DateTime?)TryParseHelper.StrToDateTime(startDate) == null)
                    {
                        isError = true;
                        dr["土地起始日期"] = startDate + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[StartDate]=@StartDate,");
                }

                if (dt.Columns.Contains("土地终止日期"))
                {
                    var startendDate = dt.Rows[i]["土地终止日期"].ToString().Trim();
                    dp.startenddate = (DateTime?)TryParseHelper.StrToDateTime(startendDate);
                    dr["土地终止日期"] = startendDate;
                    if (!string.IsNullOrEmpty(startendDate) && (DateTime?)TryParseHelper.StrToDateTime(startendDate) == null)
                    {
                        isError = true;
                        dr["土地终止日期"] = startendDate + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[StartEndDate]=@StartEndDate,");
                }

                if (dt.Columns.Contains("土地使用年限"))
                {
                    var useyear = dt.Rows[i]["土地使用年限"].ToString().Trim();
                    dp.usableyear = (int?)TryParseHelper.StrToInt32(useyear);
                    dr["土地使用年限"] = useyear;
                    if (!string.IsNullOrEmpty(useyear) && TryParseHelper.StrToInt32(useyear) == null)
                    {
                        isError = true;
                        dr["土地使用年限"] = useyear + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[UsableYear]=@UsableYear,");
                }

                if (dt.Columns.Contains("主建筑物类型"))
                {
                    var buildingTypeName = dt.Rows[i]["主建筑物类型"].ToString().Trim();
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
                    dp.buildingtypecode = buildingTypeCode;
                    dr["主建筑物类型"] = buildingTypeName;
                    if (!string.IsNullOrEmpty(buildingTypeName) && buildingTypeCode == -1)
                    {
                        isError = true;
                        dr["主建筑物类型"] = buildingTypeName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[BuildingTypeCode]=@BuildingTypeCode,");
                }

                if (dt.Columns.Contains("容积率"))
                {
                    var cubageRate = dt.Rows[i]["容积率"].ToString().Trim();
                    dp.cubagerate = (decimal?)TryParseHelper.StrToDecimal(cubageRate);
                    dr["容积率"] = cubageRate;
                    if (!string.IsNullOrEmpty(cubageRate) && (decimal?)TryParseHelper.StrToDecimal(cubageRate) == null)
                    {
                        isError = true;
                        dr["容积率"] = cubageRate + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[CubageRate]=@CubageRate,");
                }

                if (dt.Columns.Contains("绿化率"))
                {
                    var greenRate = dt.Rows[i]["绿化率"].ToString().Trim();
                    dp.greenrate = (decimal?)TryParseHelper.StrToDecimal(greenRate);
                    dr["绿化率"] = greenRate;
                    if (!string.IsNullOrEmpty(greenRate) && (decimal?)TryParseHelper.StrToDecimal(greenRate) == null)
                    {
                        isError = true;
                        dr["绿化率"] = greenRate + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[GreenRate]=@GreenRate,");

                }

                if (dt.Columns.Contains("占地面积"))
                {
                    var landArea = dt.Rows[i]["占地面积"].ToString().Trim();
                    dp.landarea = (decimal?)TryParseHelper.StrToDecimal(landArea);
                    dr["占地面积"] = landArea;
                    if (!string.IsNullOrEmpty(landArea) && TryParseHelper.StrToDecimal(landArea) == null)
                    {
                        isError = true;
                        dr["占地面积"] = landArea + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[LandArea]=@LandArea,");
                }

                if (dt.Columns.Contains("建筑面积"))
                {
                    var buildingArea = dt.Rows[i]["建筑面积"].ToString().Trim();
                    dp.buildingarea = (decimal?)TryParseHelper.StrToDecimal(buildingArea);
                    dr["建筑面积"] = buildingArea;
                    if (!string.IsNullOrEmpty(buildingArea) && TryParseHelper.StrToDecimal(buildingArea) == null)
                    {
                        isError = true;
                        dr["建筑面积"] = buildingArea + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[BuildingArea]=@BuildingArea,");
                }

                if (dt.Columns.Contains("可销售面积"))
                {
                    var salableArea = dt.Rows[i]["可销售面积"].ToString().Trim();
                    dp.salablearea = (decimal?)TryParseHelper.StrToDecimal(salableArea);
                    dr["可销售面积"] = salableArea;
                    if (!string.IsNullOrEmpty(salableArea) && TryParseHelper.StrToDecimal(salableArea) == null)
                    {
                        isError = true;
                        dr["可销售面积"] = salableArea + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[salablearea]=@salablearea,");
                }

                if (dt.Columns.Contains("封顶日期"))
                {
                    var coverDate = dt.Rows[i]["封顶日期"].ToString().Trim();
                    dp.coverdate = (DateTime?)TryParseHelper.StrToDateTime(coverDate);
                    dr["封顶日期"] = coverDate;
                    if (!string.IsNullOrEmpty(coverDate) && TryParseHelper.StrToDateTime(coverDate) == null)
                    {
                        isError = true;
                        dr["封顶日期"] = coverDate + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[coverdate]=@coverdate,");
                }

                if (dt.Columns.Contains("竣工日期"))
                {
                    var endDate = dt.Rows[i]["竣工日期"].ToString().Trim();
                    dp.enddate = (DateTime?)TryParseHelper.StrToDateTime(endDate);
                    dr["竣工日期"] = endDate;
                    if (!string.IsNullOrEmpty(endDate) && TryParseHelper.StrToDateTime(endDate) == null)
                    {
                        isError = true;
                        dr["竣工日期"] = endDate + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[EndDate]=@EndDate,");
                }


                if (dt.Columns.Contains("开盘日期"))
                {
                    var saleDate = dt.Rows[i]["开盘日期"].ToString().Trim();
                    dp.saledate = (DateTime?)TryParseHelper.StrToDateTime(saleDate);
                    dr["开盘日期"] = saleDate;
                    if (!string.IsNullOrEmpty(saleDate) && TryParseHelper.StrToDateTime(saleDate) == null)
                    {
                        isError = true;
                        dr["开盘日期"] = saleDate + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[SaleDate]=@SaleDate,");
                }


                if (dt.Columns.Contains("开工日期"))
                {
                    var buildingDate = dt.Rows[i]["开工日期"].ToString().Trim();
                    dp.buildingdate = (DateTime?)TryParseHelper.StrToDateTime(buildingDate);
                    dr["开工日期"] = buildingDate;
                    if (!string.IsNullOrEmpty(buildingDate) && TryParseHelper.StrToDateTime(buildingDate) == null)
                    {
                        isError = true;
                        dr["开工日期"] = buildingDate + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[BuildingDate]=@BuildingDate,");
                }

                if (dt.Columns.Contains("入伙日期"))
                {
                    var joinDate = dt.Rows[i]["入伙日期"].ToString().Trim();
                    dp.joindate = (DateTime?)TryParseHelper.StrToDateTime(joinDate);
                    dr["入伙日期"] = joinDate;
                    if (!string.IsNullOrEmpty(joinDate) && TryParseHelper.StrToDateTime(joinDate) == null)
                    {
                        isError = true;
                        dr["入伙日期"] = joinDate + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[JoinDate]=@JoinDate,");
                }


                if (dt.Columns.Contains("项目均价"))
                {
                    var avgPrice = dt.Rows[i]["项目均价"].ToString().Trim();
                    dp.averageprice = (decimal?)TryParseHelper.StrToDecimal(avgPrice);
                    dr["项目均价"] = avgPrice;
                    if (!string.IsNullOrEmpty(avgPrice) && TryParseHelper.StrToDecimal(avgPrice) == null)
                    {
                        isError = true;
                        dr["项目均价"] = avgPrice + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[AveragePrice]=@AveragePrice,");
                }


                if (dt.Columns.Contains("开盘均价"))
                {
                    var salePrice = dt.Rows[i]["开盘均价"].ToString().Trim();
                    dp.saleprice = (decimal?)TryParseHelper.StrToDecimal(salePrice);
                    dr["开盘均价"] = salePrice;
                    if (!string.IsNullOrEmpty(salePrice) && TryParseHelper.StrToDecimal(salePrice) == null)
                    {
                        isError = true;
                        dr["开盘均价"] = salePrice + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[SalePrice]=@SalePrice,");
                }


                if (dt.Columns.Contains("总栋数"))
                {
                    var buildingNum = dt.Rows[i]["总栋数"].ToString().Trim();
                    dp.buildingnum = (int?)TryParseHelper.StrToInt32(buildingNum);
                    dr["总栋数"] = buildingNum;
                    if (!string.IsNullOrEmpty(buildingNum) && TryParseHelper.StrToInt32(buildingNum) == null)
                    {
                        isError = true;
                        dr["总栋数"] = buildingNum + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[BuildingNum]=@BuildingNum,");
                }

                if (dt.Columns.Contains("总套数"))
                {
                    var totalNum = dt.Rows[i]["总套数"].ToString().Trim();
                    dp.totalnum = (int?)TryParseHelper.StrToInt32(totalNum);
                    dr["总套数"] = totalNum;
                    if (!string.IsNullOrEmpty(totalNum) && TryParseHelper.StrToInt32(totalNum) == null)
                    {
                        isError = true;
                        dr["总套数"] = totalNum + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[TotalNum]=@TotalNum,");
                }

                if (dt.Columns.Contains("车位数"))
                {
                    var parkingNum = dt.Rows[i]["车位数"].ToString().Trim();
                    dp.parkingnumber = (int?)TryParseHelper.StrToInt32(parkingNum);
                    dr["车位数"] = parkingNum;
                    if (!string.IsNullOrEmpty(parkingNum) && TryParseHelper.StrToInt32(parkingNum) == null)
                    {
                        isError = true;
                        dr["车位数"] = parkingNum + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[ParkingNumber]=@ParkingNumber,");
                }

                if (dt.Columns.Contains("车位描述"))
                {
                    var parkingDesc = dt.Rows[i]["车位描述"].ToString().Trim();
                    dp.parkingdesc = parkingDesc;
                    dr["车位描述"] = parkingDesc;
                    if (!string.IsNullOrEmpty(parkingDesc) && parkingDesc.Length > 100)
                    {
                        isError = true;
                        dr["车位描述"] = parkingDesc + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[ParkingDesc]=@ParkingDesc,");
                }

                if (dt.Columns.Contains("开发商"))
                {
                    var developer = dt.Rows[i]["开发商"].ToString().Trim();
                    dp.DeveCompanyName = developer;
                    dr["开发商"] = developer;
                    if (!string.IsNullOrEmpty(developer) && developer.Length > 200)
                    {
                        isError = true;
                        dr["开发商"] = developer + "#error";
                    }

                    // modifiedProperty.Add("[ManagerPrice]=@ManagerPrice,");
                }

                if (dt.Columns.Contains("物管公司"))
                {
                    var managerCompany = dt.Rows[i]["物管公司"].ToString().Trim();
                    dp.ManagerCompanyName = managerCompany;
                    dr["物管公司"] = managerCompany;
                    if (!string.IsNullOrEmpty(managerCompany) && managerCompany.Length > 200)
                    {
                        isError = true;
                        dr["物管公司"] = managerCompany + "#error";
                    }

                }

                if (dt.Columns.Contains("物管费"))
                {
                    var managerPrice = dt.Rows[i]["物管费"].ToString().Trim();
                    dp.managerprice = managerPrice;
                    dr["物管费"] = managerPrice;
                    if (!string.IsNullOrEmpty(managerPrice) && managerPrice.Length > 50)
                    {
                        isError = true;
                        dr["物管费"] = managerPrice + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[ManagerPrice]=@ManagerPrice,");
                }


                if (dt.Columns.Contains("物管电话"))
                {
                    var managerPhone = dt.Rows[i]["物管电话"].ToString().Trim();
                    dp.managertel = managerPhone;
                    dr["物管电话"] = managerPhone;
                    if (!string.IsNullOrEmpty(managerPhone) && managerPhone.Length > 50)
                    {
                        isError = true;
                        dr["物管电话"] = managerPhone + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[ManagerTel]=@ManagerTel,");
                }

                if (dt.Columns.Contains("项目概况"))
                {
                    var detail = dt.Rows[i]["项目概况"].ToString().Trim();
                    dp.detail = detail;
                    dr["项目概况"] = detail;
                    if (i == 0)
                        modifiedProperty.Add("[Detail]=@Detail,");
                }

                if (dt.Columns.Contains("东"))
                {
                    var east = dt.Rows[i]["东"].ToString().Trim();
                    dp.east = east;
                    dr["东"] = east;
                    if (east.Length > 100)//数据格式错误
                    {
                        isError = true;
                        dr["东"] = east + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("east = @east,");
                }

                if (dt.Columns.Contains("西"))
                {
                    var west = dt.Rows[i]["西"].ToString().Trim();
                    dp.west = west;
                    dr["西"] = west;
                    if (west.Length > 100)//数据格式错误
                    {
                        isError = true;
                        dr["西"] = west + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("west = @west,");
                }

                if (dt.Columns.Contains("南"))
                {
                    var south = dt.Rows[i]["南"].ToString().Trim();
                    dp.south = south;
                    dr["南"] = south;
                    if (south.Length > 100)//数据格式错误
                    {
                        isError = true;
                        dr["南"] = south + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("south = @south,");
                }

                if (dt.Columns.Contains("北"))
                {
                    var north = dt.Rows[i]["北"].ToString().Trim();
                    dp.north = north;
                    dr["北"] = north;
                    if (north.Length > 100)//数据格式错误
                    {
                        isError = true;
                        dr["北"] = north + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add(" north = @north,");
                }

                if (dt.Columns.Contains("经度"))
                {
                    var x = dt.Rows[i]["经度"].ToString().Trim();
                    dp.x = (decimal?)TryParseHelper.StrToDecimal(x);
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
                    dp.y = (decimal?)TryParseHelper.StrToDecimal(y);
                    dr["纬度"] = y;
                    if (!string.IsNullOrEmpty(y) && TryParseHelper.StrToDecimal(y) == null)
                    {
                        isError = true;
                        dr["纬度"] = y + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[y]=@y,");
                }

                if (dt.Columns.Contains("内部认购日期"))
                {
                    var innerSaleDate = dt.Rows[i]["内部认购日期"].ToString().Trim();
                    dp.innersaledate = (DateTime?)TryParseHelper.StrToDateTime(innerSaleDate);
                    dr["内部认购日期"] = innerSaleDate;
                    if (!string.IsNullOrEmpty(innerSaleDate) && TryParseHelper.StrToDateTime(innerSaleDate) == null)
                    {
                        isError = true;
                        dr["内部认购日期"] = innerSaleDate + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[innersaledate]=@innersaledate,");
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
                    dp.rightcode = rightCode;
                    dr["产权形式"] = rightName;
                    if (!string.IsNullOrEmpty(rightName) && rightCode <= 0)
                    {
                        isError = true;
                        dr["产权形式"] = rightName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[rightcode]=@rightcode,");
                }

                if (dt.Columns.Contains("办公面积"))
                {
                    var officeArea = dt.Rows[i]["办公面积"].ToString().Trim();
                    dp.officearea = (decimal?)TryParseHelper.StrToDecimal(officeArea);
                    dr["办公面积"] = officeArea;
                    if (!string.IsNullOrEmpty(officeArea) && TryParseHelper.StrToDecimal(officeArea) == null)
                    {
                        isError = true;
                        dr["办公面积"] = officeArea + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[officearea]=@officearea,");
                }

                if (dt.Columns.Contains("商业面积"))
                {
                    var businessArea = dt.Rows[i]["商业面积"].ToString().Trim();
                    dp.businessarea = (decimal?)TryParseHelper.StrToDecimal(businessArea);
                    dr["商业面积"] = businessArea;
                    if (!string.IsNullOrEmpty(businessArea) && TryParseHelper.StrToDecimal(businessArea) == null)
                    {
                        isError = true;
                        dr["商业面积"] = businessArea + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[businessarea]=@businessarea,");
                }

                if (dt.Columns.Contains("工业面积"))
                {
                    var industryArea = dt.Rows[i]["工业面积"].ToString().Trim();
                    dp.industryarea = (decimal?)TryParseHelper.StrToDecimal(industryArea);
                    dr["工业面积"] = industryArea;
                    if (!string.IsNullOrEmpty(industryArea) && TryParseHelper.StrToDecimal(industryArea) == null)
                    {
                        isError = true;
                        dr["工业面积"] = industryArea + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[industryarea]=@industryarea,");
                }

                if (dt.Columns.Contains("其他用途面积"))
                {
                    var otherArea = dt.Rows[i]["其他用途面积"].ToString().Trim();
                    dp.otherarea = (decimal?)TryParseHelper.StrToDecimal(otherArea);
                    dr["其他用途面积"] = otherArea;
                    if (!string.IsNullOrEmpty(otherArea) && TryParseHelper.StrToDecimal(otherArea) == null)
                    {
                        isError = true;
                        dr["其他用途面积"] = otherArea + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[otherArea]=@otherArea,");
                }

                if (dt.Columns.Contains("土地规划用途"))
                {
                    var flag = false;
                    var planPurposeName = dt.Rows[i]["土地规划用途"].ToString().Trim();
                    var planpurposenamelist = planPurposeName.Split(',');
                    var planpurposecodelist = new List<int>();
                    foreach (var planpurpose in planpurposenamelist)
                    {
                        int planpurposecode = GetCodeByName(planpurpose, SYS_Code_Dict._土地规划用途);
                        if (!string.IsNullOrEmpty(planpurpose) && planpurposecode <= 0)
                        {
                            flag = true;
                        }
                        else
                        {
                            planpurposecodelist.Add(planpurposecode);
                        }
                    }
                    dp.planpurpose = string.Join(",", planpurposecodelist.ToArray());
                    dr["土地规划用途"] = planPurposeName;
                    if (flag)
                    {
                        isError = true;
                        dr["土地规划用途"] = planPurposeName + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("[planpurpose]=@planpurpose,");
                }

                if (dt.Columns.Contains("价格调查时间"))
                {
                    var priceDate = dt.Rows[i]["价格调查时间"].ToString().Trim();
                    dp.pricedate = (DateTime?)TryParseHelper.StrToDateTime(priceDate);
                    dr["价格调查时间"] = priceDate;
                    if (!string.IsNullOrEmpty(priceDate) && TryParseHelper.StrToDateTime(priceDate) == null)
                    {
                        isError = true;
                        dr["价格调查时间"] = priceDate + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[pricedate]=@pricedate,");
                }

                if (dt.Columns.Contains("价格修正系数"))
                {
                    var weight = dt.Rows[i]["价格修正系数"].ToString().Trim();
                    dp.weight = (decimal?)TryParseHelper.StrToDecimal(weight);
                    dr["价格修正系数"] = weight;
                    if (!string.IsNullOrEmpty(weight) && TryParseHelper.StrToDecimal(weight) == null)
                    {
                        isError = true;
                        dr["价格修正系数"] = weight + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[weight]=@weight,");
                }

                if (dt.Columns.Contains("是否完成基础数据"))
                {
                    var isComplete = dt.Rows[i]["是否完成基础数据"].ToString().Trim();
                    var isCompleteCode = YesOrNo(isComplete);
                    dp.iscomplete = isCompleteCode;
                    dr["是否完成基础数据"] = isComplete;
                    if (!string.IsNullOrEmpty(isComplete) && isCompleteCode == -1)
                    {
                        isError = true;
                        dr["是否完成基础数据"] = isComplete + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[iscomplete]=@iscomplete,");
                }

                if (dt.Columns.Contains("是否可估"))
                {
                    var isEValueText = dt.Rows[i]["是否可估"].ToString().Trim();
                    var isEValueValue = YesOrNo(isEValueText);
                    dp.isevalue = isEValueValue;
                    dr["是否可估"] = isEValueText;
                    if (!string.IsNullOrEmpty(isEValueText) && isEValueValue == -1)
                    {
                        isError = true;
                        dr["是否可估"] = isEValueText + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[isevalue]=@isevalue,");

                }

                if (dt.Columns.Contains("拼音简写"))
                {
                    var pinYin = dt.Rows[i]["拼音简写"].ToString().Trim();
                    dp.pinyin = string.IsNullOrEmpty(pinYin) ? StringHelper.GetPYString(dp.projectname) : pinYin;
                    dr["拼音简写"] = pinYin;
                    if (!string.IsNullOrEmpty(pinYin) && pinYin.Length > 50)
                    {
                        isError = true;
                        dr["拼音简写"] = pinYin + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[pinyin]=@pinyin,");
                }

                //if (dt.Columns.Contains("环线ID"))
                //{
                //    var areaLineId = dt.Rows[i]["环线ID"].ToString().Trim();
                //    dp.arealineid = (int?)TryParseHelper.StrToInt32(areaLineId);
                //    dr["环线ID"] = areaLineId;
                //    if (!string.IsNullOrEmpty(areaLineId) && TryParseHelper.StrToInt32(areaLineId) == null)
                //    {
                //        isError = true;
                //        dr["环线ID"] = areaLineId + "#error";
                //    }

                //    if (i == 0)
                //        modifiedProperty.Add("[arealineid]=@arealineid,");
                //}

                if (dt.Columns.Contains("楼盘名称全拼"))
                {
                    var pinYinAll = dt.Rows[i]["楼盘名称全拼"].ToString().Trim();
                    dp.pinyinall = string.IsNullOrEmpty(pinYinAll) ? StringHelper.GetAllPYString(dp.projectname) : pinYinAll;
                    dr["楼盘名称全拼"] = pinYinAll;
                    if (!string.IsNullOrEmpty(pinYinAll) && pinYinAll.Length > 500)
                    {
                        isError = true;
                        dr["楼盘名称全拼"] = pinYinAll + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[pinyinall]=@pinyinall,");
                }

                if (dt.Columns.Contains("比例尺"))
                {
                    var xyScale = dt.Rows[i]["比例尺"].ToString().Trim();
                    dp.xyscale = (int?)TryParseHelper.StrToInt32(xyScale);
                    dr["比例尺"] = xyScale;
                    if (!string.IsNullOrEmpty(xyScale) && TryParseHelper.StrToInt32(xyScale) == null)
                    {
                        isError = true;
                        dr["比例尺"] = xyScale + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[xyscale]=@xyscale,");
                }

                if (dt.Columns.Contains("是否空楼盘"))
                {
                    var isEmpty = dt.Rows[i]["是否空楼盘"].ToString().Trim();
                    var isEmptyValue = YesOrNo(isEmpty);
                    dp.isempty = isEmptyValue;
                    dr["是否空楼盘"] = isEmpty;
                    if (!string.IsNullOrEmpty(isEmpty) && isEmptyValue == -1)
                    {
                        isError = true;
                        dr["是否空楼盘"] = isEmpty + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[isempty]=@isempty,");
                }

                //if (dt.Columns.Contains("总楼盘ID"))
                //{
                //    var totalId = dt.Rows[i]["总楼盘ID"].ToString().Trim();
                //    dp.totalid = (int)TryParseHelper.StrToInt32(totalId, 0);
                //    dr["总楼盘ID"] = totalId;
                //    if (!string.IsNullOrEmpty(totalId) && TryParseHelper.StrToInt32(totalId) == null)
                //    {
                //        isError = true;
                //        dr["总楼盘ID"] = totalId + "#error";
                //    }

                //    if (i == 0)
                //        modifiedProperty.Add("[totalid]=@totalid,");
                //}

                if (dt.Columns.Contains("建筑质量"))
                {
                    var buildingQuality = dt.Rows[i]["建筑质量"].ToString().Trim();
                    var buildingQualityCode = -1;
                    if (codeCach == null)
                    {
                        buildingQualityCode = GetCodeByName(buildingQuality, SYS_Code_Dict._建筑等级);
                    }
                    else
                    {
                        var obj = codeCach.FirstOrDefault(m => m.codename == buildingQuality && m.id == SYS_Code_Dict._建筑等级);
                        buildingQualityCode = obj == null ? -1 : obj.code;
                    }

                    dp.BuildingQuality = buildingQualityCode;
                    dr["建筑质量"] = buildingQuality;
                    if (!string.IsNullOrEmpty(buildingQuality) && buildingQualityCode <= 0)
                    {
                        isError = true;
                        dr["建筑质量"] = buildingQuality + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[BuildingQuality]=@BuildingQuality,");
                }

                if (dt.Columns.Contains("小区规模"))
                {
                    var housingScale = dt.Rows[i]["小区规模"].ToString().Trim();
                    var housingScaleCode = -1;
                    if (codeCach == null)
                    {
                        housingScaleCode = GetCodeByName(housingScale, SYS_Code_Dict._小区规模);
                    }
                    else
                    {
                        var obj = codeCach.FirstOrDefault(m => m.codename == housingScale && m.id == SYS_Code_Dict._小区规模);
                        housingScaleCode = obj == null ? -1 : obj.code;
                    }

                    dp.HousingScale = housingScaleCode;
                    dr["小区规模"] = housingScale;
                    if (!string.IsNullOrEmpty(housingScale) && housingScaleCode <= 0)
                    {
                        isError = true;
                        dr["小区规模"] = housingScale + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[HousingScale]=@HousingScale,");
                }

                if (dt.Columns.Contains("楼栋备注"))
                {
                    var buildingDetail = dt.Rows[i]["楼栋备注"].ToString().Trim();
                    dp.BuildingDetail = buildingDetail;
                    dr["楼栋备注"] = buildingDetail;
                    if (!string.IsNullOrEmpty(buildingDetail) && buildingDetail.Length > 500)
                    {
                        isError = true;
                        dr["楼栋备注"] = buildingDetail + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[BuildingDetail]=@BuildingDetail,");
                }

                if (dt.Columns.Contains("房号备注"))
                {
                    var houseDetail = dt.Rows[i]["房号备注"].ToString().Trim();
                    dp.HouseDetail = houseDetail;
                    dr["房号备注"] = houseDetail;
                    if (!string.IsNullOrEmpty(houseDetail) && houseDetail.Length > 500)
                    {
                        isError = true;
                        dr["房号备注"] = houseDetail + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[HouseDetail]=@HouseDetail,");
                }

                if (dt.Columns.Contains("地下室用途"))
                {
                    var basementPurpose = dt.Rows[i]["地下室用途"].ToString().Trim();
                    dp.BasementPurpose = basementPurpose;
                    dr["地下室用途"] = basementPurpose;
                    if (!string.IsNullOrEmpty(basementPurpose) && basementPurpose.Length > 500)
                    {
                        isError = true;
                        dr["地下室用途"] = basementPurpose + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[BasementPurpose]=@BasementPurpose,");
                }

                if (dt.Columns.Contains("物业管理质量"))
                {
                    var managerQualityName = dt.Rows[i]["物业管理质量"].ToString().Trim();
                    var managerQualityCode = -1;
                    if (codeCach == null)
                    {
                        managerQualityCode = GetCodeByName(managerQualityName, SYS_Code_Dict._建筑等级);
                    }
                    else
                    {
                        var obj = codeCach.FirstOrDefault(m => m.codename == managerQualityName && m.id == SYS_Code_Dict._建筑等级);
                        managerQualityCode = obj == null ? -1 : obj.code;
                    }
                    dp.ManagerQuality = managerQualityCode;
                    dr["物业管理质量"] = managerQualityName;
                    if (!string.IsNullOrEmpty(managerQualityName) && managerQualityCode <= 0)
                    {
                        isError = true;
                        dr["物业管理质量"] = managerQualityName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[ManagerQuality]=@ManagerQuality,");
                }

                if (dt.Columns.Contains("设备设施"))
                {
                    var facilities = dt.Rows[i]["设备设施"].ToString().Trim();
                    dp.Facilities = facilities;
                    dr["设备设施"] = facilities;
                    if (!string.IsNullOrEmpty(facilities) && facilities.Length > 500)
                    {
                        isError = true;
                        dr["设备设施"] = facilities + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[Facilities]=@Facilities,");
                }

                if (dt.Columns.Contains("配套等级"))
                {
                    var appendageClassName = dt.Rows[i]["配套等级"].ToString().Trim();
                    var appendageClassCode = -1;
                    if (codeCach == null)
                    {
                        appendageClassCode = GetCodeByName(appendageClassName, SYS_Code_Dict._建筑等级);
                    }
                    else
                    {
                        var obj = codeCach.FirstOrDefault(m => m.codename == appendageClassName && m.id == SYS_Code_Dict._建筑等级);
                        appendageClassCode = obj == null ? -1 : obj.code;
                    }
                    dp.AppendageClass = appendageClassCode;
                    dr["配套等级"] = appendageClassName;
                    if (!string.IsNullOrEmpty(appendageClassName) && appendageClassCode <= 0)
                    {
                        isError = true;
                        dr["配套等级"] = appendageClassName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[AppendageClass]=@AppendageClass,");
                }

                if (dt.Columns.Contains("区域分析"))
                {
                    var regionalAnalysis = dt.Rows[i]["区域分析"].ToString().Trim();
                    dp.RegionalAnalysis = regionalAnalysis;
                    dr["区域分析"] = regionalAnalysis;
                    if (!string.IsNullOrEmpty(regionalAnalysis) && regionalAnalysis.Length > 500)
                    {
                        isError = true;
                        dr["区域分析"] = regionalAnalysis + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[RegionalAnalysis]=@RegionalAnalysis,");
                }

                if (dt.Columns.Contains("有利因素"))
                {
                    var wrinkle = dt.Rows[i]["有利因素"].ToString().Trim();
                    dp.Wrinkle = wrinkle;
                    dr["有利因素"] = wrinkle;
                    if (!string.IsNullOrEmpty(wrinkle) && wrinkle.Length > 200)
                    {
                        isError = true;
                        dr["有利因素"] = wrinkle + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[Wrinkle]=@Wrinkle,");
                }

                if (dt.Columns.Contains("不利因素"))
                {
                    var aversion = dt.Rows[i]["不利因素"].ToString().Trim();
                    dp.Aversion = aversion;
                    dr["不利因素"] = aversion;
                    if (!string.IsNullOrEmpty(aversion) && aversion.Length > 200)
                    {
                        isError = true;
                        dr["不利因素"] = aversion + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[Aversion]=@Aversion,");
                }

                if (dt.Columns.Contains("数据来源"))
                {
                    var sourcename = dt.Rows[i]["数据来源"].ToString().Trim();
                    dp.SourceName = sourcename;
                    dr["数据来源"] = sourcename;
                    if (!string.IsNullOrEmpty(sourcename) && sourcename.Length > 50)
                    {
                        isError = true;
                        dr["数据来源"] = sourcename + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[SourceName]=@SourceName,");
                }
                if (i == 0)
                {
                    modifiedProperty.Add("[PurposeCode]=@PurposeCode,");
                    modifiedProperty.Add("[SaveDateTime]=getdate(),");
                    modifiedProperty.Add("[SaveUser]=@SaveUser, ");
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
                    listTrue.Add(dp);
                }

            }
        }
    }
}
