using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Data.ServicesImpl;
using FXT.DataCenter.Infrastructure.Redis;
using FXT.DataCenter.Infrastructure.Common.Common;
using System.Configuration;
using System.Text;
using System.Net;

namespace FXT.DataCenter.WebUI.Infrastructure.WebSecurity
{
    /// <summary>
    /// 登录凭据
    /// </summary>
    public class Passport
    {

        public static Passport Current
        {
            get
            {
                return new Passport();
            }
        }
        /// <summary>
        /// 当前用户是否已登录
        /// </summary>
        public bool IsAuthenticated { get { return HttpContext.Current.User.Identity.IsAuthenticated; } }

        public string ID
        {
            get
            {
                if (IsAuthenticated)
                {
                    return HttpContext.Current.User.Identity.Name.Split('|')[0];
                }
                return string.Empty;
            }
        }

        public string TrueName
        {
            get
            {
                if (IsAuthenticated)
                {
                    return HttpContext.Current.User.Identity.Name.Split('|')[5];
                }
                return string.Empty;
            }
        }

        public ICollection<SYS_Role> Roles
        {
            get
            {
                if (IsAuthenticated)
                {
                    IUser _user = new User();
                    return _user.GetRolesByUserName(UserName, FxtCompanyId, CityId).ToList();
                }
                return new List<SYS_Role>();
            }
        }

        public ICollection<SYS_Menu> Menus
        {
            get
            {
                if (IsAuthenticated)
                {
                    IUser user = new User();
                    return user.GetMenusByUserName(UserName, FxtCompanyId, CityId).ToList();
                }
                return new List<SYS_Menu>();
            }
        }

        public int FxtCompanyId
        {
            get
            {
                if (IsAuthenticated)
                {
                    return int.Parse(HttpContext.Current.User.Identity.Name.Split('|')[2]);
                }

                return 0;
            }
        }

        public int CityId
        {
            get
            {
                if (IsAuthenticated)
                {
                    return int.Parse(HttpContext.Current.User.Identity.Name.Split('|')[1]);
                }

                return 0;
            }
        }

        public int ProductTypeCode
        {
            get
            {
                if (IsAuthenticated)
                {
                    return int.Parse(HttpContext.Current.User.Identity.Name.Split('|')[3]);
                }

                return 0;
            }
        }

        public IList<CompanyProduct_Module> AccessedModules
        {
            get
            {
                if (!IsAuthenticated)
                {
                    return new List<CompanyProduct_Module>();
                }

                var con = RedisConnection.Connection;
                var database = con.GetDatabase();

                var key = "Share:fxtdcModules:" + FxtCompanyId;
                var modules = database.Get<List<CompanyProduct_Module>>(key);

                if (modules == null)
                {
                    modules = new List<CompanyProduct_Module>();
                    //var data = new CompanyProduct_ModuleDAL().GetAccessedModules(this.FxtCompanyId.ToString(), this.ProductTypeCode.ToString(), this.UserName);
                    //database.Set(key, data.ToList());
                    //modules = database.Get<List<CompanyProduct_Module>>(key);
                }

                return modules;
            }
        }

        /// <summary>
        /// 产品开通的一级模块
        /// </summary>
        public IList<CompanyProduct_Module> AccessedFirstModules
        {
            get
            {
                if (!IsAuthenticated)
                {
                    return new List<CompanyProduct_Module>();
                }

                var con = RedisConnection.Connection;
                var database = con.GetDatabase();

                var key = "Share:fxtdcFirstModules:" + FxtCompanyId;
                var modules = database.Get<List<CompanyProduct_Module>>(key);

                if (modules == null)
                {
                    modules = new List<CompanyProduct_Module>();
                    //var data = new CompanyProduct_ModuleDAL().GetAccessedModules(this.FxtCompanyId.ToString(), this.ProductTypeCode.ToString(), this.UserName).Where(m => m.parentmodulecode == 0);
                    //database.Set(key, data.ToList());
                    //modules = database.Get<List<CompanyProduct_Module>>(key);
                }

                return modules;
            }
        }

