using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FxtDataAcquisition.Common
{
    public static class StringHelp
    {

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
        public static bool CheckDecimal(this string dateStr)
        {
            decimal dt;
            if (string.IsNullOrEmpty(dateStr) || !decimal.TryParse(dateStr, out dt))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 判断一个字符串是否为合法整数(不限制长度)
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns></returns>
        public static bool CheckInteger(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }
            string pattern = @"^\d+$";
            return Regex.IsMatch(s, pattern);
        }
        /// <summary>
        /// 验证字符串是否为日期字符串
        /// </summary>
        /// <param name="dateStr"></param>
        /// <returns></returns>
        public static bool CheckIsDate(this string dateStr)
        {
            DateTime dt;
            if (string.IsNullOrEmpty(dateStr) || !DateTime.TryParse(dateStr, out dt))
            {
                return false;
            }
            return true;
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
        public static string ConvertToString(this long[] ints)
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
            foreach (long str in ints)
            {
                sb.Append(str).Append(",");
            }
            return sb.ToString().TrimEnd(',');
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
                if (StringHelp.CheckInteger(id))
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
        /// 字符串数组转换成int集合
        /// </summary>
        /// <param name="strings"></param>
        /// <returns></returns>
        public static List<int> ConvertToIntList(this string[] strings)
        {
            List<int> intList = new List<int>();
            if (strings == null)
            {
                return intList;
            }
            foreach (string id in strings)
            {
                if (StringHelp.CheckInteger(id))
                {
                    intList.Add(Convert.ToInt32(id));
                }
            }
            return intList;
        }
        /// <summary>
        /// 字符串数组转换成long数组
        /// </summary>
        /// <param name="strings"></param>
        /// <returns></returns>
        public static long[] ConvertToLongs(this string[] strings)
        {
            long[] longs = null;
            if (strings == null)
            {
                return null;
            }
            List<long> list = new List<long>();
            foreach (string id in strings)
            {
                if (StringHelp.CheckInteger(id))
                {
                    list.Add(Convert.ToInt64(id));
                }
            }
            if (list != null && list.Count > 0)
            {
                longs = list.ToArray();
            }
            return longs;
        }
        /// <summary>
        /// 字符串转换成int数组
        /// </summary>
        /// <param name="strings"></param>
        /// <param name="_char">数组分隔符</param>
        /// <returns></returns>
        public static int[] ConvertToInts(this string strings, char _char)
        {
            if (string.IsNullOrEmpty(strings))
            {
                return null;
            }
            return strings.Split(_char).ConvertToInts();
        }
        /// <summary>
        /// 字符串转换成int集合
        /// </summary>
        /// <param name="strings"></param>
        /// <param name="_char">数组分隔符</param>
        /// <returns></returns>
        public static List<int> ConvertToIntList(this string strings, char _char)
        {
            if (string.IsNullOrEmpty(strings))
            {
                return null;
            }
            return strings.Split(_char).ConvertToIntList();
        }
        /// <summary>
        /// 指示指定的字符串是 null 还是 System.String.Empty 字符串
        /// </summary>
        /// <param name="strings"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string strings)
        {
            return string.IsNullOrEmpty(strings);
        }
    }
}
