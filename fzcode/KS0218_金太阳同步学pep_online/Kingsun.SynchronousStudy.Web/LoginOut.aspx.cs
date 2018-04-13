using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kingsun.PSO;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.BLL;

namespace Kingsun.SynchronousStudy.Web
{
    public partial class LoginOut : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
            HttpContext context = HttpContext.Current;
            Kingsun.PSO.PSOCookie psoCookie = new Kingsun.PSO.PSOCookie(HttpContext.Current);
            ClientUserinfo userinfo = psoCookie.GetCookieUserInfo();
            if (userinfo == null)
            {
                Response.Write("LogOutSucceed");
                Response.End();
                return;
            }
            string message = "";
            string result = userBLL.LoginOut(System.Configuration.ConfigurationManager.AppSettings["AppID"], userinfo.UserID, out message);
            Session.RemoveAll();
            if (string.IsNullOrEmpty(message))
            {
                psoCookie.DeleteAllUserCookie();
                Response.Write("LogOutSucceed");
                Response.End();
            }
            else
            {
                Response.Write(message);
                Response.End();
            }
        }
    }
}