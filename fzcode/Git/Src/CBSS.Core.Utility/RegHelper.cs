using System;
using System.Text.RegularExpressions;

namespace CBSS.Core.Utility
{
    /// <summary>
    /// 常用正则匹配类
    /// </summary>
    public static class RegHelper
    {

        public static bool filterSql(this string str)
        {
            bool result = true;
            string SqlStr = @"exec|execute|select|insert|delete|update|alter|create|drop|\*|
                   chr|char|asc|mid|substring|master|truncate|declare|xp_cmdshell|restore|backup|
                   net +user|net +localgroup +administrators";
            string str_Regex = @"\b(" + SqlStr + @")\b";
            Regex Regex = new Regex(str_Regex, RegexOptions.IgnoreCase);
            if (true != Regex.IsMatch(str))
            {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 是否GUID
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsGUID(this string str)
        {
            Match m = Regex.Match(str, @"^[0-9a-f]{8}(-[0-9a-f]{4}){3}-[0-9a-f]{12}$", RegexOptions.IgnoreCase);
            if (m.Success)
            {
                //可以转换
                //Guid guid = new Guid(str);
                return true;
            }
            else
            {
                //不可转换
                return false;
            }
        }
        /// <summary>
        /// 是否电子邮件
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsEmail(string s)
        {
            string text1 = @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";
            return Regex.IsMatch(s, text1);
        }

        /// <summary>
        /// 是否Ip
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsIp(string s)
        {
            string text1 = @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$";
            return Regex.IsMatch(s, text1);
        }

        /// <summary>
        /// 是否整数
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNumeric(string s)
        {
            string text1 = @"^\-?[0-9]+$";
            return Regex.IsMatch(s, text1);
        }

        /// <summary>
        /// 是否绝对路径
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsPhysicalPath(string s)
        {
            string text1 = @"^\s*[a-zA-Z]:.*$";
            return Regex.IsMatch(s, text1);
        }

        /// <summary>
        /// 是否相对路径
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsRelativePath(string s)
        {
            if ((s == null) || (s == ""))
            {
                return false;
            }
            if (s.StartsWith("/") || s.StartsWith("?"))
            {
                return false;
            }
            if (Regex.IsMatch(s, @"^\s*[a-zA-Z]{1,10}:.*$"))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 是否安全字符串，例如包含"slect insert"等注入关键字
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsSafety(string s)
        {
            string text1 = s.Replace("%20", " ");
            text1 = Regex.Replace(text1, @"\s", " ");
            string text2 = "select |insert |delete from |count\\(|drop table|update |truncate |asc\\(|mid\\(|char\\(|xp_cmdshell|exec master|net localgroup administrators|:|net user|\"|\\'| or ";
            return !Regex.IsMatch(text1, text2, RegexOptions.IgnoreCase);
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“RegHelper.IsUnicode(string)”的 XML 注释
        public static bool IsUnicode(string s)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“RegHelper.IsUnicode(string)”的 XML 注释
        {
            string text1 = @"^[\u4E00-\u9FA5\uE815-\uFA29]+$";
            return Regex.IsMatch(s, text1);
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“RegHelper.IsUrl(string)”的 XML 注释
        public static bool IsUrl(string s)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“RegHelper.IsUrl(string)”的 XML 注释
        {
            string text1 = @"^(http|https|ftp|rtsp|mms):(\/\/|\\\\)[A-Za-z0-9%\-_@]+\.[A-Za-z0-9%\-_@]+[A-Za-z0-9\.\/=\?%\-&_~`@:\+!;]*$";
            return Regex.IsMatch(s, text1, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 是否是身份证号，验证以下3种情况:
        /// 1、身份证号码为15位数字；
        /// 2、身份证号码为18位数字；
        /// 3、身份证号码为17位数字+1个字母  
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsIdentityCard(string s)
        {
            return Regex.IsMatch(s, @"^(^\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 是否是手机号
        /// </summary>
        /// <param name="s"></param>
        /// <param name="isRestrict">是否按严格格式验证</param>
        /// <returns></returns>
        public static bool IsMobileNo(string s, bool isRestrict = false)
        {
            if (!isRestrict)
            {
                return Regex.IsMatch(s, @"^[1]\d{10}$", RegexOptions.IgnoreCase);
            }
            return Regex.IsMatch(s, @"^[1][3-8]\d{9}$", RegexOptions.IgnoreCase);
        }
    }
}

