using KSWF.Framework.BLL;
using KSWF.Web.Admin.Controllers;
using KSWF.WFM.Constract.Models;
using KSWF.WFM.Constract.VW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace KSWF.Web.Admin
{
    public class AuthenticationAttribute : ActionFilterAttribute
    {
        public bool IsCheck { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (IsCheck)
            {
                base.OnActionExecuting(filterContext);
                if (filterContext.HttpContext.Session["LoginInfo"] != null || filterContext.HttpContext.Session["Action"] != null)
                {
                    filterContext.HttpContext.Session["LoginInfo"] = filterContext.HttpContext.Session["LoginInfo"];
                    filterContext.HttpContext.Session["Action"] = filterContext.HttpContext.Session["Action"];
                    filterContext.HttpContext.Session.Timeout = 20;
                }
                else
                {
                   
                    Manage manage = new Manage();
                    HttpCookie cookieName = System.Web.HttpContext.Current.Request.Cookies.Get("sessionId");
                    if (cookieName != null)
                    {
                        string mastername= Core.Utility.PublicHelp.DecryptString( cookieName.Value, "12345678");
                        List<com_master> master = manage.SelectSearch<com_master>(x => x.mastername == mastername);

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
                                //HttpCookie cookie = new HttpCookie("mastername");
                                //cookie.Expires = DateTime.Now.AddMinutes(20);
                                //cookie.Value = LoginName;
                                //HttpContext.Response.Cookies.Add(cookie);
                                filterContext.HttpContext.Session["Action"] = action;
                                filterContext.HttpContext.Session["LoginInfo"] = master[0];
                                filterContext.HttpContext.Session.Timeout = 20;
                            }
                        }
                    }
                    else
                    {
                        //Authority 权限session
                        UrlHelper url = new UrlHelper(filterContext.RequestContext);
                        string result = string.Format("<script type='text/javascript'> window.top.location = '" + url.Content("~/Login/Index") + "';</script>");
                        filterContext.Result = new ContentResult() { Content = result };
                        return;
                    }
                }
            }
        }
    }
}