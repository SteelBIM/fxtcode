using CAS.Common.MVC4;
using FxtDataAcquisition.Common;
using FxtDataAcquisition.Domain.DTO.FxtDataCenterDTO;
using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
using FxtDataAcquisition.FxtAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace FxtDataAcquisition.FxtAPI.FxtUserCenter
{
    public static class Common
    {
        #region(用户中心functionName)
        /// <summary>
        /// 查询用户列表
        /// </summary>
        public const string userseven = "userseven";
        /// <summary>
        /// 获取公司开通产品的城市ID
        /// </summary>
        public const string cptcityids = "cptcityids";
        /// <summary>
        /// 获取公司开通产品模块的城市ID
        /// </summary>
        public const string cptmcityids = "cptmcityids";

        #endregion

        /// <summary>
        /// 用户中心WCFAPI 登录
        /// </summary>
        public static string apiUrl_Login
        {
            get
            {
                return CommonUtility.GetConfigSetting("wcfusercenterservice_login");
            }
        }
        /// <summary>
        /// 用于登陆API加密的KEY
        /// </summary>
        public static string appKey_Login
        {
            get
            {
                return CommonUtility.GetConfigSetting("wcfusercenterservice_appkey_login");
            }
        }
        /// <summary>
        /// 用于登陆密码加密的KEY
        /// </summary>
        public static string appKey_LoginPwd
        {
            get
            {
                return CommonUtility.GetConfigSetting("wcfusercenterservice_appkey_loginpwd");
            }
        }
        /// <summary>
        /// 用户中心API的code
        /// </summary>
        public static int appId
        {
            get
            {
                return Convert.ToInt32(CommonUtility.GetConfigSetting("wcfusercenterservice_appid"));
            }
        }
        /// <summary>
        /// 当前产品(无纸化住宅物业信息采集系统)code
        /// </summary>
        public static int systypeCode
        {
            get
            {
                return Convert.ToInt32(CommonUtility.GetConfigSetting("wcfusercenterservice_systypecode"));
            }
        }
        /// <summary>
        /// 用于功能机密
        /// </summary>
        /// <param name="signname"></param>
        /// <param name="time"></param>
        /// <param name="functionname"></param>
        /// <param name="appList">当前拥有拥有的appList</param>
        /// <returns></returns>
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
        /// 用于登陆加密
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string GetCode(string str, string key)
        {
            string code = EncryptHelper.GetMd5(str, key);
            return code;
        }
        /// <summary>
        /// 转换用户中心结果对象
        /// </summary>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static DataCenterResult GetDataCenterResult(string jsonStr)
        {
            if (string.IsNullOrEmpty(jsonStr))
            {
                return new DataCenterResult();
            }
            //LogHelper.Info(jsonStr);
            DataCenterResult obj = jsonStr.ParseJSONjss<DataCenterResult>();
            return obj;
        }
        public static DataCenterResult PostUserCenter_Login(string userName, string password)
        {
            //LogHelper.Info("Common.apiUrl_Login=" + Common.apiUrl_Login);
            string nowTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            string _password = GetCode(password, appKey_LoginPwd);
            //string _password = EncryptHelper.GetMd5("Cmbfxtproduct*&2014");// GetCode(password, appKey_LoginPwd);
            var para = new
            {
                sinfo = JsonHelp.ToJSONjss(new
                {
                    time = nowTime,
                    code = Common.GetCode(nowTime, appKey_Login)
                }),
                info = JsonHelp.ToJSONjss(new
                {
                    uinfo = new { username = userName, token = "", password = _password },
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
                    funinfo = new { }
                })
            };
            string test = para.ToJSONjss();
            //LogHelper.Info("test=" + test);
            HttpClient client = new HttpClient();
            HttpResponseMessage hrm = client.PostAsJsonAsync(new Uri(Common.apiUrl_Login), para).Result;
            //HttpResponseMessage hrm = client.PostAsJsonAsync(new Uri("http://192.168.0.7:9994/uc/active"), para).Result;
            string str = hrm.Content.ReadAsStringAsync().Result;
            //LogHelper.Info("str=" + str);
            DataCenterResult result = Common.GetDataCenterResult(str);
            return result;
        }
        /// <summary>
        /// 调用用户中心接口
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="signName"></param>
        /// <param name="functionName"></param>
        /// <param name="parasObj"></param>
        /// <param name="appList">当前用户所拥有的api信息集合</param>
        /// <returns></returns>
        public static DataCenterResult PostUserCenter(string userName, string signName, string functionName, object parasObj, List<Apps> appList)
        {

            string appPwd = "";
            string appKey = "";
            string apiUrl = "";
            FxtApiCommon.GetNowApiInfo(Common.appId, appList, out apiUrl, out appPwd, out appKey);
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
            //apiUrl = "http://192.168.0.7:9994/uc/active";
            HttpResponseMessage hrm = client.PostAsJsonAsync(apiUrl, para).Result;
            string str = hrm.Content.ReadAsStringAsync().Result;
            DataCenterResult result = Common.GetDataCenterResult(str);
            return result;
        }
    }
}
