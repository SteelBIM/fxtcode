using System;
using System.ServiceModel.Activation;
using System.Text;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;

using CAS.Common;
using CAS.Entity;
using FxtCenterServiceOpen.Actualize.Common;
using FxtCenterServiceOpen.Actualize.VQOperater;
using FxtCenterServiceOpen.Contract;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using FxtCenterServiceOpen.Actualize.UserCenter;

namespace FxtCenterServiceOpen.Actualize
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class DataCenterService : IDataCenterService
    {
        private string apiUrl = System.Configuration.ConfigurationManager.AppSettings["datacenterapi"];

        private static Dictionary<string, string> dicZipCodeMap = new Dictionary<string, string>();

        private static Dictionary<string, string> dicZipCodeCityIdMap = new Dictionary<string, string>();
        //public string Entrance(string sinfo, string info)
        public WCFJsonData Entrance(string sinfo, string info)
        {
            WCFJsonData returnData = new WCFJsonData();
            try
            {

                JObject jObjSInfo = JObject.Parse(sinfo);
                string funtionname = jObjSInfo.Value<string>("functionname");
                if (!MethodDictionary.MethodDic.ContainsKey(funtionname))
                {
                    returnData.data = "非法访问未开放接口!";
                    returnData.returntext = "errorjsonData";
                    returnData.returntype = -1;
                    return returnData;
                }

                #region 城市国际码匹配

                JavaScriptSerializer jss = new JavaScriptSerializer();
                //评估发展中心使用时房号数太多，先把最大值设大点，待评估发展中心取数据修改后去掉设置值
                jss.MaxJsonLength = 10240000;

                JObject jObjInfo = JObject.Parse(info);
                JObject jObjFInfo = jObjInfo["funinfo"] as JObject;

                //房讯通使用的国标码
                if (dicZipCodeCityIdMap.Count <= 0)
                {
                    dicZipCodeCityIdMap = ZipCodeHelper.GetZipCodeCityIdMap();
                }

                string cityId = jObjFInfo.Value<string>("cityid");//城市Id参数值
                string zipcode = jObjFInfo.Value<string>("zipcode");//国标码参数值
                cityId=string.IsNullOrEmpty(cityId) ? "" : cityId;
                zipcode = string.IsNullOrEmpty(zipcode) ? "" : zipcode;

                //添加属性fxtcompanyid
                if (jObjFInfo.Property("fxtcompanyid") == null)
                {
                    string signname = jObjSInfo.Value<string>("signname");//签名
                    string outjson = string.Empty;
                    JsonReturnData rstjson = new JsonReturnData();
                    CompanyModel company = CallUserCenter.GetCompanyModelBySignName(signname, out rstjson, out outjson);
                    jObjFInfo.Add("fxtcompanyid", company.CompanyID);
                    LogHelper.Info("signname:" + signname);
                }

                //cityid没有值并且zipcode有值 则通过zipcode获取cityid值
                if (string.IsNullOrEmpty(cityId) && !string.IsNullOrEmpty(zipcode))
                {
                    string signname = jObjSInfo.Value<string>("signname");//签名
                    //房讯通Zipcode缓存存在记录
                    if (dicZipCodeCityIdMap.ContainsKey(zipcode))
                    {
                        cityId = dicZipCodeCityIdMap[zipcode];
                    }
                    else//房讯通Zipcode缓存不存在记录
                    {
                        if (dicZipCodeMap.ContainsKey(signname + zipcode))//客户Zipcode缓存存在记录
                        {
                            string fxtZipCode = dicZipCodeMap[signname + zipcode];
                            if (dicZipCodeCityIdMap.ContainsKey(fxtZipCode))//存在客户zipcode跟FXT映射关系
                                cityId = dicZipCodeCityIdMap[fxtZipCode];
                        }
                        else
                        {
                            string mapPath = System.Web.HttpContext.Current.Server.MapPath("~/ZipCodeMap/" + signname + ".xml");
                            if (File.Exists(mapPath)&&ZipCodeHelper.IsXml(mapPath))//存在差异国标码文件
                            {
                                List<ZipCodeMapModel> lstZipCode = new List<ZipCodeMapModel>();
                                XmlSerializer xsZipCode = new XmlSerializer(lstZipCode.GetType());
                                using (FileStream fs = new FileStream(mapPath, FileMode.Open, FileAccess.Read))//反序列化
                                {                                    
                                    lstZipCode = (List<ZipCodeMapModel>)xsZipCode.Deserialize(fs);
                                }

                                if (lstZipCode != null && lstZipCode.Count > 0)
                                {
                                    foreach (ZipCodeMapModel item in lstZipCode)
                                    {
                                        string dicKey = signname + item.zipcode;
                                        if (!dicZipCodeMap.ContainsKey(dicKey))
                                            dicZipCodeMap.Add(dicKey, item.fxtzipcode);
                                    }
                                }
                            }
                            if (dicZipCodeMap.ContainsKey(signname + zipcode))
                            {
                                string  fxtDicZCKey = dicZipCodeMap[signname + zipcode];
                                cityId = dicZipCodeCityIdMap.ContainsKey(fxtDicZCKey) ? dicZipCodeCityIdMap[fxtDicZCKey] : "";
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(cityId))//CityId还是没有值查询数据库
                    {
                        Dictionary<string, string> cityData = ZipCodeHelper.GetZipCodeCityIdMap(zipcode);
                        if (cityData.Count > 0)
                        {
                            cityId = cityData[zipcode];
                            dicZipCodeCityIdMap = ZipCodeHelper.GetZipCodeCityIdMap();//更新缓存
                        }
                    }

                    if (jObjFInfo.Property("cityid") == null)//添加属性cityid
                        jObjFInfo.Add("cityid", cityId);
                    else
                        jObjFInfo.Property("cityid").Value = cityId;//赋值cityid

                    info = jObjInfo.ToString();//发送运营中心使用新的info
                }
                #endregion

                string postArgs = new { sinfo = sinfo, info = jObjInfo.ToString() }.ToJson();
                string result = WebCommon.APIPostBack(apiUrl, postArgs, false, "application/json");
                returnData = JSONHelper.JSONToObject<WCFJsonData>(result);

                //发送记录到运营中心 2015.12.02 wb add 
                CallOperaterCenter.QuerySendToCenterAsync(sinfo, info, result);

                return returnData;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                returnData.data = "";
                returnData.returntext = "errorjsonData";
                returnData.returntype = -1;
                return returnData;
            }
        }
    }
}
