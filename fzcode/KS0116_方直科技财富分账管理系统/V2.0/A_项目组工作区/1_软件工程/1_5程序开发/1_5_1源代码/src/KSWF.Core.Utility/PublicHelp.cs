using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace KSWF.Core.Utility
{
    public class PublicHelp
    {
        /// <summary>
        /// 公司唯一id
        /// </summary>
        /// <returns></returns>
        public static string OrgId
        {
            get
            {
                return "KSWF";
            }
        }

        public static string AgentId()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssffff");
        }

        #region  过滤sql特殊字符  public static string DelSQLStr(string str)
        /// <summary>
        /// 过滤sql特殊字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string DelSQLStr(string str)
        {
            if (str == null || str == "")
                return "";
            str = str.Replace(";", "");
            str = str.Replace("'", "");
            str = str.Replace("&", "");
            str = str.Replace("%20", "");
            str = str.Replace("--", "");
            str = str.Replace("==", "");
            str = str.Replace("<", "");
            str = str.Replace(">", "");
            str = str.Replace("%", "");
            str = str.Replace("+", "");
            str = str.Replace("-", "");
            str = str.Replace("=", "");
            str = str.Replace(",", "");
            return str;
        }
        #endregion

        #region  MD5加密  string pswToSecurity(string strpsw)
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="strpsw">要加密的字符串</param>
        /// <returns>加密结果</returns>
        public static string pswToSecurity(string strpsw)
        {
            if (!string.IsNullOrEmpty(strpsw) && strpsw.Length != 0)
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(strpsw, "MD5");
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion

        /// <summary>
        /// 将数字转成日期类型
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static DateTime ConvertIntDateTime(double d)
        {
            System.DateTime time = System.DateTime.MinValue;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            time = startTime.AddSeconds(d);
            return time;
        }
        ///
        /// 将c# DateTime时间格式转换为Unix时间戳格式
        ///
        /// 时间
        /// double
        public static double ConvertDateTimeInt(System.DateTime time)
        {
            double intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            intResult = (time - startTime).TotalSeconds;
            return intResult;
        }
        public static string Percentage(decimal? number)
        {
            if (number > 0)
            {
                return number * 100 + "%";
            }
            return "";
        }
        /// <summary>
        /// 产品渠道来源
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public static string ProductName(int? channel)
        {
            if (channel == 1)
                return "同步学";
            else if (channel == 2)
                return "C++客户端";
            else if (channel == 4)
                return "人教C++客户端";
            else if (channel == 201)
                return "快乐习字";
            else if (channel == 202)
                return "LIP正音";
            else if (channel == 301)
                return "人教优学APP";
            else if (channel == 302)
                return "人教优学客户端";
              else if (channel == 303)
                return "同步学人教PEP";
            return channel.ToString();
        }
        /// <summary>
        /// decimal取两位小数
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        public static string TakeTwoDecimalPlaces(decimal? money)
        {
            if (money != null)
            {
                if (money > 0)
                {
                    return Convert.ToDecimal(money).ToString("0.00");
                }
            }
            return money.ToString();
        }

        #region 字符串加密

        /// <summary>   
        /// 利用DES加密算法加密字符串（可解密）   
        /// </summary>   
        /// <param name="plaintext">被加密的字符串</param>   
        /// <param name="key">密钥（只支持8个字节的密钥）</param>   
        /// <returns>加密后的字符串</returns>   
        public static string EncryptString(string plaintext, string key)
        {
            //访问数据加密标准(DES)算法的加密服务提供程序 (CSP) 版本的包装对象   
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Key = ASCIIEncoding.ASCII.GetBytes(key);　//建立加密对象的密钥和偏移量   
            des.IV = ASCIIEncoding.ASCII.GetBytes(key);　 //原文使用ASCIIEncoding.ASCII方法的GetBytes方法   

            byte[] inputByteArray = Encoding.Default.GetBytes(plaintext);//把字符串放到byte数组中   

            MemoryStream ms = new MemoryStream();//创建其支持存储区为内存的流　   
            //定义将数据流链接到加密转换的流   
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            //上面已经完成了把加密后的结果放到内存中去   
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }

        /// <summary>   
        /// 利用DES解密算法解密密文（可解密）   
        /// </summary>   
        /// <param name="ciphertext">被解密的字符串</param>   
        /// <param name="key">密钥（只支持8个字节的密钥，同前面的加密密钥相同）</param>   
        /// <returns>返回被解密的字符串</returns>   
        public static string DecryptString(string ciphertext, string key)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                byte[] inputByteArray = new byte[ciphertext.Length / 2];
                for (int x = 0; x < ciphertext.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(ciphertext.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }

                des.Key = ASCIIEncoding.ASCII.GetBytes(key);　//建立加密对象的密钥和偏移量，此值重要，不能修改   
                des.IV = ASCIIEncoding.ASCII.GetBytes(key);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);

                cs.Write(inputByteArray, 0, inputByteArray.Length);

                cs.FlushFinalBlock();

                //建立StringBuild对象，createDecrypt使用的是流对象，必须把解密后的文本变成流对象   
                StringBuilder ret = new StringBuilder();

                return System.Text.Encoding.Default.GetString(ms.ToArray());
            }
            catch (Exception)
            {
                return "error";
            }
        }

        #endregion


        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        /// <returns>若失败则返回回送地址</returns>
        public static string GetIP()
        {
            //如果客户端使用了代理服务器，则利用HTTP_X_FORWARDED_FOR找到客户端IP地址
            string userHostAddress = HttpContext.Current.Request.ServerVariables.Get("Remote_Addr").ToString();
            //否则直接读取REMOTE_ADDR获取客户端IP地址
            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            //前两者均失败，则利用Request.UserHostAddress属性获取IP地址，但此时无法确定该IP是客户端IP还是代理IP
            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = HttpContext.Current.Request.UserHostAddress;
            }
            //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
            if (!string.IsNullOrEmpty(userHostAddress) && IsIP(userHostAddress))
            {
                return userHostAddress;
            }
            return "127.0.0.1";
        }
        /// <summary>
        /// 检查IP地址格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
    }
}
