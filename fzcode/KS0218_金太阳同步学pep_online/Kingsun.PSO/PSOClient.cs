using Kingsun.IBS.BLL;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.Model;
using Kingsun.SynchronousStudy.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

namespace Kingsun.PSO
{
    public class PSOClient : IHttpModule
    {

        IIBSData_AreaSchRelationBLL areaBLL = new IBSData_AreaSchRelationBLL();
        IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
        IIBSData_ClassUserRelationBLL classBLL = new IBSData_ClassUserRelationBLL();
        IIBSData_SchClassRelationBLL schBLL = new IBSData_SchClassRelationBLL();
        //#region IHttpModule 成员

        //public void Dispose()
        //{
        //    //throw new NotImplementedException();
        //}

        //public void Init(HttpApplication context)
        //{
        //    context.AcquireRequestState += new EventHandler(context_AcquireRequestState);
        //}

        ///// <summary>
        ///// 身份验证过程，第一次采用跳转验证，写入子系统本地Cookie，第二次采用Web服务调用，依附于本地Cookie
        ///// 并根据uums提供的登录状态和在线状态进行身份验证
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void context_AcquireRequestState(object sender, EventArgs e)
        //{
        //    HttpApplication app = sender as HttpApplication;
        //    HttpContext context = app.Context;
        //    IHttpHandler _IHttpHandler = context.CurrentHandler;
        //    Page _Page = _IHttpHandler as Page;
        //    if (_Page != null)
        //    {
        //        //判断是否需要验证，如果不需要返回

        //        if (!IsNeedValidate(context)) { return; }

        //        PSOCookie psoCookie = new PSOCookie(context);
        //        ClientUserinfo clientInfo = psoCookie.GetCookieUserInfo();

        //        //第一次登录子系统，采用脚本验证，并生成子系统Cookie信息
        //        if (clientInfo == null)
        //        {
        //            if (IsValidatePage(context))
        //            {
        //                ScriptValidate(context, true);
        //            }
        //            else
        //            {
        //                ScriptValidate(context, false);
        //            }
        //        }
        //        else//再次登录子系统，采用Web服务验证
        //        {
        //            WebServiceValidate(context, clientInfo);
        //        }
        //    }
        //}

        ///// <summary>
        ///// 脚本身份验证，通过签入的javascript脚本进行uums远程验证
        ///// </summary>
        ///// <param name="context"></param>
        //private void ScriptValidate(HttpContext context, bool isValidate)
        //{
        //    IHttpHandler _IHttpHandler = context.CurrentHandler;
        //    Page _Page = _IHttpHandler as Page;
        //    if (_Page != null)
        //    {
        //        ClientScriptManager csm = _Page.ClientScript;
        //        string appID = ConfigurationManager.AppSettings["AppID"].ToString();
        //        string uumsURL = ConfigurationManager.AppSettings["uumsRoot"] as string;
        //        string cookieName = ConfigurationManager.AppSettings["cookieName"] as string;
        //        string Root = ConfigurationManager.AppSettings["Root"] as string;
        //        string Loginurl = ConfigurationManager.AppSettings["LoginPage"] as string;
        //        bool isAdmin = IsAdminPage();
        //        if (isAdmin)
        //        {
        //            Loginurl = ConfigurationManager.AppSettings["AdminPage"] as string;
        //        }
        //        //string webServiceURL = ConfigurationManager.AppSettings["syncWebServiceUrl"].ToString() + "/AddUserLoginTimes?UserID=";

        //        StringBuilder scriptModule = new StringBuilder();
        //        scriptModule.Append(uumsURL);
        //        scriptModule.Append("/UserService/LoginUser.aspx?cookieID=");
        //        scriptModule.Append(cookieName);
        //        scriptModule.Append("&style=setCookie&isValidate=");
        //        scriptModule.Append(isValidate.ToString());
        //        scriptModule.Append("&appID=");
        //        scriptModule.Append(appID);
        //        scriptModule.Append("&backURL=");
        //        scriptModule.Append(GetBackURL(context));
        //        scriptModule.Append("&isAdmin=");
        //        scriptModule.Append(isAdmin.ToString());
        //        scriptModule.Append("&loginurl=");
        //        scriptModule.Append(context.Server.UrlEncode(Loginurl));

