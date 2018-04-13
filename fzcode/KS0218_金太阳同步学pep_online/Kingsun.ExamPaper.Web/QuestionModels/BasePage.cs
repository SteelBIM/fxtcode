using Kingsun.ExamPaper.Common;
using System;
using System.Web;
//using Kingsun.SunnyTask.BLL;
//using Kingsun.SunnyTask.Common;
//using Kingsun.SunnyTask.Model;
//using StackExchange.Redis;

namespace Kingsun.SunnyTask.Web
{
    public class BasePage : System.Web.UI.Page
    {
        public BasePage()
        {
            this.Load += new EventHandler(BasePage_Load);
         
        }
        //UserInfoBLL userbll = new UserInfoBLL();
        //public Kingsun.PSO.ClientUserinfo clientUser = null;
        //public Tb_UserInfo CurrentUserInfo = null;
        //public QTb_Subject CurrentSubject = null;//记录从优教学首页进入作业时的学科

        public void WriteResult(object data, string message = "")
        {
            object obj = new { Success = true, Data = data, Message = message };
            Response.Write(JsonHelper.EncodeJson(obj));
            Response.End();
        }
        public void WriteErrorResult(string message = "")
        {
            object obj = new { Success = false, Data = "", Message = message };
            Response.Write(JsonHelper.EncodeJson(obj));
            Response.End();
        }

        public void BasePage_Load(object sender, EventArgs e)
        {          
            

            //Kingsun.PSO.PSOCookie cookie = new PSO.PSOCookie(HttpContext.Current);
            //clientUser = cookie.GetCookieUserInfo();
            //if (clientUser != null)
            //{
            //    CurrentUserInfo = userbll.SyncUserInfo(clientUser.UserID);
            //    if (CurrentUserInfo == null)
            //    {
            //        //CurrentUserInfo = new Tb_UserInfo();
            //        return;
            //    }
            //    else
            //    {
            //        CurrentSubject = new SunnyTeachBLL().GetCurrentSubject(CurrentUserInfo.UserID);
            //    }

            //    string requestUrl = HttpContext.Current.Request.Url.ToString();
            //    string RedisPathNew = System.Configuration.ConfigurationManager.ConnectionStrings["RedisPath"].ConnectionString;
            //    //用户为教师时
            //    if (CurrentUserInfo.UserRoles.Value == 12)
            //    {
            //        if (requestUrl.ToLower().IndexOf("mathmodel") < 0 && requestUrl.ToLower().IndexOf("chinesemodel") < 0 &&
            //            requestUrl.ToLower().IndexOf("questionmodel") < 0 && requestUrl.ToLower().IndexOf("admin/") < 0
            //            && requestUrl.ToLower().IndexOf("others/") < 0 && requestUrl.ToLower().IndexOf("weixin/") < 0
            //            && requestUrl.ToLower().IndexOf("taskquepreview") < 0)
            //        {
            //            if (!(new SunnyTeachBLL().CheckPemission(CurrentUserInfo.UserID, HttpContext.Current.Request.Url.LocalPath.ToLower())))
            //            {
            //                Response.Write("你没有权限访问当前页面");
            //                Response.End();
            //            }
            //        }
            //        if (requestUrl.ToLower().IndexOf("student/") >= 0)
            //        {
            //            HttpContext.Current.Response.Redirect("../Teacher/ClassTaskList.aspx");
            //        }
            //        //判断redis连接状态
            //        if (requestUrl.ToLower().IndexOf("teacher/classtaskofstu.aspx") >= 0)
            //        {
            //            try
            //            {
            //                if (!RedisClusterManager.IsConnected)
            //                {
            //                    System.Web.HttpContext.Current.Response.Write("<script>alert('服务器连接失败，请重试!');</script>");
            //                    HttpContext.Current.Response.Redirect("../Teacher/ClassTaskList.aspx");
            //                }
            //            }
            //            catch (Exception)
            //            {
            //                System.Web.HttpContext.Current.Response.Write("<script>alert('服务器连接失败，请重试!');</script>");
            //                HttpContext.Current.Response.Redirect("../Teacher/ClassTaskList.aspx");
            //            }
            //        }
            //    }
            //    //用户为学生时
            //    else if (CurrentUserInfo.UserRoles.Value == 26)
            //    {
            //        if (requestUrl.ToLower().IndexOf("teacher/") >= 0)
            //        {
            //            HttpContext.Current.Response.Redirect("../Student/StuTaskList.aspx");
            //        }
            //        //判断redis连接状态
            //        if (requestUrl.ToLower().IndexOf("mathmodels/") >= 0 || requestUrl.ToLower().IndexOf("chinesemodels/") >= 0 
            //            || requestUrl.ToLower().IndexOf("questionmodels/") >= 0 || requestUrl.ToLower().IndexOf("student/stutasklist.aspx") >= 0)
            //        {
            //            try
            //            {
            //                if (!RedisClusterManager.IsConnected)
            //                {
            //                    System.Web.HttpContext.Current.Response.Write("<script>alert('服务器连接失败，请重试!');</script>");
            //                    HttpContext.Current.Response.Redirect(AppSetting.Root);

            //                }
            //            }
            //            catch(Exception) {
            //                System.Web.HttpContext.Current.Response.Write("<script>alert('服务器连接失败，请重试!');</script>");
            //                HttpContext.Current.Response.Redirect(AppSetting.Root);

            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    clientUser = new PSO.ClientUserinfo();
            //}
        }
    }
}