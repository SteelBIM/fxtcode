using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CBSS.Web.Admin.Common;
using CBSS.Core.Utility;
using CBSS.Framework.Web;
using CBSS.Account.Contract;
using CBSS.Framework.Contract;
using System.Text;
using CBSS.Account.Contract.ViewModel;

namespace CBSS.Web.Admin.Areas.Account.Controllers
{
    public class AuthController : AdminControllerBase
    {
        [AuthorizeIgnore]
        public ActionResult Login()
        {
           // CBSS.Core.Utility.ZipHelper.GetFoldAll("F://TbxResources");
            return View();
        }

        [HttpPost]
        [AuthorizeIgnore]
        public ActionResult Login(string username, string password, string verifycode)
        {
            //if (!VerifyCodeHelper.CheckVerifyCode(verifycode, this.CookieContext.VerifyCodeGuid))
            //{
            //    ModelState.AddModelError("error", "验证码错误");
            //    return View();
            //}

            var loginInfo = this.AccountService.Login(username, password);

            if (loginInfo != null && loginInfo.UserID > 0)
            {
                this.CookieContext.UserToken = loginInfo.LoginToken;
                this.CookieContext.UserName = loginInfo.LoginName;
                this.CookieContext.UserId = loginInfo.UserID;
                //HttpCookie cookie = new HttpCookie(SecurityHelper.MD5("CBSSUser"));
                //cookie.Expires = DateTime.Now.AddDays(1);
                //cookie.Value = loginInfo.UserID.ToString();
                //HttpContext.Response.Cookies.Add(cookie);
                Session["LoginInfo"] = null;
                Session["LoginInfo"] = loginInfo;
                return Redirect("/Account/Auth/Index");
            }
            else
            {
                ModelState.AddModelError("error", "用户名或密码错误");
                return View();
            }
        }
        public List<v_allaction> Get_AllAction()
        {
            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(SecurityHelper.MD5("CBSSUser"));
            string UserID = cookie != null ? cookie.Value : "";
            if (!string.IsNullOrEmpty(UserID))
            {
                IEnumerable<v_allaction> v_Allaction = this.AccountService.Get_AllActionByUserID(UserID);
                if (v_Allaction != null && v_Allaction.Count() > 0)
                {
                    return v_Allaction.ToList();
                }
            }
            Logout();
            return null;
        }
        public ActionResult Logout()
        {
            this.AccountService.Logout(this.CookieContext.UserToken);
            this.CookieContext.UserToken = Guid.Empty;
            this.CookieContext.UserName = string.Empty;
            this.CookieContext.UserId = 0;
            Session["LoginInfo"] = null;
            return RedirectToAction("Login");
        }

        public ActionResult ModifyPwd()
        {
            var model = this.AccountService.GetUser(this.LoginInfo.UserID);
            return View(model);
        }

        [HttpPost]
        public ActionResult ModifyPwd(FormCollection collection)
        {
            var model = this.AccountService.GetUser(this.LoginInfo.UserID);
            this.TryUpdateModel<Sys_User>(model);

            try
            {
                bool flag = this.AccountService.ModifyPwd(model);
                if (flag)
                { 
                    string content = string.Format("<script>alert('修改成功');parent.location.href='{0}'</script>", "Login");
                    return this.Content(content);
                }
                else
                {
                    this.ModelState.AddModelError("", "修改失败，请稍后再试！");
                }
            }
            catch (BusinessException e)
            {
                this.ModelState.AddModelError(e.Name, e.Message);
                return View(model);
            }

            return this.RefreshParent();
        }

        public ActionResult Index()
        {
            return View();
        }

        //[AuthorizeIgnore]
        //public ActionResult VerifyImage()
        //{
        //    var s1 = new ValidateCode_Style4();
        //    string code = "6666";
        //    byte[] bytes = s1.CreateImage(out code);

        //    this.CookieContext.VerifyCode = code;

        //    return File(bytes, @"image/jpeg");

        //}

    }
}
