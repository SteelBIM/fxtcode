using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aspose.Words;
using Aspose.Words.Tables;
using System.IO;
using Aspose.Words.Drawing;
using System.Diagnostics;
using Microsoft.International.Formatters;
using System.Globalization;

namespace CAS.Common.Aspose
{
    public class AsposeHelper
    {
        public static string ConvertDate(string date)
        {
            if (date.IndexOf("年") < 0)
            {
                date = Convert.ToDateTime(date).ToString("yyyy年MM月d日");
            }
            //2015年1月2日
            string result = string.Empty;
            //年份
            string year =date.Substring(0,4);
            
            int yearIndex = date.IndexOf("年");
            int monthIndex = date.IndexOf("月");
            int dayIndex = date.IndexOf("日");
            //月
            string month = date.Substring(yearIndex+1,monthIndex-yearIndex-1);
            //日
            string day = date.Substring(monthIndex + 1, dayIndex - monthIndex - 1);

            result = string.Format("{0}年{1}月{2}日",GetCnDate(year,true),GetCnDate(month),GetCnDate(day));
            return result;
        }

        /// <summary>
        /// 获取中文年份
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        private static string GetCnDate(string num,bool isSpecial)
        {
            string result = string.Empty;
            if (isSpecial)
            {
                char[] yearChar = num.ToCharArray();
                foreach (char item in yearChar)
                {
                    result += InternationalNumericFormatter.FormatWithCulture("Ln", Convert.ToInt32(item.ToString()), null, new CultureInfo("zh-CHS"));
                }
            }
            else
            {
                result=InternationalNumericFormatter.FormatWithCulture("Ln", Convert.ToInt32(num), null, new CultureInfo("zh-CHS"));
            }
            return result;
        }

        /// <summary>
        /// 获取中文年份
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        private static string GetCnDate(string num)
        {
            return GetCnDate(num,false);
        }

        public static bool TryGetDecimal(string value)
        {
            decimal i=0;
            return decimal.TryParse(value,out i);
        }
        public static bool TryGetDateTime(string value)
        {
            DateTime i = DateTime.Today;
            return DateTime.TryParse(value, out i);
        }
        public static bool TryGetbool(string value)
        {
            bool i = false;
            return bool.TryParse(value, out i);
        }

        /// <summary>
        /// 判断当前需要添加进dicExcel对象的名称是否存在，如果不存在则添加，如果存在原始值为空则替换
        /// </summary>
        /// <param name="dicExcel">引用传递</param>
        /// <param name="showName"></param>
        /// <param name="value"></param>
        public static void AddItem(Dictionary<string, string> dicExcel, string showName, object value)
        {
            if (null != value)
            {
                if (dicExcel.ContainsKey(showName))
                {
                    if (string.IsNullOrEmpty(dicExcel[showName]))//为空才替换，kevin
                        dicExcel[showName] = value.ToString();
                }
                else
                    dicExcel.Add(showName, value.ToString());
            }
            else
            {
                //对象为null，且不存在该键值则替换为""空
                if (!dicExcel.ContainsKey(showName))
                {
                    dicExcel.Add(showName, "");
                }
            }
        }
    }
}
