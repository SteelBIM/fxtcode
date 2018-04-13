using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Reflection;

namespace CBSS.Core.Utility
{
    /// <summary>
    /// 向远程Url Post/Get数据类
    /// </summary>
    public class HttpHelper
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“HttpHelper.HttpPost<T>(string, object, SerializationType)”的 XML 注释
        public static T HttpPost<T>(string uri, object data, SerializationType serializationType)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“HttpHelper.HttpPost<T>(string, object, SerializationType)”的 XML 注释
        {
            string responseText = HttpPost(uri, data, serializationType);

            T t = default(T);
            if (serializationType == SerializationType.Xml)
            {
                t = responseText.FromXml<T>();
            }
            else if (serializationType == SerializationType.Json)
            {
                t = responseText.FromJson<T>();
            }
            return t;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“HttpHelper.HttpPost(string, object, SerializationType)”的 XML 注释
        public static string HttpPost(string uri, object data, SerializationType serializationType)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“HttpHelper.HttpPost(string, object, SerializationType)”的 XML 注释
        {
            HttpWebRequest request = HttpWebRequest.Create(uri) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";

            string dataStr = string.Empty;
            if (data is string)
            {
                dataStr = (string)data;
            }
            else
            {
                if (serializationType == SerializationType.Xml)
                {
                    dataStr = data.ToXml();
                    object o = XMLHelper.XmlDeserialize(data.GetType(), dataStr);
                }
                else if (serializationType == SerializationType.Json)
                {
                    request.ContentType = "application/json";
                    dataStr = data.ToJson();
                }
            }
            CNNWebClient wc = new CNNWebClient();
            wc.Timeout = 300;
            var t = wc.UploadData(uri, "POST", Encoding.UTF8.GetBytes(dataStr));
            string tText = Encoding.UTF8.GetString(t);
            /*
            System.Diagnostics.Debug.WriteLine(tText);
            byte[] buffer;
            Stream stream;
            if (!string.IsNullOrWhiteSpace(dataStr))
            {
                buffer = Encoding.UTF8.GetBytes(dataStr);
                stream = request.GetRequestStream();
                stream.Write(buffer, 0, buffer.Length);
            }
            //request.d
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            stream = response.GetResponseStream();
            buffer = new byte[response.ContentLength];
            stream.Read(buffer, 0, (int)response.ContentLength);
            //string encoding = string.IsNullOrWhiteSpace(response.ContentEncoding) ? "uft-8" : response.ContentEncoding;
            string responseText = Encoding.UTF8.GetString(buffer);
            System.Diagnostics.Debug.WriteLine(responseText);
            return responseText;
             */
            return tText;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“HttpHelper.HttpPost(string, NameValueCollection)”的 XML 注释
        public static string HttpPost(string uri, System.Collections.Specialized.NameValueCollection data)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“HttpHelper.HttpPost(string, NameValueCollection)”的 XML 注释
        {
            CNNWebClient wc = new CNNWebClient();
            wc.Encoding = Encoding.UTF8;
            wc.Timeout = 300;
            var t = wc.UploadValues(uri, "POST", data);
            string tText = Encoding.UTF8.GetString(t);
            return tText;
        }


#pragma warning disable CS1591 // 缺少对公共可见类型或成员“HttpHelper.HttpGet<T>(string, SerializationType)”的 XML 注释
        public static T HttpGet<T>(string uri, SerializationType serializationType)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“HttpHelper.HttpGet<T>(string, SerializationType)”的 XML 注释
        {
            string responseText = ModHttpGet(uri);

            T t = default(T);
            if (serializationType == SerializationType.Xml)
            {
                t = responseText.FromXml<T>();
            }
            else if (serializationType == SerializationType.Json)
            {
                t = responseText.FromJson<T>();
            }
            return t;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“HttpHelper.SetHeaderValue(WebHeaderCollection, string, string)”的 XML 注释
        public static void SetHeaderValue(WebHeaderCollection header, string name, string value)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“HttpHelper.SetHeaderValue(WebHeaderCollection, string, string)”的 XML 注释
        {
            var property = typeof(WebHeaderCollection).GetProperty("InnerCollection", BindingFlags.Instance | BindingFlags.NonPublic);
            if (property != null)
            {
                var collection = property.GetValue(header, null) as NameValueCollection;
                collection[name] = value;
            }
        }
        /// <summary>
        /// http get通用方法
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string HttpGet(string uri)
        {
            StringBuilder respBody = new StringBuilder();
            HttpWebRequest request = HttpWebRequest.Create(uri) as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            byte[] buffer = new byte[8192];
            Stream stream;
            stream = response.GetResponseStream();
            int count = 0;
            do
            {
                count = stream.Read(buffer, 0, buffer.Length);
                if (count != 0)
                    respBody.Append(Encoding.UTF8.GetString(buffer, 0, count));
            }
            while (count > 0);
            string responseText = respBody.ToString();
            return responseText;
        }
        /// <summary>
        /// GET方法，仅限MOD接口专用
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string ModHttpGet(string uri)
        {
            StringBuilder respBody = new StringBuilder();
            HttpWebRequest request = HttpWebRequest.Create(uri) as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            //小学100201初中100200
            SetHeaderValue(request.Headers, "appid", "100200");
            SetHeaderValue(request.Headers, "userflag:", "pc");
            SetHeaderValue(request.Headers, "platform:", "3");
            SetHeaderValue(request.Headers, "requestId:", new Random().Next(1, int.MaxValue).ToString());
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            byte[] buffer = new byte[8192];
            Stream stream;
            stream = response.GetResponseStream();
            int count = 0;
            do
            {
                count = stream.Read(buffer, 0, buffer.Length);
                if (count != 0)
                    respBody.Append(Encoding.UTF8.GetString(buffer, 0, count));
            }
            while (count > 0);
            string responseText = respBody.ToString();
            return responseText;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“HttpHelper.CNNWebClient”的 XML 注释
        public class CNNWebClient : WebClient
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“HttpHelper.CNNWebClient”的 XML 注释
        {

            private int _timeOut = 200;

            /// <summary>
            /// 过期时间
            /// </summary>
            public int Timeout
            {
                get
                {
                    return _timeOut;
                }
                set
                {
                    if (value <= 0)
                        _timeOut = 200;
                    _timeOut = value;
                }
            }

            /// <summary>
            /// 重写GetWebRequest,添加WebRequest对象超时时间
            /// </summary>
            /// <param name="address"></param>
            /// <returns></returns>
            protected override WebRequest GetWebRequest(Uri address)
            {
                HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
                request.Timeout = 1000 * Timeout;
                request.ReadWriteTimeout = 1000 * Timeout;
                return request;
            }
        }
    }
}
