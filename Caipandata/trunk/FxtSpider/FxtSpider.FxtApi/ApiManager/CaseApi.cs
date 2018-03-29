using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.DAL.LinqToSql;
using FxtSpider.FxtApi.Fxt.Wcf;
using FxtSpider.FxtApi.Model;
using FxtSpider.Dll.Manager;
using FxtSpider.FxtApi.Common;
using FxtSpider.Common;
using FxtSpider.FxtApi.Fxt.Api;
using Newtonsoft.Json.Linq;
using log4net;
using FxtSpider.FxtApi.FxtApiClientManager;

namespace FxtSpider.FxtApi.ApiManager
{
    /// <summary>
    /// 案例api
    /// </summary>
    public static class CaseApi
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(CaseApi));

        #region (更新)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_list"></param>
        /// <param name="caseIds">输出过滤掉的案例ID</param>
        /// <returns></returns>
        public static bool 发布需要整理的数据到服务器(List<VIEW_案例信息_城市表_网站表> _list, out List<案例库上传信息过滤表> 过滤信息, out Dictionary<long, int> 原始库ID对应成功的房讯通ID, FxtAPIClientExtend _fxtApi = null)
        {
            
            原始库ID对应成功的房讯通ID = new Dictionary<long, int>();
            过滤信息 = new List<案例库上传信息过滤表>();
            string json = "";
            string nowIp =WcfCheck.GetWcfCheckIp();
            string validate = WcfCheck.GetWcfCheckValidData();
            if (_list == null || _list.Count < 1)
            {
                return true;
            }
            FxtAPIClientExtend fxtServer = new FxtAPIClientExtend(_fxtApi);
            try
            {
                json = _list.FxtApi_GetJson();
                if (string.IsNullOrEmpty(json))
                {
                    fxtServer.Abort();
                    return true;
                }


                string name = "SpiderExport";
                var para = new { data = json };
                string jsonStr = Convert.ToString(EntranceApi.Entrance_FxtSpider(name, para.ToJSONjss(), _fxtApi: fxtServer));

                List<SpiderExportResult> list = JsonHelp.ParseJSONList<SpiderExportResult>(jsonStr);
                foreach (SpiderExportResult obj in list)
                {
                    if (obj.Success.ToLower().Equals("false"))
                    {
                        VIEW_案例信息_城市表_网站表 obj2 = _list.Find(delegate(VIEW_案例信息_城市表_网站表 _obj2) { return _obj2.ID == Convert.ToInt64(obj.ID); });
                        if (obj2 != null)
                        {
                            int 错误类型ID = StaticValue.其他_ID;
                            string 错误说明 = "";
                            if (obj.Remark.Equals(SpiderExportResult.Remark_楼盘名不存在))
                            {
                                错误类型ID = StaticValue.楼盘名不存在_ID;
                                错误说明 = "楼盘名不存在";
                            }
                            else if (obj.Remark.Equals(SpiderExportResult.Remark_系统异常))
                            {
                                错误类型ID = StaticValue.系统异常_ID;
                                错误说明 = "系统异常";
                            }
                            else
                            {
                                错误类型ID = StaticValue.其他_ID;
                                错误说明 = obj.Remark;
                            }
                            案例库上传信息过滤表 obj3 = new 案例库上传信息过滤表 { 案例ID = obj2.ID, 城市ID = obj2.城市ID, 网站ID = obj2.网站ID, 错误类型ID = 错误类型ID, 错误说明 = 错误说明, 过滤时间 = DateTime.Now };
                            过滤信息.Add(obj3);
                        }
                    }
                    else
                    {
                        原始库ID对应成功的房讯通ID.Add(Convert.ToInt64(obj.ID), obj.FxtId);
                    }
                }
                fxtServer.Abort();
            }
            catch (Exception ex)
            {
                log.Error("发布需要整理的数据到服务器(List<VIEW_案例信息_城市表_网站表> _list, out List<案例库上传信息过滤表> 过滤信息,out Dictionary<long,int> 原始库ID对应成功的房讯通ID)", ex);
                fxtServer.Abort();
                return false;
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_obj"></param>
        /// <param name="过滤信息">输出过滤掉的案例</param>
        /// <returns></returns>
        public static bool 发布需要整理的数据到服务器(VIEW_案例信息_城市表_网站表 _obj, out 案例库上传信息过滤表 过滤信息, out int fxtId, FxtAPIClientExtend _fxtApi = null)
        {
            fxtId = 0;
            bool result = true;
            Dictionary<long, int> dic = new Dictionary<long, int>();
            List<VIEW_案例信息_城市表_网站表> _list = new List<VIEW_案例信息_城市表_网站表>();
            List<案例库上传信息过滤表> 过滤信息List = new List<案例库上传信息过滤表>();
            过滤信息 = null;
            if (_obj == null)
            {
                return true;
            }
            _list.Add(_obj);
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            try
            {
                result = 发布需要整理的数据到服务器(_list, out 过滤信息List, out dic, _fxtApi: fxtApi);
                if (过滤信息List != null && 过滤信息List.Count > 0)
                {
                    过滤信息 = 过滤信息List[0];
                }
                if (dic.ContainsKey(_obj.ID))
                {
                    fxtId = dic[_obj.ID];
                }
                fxtApi.Abort();
            }
            catch (Exception ex)
            {
                log.Error("发布需要整理的数据到服务器(VIEW_案例信息_城市表_网站表 _obj, out 案例库上传信息过滤表 过滤信息,out int fxtId)", ex);
                fxtApi.Abort();
                return false;
            }
            return result;
        }
        /// <summary>
        /// 删除案例
        /// </summary>
        /// <param name="cityName">城市名称</param>
        /// <param name="caseIds"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool DeleteCaseByCityNameAndCaseIds(string cityName, int[] caseIds,out string message,FxtAPIClientExtend _fxtApi=null)
        {
            message = "";
            if (caseIds == null || caseIds.Length < 1)
            {
                return true;
            }
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            try
            {

                string name = "DeleteCaseByCityNameAndCaseIds";
                var para = new { cityName = cityName, caseIds = caseIds.ConvertToString() };
                string jsonStr = Convert.ToString(EntranceApi.Entrance(name, para.ToJSONjss(), _fxtApi: fxtApi));

                FxtApi_Result result = FxtApi_Result.ConvertToObj(jsonStr);
                bool _re = true;
                if (result.Result == 0)
                {
                    message = result.Message.DecodeField();
                    _re= false;
                }
                fxtApi.Abort();
                return _re;
            }
            catch (Exception ex)
            {
                message = "系统异常";
                log.Error("DeleteCaseByCityIdAndCaseIds(string cityName, int[] caseIds,out string message)", ex);
                fxtApi.Abort();
                return false;
            }
            return true;
        }
        /// <summary>
        /// 根据城市ID和多个caseId删除案例
        /// </summary>
        /// <param name="fxtCityId"></param>
        /// <param name="caseIds"></param>
        /// <param name="message"></param>
        /// <param name="_fxtApi"></param>
        /// <returns></returns>
        public static bool DeleteCaseByFxtCityIdAndCaseIds(int fxtCityId, int[] caseIds, out string message, FxtAPIClientExtend _fxtApi = null)
        {

            message = "";
            if (caseIds == null || caseIds.Length < 1)
            {
                return true;
            }
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            try
            {
                string name = "DeleteCaseByCityIdAndCaseIds";
                var para = new { cityId = fxtCityId, caseIds = caseIds.ConvertToString() };
                string json = Convert.ToString(EntranceApi.Entrance(name, para.ToJSONjss(), _fxtApi: fxtApi));
                FxtApi_PublicResult result = FxtApi_PublicResult.ConvertToObj(json);
                bool _re = true;
                if (result.type == 0)
                {
                    message = result.message.DecodeField();
                    _re = false;
                }
                fxtApi.Abort();
                return _re;
            }
            catch (Exception ex)
            {
                message = "系统异常";
                log.Error("DeleteCaseByCityIdAndCaseIds(int cityId, int[] caseIds,out string message)", ex);
                fxtApi.Abort();
                return false;
            }
            return true;
        }

        public static bool UpdateCase(int fxtCityId, FxtApi_DATCase caseObj, out string message, FxtAPIClientExtend _fxtApi = null)
        {
            message = "";
            if (caseObj == null)
            {
                return true;
            }
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            try
            {
                string caseJson = caseObj.EncodeField().ToJSONjss();


                string name = "UpdateCase";
                var para = new { cityId = fxtCityId, caseJson = caseJson };
                string jsonStr = Convert.ToString(EntranceApi.Entrance(name, para.ToJSONjss(), _fxtApi: fxtApi));

                FxtApi_Result result = FxtApi_Result.ConvertToObj(jsonStr);
                if (result.Result == 0)
                {
                    message = result.Message.DecodeField();
                    fxtApi.Abort();
                    return false;
                }
                fxtApi.Abort();
                caseObj.DecodeField();
            }
            catch (Exception ex)
            {
                message = "系统异常";
                log.Error("UpdateCase(int fxtCityId, FxtApi_DATCase caseObj, out string message)", ex);
                fxtApi.Abort();
                return false;
            }
            return true;
        }
        /// <summary>
        /// 新增案例
        /// </summary>
        /// <param name="caseId"></param>
        /// <param name="fxtCityId"></param>
        /// <param name="projectId"></param>
        /// <param name="buildingId"></param>
        /// <param name="houseNo"></param>
        /// <param name="caseDate"></param>
        /// <param name="purposeCode"></param>
        /// <param name="buildingArea"></param>
        /// <param name="unitPrice"></param>
        /// <param name="totalPrice"></param>
        /// <param name="caseTypeCode"></param>
        /// <param name="structureCode"></param>
        /// <param name="buildingTypeCode"></param>
        /// <param name="floorNumber"></param>
        /// <param name="totalFloor"></param>
        /// <param name="houseTypeCode"></param>
        /// <param name="frontCode"></param>
        /// <param name="moneyUnitCode"></param>
        /// <param name="remark"></param>
        /// <param name="areaId"></param>
        /// <param name="buildingDate"></param>
        /// <param name="fitmentCode"></param>
        /// <param name="subHouse"></param>
        /// <param name="peiTao"></param>
        /// <param name="createUser"></param>
        /// <param name="sourceName"></param>
        /// <param name="sourceLink"></param>
        /// <param name="sourcePhone"></param>
        /// <param name="message"></param>
        /// <param name="_fxtApi"></param>
        /// <returns></returns>
        public static FxtApi_DATCase InsertCase(int fxtCityId, int projectId, int? buildingId, string houseNo, DateTime caseDate, int? purposeCode,
            decimal? buildingArea, decimal? unitPrice, decimal? totalPrice, int? caseTypeCode, int? structureCode, int? buildingTypeCode,
            int? floorNumber, int? totalFloor, int? houseTypeCode, int? frontCode, int? moneyUnitCode, string remark, int? areaId,
            string buildingDate, int? fitmentCode, string subHouse, string peiTao, string createUser, string sourceName, string sourceLink, string sourcePhone, out string message, FxtAPIClientExtend _fxtApi = null)
        {
            message = "";
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            FxtApi_DATCase caseObj = null;
            try
            {

                string name = "InsertCase";
                var para = new {
                    cityId = fxtCityId,
                    projectId = projectId,
                    buildingId = buildingId,
                    houseNo = houseNo,
                    caseDate = caseDate,
                    purposeCode = purposeCode,
                    buildingArea = buildingArea,
                    unitPrice = unitPrice,
                    totalPrice = totalPrice,
                    caseTypeCode = caseTypeCode,
                    structureCode = structureCode,
                    buildingTypeCode = buildingTypeCode,
                    floorNumber = floorNumber,
                    totalFloor = totalFloor,
                    houseTypeCode = houseTypeCode,
                    frontCode = frontCode,
                    moneyUnitCode = moneyUnitCode,
                    remark = remark,
                    areaId = areaId,
                    buildingDate = buildingDate,
                    fitmentCode = fitmentCode,
                    subHouse = subHouse,
                    peiTao = peiTao,
                    createUser = createUser,
                    sourceName = sourceName,
                    sourceLink = sourceLink,
                    sourcePhone = sourcePhone
                };
                string jsonStr = Convert.ToString(EntranceApi.Entrance(name, para.ToJSONjss(), _fxtApi: fxtApi));

                FxtApi_Result result = FxtApi_Result.ConvertToObj(jsonStr);
                if (result.Result == 0)
                {
                    message = result.Message.DecodeField();
                    fxtApi.Abort();
                    return null;
                }
                caseObj = FxtApi_DATCase.ConvertToObj(result.Detail).DecodeField();
                fxtApi.Abort();

            }
            catch (Exception ex)
            {
                message = "系统异常";
                log.Error("InsertCase(int caseId, int fxtCityId, int projectId,....)", ex);
                fxtApi.Abort();
                return null;
            }
            return caseObj;
        }

        #endregion

        #region (查询)

        /// <summary>
        /// 根据房讯通城市ID and 多个案例ID获取案例信息
        /// </summary>
        /// <param name="fxtCityId"></param>
        /// <param name="fxtIds"></param>
        /// <returns></returns>
        public static List<FxtApi_DATCase> GetCaseByFxtCityIdAndCaseIds(int fxtCityId, int[] fxtCaseIds, FxtAPIClientExtend _fxtApi = null)
        {
            if (fxtCaseIds == null || fxtCaseIds.Length < 1)
            {
                return new List<FxtApi_DATCase>();
            }
            List<FxtApi_DATCase> list = new List<FxtApi_DATCase>();
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            try
            {
                StringBuilder sb = new StringBuilder("");
                foreach (int id in fxtCaseIds)
                {
                    sb.Append(id).Append(",");
                }
                string str = sb.ToString().TrimEnd(',');

                string name = "GetCaseByCityIdAndCaseIds";
                var para = new { cityId = fxtCityId, caseIds = str };
                string jsonStr = Convert.ToString(EntranceApi.Entrance(name, para.ToJSONjss(), _fxtApi: fxtApi));

                if (string.IsNullOrEmpty(jsonStr))
                {
                    return new List<FxtApi_DATCase>();
                }
                list = FxtApi_DATCase.ConvertToObjList(jsonStr);
                list = list.DecodeField<FxtApi_DATCase>();
                fxtApi.Abort();
            }
            catch (Exception ex)
            {
                log.Error(string.Format("GetCaseByFxtCityIdAndCaseIds(int fxtCityId:{0}, int[] fxtCaseIds)", fxtCityId), ex);
                fxtApi.Abort();
            }
            return list;
        }
        /// <summary>
        /// 根据房讯通城市ID and 案例ID获取案例信息
        /// </summary>
        /// <param name="fxtCityId"></param>
        /// <param name="fxtCaseId"></param>
        /// <returns></returns>
        public static FxtApi_DATCase GetCaseByFxtCityIdAndCaseId(int fxtCityId, int fxtCaseId, FxtAPIClientExtend _fxtApi = null)
        {
            FxtApi_DATCase _case = null;
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            try
            {
                JObject jObjPara = new JObject();
                jObjPara.Add(new JProperty("cityId", fxtCityId));
                jObjPara.Add(new JProperty("caseId", fxtCaseId));
                string methodName = "GetCaseByCityIdAndCaseId";
                string jsonStr = Convert.ToString(EntranceApi.Entrance(methodName, jObjPara.ToJSONjss(), _fxtApi: fxtApi));
                if (string.IsNullOrEmpty(jsonStr))
                {
                    fxtApi.Abort();
                    return _case;
                }
                _case = FxtApi_DATCase.ConvertToObj(jsonStr);
                _case = _case.DecodeField();
                fxtApi.Abort();
            }
            catch (Exception ex)
            {
                log.Error(string.Format("GetCaseByFxtCityIdAndCaseId(int fxtCityId:{0}, int fxtCaseId:{1}, FxtAPIClientExtend _fxtApi = null)", fxtCityId,fxtCaseId), ex);
                fxtApi.Abort();
            }
            return _case;
        }

        /// <summary>
        /// 根据房讯通城市ID and 多个楼盘Ids获取案例Id+楼盘名
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="fxtCaseIds"></param>
        /// <returns></returns>
        public static Dictionary<int, string> GetCaseIdJoinProjectNameByFxtCityIdAndCaseIds(int fxtCityId, int[] fxtCaseIds, FxtAPIClientExtend _fxtApi = null)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            try
            {
                if (fxtCaseIds == null || fxtCaseIds.Length < 1)
                {
                    return dic;
                }
                string fxtCaseIdsStr = fxtCaseIds.ConvertToString();


                string name = "GetCaseIdJoinProjectNameByCityIdAndCaseIds";
                var para = new { cityId = fxtCityId, caseIds = fxtCaseIdsStr };
                string jsonStr = Convert.ToString(EntranceApi.Entrance(name, para.ToJSONjss(), _fxtApi: fxtApi));
                fxtApi.Abort();
                if (string.IsNullOrEmpty(jsonStr))
                {
                    return dic;
                }
                JArray jarray = JArray.Parse(jsonStr);
                foreach (JObject jobject in jarray)
                {
                    int fxtCaseId = jobject["CaseId"].Value<int>();
                    string projectName = jobject["ProjectName"].Value<string>();
                    projectName = projectName.DecodeField();
                    if (!dic.ContainsKey(fxtCaseId))
                    {
                        dic.Add(fxtCaseId, projectName);
                    }
                }
            }
            catch (Exception ex)
            {
                fxtApi.Abort();
                log.Error(string.Format("GetCaseIdJoinProjectNameByFxtCityIdAndCaseIds(int fxtCityId:{0}, int[] fxtCaseIds)", fxtCityId), ex);
            }
            return dic;
        }
        /// <summary>
        /// 根据城市名称 and 多个楼盘Ids获取案例Id+楼盘名
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="fxtCaseIds"></param>
        /// <returns></returns>
        public static Dictionary<int, string> GetCaseIdJoinProjectNameByCityNameAndCaseIds(string cityName, int[] fxtCaseIds, FxtAPIClientExtend _fxtApi = null)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            try
            {

                string name = "GetCityByCityName";
                var para = new { cityName = cityName };
                string jsonStr = Convert.ToString(EntranceApi.Entrance(name, para.ToJSONjss(), _fxtApi: fxtApi));

                if (string.IsNullOrEmpty(jsonStr))
                {
                    fxtApi.Abort();
                    return dic;
                }
                FxtApi_SYSCity city = FxtApi_SYSCity.ConvertToObj(jsonStr);
                if (city == null)
                {
                    fxtApi.Abort();
                    return dic;
                }
                int cityId = city.CityId;
                dic = GetCaseIdJoinProjectNameByFxtCityIdAndCaseIds(cityId, fxtCaseIds,_fxtApi:fxtApi);
                fxtApi.Abort();
            }
            catch (Exception ex)
            {
                fxtApi.Abort();
                log.Error(string.Format("GetCaseIdJoinProjectNameByCityNameAndCaseIds(string cityName:{0}, int[] fxtCaseIds)", cityName == null ? "null" : ""), ex);
            }
            return dic;
        }
        /// <summary>
        /// 查询案例
        /// </summary>
        /// <param name="fxtCityId"></param>
        /// <param name="projectId"></param>
        /// <param name="buildingTypeCode"></param>
        /// <param name="purposeCode">用途</param>
        /// <param name="buildingAreaCode">面积段code</param>
        /// <param name="startDate">案例时间</param>
        /// <param name="endDate"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <param name="isGetCount">是否获取总个数</param>
        /// <returns></returns>
        public static List<FxtApi_DATCase> GetCaseByFxtCityIdAndFxtProjectIdAndBuildingTypeCodeAndPurposeCodeAndAreaTypeAndDate(int fxtCityId, int projectId,string fxtCompanyIds, int? buildingTypeCode, int purposeCode, int? buildingAreaCode, DateTime? startDate, DateTime? endDate, int pageIndex, int pageSize, out int count, bool isGetCount = true, FxtAPIClientExtend _fxtApi = null)
        {
            count = 0;
            int isgetcount = 1;
            if (!isGetCount)
            {
                isgetcount = 0;
            }
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);


            string name = "GetCaseByCityIdAndProjectIdAndPurposeCodeAndBuildingTypeCodeAndAreaTypeAndDateAndPage";
            var para = new
            {
                cityId = fxtCityId,
                projectId = projectId,
                fxtCompanyIds = fxtCompanyIds,
                buildingTypeCode = buildingTypeCode,
                purposeCode = purposeCode,
                buildingAreaCode = buildingAreaCode,
                startDate = Convert.ToString(startDate),
                endDate = Convert.ToString(endDate),
                pageIndex = pageIndex,
                pageSize = pageSize,
                isGetCount=isgetcount
            };
            string jsonStr = Convert.ToString(EntranceApi.Entrance(name, para.ToJSONjss(), _fxtApi: fxtApi));

            if (string.IsNullOrEmpty(jsonStr))
            {
                fxtApi.Abort();
                return new List<FxtApi_DATCase>();
            }
            FxtApi_ResultPageList listObj = FxtApi_ResultPageList.ConvertToObj(jsonStr);
            count = listObj.Count;
            List<FxtApi_DATCase> list = FxtApi_DATCase.ConvertToObjList(listObj.ObjJson);
            list = list.DecodeField();
            fxtApi.Abort();
            return list;
        }
        /// <summary>
        /// 获取楼盘中相关用途下的案例个数
        /// </summary>
        /// <param name="fxtCityId"></param>
        /// <param name="projectId"></param>
        /// <param name="dates"></param>
        /// <param name="_fxtApi"></param>
        /// <returns></returns>
        public static List<FxtApi_ProjectJoinPurposeTypeJoinCaseCount> GetCaseCountJoinProjectJoinPurposeTypeByCityIdAndProjectIdAndDates(int fxtCityId, int projectId,string fxtCompanyIds, string[] dates, FxtAPIClientExtend _fxtApi = null)
        {
            List<FxtApi_ProjectJoinPurposeTypeJoinCaseCount> objList = new List<FxtApi_ProjectJoinPurposeTypeJoinCaseCount>();
            string _dates = dates.ConvertToString();
            if (string.IsNullOrEmpty(_dates))
            {
                return objList;
            }
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);


            string name = "GetCaseCountJoinProjectJoinPurposeTypeByCityIdAndProjectIdAndDates";
            var para = new { cityId = fxtCityId, projectId = projectId,fxtCompanyIds=fxtCompanyIds, dates = _dates };
            string jsonStr = Convert.ToString(EntranceApi.Entrance(name, para.ToJSONjss(), _fxtApi: fxtApi));

            if (string.IsNullOrEmpty(jsonStr))
            {
                return objList;
            }
            objList = FxtApi_ProjectJoinPurposeTypeJoinCaseCount.ConvertToObjList(jsonStr);
            fxtApi.Abort();
            return objList;
        }
        /// <summary>
        /// 获取指定日期楼盘下普通住宅的案例个数(各建筑面积,建筑类型下的案例个数)
        /// </summary>
        /// <param name="fxtCityId"></param>
        /// <param name="projectId"></param>
        /// <param name="date"></param>
        /// <param name="_fxtApi"></param>
        /// <returns></returns>
        public static List<FxtApi_ProjectJoinBuildingTypeJoinAreaTypeJoinPublicCaseCount> GetCaseCountJoinProjectJoinBuildingTypeJoinAreaTypeByPublicPurposeAndCityIdAndProjectIdAndDate(int fxtCityId, int projectId,string fxtCompanyIds, string date, FxtAPIClientExtend _fxtApi = null)
        {
            List<FxtApi_ProjectJoinBuildingTypeJoinAreaTypeJoinPublicCaseCount> objList = new List<FxtApi_ProjectJoinBuildingTypeJoinAreaTypeJoinPublicCaseCount>();
         
            if (string.IsNullOrEmpty(date))
            {
                return objList;
            }
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);


            string name = "GetCaseCountJoinProjectJoinBuildingTypeJoinAreaTypeByPublicPurposeAndCityIdAndProjectIdAndDate";
            var para = new { cityId = fxtCityId, projectId = projectId,fxtCompanyIds=fxtCompanyIds, date = date };
            string jsonStr = Convert.ToString(EntranceApi.Entrance(name, para.ToJSONjss(), _fxtApi: fxtApi));

            if (string.IsNullOrEmpty(jsonStr))
            {
                return objList;
            }
            objList = FxtApi_ProjectJoinBuildingTypeJoinAreaTypeJoinPublicCaseCount.ConvertToObjList(jsonStr);
            fxtApi.Abort();
            return objList;
        }
        /// <summary>
        /// 获取指定日期楼盘下各别墅用途的案例个数
        /// </summary>
        /// <param name="fxtCityId"></param>
        /// <param name="projectId"></param>
        /// <param name="date"></param>
        /// <param name="_fxtApi"></param>
        /// <returns></returns>
        public static List<FxtApi_ProjectJoinPurposeTypeJoinVillaCaseCount> GetCaseCountJoinProjectJoinPurposeTypeByVillaPurposeAndCityIdAndProjectIdAndDate(int fxtCityId, int projectId,string fxtCompanyIds, string date, FxtAPIClientExtend _fxtApi = null)
        {
            List<FxtApi_ProjectJoinPurposeTypeJoinVillaCaseCount> objList = new List<FxtApi_ProjectJoinPurposeTypeJoinVillaCaseCount>();

            if (string.IsNullOrEmpty(date))
            {
                return objList;
            }
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);


            string name = "GetCaseCountJoinProjectJoinPurposeTypeByVillaPurposeAndCityIdAndProjectIdAndDate";
            var para = new { cityId = fxtCityId, projectId = projectId,fxtCompanyIds=fxtCompanyIds, date = date };
            string jsonStr = Convert.ToString(EntranceApi.Entrance(name, para.ToJSONjss(), _fxtApi: fxtApi));

            if (string.IsNullOrEmpty(jsonStr))
            {
                return objList;
            }
            objList = FxtApi_ProjectJoinPurposeTypeJoinVillaCaseCount.ConvertToObjList(jsonStr);
            fxtApi.Abort();
            return objList;
        }
        #endregion
    }
}
