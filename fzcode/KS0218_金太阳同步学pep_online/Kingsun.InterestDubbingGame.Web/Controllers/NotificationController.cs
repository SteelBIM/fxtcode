using Kingsun.InterestDubbingGame.Model;
using Kingsun.SynchronousStudy.Common;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Kingsun.InterestDubbingGame.Web.Controllers
{
    public class NotificationController : Controller
    {
        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        //
        // GET: /Notification/

        public ActionResult Index()
        {
            string UserID = "";
            if (Request["UserID"] == null || Request["UserID"] == "")
            {
                Response.Write("<script>alert('参数错误！UserID不能为空！')</script>");
                return View();
            }
            else
            {
                UserID = Request["UserID"];     
            }
            TB_InterestDubbingGame_UserInfo userInfo = GetUserInfo(UserID);
            ViewData["userInfo"] = userInfo;
            return View();
        }
       static RedisHashHelper redis = new RedisHashHelper();
        /// <summary>
        /// 获取用户报名信息
        /// </summary>
        /// <returns></returns>
        public TB_InterestDubbingGame_UserInfo GetUserInfo(string UserID)
        {
            try
            { 
                TB_InterestDubbingGame_UserInfo userInfo = redis.Get<TB_InterestDubbingGame_UserInfo>("TB_InterestDubbingGame_UserInfo", UserID);
                if (userInfo != null)
                {
                    return userInfo;
                }
                else
                {
                    string sql = string.Format("select *from dbo.TB_InterestDubbingGame_UserInfo where UserID='{0}'", UserID);
                    DataSet set = SqlHelper.ExecuteDataset(SqlHelper.InterestDubbingGameConnectionStr, CommandType.Text, sql);
                    if (set != null && set.Tables[0].Rows.Count > 0)
                    {
                        List<TB_InterestDubbingGame_UserInfo> list = JsonHelper.DataSetToIList<TB_InterestDubbingGame_UserInfo>(set, 0);
                        return list[0];
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                return null;
            }
        }
    }
}
