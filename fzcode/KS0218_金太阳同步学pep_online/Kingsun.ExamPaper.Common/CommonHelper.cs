using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Kingsun.ExamPaper.Common
{
    public class CommonHelper
    {
        #region "检测是否为有效的电话"
        public static bool IsValidPhone(string phone)
        {
            Regex rcode = new Regex(@"^[1][3-8]+\d{9}$|^(0\d{2,3}-?|\(0\d{2,3}\))?[1-9]\d{4,7}(-\d{1,8})?$");
            if (!rcode.IsMatch(phone))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region "检测是否为有效邮件地址格式"
        /// <summary>
        /// 检测是否为有效邮件地址格式
        /// </summary>
        /// <param name="strIn">输入邮件地址</param>
        /// <returns></returns>
        public static bool IsValidEmail(string strIn)
        {
            return Regex.IsMatch(strIn, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        public static bool IsValidEmail1(string strValid)
        {
            Regex rCode = new Regex(@"^\w+((-\w+)|(\.\w+))*\@\w+((\.|-)\w+)*\.\w+$");
            if (!rCode.IsMatch(strValid))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region "通过RNGCryptoServiceProvider 生成随机数 0-9"
        /// <summary>
        /// 通过RNGCryptoServiceProvider 生成随机数 0-9 
        /// </summary>
        /// <param name="length">随机数长度</param>
        /// <returns></returns>
        public static string RndNumRNG(int length)
        {
            byte[] bytes = new byte[16];
            RNGCryptoServiceProvider r = new RNGCryptoServiceProvider();
            StringBuilder sb = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                r.GetBytes(bytes);
                sb.AppendFormat("{0}", (int)((decimal)bytes[0] / 256 * 10));
            }
            return sb.ToString();

        }
        #endregion
    }
}
