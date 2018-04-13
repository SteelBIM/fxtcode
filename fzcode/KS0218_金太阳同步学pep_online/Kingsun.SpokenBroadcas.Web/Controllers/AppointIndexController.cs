using Kingsun.SpokenBroadcas.BLL;
using Kingsun.SpokenBroadcas.Common;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;


namespace Kingsun.SpokenBroadcas.Web.Controllers
{
    public class AppointIndexController : Controller
    {
        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        //
        // GET: /Course/

        public ActionResult Index()
        {
            return View();
        }
        public JsonResult AddCollectUserInfo(string UserName, string TelePhone)
        {
            try
            {
                string sql = string.Format("insert into Tb_UserInfoTemp(UserName,TelePhone) values('{0}','{1}')", UserName, TelePhone);
                int flagCount = SqlHelper.ExecuteNonQuery(AppSetting.SpokenConnectionString, CommandType.Text, sql);
                if (flagCount > 0)
                {
                    return Json(new { Success = true, Data = "", Msg = "提交成功！" });
                }
                else
                {
                    return Json(new { Success = false, Data = "", Msg = "提交失败！" });
                } 
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                return Json(new { Success = false, Data = "", Msg = "提交失败！" });
            }

        }
    }
}
