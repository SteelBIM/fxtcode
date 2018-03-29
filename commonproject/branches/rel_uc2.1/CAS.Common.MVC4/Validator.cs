using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Mail;
using CAS.Entity;

namespace CAS.Common.MVC4
{
    public class Validator
    {
        public static bool HasInvalidCtrlChar(string input)
        {
            Regex rex = new Regex(@"[\x01-\x08\x0B-\x0C\x0E-\x1F\x7F-\x84\x86-\x9F]");
            return rex.IsMatch(input);
        }

        /// <summary>
        /// Regex:^[\da-zA-Z_@ ]{1,100}$
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsValidChar(string value)
        {
            if (string.IsNullOrEmpty(value)) value = "";
            bool result = Regex.IsMatch(value, @"^[\da-zA-Z_@ ]{1,100}$");

            return result;
        }

        /// <summary>
        /// Regex:^[\d]{7,12}$
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsPhoneNumber(string value, bool isRequired)
        {
            bool result = Regex.IsMatch(value, @"^[\d]{7,12}$", RegexOptions.IgnoreCase);
            if (!isRequired)
            {
                result = string.IsNullOrEmpty(value) || result;
            }
            return result;
        }
        /// <summary>
        /// Regex:^0|1$
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsFlagNum(string value)
        {
            return Regex.IsMatch(value, @"^0|1$");
        }

        /// <summary>
        /// by int.TryParse
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInt(string value)
        {
            bool result = false;
            int i = 0;
            result = int.TryParse(value, out i);
            return result;
        }

        /// <summary>
        /// Regex:^\d{1,9}$
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsPrice(string value)
        {
            return Regex.IsMatch(value, @"^\d{1,9}$");
        }
        public static bool IsPrice(string value, bool isRequired)
        {
            bool result = Regex.IsMatch(value, @"^\d{1,9}$");
            if (!isRequired)
            {
                result = string.IsNullOrEmpty(value) || result;
            }
            return result;
        }
        
        /// <summary>
        /// by int.TryParse
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInt(string value, bool isRequired)
        {
            bool result = IsInt(value);
            if (!isRequired)
            {
                result = string.IsNullOrEmpty(value) || result;
            }
            return result;
        }
        /// <summary>
        /// by float.TryParse
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsFloat(string value)
        {
            bool result = false;
            float f = 0;
            result = float.TryParse(value, out f);
            return result;
        }

        /// <summary>
        /// IsInt(value) or IsFloat(value)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumeric(string value)
        {
            return IsInt(value) || IsFloat(value);
        }

        public static bool IsTime(string value, bool isRequired)
        {
            bool result = Regex.IsMatch(value, @"^((2[0123])|([01]\d)|\d):(([0-5]\d)|\d)(:(([0-5]\d)|\d))(\s?[AM,PM]{0,2})?$", RegexOptions.IgnoreCase);
            if (!isRequired)
            {
                result = string.IsNullOrEmpty(value) || result;
            }
            return result;
        }

        public static bool IsShortTime(string value, bool isRequired)
        {
            bool result = Regex.IsMatch(value, @"^((2[0123])|([01]\d)|\d):(([0-5]\d)|\d)(\s?[AM,PM]{0,2})?$", RegexOptions.IgnoreCase);
            if (!isRequired)
            {
                result = string.IsNullOrEmpty(value) || result;
            }
            return result;
        }

        public static bool IsDateTime(string value, bool isRequired)
        {
            bool result = Regex.IsMatch(value, @"^((((19|20)(([02468][048])|([13579][26]))-02-29))|((20[0-9][0-9])|(19[0-9][0-9]))-((((0[1-9])|(1[0-2]))-((0[1-9])|(1\d)|(2[0-8])))|((((0[13578])|(1[02]))-31)|(((01,3-9])|(1[0-2]))-(29|30)))))\s((2[0123])|([01]\d)|\d):(([0-5]\d)|\d)(:(([0-5]\d)|\d))(\s?[AM,PM]{0,2})?$", RegexOptions.IgnoreCase);
            if (!isRequired)
            {
                result = string.IsNullOrEmpty(value) || result;
            }
            return result;
        }

