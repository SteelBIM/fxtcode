using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Xml.Serialization;
using System.Xml;
using Kingsun.Common;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;

namespace CourseActivate.Core.Utility
{
    public class JsonHelper
    {

        #region Newtonsoft.Json 推荐使用

        /// <summary>
        /// 普通加密JSON
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(object obj)
        {
            if (obj == null)
            {
                return "";
            }

            return JsonConvert.SerializeObject(obj, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
        }
        /// <summary>
        /// 普通解密JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T FromJson<T>(string jsonStr)
        {
            if (string.IsNullOrWhiteSpace(jsonStr)) return default(T);
            else
            {
                var jSetting = new JsonSerializerSettings();
                jSetting.NullValueHandling = NullValueHandling.Ignore;
                //jSetting.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                return JsonConvert.DeserializeObject<T>(jsonStr, jSetting);
            }
        }

        /// <summary>
        /// 时间高精度的json数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToTimePrecisionJson(object obj)
        {
            return null == obj ? "" : JsonConvert.SerializeObject(obj, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff" });
        }
        /// <summary>
        /// 时间高精度解密JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T FromJsonTimePrecision<T>(string jsonStr)
        {
            if (string.IsNullOrWhiteSpace(jsonStr))
            {
                return default(T);
            }
            else
            {
                var jSetting = new JsonSerializerSettings();
                jSetting.NullValueHandling = NullValueHandling.Ignore;
                jSetting.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff" });
                return JsonConvert.DeserializeObject<T>(jsonStr, jSetting);
            }
        }

        /// <summary>
        /// 普通加密JSON,忽略空
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJsonIgnoreNull(object obj)
        {
            if (obj == null)
            {
                return "";
            }

            CustomJsonConverter convert = new CustomJsonConverter(); //自定义转换器
            convert.PropertyNullValueReplaceValue = ""; //设置替换NULL值得字符串
            return JsonConvert.SerializeObject(obj, convert);
        }
        /// <summary>
        /// 普通解密JSON,忽略空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T FromJsonIgnoreNull<T>(string jsonStr)
        {
            if (string.IsNullOrWhiteSpace(jsonStr)) return default(T);
            else
            {

                CustomJsonConverter convert = new CustomJsonConverter(); //自定义转换器
                convert.PropertyNullValueReplaceValue = ""; //设置替换NULL值得字符串
                return JsonConvert.DeserializeObject<T>(jsonStr, convert);
            }
        }

        /// <summary>把字符串转换成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonValue"></param>
        /// <returns></returns>
        public static T GetObjFromJson<T>(string jsonValue, T obj)
        {
            try
            {
                return JsonConvert.DeserializeAnonymousType(jsonValue, obj);
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        /// <summary>把字符串转换成对象LIST
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonValue"></param>
        /// <returns></returns>
        public static List<T> JSONStringToList<T>(string JsonStr)
        {
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            List<T> objs = Serializer.Deserialize<List<T>>(JsonStr);
            return objs;
        }
        #endregion

        #region XmlObjectSerializer

        /// <summary> 
        /// Json序列化 
        /// </summary> 
        public static string JsonSerializer<T>(T obj)
        {
            string jsonString = string.Empty;
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));

                using (MemoryStream ms = new MemoryStream())
                {
                    serializer.WriteObject(ms, obj);
                    jsonString = Encoding.UTF8.GetString(ms.ToArray());
                }
            }
            catch
            {
                jsonString = string.Empty;
            }
            return jsonString;
        }

        /// <summary> 
        /// Json反序列化
        /// </summary> 
        public static T JsonDeserialize<T>(string jsonString)
        {
            T obj = Activator.CreateInstance<T>();
            try
            {
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
                {
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(obj.GetType());//typeof(T)
                    T jsonObject = (T)ser.ReadObject(ms);
                    ms.Close();

                    return jsonObject;
                }
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        #endregion

        /// <summary>
        /// 序列化XML
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string Serializer(Type type, object obj)
        {
            MemoryStream Stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(type);
            try
            {
                //序列化对象
                xml.Serialize(Stream, obj);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            Stream.Position = 0;
            StreamReader sr = new StreamReader(Stream);
            string str = sr.ReadToEnd();

            sr.Dispose();
            Stream.Dispose();

            return str;
        }

        public static XmlDocument JsonToXmlNode(string json)
        {
            return JsonConvert.DeserializeXmlNode(json);
        }

        #region 序列号 原有
        /// <summary>
        /// 对象JSON序列化接口
        /// </summary>
        /// <param name="obj">序列化对象</param>
        /// <returns></returns>
        public static string EncodeJson(object obj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = int.MaxValue;
            return serializer.Serialize(obj);
        }
        /// <summary>
        /// 对象反序列化接口
        /// </summary>
        /// <typeparam name="T">反序列化对象类型</typeparam>
        /// <param name="json">序列化字符串</param>
        /// <returns></returns>
        public static T DecodeJson<T>(string json) where T : new()
        {
            T obj;
            if (!String.IsNullOrEmpty(json))
            {
                
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = int.MaxValue;
                obj = (T)serializer.Deserialize(json, typeof(T));
                
            }
            else
            {
                obj = default(T);
            }
            return obj;
        }

        /// <summary>
        /// 对象JSON序列化接口
        /// </summary>
        /// <param name="obj">序列化对象</param>
        /// <returns></returns>
        public static string DeepEncodeJson(object obj)
        {
            DataContractJsonSerializer json = new DataContractJsonSerializer(obj.GetType());

            string szJson = "";

            //序列化

            using (MemoryStream stream = new MemoryStream())
            {

                json.WriteObject(stream, obj);

                szJson = Encoding.UTF8.GetString(stream.ToArray());

            }
            return szJson;
        }

        /// <summary>
        /// 对象反序列化接口
        /// </summary>
        /// <typeparam name="T">反序列化对象类型</typeparam>
        /// <param name="json">序列化字符串</param>
        /// <returns></returns>
        public static T DeepDecodeJson<T>(string json) where T : new()
        {
            T obj = default(T);
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {

                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));

                obj = (T)serializer.ReadObject(ms);

            }
            return obj;
        }
        #endregion

    }

    public class DateTimeConverter : JavaScriptConverter
    {

        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            if (string.IsNullOrEmpty(dictionary["Value"].ToString()))
                return null;

            return DateTime.Parse(dictionary["Value"].ToString());
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {

            IDictionary<string, object> result = new Dictionary<string, object>();
            if (obj == null)
                result["Value"] = string.Empty;
            else
                result["Value"] = ((DateTime)obj).ToString("yyyy-MM-dd HH:mm:ss");
            return result;
        }

        public override IEnumerable<Type> SupportedTypes
        {
            get { yield return typeof(DateTime); }
        }
    }
}
