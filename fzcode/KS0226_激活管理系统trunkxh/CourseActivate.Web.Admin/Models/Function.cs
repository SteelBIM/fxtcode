using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace CourseActivate.Web.Admin.Models
{
    /// <summary>
    /// 验证类
    /// </summary>
    public class Function
    {
        #region 用正则表达式实现.验证输入是否是数字
        public bool IsValidNumer(string str)
        {
            Regex reg1
               = new Regex(@"^[-]?/d+[.]?/d*$");
            return reg1.IsMatch(str);
        }
        #endregion

        #region 验证是否为小数
        public bool IsValidDecimal(string str)
        {
            return Regex.IsMatch(str, @"[0]./d{1,2}|[1]");
        }

        #endregion

        #region 验证Email地址
        public bool IsValidEmail(string strIn)
        {
            // Return true if strIn is in valid e-mail format.
            return Regex.IsMatch(strIn, @"^([/w-/.]+)@((/[[0-9]{1,3}/.[0-9]{1,3}/.[0-9]{1,3}/.)|(([/w-]+/.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(/]?)$");
        }
        #endregion

        #region dd-mm-yy 的日期形式代替 mm/dd/yy 的日期形式。
        public string MDYToDMY(String input)
        {
            return Regex.Replace(input, "//b(?//d{1,2})/(?//d{1,2})/(?//d{2,4})//b", "${day}-${month}-${year}");
        }
        #endregion

        #region 验证是否为电话号码
        public bool IsValidTelNum(string strIn)
        {
            return Regex.IsMatch(strIn, @"(/d+-)?(/d{4}-?/d{7}|/d{3}-?/d{8}|^/d{7,8})(-/d+)?");
        }
        #endregion

        #region 验证年月日
        bool IsValidDate(string strIn)
        {
            return Regex.IsMatch(strIn, @"^2/d{3}-(?:0?[1-9]|1[0-2])-(?:0?[1-9]|[1-2]/d|3[0-1])(?:0?[1-9]|1/d|2[0-3])?:0?[1-9]|[1-5]/d)?:0?[1-9]|[1-5]/d)$");
        }
        #endregion

        #region 验证后缀名
        bool IsValidPostfix(string strIn)
        {
            return Regex.IsMatch(strIn, @"/.(?i:gif|jpg)$");
        }
        #endregion

        #region 验证字符串长度是否在指定的长度之间
        bool IsValidByte(string strIn,int start,int end)
        {
            return Regex.IsMatch(strIn, @"^[a-z]{" + start + "," + end + "}$");
        }
        #endregion

        #region 验证IP
        bool IsValidIp(string strIn)
        {
            return Regex.IsMatch(strIn, @"^(/d{1,2}|1/d/d|2[0-4]/d|25[0-5])/.(/d{1,2}|1/d/d|2[0-4]/d|25[0-5])/.(/d{1,2}|1/d/d|2[0-4]/d|25[0-5])/.(/d{1,2}|1/d/d|2[0-4]/d|25[0-5])$");
        }
        #endregion
    }
}