        /// <summary>
        /// 产品开通城市
        /// </summary>
        public IList<CompanyProduct_Module> AccessedCities
        {
            get
            {
                if (!IsAuthenticated)
                {
                    return new List<CompanyProduct_Module>();
                }

                var con = RedisConnection.Connection;
                var database = con.GetDatabase();

                var key = "Share:fxtdcCities:" + FxtCompanyId;
                var cities = database.Get<List<CompanyProduct_Module>>(key);

                if (cities == null)
                {
                    cities = new List<CompanyProduct_Module>();
                    //var data = new CompanyProduct_ModuleDAL().GetAccessedCities(this.FxtCompanyId.ToString(), this.ProductTypeCode.ToString(), this.UserName);
                    //database.Set(key, data.ToList());
                    //cities = database.Get<List<CompanyProduct_Module>>(key);
                }

                return cities;
            }
        }


        /// <summary>
        /// 登录唯一标识符
        /// </summary>
        public string LoginPasCode
        {
            get
            {
                if (IsAuthenticated)
                {
                    return HttpContext.Current.User.Identity.Name.Split('|')[4];
                }
                return string.Empty;
            }
        }
        /// <summary>
        /// 用户ID
        /// admin@fxt.com
        /// </summary>
        public string UserName
        {
            get
            {
                if (IsAuthenticated)
                {
                    return HttpContext.Current.User.Identity.Name.Split('|')[0];
                }

                return string.Empty;
            }
        }
        /// <summary>
        /// 用户ID
        /// admin@fxt.com
        /// </summary>
        public string CityName
        {
            get
            {
                if (IsAuthenticated)
                {
                    return HttpContext.Current.User.Identity.Name.Split('|')[6];
                }

                return string.Empty;
            }
        }


        //private List<CompanyProduct_Module> FxtUserCenterService_GetCPM(int fxtcompanyid, int producttypecode, int procode)
        //{
        //    string api = ConfigurationManager.AppSettings["fxtusercenterservice"];
        //    string appid = ConfigurationManager.AppSettings["usercenterserviceappid"];
        //    string apppwd = ConfigurationManager.AppSettings["usercenterserviceapppwd"];
        //    string appkey = ConfigurationManager.AppSettings["usercenterserviceappkey"];
        //    string signname = ConfigurationManager.AppSettings["signname"];
        //    string time = DateTime.Now.ToString("yyyyMMddHHmmss");
        //    string functionname = "cpmfour";

        //    string[] pwdArray = { appid, apppwd, signname, time, functionname };
        //    string code = EncryptHelper.GetMd5(pwdArray, appkey);

        //    List<CompanyProduct_Module> cpm = null;
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(api))
        //        {
        //            var sinfo = new { appid = appid, apppwd = apppwd, signname = signname, time = time, code = code, functionname = functionname };
        //            var info = new
        //            {
        //                uinfo = new { token = "" },
        //                appinfo = new ApplicationInfo(procode.ToString()),
        //                funinfo = new { companyid = fxtcompanyid, producttypecode = producttypecode, search = new { Page = false, PageIndex = 1, PageRecords = int.MaxValue } }
        //            };
        //            var post = "{\"sinfo\":\"" + JSONHelper.ObjectToJSON(sinfo).Replace("\"", "'") + "\",\"info\":\"" + JSONHelper.ObjectToJSON(info).Replace("\"", "'") + "\"}";
        //            var str = APIPostBack(api, post, "application/json");
        //            var rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
        //            if (rtn.returntype > 0)
        //            {
        //                cpm = new List<CompanyProduct_Module>();
        //                cpm = JSONHelper.JSONToObject<CompanyProduct_Module[]>(rtn.data.ToString()).ToList();
        //                return cpm;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return cpm;
        //}
        //private string APIPostBack(string url, string post, string contentType)
        //{
        //    byte[] postData = Encoding.UTF8.GetBytes(post);
        //    var client = new WebClient();
        //    client.Headers.Add("Content-Type", contentType);
        //    client.Headers.Add("ContentLength", postData.Length.ToString());
        //    //这里url要组装安全标记等参数
        //    var result = "";
        //    try
        //    {
        //        byte[] responseData = client.UploadData(url, "POST", postData);
        //        result = Encoding.UTF8.GetString(responseData);
        //        //找退出原因
        //        //LogHelper.Info(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        result = JSONHelper.GetJson(null, 0, ex.Message, ex);
        //    }
        //    client.Dispose();
        //    return result;
        //}
    }
}
