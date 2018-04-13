using CourseActivate.Activate.Constract.Model;
using CourseActivate.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace CourseActivate.Web.Admin.Controllers
{
    public class MessageMgrController : BaseController
    {
        //
        // GET: /MessageMgr/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /ActivateManagement/



        [HttpPost]
        public JsonResult GetPushSetList()
        {
            var list = Manage.SelectAll<tb_pushset>();
            PageParameter<tb_pushset> pageParameter = new PageParameter<tb_pushset>();
            pageParameter.PageIndex = 1;
            pageParameter.PageSize = int.MaxValue;
            //搜索条件:           

            pageParameter.OrderColumns = t1 => t1.num;
            pageParameter.IsOrderByASC = 0;
            int total;
            IList<tb_pushset> pushes = base.Manage.SelectPage<tb_pushset>(pageParameter, out total);
            return Json(new { total = total, rows = pushes });
        }
        [HttpPost]
        /// <summary>
        /// 删除批次
        /// </summary>
        /// <returns></returns>
        public JsonResult Delete(int pushid)
        {
            base.Manage.Delete<tb_pushset>(o => o.pushid == pushid);
            return Json(new { Success = true });
        }





        public ActionResult Create(int actNum)
        {
            var response = Add<tb_pushset>(new tb_pushset() { num = actNum, createtime = DateTime.Now });

            return Json(new { Success = true });

        }
    }
}