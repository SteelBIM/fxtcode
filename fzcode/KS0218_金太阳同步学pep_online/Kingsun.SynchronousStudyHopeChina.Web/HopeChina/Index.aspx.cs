using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kingsun.PSO;
using Kingsun.SynchronousStudy.Common;

namespace Kingsun.SynchronousStudyHopeChina.Web.HopeChina
{
    public partial class Index : System.Web.UI.Page
    {
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
                Kingsun.PSO.UUMSService.FZUUMS_UserService service = new Kingsun.PSO.UUMSService.FZUUMS_UserService();
                string result = service.LoginOut(AppSetting.AppID, userinfo.UserID, out message);
                Session.RemoveAll();
                if (string.IsNullOrEmpty(message))
                {
                    psoCookie.DeleteAllUserCookie();
                }
            }
        }


    }
}