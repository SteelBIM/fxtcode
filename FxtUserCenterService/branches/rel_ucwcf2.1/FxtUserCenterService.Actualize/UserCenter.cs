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
                    if (string.IsNullOrWhiteSpace(actualMethodName))
                    {
                        return JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "非法访问");
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
                jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "用户或密码错误");
                #endif
                //jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "非法访问");
                return jsonData;
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="d">账号</param>
        /// <param name="um">原密码:desc</param>
        /// <param name="np">新密码：明文</param>
        /// <returns></returns>
        public WCFJsonData UpdatePassWord(string d, string um, string np)
        {
            WCFJsonData jsonData = new WCFJsonData();
            try
            {
                //根据密码错误有无，判断是否要调用数据库查询
                string oldpwd = np;
                np = EncryptHelper.TextToPassword(np);
                if (SimplePassWordBL.CheckIsSimplePassWord(oldpwd) == 1)
                {
                    return JSONHelper.GetWcfJson((int)EnumHelper.Status.PassWordIsExistsSimplePassWord, ConstCommon.WcfNewPassWordIsNotSafe);
                }
                else
                {
                    int index = Implement.UpdatePassWord(d, np, um);
                    if (index == 0)//账号与原密码不匹配
                    {
                        jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "用户或原密码错误");
                    }
                    else
                    {
                        jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.Success, "密码修改成功");
                    }
                }
                return jsonData;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
#if DEBUG
                jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, ex.Message);
#else
                jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "用户或密码错误");
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
