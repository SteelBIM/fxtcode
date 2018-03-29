using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using OpenPlatform.Framework.Utils;
using OpenPlatform.Framework.Utils.Log;

namespace OpenPlatform.Api.Infrastructure
{
    public class Token
    {
        private Token()
        {
        }

        public static string SurveryToken
        {
            get
            {
                var token = HttpContext.Current.Cache["surveytoken"];
                if (token == null)
                {
                    token = GetSurveyToken();
                    HttpContext.Current.Cache["surveytoken"] = token;
                }
                return token.ToString();
            }
        }

        public static void ResetSurveryToken()
        {
            HttpContext.Current.Cache["surveytoken"] = null;
        }

        /// <summary>
        /// 获取云查勘token
        /// </summary>
        /// <returns></returns>
        public static string GetSurveyToken()
        {
            try
            {
                dynamic yckObj = GetYckConfigs();
                var loginInfo = new SurveyApi.LoginInfoEntity
                {
                    Username = "admin@gjb",
                    PwdToSurvey = SurveyApi.GetPassWordMd5("admin@#$^&!!"),
                    Token = "",
                    Fxtcompanyid = 365
                };


                var sapi = new SurveyApi(loginInfo);
                sapi.UrlApi = sapi.UrlApi + "login/valid";
                sapi.body = new
                {
                    fxtCompanyId = loginInfo.Fxtcompanyid,
                    productTypeCode = yckObj.systypecode as string,
                    thirdpartyData = new
                    {
                        apps = new List<SurveyApi.Apps>
                        {
                            new SurveyApi.Apps
                            {
                               appid = yckObj.appid as string,
                               apppwd = yckObj.apppwd as string,
                               appurl= yckObj.apiurl as string,
                               appkey = yckObj.appkey as string

                            }
                        },
                        signName = yckObj.signname as string
                    }
                };
                var postData = sapi.GetJsonString(true);
                var result = ApiPostBack(sapi.UrlApi, postData, "application/json");
                var reslutData = JsonConvert.DeserializeObject<SurveyApi.ReturnData>(result);
                if (reslutData.returntype == 0 && reslutData.returntext != null)
                {
                    NLogHelper.Error(new ErrorLog { Exception = new Exception("云查勘登录接口调用出错:" + reslutData.returntext) });
                    return null;
                }
                var data = JsonConvert.DeserializeObject<SurveyApi.SurveyReturnData<Dictionary<string, object>>>(result);

                return data.body != null ? data.body["token"].ToString() : null;
            }
            catch (Exception ex)
            {
                NLogHelper.Error(new ErrorLog { Exception = new Exception("云查勘登录接口调用出错:" + ex.Message) });
                return null;
            }
        }

        /// <summary>
        /// 获取云查勘接口配置参数
        /// </summary>
        /// <returns></returns>
        private static dynamic GetYckConfigs()
        {
            dynamic yckObj = new ExpandoObject();
            yckObj.appid = ConfigurationHelper.YckAppId;
            yckObj.apppwd = ConfigurationHelper.YckAppWd;
            yckObj.systypecode = ConfigurationHelper.YckSysTypeCode;
            yckObj.signname = ConfigurationHelper.YckSignName;
            yckObj.appkey = ConfigurationHelper.YckAppKey;
            yckObj.apiurl = ConfigurationHelper.YckAppUrl;

            return yckObj;
        }

        /// <summary>
        /// http 请求响应
        /// </summary>
        /// <param name="url"></param>
        /// <param name="post"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        private static string ApiPostBack(string url, string post, string contentType)
        {
            byte[] postData = Encoding.UTF8.GetBytes(post);
            var client = new WebClient();
            client.Headers.Add("Content-Type", contentType);
            client.Headers.Add("ContentLength", postData.Length.ToString());
            //这里url要组装安全标记等参数
            string result;
            try
            {
                byte[] responseData = client.UploadData(url, "POST", postData);
                result = Encoding.UTF8.GetString(responseData);
                //找退出原因
                //LogHelper.Info(result);
            }
            catch (Exception ex)
            {
                result = JsonHelper.GetJson(null, 0, ex.Message, ex);
            }
            client.Dispose();
            return result;
        }
    }
}