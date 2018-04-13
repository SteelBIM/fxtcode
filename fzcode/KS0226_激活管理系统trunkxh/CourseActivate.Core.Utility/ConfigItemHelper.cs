using System;
using System.Configuration;
using System.Web;

namespace CourseActivate.Core.Utility
{
    /// <summary>
    /// 配置节帮助类
    /// </summary>
    public static class ConfigItemHelper
    {

        //公钥,用于RSA加密
        public static string PublicKey
        {
            get
            {
                string defaultResult = string.Empty;
                return string.IsNullOrEmpty(publicKey) ? defaultResult : publicKey;
            }

        }

        //公钥,用于RSA解密
        public static string PrivateKey
        {
            get
            {
                string defaultResult = string.Empty;
                return string.IsNullOrEmpty(privateKey) ? defaultResult : privateKey;
            }

        }
        /// <summary>
        /// 是否从文件读取秘钥
        /// </summary>
        public static bool KeyIsReadFile
        {
            get
            {
                bool defaultResult;
                return bool.TryParse(keyIsReadFile, out defaultResult) && defaultResult;

            }

        }

        /// <summary>
        /// 
        /// </summary>
        public static string ReadPath
        {
            get
            {
                string defaultResult = "D:\\RJ_PEP_3A\\";
                return string.IsNullOrEmpty(readPath) ? defaultResult : readPath;

            }

        }

        /// <summary>
        /// 
        /// </summary>
        public static string WebHost
        {
            get
            {
                string defaultResult = string.Empty;
                return string.IsNullOrEmpty(webHost) ? defaultResult : webHost;

            }
        }

        /// <summary>
        /// 过期失效
        /// </summary>
        public static int EndDate
        {
            get
            {
                int defaultResult = 0;
                int.TryParse(endDate, out defaultResult);
                return defaultResult;

            }

        }

        private static int _SyncDBCount = -1;

        public static int SyncDBCount
        {
            get
            {
                if (_SyncDBCount == -1)
                {
                    string str = ConfigurationManager.AppSettings["SyncDBCount"];
                    int.TryParse(str, out _SyncDBCount);
                }
                return _SyncDBCount;
            }
        }

        private static int _SyncDBTimes = -1;

        public static int SyncDBTimes
        {
            get
            {
                if (_SyncDBTimes == -1)
                {
                    string str = ConfigurationManager.AppSettings["SyncDBTimes"];
                    int.TryParse(str, out _SyncDBTimes);
                }
                return _SyncDBTimes;
            }

        }


        public static string GetPrivateKey()
        {
            string privatKeyPaht = "~/EncryptKey";
            privatKeyPaht = HttpContext.Current.Server.MapPath(privatKeyPaht) + "\\PrivateKey.txt";
            string privatTxt = FileOperate.ReadFile(privatKeyPaht);

            string privatKey = privatTxt.Replace("-----BEGIN PRIVATE KEY-----", "")
                    .Replace("-----END PRIVATE KEY-----", "")
                    .Replace("\n", "")
                    .Replace("\r", "")
                    .Replace(" ", "");
            return privatKey;
        }

        private static readonly string readPath = ConfigurationManager.AppSettings["ReadPath"];
        private static readonly string publicKey = ConfigurationManager.AppSettings["PublicKey"];
        private static readonly string privateKey = ConfigurationManager.AppSettings["PrivateKey"];

        private static readonly string keyIsReadFile = ConfigurationManager.AppSettings["KeyIsReadFile"];
        private static readonly string endDate = ConfigurationManager.AppSettings["EndDate"];
        private static readonly string webHost = ConfigurationManager.AppSettings["WebHost"];
    }
}
