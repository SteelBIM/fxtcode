using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using FxtSpider.DAL.LinqToSql;

namespace FxtSpider.Common
{
    /// <summary>
    /// 用于整个解决方案json格式处理
    /// </summary>
    public static class JsonHelp
    {
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
        /// <summary>
        /// 将JSON格式字符串反序列化成相应的T类型的List泛型集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<T> ParseJSONList<T>(this string str)
        {
            List<T> lst = new List<T>();

            if (string.IsNullOrEmpty(str))
            {
                return lst; 
            }
            lst= JsonConvert.DeserializeObject<List<T>>(str);
            //if (string.IsNullOrEmpty(str) || str == "[]")
            //{
            //    return lst;
            //}
            //if (str.Substring(0, 1) == "[")
            //{
            //    string s1 = str.Remove(str.Length - 1, 1).Remove(0, 1);
            //    str = s1.Replace("},{", "};{");
            //}
            //string[] strs = str.Split(';');
            //for (int i = 0; i < strs.Length; i++)
            //{
            //    T t = ParseJSONjss<T>(strs[i]);
            //    lst.Add(t);
            //}
            return lst;
        }

        /// <summary>
        /// 将JSON格式字符串反序列化成相应的T类型对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T ParseJSONjss<T>(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return default(T); ;
            }
            return JsonConvert.DeserializeObject<T>(str);
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //return serializer.Deserialize<T>(str);
        }
        /// <summary>
        /// 将对象序列化成JSON格式字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJSONjss(this object obj)
        {
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //return serializer.Serialize(obj);
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 对实体中字符串进行编码
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T EncodeField<T>(this T t)
        {
            if (t == null)
            {
                return t;
            }

            T _t = t;
            Type type = _t.GetType();
            PropertyInfo[] propertys = type.GetProperties();
            foreach (PropertyInfo property in propertys)
            {
                string propertyType = property.PropertyType.FullName;
                if (propertyType == "System.String")
                {
                    object propertyValue = property.GetValue(_t, null);
                    if (propertyValue != null)
                    {
                        property.SetValue(_t, EncodeField(Convert.ToString(propertyValue)), null);
                    }
                }

            }
            return _t;
        }
        /// <summary>
        /// 对实体中字符串进行解码
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T DecodeField<T>(this T t)
        {
            if (t == null)
            {
                return t;
            }

            T _t = t;
            Type type = _t.GetType();
            PropertyInfo[] propertys = type.GetProperties();
            foreach (PropertyInfo property in propertys)
            {
                string propertyType = property.PropertyType.FullName;
                if (propertyType == "System.String")
                {
                    object propertyValue = property.GetValue(_t, null);
                    if (propertyValue != null)
                    {
                        property.SetValue(_t, DecodeField(Convert.ToString(propertyValue)), null);
                    }
                }

            }
            return _t;
        }

        /// <summary>
        /// 对实体中字符串进行编码
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> EncodeField<T>(this List<T> t)
        {
            if (t == null)
            {
                return null;
            }
            List<T> _t = new List<T>();
            for (int i = 0; i < t.Count; i++)
            {
                _t.Add(EncodeField<T>(t[i]));
            }
            return _t;
        }
        /// <summary>
        /// 对实体中字符串进行解码
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> DecodeField<T>(this List<T> t)
        {
            if (t == null)
            {
                return null;
            }
            List<T> _t = new List<T>();
            for (int i = 0; i < t.Count; i++)
            {
                _t.Add(DecodeField<T>(t[i]));
            }
            return _t;
        }

        public static string ToJSONjss(this Dat_SpiderErrorLog obj)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"ID\":").Append(obj.ID).Append(",")
              .Append("\"CityId\":").Append(obj.CityId).Append(",")
              .Append("\"WebId\":").Append(obj.WebId).Append(",")
              .Append("\"Url\":\"").Append(obj.Url == null ? "" : obj.Url.EncodeField()).Append("\",")
              .Append("\"ErrorTypeCode\":").Append(obj.ErrorTypeCode).Append(",")
              .Append("\"CreateTime\":\"").Append(obj.CreateTime == null ? "" : obj.CreateTime.ToString().EncodeField()).Append("\",")
              .Append("\"Remark\":\"").Append(obj.Remark == null ? "" : obj.Remark.EncodeField()).Append("\"}");
            return sb.ToString();
        }
        public static string ToJSONjss(this List<Dat_SpiderErrorLog> objList)
        {
            StringBuilder sb = new StringBuilder();
            if (objList == null)
            {
                return "null";
            }
            sb.Append("[");
            foreach (Dat_SpiderErrorLog obj in objList)
            {
                sb.Append(obj.ToJSONjss()).Append(",");
            }

            return sb.ToString().TrimEnd(',') + "]";
        }
    }
}
