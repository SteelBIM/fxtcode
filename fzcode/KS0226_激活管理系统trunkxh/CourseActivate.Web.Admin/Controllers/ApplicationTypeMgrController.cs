using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CourseActivate.Activate.Constract.Model;
using CourseActivate.Core.Utility;

namespace CourseActivate.Web.Admin.Controllers
{
    public class ApplicationTypeMgrController : BaseController
    {
        //
        // GET: /ApplicationTypeMgr/
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ApplicationTypeMgr_View(int pagesize, int pageindex)
        {
            PageParameter<tb_devicetype> pageParameter = new PageParameter<tb_devicetype>();
            pageParameter.PageIndex = setpageindex(pageindex, pagesize);
            pageParameter.PageSize = pagesize;
            pageParameter.OrderColumns = t1 => t1.devicetypeid;
            pageParameter.IsOrderByASC = 0;
            int total;
            IList<tb_devicetype> batches = Manage.SelectPage<tb_devicetype>(pageParameter, out total);
            return Json(new { total = total, rows = batches });
        }
    }
}