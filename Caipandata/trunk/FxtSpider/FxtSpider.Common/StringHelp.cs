using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FxtSpider.Common
{
    public static class StringHelp
    {
        /// <summary>
        /// 取字符串指定长度字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string GetSubstring(this string str, int maxlen)
        {
            if (str == null || str.Length <= maxlen)
            {
                return str;
            }
            str = str.Substring(0, maxlen);
            return str;
        }
        public static int 获取相差天数(DateTime date)
        {
            DateTime time_Now = DateTime.Now;    //获取当前时间；
            TimeSpan ts_now = new TimeSpan(date.Ticks);     //当前时间的 TimeSpan 结构对象；
            TimeSpan ts_end = new TimeSpan(time_Now.Ticks);    //团购结束时间的 TimeSpan 结构对象；

            TimeSpan ts_diff = ts_now.Subtract(ts_end).Duration();   // 两时间相差的TimeSpan
            int dateDiff = ts_diff.Days;
            return dateDiff;
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
        /// 判断一个字符串是否为合法整数(不限制长度)
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns></returns>
        public static bool IsInteger(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }
            int i;
            if (string.IsNullOrEmpty(s) || !int.TryParse(s, out i))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 检查是否为Decimal类型数据
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool CheckDecimal(this string str)
        {
            decimal dt;
            if (string.IsNullOrEmpty(str) || !decimal.TryParse(str, out dt))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 将字符串中的数学数字转换成中文数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string NumberConvertToChinese(string str)
        {
            str = str.Replace("1", "一");
            str = str.Replace("2", "两");
            str = str.Replace("3", "三");
            str = str.Replace("4", "四");
            str = str.Replace("5", "五");
            str = str.Replace("6", "六");
            str = str.Replace("7", "七");
            str = str.Replace("8", "八");
            str = str.Replace("9", "九");
            str = str.Replace("0", "零");
            return str;
        }
        /// <summary>
        /// 将字符串中的中文数字转换成数学数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ChineseConvertToNumber(string str)
        {
            str = str.Replace("一", "1");
            str = str.Replace("二", "2");
            str = str.Replace("两", "2");
            str = str.Replace("三", "3");
            str = str.Replace("四", "4");
            str = str.Replace("五", "5");
            str = str.Replace("六", "6");
            str = str.Replace("七", "7");
            str = str.Replace("八", "8");
            str = str.Replace("九", "9");
            str = str.Replace("零", "0");
            return str;
        }

        /// <summary>
        /// 特殊字符串替换
        /// </summary> 
        public static string ToRemoveSpe(this string strTemp)
        {
            if (strTemp == null)
                strTemp = "";
            strTemp = strTemp.Replace("*", "");
            strTemp = strTemp.Replace("?", "");
            strTemp = strTemp.Replace("#", "");
            strTemp = strTemp.Replace("@", "");
            strTemp = strTemp.Replace("^", "");
            strTemp = strTemp.Replace("&", "");
            strTemp = strTemp.Replace("+", "");
            strTemp = strTemp.Replace("-", "");
            strTemp = strTemp.Replace("(", "");
            strTemp = strTemp.Replace(")", "");
            strTemp = strTemp.Replace("!", "");
            strTemp = strTemp.Replace("`", "");
            strTemp = strTemp.Replace("~", "");
            strTemp = strTemp.Replace("<", "");
            strTemp = strTemp.Replace(">", "");
            strTemp = strTemp.Replace("'", "");
            strTemp = strTemp.Replace("\"", "");
            strTemp = strTemp.Replace("\\", "");
            strTemp = strTemp.Replace("|", "");
            strTemp = strTemp.Replace("=", "");
            strTemp = strTemp.Replace(",", "");
            strTemp = Regex.Replace(strTemp, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            return strTemp;
        }

        public static string RemoveHeml(this string str)
        {
            if (str == null)
            {
                return null;
            }
            str = str.Replace("&nbsp;", "");
            str = str.Replace("&nbsp", "");
            return str;
        }

        /// <summary>
        /// 删除html标签。
        /// </summary>
        /// <param name="html">输入的字符串。</param>
        /// <returns>没有html标签的字符串。</returns>
        public static string RemoveHTMLTags(this string html)
        {
            return Regex.Replace(Regex.Replace(Regex.Replace((html ?? string.Empty).Replace("&nbsp;", " ").Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ").Replace("\t", " "), "<\\/?[^>]+>", "\r\n"), "(\r\n)+", "\r\n"), "(\\s)+", " ").Trim();
        }

        /// <summary>
        /// 验证字符串是否为日期字符串
        /// </summary>
        /// <param name="dateStr"></param>
        /// <returns></returns>
        public static bool CheckStrIsDate(this string dateStr)
        {
            DateTime dt;
            if (string.IsNullOrEmpty(dateStr) || !DateTime.TryParse(dateStr, out dt))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// js中的escape编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string JsEscape(this string str)
        {
            if (str == null)
            {
                return null;
            }
            return Microsoft.JScript.GlobalObject.escape(str);
        }
        /// <summary>
        /// js中的encodeURI编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string JsEncodeURI(this string str)
        {
            if (str == null)
            {
                return null;
            }
            return Microsoft.JScript.GlobalObject.encodeURI(str);
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
                if (StringHelp.IsInteger(id)) 
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
        /// 字符串转换成int数组
        /// </summary>
        /// <param name="strings">数组分隔符</param>
        /// <returns></returns>
        public static int[] ConvertToInts(this string strings,char _char)
        {
            if (strings == null)
            {
                return null;
            }
            return strings.Split(_char).ConvertToInts();
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
                if (StringHelp.IsInteger(id))
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

        public static string ToString2(this int? intValue)
        {
            if (intValue == null)
            {
                return "";
            }
            return intValue.ToString();
        }
    }
}
