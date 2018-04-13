using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.SynchronousStudy.Common
{
    public class AppSetting
    {
        private static string _connectionString = string.Empty;
        private static string _interestdubbinggamenectionString = string.Empty;
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
        public static string InterestDubbingGameConnectionStr
        {
            get
            {
                if (string.IsNullOrEmpty(_interestdubbinggamenectionString))
                {
                    string k = "KS0210KINGSUNSOFT2008123456789";
                    _interestdubbinggamenectionString =
                        System.Configuration.ConfigurationManager.ConnectionStrings["InterestDubbingGameConnectionStr"]
                            .ConnectionString;
                    //_connectionString = xxtea.Decrypt(_connectionString, k);
                }
                return _interestdubbinggamenectionString;
            }
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

        private static string _uumsRootUrl;

        public static string UumsRootUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_uumsRootUrl))
                {
                    _uumsRootUrl = ConfigurationManager.AppSettings["uumsRoot"];
                }
                return _uumsRootUrl;
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

        private static string _fileServerUrl;

        public static string FileServerUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_fileServerUrl))
                {
                    _fileServerUrl = ConfigurationManager.AppSettings["FileServerUrl"] + "GetFiles.ashx";
                }
                return _fileServerUrl;
            }
        }

        private static string _previewUrl;

        public static string PreviewUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_previewUrl))
                {
                    _previewUrl = ConfigurationManager.AppSettings["FileServerUrl"] + "Preview.ashx";
                }
                return _previewUrl;
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
                    _uploadFileUrl = ConfigurationManager.AppSettings["TaskHelperUrl"];
                }
                return _uploadFileUrl;
            }
        }

        private static string _DownZipFile;
        /// <summary>
        /// 文件上传地址，上传题目文件时使用
        /// </summary>
        public static string DownZipFile
        {
            get
            {
                if (string.IsNullOrEmpty(_DownZipFile))
                {
                    _DownZipFile = ConfigurationManager.AppSettings["DownZipFile"] as string;
                }
                return _DownZipFile;
            }
        }


        /// <summary>
        /// 根据userid 往redis添加对应版本
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public static bool SetValidUserRecord(string UserId)
        {
            //RedisHashHelper redis = new RedisHashHelper();
            //if (string.IsNullOrEmpty(UserId))
            //{
            //    return false;
            //}
            //string value = redis.Get("ValidUserRecord_Day" + DateTime.Now.ToShortDateString(), UserId);//从Redis读取值
            //if (string.IsNullOrEmpty(value))
            //{
            //    redis.Set("ValidUserRecord_Day" + DateTime.Now.ToShortDateString(), UserId, DateTime.Now.ToShortDateString());//写入Redis
            //    return true;
            //}
            //else
            //{
            //    if (Convert.ToDateTime(value).ToShortDateString() != DateTime.Now.ToShortDateString())
            //    {
            //        redis.Set("ValidUserRecord_Day" + DateTime.Now.ToShortDateString(), UserId, DateTime.Now.ToShortDateString());//写入Redis
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            return false;
        }

        /// <summary>
        /// 登录埋点
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="AppId"></param>
        /// <param name="Versions">版本</param>
        /// <param name="LoginType">0登录 1注册</param>
        /// <param name="DownloadChannel">渠道</param>
        /// <returns></returns>
        public static bool SetValidUserLogin(string UserId, string AppId, string Versions,  int DownloadChannel = 0)
        {
            RedisHashHelper redis = new RedisHashHelper();
            if (string.IsNullOrEmpty(UserId))
            {
                return false;
            }
            if (string.IsNullOrEmpty(Versions))
                Versions = "";

            string key = AppId + "_" + UserId;

            if (string.IsNullOrEmpty(redis.Get("UserLoginRecord", key)))
            {
                UserLogin entity = new UserLogin() { UserId = UserId, AppId = AppId, CreateTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")), DownloadChannel = DownloadChannel, Versions = Versions, LoginType = 0 };
                return redis.Set("UserLoginRecord", key, entity);
            }
            return true;
        }

        /// <summary>
        /// 注册埋点
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="AppId"></param>
        /// <param name="Versions">版本</param>
        /// <param name="LoginType">0登录 1注册</param>
        /// <param name="DownloadChannel">渠道</param>
        /// <returns></returns>
        public static bool SetValidUserRegister(string UserId, string AppId, string Versions, int DownloadChannel = 0)
        {
            RedisHashHelper redis = new RedisHashHelper();
            if (string.IsNullOrEmpty(UserId))
            {
                return false;
            }
            if (string.IsNullOrEmpty(Versions))
                Versions = "";

            string key = AppId + "_" + UserId;
            UserLogin entity = new UserLogin() { UserId = UserId, AppId = AppId, CreateTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")), DownloadChannel = DownloadChannel, Versions = Versions, LoginType = 1 };
            RedisHashHelper hase = new RedisHashHelper();
            return redis.Set("UseLoginRecord", key, entity);

        }

        /// <summary>
        /// 记录使用次数
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="AppId"></param>
        /// <param name="Versions"></param>
        /// <param name="DownloadChannel"></param>
        /// <returns></returns>
        public static bool SetValidUserUsageNumber(string UserId, string AppId, string Versions, int DownloadChannel = 0)
        {
            RedisHashHelper redis = new RedisHashHelper();
            if (string.IsNullOrEmpty(UserId))
            {
                return false;
            }
            if (string.IsNullOrEmpty(Versions))
                Versions = "";

            UsageDetails usagedetailsentity = new UsageDetails();
            usagedetailsentity.AppId = AppId;
            usagedetailsentity.UsageNumber = 1;
            usagedetailsentity.UserId = UserId;
            usagedetailsentity.Versions = Versions;
            usagedetailsentity.UsageTimeLength = 0;
            usagedetailsentity.DownloadChannel = DownloadChannel;

            string tablename = "UserAppRecord_" + DateTime.Now.ToString("yyyy-MM-dd");
            string key = AppId + "_" + Versions + "_" + DownloadChannel + "_" + UserId;

            UsageDetails entiy = redis.Get<UsageDetails>(tablename, key);
            if (entiy != null)
                usagedetailsentity.UsageNumber = entiy.UsageNumber + 1;

            return redis.Set(tablename, key, usagedetailsentity);

        }

       /// <summary>
       /// 记录使用时长 
       /// </summary>
       /// <param name="UserId"></param>
       /// <param name="AppId"></param>
       /// <param name="Versions"></param>
       /// <param name="UsageTimeLength"></param>
       /// <param name="DownloadChannel"></param>
       /// <returns></returns>
        public static bool SetValidUserUsageTimeLength(string UserId, string AppId, string Versions,int UsageTimeLength, int DownloadChannel = 0)
        {
            RedisHashHelper redis = new RedisHashHelper();
            if (string.IsNullOrEmpty(UserId))
            {
                return false;
            }
            if (string.IsNullOrEmpty(Versions))
                Versions = "";

            UsageDetails usagedetailsentity = new UsageDetails();
            usagedetailsentity.AppId = AppId;
            usagedetailsentity.UsageNumber = 1;
            usagedetailsentity.UsageTimeLength = UsageTimeLength;
            usagedetailsentity.UserId = UserId;
            usagedetailsentity.Versions = Versions;
            usagedetailsentity.DownloadChannel = DownloadChannel;

            string tablename = "UserAppTimesRecord_" + DateTime.Now.ToString("yyyy-MM-dd");
            string key = AppId + "_" + Versions + "_" + DownloadChannel + "_" + UserId;

            UsageDetails entity = redis.Get<UsageDetails>(tablename, key);

            if (entity != null)
            {
                usagedetailsentity.UsageNumber = entity.UsageNumber + 1;
                usagedetailsentity.UsageTimeLength = entity.UsageTimeLength + usagedetailsentity.UsageTimeLength;
            }
            bool reust= redis.Set(tablename, key, usagedetailsentity);
            if (entity != null)
                return true;
            return reust;
        }
      

    }
}
