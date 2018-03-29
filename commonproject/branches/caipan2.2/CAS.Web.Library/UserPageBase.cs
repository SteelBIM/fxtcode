using System;
using CAS.Common;
using CAS.Entity.DBEntity;
using System.Text;
using System.Web;
namespace CAS.Web.Library
{
    /// <summary>
    /// 登录后进入的页面基类，需要实现登录状态检查代码
    /// </summary>
    public class UserPageBase:PageBase
    {
        protected string loginerror="";
        protected override void OnLoad(EventArgs e)
        {           
            //不能用全局SISSION，因为api是不同域
            //转向登录页，完成登录后把登录的账号及令牌传过来，保存到本域的SESSION中
            //本域的session过期就转向登录页重新登录，重复上一步
            Public.NeedReLogin = false;
            if (Session["loginerror"] != null)
            {
                loginerror = Session["loginerror"].ToString();
                Session["loginerror"] = null;
            }
            if (Public.LoginInfo.userid.Length <= 0) 
            {
                //传入加密后的userid和token，解密userid，然后进数据库找对应的登录记录。
                if (GetRequest("token").Length > 0)
                {
                    string token = GetRequest("token");
                    string cityid = GetRequest("cityid");
                    //从数据库判断登录状态。
                    //放入本域session中,LoginInfo从session中取值。
                    //这里读取登录信息
                    //令牌应该只能使用一次，使用过后api会删除，不可再用                    
                    string jsonResult = Public.APIPostBackInCSharp("user.logincheck", "token=" + token + "&cityid=" + cityid);
                    JSONHelper.ReturnData data = JSONHelper.JSONToObject<JSONHelper.ReturnData>(jsonResult);
                    if (data.returntype == 1)
                    {
                        string j = JSONHelper.ObjectToJSON(data.data);
                        LoginUserAndCity user = JSONHelper.JSONToObject<LoginUserAndCity>(j);
                        if (!string.IsNullOrEmpty(user.validstatus))
                            Session["loginerror"] = user.validstatus;
                        else
                            Public.SetLoginInfo(user);
                    }
                    else
                    {
                        Session["loginerror"] = "可能是账号未审核。";
                    }
                    //转向目标页
                    string url = StringHelper.RemoveUrlParameters(Context.Request.RawUrl, new string[] { "token", "cityid" });
                    Context.Response.Redirect(url.Replace("default.aspx", ""));
                    return;
                }
                else
                {
                    //这里是登录，为避免URL参数暴露，改为post方式。
                    //避免IFRAME页转登录页，判断是否为子页，子页调用js来转向 kevin
                    string url = "http://" + Public.RootUrlFull + "default.aspx";
                    if (Context.Request.Url.AbsoluteUri.ToLower() == url.ToLower())
                        Login();
                    else
                        Public.NeedReLogin = true;
                    return;
                }
            }
            base.OnLoad(e);
        }

       
        /// <summary>
        /// 登录代码
        /// </summary>
        protected void Login()
        {
            string url = StringHelper.RemoveUrlParameters(Context.Request.Url.ToString(), new string[] { "token", "cityid" });
            string posts = "type=post&url=page/common/login.aspx&issingle=" + Public.CompanyFxt.issingle.ToString() + "&systypecode=" + Public.SysTypeCode.ToString() + "&tourl=" + HttpUtility.UrlEncode(url);
            if (loginerror != "") { posts += "&loginerror=" + loginerror; }
            //string result = Public.APIPostBack("http://" + Public.RootUrlFull + "handlers/api.ashx", posts, false);
            string result = Public.APIPostBack(CAS.Common.WebCommon.GetConfigSetting("apiurl") + "page/common/login.aspx", posts, true);
            //string str = @"";
            Response.Write(result);
            Response.End();
        }
        /* 这里改了么，还没有定吧？，用户还没有集成，进old先用/old kevin
        protected void Login() {
            Response.Redirect("/old/default.aspx");
            Response.End();
        }
         * */
    }
}