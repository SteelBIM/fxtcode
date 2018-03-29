using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace FxtDataAcquisition.Common
{
    public static class JsonHelp
    {
        public static string ToJson(this object obj)
        {
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return JsonConvert.SerializeObject(obj, timeFormat);
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
        public static string GetApiJson(int type, string message, object data = null, int count = 0)
        {

            var json = new
            {
                type = type,
                message = message,
                data = data,
                count = count
            };
            return ToJson(json);
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
        /// <summary>
        /// 将对象序列化成JSON格式字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJSONjss(this object obj)
        {
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //return serializer.Serialize(obj);
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return JsonConvert.SerializeObject(obj, timeFormat);
        }

        /// <summary>
        /// 将对象序列化成JSON格式字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJSONjss(this object obj,bool nceLoopHandling = true)
        {
            ReferenceLoopHandling handling = nceLoopHandling ? ReferenceLoopHandling.Serialize : ReferenceLoopHandling.Ignore;
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            List<JsonConverter> jcs = new List<JsonConverter>(){
                timeFormat
            };
            JsonSerializerSettings Settings =new JsonSerializerSettings
            {
                //这句是解决问题的关键,也就是json.net官方给出的解决配置选项.                 
                ReferenceLoopHandling = handling,
                Converters = jcs
            };

            return JsonConvert.SerializeObject(obj, Settings);
        }

        /// <summary>
        /// 将JSON格式字符串反序列化成相应的T类型对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T ParseJSONjss<T>(this string str)
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
            lst = JsonConvert.DeserializeObject<List<T>>(str);
            return lst;
        }
    }
}
