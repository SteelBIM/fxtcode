using CourseActivate.Activate.Constract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseActivate.Web.Admin.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            int currentYear = DateTime.Now.Year;
            int currentMonth = DateTime.Now.Month;
            List<tb_activatemonthrecord> list = SelectSearch<tb_activatemonthrecord>(i => i.year == currentYear);
            ViewBag.YearCount = list.Sum(i => i.num).GetValueOrDefault();
            tb_activatemonthrecord amr = list.Where(i => i.month == currentMonth).FirstOrDefault();
            if (amr != null)
            {
                ViewBag.CurrentMonthCount = amr.num ?? 0;
            }
            else
            {
                ViewBag.CurrentMonthCount = 0;
            }
            amr = list.Where(i => i.month == (currentMonth - 1)).FirstOrDefault();
            if (amr != null)
            {
                ViewBag.PrevMonthCount = amr.num ?? 0;
            }
            else
            {
                ViewBag.PrevMonthCount = 0;
            }
            return View();
        }

        public JsonResult GetNoticeMessage()
        {
            List<tb_pushmessage> list = SelectSearch<tb_pushmessage>(i => (i.towho == masterinfo.masterid || i.towho == 0) && i.hasread == 0);
            return Json(list);
        }

        public string ReadMessage(int id)
        {
            bool b = Update<tb_pushmessage>(new { hasread = 1, readdate = DateTime.Now, readmaster = masterinfo.mastername }, i => i.pushid == id);
            return b.ToString();
        }
    }
}
