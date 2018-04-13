using Kingsun.IBS.BLL;
using Kingsun.IBS.IBLL;
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
    public class ApplyController : Controller
    {
        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
        private static IIBSData_ClassUserRelationBLL classBLL = new IBSData_ClassUserRelationBLL();

        //
        // GET: /Apply/

        public ActionResult Index()
        { 
            string UserID = "";
            string AppID = "";
            if (Request["UserID"] == null || Request["UserID"] == "")
            {
                Response.Write("<script>alert('参数错误！UserID不能为空！')</script>");
                return View();
            }
            else
            {
                UserID = Request["UserID"];
            }
            if (Request["AppID"] == null || Request["AppID"] == "")
            {
                Response.Write("<script>alert('参数错误！AppID不能为空！')</script>");
                return View();
            }
            else
            {
                AppID = Request["AppID"];
            }
            CheckApply(UserID,AppID);
            //VersionInfoToRedis(AppID);
            return View();
        }
        /// <summary>
        /// 版本信息存入Redis
        /// </summary>
        /// <param name="AppID"></param>
        public void VersionInfoToRedis(string AppID)
        {
            try
            {
                RedisHashHelper redis = new RedisHashHelper();
                VersionModel versionModel = redis.Get<VersionModel>("VersionModel", AppID);
                if (versionModel == null)
                {
                    //根据AppID获取版本信息
                    string sql = string.Format(" select *from dbo.TB_APPManagement where ID='{0}'", AppID);
                    DataSet set = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                    VersionModel model = JsonHelper.DataSetToIList<VersionModel>(set, 0)[0];
                    if (model != null)
                    {
                        bool flag = redis.Set<VersionModel>("VersionModel", AppID, model);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
        }
        static RedisHashHelper redis = new RedisHashHelper();
        /// <summary>
        /// 根据UserID判断该用户是否已报名
        /// </summary>
        /// <param name="UserID"></param>
        public void CheckApply(string UserID, string AppID)
        {
            try
            {
               
                TB_InterestDubbingGame_UserInfo userInfo = redis.Get<TB_InterestDubbingGame_UserInfo>("TB_InterestDubbingGame_UserInfo", UserID);
                if (userInfo != null)
                {
                    Response.Redirect("../Notification/Index?UserID=" + UserID + "&AppID=" + AppID);
                }
                else
                {
                    string sql = string.Format("select *from dbo.TB_InterestDubbingGame_UserInfo where UserID='{0}'", UserID);
                    DataSet set = SqlHelper.ExecuteDataset(SqlHelper.InterestDubbingGameConnectionStr, CommandType.Text, sql);
                    if (set != null && set.Tables[0].Rows.Count > 0)
                    {
                        Response.Redirect("../Notification/Index?UserID=" + UserID + "&AppID=" + AppID);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public JsonResult GetTB_UUMSUser(string UserID)
        {
            try
            {
                if (Request["UserID"] == null || Request["UserID"] == "")
                {
                    return Json(new { Success = false, Data = "", Msg = "参数错误！UserID不能为空！" });
                }
                var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(UserID));
                if (user != null) 
                {
                    return Json(new { Success = true, Data = user, Msg = "" });
                }
                return Json(new { Success = false, Data = "", Msg = "该用户不存在！" });
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                return Json(new { Success = false, Data = "", Msg = "操作异常！" });
            }
        }
        /// <summary>
        /// 判断是否已加入班级
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public JsonResult CheckClassInfoByStuID(string UserID)
        {
            try
            {
                if (string.IsNullOrEmpty(UserID))
                {
                    return Json(new { Success = false, Data = "", Msg = "参数错误！UserID不能为空！" });
                }
                var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(UserID));
                if (user != null)
                {
                    if (user.ClassSchList.Count > 0)
                    {
                        return Json(new { Success = true, Data = "", Msg = "" });
                    }
                    else 
                    {
                        return Json(new { Success = false, Data = "", Msg = "暂未加入班级！" });
                    }
                   
                }
                return Json(new { Success = false, Data = "", Msg = "找不到用户！" });
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                return Json(new { Success = false, Data = "", Msg = "操作异常！" });
            }
        }
    }
    public class VersionModel
    {
        public string ID { get; set; }
        public string VersionName { get; set; }
        public string VersionID { get; set; }
    }

}
