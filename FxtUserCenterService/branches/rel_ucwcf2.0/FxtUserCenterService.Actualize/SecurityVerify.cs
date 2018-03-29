using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity;
using CAS.Common;
using Newtonsoft.Json.Linq;
using FxtUserCenterService.Logic;
using FxtUserCenterService.Entity;

namespace FxtUserCenterService.Actualize
{
    public class SecurityVerify
    {
        /// <summary>
        /// 接口安全验证
        /// </summary>
        /// <param name="certifyargs">验证参数</param>
        /// <param name="funargs">功能参数</param>
        /// <returns></returns>
        public static WCFJsonData ApiBaseVerify(string sinfo, string info)
        {
            WCFJsonData jsonData = new WCFJsonData();
            //验证参数不能为空
            if (string.IsNullOrEmpty(sinfo) || string.IsNullOrEmpty(info))
            {
                jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "非法访问");
            }
            else
            {
                string code = string.Empty;//加密后参数
                JObject objCertify = null;//验证对象
                JObject objFun = null;//功能对象


                try
                {
                    JObject objSinfo = JObject.Parse(sinfo);
                    objSinfo.Remove("funname");
                    objCertify = JObject.Parse(sinfo);
                    objFun = JObject.Parse(info);
                    string[] certifyArray = new string[5];


                    Dictionary<string, string> errList = new Dictionary<string, string>();
                    string[] request = WcfConst.WcfApiSecurity;

                    #region certifyargs不为空验证
                    for (int i = 0; i < request.Length; i++)
                    {
                        if (objCertify[request[i]] == null || string.IsNullOrEmpty(objCertify[request[i]].ToString()))
                        {
                            errList.Add(request[i], "必传参数");
                        }
                        else
                        {
                            if (request[i] != "code")
                            {
                                certifyArray[i] = objCertify[request[i]].ToString();
                            }
                            else
                            {
                                code = objCertify[request[i]].ToString();
                            }
                        }
                    }

                    #endregion

                    #region 参数值匹配验证
                    if (errList.Count > 0)
                    {
#if DEBUG
                        jsonData = JSONHelper.GetWcfJson(null, (int)EnumHelper.Status.None, "必传参数缺失", errList);
#else
                        jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "非法访问");
#endif
                    }
                    else
                    {
                        //这里要加上平台slaptype，不同的平台有不同的功能配置 kevin 20140330
                        CompanyProductApp companyInfo = CompanyProductAppBL.GetAppkey(StringHelper.TryGetInt(certifyArray[0]), certifyArray[2], int.Parse(objFun["appinfo"]["systypecode"] == null ? "0" : objFun["appinfo"]["systypecode"].ToString()), certifyArray[4], certifyArray[1], objFun["appinfo"]["splatype"].ToString());// 
                        if (companyInfo == null)
                        {
                            jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "Appid,Apppwd 不存在");
                        }
                        else
                        {
                            string appkey = companyInfo.appkey;
                            //string functionname = objSinfo.Value<string>("functionname");
                            //string type = func.Value<string>("type");
                            //if (!string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(functionname))
                            //{
                            //    certifyArray[4] = type;
                            //}
                            //else
                            //{
                            //    certifyArray[4] = functionname;
                            //}

                            int dateVeify = Validator.ApiFunctionArgsVerify(certifyArray, code, appkey);

                            switch (dateVeify)
                            {
                                case -1:
                                   #if DEBUG
                                    jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.None, "加密字符串不匹配");
                                #else
                                    jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "非法访问");
                                #endif
                                    break;
                                case 0:
                                    #if DEBUG
                                    jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.None, "时间误差过大");
#else
                            jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "您所设置的时间与北京时间不符，请将时间设置为北京时间："+DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
#endif
                                    break;
                                case 1://执行功能

                                    jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.Success, "Success");
                                    break;
                            }
                        }
                    }


                    #endregion

                    #region token 校验
                    if (objFun["uinfo"]["token"] !=null && new string[] { "ios", "android" }.Contains(objFun["uinfo"]["token"].ToString().ToLower()))
                    {
                        var reusult =  MobilePushBL.VerifyToken(objFun["uinfo"]["token"].ToString());

                        if (reusult <= 0) {
#if DEBUG
                            jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.None, "token效验失败");
#else
                           jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "非法访问");
#endif

                            // jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.None, "token效验失败");
                        }
                       
                    }
                    #endregion


                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                    jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "非法访问");
                    return jsonData;
                }
            }
            return jsonData;
        }

        /// <summary>
        /// 接口安全验证
        /// </summary>
        /// <param name="certifyargs">验证参数</param>
        /// <param name="funargs">功能参数</param>
        /// <returns></returns>
        public static WCFJsonData ApiLoginVerify(string sinfo, string info)
        {
            WCFJsonData jsonData = new WCFJsonData();
            //sinfomd5加密key  
            if (string.IsNullOrEmpty(sinfo) || string.IsNullOrEmpty(info))
            {
                jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "用户或密码错误");
            }
            else
            {
                string code = string.Empty;//加密后参数
                SecurityInfo securInfo = JSONHelper.JSONToObject<SecurityInfo>(sinfo);
                ParametersInfo paramInfo = JSONHelper.JSONToObject<ParametersInfo>(info);

                try
                {
                    Dictionary<string, string> errList = new Dictionary<string, string>();
                    #region 将参数值为空的参数添加到errList
                    if (string.IsNullOrEmpty(securInfo.time))
                    {
                        errList.Add("time", "必传参数");
                    }
                    if (string.IsNullOrEmpty(securInfo.code))
                    {
                        errList.Add("code", "必传参数");
                    }

                    if (string.IsNullOrEmpty(paramInfo.uinfo.username))
                    {
                        errList.Add("uinfo.username", "必传参数");
                    }
                    if (string.IsNullOrEmpty(paramInfo.uinfo.password))
                    {
                        errList.Add("uinfo.password", "必传参数");
                    }

                    #endregion

                    if (errList.Count > 0)
                    {
#if DEBUG
                        jsonData = JSONHelper.GetWcfJson(null, (int)EnumHelper.Status.None, "必传参数缺失", errList);
#else
                        jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "用户或密码错误");
#endif
                    }
                    else
                    {
                        #region 参数值匹配验证
                        int dateVeify = Validator.CheckSysCode(securInfo.time, securInfo.code);
                        switch (dateVeify)
                        {
                            case -1:
#if DEBUG
                                jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.None, "加密字符串不匹配");
#else
                                jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "用户或密码错误");
#endif
                                break;
                            case 0:
#if DEBUG
                                jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.None, "时间误差过大");
#else
                                jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "您所设置的时间与北京时间不符，请将时间设置为北京时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
#endif
                                break;
                            case 1:
                                if (paramInfo.funinfo==null) { paramInfo.funinfo = new FunInfo(); paramInfo.funinfo.weburl = ""; }
                                jsonData = UserBL.GetUserAndAppInfo(null, paramInfo.uinfo.password, StringHelper.TryGetInt(paramInfo.appinfo.systypecode), paramInfo.uinfo.username, paramInfo.appinfo.stype, paramInfo.funinfo.weburl, paramInfo.appinfo.splatype, paramInfo.appinfo.channel);
                                break;
                        }


                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                    jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "用户或密码错误");
                    return jsonData;
                }
            }
            return jsonData;
        }


    }
}
