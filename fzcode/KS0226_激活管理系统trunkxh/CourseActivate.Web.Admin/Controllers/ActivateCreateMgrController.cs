using CourseActivate.Activate.BLL;
using CourseActivate.Activate.Constract.Model;
using CourseActivate.Core.Utility;
using CourseActivate.Web.Admin.Models;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace CourseActivate.Web.Admin.Controllers
{
    public class ActivateCreateMgrController : BaseController
    {
        //
        // GET: /ActivateManagement/
        ActivateManagementBLL manage = new ActivateManagementBLL();
        public ActionResult CreateActCode()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetBatchList(int pagesize, int pageindex, DateTime? startdate, DateTime? enddate, string batchcode = "", int bookid = -1, int publishid = -1, int activatetypeid = -1, int state = -1)
        {
            PageParameter<v_batchinfos> pageParameter = new PageParameter<v_batchinfos>();
            //  pageParameter.PageIndex = setpageindex(pageindex, pagesize);
            pageParameter.PageIndex = pageindex;
            pageParameter.PageSize = pagesize;

            //搜索条件:
            pageParameter.Where =
                o => (o.batchcode == batchcode || batchcode == "")
                && (o.bookid == bookid || bookid == -1)
                && (o.publishid == publishid || publishid == -1)
                && (o.activatetypeid == activatetypeid || activatetypeid == -1)
                && (o.status == state || state == -1)
            && (startdate == null || o.createtime >= startdate.Value)
            && (enddate == null || o.createtime <= enddate.Value.AddDays(1));

            pageParameter.OrderColumns = t1 => t1.createtime;
            pageParameter.IsOrderByASC = 0;
            int total;
            IList<v_batchinfos> batches = base.Manage.SelectPage<v_batchinfos>(pageParameter, out total);
            return Json(new { total = total, rows = batches });
        }
        [HttpPost]
        /// <summary>
        /// 删除批次
        /// </summary>
        /// <returns></returns>
        public JsonResult BatchDelete(int batchId)
        {
            if (new ActivateManagementBLL().CheckBatchIsInsertRedis(batchId))
            {
                return Json(new { Success = false, ErrorMsg = "当前批次激活码正在导入到redis!" });
            }
            using (TransactionScope scope = new TransactionScope())
            {
                var batch = base.Manage.Select<tb_batch>(batchId);
                if (batch != null && batch.status > 0)
                {
                    return Json(new { Success = false, ErrorMsg = "只有未启用过的激活码才能删除!" });
                }
                #region  删除redis操作
                List<tb_batchactivate> list = manage.SelectSearch<tb_batchactivate>(o => o.batchid == batchId);
                #endregion

                base.Manage.Delete<tb_batchbooks>(o => o.batchid == batchId);
                base.Manage.Delete<tb_batchactivate>(o => o.batchid == batchId);
                base.Manage.Delete<tb_batch>(o => o.batchid == batchId);
                scope.Complete();

                #region  删除redis操作
                var obj = from u in list select RedisConfiguration.ActivateKey + u.activatecode;
                Activate.BLL.ActivateCourseBLL bll = new ActivateCourseBLL();
                Task.Run(() => new ActivateCourseBLL().RemoveBatch(obj.ToList(), batchId));
                #endregion
                return Json(new { Success = true });
            }
        }

        public JsonResult UpdateState(int batchId, int status)
        {
            if (status == 0)//未启用
            {
                status = 1;//启用
            }
            else if (status == 1)
            {
                status = 2;//禁用
            }
            else if (status == 2)
            {
                status = 1;
            }
            bool result = base.Manage.CustomUpdateEntity<tb_batch>(o => o.batchid.ToString(), new tb_batch { batchid = batchId, status = status }, o => o.status.ToString());
            #region redis同步
            new ActivateCourseBLL().SetBatchRedis(batchId, status);
            #endregion

            return Json(new { Success = result });
        }

        public ActionResult Index()
        {
            return View("ActivateIndex");
        }

        public ActionResult CreateActivationCode(int actNum, int typeId, string remark, int bookId = 0)
        {
            var response = manage.InsertBatchActivateCode(actNum, typeId, bookId, remark);

            return RedirectToAction("Index");

        }

        public FileStreamResult ExportByBatch(string batch)
        {
            XSSFWorkbook workbook = new XSSFWorkbook();

            var actCodeList = SelectSearch<tb_batchactivate>(o => o.activatecode.StartsWith(batch));

            ISheet sheet = workbook.CreateSheet("激活码信息");
            var head = sheet.CreateRow(0).CreateCell(0);
            head.SetCellValue("激活码");
            sheet.SetColumnWidth(0, 20 * 256);

            for (int i = 0; i < actCodeList.Count; i++)
            {
                var cell = sheet.CreateRow(i + 1).CreateCell(0);
                cell.SetCellType(CellType.String);
                cell.SetCellValue(actCodeList[i].activatecode);
            }

            //  var buffer = workbook.GetBytes();

            using (NpoiMemoryStream ms = new NpoiMemoryStream())
            {
                workbook.Write(ms);//将激活码表格写入内存流               
                ms.Flush();
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/vnd.ms-excel", Server.UrlEncode(batch + ".xlsx"));
            }
        }

        public class NpoiMemoryStream : MemoryStream
        {
            public NpoiMemoryStream()
            {
                AllowClose = false;
            }

            public bool AllowClose { get; set; }

            public override void Close()
            {
                if (AllowClose)
                    base.Close();
            }
        }
    }
}