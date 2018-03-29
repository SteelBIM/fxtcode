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
using System.Web;
using System.Text.RegularExpressions;
using FxtUserCenterService.Logic;
using OpenPlatform.Framework.FlowMonitor;
using FxtUserCenterService.Entity;
using System.Threading.Tasks;

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
           // sinfo = HttpUtility.UrlDecode(sinfo); info = HttpUtility.UrlDecode(info);
           //Logger.WriteLog(sinfo, info, DateTime.Now.ToString("yyyyMMdd"));
            WCFJsonData jsonData = new WCFJsonData();
            //安全认证
            jsonData = SecurityVerify.ApiBaseVerify(sinfo, info); 
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
                    var actualMethodName = MethodDictionary.GetMethodInfo(fucName);// GetActualMethodName(fucName);//返回配置中对应的真实方法名
                    if (string.IsNullOrWhiteSpace(actualMethodName))
                    {
                        return JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "非法访问(10002)");
                    }

                    string[] noWriteFucnames = { "companysix", "companythirteen" };
                    if (!noWriteFucnames.Contains(fucName))
                    {
                        JObject appinfo = objinfo["appinfo"] as JObject;
                        int systypecode = appinfo.Value<int>("systypecode");
                        string signName = objSinfo["signname"].ToString();
                        CompanyInfo company = CompanyBL.GetCompanyBySignName(signName);
                        string temps = "{\"info\":\"" + info + "\",\"sinfo\":\"" + sinfo + "\"}";
                        //var isOverflow = Api.Flow.OverflowUserCenter(company.companyid, DateTime.Now, ApiType.UserCenterApi, -1, functionName: fucName, productTypeCode: systypecode, requestParameter: temps);
                        Task.Run(() => OperateLogBL.InsertApiInvokeLogUsercenter(company.companyid, fucName, (int)ApiType.UserCenterApi, systypecode, "", temps));
                    }

                    var type = typeof(Implement);
                    var obj = Activator.CreateInstance(type);
                    var methodParams = new object[2] { sinfo, info };
                    jsonData = type.GetMethod(actualMethodName).Invoke(obj, methodParams) as WCFJsonData;
                    
                }
                return jsonData;
            }
            catch (Exception ex)
            {
				 ex.Source += string.Format("sinfo:{0}\r\ninfo:{0}\r\n",sinfo,info);
                LogHelper.Error(ex);
#if DEBUG
                jsonData = JSONHelper.GetWcfJson(ex,(int)EnumHelper.Status.Failure, ex.Message);
#else
                jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "非法访问(10001)");
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
            sinfo = HttpUtility.UrlDecode(sinfo); info = HttpUtility.UrlDecode(info);
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
                jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "用户或密码错误(10014)");
#endif
                //jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "非法访问");
                return jsonData;
            }
        }

        /// <summary>
        /// 招行审贷平台登录接口
        /// </summary>
        /// <param name="sinfo"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public WCFJsonData CmbUserLogin(string sinfo, string info)
        {
            sinfo = HttpUtility.UrlDecode(sinfo); info = HttpUtility.UrlDecode(info);
            // Logger.WriteLog(sinfo, info, DateTime.Now.ToString("yyyyMMdd"));

            WCFJsonData jsonData = new WCFJsonData();
            try
            {
                jsonData = SecurityVerify.CmbApiLoginVerify(sinfo, info);

                return jsonData;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
#if DEBUG
                jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, ex.Message);
#else
                jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "用户或密码错误(10015)");
#endif
                //jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "非法访问");
                return jsonData;
            }
        }

        /// <summary>
        /// 联系我们
        /// </summary>
        /// <param name="tl">标题</param>
        /// <param name="ct">内容</param>
        /// <param name="to">接收方</param>
        /// <returns></returns>
        public WCFJsonData ConnectUs(string tl, string ct, string to, string callback)
        {
            WCFJsonData jsonData = new WCFJsonData();
            try
            {
                tl = HttpUtility.UrlDecode(tl);
                ct = HttpUtility.UrlDecode(ct);
                string recivemail = string.Empty;
                switch (to)
                {
                    case "applytry":
                        recivemail = WebCommon.GetConfigSetting("applytrymail");
                        break;
                    default:
                        break;
                }
                //根据密码错误有无，判断是否要调用数据库查询
                MailHelper.SendEmail(recivemail
                   , WebCommon.GetConfigSetting("sendmail")
                   , tl
                   , ct
                   , WebCommon.GetConfigSetting("mailaccount")
                   , WebCommon.GetConfigSetting("mailpassword")
                   , WebCommon.GetConfigSetting("smtpserver")
                   , StringHelper.TryGetInt(WebCommon.GetConfigSetting("smtpport")));
               jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.Success, "Success");
                return jsonData;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
#if DEBUG
                jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, ex.Message);
#else
                jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "发件箱账号或密码错误");
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
