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
        public void HumanHouseExcelUpload(int cityid, int fxtcompanyid, string filePath, string userid, string taskName)
        {
            var taskId = 0;
            try
            {
                //在任务列表创建一条记录  iscomplete:0,代表否；1，代表是
                var task = new DAT_ImportTask
                {
                    TaskName = taskName,
                    ImportType = SYS_Code_Dict.批量导入类型.业主房号信息,
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
                List<DAT_HumanHouse> listTrue;//正确数据
                DataTable dtError;//格式错误数据
                DataFilter(taskId, integer, cityid, fxtcompanyid, data, out listTrue, out dtError, out modifiedProperty);

                //错误数据写入Excel
                var fileNamePath = string.Empty;
                if (dtError.Rows.Count > 0)
                {
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "业主房号错误数据.xlsx";
                    var folder = MapPath("NeedHandledFiles/FailureData/" + fxtcompanyid);
                    fileNamePath = Path.Combine(folder, fileName);
                    excelHelper.CreateExcel(dtError, fileNamePath, folder);
                }

                var failureNum = 0;
                var index4True = 0;//用于统计进度
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

                    m.Creator = userid;
                    m.IsGroup = 0;
                    m.Valid = 1;

                    var insertResult = _humanHouse.AddHumanHouse(m);
                    if (insertResult <= 0) failureNum = failureNum + 1;
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
                LogHelper.WriteLog("HumanHouseExcelUpload", "", userid, cityid, fxtcompanyid, ex);
            }
        }

        private void DataFilter(int taskId, double integer, int cityId, int fxtCompanyId, DataTable dt, out List<DAT_HumanHouse> listTrue, out DataTable dtError, out List<string> modifiedProperty)
        {
            modifiedProperty = new List<string>();
            listTrue = new List<DAT_HumanHouse>();
            dtError = new DataTable();

            foreach (DataColumn column in dt.Columns)
                dtError.Columns.Add(column.Caption);

            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var dh = new DAT_HumanHouse();
                var dr = dtError.NewRow();
                dh.FxtcompanyId = fxtCompanyId;
                dh.CityId = cityId;
                dh.CreateTime = DateTime.Now;
                var isError = false;

                var areaId = 0;
                var areaName = dt.Rows[i]["*行政区"].ToString().Trim();
                areaId = GetAreaId(cityId, areaName);
                dh.AreaId = areaId;
                dr["*行政区"] = areaName;
                if (string.IsNullOrEmpty(areaName) || areaId == -1)
                {
                    isError = true;
                    dr["*行政区"] = areaName + "#error";
                }

                var projectId = 0;
                var projectName = dt.Rows[i]["*楼盘名称"].ToString().Trim();
                var obj = ProjectIdByName(fxtCompanyId, cityId, areaId, projectName).FirstOrDefault();
                projectId = obj == null ? 0 : obj.projectid;
                dh.ProjectId = projectId;
                dr["*楼盘名称"] = projectName;
                if (string.IsNullOrEmpty(projectName) || projectId <= 0)
                {
                    isError = true;
                    dr["*楼盘名称"] = projectName + "#error";
                }

                var buildingId = 0;
                var buildingName = dt.Rows[i]["*楼栋名称"].ToString().Trim();
                buildingId = BuildingIdByName(projectId, buildingName, cityId, fxtCompanyId);
                dh.BuildingId = buildingId;
                dr["*楼栋名称"] = buildingName;
                if (string.IsNullOrEmpty(buildingName) || buildingId <= 0)
                {
                    isError = true;
                    dr["*楼栋名称"] = buildingName + "#error";
                }

                var houseName = dt.Rows[i]["*房号名称"].ToString().Trim();
                var houseId = HouseIdByName(buildingId, houseName, cityId, fxtCompanyId);
                dh.HouseId = houseId;
                dr["*房号名称"] = houseName;
                if (string.IsNullOrEmpty(houseName) || houseId <= 0)
                {
                    isError = true;
                    dr["*房号名称"] = houseName + "#error";
                }

                var name = dt.Rows[i]["*产权人"].ToString().Trim();
                dh.Name = name;
                dr["*产权人"] = name;
                if (string.IsNullOrEmpty(name) || (!string.IsNullOrEmpty(name) && name.Length > 50))
                {
                    isError = true;
                    dr["*产权人"] = name + "#error";
                }

                if (dt.Columns.Contains("性别"))
                {
                    var sexname = dt.Rows[i]["性别"].ToString().Trim();
                    var sex = GetCodeByName(sexname, SYS_Code_Dict._性别);
                    dh.Sex = sex;
                    dr["性别"] = sexname;
                    if (!string.IsNullOrEmpty(sexname) && sex == -1)
                    {
                        isError = true;
                        dr["性别"] = sexname + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[Sex]=@Sex,");
                }

                if (dt.Columns.Contains("年龄"))
                {
                    var age = dt.Rows[i]["年龄"].ToString().Trim();
                    dh.Age = (int?)TryParseHelper.StrToInt32(age);
                    dr["年龄"] = age;
                    if (!string.IsNullOrEmpty(age) && TryParseHelper.StrToInt32(age) == null)
                    {
                        isError = true;
                        dr["年龄"] = age + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[Age]=@Age,");
                }

                if (dt.Columns.Contains("年龄段"))
                {
                    var ageGroupName = dt.Rows[i]["年龄段"].ToString().Trim();
                    var ageGroupCode = GetCodeByName(ageGroupName, SYS_Code_Dict._年龄段);
                    dh.AgeGroup = ageGroupCode;
                    dr["年龄段"] = ageGroupName;
                    if (!string.IsNullOrEmpty(ageGroupName) && ageGroupCode == -1)
                    {
                        isError = true;
                        dr["年龄段"] = ageGroupName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[AgeGroup]=@AgeGroup,");
                }

                if (dt.Columns.Contains("籍贯"))
                {
                    var origin = dt.Rows[i]["籍贯"].ToString().Trim();
                    dh.Origin = origin;
                    dr["籍贯"] = origin;
                    if (!string.IsNullOrEmpty(origin) && origin.Length > 50)
                    {
                        isError = true;
                        dr["籍贯"] = origin + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[Origin]=@Origin,");
                }

                if (dt.Columns.Contains("出生年月"))
                {
                    var birthday = dt.Rows[i]["出生年月"].ToString().Trim();
                    dh.Birthday = (DateTime?)TryParseHelper.StrToDateTime(birthday);
                    dr["出生年月"] = birthday;
                    if (!string.IsNullOrEmpty(birthday) && TryParseHelper.StrToDateTime(birthday) == null)
                    {
                        isError = true;
                        dr["出生年月"] = birthday + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("Birthday=@Birthday,");
                }

                if (dt.Columns.Contains("身份证"))
                {
                    var card = dt.Rows[i]["身份证"].ToString().Trim();
                    dh.IDCard = card;
                    dr["身份证"] = card;
                    if (!string.IsNullOrEmpty(card) && card.Length > 25)
                    {
                        isError = true;
                        dr["身份证"] = card + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[IDCard]=@IDCard,");
                }

                if (dt.Columns.Contains("婚姻状态"))
                {
                    var marriageName = dt.Rows[i]["婚姻状态"].ToString().Trim();
                    var marriage = GetCodeByName(marriageName, SYS_Code_Dict._婚姻状态);
                    dh.Marriage = marriage;
                    dr["婚姻状态"] = marriageName;
                    if (!string.IsNullOrEmpty(marriageName) && marriage == -1)
                    {
                        isError = true;
                        dr["婚姻状态"] = marriageName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[Marriage]=@Marriage,");
                }

                if (dt.Columns.Contains("手机号码"))
                {
                    var telephone = dt.Rows[i]["手机号码"].ToString().Trim();
                    dh.Telephone = telephone;
                    dr["手机号码"] = telephone;
                    if (!string.IsNullOrEmpty(telephone) && telephone.Length > 20)
                    {
                        isError = true;
                        dr["手机号码"] = telephone + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[Telephone]=@Telephone,");
                }

                if (dt.Columns.Contains("学历"))
                {
                    var educationName = dt.Rows[i]["学历"].ToString().Trim();
                    var education = GetCodeByName(educationName, SYS_Code_Dict._学历);
                    dh.Education = education;
                    dr["学历"] = educationName;
                    if (!string.IsNullOrEmpty(educationName) && education == -1)
                    {
                        isError = true;
                        dr["学历"] = educationName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[Education]=@Education,");
                }

                if (dt.Columns.Contains("行业"))
                {
                    var occupationName = dt.Rows[i]["行业"].ToString().Trim();
                    var occupation = GetCodeByName(occupationName, SYS_Code_Dict._行业大类);
                    dh.Occupation = occupation;
                    dr["行业"] = occupationName;
                    if (!string.IsNullOrEmpty(occupationName) && occupation == -1)
                    {
                        isError = true;
                        dr["行业"] = occupationName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[Occupation]=@Occupation,");
                }

                if (dt.Columns.Contains("职位"))
                {
                    var positionName = dt.Rows[i]["职位"].ToString().Trim();
                    var position = GetCodeByName(positionName, SYS_Code_Dict._职位);
                    dh.Position = position;
                    dr["职位"] = positionName;
                    if (!string.IsNullOrEmpty(positionName) && position == -1)
                    {
                        isError = true;
                        dr["职位"] = positionName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[Position]=@Position,");
                }

                if (dt.Columns.Contains("年薪范围"))
                {
                    var salaryName = dt.Rows[i]["年薪范围"].ToString().Trim();
                    var salary = GetCodeByName(salaryName, SYS_Code_Dict._年薪资范围);
                    dh.Salary = salary;
                    dr["年薪范围"] = salaryName;
                    if (!string.IsNullOrEmpty(salaryName) && salary == -1)
                    {
                        isError = true;
                        dr["年薪范围"] = salaryName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[Salary]=@Salary,");
                }

                if (dt.Columns.Contains("常用交通工具"))
                {
                    var transportationName = dt.Rows[i]["常用交通工具"].ToString().Trim();
                    var transportation = GetCodeByName(transportationName, SYS_Code_Dict._常用交通工具);
                    dh.Transportation = transportation;
                    dr["常用交通工具"] = transportationName;
                    if (!string.IsNullOrEmpty(transportationName) && transportation == -1)
                    {
                        isError = true;
                        dr["常用交通工具"] = transportationName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[Transportation]=@Transportation,");
                }

                if (dt.Columns.Contains("所在公司"))
                {
                    var company = dt.Rows[i]["所在公司"].ToString().Trim();
                    dh.Company = company;
                    dr["所在公司"] = company;
                    if (!string.IsNullOrEmpty(company) && company.Length > 50)
                    {
                        isError = true;
                        dr["所在公司"] = company + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[Company]=@Company,");
                }

                if (dt.Columns.Contains("家庭成员总数"))
                {
                    var familyNum = dt.Rows[i]["家庭成员总数"].ToString().Trim();
                    dh.FamilyNum = (int?)TryParseHelper.StrToInt32(familyNum);
                    dr["家庭成员总数"] = familyNum;
                    if (!string.IsNullOrEmpty(familyNum) && (int?)TryParseHelper.StrToInt32(familyNum) == null)
                    {
                        isError = true;
                        dr["家庭成员总数"] = familyNum + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[FamilyNum]=@FamilyNum,");
                }

                if (dt.Columns.Contains("购房数"))
                {
                    var houses = dt.Rows[i]["购房数"].ToString().Trim();
                    dh.Houses = (int?)TryParseHelper.StrToInt32(houses);
                    dr["购房数"] = houses;
                    if (!string.IsNullOrEmpty(houses) && (int?)TryParseHelper.StrToInt32(houses) == null)
                    {
                        isError = true;
                        dr["购房数"] = houses + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[Houses]=@Houses,");
                }

                if (dt.Columns.Contains("房产证号"))
                {
                    var propertyNumber = dt.Rows[i]["房产证号"].ToString().Trim();
                    dh.PropertyNumber = propertyNumber;
                    dr["房产证号"] = propertyNumber;
                    if (!string.IsNullOrEmpty(propertyNumber) && propertyNumber.Length > 50)
                    {
                        isError = true;
                        dr["房产证号"] = propertyNumber + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[PropertyNumber]=@PropertyNumber,");
                }

                if (dt.Columns.Contains("产权人占有份额_百分比"))
                {
                    var ownerProportionstr = dt.Rows[i]["产权人占有份额_百分比"].ToString().Trim();
                    var ownerProportion = (decimal?)TryParseHelper.StrToDecimal(ownerProportionstr);
                    dh.OwnerProportion = ownerProportion;
                    dr["产权人占有份额_百分比"] = ownerProportionstr;
                    if (!string.IsNullOrEmpty(ownerProportionstr) && ownerProportion == null)
                    {
                        isError = true;
                        dr["产权人占有份额_百分比"] = ownerProportionstr + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[OwnerProportion]=@OwnerProportion,");
                }

                if (dt.Columns.Contains("共有人"))
                {
                    var share = dt.Rows[i]["共有人"].ToString().Trim();
                    dh.Share = share;
                    dr["共有人"] = share;
                    if (!string.IsNullOrEmpty(share) && share.Length > 50)
                    {
                        isError = true;
                        dr["共有人"] = share + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[Share]=@Share,");
                }

                if (dt.Columns.Contains("是否抵押"))
                {
                    var isMortgageStr = dt.Rows[i]["是否抵押"].ToString().Trim();
                    var isMortgage = YesOrNo(isMortgageStr);
                    dh.IsMortgage = isMortgage;
                    dr["是否抵押"] = isMortgageStr;
                    if (!string.IsNullOrEmpty(isMortgageStr) && isMortgage == null)
                    {
                        isError = true;
                        dr["是否抵押"] = isMortgageStr + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[IsMortgage]=@IsMortgage,");
                }

                if (dt.Columns.Contains("是否查封"))
                {
                    var isSealUpStr = dt.Rows[i]["是否查封"].ToString().Trim();
                    var isSealUp = YesOrNo(isSealUpStr);
                    dh.IsSealUp = isSealUp;
                    dr["是否查封"] = isSealUpStr;
                    if (!string.IsNullOrEmpty(isSealUpStr) && isSealUp == null)
                    {
                        isError = true;
                        dr["是否查封"] = isSealUpStr + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[IsSealUp]=@IsSealUp,");
                }

                if (dt.Columns.Contains("是否自住"))
                {
                    var isSelfStr = dt.Rows[i]["是否自住"].ToString().Trim();
                    var isSelf = YesOrNo(isSelfStr);
                    dh.IsSelf = isSelf;
                    dr["是否自住"] = isSelfStr;
                    if (!string.IsNullOrEmpty(isSelfStr) && isSelf == null)
                    {
                        isError = true;
                        dr["是否自住"] = isSelfStr + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[IsSelf]=@IsSelf,");
                }

                if (dt.Columns.Contains("是否贷款"))
                {
                    var isLoanedStr = dt.Rows[i]["是否贷款"].ToString().Trim();
                    var isLoaned = YesOrNo(isLoanedStr);
                    dh.IsLoaned = isLoaned;
                    dr["是否贷款"] = isLoanedStr;
                    if (!string.IsNullOrEmpty(isLoanedStr) && isLoaned == null)
                    {
                        isError = true;
                        dr["是否贷款"] = isLoanedStr + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[IsLoaned]=@IsLoaned,");
                }

                if (dt.Columns.Contains("是否有车位"))
                {
                    var isParkingStr = dt.Rows[i]["是否有车位"].ToString().Trim();
                    var isParking = YesOrNo(isParkingStr);
                    dh.IsParking = isParking;
                    dr["是否有车位"] = isParkingStr;
                    if (!string.IsNullOrEmpty(isParkingStr) && isParking == null)
                    {
                        isError = true;
                        dr["是否有车位"] = isParkingStr + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[IsParking]=@IsParking,");
                }

                if (dt.Columns.Contains("产权形式"))
                {
                    var rightCodeName = dt.Rows[i]["产权形式"].ToString().Trim();
                    var rightCode = GetCodeByName(rightCodeName, SYS_Code_Dict._产权形式);
                    dh.RightCode = rightCode;
                    dr["产权形式"] = rightCodeName;
                    if (!string.IsNullOrEmpty(rightCodeName) && rightCode == null)
                    {
                        isError = true;
                        dr["产权形式"] = rightCodeName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[RightCode]=@RightCode,");
                }

                if (dt.Columns.Contains("建筑面积_平方米"))
                {
                    var buildingAreaStr = dt.Rows[i]["建筑面积_平方米"].ToString().Trim();
                    var buildingArea = (decimal?)TryParseHelper.StrToDecimal(buildingAreaStr);
                    dh.BuildingArea = buildingArea;
                    dr["建筑面积_平方米"] = buildingAreaStr;
                    if (!string.IsNullOrEmpty(buildingAreaStr) && buildingArea == null)
                    {
                        isError = true;
                        dr["建筑面积_平方米"] = buildingAreaStr + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[BuildingArea]=@BuildingArea,");
                }

                if (dt.Columns.Contains("建筑结构"))
                {
                    var buildingStructureCodeName = dt.Rows[i]["建筑结构"].ToString().Trim();
                    var buildingStructureCode = GetCodeByName(buildingStructureCodeName, SYS_Code_Dict._建筑结构);
                    dh.BuildingStructureCode = buildingStructureCode;
                    dr["建筑结构"] = buildingStructureCodeName;
                    if (!string.IsNullOrEmpty(buildingStructureCodeName) && buildingStructureCode == null)
                    {
                        isError = true;
                        dr["建筑结构"] = buildingStructureCodeName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[BuildingStructureCode]=@BuildingStructureCode,");
                }

                if (dt.Columns.Contains("户型结构"))
                {
                    var structureCodeName = dt.Rows[i]["户型结构"].ToString().Trim();
                    var structureCode = GetCodeByName(structureCodeName, SYS_Code_Dict._户型结构);
                    dh.StructureCode = structureCode;
                    dr["户型结构"] = structureCodeName;
                    if (!string.IsNullOrEmpty(structureCodeName) && structureCode == null)
                    {
                        isError = true;
                        dr["户型结构"] = structureCodeName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[StructureCode]=@StructureCode,");
                }

                if (dt.Columns.Contains("建筑年代"))
                {
                    var buildingDateName = dt.Rows[i]["建筑年代"].ToString().Trim();
                    var buildingDate = GetCodeByName(buildingDateName, SYS_Code_Dict._建筑年代);
                    dh.BuildingDate = buildingDate;
                    dr["建筑年代"] = buildingDateName;
                    if (!string.IsNullOrEmpty(buildingDateName) && buildingDate == null)
                    {
                        isError = true;
                        dr["建筑年代"] = buildingDateName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[BuildingDate]=@BuildingDate,");
                }

                if (dt.Columns.Contains("装修档次"))
                {
                    var zhuangXiuName = dt.Rows[i]["装修档次"].ToString().Trim();
                    var zhuangXiu = GetCodeByName(zhuangXiuName, SYS_Code_Dict._装修档次);
                    dh.ZhuangXiu = zhuangXiu;
                    dr["装修档次"] = zhuangXiuName;
                    if (!string.IsNullOrEmpty(zhuangXiuName) && zhuangXiu == null)
                    {
                        isError = true;
                        dr["装修档次"] = zhuangXiuName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[ZhuangXiu]=@ZhuangXiu,");
                }

                if (dt.Columns.Contains("贷款额度"))
                {
                    var loanedLinesName = dt.Rows[i]["贷款额度"].ToString().Trim();
                    var loanedLines = GetCodeByName(loanedLinesName, SYS_Code_Dict._贷款额度);
                    dh.LoanedLines = loanedLines;
                    dr["贷款额度"] = loanedLinesName;
                    if (!string.IsNullOrEmpty(loanedLinesName) && loanedLines == null)
                    {
                        isError = true;
                        dr["贷款额度"] = loanedLinesName + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[LoanedLines]=@LoanedLines,");
                }

                if (dt.Columns.Contains("贷款期限_月"))
                {
                    var loanedDeadlineStr = dt.Rows[i]["贷款期限_月"].ToString().Trim();
                    var loanedDeadline = (int?)TryParseHelper.StrToInt32(loanedDeadlineStr);
                    dh.LoanedDeadline = loanedDeadline;
                    dr["贷款期限_月"] = loanedDeadlineStr;
                    if (!string.IsNullOrEmpty(loanedDeadlineStr) && loanedDeadline == null)
                    {
                        isError = true;
                        dr["贷款期限_月"] = loanedDeadlineStr + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[LoanedDeadline]=@LoanedDeadline,");
                }

                if (dt.Columns.Contains("附属物业"))
                {
                    var subHouse = dt.Rows[i]["附属物业"].ToString().Trim();
                    dh.SubHouse = subHouse;
                    dr["附属物业"] = subHouse;
                    if (!string.IsNullOrEmpty(subHouse) && subHouse.Length > 50)
                    {
                        isError = true;
                        dr["附属物业"] = subHouse + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[SubHouse]=@SubHouse,");
                }

                if (dt.Columns.Contains("备注"))
                {
                    var remark = dt.Rows[i]["备注"].ToString().Trim();
                    dh.Remark = remark;
                    dr["备注"] = remark;
                    if (!string.IsNullOrEmpty(remark) && remark.Length > 200)
                    {
                        isError = true;
                        dr["备注"] = remark + "#error";
                    }

                    if (i == 0)
                        modifiedProperty.Add("[Remark]=@Remark,");
                }


                if (i == 0)
                {
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
                    listTrue.Add(dh);
                }
            }
        }

    }
}
