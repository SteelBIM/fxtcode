using System.Web.Mvc;
using CourseActivate.Account.Constract.Models;
using System.Collections.Generic;
using CourseActivate.Core.Utility;
using CourseActivate.Framework.BLL;
using CourseActivate.Account.Constract.VW;
using System.Linq;
using System.Web;
using System;
using CourseActivate.Activate.BLL;

namespace CourseActivate.Web.Admin.Controllers
{
    public class LoginController : Controller
    {
        [AuthenticationAttribute(IsCheck = false)]
        public ActionResult Index()
        {
            CookieHelper.ClearCookie("sessionId");
            CookieHelper.ClearCookie("LoginInfo");
            CookieHelper.ClearCookie("ActionInfo");
            CourseActivate.Web.Admin.Controllers.BaseController.vw_actionlist = null;
            return View();
        }
        Manage manage = new Manage();
        [AuthenticationAttribute(IsCheck = false)]
        public int Login(string LoginName, string LoginPwd)
        {
            LoginName = PublicHelp.DelSQLStr(LoginName);
            LoginPwd = PublicHelp.DelSQLStr(LoginPwd);
            if (!string.IsNullOrEmpty(LoginName) && !string.IsNullOrEmpty(LoginPwd))
            {
                string pwd = PublicHelp.pswToSecurity(LoginPwd);
                List<com_master> master = manage.SelectSearch<com_master>(x => (x.mastername == LoginName && x.password == pwd));

                if (master != null && master.Count > 0)
                {

                    List<vw_action> action = new List<vw_action>();
                    if (master[0].groupid == 0)
                    {
                        List<vw_allaction> allaction = manage.SelectAll<vw_allaction>();
                        for (int i = 0; i < allaction.Count; i++)
                        {
                            action.Add(allaction[i]);
                        }
                    }
                    else
                        action = manage.SelectSearch<vw_action>(x => x.groupid == master[0].groupid, 10000, " parentsequence,sequence ");

                    if (action != null && action.Count > 0)
                    {
                        if (master[0].groupid == 0)
                            master[0].dataauthority = 0;
                        else
                        {
                            com_group comgroup = manage.Select<com_group>(master[0].groupid.ToString());
                            if (comgroup != null)
                                master[0].dataauthority = comgroup.dataauthority;
                        }
                        action = action.OrderBy(i => i.parentsequence).ThenBy(i => i.sequence).ToList();

                        LoginRedis logredis = new LoginRedis();
                        string sessionId = Core.Utility.PublicHelp.EncryptString(LoginName, "12345678");
                        CookieHelper.SetCookie("sessionId", sessionId,7);
                        master[0].remark = "";
                        CookieHelper.SetCookie("LoginInfo", JsonHelper.ToJson(master[0]), 7);
                        CookieHelper.SetCookie("Mastername", master[0].mastername, 7);
                        CookieHelper.SetCookie("ActionInfo", JsonHelper.ToJson(action), 7);
                        return 1;
                    }

                }
            }
            return 0;
        }
    }
}
