using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CourseActivate.Activate.Constract.Model;
using CourseActivate.Core.Utility;

namespace CourseActivate.Web.Admin.Controllers
{
    public class PublishController : BaseController
    {

        //
        // GET: /Publish/
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetData(int pagesize, int pageindex)
        {
            PageParameter<tb_publish> pageParameter = new PageParameter<tb_publish>();
            pageParameter.PageIndex = setpageindex(pageindex, pagesize);
            pageParameter.PageSize = pagesize;
            pageParameter.OrderColumns = t1 => t1.publishid;
            pageParameter.IsOrderByASC = 0;
            int total;
            IList<tb_publish> batches = base.Manage.SelectPage<tb_publish>(pageParameter, out total);
            return Json(new { total = total, rows = batches });
          //return Json(base.SelectAll<tb_publish>());
        }

        public JsonResult AddPublishInfo(tb_publish pl)
        {
            tb_publish publish = new tb_publish();
            publish.publishname = pl.publishname;
            publish.Remarks = pl.Remarks;
            publish.status = 0;
            publish.createTime = DateTime.Now;
            return Json(base.Add<tb_publish>(publish));
        }

        public JsonResult UpdateStatus(tb_publish pl)
        {
            tb_publish publish = new tb_publish();
            publish.publishid = pl.publishid;
            publish.status = pl.status;
            return Json(Update<tb_publish>(pl));
        }

        public JsonResult DeleteStatus(int id)
        {
            return Json(DeleteById<tb_publish>(id));
        }
    }
}