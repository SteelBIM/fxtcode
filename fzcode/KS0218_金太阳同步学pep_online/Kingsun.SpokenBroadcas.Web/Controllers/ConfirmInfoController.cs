using Kingsun.SpokenBroadcas.BLL;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Kingsun.SpokenBroadcas.Web.Controllers
{
    public class ConfirmInfoController : Controller
    {
        static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        UserBLL userBll = new UserBLL();
        //
        // GET: /ConfirmInfo/
        static string UserID = "";
        public ActionResult Index()
        {
            if (Request["UserID"] != null && Request["UserID"] != "")
            {
                UserID = Request["UserID"];
            }
            else
            {
                Response.Write("<script>alert('参数错误！UserID不能为空！')</script>");
            }
            return View();
        }
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="TrueName"></param>
        /// <param name="TelePhone"></param>
        /// <returns></returns>
        public JsonResult UpdateUserInfo(string UserID, string TrueName, string TelePhone)
        {
            try
            {
                bool flag = userBll.UpdateUserInfo(UserID, TrueName, TelePhone);
                if (flag)
                {
                    return Json(new { Success = true, Data = "", Msg = "修改成功" });
                }
                else
                {
                    return Json(new { Success = false, Data = "", Msg = "修改失败" });
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                return Json(new { Success = false, Data = "", Msg = "操作异常" });
            }
        }
    }
}
