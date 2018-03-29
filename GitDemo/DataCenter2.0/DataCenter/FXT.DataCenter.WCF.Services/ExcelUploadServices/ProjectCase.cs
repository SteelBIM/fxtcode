using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
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
        public void ProjectCaseExcelUpload(int cityid, int fxtcompanyid, string filePath, string userid, string taskName)
        {
            var taskId = 0;

            try
            {
                //在任务列表创建一条记录   iscomplete:0,代表否；1，代表是
                var task = new DAT_ImportTask
                {
                    TaskName = taskName,
                    ImportType = SYS_Code_Dict.批量导入类型.楼盘案例,
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

                List<DAT_Case> correctData;//正确数据
                List<DAT_CaseTemp> projectNameMisMatch;//楼盘名称不匹配数据
                DataTable dataFormatError;//格式错误数据
                DataFilter(taskId, integer, cityid, fxtcompanyid, out correctData, out projectNameMisMatch, out dataFormatError, data, userid);

                //错误数据写入excel
                string fileNamePath = string.Empty;
                if (dataFormatError.Rows.Count > 0)
                {
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "楼盘案例格式错误数据.xlsx";
                    var folder = MapPath("NeedHandledFiles/FailureData/" + fxtcompanyid.ToString().Trim());
                    fileNamePath = Path.Combine(folder, fileName);
                    excelHelper.CreateExcel(dataFormatError, fileNamePath, folder);
                }

                var failureNum = 0;
                var index4True = 0;//用于统计进度
                //正确数据添加到数据表
                foreach (var item in correctData)
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

                    item.creator = userid;
                    item.saveuser = userid;
                    //if (item.buildingtypecode == null || item.buildingtypecode <= 0)
                    //{
                    //    if (item.floornumber == null || item.floornumber <= 0)
                    //    {
                    //        var project = _datProject.GetSingleProject(item.projectid, (int)item.cityid, item.fxtcompanyid);
                    //        item.buildingtypecode = project == null ? -1 : project.buildingtypecode;
                    //    }
                    //    else
                    //    {
                    //        item.buildingtypecode = BuildingTypeCode((int)item.floornumber);
                    //    }
                    //}

                    var insertResult = _projectCase.AddProjectCase(item);
                    if (insertResult <= 0) failureNum = failureNum + 1;
                }

                //楼盘名称不匹配数据添加到临时表
                projectNameMisMatch.ForEach(m => { m.TaskID = taskId; _projectCaseTask.AddProjectNameMisMatch(m); });

                //更新任务状态
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
                _importTask.UpdateTask(taskId, correctData.Count - failureNum, dataFormatError.Rows.Count, projectNameMisMatch.Count, relativePath, 1, 100);

            }
            catch (Exception ex)
            {
                _importTask.UpdateTask(taskId, 0, 0, 0, "", -1);
                LogHelper.WriteLog("ProjectCaseExcelUpload", "", userid, cityid, fxtcompanyid, ex);
            }

        }

        //数据筛选（筛选出格式错误数据，楼盘名称不匹配数据，以及正确数据）
        private void DataFilter(int taskId, double integer, int cityId, int fxtCompanyId, out List<DAT_Case> correctData, out List<DAT_CaseTemp> projectNameMisMatch, out DataTable dataFormatError, DataTable dt, string userid)
        {
            correctData = new List<DAT_Case>();
            projectNameMisMatch = new List<DAT_CaseTemp>();
            dataFormatError = new DataTable();

            //从redis中取出数据
            //var projectsCach = GetProjectCach(cityId, fxtCompanyId);
            //var buildingCach = GetBuildingCach(cityId, fxtCompanyId);
            var areaCach = GetAreaCach(cityId);
            var codeCach = GetCodeCach();


            foreach (DataColumn column in dt.Columns)
                dataFormatError.Columns.Add(column.ColumnName);


            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var isMisMatch = false;
                var isError = false;
                var projectcase = new DAT_Case();
                var datCaseTemp = new DAT_CaseTemp();
                var dr = dataFormatError.NewRow();

                var areaid = 0;
                var areaName = "";
                if (dt.Columns.Contains("行政区"))
                {
                    areaName = dt.Rows[i]["行政区"].ToString().Trim();
                    if (areaCach == null)
                    {
                        areaid = GetAreaId(cityId, areaName);
                    }
                    else
                    {
                        var obj = areaCach.FirstOrDefault(m => m.areaname == areaName);
                        areaid = obj == null ? 0 : obj.areaid;
                    }
                    datCaseTemp.AreaId = areaid;
                    datCaseTemp.AreaName = areaName;
                    projectcase.areaid = areaid;
                    projectcase.AreaName = areaName;
                    dr["行政区"] = areaName;
                    if (!string.IsNullOrEmpty(areaName) && areaName.Length > 20)
                    {
                        isError = true;
                        dr["行政区"] = areaName + "#error";
                    }
                }

                var projectid = 0;
                var projectName = dt.Rows[i]["*楼盘名称"].ToString().Trim();
                var projectIds = ProjectIdByName(fxtCompanyId, cityId, areaid, projectName).Select(m => m.projectid).Distinct();
                projectid = projectIds.FirstOrDefault();
                projectcase.projectid = projectid;
                datCaseTemp.ProjectName = projectName;
                dr["*楼盘名称"] = projectName;
                if (string.IsNullOrEmpty(projectName) || projectIds.Count() > 1)//楼盘名称不匹配
                {
                    isError = true;
                    dr["*楼盘名称"] = (string.IsNullOrEmpty(projectName) ? "必填" : projectName) + "#error";
                }
                else if (!string.IsNullOrEmpty(projectName) && projectid <= 0)
                {
                    var pid = GetProjectMatchProjectId(projectName, areaName, cityId, fxtCompanyId);
                    if (pid != null && pid.ProjectNameId > 0)
                    {
                        projectcase.projectid = (int)pid.ProjectNameId;
                        projectcase.ProjectName = pid.ProjectName;
                        datCaseTemp.ProjectId = pid.ProjectNameId;
                        datCaseTemp.ProjectName = pid.ProjectName;
                    }
                    else
                    {
                        isMisMatch = true;
                    }
                }
                else if (projectid > 0)
                {
                    isMisMatch = false;
                }
                else
                {
                    isMisMatch = true;
                }

                var casedate = dt.Rows[i]["*案例时间"].ToString().Trim();
                datCaseTemp.CaseDate = TryParseHelper.StrToDateTime(casedate) == null ? (DateTime)SqlDateTime.MinValue : (DateTime)TryParseHelper.StrToDateTime(casedate);
                projectcase.casedate = TryParseHelper.StrToDateTime(casedate) == null ? (DateTime)SqlDateTime.MinValue : (DateTime)TryParseHelper.StrToDateTime(casedate);
                dr["*案例时间"] = casedate;
                if (string.IsNullOrEmpty(casedate) || TryParseHelper.StrToDateTime(casedate) == null)
                {
                    isError = true;
                    dr["*案例时间"] = (string.IsNullOrEmpty(casedate) ? "必填" : casedate) + "#error";
                }

                var purposeName = dt.Rows[i]["*用途"].ToString().Trim();
                int purposecode = -1;
                if (codeCach == null)
                {
                    purposecode = GetCodeByName(purposeName, SYS_Code_Dict._居住用途);
                }
                else
                {
                    var obj = codeCach.FirstOrDefault(m => m.codename == purposeName && m.id == SYS_Code_Dict._居住用途);
                    purposecode = obj == null ? -1 : obj.code;
                }

                datCaseTemp.PurposeCode = purposecode;
                projectcase.purposecode = purposecode;
                dr["*用途"] = purposeName;
                if (string.IsNullOrEmpty(purposeName) || purposecode == -1)
                {
                    isError = true;
                    dr["*用途"] = (string.IsNullOrEmpty(purposeName) ? "必填" : purposeName) + "#error";
                }

                bool buildingAreaFlag = false;
                var buildingarea = dt.Rows[i]["*建筑面积"].ToString().Trim();
                datCaseTemp.BuildingArea = (decimal?)TryParseHelper.StrToDecimal(buildingarea);
                projectcase.buildingarea = (decimal?)TryParseHelper.StrToDecimal(buildingarea);
                dr["*建筑面积"] = buildingarea;
                if (string.IsNullOrEmpty(buildingarea) || TryParseHelper.StrToDecimal(buildingarea) == null)
                {
                    isError = true;
                    dr["*建筑面积"] = (string.IsNullOrEmpty(buildingarea) ? "必填" : buildingarea) + "#error";
                }
                else
                {
                    buildingAreaFlag = true;
                }

                bool unitPriceFlag = false;
                var unitprice = dt.Rows[i]["*单价"].ToString().Trim();
                datCaseTemp.UnitPrice = (decimal?)TryParseHelper.StrToDecimal(unitprice);
                projectcase.unitprice = (decimal?)TryParseHelper.StrToDecimal(unitprice);
                dr["*单价"] = unitprice;
                if (string.IsNullOrEmpty(unitprice) || TryParseHelper.StrToDecimal(unitprice) == null || (decimal?)TryParseHelper.StrToDecimal(unitprice) <= 0)
                {
                    isError = true;
                    dr["*单价"] = (string.IsNullOrEmpty(unitprice) ? "必填" : unitprice) + "#error";
                }
                else
                {
                    unitPriceFlag = true;
                }

                var totalPrice = dt.Rows[i]["*总价"].ToString().Trim();
                datCaseTemp.TotalPrice = (decimal?)TryParseHelper.StrToDecimal(totalPrice);
                projectcase.totalprice = (decimal?)TryParseHelper.StrToDecimal(totalPrice);
                dr["*总价"] = totalPrice;
                if (string.IsNullOrEmpty(totalPrice) || TryParseHelper.StrToDecimal(totalPrice) == null)
                {
                    isError = true;
                    dr["*总价"] = (string.IsNullOrEmpty(totalPrice) ? "必填" : totalPrice) + "#error";
                }
                else if (buildingAreaFlag && unitPriceFlag && Math.Abs((decimal)datCaseTemp.BuildingArea * (decimal)datCaseTemp.UnitPrice - (decimal)datCaseTemp.TotalPrice) / (decimal)datCaseTemp.TotalPrice > (decimal)0.01)
                {
                    isError = true;
                    dr["*总价"] = "总价有误差" + totalPrice + "#error";
                }

                var caseTypeName = dt.Rows[i]["*案例类型"].ToString().Trim();
                var caseTypeCode = -1;
                if (codeCach == null)
                {
                    caseTypeCode = GetCodeByName(caseTypeName, SYS_Code_Dict._案例类型);
                }
                else
                {
                    var obj = codeCach.FirstOrDefault(m => m.codename == caseTypeName && m.id == SYS_Code_Dict._案例类型);
                    caseTypeCode = obj == null ? -1 : obj.code;
                }

                datCaseTemp.CaseTypeCode = caseTypeCode;
                projectcase.casetypecode = caseTypeCode;
                dr["*案例类型"] = caseTypeName;
                if (string.IsNullOrEmpty(caseTypeName) || caseTypeCode == -1)
                {
                    isError = true;
                    dr["*案例类型"] = (string.IsNullOrEmpty(caseTypeName) ? "必填" : caseTypeName) + "#error";
                }

                if (dt.Columns.Contains("楼栋名称"))
                {
                    var buildingName = dt.Rows[i]["楼栋名称"].ToString().Trim();
                    var buildingId = 0;
                    buildingId = BuildingIdByName(projectid, buildingName, cityId, fxtCompanyId);
                    //if (buildingCach == null)
                    //{
                    //    buildingId = BuildingIdByName(projectid, buildingName, cityId, fxtCompanyId);
                    //}
                    //else
                    //{
                    //    var obj = buildingCach.FirstOrDefault(m => m.projectid == projectid && m.buildingname == buildingName);
                    //    buildingId = obj == null ? 0 : obj.buildingid;
                    //}
                    datCaseTemp.BuildingId = buildingId;
                    datCaseTemp.BuildingName = buildingName;
                    projectcase.buildingid = buildingId;
                    projectcase.buildingname = buildingName;
                    dr["楼栋名称"] = buildingName;
                    if (!string.IsNullOrEmpty(buildingName) && buildingId <= 0)//数据错误
                    {
                        isError = true;
                        dr["楼栋名称"] = buildingName + "#error";
                    }
                }

                if (dt.Columns.Contains("楼层"))
                {
                    var floornumber = dt.Rows[i]["楼层"].ToString().Trim();
                    datCaseTemp.FloorNumber = (int?)TryParseHelper.StrToInt32(floornumber);
                    projectcase.floornumber = (int?)TryParseHelper.StrToInt32(floornumber);
                    dr["楼层"] = floornumber;
                    if (!string.IsNullOrEmpty(floornumber) && TryParseHelper.StrToInt32(floornumber) == null)
                    {
                        isError = true;
                        dr["楼层"] = floornumber + "#error";
                    }
                }

                if (dt.Columns.Contains("房号"))
                {
                    var houseNo = dt.Rows[i]["房号"].ToString().Trim();
                    datCaseTemp.HouseNo = houseNo;
                    projectcase.houseno = houseNo;
                    dr["房号"] = houseNo;
                    if (houseNo.Length > 100)
                    {
                        isError = true;
                        dr["房号"] = houseNo + "#error";
                    }
                }

                if (dt.Columns.Contains("总楼层"))
                {
                    var totalFloor = dt.Rows[i]["总楼层"].ToString().Trim();
                    datCaseTemp.TotalFloor = (int?)TryParseHelper.StrToInt32(totalFloor);
                    projectcase.totalfloor = (int?)TryParseHelper.StrToInt32(totalFloor);
                    dr["总楼层"] = totalFloor;
                    if (!string.IsNullOrEmpty(totalFloor) && TryParseHelper.StrToInt32(totalFloor) == null)
                    {
                        isError = true;
                        dr["总楼层"] = totalFloor + "#error";
                    }
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
                    datCaseTemp.FrontCode = frontCode;
                    projectcase.frontcode = frontCode;
                    dr["朝向"] = frontName;
                    if (!string.IsNullOrEmpty(frontName) && frontCode == -1)
                    {
                        isError = true;
                        dr["朝向"] = frontName + "#error";
                    }

                }

                if (dt.Columns.Contains("币种"))
                {
                    var momenyTypeName = dt.Rows[i]["币种"].ToString().Trim();
                    var momenyTypeCode = -1;
                    if (codeCach == null)
                    {
                        momenyTypeCode = GetCodeByName(momenyTypeName, SYS_Code_Dict._币种);
                    }
                    else
                    {
                        var obj = codeCach.FirstOrDefault(m => m.codename == momenyTypeName && m.id == SYS_Code_Dict._币种);
                        momenyTypeCode = obj == null ? -1 : obj.code;
                    }

                    datCaseTemp.MoneyUnitCode = momenyTypeCode;
                    projectcase.moneyunitcode = momenyTypeCode;
                    dr["币种"] = momenyTypeName;
                    if (!string.IsNullOrEmpty(momenyTypeName) && momenyTypeCode == -1)
                    {
                        isError = true;
                        dr["币种"] = momenyTypeName + "#error";
                    }
                }

                if (dt.Columns.Contains("户型结构"))
                {
                    var structrueName = dt.Rows[i]["户型结构"].ToString().Trim();
                    var structurecode = -1;
                    if (codeCach == null)
                    {
                        structurecode = GetCodeByName(structrueName, SYS_Code_Dict._户型结构);
                    }
                    else
                    {
                        var obj = codeCach.FirstOrDefault(m => m.codename == structrueName && m.id == SYS_Code_Dict._户型结构);
                        structurecode = obj == null ? -1 : obj.code;
                    }
                    datCaseTemp.StructureCode = structurecode;
                    projectcase.structurecode = structurecode;
                    dr["户型结构"] = structrueName;
                    if (!string.IsNullOrEmpty(structrueName) && structurecode == -1)
                    {
                        isError = true;
                        dr["户型结构"] = structrueName + "#error";
                    }
                }

                if (dt.Columns.Contains("建筑类型"))
                {
                    var buildingTypeName = dt.Rows[i]["建筑类型"].ToString().Trim();
                    var buildingtypeCode = -1;
                    if (codeCach == null)
                    {
                        buildingtypeCode = GetCodeByName(buildingTypeName, SYS_Code_Dict._建筑类型);
                    }
                    else
                    {
                        var obj = codeCach.FirstOrDefault(m => m.codename == buildingTypeName && m.id == SYS_Code_Dict._建筑类型);
                        buildingtypeCode = obj == null ? -1 : obj.code;
                    }
                    datCaseTemp.BuildingTypeCode = buildingtypeCode;
                    projectcase.buildingtypecode = buildingtypeCode;
                    dr["建筑类型"] = buildingTypeName;
                    if (!string.IsNullOrEmpty(buildingTypeName) && buildingtypeCode == -1)
                    {
                        isError = true;
                        dr["建筑类型"] = buildingTypeName + "#error";
                    }
                }

                if (dt.Columns.Contains("户型"))
                {
                    var houseTypeName = dt.Rows[i]["户型"].ToString().Trim();
                    if (!houseTypeName.Equals("单房"))
                    {
                        houseTypeName = houseTypeName.Replace("房", "室");
                    }
                    var houseTypeCode = -1;
                    if (codeCach == null)
                    {
                        houseTypeCode = GetCodeByName(houseTypeName, SYS_Code_Dict._户型);
                    }
                    else
                    {
                        var obj = codeCach.FirstOrDefault(m => m.codename == houseTypeName && m.id == SYS_Code_Dict._户型);
                        houseTypeCode = obj == null ? -1 : obj.code;
                    }

                    datCaseTemp.HouseTypeCode = houseTypeCode;
                    projectcase.housetypecode = houseTypeCode;
                    dr["户型"] = houseTypeName;
                    if (!string.IsNullOrEmpty(houseTypeName) && houseTypeCode == -1)
                    {
                        isError = true;
                        dr["户型"] = houseTypeName + "#error";
                    }
                }

                if (dt.Columns.Contains("备注"))
                {
                    var remark = dt.Rows[i]["备注"].ToString().Trim();
                    datCaseTemp.Remark = remark;
                    projectcase.remark = remark;
                    dr["备注"] = remark;
                    if (remark.Length > 255)
                    {
                        isError = true;
                        dr["备注"] = remark + "#error";
                    }
                }

                if (dt.Columns.Contains("建筑年代"))
                {
                    var buildingdate = dt.Rows[i]["建筑年代"].ToString().Trim();
                    datCaseTemp.BuildingDate = buildingdate;
                    projectcase.buildingdate = buildingdate;
                    dr["建筑年代"] = buildingdate;
                    if (buildingdate.Length > 20)
                    {
                        isError = true;
                        dr["建筑年代"] = buildingdate + "#error";
                    }
                }

                if (dt.Columns.Contains("装修"))
                {
                    var zhuangxiu = dt.Rows[i]["装修"].ToString().Trim();
                    datCaseTemp.ZhuangXiu = zhuangxiu;
                    projectcase.zhuangxiu = zhuangxiu;
                    dr["装修"] = zhuangxiu;
                    if (zhuangxiu.Length > 20)
                    {
                        isError = true;
                        dr["装修"] = zhuangxiu + "#error";
                    }
                }

                if (dt.Columns.Contains("附属房屋"))
                {
                    var subhouse = dt.Rows[i]["附属房屋"].ToString().Trim();
                    datCaseTemp.SubHouse = subhouse;
                    projectcase.subhouse = subhouse;
                    dr["附属房屋"] = subhouse;
                    if (subhouse.Length > 50)
                    {
                        isError = true;
                        dr["附属房屋"] = subhouse + "#error";
                    }
                }

                if (dt.Columns.Contains("配套"))
                {
                    var peitao = dt.Rows[i]["配套"].ToString().Trim();
                    datCaseTemp.PeiTao = peitao;
                    projectcase.peitao = peitao;
                    dr["配套"] = peitao;
                    if (peitao.Length > 100)
                    {
                        isError = true;
                        dr["配套"] = peitao + "#error";
                    }
                }

                //if (dt.Columns.Contains("创建人"))
                //{
                //    var creator = dt.Rows[i]["创建人"].ToString().Trim();
                //    datCaseTemp.Creator = creator;
                //    projectcase.creator = creator;
                //    dr["创建人"] = creator;
                //    if (creator.Length > 50)
                //    {
                //        isError = true;
                //        dr["创建人"] = creator + "#error";
                //    }
                //}

                if (dt.Columns.Contains("来源"))
                {
                    var sourcename = dt.Rows[i]["来源"].ToString().Trim();
                    datCaseTemp.SourceName = sourcename;
                    projectcase.sourcename = sourcename;
                    dr["来源"] = sourcename;
                    if (sourcename.Length > 100)
                    {
                        isError = true;
                        dr["来源"] = sourcename + "#error";
                    }
                }

                if (dt.Columns.Contains("链接"))
                {
                    var sourcelink = dt.Rows[i]["链接"].ToString().Trim();
                    datCaseTemp.SourceLink = sourcelink;
                    projectcase.sourcelink = sourcelink;
                    dr["链接"] = sourcelink;
                    if (sourcelink.Length > 200)
                    {
                        isError = true;
                        dr["链接"] = sourcelink + "#error";
                    }

                }

                if (dt.Columns.Contains("电话"))
                {
                    var sourcephone = dt.Rows[i]["电话"].ToString().Trim();
                    datCaseTemp.SourcePhone = sourcephone;
                    projectcase.sourcephone = sourcephone;
                    dr["电话"] = sourcephone;
                    if (sourcephone.Length > 50)
                    {
                        isError = true;
                        dr["电话"] = sourcephone + "#error";
                    }
                }

                if (dt.Columns.Contains("使用面积"))
                {
                    var usablearea = dt.Rows[i]["使用面积"].ToString().Trim();
                    datCaseTemp.UsableArea = (decimal?)TryParseHelper.StrToDecimal(usablearea);
                    projectcase.usablearea = (decimal?)TryParseHelper.StrToDecimal(usablearea);
                    dr["使用面积"] = usablearea;
                    if (!string.IsNullOrEmpty(usablearea) && TryParseHelper.StrToDecimal(usablearea) == null)
                    {
                        isError = true;
                        dr["使用面积"] = usablearea + "#error";
                    }
                }

                if (dt.Columns.Contains("剩余年限"))
                {
                    var remainyear = dt.Rows[i]["剩余年限"].ToString().Trim();
                    datCaseTemp.RemainYear = (int?)TryParseHelper.StrToInt32(remainyear);
                    projectcase.remainyear = (int?)TryParseHelper.StrToInt32(remainyear);
                    dr["剩余年限"] = remainyear;
                    if (!string.IsNullOrEmpty(remainyear) && TryParseHelper.StrToInt32(remainyear) == null)
                    {
                        isError = true;
                        dr["剩余年限"] = remainyear + "#error";
                    }
                }

                if (dt.Columns.Contains("成新率"))
                {
                    var depreciation = dt.Rows[i]["成新率"].ToString().Trim();
                    datCaseTemp.Depreciation = (decimal?)TryParseHelper.StrToDecimal(depreciation);
                    projectcase.depreciation = (decimal?)TryParseHelper.StrToDecimal(depreciation);
                    dr["成新率"] = depreciation;
                    if (!string.IsNullOrEmpty(depreciation) && TryParseHelper.StrToDecimal(depreciation) == null)
                    {
                        isError = true;
                        dr["成新率"] = depreciation + "#error";
                    }
                }

                datCaseTemp.CityID = cityId;
                datCaseTemp.FXTCompanyId = fxtCompanyId;
                datCaseTemp.CreateDate = DateTime.Now;
                datCaseTemp.Creator = userid;
                datCaseTemp.SaveDateTime = DateTime.Now;

                projectcase.cityid = cityId;
                projectcase.fxtcompanyid = fxtCompanyId;
                projectcase.createdate = DateTime.Now;
                projectcase.creator = userid;
                projectcase.savedatetime = DateTime.Now;

                ////需把面积大于180，楼层小于等于4层的案例用途修改成别墅，别墅不参与价格计算
                ////BuildingArea > 180
                ////and TotalFloor > 0
                ////and TotalFloor <= 4
                ////and PurposeCode not in (1002005,1002006,1002007,1002008,1002027)
                //if (!isError)
                //{
                //    var purposecodelist = new List<int>();
                //    purposecodelist.Add(1002005);
                //    purposecodelist.Add(1002006);
                //    purposecodelist.Add(1002007);
                //    purposecodelist.Add(1002008);
                //    purposecodelist.Add(1002027);
                //    if (projectcase.buildingarea > 180 && projectcase.totalfloor > 0 && projectcase.totalfloor <= 4 && !purposecodelist.Contains(projectcase.purposecode))
                //    {
                //        isError = true;
                //        dr["*用途"] = purposeName + "面积>180且总楼层<=4，请确认是否为别墅用途#error";
                //    }
                //}

                if ((isMisMatch == false) && (isError == false)) //楼盘名称匹配上且数据格式正确
                {
                    correctData.Add(projectcase);
                }
                else if (isMisMatch && (isError == false))//楼盘名称不匹配，但其他数据格式都正确
                {
                    //如果楼盘名称不匹配，在待建楼盘库里查找，如果已有待建楼盘，则把待建楼盘的waitProjectid赋值。待建楼盘的行政区为空，所以只判断行政区为空的案例。
                    if (!(datCaseTemp.AreaId > 0))
                    {
                        var wp = GetWaitProjectMatchProjectId(datCaseTemp.ProjectName, cityId, fxtCompanyId);
                        if (wp != null)
                        {
                            datCaseTemp.ProjectId = wp.WaitProjectId;
                        }
                    }
                    projectNameMisMatch.Add(datCaseTemp);
                }
                else //格式错误的数据
                {
                    dataFormatError.Rows.Add(dr);
                }

                if (i > 0 && integer > 0)
                {
                    if (i % integer == 0)
                    {
                        _importTask.TaskStepsIncreased(taskId);
                    }
                }
            }
        }
    }
}
