using CourseActivate.Account.Constract.Models;
using CourseActivate.Account.Constract.VW;
using CourseActivate.Activate.BLL;
using CourseActivate.Core.Utility;
using CourseActivate.Framework.BLL;
using CourseActivate.Web.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CourseActivate.Web.Admin
{
    public class AuthenticationAttribute : ActionFilterAttribute
    {
        public bool IsCheck { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (IsCheck)
            {
                base.OnActionExecuting(filterContext);
                if (CookieHelper.GetCookie("sessionId") != null&&CookieHelper.GetCookie("LoginInfo")!=null)
                {
                    Manage manage = new Manage();
                    string sessionId = CookieHelper.GetCookie("sessionId");
                    string mastername = Core.Utility.PublicHelp.DecryptString(sessionId, "12345678");
                    List<com_master> master = manage.SelectSearch<com_master>(x => x.mastername == mastername);

                    if (master != null && master.Count > 0)
                    {
                        BaseController basec = new BaseController();
                        List<vw_action> action = new List<vw_action>();
                        if (master[0].groupid == 0)
                        {
                            List<vw_allaction> allaction = manage.SelectAll<vw_allaction>();
                            allaction.OrderBy(i => i.parentsequence).ThenBy(i => i.sequence);
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

                            action = action.OrderBy(i => i.parentsequence).ThenBy(i => i.sequence).ToList();
                        }
                    }
                }
                else
                {
                    //Authority 权限
                    UrlHelper url = new UrlHelper(filterContext.RequestContext);
                    string result = string.Format("<script type='text/javascript'> window.top.location = '" + url.Content("~/Login/Index") + "';</script>");
                    filterContext.Result = new ContentResult() { Content = result };
                    return;
                }
                
            }
        }
    }
}