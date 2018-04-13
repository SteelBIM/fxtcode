using System;
using System.Configuration;
using System.Web;
//using Kingsun.PSO;
//using Kingsun.SunnyTask.Common;

namespace Kingsun.SunnyTask.Web.Account
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session != null && Session["PermPages"] != null)
            {
                Session["PermPages"] = null;
            }
            HttpContext context = HttpContext.Current;
            //Kingsun.PSO.PSOCookie psoCookie = new Kingsun.PSO.PSOCookie(HttpContext.Current);

            //string directURL = ConfigurationManager.AppSettings["uumsRoot"].ToString() + "/UserService/LoginOut.aspx?";
            //ClientUserinfo userinfo = psoCookie.GetCookieUserInfo();
            //if (userinfo == null)
            //{
            //    Response.Write("LogOutSucceed");
            //    Response.End();
            //    return;
            //}
            //string message = "";
            //PSO.UUMSService.FZUUMS_UserService service = new PSO.UUMSService.FZUUMS_UserService();
            //string result = service.LoginOut(AppSetting.AppID, userinfo.UserID, out message);
            //Session.RemoveAll();
            //if (string.IsNullOrEmpty(message))
            //{
            //    psoCookie.DeleteAllUserCookie();
            //    Response.Write("LogOutSucceed");
            //    Response.End();
            //}
            //else
            //{
            //    Response.Write(message);
            //}
        }
    }
}