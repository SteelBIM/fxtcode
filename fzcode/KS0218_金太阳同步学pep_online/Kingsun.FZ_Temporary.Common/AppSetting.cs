using System;
using System.Configuration;

namespace Kingsun.FZ_Temporary.Common
{
    public class AppSetting
    {
        private static string _connectionString = string.Empty;
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["KingsunConnectionStr"].ConnectionString;
                }
                return _connectionString;
            }
        }
        /// <summary>
        /// 同步学链接
        /// </summary>
        public static string SyncConnectionString
        {
            get { return System.Configuration.ConfigurationManager.ConnectionStrings["KingsunConnectionStrSync"].ConnectionString; }
        }

        public static string GetValue(string key)
        {
            string result = "";
            if (string.IsNullOrEmpty(key))
            {
                return result;
            }
            result = ConfigurationManager.AppSettings[key];
            return result;
        }


        private static string _appID;
        public static string AppID
        {
            get
            {
                if (string.IsNullOrEmpty(_appID))
                {
                    _appID = ConfigurationManager.AppSettings["AppID"];
                }
                return _appID;
            }
        }

        private static string _cookieID;
        public static string CookieID
        {
            get
            {
                if (string.IsNullOrEmpty(_cookieID))
                {
                    _cookieID = ConfigurationManager.AppSettings["cookieName"];
                }
                return _cookieID;
            }
        }

        private static string _root;
        public static string Root
        {
            get
            {
                if (string.IsNullOrEmpty(_root))
                {
                    _root = ConfigurationManager.AppSettings["Root"];
                }
                return _root;
            }
        }

        private static string _task;
        public static string Task
        {
            get
            {
                if (string.IsNullOrEmpty(_task))
                {
                    _task = ConfigurationManager.AppSettings["Task"];
                }
                return _task;
            }
        }

        private static string _uploadFileUrl;
        /// <summary>
        /// 文件上传地址，上传题目文件时使用
        /// </summary>
        public static string UploadFileUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_uploadFileUrl))
                {
                    _uploadFileUrl = ConfigurationManager.AppSettings["UploadFileUrl"];
                }
                return _uploadFileUrl;
            }
        }

        private static bool _onlyCheck = false;
        /// <summary>
        /// 是否只用于校验，不进行导入操作
        /// </summary>
        public static bool OnlyCheck
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["OnlyCheck"]))
                {
                    _onlyCheck = Convert.ToBoolean(ConfigurationManager.AppSettings["OnlyCheck"]);
                }
                return _onlyCheck;
            }
        }

        private static string _editionName;
        /// <summary>
        /// 导入工具配置的版本名称
        /// </summary>
        public static string EditionName
        {
            get
            {
                if (string.IsNullOrEmpty(_editionName))
                {
                    _editionName = ConfigurationManager.AppSettings["EditionName"];
                }
                return _editionName;
            }
        }

        private static int _editionID = 0;
        /// <summary>
        /// 导入工具配置的版本ID
        /// </summary>
        public static int EditionID
        {
            get
            {
                if (_editionID == 0)
                {
                    _editionID = Convert.ToInt32(ConfigurationManager.AppSettings["EditionID"].ToString());
                }
                return _editionID;
            }
        }
    }
}
