using CBSS.Cfgmanager.BLL;
using CBSS.Cfgmanager.Contract.DataModel;
using CBSS.Framework.Contract;
using CBSS.Framework.Contract.Enums;
using CBSS.Web.Admin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBSS.Web.Admin.Areas.Cfgmanager.Controllers
{
    public class Log4netController : ControllerBase
    {
        //
        // GET: /Cfgmanager/Log4net/

        public ActionResult Index()
        { 
            ViewBag.Edit = action.Edit;
            ViewBag.Del = action.Del;
            var Logger = new SelectList(WebControl.GetSelectList(typeof(Core.Log.LoggerType)), "Value", "Text");
            ViewData["Logger"] = Logger;
            return View();
        }
        public JsonResult GetLog4netPage(int pagesize, int pageindex, string Level, string Logger)
        {
            Log4netRequest request = new Log4netRequest();
            request.PageIndex = setpageindex(pageindex, pagesize);
            request.PageSize = pagesize;
            request.Level = Level;
            request.Logger = Logger;
            int total = 0;
            var list = CfgmanagerService.GetLog4net(out total, request);
            return Json(new { total = total, rows = list });
        }
        public ActionResult Edit(int id)
        {
            var Log4netModel = CfgmanagerService.GetLog4net(id);
            return View("Edit", Log4netModel);
        }
        [HttpPost]
        public ActionResult Delete(string ids)
        {
            List<int> ListIds = new List<int>();
            if (string.IsNullOrEmpty(ids))
            {
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in ids.Split(','))
                {
                    ListIds.Add(Convert.ToInt32(item));
                }
            }
            bool flag = CfgmanagerService.DeleteLog4net(ListIds);
            return Json(flag);
        }
    }
}
