using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace FxtDataAcquisition.API.Common
{
        /// <summary>
        /// API入口帮助类
        /// </summary>
        public static class EntranceHelper
        {
            public static XmlDocument ApiConfig = new XmlDocument();
            static EntranceHelper()
            {
                ApiConfig.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MatchClass.xml"));
            }
            /// <summary>
            /// 得到验证码
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
            /// 进行MD5效验
            /// </summary>
            /// <param name="strmd5"></param>
            /// <returns></returns>
            static string GetMd5(string strmd5)
            {
                byte[] md5Bytes = ASCIIEncoding.Default.GetBytes(strmd5);
                byte[] encodedBytes;
                MD5 md5 = new MD5CryptoServiceProvider();
                encodedBytes = md5.ComputeHash(md5Bytes);
                string nn = BitConverter.ToString(encodedBytes);
                nn = Regex.Replace(nn, "-", "");//因为转化完的都是34-2d这样的，所以替换掉- 
                nn = nn.ToLower();//根据需要转化成小写
                return nn;
            }
            /// <summary>
            /// 根据配置获取类
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            static MatchClass GetMatchClass(string key)
            {
                XmlNode xml = ApiConfig.SelectSingleNode("/Match/Class[@Key='" + key + "']");
                if (xml == null)
                {
                    return null;
                }
                string library = xml.Attributes["Library"].Value;
                string className = xml.Attributes["ClassName"].Value;
                return new MatchClass() { Key = key, ClassName = className, Library = library };
            }
            /// <summary>
            /// 根据配置获取方法
            /// </summary>
            /// <param name="type"></param>
            /// <param name="functionName"></param>
            /// <returns></returns>
            public static MethodInfo GetMethodInfo(string type, string functionName,out object objClass)
            {
               //return CAS.Common.EntranceHelper.GetMethodInfo(ApiConfig, type, functionName, out objClass);
                objClass = null;
                MatchClass mc = GetMatchClass(type);
                if (mc == null)
                {
                    return null;
                }
                Assembly ass = System.Reflection.Assembly.Load(mc.Library);
                objClass = System.Reflection.Assembly.Load(mc.Library).CreateInstance(mc.ClassName);
                if (objClass == null)
                {
                    return null;
                }
                XmlNode xml = ApiConfig.SelectSingleNode("/Match/Class[@Key='" + type + "']/Method[@Key='" + functionName + "']");
                if (xml == null)
                {
                    return null;
                }
                string methodName = xml.Attributes["MethodName"].Value;
                MethodInfo method = objClass.GetType().GetMethod(methodName);
                return method;
            }
        }
        public class MatchClass
        {
            /// <summary>
            /// 键
            /// </summary>
            public string Key { get; set; }
            /// <summary>
            /// 类库名称
            /// </summary>
            public string Library { get; set; }
            /// <summary>
            /// 类名
            /// </summary>
            public string ClassName { get; set; }
        }
    
}