        //        csm.RegisterClientScriptInclude("checklogin", scriptModule.ToString());
        //    }
        //}

        ///// <summary>
        ///// Web服务验证，根据提供的用户编号和用户ID请求UUMS进行身份验证
        ///// </summary>
        //private void WebServiceValidate(HttpContext context, ClientUserinfo clientInfo)
        //{
        //    string appID = ConfigurationManager.AppSettings["AppID"] as string;
        //    //获取验证失败跳转页面
        //    UUMSService.FZUUMS_UserService uumsservice = new UUMSService.FZUUMS_UserService();
        //    //UUMSService.FZUUMS_Service uumsService = new FZUUMS_Service();
        //    //由于uums后台在线用户管理列表中显示用户登录的客户端IP地址时显示有误的问题,现在针对uums登录接口CheckLoginUserState(增加一个参数:'string clientIP')这个参数,各子系统的pso层在验证用户登录时与uums的相应的这个接口增加这个对数,该参数是各子系统获取到当前用户访问的客户端IP地址的值,把它传给uums
        //    //2009-9-7 由吴学伟提出。

        //    //用户ip地址。

        //    string clientIP = "";// context.Request.UserHostAddress;
        //    //clientIP = PSO.FrameWork.Common.GetIPAddress();
        //    uumsUserState returnState = uumsservice.CheckLoginUserState(clientInfo.UserID, clientInfo.UserNumber, appID, clientIP);

        //    switch (returnState)
        //    {
        //        case uumsUserState.notOnLine://用户未在线，验证通过
        //        case uumsUserState.selfOnline://用户是本用户在线，验证通过
        //            return;
        //        case uumsUserState.appForbidden:
        //            DeleteClientInfo(context, "AppNotExistOrForbidden");
        //            break;
        //        case uumsUserState.otherOnline://用户被其他用户在线，验证不通过
        //        case uumsUserState.notLogin://用户未登录,验证不通过
        //        default://其他情况验证不通过
        //            DeleteClientInfo(context, null);
        //            break;
        //    }
        //}

        ///// <summary>
        ///// 删除本地cookie和Sesison
        ///// </summary>
        //private void DeleteClientInfo(HttpContext context, string appID)
        //{
        //    IHttpHandler _IHttpHandler = context.CurrentHandler;
        //    Page _Page = _IHttpHandler as Page;
        //    if (_Page != null)
        //    {
        //        string isAdmin = IsAdminPage().ToString();

        //        ClientScriptManager csm = _Page.ClientScript;
        //        string uumsURL = ConfigurationManager.AppSettings["uumsRoot"].ToString();
        //        string cookieName = ConfigurationManager.AppSettings["cookieName"] as string;
        //        StringBuilder scriptModule = new StringBuilder();
        //        scriptModule.Append(uumsURL);
        //        scriptModule.Append("/UserService/LoginUser.aspx?cookieID=");
        //        scriptModule.Append(cookieName);
        //        scriptModule.Append("&style=deleteCookie&isValidate=");
        //        scriptModule.Append(IsValidatePage(context).ToString());
        //        scriptModule.Append("&appID=");
        //        scriptModule.Append(appID);
        //        scriptModule.Append("&backURL=");
        //        scriptModule.Append(GetBackURL(context));
        //        scriptModule.Append("&isAdmin=");
        //        scriptModule.Append(isAdmin);

        //        csm.RegisterClientScriptInclude("logOut", scriptModule.ToString());
        //    }
        //}


        //#region 内部函数
        ///// <summary>
        ///// 获取不需要PSO验证的页面文件名（过滤页面）
        ///// </summary>
        ///// <param name="context">HttpContext对象</param>
        ///// <returns>文件名列表</returns>
        //private string[] GetExceptPage(HttpContext context, string configName)
        //{
        //    string[] exceptPages;
        //    string PageList = ConfigurationManager.AppSettings[configName];
        //    if (PageList.IndexOf(',') != -1)
        //    {
        //        exceptPages = PageList.Split(',');
        //    }
        //    else
        //    {
        //        exceptPages = new string[1];
        //        exceptPages[0] = PageList;
        //    }
        //    return exceptPages;
        //}

