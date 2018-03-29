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


        public void BusinessCaseExcelUpload(int cityid, int fxtcompanyid, string filePath, string userid, string taskName)
        {
            var taskId = 0;

            try
            {
                //在任务列表创建一条记录
                var task = new DAT_ImportTask()
                {
                    TaskName = taskName,
                    ImportType = SYS_Code_Dict.批量导入类型.商业案例,
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

                List<Dat_Case_Biz> listTrue;//正确数据
                DataTable dtError;//格式错误数据
                DataFilter(userid, cityid, fxtcompanyid, out listTrue, out dtError, data);

                //错误数据写入Excel
                var fileNamePath = string.Empty;
                if (dtError.Rows.Count > 0)
                {
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "商业案例格式错误数据.xlsx";
                    var folder = MapPath("NeedHandledFiles/FailureData/" + fxtcompanyid);
                    fileNamePath = Path.Combine(folder, fileName);
                    excelHelper.CreateExcel(dtError, fileNamePath, folder);

                }

                var failureNum = 0;

                //正确数据写入表中
                //listTrue.ForEach(m => _businessCase.AddCaseBiz(m));
                foreach (var item in listTrue)
                {
                    item.CreateTime = DateTime.Now;
                    item.Creator = userid;
                    var insertResult = _businessCase.AddCaseBiz(item);
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
                _importTask.UpdateTask(taskId, listTrue.Count - failureNum, dtError.Rows.Count, 0, relativePath, 1);
            }
            catch (Exception ex)
            {
                _importTask.UpdateTask(taskId, 0, 0, 0, "", -1);
                LogHelper.WriteLog("BusinessCaseExcelUploadTask", "", userid, cityid, fxtcompanyid, ex);
            }

        }
        private void DataFilter(string userId, int cityId, int fxtCompanyId, out List<Dat_Case_Biz> listTrue, out DataTable dtError, DataTable dt)
        {

            listTrue = new List<Dat_Case_Biz>();
            dtError = new DataTable();

            foreach (DataColumn column in dt.Columns)
            {
                dtError.Columns.Add(column.Caption);
            }

            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var isError = false;
                var caseBiz = new Dat_Case_Biz();
                var dr = dtError.NewRow();

                var areaName = dt.Rows[i]["*行政区"].ToString().Trim();
                var areaId = GetAreaId(cityId, areaName);
                dr["*行政区"] = areaName;
                caseBiz.AreaId = areaId;
                if (string.IsNullOrEmpty(areaName) || (!string.IsNullOrEmpty(areaName) && areaId < 0))
                {
                    isError = true;
                    dr["*行政区"] = areaName + "#error";
                }

                var subAreaName = dt.Rows[i]["商圈"].ToString().Trim();
                var subAreaId = GetSubAreaId(subAreaName, areaId);
                dr["商圈"] = subAreaName;
                caseBiz.SubAreaId = subAreaId;
                if ((!string.IsNullOrEmpty(subAreaName) && subAreaId < 0))
                {
                    isError = true;
                    dr["商圈"] = subAreaName + "#error";
                }

                var projectName = dt.Rows[i]["*商业街"].ToString().Trim();
                var projectIds = GetProjectId(cityId, areaId, fxtCompanyId, projectName);
                dr["*商业街"] = projectName;
                caseBiz.ProjectId = projectIds.FirstOrDefault();
                caseBiz.ProjectName = projectName;
                if (string.IsNullOrEmpty(projectName))
                {
                    isError = true;
                    dr["*商业街"] = projectName + "#error";
                }

                var buildingName = dt.Rows[i]["商业楼栋"].ToString().Trim();
                var buildingId = 0;
                dr["商业楼栋"] = buildingName;
                caseBiz.BuildingId = buildingId;
                caseBiz.BuildingName = buildingName.Length > 50 ? buildingName.Substring(0, 50) : buildingName;

                var houseName = dt.Rows[i]["商业房号"].ToString().Trim();
                var houseId = 0;
                dr["商业房号"] = houseName;
                caseBiz.HouseId = houseId;
                caseBiz.HouseName = houseName;

                var address = dt.Rows[i]["项目地址"].ToString().Trim();
                dr["项目地址"] = address;
                caseBiz.Address = address;
                if (!string.IsNullOrEmpty(address) && address.Length > 512)
                {
                    isError = true;
                    dr["项目地址"] = address + "#error";
                }

                bool buildingAreaFlag = false;
                var buildingAreaStr = dt.Rows[i]["*建筑面积"].ToString().Trim();
                var buildingArea = TryParseHelper.StrToDecimal(buildingAreaStr, -1);
                dr["*建筑面积"] = buildingAreaStr;
                caseBiz.BuildingArea = buildingArea;
                if (string.IsNullOrEmpty(buildingAreaStr) || buildingArea <= 0)
                {
                    isError = true;
                    dr["*建筑面积"] = buildingAreaStr + "#error";
                }
                else
                {
                    buildingAreaFlag = true;
                }

                bool unitPriceFlag = false;
                var unitPriceStr = dt.Rows[i]["*单价"].ToString().Trim();
                var unitPrice = TryParseHelper.StrToDecimal(unitPriceStr, -1);
                dr["*单价"] = unitPriceStr;
                caseBiz.UnitPrice = unitPrice;
                if (string.IsNullOrEmpty(unitPriceStr) || unitPrice <= 0)
                {
                    isError = true;
                    dr["*单价"] = unitPriceStr + "#error";
                }
                else
                {
                    unitPriceFlag = true;
                }

                var totalPriceStr = dt.Rows[i]["*总价"].ToString().Trim();
                var totalPrice = TryParseHelper.StrToDecimal(totalPriceStr, -1);
                dr["*总价"] = totalPriceStr;
                caseBiz.TotalPrice = totalPrice;
                if (string.IsNullOrEmpty(totalPriceStr) || totalPrice == -1)
                {
                    isError = true;
                    dr["*总价"] = totalPriceStr + "#error";
                }
                else if (buildingAreaFlag && unitPriceFlag && Math.Abs(buildingArea * unitPrice - totalPrice) / totalPrice > (decimal)0.01)
                {
                    isError = true;
                    dr["*总价"] = "总价有误差" + totalPriceStr + "#error";
                }

                var casedate = dt.Rows[i]["*案例时间"].ToString().Trim();
                caseBiz.CaseDate = TryParseHelper.StrToDateTime(casedate) == null ? (DateTime)SqlDateTime.MinValue : (DateTime)TryParseHelper.StrToDateTime(casedate);
                dr["*案例时间"] = casedate;
                if (string.IsNullOrEmpty(casedate) || TryParseHelper.StrToDateTime(casedate) == null)
                {
                    isError = true;
                    dr["*案例时间"] = casedate + "#error";
                }

                var caseTypeName = dt.Rows[i]["*案例类型"].ToString().Trim();
                var caseTypeCode = GetCodeByName(caseTypeName, SYS_Code_Dict._案例类型);
                caseBiz.CaseTypeCode = caseTypeCode;
                dr["*案例类型"] = caseTypeName;
                if (string.IsNullOrEmpty(caseTypeName) || (!string.IsNullOrEmpty(caseTypeName) && caseTypeCode == -1))
                {
                    isError = true;
                    dr["*案例类型"] = caseTypeName + "#error";
                }

                var rentTypeName = dt.Rows[i]["租金方式"].ToString().Trim();
                var rentTypeCode = GetCodeByName(rentTypeName, SYS_Code_Dict._租金方式);
                caseBiz.RentTypeCode = rentTypeCode;
                dr["租金方式"] = rentTypeName;
                if (!string.IsNullOrEmpty(rentTypeName) && rentTypeCode == -1)
                {
                    isError = true;
                    dr["租金方式"] = rentTypeName + "#error";
                }

                var rentRateStr = dt.Rows[i]["租金增长率_百分比/年"].ToString().Trim();
                var rentRate = (decimal?)TryParseHelper.StrToDecimal(rentRateStr);
                dr["租金增长率_百分比/年"] = rentRateStr;
                caseBiz.RentRate = rentRate;
                if (!string.IsNullOrEmpty(rentRateStr) && rentRate == -1)
                {
                    isError = true;
                    dr["租金增长率_百分比/年"] = rentRateStr + "#error";
                }

                var houseTypeName = dt.Rows[i]["商铺类型"].ToString().Trim();
                var houseType = GetCaseHouseType(houseTypeName);
                caseBiz.HouseType = houseType;
                dr["商铺类型"] = houseTypeName;
                if (!string.IsNullOrEmpty(houseTypeName) && houseType == -1)
                {
                    isError = true;
                    dr["商铺类型"] = houseTypeName + "#error";
                }

                var bizCodeName = dt.Rows[i]["经营业态"].ToString().Trim();
                var bizCode = string.Empty;
                dr["经营业态"] = bizCodeName;
                var flag = true;
                var bizCodeNameList = bizCodeName.Contains("，") ? bizCodeName.Split('，') : (bizCodeName.Contains(",") ? bizCodeName.Split(',') : null);
                if (bizCodeNameList != null)
                {
                    foreach (var bizCodeNameStr in bizCodeNameList)
                    {
                        var value = GetCaseBizCode(bizCodeNameStr);
                        bizCode += value + ",";
                        if (!string.IsNullOrEmpty(bizCodeNameStr) && value == -1)
                        {
                            flag = false;
                        }
                    }
                    caseBiz.BizCode = bizCode.TrimEnd(',');
                    if (!flag)
                    {
                        isError = true;
                        dr["经营业态"] = bizCodeName + "#error";
                    }
                }
                else
                {
                    var value = GetCaseBizCode(bizCodeName);
                    caseBiz.BizCode = value.ToString();
                    if (!string.IsNullOrEmpty(bizCodeName) && value < 1)
                    {
                        isError = true;
                        dr["经营业态"] = bizCodeName + "#error";
                    }
                }

                var floorNo = dt.Rows[i]["所在楼层"].ToString().Trim();
                dr["所在楼层"] = floorNo;
                caseBiz.FloorNo = floorNo;
                if (!string.IsNullOrEmpty(floorNo) && floorNo.Length > 128)
                {
                    isError = true;
                    dr["所在楼层"] = floorNo + "#error";
                }

                var totalFloorStr = dt.Rows[i]["总楼层"].ToString().Trim();
                var totalFloor = (int?)TryParseHelper.StrToInt32(totalFloorStr);
                dr["总楼层"] = totalFloorStr;
                caseBiz.TotalFloor = totalFloor;
                if (!string.IsNullOrEmpty(totalFloorStr) && totalFloor == -1)
                {
                    isError = true;
                    dr["总楼层"] = totalFloorStr + "#error";
                }

                var fitmentName = dt.Rows[i]["装修情况"].ToString().Trim();
                fitmentName = fitmentName == "精装修" ? "精装" : (fitmentName == "简装修" ? "简装" : fitmentName);
                var fitment = GetCodeByName(fitmentName, SYS_Code_Dict._装修情况);
                caseBiz.Fitment = fitment;
                dr["装修情况"] = fitmentName;
                if (!string.IsNullOrEmpty(fitmentName) && fitment == -1)
                {
                    isError = true;
                    dr["装修情况"] = fitmentName + "#error";
                }

                var managerPriceStr = dt.Rows[i]["物业费_元/平方米*月"].ToString().Trim();
                var managerPrice = (decimal?)TryParseHelper.StrToDecimal(managerPriceStr);
                dr["物业费_元/平方米*月"] = managerPriceStr;
                caseBiz.ManagerPrice = managerPrice;
                if (!string.IsNullOrEmpty(managerPriceStr) && managerPrice == -1)
                {
                    isError = true;
                    dr["物业费_元/平方米*月"] = managerPriceStr + "#error";
                }

                var AgencyCompany = dt.Rows[i]["中介公司"].ToString().Trim();
                dr["中介公司"] = AgencyCompany;
                caseBiz.AgencyCompany = AgencyCompany;
                if (!string.IsNullOrEmpty(AgencyCompany) && AgencyCompany.Length > 128)
                {
                    isError = true;
                    dr["中介公司"] = AgencyCompany + "#error";
                }

                var Agent = dt.Rows[i]["中介人员"].ToString().Trim();
                dr["中介人员"] = Agent;
                caseBiz.Agent = Agent;
                if (!string.IsNullOrEmpty(Agent) && Agent.Length > 128)
                {
                    isError = true;
                    dr["中介人员"] = Agent + "#error";
                }

                var AgencyTel = dt.Rows[i]["中介电话"].ToString().Trim();
                dr["中介电话"] = AgencyTel;
                caseBiz.AgencyTel = AgencyTel;
                if (!string.IsNullOrEmpty(AgencyTel) && AgencyTel.Length > 100)
                {
                    isError = true;
                    dr["中介电话"] = AgencyTel + "#error";
                }

                var sourceName = dt.Rows[i]["来源名称"].ToString().Trim();
                dr["来源名称"] = sourceName;
                caseBiz.SourceName = sourceName;
                if (!string.IsNullOrEmpty(sourceName) && sourceName.Length > 100)
                {
                    isError = true;
                    dr["来源名称"] = sourceName + "#error";
                }

                var sourceLink = dt.Rows[i]["来源链接"].ToString().Trim();
                dr["来源链接"] = sourceLink;
                caseBiz.SourceLink = sourceLink;
                if (!string.IsNullOrEmpty(sourceLink) && sourceLink.Length > 200)
                {
                    isError = true;
                    dr["来源链接"] = sourceLink + "#error";
                }

                var sourcePhone = dt.Rows[i]["来源电话"].ToString().Trim();
                dr["来源电话"] = sourcePhone;
                caseBiz.SourcePhone = sourcePhone;
                if (!string.IsNullOrEmpty(sourcePhone) && sourcePhone.Length > 50)
                {
                    isError = true;
                    dr["来源电话"] = sourcePhone + "#error";
                }

                caseBiz.CityId = cityId;
                caseBiz.FxtCompanyId = fxtCompanyId;
                caseBiz.Creator = userId;
                caseBiz.SaveUser = userId;

                if (isError)//该行数据中，至少有一列数据格式错误
                {
                    dtError.Rows.Add(dr);
                }
                else
                {
                    listTrue.Add(caseBiz);
                }
            }
        }

        private int GetCaseHouseType(string HouseTypeName)
        {
            int HouseType = -1;
            switch (HouseTypeName)
            {
                case "住宅底商": HouseType = 1119001; break;
                case "商业街商铺": HouseType = 1110001; break;
                case "临街门面": HouseType = 1107002; break;
                case "写字楼配套": HouseType = 1119002; break;
                case "购物中心/百货": HouseType = 1118004; break;
                case "宾馆酒店": HouseType = 1118006; break;
                case "旅游点商铺": HouseType = 1124001; break;
                case "主题卖场": HouseType = 1118002; break;
                case "其他": HouseType = 1118011; break;
                default: HouseType = -1; break;
            }
            return HouseType;
        }

        private int GetCaseBizCode(string BizCodeName)
        {
            int BizCode = -1;
            switch (BizCodeName)
            {
                case "餐饮": BizCode = 6018012; break;
                case "服饰鞋帽箱包": BizCode = 1120004; break;
                case "休闲娱乐": BizCode = 6018014; break;
                case "美容美发": BizCode = 1133001; break;
                case "服务业": BizCode = 6018019; break;
                case "百货店": BizCode = 6018006; break;
                case "酒店宾馆": BizCode = 1133027; break;
                case "家居建材": BizCode = 1120003; break;
                case "酒吧": BizCode = 1133034; break;
                case "汽车美容": BizCode = 1133002; break;
                case "其他": BizCode = 1120014; break;
                default: BizCode = -1; break;
            }
            return BizCode;
        }
    }
}
