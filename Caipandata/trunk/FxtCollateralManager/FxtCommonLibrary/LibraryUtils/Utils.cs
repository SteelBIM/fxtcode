using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
/**
 * 作者:李晓东
 * 摘要:2014.02.13 新建
 *      2014.02.18 修改人:李晓东
 *                 新增:DeleteFile删除文件,ObjectTypeValue获得引用类型中某个值
 *      2014.02.26 修改人:李晓东
 *                 新增:MD5,GetWcfCode,GetJObjectValue 方法
 * **/
namespace FxtCommonLibrary.LibraryUtils
{
    public static class Utils
    {
        public static string CommonKey = DateTime.Now.ToString("yyyy-MM-dd");
        /// <summary>
        /// 反序列化字符串
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="val">字符串</param>
        /// <returns>反序列化对象</returns>
        public static T Deserialize<T>(string val)
        {
            return JsonConvert.DeserializeObject<T>(val);
        }

        /// <summary>
        /// 序列化字符串
        /// </summary>
        /// <param name="obj">任意对象</param>
        /// <returns>字符串</returns>
        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 公用是否为空
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(string str)
        {
            return string.IsNullOrEmpty(str);
        }
        /// <summary>
        /// 任意变量、值处理
        /// </summary>
        /// <param name="obj">值</param>
        /// <returns>string</returns>
        public static string ObjectIsNull(object obj)
        {
            return obj == null ? "" : obj.ToString();
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path">文件地址+文件名称</param>
        public static void DeleteFile(string path)
        {
            File.Delete(path);
        }
        /// <summary>
        /// 删除指定目录
        /// </summary>
        /// <param name="path">目录地址</param>
        public static void DeleteDir(string path)
        {
            Directory.Delete(path);
        }

        /// <summary>
        /// 获得引用类型中某个值
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="obj">对象</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns></returns>
        public static T ObjectTypeValue<T>(object obj, string propertyName)
        {
            return (T)obj.GetType().GetProperty(propertyName).GetValue(obj, null);
        }

        /// <summary>
        /// 获得虚拟目录
        /// </summary>
        /// <param name="file">文件</param>
        /// <returns></returns>
        public static string ServerMapPath(string file)
        {
            return System.Web.HttpContext.Current.Server.MapPath(file);
        }

        /// <summary>
        /// 获得指定转换格式日期时间
        /// </summary>
        /// <param name="format">转换格式,如:yyyy-MM-dd</param>
        /// <param name="day">得到指定天数的日期时间</param>
        /// <returns></returns>
        public static string GetDateTime(string format = null, int day = 0)
        {
            DateTime dt = day.Equals(0) ? DateTime.Now : DateTime.Now.AddDays(day);
            if (!IsNullOrEmpty(format))
                return dt.ToString(format);
            return dt.ToString();
        }

        /// <summary>
        /// 获得指定目录下的所有文件
        /// </summary>
        /// <param name="path">目录地址</param>
        /// <returns></returns>
        public static string[] GetFils(string path)
        {
            return Directory.GetFiles(path);
        }

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="path">目录地址</param>
        public static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        /// <summary>
        /// 解析服务端JSON中的指定值
        /// </summary>
        /// <param name="obj">字符串</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static string GetJObjectValue(object obj, string key)
        {
            if (obj != null && !obj.Equals("null") && !obj.Equals("[]"))
            {
                JObject _jobject = JObject.Parse(obj.ToString());
                return _jobject[key].ToString();
            }
            return "";
        }

        /// <summary>
        /// 获得服务请求密匙
        /// </summary>
        /// <param name="strCode">字符串</param>
        /// <returns></returns>
        public static string GetWcfCode(string strCode)
        {
            string strDay = CommonKey;
            string strDayReverse = string.Empty;
            IEnumerable<char> iableReverse = strDay.ToString().Reverse();
            foreach (char chReverse in iableReverse)
            {
                strDayReverse += chReverse;
            }
            return GetMd5(string.Format("{0}{1}{2}", strDay, strCode, strDayReverse));
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="strmd5">MD5加密字符串</param>
        /// <returns></returns>
        public static string GetMd5(string strmd5)
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
        /// 编码
        /// </summary>
        /// <param name="originalText"></param>
        /// <returns></returns>
        public static string EncodeField(this string originalText)
        {
            if (originalText == null)
            {
                return null;
            }

            return HttpUtility.UrlEncode(originalText).Replace("+", "%20");
        }
        /// <summary>
        /// 解码
        /// </summary>
        /// <param name="originalText"></param>
        /// <returns></returns>
        public static string DecodeField(this string originalText)
        {
            if (originalText == null)
            {
                return null;
            }

            return HttpUtility.UrlDecode(originalText).Replace("%20", "+");
        }
    }
}