        ///// <summary>
        ///// 判断当前页面是否需要验证，要验证返回true，不需要验证返回false
        ///// </summary>
        ///// <returns></returns>
        //public bool IsNeedValidate(HttpContext context)
        //{
        //    //获得不需要验证的网页列表
        //    string[] exceptPages = GetExceptPage(context, "Deny");
        //    //获取当前页面URL
        //    string[] pathList = context.Request.Url.Segments;
        //    foreach (string nextPage in exceptPages)
        //    {
        //        //如果该网页不需要登录验证，返回
        //        if (pathList.Contains(nextPage))
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        ///// <summary>
        ///// 判断当前页面是否是公共页面，如果是公共页面，进行已登录用户的信息更新，并穿透身份验证
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //public bool IsValidatePage(HttpContext context)
        //{
        //    //获得不需要验证的网页列表
        //    string[] exceptPages = GetExceptPage(context, "ValidatePage");
        //    //获取当前页面URL            
        //    string[] pathList = context.Request.Url.Segments;

        //    foreach (string nextPage in exceptPages)
        //    {
        //        //如果该网页不需要登录验证，返回
        //        if (pathList.Contains(nextPage))
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        ///// <summary>
        ///// 判断是否是后台页面
        ///// </summary>
        ///// <returns></returns>
        //private bool IsAdminPage()
        //{
        //    bool isAdmin = false;
        //    //配置文件中配置目录
        //    string[] adminlist = ConfigurationManager.AppSettings["IsAdmin"].ToString().Split(',');
        //    if (adminlist == null || adminlist.Length == 0)
        //    {
        //        return isAdmin;
        //    }
        //    foreach (string s in adminlist)
        //    {
        //        if (HttpContext.Current.Request.Url.ToString().ToLower().IndexOf(s.ToLower()) > -1)
        //        {
        //            isAdmin = true;
        //            break;
        //        }
        //    }
        //    return isAdmin;
        //}

        ///// <summary>
        ///// 获得返回地址，左边菜单页面过滤
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //public string GetBackURL(HttpContext context)
        //{
        //    //string[] needReplacePage = GetExceptPage(context, "needReplacePage");
        //    //string aimPage = ConfigurationManager.AppSettings["aimPage"] as string;
        //    //string backURL = string.Empty;
        //    //string currengPage = context.Request.Url.Segments[context.Request.Url.Segments.Length - 1].ToLower();
        //    //if(currengPage == "left.aspx" || currengPage == "top.aspx")
        //    //{
        //    //    backURL = ReplacePage(context);
        //    //}
        //    //else if (needReplacePage.Contains(currengPage))
        //    //{
        //    //    backURL = aimPage;
        //    //}
        //    //else
        //    //{
        //    //    backURL = context.Request.Url.ToString();
        //    //}

        //    //return backURL;
        //    string backURL = string.Empty;
        //    string[] requesturl = context.Request.Url.Segments;
        //    if (requesturl.Contains("Left.aspx") || requesturl.Contains("Top.aspx") || requesturl.Contains("Welcome.aspx"))
        //    {
        //        backURL = "http://" + context.Request.ServerVariables["http_host"];
        //        for (int i = 0; i < context.Request.Url.Segments.Length - 2; i++)
        //        {
        //            backURL += context.Request.Url.Segments[i];
        //        }
        //        backURL += "Index.aspx";
        //    }
        //    else if (context.Request.Url.Segments.Contains("logout.aspx"))
        //    {
        //        backURL = string.Empty;
        //    }
        //    else
        //    {

        //        backURL = context.Request.Url.ToString();
        //        if (backURL.IndexOf("?backurl=") > -1)
        //        {
        //            backURL = backURL.Substring(0, backURL.IndexOf("?backurl="));
        //        }
        //    }
        //    return backURL;
        //    //return context.Server.UrlEncode(backURL);
        //}

        ///// <summary>
        ///// 网址替换
        ///// </summary>
        ///// <param name="context"></param>
        ///// <param name="replcacePage"></param>
        ///// <returns></returns>
        //private string ReplacePage(HttpContext context)
        //{
        //    string backURL = "http://" + context.Request.ServerVariables["http_host"];

        //    if (context.Request.Url.ToString().ToLower().IndexOf("fzsyncclass") > 0)
        //    {
        //        backURL += "/Index.aspx";
        //    }
        //    else
        //    {
        //        backURL += "/Index.aspx";
        //    }
        //    return backURL;
        //}
        //#endregion

