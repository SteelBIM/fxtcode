using System;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Script.Serialization;

namespace Kingsun.SpokenBroadcas.Common
{
    public class JsonHelper
    {

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

        /// <summary>
        /// 把DataTable转化为json格式
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static object Dtb2Object(DataTable dtb)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            System.Collections.ArrayList dic = new System.Collections.ArrayList();
            if (dtb != null)
            {
                foreach (DataRow dr in dtb.Rows)
                {
                    System.Collections.Generic.Dictionary<string, object> drow = new System.Collections.Generic.Dictionary<string, object>();
                    foreach (DataColumn dc in dtb.Columns)
                    {
                        drow.Add(dc.ColumnName, dr[dc.ColumnName]);
                    }
                    dic.Add(drow);
                }
            }
            //序列化       
            return dic;
        }
    }
}
