using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace OpenPlatform.Framework.Utils
{
  public class JsonHelper
    {
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
            var data = new ReturnData();
            data.returntype = status;
            if (obj != null && obj.GetType().Name.IndexOf("DataTable", StringComparison.Ordinal) >= 0)
                data.data = DataTableToList((DataTable)obj);
            else
                data.data = obj;
#if DEBUG
            if (debug != null && debug.GetType().Name.IndexOf("Exception", StringComparison.Ordinal) >= 0)
            {
                var ex = (Exception)debug;
                var de = new ExceptionData();
                de.source = ex.Source;
                de.message = ex.Message;
                de.trace = ex.StackTrace;
                debug = de;
                //LogHelper.Error(ex);
            }
            data.debug = debug;
#endif
            data.returntext = message;
            return ObjectToJSON(data);
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
            var list
            = new List<Dictionary<string, object>>();
            foreach (DataRow dr in dt.Rows)
            {
                var dic = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    dic.Add(dc.ColumnName, dr[dc.ColumnName]);
                }
                list.Add(dic);
            }
            return list;
        }

        /// <summary> 
        /// 对象转JSON 
        /// </summary> 
        /// <param name="obj">对象</param> 
        /// <returns>JSON格式的字符串</returns> 
        public static string ObjectToJSON(object obj)
        {
            var jss = new JavaScriptSerializer {MaxJsonLength = 10240000};
            //评估发展中心使用时房号数太多，先把最大值设大点，待评估发展中心取数据修改后去掉设置值

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
        /// JSON文本转对象,泛型方法 
        /// </summary> 
        /// <typeparam name="T">类型</typeparam> 
        /// <param name="jsonText">JSON文本</param> 
        /// <returns>指定类型的对象</returns> 
        public static T JSONToObject<T>(string jsonText)
        {
            var jss = new JavaScriptSerializer();
            try
            {
                return jss.Deserialize<T>(jsonText);
            }
            catch (Exception ex)
            {
                throw new Exception("JSONHelper.JSONToObject(): " + ex.Message);
            }
        }
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

    }
}