        //#endregion
        #region IHttpModule 成员

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public void Init(HttpApplication context)
        {
            context.AcquireRequestState += new EventHandler(context_AcquireRequestState);
        }

        /// <summary>
        /// 身份验证过程，第一次采用跳转验证，写入子系统本地Cookie，第二次采用Web服务调用，依附于本地Cookie
        /// 并根据uums提供的登录状态和在线状态进行身份验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void context_AcquireRequestState(object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;
            HttpContext context = app.Context;
            IHttpHandler _IHttpHandler = context.CurrentHandler;
            Page _Page = _IHttpHandler as Page;
            if (_Page != null)
            {
                //判断是否需要验证，如果不需要返回

                if (!IsNeedValidate(context)) { return; }

                PSOCookie psoCookie = new PSOCookie(context);
                ClientUserinfo clientInfo = psoCookie.GetCookieUserInfo();

                //第一次登录子系统，采用脚本验证，并生成子系统Cookie信息
                if (clientInfo == null)
                {
                    if (IsValidatePage(context))
                    {
                        ScriptValidate(context, true);
                    }
                    else
                    {
                        ScriptValidate(context, false);
                    }
                }
                else//再次登录子系统，采用Web服务验证
                {
                    //开发过程中注销-肖超20160112
                    bool result = false;
                    string linkurl = context.Request.Url.LocalPath.ToLower();
                    //首页不做验证
                    string[] notVerify = new string[] { "/index.aspx", "/welcome.aspx", "loginout.aspx" };
                    if (notVerify.Contains(linkurl) || context.Request.Url.ToString().ToLower().Contains("/usercontrol/")
                        || context.Request.Url.ToString().ToLower().Contains("/wechat/"))
                    {
                        result = true;
                    }
                    else
                    {  //判断访问权限    
                        //Kingsun.PSO.UUMSService.FZUUMS_UserService service = new PSO.UUMSService.FZUUMS_UserService();
                        //Kingsun.PSO.UUMSService.V_GroupPower[] powerlist = service.GetUserPowerList(clientInfo.UserID, ConfigurationManager.AppSettings["AppID"]);
                        IList<PowerList> powerlist = (IList<PowerList>)context.Session["UserPower"];
                       
                        if (powerlist == null || powerlist.Count == 0)
                        {

                            powerlist = userBLL.GetUserPowerList(clientInfo.UserID, ConfigurationManager.AppSettings["AppID"]);
                            context.Session["UserPower"] = powerlist;
                        }
                        if (powerlist != null)
                        {
                            foreach (var vgp in powerlist)
                            {
                                if (vgp.LinkUrl == linkurl)
                                {
                                    result = true;
                                }
                            }
                        }
                        //if (powerlist != null)
                        //{
                        //    foreach (Kingsun.PSO.UUMSService.V_GroupPower vgp in powerlist)
                        //    {
                        //        if (vgp.LinkUrl == linkurl)
                        //        {
                        //            result = true;
                        //        }
                        //    }
                        //}
                    }
                    if (!result)
                    {
                        context.Response.AddHeader("Content-Type", "text/html;charset=UTF-8");
                        context.Response.Write("您没有访问该页面的权限");
                       // context.Response.End();
                    }


                    WebServiceValidate(context, clientInfo);
                }
            }
        }

        /// <summary>
        /// 脚本身份验证，通过签入的javascript脚本进行uums远程验证
        /// </summary>
        /// <param name="context"></param>
        private void ScriptValidate(HttpContext context, bool isValidate)
        {
            IHttpHandler _IHttpHandler = context.CurrentHandler;
            Page _Page = _IHttpHandler as Page;
            if (_Page != null)
            {
                ClientScriptManager csm = _Page.ClientScript;
                string appID = ConfigurationManager.AppSettings["AppID"].ToString();
                string uumsURL = ConfigurationManager.AppSettings["uumsRoot"] as string;
                string cookieName = ConfigurationManager.AppSettings["cookieName"] as string;
                string Root = ConfigurationManager.AppSettings["Root"] as string;
                string Loginurl = ConfigurationManager.AppSettings["LoginPage"] as string;
                bool isAdmin = IsAdminPage();
                if (isAdmin)
                {
                    Loginurl = ConfigurationManager.AppSettings["AdminPage"] as string;
                }
                //string webServiceURL = ConfigurationManager.AppSettings["syncWebServiceUrl"].ToString() + "/AddUserLoginTimes?UserID=";

                StringBuilder scriptModule = new StringBuilder();
                scriptModule.Append(uumsURL);
                scriptModule.Append("/UserService/LoginUser.aspx?cookieID=");
                scriptModule.Append(cookieName);
                scriptModule.Append("&style=setCookie&isValidate=");
                scriptModule.Append(isValidate.ToString());
                scriptModule.Append("&appID=");
                scriptModule.Append(appID);
                scriptModule.Append("&backURL=");
                scriptModule.Append(GetBackURL(context));
                scriptModule.Append("&isAdmin=");
                scriptModule.Append(isAdmin.ToString());
                scriptModule.Append("&loginurl=");
                scriptModule.Append(context.Server.UrlEncode(Loginurl));
                csm.RegisterClientScriptInclude("checklogin", scriptModule.ToString());
            }
        }

