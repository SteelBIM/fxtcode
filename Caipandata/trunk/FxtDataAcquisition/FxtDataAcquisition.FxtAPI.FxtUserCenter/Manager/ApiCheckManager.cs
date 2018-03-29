using CAS.Common;
using CAS.Common.MVC4;
using CAS.Entity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.FxtAPI.FxtUserCenter.Manager
{
    public static class ApiCheckManager
    {
        /// <summary>
        /// 接口加密方式参数安全验证 1：验证通过，0：时间误差过大，-1：加密码字符串不匹配
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appPwd"></param>
        /// <param name="signName"></param>
        /// <param name="time"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        static int ApiCodeVerify(string appId, string appPwd, string signName, string time, string code)
        {
            string appKey = "test";
            int result = Validator.ApiFunctionArgsVerify(new string[] { appId, signName, appPwd, time, appKey },code ,null);
            return result;
        }
        /// <summary>
        /// 功能权限验证
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appPwd"></param>
        /// <param name="signName"></param>
        /// <param name="functionName"></param>
        /// <param name="appUrl"></param>
        /// <returns>获取到key</returns>
        static string ApiFunctionArgsVerify(string appId, string appPwd, string signName, string functionName, string appUrl)
        {
            string userCenterAppUrl = "";
            string userCenterAppId = "";
            string userCenterAppPwd = "";
            string userCenterSignName = "";
            string userCenterTime = "";
            string userCenterFunctionName = "";
            string userCenterFunctionParameter = "";
            string userCenterAppKey = "";
            string userCenterCode = Validator.GetApiFunctionArgsVerifyCode(new string[]{userCenterAppId, userCenterSignName
                , userCenterAppPwd, userCenterTime, userCenterAppKey}, null);

            return "";
        }
        /// <summary>
        /// API授权验证 1：验证通过，0：无权限
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="apppwd"></param>
        /// <param name="signname"></param>
        /// <param name="time"></param>
        /// <param name="functionName"></param>
        /// <param name="appUrl"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static int ApiAuthorizationVerify(JObject sinfoObj, JObject infoObj,string functionname,out UserCheck _uc)
        {
            _uc = null;
            string securityJson = WebCommon.ApiSecurityStringOfJson(sinfoObj, functionname, infoObj, "companysix");
            string returntext = "";
            int returntype = 1;
            _uc = WebCommon.FxtUserCenterService_GetCompanyBySignName(securityJson, out returntext, out returntype);
            if (returntype == 1)
            {
                return 1;
            }
            return 0;
            ////未获取到key
            //string appKey = ApiFunctionArgsVerify(appid, apppwd, signname, functionName, appUrl);
            //if (string.IsNullOrEmpty(appKey))
            //{
            //    return 0;
            //}
            ////加密验证
            //int result = ApiCodeVerify(appid, apppwd, signname, time, code);
            //if (result != 1)
            //{
            //    return 0;
            //}
            //return 1;
            
        }
    }
}
