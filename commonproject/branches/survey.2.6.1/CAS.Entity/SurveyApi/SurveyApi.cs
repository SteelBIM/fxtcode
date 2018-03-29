using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Web.Script.Serialization;
using System.Configuration;
using Newtonsoft.Json;

namespace CAS.Entity
{
    /*
     *  byte 
     *  2014-03-16
     */
    public class SurveyApi
    {
        private AllInfo _info;
        private SecurityInfo _sInfo;
        private string _urlApi;
        public SurveyApi(LoginInfoEntity userContext, string sysTypeCode, string appId)
        {
            _jss.MaxJsonLength = 10240000;
            List<Apps> apps = userContext.apps;
            string appPwd = "",
                appKey = "";
            if (null != apps && 0 < apps.Count)
            {
                Apps app = apps.FirstOrDefault(query => query.appid == appId);
                if (null != app)
                {
                    appPwd = app.apppwd;
                    appKey = app.appkey;
                    _urlApi = app.appurl;
                }
            }
            _sInfo = new SecurityInfo(userContext, appId, appPwd, appKey);
            _info = new AllInfo(userContext, sysTypeCode);

        }
        /// <summary>
        /// 估价宝使用.
        /// </summary>
        /// <param name="userContext"></param>
        public SurveyApi(LoginInfoEntity userContext)
            : this(userContext, "1003018", "1003102")
        {

        }
        public SecurityInfo sinfo
        {
            get
            {
                return _sInfo;
            }
            set
            {
                _sInfo = value;
            }
        }
        public AllInfo info
        {
            get
            {
                return _info;
            }
        }
        JavaScriptSerializer _jss = new JavaScriptSerializer();
        public string GetJsonString()
        {
            //string sinfo = _jss.Serialize(this._sInfo).Replace("\"", "'");
            //string info = _jss.Serialize(this._info).Replace("\"", "'");
            //return "{\"sinfo\":\"" + sinfo + "\",\"info\":\"" + info + "\"}";
            //modify \的问题  不能解析
            string sinfo = _jss.Serialize(this._sInfo);
            string info = _jss.Serialize(this._info);
            return new { sinfo = sinfo, info = info }.ToJsonString();
        }
        public string UrlApi
        {
            get
            {
                return _urlApi;
            }
            set
            {
                _urlApi = value;
            }
        }

    }
    /// <summary>
    /// 扩展类
    /// </summary>
    internal static class SurveyExtensionHelper
    {
        /// <summary>
        /// 将对象转化成json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJsonString(this Object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
    public class SecurityInfo
    {
        private string _signName,
            _appId,
            _appPwd,
            _time,
            _appKey;
        public SecurityInfo(LoginInfoEntity userContext)
        {
            _signName = userContext.signname;
        }
        public SecurityInfo(LoginInfoEntity userContext, string appId, string appPwd, string appKey)
        {
            _signName = userContext.signname;
            _appId = appId;
            _appPwd = appPwd;
            _time = DateTime.Now.ToString("yyyyMMddHHmmss");
            _appKey = appKey;
        }
        public string appid
        {
            get
            {
                return _appId;
            }
        }
        public string apppwd
        {
            get
            {
                return _appPwd;
            }
        }
        public string signname
        {
            get
            {
                return _signName;
            }
        }
        public string time
        {
            get
            {
                return _time;
            }
        }
        public string functionname
        {
            set;
            get;
        }
        private string[] _securityArray = new string[5];
        public string code
        {
            get
            {
                _securityArray[0] = this.appid;
                _securityArray[1] = this.apppwd;
                _securityArray[2] = this.signname;
                _securityArray[3] = this.time;
                _securityArray[4] = this.functionname;
                Array.Sort(_securityArray);
                return FormsAuthentication.HashPasswordForStoringInConfigFile(string.Join("", _securityArray) + _appKey
                    , "MD5").ToLower();
            }
        }
    }
    public class AllInfo
    {
        private UserInfo _uinfo;
        private ApplicationInfo _appinfo;
        public AllInfo(LoginInfoEntity userContext, string sysTypeCode)
        {
            _uinfo = new UserInfo(userContext);
            _appinfo = new ApplicationInfo(sysTypeCode);
        }
        public ApplicationInfo appinfo
        {
            get
            {
                return _appinfo;
            }
            set
            {
                _appinfo = value;
            }
        }

        public UserInfo uinfo
        {
            get
            {
                return _uinfo;
            }
        }
        public dynamic funinfo { get; set; }
    }
    public class ApplicationInfo
    {
        private string _systypecode = "";//默认值请勿修改
        public ApplicationInfo()
        {
        }
        public ApplicationInfo(string sysTypeCode)
        {
            _systypecode = sysTypeCode;
        }
        public string splatype
        {
            get
            {
                return ConfigurationManager.AppSettings["SplaType"];
            }
        }
        public string platver
        {
            get
            {
                return ConfigurationManager.AppSettings["PlatVer"];
            }
        }
        public string stype
        {
            get
            {
                return ConfigurationManager.AppSettings["SType"];
            }
        }
        public string version
        {
            get
            {
                return ConfigurationManager.AppSettings["Version"];
            }
        }
        public string vcode
        {
            get
            {
                return ConfigurationManager.AppSettings["VCode"];
            }
        }
        public string systypecode
        {
            get
            {
                return _systypecode;
            }
        }
        public string channel
        {
            get
            {
                return ConfigurationManager.AppSettings["Channel"];
            }
        }
    }
    public class UserInfo
    {
        private string _username;
        private string _password;
        private int _fxtcompanyid;
        private int _subcompanyid;
        public UserInfo(LoginInfoEntity userContext)
        {
            this._username = userContext.username;
            this._password = userContext.userpwd;
            this._fxtcompanyid = userContext.fxtcompanyid;
            this._subcompanyid = userContext.subcompanyid;
        }
        public string username
        {
            get
            {
                return _username;
            }
        }
        public string password
        {
            get
            {
                return _password;
            }
        }
        public int fxtcompanyid
        {
            get
            {
                return _fxtcompanyid;
            }
        }
        public int subcompanyid
        {
            get
            {
                return _subcompanyid;
            }
            set
            {
                _subcompanyid = value;
            }
        }
    }
    [Serializable]
    public class Apps
    {
        public string appid { get; set; }
        public string apppwd { get; set; }
        public string appurl { get; set; }
        public string appkey { get; set; }
    }

}
