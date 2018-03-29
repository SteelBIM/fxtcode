using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtService.Contract.FxtSpiderInterface;
using Newtonsoft.Json.Linq;
using FxtNHibernater.Data;
using System.Linq.Expressions;
using FxtNHibernate.DATProjectDomain.Entities;
using Newtonsoft.Json;
using FxtService.Common;
using System.Runtime.Serialization;
using log4net;

/**
 * 作者: 李晓东
 * 时间: 2013.11.27
 * 摘要: 新建Wcf (具体化)Actualize Fxtspider(实现接口IFxtspider)
 *       2013.12.05 增加 GetDatProject 修改人:李晓东
 * **/
namespace FxtService.Service.FxtSpiderActualize
{
    [FxtService.Service.ServiceBehavior]
    public class Fxtspider : IFxtspider
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(Fxtspider));
        List<string> FilterField()
        {
            List<string> list = new List<string>();
            list.Add("行政区");
            list.Add("ID");
            list.Add("片区");
            return list;
        }

        //抓取导入,备注,要与SYS_City_Table关联操作添加楼信息 2013.11.29 16:56
        public string SpiderExport(string data)
        {
            int fxtCompanyId = 25;
            List<JObject> rList = new List<JObject>();
            JArray _jarray = JArray.Parse(data);//解析导入数据
            int i = 0;
            MSSQLDBDAL _dbDal = new MSSQLDBDAL();
            while (_jarray.Count > i)
            {
                JObject _jobject = (JObject)_jarray[i];
                i++;
                string _jobjectID = _jobject["ID"].Value<string>();
                string _jobjectCity = _jobject["城市"].Value<string>();
                string _jobjectArea = _jobject["行政区"].Value<string>();
                string _jobjectSubArea = _jobject["片区"].Value<string>();
                string _jobjectProject = _jobject["楼盘名"].Value<string>();

                SYSCity modelSyscity = new SYSCity()
                {
                    CityName = _jobjectCity
                };
                //验证城市
                var vSysCity = UtilityDALHelper.GetCity(_dbDal, modelSyscity);
                if (vSysCity == null)
                {
                    rList.Add(SetResult(_jobjectID, string.Format("城市{0}找不到", _jobjectCity)));
                    continue;
                }
                //验证城市楼盘
                modelSyscity.CityId = vSysCity.CityId;
                var vSysCityTable = UtilityDALHelper.GetCityTable(_dbDal, vSysCity);
                if (vSysCityTable == null)
                {
                    rList.Add(SetResult(_jobjectID, "城市楼盘表找不到"));
                    continue;
                }
                //验证行政区
                var vSysArea = UtilityDALHelper.GetSYSArea(_dbDal, vSysCity, _jobjectArea);
                //if (vSysArea == null)
                //{
                //    rList.Add(SetResult(_jobjectID, string.Format("行政区:{0}找不到", _jobjectArea)));
                //    continue;
                //}
                object _datCase =  new DATCase();//默认案例(楼信息)
                string datProjectId = string.Empty;//楼盘ID
                //检索把 楼信息(案例) 添加到所属分区中
                if (!string.IsNullOrEmpty(vSysCityTable.ProjectTable.Trim()))
                {
                    //相关区域楼盘是否找到
                    //根据全名
                    //string hsql =
                    //    string.Format("{0} Valid=1 and ProjectName='{1}' and CityID={2}",
                    //    Utility.GetMSSQL_SQL(typeof(DATProject),
                    //    vSysCityTable.ProjectTable.Trim()),
                    //    _jobjectProject,
                    //    modelSyscity.CityId);

                    //DATProject datProject = _dbDal.GetCustomSQLQueryEntity<DATProject>(hsql);
                    DATProject datProject = UtilityDALHelper.GetProjectByCompanyIdAndCityIdAndProjectName(_dbDal, vSysCityTable.ProjectTable, fxtCompanyId, vSysCity.CityId, _jobjectProject);
                    //根据别名
                    if (datProject == null)
                    {
                        //string hsql2 = string.Format("{0} Valid=1 and  OtherName='{1}' and CityID={2}",
                        //                       Utility.GetMSSQL_SQL(typeof(DATProject),
                        //                       vSysCityTable.ProjectTable.Trim()),
                        //                       _jobjectProject,
                        //                       modelSyscity.CityId);

                        //datProject = _dbDal.GetCustomSQLQueryEntity<DATProject>(hsql2);
                        datProject = UtilityDALHelper.GetProjectByCompanyIdAndCityIdAndOtherName(_dbDal, vSysCityTable.ProjectTable, fxtCompanyId, vSysCity.CityId, _jobjectProject);
                    }
                    //如果不存在,继续下一个检索
                    //根据网络名
                    if (datProject == null)//楼盘不存在要记录
                    {
                        var sysProjectMatch = UtilityDALHelper.GetListSYSProjectMatch_Fxt(_dbDal, _jobjectProject, vSysCity.CityId);

                        if (sysProjectMatch == null || sysProjectMatch.Count < 1)//检测网络名是否存在
                        {
                            rList.Add(SetResult(_jobjectID, "1"));
                            continue;
                        }
                        else
                        {
                            datProject = UtilityDALHelper.GetProjectByTableNameAndProjectId(_dbDal, vSysCityTable.ProjectTable.Trim(), Convert.ToInt32(sysProjectMatch[0].ProjectNameId));
                            //网络名关联的楼盘不存在||可能存在多个
                            if (datProject == null || !CheckProjectMatchJoinProjectName(datProject.ProjectName))
                            {
                                rList.Add(SetResult(_jobjectID, "1"));
                                continue;
                            }
                            datProjectId = sysProjectMatch[0].ProjectNameId.ToString();                             

                        }
                    }
                    else
                    {
                        datProjectId = datProject.ProjectId.ToString();
                    }

                }
                else
                {
                    rList.Add(SetResult(_jobjectID, string.Format("城市ID:{0}对应表不存在", _jobjectCity == null ? "null" : _jobjectCity)));
                    continue;
                }
                

                if (!string.IsNullOrEmpty(vSysCityTable.CaseTable.Trim()))//案例(楼信息)
                {
                    string strCityTable = vSysCityTable.CaseTable.Trim();

                    //反射创建动态创建对象
                    _datCase = Utility.LoadAssembly("FxtNHibernate.DATProjectDomain",
                        string.Format("FxtNHibernate.DATProjectDomain.Entities.{0}", strCityTable));
                }

                foreach (var _vobject in _jobject)
                {
                    string key = _vobject.Key;
                    //去除相关无效信息
                    if (FilterField().Where(filter => filter.ToLower().Contains(key.ToLower()))
                        .Any()) continue;
                    object value = _vobject.Value.Value<string>();
                    string[] tableArray = GetTableDictionary(key).Split(',');//与外表有关系的列
                    string tableDictionaryValue = GetTableDictionary(key);
                    string pTypeKey =
                        tableArray.Length == 1 ? GetTableDictionary(key) : tableArray[0];

                    var propertyObj = _datCase.GetType()
                             .GetProperties()
                             .Where(pInfo =>
                                 pInfo.Name.Equals(pTypeKey)).FirstOrDefault();

                    if (propertyObj == null) continue;

                    bool convertResult = true;
                    object defaultValue = null;
                    JObject resultObjict = null;
                    int inull = 0;
                    if (tableArray.Length > 1 && !tableArray[1].Equals("notNull"))//是否与其他表有关系
                    {
                        switch (tableArray[1])
                        {
                            case "SYS_City"://城市
                                //根据 城市名称 找到相应的ID
                                value = modelSyscity != null ? modelSyscity.CityId.ToString() : null;
                                value = valueType(propertyObj.PropertyType, value.ToString(), out convertResult, out defaultValue);//城市ID
                                break;
                            case "DAT_Project"://楼盘(楼宇)
                                value = valueType(propertyObj.PropertyType, datProjectId, out convertResult, out defaultValue);//楼盘(楼宇)ID
                                break;
                            case "SYS_Area"://行政区
                                //根据 行政区 找到相应的ID
                                if (vSysArea != null && vSysArea.AreaId == 0)
                                    Utility.ModelSetValue(_datCase, "AreaName", value);
                                value = vSysArea != null ? vSysArea.AreaId.ToString() : null;
                                value = valueType(propertyObj.PropertyType, value.ToString(), out convertResult, out defaultValue);//行政区ID
                                break;
                            case "DAT_Building"://楼宇
                                //传递名称为空
                                if (string.IsNullOrEmpty(Convert.ToString(value)))
                                {
                                    value = null;
                                    inull = 1;
                                    break;
                                }
                                //找ID
                                var vdatBuilding = _dbDal.GetCustom<DATBuilding>(
                                     (Expression<Func<DATBuilding, bool>>)(datBuilding =>
                                     datBuilding.BuildingName == value.ToString())
                                     );
                                value = vdatBuilding != null ? vdatBuilding.BuildingId.ToString() : "";
                                value = valueType(propertyObj.PropertyType, value.ToString(), out convertResult, out defaultValue);//楼宇ID
                                break;
                            case "SYS_Code"://Code

                                //如果Code为户型
                                if (int.Parse(tableArray[3]) == 4001)
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(value)))
                                    {
                                        value = value.ToString().Replace("房", "室");
                                    }
                                }
                                if (string.IsNullOrEmpty(Convert.ToString(value)) || string.IsNullOrEmpty(value.ToString().Trim()))
                                {
                                    inull = 1;
                                    break;
                                }
                                //根据 Code 找到相应的ID
                                var vsysCode = _dbDal.GetCustom<SYSCode>(
                                    (Expression<Func<SYSCode, bool>>)(sysCode =>
                                    sysCode.CodeName == value.ToString() &&
                                    sysCode.ID == int.Parse(tableArray[3]))
                                    );
                                //相应Code必须存在,否则退出
                                if (vsysCode == null && !string.IsNullOrEmpty(value.ToString()))
                                {
                                    resultObjict = SetResult(_jobjectID, string.Format("{0}找不到!", key));
                                    //rList.Add(SetResult(_jobjectID, string.Format("{0}找不到!", key)));
                                    inull = 1;
                                    break;
                                }
                                ////案例类型、用途不能为空 3001 案例类型  1002 用途
                                //if (vsysCode == null && (key.Contains("3001") || key.Contains("1002")))
                                //{
                                //    resultObjict = SetResult(_jobjectID, string.Format("{0}不允许为空!", key));
                                //    //rList.Add(SetResult(_jobjectID, string.Format("{0}不允许为空!", key)));
                                //    inull = 1;
                                //}
                                if (vsysCode != null && vsysCode.Code == 0 && key.Contains("装修"))
                                {
                                    Utility.ModelSetValue(_datCase, "ZhuangXiu", value);
                                }
                                value = vsysCode != null ? vsysCode.Code.ToString() : "";
                                value = valueType(propertyObj.PropertyType, value.ToString(), out convertResult, out defaultValue);//Code
                                break;
                            default:
                                break;
                        }
                        //if (inull > 0)//如果有必填项且为空就跳出
                        //    break;
                    }
                    else
                    {
                        if (key.Contains("配套设施"))
                        {
                            //附属物业
                            string[] arraySubHouse = { "储藏室", "阁楼", "地下室", "阳光房", "储藏室/地下室" };
                            StringBuilder sbSubHouse = new StringBuilder();
                            arraySubHouse.Where(sh =>
                                value.ToString().Split(',')
                                .Where(keyval => keyval.Equals(sh)).Any())
                                .ToList<string>().ForEach(val =>
                                {
                                    sbSubHouse.AppendFormat("{0},", val);
                                });
                            Utility.ModelSetValue(_datCase, "SubHouse", sbSubHouse.ToString().TrimEnd(','));
                        }
                        value = valueType(propertyObj.PropertyType, value.ToString(), out convertResult, out defaultValue);
                    }
                    //有值为空
                    if (inull > 0)
                    {
                        //并且为必填
                        if (tableDictionaryValue.Contains("notNull"))
                        {
                            if (resultObjict == null)
                            {
                                resultObjict = SetResult(_jobjectID, string.Format("{0}不允许为空!", key));
                            }
                            rList.Add(resultObjict);
                            break;
                        }
                        value = defaultValue;
                    }
                    else if (!convertResult)//数据类型转换失败
                    {
                        //并且为必填
                        if (tableDictionaryValue.Contains("notNull"))
                        {
                            if (resultObjict == null)
                            {
                                resultObjict = SetResult(_jobjectID, string.Format("{0}格式错误!值:{1}", key, Convert.ToString(value)));
                            }
                            rList.Add(resultObjict);
                            break;
                        }
                        value = defaultValue;
                    }
                    propertyObj.SetValue(_datCase, value, null);
                }

                var propertyObjValid = _datCase.GetType()
                         .GetProperties()
                         .Where(pInfo =>
                             pInfo.Name.Equals("Valid")).FirstOrDefault();
                propertyObjValid.SetValue(_datCase, 1, null);
                var propertyObjFXTCompanyId = _datCase.GetType()
                         .GetProperties()
                         .Where(pInfo =>
                             pInfo.Name.Equals("FXTCompanyId")).FirstOrDefault();
                propertyObjFXTCompanyId.SetValue(_datCase, 25, null);
                var propertyObjCompanyId = _datCase.GetType()
                         .GetProperties()
                         .Where(pInfo =>
                             pInfo.Name.Equals("CompanyId")).FirstOrDefault();
                propertyObjCompanyId.SetValue(_datCase, 25, null);
                var propertyObjCreator = _datCase.GetType()
                         .GetProperties()
                         .Where(pInfo =>
                             pInfo.Name.Equals("Creator")).FirstOrDefault();
                propertyObjCreator.SetValue(_datCase, "网上析取", null);
                var propertyObjSourceLink = _datCase.GetType()
                         .GetProperties()
                         .Where(pInfo =>
                             pInfo.Name.Equals("SourceLink")).FirstOrDefault();
                string sourceLike = Convert.ToString(propertyObjSourceLink.GetValue(_datCase, null));
                if (!string.IsNullOrEmpty(sourceLike) && sourceLike.Length >= 200)
                {
                    propertyObjSourceLink.SetValue(_datCase, null, null);
                }
                try
                {
                    Utility.ModelSetValue(_datCase, "CreateDate", DateTime.Now);
                    Utility.ModelSetValue(_datCase, "SaveDateTime", DateTime.Now);
                    DATCase case2 = JsonConvert.DeserializeObject<DATCase>(JsonConvert.SerializeObject(_datCase));
                    DATCase existsObj = GetCaseIdentical(_dbDal, case2, vSysCityTable.CaseTable);
                    object _obj = null;
                    //如果未导入过
                    if (existsObj == null)
                    {
                        if (case2.PurposeCode == null || case2.PurposeCode == 0)
                        {
                            rList.Add(SetResult(_jobjectID, "用途不能为null"));
                            continue;
                        }
                        //为别墅类型
                        if (UtilityDALHelper.GetVillaPurposeCodes().Contains(Convert.ToInt32(case2.PurposeCode)))
                        {
                            //超过范围
                            if (Convert.ToDecimal(case2.UnitPrice) <= 3000 || Convert.ToDecimal(case2.UnitPrice) >= 200000)
                            {
                                rList.Add(SetResult(_jobjectID, "价格超过范围", true, fxtId: -1));
                                continue;
                            }
                        }
                        else
                        {

                            int buildingAreaTypeCode = GetSpiderExportBuildingAreaTypeCode(case2.BuildingArea);
                            int avgPrice = 0;
                            DATAvgPriceDay dvgPriceObj = UtilityDALHelper.GetDATAvgPriceDayByProjectIdAndCityIdAndBuildingAreaType(_dbDal, Convert.ToInt32(case2.ProjectId), vSysCity.CityId, buildingAreaTypeCode);
                            if (dvgPriceObj == null && buildingAreaTypeCode != 1005000)
                            {
                                dvgPriceObj = UtilityDALHelper.GetDATAvgPriceDayByProjectIdAndCityIdAndBuildingAreaType(_dbDal, Convert.ToInt32(case2.ProjectId), vSysCity.CityId, buildingAreaTypeCode);
                            }
                            if (dvgPriceObj != null)
                            {
                                avgPrice = dvgPriceObj.AvgPrice;
                            }
                            int maxAvgPrice = avgPrice + Convert.ToInt32(avgPrice * 0.15);
                            int minAvgPrice = avgPrice = Convert.ToInt32(avgPrice * 0.15);
                            //超过范围
                            if (Convert.ToDecimal(case2.UnitPrice) < minAvgPrice || Convert.ToDecimal(case2.UnitPrice) > maxAvgPrice)
                            {
                                rList.Add(SetResult(_jobjectID, "价格超过范围", true, fxtId: -1));
                                continue;
                            }
                        }
                        _obj = _dbDal.Create(_datCase);
                    }
                    else
                    {
                        _obj = existsObj.CaseID;
                    }
                    if (!string.IsNullOrEmpty(_obj.ToString()) && long.Parse(_obj.ToString()) > 0)
                    {
                        rList.Add(SetResult(_jobjectID, "", true, fxtId: Convert.ToInt32(_obj)));
                    }
                    else
                    {
                        rList.Add(SetResult(_jobjectID, "添加信息失败"));
                    }
                }
                catch (Exception ex)
                {
                    rList.Add(SetResult(_jobjectID, ex.Message));
                }
                log.Debug("导入案例:"+JsonConvert.SerializeObject(_datCase));
            }
            _dbDal.Close();
            return JsonConvert.SerializeObject(rList);
        }
        /// <summary>
        /// 获取导入案例的面积段code
        /// </summary>
        /// <param name="buildingArea"></param>
        /// <returns></returns>
        int GetSpiderExportBuildingAreaTypeCode(decimal? buildingArea)
        {
            int buildingAreaTypeCode = 1005000;
            if (buildingArea!= null && buildingArea != 0)
            {
                if (buildingArea < 60)
                {
                    buildingAreaTypeCode = 1005001;
                }
                else if (buildingArea >= 40 && buildingArea < 90)
                {
                    buildingAreaTypeCode = 1005002;
                }
                else if (buildingArea >= 90 && buildingArea <= 144)
                {
                    buildingAreaTypeCode = 1005003;
                }
                else
                {
                    buildingAreaTypeCode = 1005004;
                }
            }
            return buildingAreaTypeCode;
        }
        /// <summary>
        /// 相同案例是否已经导入过
        /// </summary>
        /// <param name="db"></param>
        /// <param name="obj"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        DATCase GetCaseIdentical(MSSQLDBDAL db, DATCase obj,string table)
        {
            return null;
            string startDt = obj.CaseDate.AddMonths(-1).Date.ToString("yyyy-MM-dd");
            string endDt = obj.CaseDate.ToString("yyyy-MM-dd") + " 23:59:59";
            string sql = string.Format("{0} Valid=1 and  BuildingTypeCode{1} and HouseTypeCode{2} and FrontCode{3} and FitmentCode{4} "+
                                          " and BuildingArea{5} and UnitPrice{6} and FloorNumber{7} and TotalFloor{8} and CaseDate>='{9}' and CaseDate<='{10}'",
                                               Utility.GetMSSQL_SQL(typeof(DATCase),
                                              table.Trim()),
                                              obj.BuildingTypeCode == null ? " is null" : " =" + obj.BuildingTypeCode,
                                              obj.HouseTypeCode == null ? " is null" : " =" + obj.HouseTypeCode,
                                              obj.FrontCode == null ? " is null" : " =" + obj.FrontCode,
                                              obj.FitmentCode == null ? " is null" : " =" + obj.FitmentCode,//
                                               obj.BuildingArea == null ? " is null" : " =" + obj.BuildingArea,
                                               obj.UnitPrice == null ? " is null" : " =" + obj.UnitPrice,
                                               obj.FloorNumber== null ? " is null" : " =" + obj.FloorNumber,
                                               obj.TotalFloor == null ? " is null" : " =" + obj.TotalFloor, 
                                               startDt, endDt
                                               );

            var caseObj = db.GetCustomSQLQueryEntity<DATCase>(sql);
            return caseObj;
        }
        JObject SetResult(string id, string remark, bool success = false,int fxtId=0)
        {
            JObject _jobject = new JObject();
            _jobject.Add(new JProperty("ID", id));
            _jobject.Add(new JProperty("Remark", remark));
            _jobject.Add(new JProperty("Success", success));
            _jobject.Add(new JProperty("FxtId", fxtId));
            return _jobject;
        }

        private string GetTableDictionary(string key)
        {
            Dictionary<string, string> list = new Dictionary<string, string>();
            list.Add("城市", "CityID,SYS_City,CityName,notNull");//DAT_Case表 城市 CityID to SYS_City表 CityName
            list.Add("楼盘名", "ProjectId,DAT_Project,ProjectName,notNull");//DAT_Case表 楼盘ID ProjectId int to DAT_Project表 ProjectName
            list.Add("案例时间", "CaseDate,notNull");//DAT_Case表 时间 datetime
            list.Add("行政区", "AreaID,SYS_Area,AreaName,notNull");//DAT_Project表 行政区 int to SYS_Area表 AreaName
            list.Add("片区", "SubAreaId,SYS_SubArea,SubAreaName");//DAT_Project表 片区 int to SYS_SubArea表 SubAreaName
            list.Add("楼栋", "BuildingId,DAT_Building,BuildingName");//DAT_Case表 楼宇 int to DAT_Building表 BuildingName
            list.Add("房号", "HouseNo");//DAT_Case 房号 nvarchar(100)
            list.Add("用途", "PurposeCode,SYS_Code,CodeName,"+Utility.CodeID_2+",notNull");//DAT_Case表 用途Code int  to SYS_Code表 CodeName
            list.Add("面积", "BuildingArea,notNull");// DAT_Case表  建筑面积
            list.Add("单价", "UnitPrice,notNull");// DAT_Case表  单价
            list.Add("案例类型", "CaseTypeCode,SYS_Code,CodeName," + Utility .CodeID_3+ ",notNull");//DAT_Case表 案例类型 int  to SYS_Code表 CodeName
            list.Add("结构", "StructureCode,SYS_Code,CodeName,"+Utility.CodeID_4);//DAT_Case表 建筑结构 int  to SYS_Code表 CodeName
            list.Add("建筑类型", "BuildingTypeCode,SYS_Code,CodeName," + Utility.CodeID_5+ "");//DAT_Case表 建筑类型 int  to SYS_Code表 CodeName
            list.Add("总价", "TotalPrice,notNull");//DAT_Case表 总价 numeric
            list.Add("所在楼层", "FloorNumber");//DAT_Case表 楼层 int
            list.Add("总楼层", "TotalFloor");//DAT_Case表 总层数 int
            list.Add("户型", "HouseTypeCode,SYS_Code,CodeName," + Utility.CodeID_6 + "");//DAT_Case表 户型Code int to SYS_Code表 CodeName
            list.Add("朝向", "FrontCode,SYS_Code,CodeName, " + Utility.CodeID_7 + "");//DAT_Case表 朝向Code int to SYS_Code表 CodeName
            //list.Add("装修", "FitmentCode,SYS_Code,CodeName," + Utility.CodeID_9 + "");//DAT_Case表 装修情况 int to SYS_Code表 CodeName
            list.Add("装修", "ZhuangXiu");
            list.Add("建筑年代", "BuildingDate");//DAT_Case表 建筑年代 BuildingDate
            list.Add("信息", "Rmark");//DAT_Case表 说明 nvarchar(255)
            list.Add("电话", "SourcePhone");//DAT_Case表 来源电话 nvarchar(50)
            list.Add("URL", "SourceLink");//DAT_Case表 来源链接 nvarchar(200)
            list.Add("币种", "MoneyUnitCode,SYS_Code,CodeName," + Utility.CodeID_8 + "");//DAT_Case表 货币单位Code int to SYS_Code表 CodeName
            list.Add("地址", "Address");//DAT_Project表 楼盘地址 nvarchar(600)
            list.Add("创建时间", "CreateDate");////DAT_Case表 创建时间  此时间的值与(SaveDateTime)保存时间一样
            //list.Add("建筑形式", "");
            //list.Add("花园面积", "");
            //list.Add("厅结构", "");
            //list.Add("车位数量", "");
            list.Add("配套设施", "PeiTao");//DAT_Case表 配套设施 PeiTao
            //list.Add("地下室面积", "");
            list.Add("网站", "SourceName");//DAT_Case表 来源 SourceName nvarchar(100)
            list.Add("来源", "SourceName");//DAT_Case表 来源 SourceName nvarchar(100)
            return list.ContainsKey(key) ? list[key] : "";
        }

        private bool CheckProjectMatchJoinProjectName(string projectName)
        {
            return true;
        }

        object valueType(Type t, string value,out bool convertResult,out object defaultValue)
        {
            convertResult = true;
            defaultValue = null;
            object o = null;
            string strName = t.Name;
            if (t.Name == "Nullable`1")
            {
                strName = t.GetGenericArguments()[0].Name;
            }
            if (value == null)
            {
                return null;
            }

            switch (strName.Trim())
            {
                case "Decimal":
                    value = StringHelp.TrimBlank(value);
                    if (!StringHelp.IsDecimal(value))
                    {
                        o = value;
                        convertResult = false;
                        defaultValue = 0;
                        break;
                    }
                    o = decimal.Parse(string.IsNullOrEmpty(value) ? "0.00" : value);
                    break;
                case "Int32":
                    value = StringHelp.TrimBlank(value);
                    if (!StringHelp.IsInteger(value))
                    {
                        o = value;
                        convertResult = false;
                        defaultValue = 0;
                        break;
                    }
                    o = int.Parse(string.IsNullOrEmpty(value) ? "0" : value);
                    break;
                case "Float":
                    value = StringHelp.TrimBlank(value);
                    if (!StringHelp.IsFloat(value))
                    {
                        o = value;
                        convertResult = false;
                        defaultValue = 0;
                        break;
                    }
                    o = float.Parse(string.IsNullOrEmpty(value) ? "0.0f" : value);
                    break;
                case "DateTime":
                    value = value == null ? value : value.Trim();
                    if (string.IsNullOrEmpty(StringHelp.TrimBlank(value)) || !StringHelp.IsDateTime(value))
                    {
                        o = value;
                        convertResult = false;
                        defaultValue = DateTime.Now;
                        break;
                    }
                    o = !string.IsNullOrEmpty(value) ? DateTime.Parse(value) : DateTime.Now;
                    break;
                default:
                    o = value;
                    break;
            }
            return o;
        }

        //获得楼盘信息
        public string GetDatProject(int cityId, string projectName, string sDate, string eDate, int pageSize, int pageIndex)
        {
            MSSQLDBDAL _mssqldbDal = new MSSQLDBDAL();

            DATProject datProject = new DATProject()
            {
                CityID = cityId,
                ProjectName = projectName
            };

            SYSCity _sysctiy = new SYSCity() { CityId = datProject.CityID };

            var sysCity = UtilityDALHelper.GetCity(_mssqldbDal, _sysctiy);

            var sysCityTable = UtilityDALHelper.GetCityTable(_mssqldbDal, _sysctiy);

            string hsql = string.Format(" {0} {1}", Utility.GetMSSQL_HSQL(sysCityTable.ProjectTable),
                Utility.GetModelFieldKeyValue(datProject));

            UtilityPager pager = new UtilityPager(pageSize, pageIndex);

            var listDatProject = _mssqldbDal.HQueryPagerList<DATProject>(pager, hsql);

            return "[{" + string.Format("\"List\":{0},\"PageCount\":{1}", JsonConvert.SerializeObject(listDatProject), pager.Count) + "}]";
        }
    }
}
