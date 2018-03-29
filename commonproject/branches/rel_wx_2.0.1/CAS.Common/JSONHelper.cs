using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Script.Serialization;
using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;
namespace CAS.Common
{
    /// <summary> 
    /// JSON帮助类 
    /// </summary> 
    public class JSONHelper
    {
        /// <summary>
        /// 统一返回的json格式，全部属性小写
        /// </summary>
        [Serializable]
        public class ReturnData
        {
            public int returntype { get; set; } //1为正确
            public object returntext { get; set; }
            public object data { get; set; }
            public object debug { get; set; }
        }

        [Serializable]
        public class ExceptionData
        {
            public string source { get; set; }
            public string message { get; set; }
            public string trace { get; set; }          
        }

        /// <summary>
        /// 统一返回json列表数据的格式
        /// </summary>
        public class ListData
        {
            private int _page = 1;
            public int page { get { return _page; } set { _page = value; } }
            private int _total = 0;
            public int total { get { return _total; } set { _total = value; } }
            public object rows { get; set; }
        }

        /// <summary>
        /// 带debug的json返回
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="status"></param>
        /// <param name="message"></param>
        /// <param name="debug"></param>
        /// <returns></returns>
        public static string GetJson(object obj, int status, string message, object debug)
        {
            ReturnData data = new JSONHelper.ReturnData();
            data.returntype = status;
            if (obj != null && obj.GetType().Name.IndexOf("DataTable") >= 0)
                data.data = JSONHelper.DataTableToList((DataTable)obj);
            else
                data.data = obj;
#if DEBUG
            if (debug != null && debug.GetType().Name.IndexOf("Exception") >= 0)
            {
                Exception ex = (Exception)debug;
                ExceptionData de = new ExceptionData();
                de.source = ex.Source;
                de.message = ex.Message;
                de.trace = ex.StackTrace;
                debug = de;
                LogHelper.Error(ex);
            }
            data.debug = debug;
#endif
            data.returntext = message;
            return ObjectToJSON(data);
        }
        
        /// <summary> 
        /// 对象转JSON 
        /// </summary> 
        /// <param name="obj">对象</param> 
        /// <returns>JSON格式的字符串</returns> 
        public static string ObjectToJSON(object obj)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            //评估发展中心使用时房号数太多，先把最大值设大点，待评估发展中心取数据修改后去掉设置值
            jss.MaxJsonLength=10240000;

            try
            {
                //string json = jss.Serialize(obj);
                //Regex reg = new Regex("[\"][a-zA-Z0-9_]*[\"]:null[ ]*[,]?");
                //json = reg.Replace(json, string.Empty);
                //return json;
                return jss.Serialize(obj);//.Replace(":null",":\"\"");
            }
            catch (Exception ex)
            {
                throw new Exception("JSONHelper.ObjectToJSON(): " + ex.Message);
            }
        }
        /// <summary> 
        /// 数据表转键值对集合 
        /// 把DataTable转成 List集合, 存每一行  
        /// 集合中放的是键值对字典,存每一列 
        /// </summary> 
        /// <param name="dt">数据表</param> 
        /// <returns>哈希表数组</returns> 
        public static List<Dictionary<string, object>> DataTableToList(DataTable dt)
        {
            List<Dictionary<string, object>> list
            = new List<Dictionary<string, object>>();
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    dic.Add(dc.ColumnName, dr[dc.ColumnName]);
                }
                list.Add(dic);
            }
            return list;
        }


        public static List<Dictionary<string, object>> DataTableToOrderList(DataTable dt) 
        {
            List<Dictionary<string, object>> list= new List<Dictionary<string, object>>();
            if (dt!=null && dt.Rows.Count>0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        dic.Add(dt.Columns[j].ColumnName,dt.Rows[i][j]);   
                    }
                    list.Add(dic);
                }
            }
            return list;
        }

        /// <summary> 
        /// 数据集转键值对数组字典 
        /// </summary> 
        /// <param name="dataSet">数据集</param> 
        /// <returns>键值对数组字典</returns> 
        public static Dictionary<string, List<Dictionary<string, object>>> DataSetToDic(DataSet ds)
        {
            Dictionary<string, List<Dictionary<string, object>>> result = new Dictionary<string, List<Dictionary<string, object>>>();
            foreach (DataTable dt in ds.Tables)
                result.Add(dt.TableName, DataTableToList(dt));
            return result;
        }
        /// <summary> 
        /// 数据表转JSON 
        /// </summary> 
        /// <param name="dataTable">数据表</param> 
        /// <returns>JSON字符串</returns> 
        public static string DataTableToJSON(DataTable dt)
        {
            return ObjectToJSON(DataTableToList(dt));
        }
        /// <summary> 
        /// JSON文本转对象,泛型方法 
        /// </summary> 
        /// <typeparam name="T">类型</typeparam> 
        /// <param name="jsonText">JSON文本</param> 
        /// <returns>指定类型的对象</returns> 
        public static T JSONToObject<T>(string jsonText)
        {           
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                return jss.Deserialize<T>(jsonText);
            }
            catch (Exception ex)
            {
                throw new Exception("JSONHelper.JSONToObject(): " + ex.Message);
            }
        }


        public static List<T> JSONStringToList<T>(string JsonStr)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                return  jss.Deserialize<List<T>>(JsonStr);
            }
            catch (Exception ex)
            {
                throw new Exception("JSONHelper.JSONToObject(): " + ex.Message);
            }           
        }


        /// <summary> 
        /// 将JSON文本转换为数据表数据 
        /// </summary> 
        /// <param name="jsonText">JSON文本</param> 
        /// <returns>数据表字典</returns> 
        public static Dictionary<string, List<Dictionary<string, object>>> TablesDataFromJSON(string jsonText)
        {
            return JSONToObject<Dictionary<string, List<Dictionary<string, object>>>>(jsonText);
        }
        /// <summary> 
        /// 将JSON文本转换成数据行 
        /// </summary> 
        /// <param name="jsonText">JSON文本</param> 
        /// <returns>数据行的字典</returns> 
        public static Dictionary<string, object> DataRowFromJSON(string jsonText)
        {
            return JSONToObject<Dictionary<string, object>>(jsonText);
        }
    }

}