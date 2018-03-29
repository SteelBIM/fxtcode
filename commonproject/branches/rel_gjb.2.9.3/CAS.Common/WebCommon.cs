using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Collections;
using System.Net;
using System.Threading;
using CAS.Entity;
using System.Reflection;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System.IO;
using System.Drawing.Imaging;
using CAS.Entity.SurveyEntityNew;
using Newtonsoft.Json.Linq;
using System.Drawing.Drawing2D;

namespace CAS.Common
{
    public class WebCommon
    {
        /// <summary>
        /// 数据中心API kevin
        /// </summary>
        public static string FxtDataCenterService
        {
            get
            {
                return GetConfigSetting("fxtdatacenterservice");
            }
        }
        /// <summary>
        /// 用户中心API kevin
        /// </summary>
        public static string FxtUserCenterService
        {
            get
            {
                return GetConfigSetting("fxtusercenterservice");
            }
        }

        public static string FxtUserCenterLoginService
        {
            get
            {
                return GetConfigSetting("fxtusercenterloginservice");
            }
        }

        /// <summary>
        /// 用户中心WCFAPI Hody
        /// </summary>
        public static string WCFUserCenterService
        {
            get
            {
                return WebCommon.GetConfigSetting("wcfusercenterservice");
            }
        }

        /// <summary>
        /// 用户中心WCFAPI登录 Hody
        /// </summary>
        public static string WCFUserCenterServiceLogin
        {
            get
            {
                return WebCommon.GetConfigSetting("wcfusercenterloginservice");
            }
        }
        /// <summary>
        /// 数据中心WCFAPI Hody
        /// </summary>
        public static string WCFDataCenterService
        {
            get
            {
                return WebCommon.GetConfigSetting("wcfdatacenterservice");
            }
        }

        /// <summary>
        /// 查勘中心API caoq
        /// </summary>
        public static string FxtSurveyCenterService
        {
            get
            {
                return GetConfigSetting("fxtsurveycenterservice");
            }
        }

        /// <summary>
        /// 产品Code Hody
        /// </summary>
        public static string FxtSysTypeCode
        {
            get
            {
                return GetConfigSetting("systypecode");
            }
        }

        /// <summary>
        /// 根据公司id，产品code获取web地址
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="systypecode"></param>
        /// <returns></returns>
        public static string GetWebUrlByCompanyId(int companyid, int systypecode)
        {
            string msg = null;
            UserCheck user = WebCommon.FxtUserCenterService_GetCompanyProductByParam(companyid, null, systypecode, out msg);
            if (user != null)
            {
                msg = user.weburl;
            }
            return msg;
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
            if (!posts.StartsWith("http://") && !posts.StartsWith("?") && !posts.StartsWith("&"))
            {
                tmpstr = "&" + posts;
            }
            tmp = tmpstr.Split(new char[] { '?', '&' });
            List<string> tmpkey = new List<string>();
            for (int i = 0; i < tmp.Length; i++)
            {
                if (tmp[i].IndexOf("=") > 0)
                {
                    tmpkey.Add(tmp[i].Split('=')[0]);
                }
            }
            if (!tmpkey.Contains<string>("strdate"))
            {
                posts += "&strdate=" + HttpUtility.UrlEncode(dt.ToString());
            }
            if (!tmpkey.Contains<string>("strcode"))
            {
                posts += "&strcode=" + StringHelper.GetMd5("123" + dt.ToString() + "321");
            }
            //加上当前IP
            posts += "&sourceip=" + WebCommon.GetIPAddress();
            if (posts.StartsWith("http://") && posts.IndexOf("&") > 0 && posts.IndexOf("?") < 0)
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
            WebClient client = new WebClient();
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
        /// 以GET方式创建Request请求,返回字符串(只适合返回文本类型,图片文件流除外)
        /// 潘锦发 20160316
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns></returns>
        public static string APIGetByRequest(string url)
        {
            string result = "";
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url); //创建请求
            request.Method = "GET";
            using (HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse())//组建请求体
            {
                string fullResponse = "";
                using (Stream data = myResponse.GetResponseStream()) //获取返回的数据流
                {
                    using (StreamReader responseReader = new StreamReader(data))
                    {
                        fullResponse = responseReader.ReadToEnd();
                    }
                }
                if (myResponse.StatusCode == HttpStatusCode.OK) //判断状态
                {
                    result = fullResponse;
                }
                else
                {
                    result = "";
                }
            }
            return result;
        }

        /// <summary>
        /// 调用API POST数据，并不返回数据，不阻塞当前线程
        /// </summary>
        /// <param name="url"></param>
        /// <param name="posts"></param>
        public static void APIPost(string url, string posts, bool check)
        {
            APIPost(url, posts, check,null);
        }
        /// <summary>
        /// 调用API POST数据，并不返回数据，不阻塞当前线程
        /// </summary>
        /// <param name="url"></param>
        /// <param name="posts"></param>
        public static void APIPostByJson(string url, string posts, bool check)
        {
            APIPost(url, posts, check, "application/json");
        }
        /// <summary>
        /// 调用API POST数据，并不返回数据，不阻塞当前线程
        /// </summary>
        /// <param name="url"></param>
        /// <param name="posts"></param>
        public static void APIPost(string url, string posts, bool check, string contentType)
        {
            contentType = !string.IsNullOrEmpty(contentType) ? contentType : "application/x-www-form-urlencoded";
            if (check)
                posts = TimeString(posts);
            byte[] postData = Encoding.UTF8.GetBytes(posts);
            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", contentType);
            client.Headers.Add("ContentLength", postData.Length.ToString());
            //这里url要组装安全标记等参数
            client.UploadDataAsync(new Uri(url), "POST", postData);
        }
        /// <summary>
        /// 调用API POST数据，并不返回数据，不阻塞当前线程
        /// </summary>
        /// <param name="url"></param>
        /// <param name="posts"></param>
        public static void APIPost(string url, string posts)
        {
            APIPost(url, posts, true);
        }

        /// <summary>
        /// 调用API POST数据，并返回数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="posts"></param>
        /// <returns></returns>
        public static string APIPostBack(string url, string posts, bool check)
        {
            return APIPostBack(url, posts, check, "application/x-www-form-urlencoded");
        }


