using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Kingsun.SynchronousStudy.Common.Model;

namespace Kingsun.SynchronousStudy.Common
{
    public class JsonHelper
    {



        public static HttpResponseMessage GetErrorResult(string message)
        {
            object obj = new { Success = false, data = "", Message = message, SystemTime = DateTime.Now };
            return KingsunResponse.toJson(obj);
        }
        public static HttpResponseMessage GetErrorResult(int errorCode, string message)
        {
            object obj = new { Success = false, ErrorCode = errorCode, data = "", Message = message, SystemTime = DateTime.Now };
            return KingsunResponse.toJson(obj);
        }

        public static HttpResponseMessage GetResult(object Data, string message = "")
        {
            object obj = new { Success = true, data = Data, Message = message, SystemTime = DateTime.Now };
            return KingsunResponse.toJson(obj);
        }

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
            foreach (DataRow dr in dtb.Rows)
            {
                System.Collections.Generic.Dictionary<string, object> drow = new System.Collections.Generic.Dictionary<string, object>();
                foreach (DataColumn dc in dtb.Columns)
                {
                    drow.Add(dc.ColumnName, dr[dc.ColumnName]);
                }
                dic.Add(drow);
            }
            //序列化       
            return dic;
        }


        /// <summary>  
        /// 将Datatable转换为List集合  
        /// </summary>  
        /// <typeparam name="T">类型参数</typeparam>  
        /// <param name="dt">datatable表</param>  
        /// <returns></returns>  
        public static List<T> DataTableToList<T>(DataTable dt)
        {
            var list = new List<T>();
            Type t = typeof(T);
            var plist = new List<PropertyInfo>(typeof(T).GetProperties());

            foreach (DataRow item in dt.Rows)
            {
                T s = System.Activator.CreateInstance<T>();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    PropertyInfo info = plist.Find(p => p.Name == dt.Columns[i].ColumnName);
                    if (info != null)
                    {
                        if (!Convert.IsDBNull(item[i]))
                        {
                            info.SetValue(s, item[i], null);
                        }
                    }
                }
                list.Add(s);
            }
            return list;
        }
        /// <summary> 
        /// DataSet装换为泛型集合 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="ds">DataSet</param> 
        /// <param name="tableIndex">待转换数据表索引</param> 
        /// <returns></returns> 
        public static List<T> DataSetToIList<T>(DataSet ds, int tableIndex)
        {
            if (ds == null || ds.Tables.Count < 0)
                return null;
            if (tableIndex > ds.Tables.Count - 1)
                return null;
            if (tableIndex < 0)
                tableIndex = 0;

            DataTable p_Data = ds.Tables[tableIndex];
            // 返回值初始化 
            List<T> result = new List<T>();
            for (int j = 0; j < p_Data.Rows.Count; j++)
            {
                T _t = (T)Activator.CreateInstance(typeof(T));
                PropertyInfo[] propertys = _t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    for (int i = 0; i < p_Data.Columns.Count; i++)
                    {
                        // 属性与字段名称一致的进行赋值 
                        if (pi.Name.Equals(p_Data.Columns[i].ColumnName))
                        {
                            // 数据库NULL值单独处理 
                            if (p_Data.Rows[j][i] != DBNull.Value)
                                pi.SetValue(_t, p_Data.Rows[j][i], null);
                            else
                                pi.SetValue(_t, null, null);
                            break;
                        }
                    }
                }
                result.Add(_t);
            }
            return result;
        }

        //判断字符串是否为空
        public static string StringTOJson(string str)
        {
            string result = "";
            if (string.IsNullOrEmpty(str))
            {
                return result;
            }
            return str;
        }

        /// <summary>
        /// 过滤特殊字符
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>json字符串</returns>
        public static string String2Json(string s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\""); break;
                    case '\\':
                        sb.Append("\\\\"); break;
                    //case '/':
                    //    sb.Append("\\/"); break;
                    case '\b':
                        sb.Append("\\b"); break;
                    case '\f':
                        sb.Append("\\f"); break;
                    case '\n':
                        sb.Append("\\n"); break;
                    case '\r':
                        sb.Append("\\r"); break;
                    case '\t':
                        sb.Append("\\t"); break;
                    case '+':
                        sb.Append("\\n"); break;
                    case '\'':
                        sb.Append("\'\'"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 把字符串转化为秒
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string GetDate(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return "0";
            }
            else
            {
                DateTime date = DateTime.Now;
                string dateStr = date.ToString("yyyy-MM-dd");
                DateTime nowdatetime = DateTime.Parse(dateStr);
                DateTime enddatetime = Convert.ToDateTime(s);
                TimeSpan nowtimespan = new TimeSpan(nowdatetime.Ticks);
                TimeSpan endtimespan = new TimeSpan(enddatetime.Ticks);
                TimeSpan timespan = nowtimespan.Subtract(endtimespan).Duration();
                // DateTime dt = Convert.ToDateTime(dataRow[column.ColumnName].ToString());
                //  int time = (Convert.ToInt32(dt.Hour) * 3600) + (Convert.ToInt32(dt.Minute) * 60) + Convert.ToInt32(dt.Second);
                //  TimeSpan endtimespan = new TimeSpan(dt.Ticks);
                return timespan.TotalSeconds.ToString();
            }
        }

        public static DataSet SelectOrderSql(string AppID, string sql)
        {
            string _bj = ConfigurationManager.ConnectionStrings["BJDBConnectionStr"].ConnectionString;
            string _sz = ConfigurationManager.ConnectionStrings["SZDBConnectionStr"].ConnectionString;
            string _ot = ConfigurationManager.ConnectionStrings["OTDBConnectionStr"].ConnectionString;
            string _rjpep = ConfigurationManager.ConnectionStrings["RJPEPDBConnectionStr"].ConnectionString;
            string _rjyx = ConfigurationManager.ConnectionStrings["RJYXDBConnectionStr"].ConnectionString;

            DataSet ds = new DataSet();
            switch (AppID)
            {
                case "0a94ceaf-8747-4266-bc05-ed8ae2e7e410": //北京版
                    ds = SqlHelper.ExecuteDataset(_bj, CommandType.Text, sql);
                    break;
                case "1548d0a3-ca8e-4702-9c2c-f0ba0cacd385": //广州版
                    ds = SqlHelper.ExecuteDataset(_ot, CommandType.Text, sql);
                    break;
                case "241ea176-fce7-4bd7-a65f-a7978aac1cd2": //牛津深圳版
                    ds = SqlHelper.ExecuteDataset(_sz, CommandType.Text, sql);
                    break;
                case "37ca795d-42a6-4117-84f3-f4f856e03c62": //广东版
                    ds = SqlHelper.ExecuteDataset(_ot, CommandType.Text, sql);
                    break;
                case "41efcd18-ad8c-4585-8b6c-e7b61f49914c": //新课标标准实验版
                    ds = SqlHelper.ExecuteDataset(_ot, CommandType.Text, sql);
                    break;
                case "43716a9b-7ade-4137-bdc4-6362c9e1c999": //牛津上海本地版
                    ds = SqlHelper.ExecuteDataset(_ot, CommandType.Text, sql);
                    break;
                case "5373bbc9-49d4-47df-b5b5-ae196dc23d6d": //人教PEP版(同步学)
                    ds = SqlHelper.ExecuteDataset(_rjpep, CommandType.Text, sql);
                    break;
                case "64a8de22-cea0-4026-ab36-5a70f94dd6e4": //人教版新起点
                    ds = SqlHelper.ExecuteDataset(_rjpep, CommandType.Text, sql);
                    break;
                case "333d7cfc-cb4f-49d2-8ded-025e7d0fe766": //江苏译林
                    ds = SqlHelper.ExecuteDataset(_ot, CommandType.Text, sql);
                    break;
                case "8170b2bf-82a8-4c2d-9458-ae9d43cac5e3": //人教版
                    ds = SqlHelper.ExecuteDataset(_rjpep, CommandType.Text, sql);
                    break;
                case "9426808e-da8e-488c-9827-b082c19b62a7": //牛津上海全国版
                    ds = SqlHelper.ExecuteDataset(_ot, CommandType.Text, sql);
                    break;
                case "f0a9e1a7-b4cf-4a37-8fd1-932a66070afa": //山东版
                    ds = SqlHelper.ExecuteDataset(_ot, CommandType.Text, sql);
                    break;
                default:
                    break;
            }
            return ds;
        }
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