        /// <summary>
        /// 是否为日期型字符串
        /// </summary>
        /// <param name="StrSource">日期字符串(2008-05-08)</param>
        /// <returns></returns>
        public static bool IsDate(string value, bool isRequired)
        {
            bool result = Regex.IsMatch(value, @"^((?!0000)[0-9]{4}-((0[1-9]|1[0-2])-(0[1-9]|1[0-9]|2[0-8])|(0[13-9]|1[0-2])-(29|30)|(0[13578]|1[02])-31)|([0-9]{2}(0[48]|[2468][048]|[13579][26])|(0[48]|[2468][048]|[13579][26])00)-02-29)$", RegexOptions.IgnoreCase);
            if (!isRequired)
            {
                result = string.IsNullOrEmpty(value) || result;
            }
            return result;            
        }
 
        public static bool IsIP(string value, bool isRequired)
        {
            bool result = Regex.IsMatch(value, @"^(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])$", RegexOptions.IgnoreCase);
            if (!isRequired)
            {
                result = string.IsNullOrEmpty(value) || result;
            }
            return result;
        }

        public static bool IsEmail(string value, bool isRequired)
        {
            bool result = true;
            bool isEmpty = string.IsNullOrEmpty(value);
            if (!isEmpty)
            {
                try
                {
                    string e = (new MailAddress(value)).Address;
                    result = true && value.Length < 51;
                }
                catch (FormatException)
                {
                    result = false;
                }
            }
            if (!isRequired)
            {
                result = isEmpty || result;
            }

            return result;
        }

        ///// <summary>
        ///// 接口功能参数安全验证 1：验证通过，0：时间误差过大，-1：加密码字符串不匹配
        ///// </summary>
        ///// <param name="certifyArray">certifyArray：安全验证参数 {"appid", "signname", "apppwd", "time", "code","appkey"}</param>
        ///// <param name="type">type：功能名 </param>
        ///// <returns></returns>
        //public static int ApiFunctionArgsVerify(string[] certifyArray, string[] type)
        //{            
        //    string[] curArray = certifyArray;
        //    Array.Sort(certifyArray);
        //    Array.Copy(certifyArray, type, 1);
        //    string certify = string.Join("", certifyArray);
        //    string certifyCode = EncryptHelper.GetMd5(certify, curArray[5]);
        //    if (certifyCode != curArray[4])
        //    {
        //        return -1;
        //    }
        //    else