        public static string APIPostBack(string url, string posts, bool check, string contentType)
        {
            //检查参数，登录接口不需要
            if (check)
                posts = TimeString(posts);
            byte[] postData = Encoding.UTF8.GetBytes(posts);
            //找退出原因
            //LogHelper.Info(url + posts);
            WebClient client = new WebClient();

            client.Headers.Add("Content-Type", contentType);
            client.Headers.Add("ContentLength", postData.Length.ToString());
            //这里url要组装安全标记等参数
            byte[] responseData = null;
            string result = "";
            try
            {
                responseData = client.UploadData(url, "POST", postData);
                result = Encoding.UTF8.GetString(responseData);
                //找退出原因
                //LogHelper.Info(result);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, message: "错误url:" + (url ?? "null"));
                result = JSONHelper.GetJson(null, 0, ex.Message + ",错误url:" + (url ?? "null"), ex);
            }
            client.Dispose();
            return result;
        }
        /// <summary>
        /// 中心服务器检查用户的接口 kevin 2013-4-2
        /// </summary>
        /// <param name="username"></param>
        /// <param name="systypecode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static UserCheck FxtUserCenterService_GetUser(string username, int systypecode, out Exception msg, string password, ref List<Apps> apps)
        {
            //web.config设置中心用户API的地址
            string api = FxtUserCenterLoginService;
            UserCheck usercheck = null;
            msg = null;
            try
            {

                if (!string.IsNullOrEmpty(api))
                {
                    string time = DateTime.Now.ToString("yyyyMMddHHmmss");
                    var sinfo = new
                    {
                        time = time,
                        code = StringHelper.GetLoginCodeMd5(time)
                    };
                    var info = new
                    {
                        uinfo = new { username = username, password = StringHelper.GetPassWordMd5(password) },
                        appinfo = new CAS.Entity.ApplicationInfo(systypecode.ToString())
                    };
                    string post = "{\"sinfo\":\"" + JSONHelper.ObjectToJSON(sinfo).Replace("\"", "'") + "\",\"info\":\"" + JSONHelper.ObjectToJSON(info).Replace("\"", "'") + "\"}";
                    string str = APIPostBack(api, post, false, "application/json");
                    JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                    if (rtn.returntype > 0)
                    {
                        usercheck = new UserCheck();
                        var returnSInfo = JSONHelper.JSONToObject<dynamic>(rtn.data.ToString())["sinfo"];
                        //string appId = Convert.ToString(returnSInfo["appid"]);
                        //string appPwd = Convert.ToString(returnSInfo["apppwd"]);
                        //string appUrl = Convert.ToString(returnSInfo["appurl"]);
                        //string appKey = Convert.ToString(returnSInfo["appkey"]);
                        string appsString = JSONHelper.ObjectToJSON(returnSInfo["apps"]);
                        apps = JSONHelper.JSONToObject<List<CAS.Entity.Apps>>(appsString);
                        string signName = Convert.ToString(returnSInfo["signname"]);
                        string businessDB = Convert.ToString(returnSInfo["businessdb"]);
                        int producttypecode = Convert.ToInt32(returnSInfo["producttypecode"]); //添加产品类型 潘锦发  20151101
                        var returnUInfo = JSONHelper.JSONToObject<dynamic>(rtn.data.ToString())["uinfo"];
                        string trueName = Convert.ToString(returnUInfo["truename"]);
                        string companyname = Convert.ToString(returnUInfo["companyname"]);
                        int fxtcompanyid = Convert.ToInt32(returnUInfo["fxtcompanyid"]);
                        int maxaccountnumber = Convert.ToInt32(returnUInfo["maxaccountnumber"]);

                        usercheck.signname = signName;
                        usercheck.truename = trueName;
                        usercheck.companyid = fxtcompanyid;
                        usercheck.businessdb = businessDB;
                        usercheck.producttypecode = producttypecode;
                        usercheck.companyname = companyname;
                        usercheck.maxaccountnumber = maxaccountnumber;
                        usercheck.status = Convert.ToInt32(returnUInfo["status"]);
                        usercheck.returntext = returnUInfo["returntext"];
                        usercheck.editpwdurl = returnUInfo["editpwdurl"];

                        return usercheck;
                    }
                    else
                    {
                        ////检测到账号对应密码存在风险，修改跳转到业务修改密码
                        //if (rtn.returntype == (int)EnumHelper.Status.SimplePassWord)
                        //{
                        //    usercheck = new UserCheck();
                        //    usercheck.status = rtn.returntype;
                        //    usercheck.editpwdurl = (rtn.data == null) ? "" : JSONHelper.JSONToObject<string>(rtn.data.ToString());
                        //}
                        msg = new Exception(rtn.returntext.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex;
            }
            return usercheck;
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        public static int UpdateFxtUserCenter(string username, string password, LoginInfoEntity loginInfo, string oldPassword)
        {
            //int result = 0;

            SurveyApi sa = new SurveyApi(loginInfo, ((int)EnumHelper.Codes.SysTypeCodeGJB).ToString(), "1003105");
            sa.sinfo.functionname = "usereight";
            sa.info.funinfo = new
            {
                username = username,
                password = password,
                oldpassword = oldPassword
            };
            string str = APIPostBack(sa.UrlApi, sa.GetJsonString(), false, "application/json");
            JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
            //if (rtn.returntype > 0)
            //{
            //    result = rtn.returntype;
            //}

            return rtn.returntype;
        }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        public static int UpdateFxtUserCenter(CAS.Entity.GJBEntity.SYS_User model, LoginInfoEntity loginInfo, string oldPassword)
        {
            int result = 0;
            SurveyApi sa = new SurveyApi(loginInfo, ((int)EnumHelper.Codes.SysTypeCodeGJB).ToString(), "1003105");
            sa.sinfo.functionname = "userfive";
            if (model.userpwd == "" || model.userpwd == null)
            {
                sa.info.funinfo = new
                {
                    companyid = loginInfo.fxtcompanyid,
                    username = model.username,
                    emailstr = model.emailstr,
                    mobile = model.mobile,
                    wxopenid = model.wxopenid,
                    truename = model.truename,
                    password = oldPassword,
                    oldpassword = oldPassword
                };
            }
            else
            {
                model.userpwd = EncryptHelper.TextToPassword(model.userpwd);//加密
                sa.info.funinfo = new
                {
                    companyid = loginInfo.fxtcompanyid,
                    username = model.username,
                    emailstr = model.emailstr,
                    mobile = model.mobile,
                    wxopenid = model.wxopenid,
                    truename = model.truename,
                    password = model.userpwd,
                    oldpassword = oldPassword
                };
            }

            string str = APIPostBack(sa.UrlApi, sa.GetJsonString(), false, "application/json");
            JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
            if (rtn.returntype > 0)
            {
                result = rtn.returntype;
            }

            return result;
        }

        /// <summary>
        /// 新增中心用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static int AddFxtUserCenter(CAS.Entity.GJBEntity.SYS_User model, LoginInfoEntity loginInfo)
        {
            int result = 0;
            SurveyApi sa = new SurveyApi(loginInfo, ((int)EnumHelper.Codes.SysTypeCodeGJB).ToString(), "1003105");
            sa.sinfo.functionname = "userfour";
            sa.info.funinfo = new
                {
                    companyid = loginInfo.fxtcompanyid,
                    username = model.username,
                    password = model.userpwd,
                    emailstr = model.emailstr,
                    mobile = model.mobile,
                    wxopenid = model.wxopenid,
                    truename = model.truename,
                    isinner = 1
                };
            string str = APIPostBack(sa.UrlApi, sa.GetJsonString(), false, "application/json");
            JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
            if (rtn.returntype > 0)
            {
                result = rtn.returntype;
            }

            return result;
        }

        /// <summary>
        /// 删除中心数据库用户
        /// </summary>
        /// <param name="username"></param>
        /// <param name="companyid"></param>
        /// <returns></returns>
        public static bool DeleteFxtUserCenter(string usernames, int companyid, LoginInfoEntity loginInfo)
        {
            return DeleteFxtUserCenter(usernames, companyid, loginInfo, 0);
        }

        /// <summary>
        /// 删除中心数据库用户
        /// </summary>
        /// <param name="username"></param>
        /// <param name="companyid"></param>
        /// <returns></returns>
        public static bool DeleteFxtUserCenter(string usernames, int companyid, LoginInfoEntity loginInfo, int valid)
        {
            bool result = false;
            SurveyApi sa = new SurveyApi(loginInfo, ((int)EnumHelper.Codes.SysTypeCodeGJB).ToString(), "1003105");
            sa.sinfo.functionname = "usersix";
            sa.info.funinfo = new
            {
                username = usernames,
                companyid = companyid,
                valid = valid
            };
            string str = APIPostBack(sa.UrlApi, sa.GetJsonString(), false, "application/json");
            JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
            if (rtn.returntype > 0)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        public static int[] FxtUserCenterService_GetProductList(int companyId, LoginInfoEntity loginInfo)
        {
            int[] result = null;
            SurveyApi sa = new SurveyApi(loginInfo, ((int)EnumHelper.Codes.SysTypeCodeGJB).ToString(), "1003105");
            sa.sinfo.functionname = "cptwo";
            sa.info.funinfo = new
            {
                companyid = companyId
            };
            string str = APIPostBack(sa.UrlApi, sa.GetJsonString(), false, "application/json");
            JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
            if (rtn.returntype > 0)
            {
                result = JSONHelper.JSONToObject<int[]>(rtn.data.ToString());
            }
            return result;
        }

        /// <summary>
        /// 根据公司code获取公司信息
        /// 调用新的用户中心接口 webapi形式
        /// 旧的wcf方式 handlers/company.ashx已废弃
        /// 潘锦发 20160622
        /// </summary>
        /// <param name="companycode"></param>
        /// <returns></returns>
        public static UserCheck FxtUserCenterService_GetCompanyByCode(string companycode)
        {
            //web.config设置中心用户API的地址
            string api = FxtUserCenterService;
            string appId = WebCommon.GetConfigSetting("UserCenterServiceAppId");
            string appPwd = WebCommon.GetConfigSetting("UserCenterServiceAppPwd");
            string appKey = WebCommon.GetConfigSetting("UserCenterServiceAppKey");
            string signname = WebCommon.GetConfigSetting("SignName");
            UserCheck usercheck = null;
            if (!string.IsNullOrEmpty(api))
            {
                LoginInfoEntity loginInfo = new LoginInfoEntity();
                loginInfo.signname = signname;
                SurveyApi sa = new SurveyApi(loginInfo);
                sa.info.appinfo = new ApplicationInfo(((int)EnumHelper.Codes.SysTypeCodeGJB).ToString());
                sa.sinfo = new SecurityInfo(loginInfo, appId, appPwd, appKey);
                sa.sinfo.functionname = "companyone";
                sa.info.funinfo = new { companycode = companycode };
                string str = APIPostBack(api, sa.GetJsonString(), false, "application/json");
                if (!string.IsNullOrEmpty(str))
                {
                    JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                    if (rtn.returntype > 0)
                    {
                        usercheck = new UserCheck();
                        var companyInfo = JSONHelper.JSONToObject<dynamic>(rtn.data.ToString());
                        string businessDB = Convert.ToString(companyInfo["businessdb"]);
                        int fxtcompanyid = Convert.ToInt32(companyInfo["companyid"]);
                        string wxid = Convert.ToString(companyInfo["wxid"]);
                        string wxname = Convert.ToString(companyInfo["wxname"]);
                        usercheck.companyid = fxtcompanyid;
                        usercheck.businessdb = businessDB;
                        usercheck.wxname = wxname;
                        usercheck.wxid = wxid;
                        return usercheck;
                    }
                }
            }
            return usercheck;
        }
        /// <summary>
        /// 根据公司code获取公司信息
        /// 勿调用
        /// 此方法经检查已经失效 调用的数据中心接口已废弃  20160622 pjf
        /// </summary>
        /// <param name="companycode"></param>
        /// <returns></returns>
        public static UserCheck FxtUserCenterService_GetCompanyByCode(string companycode, out string msg)
        {
            //web.config设置中心用户API的地址
            string api = GetConfigSetting("fxtusercentercompanyinfoservice");  //获取公司CODE相关信息的API地址
            string appId = WebCommon.GetConfigSetting("UserCenterServiceAppId");
            string appPwd = WebCommon.GetConfigSetting("UserCenterServiceAppPwd");
            string appKey = WebCommon.GetConfigSetting("UserCenterServiceAppKey");
            string signname = WebCommon.GetConfigSetting("SignName");
            UserCheck usercheck = null;
            msg = string.Empty;
            if (!string.IsNullOrEmpty(api))
            {
                LoginInfoEntity loginInfo = new LoginInfoEntity();
                loginInfo.signname = signname;
                SurveyApi sa = new SurveyApi(loginInfo);
                sa.info.appinfo = new ApplicationInfo(((int)EnumHelper.Codes.SysTypeCodeGJB).ToString());
                sa.sinfo = new SecurityInfo(loginInfo, appId, appPwd, appKey);
                sa.sinfo.functionname = "companyone";
                sa.info.funinfo = new { companycode = companycode };

                string str = APIPostBack(api, sa.GetJsonString(), false, "application/json");

                if (!string.IsNullOrEmpty(str))
                {
                    JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                    if (rtn.returntype > 0)
                    {
                        usercheck = new UserCheck();
                        var companyInfo = JSONHelper.JSONToObject<dynamic>(rtn.data.ToString());
                        string businessDB = Convert.ToString(companyInfo["businessdb"]);
                        int fxtcompanyid = Convert.ToInt32(companyInfo["companyid"]);

                        usercheck.companyid = fxtcompanyid;
                        usercheck.businessdb = businessDB;
                        return usercheck;
                    }
                    else
                    {
                        msg = rtn.returntext.ToString();
                    }
                }
            }
            return usercheck;
        }

        /// <summary>
        /// 根据公司Code获取公司信息
        /// web.config下appSettings添加配置wcfusercenterservice用户中心功能地址
        /// </summary>
        /// <param name="companycode"></param>
        /// <param name="returntext">输出:说明</param>
        /// <param name="returntype">输出:(1:成功,-1失败)</param>
        /// <returns></returns>
        public static UserCheck FxtUserCenterService_GetCompanyByCode(string postJson, out string returntext, out int returntype)
        {
            //web.config设置中心用户API的地址
            string api = WCFUserCenterService;
            UserCheck usercheck = new UserCheck();
            returntext = string.Empty;
            returntype = 0;
            if (!string.IsNullOrEmpty(api))
            {
                try
                {
                    string str = WcfApiMethodOfPost(api, postJson); //LogHelper.Info(str);
                    if (!string.IsNullOrEmpty(str))
                    {
                        WCFJsonData rtn = JSONHelper.JSONToObject<WCFJsonData>(str);

                        if (rtn.returntype > 0)
                        {
                            usercheck = JSONHelper.JSONToObject<UserCheck>(rtn.data.ToString());
                            returntext = rtn.returntext.ToString();
                            returntype = rtn.returntype;
                        }
                        else
                        {
                            returntext = rtn.returntext.ToString();
                            returntype = rtn.returntype;
                            LogHelper.Info("用户中心验证失败:" + returntext);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                    throw ex;
                }
            }
            return usercheck;
        }

        /// <summary>
        /// 根据公司Code获取公司信息
        /// web.config下appSettings添加配置wcfusercenterservice用户中心功能地址
        /// </summary>
        /// <param name="companycode"></param>
        /// <param name="returntext">输出:说明</param>
        /// <param name="returntype">输出:(1:成功,-1失败)</param>
        /// <returns></returns>
        public static string FxtUserCenterService_SSone(string postJson, out string returntext, out int returntype)
        {
            //web.config设置中心用户API的地址
            string api = WCFUserCenterService;
            string result = string.Empty;
            UserCheck usercheck = new UserCheck();
            returntext = string.Empty;
            returntype = 0;
            if (!string.IsNullOrEmpty(api))
            {
                result = WcfApiMethodOfPost(api, postJson);

            }
            return result;
        }

        /// <summary>
        /// 获取公司列表
        /// </summary>
        /// <returns>数据库连接列表</returns>
        public static List<UserCheck> GetUserCenterCompanyList()
        {
            //PublicCacheCommon.Get<List<UserCheck>>(CacheKey.UserCenterCompanyList)
            List<UserCheck> list = PublicCacheCommon.Get<List<UserCheck>>(CacheKey.UserCenterCompanyList);
            if (list != null) return list;

            //web.config设置中心用户API的地址
            string api = FxtUserCenterService;
            string appId = WebCommon.GetConfigSetting("UserCenterServiceAppId");
            string appPwd = WebCommon.GetConfigSetting("UserCenterServiceAppPwd");
            string appKey = WebCommon.GetConfigSetting("UserCenterServiceAppKey");
            string signname = WebCommon.GetConfigSetting("SignName");
            
            if (!string.IsNullOrEmpty(api))
            {
                LoginInfoEntity loginInfo = new LoginInfoEntity();
                loginInfo.signname = signname;
                SurveyApi sa = new SurveyApi(loginInfo);
                sa.info.appinfo = new ApplicationInfo(((int)EnumHelper.Codes.SysTypeCodeGJB).ToString());
                sa.sinfo = new SecurityInfo(loginInfo, appId, appPwd, appKey);
                sa.sinfo.functionname = "companyten";
                sa.info.funinfo = new { systypecode = 1003018 };
                string str = APIPostBack(api, sa.GetJsonString(), false, "application/json");
                if (!string.IsNullOrEmpty(str))
                {
                    JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                    if (rtn.returntype > 0)
                    {
                        list=JSONHelper.JSONStringToList<UserCheck>(rtn.data.ToString());
                        PublicCacheCommon.Set<List<UserCheck>>(CacheKey.UserCenterCompanyList, list, 24 * 60);
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// 获取公司开通产品的城市
        /// </summary>
        /// <param name="companySignName">公司signame</param>
        /// <param name="productCode">产品code</param>
        /// <returns></returns>
        public static List<int> FxtUserCenterService_GetCompanyOpenProductCityIds(string companySignName, int productCode)
        {
            //web.config设置中心用户API的地址
            string api = FxtUserCenterService;
            string appId = WebCommon.GetConfigSetting("UserCenterServiceAppId");
            string appPwd = WebCommon.GetConfigSetting("UserCenterServiceAppPwd");
            string appKey = WebCommon.GetConfigSetting("UserCenterServiceAppKey");
            string signname = WebCommon.GetConfigSetting("SignName");
            List<int> cityList = new List<int>();
            if (!string.IsNullOrEmpty(api))
            {
                LoginInfoEntity loginInfo = new LoginInfoEntity();
                loginInfo.signname = signname;
                SurveyApi sa = new SurveyApi(loginInfo);
                sa.info.appinfo = new ApplicationInfo(((int)EnumHelper.Codes.SysTypeCodeGJB).ToString());
                sa.sinfo = new SecurityInfo(loginInfo, appId, appPwd, appKey);
                sa.sinfo.functionname = "cptcityids";
                sa.info.funinfo = new { signname = companySignName, productcode = productCode };
                string str = APIPostBack(api, sa.GetJsonString(), false, "application/json");
                if (!string.IsNullOrEmpty(str))
                {
                    JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                    if (rtn.returntype > 0)
                    {
                        cityList = JSONHelper.JSONStringToList<int>(rtn.data.ToString());
                    }
                }
            }
            return cityList;
        }
        /// <summary>
        /// 给公司开通产品城市
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <param name="productCode">产品code</param>
        /// <param name="cityIds">多个城市ID，逗号分隔</param>
        /// <param name="overdate">到期时间</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static int FxtUserCenterService_OpenCompanyProductCity(int companyId, int productCode,string cityIds,DateTime overdate,out string msg)
        {
            msg = "";
            //web.config设置中心用户API的地址
            string api = FxtUserCenterService;
            string appId = WebCommon.GetConfigSetting("UserCenterServiceAppId");
            string appPwd = WebCommon.GetConfigSetting("UserCenterServiceAppPwd");
            string appKey = WebCommon.GetConfigSetting("UserCenterServiceAppKey");
            string signname = WebCommon.GetConfigSetting("SignName");
            List<int> cityList = new List<int>();
            if (!string.IsNullOrEmpty(api))
            {
                LoginInfoEntity loginInfo = new LoginInfoEntity();
                loginInfo.signname = signname;
                SurveyApi sa = new SurveyApi(loginInfo);
                sa.info.appinfo = new ApplicationInfo(((int)EnumHelper.Codes.SysTypeCodeGJB).ToString());
                sa.sinfo = new SecurityInfo(loginInfo, appId, appPwd, appKey);
                sa.sinfo.functionname = "cpfive";
                sa.info.funinfo = new { addcompanyid = companyId, addproducttypecode = productCode, cityids = cityIds, overdate = overdate.ToString("yyyy-MM-dd") };
                string str = APIPostBack(api, sa.GetJsonString(), false, "application/json");
                if (!string.IsNullOrEmpty(str))
                {
                    JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                    if (rtn.returntype < 1)
                    {
                        msg = rtn.returntext.ToString();
                    }
                    return rtn.returntype;
                }
            }
            return 1;
        }
        /// <summary>
        /// 根据公司SignName获取公司信息
        /// web.config下appSettings添加配置wcfusercenterservice用户中心功能地址
        /// </summary>
        /// <param name="companycode"></param>
        /// <param name="returntext">输出:说明</param>
        /// <param name="returntype">输出:(1:成功,-1失败)</param>
        /// <returns></returns>
        public static UserCheck FxtUserCenterService_GetCompanyBySignName(string securityJson, out string returntext, out int returntype)
        {
            //web.config设置中心用户API的地址
            string api = WCFUserCenterService;
            UserCheck usercheck = new UserCheck();
            returntext = string.Empty;
            returntype = 0;
            if (!string.IsNullOrEmpty(api))
            {
                string str = WcfApiMethodOfPost(api, securityJson);
                if (!string.IsNullOrEmpty(str))
                {
                    WCFJsonData rtn = JSONHelper.JSONToObject<WCFJsonData>(str);
                    if (rtn.returntype > 0)
                    {
                        usercheck = JSONHelper.JSONToObject<UserCheck>(rtn.data.ToString());
                        returntext = rtn.returntext.ToString();
                        returntype = rtn.returntype;
                    }
                    else
                    {
                        returntext = rtn.returntext.ToString();
                        returntype = rtn.returntype;
                    }
                }
            }
            return usercheck;
        }

        /// <summary>
        /// 根据产品code查评估机构信息  
        /// hody
        /// </summary>
        /// <param name="companycode"></param>
        /// <returns></returns>
        public static string FxtUserCenterService_GetCompanyInfoByProductCode(SearchBase search, int producttypecode, string key)
        {
            //web.config设置中心用户API的地址
            string api = FxtUserCenterService;
            string str = string.Empty;

            if (!string.IsNullOrEmpty(api))
            {
                string url = api + "handlers/company.ashx";
                str = APIPostBack(url, string.Format("type={0}&producttypecode={1}&key={2}&pageindex={3}&pagerecords={4}", "companysearch", producttypecode, key, search.PageIndex, search.PageRecords), true);

            }
            return str;
        }
        /// <summary>
        /// 根据公司id获取公司信息
        /// </summary>
        /// <param name="companycode"></param>
        /// <returns></returns>
        public static UserCheck FxtUserCenterService_GetCompanyByCompanyId(int companyid, out string msg)
        {
            //web.config设置中心用户API的地址
            string api = FxtUserCenterService;
            UserCheck usercheck = null;
            msg = string.Empty;
            if (!string.IsNullOrEmpty(api))
            {
                string url = api + "handlers/company.ashx";
                string str = APIPostBack(url, string.Format("type={0}&companyid={1}", "companyid", companyid), true);
                if (!string.IsNullOrEmpty(str))
                {
                    JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                    if (rtn.returntype > 0)
                    {
                        usercheck = new UserCheck();
                        return JSONHelper.JSONToObject<UserCheck>(JSONHelper.ObjectToJSON(rtn.data));
                    }
                    else
                    {
                        msg = rtn.returntext.ToString();
                    }
                }
            }
            return usercheck;
        }

        /// <summary>
        /// 根据公司id获取公司信息
        /// </summary>
        /// <param name="companycode"></param>
        /// <returns></returns>
        public static string FxtUserCenterService_EditCompany(string companycode, string wxid, string wxname)
        {
            //web.config设置中心用户API的地址
            string api = FxtUserCenterService;
            string massage = string.Empty;
            UserCheck usercheck = null;
            if (!string.IsNullOrEmpty(api))
            {
                string url = api + "handlers/company.ashx";
                string str = APIPostBack(url, string.Format("type={0}&companycode={3}&wxid={1}&wxname={2}", "editcompany", wxid, wxname, companycode), true);
                if (!string.IsNullOrEmpty(str))
                {
                    JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                    if (rtn.returntype > 0)
                    {
                        usercheck = new UserCheck();
                        return massage = "1";
                    }
                    else
                    {
                        massage = "-1";
                    }
                }
            }
            return massage;
        }

        /// <summary>
        /// 根据公司id和产品code获取信息(caoq 2013-7-12)
        /// </summary>
        /// <param name="companyid">公司id</param>
        /// <param name="companyid">公司标识</param>
        /// <param name="producttypecode">产品code</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static UserCheck FxtUserCenterService_GetCompanyProductByParam(int companyid, string signname, int producttypecode, out string msg)
        {
            //web.config设置中心用户API的地址
            string api = FxtUserCenterService;
            UserCheck usercheck = null;
            msg = string.Empty;
            if (!string.IsNullOrEmpty(api))
            {
                string url = api + "handlers/companyproduct.ashx";
                string str = APIPostBack(url, string.Format("type={0}&companyid={1}&signname={2}&producttypecode={3}", "companyproduct", companyid, signname, producttypecode), true);
                if (!string.IsNullOrEmpty(str))
                {
                    JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                    if (rtn.returntype > 0)
                    {
                        usercheck = new UserCheck();
                        return JSONHelper.JSONToObject<UserCheck>(JSONHelper.ObjectToJSON(rtn.data));
                    }
                    else
                    {
                        msg = rtn.returntext.ToString();
                    }
                }
            }
            return usercheck;
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="companyid">公司ID</param>
        /// <param name="companycode">公司CODE</param>
        /// <param name="userids">用户ID</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static List<UserCheck> FxtUserCenterService_GetUserList(int companyid, string companycode, string[] userids, out string msg)
        {
            //web.config设置中心用户API的地址
            string api = FxtUserCenterService;
            List<UserCheck> list = null;
            msg = string.Empty;
            if (!string.IsNullOrEmpty(api))
            {
                string url = api + "handlers/user_list.ashx";
                string uids = (userids == null || userids.Length == 0) ? null : string.Join(",", userids.Select(i => i).ToArray());
                string str = APIPostBack(url, string.Format("type={0}&companyid={1}&companycode={2}&userids={3}", "list", companyid, companycode, uids), true);
                if (!string.IsNullOrEmpty(str))
                {
                    JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                    if (rtn.returntype > 0)
                    {
                        list = new List<UserCheck>();
                        return JSONHelper.JSONStringToList<UserCheck>(JSONHelper.ObjectToJSON(rtn.data));
                    }
                    else
                    {
                        msg = rtn.returntext.ToString();
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 绑定用户手机推送号信息
        /// </summary>
        /// <param name="username"></param>
        /// <param name="producttypecode"></param>
        /// <param name="splatype"></param>
        /// <param name="phshuserid"></param>
        /// <param name="channelid"></param>
        /// <param name="msg"></param>
        public static string FxtUserCenterService_BindUserMobilePush(string username, int producttypecode, string splatype, string phshuserid, string channelid, out string msg)
        {
            string api = FxtUserCenterService;
            msg = string.Empty;
            if (!string.IsNullOrEmpty(api))
            {
                string url = api + "handlers/mobilepush.ashx";
                string str = APIPostBack(url, string.Format("type={0}&username={1}&producttypecode={2}&splatype={3}&phshuserid={4}&channelid={5}"
                    , "bind", username, producttypecode, splatype, phshuserid, channelid), true);
                if (!string.IsNullOrEmpty(str))
                {
                    JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                    if (rtn.returntype > 0)
                    {
                        return msg = "1";
                    }
                    else
                    {
                        msg = "-1";
                    }
                }
            }
            return msg;
        }

        /// <summary>
        /// 清除用户绑定的手机推送号信息
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="producttypecode"></param>
        /// <param name="channelid">渠道Id</param>
        /// <param name="phshuserid">设备Id</param>
        /// <param name="splatype">手机平台类型</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string FxtUserCenterService_ClearUserMobilePush(string username, int producttypecode, string channelid, string phshuserid, string splatype, out string msg)
        {
            string api = FxtUserCenterService;
            msg = string.Empty;
            if (!string.IsNullOrEmpty(api))
            {
                string url = api + "handlers/mobilepush.ashx";
                string str = APIPostBack(url, string.Format("type={0}&username={1}&producttypecode={2}&channelid={3}&phshuserid={4}&splatype={5}", "exit", username, producttypecode, channelid, phshuserid, splatype), true);
                if (!string.IsNullOrEmpty(str))
                {
                    JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                    if (rtn.returntype > 0)
                    {
                        return msg = "1";
                    }
                    else
                    {
                        msg = "-1";
                    }
                }
            }
            return msg;
        }

        /// <summary>
        /// 推送信息到手机
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="neirong">消息内容</param>
        /// <param name="entrustid"></param>
        /// <param name="systypecode">产品CODE</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static int FxtUserCenterService_SendMesToMobile(string username, string neirong, long entrustid, int systypecode, out string msg, LoginInfoEntity loginInfo)
        {
            int result = 0;
            msg = "";
            string api = FxtUserCenterService;
            if (!string.IsNullOrEmpty(api))
            {
                var sinfo = new CAS.Entity.SecurityInfo(loginInfo, loginInfo.apps.FirstOrDefault().appid, loginInfo.apps.FirstOrDefault().apppwd, loginInfo.apps.FirstOrDefault().appkey);
                sinfo.functionname = "mptwo";
                var info = new
                {
                    uinfo = new CAS.Entity.UserInfo(loginInfo),
                    appinfo = new CAS.Entity.ApplicationInfo(systypecode.ToString()),
                    funinfo = new
                    {
                        companyid = loginInfo.fxtcompanyid,
                        username = username,
                        neirong = neirong,
                        entrustid = entrustid,
                        systypecode = EnumHelper.Codes.SysTypeCodeCMB_Bank
                    }
                };
                string post = "{\"sinfo\":\"" + JSONHelper.ObjectToJSON(sinfo).Replace("\"", "'") + "\",\"info\":\"" + JSONHelper.ObjectToJSON(info).Replace("\"", "'") + "\"}";
                string str = APIPostBack(api, post, false, "application/json");
                JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                msg = rtn.returntext.ToString();
                if (rtn.returntype > 0)
                {
                    result = rtn.returntype;
                }
            }
            return result;
        }


        /// <summary>
        /// 设置微信公众号信息
        /// </summary>
        /// <param name="fxtCompanyId">评估机构ID</param>
        /// <param name="wxname"></param>
        /// <param name="wxid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static int FxtUserCenterService_SetCompanyWXByCompanyId(int fxtCompanyId, string wxname, string wxid, out string msg)
        {
            //web.config设置中心用户API的地址
            string api = FxtUserCenterService;
            string appId = WebCommon.GetConfigSetting("UserCenterServiceAppId");
            string appPwd = WebCommon.GetConfigSetting("UserCenterServiceAppPwd");
            string appKey = WebCommon.GetConfigSetting("UserCenterServiceAppKey");
            string signname = WebCommon.GetConfigSetting("SignName");
            msg = "";
            if (!string.IsNullOrEmpty(api))
            {
                LoginInfoEntity loginInfo = new LoginInfoEntity();
                loginInfo.signname = signname;
                SurveyApi sa = new SurveyApi(loginInfo);
                sa.info.appinfo = new ApplicationInfo(((int)EnumHelper.Codes.SysTypeCodeGJB).ToString());
                sa.sinfo = new SecurityInfo(loginInfo, appId, appPwd, appKey);
                sa.sinfo.functionname = "companyeleven";
                sa.info.funinfo = new { companyid = fxtCompanyId.ToString(), wxid = wxid, wxname = wxname };
                string str = APIPostBack(api, sa.GetJsonString(), false, "application/json");
                if (!string.IsNullOrEmpty(str))
                {
                    JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                    if (rtn.returntype < 1)
                    {
                        msg = Convert.ToString(rtn.returntext);
                    }
                    return rtn.returntype;
                }
            }
            return 1;
        }
        //取配置文件appsetting值
        public static string GetConfigSetting(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }

        public static string GetIPAddress()
        {
            try
            {
                string forwarded = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                string remote = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                string IP = GetIPAddress(forwarded, remote);
                return IP;
            }
            catch { return ""; }
        }
        /// <summary>
        /// 获得浏览器名称以及版本
        /// </summary>
        /// <returns></returns>
        public static string GetBrowserVesion()
        {
            try
            {
                string BrowserVesion = HttpContext.Current.Request.Browser.Browser + "|" + HttpContext.Current.Request.Browser.Version;

                return BrowserVesion;
            }
            catch { return ""; }
        }

        /// <summary>
        /// 取得客户端真实IP。如果有代理则取第一个非内网地址  
        /// </summary>
        /// <param name="forwarded">获得IP的参数</param>
        /// <param name="remote"></param>
        /// <returns></returns>
        public static string GetIPAddress(string forwarded, string remote)
        {
            //forwarded＝ HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]
            //remote ＝ HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]
            string result = String.Empty;
            result = forwarded;
            if (result != null && result != String.Empty)
            {
                //可能有代理  
                if (result.IndexOf(".") == -1)        //没有“.”肯定是非IPv4格式  
                    result = null;
                else
                {
                    if (result.IndexOf(",") != -1)
                    {
                        //有“,”，估计多个代理。取第一个不是内网的IP。  
                        result = result.Replace("  ", "").Replace("'", "");
                        string[] temparyip = result.Split(",;".ToCharArray());
                        for (int i = 0; i < temparyip.Length; i++)
                        {
                            if (IsIPAddress(temparyip[i])
                                    && temparyip[i].Substring(0, 3) != "10."
                                    && temparyip[i].Substring(0, 7) != "192.168"
                                    && temparyip[i].Substring(0, 7) != "172.16.")
                            {
                                return temparyip[i];        //找到不是内网的地址  
                            }
                        }
                    }
                    else if (IsIPAddress(result))  //代理即是IP格式  
                        return result;
                    else
                        result = null;        //代理中的内容  非IP，取IP  
                }
            }

            string IpAddress = (result != null && result != String.Empty) ? result : remote;

            if (null == result || result == String.Empty)
                result = remote;
            if (result == null || result == String.Empty)
                result = HttpContext.Current.Request.UserHostAddress;
            return result;
        }
        /// <summary>
        /// 判断是否是IP地址格式 0.0.0.0
        /// </summary>
        /// <param name="str1">待判断的IP地址</param>
        /// <returns>true or false</returns>
        public static bool IsIPAddress(string str1)
        {
            if (str1 == null || str1 == string.Empty || str1.Length < 7 || str1.Length > 15) return false;

            string regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$";

            Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);

            return regex.IsMatch(str1);
        }

        /// <summary>
        /// 产生随机字符串，用于客户端随机命名
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string GetRndString(int len)
        {
            string s = Guid.NewGuid().ToString().Replace("-", "");
            return s.Substring(0, len > s.Length ? s.Length : len);
        }

        /// <summary>
        /// 随机生成指定位数字符串:必须包含字母与数字
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string GetLenRndString(int len)
        {

            Random rd = new Random();
            string str = "abcdefghijklmnopqrstuvwxyz0123456789";
            string result = "";
            for (int i = 0; i < len; i++)
            {
                result += str[rd.Next(str.Length)];
            }
            Regex regex = new Regex(@"\d+");
            if (!regex.IsMatch(result))
            {
                result = result.Insert(3, "8");
            }

            return result;
        }



        /// <summary>
        /// 取随机数
        /// </summary>
        /// <returns></returns>
        public static int GetRandom()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            int rtn = BitConverter.ToInt32(bytes, 0);
            if (rtn < 0) rtn = 0 - rtn;
            return rtn;
        }

        /*产生验证码*/
        public static string CreateCode(int codeLength)
        {

            string so = "1,2,3,4,5,6,7,8,9,0,A,B,C,D,G,H";//,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z
            string[] strArr = so.Split(',');
            string code = "";
            Random rand = new Random();
            for (int i = 0; i < codeLength; i++)
            {
                code += strArr[rand.Next(0, strArr.Length)];
            }
            return code;
        }

        /*产生验证码*/
        public static string NumCode(int codeLength)
        {

            string so = "1,2,3,4,5,6,7,8,9,0";//,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z
            string[] strArr = so.Split(',');
            string code = "";
            Random rand = new Random();
            for (int i = 0; i < codeLength; i++)
            {
                code += strArr[rand.Next(0, strArr.Length)];
            }
            return code;
        }

        /*产生验证图片*/
        public static System.IO.MemoryStream CreateImages(string code)
        {

            Bitmap image = new Bitmap(60, 20);
            Graphics g = Graphics.FromImage(image);
            WebColorConverter ww = new WebColorConverter();
            g.Clear((Color)ww.ConvertFromString("#FAE264"));

            Random random = new Random();
            //画图片的背景噪音线
            for (int i = 0; i < 12; i++)
            {
                int x1 = random.Next(image.Width);
                int x2 = random.Next(image.Width);
                int y1 = random.Next(image.Height);
                int y2 = random.Next(image.Height);

                g.DrawLine(new Pen(Color.LightGray), x1, y1, x2, y2);
            }
            Font font = new Font("Arial", 15, FontStyle.Bold | FontStyle.Italic);
            System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(
             new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.Gray, 1.2f, true);
            g.DrawString(code, font, brush, 0, 0);

            //画图片的前景噪音点
            for (int i = 0; i < 10; i++)
            {
                int x = random.Next(image.Width);
                int y = random.Next(image.Height);
                image.SetPixel(x, y, Color.White);
            }

            //画图片的边框线
            g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);

            g.Dispose();
            image.Dispose();

            return ms;
        }

        /// <summary>
        /// 发消息
        /// </summary>
        /// <param name="url">消息服务器</param>
        /// <param name="cityid"></param>
        /// <param name="userid">发送者</param>
        /// <param name="touserid">接收者</param>
        /// <param name="msgid">消息id来自message表</param>
        /// <param name="tosystypecode">接收方系统类型</param>
        /// <param name="fxtcompanyid">运营方id</param>
        /// <param name="messagetype">消息类型</param>
        public static void SendMessage(string url, int cityid, string userid, string touserid, int msgid, int tosystypecode, int fxtcompanyid, int messagetype)
        {
            MessageBody msg = new MessageBody();
            msg.url = url;
            msg.userid = userid;
            msg.touserid = touserid;
            msg.cityid = cityid;
            msg.msgid = msgid;
            msg.tosystypecode = tosystypecode;
            msg.fxtcompanyid = fxtcompanyid;
            msg.messagetype = messagetype;
            Thread t = new Thread(WebCommon.SendMessageToServer);
            t.IsBackground = true;
            t.Start(msg);
        }
        //public static void SendMessage(string url, int msgid,DatMessage model)
        //{
        //    MessageBody msg = new MessageBody();
        //    msg.url = url;
        //    //msg.userid = model.senduserid;
        //    //msg.touserid = model.touserid;
        //    //msg.cityid = model.cityid;
        //    //msg.msgid = msgid;
        //    //msg.tosystypecode = 1003003;
        //    //msg.companyid = model.companyid;
        //    //msg.messagetype = model.messagetype;
        //    Thread t = new Thread(WebCommon.SendMessageToServer);
        //    t.IsBackground = true;
        //    t.Start(msg);            
        //}

        /// <summary>
        /// 发消息
        /// </summary>
        /// <param name="msg"></param>
        public static void SendMessage(MessageBody msg)
        {
            Thread t = new Thread(WebCommon.SendMessageToServer);
            t.IsBackground = true;
            t.Start(msg);
        }

        /// <summary>
        /// 发送消息到服务器，用线程是为了不阻塞主线程 kevin
        /// </summary>
        /// <param name="o"></param>
        public static void SendMessageToServer(object o)
        {
            if (o is MessageBody)
            {
                try
                {
                    MessageBody msg = (MessageBody)o;
                    string posts = string.Format("cityid={0}&userid={1}&touserid={2}&msgid={3}&tosystypecode={4}&fxtcompanyid={5}&messagetype={6}"
                        , msg.cityid, msg.userid, msg.touserid, msg.msgid, msg.tosystypecode, msg.fxtcompanyid, msg.messagetype);
                    byte[] postData = Encoding.UTF8.GetBytes(posts);
                    WebClient client = new WebClient();
                    client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    client.Headers.Add("ContentLength", postData.Length.ToString());

                    client.UploadData(new Uri(msg.url), "POST", postData);
                }
                catch (Exception ex) { LogHelper.Error(ex); }
            }
        }

        public static string GetRequest(string key)
        {
            return GetRequest(key, string.Empty);
        }

        /// <summary>
        /// 取request参数，注意不能用request[key]，因为这样会包含cookie和servervariables
        /// 修改人：曾智磊，2014-12-19
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defVal"></param>
        /// <returns></returns>
        public static string GetRequest(string key, string defVal, HttpRequest request = null)
        {
            HttpRequest Request = request;
            if (request == null)
            {
                Request = HttpContext.Current.Request;
            }
            if (Request.QueryString[key] != null)
                return Request.QueryString[key];
            if (Request.Form[key] != null)
                return Request.Form[key];
            return defVal;
        }

        /// <summary>
        /// 从实体中获取数据，组装成字典，用于报告生成或其他 kevin
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Dictionary<string, string> getDictFromModel<T>(T t)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            Type type = t.GetType();
            PropertyInfo[] infos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo info in infos)
            {
                dict.Add(info.Name.ToLower(), info.GetValue(t, null) == null ? "" : info.GetValue(t, null).ToString());
            }
            return dict;
        }

        /// <summary>
        /// 根据http请求自动生成实体
        /// </summary>        
        public static T InitModel<T>(HttpRequest request, T t, string preFix) where T : CAS.Entity.BaseDAModels.BaseTO
        {
            Type type = t.GetType();
            PropertyInfo[] infos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            string[] requestKeys = request.QueryString.AllKeys
                                   .Concat(request.Form.AllKeys)
                                   .ToArray();
            foreach (string key in requestKeys)
            {
                foreach (PropertyInfo info in infos)
                {
                    if (preFix + info.Name.ToLower() == key.ToLower())
                    {
                        switch (info.PropertyType.FullName.Split(',')[0])
                        {
                            case "System.String":
                                info.SetValue(t, GetRequest(key), null);
                                break;
                            case "System.Int64":
                                info.SetValue(t, StringHelper.TryGetLongObject(GetRequest(key)), null);
                                break;
                            case "System.Nullable`1[[System.Int64":
                                info.SetValue(t, StringHelper.TryGetLongObject(GetRequest(key)), null);
                                break;
                            case "System.Int32":
                                info.SetValue(t, StringHelper.TryGetIntObject(GetRequest(key)), null);
                                break;
                            case "System.Nullable`1[[System.Int32":
                                info.SetValue(t, StringHelper.TryGetIntObject(GetRequest(key)), null);
                                break;
                            case "System.Int16":
                                info.SetValue(t, StringHelper.TryGetShortObject(GetRequest(key)), null);
                                break;
                            case "System.Nullable`1[[System.Int16":
                                info.SetValue(t, StringHelper.TryGetShortObject(GetRequest(key)), null);
                                break;
                            case "System.DateTime":
                                info.SetValue(t, StringHelper.TryGetDateTime(GetRequest(key)), null);
                                break;
                            case "System.Nullable`1[[System.DateTime":
                                info.SetValue(t, StringHelper.TryGetDateTime(GetRequest(key)), null);
                                break;
                            case "System.Decimal":
                                info.SetValue(t, StringHelper.TryGetDecimalObject(GetRequest(key)), null);
                                break;
                            case "System.Nullable`1[[System.Decimal":
                                info.SetValue(t, StringHelper.TryGetDecimalObject(GetRequest(key)), null);
                                break;
                            case "System.Double":
                                info.SetValue(t, StringHelper.TryGetDoubleObject(GetRequest(key)), null);
                                break;
                            case "System.Nullable`1[[System.Double":
                                info.SetValue(t, StringHelper.TryGetDoubleObject(GetRequest(key)), null);
                                break;
                            case "System.Boolean":
                                info.SetValue(t, StringHelper.TryGetBool(GetRequest(key)), null);
                                break;
                            case "System.Nullable`1[[System.Boolean":
                                info.SetValue(t, StringHelper.TryGetBool(GetRequest(key)), null);
                                break;
                            case "System.Byte":
                                info.SetValue(t, StringHelper.TryGetByte(GetRequest(key)), null);
                                break;
                            case "System.Nullable`1[[System.Byte":
                                info.SetValue(t, StringHelper.TryGetByte(GetRequest(key)), null);
                                break;
                        }
                        break;
                    }
                }
            }

            return t;
        }
        /// <summary>       
        /// 根据键值对字典自动生成实体
        /// 创建人：曾智磊，20150114
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="t"></param>
        /// <param name="preFix"></param>
        /// <returns></returns>
        public static T InitModel<T>(Dictionary<string, string> dicPropertyInfo, T t, string preFix) where T : CAS.Entity.BaseDAModels.BaseTO
        {
            Type type = t.GetType();
            PropertyInfo[] infos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            string[] requestKeys = dicPropertyInfo.Keys.ToArray<string>();
            foreach (string key in requestKeys)
            {
                foreach (PropertyInfo info in infos)
                {
                    if (preFix + info.Name.ToLower() == key.ToLower())
                    {
                        switch (info.PropertyType.FullName.Split(',')[0])
                        {
                            case "System.String":
                                info.SetValue(t, dicPropertyInfo[key], null);
                                break;
                            case "System.Int64":
                                info.SetValue(t, StringHelper.TryGetLongObject(dicPropertyInfo[key]), null);
                                break;
                            case "System.Nullable`1[[System.Int64":
                                info.SetValue(t, StringHelper.TryGetLongObject(dicPropertyInfo[key]), null);
                                break;
                            case "System.Int32":
                                info.SetValue(t, StringHelper.TryGetIntObject(dicPropertyInfo[key]), null);
                                break;
                            case "System.Nullable`1[[System.Int32":
                                info.SetValue(t, StringHelper.TryGetIntObject(dicPropertyInfo[key]), null);
                                break;
                            case "System.Int16":
                                info.SetValue(t, StringHelper.TryGetShortObject(dicPropertyInfo[key]), null);
                                break;
                            case "System.Nullable`1[[System.Int16":
                                info.SetValue(t, StringHelper.TryGetShortObject(dicPropertyInfo[key]), null);
                                break;
                            case "System.DateTime":
                                info.SetValue(t, StringHelper.TryGetDateTime(dicPropertyInfo[key]), null);
                                break;
                            case "System.Nullable`1[[System.DateTime":
                                info.SetValue(t, StringHelper.TryGetDateTime(dicPropertyInfo[key]), null);
                                break;
                            case "System.Decimal":
                                info.SetValue(t, StringHelper.TryGetDecimalObject(dicPropertyInfo[key]), null);
                                break;
                            case "System.Nullable`1[[System.Decimal":
                                info.SetValue(t, StringHelper.TryGetDecimalObject(dicPropertyInfo[key]), null);
                                break;
                            case "System.Double":
                                info.SetValue(t, StringHelper.TryGetDoubleObject(dicPropertyInfo[key]), null);
                                break;
                            case "System.Nullable`1[[System.Double":
                                info.SetValue(t, StringHelper.TryGetDoubleObject(dicPropertyInfo[key]), null);
                                break;
                            case "System.Boolean":
                                info.SetValue(t, StringHelper.TryGetBool(dicPropertyInfo[key]), null);
                                break;
                            case "System.Nullable`1[[System.Boolean":
                                info.SetValue(t, StringHelper.TryGetBool(dicPropertyInfo[key]), null);
                                break;
                            case "System.Byte":
                                info.SetValue(t, StringHelper.TryGetByte(dicPropertyInfo[key]), null);
                                break;
                            case "System.Nullable`1[[System.Byte":
                                info.SetValue(t, StringHelper.TryGetByte(dicPropertyInfo[key]), null);
                                break;
                        }
                        break;
                    }
                }
            }

            return t;
        }
        /// <summary>
        /// 根据键值对字典自动生成实体
        /// 创建人：曾智磊，20150114
        /// </summary>
        public static T InitModel<T>(Dictionary<string, string> dicPropertyInfo) where T : CAS.Entity.BaseDAModels.BaseTO, new()
        {
            T t = new T();
            return WebCommon.InitModel<T>(dicPropertyInfo, t, null);
        }
        /// <summary>
        /// 根据http请求自动生成实体
        /// </summary>
        public static T InitModel<T>(HttpRequest request) where T : CAS.Entity.BaseDAModels.BaseTO, new()
        {
            T t = new T();
            return InitModel<T>(request, t, null);
        }

        /// <summary>
        /// 根据指定字段前缀生成实体
        /// </summary>
        public static T InitModel<T>(HttpRequest request, string preFix) where T : CAS.Entity.BaseDAModels.BaseTO, new()
        {
            T t = new T();
            return WebCommon.InitModel<T>(request, t, preFix);
        }


        /// <summary>
        /// 是否管理员
        /// </summary>
        /// <param name="username"></param>
        public static bool IsAdmin(string username)
        {
            if (!string.IsNullOrEmpty(username) && username.IndexOf('@') != -1)
                return username.Split('@')[0] == ConstCommon.Administrator;
            return false;
        }
        /// <summary>
        /// 获取CAS权限
        /// api, cas
        /// kingfer 20161014
        /// </summary>
        /// <param name="priv">功能权限</param>
        /// <param name="rights">用户权限</param>
        /// <returns></returns>
        public static bool HasCASPrivilege(string priv, string rights)
        {
            return !string.IsNullOrEmpty(rights) && ("," + rights + ",").IndexOf("," + priv + ",") >= 0;
        }

        /// <summary>
        /// 创建二维码 kevin
        /// </summary>
        /// <param name="content"></param>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static bool CreateQrCode(string content, string filepath)
        {
            try
            {
                QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
                QrCode qrCode = new QrCode();
                qrEncoder.TryEncode(content, out qrCode);
                //GraphicsRenderer renderer = new GraphicsRenderer(new FixedModuleSize(30, QuietZoneModules.Four), Brushes.Black, Brushes.White);  
                GraphicsRenderer gRender = new GraphicsRenderer(new FixedModuleSize(5, QuietZoneModules.Four));
                using (FileStream stream = new FileStream(filepath, FileMode.Create))
                {
                    gRender.WriteToStream(qrCode.Matrix, ImageFormat.Png, stream);
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return false;
            }
        }
        /// <summary>
        /// 创建二维码 ,中间生成公司Logo
        /// 潘锦发
        /// </summary>
        /// <param name="content"></param>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static bool CreateQrCode(string content, System.Drawing.Image imgLogo, string filepath)
        {
            try
            {
                QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
                QrCode qrCode = new QrCode();
                qrEncoder.TryEncode(content, out qrCode);
                //GraphicsRenderer renderer = new GraphicsRenderer(new FixedModuleSize(30, QuietZoneModules.Four), Brushes.Black, Brushes.White);  
                GraphicsRenderer gRender = new GraphicsRenderer(new FixedModuleSize(5, QuietZoneModules.Four));
                byte[] bytes = null;
                System.Drawing.Image imgSource = null;
                //将二维码图片保存成图片类型
                using (MemoryStream stream = new  MemoryStream())
                {
                    gRender.WriteToStream(qrCode.Matrix, ImageFormat.Png, stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.Flush();
                    bytes= stream.ToArray();
                }
                if (bytes != null)
                {
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes))
                    {
                        System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
                        ms.Flush();
                        imgSource = returnImage;
                    }
                }
                if (imgSource != null)
                {
                    if (imgLogo != null)// 如果logo 存在将logo图片合并到二维码中
                    {
                        Bitmap bmpSourceImage = new Bitmap(imgSource);
                        Bitmap bmpAttachImage = new Bitmap(imgLogo);
                        System.Drawing.Image qrImage = CombinImage(bmpSourceImage, bmpAttachImage);
                        qrImage.Save(filepath, ImageFormat.Png);
                    }
                    else//如果不存在 则按原二维码
                    {
                        imgSource.Save(filepath, ImageFormat.Png);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return false;
            }
        }
        /// <summary>  
        /// 将两种图片合并，类似相册，有个  
        /// 背景图，中间贴自己的目标图片  
        /// </summary>  
        /// <param name="sourceImage">粘贴的源图片</param>  
        /// <param name="destBitmap">粘贴的目标图片</param>  
        public static Bitmap CombinImage(Bitmap sourceImage, Bitmap destBitmap)
        {
            int height = Convert.ToInt32(sourceImage.Height * 0.15);
            int width = Convert.ToInt32(sourceImage.Width * 0.15);
            destBitmap = KiResizeImage(destBitmap, width, height);
            using (var g = Graphics.FromImage(sourceImage))
            {
                //g.DrawImage(img, 照片与相框的左边距, 照片与相框的上边距, 照片宽, 照片高); 
                g.DrawImage(sourceImage, 0, 0, sourceImage.Width, sourceImage.Height);
                g.FillRectangle(System.Drawing.Brushes.White, sourceImage.Width / 2 - destBitmap.Width / 2 - 3, sourceImage.Width / 2 - destBitmap.Width / 2 - 3, destBitmap.Width + 6, destBitmap.Height+6);//相片四周刷一层白色边框  
                g.DrawImage(destBitmap, sourceImage.Width / 2 - destBitmap.Width / 2, sourceImage.Width / 2 - destBitmap.Width / 2, destBitmap.Width, destBitmap.Height);
                GC.Collect();
                return sourceImage;
            }
        }
        /// <summary>
        /// Resize图片
        /// </summary>
        /// <param name="bmp">原始Bitmap</param>
        /// <param name="newW">新的宽度</param>
        /// <param name="newH">新的高度</param>
        /// <returns>处理以后的图片</returns>
        public static Bitmap KiResizeImage(Bitmap bmp, int newW, int newH )
        {
            try
            {
                Bitmap b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);                // 插值算法的质量
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                // 高质量
                g.SmoothingMode = SmoothingMode.HighQuality; g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose(); return b;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 数据反射给实体赋值 kevin
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="tbname"></param>
        public static void getValues<T>(T model, Hashtable tbname)
        {
            Type type = model.GetType();
            PropertyInfo[] infos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo info in infos)
            {
                if (tbname.ContainsKey(info.Name))
                {
                    if (!string.IsNullOrEmpty(tbname[info.Name].ToString()))
                    {
                        Type propertyType = info.PropertyType;
                        //Nullable的，要取出实际数据类型
                        if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            propertyType = propertyType.GetGenericArguments()[0];
                        }
                        //值转换
                        object val = Convert.ChangeType(tbname[info.Name], propertyType);
                        info.SetValue(model, val, null);
                    }
                }
            }
        }

        /// <summary>
        /// 自定义字段处理 kevin 2013-3-14
        /// 供API和调度中心使用
        /// 1：获取查勘系统字段，赋值给实体
        /// 2：返回后面页面显示、EXCEL使用的hashtable
        /// 20160511,rock:添加参数containSysField，由于查勘中字段标题能客户自定义，所以加这个来兼容新老字段
        /// </summary>
        /// <param name="model"></param>
        /// <param name="customfields"></param>
        /// <param name="compatibleSysFieldTitle">是否同时兼容客户改前的系统字段</param>
        /// <returns>返回字段信息20160511,rock</returns>
        public static List<mobileFieldValues> GetSurveyFromCustomFields<T>(T model, string customfields, Dictionary<string, string> tbcname, bool compatibleSysFieldTitle = true)
        {
            Hashtable tbname = new Hashtable();//匹配系统字段            
            List<mobileFields> groups = null;
            List<mobileFieldValues> resultFieldList = new List<mobileFieldValues>();
            //绑定字典（匿名函数）
            Func<string, string, object> bindDic = delegate(string _key, string _value)
            {
                #region
                if (tbcname.ContainsKey(_key))//匹配EXCEL模板字段  
                {
                    if (string.IsNullOrEmpty(tbcname[_key]))//为空才替换，byte
                    {
                        tbcname[_key] = _value;
                    }
                }
                else
                {
                    tbcname.Add(_key, _value);
                }
                return null;
                #endregion
            };
            if (!string.IsNullOrEmpty(customfields))
            {
                groups = JSONHelper.JSONStringToList<mobileFields>(customfields);
            }
            if (null != groups)
            {
                foreach (mobileFields item in groups)
                {
                    //系统分类直接由手机返回给实体，不用解析 kevin
                    if (item.n != "survey_bz" && item.n != "house_scal")
                    {
                        string strs = JSONHelper.ObjectToJSON(item.v);
                        List<mobileFieldValues> fields = JSONHelper.JSONStringToList<mobileFieldValues>(strs);
                        resultFieldList.AddRange(fields);
                        foreach (mobileFieldValues field in fields)
                        {
                            bool sysfield = field.f > 0;
                            if (sysfield)
                            {
                                if (tbname.ContainsKey(field.n.ToLower()))
                                {
                                    tbname[field.n.ToLower()] = field.s;
                                }
                                else
                                {
                                    tbname.Add(field.n.ToLower(), field.s);//匹配系统字段,字段名称小写，与实体一致
                                }
                            }
                            string v = string.IsNullOrEmpty(field.s) ? "" : field.s;
                            v = field.t == "c" ? v.Replace(",", "、") : v;//将报告生成的复选框以顿号隔开
                            bindDic(field.c, v);
                            string nKey = field.c;
                            //由于查勘中字段标题能客户自定义，所以加这个来兼容新老字段20160511,rock
                            if (compatibleSysFieldTitle && !string.IsNullOrEmpty(field.o))
                            {
                                nKey = field.o;
                                bindDic(field.o, v);
                            }
                            //特殊处理，因为估价宝中委估对象叫“装修”，而查勘中叫"装修档次"，由于客户已经配了字段为“装修”的模板，为了不让客户改模板所以需要兼容
                            if (nKey == "装修档次")
                            {
                                bindDic("装修", v);
                            }
                        }
                    }
                }
                //从数据反射给实体赋值 kevin
                getValues(model, tbname);
            }
            return resultFieldList;
        }

        /// <summary>
        /// 从微信下载附件.
        /// 潘锦发 修改为oss上传 20160311
        /// </summary>
        /// <param name="url"></param>
        /// <param name="companyid"></param>
        /// <returns></returns>
        public static string Download(string url, int companyid)
        {
            string imageUrl = string.Empty;
            // url = http://ww2.sinaimg.cn/bmiddle/43a39d58gw1e87bhe0nevg208c04nx6s.gif
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream reader = response.GetResponseStream();
            string file = "";
            string uploadpath = OssHelper.GetServerPath("/", companyid.ToString(), OssHelper.ossTempFileUploadPath, "wx", false);
            string savepath = HttpContext.Current.Server.MapPath(uploadpath);
            if (!Directory.Exists(savepath))
                Directory.CreateDirectory(savepath);
            imageUrl = WebCommon.GetRndString(20) + ".jpg";
            file = savepath + imageUrl;
            imageUrl = OssHelper.CreateUploadPath(companyid.ToString(), imageUrl);
            //string thumfilepath = file.Replace(".jpg", "_wx.jpg");
            FileStream writer = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write);
            byte[] buff = new byte[512];
            int c = 0; //实际读取的字节数
            while ((c = reader.Read(buff, 0, buff.Length)) > 0)
            {
                writer.Write(buff, 0, c);
            }
            writer.Close();
            writer.Dispose();
            reader.Close();
            reader.Dispose();
            response.Close();
            //ImageUtil.MakeThumbnail(file, thumfilepath, 280, 160, CAS.Common.ImageUtil.ThumbnailCompressType.BaseOnProportion, "jpg");

            //上传文件到OSS
            OssHelper.UploadFile(imageUrl, companyid.ToString(), file, false);
            //OssHelper.UploadFile(imageUrl.Replace(".jpg", "_wx.jpg"), companyid.ToString(), thumfilepath, false);//缩略图
            return imageUrl;
        }
        /// <summary>
        /// 从微信下载附件.
        /// 潘锦发 修改为oss上传 20160311
        /// </summary>
        /// <param name="url"></param>
        /// <param name="companyid"></param>
        /// <param name="Virtualpath">是否返回虚拟路径</param>
        /// <returns></returns>
        public static string Download(string url, int companyid, bool Virtualpath)
        {
            string imageUrl = string.Empty;
            // url = http://ww2.sinaimg.cn/bmiddle/43a39d58gw1e87bhe0nevg208c04nx6s.gif
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream reader = response.GetResponseStream();
            string file = "";
            string uploadpath = OssHelper.GetServerPath("/", companyid.ToString(), OssHelper.ossTempFileUploadPath, "wx", false);
            string savepath = HttpContext.Current.Server.MapPath(uploadpath);
            if (!Directory.Exists(savepath))
                Directory.CreateDirectory(savepath);
            imageUrl = WebCommon.GetRndString(20) + ".jpg";
            file = savepath + imageUrl;
            imageUrl =  uploadpath  + imageUrl;
            FileStream writer = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write);
            byte[] buff = new byte[512];
            int c = 0; //实际读取的字节数
            while ((c = reader.Read(buff, 0, buff.Length)) > 0)
            {
                writer.Write(buff, 0, c);
            }
            writer.Close();
            writer.Dispose();
            reader.Close();
            reader.Dispose();
            response.Close();
            //上传文件到OSS
            OssHelper.UploadFile(imageUrl, companyid.ToString(), file, false);
            if (Virtualpath)
            {
                return file;
            }
            return imageUrl;
        }

        /// <summary>
        /// 写日志(用于跟踪)
        /// </summary>
        public static void WriteLog(string strMemo, string state)
        {
            string filename = HttpContext.Current.Server.MapPath("/logs/log.txt");
            if (!string.IsNullOrEmpty(state))
            {
                filename = HttpContext.Current.Server.MapPath("/logs/log" + state + ".txt");
            }
            StreamWriter sr = null;
            Directory.CreateDirectory(HttpContext.Current.Server.MapPath("/logs/"));
            try
            {
                if (!File.Exists(filename))
                {
                    sr = new StreamWriter(filename, true);
                    sr.WriteLine(strMemo);
                }
                else
                {
                    sr = new StreamWriter(filename, true);
                    sr.WriteLine(strMemo);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (sr != null)
                    sr.Close();
            }
        }

        /// <summary>
        ///  用Post的方式调用WcfApi
        /// </summary>
        /// <param name="url">WcfApi的url</param>
        /// <param name="posts">Post过去的参数</param>
        /// <param name="ContentType">content-Type标头</param>
        /// <returns></returns>
        public static string WcfApiMethodOfPost(string url, string posts)
        {
            return WcfApiMethodOfPost(url, posts, "application/json");
        }

        /// <summary>
        ///  用Post的方式调用WcfApi
        /// </summary>
        /// <param name="url">WcfApi的url</param>
        /// <param name="posts">Post过去的参数</param>
        /// <param name="ContentType">content-Type标头</param>
        /// <returns></returns>
        public static string WcfApiMethodOfPost(string url, string posts, string contentType)
        {
            try
            {
                string result = string.Empty;
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.ContentType = contentType;//"application/json";
                request.Method = "POST";
                MemoryStream memory = new MemoryStream();
                byte[] postdata = Encoding.GetEncoding("UTF-8").GetBytes(posts);
                request.ContentLength = postdata.Length;
                Stream newStream = request.GetRequestStream();
                newStream.Write(postdata, 0, postdata.Length);
                newStream.Close();

                HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                result = reader.ReadToEnd();
                reader.Close();
                myResponse.Close();
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, message: "错误url:" + (url ?? "null"));
                return "";
                // throw ex;
            }
        }


        /// <summary>
        /// 拼接安全接口所需参数 并返回数据连接字符串
        /// </summary>
        /// <param name="sinfo">sinfo</param>
        /// <param name="type">执行方法名</param>
        /// <param name="objinfo">JObject objinfo = JObject.Parse(info);</param>
        /// <returns></returns>
        public static string ApiSecurityStringOfJson(JObject objSinfo, string funtionname, JObject objinfo)
        {
            return ApiSecurityStringOfJson(objSinfo, funtionname, objinfo, "companysix");
        }

        /// <summary>
        /// 拼接安全接口所需参数；funname=none只验证功能权限
        /// </summary>
        /// <param name="sinfo">sinfo</param>
        /// <param name="type">执行方法名</param>
        /// <param name="objinfo">JObject objinfo = JObject.Parse(info);</param>
        /// <param name="funname">验证功能权限并调用用户中心功能；</param>
        /// <returns>{sinfo:{},info:{}}</returns>
        public static string ApiSecurityStringOfJson(JObject objSinfo, string funtionname, JObject objinfo, string funname)
        {
            var args = new
            {
                sinfo = new { appid = objSinfo.Value<string>("appid"), apppwd = objSinfo.Value<string>("apppwd"), signname = objSinfo.Value<string>("signname"), time = objSinfo.Value<string>("time"), code = objSinfo.Value<string>("code"), functionname = objSinfo.Value<string>("functionname"), funname = funname }.ToJson(),
                info = new
                {
                    appinfo = objinfo["appinfo"],
                    uinfo = objinfo["uinfo"],
                    funinfo = ""
                }.ToJson()
            };
            return args.ToJson();
        }


        /// <summary>
        /// 拼接安全接口所需参数；funname=none只验证功能权限
        /// </summary>
        /// <param name="sinfo">sinfo</param>
        /// <param name="objinfo">JObject objinfo = JObject.Parse(info);</param>
        /// <param name="funname">验证功能权限并调用用户中心功能；</param>
        /// <returns>{sinfo:{},info:{}}</returns>
        public static string ApiSecurityStringOfJson(JObject objSinfo, JObject objinfo, string funname, int companyId)
        {
            var args = new
            {
                sinfo = new { appid = objSinfo.Value<string>("appid"), apppwd = objSinfo.Value<string>("apppwd"), signname = objSinfo.Value<string>("signname"), time = objSinfo.Value<string>("time"), code = objSinfo.Value<string>("code"), functionname = objSinfo.Value<string>("functionname"), funname = funname }.ToJson(),
                info = new
                {
                    appinfo = objinfo["appinfo"],
                    uinfo = objinfo["uinfo"],
                    funinfo = new { companyid = companyId }
                }.ToJson()
            };
            return args.ToJson();
        }

        /// <summary>
        /// 拼接安全接口所需参数；funname=none只验证功能权限
        /// </summary>
        /// <param name="sinfo">sinfo</param>
        /// <param name="objinfo">JObject objinfo = JObject.Parse(info);</param>
        /// <param name="funname">验证功能权限并调用用户中心功能；</param>
        /// <returns>{sinfo:{},info:{}}</returns>
        public static string ApiSecurityStringOfJson(string appid, string apppwd, string signName, string time, string code, string functionName, string funname, string companyCode, int systypecode, string splatype)
        {
            var args = new
            {
                sinfo = new { appid = appid, apppwd = apppwd, signname = signName, time = time, code = code, functionname = functionName, funname = funname }.ToJson(),
                info = new
                {
                    appinfo = new { systypecode = systypecode, splatype = splatype },
                    uinfo = new { },
                    funinfo = new { companycode = companyCode }
                }.ToJson()
            };
            return args.ToJson();
        }

        /// <summary>
        /// 功能请求参数
        /// </summary>
        /// <param name="GJBWX_SInfoOfConfigKey">sinfo</param>
        /// <param name="objUinfo">用户信息：暂时未使用</param>
        /// <param name="objAppInfo">平台信息：暂时未使用</param>
        /// <param name="funinfo">功能参数信息</param>
        /// <returns></returns>
        public static string PostArgsForConfig(SInfoOfConfigKey model, Object objUinfo, Object objAppInfo, Object objFunInfo, Object appinfo)
        {
            string appid = model.appid,
                   apppwd = WebCommon.GetConfigSetting(model.apppwdKey),
                   signname = WebCommon.GetConfigSetting(model.signnameKey),
                   time = DateTime.Now.ToString("yyyyMMddHHmmss"),
                   appkey = WebCommon.GetConfigSetting(model.appKey);

            string[] pwdArray = { appid, apppwd, signname, time, model.functionName };
            string code = EncryptHelper.GetMd5(pwdArray, appkey);
            Object uinfo = new { username = "", token = "" };
            if (objUinfo != null)
            {
                uinfo = objUinfo;
            }

            var args = new
            {
                sinfo = new
                {
                    appid = appid,
                    apppwd = apppwd,
                    signname = signname,
                    time = time,
                    code = code,
                    functionname = model.functionName
                }.ToJson(),
                info = new
                {
                    appinfo = appinfo,
                    uinfo = uinfo,
                    funinfo = objFunInfo
                }.ToJson()
            };
            return args.ToJson();
        }

        /// <summary>
        /// 请求API，返回类型：JSONHelper.ReturnData
        /// </summary>
        /// <param name="url">api地址</param>
        /// <param name="posts">参数</param>
        /// <param name="check">检查参数</param>
        /// <param name="hastoken">是否需求token</param>
        /// <returns></returns>
        public static void RequestAPIForWinform(string url, string posts, bool check, bool hastoken, bool iswinform)
        {
            string result = string.Empty;
            JSONHelper.ReturnData data = new JSONHelper.ReturnData();
            if (hastoken)
            {
                string token = WebCommon.GetRndString(20);
                if (iswinform)
                {
                    FileHelper.SaveTokenForWinform(token, "suppliermanager");
                }
                else
                {
                    FileHelper.SaveToken(token, "suppliermanager");
                }
                if (string.IsNullOrEmpty(posts))
                {
                    posts = new { token = token }.ToJson();// string.Format("token={0}", token);
                }
                else
                {
                    posts = new { args = posts, token = token }.ToJson();
                    //posts += "&token=" + token;
                }
            }
            result = WebCommon.WcfApiMethodOfPost(url, posts, "application/json");

        }


        /// <summary>
        /// 请求API，返回类型：JSONHelper.ReturnData
        /// </summary>
        /// <param name="url">api地址</param>
        /// <param name="posts">参数</param>
        /// <param name="check">检查参数</param>
        /// <param name="hastoken">是否需求token</param>
        /// <param name="iswinform">是否winform调用。</param>
        /// <returns></returns>
        public static JSONHelper.ReturnData RequestAPIForReturnData(string url, string posts, bool check, bool hastoken, bool iswinform)
        {
            string result = string.Empty;
            JSONHelper.ReturnData data = new JSONHelper.ReturnData();
            if (hastoken)
            {
                string token = WebCommon.GetRndString(20);
                if (iswinform)
                {
                    FileHelper.SaveTokenForWinform(token, "suppliermanager");
                }
                else
                {
                    FileHelper.SaveToken(token, "suppliermanager");
                }

                if (string.IsNullOrEmpty(posts))
                {
                    posts = string.Format("token={0}", token);
                }
                else
                {
                    posts += "&token=" + token;
                }
            }
            result = WebCommon.APIPostBack(url, posts, check);
            data = JSONHelper.JSONToObject<JSONHelper.ReturnData>(result);
            return data;
        }

        /// <summary>
        /// 根据 User Agent 获取操作系统名称
        /// </summary>
        public static string GetOSNameByUserAgent(string userAgent)
        {
            string osVersion = "未知";
            userAgent = userAgent.ToLower();
            if (userAgent.Contains("nt 6.2"))
            {
                osVersion = "Windows 8/Server 2012";
            }
            else if (userAgent.Contains("nt 6.1"))
            {
                osVersion = "Windows 7/Server 2008 R2";
            }
            else if (userAgent.Contains("nt 6.0"))
            {
                osVersion = "Windows Vista/Server 2008";
            }
            else if (userAgent.Contains("nt 5.2"))
            {
                osVersion = "Windows Server 2003";
            }
            else if (userAgent.Contains("nt 5.1"))
            {
                osVersion = "Windows XP";
            }
            else if (userAgent.Contains("nt 5"))
            {
                osVersion = "Windows 2000";
            }
            else if (userAgent.Contains("nt 4"))
            {
                osVersion = "Windows NT4";
            }
            else if (userAgent.Contains("me"))
            {
                osVersion = "Windows Me";
            }
            else if (userAgent.Contains("98"))
            {
                osVersion = "Windows 98";
            }
            else if (userAgent.Contains("95"))
            {
                osVersion = "Windows 95";
            }
            else if (userAgent.Contains("ipad"))
            {
                osVersion = "iPad";
            }
            else if (userAgent.Contains("iphone"))
            {
                osVersion = "iphone";
            }
            else if (userAgent.Contains("mac"))
            {
                osVersion = "Mac";
            }
            else if (userAgent.Contains("android"))
            {
                osVersion = "Android";
            }
            else if (userAgent.Contains("unix"))
            {
                osVersion = "UNIX";
            }
            else if (userAgent.Contains("linux"))
            {
                osVersion = "Linux";
            }
            else if (userAgent.Contains("sunos"))
            {
                osVersion = "SunOS";
            }
            return osVersion;
        }
    }


    /// <summary>
    /// 消息主体
    /// </summary>
    public class MessageBody
    {
        public string url { get; set; }
        public string userid { get; set; }
        public string touserid { get; set; }
        public int cityid { get; set; }
        public int msgid { get; set; }
        public int tosystypecode { get; set; }
        public int fxtcompanyid { get; set; }
        public int companyid { get; set; }
        public int messagetype { get; set; }
    }
}