        /// <summary>
        /// Web服务验证，根据提供的用户编号和用户ID请求UUMS进行身份验证
        /// </summary>
        private void WebServiceValidate(HttpContext context, ClientUserinfo clientInfo)
        {

            string appID = ConfigurationManager.AppSettings["AppID"] as string;
            //获取验证失败跳转页面

            //UUMSService.FZUUMS_Service uumsService = new FZUUMS_Service();
            //由于uums后台在线用户管理列表中显示用户登录的客户端IP地址时显示有误的问题,现在针对uums登录接口CheckLoginUserState(增加一个参数:'string clientIP')这个参数,各子系统的pso层在验证用户登录时与uums的相应的这个接口增加这个对数,该参数是各子系统获取到当前用户访问的客户端IP地址的值,把它传给uums
            //2009-9-7 由吴学伟提出。

            //用户ip地址。

            string clientIP = "";// context.Request.UserHostAddress;
            //clientIP = PSO.FrameWork.Common.GetIPAddress();
            UserStateEnum returnState = userBLL.CheckLoginUserState(clientInfo.UserID, clientInfo.UserNumber, appID, clientIP);

            switch (returnState)
            {
                case UserStateEnum.notOnLine://用户未在线，验证通过
                case UserStateEnum.selfOnline://用户是本用户在线，验证通过
                    return;
                case UserStateEnum.appForbidden:
                    DeleteClientInfo(context, "AppNotExistOrForbidden");
                    break;
                case UserStateEnum.otherOnline://用户被其他用户在线，验证不通过
                case UserStateEnum.notLogin://用户未登录,验证不通过
                default://其他情况验证不通过
                    DeleteClientInfo(context, null);
                    break;
            }
        }

        /// <summary>
        /// 删除本地cookie和Sesison
        /// </summary>
        private void DeleteClientInfo(HttpContext context, string appID)
        {
            IHttpHandler _IHttpHandler = context.CurrentHandler;
            Page _Page = _IHttpHandler as Page;
            if (_Page != null)
            {
                string isAdmin = IsAdminPage().ToString();

                ClientScriptManager csm = _Page.ClientScript;
                string uumsURL = ConfigurationManager.AppSettings["uumsRoot"].ToString();
                string cookieName = ConfigurationManager.AppSettings["cookieName"] as string;
                StringBuilder scriptModule = new StringBuilder();
                scriptModule.Append(uumsURL);
                scriptModule.Append("/UserService/LoginUser.aspx?cookieID=");
                scriptModule.Append(cookieName);
                scriptModule.Append("&style=deleteCookie&isValidate=");
                scriptModule.Append(IsValidatePage(context).ToString());
                scriptModule.Append("&appID=");
                scriptModule.Append(appID);
                scriptModule.Append("&backURL=");
                scriptModule.Append(GetBackURL(context));
                scriptModule.Append("&isAdmin=");
                scriptModule.Append(isAdmin);

                csm.RegisterClientScriptInclude("logOut", scriptModule.ToString());
            }
        }


        #region 内部函数
        /// <summary>
        /// 获取不需要PSO验证的页面文件名（过滤页面）
        /// </summary>
        /// <param name="context">HttpContext对象</param>
        /// <returns>文件名列表</returns>
        private string[] GetExceptPage(HttpContext context, string configName)
        {
            string[] exceptPages;
            string PageList = ConfigurationManager.AppSettings[configName];
            if (PageList.IndexOf(',') != -1)
            {
                exceptPages = PageList.Split(',');
            }
            else
            {
                exceptPages = new string[1];
                exceptPages[0] = PageList;
            }
            return exceptPages;
        }

