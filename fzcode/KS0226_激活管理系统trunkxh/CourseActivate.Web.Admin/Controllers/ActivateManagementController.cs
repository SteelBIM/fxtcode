//using CourseActivate.Activate.BLL;
//using CourseActivate.Activate.Constract.Model;
//using CourseActivate.Core.Utility;
//using CourseActivate.Web.Admin.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Transactions;
//using System.Web;
//using System.Web.Mvc;

//namespace CourseActivate.Web.Admin.Controllers
//{
//    public class ActivateManagementController : BaseController
//    {
//        //
//        // GET: /ActivateManagement/
//        ActivateManagementBLL manage = new ActivateManagementBLL();
//        public ActionResult CreateActCode()
//        {
//            return View();
//        }

//        [HttpPost]
//        public JsonResult GetBatchList(int pagesize, int pageindex)
//        {
//            PageParameter<batchinfos> pageParameter = new PageParameter<batchinfos>();
//            pageParameter.PageIndex = setpageindex(pageindex, pagesize);
//            pageParameter.PageSize = pagesize;
//            pageParameter.OrderColumns = t1 => t1.activatetypeid;
//            pageParameter.IsOrderByASC = 0;
//            int total;
//            IList<batchinfos> batches = base.Manage.SelectPage<batchinfos>(pageParameter, out total);
//            return Json(new { total = total, rows = batches });
//        }
//        [HttpPost]
//        /// <summary>
//        /// 删除批次
//        /// </summary>
//        /// <returns></returns>
//        public JsonResult BatchDelete(int batchId)
//        {
//            using (TransactionScope scope = new TransactionScope())
//            {
//                base.Manage.Delete<tb_batchactivate>(o => o.batchid == batchId);
//                base.Manage.Delete<tb_batch>(o => o.batchid == batchId);

//                scope.Complete();
//                return Json(new { Success = true });
//            }
//        }

//        public JsonResult UpdateState(int batchId, int status)
//        {
//            if (status == 0)//未启用
//            {
//                status = 1;//启用
//            }
//            else if (status == 1)
//            {
//                status = 2;//禁用
//            }
//            else if (status == 2)
//            {
//                status = 1;
//            }
//            bool result = base.Manage.CustomUpdate<tb_batch>(new tb_batch { batchid = batchId, status = status }, o => o.status.ToString());
//            return Json(new { Success = result });
//        }

//        public ActionResult Index()
//        {
//            return View();
//        }

//        public ActionResult CreateActivationCode(int actNum, int typeId, string remark, int bookId = 0)
//        {
//            var response = manage.InsertBatchActivateCode(actNum, typeId, bookId, remark);
//            return RedirectToAction("Index");
//        }

//    }
//}