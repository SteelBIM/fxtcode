using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kingsun.ExamPaper.BLL;
using Kingsun.ExamPaper.Common;
using Kingsun.ExamPaper.Model;
using Kingsun.ExamPaper.Wechat.api;
using Kingsun.ExamPaper.Wechat.Models;
using Kingsun.ExamPaper.WeChat.RelationService;

namespace Kingsun.ExamPaper.WeChat.ClassManagement
{
    public partial class SignIn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Oauth2 o2 = new Oauth2();
            //string appId = System.Configuration.ConfigurationManager.AppSettings["AppID"];
            //string wxAppId = System.Configuration.ConfigurationManager.AppSettings["WXappid"];
            //string appSecret = System.Configuration.ConfigurationManager.AppSettings["WXsecret"];

            //if (Request.QueryString["code"] != null && Request.QueryString["code"] != "")
            //{
            //    UserOpenBLL userBll = new UserOpenBLL();
            //    var code = Request.QueryString["code"];
            //    string openid = o2.CodeGetOpenid(wxAppId, appSecret, code);
            //    hfOpenId.Value = openid;
            //    string where = "OpenID='" + openid + "'";
            //    TB_UserOpenID openInfo = userBll.GetPhoneByOpenId(where);
            //    if (openInfo != null)
            //    {
            //        RelationService.RelationService relationService = new RelationService.RelationService();
            //        ReturnInfo result = relationService.GetUserInfoByUserID(openInfo.UserID.ToString());
            //        if (result.Success)
            //        {
            //            TB_UUMSUser userInfo = JsonHelper.DecodeJson<TB_UUMSUser>(result.Data.ToString());
            //            if (userInfo.UserType == 12)
            //            {
            //                Response.Redirect("ClassList.aspx?UserID=" + openInfo.UserID);
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    var resultStr = o2.GetCodeUrl(wxAppId, Request.Url.AbsoluteUri, "snsapi_base");
            //    Response.Redirect(resultStr);
            //}
        }
    }
}