using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.PSO
{
   public class Common
    {
        /// <summary>
        /// 获得客户端的ip地址
        /// </summary>
        /// <returns></returns>
        public static string GetIPAddress()
        {
            string user_IP = string.Empty;
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
            {
                if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                {
                    user_IP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                }
                else
                {
                    user_IP = System.Web.HttpContext.Current.Request.UserHostAddress;
                }
            }
            else
            {
                user_IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            return user_IP;
        }


        #region "产生GUID"
        /// <summary>
        /// 获取一个GUID的HashCode
        /// </summary>
        public static int GetGUIDHashCode
        {
            get
            {
                return Math.Abs(GetGUID.GetHashCode());
            }
        }
        /// <summary>
        /// 获取一个GUID字符串

        /// </summary>
        public static string GetGUID
        {
            get
            {
                return Guid.NewGuid().ToString();
            }
        }
        #endregion

        /// <summary>
        /// 将url中的参数编码，以便于在url中传递

        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string URLEnCode(string url)
        {
            string codeURL = url.Replace('?', '*');
            codeURL = codeURL.Replace('&', '^');
            return codeURL;
        }

        /// <summary>
        /// 将url中的参数解码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string URLDeCode(string url)
        {
            string codeURL = url.Replace('*', '?');
            codeURL = codeURL.Replace('^', '&');
            return codeURL;
        }

        #region "按当前日期和时间生成随机数"
        /// <summary>
        /// 按当前日期和时间生成随机数

        /// </summary>
        /// <param name="Num">附加随机数长度</param>
        /// <returns></returns>
        public static string sRndNum(int Num)
        {
            string sTmp_Str = System.DateTime.Today.Year.ToString() + System.DateTime.Today.Month.ToString("00") + System.DateTime.Today.Day.ToString("00") + System.DateTime.Now.Hour.ToString("00") + System.DateTime.Now.Minute.ToString("00") + System.DateTime.Now.Second.ToString("00");
            return sTmp_Str + RndNum(Num);
        }
        #endregion

        #region 生成0-9随机数

        /// <summary>
        /// 生成0-9随机数

        /// </summary>
        /// <param name="VcodeNum">生成长度</param>
        /// <returns></returns>
        public static string RndNum(int VcodeNum)
        {
            StringBuilder sb = new StringBuilder(VcodeNum);
            Random rand = new Random();
            for (int i = 1; i < VcodeNum + 1; i++)
            {
                int t = rand.Next(9);
                sb.AppendFormat("{0}", t);
            }
            return sb.ToString();
        }
        #endregion

    }

    /// <summary>
    ///安全验证码类
    /// </summary>
    public class Security
    {
        public string seedKey
        {
            get;
            set;
        }
        public string SecurityKey
        {
            get;
            set;
        }
        /// <summary>
        /// 用于随机产生验证码

        /// </summary>
        public Security()
        {
            Random Seed = new Random();
            seedKey = Seed.Next(1, int.MaxValue).ToString();
            SecurityKey = GetCode(seedKey);
        }

        /// <summary>
        /// 根据提供的原始码生成验证码

        /// </summary>
        /// <param name="paraSeedKey"></param>
        public Security(string paraSeedKey)
        {
            seedKey = paraSeedKey;
            SecurityKey = GetCode(paraSeedKey);
        }

        ///// <summary>
        ///// 使用已有的验证码重新hash获得新验证码
        ///// </summary>
        ///// <param name="security"></param>
        //public Security(string security)
        //{
        //    SecurityKey = GetCode(security);
        //}
        private string GetCode(string security)
        {
            byte[] Value;
            UnicodeEncoding Code = new UnicodeEncoding();
            byte[] Message = Code.GetBytes(security);
            SHA512Managed Arithmetic = new SHA512Managed();
            Value = Arithmetic.ComputeHash(Message);
            string outSecurity = "";
            foreach (byte o in Value)
            {
                outSecurity += (int)o + "O";
            }
            return outSecurity;
        }
    }

    /// <summary>
    /// xxtea 的摘要说明。
    /// </summary>
    public class xxtea
    {
        private static Byte[] Encrypt(Byte[] Data, Byte[] Key)
        {
            if (Data.Length == 0)
            {
                return Data;
            }
            return ToByteArray(Encrypt(ToUInt32Array(Data, true), ToUInt32Array(Key, false)), false);
        }
        private static Byte[] Decrypt(Byte[] Data, Byte[] Key)
        {
            if (Data.Length == 0)
            {
                return Data;
            }
            return ToByteArray(Decrypt(ToUInt32Array(Data, false), ToUInt32Array(Key, false)), true);
        }

        private static UInt32[] Encrypt(UInt32[] v, UInt32[] k)
        {
            Int32 n = v.Length - 1;
            if (n < 1)
            {
                return v;
            }
            if (k.Length < 4)
            {
                UInt32[] Key = new UInt32[4];
                k.CopyTo(Key, 0);
                k = Key;
            }
            UInt32 z = v[n], y = v[0], delta = 0x9E3779B9, sum = 0, e;
            Int32 p, q = 6 + 52 / (n + 1);
            while (q-- > 0)
            {
                sum = unchecked(sum + delta);
                e = sum >> 2 & 3;
                for (p = 0; p < n; p++)
                {
                    y = v[p + 1];
                    z = unchecked(v[p] += (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z));
                }
                y = v[0];
                z = unchecked(v[n] += (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z));
            }
            return v;
        }
        private static UInt32[] Decrypt(UInt32[] v, UInt32[] k)
        {
            Int32 n = v.Length - 1;
            if (n < 1)
            {
                return v;
            }
            if (k.Length < 4)
            {
                UInt32[] Key = new UInt32[4];
                k.CopyTo(Key, 0);
                k = Key;
            }
            UInt32 z = v[n], y = v[0], delta = 0x9E3779B9, sum, e;
            Int32 p, q = 6 + 52 / (n + 1);
            sum = unchecked((UInt32)(q * delta));
            while (sum != 0)
            {
                e = sum >> 2 & 3;
                for (p = n; p > 0; p--)
                {
                    z = v[p - 1];
                    y = unchecked(v[p] -= (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z));
                }
                z = v[n];
                y = unchecked(v[0] -= (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z));
                sum = unchecked(sum - delta);
            }
            return v;
        }
        private static UInt32[] ToUInt32Array(Byte[] Data, Boolean IncludeLength)
        {
            Int32 n = (((Data.Length & 3) == 0) ? (Data.Length >> 2) : ((Data.Length >> 2) + 1));
            UInt32[] Result;
            if (IncludeLength)
            {
                Result = new UInt32[n + 1];
                Result[n] = (UInt32)Data.Length;
            }
            else
            {
                Result = new UInt32[n];
            }
            n = Data.Length;
            for (Int32 i = 0; i < n; i++)
            {
                Result[i >> 2] |= (UInt32)Data[i] << ((i & 3) << 3);
            }
            return Result;
        }
        private static Byte[] ToByteArray(UInt32[] Data, Boolean IncludeLength)
        {
            Int32 n;
            if (IncludeLength)
            {
                n = (Int32)Data[Data.Length - 1];
            }
            else
            {
                n = Data.Length << 2;
            }
            Byte[] Result = new Byte[n];
            for (Int32 i = 0; i < n; i++)
            {
                Result[i] = (Byte)(Data[i >> 2] >> ((i & 3) << 3));
            }
            return Result;
        }
        public static string Encrypt(string Target)
        {
            return Encrypt(Target, string.Empty);
        }
        public static string Encrypt(string Target, string Key)
        {
            if (0 == Key.Length)
                Key = "1234567890abcdef";
            System.Text.Encoding encoder = System.Text.Encoding.UTF8;
            Byte[] data = Encrypt(encoder.GetBytes(Target), encoder.GetBytes(Key));
            return System.Convert.ToBase64String(data);

        }
        public static string Decrypt(string Target)
        {
            return Decrypt(Target, string.Empty);
        }
        public static string Decrypt(string Target, string Key)
        {
            if (Target != null)
            {
                if (0 == Key.Length)
                    Key = "1234567890abcdef";
                System.Text.Encoding encoder = System.Text.Encoding.UTF8;
                return encoder.GetString(Decrypt(System.Convert.FromBase64String(Target), encoder.GetBytes(Key)));
            }
            else
            {
                return string.Empty;
            }
        }
    }

    /// <summary>
    /// 页面跳转辅助类，封装页面跳转操作
    /// </summary>
    public class UserServiceBase
    {
        string[] nameList = new string[]{"Command","AppID","UserID","UserName","PassWord",
            "UserIP","Email","PreviousURL","CacheID"};//跳转URL参数列表
        string[] valueList = new string[9];//跳转URL参数值列表        
        //默认跳转URL
        private string directURL = ConfigurationManager.AppSettings["uumsRoot"].ToString() + "/UserService/LoginOut.aspx?";

        /// <summary>
        /// 生成跳转URL和参数信息

        /// </summary>
        /// <returns></returns>
        public string GetURL()
        {
            if (directURL.Length != 0 && directURL != null)
            {
                StringBuilder returnURL = new StringBuilder();

                returnURL.Append(directURL);

                //添加参数名和参数值

                for (int i = 0; i < nameList.Length; i++)
                {
                    returnURL.Append(nameList[i]);
                    returnURL.Append("=");
                    returnURL.Append(valueList[i]);
                    returnURL.Append("&");
                }

                //生成安全校验码

                Security security = new Security();
                string securityKey = security.SecurityKey;

                returnURL.Append("securityKey=");
                returnURL.Append(securityKey);

                return returnURL.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 登录接口
        /// </summary>
        /// <param name="appID">应用ID</param>
        /// <param name="userName">用户名</param>
        /// <param name="userPwd">用户密码</param>
        /// <param name="userIP">用户IP</param>
        /// <param name="paraURL">当前URL</param>
        public void LogIn(string appID, string userName, string userPwd, string userIP, string paraURL)
        {
            string currentURL = FilterURLMessage(paraURL);
            SetList("CheckLogin", appID, string.Empty, userName, userPwd, userIP, string.Empty, currentURL, string.Empty);
        }

        /// <summary>
        /// 注销接口
        /// </summary>
        /// <param name="appID">应用ID</param>
        /// <param name="userName">用户名</param>
        /// <param name="paraURL">当前URL</param>
        public void LogOut(string appID, string userName, string paraURL)
        {
            string currentURL = FilterURLMessage(paraURL);
            SetList("LogoutUser", appID, string.Empty, userName, string.Empty, string.Empty, string.Empty, currentURL, string.Empty);
        }

        /// <summary>
        /// 注册用户接口
        /// </summary>
        /// <param name="appID">应用ID</param>
        /// <param name="userName">用户名</param>
        /// <param name="userPwd">用户密码</param>
        /// <param name="userIP">用户IP</param>
        /// <param name="userEmail">用户邮箱</param>
        /// <param name="paraURL">当前URL</param>
        public void Register(string appID, string userName, string userPwd, string userIP, string userEmail, string paraURL)
        {
            string currentURL = FilterURLMessage(paraURL);
            SetList("RegUser", appID, string.Empty, userName, userPwd, userIP, userEmail, currentURL, string.Empty);
        }

        /// <summary>
        /// 更新用户信息接口
        /// </summary>
        /// <param name="appID">应用ID</param>
        /// <param name="userName">用户名</param>
        /// <param name="userPwd">用户密码</param>
        /// <param name="userEmail">用户邮箱</param>
        /// <param name="paraURL">当前URL</param>
        public void Update(string appID, string userName, string userPwd, string userEmail, string paraURL)
        {
            string currentURL = FilterURLMessage(paraURL);
            SetList("UpdateUser", appID, string.Empty, userName, userPwd, string.Empty, userEmail, currentURL, string.Empty);
        }

        /// <summary>
        /// 查询密码接口
        /// </summary>
        /// <param name="appID">应用ID</param>
        /// <param name="userName">用户名</param>
        /// <param name="paraURL">当前URL</param>
        public void GetPassWord(string appID, string userName, string paraURL)
        {
            string currentURL = FilterURLMessage(paraURL);
            SetList("GetPassWord", appID, string.Empty, userName, string.Empty, string.Empty, string.Empty, currentURL, string.Empty);
        }

        /// <summary>
        /// 查询应用ID接口
        /// </summary>
        /// <param name="appID">应用ID</param>
        /// <param name="paraURL">当前URL</param>
        public void CheckAppID(string appID, string paraURL)
        {
            string currentURL = FilterURLMessage(paraURL);
            SetList("CheckAppID", appID, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, currentURL, string.Empty);
        }

        /// <summary>
        /// 设置网址参数信息
        /// </summary>
        /// <param name="command">跳转命令</param>
        /// <param name="appID">应用ID</param>
        /// <param name="userID">用户ID</param>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">用户密码</param>
        /// <param name="userIP">用户IP</param>
        /// <param name="email">用户邮箱</param>
        /// <param name="previousURL">当前URL</param>
        /// <param name="cacheID">缓存ID</param>
        private void SetList(string command, string appID, string userID, string userName, string passWord,
            string userIP, string email, string previousURL, string cacheID)
        {
            if (command.Length != 0)
            {
                valueList[0] = command;
            }
            if (command.Length != 0)
            {
                valueList[1] = appID;
            }
            if (command.Length != 0)
            {
                valueList[2] = userID;
            }
            if (command.Length != 0)
            {
                valueList[3] = userName;
            }
            if (command.Length != 0)
            {
                valueList[4] = passWord;
            }
            if (command.Length != 0)
            {
                valueList[5] = userIP;
            }
            if (command.Length != 0)
            {
                valueList[6] = email;
            }
            if (command.Length != 0)
            {
                valueList[7] = previousURL;
            }
            if (command.Length != 0)
            {
                valueList[8] = cacheID;
            }
        }

        /// <summary>
        /// 填充默认参数列表中的参数值
        /// </summary>
        /// <param name="valueList"></param>
        /// <returns></returns>
        private bool SetList(List<string> paraValueList)
        {
            if (nameList.Length == paraValueList.Count)
            {
                //填充参数值

                for (int i = 0; i < nameList.Length; i++)
                {
                    valueList[i] = paraValueList[i];
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 过滤网址中的mess消息信息，防止消息重复出现
        /// </summary>
        /// <param name="url">当前URL</param>
        /// <returns>过滤后结果</returns>
        private string FilterURLMessage(string url)
        {
            int endIndex = url.IndexOf('?');
            if (endIndex != -1)
            {
                url = url.Substring(0, endIndex);
            }

            return url;
        }


        /// <summary>
        /// 默认跳转URL
        /// </summary>
        public string DirectURL
        {
            get
            {
                return directURL;
            }
            set
            {
                directURL = value;
            }
        }
    }
}
