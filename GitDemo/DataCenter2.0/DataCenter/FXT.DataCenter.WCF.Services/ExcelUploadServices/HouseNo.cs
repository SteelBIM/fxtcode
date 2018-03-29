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
using FXT.DataCenter.Infrastructure.Redis;


namespace FXT.DataCenter.WCF.Services
{
    public partial class ExcelUpload
    {
        public void HouseNoExcelUpload(int cityid, int fxtcompanyid, string filePath, string userid, string taskName)
        {
            var taskId = 0;

            try
            {
                //在任务列表创建一条记录 iscomplete:0,代表否；1，代表是
                var task = new DAT_ImportTask()
                {
                    TaskName = taskName,
                    ImportType = SYS_Code_Dict.批量导入类型.住宅房号信息,
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

                List<string> modifiedProperty;//要修改的属性
                List<DAT_House> listTrue;//正确数据
                DataTable dtError;//格式错误数据
                DataFilter(taskId, integer, cityid, fxtcompanyid, data, out listTrue, out dtError, out modifiedProperty);

                //错误数据写入Excel
                var fileNamePath = string.Empty;
                if (dtError.Rows.Count > 0)
                {
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "房号错误数据.xlsx";
                    var folder = MapPath("NeedHandledFiles/FailureData/" + fxtcompanyid.ToString().Trim());
                    fileNamePath = Path.Combine(folder, fileName);
                    excelHelper.CreateExcel(dtError, fileNamePath, folder);

                }

                var failureNum = 0;
                var index4True = 0;//用于统计进度
                //正确数据添加到Project表中
                //listTrue.ForEach(m => _datHouse.AddHouse(m));
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

                    //var house = _datHouse.ValidateHouseNo(cityid, fxtcompanyid, item.buildingid, item.housename, item.unitno);
                    var house = _datHouse.ValidateHouseNo(cityid, fxtcompanyid, item.buildingid, item.floorno.ToString(), item.unitno);
                    var houseid = house == null ? 0 : house.houseid;
                    if (houseid > 0)//存在该楼栋名称则更新该楼栋信息
                    {
                        item.houseid = houseid;
                        item.saveuser = userid;
                        item.fxtcompanyid = house.fxtcompanyid;
                        var modifyResult = _datHouse.UpdateHouse4Excel(item, fxtcompanyid, modifiedProperty);
                        if (modifyResult <= 0) failureNum = failureNum + 1;
                    }
                    else//新增该楼栋信息
                    {
                        item.creator = userid;
                        var insertResult = _datHouse.AddHouse(item);
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
                LogHelper.WriteLog("HouseNoExcelUpload", "", userid, cityid, fxtcompanyid, ex);
            }
        }

        public void HouseNoNewExcelUpload(int cityid, int fxtcompanyid, string filePath, string userid, string taskName)
        {
            var taskId = 0;

            try
            {
                //在任务列表创建一条记录 iscomplete:0,代表否；1，代表是
                var task = new DAT_ImportTask()
                {
                    TaskName = taskName,
                    ImportType = SYS_Code_Dict.批量导入类型.住宅房号信息,
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

                //第一次转换
                DataTable table = new DataTable();
                #region 创建列
                table.Columns.Add("*行政区");
                table.Columns.Add("*楼盘名称");
                table.Columns.Add("*楼栋名称");
                table.Columns.Add("*房号名称");
                table.Columns.Add("*所在楼层");
                table.Columns.Add("*单元室号");
                table.Columns.Add("实际层");
                if (data.Columns.Contains("建筑面积"))
                {
                    table.Columns.Add("建筑面积");
                }
                if (data.Columns.Contains("套内面积"))
                {
                    table.Columns.Add("套内面积");
                }
                if (data.Columns.Contains("附属房屋类型"))
                {
                    table.Columns.Add("附属房屋类型");
                }
                if (data.Columns.Contains("附属房屋面积"))
                {
                    table.Columns.Add("附属房屋面积");
                }
                if (data.Columns.Contains("户型"))
                {
                    table.Columns.Add("户型");
                }
                if (data.Columns.Contains("户型结构"))
                {
                    table.Columns.Add("户型结构");
                }
                if (data.Columns.Contains("用途"))
                {
                    table.Columns.Add("用途");
                }
                if (data.Columns.Contains("朝向"))
                {
                    table.Columns.Add("朝向");
                }
                if (data.Columns.Contains("景观"))
                {
                    table.Columns.Add("景观");
                }
                if (data.Columns.Contains("通风采光"))
                {
                    table.Columns.Add("通风采光");
                }
                if (data.Columns.Contains("装修"))
                {
                    table.Columns.Add("装修");
                }
                if (data.Columns.Contains("是否有厨房"))
                {
                    table.Columns.Add("是否有厨房");
                }
                if (data.Columns.Contains("阳台数"))
                {
                    table.Columns.Add("阳台数");
                }
                if (data.Columns.Contains("洗手间数"))
                {
                    table.Columns.Add("洗手间数");
                }
                if (data.Columns.Contains("面积确认"))
                {
                    table.Columns.Add("面积确认");
                }
                if (data.Columns.Contains("噪音情况"))
                {
                    table.Columns.Add("噪音情况");
                }
                if (data.Columns.Contains("备注"))
                {
                    table.Columns.Add("备注");
                }
                table.Columns.Add("单价");
                table.Columns.Add("总价");
                table.Columns.Add("价格系数");
                table.Columns.Add("是否可估");

                #endregion
                #region 循环转换
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    string area = data.Rows[i]["*行政区"].ToString();
                    string projectName = data.Rows[i]["*楼盘名称"].ToString();
                    string buildingName = data.Rows[i]["*楼栋名称"].ToString();
                    string houseName = data.Rows[i]["*室号"].ToString();
                    string UnitNo = data.Columns.Contains("单元号") ? data.Rows[i]["单元号"].ToString() : "";
                    int zlc = 0;
                    int.TryParse(data.Rows[i]["*结束楼层"].ToString(), out zlc);
                    int qslc = 0;
                    int.TryParse(data.Rows[i]["*起始楼层"].ToString(), out qslc);
                    for (int t = qslc; t <= zlc; t++)
                    {
                        DataRow row = table.NewRow();
                        row["*行政区"] = area;
                        row["*楼盘名称"] = projectName;
                        row["*楼栋名称"] = buildingName;
                        row["*所在楼层"] = t;
                        row["*单元室号"] = UnitNo + houseName;
                        row["*房号名称"] = UnitNo + t + houseName.TrimStart('\'');
                        row["实际层"] = t;

                        if (data.Columns.Contains("建筑面积"))
                        {
                            row["建筑面积"] = data.Rows[i]["建筑面积"].ToString();
                        }
                        if (data.Columns.Contains("套内面积"))
                        {
                            row["套内面积"] = data.Rows[i]["套内面积"].ToString();
                        }
                        if (data.Columns.Contains("附属房屋类型"))
                        {
                            row["附属房屋类型"] = data.Rows[i]["附属房屋类型"].ToString();
                        }
                        if (data.Columns.Contains("附属房屋面积"))
                        {
                            row["附属房屋面积"] = data.Rows[i]["附属房屋面积"].ToString();
                        }
                        if (data.Columns.Contains("户型"))
                        {
                            row["户型"] = data.Rows[i]["户型"].ToString();
                        }
                        if (data.Columns.Contains("户型结构"))
                        {
                            row["户型结构"] = data.Rows[i]["户型结构"].ToString();
                        }
                        if (data.Columns.Contains("用途"))
                        {
                            row["用途"] = data.Rows[i]["用途"].ToString();
                        }
                        if (data.Columns.Contains("朝向"))
                        {
                            row["朝向"] = data.Rows[i]["朝向"].ToString();
                        }
                        if (data.Columns.Contains("景观"))
                        {
                            row["景观"] = data.Rows[i]["景观"].ToString();
                        }
                        if (data.Columns.Contains("通风采光"))
                        {
                            row["通风采光"] = data.Rows[i]["通风采光"].ToString();
                        }
                        if (data.Columns.Contains("装修"))
                        {
                            row["装修"] = data.Rows[i]["装修"].ToString();
                        }
                        if (data.Columns.Contains("是否有厨房"))
                        {
                            row["是否有厨房"] = data.Rows[i]["是否有厨房"].ToString();
                        }
                        if (data.Columns.Contains("阳台数"))
                        {
                            row["阳台数"] = data.Rows[i]["阳台数"].ToString();
                        }
                        if (data.Columns.Contains("洗手间数"))
                        {
                            row["洗手间数"] = data.Rows[i]["洗手间数"].ToString();
                        }
                        if (data.Columns.Contains("面积确认"))
                        {
                            row["面积确认"] = data.Rows[i]["面积确认"].ToString();
                        }
                        if (data.Columns.Contains("噪音情况"))
                        {
                            row["噪音情况"] = data.Rows[i]["噪音情况"].ToString();
                        }
                        if (data.Columns.Contains("备注"))
                        {
                            row["备注"] = data.Rows[i]["备注"].ToString();
                        }
                        row["是否可估"] = "是";

                        //string BuildArea = data.Columns.Contains("建筑面积") ? data.Rows[i]["建筑面积"].ToString() : "";
                        //string InnerBuildingArea = data.Columns.Contains("套内面积") ? data.Rows[i]["套内面积"].ToString() : "";
                        //string SubHouseType = data.Columns.Contains("附属房屋类型") ? data.Rows[i]["附属房屋类型"].ToString() : "";
                        //string SubHouseArea = data.Columns.Contains("附属房屋面积") ? data.Rows[i]["附属房屋面积"].ToString() : "";
                        //string HouseTypeCode = data.Columns.Contains("户型") ? data.Rows[i]["户型"].ToString() : "";
                        //string StructureCode = data.Columns.Contains("户型结构") ? data.Rows[i]["户型结构"].ToString() : "";
                        //string PurposeCode = data.Columns.Contains("用途") ? data.Rows[i]["用途"].ToString() : "";
                        //string FrontCode = data.Columns.Contains("朝向") ? data.Rows[i]["朝向"].ToString() : "";
                        //string SightCode = data.Columns.Contains("景观") ? data.Rows[i]["景观"].ToString() : "";
                        //string VDCode = data.Columns.Contains("通风采光") ? data.Rows[i]["通风采光"].ToString() : "";
                        //string FitmentCode = data.Columns.Contains("装修") ? data.Rows[i]["装修"].ToString() : "";
                        //string Cookroom = data.Columns.Contains("是否有厨房") ? data.Rows[i]["是否有厨房"].ToString() : "";
                        //string Balcony = data.Columns.Contains("阳台数") ? data.Rows[i]["阳台数"].ToString() : "";
                        //string Toilet = data.Columns.Contains("洗手间数") ? data.Rows[i]["洗手间数"].ToString() : "";
                        //string IsShowBuildingArea = data.Columns.Contains("面积确认") ? data.Rows[i]["面积确认"].ToString() : "";

                        table.Rows.Add(row);
                    }
                }
                #endregion

                var integer = Math.Floor(Convert.ToDouble(table.Rows.Count / 50));

                List<string> modifiedProperty;//要修改的属性
                List<DAT_House> listTrue;//正确数据
                DataTable dtError;//格式错误数据
                DataFilter(taskId, integer, cityid, fxtcompanyid, table, out listTrue, out dtError, out modifiedProperty);

                //错误数据写入Excel
                var fileNamePath = string.Empty;
                if (dtError.Rows.Count > 0)
                {
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "房号错误数据.xlsx";
                    var folder = MapPath("NeedHandledFiles/FailureData/" + fxtcompanyid.ToString().Trim());
                    fileNamePath = Path.Combine(folder, fileName);
                    excelHelper.CreateExcel(dtError, fileNamePath, folder);
                }

                var failureNum = 0;
                var index4True = 0;//用于统计进度
                //正确数据添加到Project表中
                //listTrue.ForEach(m => _datHouse.AddHouse(m));
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

                    //var house = _datHouse.ValidateHouseNo(cityid, fxtcompanyid, item.buildingid, item.housename, item.unitno);
                    var house = _datHouse.ValidateHouseNo(cityid, fxtcompanyid, item.buildingid, item.floorno.ToString(), item.unitno);
                    var houseid = house == null ? 0 : house.houseid;
                    if (houseid > 0)//存在该楼栋名称则更新该楼栋信息
                    {
                        item.houseid = houseid;
                        item.saveuser = userid;
                        item.fxtcompanyid = house.fxtcompanyid;
                        var modifyResult = _datHouse.UpdateHouse4Excel(item, fxtcompanyid, modifiedProperty);
                        if (modifyResult <= 0) failureNum = failureNum + 1;
                    }
                    else//新增该楼栋信息
                    {
                        item.creator = userid;
                        var insertResult = _datHouse.AddHouse(item);
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
                LogHelper.WriteLog("HouseNoExcelUpload", "", userid, cityid, fxtcompanyid, ex);
            }
        }

        private void DataFilter(int taskId, double integer, int cityId, int fxtCompanyId, DataTable dt, out List<DAT_House> listTrue, out DataTable dtError, out List<string> modifiedProperty)
        {
            modifiedProperty = new List<string>();
            listTrue = new List<DAT_House>();
            dtError = new DataTable();

            //从redis中取楼盘，楼栋数据
            var projectsCach = GetProjectCach(cityId, fxtCompanyId);
            var buildingCach = GetBuildingCach(cityId, fxtCompanyId);
            var areaCach = GetAreaCach(cityId);
            var codeCach = GetCodeCach();

            foreach (DataColumn column in dt.Columns)
                dtError.Columns.Add(column.Caption);

            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var dh = new DAT_House();
                var dr = dtError.NewRow();
                var isError = false;

                dh.cityid = cityId;
                dh.fxtcompanyid = fxtCompanyId;
                dh.createtime = DateTime.Now;

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

                var projectId = 0;
                var projectName = dt.Rows[i]["*楼盘名称"].ToString().Trim();

                var projectobj = ProjectIdByName(fxtCompanyId, cityId, areaId, projectName).FirstOrDefault();
                projectId = projectobj == null ? 0 : projectobj.projectid;
                dr["*楼盘名称"] = projectName;
                if (string.IsNullOrEmpty(projectName) || projectName.Length > 80 || projectId <= 0)
                {
                    isError = true;
                    dr["*楼盘名称"] = (string.IsNullOrEmpty(projectName) ? "必填" : projectName) + "#error";
                }

                var buildingName = dt.Rows[i]["*楼栋名称"].ToString().Trim();

                var buildingId = 0;
                buildingId = BuildingIdByName(projectId, buildingName, cityId, fxtCompanyId);

                dh.buildingid = buildingId;
                dr["*楼栋名称"] = buildingName;
                if (string.IsNullOrEmpty(buildingName) || buildingName.Length > 150 || buildingId <= 0)
                {
                    isError = true;
                    dr["*楼栋名称"] = (string.IsNullOrEmpty(buildingName) ? "必填" : buildingName) + "#error";
                }

                var houseName = dt.Rows[i]["*房号名称"].ToString().Replace("‘", "").Replace("'", "").Trim();
                dh.housename = houseName;
                dr["*房号名称"] = houseName;
                if (string.IsNullOrEmpty(houseName) || houseName.Length > 28)
                {
                    isError = true;
                    dr["*房号名称"] = (string.IsNullOrEmpty(houseName) ? "必填" : houseName) + "#error";
                }

                var floorNo = dt.Rows[i]["*所在楼层"].ToString().Trim();
                dh.floorno = TryParseHelper.StrToInt32(floorNo, 0);
                dr["*所在楼层"] = floorNo;
                if (string.IsNullOrEmpty(floorNo) || TryParseHelper.StrToInt32(floorNo, -10) == -10)
                {
                    isError = true;
                    dr["*所在楼层"] = (string.IsNullOrEmpty(floorNo) ? "必填" : floorNo) + "#error";
                }

                var unitNo = dt.Rows[i]["*单元室号"].ToString().Replace("‘", "").Replace("'", "").Trim();
                dh.unitno = unitNo;
                dr["*单元室号"] = unitNo;
                if (string.IsNullOrEmpty(unitNo) || unitNo.Length > 18)
                {
                    isError = true;
                    dr["*单元室号"] = (string.IsNullOrEmpty(unitNo) ? "必填" : unitNo) + "#error";
                }

                if (dt.Columns.Contains("建筑面积"))
                {
                    var buildingArea = dt.Rows[i]["建筑面积"].ToString().Trim();
                    dh.buildarea = (decimal?)TryParseHelper.StrToDecimal(buildingArea);
                    dr["建筑面积"] = buildingArea;
                    if (!string.IsNullOrEmpty(buildingArea) && TryParseHelper.StrToDecimal(buildingArea) == null)
                    {
                        isError = true;
                        dr["建筑面积"] = buildingArea + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("buildarea = @buildarea,");
                }

                if (dt.Columns.Contains("套内面积"))
                {
                    var innerBuildingArea = dt.Rows[i]["套内面积"].ToString().Trim();
                    dh.innerbuildingarea = (decimal?)TryParseHelper.StrToDecimal(innerBuildingArea);
                    dr["套内面积"] = innerBuildingArea;
                    if (!string.IsNullOrEmpty(innerBuildingArea) &&
                        TryParseHelper.StrToDecimal(innerBuildingArea) == null)
                    {
                        isError = true;
                        dr["套内面积"] = innerBuildingArea + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("innerbuildingarea = @innerbuildingarea,");
                }

                if (dt.Columns.Contains("户型"))
                {
                    var houseTypeName = dt.Rows[i]["户型"].ToString().Trim();
                    int houseTypeCode = -1;
                    if (codeCach == null)
                    {
                        houseTypeCode = GetCodeByName(houseTypeName, SYS_Code_Dict._户型);
                    }
                    else
                    {
                        var obj = codeCach.FirstOrDefault(m => m.codename == houseTypeName && m.id == SYS_Code_Dict._户型);
                        houseTypeCode = obj == null ? -1 : obj.code;
                    }

                    dh.housetypecode = houseTypeCode;
                    dr["户型"] = houseTypeName;
                    if (!string.IsNullOrEmpty(houseTypeName) && houseTypeCode == -1)
                    {
                        isError = true;
                        dr["户型"] = houseTypeName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("housetypecode = @housetypecode,");
                }

                if (dt.Columns.Contains("户型结构"))
                {
                    var structrueName = dt.Rows[i]["户型结构"].ToString().Trim();
                    int structureCode = -1;
                    if (codeCach == null)
                    {
                        structureCode = GetCodeByName(structrueName, SYS_Code_Dict._户型结构);
                    }
                    else
                    {
                        var obj = codeCach.FirstOrDefault(m => m.codename == structrueName && m.id == SYS_Code_Dict._户型结构);
                        structureCode = obj == null ? -1 : obj.code;
                    }

                    dh.structurecode = structureCode;
                    dr["户型结构"] = structrueName;
                    if (!string.IsNullOrEmpty(structrueName) && structureCode == -1)
                    {
                        isError = true;
                        dr["户型结构"] = structrueName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("structurecode = @structurecode,");
                }

                if (dt.Columns.Contains("总价"))
                {
                    var totalPrice = dt.Rows[i]["总价"].ToString().Trim();
                    dh.totalprice = (decimal?)TryParseHelper.StrToDecimal(totalPrice);
                    dr["总价"] = totalPrice;
                    if (!string.IsNullOrEmpty(totalPrice) && TryParseHelper.StrToDecimal(totalPrice) == null)
                    {
                        isError = true;
                        dr["总价"] = totalPrice + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("totalprice = @totalprice,");
                }

                if (dt.Columns.Contains("单价"))
                {
                    var unitPrice = dt.Rows[i]["单价"].ToString().Trim();
                    dh.unitprice = (decimal?)TryParseHelper.StrToDecimal(unitPrice);
                    dr["单价"] = unitPrice;
                    if (!string.IsNullOrEmpty(unitPrice) && TryParseHelper.StrToDecimal(unitPrice) == null)
                    {
                        isError = true;
                        dr["单价"] = unitPrice + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("unitprice = @unitprice,");
                }

                if (dt.Columns.Contains("朝向"))
                {
                    var frontName = dt.Rows[i]["朝向"].ToString().Trim();
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
                    dh.frontcode = frontCode;
                    dr["朝向"] = frontName;
                    if (!string.IsNullOrEmpty(frontName) && frontCode == -1)
                    {
                        isError = true;
                        dr["朝向"] = frontName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("frontcode = @frontcode,");
                }

                if (dt.Columns.Contains("景观"))
                {
                    var sightName = dt.Rows[i]["景观"].ToString().Trim();
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
                    dh.sightcode = sightCode;
                    dr["景观"] = sightName;
                    if (!string.IsNullOrEmpty(sightName) && sightCode == -1)
                    {
                        isError = true;
                        dr["景观"] = sightName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("sightcode = @sightcode,");
                }

                if (dt.Columns.Contains("用途"))
                {
                    var purposeName = dt.Rows[i]["用途"].ToString().Trim();
                    var purposeCode = GetCodeByName(purposeName, SYS_Code_Dict._居住用途);
                    dh.purposecode = purposeCode;
                    dr["用途"] = purposeName;
                    if (!string.IsNullOrEmpty(purposeName) && purposeCode == -1)
                    {
                        isError = true;
                        dr["用途"] = purposeName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("purposecode = @purposecode,");
                }

                if (dt.Columns.Contains("价格系数"))
                {
                    var weight = dt.Rows[i]["价格系数"].ToString().Trim();
                    dh.weight = (decimal?)TryParseHelper.StrToDecimal(weight);
                    dr["价格系数"] = weight;
                    if (!string.IsNullOrEmpty(weight) && TryParseHelper.StrToDecimal(weight) == null)
                    {
                        isError = true;
                        dr["价格系数"] = weight + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("weight = @weight,");
                }

                if (dt.Columns.Contains("备注"))
                {
                    var remark = dt.Rows[i]["备注"].ToString().Trim();
                    dh.remark = remark;
                    dr["备注"] = remark;
                    if (!string.IsNullOrEmpty(remark) && remark.Length > 500)
                    {
                        isError = true;
                        dr["备注"] = remark + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[remark]=@remark,");
                }

                if (dt.Columns.Contains("是否可估"))
                {
                    var isEValueText = dt.Rows[i]["是否可估"].ToString().Trim();
                    var isEValueValue = YesOrNo(isEValueText);
                    dh.isevalue = isEValueValue;
                    dr["是否可估"] = isEValueText;
                    if (!string.IsNullOrEmpty(isEValueText) && isEValueValue == -1)
                    {
                        isError = true;
                        dr["是否可估"] = isEValueText + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[isevalue]=@isevalue,");
                }

                if (dt.Columns.Contains("面积确认"))
                {
                    var isShowBuildingAreaText = dt.Rows[i]["面积确认"].ToString().Trim();
                    var isShowBuildingAreaValue = YesOrNo(isShowBuildingAreaText);
                    dh.isshowbuildingarea = isShowBuildingAreaValue;
                    dr["面积确认"] = isShowBuildingAreaText;
                    if (!string.IsNullOrEmpty(isShowBuildingAreaText) && isShowBuildingAreaValue == -1)
                    {
                        isError = true;
                        dr["面积确认"] = isShowBuildingAreaText + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[isshowbuildingarea]=@isshowbuildingarea,");
                }

                if (dt.Columns.Contains("附属房屋类型"))
                {
                    var subHouseTypeName = dt.Rows[i]["附属房屋类型"].ToString().Trim();
                    var subHouseTypeCode = -1;
                    if (codeCach == null)
                    {
                        subHouseTypeCode = GetCodeByName(subHouseTypeName, SYS_Code_Dict._附属房屋类型);
                    }
                    else
                    {
                        var obj = codeCach.FirstOrDefault(m => m.codename == subHouseTypeName && m.id == SYS_Code_Dict._附属房屋类型);
                        subHouseTypeCode = obj == null ? -1 : obj.code;
                    }
                    dh.subhousetype = subHouseTypeCode;
                    dr["附属房屋类型"] = subHouseTypeName;
                    if (!string.IsNullOrEmpty(subHouseTypeName) && subHouseTypeCode == -1)
                    {
                        isError = true;
                        dr["附属房屋类型"] = subHouseTypeName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[subhousetype]=@subhousetype,");
                }

                if (dt.Columns.Contains("附属房屋面积"))
                {
                    var subHouseArea = dt.Rows[i]["附属房屋面积"].ToString().Trim();
                    dh.subhousearea = (decimal?)TryParseHelper.StrToDecimal(subHouseArea);
                    dr["附属房屋面积"] = subHouseArea;
                    if (!string.IsNullOrEmpty(subHouseArea) && TryParseHelper.StrToDecimal(subHouseArea) == null)
                    {
                        isError = true;
                        dr["附属房屋面积"] = subHouseArea + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[subhousearea]=@subhousearea,");
                }

                if (dt.Columns.Contains("实际层"))
                {
                    var nominalFloor = dt.Rows[i]["实际层"].ToString().Trim();
                    dh.nominalfloor = nominalFloor;
                    dr["实际层"] = nominalFloor;
                    if (!string.IsNullOrEmpty(nominalFloor) && nominalFloor.Length > 20)
                    {
                        isError = true;
                        dr["实际层"] = nominalFloor + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[nominalfloor]=@nominalfloor,");
                }

                if (dt.Columns.Contains("通风采光"))
                {
                    var vdName = dt.Rows[i]["通风采光"].ToString().Trim();
                    var vdCode = -1;
                    if (codeCach == null)
                    {
                        vdCode = GetCodeByName(vdName, SYS_Code_Dict._通风采光);
                    }
                    else
                    {
                        var obj = codeCach.FirstOrDefault(m => m.codename == vdName && m.id == SYS_Code_Dict._通风采光);
                        vdCode = obj == null ? -1 : obj.code;
                    }
                    dh.VDCode = vdCode;
                    dr["通风采光"] = vdName;
                    if (!string.IsNullOrEmpty(vdName) && vdCode == -1)
                    {
                        isError = true;
                        dr["通风采光"] = vdName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[VDCode]=@VDCode,");
                }

                if (dt.Columns.Contains("装修"))
                {
                    var fitmentName = dt.Rows[i]["装修"].ToString().Trim();
                    var fitmentCode = -1;
                    if (codeCach == null)
                    {
                        fitmentCode = GetCodeByName(fitmentName, SYS_Code_Dict._装修档次);
                    }
                    else
                    {
                        var obj = codeCach.FirstOrDefault(m => m.codename == fitmentName && m.id == SYS_Code_Dict._装修档次);
                        fitmentCode = obj == null ? -1 : obj.code;
                    }
                    dh.FitmentCode = fitmentCode;
                    dr["装修"] = fitmentName;
                    if (!string.IsNullOrEmpty(fitmentName) && fitmentCode == -1)
                    {
                        isError = true;
                        dr["装修"] = fitmentName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[FitmentCode]=@FitmentCode,");
                }

                if (dt.Columns.Contains("是否有厨房"))
                {
                    var cookroomText = dt.Rows[i]["是否有厨房"].ToString().Trim();
                    var cookroomValue = YesOrNo(cookroomText);
                    dh.Cookroom = cookroomValue;
                    dr["是否有厨房"] = cookroomText;
                    if (!string.IsNullOrEmpty(cookroomText) && cookroomValue == -1)
                    {
                        isError = true;
                        dr["是否有厨房"] = cookroomText + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[Cookroom]=@Cookroom,");
                }

                if (dt.Columns.Contains("阳台数"))
                {
                    var balcony = dt.Rows[i]["阳台数"].ToString().Trim();
                    dh.Balcony = (int?)TryParseHelper.StrToInt32(balcony);
                    dr["阳台数"] = balcony;
                    if (!string.IsNullOrEmpty(balcony) && TryParseHelper.StrToInt32(balcony) == null)
                    {
                        isError = true;
                        dr["阳台数"] = balcony + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[Balcony]=@Balcony,");
                }

                if (dt.Columns.Contains("洗手间数"))
                {
                    var toilet = dt.Rows[i]["洗手间数"].ToString().Trim();
                    dh.Toilet = (int?)TryParseHelper.StrToInt32(toilet);
                    dr["洗手间数"] = toilet;
                    if (!string.IsNullOrEmpty(toilet) && TryParseHelper.StrToInt32(toilet) == null)
                    {
                        isError = true;
                        dr["洗手间数"] = toilet + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[Toilet]=@Toilet,");
                }

                if (dt.Columns.Contains("噪音情况"))
                {
                    var noiseCodeName = dt.Rows[i]["噪音情况"].ToString().Trim();
                    var noiseCode = -1;
                    if (codeCach == null)
                    {
                        noiseCode = GetCodeByName(noiseCodeName, SYS_Code_Dict._噪音情况);
                    }
                    else
                    {
                        var obj = codeCach.FirstOrDefault(m => m.codename == noiseCodeName && m.id == SYS_Code_Dict._噪音情况);
                        noiseCode = obj == null ? -1 : obj.code;
                    }
                    dh.NoiseCode = noiseCode;
                    dr["噪音情况"] = noiseCodeName;
                    if (!string.IsNullOrEmpty(noiseCodeName) && noiseCode == -1)
                    {
                        isError = true;
                        dr["噪音情况"] = noiseCodeName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[NoiseCode]=@NoiseCode,");
                }

                if (i > 0 && integer > 0)
                {
                    if (i % integer == 0)
                    {
                        _importTask.TaskStepsIncreased(taskId);
                    }
                }

                if (i == 0)
                {
                    modifiedProperty.Add("housename = @housename,");
                    modifiedProperty.Add("savedatetime = getdate(),");
                    modifiedProperty.Add("saveuser = @saveuser,");
                    modifiedProperty.Add("[Valid]=1 ");
                }

                if (isError)
                {
                    dtError.Rows.Add(dr);
                }
                else
                {
                    listTrue.Add(dh);
                }
            }
        }
    }
}
