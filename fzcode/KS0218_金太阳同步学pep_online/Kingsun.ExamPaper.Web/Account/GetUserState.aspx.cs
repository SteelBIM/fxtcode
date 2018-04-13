using System;
using System.Web;
//using Kingsun.SunnyTask.BLL;
//using Kingsun.SunnyTask.Common;
//using Kingsun.SunnyTask.Model;

namespace Kingsun.SunnyTask.Web.Account
{
    public partial class GetUserState : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string Action = "";
            if (string.IsNullOrEmpty(Request.QueryString["Action"]))
            {
                Response.Write("");
                Response.End();
            }
            Action = Request.QueryString["Action"];
            if (Action == "GetUserInfo")
            {
                //Kingsun.PSO.PSOCookie cookie = new PSO.PSOCookie(HttpContext.Current);
                //Kingsun.PSO.ClientUserinfo clientUser = cookie.GetCookieUserInfo();
                //if (clientUser == null)
                //{
                //    Response.Write("");
                //    Response.End();
                //    return;
                //}
                //Tb_UserInfo userInfo = new UserInfoBLL().SyncUserInfo(clientUser.UserID, 1);
                ////var obj = new { UserName = clientUser.UserName, UserID = clientUser.UserID, AvatarUrl = clientUser.AvatarUrl, TrueName = clientUser.TrueName };
                //if (userInfo != null && userInfo.UserRoles == 12)
                //{
                //    int count = 0;
                //    count = new SunnyTeachBLL().GetMsgCount(clientUser.UserID);
                //    var obj = new { UserInfo = userInfo, MsgCount = count };
                //    Response.Write(JsonHelper.EncodeJson(obj));
                //    Response.End();
                //}
                //else {
                //    Response.Write(JsonHelper.EncodeJson(userInfo));
                //    Response.End();
                //}
            }
        }
    }
}