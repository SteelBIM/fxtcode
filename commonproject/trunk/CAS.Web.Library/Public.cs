using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CAS.Common;
using System.Text;
using System.Net;
using CAS.Entity;
using CAS.Entity.DBEntity;
using System.Collections;
using System.IO;
using System.Data;
using System.Configuration;
using System.Threading;
namespace CAS.Web.Library
{
    public class CASWebClient : WebClient 
    {
        //重写，设置超时时间
        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
            request.Timeout = 10000 * 5;
            request.ReadWriteTimeout = 10000 * 5;
            return request;
        }
    }

    public class Public
    {
        //静态文件版本
        public static string StaticVersion = "";

        private static IList<string> _DllPaths =null;
        /// <summary>
        /// 要注册的插件DLL
        /// </summary>
        public static IList<string> DllPaths
        {
            get { return _DllPaths; }
            set { _DllPaths = value; }
        }

        /// <summary>
        /// 设置登录信息 kevin
        /// </summary>
        /// <param name="login"></param>
        public static void SetLoginInfo(LoginUserAndCity user)
        {
            //这里不直接把login赋给LoginInfo，而通过session中转，是因为如果直接赋值，就会变成全局变量永不过期。            
            if (user != null)
            {
                SessionHelper.Add(ConstCommon.UserId, user.userid);
                SessionHelper.Add(ConstCommon.UserName, user.username);
                SessionHelper.Add(ConstCommon.CompanyId, user.fk_companyid);
                SessionHelper.Add(ConstCommon.CompanyName, user.companyname);
                SessionHelper.Add(ConstCommon.DepartmentId, user.fk_departmentid);
                SessionHelper.Add(ConstCommon.DepartmentName, user.departmentname);
                SessionHelper.Add(ConstCommon.USERTOKEN, user.usertoken);
                SessionHelper.Add(ConstCommon.MobilePhone, user.mobilephone);
                SessionHelper.Add(ConstCommon.CityId, user.cityid);
                SessionHelper.Add(ConstCommon.CityName, user.cityname);
                SessionHelper.Add(ConstCommon.ProvinceId, user.provinceid);
                SessionHelper.Add(ConstCommon.ProvinceName, user.provincename);
                SessionHelper.Add<int[]>(ConstCommon.UserRights, user.userrights);
                SessionHelper.Add(ConstCommon.SysTypeCode,user.fk_systypecode);
                SessionHelper.Add(ConstCommon.FxtCompanyId, user.fk_fxt_companyid);
                SessionHelper.Add(ConstCommon.DeptFullName, user.deptfullname);
                SessionHelper.Add(ConstCommon.topdeptid, user.topdeptid);
                SessionHelper.Add(ConstCommon.FDepartmentId, user.fdepartmentid);
                SessionHelper.Add(ConstCommon.SourceIP, user.sourceip);                
                //CompanyFxt.fk_companyid作为运营方ID主要用于前台网站，各工作台还是登录后使用logininfo.fxtcompanyid
                //现在没有运营方ID的概念了，都用fxtcompanyid
                if(CompanyFxt!=null)
                CompanyFxt.fk_companyid = user.fk_fxt_companyid;
            }
            else {
                SessionHelper.DelAll();
            }
        }

        /// <summary>
        /// 用户是否为超级管理员（管理所有城市）
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static bool IsSuperAdmin(string userid) {
            return userid == ConfigurationManager.AppSettings["superadmin"];
        }

        private static LoginInfoEntity loginInfo = null;
        /// <summary>
        /// 登录后公用变量 kevin
        /// </summary>
        public static LoginInfoEntity LoginInfo{
            get {
                loginInfo = new LoginInfoEntity();
                loginInfo.userid = SessionHelper.Get(ConstCommon.UserId);
                if (!string.IsNullOrEmpty(loginInfo.userid))
                {
                    loginInfo.username = SessionHelper.Get(ConstCommon.UserName);
                    loginInfo.companyid = StringHelper.TryGetInt(SessionHelper.Get(ConstCommon.CompanyId));
                    loginInfo.companyname = SessionHelper.Get(ConstCommon.CompanyName);
                    loginInfo.departmentid = StringHelper.TryGetInt(SessionHelper.Get(ConstCommon.DepartmentId));
                    loginInfo.departmentname = SessionHelper.Get(ConstCommon.DepartmentName);
                    loginInfo.usertoken = SessionHelper.Get(ConstCommon.USERTOKEN);
                    loginInfo.mobilephone = SessionHelper.Get(ConstCommon.MobilePhone);
                    loginInfo.cityid = StringHelper.TryGetInt(SessionHelper.Get(ConstCommon.CityId));
                    loginInfo.cityname = SessionHelper.Get(ConstCommon.CityName);
                    loginInfo.provinceid = StringHelper.TryGetInt(SessionHelper.Get(ConstCommon.ProvinceId));
                    loginInfo.provincename = SessionHelper.Get(ConstCommon.ProvinceName);
                    loginInfo.systypecode = StringHelper.TryGetInt(SessionHelper.Get(ConstCommon.SysTypeCode));
                    loginInfo.userrights = SessionHelper.Get<int[]>(ConstCommon.UserRights);
                    loginInfo.fxtcompanyid = StringHelper.TryGetInt(SessionHelper.Get(ConstCommon.FxtCompanyId));
                    loginInfo.deptfullname = SessionHelper.Get(ConstCommon.DeptFullName);
                    loginInfo.topdeptid = StringHelper.TryGetInt(SessionHelper.Get(ConstCommon.topdeptid));
                    loginInfo.fdepartmentid = StringHelper.TryGetInt(SessionHelper.Get(ConstCommon.FDepartmentId));
                    loginInfo.sourceip = SessionHelper.Get(ConstCommon.SourceIP);
                }
                return loginInfo;
            }
            set { loginInfo = value; }
        }

        public static bool HasPrivilege(PrivilegeHelper.PrivilegeNo priv)
        {
            return HasPrivilege(priv, false);
        }
        public static bool HasPrivilege(PrivilegeHelper.PrivilegeNo priv, bool isAutoEnd)
        {
            bool hasRight = false;
            if (null != Public.LoginInfo.userrights) hasRight = Public.LoginInfo.userrights.Contains((int)priv) || Public.LoginInfo.userrights.Contains((int)PrivilegeHelper.PrivilegeNo.SuperAdmin);
            if (!hasRight && isAutoEnd)
            {
                HttpContext.Current.Response.Write("没有权限");
                HttpContext.Current.Response.End();
            }
            return hasRight; 
        }

        /// <summary>
        /// 公司信息，根据域名从companyfxt表中获取。
        /// </summary>
        private static PriviCompanyfxt companyFxt=null;
        public static PriviCompanyfxt CompanyFxt
        {
            get { return companyFxt; }
            set { companyFxt = value; }
        }
        /// <summary>
        /// 当前域名
        /// </summary>
        /// <returns></returns>
        public static string GetHost() { 
            //string host = HttpContext.Current.Request.Url.Host;
            //为了方便，这里采用域名加端口来识别，否则本地调试比较麻烦
            string host = HttpContext.Current.Request.Url.Authority;  
            //80端口为默认，去除。
            if (host.Contains(":80") && host.Split(':')[1]=="80")
            {
                host = host.Split(':')[0];
            }
            return host;
        }
        public static void SetCompanyFxt()
        {
            string json = null;
            string host = GetHost();
            if (string.IsNullOrEmpty(host))
            {
                throw new Exception("Invalid host");
            }
            string args = "&sysurl=" + host + "&type=base";
            string baseinfo = "fxtcompany.privitfxtcompanyfxt";
            try
            {
                json = Public.APIPostBackInCSharp(baseinfo, args);
                JSONHelper.ReturnData data = JSONHelper.JSONToObject<JSONHelper.ReturnData>(json);
                string da = JSONHelper.ObjectToJSON(data.data);
                companyFxt = JSONHelper.JSONToObject<PriviCompanyfxt>(da);
            }
            catch (Exception ex) {
                LogHelper.Error(ex);
            }
        }        
      
        /// <summary>
        /// 虚拟文件对应的路径
        /// </summary>
        public struct DllPath
        {
            public static string Basic = "~/CAS.Web.Controls.dll/";
        }

        /// <summary>
        /// 虚拟控件的常量定义
        /// </summary>
        public struct DllControl
        {
            /// <summary>
            /// 消息控件
            /// </summary>
            public struct UCMessage
            {
                public static string MessageList = "UCMessage.MessageList";
                public static string NewMessage = "UCMessage.NewMessage";
            }
            /// <summary>
            /// 区域城市控件
            /// </summary>
            public struct UCCityArea
            {
                public static string Tree = "UCCityArea.Tree";
            }
            /// <summary>
            /// 公共控件
            /// </summary>
            public struct UCPublic 
            {
                public static string Tree = "UCPublic.Tree";
            }

            /// <summary>
            /// 报告控件
            /// </summary>
            public struct UCReports
            {
                public static string ReportsList = "UCReports.ReportsList";
                public static string BaseInfo = "UCReports.BaseInfo";
                public static string BusinessInfo = "UCReports.BusinessInfo";
                public static string Result = "UCReports.Result";
            }
        }

        public static string LoadControlFromDll(string ctl)
        {
            return LoadControlFromDll(DllPath.Basic, ctl);
        }

        /// <summary>
        /// 加载虚拟文件用户控件，注意与上面定义的dllpath对应
        /// </summary>
        /// <param name="dllpath"></param>
        /// <param name="ctl"></param>
        /// <returns></returns>
        public static string LoadControlFromDll(string dllpath ,string ctl)
        {
            string path =dllpath.Substring(dllpath.IndexOf("/")+1);
            path = path.Substring(0,path.Length-4);
            return dllpath + path  + ctl + ".ascx";
        }


        /// <summary>
        /// 当前站点域名
        /// </summary>
        private static string domain = null;
        public static string Domain
        {
            get
            {
                return domain == null ? WebCommon.GetConfigSetting("domain") : domain;
            }
            set
            {
                domain = value;
            }
        }

        private static int systypecode = 0;
        /// <summary>
        /// 当前产品CODE，调试时因为域名都是localhost的原因，不能从域名读到systypecode
        /// 但发布后就应该从数据库中读取，而不是web.config，这样才能实现一套代码用于多个产品
        /// 比如银行工作台用于法院和国资委
        /// </summary>
        public static int SysTypeCode
        {
            get
            {
                return CompanyFxt.fk_systypecode;
            }
            set
            {
                systypecode = value;
            }
        }


        /// <summary>
        /// 当前产品名称
        /// </summary>
        private static string productname = null;
        public static string ProductName
        {
            get
            {
                return CompanyFxt.websysname;
                //return productname == null ? WebCommon.GetConfigSetting("productname") : productname;
            }
            set
            {
                productname = value;
            }
        }

        private static string msgserverpath = null;
        /// <summary>
        /// 消息服务器
        /// </summary>
        public static string MsgServerPath
        {
            get
            {
                return CompanyFxt.msgserverpath;
            }
            set
            {
                msgserverpath = value;
            }
        }
        
        /// <summary>
        /// 转到错误页
        /// </summary>
        /// <param name="errmsg"></param>
        public static void ResponseWriteError(string errmsg)
        {
            HttpContext.Current.Response.Write(errmsg);
            HttpContext.Current.Response.End();
            return;           
        }

        private static string _RootUrl=null;
        /// <summary>
        /// 当前站点目录，测试了一下，还不好用子目录，有些问题 kevin
        /// </summary>
        public static string RootUrl
        {
            get { return _RootUrl == null ? WebCommon.GetConfigSetting("rooturl") : _RootUrl; }
            set { _RootUrl = value; }
        }

        private static string _RootUrlFull=null;
        //当前站点全路径
        public static string RootUrlFull
        {
            get { return _RootUrlFull == null ? HttpContext.Current.Request.Url.Authority.ToLower() + RootUrl : _RootUrlFull; }
            set { _RootUrlFull = value; }
        }

        private static string apiurl;
        /// <summary>
        /// API的网址，应该从web.config读出，不从数据库读是因为程序启动时如果没有API地址就没有可能读数据库。 kevin
        /// </summary>
        public static string APIUrl
        {
            get
            {
                return string.IsNullOrEmpty(apiurl)? WebCommon.GetConfigSetting("apiurl"):apiurl;
            }
            set 
            {
                apiurl = value;
            }
        }

        private static string staticurl;
        /// <summary>
        /// 静态公共文件的网址，应该从数据库读出
        /// </summary>
        public static string StaticUrl
        {
            get
            {
                return CompanyFxt == null ? "" : StringHelper.GetHttpUrl(CompanyFxt.staticweburl);
            }
            set 
            {
                staticurl = value;
            }
        }


        private static string _SurveyCenterUrl;
        /// <summary>
        /// 调度中心的地址，应该从数据库读出
        /// </summary>
        public static string SurveyCenterUrl
        {
            get
            {
                return CompanyFxt == null ? "" : StringHelper.GetHttpUrl(CompanyFxt.surveycenterurl);
            }
            set
            {
                _SurveyCenterUrl = value;
            }
        }

        /// <summary>
        /// 当前主题模板名称，从数据库读出。
        /// </summary>
        private static string currentTheme = null;
        public static string CurrentTheme
        {
            get
            {
                return CompanyFxt == null ? "" : CompanyFxt.themename;
            }
            set
            {
                currentTheme = value;
            }
        }

        /// <summary>
        /// 当前主题样式名称，登录后根据个人设置来选择。
        /// </summary>
        private static string currentStyle = null;
        public static string CurrentStyle
        {
            get
            {
                return CompanyFxt == null ? "" : CompanyFxt.stylename;
            }
            set
            {
                currentStyle = value;
            }
        }
       

        /// <summary>
        /// 主题模板资源路径
        /// </summary>
        public static string TemplatePath
        {
            get
            {
                return RootUrl + "templates/" + CurrentTheme + "/";
            }
        }

        /// <summary>
        /// 主题模板样式资源路径
        /// </summary>
        public static string TemplateStylePath
        {
            get
            {
                return RootUrl + "templates/" + CurrentTheme + "/css/" + CurrentStyle + "/";
            }
        }

        /// <summary>
        /// 检查时间合法的字符串参数
        /// </summary>
        /// <returns></returns>        
        public static string TimeString(string posts)
        {
            DateTime dt = DateTime.Now;
            //这里改为数组，原来用indexof会引起相似名称的参数冲突 kevin
            string[] tmp = null;
            string tmpstr = posts;
            if (!posts.StartsWith("http://") && !posts.StartsWith("?") && !posts.StartsWith("&")) {
                tmpstr = "&" + posts;
            }
            tmp = tmpstr.Split(new char[] { '?', '&' });
            List<string> tmpkey=new List<string>();
            for (int i = 0; i < tmp.Length; i++)
            {
                if (tmp[i].IndexOf("=") > 0) {
                    tmpkey.Add(tmp[i].Split('=')[0]);
                }
            }
            if (!tmpkey.Contains<string>("strdate"))
            {
                posts += "&strdate=" + HttpUtility.UrlEncode(dt.ToString("yyyy-MM-dd hh:mm:ss"));
            }
            if (!tmpkey.Contains<string>("strcode"))
            {
                posts += "&strcode=" + StringHelper.GetMd5("123" + dt.ToString("yyyy-MM-dd hh:mm:ss") + "321");
            }
            //加上来源，用于api读取相应的配置，从company_fxt中。
            if (!tmpkey.Contains<string>("cas_source"))
                posts += "&cas_source=" + GetHost();
            //加上当前运营方ID
            if (!tmpkey.Contains<string>("fxtcompanyid") && LoginInfo.fxtcompanyid > 0)
                posts += "&fxtcompanyid=" + LoginInfo.fxtcompanyid.ToString();
            //加上当前站点的域名和目录，用于api相应设置。
            posts += "&rooturl=" + HttpUtility.UrlEncode(RootUrl);
            posts += "&rooturlfull=" + HttpUtility.UrlEncode(RootUrlFull);
            //消息服务器
            if (!tmpkey.Contains<string>("msgserver") && companyFxt != null && MsgServerPath!=null)
                posts += "&msgserver=" + MsgServerPath.ToString();
            //加上当前用户ID
            if (!tmpkey.Contains<string>("token") && LoginInfo.usertoken != null)
                posts += "&token=" + loginInfo.usertoken;
            //加上当前IP
            posts += "&sourceip=" + WebCommon.GetIPAddress();
            if (posts.StartsWith("http://") && posts.IndexOf("&")>0 && posts.IndexOf("?") < 0)
            {
                posts = posts.Substring(0, posts.IndexOf("&")) + "?" + posts.Substring(posts.IndexOf("&") + 1); ;
            }
            return posts;
        }

        /// <summary>
        /// 调用API GET数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string APIGet(string url)
        {
            url = TimeString(url);
            CASWebClient client = new CASWebClient();
            client.Encoding = Encoding.UTF8;
            //这里url要组装安全标记等参数
            try
            {
                string result = client.DownloadString(url);
                client.Dispose();
                return result;
            }
            catch (Exception ex)
            {
#if DEBUG
                return ex.Message + "<br>源URL:" + url;
#else
                return "访问页面出错";
#endif
            }
        }

        /// <summary>
        /// 调用API POST数据，并不返回数据，不阻塞当前线程
        /// </summary>
        /// <param name="url"></param>
        /// <param name="posts"></param>
        public static void APIPost(string url, string posts)
        {
            posts = TimeString(posts);
            byte[] postData = Encoding.UTF8.GetBytes(posts);
            CASWebClient client = new CASWebClient();
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            client.Headers.Add("ContentLength", postData.Length.ToString());
            //这里url要组装安全标记等参数
            client.UploadDataAsync(new Uri(url), "POST", postData);
        }

        public static string APIPostBackInCSharp(string url,string posts)
        {
            url = Public.APIUrl + url.Replace(".", "/") + ".ashx";
            return APIPostBack(url, posts,true);
        }
        public static T APIPostBackInCSharp<T>(string url, string posts)
        {
            T t = default(T);
            string jsonResult = Public.APIPostBackInCSharp(url, posts);
            JSONHelper.ReturnData data = JSONHelper.JSONToObject<JSONHelper.ReturnData>(jsonResult);
            if (data.returntype == 1)
            {
                string j = JSONHelper.ObjectToJSON(data.data);
                if (!string.IsNullOrEmpty(j))
                {
                    t = JSONHelper.JSONToObject<T>(j);
                }
            }
            return t;
        }

        /// <summary>
        /// 调用API POST数据，并返回数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="posts"></param>
        /// <returns></returns>
        public static string APIPostBack(string url, string posts,bool check)        
        {
            //检查参数，登录接口不需要
            if(check)
                posts = TimeString(posts);
            byte[] postData = Encoding.UTF8.GetBytes(posts);
            //找退出原因
            //LogHelper.Info(url + posts);
            CASWebClient client = new CASWebClient();
            
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            client.Headers.Add("ContentLength", postData.Length.ToString());
            //这里url要组装安全标记等参数
            byte[] responseData = null;
            string result = "";
            try
            {
                responseData = client.UploadData(url, "POST", postData);
                result =  Encoding.UTF8.GetString(responseData);
                //找退出原因
                //LogHelper.Info(result);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                result = JSONHelper.GetJson(null,0,ex.Message,ex);
            }
            client.Dispose();
            return result;
        }

        public static string GetRequest(string key)
        {
            return GetRequest(key, string.Empty);
        }

        /// <summary>
        /// 取request参数，注意不能用request[key]，因为这样会包含cookie和servervariables
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defVal"></param>
        /// <returns></returns>
        public static string GetRequest(string key, string defVal)
        {
            HttpRequest Request = HttpContext.Current.Request;
            if (Request.QueryString[key] != null)
                return Request.QueryString[key];
            if (Request.Form[key] != null)
                return Request.Form[key];
            return defVal;
        }

        /// <summary>
        /// 取code列表
        /// </summary>
        /// <param name="codeid"></param>
        /// <returns></returns>
        public static List<SYSCode> GetCodes(int codeid)
        {
            if (AllCodes == null) return null;
            return AllCodes.Where(t => t.id == codeid).ToList<SYSCode>();
        }
        public static List<SYSCode> GetCodes(int codeid, int subcode)
        {
            if (AllCodes == null) return null;
            return AllCodes.Where(t => t.id == codeid && t.subcode == subcode).ToList<SYSCode>();
        }

        public static List<SYSCode> AllCodes {
            get
            {
                string url = "code.codelist";
                List<SYSCode> list = CacheHelper.Get<List<SYSCode>>(url);
                if (list != null) return list;

                string result;
                string str = "type=all";
                result = Public.APIPostBackInCSharp(url, str);
                JSONHelper.ReturnData data = JSONHelper.JSONToObject<JSONHelper.ReturnData>(result);
                if (data.returntype == 1)
                {
                    string x = JSONHelper.ObjectToJSON(data.data);
                    list = JSONHelper.JSONToObject<List<SYSCode>>(x);
                    CacheHelper.Set<List<SYSCode>>(url, list);
                    return list;
                }
                else
                    return null;
            }
        }

        public static DataTable GetDatePoint(int cityid,int companyid,int systypecode,int fxtcompanyid) 
        {
            string result;
            DataTable dt = null;
            string url = "tax.datepointlist";
            string str = "cityid=" + cityid.ToString() + "&systypecode=" +systypecode.ToString() + "&companyid=" + companyid.ToString();
            result = Public.APIPostBackInCSharp(url, str);
            JSONHelper.ReturnData data = JSONHelper.JSONToObject<JSONHelper.ReturnData>(result);
            if (data.returntype == 1)
            {
                string x = JSONHelper.ObjectToJSON(data.data);
                dt = JSONHelper.JSONToObject<DataTable>(x);
            }
            return dt;
        }

        /// <summary>
        /// 取user列表
        /// </summary>
        /// <param name="codeid"></param>
        /// <returns></returns>
        public static List<PriviUser> GetUser(int fxtcompanyid, int cityid, int systypecode, int companyId, int departmentId, int postid)
        {
            string result;
            string url = "user.userddr";
            string str = "fxtcompanyid=" + fxtcompanyid.ToString() + "&cityid=" + cityid.ToString() + "&systypecode=" + systypecode + "&did=" + departmentId + "&cid=" + companyId + "&postid="+postid;
            result = Public.APIPostBackInCSharp(url, str);
            JSONHelper.ReturnData data = JSONHelper.JSONToObject<JSONHelper.ReturnData>(result);
            if (data.returntype == 1)
            {
                string x = JSONHelper.ObjectToJSON(data.data);
                List<PriviUser> list = JSONHelper.JSONToObject<List<PriviUser>>(x);
                return list;
            }
            else
                return null;
        }

        /// <summary>
        /// 取company列表
        /// </summary>
        /// <param name="codeid"></param>
        /// <returns></returns>
        public static List<PriviCompany> GetCompany(int fxtcompanyid,int cityid,int systypecode)
        {
            string result;
            string url = "survey.HCompany";
            string str = "fxtcompanyid=" + fxtcompanyid.ToString() + "&cityid=" + cityid.ToString() + "&systypecode=" + systypecode;
            result = Public.APIPostBackInCSharp(url, str);
            JSONHelper.ReturnData data = JSONHelper.JSONToObject<JSONHelper.ReturnData>(result);
            if (data.returntype == 1)
            {
                string x = JSONHelper.ObjectToJSON(data.data);
                List<PriviCompany> list = JSONHelper.JSONToObject<List<PriviCompany>>(x);
                return list;
            }
            else
                return null;
        }
        

        /// <summary>
        /// 取company列表
        /// </summary>
        /// <param name="codeid"></param>
        /// <returns></returns>
        public static List<DATQueryFiles> GetQueryFilesList(int fxtcompanyid, int cityid, int systypecode,int surveyid,string token)
        {
            string result;
            string url = "survey.queryfiles";
            string str = "fxtcompanyid=" + fxtcompanyid.ToString() + "&cityid=" + cityid.ToString() + "&systypecode=" + systypecode + "&type=6051001" + "&surveyid=" + surveyid + "&token=" + token;
            result = Public.APIPostBackInCSharp(url, str);
            JSONHelper.ReturnData data = JSONHelper.JSONToObject<JSONHelper.ReturnData>(result);
            if (data.returntype == 1)
            {
                string x = JSONHelper.ObjectToJSON(data.data);
                List<DATQueryFiles> list = JSONHelper.JSONToObject<List<DATQueryFiles>>(x);
                return list;
            }
            else
                return null;
        }


        /// <summary>
        /// 取code列表并拼接成select
        /// </summary>
        /// <param name="codeid"></param>
        /// <param name="defVal"></param>
        /// <returns></returns>
        public static string GetDropdownArea(int cityid, int defVal)
        {
            List<SYSArea> list = GetArea(cityid);
            string result = "";
            if (null != list)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    result += string.Format("<option value='{0}' {1}/>{2}</option>"
                        , list[i].areaid, list[i].areaid == defVal ? "checked" : "", list[i].areaname
                        );
                }
            }
            return result;
        }

        public static string GetAllBranch(int companyid,int departmentid,int defaultVal) 
        {
            List<Dictionary<string, object>> dt = new List<Dictionary<string, object>>();
            
            string result;
            string url = "company.companylist";
            string str = "companyId=" + companyid + "&departmentid=" + departmentid + "&type=allbranch";
            result = Public.APIPostBackInCSharp(url, str);
            JSONHelper.ReturnData data = JSONHelper.JSONToObject<JSONHelper.ReturnData>(result);
            if (data.returntype == 1)
            {
                string x = JSONHelper.ObjectToJSON(data.data);
                dt = JSONHelper.JSONToObject<List<Dictionary<string, object>>>(x);
            }
            if (dt.Count>0)
            {
                return GetDetailsName(dt[0]["Fk_parentId"].ToString(), dt, defaultVal,1,new ArrayList());
            }
            else
            {
                return "";
            }
            
        }

        public static string GetAllBranchIgnoreCity(int companyid, int departmentid, int defaultVal)
        {
            List<Dictionary<string, object>> dt = new List<Dictionary<string, object>>();

            string result;
            string url = "company.companylist";
            string str = "companyId=" + companyid + "&departmentid=" + departmentid + "&type=allbranchignorecity";
            result = Public.APIPostBackInCSharp(url, str);
            JSONHelper.ReturnData data = JSONHelper.JSONToObject<JSONHelper.ReturnData>(result);
            if (data.returntype == 1)
            {
                string x = JSONHelper.ObjectToJSON(data.data);
                dt = JSONHelper.JSONToObject<List<Dictionary<string, object>>>(x);
            }
            if (dt.Count > 0)
            {
                return GetDetailsName(dt[0]["Fk_parentId"].ToString(), dt, defaultVal, 1, new ArrayList());
            }
            else
            {
                return "";
            }

        }

        /// <summary>
        /// 递归加载部门
        /// </summary>
        /// <param name="parentid">默认加载项</param>
        /// <param name="ds">递归集合</param>
        /// <returns></returns>
        private static string GetDetailsName(string parentid, List<Dictionary<string, object>> ds, int selected, int level, ArrayList fullname)
        {

            string str1 = "";
            string str2 = "";
            int str3 = 0;
            string str4 = "";
            string nb = "";
            string temp = "";
            // DataRow[] drs = ds.Tables[0].Select("fk_parentid=" + parentid);
            if (ds.Count == 0) return "";
            //int i = 0;
            foreach (Dictionary<string, object> dr2 in ds)
            {
                if (dr2["Fk_parentId"].ToString() == parentid)
                {
                    string deptname = dr2["departmentName"].ToString();
                    int lv = Convert.ToInt32(dr2["Lv"]);
                    if (fullname.Count < lv) fullname.Add(deptname);
                    else fullname[lv - 1] = deptname;
                    string tmpfullname = "";
                    for (int i = 0; i < lv; i++)
                    {
                        nb += "&nbsp;&nbsp;";
                        tmpfullname += fullname[i];
                    }

                    str1 = nb + deptname;
                    str2 = tmpfullname;
                    str3 =Convert.ToInt32(dr2["departmentId"]);
                    if ( str3 == selected)
                    {
                        str4 = string.Format("<option value='{0}' rel='{1}' selected = 'selected'>{2}</option>", str3, str2, str1);
                    }
                    else
                    {
                        str4 = string.Format("<option value='{0}' rel='{1}'>{2}</option>", str3, str2, str1);
                    }

                    nb = "";

                    temp += str4;
                    temp += GetDetailsName(dr2["departmentId"].ToString(), ds, selected, lv,fullname);
                }

            }
            return temp;
        }


        /// <summary>
        /// 递归加载部门
        /// </summary>
        /// <param name="parentid"></param>
        /// <param name="dt"></param>
        /// <param name="selected"></param>
        /// <param name="level"></param>
        /// <param name="fullname"></param>
        /// <returns></returns>
        public static string GetDetailsName(string parentid, DataTable dt, int selected, int level, ArrayList fullname)
        {
            string str1 = "";
            string str2 = "";
            int str3 = 0;
            string str4 = "";
            string nb = "";
            string temp = "";          
            if (dt.Rows.Count == 0) return "";
            foreach (DataRow  dr2 in dt.Rows){            
                if (dr2["Fk_parentId"].ToString() == parentid)
                {
                    string deptname = dr2["departmentName"].ToString();
                    int lv = Convert.ToInt32(dr2["Lv"]);
                    if (fullname.Count < lv) fullname.Add(deptname);
                    else fullname[lv - 1] = deptname;
                    string tmpfullname = "";
                    for (int i = 0; i < lv; i++)
                    {
                        nb += "&nbsp;&nbsp;";
                        tmpfullname += fullname[i];
                    }
                    str1 = nb + deptname;
                    str2 = tmpfullname;
                    str3 = Convert.ToInt32(dr2["departmentId"]);
                    if (str3 == selected)
                    {
                        str4 = string.Format("<option value='{0}' rel='{1}' selected = 'selected'>{2}</option>", str3, str2, str1);
                    }
                    else
                    {
                        str4 = string.Format("<option value='{0}' rel='{1}'>{2}</option>", str3, str2, str1);
                    }

                    nb = "";

                    temp += str4;
                    temp += GetDetailsName(dr2["departmentId"].ToString(), dt, selected, lv, fullname);
                }

            }
            return temp;
        }


        /// <summary>
        /// 获取区域
        /// </summary>
        /// <param name="cityid"></param>
        /// <returns></returns>
        private static List<SYSArea> GetArea(int cityid) 
        {
            string result;
            string url = "cityarea.arealist";
            string str = "type=tree&cityid=" + cityid.ToString();
            result = Public.APIPostBackInCSharp(url, str);
            JSONHelper.ReturnData data = JSONHelper.JSONToObject<JSONHelper.ReturnData>(result);
            if (data.returntype == 1)
            {
                string x = JSONHelper.ObjectToJSON(data.data);
                List<SYSArea> list = JSONHelper.JSONToObject<List<SYSArea>>(x);
                return list;
            }
            else
                return null;
        }

        /// <summary>
        /// 取code列表并拼接成radiolist
        /// </summary>
        /// <param name="codeid"></param>
        /// <returns></returns>
        public static string GetRadioCodes(int codeid,int defVal)
        {
            List<SYSCode> list = GetCodes(codeid);
            string result = "";
            string name = WebCommon.GetRndString(6) + codeid.ToString();
            for (int i = 0; i < list.Count; i++)
            {                
                string chk = defVal == 0 && i == 0 ? "checked='checked'" : ( list[i].code == defVal ? "checked='checked'" : "");
                result += string.Format("<input class='dn' type='radio' value='{0}' name='{1}' {2}/><label class='dn'>{3}</label>"
                    ,list[i].code,name,chk,list[i].codename
                    );
            }
            return result;
        }

        /// <summary>
        /// 取code列表并拼接成checkboxlist
        /// </summary>
        /// <param name="codeid"></param>
        /// <param name="defVal"></param>
        /// <returns></returns>
        public static string GetCheckboxCodes(int codeid, int defVal)
        {
            List<SYSCode> list = GetCodes(codeid);
            string result = "";
            for (int i = 0; i < list.Count; i++)
            {
                string chk = list[i].code == defVal ? "checked='checked'" : "";
                result += string.Format("<input  type='checkbox' value='{0}' {1}/><label >{2}</label>"
                    , list[i].code, chk, list[i].codename
                    );
            }
            return result;
        }

       /// <summary>
       /// 获取人员(查勘员传9，业务员不传)
       /// </summary>
       /// <param name="cityid"></param>
       /// <param name="fxtcompanyid"></param>
       /// <param name="systypecode"></param>
       /// <param name="companyId"></param>
       /// <param name="departmentId"></param>
       /// <param name="postid"></param>
       /// <returns></returns>
        public static string GetDropdownUser(int cityid, int fxtcompanyid, int systypecode, int companyId, int departmentId,int postid,int isAll)
        {
            List<PriviUser> list = GetUser(fxtcompanyid, cityid, systypecode, companyId, departmentId, postid);
            string result = "";
            if (list.Count > 0)
            {
                if (isAll == 1)
                {
                    result += "<option value='' checked>全部</option>";
                }
                for (int i = 0; i < list.Count; i++)
                {
                    result += string.Format("<option value='{0}'>{1}</option>"
                           , list[i].userid, list[i].username
                           );
                }
            }
            return result;
        }

        /// <summary>
        /// 获取人员
        /// </summary>
        /// <param name="cityid"></param>
        /// <returns></returns>
        public static string GetCompanyDDR(int cityid, int fxtcompanyid, int systypecode)
        {
            List<PriviCompany> list = GetCompany(fxtcompanyid, cityid, systypecode);
            string result = "";
            for (int i = 0; i < list.Count; i++)
            {
                result += "<option value=''  checked>全部</option>";
                result += string.Format("<option value='{0}'>{1}</option>"
                       , list[i].companyid, list[i].companyname
                       );
            }
            return result;
        }

        /// <summary>
        /// 取code列表并拼接成select
        /// </summary>
        /// <param name="codeid"></param>
        /// <param name="defVal"></param>
        /// <returns></returns>
        public static string GetDropdownCodes(int codeid, int defVal)
        {           
            List<SYSCode> list = GetCodes(codeid);
            string result = "";
            if (null != list)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    result += string.Format("<option value='{0}' {1}>{2}</option>"
                        , list[i].code, list[i].code == defVal ? "checked" : "", list[i].codename
                        );
                }
            }
            return result;
        }
        public static string GetDropdownCodes(int codeid, int subcode, int defVal)
        {
            List<SYSCode> list = GetCodes(codeid, subcode);
            string result = "";
            if (null != list)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    result += string.Format("<option value='{0}' {1}>{2}</option>"
                        , list[i].code, list[i].code == defVal ? "checked" : "", list[i].codename
                        );
                }
            }
            return result;
        }


      
        /// <summary>
        /// 取code列表并拼接成select,排除不要的code
        /// </summary>
        /// <param name="codeid"></param>
        /// <param name="defVal"></param>
        /// <returns></returns>
        public static string GetDropdownCodes(int codeid, int defVal,string code)
        {
            List<SYSCode> list = GetCodes(codeid);
            string result = "";
            if (null != list)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].code.ToString().IndexOf(code)!=-1) continue;
                    result += string.Format("<option value='{0}' {1}>{2}</option>"
                        , list[i].code, list[i].code == defVal ? "checked" : "", list[i].codename
                        );
                }
            }
            return result;
        }
        
        public delegate string[] GetOptionParams<T>(T t);
        /// <summary>
        /// 生成下拉选项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="defaultValue"></param>
        /// <param name="getParams"></param>
        /// <returns></returns>
        public static string GetDropDownOptions<T>(List<T> list, string defaultValue, GetOptionParams<T> getParams)
        {
            string result = string.Empty;
            if (null != list && list.Count > 0)
            {
                StringBuilder sbOptions = new StringBuilder(list.Count);
                string[] oParams = null;
                foreach(T t in list)
                {
                    oParams = getParams(t);
                    sbOptions.Append(string.Format("<option value='{0}' {1}>{2}</option>", new string[] { oParams[0], defaultValue == oParams[0] ? "checked" : "", oParams[1] }));
                }
                result = sbOptions.ToString();
            }
            return result;
        }
        public static string GetDropDownOptions<T>(string url, string postData, string defaultValue, GetOptionParams<T> getParams)
        {
            string result = string.Empty;
            string jsonResult = Public.APIPostBackInCSharp(url, postData);
            JSONHelper.ReturnData data = JSONHelper.JSONToObject<JSONHelper.ReturnData>(jsonResult);
            if (data.returntype == 1)
            {
                string j = JSONHelper.ObjectToJSON(data.data);
                List<T> list = JSONHelper.JSONToObject<List<T>>(j);
                result = Public.GetDropDownOptions<T>(list, defaultValue, getParams);
            }
            return result;
        }
        /// <summary>
        /// 每个产品里可以管理其他产品的列表
        /// </summary>
        /// <param name="currentSysTypeCode"></param>
        /// <returns></returns>
        public static int[] GetManageSysTypeCode(int currentSysTypeCode)
        {
            int[] manageSysTypeCode = null;
            switch (currentSysTypeCode)
            {
                case 1003005://调度中心
                    manageSysTypeCode = new int[] { 1003005, 1003008 };
                    break;
                case 1003008://云查勘企业版
                    manageSysTypeCode = new int[] { 0 };
                    break;
                case 1003011://管理工作台
                    manageSysTypeCode = new int[] { 1003011 };
                    break;
                case 1003012://评估机构工作台
                    manageSysTypeCode = new int[] { 1003005, 1003008, 1003012 };
                    break;
                case 1003013://银行工作台
                    manageSysTypeCode = new int[] { 1003013 };
                    break;
                case 1003014://协会工作台
                    manageSysTypeCode = new int[] { 1003014 };
                    break;
                case 1003015://国资委工作台
                    manageSysTypeCode = new int[] { 1003015 };
                    break;
                case 1003016://法院工作台
                    manageSysTypeCode = new int[] { 1003016 };
                    break;
                default:
                    manageSysTypeCode = new int[] { 0 };
                    break;
            }
            return manageSysTypeCode;
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        public static string GetMenus()
        {
            //从数据库取各工作台的菜单来显示
            Link[] links = new Link[5];
            string menu = "";
            for (int i = 0; i < links.Length; i++)
            {
                links[i] = new Link();
                links[i].Index = i;
                links[i].ID = "Link" + i.ToString();
                links[i].Href = "myspace/report/default.aspx";
                links[i].Name = "Link" + i.ToString();
                links[i].Caption = "模块" + i.ToString();
                //links[i].Target = "_self";
                links[i].Title = "这里是模块" + i.ToString() + "的按钮";
                links[i].Class = "";

                menu += string.Format("<li{0}><a href=\"{1}\"{2}{3}{4}{5}>{6}</a></li>"
                    , links[i].IsSelected ? " class=\"selected\"" : ""
                    , links[i].Href
                    , links[i].Target == null ? "" : " target=\"" + links[i].Target + "\""
                    , links[i].Title == null ? "" : " title=\"" + links[i].Title + "\""
                    , links[i].Class == null ? "" : " class=\"" + links[i].Class + "\""
                    , links[i].Name == null ? "" : " id=\"" + links[i].Name + "\""
                    , links[i].Caption
                    );
            }
            return menu;
        }

        /// <summary>
        /// 母版页类型
        /// </summary>
        public struct MasterType
        {
            public static string DemoFirst = "demofirst";
            /// <summary>
            /// 顶级母版页
            /// </summary>
            public static string First = "first";
            /// <summary>
            /// 次级母版页
            /// </summary>
            public static string Second = "second";
            /// <summary>
            /// 三级母版页
            /// </summary>
            public static string Third = "third";

        }

        /// <summary>
        /// 是否需要重新登录 kevin
        /// </summary>
        public static bool NeedReLogin = false;
    }
}