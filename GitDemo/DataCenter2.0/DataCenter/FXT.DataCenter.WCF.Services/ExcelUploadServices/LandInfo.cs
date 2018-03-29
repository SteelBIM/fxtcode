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
        public void LandInfoExcelUpload(int cityid, int fxtcompanyid, string filePath, string userid,
            string taskName)
        {
            var taskId = 0;
            try
            {
                //在任务列表创建一条记录  iscomplete:0,代表否；1，代表是
                var task = new DAT_ImportTask()
                {
                    TaskName = taskName,
                    ImportType = SYS_Code_Dict.批量导入类型.土地信息,
                    CityID = cityid,
                    FXTCompanyId = fxtcompanyid,
                    CreateDate = DateTime.Now,
                    Creator = userid,
                    IsComplete = 0,
                    SucceedNumber = 0,
                    DataErrNumber = 0,
                    NameErrNumber = 0,
                    FilePath = "",
                    Steps=1
                };
                taskId = _importTask.AddTask(task);

                var excelHelper = new ExcelHandle(filePath);
                var data = excelHelper.ExcelToDataTable("Sheet1", true);

                var integer = Math.Floor(Convert.ToDouble(data.Rows.Count / 50));

                List<string> modifiedProperty;
                List<DAT_Land> correctData;//正确数据
                DataTable dataFormatError;//格式错误数据
                DataFilter(taskId,integer,cityid, fxtcompanyid, out correctData, out dataFormatError, out modifiedProperty, data);
                //错误数据写入excel
                string fileNamePath = string.Empty;
                if (dataFormatError.Rows.Count > 0)
                {
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "土地基础信息格式错误数据.xlsx";
                    var folder = MapPath("NeedHandledFiles/FailureData/" + fxtcompanyid);
                    fileNamePath = Path.Combine(folder, fileName);
                    excelHelper.CreateExcel(dataFormatError, fileNamePath, folder);

                }

                var failureNum = 0;
                var index4True = 0;//用于统计进度
                //正确数据添加到数据表
                correctData.ForEach(m =>
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

                    var isExist = _datLand.GetAllLandInfo(fxtcompanyid, m.cityid, Convert.ToInt32(m.areaid), m.landno);
                    if (isExist != null)//存在该土地名称则更新该土地信息
                    {

                        m.landid = isExist.landid;
                        m.saveuser = userid;
                        m.savedate = DateTime.Now;

                        var modifyResult = _datLand.UpdateDAT_Land(m, fxtcompanyid);
                        if (modifyResult <= 0) failureNum = failureNum + 1;
                    }
                    else//新增该土地信息
                    {
                        m.creator = userid;
                        m.createdate = DateTime.Now;

                        var insertResult = _datLand.AddDAT_Land(m);
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
                _importTask.UpdateTask(taskId, correctData.Count - failureNum, dataFormatError.Rows.Count, 0, relativePath, 1,100);
            }
            catch (Exception ex)
            {
                _importTask.UpdateTask(taskId, 0, 0, 0, "", -1);
                LogHelper.WriteLog("LandInfoExcelUpload", "", userid, cityid, fxtcompanyid, ex);
            }
        }


        private void DataFilter(int taskId, double integer, int cityId, int fxtCompanyId, out List<DAT_Land> listTrue, out DataTable dtError, out List<string> modifiedProperty, DataTable dt)
        {

            modifiedProperty = new List<string>();
            listTrue = new List<DAT_Land>();
            dtError = new DataTable();
            foreach (DataColumn column in dt.Columns)
                dtError.Columns.Add(column.Caption);


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var isSkip = false;
                var land = new DAT_Land();
                var dr = dtError.NewRow();
                land.cityid = cityId;
                land.fxtcompanyid = fxtCompanyId;


                var areaid = 0;
                if (dt.Columns.Contains("*行政区"))
                {
                    var areaName = dt.Rows[i]["*行政区"].ToString().Trim();
                    areaid = GetAreaId(cityId, areaName);
                    land.areaid = areaid;
                    dr["*行政区"] = areaName;
                    if (string.IsNullOrEmpty(areaName) || areaid == -1)//该行政区不存在
                    {
                        isSkip = true;
                        dr["*行政区"] = areaName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("AreaId=@AreaId,");
                }

                if (dt.Columns.Contains("片区"))
                {
                    var subareaName = dt.Rows[i]["片区"].ToString().Trim();
                    var subareaid = SubAreaIdByName(subareaName, areaid);
                    land.subareaid = subareaid;
                    dr["片区"] = subareaName;
                    if (!string.IsNullOrEmpty(subareaName) && subareaid <= 0)//该行政区不存在
                    {
                        isSkip = true;
                        dr["片区"] = subareaName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("SubAreaId=@SubAreaId,");
                }


                if (dt.Columns.Contains("*宗地号"))
                {
                    var landno = dt.Rows[i]["*宗地号"].ToString().Trim();
                    //bool flag = _datLand.ValidLandNo(cityId, fxtCompanyId, landno);
                    land.landno = landno;
                    dr["*宗地号"] = landno;
                    if (string.IsNullOrEmpty(landno))
                    {
                        isSkip = true;
                        dr["*宗地号"] = landno + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("landno = @landno,");
                }


                if (dt.Columns.Contains("*土地使用证号"))
                {
                    var fieldno = dt.Rows[i]["土地使用证号"].ToString().Trim();
                    land.fieldno = fieldno;
                    dr["土地使用证号"] = fieldno;
                    if (!string.IsNullOrEmpty(fieldno) && fieldno.Length > 100)
                    {
                        isSkip = true;
                        dr["土地使用证号"] = fieldno + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("fieldno = @fieldno,");
                }


                if (dt.Columns.Contains("图号"))
                {
                    var mapno = dt.Rows[i]["图号"].ToString().Trim();
                    land.mapno = mapno;
                    dr["图号"] = mapno;
                    if (mapno.Length > 100)
                    {
                        isSkip = true;
                        dr["图号"] = mapno + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("mapno = @mapno,");
                }


                if (dt.Columns.Contains("土地名称"))
                {
                    var landname = dt.Rows[i]["土地名称"].ToString().Trim();
                    land.landname = landname;
                    dr["土地名称"] = landname;
                    if (landname.Length > 100)
                    {
                        isSkip = true;
                        dr["土地名称"] = landname + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("landname = @landname,");
                }


                if (dt.Columns.Contains("土地位置"))
                {
                    var address = dt.Rows[i]["土地位置"].ToString().Trim();
                    land.address = address;
                    dr["土地位置"] = address;
                    if (address.Length > 100)
                    {
                        isSkip = true;
                        dr["土地位置"] = address + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("address = @address,");
                }

                if (dt.Columns.Contains("使用权类型"))
                {
                    var landtypeName = dt.Rows[i]["使用权类型"].ToString().Trim();
                    var landtypecode = Sys_TypeCodeOrName.GetLandTypeCode(landtypeName);
                    land.landtypecode = landtypecode;
                    dr["使用权类型"] = landtypeName;
                    if (!string.IsNullOrEmpty(landtypeName) && landtypecode == -1)
                    {
                        isSkip = true;
                        dr["使用权类型"] = landtypeName + "#error";

                    }

                    if (i == 0)
                        modifiedProperty.Add("landtypecode = @landtypecode,");
                }


                if (dt.Columns.Contains("使用权性质"))
                {
                    var usetypeName = dt.Rows[i]["使用权性质"].ToString().Trim();
                    var usetypecode = Sys_TypeCodeOrName.GetUseTypeCode(usetypeName);
                    land.usetypecode = usetypecode;
                    dr["使用权性质"] = usetypeName;
                    if (!string.IsNullOrEmpty(usetypeName) && usetypecode == -1)
                    {

                        isSkip = true;
                        dr["使用权性质"] = usetypeName + "#error";

                    }

                    if (i == 0)
                        modifiedProperty.Add("usetypecode = @usetypecode,");
                }


                if (dt.Columns.Contains("土地使用起始日期"))
                {
                    var startdate = dt.Rows[i]["土地使用起始日期"].ToString().Trim();
                    land.startdate = (DateTime?)TryParseHelper.StrToDateTime(startdate);
                    dr["土地使用起始日期"] = startdate;
                    if (!string.IsNullOrEmpty(startdate) && TryParseHelper.StrToDateTime(startdate) == null)
                    {
                        isSkip = true;
                        dr["土地使用起始日期"] = startdate + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("startdate = @startdate,");

                }



                if (dt.Columns.Contains("土地使用结束日期"))
                {
                    var enddate = dt.Rows[i]["土地使用结束日期"].ToString().Trim();
                    land.enddate = (DateTime?)TryParseHelper.StrToDateTime(enddate);
                    dr["土地使用结束日期"] = enddate;
                    if (!string.IsNullOrEmpty(enddate) && TryParseHelper.StrToDateTime(enddate) == null)
                    {
                        isSkip = true;
                        dr["土地使用结束日期"] = enddate + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("enddate = @enddate,");
                }


                if (dt.Columns.Contains("土地使用年限"))
                {
                    var useyear = dt.Rows[i]["土地使用年限"].ToString().Trim();
                    land.useyear = (int?)TryParseHelper.StrToInt32(useyear);
                    dr["土地使用年限"] = useyear;
                    if (!string.IsNullOrEmpty(useyear) && TryParseHelper.StrToInt32(useyear) == null)//数据格式错误
                    {
                        isSkip = true;
                        dr["土地使用年限"] = useyear + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("useyear = @useyear,");
                }

                if (dt.Columns.Contains("规划用途"))
                {
                    var planpurName = dt.Rows[i]["规划用途"].ToString().Trim();
                    //var planpurpose = Sys_TypeCodeOrName.GetPurposeCode(planpurName);
                    land.planpurpose = planpurName;
                    dr["规划用途"] = planpurName;
                    if (planpurName.Length > 100)
                    {
                        isSkip = true;
                        dr["规划用途"] = planpurName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("planpurpose = @planpurpose,");
                }

                if (dt.Columns.Contains("实际用途"))
                {
                    var factpurposeName = dt.Rows[i]["实际用途"].ToString().Trim();
                    var factpurpose = Sys_TypeCodeOrName.GetPurposeCode(factpurposeName);
                    land.factpurpose = factpurpose.ToString().Trim();
                    dr["实际用途"] = factpurposeName;
                    if (!string.IsNullOrEmpty(factpurposeName) && factpurpose == -1)
                    {
                        isSkip = true;
                        dr["实际用途"] = factpurposeName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("factpurpose = @factpurpose,");
                }

                if (dt.Columns.Contains("土地面积"))
                {
                    var landarea = dt.Rows[i]["土地面积"].ToString().Trim();
                    land.landarea = (decimal?)TryParseHelper.StrToDecimal(landarea);
                    dr["土地面积"] = landarea;
                    if (!string.IsNullOrEmpty(landarea) && TryParseHelper.StrToDecimal(landarea) == null)
                    {
                        isSkip = true;
                        dr["土地面积"] = landarea + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("landarea = @landarea,");
                }

                if (dt.Columns.Contains("规划总建筑面积"))
                {
                    var buildingarea = dt.Rows[i]["规划总建筑面积"].ToString().Trim();
                    land.buildingarea = (decimal?)TryParseHelper.StrToDecimal(buildingarea);
                    dr["规划总建筑面积"] = buildingarea;
                    if (!string.IsNullOrEmpty(buildingarea) && TryParseHelper.StrToDecimal(buildingarea) == null)
                    {

                        isSkip = true;
                        dr["规划总建筑面积"] = buildingarea + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add(" buildingarea = @buildingarea,");
                }


                if (dt.Columns.Contains("容积率"))
                {
                    var cubagerate = dt.Rows[i]["容积率"].ToString().Trim();
                    land.cubagerate = (decimal?)TryParseHelper.StrToDecimal(cubagerate);
                    dr["容积率"] = cubagerate;
                    if (!string.IsNullOrEmpty(cubagerate) && TryParseHelper.StrToDecimal(cubagerate) == null)
                    {

                        isSkip = true;
                        dr["容积率"] = cubagerate + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("cubagerate = @cubagerate,");
                }


                if (dt.Columns.Contains("最大容积率"))
                {
                    var maxcubagerate = dt.Rows[i]["最大容积率"].ToString().Trim();
                    land.maxcubagerate = (decimal?)TryParseHelper.StrToDecimal(maxcubagerate);
                    dr["最大容积率"] = maxcubagerate;
                    if (!string.IsNullOrEmpty(maxcubagerate) && TryParseHelper.StrToDecimal(maxcubagerate) == null)
                    {

                        isSkip = true;
                        dr["最大容积率"] = maxcubagerate + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("maxcubagerate = @maxcubagerate,");

                }

                if (dt.Columns.Contains("最小容积率"))
                {
                    var mincubagerate = dt.Rows[i]["最小容积率"].ToString().Trim();
                    land.mincubagerate = (decimal?)TryParseHelper.StrToDecimal(mincubagerate);
                    dr["最小容积率"] = mincubagerate;
                    if (!string.IsNullOrEmpty(mincubagerate) && TryParseHelper.StrToDecimal(mincubagerate) == null)
                    {
                        isSkip = true;
                        dr["最小容积率"] = mincubagerate + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("mincubagerate = @mincubagerate,");
                }


                if (dt.Columns.Contains("覆盖率"))
                {
                    var coverage = dt.Rows[i]["覆盖率"].ToString().Trim();
                    land.coverage = (decimal?)TryParseHelper.StrToDecimal(coverage);
                    dr["覆盖率"] = coverage;
                    if (!string.IsNullOrEmpty(coverage) && TryParseHelper.StrToDecimal(coverage) == null)
                    {
                        isSkip = true;
                        dr["覆盖率"] = coverage + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("coverage = @coverage,");
                }


                if (dt.Columns.Contains("最大覆盖率"))
                {
                    var maxcoverage = dt.Rows[i]["最大覆盖率"].ToString().Trim();
                    land.maxcoverage = (decimal?)TryParseHelper.StrToDecimal(maxcoverage);
                    dr["最大覆盖率"] = maxcoverage;
                    if (!string.IsNullOrEmpty(maxcoverage) && TryParseHelper.StrToDecimal(maxcoverage) == null)
                    {
                        isSkip = true;
                        dr["最大覆盖率"] = maxcoverage + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("maxcoverage = @maxcoverage,");
                }


                if (dt.Columns.Contains("绿化率"))
                {
                    var greenrage = dt.Rows[i]["绿化率"].ToString().Trim();
                    land.greenrage = (decimal?)TryParseHelper.StrToDecimal(greenrage);
                    dr["绿化率"] = greenrage;
                    if (!string.IsNullOrEmpty(greenrage) && TryParseHelper.StrToDecimal(greenrage) == null)
                    {
                        isSkip = true;
                        dr["绿化率"] = greenrage + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("greenrage = @greenrage,");
                }


                if (dt.Columns.Contains("最小绿化率"))
                {
                    var mingreenrage = dt.Rows[i]["最小绿化率"].ToString().Trim();
                    land.mingreenrage = (decimal?)TryParseHelper.StrToDecimal(mingreenrage);
                    dr["最小绿化率"] = mingreenrage;
                    if (!string.IsNullOrEmpty(mingreenrage) && TryParseHelper.StrToDecimal(mingreenrage) == null)
                    {
                        isSkip = true;
                        dr["最小绿化率"] = mingreenrage + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("mingreenrage = @mingreenrage,");
                }


                if (dt.Columns.Contains("土地形状"))
                {
                    var landshapeName = dt.Rows[i]["土地形状"].ToString().Trim();
                    var landshapecode = Sys_TypeCodeOrName.GetLandShapeCode(landshapeName);
                    land.landshapecode = landshapecode;
                    dr["土地形状"] = landshapeName;
                    if (!string.IsNullOrEmpty(landshapeName) && landshapecode == -1)
                    {
                        isSkip = true;
                        dr["土地形状"] = landshapeName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("landshapecode = @landshapecode,");
                }


                if (dt.Columns.Contains("开发程度"))
                {
                    var developmentName = dt.Rows[i]["开发程度"].ToString().Trim();
                    var developmentcode = Sys_TypeCodeOrName.GetLandShapeCode(developmentName);
                    land.developmentcode = developmentcode;
                    dr["开发程度"] = developmentName;
                    if (!string.IsNullOrEmpty(developmentName) && developmentcode == -1)
                    {
                        isSkip = true;
                        dr["开发程度"] = developmentName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("developmentcode = @developmentcode,");
                }


                if (dt.Columns.Contains("土地利用状况"))
                {
                    var landusestatus = dt.Rows[i]["土地利用状况"].ToString().Trim();
                    land.landusestatus = landusestatus;
                    dr["土地利用状况"] = landusestatus;
                    if (landusestatus.Length > 100)
                    {
                        isSkip = true;
                        dr["土地利用状况"] = landusestatus + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("landusestatus = @landusestatus,");
                }


                if (dt.Columns.Contains("土地等级"))
                {
                    var landclassName = dt.Rows[i]["土地等级"].ToString().Trim();
                    var landclass = Sys_TypeCodeOrName.GetLandClassCode(landclassName);
                    land.landclass = landclass;
                    dr["土地等级"] = landclassName;
                    if (!string.IsNullOrEmpty(landclassName) && landclass == -1)
                    {
                        isSkip = true;
                        dr["土地等级"] = landclassName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("landclass = @landclass,");
                }


                if (dt.Columns.Contains("建筑物限高"))
                {
                    var heightlimited = dt.Rows[i]["建筑物限高"].ToString().Trim();
                    land.heightlimited = (int?)TryParseHelper.StrToInt32(heightlimited);
                    dr["建筑物限高"] = heightlimited;
                    if (!string.IsNullOrEmpty(heightlimited) && TryParseHelper.StrToInt32(heightlimited) == null)//数据格式错误
                    {

                        isSkip = true;
                        dr["建筑物限高"] = heightlimited + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("heightlimited = @heightlimited,");
                }

                if (dt.Columns.Contains("规划限制"))
                {
                    var planlimited = dt.Rows[i]["规划限制"].ToString().Trim();
                    land.planlimited = planlimited;
                    dr["规划限制"] = planlimited;
                    if (planlimited.Length > 100)//数据格式错误
                    {
                        isSkip = true;
                        dr["规划限制"] = planlimited + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("planlimited = @planlimited,");
                }


                if (dt.Columns.Contains("四至 东"))
                {
                    var east = dt.Rows[i]["四至 东"].ToString().Trim();
                    land.east = east;
                    dr["四至 东"] = east;
                    if (east.Length > 100)//数据格式错误
                    {
                        isSkip = true;
                        dr["四至 东"] = east + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("east = @east,");
                }


                if (dt.Columns.Contains("四至 西"))
                {
                    var west = dt.Rows[i]["四至 西"].ToString().Trim();
                    land.west = west;
                    dr["四至 西"] = west;
                    if (west.Length > 100)//数据格式错误
                    {
                        isSkip = true;
                        dr["四至 西"] = west + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("west = @west,");
                }


                if (dt.Columns.Contains("四至 南"))
                {
                    var south = dt.Rows[i]["四至 南"].ToString().Trim();
                    land.south = south;
                    dr["四至 南"] = south;
                    if (south.Length > 100)//数据格式错误
                    {
                        isSkip = true;
                        dr["四至 南"] = south + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("south = @south,");
                }


                if (dt.Columns.Contains("四至 北"))
                {
                    var north = dt.Rows[i]["四至 北"].ToString().Trim();
                    land.north = north;
                    dr["四至 北"] = north;
                    if (north.Length > 100)//数据格式错误
                    {
                        isSkip = true;
                        dr["四至 北"] = north + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add(" north = @north,");
                }


                if (dt.Columns.Contains("土地所有者"))
                {
                    var landownerName = dt.Rows[i]["土地所有者"].ToString().Trim();
                    land.landownerid = 0;
                    dr["土地所有者"] = landownerName;
                    if (!string.IsNullOrEmpty(landownerName))
                    {
                        var companyInfo = _datCompany.GetDAT_CompanyInfo(landownerName);
                        land.landownerid = companyInfo == null ? _datCompany.AddDAT_Compandy(landownerName, 1003002, cityId.ToString().Trim(), fxtCompanyId) : companyInfo.CompanyId;
                    }

                    if (i == 0)
                        modifiedProperty.Add("landownerid = @landownerid,");
                }


                if (dt.Columns.Contains("土地使用者"))
                {
                    var landownerNameTo = dt.Rows[i]["土地使用者"].ToString().Trim();
                    land.landuseid = 0;
                    dr["土地使用者"] = landownerNameTo;
                    if (!string.IsNullOrEmpty(landownerNameTo))
                    {
                        var companyInfo = _datCompany.GetDAT_CompanyInfo(landownerNameTo);
                        land.landuseid = companyInfo == null ? _datCompany.AddDAT_Compandy(landownerNameTo, 1003002, cityId.ToString().Trim(), fxtCompanyId) : _datCompany.GetDAT_CompanyInfo(landownerNameTo).CompanyId;
                    }

                    if (i == 0)
                        modifiedProperty.Add("landuseid = @landuseid,");
                }


                if (dt.Columns.Contains("距离商服中心距离"))
                {
                    var businesscenterdistance = dt.Rows[i]["距离商服中心距离"].ToString().Trim();
                    land.businesscenterdistance = (decimal?)TryParseHelper.StrToDecimal(businesscenterdistance);
                    dr["距离商服中心距离"] = businesscenterdistance;
                    if (!string.IsNullOrEmpty(businesscenterdistance) && TryParseHelper.StrToDecimal(businesscenterdistance) == null)
                    {
                        isSkip = true;
                        dr["距离商服中心距离"] = businesscenterdistance + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("businesscenterdistance = @businesscenterdistance,");
                }


                if (dt.Columns.Contains("交通条件"))
                {
                    var traffic = dt.Rows[i]["交通条件"].ToString().Trim();
                    land.traffic = traffic;
                    dr["交通条件"] = traffic;
                    if (traffic.Length > 100)
                    {
                        isSkip = true;
                        dr["交通条件"] = traffic + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("traffic = @traffic,");
                }


                if (dt.Columns.Contains("基础设施状况"))
                {
                    var infrastructure = dt.Rows[i]["基础设施状况"].ToString().Trim();
                    land.infrastructure = infrastructure;
                    dr["基础设施状况"] = infrastructure;
                    if (infrastructure.Length > 100)
                    {
                        isSkip = true;
                        dr["基础设施状况"] = infrastructure + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("infrastructure = @infrastructure,");
                }

                if (dt.Columns.Contains("公用设施状况"))
                {
                    var publicservice = dt.Rows[i]["公用设施状况"].ToString().Trim();
                    land.publicservice = publicservice;
                    dr["公用设施状况"] = publicservice;
                    if (publicservice.Length > 100)
                    {
                        isSkip = true;
                        dr["公用设施状况"] = publicservice + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("publicservice = @publicservice,");
                }


                if (dt.Columns.Contains("环境质量"))
                {
                    var environmentName = dt.Rows[i]["环境质量"].ToString().Trim();
                    var environmentcode = Sys_TypeCodeOrName.GetEnvironmentCode(environmentName);
                    land.environmentcode = environmentcode;
                    dr["环境质量"] = environmentName;
                    if (!string.IsNullOrEmpty(environmentName) && environmentcode == -1)
                    {
                        isSkip = true;
                        dr["环境质量"] = environmentName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("environmentcode = @environmentcode,");
                }

                if (dt.Columns.Contains("许可证时间"))
                {
                    var licencedate = dt.Rows[i]["许可证时间"].ToString().Trim();
                    land.licencedate = (DateTime?)TryParseHelper.StrToDateTime(licencedate);
                    dr["许可证时间"] = licencedate;
                    if (!string.IsNullOrEmpty(licencedate) && TryParseHelper.StrToDateTime(licencedate) == null)
                    {
                        isSkip = true;
                        dr["许可证时间"] = licencedate + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("licencedate = @licencedate,");
                }

                if (dt.Columns.Contains("宗地自身条件"))
                {
                    var landdetail = dt.Rows[i]["宗地自身条件"].ToString().Trim();
                    land.landdetail = landdetail;
                    dr["宗地自身条件"] = landdetail;
                    if (landdetail.Length > 100)
                    {
                        isSkip = true;
                        dr["宗地自身条件"] = landdetail + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("landdetail = @landdetail,");
                }

                if (dt.Columns.Contains("地价指数"))
                {
                    var weight = dt.Rows[i]["地价指数"].ToString().Trim();
                    land.weight = (decimal?)TryParseHelper.StrToDecimal(weight);
                    dr["地价指数"] = weight;
                    if (!string.IsNullOrEmpty(weight) && TryParseHelper.StrToDecimal(weight) == null)
                    {
                        isSkip = true;
                        dr["地价指数"] = weight + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("weight = @weight,");
                }


                if (dt.Columns.Contains("综合修正系数"))
                {
                    var coefficient = dt.Rows[i]["综合修正系数"].ToString().Trim();
                    land.coefficient = (decimal?)TryParseHelper.StrToDecimal(coefficient);
                    dr["综合修正系数"] = coefficient;
                    if (!string.IsNullOrEmpty(coefficient) && TryParseHelper.StrToDecimal(coefficient) == null)
                    {
                        isSkip = true;
                        dr["综合修正系数"] = coefficient + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("coefficient = @coefficient,");
                }


                if (dt.Columns.Contains("经度"))
                {
                    var x = dt.Rows[i]["经度"].ToString().Trim();
                    land.x = (decimal?)TryParseHelper.StrToDecimal(x);
                    dr["经度"] = x;
                    if (!string.IsNullOrEmpty(x) && TryParseHelper.StrToDecimal(x) == null)
                    {
                        isSkip = true;
                        dr["经度"] = x + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("x = @x,");
                }


                if (dt.Columns.Contains("纬度"))
                {
                    var y = dt.Rows[i]["纬度"].ToString().Trim();
                    land.y = (decimal?)TryParseHelper.StrToDecimal(y);
                    dr["纬度"] = y;
                    if (!string.IsNullOrEmpty(y) && TryParseHelper.StrToDecimal(y) == null)
                    {
                        isSkip = true;
                        dr["纬度"] = y + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add(" y = @y,");
                }


                if (dt.Columns.Contains("经纬度规模"))
                {
                    var xyscale = dt.Rows[i]["经纬度规模"].ToString().Trim();
                    land.xyscale = (int?)TryParseHelper.StrToInt32(xyscale);
                    dr["经纬度规模"] = xyscale;
                    if (!string.IsNullOrEmpty(xyscale) && TryParseHelper.StrToInt32(xyscale) == null)
                    {
                        isSkip = true;
                        dr["经纬度规模"] = xyscale + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("xyscale = @xyscale,");
                }

                if (dt.Columns.Contains("备注"))
                {
                    var remark = dt.Rows[i]["备注"].ToString().Trim();
                    land.remark = remark;
                    dr["备注"] = remark;
                    if (remark.Length > 500)
                    {
                        isSkip = true;
                        dr["备注"] = remark + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add(" remark = @remark,");
                }

                if (i == 0)
                {
                    modifiedProperty.Add("savedate = getdate(),");
                    modifiedProperty.Add("saveuser = @saveuser,");
                }

                // modifiedProperty.Add("valid = @valid ");

                if (isSkip)//该行数据中，至少有一列数据格式错误
                {
                    dtError.Rows.Add(dr);
                }
                else
                {
                    listTrue.Add(land);
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
