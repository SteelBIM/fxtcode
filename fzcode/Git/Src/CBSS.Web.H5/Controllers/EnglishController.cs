using CBSS.Core.Utility;
using CBSS.Framework.Contract.API;
using CBSS.Framework.Contract.Enums;
using CBSS.IBS.BLL;
using CBSS.IBS.Contract;
using CBSS.IBS.Contract.IBSResource;
using CBSS.IBS.IBLL;
using CBSS.Tbx.BLL;
using CBSS.Tbx.Contract.DataModel;
using CBSS.Tbx.Contract.ViewModel;
using CBSS.Tbx.IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CBSS.Web.H5.Controllers
{
    public class EnglishController : Controller
    {
        //
        // GET: /English/
        static ITbxService tbxService = new TbxService();
        static IIBSService ibsService = new IBSService(); 
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult HelpCenter()
        {
            return View();
        }

        public ActionResult DictationReport(string id)
        {
            Rds_UserWordDictationShare response = tbxService.GetShareReport<Rds_UserWordDictationShare>(id);
            //   var model=re
            return View(response);
        }

        public ActionResult DoGet()
        {
            return View();
        }

        public JsonResult DoGetJson(string mod)
        {
            try
            {
                var json = HttpHelper.ModHttpGet(mod);
                if (string.IsNullOrEmpty(json))
                {
                    return Json(new { data="无数据"},JsonRequestBehavior.AllowGet);
                }
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message+ex.StackTrace, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult HearResource(string UserID, int BookID, int FirstTitleID, int SecondTitleID, int FirstModularID, int SecondModularID)
        {
            return View();
        }


        public ActionResult InterestDubbing(string UserID, string VideoFileID, int IsEnableOss, string AppID)
        {
            return View();
        }

        public JsonResult GetUser()
        {
            string userID = Request.Form["UserID"];
            var user = ibsService.Search(a => a.UserID == Convert.ToInt32(userID));
            if (user != null)
            {
                return Json(user.ToJson());
            }
            else
            {
                return Json("");
#pragma warning disable CS0162 // 检测到无法访问的代码
                Response.End();
#pragma warning restore CS0162 // 检测到无法访问的代码
            }
        }
        

        public ActionResult HearResourceShare()
        {
            return View();
        }
    }
}
