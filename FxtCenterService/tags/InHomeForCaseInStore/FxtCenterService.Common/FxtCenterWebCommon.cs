using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Common;
using Newtonsoft.Json.Linq;

namespace FxtCenterService.Common
{
    public class FxtCenterWebCommon
    {
        ///// <summary>
        ///// 调用用户中心的
        ///// </summary>
        ///// <param name="functionName">调用方法名</param>
        ///// <param name="funinfo">功能参数,用匿名对象</param>
        ///// <returns></returns>
        //public static string UserCenterApiPostData(string functionName, object funinfo)
        //{
        //    string appID = WebCommon.GetConfigSetting("FxtDataCenterAppID");//调用方ID
        //    string appPwd = WebCommon.GetConfigSetting("FxtDataCenterApppwd");//调用方（应用）密码
        //    string appKey = WebCommon.GetConfigSetting("FxtDataCenterAppKey");//调用方（应用）键
        //    string signName = WebCommon.GetConfigSetting("FxtDataCenterSignName");//公司标识
        //    string productTypeCode = WebCommon.GetConfigSetting("FxtDataCenterSystypeCode");//产品类型编号
        //    string callTime = DateTime.Now.ToString("yyyyMMddHHmmss");
        //    string[] pwdArray = { appID, appPwd, signName, callTime, functionName };
        //    string validCode = EncryptHelper.GetMd5(pwdArray, appKey);
        //    var postData = new
        //    {
        //        sinfo = new {
        //            appid = appID,
        //            apppwd = appPwd,
        //            signname = signName,
        //            time = callTime,
        //            functionname = functionName,
        //            code = validCode
        //        },
        //        info = new {
        //            uinfo = new {},
        //            appinfo = new {
        //                systypecode = productTypeCode,
        //                splatype = ""
        //            },
        //            funinfo = funinfo
        //        }
        //    };
        //    return postData.ToJson();
        //}

        /// <summary>
        /// Post用户中心的字符串数据
        /// 该处appID正常情况为1003104
        /// </summary>
        /// <param name="sinfo">验证信息</param>
        /// <param name="info">功能信息</param>
        /// <param name="execFunName">调用数据中心实际的功能方法</param>
        /// <param name="funinfo">实际调用的功能方法</param>
        /// <returns></returns>
        public static string UserCenterApiPostData(JObject sinfo, string productTypeCode, string execFunName, object funinfo)
        {
            JObject newsinfo = new JObject(sinfo);
            newsinfo.Add("funname", execFunName);
            var postData = new
            {
                sinfo = newsinfo.ToJson(),
                info = new
                {
                    uinfo = new { },
                    appinfo = new
                    {
                        systypecode = productTypeCode,
                        splatype = ""
                    },
                    funinfo = funinfo
                }.ToJson()
            };
            return postData.ToJson();
        }

        /// <summary>
        /// POST调用用户中心接口
        /// </summary>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string UserCenterApiPost(string postData)
        {
            return WebCommon.WcfApiMethodOfPost(WebCommon.WCFUserCenterService, postData);
        }
    }
}