        //    {
        //        return CheckTime(type[3]);
        //    }
        //}
        /// <summary>
        /// 接口功能参数安全验证 1：验证通过，0：时间误差过大，-1：加密码字符串不匹配
        /// </summary>
        /// <param name="certifyArray">certifyArray：安全验证参数 {"appid", "signname", "apppwd", "time","appkey"}</param>
        /// <param name="type">type：功能名 </param>
        /// <returns></returns>
        public static int ApiFunctionArgsVerify(string[] certifyArray,string code,string appkey)
        {
            string strdate = certifyArray[3];
            string certifyCode = GetApiFunctionArgsVerifyCode(certifyArray, appkey);
            if (certifyCode != code)
            {
                return -1;
            }
            else
            {
                return 1;//CheckTime(strdate);//用于验证时间差，暂时去掉的原因是配合云查勘IOS通过验证
            }
        }
        /// <summary>
        /// 获取功能参数安全验证加密code
        /// </summary>
        /// <param name="certifyArray">certifyArray：加密所使用的参数 {"appid", "signname", "apppwd", "time","appkey"}</param>
        /// <param name="type">type:{functionname}</param>
        /// <returns></returns>
        public static string GetApiFunctionArgsVerifyCode(string[] certifyArray,string appkey)
        {
            string[] curArray = certifyArray;  
            Array.Sort(certifyArray);
            string certify = string.Join("", certifyArray);
            string certifyCode = EncryptHelper.GetMd5(certify, appkey);
            return certifyCode;
        }
        /// <summary>
        /// 检查验证码  1：验证通过，0：时间误差过大，-1：加密码字符串不匹配
        /// </summary>
        /// <param name="strDate">发送请求的时间</param>
        /// <param name="strCode">加密码</param>
        /// <returns></returns>
        public static int CheckSysCode(string strdate,string strcode)
        {
            string md5code =  EncryptHelper.GetMd5(strdate, ConstCommon.WcfLoginMd5Key);
            if (md5code!=strcode)
            {
                return -1;
            }
            else
            {
                return 1;//CheckTime(strdate);//用于验证时间差，暂时去掉的原因是配合云查勘IOS通过验证
            }
        }
        /// <summary>
        /// 检查验证码  0：时间误差过大
        /// </summary>
        /// <param name="strDate">发送请求的时间</param>
        /// <returns></returns>
        public static int CheckTime(string strdate) 
        {
            DateTime date = DateTime.ParseExact(strdate, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
            long ltime = DateTime.Now.ToFileTime() - date.ToFileTime();
            if (ltime > ConstCommon.WcfApiTimeOut || ltime < -ConstCommon.WcfApiTimeOut)//误差超过10分钟
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        #region 验证
        //        /// <summary>
        //        /// 验证必传参数
        //        /// </summary>
        //        /// <param name="functionname">功能方法</param>
        //        /// <param name="inputArgs">参数信息</param>
        //        /// <param name="validArgsName">必传参数名</param>
        //        /// <param name="outArgs">缺少必传参数时返回结果</param>
        //        /// <returns></returns>
        //        public static WCFJsonData WcfApiNeedArgs(string inputArgs, string funargs) 
        //        {
        //            WCFJsonData jsonData = new WCFJsonData();
        //            //验证参数不能为空
        //            if (string.IsNullOrEmpty(inputArgs) || string.IsNullOrEmpty(funargs))
        //            {
        //                jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "非法访问");
        //            }
        //            else
        //            {
        //                string code = string.Empty;//加密后参数
        //                JObject objCertify = null;//验证对象
        //                JObject objFun = null;//功能对象
        //                string functionname = string.Empty;

        //                try
        //                {
        //                    objCertify = JObject.Parse(inputArgs);
        //                    objFun = JObject.Parse(funargs);
        //                    string[] certifyArray = new string[] { };

        //                    Dictionary<string, string> errList = new Dictionary<string, string>();
        //                    string[] request = WcfConst.WcfApiSecurity;
        //                    #region certifyargs不为空验证
        //                    for (int i = 0; i < request.Length; i++)
        //                    {
        //                        if (objCertify[request[i]] == null || string.IsNullOrEmpty(objCertify[request[i]].ToString()))
        //                        {
        //                            errList.Add(request[i], "必传参数");
        //                        }
        //                        else
        //                        {
        //                            if (request[i] != "code")
        //                            {
        //                                certifyArray[i] = objCertify[request[i]].ToString();
        //                            }
        //                            else
        //                            {
        //                                code = objCertify[request[i]].ToString();
        //                            }
        //                        }
        //                    }
        //                    functionname = objFun["functionname"].ToString();
        //                    if (string.IsNullOrEmpty(funargs))
        //                    {
        //                        errList.Add("functionname", "必传参数");
        //                    }
        //                    #endregion

        //                    #region 参数值匹配验证
        //                    if (errList.Count > 0)
        //                    {
        //#if DEBUG
        //                        jsonData = JSONHelper.GetWcfJson(null, (int)EnumHelper.Status.None, "必传参数缺失", errList);
        //#else
        //                        jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "非法访问");
        //#endif
        //                    }
        //                    else
        //                    {
        //                        Array.Sort(certifyArray);
        //                    }
        //                    #endregion


        //                }
        //                catch (Exception ex)
        //                {
        //                    LogHelper.Error(ex);
        //                    jsonData = JSONHelper.GetWcfJson((int)EnumHelper.Status.Failure, "非法访问");
        //                    return jsonData;
        //                }
        //            }
        //            return jsonData;
        //        }
        #endregion
   
    }
}
