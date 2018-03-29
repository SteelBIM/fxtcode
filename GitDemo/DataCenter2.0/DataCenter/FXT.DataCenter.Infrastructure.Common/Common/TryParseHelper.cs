using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Infrastructure.Common.Common
{
    /// <summary>
    /// 整形相关操作类
    /// </summary>
    public class TryParseHelper
    {
        /// <summary>
        /// 把字符串转换为Int
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defaultValue">转换失败时的默认值</param>
        /// <returns>转换后的结果</returns>
        public static int StrToInt32(string str, int defaultValue)
        {
            int i = 0;
            return Int32.TryParse(str, out i) ? i : defaultValue;
        }

        public static object StrToInt32(string str)
        {
            int i = 0;
            if(Int32.TryParse(str, out i)) return i;
            return null;
        }

        public static decimal StrToDecimal(string str, int defaultValue)
        {
            decimal i = 0;
            return decimal.TryParse(str, out i) ? i : defaultValue;
        }

        public static object StrToDecimal(string str)
        {
            decimal i = 0;
            if (decimal.TryParse(str, out i)) return i;
            return null;
        }


        public static string StrToDateTime(string str, string defaultValue)
        {
            DateTime i = DateTime.Now;
            return DateTime.TryParse(str, out i) ? i.ToString(): defaultValue;
        }

        public static object StrToDateTime(string str)
        {
            DateTime i = DateTime.Now;
            if(DateTime.TryParse(str, out i)) return i;
            return null;
        }

    }
}
