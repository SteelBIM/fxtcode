using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FxtSpider.Manager.Common;
using System.Net.Http;
using Newtonsoft.Json;
using FxtSpider.DAL.LinqToSql;
using FxtSpider.Bll;
using FxtSpider.Common;
using System.Text;
using System.IO;
using FxtSpider.Manager.Web.Models;
using FxtSpider.DAL.LinqToSqlModels;
using Newtonsoft.Json.Linq;
using FxtSpider.Manager.Web.Common;
using System.Web.Caching;
using FxtSpider.FxtApi.Model;
using FxtSpider.FxtApi.ApiManager;
using System.Linq.Expressions;
using FxtSpider.Bll.FxtApiManager;
using System.Reflection;
using FxtSpider.FxtApi.FxtApiClientManager;

namespace FxtSpider.Manager.Web.Controllers
{
    public class OperationMaintenanceController : BaseController
    {
        //
        // GET: /OperationMaintenance/

        public ActionResult Index()
        {
            return View();
        }

        object valueType(Type t, object value)
        {
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
                    value = Convert.ToDecimal(value);
                    break;
                case "Int32":
                    value = Convert.ToInt32(value);
                    break;
                case "Int64":
                    value = Convert.ToInt64(value);
                    break;
                case "Float":
                    value = float.Parse(Convert.ToString(value));
                    break;
                case "DateTime":
                    value = Convert.ToDateTime(value);
                    break;
                default:
                    value = Convert.ToString(value);
                    break;
            }
            return value;
        }

        #region (CasePriceAnalyse.cshtml)

