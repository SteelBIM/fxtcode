using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace FxtCenterServiceOpen.Actualize.Common
{
    public class JsonDataHelper
    {
        /// <summary>
        /// Joson字符转为ReturnData对象
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        protected static JsonReturnData JsonToReturnData(string json)
        {
            var returnData = JSONToObject<JsonReturnData>(json);
            return returnData;
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

        /// <summary>
        /// JSON文本转对象,泛型方法 
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="json">JSON文本</param>
        /// <param name="returnData">转出参数：ReturnData</param>
        /// <returns>指定类型的对象</returns>
        public static T JSONToObject<T>(string json, out JsonReturnData returnData)
        {
            returnData = JsonToReturnData(json);
            string result = (null == returnData.data || returnData.returntype != 1) ? "" : returnData.data.ToString();
            return JSONToObject<T>(result);
        }

        /// <summary>
        /// json转List<T>对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static List<T> JsonToList<T>(string json)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                return jss.Deserialize<List<T>>(json);
            }
            catch (Exception ex)
            {
                throw new Exception("JSONHelper.JSONToObject(): " + ex.Message);
            }
        }

        /// <summary>
        /// 将json转换为List,用Out转出returnData对象
        /// </summary>
        /// <typeparam name="T">泛型对象</typeparam>
        /// <param name="json">Json结果字符串</param>
        /// <param name="returnData">转出参数：ReturnData</param>
        /// <returns>List<T></returns>
        public static List<T> JsonToList<T>(string json, out JsonReturnData returnData)
        {
            returnData = JsonToReturnData(json);
            string result = (null == returnData.data || returnData.returntype != 1) ? "" : returnData.data.ToString();
            return JsonToList<T>(result);
        }
    }
}
