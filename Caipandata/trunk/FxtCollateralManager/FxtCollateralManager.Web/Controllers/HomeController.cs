using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FxtCollateralManager.Common;
using FxtCommonLibrary.LibraryUtils;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using CAS.Common.MVC4;
using System.Web.SessionState;
using FxtNHibernate.FxtLoanDomain.Entities;
using FxtNHibernate.DTODomain.APIActualizeDTO;
using FxtNHibernate.DTODomain.FxtLoanDTO;
using FxtCollateralManager.Common.FxtAPI;


/***
 * 作者:  李晓东
 * 时间:  2013.12.4
 * 摘要:  创建 HomeController 默认页类
 * **/
namespace FxtCollateralManager.Web.Controllers
{

    public class HomeController : BaseController
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            //HttpClient httpClient = new HttpClient();
            //var aa = httpClient.GetStringAsync("http://localhost:6300/API/FxtTask.svc/TList/JSON").Result;
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        //登陆超时
        public ActionResult AuthorOut() {
            return View();
        }

        //用户登录
        public ActionResult UserLogin(string userName, string userPwd)
        {
            JObject jobject = new JObject();
            jobject.Add("userName", userName);
            jobject.Add("userPwd", EncryptHelper.TextToPassword(userPwd));
            using (FxtAPIClient client = new FxtAPIClient())
            {
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),"A", _UserLogin, Utils.Serialize(jobject));
                UserInfo myuser = Utils.Deserialize<UserInfo>(Utils.GetJObjectValue(result, "data"));
                if (myuser != null && myuser.Id > 0) //登录成功，用户信息保存至session
                {
                    SessionHelper.Add("UserId", myuser.Id);
                    SessionHelper.Add("UserName", myuser.UserName);
                    SessionHelper.Add("TrueName", myuser.TrueName);
                    SessionHelper.Add("FxtCompanyId", myuser.FxtCompanyId);
                    SessionHelper.Add("CustomerId", myuser.CustomerId);
                    SessionHelper.Add("EmailStr", myuser.EmailStr);
                    SessionHelper.Add("Mobile", myuser.Mobile);
                    SessionHelper.Add("CustomerName", myuser.CustomerName);
                    SessionHelper.Add("CustomerType", myuser.CustomerType);                    
                }
                return Json(result);
            }
        }

        //用户注销
        public ActionResult LoginOut()
        {
            SessionHelper.DelAll();
            return null;
        }

        public ActionResult Content()
        {
            return View();
        }

        public FileResult Download(string path, string file)
        {
            if (!Utils.IsNullOrEmpty(path))
                path = GetCurrentPath(System.IO.Path.Combine(path, file));
            else
                path = GetCurrentPath(System.IO.Path.Combine(file));
            return File(path, "application/ms-excel", file);
        }
    }
}
