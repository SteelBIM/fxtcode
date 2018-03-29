using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace FxtSpider.FxtApi.ApiManager
{
    public static class WcfCheck
    {
        public static string GetWcfCheckIp()
        {
            return "";
        }
        public static string GetWcfCheckValidData()
        {
            return "";
        }
        public static string GetWcfCheckValidDate()
        {
            return "abc";
        }
        public static string GetWcfCheckValidCode()
        {
            return GetCode(GetWcfCheckValidDate());
        }
        /// <summary>
        /// 得到验证码
        /// </summary>
        /// <returns></returns>
        public static string GetCode(string strCode)
        {
            string strDay = DateTime.Now.ToString("yyyy-MM-dd");
            string strDayReverse = string.Empty;
            IEnumerable<char> iableReverse = strDay.Reverse();
            foreach (char chReverse in iableReverse)
            {
                strDayReverse += chReverse;
            }
            return GetMd5(string.Format("{0}{1}{2}", strDay, strCode, strDayReverse));
        }

        /// <summary>
        /// 进行MD5效验
        /// </summary>
        /// <param name="strmd5"></param>
        /// <returns></returns>
        static string GetMd5(string strmd5)
        {
            byte[] md5Bytes = ASCIIEncoding.Default.GetBytes(strmd5);
            byte[] encodedBytes;
            MD5 md5 = new MD5CryptoServiceProvider();
            encodedBytes = md5.ComputeHash(md5Bytes);
            string nn = BitConverter.ToString(encodedBytes);
            nn = Regex.Replace(nn, "-", "");//因为转化完的都是34-2d这样的，所以替换掉- 
            nn = nn.ToLower();//根据需要转化成小写
            return nn;
        }
    }
}
