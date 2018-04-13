using System.Web.Mvc;
using KSWF.WFM.Constract.Models;
using System.Collections.Generic;
using KSWF.Core.Utility;
using KSWF.Framework.BLL;
using KSWF.WFM.Constract.VW;
using System.Threading;
using System;
using System.Web;

namespace KSWF.Web.Admin.Controllers
{
    public class LoginController : Controller
    {
        [AuthenticationAttribute(IsCheck = false)]
        public ActionResult Index()
        {
            ServiceArea area = new ServiceArea();
            Session["Action"] = null;
            Session["LoginInfo"] = null;
            HttpContext.Response.Cookies["sessionId"].Expires = DateTime.Now.AddSeconds(-1);
            return View();
        }

        [AuthenticationAttribute(IsCheck = false)]
        public int Login(string LoginName, string LoginPwd)
        {
            LoginName = PublicHelp.DelSQLStr(LoginName).Trim();
            LoginPwd = PublicHelp.DelSQLStr(LoginPwd);
            if (!string.IsNullOrEmpty(LoginName) && !string.IsNullOrEmpty(LoginPwd))
            {
                string pwd = PublicHelp.pswToSecurity(LoginPwd);
                Manage manage = new Manage();
                DateTime currenttime = DateTime.Now.AddDays(-30);
               // List<com_master> master = manage.SelectSearch<com_master>(x => ((x.state == 0 && x.mastername == LoginName && x.password == pwd) || (x.mastertype == 1 && x.mastername == LoginName && x.password == pwd && x.createtime > currenttime)));
                List<com_master> master = manage.SelectSearch<com_master>(x => ((x.state == 0 && x.mastername == LoginName && x.password == pwd)));

                if (master != null && master.Count > 0)
                {
                    BaseController basec = new BaseController();
                    List<vw_action> action = new List<vw_action>();
                    if (master[0].groupid == 0)
                    {
                        List<vw_allaction> allaction = manage.SelectAll<vw_allaction>();
                        for (int i = 0; i < allaction.Count; i++)
                            action.Add(allaction[i]);
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

                        HttpCookie cookie = new HttpCookie("sessionId");
                        cookie.Expires = DateTime.Now.AddMinutes(10);
                        cookie.Value = Core.Utility.PublicHelp.EncryptString(LoginName, "12345678");
                        HttpContext.Response.Cookies.Add(cookie);
                        Session["Action"] = action;
                        Session["LoginInfo"] = master[0];
                        Session.Timeout = 20;
                        string Ip = Core.Utility.PublicHelp.GetIP();
                        if (manage.GetTotalCount<master_loginlog>(t => t.mastername == LoginName) > 0)
                        {
                            manage.Update<master_loginlog>(new { lastloginip = Ip, lastlogintime = DateTime.Now }, t => t.mastername == LoginName);
                        }
                        else
                        {
                            manage.Insert<master_loginlog>(new master_loginlog() 
                            { mastername = LoginName,
                                loginip = Ip,
                                logintime = DateTime.Now,
                                lastloginip = Ip,
                                lastlogintime = DateTime.Now
                            });
                        }
                        //Thread.Sleep(500);
                        if (master[0].state == 1)//登录用户为已删除的代理商
                        {
                            return Convert.ToInt32(Core.Utility.PublicHelp.ConvertDateTimeInt(master[0].createtime.AddDays(30)));
                        }
                        return 1;
                    }
                }
            }
            return 0;
        }
    }
}
