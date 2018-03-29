using System;
using Newtonsoft.Json;

namespace CAS.Common
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
        /// 将对象转化成key小写json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToLowerJson(this Object obj)
        {
            return LowercaseJsonSerializer.SerializeObject(obj);
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

        #region 类型转换

        #region 数值 decimal、double、int、long
        /// <summary>
        /// 转换成decimal类型，可返回需要的小数位数
        /// </summary>
        /// <param name="def">转换失败后返回的默认值</param>
        /// <param name="num">需要保留的小数位数</param>
        /// <returns></returns>
        public static decimal VDecimal(this object obj, decimal def = 0, int num = -1)
        {
            if (obj == null)
                return def;
            decimal temp = def;
            if (decimal.TryParse(obj.ToString(), out temp))
            {
                if (num >= 0)
                    return Math.Round(temp, num);
                return temp;
            }
            return def;
        }
        /// <summary>
        /// 转换成double类型，可返回需要的小数位数
        /// </summary>
        /// <param name="def">转换失败后返回的默认值</param>
        /// <param name="num">需要保留的小数位数</param>
        /// <returns></returns>
        public static double VDouble(this object obj, double def = 0, int num = -1)
        {
            if (obj == null)
                return def;
            double temp = def;
            if (double.TryParse(obj.ToString(), out temp))
            {
                if (num >= 0)
                    return Math.Round(temp, num);
                return temp;
            }
            return def;
        }
        /// <summary>
        /// 转换成int类型
        /// </summary>
        /// <param name="def">转换失败后返回的默认值</param>
        /// <returns></returns>
        public static int VInt(this object obj, int def = 0)
        {
            if (obj == null)
                return def;
            int temp = def;
            if (int.TryParse(obj.ToString(), out temp))
                return temp;
            return def;
        }
        /// <summary>
        /// 转换成long类型
        /// </summary>
        /// <param name="def">转换失败后返回的默认值</param>
        /// <returns></returns>
        public static long VLong(this object obj, long def = 0)
        {
            if (obj == null)
                return def;
            long temp = def;
            if (long.TryParse(obj.ToString(), out temp))
                return temp;
            return def;
        }
        #endregion

        #region 转换成bool类型
        /// <summary>
        /// 转换成bool类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool VBool(this object obj)
        {
            if (obj == null)
                return false;
            bool temp = false;
            if (bool.TryParse(obj.ToString(), out temp))
                return temp;
            return false;
        } 
        #endregion

        #region 转换成string类型
        /// <summary>
        /// 转换成string类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string VString(this object obj)
        {
            if (obj == null)
                return "";
            return obj.ToString();
        } 
        #endregion

        #region 转换成DateTime类型
        /// <summary>
        /// 转换成DateTime类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime VDateTime(this object obj)
        {
            DateTime dt = new DateTime();
            if (obj == null)
                return dt;
            if (DateTime.TryParse(obj.ToString(), out dt))
                return dt;
            return dt;
        } 
        #endregion
        #endregion
    }
}
