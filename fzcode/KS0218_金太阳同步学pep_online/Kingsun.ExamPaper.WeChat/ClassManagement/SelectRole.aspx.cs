using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kingsun.ExamPaper.BLL;
using Kingsun.ExamPaper.Common;
using Kingsun.ExamPaper.Model;
using Kingsun.ExamPaper.Wechat.api;

namespace Kingsun.ExamPaper.WeChat.ClassManagement
{
    public partial class SelectRole : System.Web.UI.Page
    {
        string appId = System.Configuration.ConfigurationManager.AppSettings["AppID"];
        string wxAppId = System.Configuration.ConfigurationManager.AppSettings["WXappid"];
        string appSecret = System.Configuration.ConfigurationManager.AppSettings["WXsecret"];
        FZUUMS_UserService.FZUUMS_UserService UUMSService = new FZUUMS_UserService.FZUUMS_UserService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["code"] != null && Request.QueryString["code"] != "")
            {
                Oauth2 o2 = new Oauth2();
                UserOpenBLL userBll = new UserOpenBLL();
                var code = Request.QueryString["code"];
                string openid = o2.CodeGetOpenid(wxAppId, appSecret, code);
                string where = "OpenID='" + openid + "'";
                TB_UserOpenID openInfo = userBll.GetPhoneByOpenId(where);
                if (openInfo != null)
                {
                    hfOpenId.Value = openid;
                    hfUserid.Value = openInfo.UserID.ToString();
                }
            }
            else
            {
                Oauth2 o2 = new Oauth2();
                var resultStr = o2.GetCodeUrl(wxAppId, Request.Url.AbsoluteUri, "snsapi_base");
                Response.Redirect(resultStr);
            }
        }

        protected void btnTea_Click(object sender, EventArgs e)
        {
            FZUUMS_UserService.User fzUser = new FZUUMS_UserService.User
            {
                UserID = hfUserid.Value.ToString(),
                UserType = 12
            };
            FZUUMS_UserService.ReturnInfo ri = UUMSService.UpdateUserInfo2(appId, fzUser);
            if (ri.Success)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "", "<script>alert('修改身份为老师')</script>");
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "", "<script>alert('修改失败,原因为：" + ri.ErrorMsg + "')</script>");
            }
        }

        protected void btnStu_Click(object sender, EventArgs e)
        {
            FZUUMS_UserService.User fzUser = new FZUUMS_UserService.User
            {
                UserID = hfUserid.Value.ToString(),
                UserType = 26
            };
            FZUUMS_UserService.ReturnInfo ri = UUMSService.UpdateUserInfo2(appId, fzUser);
            if (ri.Success)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "", "<script>alert('修改身份为学生')</script>");
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "", "<script>alert('修改失败,原因为：" + ri.ErrorMsg + "')</script>");
            }
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            FZUUMS_UserService.User fzUser = new FZUUMS_UserService.User
            {
                UserID = hfUserid.Value.ToString(),
                SchoolID = 0,
                SchoolName = ""
            };
            FZUUMS_UserService.ReturnInfo ri = UUMSService.UpdateUserInfo2(appId, fzUser);
            if (ri.Success)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "", "<script>alert('成功学校信息清空')</script>");
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "", "<script>alert('修改失败,原因为：" + ri.ErrorMsg + "')</script>");
            }
        }

        protected void btnOut_Click(object sender, EventArgs e)
        {
            string sql = string.Format(@"DELETE FROM [dbo].[TB_UserOpenID] WHERE OpenID='{0}'", hfOpenId.Value);
            if (SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql) > 0)
            {
                ClientScript.RegisterStartupScript(GetType(), "tishi", "<script type=\"text/javascript\">alert('退出登录！');</script>");
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "", "<script>alert('修改失败,原因为：OpenID=" + hfOpenId.Value + "')</script>");
            }
        }
    }
}