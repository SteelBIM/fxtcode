using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kingsun.ExamPaper.Common;
using Kingsun.PSO;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.BLL;

namespace Kingsun.SynchronousStudy.Web
{
    public class CheckLogin
    {
        private static IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
        public static ClientUserinfo Check(HttpContext context)
        {
            ClientUserinfo UserInfo = new ClientUserinfo();
            PSOCookie cookie = new PSOCookie(HttpContext.Current);
            UserInfo = cookie.GetCookieUserInfo();
            if (UserInfo == null)
            {
                UserInfo = new ClientUserinfo();
                UserInfo.UserID = "";
                //context.Response.Redirect("~/Login.aspx?backurl=" + context.Request.RawUrl);
            }
            // Kingsun.PSO.UUMSService.FZUUMS_UserService service = new PSO.UUMSService.FZUUMS_UserService();
            // //FZUUMS.UserService.FZUUMS_UserService uumsservice = new UserService.FZUUMS_UserService();
            //// string menuHtml = service.GetUserPower(UserInfo.UserID, AppSetting.AppID);
            // string menuList = service.GetUserPowerID(UserInfo.UserID, AppSetting.AppID);

            // if (string.IsNullOrEmpty(menuList)||!menuList.Contains(ConfigurationManager.AppSettings["Mnum"]))
            // {
            //     context.Response.Write("无权限访问此页面");
            //     context.Response.End();
            // }

            return UserInfo;
        }


        public static ClientUserinfo Check(HttpContext context, ref string menuList)
        {
            ClientUserinfo UserInfo = new ClientUserinfo();

            PSOCookie cookie = new PSOCookie(HttpContext.Current);
            UserInfo = cookie.GetCookieUserInfo();
            if (UserInfo == null)
            {
                UserInfo = new ClientUserinfo();
                UserInfo.UserID = "";
                // context.Response.Redirect("~/Login.aspx?backurl=" + context.Request.RawUrl);
            }
            //FZUUMS.UserService.FZUUMS_UserService uumsservice = new UserService.FZUUMS_UserService();
            // menuHtml = service.GetUserPower(UserInfo.UserID, AppSetting.AppID);
            menuList = userBLL.GetUserPowerID(UserInfo.UserID, AppSetting.AppID);
            //if (string.IsNullOrEmpty(menuList)||!menuList.Contains(ConfigurationManager.AppSettings["Mnum"]))
            //{
            //    context.Response.Write("无权限访问此页面");
            //}

            return UserInfo;
        }
    }
}