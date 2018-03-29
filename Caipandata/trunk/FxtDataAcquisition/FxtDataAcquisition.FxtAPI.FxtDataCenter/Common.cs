using CAS.Common.MVC4;
using FxtDataAcquisition.Common;
using FxtDataAcquisition.Domain.DTO.FxtDataCenterDTO;
using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
using FxtDataAcquisition.Domain.Models;
using FxtDataAcquisition.FxtAPI.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace FxtDataAcquisition.FxtAPI.FxtDataCenter
{
    public static class Common
    {
        #region(数据中心functionName)
        /// <summary>
        /// 获取楼盘下拉列表
        /// </summary>
        public const string projectdropdownlist = "projectdropdownlist";
        /// <summary>
        /// 获取楼盘下拉列表（分页）
        /// </summary>
        public const string plist = "plist";
        /// <summary>
        /// 获取楼栋列表
        /// </summary>
        public const string queryautobuildinginfolist = "queryautobuildinginfolist";
        /// <summary>
        /// 获取房号列表
        /// </summary>
        public const string queryhousedropdownlist = "queryhousedropdownlist";
        /// <summary>
        /// 获取省份列表
        /// </summary>
        public const string provincelist = "provincelist";
        /// <summary>
        /// 获取城市列表
        /// </summary>
        public const string citylist = "citylist";
        /// <summary>
        /// 获取行政区列表
        /// </summary>
        public const string garealist = "garealist";
        /// <summary>
        /// 获取片列表
        /// </summary>
        public const string gsubarealist = "gsubarealist";
        /// <summary>
        /// 获取系统CODE列表根据ID
        /// </summary>
        public const string gscodelist = "gscodelist";
        /// <summary>
        /// 给楼盘新增照片
        /// </summary>
        public const string addprojectphoto = "addprojectphoto";
        /// <summary>
        /// 从采集系统导入楼盘信息到数据中心
        /// </summary>
        public const string importprojectdata = "importprojectdata";
        /// <summary>
        /// 获取楼盘楼栋房号列表 
        /// </summary>
        public const string getprojectbuildinghouselist = "getprojectbuildinghouselist";
        /// <summary>
        /// 获取楼盘详细信息(包含codeName)
        /// </summary>
        public const string gpdinfo = "gpdinfo";
        /// <summary>
        /// 获取用户角色的城市权限
        /// </summary>
        public const string getsysroleuserids = "getsysroleuserids";
        /// <summary>
        /// 根据楼盘ID获取楼栋、房号数量
        /// </summary>
        public const string buildinghousetotal = "buildinghousetotal";
        #endregion

        /// <summary>
        /// 数据中心API的code
        /// </summary>
        public static int appId
        {
            get
            {
                return Convert.ToInt32(CommonUtility.GetConfigSetting("wcfdatacenterservice_appid"));
            }
        }
        /// <summary>
        /// 当前产品(无纸化住宅物业信息采集系统)code
        /// </summary>
        public static int systypeCode
        {
            get
            {
                return Convert.ToInt32(CommonUtility.GetConfigSetting("wcfdatacenterservice_systypecode"));
            }
        }
        public static string GetCode(string signname, string time, string functionname, List<Apps> appList)
        {
            string appPwd = "";
            string appKey = "";
            string appUrl = "";
            FxtApiCommon.GetNowApiInfo(Common.appId, appList, out appUrl, out appPwd, out appKey);
            string[] pwdArray = { Common.appId.ToString(), appPwd, signname, time, functionname };
            string code = EncryptHelper.GetMd5(pwdArray, appKey);
            return code;
        }
        /// <summary>
        /// 转换数据中心结果对象
        /// </summary>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static DataCenterResult GetDataCenterResult(string jsonStr)
        {
            if (string.IsNullOrEmpty(jsonStr))
            {
                return new DataCenterResult();
            }
            DataCenterResult obj = jsonStr.ParseJSONjss<DataCenterResult>();
            return obj;
        }
        /// <summary>
        /// 调用数据中心接口
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="signName"></param>
        /// <param name="functionName"></param>
        /// <param name="parasObj"></param>
        /// <returns></returns>
        public static DataCenterResult PostDataCenter(string userName, string signName, string functionName, object parasObj, List<Apps> appList)
        {
            try
            {
                string appPwd = "";
                string appKey = "";
                string apiUrl = "";
                FxtApiCommon.GetNowApiInfo(Common.appId, appList, out apiUrl, out appPwd, out appKey);
                apiUrl = "http://localhost:40455/dc/active";
                string nowTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                var para = new
                {
                    sinfo = JsonHelp.ToJSONjss(new
                    {
                        functionname = functionName,
                        appid = Common.appId,
                        apppwd = appPwd,
                        signname = signName,
                        time = nowTime,
                        code = Common.GetCode(signName, nowTime, functionName, appList)
                    }),
                    info = JsonHelp.ToJSONjss(new
                    {
                        uinfo = new { username = userName, token = "" },
                        appinfo = new
                        {
                            splatype = "win",
                            platVer = "2007",
                            stype = "",//验证
                            version = "4.26",
                            vcode = "1",
                            systypecode = Common.systypeCode.ToString(),//验证
                            channel = "360"
                        },
                        funinfo = parasObj
                    })
                };

                HttpClient client = new HttpClient();
                //client.BaseAddress = new Uri(apiUrl);
                client.Timeout = new TimeSpan(0,10,0);
                HttpResponseMessage hrm = client.PostAsJsonAsync(new Uri(apiUrl), para).Result;
                string str = hrm.Content.ReadAsStringAsync().Result;
                DataCenterResult result = Common.GetDataCenterResult(str);
                return result;
            }
            catch (AggregateException ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 调用数据中心接口(用于上传文件)
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="signName"></param>
        /// <param name="functionName"></param>
        /// <param name="parasObj"></param>
        /// <param name="data"></param>
        /// <param name="appList"></param>
        /// <returns></returns>
        public static DataCenterResult PostDataCenter(string userName, string signName, string functionName, object parasObj, byte[] data, List<Apps> appList)
        {
            string appPwd = "";
            string appKey = "";
            string apiUrl = "";
            FxtApiCommon.GetNowApiInfo(Common.appId, appList, out apiUrl, out appPwd, out appKey);
            //apiUrl = "http://192.168.1.104:8100/dc/active";
            string nowTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            string sinfo = JsonHelp.ToJSONjss(new
                 {
                     functionname = functionName,
                     appid = Common.appId,
                     apppwd = appPwd,
                     signname = signName,
                     time = nowTime,
                     code = Common.GetCode(signName, nowTime, functionName, appList)
                 });
            string info = JsonHelp.ToJSONjss(new
                {
                    uinfo = new { username = userName, token = "" },
                    appinfo = new
                    {
                        splatype = "win",
                        platVer = "2007",
                        stype = "",//验证
                        version = "4.26",
                        vcode = "1",
                        systypecode = Common.systypeCode.ToString(),//验证
                        channel = "360"
                    },
                    funinfo = parasObj
                });
            string url = apiUrl + "/upload";
            url = url + "?sinfo=" + sinfo.EncodeField() + "&info=" + info.EncodeField();
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
            webrequest.ContentType = "application/octet-stream";
            webrequest.Method = "POST";
            Stream requestStream = webrequest.GetRequestStream();
            byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n–—————————7d930d1a850658–\r\n");
            requestStream.Write(data, 0, data.Length);
            // 输入尾部数据 
            requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
            WebResponse responce = webrequest.GetResponse();
            Stream s = responce.GetResponseStream();
            StreamReader sr = new StreamReader(s);
            // 返回数据流(源码) 
            string str = sr.ReadToEnd();
            DataCenterResult result = Common.GetDataCenterResult(str);
            return result;
        }

        public static string ConvertToJson(List<PCompany> list)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (PCompany obj in list)
            {
                sb.Append("{");
                sb.Append("\"projectid\":").Append(obj.ProjectId).Append(",");
                sb.Append("\"companyname\":").Append(obj.CompanyName == null ? "null" : "\"" + obj.CompanyName + "\"").Append(",");
                sb.Append("\"companytype\":").Append(obj.CompanyType).Append(",");
                sb.Append("\"cityid\":").Append(obj.CityId);
                sb.Append("},");
            }
            return sb.ToString().TrimEnd(',') + "]";
        }

    }
}
