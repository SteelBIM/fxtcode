using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Common;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using OpenPlatform.Framework.FlowMonitor;

namespace FxtCenterService.Actualize
{
    public class DataCenterCommon
    {
        /// <summary>
        /// 公共属性
        /// </summary>
        /// <param name="funinfo"></param>
        /// <returns></returns>
        public static SearchBase InitSearBase(JObject funinfo)
        {
            SearchBase search = new SearchBase();
            search.CityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            search.PageIndex = StringHelper.TryGetInt(funinfo.Value<string>("pageindex"));
            //分页参数
            if (search.PageIndex > 0)
            {
                search.Page = true;
                string pagerecords = funinfo.Value<string>("pagerecords");
                if (!string.IsNullOrEmpty(pagerecords))
                    search.PageRecords = StringHelper.TryGetInt(pagerecords);
            }
            //排序参数
            string sortname = funinfo.Value<string>("sortname");
            if (!string.IsNullOrEmpty(sortname))
                search.OrderBy = sortname;

            string sortorder = funinfo.Value<string>("sortorder");
            if (!string.IsNullOrEmpty(sortorder))
                search.OrderBy += " " + sortorder;
            if (!string.IsNullOrEmpty(funinfo.Value<string>("orderby")))
            {
                search.OrderBy = funinfo.Value<string>("orderby");
            }
            return search;
        }

        /// <summary>
        /// 通过反射找方法
        /// </summary>
        /// <param name="type">代理方法名</param>
        /// <returns></returns>
        public static MethodInfo GetMethodInfo(string type)
        {
            Type objectCon = typeof(DataController);
            string methName = MethodDictionary.MethodDic[type];
            MethodInfo methInfo = objectCon.GetMethod(methName);
            return methInfo;
        }

        /// <summary>
        /// 进行MD5效验
        /// </summary>
        /// <param name="strmd5"></param>
        /// <returns></returns>
        public static string GetMd5(string strmd5)
        {
            byte[] md5Bytes = ASCIIEncoding.Default.GetBytes(strmd5);
            byte[] encodedBytes;
            MD5 md5;
            md5 = new MD5CryptoServiceProvider();
            //FileStream fs= new FileStream(filepath,FileMode.Open,FileAccess.Read);
            encodedBytes = md5.ComputeHash(md5Bytes);
            string nn = BitConverter.ToString(encodedBytes);
            nn = Regex.Replace(nn, "-", "");//因为转化完的都是34-2d这样的，所以替换掉- 
            nn = nn.ToLower();//根据需要转化成小写
            //fs.Close();
            return nn;
        }

        /// <summary>
        /// 得到WCF需要的验证码
        /// </summary>
        /// <returns></returns>
        public static string GetCode(string strCode)
        {
            string strDay = DateTime.Now.ToString("yyyy-MM-dd");
            string strDayReverse = string.Empty;
            IEnumerable<char> iableReverse = strDay.Reverse();
            foreach (char chReverse in iableReverse)
            {
                strDayReverse += chReverse;
            }
            return GetMd5(string.Format("{0}{1}{2}", strDay, strCode, strDayReverse));
        }
        /// <summary>
        /// 检查Api与functionname是否与当前匹配
        /// </summary>
        /// <param name="Appid"></param>
        /// <param name="functionName"></param>
        /// <returns></returns>
        public static bool VerifyAppidAndFunctionnanmeIsMatch(string Appid, string functionName)
        {
            bool isMatch = false;
            try
            {
                if (Appid != ConstCommon.dataCenterApiCode.ToString())
                {
                    isMatch = true;
                }

                if (!MethodDictionary.MethodDic.ContainsKey("functionName"))
                {
                    isMatch = true;
                }


                return isMatch;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    /// <summary>
    /// 流量过滤
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class OverflowAttrbute : Attribute
    {
        /// <summary>
        /// 过滤类型
        /// </summary>
        public ApiType Type;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiType">过滤类型</param>
        public OverflowAttrbute(ApiType apiType)
        {
            Type = apiType;
        }
    }
}
