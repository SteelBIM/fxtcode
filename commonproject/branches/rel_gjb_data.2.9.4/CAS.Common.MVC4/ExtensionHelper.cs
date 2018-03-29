using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace CAS.Common.MVC4
{
    /// <summary>
    /// 扩展类
    /// </summary>
    public static class ExtensionHelper
    {
        /// <summary>
        /// 将对象转化成json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this Object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 将json字符串转化成对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object StringToJson(this string obj)
        {
            return JsonConvert.DeserializeObject(obj);
        }

        /// <summary>
        /// 将json字符串转化成对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object StringToJson<T>(this string obj) where T : CAS.Entity.BaseDAModels.BaseTO, new()
        {
            return JsonConvert.DeserializeObject<T>(obj);
        }
    }
}
