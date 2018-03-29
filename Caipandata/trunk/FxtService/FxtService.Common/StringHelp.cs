using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.Reflection;

namespace FxtService.Common
{
    /**
     * EncodeField(this string originalText),DecodeField(this string originalText)方法移至Utility.cs
     **/
    public static class StringHelp
    {
        ///// <summary>
        ///// 编码
        ///// </summary>
        ///// <param name="originalText"></param>
        ///// <returns></returns>
        //public static string EncodeField(this string originalText)
        //{
        //    if (originalText == null)
        //    {
        //        return null;
        //    }
            
        //    return HttpUtility.UrlEncode(originalText).Replace("+", "%20");
        //}
        ///// <summary>
        ///// 解码
        ///// </summary>
        ///// <param name="originalText"></param>
        ///// <returns></returns>
        //public static string DecodeField(this string originalText)
        //{
        //    if (originalText == null)
        //    {
        //        return null;
        //    }

        //    return HttpUtility.UrlDecode(originalText).Replace("%20", "+");
        //}
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
        public static IList<T> EncodeField<T>(this IList<T> t)
        {
            if (t == null)
            {
                return null;
            }
            IList<T> _t =new List<T>();
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
        public static IList<T> DecodeField<T>(this IList<T> t)
        {
            if (t == null)
            {
                return null;
            }
            IList<T> _t = new List<T>();
            for (int i = 0; i < t.Count; i++)
            {
                _t.Add(DecodeField<T>(t[i]));
            }
            return _t;
        }

        /// <summary>
        /// 去除字符串所有空格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string TrimBlank(this string str)
        {
            if (str == null)
            {
                return null;
            }
            return Regex.Replace(str, @"\s", "");
        }
        /// <summary>
        /// 字符串数组转换成int数组
        /// </summary>
        /// <param name="strings"></param>
        /// <returns></returns>
        public static int[] ConvertToInts(this string[] strings)
        {
            int[] ints = null;
            if (strings == null)
            {
                return null;
            }
            List<int> list = new List<int>();
            foreach (string id in strings)
            {
                if (IsInteger(id))
                {
                    list.Add(Convert.ToInt32(id));
                }
            }
            if (list != null && list.Count > 0)
            {
                ints = list.ToArray();
            }
            return ints;
        }
        /// <summary>
        /// 将int数组转换成","分隔的字符串
        /// </summary>
        /// <param name="ints"></param>
        /// <returns></returns>
        public static string ConvertToString(this int[] ints)
        {
            if (ints == null)
            {
                return null;
            }
            if (ints.Length < 1)
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            foreach (int str in ints)
            {
                sb.Append(str).Append(",");
            }
            return sb.ToString().TrimEnd(',');
        }
        /// <summary>
        /// 将long数组转换成","分隔的字符串
        /// </summary>
        /// <param name="ints"></param>
        /// <returns></returns>
        public static string ConvertToString(this long[] longs)
        {
            if (longs == null)
            {
                return null;
            }
            if (longs.Length < 1)
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            foreach (int str in longs)
            {
                sb.Append(str).Append(",");
            }
            return sb.ToString().TrimEnd(',');
        } /// <summary>
        /// 将string数组转换成","分隔的字符串
        /// </summary>
        /// <param name="ints"></param>
        /// <returns></returns>
        public static string ConvertToString(this string[] strings)
        {
            if (strings == null)
            {
                return null;
            }
            if (strings.Length < 1)
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            foreach (string str in strings)
            {
                sb.Append(str).Append(",");
            }
            return sb.ToString().TrimEnd(',');
        }
        /// <summary>
        /// 判断一个字符串是否为合法整数(不限制长度)
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns></returns>
        public static bool IsInteger(object s)
        {
            try
            {
                int num2 = 0;
                if (Int32.TryParse(Convert.ToString(s), out num2) == true)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }
        public static bool IsFloat(object s)
        {
            try
            {
                float num2 = 0;
                if (float.TryParse(Convert.ToString(s), out num2) == true)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }
        public static bool IsDateTime(object s)
        {
            try
            {
                DateTime num2 = new DateTime();
                if (DateTime.TryParse(Convert.ToString(s), out num2) == true)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }
        public static bool IsDecimal(object s)
        {
            try
            {
                Decimal num2 = 0;
                if (Decimal.TryParse(Convert.ToString(s), out num2) == true)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }
    }
}