        public ActionResult CasePriceAnalyse()
        {
            //JObject jobjtest = new JObject();
            //jobjtest.Add(new JProperty("abc", "ff"));
            //JProperty prop = jobjtest.Property("abc");
            //JProperty prop2 = jobjtest.Property("bcd");

            //string test = "{\"count\":12,\"list\":[{\"id\":1,\"name\":\"辅导费\"},{\"id\":2,\"name\":\"辅导费\"},{\"id\":3,\"name\":\"辅导费\"}]}";
            //string test2 = "[{\"id\":1,\"name\":\"辅导费\"},{\"id\":2,\"name\":\"辅导费\"},{\"id\":3,\"name\":\"辅导费\"}]";
            //string test3 = "{\"Result\":1,\"Message\":\"似懂非懂\",\"Detail\":\"地方法\"}";
            //FxtApi_Result testobj = FxtApi_Result.ConvertToObj(test3);
            ////JArray jarray = JArray.Parse(test);
            //JObject jobj = JObject.Parse(test);
            //int str = jobj["count"].Value<int>();
            //string jArray = Convert.ToString(jobj["list"].Value<object>());
            //FxtApi_Result testobj2 = new FxtApi_Result();
            //JObject jObject = JObject.Parse(test3);
            //foreach (var _jobj in jObject)
            //{
            //    string key = _jobj.Key;
            //    object value = _jobj.Value.Value<JValue>().Value;
            //    var propertyObj = testobj2.GetType()
            //             .GetProperties()
            //             .Where(pInfo =>
            //                 pInfo.Name.Equals(key)).FirstOrDefault();

            //    value = valueType(propertyObj.PropertyType, value);
            //    propertyObj.SetValue(testobj2, value, null);
            //}
            ////ProjectName,Month1_Type1,Month1_Type2,Month2_Type1,Month2_Type2,
            ViewBag.NowDate = DateTime.Now.ToString("yyyy-MM-dd");
            return View();
        }
        /// <summary>
        /// 根据城市名称获取楼盘列表
        /// </summary>
        /// <param name="cityName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [FxtSpider.Manager.Common.AuthorizeAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult CasePriceAnalyse_GetProjectListByCityName_Api(string cityName, int pageIndex, int pageSize, string isGetCount)
        {
            int count = 0;
            bool isGetCount2 = false;
            //是否获取总个数
            if (!string.IsNullOrEmpty(isGetCount) && isGetCount.Equals("1"))
            {
                isGetCount2 = true;
            }
            List<FxtApi_DATProject> list = new List<FxtApi_DATProject>();
            cityName = cityName.DecodeField();
            if (!string.IsNullOrEmpty(cityName))
            {
                list = ProjectApi.GetProjectByCityName(cityName, pageIndex, pageSize, out count, isGetCount: isGetCount2);
            }
            string listJson = list == null ? "null" : list.EncodeField<FxtApi_DATProject>().ToJSONjss();
            string resultJson = new StringBuilder()
                             .Append("{\"List\":").Append(string.IsNullOrEmpty(listJson) ? "null" : listJson)
                             .Append(",\"Count\":").Append(count)
                             .Append(",\"FxtCityId\":").Append(list != null && list.Count > 0 ? list[0].CityID : 0)
                             .Append("}").ToString();
            resultJson = resultJson.MvcResponseJson();
            Response.Write(resultJson);
            Response.End();
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="monthCount"></param>
        /// <returns></returns>
        [FxtSpider.Manager.Common.AuthorizeAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult CasePriceAnalyse_GetPriceByMonthsAndProjectId_Api(string dates, int projectId, int fxtCityId,string fxtCompanyIds)
        {
            string json = "{{\"FxtCompanyIds\":\"{0}\",\"FxtCityId\":\"" + fxtCityId + "\",\"Obj\":{1}}}";
            JObject jObj = new JObject();
            jObj.Add(new JProperty("ProjectId", projectId));
            string[] _dates = string.IsNullOrEmpty(dates) ? null : dates.Split(',');
            if (_dates == null)
            {
                json = string.Format(json, "", JsonConvert.SerializeObject(jObj));
                Response.Write(json.MvcResponseJson());
                Response.End();
                return null;
            }
            if (string.IsNullOrEmpty(fxtCompanyIds))
            {
                fxtCompanyIds = PriviCompanyShowDataApi.GetFxtPriviCompanyShowDataCaseCompanyIds(fxtCityId);
            }
            int[] ints = new int[] { projectId };
            List<JObject> list = GetCross(dates, ints, fxtCityId, fxtCompanyIds);
            if (list != null && list.Count > 0)
            {
                jObj = list[0];
            }

            //输出数据
            json = string.Format(json, fxtCompanyIds, JsonHelp.ToJSONjss(jObj));
            Response.Write(json.MvcResponseJson());
            Response.End();
            return null;
        }
        [FxtSpider.Manager.Common.AuthorizeAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult CasePriceAnalyse_GetPriceByMonthsAndProjectIds_Api(string dates, string projectIds, int fxtCityId, string fxtCompanyIds)
        {

            string json = "{{\"FxtCompanyIds\":\"{0}\",\"FxtCityId\":\"" + fxtCityId + "\",\"List\":{1}}}";
            List<JObject> jObjList = new List<JObject>();
            string[] _dates = string.IsNullOrEmpty(dates) ? null : dates.Split(',');
            if (_dates == null || string.IsNullOrEmpty(projectIds))
            {
                json = string.Format(json, "", JsonConvert.SerializeObject(jObjList));
                Response.Write(json.MvcResponseJson());
                Response.End();
                return null;
            }
            int[] ints = projectIds.Split(',').ConvertToInts();
            if (string.IsNullOrEmpty(fxtCompanyIds))
            {
                fxtCompanyIds = PriviCompanyShowDataApi.GetFxtPriviCompanyShowDataCaseCompanyIds(fxtCityId);
            }
            jObjList = GetCross(dates, ints, fxtCityId, fxtCompanyIds);
            //输出数据
            json = string.Format(json, fxtCompanyIds, jObjList.ToJSONjss());
            Response.Write(json.MvcResponseJson());
            Response.End();
            return null;
        }
        List<JObject> GetCross(string dates, int[] projectIds, int fxtCityId, string fxtCompanyIds)
        {
            List<JObject> jObjList = new List<JObject>();
            string[] _dates = string.IsNullOrEmpty(dates) ? null : dates.Split(',');
            if (_dates == null)
            {
                return jObjList;
            }
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend();
            List<FxtApi_DATProjectAvgPrice> dataList = ProjectAvgPriceApi.GetCross(projectIds, fxtCityId, null, _dates, _fxtApi: fxtApi);

            foreach (int projectId in projectIds)
            {
                JObject jObj = new JObject();
                jObj.Add(new JProperty("ProjectId", projectId));
                jObj.Add(new JProperty("CityID", fxtCityId));
                List<FxtApi_DATProjectAvgPrice> avgPriceList = dataList.Where(p => p.ProjectId == projectId).ToList(); //ProjectAvgPriceApi.GetCross(projectId, fxtCityId, null, _dates, _fxtApi: fxtApi);
                List<FxtApi_ProjectJoinPurposeTypeJoinCaseCount> countList = CaseApi.GetCaseCountJoinProjectJoinPurposeTypeByCityIdAndProjectIdAndDates(fxtCityId, projectId,fxtCompanyIds, _dates, _fxtApi: fxtApi);
                foreach (string date in _dates)
                {
                    //*************普通住宅************//
                    //均价
                    List<FxtApi_DATProjectAvgPrice> publicList = avgPriceList.Where(tbl => tbl.AvgPrice > 0 && tbl.PurposeType == SysCodeApi.Code1 && tbl.AvgPriceDate == date.Replace("-", "")).ToList();
                    int sumPrice = publicList.Sum(tbl => tbl.AvgPrice); int sumCount = publicList.Count;
                    int publicAvgPrice = sumCount == 0 ? 0 : sumPrice / sumCount;
                    jObj.Add(new JProperty("PublicAvgPrice_" + date, publicAvgPrice));
                    //*************别墅**************//
                    //均价
                    List<FxtApi_DATProjectAvgPrice> villaList = avgPriceList.Where(tbl => tbl.AvgPrice > 0 && tbl.PurposeType != SysCodeApi.Code1 && tbl.AvgPriceDate == date.Replace("-", "")).ToList();
                    int sumPrice2 = villaList.Sum(tbl => tbl.AvgPrice); int sumCount2 = villaList.Count;
                    int villaAvgPrice = sumCount2 == 0 ? 0 : sumPrice2 / sumCount2;
                    jObj.Add(new JProperty("VillaAvgPrice_" + date, villaAvgPrice));

                    //总量
                    FxtApi_ProjectJoinPurposeTypeJoinCaseCount countObj = countList.Where(tbl => tbl.Date == date).FirstOrDefault();
                    if (countObj != null)
                    {
                        jObj.Add(new JProperty("PublicCount_" + date, countObj.PurposePublicCount));
                        jObj.Add(new JProperty("VillaCount_" + date, countObj.PurposeVillaCount));
                    }
                }
                //计算张跌幅
                foreach (string date in _dates)
                {
                    string lastDate = CommonHelp.GetDateTimeMoths(date, 0, "yyyy-MM");

                    //普通住宅
                    double floatValue1 = 1;
                    int publicAvgPrice = jObj["PublicAvgPrice_" + date].Value<int>();
                    if (jObj.Property("PublicAvgPrice_" + lastDate) != null)
                    {
                        int lastAvgPrice = jObj["PublicAvgPrice_" + lastDate].Value<int>();

                        if (lastAvgPrice > 0)
                        {
                            floatValue1 = (Convert.ToDouble(publicAvgPrice) / Convert.ToDouble(lastAvgPrice)) - 1;
                            floatValue1 = Convert.ToDouble(Decimal.Round(Convert.ToDecimal(floatValue1), 4));
                        }
                        else if (publicAvgPrice <= 0)
                        {
                            floatValue1 = 0;
                        }
                    }
                    jObj.Add(new JProperty("PublicFloat_" + date, floatValue1));
                    //别墅
                    double floatValue2 = 1;
                    int villaAvgPrice = jObj["VillaAvgPrice_" + date].Value<int>();
                    if (jObj.Property("VillaAvgPrice_" + lastDate) != null)
                    {
                        int lastAvgPrice = jObj["VillaAvgPrice_" + lastDate].Value<int>();

                        if (lastAvgPrice > 0)
                        {
                            floatValue2 = (Convert.ToDouble(villaAvgPrice) / Convert.ToDouble(lastAvgPrice)) - 1;
                            floatValue2 = Convert.ToDouble(Decimal.Round(Convert.ToDecimal(floatValue1), 4));
                        }
                        else if (villaAvgPrice <= 0)
                        {
                            floatValue2 = 0;
                        }
                    }
                    jObj.Add(new JProperty("VillaFloat_" + date, floatValue2));

                }
                jObjList.Add(jObj);
            }
            fxtApi.Abort();
            return jObjList;
        }
        /// <summary>
        /// 普通住宅交叉值均价
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="fxtCityId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [FxtSpider.Manager.Common.AuthorizeAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult CasePriceAnalyse_GetPublicCrossAvgPriceByProjectIdAndDate_Api(int projectId, int fxtCityId, string fxtCompanyIds, string date)
        {
            string json = "{{\"FxtCompanyIds\":\"{0}\",\"FxtCityId\":\"" + fxtCityId + "\",\"List\":{1}}}";
            List<JObject> jObjList = new List<JObject>();
            if (string.IsNullOrEmpty(date))
            {
                json = string.Format(json, "", JsonConvert.SerializeObject(jObjList));
                Response.Write(json.MvcResponseJson());
                Response.End();
                return null;
            }
            string lastDate = Convert.ToDateTime(date).AddMonths(-1).ToString("yyyy-MM");
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend();
            if (string.IsNullOrEmpty(fxtCompanyIds))
            {
                fxtCompanyIds = PriviCompanyShowDataApi.GetFxtPriviCompanyShowDataCaseCompanyIds(fxtCityId,_fxtApi:fxtApi);
            }
            List<FxtApi_DATProjectAvgPrice> list = ProjectAvgPriceApi.GetCross(new int[] { projectId }, fxtCityId, new int[] { SysCodeApi.Code1 }, new string[] { lastDate, date }, _fxtApi: fxtApi);
            List<FxtApi_ProjectJoinBuildingTypeJoinAreaTypeJoinPublicCaseCount> countList = CaseApi.GetCaseCountJoinProjectJoinBuildingTypeJoinAreaTypeByPublicPurposeAndCityIdAndProjectIdAndDate(fxtCityId, projectId,fxtCompanyIds, date, _fxtApi: fxtApi);
            fxtApi.Abort();
            List<FxtApi_SYSCode> codeList = SysCodeApi.BuildingTypeCodeList;
            foreach (FxtApi_SYSCode code in codeList)
            {
                JObject jObj = new JObject();
                jObj.Add(new JProperty("ProjectId", projectId));
                jObj.Add(new JProperty("CityId", fxtCityId));
                jObj.Add(new JProperty("Date", date));
                jObj.Add(new JProperty("BuildingTypeCode", code.Code));
                jObj.Add(new JProperty("BuildingTypeCodeName", code.CodeName));
                jObj.Add(new JProperty("PurposeCode", SysCodeApi.Code1));
                #region 个数
                int areaCount1 = 0;
                FxtApi_ProjectJoinBuildingTypeJoinAreaTypeJoinPublicCaseCount countObj1 = countList.Where(p => p.BuildingTypeCode == code.Code && p.BuildingAreaTypeCode == SysCodeApi.Code3).FirstOrDefault();
                if (countObj1 != null)
                {
                    areaCount1 = countObj1.Count;
                }
                int areaCount2 = 0;
                FxtApi_ProjectJoinBuildingTypeJoinAreaTypeJoinPublicCaseCount countObj2 = countList.Where(p => p.BuildingTypeCode == code.Code && p.BuildingAreaTypeCode == SysCodeApi.Code4).FirstOrDefault();
                if (countObj2 != null)
                {
                    areaCount2 = countObj2.Count;
                }
                int areaCount3 = 0;
                FxtApi_ProjectJoinBuildingTypeJoinAreaTypeJoinPublicCaseCount countObj3 = countList.Where(p => p.BuildingTypeCode == code.Code && p.BuildingAreaTypeCode == SysCodeApi.Code5).FirstOrDefault();
                if (countObj3 != null)
                {
                    areaCount3 = countObj3.Count;
                }
                int areaCount4 = 0;
                FxtApi_ProjectJoinBuildingTypeJoinAreaTypeJoinPublicCaseCount countObj4 = countList.Where(p => p.BuildingTypeCode == code.Code && p.BuildingAreaTypeCode == SysCodeApi.Code6).FirstOrDefault();
                if (countObj4 != null)
                {
                    areaCount4 = countObj4.Count;
                }
                int areaCount5 = 0;
                FxtApi_ProjectJoinBuildingTypeJoinAreaTypeJoinPublicCaseCount countObj5 = countList.Where(p => p.BuildingTypeCode == code.Code && p.BuildingAreaTypeCode == SysCodeApi.Code7).FirstOrDefault();
                if (countObj5 != null)
                {
                    areaCount5 = countObj5.Count;
                }
                jObj.Add(new JProperty("Area1Count", areaCount1));
                jObj.Add(new JProperty("Area1Code", SysCodeApi.Code3));
                jObj.Add(new JProperty("Area2Count", areaCount2));
                jObj.Add(new JProperty("Area2Code", SysCodeApi.Code4));
                jObj.Add(new JProperty("Area3Count", areaCount3));
                jObj.Add(new JProperty("Area3Code", SysCodeApi.Code5));
                jObj.Add(new JProperty("Area4Count", areaCount4));
                jObj.Add(new JProperty("Area4Code", SysCodeApi.Code6));
                jObj.Add(new JProperty("Area5Count", areaCount5));
                jObj.Add(new JProperty("Area5Code", SysCodeApi.Code7));

                #endregion

                #region 这个月均价
                int areaAvgPrice1 = 0;
                FxtApi_DATProjectAvgPrice avgPrice1 = list.Where(p => p.BuildingTypeCode == code.Code && p.BuildingAreaType == SysCodeApi.Code3 && p.AvgPriceDate == date.Replace("-", "")).FirstOrDefault();
                if (avgPrice1 != null)
                {
                    areaAvgPrice1 = avgPrice1.AvgPrice;
                }
                int areaAvgPrice2 = 0;
                FxtApi_DATProjectAvgPrice avgPrice2 = list.Where(p => p.BuildingTypeCode == code.Code && p.BuildingAreaType == SysCodeApi.Code4 && p.AvgPriceDate == date.Replace("-", "")).FirstOrDefault();
                if (avgPrice2 != null)
                {
                    areaAvgPrice2 = avgPrice2.AvgPrice;
                }
                int areaAvgPrice3 = 0;
                FxtApi_DATProjectAvgPrice avgPrice3 = list.Where(p => p.BuildingTypeCode == code.Code && p.BuildingAreaType == SysCodeApi.Code5 && p.AvgPriceDate == date.Replace("-", "")).FirstOrDefault();
                if (avgPrice3 != null)
                {
                    areaAvgPrice3 = avgPrice3.AvgPrice;
                }
                int areaAvgPrice4 = 0;
                FxtApi_DATProjectAvgPrice avgPrice4 = list.Where(p => p.BuildingTypeCode == code.Code && p.BuildingAreaType == SysCodeApi.Code6 && p.AvgPriceDate == date.Replace("-", "")).FirstOrDefault();
                if (avgPrice4 != null)
                {
                    areaAvgPrice4 = avgPrice4.AvgPrice;
                }
                int areaAvgPrice5 = 0;
                FxtApi_DATProjectAvgPrice avgPrice5 = list.Where(p => p.BuildingTypeCode == code.Code && p.BuildingAreaType == SysCodeApi.Code7 && p.AvgPriceDate == date.Replace("-", "")).FirstOrDefault();
                if (avgPrice5 != null)
                {
                    areaAvgPrice5 = avgPrice5.AvgPrice;
                }
                jObj.Add(new JProperty("Area1AvgPrice", areaAvgPrice1));
                jObj.Add(new JProperty("Area2AvgPrice", areaAvgPrice2));
                jObj.Add(new JProperty("Area3AvgPrice", areaAvgPrice3));
                jObj.Add(new JProperty("Area4AvgPrice", areaAvgPrice4));
                jObj.Add(new JProperty("Area5AvgPrice", areaAvgPrice5));
                #endregion

                #region 浮动值

                double areaFloatValue1 = 1;
                FxtApi_DATProjectAvgPrice last_avgPrice1 = list.Where(p => p.BuildingTypeCode == code.Code && p.BuildingAreaType == SysCodeApi.Code3 && p.AvgPriceDate == lastDate.Replace("-", "")).FirstOrDefault();
                if (last_avgPrice1 != null)
                {
                    if (last_avgPrice1.AvgPrice > 0)
                    {
                        areaFloatValue1 = (Convert.ToDouble(areaAvgPrice1) / Convert.ToDouble(last_avgPrice1.AvgPrice)) - 1;
                        areaFloatValue1 = Convert.ToDouble(Decimal.Round(Convert.ToDecimal(areaFloatValue1), 4));
                    }
                    else if (areaAvgPrice1 <= 0)
                    {
                        areaFloatValue1 = 0;
                    }
                }
                double areaFloatValue2 = 1;
                FxtApi_DATProjectAvgPrice last_avgPrice2 = list.Where(p => p.BuildingTypeCode == code.Code && p.BuildingAreaType == SysCodeApi.Code4 && p.AvgPriceDate == lastDate.Replace("-", "")).FirstOrDefault();
                if (last_avgPrice2 != null)
                {
                    if (last_avgPrice2.AvgPrice > 0)
                    {
                        areaFloatValue2 = (Convert.ToDouble(areaAvgPrice2) / Convert.ToDouble(last_avgPrice2.AvgPrice)) - 1;
                        areaFloatValue2 = Convert.ToDouble(Decimal.Round(Convert.ToDecimal(areaFloatValue2), 4));
                    }
                    else if (areaAvgPrice2 <= 0)
                    {
                        areaFloatValue2 = 0;
                    }
                }
                double areaFloatValue3 = 1;
                FxtApi_DATProjectAvgPrice last_avgPrice3 = list.Where(p => p.BuildingTypeCode == code.Code && p.BuildingAreaType == SysCodeApi.Code5 && p.AvgPriceDate == lastDate.Replace("-", "")).FirstOrDefault();
                if (last_avgPrice3 != null)
                {
                    if (last_avgPrice3.AvgPrice > 0)
                    {
                        areaFloatValue3 = (Convert.ToDouble(areaAvgPrice3) / Convert.ToDouble(last_avgPrice3.AvgPrice)) - 1;
                        areaFloatValue3 = Convert.ToDouble(Decimal.Round(Convert.ToDecimal(areaFloatValue3), 4));
                    }
                    else if (areaAvgPrice3 <= 0)
                    {
                        areaFloatValue3 = 0;
                    }
                }
                double areaFloatValue4 = 1;
                FxtApi_DATProjectAvgPrice last_avgPrice4 = list.Where(p => p.BuildingTypeCode == code.Code && p.BuildingAreaType == SysCodeApi.Code6 && p.AvgPriceDate == lastDate.Replace("-", "")).FirstOrDefault();
                if (last_avgPrice4 != null)
                {
                    if (last_avgPrice4.AvgPrice > 0)
                    {
                        areaFloatValue4 = (Convert.ToDouble(areaAvgPrice4) / Convert.ToDouble(last_avgPrice4.AvgPrice)) - 1;
                        areaFloatValue4 = Convert.ToDouble(Decimal.Round(Convert.ToDecimal(areaFloatValue4), 4));
                    }
                    else if (areaAvgPrice4 <= 0)
                    {
                        areaFloatValue4 = 0;
                    }
                }
                double areaFloatValue5 = 1;
                FxtApi_DATProjectAvgPrice last_avgPrice5 = list.Where(p => p.BuildingTypeCode == code.Code && p.BuildingAreaType == SysCodeApi.Code7 && p.AvgPriceDate == lastDate.Replace("-", "")).FirstOrDefault();
                if (last_avgPrice5 != null)
                {
                    if (last_avgPrice5.AvgPrice > 0)
                    {
                        areaFloatValue5 = (Convert.ToDouble(areaAvgPrice5) / Convert.ToDouble(last_avgPrice5.AvgPrice)) - 1;
                        areaFloatValue5 = Convert.ToDouble(Decimal.Round(Convert.ToDecimal(areaFloatValue5), 4));
                    }
                    else if (areaAvgPrice5 <= 0)
                    {
                        areaFloatValue5 = 0;
                    }
                }
                jObj.Add(new JProperty("Area1FloatValue", areaFloatValue1));
                jObj.Add(new JProperty("Area2FloatValue", areaFloatValue2));
                jObj.Add(new JProperty("Area3FloatValue", areaFloatValue3));
                jObj.Add(new JProperty("Area4FloatValue", areaFloatValue4));
                jObj.Add(new JProperty("Area5FloatValue", areaFloatValue5));

                #endregion

                jObjList.Add(jObj);
            }

            //输出数据
            json = string.Format(json, fxtCompanyIds, jObjList.ToJSONjss());
            Response.Write(json.MvcResponseJson());
            Response.End();
            return null;
        }
        /// <summary>
        /// 别墅交叉值均价
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="fxtCityId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [FxtSpider.Manager.Common.AuthorizeAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult CasePriceAnalyse_GetVillaCrossAvgPriceByProjectIdAndDate_Api(int projectId, int fxtCityId, string fxtCompanyIds, string date)
        {

            string json = "{{\"FxtCompanyIds\":\"{0}\",\"FxtCityId\":\"" + fxtCityId + "\",\"List\":{1}}}";
            List<JObject> jObjList = new List<JObject>();
            if (string.IsNullOrEmpty(date))
            {
                json = string.Format(json, "", JsonConvert.SerializeObject(jObjList));
                Response.Write(json.MvcResponseJson());
                Response.End();
                return null;
            }
            string lastDate = Convert.ToDateTime(date).AddMonths(-1).ToString("yyyy-MM");
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend();
            if (string.IsNullOrEmpty(fxtCompanyIds))
            {
                fxtCompanyIds = PriviCompanyShowDataApi.GetFxtPriviCompanyShowDataCaseCompanyIds(fxtCityId, _fxtApi: fxtApi);
            }
            List<FxtApi_DATProjectAvgPrice> list = ProjectAvgPriceApi.GetCross(new int[] { projectId }, fxtCityId, new int[] { SysCodeApi.Code2 }, new string[] { lastDate, date }, _fxtApi: fxtApi);
            List<FxtApi_ProjectJoinPurposeTypeJoinVillaCaseCount> countList = CaseApi.GetCaseCountJoinProjectJoinPurposeTypeByVillaPurposeAndCityIdAndProjectIdAndDate(fxtCityId, projectId,fxtCompanyIds, date, _fxtApi: fxtApi);
            List<FxtApi_SYSCode> codeList = SysCodeApi.BuildingTypeCodeList;
            fxtApi.Abort();
            JObject jObj = new JObject();
            jObj.Add(new JProperty("ProjectId", projectId));
            jObj.Add(new JProperty("CityId", fxtCityId));
            jObj.Add(new JProperty("Date", date));
            #region 个数
            int purposeCount1 = 0;//别墅
            FxtApi_ProjectJoinPurposeTypeJoinVillaCaseCount countObj1 = countList.Where(p => p.PurposeTypeCode == SysCodeApi.Code2).FirstOrDefault();
            if (countObj1 != null)
            {
                purposeCount1 = countObj1.Count;
            }
            int purposeCount2 = 0;//联排别墅
            FxtApi_ProjectJoinPurposeTypeJoinVillaCaseCount countObj2 = countList.Where(p => p.PurposeTypeCode == SysCodeApi.Code9).FirstOrDefault();
            if (countObj2 != null)
            {
                purposeCount2 = countObj2.Count;
            }
            int purposeCount3 = 0;//双拼别墅
            FxtApi_ProjectJoinPurposeTypeJoinVillaCaseCount countObj3 = countList.Where(p => p.PurposeTypeCode == SysCodeApi.Code11).FirstOrDefault();
            if (countObj3 != null)
            {
                purposeCount3 = countObj3.Count;
            }
            int purposeCount4 = 0;//独栋别墅
            FxtApi_ProjectJoinPurposeTypeJoinVillaCaseCount countObj4 = countList.Where(p => p.PurposeTypeCode == SysCodeApi.Code8).FirstOrDefault();
            if (countObj4 != null)
            {
                purposeCount4 = countObj4.Count;
            }
            int purposeCount5 = 0;//叠加别墅
            FxtApi_ProjectJoinPurposeTypeJoinVillaCaseCount countObj5 = countList.Where(p => p.PurposeTypeCode == SysCodeApi.Code10).FirstOrDefault();
            if (countObj5 != null)
            {
                purposeCount5 = countObj5.Count;
            }
            jObj.Add(new JProperty("Purpose1Count", purposeCount1));
            jObj.Add(new JProperty("Purpose2Count", purposeCount2));
            jObj.Add(new JProperty("Purpose3Count", purposeCount3));
            jObj.Add(new JProperty("Purpose4Count", purposeCount4));
            jObj.Add(new JProperty("Purpose5Count", purposeCount5));

            #endregion

            #region 这个月均价
            int purposeAvgPrice1 = 0;//别墅
            FxtApi_DATProjectAvgPrice avgPrice1 = list.Where(p => p.PurposeType == SysCodeApi.Code2 && p.AvgPriceDate == date.Replace("-", "")).FirstOrDefault();
            if (avgPrice1 != null)
            {
                purposeAvgPrice1 = avgPrice1.AvgPrice;
            }
            int purposeAvgPrice2 = 0;//联排别墅
            FxtApi_DATProjectAvgPrice avgPrice2 = list.Where(p => p.PurposeType == SysCodeApi.Code9 && p.AvgPriceDate == date.Replace("-", "")).FirstOrDefault();
            if (avgPrice2 != null)
            {
                purposeAvgPrice2 = avgPrice2.AvgPrice;
            }
            int purposeAvgPrice3 = 0;//双拼别墅
            FxtApi_DATProjectAvgPrice avgPrice3 = list.Where(p => p.PurposeType == SysCodeApi.Code11 && p.AvgPriceDate == date.Replace("-", "")).FirstOrDefault();
            if (avgPrice3 != null)
            {
                purposeAvgPrice3 = avgPrice3.AvgPrice;
            }
            int purposeAvgPrice4 = 0;//独栋别墅
            FxtApi_DATProjectAvgPrice avgPrice4 = list.Where(p => p.PurposeType == SysCodeApi.Code8 && p.AvgPriceDate == date.Replace("-", "")).FirstOrDefault();
            if (avgPrice4 != null)
            {
                purposeAvgPrice4 = avgPrice4.AvgPrice;
            }
            int purposeAvgPrice5 = 0;//叠加别墅
            FxtApi_DATProjectAvgPrice avgPrice5 = list.Where(p => p.PurposeType == SysCodeApi.Code10 && p.AvgPriceDate == date.Replace("-", "")).FirstOrDefault();
            if (avgPrice5 != null)
            {
                purposeAvgPrice5 = avgPrice5.AvgPrice;
            }
            jObj.Add(new JProperty("Purpose1AvgPrice", purposeAvgPrice1));
            jObj.Add(new JProperty("Purpose2AvgPrice", purposeAvgPrice2));
            jObj.Add(new JProperty("Purpose3AvgPrice", purposeAvgPrice3));
            jObj.Add(new JProperty("Purpose4AvgPrice", purposeAvgPrice4));
            jObj.Add(new JProperty("Purpose5AvgPrice", purposeAvgPrice5));
            #endregion

            #region 浮动值

            double purposeFloatValue1 = 1;//别墅
            FxtApi_DATProjectAvgPrice last_avgPrice1 = list.Where(p => p.PurposeType == SysCodeApi.Code2 && p.AvgPriceDate == lastDate.Replace("-", "")).FirstOrDefault();
            if (last_avgPrice1 != null)
            {
                if (last_avgPrice1.AvgPrice > 0)
                {
                    purposeFloatValue1 = (Convert.ToDouble(purposeAvgPrice1) / Convert.ToDouble(last_avgPrice1.AvgPrice)) - 1;
                    purposeFloatValue1 = Convert.ToDouble(Decimal.Round(Convert.ToDecimal(purposeFloatValue1), 4));
                }
                else if (purposeAvgPrice1 <= 0)
                {
                    purposeFloatValue1 = 0;
                }
            }
            double purposeFloatValue2 = 1;//联排别墅
            FxtApi_DATProjectAvgPrice last_avgPrice2 = list.Where(p => p.PurposeType == SysCodeApi.Code9 && p.AvgPriceDate == lastDate.Replace("-", "")).FirstOrDefault();
            if (last_avgPrice2 != null)
            {
                if (last_avgPrice2.AvgPrice > 0)
                {
                    purposeFloatValue2 = (Convert.ToDouble(purposeAvgPrice2) / Convert.ToDouble(last_avgPrice2.AvgPrice)) - 1;
                    purposeFloatValue2 = Convert.ToDouble(Decimal.Round(Convert.ToDecimal(purposeFloatValue2), 4));
                }
                else if (purposeAvgPrice2 <= 0)
                {
                    purposeFloatValue2 = 0;
                }
            }
            double purposeFloatValue3 = 1;//双拼别墅
            FxtApi_DATProjectAvgPrice last_avgPrice3 = list.Where(p => p.PurposeType == SysCodeApi.Code11 && p.AvgPriceDate == lastDate.Replace("-", "")).FirstOrDefault();
            if (last_avgPrice3 != null)
            {
                if (last_avgPrice3.AvgPrice > 0)
                {
                    purposeFloatValue3 = (Convert.ToDouble(purposeAvgPrice3) / Convert.ToDouble(last_avgPrice3.AvgPrice)) - 1;
                    purposeFloatValue3 = Convert.ToDouble(Decimal.Round(Convert.ToDecimal(purposeFloatValue3), 4));
                }
                else if (purposeAvgPrice3 <= 0)
                {
                    purposeFloatValue3 = 0;
                }
            }
            double purposeFloatValue4 = 1;//独栋别墅
            FxtApi_DATProjectAvgPrice last_avgPrice4 = list.Where(p => p.PurposeType == SysCodeApi.Code8 && p.AvgPriceDate == lastDate.Replace("-", "")).FirstOrDefault();
            if (last_avgPrice4 != null)
            {
                if (last_avgPrice4.AvgPrice > 0)
                {
                    purposeFloatValue4 = (Convert.ToDouble(purposeAvgPrice4) / Convert.ToDouble(last_avgPrice4.AvgPrice)) - 1;
                    purposeFloatValue4 = Convert.ToDouble(Decimal.Round(Convert.ToDecimal(purposeFloatValue4), 4));
                }
                else if (purposeAvgPrice4 <= 0)
                {
                    purposeFloatValue4 = 0;
                }
            }
            double purposeFloatValue5 = 1;//叠加别墅
            FxtApi_DATProjectAvgPrice last_avgPrice5 = list.Where(p => p.PurposeType == SysCodeApi.Code10 && p.AvgPriceDate == lastDate.Replace("-", "")).FirstOrDefault();
            if (last_avgPrice5 != null)
            {
                if (last_avgPrice5.AvgPrice > 0)
                {
                    purposeFloatValue5 = (Convert.ToDouble(purposeAvgPrice5) / Convert.ToDouble(last_avgPrice5.AvgPrice)) - 1;
                    purposeFloatValue5 = Convert.ToDouble(Decimal.Round(Convert.ToDecimal(purposeFloatValue5), 4));
                }
                else if (purposeAvgPrice5 <= 0)
                {
                    purposeFloatValue5 = 0;
                }
            }
            jObj.Add(new JProperty("Purpose1FloatValue", purposeFloatValue1));
            jObj.Add(new JProperty("Purpose2FloatValue", purposeFloatValue2));
            jObj.Add(new JProperty("Purpose3FloatValue", purposeFloatValue3));
            jObj.Add(new JProperty("Purpose4FloatValue", purposeFloatValue4));
            jObj.Add(new JProperty("Purpose5FloatValue", purposeFloatValue5));

            #endregion


            jObj.Add(new JProperty("Purpose1Code", SysCodeApi.Code2));
            jObj.Add(new JProperty("Purpose2Code", SysCodeApi.Code9));
            jObj.Add(new JProperty("Purpose3Code", SysCodeApi.Code11));
            jObj.Add(new JProperty("Purpose4Code", SysCodeApi.Code8));
            jObj.Add(new JProperty("Purpose5Code", SysCodeApi.Code10));


            jObjList.Add(jObj);
            //输出数据
            json = string.Format(json, fxtCompanyIds, jObjList.ToJSONjss());
            Response.Write(json.MvcResponseJson());
            Response.End();
            return null;
        }
        #endregion

        #region (CaseList.cshtml)
        /// <summary>
        /// 根据各交叉条件获取案例列表
        /// </summary>
        /// <param name="fxtCityId"></param>
        /// <param name="projectId"></param>
        /// <param name="buildingTypeCode"></param>
        /// <param name="purposeCode"></param>
        /// <param name="areaTypeCode"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [FxtSpider.Manager.Common.AuthorizeAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult CaseList(int fxtCityId, int projectId, int? buildingTypeCode, int purposeCode, int? areaTypeCode, string date)
        {
            ViewBag.FxtCityId = fxtCityId;
            ViewBag.ProjectId = projectId;
            ViewBag.BuildingTypeCode = buildingTypeCode == null ? "" : buildingTypeCode.ToString();
            ViewBag.PurposeCode = purposeCode;
            ViewBag.AreaTypeCode = areaTypeCode == null ? "" : areaTypeCode.ToString();
            ViewBag.Date = date;
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend();
            List<FxtApi_DATProject> projectList = ProjectApi.GetProjectByCityIdAndProjectIds(fxtCityId, new int[] { projectId }, _fxtApi: fxtApi);
            List<FxtApi_SYSCode> purposeCodeList = SysCodeApi.GetSYSCodeById(SysCodeApi.CodeID_2, _fxtApi: fxtApi);
            List<FxtApi_SYSCode> caseTypeCodeList = SysCodeApi.GetSYSCodeById(SysCodeApi.CodeID_3, _fxtApi: fxtApi);
            List<FxtApi_SYSCode> structureCodeList = SysCodeApi.GetSYSCodeById(SysCodeApi.CodeID_4, _fxtApi: fxtApi);
            List<FxtApi_SYSCode> buildingTypeCodeList = SysCodeApi.GetSYSCodeById(SysCodeApi.CodeID_5, _fxtApi: fxtApi);
            List<FxtApi_SYSCode> houseTypeCodeList = SysCodeApi.GetSYSCodeById(SysCodeApi.CodeID_6, _fxtApi: fxtApi);
            List<FxtApi_SYSCode> frontCodeList = SysCodeApi.GetSYSCodeById(SysCodeApi.CodeID_7, _fxtApi: fxtApi);
            List<FxtApi_SYSCode> moneyUnitCodeList = SysCodeApi.GetSYSCodeById(SysCodeApi.CodeID_8, _fxtApi: fxtApi);
            List<FxtApi_SYSCode> fitmentCodeList = SysCodeApi.GetSYSCodeById(SysCodeApi.CodeID_9, _fxtApi: fxtApi);
            ViewBag.PurposeCodeList = purposeCodeList;
            ViewBag.CaseTypeCodeList = caseTypeCodeList;
            ViewBag.StructureCodeList = structureCodeList;
            ViewBag.BuildingTypeCodeList = buildingTypeCodeList;
            ViewBag.HouseTypeCodeList = houseTypeCodeList;
            ViewBag.FrontCodeList = frontCodeList;
            ViewBag.MoneyUnitCodeList = moneyUnitCodeList;
            ViewBag.FitmentCodeList = fitmentCodeList;
            ViewBag.ProjectList = projectList;
            FxtApi_SYSCode prur = purposeCodeList.Where(p => p.Code == purposeCode).FirstOrDefault();
            ViewBag.PurposeCodeName = prur == null ? "" : prur.CodeName;
            ViewBag.AreaTypeCodeName = areaTypeCode == null ? "" : BuildingArea(Convert.ToInt32(areaTypeCode));
            FxtApi_SYSCode pt = buildingTypeCodeList.Where(p => p.Code == Convert.ToInt32(buildingTypeCode)).FirstOrDefault();
            ViewBag.BuildingTypeName = pt == null ? "" : pt.CodeName;
            string lastDate = Convert.ToDateTime(date).AddMonths(-1).ToString("yyyy-MM");
            List<FxtApi_DATProjectAvgPrice> list = new List<FxtApi_DATProjectAvgPrice>();
            if (purposeCode == SysCodeApi.Code1)
            {
                list = ProjectAvgPriceApi.GetCross(new int[] { projectId }, fxtCityId, new int[] { SysCodeApi.Code1 }, new string[] { lastDate, date }, _fxtApi: fxtApi);
                FxtApi_DATProjectAvgPrice avgObj = list.Where(p => p.PurposeType == purposeCode && p.BuildingAreaType == Convert.ToInt32(areaTypeCode) && p.BuildingTypeCode == Convert.ToInt32(buildingTypeCode) && p.AvgPriceDate == date.Replace("-", "")).FirstOrDefault();
                ViewBag.AvgPrice = avgObj == null ? 0 : avgObj.AvgPrice;
                FxtApi_DATProjectAvgPrice avgObjLast = list.Where(p => p.PurposeType == purposeCode && p.BuildingAreaType == Convert.ToInt32(areaTypeCode) && p.BuildingTypeCode == Convert.ToInt32(buildingTypeCode) && p.AvgPriceDate == lastDate.Replace("-", "")).FirstOrDefault();
                ViewBag.AvgPriceLast = avgObjLast == null ? 0 : avgObjLast.AvgPrice;

            }
            else
            {
                list = ProjectAvgPriceApi.GetCross(new int[] { projectId }, fxtCityId, new int[] { SysCodeApi.Code2 }, new string[] { lastDate, date }, _fxtApi: fxtApi);
                FxtApi_DATProjectAvgPrice avgObj = list.Where(p => p.PurposeType == purposeCode && p.AvgPriceDate == date.Replace("-", "")).FirstOrDefault();
                ViewBag.AvgPrice = avgObj == null ? 0 : avgObj.AvgPrice;
                FxtApi_DATProjectAvgPrice avgObjLast = list.Where(p => p.PurposeType == purposeCode && p.AvgPriceDate == lastDate.Replace("-", "")).FirstOrDefault();
                ViewBag.AvgPriceLast = avgObjLast == null ? 0 : avgObjLast.AvgPrice;
            }
            double purposeFloatValue = 1;
            if (ViewBag.AvgPriceLast > 0)
            {
                purposeFloatValue = (Convert.ToDouble(ViewBag.AvgPrice) / Convert.ToDouble(ViewBag.AvgPriceLast)) - 1;
                purposeFloatValue = Convert.ToDouble(Decimal.Round(Convert.ToDecimal(purposeFloatValue), 4));
            }
            else if (ViewBag.AvgPrice <= 0)
            {
                purposeFloatValue = 0;
            }
            ViewBag.FloatValue = Convert.ToInt32(purposeFloatValue * 100) + "%";
            ViewBag.FxtCompanyIds = PriviCompanyShowDataApi.GetFxtPriviCompanyShowDataCaseCompanyIds(fxtCityId, _fxtApi: fxtApi);
            fxtApi.Abort();
            return View();
        }
        /// <summary>
        /// 获取案例列表
        /// </summary>
        /// <param name="fxtCityId"></param>
        /// <param name="projectId"></param>
        /// <param name="buildingTypeCode"></param>
        /// <param name="purposeCode"></param>
        /// <param name="areaTypeCode"></param>
        /// <param name="date"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="isGetCount"></param>
        /// <returns></returns>
        [FxtSpider.Manager.Common.AuthorizeAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult CaseList_GetCaseListByProjectIdAndFxtCityIdAndPurposeCodeAndBuildingTypeCodeAndAreaTypeAndDate(int fxtCityId, int projectId,string fxtCompanyIds, int? buildingTypeCode, int purposeCode, int? areaTypeCode, string date, int pageIndex, int pageSize, int isGetCount)
        {
            int count = 0;
            bool isGetCount2 = false;
            List<FxtApi_DATCase> list = new List<FxtApi_DATCase>();
            //是否获取总个数
            if (isGetCount == 1)
            {
                isGetCount2 = true;
            }
            if (!string.IsNullOrEmpty(date))
            {

                string startDate = CommonHelp.GetDateTimeMoths(date, -2, "yyyy-MM-01");
                string endDate = CommonHelp.GetDateTimeMoths(date, 0, "yyyy-MM-dd 23:59:59");
                list = CaseApi.GetCaseByFxtCityIdAndFxtProjectIdAndBuildingTypeCodeAndPurposeCodeAndAreaTypeAndDate(fxtCityId, projectId,fxtCompanyIds, buildingTypeCode, purposeCode, areaTypeCode, Convert.ToDateTime(startDate), Convert.ToDateTime(endDate), pageIndex, pageSize, out count, isGetCount: isGetCount2);
            }
            string listJson = list == null ? "null" : list.EncodeField<FxtApi_DATCase>().ToJSONjss();
            int[] areaIds = null;
            if (list != null && list.Count > 0)
            {
                List<int> _areaIds = new List<int>();
                foreach (FxtApi_DATCase _case in list)
                {
                    if (_case.AreaId != null)
                    {
                        _areaIds.Add(Convert.ToInt32(_case.AreaId));
                    }
                }
                if (_areaIds != null && _areaIds.Count > 0)
                {
                    areaIds = _areaIds.ToArray();
                }
            }
            List<FxtApi_SYSArea> areaList = new List<FxtApi_SYSArea>();
            if (areaIds != null && areaIds.Length > 0)
            {
                areaList = AreaApi.GetAreaByAreaIds(areaIds);
            }
            string listJson2 = areaList == null ? "null" : areaList.EncodeField<FxtApi_SYSArea>().ToJSONjss();

            string resultJson = new StringBuilder()
                             .Append("{\"List\":").Append(string.IsNullOrEmpty(listJson) ? "null" : listJson)
                             .Append(",\"AreaList\":").Append(string.IsNullOrEmpty(listJson2) ? "null" : listJson2)
                             .Append(",\"Count\":").Append(count)
                             .Append(",\"FxtCityId\":").Append(list != null && list.Count > 0 ? list[0].CityID : 0)
                             .Append("}").ToString();
            resultJson = resultJson.MvcResponseJson();
            Response.Write(resultJson);
            Response.End();
            return null;
        }
        /// <summary>
        /// 批量删除案例
        /// </summary>
        /// <param name="fxtCityId"></param>
        /// <param name="caseIds"></param>
        /// <returns></returns>
        [FxtSpider.Manager.Common.AuthorizeAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult CaseList_DeleteCaseByFxtCityIdAndCaseIds(int fxtCityId, string caseIds)
        {
            string json = "";
            int[] _caseId = caseIds.ConvertToInts(',');
            if (_caseId == null || _caseId.Length < 1)
            {
                Response.Write(json.MvcResponseJson(result: 0, message: "请选择要删除的案例"));
                Response.End();
                return null;
            }
            json = _caseId.ConvertToString();
            string message = "";
            bool result = CaseApi.DeleteCaseByFxtCityIdAndCaseIds(fxtCityId, _caseId, out message);
            if (!result)
            {
                Response.Write(json.MvcResponseJson(result: 0, message: message));
                Response.End();
                return null;
            }
            json = WebJsonHelp.MvcResponseJson(json, result: 1, message: message);
            Response.Write(json);
            Response.End();
            return null;
        }

        [FxtSpider.Manager.Common.AuthorizeAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult CaseList_ResetCross(int fxtCityId, int projectId, int purposeCode, int? buildingTypeCode, int? areaTypeCode, string date)
        {
            string message = "";
            string lastDate = Convert.ToDateTime(date).AddMonths(-1).ToString("yyyy-MM");
            int avgPrice = 0;
            int avgPriceLast = 0;
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend();
            if (purposeCode == SysCodeApi.Code1)
            {
                var priceObj = ProjectAvgPriceApi.ResetCross(projectId, fxtCityId, purposeCode, Convert.ToInt32(buildingTypeCode), Convert.ToInt32(areaTypeCode), date, out message, _fxtApi: fxtApi);
                if (priceObj != null)
                {
                    avgPrice = priceObj.AvgPrice;
                }
                List<FxtApi_DATProjectAvgPrice> list = ProjectAvgPriceApi.GetProjectAvgPriceByProjectIdAndCityIdAndBy(projectId, fxtCityId, purposeCode, buildingTypeCode, areaTypeCode, lastDate, 3, _fxtApi: fxtApi);
                if (list != null && list.Count > 0)
                {
                    avgPriceLast = list[0].AvgPrice;
                }
            }
            else
            {
                var priceObj = ProjectAvgPriceApi.ResetCross(projectId, fxtCityId, purposeCode, 0, 0, date, out message, _fxtApi: fxtApi);
                if (priceObj != null)
                {
                    avgPrice = priceObj.AvgPrice;
                }
                List<FxtApi_DATProjectAvgPrice> list = ProjectAvgPriceApi.GetProjectAvgPriceByProjectIdAndCityIdAndBy(projectId, fxtCityId, purposeCode, null, null, lastDate, 3, _fxtApi: fxtApi);
                if (list != null && list.Count > 0)
                {
                    avgPriceLast = list[0].AvgPrice;
                }
            }
            fxtApi.Abort();
            double purposeFloatValue = 1;
            if (avgPriceLast > 0)
            {
                purposeFloatValue = (Convert.ToDouble(avgPrice) / Convert.ToDouble(avgPriceLast)) - 1;
                purposeFloatValue = Convert.ToDouble(Decimal.Round(Convert.ToDecimal(purposeFloatValue), 4));
            }
            else if (avgPrice <= 0)
            {
                purposeFloatValue = 0;
            }
            string floatValue = Convert.ToInt32(purposeFloatValue * 100) + "%";
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"AvgPrice\":\"").Append(avgPrice).Append("\",")
              .Append("\"FloatValue\":\"").Append(floatValue).Append("\"}");
            string json = WebJsonHelp.MvcResponseJson(sb.ToString(), result: 1, message: message);
            Response.Write(json);
            Response.End();
            return null;
        }

        [FxtSpider.Manager.Common.AuthorizeAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult CaseList_SubmitPrice_Api(int caseId, int fxtCityId, string unitPrice, string totalPrice)
        {
            string json = "";
            unitPrice = string.IsNullOrEmpty(unitPrice) ? null : unitPrice;
            totalPrice = string.IsNullOrEmpty(totalPrice) ? null : totalPrice;
            if (string.IsNullOrEmpty(unitPrice))
            {
                Response.Write(json.MvcResponseJson(result: 0, message: "请填写单价"));
                Response.End();
                return null;
            }
            if (string.IsNullOrEmpty(totalPrice))
            {
                Response.Write(json.MvcResponseJson(result: 0, message: "请填写总价"));
                Response.End();
                return null;
            }
            FxtApi_DATCase _case = CaseApi.GetCaseByFxtCityIdAndCaseId(fxtCityId, Convert.ToInt32(caseId));
            if (_case == null)
            {
                Response.Write(json.MvcResponseJson(result: 0, message: "案例不存在或已被删除"));
                Response.End();
                return null;
            }
            FxtApi_DATProjectAvgPrice nowMonthAvgPrice = ProjectAvgPriceApi.GetProjectAvgNowMonths(_case.ProjectId, fxtCityId, Convert.ToInt32(_case.PurposeCode), CommonHelp.GetDateTimeMoths(_case.CaseDate.ToString(), 2, "yyyy-MM"));
            if (nowMonthAvgPrice != null && nowMonthAvgPrice.AvgPrice > 0)
            {
                decimal maxRatio = 0.25M;
                decimal maxPrice = Convert.ToDecimal(nowMonthAvgPrice.AvgPrice + (nowMonthAvgPrice.AvgPrice * maxRatio));
                decimal minPrice = Convert.ToDecimal(nowMonthAvgPrice.AvgPrice - (nowMonthAvgPrice.AvgPrice * maxRatio));
                decimal _unitPrice = Convert.ToDecimal(unitPrice);
                if (_unitPrice > maxPrice || _unitPrice < minPrice)
                {
                    Response.Write(json.MvcResponseJson(result: 0, message: string.Format("案例单价超过了当月楼盘均价{0}的25%", nowMonthAvgPrice.AvgPrice)));
                    Response.End();
                    return null;
                }
            }
            string message = "";
            _case.UnitPrice = Convert.ToDecimal(unitPrice);
            _case.TotalPrice = Convert.ToDecimal(totalPrice);
            _case.SaveDateTime = DateTime.Now;
            bool result = CaseApi.UpdateCase(fxtCityId, _case, out message);
            if (!result)
            {
                Response.Write(json.MvcResponseJson(result: 0, message: message));
                Response.End();
                return null;
            }
            json = json.MvcResponseJson(result: 1, message: message);
            Response.Write(json);
            Response.End();
            return null;
        }
        
        #endregion

        #region (SetCase_Fancybox.cshtml)
        public ActionResult SetCase_Fancybox(int fxtCityId, int projectId, int? caseId, int? buildingTypeCode, int? purposeCode, int? areaTypeCode, string date)
        {
            ViewBag.FxtCityId = fxtCityId;
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend();
            //基础数据
            List<FxtApi_SYSCode> purposeCodeList = SysCodeApi.GetSYSCodeById(SysCodeApi.CodeID_2, _fxtApi: fxtApi);
            List<FxtApi_SYSCode> caseTypeCodeList = SysCodeApi.GetSYSCodeById(SysCodeApi.CodeID_3, _fxtApi: fxtApi);
            List<FxtApi_SYSCode> structureCodeList = SysCodeApi.GetSYSCodeById(SysCodeApi.CodeID_4, _fxtApi: fxtApi);
            List<FxtApi_SYSCode> buildingTypeCodeList = SysCodeApi.GetSYSCodeById(SysCodeApi.CodeID_5, _fxtApi: fxtApi);
            List<FxtApi_SYSCode> houseTypeCodeList = SysCodeApi.GetSYSCodeById(SysCodeApi.CodeID_6, _fxtApi: fxtApi);
            List<FxtApi_SYSCode> frontCodeList = SysCodeApi.GetSYSCodeById(SysCodeApi.CodeID_7, _fxtApi: fxtApi);
            List<FxtApi_SYSCode> moneyUnitCodeList = SysCodeApi.GetSYSCodeById(SysCodeApi.CodeID_8, _fxtApi: fxtApi);
            List<FxtApi_SYSCode> fitmentCodeList = SysCodeApi.GetSYSCodeById(SysCodeApi.CodeID_9, _fxtApi: fxtApi);
            List<FxtApi_SYSArea> areaList = AreaApi.GetAreaByCityId(fxtCityId, _fxtApi: fxtApi);
            ViewBag.PurposeCodeList = purposeCodeList;
            ViewBag.CaseTypeCodeList = caseTypeCodeList;
            ViewBag.StructureCodeList = structureCodeList;
            ViewBag.BuildingTypeCodeList = buildingTypeCodeList;
            ViewBag.HouseTypeCodeList = houseTypeCodeList;
            ViewBag.FrontCodeList = frontCodeList;
            ViewBag.MoneyUnitCodeList = moneyUnitCodeList;
            ViewBag.FitmentCodeList = fitmentCodeList;
            ViewBag.AreaList = areaList;
            //默认值
            ViewBag.Date = date;
            ViewBag.NowDate = CommonHelp.GetDateTimeMoths(date, 0, "yyyy-MM-dd");
            ViewBag.AreaType = "";
            if (areaTypeCode != null)
            {
                FxtApi_SYSCode _code = SysCodeApi.GetSYSCodeByCode(Convert.ToInt32(areaTypeCode), _fxtApi: fxtApi);
                if (_code != null)
                {
                    ViewBag.AreaType = _code.CodeName;
                }
            }
            FxtApi_DATCase caseObj = new FxtApi_DATCase();
            if (caseId == null)
            {
                ViewBag.CaseId = "";
                ViewBag.ActionType = "add";
                caseObj.BuildingTypeCode = Convert.ToInt32(buildingTypeCode);
                caseObj.PurposeCode = Convert.ToInt32(purposeCode);
                List<FxtApi_DATProject> projectList = ProjectApi.GetProjectByCityIdAndProjectIds(fxtCityId, new int[] { projectId }, _fxtApi: fxtApi);
                ViewBag.ProjectName = projectList != null && projectList.Count > 0 ? projectList[0].ProjectName : "";
                caseObj.ProjectId = projectList != null && projectList.Count > 0 ? projectList[0].ProjectId : 0;
                caseObj.CaseTypeCode = SysCodeApi.Code12;
                caseObj.MoneyUnitCode = SysCodeApi.Code13;
                caseObj.StructureCode = SysCodeApi.Code14;
            }
            else
            {
                ViewBag.CaseId = Convert.ToInt32(caseId);
                ViewBag.ActionType = "update";
                caseObj = CaseApi.GetCaseByFxtCityIdAndCaseId(fxtCityId, Convert.ToInt32(caseId), _fxtApi: fxtApi);
                if (caseObj == null)
                {
                    caseObj = new FxtApi_DATCase();
                }
                List<FxtApi_DATProject> projectList = ProjectApi.GetProjectByCityIdAndProjectIds(fxtCityId, new int[] { caseObj.ProjectId }, _fxtApi: fxtApi);
                ViewBag.ProjectName = projectList != null && projectList.Count > 0 ? projectList[0].ProjectName : "";
                ViewBag.NowDate = caseObj == null ? ViewBag.NowDate : caseObj.CaseDate.ToString();
            }
            ViewBag.CaseObj = caseObj;
            fxtApi.Abort();
            return View();
        }
        [FxtSpider.Manager.Common.AuthorizeAttribute(NowRequestType = RequestType.AJAX)]
        public ActionResult SetCase_Submit_Api(string actionType, int? caseId, int fxtCityId, int projectId, string caseDate, int? purposeCode,
            string buildingArea, string unitPrice, string totalPrice, int? caseTypeCode, int? structureCode, int? buildingTypeCode,
            int? floorNumber, int? totalFloor, int? houseTypeCode, int? frontCode, int? moneyUnitCode, string remark, int? areaId,
            string buildingDate, int? fitmentCode, string subHouse, string peiTao, string createUser, string sourceName, string sourceLink, string sourcePhone)
        {
            string json = "";
            int? buildingId = null;
            string houseNo = null;
            purposeCode = purposeCode == 0 ? null : purposeCode;
            caseTypeCode = caseTypeCode == 0 ? null : caseTypeCode;
            structureCode = structureCode == 0 ? null : structureCode;
            buildingTypeCode = buildingTypeCode == 0 ? null : buildingTypeCode;
            floorNumber = floorNumber == 0 ? null : floorNumber;
            totalFloor = totalFloor == 0 ? null : totalFloor;
            houseTypeCode = houseTypeCode == 0 ? null : houseTypeCode;
            frontCode = frontCode == 0 ? null : frontCode;
            moneyUnitCode = moneyUnitCode == 0 ? null : moneyUnitCode;
            areaId = areaId == 0 ? null : areaId;
            fitmentCode = fitmentCode == 0 ? null : fitmentCode;
            caseDate = string.IsNullOrEmpty(caseDate) ? null : caseDate;
            buildingArea = string.IsNullOrEmpty(buildingArea) ? null : buildingArea;
            unitPrice = string.IsNullOrEmpty(unitPrice) ? null : unitPrice;
            totalPrice = string.IsNullOrEmpty(totalPrice) ? null : totalPrice;
            remark = string.IsNullOrEmpty(remark) ? null : remark;
            buildingDate = string.IsNullOrEmpty(buildingDate) ? null : buildingDate;
            subHouse = string.IsNullOrEmpty(subHouse) ? null : subHouse;
            peiTao = string.IsNullOrEmpty(peiTao) ? null : peiTao;
            createUser = string.IsNullOrEmpty(createUser) ? null : createUser;
            sourceName = string.IsNullOrEmpty(sourceName) ? null : sourceName;
            sourceLink = string.IsNullOrEmpty(sourceLink) ? null : sourceLink;
            sourcePhone = string.IsNullOrEmpty(sourcePhone) ? null : sourcePhone;
            if (purposeCode == null)
            {
                Response.Write(json.MvcResponseJson(result: 0, message: "请填写用途"));
                Response.End();
                return null;
            }
            if (string.IsNullOrEmpty(buildingArea))
            {
                Response.Write(json.MvcResponseJson(result: 0, message: "请填写面积"));
                Response.End();
                return null;
            }
            if (string.IsNullOrEmpty(unitPrice))
            {
                Response.Write(json.MvcResponseJson(result: 0, message: "请填写单价"));
                Response.End();
                return null;
            }
            if (string.IsNullOrEmpty(totalPrice))
            {
                Response.Write(json.MvcResponseJson(result: 0, message: "请填写总价"));
                Response.End();
                return null;
            }
            if (caseTypeCode == null)
            {
                Response.Write(json.MvcResponseJson(result: 0, message: "请填写案例类型"));
                Response.End();
                return null;
            }
            if (string.IsNullOrEmpty(caseDate))
            {
                Response.Write(json.MvcResponseJson(result: 0, message: "请填写案例时间"));
                Response.End();
                return null;
            }
            remark = remark.DecodeField();
            caseDate = caseDate.DecodeField();
            subHouse = subHouse.DecodeField();
            createUser = createUser.DecodeField();
            sourceName = sourceName.DecodeField();
            sourceLink = sourceLink.DecodeField();
            sourcePhone = sourcePhone.DecodeField();
            peiTao = peiTao.DecodeField();
            if (!caseDate.CheckStrIsDate())
            {
                Response.Write(json.MvcResponseJson(result: 0, message: "案例时间格式错误"));
                Response.End();
                return null;
            }
            FxtApi_DATProjectAvgPrice nowMonthAvgPrice = ProjectAvgPriceApi.GetProjectAvgNowMonths(projectId, fxtCityId, Convert.ToInt32(purposeCode), CommonHelp.GetDateTimeMoths(caseDate, 2, "yyyy-MM"));
            if (nowMonthAvgPrice != null && nowMonthAvgPrice.AvgPrice > 0)
            {
                decimal maxRatio = 0.25M;
                decimal maxPrice = Convert.ToDecimal(nowMonthAvgPrice.AvgPrice + (nowMonthAvgPrice.AvgPrice * maxRatio));
                decimal minPrice = Convert.ToDecimal(nowMonthAvgPrice.AvgPrice - (nowMonthAvgPrice.AvgPrice * maxRatio));
                decimal _unitPrice = Convert.ToDecimal(unitPrice);
                if (_unitPrice > maxPrice || _unitPrice < minPrice)
                {
                    Response.Write(json.MvcResponseJson(result: 0, message: string.Format("案例单价超过了当月楼盘均价{0}的25%", nowMonthAvgPrice.AvgPrice)));
                    Response.End();
                    return null;
                }
            }
            string message = "";
            FxtApi_DATCase _case = null;
            if (actionType.Equals("add"))
            {
                _case = CaseApi.InsertCase(fxtCityId, projectId, buildingId, houseNo, Convert.ToDateTime(caseDate), purposeCode,
                Convert.ToDecimal(buildingArea), Convert.ToDecimal(unitPrice), Convert.ToDecimal(totalPrice), caseTypeCode, structureCode, buildingTypeCode,
                floorNumber, totalFloor, houseTypeCode, frontCode, moneyUnitCode, remark, areaId,
               buildingDate, fitmentCode, subHouse, peiTao, createUser, sourceName, sourceLink, sourcePhone, out message);

                if (_case == null)
                {
                    Response.Write(json.MvcResponseJson(result: 0, message: message));
                    Response.End();
                    return null;
                }
            }
            else
            {
                _case = new FxtApi_DATCase();
                _case = CaseApi.GetCaseByFxtCityIdAndCaseId(fxtCityId, Convert.ToInt32(caseId));
                if (_case == null)
                {
                    Response.Write(json.MvcResponseJson(result: 0, message: "案例不存在或已被删除"));
                    Response.End();
                    return null;
                }
                _case.CaseDate = Convert.ToDateTime(caseDate); _case.PurposeCode = purposeCode;
                _case.BuildingArea = Convert.ToDecimal(buildingArea); _case.UnitPrice = Convert.ToDecimal(unitPrice);
                _case.TotalPrice = Convert.ToDecimal(totalPrice); _case.CaseTypeCode = caseTypeCode;
                _case.StructureCode = structureCode; _case.BuildingTypeCode = buildingTypeCode;
                _case.FloorNumber = floorNumber; _case.TotalFloor = totalFloor; _case.HouseTypeCode = houseTypeCode;
                _case.FrontCode = frontCode; _case.MoneyUnitCode = moneyUnitCode; _case.Remark = remark;
                _case.AreaId = areaId; _case.BuildingDate = buildingDate; _case.FitmentCode = fitmentCode;
                _case.SubHouse = subHouse; _case.PeiTao = peiTao; _case.Creator = createUser; _case.SourceName = sourceName;
                _case.SourceLink = sourceLink; _case.SourcePhone = sourcePhone;
                _case.SaveDateTime = DateTime.Now;
                bool result = CaseApi.UpdateCase(fxtCityId, _case, out message);
                if (!result)
                {
                    Response.Write(json.MvcResponseJson(result: 0, message: message));
                    Response.End();
                    return null;
                }
            }
            string _caseJson = _case.EncodeField().ToJSONjss();
            List<FxtApi_SYSArea> areaList = new List<FxtApi_SYSArea>();
            if (_case.AreaId != null)
            {
                areaList = AreaApi.GetAreaByAreaIds(new int[] { Convert.ToInt32(_case.AreaId) });
            }
            string _areaJson = areaList == null ? "null" : areaList.EncodeField<FxtApi_SYSArea>().ToJSONjss();

            string resultJson = new StringBuilder()
                         .Append("{\"CaseObj\":").Append(string.IsNullOrEmpty(_caseJson) ? "null" : _caseJson)
                         .Append(",\"AreaList\":").Append(string.IsNullOrEmpty(_areaJson) ? "null" : _areaJson)
                         .Append("}").ToString();

            json = WebJsonHelp.MvcResponseJson(resultJson, result: 1, message: message);
            Response.Write(json);
            Response.End();
            return null;
        }
        #endregion

        #region Common
        string BuildingArea(int buildingTypeCode)
        {
            string str = "";
            switch (buildingTypeCode)
            {
                case SysCodeApi.Code3:
                        str = "面积<30";
                    break;
                case SysCodeApi.Code4:
                        str = "30<=面积<60";
                    break;
                case SysCodeApi.Code5:
                        str = "60<=面积<90";
                    break;
                case SysCodeApi.Code6:
                        str = "90<=面积<120";
                    break;
                case SysCodeApi.Code7:
                        str = "120< 面积";
                    break;

            }
            return str;
        }
        #endregion

    }
}
