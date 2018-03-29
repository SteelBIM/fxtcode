using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FxtCollateralManager.Common;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using FxtCollateralManager.Common.FxtAPI;
using FxtCommonLibrary.LibraryUtils;
using FxtNHibernate.FxtLoanDomain.Entities;
using CAS.Common.MVC4;

/**
 * 作者:李晓东
 * 摘要:2013.12.31 新建
 *      2014.01.03 新增ProductList及菜单相关操作 修改人:李晓东
 *      2014.06.11 修改人 曹青
 *                 新增 客户管理 CheckCustomerName、AddEditCustomer、GetCustomer
 *                 新增 用户管理 CheckUserName、AddEditUser、GetUser
 *                 注释不使用的方法（用户、权限、产品、菜单相关）
 *      2014.06.17 修改人 曹青
 *                 新增 UserLogin、ModifyPassword
 *      2014.06.18 修改人 曹青
 *                 新增 DeleteActiveCustomer、DeleteActiveUser      
 * **/
namespace FxtCollateralManager.Web.Controllers
{
    public class UsersController : BaseController
    {
        #region 客户管理
        //客户
        public ActionResult Customer()
        {
            return View();
        }

        //验证客户名称
        [HttpPost]
        public ActionResult CheckCustomerName(int customerId, string customerName, int fxtCompanyId)
        {
            JObject jobject = new JObject();
            jobject.Add("customerId", customerId);
            jobject.Add("customerName", customerName);
            jobject.Add("fxtCompanyId", fxtCompanyId);

            using (FxtAPIClient client = new FxtAPIClient())
            {
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey), "A", _CheckCustomerName, Utils.Serialize(jobject));
                return Json(Utils.Deserialize<int>(Utils.GetJObjectValue(result, "type")) == 1);//验证字符，需要返回true/false
            }
        }

        //新增、修改客户
        [HttpPost]
        public ActionResult AddEditCustomer(SysCustomer data)
        {
            JObject jobject = new JObject();
            data.Valid = 1;
            data.CreateDate = DateTime.Now;
            data.CreateUserId = Public.LoginInfo.Id;
            jobject.Add("dataCustomer", Utils.Serialize(data));
            using (FxtAPIClient client = new FxtAPIClient())
            {
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey), "A", _AddEditCustomer, Serialize(jobject));
                return Json(result);
            }
        }

        //删除、激活客户
        [HttpPost]
        public ActionResult DeleteActiveCustomer(int customerId, int valid)
        {
            JObject jobject = new JObject();
            jobject.Add("customerId", customerId);
            jobject.Add("valid", valid);
            using (FxtAPIClient client = new FxtAPIClient())
            {
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey), "A", _DeleteActiveCustomer, Serialize(jobject));
                return Json(result);
            }
        }

        //客户详情
        [HttpPost]
        public ActionResult GetCustomer(int customerId)
        {
            JObject jobject = new JObject();
            jobject.Add("customerId", customerId);
            jobject.Add("key", "");
            jobject.Add("customerType", 0);
            jobject.Add("fxtCompanyId", 0);
            jobject.Add("valid", 1);
            jobject.Add("pageIndex", 0);
            jobject.Add("pageSize", 0);
            jobject.Add("orderProperty", "");
            using (FxtAPIClient client = new FxtAPIClient())
            {
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey), "A", _GetCustomer, Serialize(jobject));
                return Json(result);
            }
        }

        //客户列表
        [HttpPost]
        public ActionResult GetCustomerList(string key, int fxtCompanyId, int customerType, int valid, int pageIndex, int pageSize, string orderProperty)
        {
            JObject jobject = new JObject();
            jobject.Add("customerId", 0);
            jobject.Add("key", key);
            jobject.Add("customerType", customerType);
            jobject.Add("fxtCompanyId", fxtCompanyId);
            jobject.Add("valid", valid);
            jobject.Add("pageIndex", pageIndex);
            jobject.Add("pageSize", pageSize);
            jobject.Add("orderProperty", orderProperty);
            using (FxtAPIClient client = new FxtAPIClient())
            {
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey), "A", _GetCustomer, Serialize(jobject));
                return Json(result);
            }
        }
        /// <summary>
        /// 获取所有客户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAllCustomer(int customerType=0)
        {
            JObject jobject = new JObject();
            jobject.Add("customerId", 0);
            jobject.Add("key", "");
            jobject.Add("customerType", customerType);
            jobject.Add("fxtCompanyId", 0);
            jobject.Add("valid", 1);
            jobject.Add("pageIndex", 1);
            jobject.Add("pageSize", 0);
            jobject.Add("orderProperty", "CustomerName asc");
            using (FxtAPIClient client = new FxtAPIClient())
            {
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey), "A", _GetCustomer, Serialize(jobject));
                return Json(result);
            }
        }

        //获取用户中心客户列表
        [HttpPost]
        public ActionResult GetUserCenterCompany(int customerType, string companyName)
        {
            //用户中心安全验证参数
            CAS.Entity.SurveyApi sa = null;
            CAS.Entity.LoginInfoEntity logininfo = new CAS.Entity.LoginInfoEntity()
            {
                username = Public.LoginInfo.UserName,
                fxtcompanyid = StringHelper.TryGetInt(GetAppSettingValue("fxtcompanyid")),
                signname = GetAppSettingValue("fxtsignname"),
            };
            sa = new CAS.Entity.SurveyApi(logininfo, GetAppSettingValue("systypecode"), GetAppSettingValue("appid"));
            sa.sinfo = new CAS.Entity.SecurityInfo(logininfo, GetAppSettingValue("appid"), GetAppSettingValue("fxtappPwd"), GetAppSettingValue("fxtappKey"));
            sa.sinfo.functionname = "companyseven";//获取公司列表
            sa.info.funinfo = new { companytypecode = customerType, companyname = companyName };

            JObject jobject = new JObject();
            jobject.Add("sinfo", Utils.Serialize(sa.sinfo));
            jobject.Add("info", Utils.Serialize(sa.info));
            HttpResponseMessage hrm = httpClient.PostAsJsonAsync(GetAppSettingValue("fxtusercenterservice"), jobject).Result;
            string str = hrm.Content.ReadAsStringAsync().Result;
            httpClient.Dispose();
            if (hrm.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Json(ResultUserJson(str));
            }
            return null;
        }
        #endregion

        #region 用户管理
        //用户
        public ActionResult UserInfo()
        {
            return View();
        }

        //用户
        public ActionResult ModifyPassword()
        {
            return View();
        }

        //验证用户名称
        [HttpPost]
        public ActionResult CheckUserName(int id, string userName)
        {
            JObject jobject = new JObject();
            jobject.Add("id", id);
            jobject.Add("userName", userName);
            using (FxtAPIClient client = new FxtAPIClient())
            {
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),"A", _CheckUserName, Serialize(jobject));
                return Json(Utils.Deserialize<int>(Utils.GetJObjectValue(result, "type")) == 1);//验证字符，需要返回true/false
                //return Json(result);
            }
        }

        //新增、修改用户
        [HttpPost]
        public ActionResult AddEditUser(SysUser data)
        {
            JObject jobject = new JObject();
            if (!string.IsNullOrEmpty(data.UserPwd))//加密密码
                data.UserPwd = EncryptHelper.TextToPassword(data.UserPwd);
            data.Valid = 1;
            data.CreateDate = DateTime.Now;
            data.CreateUserId = Public.LoginInfo.Id;
            jobject.Add("dataUser", Utils.Serialize(data));
            using (FxtAPIClient client = new FxtAPIClient())
            {
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),"A", _AddEditUser, Serialize(jobject));
                return Json(result);
            }
        }

        //删除、激活用户
        [HttpPost]
        public ActionResult DeleteActiveUser(int id, int valid)
        {
            JObject jobject = new JObject();
            jobject.Add("id", id);
            jobject.Add("valid", valid);
            using (FxtAPIClient client = new FxtAPIClient())
            {
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),"A", _DeleteActiveUser, Serialize(jobject));
                return Json(result);
            }
        }

        //修改密码
        [HttpPost]
        public ActionResult ModifyPass(string oldUserPwd, string userPwd)
        {
            JObject jobject = new JObject();
            jobject.Add("id", Public.LoginInfo.Id);//登录用户ID
            jobject.Add("oldUserPwd", EncryptHelper.TextToPassword(oldUserPwd));
            jobject.Add("userPwd", EncryptHelper.TextToPassword(userPwd));
            using (FxtAPIClient client = new FxtAPIClient())
            {
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),"A", _ModifyPassword, Serialize(jobject));
                return Json(result);
            }
        }

        //用户详情
        [HttpPost]
        public ActionResult GetUser(int id)
        {
            JObject jobject = new JObject();
            jobject.Add("id", id);
            jobject.Add("key", "");
            jobject.Add("fxtCompanyId", 0);
            jobject.Add("customerId", 0);
            jobject.Add("valid", 1);
            jobject.Add("pageIndex", 0);
            jobject.Add("pageSize", 0);
            jobject.Add("orderProperty", "");
            using (FxtAPIClient client = new FxtAPIClient())
            {
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),"A", _GetUser, Serialize(jobject));
                return Json(result);
            }
        }

        //用户列表
        [HttpPost]
        public ActionResult GetUserList(string key, int fxtCompanyId, int customerId, int valid, int pageIndex, int pageSize, string orderProperty)
        {
            JObject jobject = new JObject();
            jobject.Add("id", 0);
            jobject.Add("key", key);
            jobject.Add("fxtCompanyId", fxtCompanyId);
            jobject.Add("customerId", customerId);
            jobject.Add("valid", valid);
            jobject.Add("pageIndex", pageIndex);
            jobject.Add("pageSize", pageSize);
            jobject.Add("orderProperty", orderProperty);
            using (FxtAPIClient client = new FxtAPIClient())
            {
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),"A", _GetUser, Serialize(jobject));
                return Json(result);
            }
        }
        #endregion


        /* oldfunction
        #region 用户
        //用户
        public ActionResult Index()
        {
            return View();
        }

        //新增用户
        [HttpPost]
        public ActionResult AddUsers(string username, string userpwd,
            string emailstr, string mobile, string wxopenid, string companyid)
        {

            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("username", username));
            postData.Add(new KeyValuePair<string, string>("userpwd", GetSysCode(userpwd)));
            postData.Add(new KeyValuePair<string, string>("emailstr", emailstr));
            postData.Add(new KeyValuePair<string, string>("mobile", mobile));
            postData.Add(new KeyValuePair<string, string>("wxopenid", wxopenid));
            postData.Add(new KeyValuePair<string, string>("companyid", companyid));
            SetSendType(postData, "add");
            HttpResponseMessage hrm =
                httpClient.PostAsync(GetUserUrl("CUser"),
                GetFormUrlEncodedContent(postData)).Result;
            string str = hrm.Content.ReadAsStringAsync().Result;
            httpClient.Dispose();
            return Json(ResultUserJson(str));
        }

        //修改用户
        [HttpPost]
        public ActionResult UpdateUsers(string username, string userpwd,
            string emailstr, string mobile, string wxopenid, string companyid)
        {
            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("username", username));
            postData.Add(new KeyValuePair<string, string>("userpwd", GetSysCode(userpwd)));
            postData.Add(new KeyValuePair<string, string>("emailstr", emailstr));
            postData.Add(new KeyValuePair<string, string>("mobile", mobile));
            postData.Add(new KeyValuePair<string, string>("wxopenid", wxopenid));
            postData.Add(new KeyValuePair<string, string>("companyid", companyid));
            SetSendType(postData, "edit");
            HttpResponseMessage hrm =
                httpClient.PostAsync(GetUserUrl("CUser"),
                GetFormUrlEncodedContent(postData)).Result;
            string str = hrm.Content.ReadAsStringAsync().Result;
            httpClient.Dispose();
            return Json(ResultUserJson(str));
        }

        //获得单个用户信息
        [HttpPost]
        public ActionResult GetUsers(string username, string companyid)
        {
            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("username", username));
            postData.Add(new KeyValuePair<string, string>("companyid", companyid));
            SetSendType(postData);
            HttpResponseMessage hrm =
                httpClient.PostAsync(GetUserUrl("FindUser"),
                GetFormUrlEncodedContent(postData)).Result;
            string str = hrm.Content.ReadAsStringAsync().Result;
            httpClient.Dispose();
            return Json(ResultUserJson(str));
        }

        //用户列表
        [HttpPost]
        public ActionResult GetUserList(string pageSize, string pageIndex)
        {
            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("pageSize", pageSize));
            postData.Add(new KeyValuePair<string, string>("pageIndex", pageIndex));
            SetSendType(postData, "all");
            HttpResponseMessage hrm =
                httpClient.PostAsync(GetUserUrl("ListUser"),
                GetFormUrlEncodedContent(postData)).Result;
            string str = hrm.Content.ReadAsStringAsync().Result;
            httpClient.Dispose();
            return Json(ResultUserJson(str));
        }
        //获取公司列表
        public ActionResult GetCompanylist()
        {
            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("pageSize", "10000"));
            postData.Add(new KeyValuePair<string, string>("pageIndex", "1"));
            SetSendType(postData);
            HttpResponseMessage hrm =
                httpClient.PostAsync(GetUserUrl("companylist"),
                GetFormUrlEncodedContent(postData)).Result;
            string str = hrm.Content.ReadAsStringAsync().Result;
            httpClient.Dispose();
            if (hrm.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Json(ResultUserJson(str));
            }
            return null;
        }

        //存放用户权限
        [HttpPost]
        public ActionResult UserPurview(string userid, int productid, string purview)
        {
            using (FxtUsersClient client = new FxtUsersClient())
            {
                return Json(ResultServerJson(null, client.UserPurview(userid, productid, purview)));
            }
        }

        //用户权限
        [HttpPost]
        public ActionResult GetUserPurview(string userid)
        {
            using (FxtUsersClient client = new FxtUsersClient())
            {
                return Json(ResultServerJson(client.GetUserPurview(userid)));
            }
        }
        #endregion

        #region 权限
        //权限
        public ActionResult Purviews()
        {
            return View();
        }
        //新增权限
        [HttpPost]
        public ActionResult AddPurviews(string purviewname)
        {
            using (FxtUsersClient userClient = new FxtUsersClient())
            {
                JObject jobject = new JObject();
                jobject.Add(new JProperty("purviewname", purviewname));
                bool flag = userClient.Purviews(JsonConvert.SerializeObject(jobject), "C");
                return Json(ResultServerJson(null, flag));
            }
        }
        //新增权限
        [HttpPost]
        public ActionResult UpdatePurviews(string purviewname, int id)
        {
            using (FxtUsersClient userClient = new FxtUsersClient())
            {
                JObject jobject = new JObject();
                jobject.Add(new JProperty("id", id));
                jobject.Add(new JProperty("purviewname", purviewname));
                bool flag = userClient.Purviews(JsonConvert.SerializeObject(jobject), "U");
                return Json(ResultServerJson(null, flag));
            }
        }
        //删除权限
        [HttpPost]
        public ActionResult DelPurview(int id)
        {
            using (FxtUsersClient userClient = new FxtUsersClient())
            {
                JObject jobject = new JObject();
                jobject.Add(new JProperty("id", id));
                bool flag = userClient.Purviews(JsonConvert.SerializeObject(jobject), "D");
                return Json(ResultServerJson(null, flag));
            }
        }

        //获得权限列表操作,带分页
        [HttpPost]
        public ActionResult GetPurviewList(int pageSize, int pageIndex)
        {
            using (FxtUsersClient userClient = new FxtUsersClient())
            {
                string result = userClient.GetPurview("", pageSize, pageIndex);
                return Json(ResultServerJson(result));
            }
        }

        //获得权限列表
        [HttpPost]
        public ActionResult GetPurviewAll()
        {
            using (FxtUsersClient userClient = new FxtUsersClient())
            {
                string result = userClient.GetPurviewAll();
                return Json(ResultServerJson(result));
            }
        }

        //获得权限单个对象
        [HttpPost]
        public ActionResult GetPurviews(int id)
        {
            using (FxtUsersClient client = new FxtUsersClient())
            {
                return Json(ResultServerJson(client.GetPurviews(id)));
            }
        }

        #endregion

        #region 产品
        //产品
        public ActionResult Products()
        {
            return View();
        }
        //新增产品操作
        [HttpPost]
        public ActionResult AddProducts(string productname)
        {
            using (FxtUsersClient userClient = new FxtUsersClient())
            {
                JObject jobject = new JObject();
                jobject.Add(new JProperty("ProductName", productname));
                bool flag = userClient.Products(JsonConvert.SerializeObject(jobject), "C");
                return Json(ResultServerJson(null, flag));
            }
        }

        //新增产品操作
        [HttpPost]
        public ActionResult UpdateProducts(int id, string productname)
        {
            using (FxtUsersClient userClient = new FxtUsersClient())
            {
                JObject jobject = new JObject();
                jobject.Add(new JProperty("Id", id));
                jobject.Add(new JProperty("ProductName", productname));
                bool flag = userClient.Products(JsonConvert.SerializeObject(jobject), "U");
                return Json(ResultServerJson(null, flag));
            }
        }

        //删除产品
        [HttpPost]
        public ActionResult DelProduct(int id)
        {
            using (FxtUsersClient userClient = new FxtUsersClient())
            {
                JObject jobject = new JObject();
                jobject.Add(new JProperty("Id", id));
                bool flag = userClient.Products(JsonConvert.SerializeObject(jobject), "D");
                return Json(ResultServerJson(null, flag));
            }
        }
        //获得产品树形
        [HttpPost]
        public ActionResult GetProductTree()
        {
            using (FxtUsersClient userClient = new FxtUsersClient())
            {
                string result = userClient.GetProduct();
                return Json(ResultServerJson(result));
            }
        }

        //获得产品列表
        [HttpPost]
        public ActionResult ProductList()
        {
            using (FxtUsersClient userClient = new FxtUsersClient())
            {
                string result = userClient.GetProductAll();
                return Json(ResultServerJson(result));
            }
        }
        //获得单个产品
        [HttpPost]
        public ActionResult GetProducts(int id)
        {
            using (FxtUsersClient userClient = new FxtUsersClient())
            {
                return Json(ResultServerJson(userClient.GetProducts(id)));
            }
        }

        //菜单分配权限
        [HttpPost]
        public ActionResult MenuPurview(int menuid, string purview)
        {
            using (FxtUsersClient userClient = new FxtUsersClient())
            {
                return Json(ResultServerJson(null, userClient.MenuPurview(menuid, purview)));
            }
        }

        //得到菜单的所有权限
        [HttpPost]
        public ActionResult GetMenuPurview(int menuid)
        {
            using (FxtUsersClient userClient = new FxtUsersClient())
            {
                return Json(ResultServerJson(userClient.GetMenuPurview(menuid)));
            }
        }

        //得到产品下面的所有菜单及菜单相关的权限
        [HttpPost]
        public ActionResult GetProductMenuByPurview(int id)
        {
            using (FxtUsersClient userClient = new FxtUsersClient())
            {
                return Json(ResultServerJson(userClient.GetProductMenuByPurview(id)));
            }
        }

        #endregion

        #region 菜单
        //菜单
        public ActionResult Menus()
        {
            return View();
        }

        //新增菜单
        [HttpPost]
        public ActionResult AddMenus(int productid, string menuname, int parentid, string url)
        {
            using (FxtUsersClient client = new FxtUsersClient())
            {
                JObject jobject = new JObject();
                jobject.Add(new JProperty("menuname", menuname));
                jobject.Add(new JProperty("parentid", parentid));
                jobject.Add(new JProperty("url", url));
                bool flag = client.Menus(productid, JsonConvert.SerializeObject(jobject), "C");
                return Json(ResultServerJson(null, flag));
            }
        }

        //更新菜单
        [HttpPost]
        public ActionResult UpdateMenus(int id, string menuname, int parentid, string url, int productid = 0)
        {
            using (FxtUsersClient client = new FxtUsersClient())
            {
                JObject jobject = new JObject();
                jobject.Add(new JProperty("id", id));
                jobject.Add(new JProperty("menuname", menuname));
                jobject.Add(new JProperty("parentid", parentid));
                jobject.Add(new JProperty("url", url));
                bool flag = client.Menus(productid, JsonConvert.SerializeObject(jobject), "U");
                return Json(ResultServerJson(null, flag));
            }
        }

        //删除菜单
        [HttpPost]
        public ActionResult DelMenu(int id)
        {
            using (FxtUsersClient userClient = new FxtUsersClient())
            {
                JObject jobject = new JObject();
                jobject.Add(new JProperty("Id", id));
                bool flag = userClient.Menus(0, JsonConvert.SerializeObject(jobject), "D");
                return Json(ResultServerJson(null, flag));
            }
        }

        //获得菜单单个对象
        [HttpPost]
        public ActionResult GetMenus(int id)
        {
            using (FxtUsersClient client = new FxtUsersClient())
            {
                return Json(ResultServerJson(client.GetMenus(id)));
            }
        }

        //获得产品列表操作,带分页
        [HttpPost]
        public ActionResult GetMenuList()
        {
            using (FxtUsersClient userClient = new FxtUsersClient())
            {
                return Json(ResultServerJson(userClient.GetMenuListAll()));
            }
        }
        //获得父级菜单
        //[HttpPost]
        //public ActionResult GetMenusByParentId()
        //{
        //    using (FxtUsersClient client = new FxtUsersClient())
        //    {
        //        string str = client.GetMenuListByParentId();
        //        return Json(ResultServerJson(str));
        //    }
        //}

        //根据产品获得父级菜单
        [HttpPost]
        public ActionResult GetMenusByProductId(int productid)
        {
            using (FxtUsersClient client = new FxtUsersClient())
            {
                string str = client.GetMenuListByProductId(productid);
                return Json(ResultServerJson(str));
            }
        }
        #endregion
        */
    }
}
