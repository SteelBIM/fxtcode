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
        public void LandCaseExcelUpload(int cityid, int fxtcompanyid, string filePath, string userid, string taskName)
        {
            var taskId = 0;

            try
            {
                //在任务列表创建一条记录
                var task = new DAT_ImportTask()
                {
                    TaskName = taskName,
                    ImportType = SYS_Code_Dict.批量导入类型.土地案例,
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

                List<DAT_CaseLand> listTrue;//正确数据
                DataTable dtError;//格式错误数据
                List<string> modifiedProperty;
                DataFilter(taskId,integer,cityid, fxtcompanyid, out listTrue, out dtError, out modifiedProperty, data);

                //错误数据写入Excel
                var fileNamePath = string.Empty;
                if (dtError.Rows.Count > 0)
                {
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "土地案例格式错误数据.xlsx";
                    var folder = MapPath("NeedHandledFiles/FailureData/" + fxtcompanyid.ToString().Trim());
                    fileNamePath = Path.Combine(folder, fileName);
                    excelHelper.CreateExcel(dtError, fileNamePath, folder);

                }

                var failureNum = 0;
                var index4True = 0;//用于统计进度
                //正确数据添加到landcase表中
                //如果宗地号不在土地基础数据库中则要把相关信息添加到土地基础数据库
                foreach (var m in listTrue)
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

                    if (LandNos(cityid, fxtcompanyid).Count(p => p.Contains(m.landno)) == 0)
                    {
                        var datLand = new DAT_Land
                        {
                            fxtcompanyid = fxtcompanyid,
                            cityid = cityid,
                            areaid = m.areaid,
                            subareaid = m.subareaid,
                            landno = m.landno,
                            address = m.landaddress,
                            planpurpose = m.landpurposedesc,
                            landarea = m.landarea,
                            createdate = DateTime.Now,
                            creator = userid
                        };

                        _datLand.AddDAT_Land(datLand);
                    }
                    m.creator = userid;
                    var insertResult = _landCase.AddLandCase(m);
                    if (insertResult <= 0) failureNum = failureNum + 1;
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
                _importTask.UpdateTask(taskId, listTrue.Count - failureNum, dtError.Rows.Count, 0, relativePath, 1,100);
            }
            catch (Exception ex)
            {
                _importTask.UpdateTask(taskId, 0, 0, 0, "", -1);
                LogHelper.WriteLog("LandCaseExcelUpload", "", userid, cityid, fxtcompanyid, ex);
            }

        }

        private IEnumerable<string> LandNos(int cityid, int fxtcompanyid)
        {
            return _datLand.GetLandNo(cityid, fxtcompanyid);
        }


        private void DataFilter(int taskId, double integer, int cityId, int fxtCompanyId, out List<DAT_CaseLand> listTrue, out DataTable dtError, out List<string> modifiedProperty, DataTable dt)
        {
            modifiedProperty = new List<string>();

            listTrue = new List<DAT_CaseLand>();
            dtError = new DataTable();

            foreach (DataColumn column in dt.Columns)
                dtError.Columns.Add(column.Caption);


            for (var i = 0; i < dt.Rows.Count; i++)
            {

                var isError = false;
                var caseland = new DAT_CaseLand();
                var dr = dtError.NewRow();

                caseland.cityid = cityId;
                caseland.fxtcompanyid = fxtCompanyId;

                if (dt.Columns.Contains("*案例日期"))
                {
                    var caseDate = dt.Rows[i]["*案例日期"].ToString().Trim();
                    var date = TryParseHelper.StrToDateTime(caseDate);
                    caseland.casedate = date == null ? DateTime.Now : (DateTime)date;
                    dr["*案例日期"] = caseDate;
                    if (string.IsNullOrEmpty(caseDate) || date == null)
                    {
                        isError = true;
                        dr["*案例日期"] = caseDate + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("casedate");
                }

                if (dt.Columns.Contains("*宗地号"))
                {
                    var landNo = dt.Rows[i]["*宗地号"].ToString().Trim();
                    caseland.landno = landNo;
                    dr["*宗地号"] = landNo;
                    if (string.IsNullOrEmpty(landNo))
                    {
                        isError = true;
                        dr["*宗地号"] = landNo + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("landno");
                }

                var areaId = 0;
                if (dt.Columns.Contains("*行政区"))
                {
                    var areaName = dt.Rows[i]["*行政区"].ToString().Trim();
                    areaId = GetAreaId(cityId, areaName);
                    caseland.areaid = areaId;
                    caseland.areaname = areaName;
                    dr["*行政区"] = areaName;
                    if (!string.IsNullOrEmpty(areaName) && areaId == -1)
                    {
                        isError = true;
                        dr["*行政区"] = areaName + "#error";
                    }
                    if (i == 0)
                    {
                        modifiedProperty.Add("areaid");
                        modifiedProperty.Add("areaname");
                    }
                }

                if (dt.Columns.Contains("片区"))
                {
                    var subAreaName = dt.Rows[i]["片区"].ToString().Trim();
                    var subAreaId = SubAreaIdByName(subAreaName, areaId);
                    caseland.subareaid = subAreaId;
                    dr["片区"] = subAreaName;
                    if (!string.IsNullOrEmpty(subAreaName) && subAreaId <= 0)
                    {
                        isError = true;
                        dr["片区"] = subAreaName + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("subareaid");
                }

                if (dt.Columns.Contains("买卖方式"))
                {
                    var barginTypeName = dt.Rows[i]["买卖方式"].ToString().Trim();
                    var barginTypeCode = GetCodeByName(barginTypeName, SYS_Code_Dict._买卖方式);
                    caseland.bargaintypecode = barginTypeCode;
                    dr["买卖方式"] = barginTypeName;
                    if (!string.IsNullOrEmpty(barginTypeName) && barginTypeCode == -1)
                    {
                        isError = true;
                        dr["买卖方式"] = barginTypeName + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("bargaintypecode");
                }

                if (dt.Columns.Contains("建面地价_元每平方米"))
                {
                    var buildUnitPrice = dt.Rows[i]["建面地价_元每平方米"].ToString().Trim();
                    caseland.buildunitprice = (decimal?)TryParseHelper.StrToDecimal(buildUnitPrice);
                    dr["建面地价_元每平方米"] = buildUnitPrice;
                    if (!string.IsNullOrEmpty(buildUnitPrice) && TryParseHelper.StrToDecimal(buildUnitPrice) == null)
                    {
                        isError = true;
                        dr["建面地价_元每平方米"] = buildUnitPrice + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("buildunitprice");
                }

                if (dt.Columns.Contains("土地单价_元每平方米"))
                {
                    var unitPrice = dt.Rows[i]["土地单价_元每平方米"].ToString().Trim();
                    caseland.landunitprice = (decimal?)TryParseHelper.StrToDecimal(unitPrice);
                    dr["土地单价_元每平方米"] = unitPrice;
                    if (!string.IsNullOrEmpty(unitPrice) && TryParseHelper.StrToDecimal(unitPrice) == null)
                    {
                        isError = true;
                        dr["土地单价_元每平方米"] = unitPrice + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("landunitprice");
                }

                if (dt.Columns.Contains("土地面积_平方米"))
                {
                    var landArea = dt.Rows[i]["土地面积_平方米"].ToString().Trim();
                    caseland.landarea = (decimal?)TryParseHelper.StrToDecimal(landArea);
                    dr["土地面积_平方米"] = landArea;
                    if (!string.IsNullOrEmpty(landArea) && TryParseHelper.StrToDecimal(landArea) == null)
                    {
                        isError = true;
                        dr["土地面积_平方米"] = landArea + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("landarea");
                }

                if (dt.Columns.Contains("起价_万元"))
                {
                    var minBargainPrice = dt.Rows[i]["起价_万元"].ToString().Trim();
                    caseland.minbargainprice = (decimal?)TryParseHelper.StrToDecimal(minBargainPrice);
                    dr["起价_万元"] = minBargainPrice;
                    if (!string.IsNullOrEmpty(minBargainPrice) && TryParseHelper.StrToDecimal(minBargainPrice) == null)
                    {
                        isError = true;
                        dr["起价_万元"] = minBargainPrice + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("minbargainprice");
                }

                if (dt.Columns.Contains("建筑面积_平方米"))
                {
                    var buildArea = dt.Rows[i]["建筑面积_平方米"].ToString().Trim();
                    caseland.buildingarea = (decimal?)TryParseHelper.StrToDecimal(buildArea);
                    dr["建筑面积_平方米"] = buildArea;
                    if (!string.IsNullOrEmpty(buildArea) && TryParseHelper.StrToDecimal(buildArea) == null)
                    {
                        isError = true;
                        dr["建筑面积_平方米"] = buildArea + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("buildingarea");
                }

                if (dt.Columns.Contains("起始日期"))
                {
                    var startDate = dt.Rows[i]["起始日期"].ToString().Trim();
                    caseland.startusabledate = (DateTime?)TryParseHelper.StrToDateTime(startDate);
                    dr["起始日期"] = startDate;
                    if (!string.IsNullOrEmpty(startDate) && TryParseHelper.StrToDateTime(startDate) == null)
                    {
                        isError = true;
                        dr["起始日期"] = startDate + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("startusabledate");
                }

                if (dt.Columns.Contains("土地使用结束日期"))
                {
                    var enddate = dt.Rows[i]["土地使用结束日期"].ToString().Trim();
                    caseland.enddate = (DateTime?)TryParseHelper.StrToDateTime(enddate);
                    dr["土地使用结束日期"] = enddate;
                    if (!string.IsNullOrEmpty(enddate) && TryParseHelper.StrToDateTime(enddate) == null)
                    {
                        isError = true;
                        dr["土地使用结束日期"] = enddate + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("enddate");
                }

                if (dt.Columns.Contains("土地用途"))
                {
                    var purposeName = dt.Rows[i]["土地用途"].ToString().Trim();
                    var purposeCode = GetCodeByName(purposeName, SYS_Code_Dict._土地用途);
                    caseland.landpurposecode = purposeCode;
                    dr["土地用途"] = purposeName;
                    if (!string.IsNullOrEmpty(purposeName) && purposeCode == -1)
                    {
                        isError = true;
                        dr["土地用途"] = purposeName + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("landpurposecode");
                }

                if (dt.Columns.Contains("规划用途"))
                {
                    var planpurName = dt.Rows[i]["规划用途"].ToString().Trim();
                    caseland.landpurposedesc = planpurName;
                    dr["规划用途"] = planpurName;
                    if (planpurName.Length > 100)
                    {
                        isError = true;
                        dr["规划用途"] = planpurName + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("LandPurposeDesc = @LandPurposeDesc,");
                }

                if (dt.Columns.Contains("竞得者"))
                {
                    var winner = dt.Rows[i]["竞得者"].ToString().Trim();
                    caseland.winner = winner;
                    dr["竞得者"] = winner;
                    if (winner.Length > 100)
                    {
                        isError = true;
                        dr["竞得者"] = winner + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("winner");
                }

                if (dt.Columns.Contains("竞得时间"))
                {
                    var windate = dt.Rows[i]["竞得时间"].ToString().Trim();
                    caseland.windate = (DateTime?)TryParseHelper.StrToDateTime(windate);
                    dr["竞得时间"] = windate;
                    if (!string.IsNullOrEmpty(windate) && TryParseHelper.StrToDateTime(windate) == null)
                    {
                        isError = true;
                        dr["竞得时间"] = windate + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("windate");
                }

                if (dt.Columns.Contains("卖方(出让方)"))
                {
                    var seller = dt.Rows[i]["卖方(出让方)"].ToString().Trim();
                    caseland.seller = seller;
                    dr["卖方(出让方)"] = seller;
                    if (seller.Length > 200)
                    {
                        isError = true;
                        dr["卖方(出让方)"] = seller + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("seller");
                }

                if (dt.Columns.Contains("成交价_万元"))
                {
                    var dealTotalPrice = dt.Rows[i]["成交价_万元"].ToString().Trim();
                    caseland.dealtotalprice = (decimal?)TryParseHelper.StrToDecimal(dealTotalPrice);
                    dr["成交价_万元"] = dealTotalPrice;
                    if (!string.IsNullOrEmpty(dealTotalPrice) && TryParseHelper.StrToDecimal(dealTotalPrice) == null)
                    {
                        isError = true;
                        dr["成交价_万元"] = dealTotalPrice + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("dealtotalprice");
                }

                if (dt.Columns.Contains("成交日期"))
                {
                    var dealDate = dt.Rows[i]["成交日期"].ToString().Trim();
                    caseland.dealdate = (DateTime?)TryParseHelper.StrToDateTime(dealDate);
                    dr["成交日期"] = dealDate;
                    if (!string.IsNullOrEmpty(dealDate) && TryParseHelper.StrToDateTime(dealDate) == null)
                    {
                        isError = true;
                        dr["成交日期"] = dealDate + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("dealdate");
                }

                if (dt.Columns.Contains("成交状态"))
                {
                    var statusName = dt.Rows[i]["成交状态"].ToString().Trim();
                    var statusCode = GetCodeByName(statusName, SYS_Code_Dict._成交状态);
                    caseland.bargainstatecode = statusCode;
                    dr["成交状态"] = statusName;
                    if (!string.IsNullOrEmpty(statusName) && statusCode == -1)
                    {
                        isError = true;
                        dr["成交状态"] = statusName + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("bargainstatecode");
                }

                if (dt.Columns.Contains("土地开发程度"))
                {
                    var developdegreecodename = dt.Rows[i]["土地开发程度"].ToString().Trim();
                    var developdegreecode = GetCodeByName(developdegreecodename, SYS_Code_Dict._土地开发情况);
                    caseland.developdegreecode = developdegreecode;
                    dr["土地开发程度"] = developdegreecodename;
                    if (!string.IsNullOrEmpty(developdegreecodename) && developdegreecode == -1)
                    {
                        isError = true;
                        dr["土地开发程度"] = developdegreecodename + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("developdegreecode");
                }

                if (dt.Columns.Contains("地址"))
                {
                    var landaddress = dt.Rows[i]["地址"].ToString().Trim();
                    caseland.landaddress = landaddress;
                    dr["地址"] = landaddress;
                    if (landaddress.Length > 255)
                    {
                        isError = true;
                        dr["地址"] = landaddress + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("landaddress");
                }

                if (dt.Columns.Contains("交易机构"))
                {
                    var bargainedby = dt.Rows[i]["交易机构"].ToString().Trim();
                    caseland.bargainedby = bargainedby;
                    dr["交易机构"] = bargainedby;
                    if (bargainedby.Length > 100)
                    {
                        isError = true;
                        dr["交易机构"] = bargainedby + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("bargainedby");
                }

                if (dt.Columns.Contains("备注"))
                {
                    var remark = dt.Rows[i]["备注"].ToString().Trim();
                    caseland.remark = remark;
                    dr["备注"] = remark;
                    if (remark.Length > 512)
                    {
                        isError = true;
                        dr["备注"] = remark + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("remark");
                }

                if (dt.Columns.Contains("挂牌日期"))
                {
                    var bargaindate = dt.Rows[i]["挂牌日期"].ToString().Trim();
                    caseland.bargaindate = (DateTime?)TryParseHelper.StrToDateTime(bargaindate);
                    dr["挂牌日期"] = bargaindate;
                    if (!string.IsNullOrEmpty(bargaindate) && TryParseHelper.StrToDateTime(bargaindate) == null)
                    {
                        isError = true;
                        dr["挂牌日期"] = bargaindate + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("bargaindate");
                }

                if (dt.Columns.Contains("土地使用年限"))
                {
                    var useyear = dt.Rows[i]["土地使用年限"].ToString().Trim();
                    caseland.usableyear = (int?)TryParseHelper.StrToInt32(useyear);
                    dr["土地使用年限"] = useyear;
                    if (!string.IsNullOrEmpty(useyear) && TryParseHelper.StrToInt32(useyear) == null)
                    {
                        isError = true;
                        dr["土地使用年限"] = useyear + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("usableyear");
                }

                if (dt.Columns.Contains("容积率"))
                {
                    var cubagerate = dt.Rows[i]["容积率"].ToString().Trim();
                    caseland.cubagerate = (decimal?)TryParseHelper.StrToDecimal(cubagerate);
                    dr["容积率"] = cubagerate;
                    if (!string.IsNullOrEmpty(cubagerate) && TryParseHelper.StrToDecimal(cubagerate) == null)
                    {
                        isError = true;
                        dr["容积率"] = cubagerate + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("cubagerate");
                }

                if (dt.Columns.Contains("最小容积率"))
                {
                    var mincubagerate = dt.Rows[i]["最小容积率"].ToString().Trim();
                    caseland.mincubagerate = (decimal?)TryParseHelper.StrToDecimal(mincubagerate);
                    dr["最小容积率"] = mincubagerate;
                    if (!string.IsNullOrEmpty(mincubagerate) && TryParseHelper.StrToDecimal(mincubagerate) == null)
                    {
                        isError = true;
                        dr["最小容积率"] = mincubagerate + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("mincubagerate");
                }

                if (dt.Columns.Contains("最大容积率"))
                {
                    var maxcubagerate = dt.Rows[i]["最大容积率"].ToString().Trim();
                    caseland.maxcubagerate = (decimal?)TryParseHelper.StrToDecimal(maxcubagerate);
                    dr["最大容积率"] = maxcubagerate;
                    if (!string.IsNullOrEmpty(maxcubagerate) && TryParseHelper.StrToDecimal(maxcubagerate) == null)
                    {
                        isError = true;
                        dr["最大容积率"] = maxcubagerate + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("maxcubagerate");
                }

                if (dt.Columns.Contains("覆盖率"))
                {
                    var coverrate = dt.Rows[i]["覆盖率"].ToString().Trim();
                    caseland.coverrate = (decimal?)TryParseHelper.StrToDecimal(coverrate);
                    dr["覆盖率"] = coverrate;
                    if (!string.IsNullOrEmpty(coverrate) && TryParseHelper.StrToDecimal(coverrate) == null)
                    {
                        isError = true;
                        dr["覆盖率"] = coverrate + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("coverrate");
                }

                if (dt.Columns.Contains("最大覆盖率"))
                {
                    var maxcoverrate = dt.Rows[i]["最大覆盖率"].ToString().Trim();
                    caseland.maxcoverrate = (decimal?)TryParseHelper.StrToDecimal(maxcoverrate);
                    dr["最大覆盖率"] = maxcoverrate;
                    if (!string.IsNullOrEmpty(maxcoverrate) && TryParseHelper.StrToDecimal(maxcoverrate) == null)
                    {
                        isError = true;
                        dr["最大覆盖率"] = maxcoverrate + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("maxcoverrate");
                }

                if (dt.Columns.Contains("联系电话"))
                {
                    var sourcephone = dt.Rows[i]["联系电话"].ToString().Trim();
                    caseland.sourcephone = sourcephone;
                    dr["联系电话"] = sourcephone;
                    if (sourcephone.Length > 50)
                    {
                        isError = true;
                        dr["联系电话"] = sourcephone + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("sourcephone");
                }

                if (dt.Columns.Contains("土地来源"))
                {
                    var landsourcename = dt.Rows[i]["土地来源"].ToString().Trim();
                    var landsourcecode = GetCodeByName(landsourcename, SYS_Code_Dict._土地来源);
                    caseland.landsourcecode = landsourcecode;
                    dr["土地来源"] = landsourcename;
                    if (!string.IsNullOrEmpty(landsourcename) && landsourcecode < 0)
                    {
                        isError = true;
                        dr["土地来源"] = landsourcename + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("landsourcecode");
                }

                if (dt.Columns.Contains("使用权性质"))
                {
                    var usetypecodename = dt.Rows[i]["使用权性质"].ToString().Trim();
                    var usetypecode = GetCodeByName(usetypecodename, SYS_Code_Dict._土地所有权);
                    caseland.useTypeCode = usetypecode;
                    dr["使用权性质"] = usetypecodename;
                    if (!string.IsNullOrEmpty(usetypecodename) && usetypecode < 0)
                    {
                        isError = true;
                        dr["使用权性质"] = usetypecodename + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("usetypecode");
                }

                if (dt.Columns.Contains("高度限制"))
                {
                    var heightlimited = dt.Rows[i]["高度限制"].ToString().Trim();
                    caseland.heightlimited = (int?)TryParseHelper.StrToInt32(heightlimited);
                    dr["高度限制"] = heightlimited;
                    if (!string.IsNullOrEmpty(heightlimited) && TryParseHelper.StrToInt32(heightlimited) == null)
                    {
                        isError = true;
                        dr["高度限制"] = heightlimited + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("heightlimited");
                }

                if (dt.Columns.Contains("规划限制"))
                {
                    var planlimited = dt.Rows[i]["规划限制"].ToString().Trim();
                    caseland.planlimited = planlimited;
                    dr["规划限制"] = planlimited;
                    if (planlimited.Length > 500)
                    {
                        isError = true;
                        dr["规划限制"] = planlimited + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("planlimited");
                }

                if (dt.Columns.Contains("土地利用状况"))
                {
                    var landusestatus = dt.Rows[i]["土地利用状况"].ToString().Trim();
                    caseland.landusestatus = landusestatus;
                    dr["土地利用状况"] = landusestatus;
                    if (landusestatus.Length > 400)
                    {
                        isError = true;
                        dr["土地利用状况"] = landusestatus + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("landusestatus");
                }

                if (dt.Columns.Contains("土地等级"))
                {
                    var landclassname = dt.Rows[i]["土地等级"].ToString().Trim();
                    var landclass = GetCodeByName(landclassname, SYS_Code_Dict._土地等级);
                    caseland.landclass = landclass;
                    dr["土地等级"] = landclassname;
                    if (!string.IsNullOrEmpty(landclassname) && landclass < 0)
                    {
                        isError = true;
                        dr["土地等级"] = landclassname + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("landclass");
                }

                if (dt.Columns.Contains("绿化率"))
                {
                    var greenrage = dt.Rows[i]["绿化率"].ToString().Trim();
                    caseland.greenrage = (decimal?)TryParseHelper.StrToDecimal(greenrage);
                    dr["绿化率"] = greenrage;
                    if (!string.IsNullOrEmpty(greenrage) && TryParseHelper.StrToDecimal(greenrage) == null)
                    {
                        isError = true;
                        dr["绿化率"] = greenrage + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("greenrage");
                }

                if (dt.Columns.Contains("最小绿化率"))
                {
                    var mingreenrage = dt.Rows[i]["最小绿化率"].ToString().Trim();
                    caseland.mingreenrage = (decimal?)TryParseHelper.StrToDecimal(mingreenrage);
                    dr["最小绿化率"] = mingreenrage;
                    if (!string.IsNullOrEmpty(mingreenrage) && TryParseHelper.StrToDecimal(mingreenrage) == null)
                    {
                        isError = true;
                        dr["最小绿化率"] = mingreenrage + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("mingreenrage");
                }

                if (dt.Columns.Contains("约定开工日期"))
                {
                    var arrangestartdate = dt.Rows[i]["约定开工日期"].ToString().Trim();
                    caseland.arrangestartdate = (DateTime?)TryParseHelper.StrToDateTime(arrangestartdate);
                    dr["约定开工日期"] = arrangestartdate;
                    if (!string.IsNullOrEmpty(arrangestartdate) && TryParseHelper.StrToDateTime(arrangestartdate) == null)
                    {
                        isError = true;
                        dr["约定开工日期"] = arrangestartdate + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("arrangestartdate");
                }

                if (dt.Columns.Contains("约定竣工日期"))
                {
                    var arrangeenddate = dt.Rows[i]["约定竣工日期"].ToString().Trim();
                    caseland.arrangeenddate = (DateTime?)TryParseHelper.StrToDateTime(arrangeenddate);
                    dr["约定竣工日期"] = arrangeenddate;
                    if (!string.IsNullOrEmpty(arrangeenddate) && TryParseHelper.StrToDateTime(arrangeenddate) == null)
                    {
                        isError = true;
                        dr["约定竣工日期"] = arrangeenddate + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("arrangeenddate");
                }

                if (dt.Columns.Contains("来源名称"))
                {
                    var sourceName = dt.Rows[i]["来源名称"].ToString().Trim();
                    caseland.sourcename = sourceName;
                    dr["来源名称"] = sourceName;
                    if (sourceName.Length > 100)
                    {
                        isError = true;
                        dr["来源名称"] = sourceName + "#error";
                    }
                    if (i == 0)
                        modifiedProperty.Add("sourcename");
                }


                if (dt.Columns.Contains("来源链接"))
                {
                    var sourceLink = dt.Rows[i]["来源链接"].ToString().Trim();
                    caseland.sourcelink = sourceLink;
                    dr["来源链接"] = sourceLink;
                    if (sourceLink.Length > 200)
                    {
                        isError = true;
                        dr["来源链接"] = sourceLink + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("sourcelink");
                }

                if (isError)//该行数据中，至少有一列数据格式错误
                {
                    dtError.Rows.Add(dr);
                }
                else
                {
                    listTrue.Add(caseland);
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
