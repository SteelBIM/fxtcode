using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtUserCenterService.Contract;
using System.ServiceModel.Activation;
using CAS.Entity;
using CAS.Common;
using Newtonsoft.Json.Linq;
using FxtUserCenterService.Actualize.Impl;
using System.Xml;
using System.Configuration;

namespace FxtUserCenterService.Actualize
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class UserCenter : IUserCenter
    {
        /// <summary>
        /// 功能入口
        /// </summary>
        /// <param name="certifyargs"></param>
        /// <param name="funargs"></param>
        /// <returns></returns>
        public WCFJsonData Entrance(string sinfo, string info)
        {
           // Logger.WriteLog(sinfo, info, DateTime.Now.ToString("yyyyMMdd"));
            LogHelper.Info(sinfo); LogHelper.Info(info);
            WCFJsonData jsonData = new WCFJsonData();
            //安全认证
            jsonData = SecurityVerify.ApiBaseVerify(sinfo, info); LogHelper.Info(jsonData.ToJson());
            try
            {//调用
                JObject objSinfo = JObject.Parse(sinfo);
                /**
                 *以方法名为入口，通过反射去调用
                 *qiuyan 2014-03-16 
                 **/
                if (jsonData.returntype == 1 && objSinfo.Value<string>("funname") != "none")
                {
                    
                    JObject objinfo = JObject.Parse(info);
                    
                    var fucName = string.Empty ;
                    if (!string.IsNullOrEmpty(objSinfo.Value<string>("funname")))
                    {
                        fucName = objSinfo.Value<string>("funname");
                    }
                    else
                    {
                        fucName = objSinfo.Value<string>("functionname");
                    }
                    var actualMethodName = GetActualMethodName(fucName);//返回配置中对应的真实方法名
                    LogHelper.Info(actualMethodName);
                    if (string.IsNullOrWhiteSpace(actualMethodName))
                    {
                        return JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "非法访问");
                    }

                    var type = typeof(Implement);
                    var obj = Activator.CreateInstance(type);
                    var methodParams = new object[2] { sinfo, info };
                    jsonData = type.GetMethod(actualMethodName).Invoke(obj, methodParams) as WCFJsonData;
                    LogHelper.Info("methodParamsResult" + jsonData.ToJson());
                }
                return jsonData;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
#if DEBUG
                jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, ex.Message);
#else
                jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "非法访问");
#endif

                //jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "非法访问");
                return jsonData;
            }
        }

        /// <summary>
        /// 登录接口
        /// </summary>
        /// <param name="sinfo"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public WCFJsonData UserLogin(string sinfo, string info)
        {
           // Logger.WriteLog(sinfo, info, DateTime.Now.ToString("yyyyMMdd"));

            WCFJsonData jsonData = new WCFJsonData();
            try
            {
                jsonData = SecurityVerify.ApiLoginVerify(sinfo, info);

                return jsonData;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                #if DEBUG
                jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, ex.Message);
                #else
                jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "非法访问");
                #endif
                //jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "非法访问");
                return jsonData;
            }
        }

        /// <summary>
        /// 返回真实的方法名
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Func<string, string> GetActualMethodName = (key) =>
         {
             return ConfigurationManager.AppSettings[key] == null ? "" : ConfigurationManager.AppSettings[key].ToString();
         };

    }
}