        /// <summary>
        /// 判断当前页面是否需要验证，要验证返回true，不需要验证返回false
        /// </summary>
        /// <returns></returns>
        public bool IsNeedValidate(HttpContext context)
        {
            //获得不需要验证的网页列表
            string[] exceptPages = GetExceptPage(context, "Deny");
            //获取当前页面URL
            string[] pathList = context.Request.Url.Segments;
            foreach (string nextPage in exceptPages)
            {
                //如果该网页不需要登录验证，返回
                if (pathList.Contains(nextPage, StringComparer.OrdinalIgnoreCase))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 判断当前页面是否是公共页面，如果是公共页面，进行已登录用户的信息更新，并穿透身份验证
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool IsValidatePage(HttpContext context)
        {
            //获得不需要验证的网页列表
            string[] exceptPages = GetExceptPage(context, "ValidatePage");
            //获取当前页面URL            
            string[] pathList = context.Request.Url.Segments;

            foreach (string nextPage in exceptPages)
            {
                //如果该网页不需要登录验证，返回
                if (pathList.Contains(nextPage, StringComparer.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断是否是后台页面
        /// </summary>
        /// <returns></returns>
        private bool IsAdminPage()
        {
            bool isAdmin = false;
            //配置文件中配置目录
            string[] adminlist = ConfigurationManager.AppSettings["IsAdmin"].ToString().Split(',');
            if (adminlist == null || adminlist.Length == 0)
            {
                return isAdmin;
            }
            foreach (string s in adminlist)
            {
                if (HttpContext.Current.Request.Url.ToString().ToLower().IndexOf(s.ToLower()) > -1)
                {
                    isAdmin = true;
                    break;
                }
            }
            return isAdmin;
        }

        /// <summary>
        /// 获得返回地址，左边菜单页面过滤
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string GetBackURL(HttpContext context)
        {
            //string[] needReplacePage = GetExceptPage(context, "needReplacePage");
            //string aimPage = ConfigurationManager.AppSettings["aimPage"] as string;
            //string backURL = string.Empty;
            //string currengPage = context.Request.Url.Segments[context.Request.Url.Segments.Length - 1].ToLower();
            //if(currengPage == "left.aspx" || currengPage == "top.aspx")
            //{
            //    backURL = ReplacePage(context);
            //}
            //else if (needReplacePage.Contains(currengPage))
            //{
            //    backURL = aimPage;
            //}
            //else
            //{
            //    backURL = context.Request.Url.ToString();
            //}

            //return backURL;
            string backURL = string.Empty;
            string[] requesturl = context.Request.Url.Segments;
            if (requesturl.Contains("Left.aspx") || requesturl.Contains("Top.aspx") || requesturl.Contains("Welcome.aspx") || requesturl.Contains("APPManagement.aspx"))
            {
                backURL = "http://" + context.Request.ServerVariables["http_host"];
                for (int i = 0; i < context.Request.Url.Segments.Length - 2; i++)
                {
                    backURL += context.Request.Url.Segments[i];
                }
                backURL += "/Index.aspx";
            }
            else if (context.Request.Url.Segments.Contains("logout.aspx"))
            {
                backURL = string.Empty;
            }
            else
            {

                backURL = context.Request.Url.ToString();
                if (backURL.IndexOf("?backurl=") > -1)
                {
                    backURL = backURL.Substring(0, backURL.IndexOf("?backurl="));
                }
            }

            return context.Server.UrlEncode(backURL);
        }

        /// <summary>
        /// 网址替换
        /// </summary>
        /// <param name="context"></param>
        /// <param name="replcacePage"></param>
        /// <returns></returns>
        private string ReplacePage(HttpContext context)
        {
            string backURL = "http://" + context.Request.ServerVariables["http_host"];

            if (context.Request.Url.ToString().ToLower().IndexOf("fzsyncclass") > 0)
            {
                backURL += "/Index.aspx";
            }
            else
            {
                backURL += "/Index.aspx";
            }
            return backURL;
        }
        #endregion

        #endregion
    }
}
