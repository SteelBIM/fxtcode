using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Text;
using System.Configuration;
using System.Web.Security;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Common.Common;
using FXT.DataCenter.Infrastructure.Common.CommonWeb;
using FXT.DataCenter.Infrastructure.Redis;
using FXT.DataCenter.WebUI.Infrastructure.WebSecurity;

namespace FXT.DataCenter.WebUI.Controllers
{

    public class LoginController : Controller
    {
        //产品CODE
        private readonly ICompanyProduct_Module _module;
        private readonly IUser _user;
        private readonly ISYS_Login _login;
        public LoginController(ICompanyProduct_Module module, ISYS_Login login, IUser user)
        {
            this._module = module;
            this._login = login;
            this._user = user;
        }

        readonly int _productTypeCode = ((int)EnumHelper.Codes.SysTypeCodeDataCenter);
        Exception _msg;
        List<Apps> _apps = new List<Apps>();

        [HttpGet]
        public ActionResult Index()
        {
            if (Passport.Current.IsAuthenticated)
            {
                return RedirectToAction("Search/Welcome", "Search");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(string username, string password, bool remember)
        {
            //用户中心服务获取用户信息
            return UserLogin(username, password, remember);
        }

        /// <summary>
        /// 登录辅助方法
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">用户密码</param>
        /// <param name="remember">是否自动登录</param>
        /// <returns></returns>
        private ActionResult UserLogin2(string userName, string password, bool remember)
        {
            var userInfo = FxtUserCenterService_GetUser(userName, out _msg, password, ref _apps, _productTypeCode);
            var userDefaultcity = "Defaultcity_" + userName;//账号默认城市
            if (_msg != null)
            {
                ModelState.AddModelError("error", "登录失败," + _msg.Message);
                return View();
            }
            var isOuterUser = (userInfo.isinner != 1);
            if (isOuterUser)
            {
                ModelState.AddModelError("error", "外部账号禁止登录数据中心！");
                return View();
            }

            if (userInfo != null)
            {
                //获取公司的模块
                var cpm = FxtUserCenterService_GetCPM(userInfo.companyid, _productTypeCode, _productTypeCode);
                //var cpm = _module.FxtUserCenterService_GetCPM(userInfo.companyid, _productTypeCode);
                var usercitylist = _module.GetAccessedModules(userInfo.companyid.ToString(), userName);
                var modules = usercitylist.Contains(0) ? cpm : cpm.Where(r => usercitylist.Contains(r.cityid));
                Dictionary<string, string> modulesNameList = _module.GetCodeNames(modules.Select(o=>o.modulecode).Distinct());
                foreach (var module in modules)
                {
                    if (modulesNameList.Keys.Contains(module.modulecode.ToString()))
                    {
                        module.CodeName = modulesNameList[module.modulecode.ToString()];
                    }
                }

                var cities = new List<CompanyProduct_Module>();
                var cs = modules.Select(t => t.cityid).Distinct().ToList();
                Dictionary<string, string> cityNameList = _module.GetCityNames(cs);
                cities = cityNameList.Select(o => { return new CompanyProduct_Module { cityid = int.Parse(o.Key), CityName = o.Value }; }).ToList();

                //var modules2 = _module.GetAccessedModules(userInfo.companyid.ToString(), _productTypeCode.ToString(), userName);
                var firstModules = modules.Where(m => m.parentmodulecode == 0).OrderBy(n => n.createdate);
                //var cities2 = _module.GetAccessedCities(userInfo.companyid.ToString(), _productTypeCode.ToString(), userName);
                System.Threading.Tasks.Task<bool> redisTask = new System.Threading.Tasks.Task<bool>(() =>
                {
                    return RedisOperated(userInfo, modules, cities, firstModules);
                });
                redisTask.Start();

                var cityId = 0;
                if (Request.Cookies[userDefaultcity] != null && !string.IsNullOrEmpty(Request.Cookies[userDefaultcity].Value))
                {
                    int.TryParse(Request.Cookies[userDefaultcity].Value, out cityId);
                }
                if (cityId == 0)
                {
                    var syslogin = _login.GetSys_Login(userName, userInfo.companyid);
                    if (syslogin == null || cities.ToList().Where(m => m.cityid == syslogin.CityId).ToList().Count == 0)
                    {
                        if (modules != null && modules.Any())
                        {
                            var firstOrDefault = modules.FirstOrDefault();
                            if (firstOrDefault != null) cityId = firstOrDefault.cityid;
                        }
                    }
                    else
                    {
                        cityId = syslogin.CityId;
                    }
                }

                var companyId = userInfo.companyid;
                var trueName = userInfo.truename;
                var pasCode = RequestHelper.GetOnlyCode();
                var cityName = cityNameList.Keys.Contains(cityId.ToString()) ? cityNameList[cityId.ToString()]:"";
                var userData = userName + "|" + cityId + "|" + companyId + "|" + _productTypeCode + "|" + pasCode + "|" + trueName + "|" + cityName;

                var ipAddress = RequestHelper.GetIP();
                var browserType = RequestHelper.GetClientBrowserVersions();

                System.Threading.Tasks.Task logTask = new System.Threading.Tasks.Task(() =>
                {
                    _login.AddSYS_Login(userName, userInfo.companyid, cityId, _productTypeCode, pasCode, ipAddress, browserType);
                });
                logTask.Start();

                //用户信息写入cookies
                FormsAuthentication.SetAuthCookie(userData, remember);
                var cookie = new HttpCookie("remember", remember ? "true" : "false");
                Response.Cookies.Add(cookie);
                Response.Cookies.Add(new HttpCookie(userDefaultcity, cityId.ToString()));//设置默认城市id

                System.Threading.Tasks.Task.WaitAll(logTask, redisTask);
                if (!redisTask.Result)
                {
                    ModelState.AddModelError("error", "写入缓存失败");
                }
                //登录日志
                //_login.AddSYS_Login(userName, userInfo.companyid, cityId, _productTypeCode, pasCode);
                //页面转向
                return RedirectToAction("Search/Welcome", "Search");
            }

            ModelState.AddModelError("error", "登录失败,用户名不存在");
            return View();
        }


        private ActionResult UserLogin(string userName, string password, bool remember)
        {
            var cityId = 6;
            var userDefaultcity = "userDefaultcity";
            var companyid = 25;
            var trueName = "张三";
            var pasCode = RequestHelper.GetOnlyCode();
            var cityName = "深圳市";
            var userData = userName + "|" + cityId + "|" + companyid + "|" + _productTypeCode + "|" + pasCode + "|" + trueName + "|" + cityName;

            var ipAddress = RequestHelper.GetIP();
            var browserType = RequestHelper.GetClientBrowserVersions();

            System.Threading.Tasks.Task logTask = new System.Threading.Tasks.Task(() =>
            {
                _login.AddSYS_Login(userName, companyid, cityId, _productTypeCode, pasCode, ipAddress, browserType);
            });
            logTask.Start();

            //用户信息写入cookies
            FormsAuthentication.SetAuthCookie(userData, remember);
            var cookie = new HttpCookie("remember", remember ? "true" : "false");
            Response.Cookies.Add(cookie);
            Response.Cookies.Add(new HttpCookie(userDefaultcity, cityId.ToString()));//设置默认城市id
            
            //登录日志
            //_login.AddSYS_Login(userName, userInfo.companyid, cityId, _productTypeCode, pasCode);
            //页面转向
            return RedirectToAction("Search/Welcome", "Search");

        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LoginOut()
        {
            //更新退出信息
            _login.UpdateSYS_Login(Passport.Current.LoginPasCode, Passport.Current.CityId);

            //清除cookies
            FormsAuthentication.SignOut();

            return RedirectToAction("Index");


        }
        /// <summary>
        /// 城市切换
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult CityReplace()
        {
            return View();
        }
        /// <summary>
        /// 城市切换
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ActionResult CityReplace(string cityId)
        {
            var cookie = System.Web.HttpContext.Current.Request.Cookies["remember"];
            var cityName = _module.GetCityName(Convert.ToInt32(cityId));
            var userDefaultcity = "Defaultcity_" + Passport.Current.UserName;//账号默认城市

            var userData = Passport.Current.ID + "|" + cityId + "|" + Passport.Current.FxtCompanyId + "|" + Passport.Current.ProductTypeCode + "|" + Passport.Current.LoginPasCode + "|" + Passport.Current.TrueName + "|" + cityName;
            FormsAuthentication.SetAuthCookie(userData, cookie != null && cookie.Value == "true");
            Response.Cookies.Add(new HttpCookie(userDefaultcity, cityId));//记录默认城市id

            return Json(new { flag = true });
        }
        /// <summary>
        /// 城市切换
        /// 城市搜索
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ActionResult CitySeach(string cityName = null)
        {
            var list = Passport.Current.AccessedCities;

            return Json(string.IsNullOrEmpty(cityName) ? list : (list.Where(m => m.CityName.Contains(cityName)).OrderBy(m => m.cityid)).ToList());

            #region 废弃代码

            //var query = list.Where(n => n.companyid == Passport.Current.FxtCompanyId).Select(m => new { ProvinceName = m.ProvinceName, m.CityName, m.cityid });

            //if (!string.IsNullOrEmpty(cityName))
            //{
            //    var result = from n in query
            //                 where n.CityName.IndexOf(cityName.Trim()) != -1
            //                 group n by new { n.cityid, n.CityName, n.ProvinceName } into g
            //                 select new
            //                 {
            //                     g.Key.cityid,
            //                     g.Key.CityName,
            //                     g.Key.ProvinceName
            //                 };
            //    return Json(result.ToList());
            //}
            //else
            //{
            //    var result = from n in query
            //                 group n by new { n.cityid, n.CityName, n.ProvinceName } into g
            //                 select new
            //                 {
            //                     g.Key.cityid,
            //                     g.Key.CityName,
            //                     g.Key.ProvinceName
            //                 };
            //    return Json(result.ToList());
            //}

            #endregion

        }

        #region 登录辅助方法
        /// <summary>
        /// 中心服务器检查用户的接口
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="msg">异常消息</param>
        /// <param name="password">密码</param>
        /// <param name="apps">app</param>
        /// <param name="procode">产品Code</param>
        /// <returns>返回UserCheck</returns>
        private UserCheck FxtUserCenterService_GetUser(string username, out Exception msg, string password, ref List<Apps> apps, int procode)
        {
            //web.config设置中心用户API的地址
            string api = ConfigurationManager.AppSettings["fxtusercenterloginservice"];
            UserCheck usercheck = null;
            msg = null;
            try
            {
                if (!string.IsNullOrEmpty(api))
                {
                    var time = DateTime.Now.ToString("yyyyMMddHHmmss");
                    var sinfo = new { time, code = StringHelper.GetLoginCodeMd5(time) };
                    var info = new
                    {
                        uinfo = new { username, password = StringHelper.GetPassWordMd5(password) },
                        appinfo = new ApplicationInfo(procode.ToString())
                    };
                    var post = "{\"sinfo\":\"" + JSONHelper.ObjectToJSON(sinfo).Replace("\"", "'") + "\",\"info\":\"" + JSONHelper.ObjectToJSON(info).Replace("\"", "'") + "\"}";
                    var str = APIPostBack(api, post, "application/json");
                    var rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                    if (rtn.returntype > 0)
                    {
                        usercheck = new UserCheck();
                        var returnSInfo = JSONHelper.JSONToObject<dynamic>(rtn.data.ToString())["sinfo"];
                        string appsString = JSONHelper.ObjectToJSON(returnSInfo["apps"]);

                        apps = JSONHelper.JSONToObject<List<Apps>>(appsString);
                        string signName = Convert.ToString(returnSInfo["signname"]);
                        string businessDb = Convert.ToString(returnSInfo["businessdb"]);
                        var returnUInfo = JSONHelper.JSONToObject<dynamic>(rtn.data.ToString())["uinfo"];
                        string trueName = Convert.ToString(returnUInfo["truename"]);
                        string companyname = Convert.ToString(returnUInfo["companyname"]);
                        int fxtcompanyid = Convert.ToInt32(returnUInfo["fxtcompanyid"]);
                        int isinner = Convert.ToInt32(returnUInfo["isinner"]);

                        usercheck.signname = signName;
                        usercheck.truename = trueName;
                        usercheck.companyid = fxtcompanyid;
                        usercheck.businessdb = businessDb;
                        usercheck.companyname = companyname;
                        usercheck.isinner = isinner;

                        return usercheck;
                    }
                    msg = new Exception(rtn.returntext.ToString());
                }
            }
            catch (Exception ex)
            {
                msg = ex;
            }
            return usercheck;
        }

        private List<CompanyProduct_Module> FxtUserCenterService_GetCPM(int fxtcompanyid, int producttypecode, int procode)
        {
            string api = ConfigurationManager.AppSettings["fxtusercenterservice"];
            string appid = ConfigurationManager.AppSettings["usercenterserviceappid"];
            string apppwd = ConfigurationManager.AppSettings["usercenterserviceapppwd"];
            string appkey = ConfigurationManager.AppSettings["usercenterserviceappkey"];
            string signname = ConfigurationManager.AppSettings["signname"];
            string time = DateTime.Now.ToString("yyyyMMddHHmmss");
            string functionname = "cpmfour";

            string[] pwdArray = { appid, apppwd, signname, time, functionname };
            string code = EncryptHelper.GetMd5(pwdArray, appkey);

            List<CompanyProduct_Module> cpm = null;
            try
            {
                if (!string.IsNullOrEmpty(api))
                {
                    var sinfo = new { appid = appid, apppwd = apppwd, signname = signname, time = time, code = code, functionname = functionname };
                    var info = new
                    {
                        uinfo = new { token = "" },
                        appinfo = new ApplicationInfo(procode.ToString()),
                        funinfo = new { 
                            companyid = fxtcompanyid, 
                            producttypecode = producttypecode, 
                            search = new { Page = false, PageIndex = 1, PageRecords = int.MaxValue } 
                        }
                    };
                    var post = "{\"sinfo\":\"" + JSONHelper.ObjectToJSON(sinfo).Replace("\"", "'") + "\",\"info\":\"" + JSONHelper.ObjectToJSON(info).Replace("\"", "'") + "\"}";
                    var str = APIPostBack(api, post, "application/json");
                    var rtn = JSONHelper.JSONToObjectTrust<JSONHelper.ReturnData>(str);
                    if (rtn.returntype > 0)
                    {

                        cpm = new List<CompanyProduct_Module>();
                        cpm = JSONHelper.JSONToObjectTrust<CompanyProduct_Module[]>(rtn.data.ToString()).ToList();
                        return cpm;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("User/FxtUserCenterService_GetCPM", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
            }
            return cpm;
        }
        /// <summary>
        /// 用户中心数据提交回调
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="post">提交数据(json)</param>
        /// <param name="contentType">类型</param>
        /// <returns></returns>
        private string APIPostBack(string url, string post, string contentType)
        {
            byte[] postData = Encoding.UTF8.GetBytes(post);
            var client = new WebClient();
            client.Headers.Add("Content-Type", contentType);
            client.Headers.Add("ContentLength", postData.Length.ToString());
            //这里url要组装安全标记等参数
            var result = "";
            try
            {
                byte[] responseData = client.UploadData(url, "POST", postData);
                result = Encoding.UTF8.GetString(responseData);
                //找退出原因
                //LogHelper.Info(result);
            }
            catch (Exception ex)
            {
                result = JSONHelper.GetJson(null, 0, ex.Message, ex);
            }
            client.Dispose();
            return result;
        }

        /// <summary>
        /// 登录redis操作
        /// </summary>
        /// <param name="userinfo"></param>
        /// <param name="modules"></param>
        /// <param name="cities"></param>
        /// <param name="firstModules"></param>
        /// <returns></returns>
        public bool RedisOperated(UserCheck userinfo, IEnumerable<CompanyProduct_Module> modules, IEnumerable<CompanyProduct_Module> cities, IEnumerable<CompanyProduct_Module> firstModules)
        {
            try
            {
                var con = RedisConnection.Connection;
                var database = con.GetDatabase();

                var key1 = "Share:fxtdcModules:" + userinfo.companyid; //开通的所有产品模块
                var key2 = "Share:fxtdcCities:" + userinfo.companyid; //开通的所有城市
                var key3 = "Share:fxtdcFirstModules:" + userinfo.companyid;//开通的产品一级模块

                database.Set(key1, modules.ToList());
                database.Set(key2, cities.ToList());
                database.Set(key3, firstModules.ToList());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        public ActionResult Unanthroize()
        {
            //更新退出信息
            _login.UpdateSYS_Login(Passport.Current.LoginPasCode, Passport.Current.CityId);

            //清除cookies
            FormsAuthentication.SignOut();

            return View();
        }
    }
}


