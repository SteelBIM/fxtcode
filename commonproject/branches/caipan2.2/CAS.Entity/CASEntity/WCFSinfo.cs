using System;

namespace CAS.Entity.CASEntity
{
    /// <summary>
    /// WCFAPI安全参数
    /// </summary>
    public class WCFSInfo
    {
        /// <summary>
        /// apicode
        /// </summary>
       public int appid { get; set; }
        /// <summary>
       /// api密码
        /// </summary>
       public string apppwd { get; set; }
        /// <summary>
       /// 公司标示
        /// </summary>
       public string signname { get; set; }
       private string _time = DateTime.Now.ToString("yyyyMMddHHmmss");
        /// <summary>
       /// 时间戳（格式：yyyyMMddHHmmss）有默认值；
        /// </summary>
       public string time { get { return _time; } set { _time = value; } }
        /// <summary>
       /// 安全参数加密key
        /// </summary>
       public string appkey { get; set; }
        /// <summary>
       ///   功能名
        /// </summary>
       public string functionname { get; set; }
    }

    /// <summary>
    /// WCFAPI用户参数
    /// </summary>
    public class WCFUInfo
    {
        private string _username = "";
        /// <summary>
        /// 账号
        /// </summary>
        public string username { get { return _username; } set { _username = value; } }
        private string _token = "";
        /// <summary>
        /// Token
        /// </summary>
        public string token { get { return _token; } set { _token = value; } }

        private string _password = "";
        /// <summary>
        /// password
        /// </summary>
        public string password { get { return _password; } set { _password = value; } }
        
    }

    /// <summary>
    /// WCFAPI用户参数
    /// </summary>
    public class WCFAPPInfo
    {
        private string _splatype = "windows";
        /// <summary>
         /// 平台类型，默认：windows
        /// </summary>
        public string splatype { get { return _splatype; } set { _splatype = value; } }

        private string _platVer = "";
        /// <summary>
        /// 平台类型版本
        /// </summary>
        public string platVer { get { return _platVer; } set { _platVer = value; } }

        private string _stype = "cas";
        /// <summary>
        /// 产品类型，默认:cas
        /// </summary>
        public string stype { get { return _stype; } set { _stype = value; } }

         private string _version = "";
         /// <summary>
         /// 产品类型版本名
        /// </summary>
        public string version { get { return _version; } set { _version = value; } }

        private string _vcode = "";
        /// <summary>
        /// 产品类型版本号
        /// </summary>
        public string vcode { get { return _vcode; } set { _vcode = value; } }

         private int _systypecode = 1003001;
        /// <summary>
         /// 产品类型编号，默认：1003001
        /// </summary>
         public int systypecode { get { return _systypecode; } set { _systypecode = value; } }

        private string _channel = "web";
        /// <summary>
        ///  产品发布渠道，默认：web
        /// </summary>
        public string channel { get { return _channel; } set { _channel = value; } }
        
    }
}
