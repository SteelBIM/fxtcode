using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Common.Common;
using FXT.DataCenter.Infrastructure.Common.CommonWeb;
using FXT.DataCenter.Infrastructure.Common.Dictionary;
using FXT.DataCenter.WebUI.Infrastructure.WebSecurity;
using Webdiyer.WebControls.Mvc;
using FXT.DataCenter.Domain.Models;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Text;
using System.Net;

namespace FXT.DataCenter.WebUI.Areas.Authorize.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        readonly int _productTypeCode = ((int)EnumHelper.Codes.SysTypeCodeDataCenter);
        private readonly IUser _user;
        private readonly IRole _role;
        private readonly ILog _log;
        private readonly ICompanyProduct_Module _module;

        public UserController(IUser user, IRole role, ILog log, ICompanyProduct_Module module)
        {
            _user = user;
            _role = role;
            this._log = log;
            this._module = module;
        }

        public ActionResult Index(string UserName, int? pageIndex)
        {
            //var result = _user.GetUsers(Passport.Current.FxtCompanyId, UserName);
            ViewBag.authorizeCityIDs =  Passport.Current.AccessedCities.Select(o => o.cityid).Distinct().ToList();
            var re = FxtUserCenterService_GetUsers(Passport.Current.FxtCompanyId, UserName, _productTypeCode);
            var role = re.AsQueryable().ToPagedList(pageIndex ?? 1, 30);
            return View(role);
        }

        //编辑
        [HttpGet]
        public ActionResult Edit(string userName)
        {
            var allcities = _module.GetCities().ToList();
            var allCompanyRoles = _role.GetRoles(Passport.Current.FxtCompanyId);
            //var companyCityMap = FxtUserCenterService_GetCityRolesByUserName(Passport.Current.FxtCompanyId, Passport.Current.ProductTypeCode, userName, _productTypeCode);
            //var cs = companyCityMap.Select(t => t.CityID).Distinct().ToList();
            var cs = FxtUserCenterService_GetCityRolesByUserName(Passport.Current.FxtCompanyId, Passport.Current.ProductTypeCode, userName, _productTypeCode).Select(t => t.CityID).Distinct().ToList();
            List<SYS_City> authorizeCities = new List<SYS_City>();
            if (cs.Count == 1 && cs[0] == 0)
            {
                authorizeCities = allcities;
            }
            else
            {
                authorizeCities = _module.GetProvinceNames(cs).ToList();
            }
            //var authorizeCities = cs.Select(o =>
            //{
            //    var province = _module.GetProvinceName(o);
            //    return new SYS_City { CityId = o, CityName = province.CityName, ProvinceId = province.ProvinceId, ProvinceName = province.ProvinceName };
            //}).ToList();
            var pclist = (from p in authorizeCities
                group p by new { p.ProvinceId, p.ProvinceName } into g
                select new ProvinceCityList
                {
                    ProvinceId = g.Key.ProvinceId,
                    ProvinceName = g.Key.ProvinceName,
                    CityList = (from h in g
                                select new PCityList
                                {
                                    CityID = h.CityId,
                                    CityName = h.CityName
                                }).ToList(),
                }).ToList();
            List<SYS_Role_User> userRoleMap = _user.GetCityRolesByUserName(userName, Passport.Current.FxtCompanyId).ToList();
            if (userRoleMap.Select(r => r.CityID).Contains(0))
            {
                var allCityData = userRoleMap.Where(o => o.CityID == 0).Select(o => o.RoleID);
                userRoleMap = userRoleMap.Where(o=>!allCityData.Contains(o.RoleID)).ToList();
                userRoleMap.AddRange(allCityData.Distinct().Select(o=>{
                    var rolename = allCompanyRoles.FirstOrDefault(oi => oi.ID == o);
                    return new SYS_Role_User { CityID = 0, CityName = "不限", RoleID = o, RoleName = rolename == null ? "" :rolename.RoleName };
                }));
            }
            else
            {
                userRoleMap = userRoleMap.Where(o => cs.Contains(o.CityID)).ToList();
            }
            userRoleMap = userRoleMap.Where(o => !string.IsNullOrEmpty(o.RoleName)).ToList();
            var datalist = (
                from p in userRoleMap
                group p by new { p.CityID, p.CityName } into g
                select new
                {
                    CityID = g.Key.CityID,
                    CityName = g.Key.CityName,
                    RoleIDs = String.Join(",", g.Select(m => m.RoleID).ToArray()),
                    RoleNames = String.Join(",", g.Select(m => m.RoleName).ToArray()),
                }).ToList();
            var baseRoles = datalist.FirstOrDefault(o => o.CityID == 0);
            if (baseRoles!=null&& !string.IsNullOrEmpty(baseRoles.RoleNames))
            {
                datalist = datalist.Select(o =>
                {
                    return new
                    {
                        CityID = o.CityID,
                        CityName = o.CityName,
                        RoleIDs = o.CityID != 0 ? string.Format("{0},{1}", baseRoles.RoleIDs, o.RoleIDs) : o.RoleIDs,
                        RoleNames = o.CityID != 0 ? string.Format("{0},{1}", baseRoles.RoleNames, o.RoleNames) : o.RoleNames
                    };
                }).ToList();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var str = serializer.Serialize(datalist);
            this.ViewBag.roles = allCompanyRoles;
            this.ViewBag.pclist = pclist;
            this.ViewBag.data = str;
            this.ViewBag.UserName = userName;
            return View();
        }

        //编辑保存
        [HttpPost]
        public ActionResult Edit(string UserName, string dataresult)
        {
            try
            {
                _user.DeleteUserRoles(Passport.Current.FxtCompanyId, UserName);
                if (!string.IsNullOrEmpty(dataresult))
                {
                    var data = dataresult.Trim(';').Split(';');
                    List<CityRole> cr = new List<CityRole>();
                    foreach (string result in data)
                    {
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        CityRole r = js.Deserialize<CityRole>(result);
                        if (!string.IsNullOrWhiteSpace(r.RoleIDs))
                        {
                            cr.Add(r);
                        }
                    }
                    for (int i = 0; i < cr.Count; i++)
                    {
                        List<string> roles = cr[i].RoleIDs.Split(',').ToList();
                        List<SYS_Role_User> sru = new List<SYS_Role_User>();
                        roles.ForEach(m => sru.Add(new SYS_Role_User
                        {
                            FxtCompanyID = Passport.Current.FxtCompanyId,
                            CityID = cr[i].CityID,
                            UserName = UserName,
                            RoleID = int.Parse(m)
                        }));
                        _user.AddUserRoles(sru);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Authorize/User/Edit", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
            }
            return this.RefreshParent();
        }

        public class ProvinceCityList
        {
            public int ProvinceId { get; set; }
            public string ProvinceName { get; set; }
            public List<PCityList> CityList { get; set; }
        }
        public class PCityList
        {
            public int CityID { get; set; }
            public string CityName { get; set; }
        }

        public class CityRole
        {
            public int CityID { get; set; }
            public string RoleIDs { get; set; }
        }


        #region 用户中心API
        private List<SYS_User> FxtUserCenterService_GetUsers(int fxtcompanyid, string username, int procode)
        {
            string api = ConfigurationManager.AppSettings["fxtusercenterservice"];
            string appid = ConfigurationManager.AppSettings["usercenterserviceappid"];
            string apppwd = ConfigurationManager.AppSettings["usercenterserviceapppwd"];
            string appkey = ConfigurationManager.AppSettings["usercenterserviceappkey"];
            string signname = ConfigurationManager.AppSettings["signname"];
            string time = DateTime.Now.ToString("yyyyMMddHHmmss");
            string functionname = "usertwelve";

            string[] pwdArray = { appid, apppwd, signname, time, functionname };
            string code = EncryptHelper.GetMd5(pwdArray, appkey);

            List<SYS_User> users = null;
            try
            {
                if (!string.IsNullOrEmpty(api))
                {
                    var sinfo = new { appid = appid, apppwd = apppwd, signname = signname, time = time, code = code, functionname = functionname };
                    var info = new
                    {
                        uinfo = new { username, token = "" },
                        appinfo = new ApplicationInfo(procode.ToString()),
                        funinfo = new { companyid = fxtcompanyid, username = username, search = new { page = false, pageIndex = 1, pageRecords = int.MaxValue } }
                    };
                    var post = "{\"sinfo\":\"" + JSONHelper.ObjectToJSON(sinfo).Replace("\"", "'") + "\",\"info\":\"" + JSONHelper.ObjectToJSON(info).Replace("\"", "'") + "\"}";
                    var str = APIPostBack(api, post, "application/json");
                    var rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                    if (rtn.returntype > 0)
                    {
                        users = JSONHelper.JSONToObject<SYS_User[]>(rtn.data.ToString()).ToList();
                        return users;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("User/FxtUserCenterService_GetUsers", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
            }
            return users;
        }

        /// <summary>
        /// 公司开通产品模块城市ID
        /// </summary>
        /// <param name="fxtcompanyid"></param>
        /// <param name="producttypecode"></param>
        /// <param name="username"></param>
        /// <param name="procode"></param>
        /// <returns></returns>
        private List<SYS_Role_User> FxtUserCenterService_GetCityRolesByUserName(int fxtcompanyid, int producttypecode, string username, int procode)
        {
            string api = ConfigurationManager.AppSettings["fxtusercenterservice"];
            string appid = ConfigurationManager.AppSettings["usercenterserviceappid"];
            string apppwd = ConfigurationManager.AppSettings["usercenterserviceapppwd"];
            string appkey = ConfigurationManager.AppSettings["usercenterserviceappkey"];
            string signname = ConfigurationManager.AppSettings["signname"];
            string time = DateTime.Now.ToString("yyyyMMddHHmmss");
            string functionname = "cpmthree";

            string[] pwdArray = { appid, apppwd, signname, time, functionname };
            string code = EncryptHelper.GetMd5(pwdArray, appkey);

            List<SYS_Role_User> users = null;

            try
            {
                if (!string.IsNullOrEmpty(api))
                {
                    var sinfo = new { appid = appid, apppwd = apppwd, signname = signname, time = time, code = code, functionname = functionname };
                    var info = new
                    {
                        uinfo = new { username, token = "" },
                        appinfo = new ApplicationInfo(procode.ToString()),
                        funinfo = new { companyid = fxtcompanyid, producttypecode = producttypecode, search = new { Page = false, PageIndex = 1, PageRecords = int.MaxValue } }
                    };
                    var post = "{\"sinfo\":\"" + JSONHelper.ObjectToJSON(sinfo).Replace("\"", "'") + "\",\"info\":\"" + JSONHelper.ObjectToJSON(info).Replace("\"", "'") + "\"}";
                    var str = APIPostBack(api, post, "application/json");
                    var rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                    if (rtn.returntype > 0)
                    {
                        users = JSONHelper.JSONToObject<SYS_Role_User[]>(rtn.data.ToString()).ToList();
                        return users;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("User/FxtUserCenterService_GetCityRolesByUserName", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
            }
            return users;
        }

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
        #endregion

    }
}
