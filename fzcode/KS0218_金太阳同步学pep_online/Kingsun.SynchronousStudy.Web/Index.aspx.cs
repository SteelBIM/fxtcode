using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kingsun.PSO;
using Kingsun.SynchronousStudy.Common;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.BLL;

namespace Kingsun.SynchronousStudy.Web
{
    public partial class Index : System.Web.UI.Page
    {
        IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
        public string menuHtml = "";
        public string menuList = "";
        public string info = "";
        public ClientUserinfo UserInfo = new ClientUserinfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            UserInfo = CheckLogin.Check(HttpContext.Current, ref menuList);
            info = UserInfo.UserName;
        }

        protected void aClose_Click(object sender, EventArgs e)
        {
            Kingsun.PSO.PSOCookie psoCookie = new Kingsun.PSO.PSOCookie(HttpContext.Current);
            Kingsun.PSO.ClientUserinfo userinfo = psoCookie.GetCookieUserInfo();
            if (userinfo == null)
            {
                return;
            }
            else
            {
                string message = "";
                string result = userBLL.LoginOut(AppSetting.AppID, userinfo.UserID, out message);
                Session.RemoveAll();
                if (string.IsNullOrEmpty(message))
                {
                    psoCookie.DeleteAllUserCookie();
                }
            }
        }


    }
}